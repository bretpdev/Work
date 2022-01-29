using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using System.IO;
using Uheaa.Common;
using Efs = Uheaa.Common.DataAccess.EnterpriseFileSystem;



namespace LITSTATFED
{
    public class AddLitigationStatusFed : FedBatchScript
    {
        private const string Eoj_Total = "Total Borrowers";
        private const string Eoj_Success = "Borrowers Successfully Processed";
        private const string Eoj_Error = "Borrowers with errors.";
        private static readonly string[] EojFields = { Eoj_Total, Eoj_Success, Eoj_Error };
        private const string FileToProcessPattern = "UNWO15.NWO15R2*";

        public AddLitigationStatusFed(ReflectionInterface ri)
            : base(ri, "LITSTATFED", "ERR_BU01", "EOJ_BU35", EojFields)
        { }

        public override void Main()
        {
            string[] filesToProcess = Directory.GetFiles(Efs.FtpFolder, FileToProcessPattern);

            if (filesToProcess.Length > 1)
                NotifyAndEnd(string.Format("Found multiple {0} files at {1}.  Please remove any duplicates before re-running this script, which will now close.", FileToProcessPattern, Efs.FtpFolder));
            else if (filesToProcess.Length == 0)
                NotifyAndEnd(string.Format("Couldn't find any files like {0} in {1}.  Script will now close.", FileToProcessPattern, Efs.FtpFolder));

            Func<string, string> getSSN = (account) =>
            {
                try
                {
                    var demog = GetDemographicsFromTx1j(account);
                    return demog.Ssn;
                }
                catch (DemographicException)
                {
                    return null;
                }
            };

            string file = filesToProcess.Single();
            foreach (FileBorrower b in FileBorrower.ParseBorrowers(file, ",", getSSN).Skip(Recovery.RecoveryValue.ToIntNullable() ?? 0))
            {
                Recovery.RecoveryValue = Eoj[Eoj_Total].ToString();
                if (ProcessBorrower(b))
                    Eoj[Eoj_Success]++;
                else
                    Eoj[Eoj_Error]++;
                Eoj[Eoj_Total]++;
            }
            File.Delete(file);
            Recovery.RecoveryValue = null;
            Eoj.Publish();
            Err.Publish();
            NotifyAndEnd("Processing Complete");
        }

        private bool ProcessBorrower(FileBorrower b)
        {
            //add ARC and set recipient id for each sequence
            foreach (int sequence in b.LoanSequences)
                if (!Atd22ByLoan(b.SSN, "CSTAY", "", "000502", new List<int>(new int[] { sequence }), "LITSTATFED", false))
                {
                    AddErrorRecord("Error adding CSTAY ARC: " + RI.Message, b.AccountNumber, sequence);
                    return false;
                }

            bool success = true;
            foreach (int sequence in b.LoanSequences)
            {
                success = success && UpdateTd1a(b.SSN, sequence);
                success = success && CheckTs26(b.AccountNumber, b.SSN, sequence);
            }
            return success;
        }

        //access TD1A through TX6X and update it
        private bool UpdateTd1a(string ssn, int loanSequence)
        {
            //access queue task in TX6X
            FastPath("TX3Z/ITX6XDZ;01;" + ssn + loanSequence.ToString("0000") + ";CSTAY");
            PutText(21, 18, "01", ReflectionInterface.Key.Enter);

            //update TD1A
            PutText(7, 16, DateTime.Today.ToString("MMddyy")); //set today's date
            decimal principal = decimal.Parse(GetText(17, 25, 10));
            decimal interest = decimal.Parse(GetText(17, 43, 10));
            decimal total = Math.Round(principal + interest);
            PutText(7, 43, total.ToString(), ReflectionInterface.Key.Enter); //combine principal + interest
            PutText(6, 21, "V", ReflectionInterface.Key.Enter); //change R to V

            return true;
        }

        //verify status on TS26
        private bool CheckTs26(string accountNumber, string ssn, int loanSequence)
        {
            FastPath("TX3Z/ITS26" + ssn);

            //select loan from selection screen
            while (RI.ScreenCode == "TSX28" && RI.Message != "90007")  //no more pages
            {
                for (int row = 8; row < 22; row++)
                {
                    string selection = GetText(row, 2, 2);
                    if (string.IsNullOrEmpty(selection))
                    {
                        Hit(ReflectionInterface.Key.F8); //next page
                        break;
                    }

                    //select the loan if it is found
                    if (int.Parse(GetText(row, 14, 4)) == loanSequence)
                    {
                        PutText(21, 12, selection);
                        Hit(ReflectionInterface.Key.Enter);
                        break;
                    }
                }
            }
            //check the status if the detail screen is displayed
            if (RI.ScreenCode == "TSX29")
            {
                if (!CheckForText(3, 10, "LITIGATION"))//add an error record if status is not "LITIGATION"
                {
                    AddErrorRecord("Upon completion loan status = " + GetText(3, 10, 20), accountNumber, loanSequence);
                    return false;
                }
            }
            //add an error record if the loan is not found
            else
            {
                AddErrorRecord("Failed to verify Litigation status", accountNumber, loanSequence);
                return false;
            }
            return true;
        }

        private void AddErrorRecord(string message, string accountNumber, int loanSequence)
        {
            Err.AddRecord(message, new { Account = accountNumber, LoanSequence = loanSequence });
        }
    }
}
