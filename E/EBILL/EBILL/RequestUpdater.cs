using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;
using Efs = Uheaa.Common.DataAccess.EnterpriseFileSystem;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace EBILL
{
    public class RequestUpdater
    {
        private const string EbillFilePattern = "Bill_Change_UT*";

        private ReflectionInterface RI { get; set; }
        private readonly DataAccess DA;

        public RequestUpdater()
        {
            DA = new DataAccess();
        }

        private bool CreateSession()
        {
            bool hasLoggedIn = false;
            for (int trys = 0; trys < 5; trys++)
            {
                BatchProcessingHelper login = BatchProcessingHelper.GetNextAvailableId("", "BatchUheaa");
                RI = new ReflectionInterface();
                hasLoggedIn = RI.Login(login.UserName, login.Password);
                if (hasLoggedIn)
                    break;
            }

            if (!hasLoggedIn)
                ProcessLogger.AddNotification(Program.LogData.ProcessLogId, "Unable to create a session and Login.  Please review and restart the application.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            return hasLoggedIn;
        }

        public int Run()
        {
            if (!CreateSession())
                return 1;

            //Get a list of all E-Bill request files.
            List<string> foundFiles = Directory.GetFiles(Efs.FtpFolder, EbillFilePattern).ToList();

            //Load All files
            foreach (string ebillFile in foundFiles)
            {
                if (new FileInfo(ebillFile).Length > 0)
                {
                    //Read the whole file into a collection of records.
                    List<RequestRecord> records = RequestRecord.FromFile(ebillFile);
                    DA.SaveRecords(records);
                }
                Repeater.TryRepeatedly(() => File.Delete(ebillFile));
            }

            Process();
            RI.CloseSession();
            return 0;
        }

        private void AddActivityComment(string accountNumber, string billingPreference, IEnumerable<int> loanSequences)
        {
            string arc = "";
            string comment = "";
            string loanSequenceString = string.Join(", ", loanSequences.Select(p => p.ToString()).ToArray());
            switch (billingPreference)
            {
                case "M":
                    arc = "EBIL1";
                    comment = "Per bwrs request updated email address.";
                    break;
                case "P":
                    arc = "EBIL2";
                    comment = string.Format("Update E-bill to paper bill for loan seqs {0} per bwrs request.", loanSequenceString);
                    break;
                case "E":
                    arc = "EBIL3";
                    comment = string.Format("Update bill to E-bill for loan seqs {0} per bwrs request. Verified/updated email.", loanSequenceString);
                    break;
            }
            ArcAddResults result = null;
            if (billingPreference == "M")
                result = AddTypeM(accountNumber, arc, comment, result);
            else
                result = AddArcByLoan(accountNumber, loanSequences, arc, comment, result);
            if (!result.ArcAdded)
                ProcessLogger.AddNotification(Program.LogData.ProcessLogId, string.Format("Unable to add ARC:{0} for Borrower:{1} Loans:{2} Comment:{3}", arc, accountNumber, billingPreference == "M" ? "ALL" : loanSequenceString, comment), NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }

        private static ArcAddResults AddArcByLoan(string accountNumber, IEnumerable<int> loanSequences, string arc, string comment, ArcAddResults result)
        {
            ArcData arcToAdd = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = accountNumber,
                Arc = arc,
                Comment = comment,
                ScriptId = Program.ScriptId,
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                LoanSequences = loanSequences.ToList(),
                RecipientId = ""
            };

            result = arcToAdd.AddArc();
            return result;
        }

        private static ArcAddResults AddTypeM(string accountNumber, string arc, string comment, ArcAddResults result)
        {
            ArcData arcToAdd = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = accountNumber,
                Arc = arc,
                Comment = comment,
                ScriptId = Program.ScriptId,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
            };

            result = arcToAdd.AddArc();
            return result;
        }

        private void Process()
        {
            List<RequestRecord> records = DA.GetAllUnprocessedRecords();

            foreach (RequestRecord record in records)
            {
                if (record.BillingPreference != "M")
                    if (UpdateBillingPreference(record.SSN, record.LoanSequence, record.BillingPreference))
                        DA.MarkUpdateSucceeded(record.EbillId);
                    else
                        DA.MarkError(record.EbillId);
                if (UpdateEmailAddress(record.SSN, record.Email))
                    DA.MarkUpdateSucceeded(record.EbillId);
                else
                    DA.MarkError(record.EbillId);
            }

            //Add a borrower/billing preference-level activity comment noting
            //which loans had their billing preference successfully updated.
            foreach (string ssn in records.Select(p => p.SSN).Distinct().OrderBy(p => p))
            {
                string accountNumber = RI.GetDemographicsFromTx1j(ssn).AccountNumber.Replace(" ", "");
                //Skip this account number if we're recovering from the ARC phase and we're already past this account.
                foreach (string billingPreference in records.Where(p => p.SSN == ssn).Select(p => p.BillingPreference).Distinct().OrderBy(p => p))
                {
                    List<int> successfulLoanSequences = DA.GetSucessfulLoanSequences(ssn, billingPreference);
                    AddActivityComment(accountNumber, billingPreference, successfulLoanSequences);
                    DA.MarkArcAdded(ssn, billingPreference);
                }
            }
        }

        private bool UpdateBillingPreference(string ssn, string loanSequence, string billingPreference)
        {
            RI.FastPath("TX3Z/CTS7C" + ssn);
            if (RI.ScreenCode == "T1X01")
            {
                ProcessLogger.AddNotification(Program.LogData.ProcessLogId, string.Format("Unable to find LoanSeq:{0} for borrower:{1} on TS7C", loanSequence, ssn), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            TS7CSelectionScreen(loanSequence);
            if(!UpdateTS7C(billingPreference))
            {
                ProcessLogger.AddNotification(Program.LogData.ProcessLogId, string.Format("Unable to find LoanSeq:{0} for borrower:{1} on TS7C", loanSequence, ssn), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }

            if (RI.CheckForText(23, 2, "01005 RECORD SUCCESSFULLY CHANGED", "01003 NO FIELDS UPDATED - NO RECORD CHANGES PROCESSED"))
                return true;
            else
            {
                AddErrorArc(ssn, loanSequence, billingPreference);
                return false;
            }
        }

        private bool UpdateTS7C(string billingPreference)
        {
            if (!RI.CheckForText(1, 72, "TSX7D")) 
                return false;

            if (RI.CheckForText(14, 48, "_"))
                RI.PutText(14, 48, "N");  //EXT TRM DEBT IND
            if (RI.CheckForText(18, 19, "__"))
            {
                string numberOfGraceMonths = (RI.CheckForText(6, 38, "STFFRD", "UNSTFD") ? "6" : RI.CheckForText(6, 38, "TILP") ? "02" : "0");
                RI.PutText(18, 19, numberOfGraceMonths);
            }
            RI.PutText(19, 42, (billingPreference == "E" ? "Y" : "N")); //E-BILL IND
            RI.Hit(Key.Enter);

            return true;
        }

        private void TS7CSelectionScreen(string loanSequence)
        {
            if (RI.CheckForText(1, 72, "TSX3S"))
            {
                for (int row = 7; RI.MessageCode != "90007"; row++)
                {
                    if (RI.CheckForText(row, 3, "  "))
                    {
                        row = 6;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }
                    if (RI.CheckForText(row, 20, loanSequence.PadLeft(4,'0')))
                    {
                        RI.PutText(22, 19, RI.GetText(row, 3, 2), Key.Enter);
                        break;
                    }
                }
            }
        }

        private void AddErrorArc(string ssn, string loanSequence, string billingPreference)
        {
            string arc = (billingPreference == "E" ? "EBIL4" : "EBIL5");
            string comment = string.Format("Unable to update billing preference for loan seq {0}", loanSequence);
            ArcAddResults result = null;
            ArcData arcToAdd = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = ssn,
                Arc = arc,
                Comment = comment,
                ScriptId = Program.ScriptId,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
            };

            result = arcToAdd.AddArc();
            if (!result.ArcAdded)
                ProcessLogger.AddNotification(Program.LogData.ProcessLogId, string.Format("Unable to add ARC:{0} for Borrower:{1} Loans:{2} Comment:{3}", arc, ssn, "ALL", comment), NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }

        private bool UpdateEmailAddress(string ssn, string email)
        {
            RI.FastPath("TX3Z/CTX1JB;" + ssn);
            if (RI.ScreenCode == "TXX1K")
            {
                ProcessLogger.AddNotification(Program.LogData.ProcessLogId, string.Format("Borrower:{0} not found on System, email:{1}", ssn, email), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            RI.Hit(Key.F2);
            RI.Hit(Key.F10);
            string existingEmail = (RI.GetText(14, 10, 60) + RI.GetText(15, 10, 60) + RI.GetText(16, 10, 60) + RI.GetText(17, 10, 60) + RI.GetText(18, 10, 14)).Trim('_');
            if (email != existingEmail)
            {
                RI.PutText(9, 20, "55"); //ADDR SOURCE CODE
                RI.PutText(11, 17, DateTime.Now.ToString("MMddyy")); //ADDR LAST VER
                RI.PutText(12, 14, "Y"); //ADDR VALID
                RI.PutText(14, 10, "", true);
                RI.PutText(15, 10, "", true);
                RI.PutText(16, 10, "", true);
                RI.PutText(17, 10, "", true);
                RI.PutText(18, 10, "", true);
                RI.PutText(14, 10, email);
                RI.Hit(Key.Enter);
            }

            return true;
        }
    }
}