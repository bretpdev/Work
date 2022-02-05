using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using System.Windows.Forms;

namespace IPACEMLLET
{
    public class IpacEmailLetters : FedBatchScript
    {
        private const string EojProcessed = "Total number of records processed";
        private const string EojNoCor = "Account reviewed no correspondence needed";
        private const string EojTotal = "Total in the file";
        private const string EojPrintErrors = "There was an error printing the letters";
        private static readonly string[] EOJ_FIELDS = { EojTotal, EojProcessed, EojNoCor, EojPrintErrors };

        private InvalidAddressFile Invalid { get; set; }
        private ValidAddressFile Valid { get; set; }

        private string CsvPath
        {
            get
            {
                return Path.Combine(EnterpriseFileSystem.TempFolder, "IPAC Payments.csv");
            }
        }

        public IpacEmailLetters(ReflectionInterface ri)
            : base(ri, "IPACEMLLET", "ERR_BU24", "EOJ_BU24", EOJ_FIELDS)
        {
            Invalid = new InvalidAddressFile(RI, !Recovery.RecoveryValue.IsNullOrEmpty());
            Valid = new ValidAddressFile(RI, !Recovery.RecoveryValue.IsNullOrEmpty());
        }

        public override void Main()
        {
            List<BorrowerData> data = GetBorrowerData();

            foreach (BorrowerData bor in data)
            {
                //We only want to process borrower with open loans.
                //This file is sent to us by another servicer so they may have paid off.
                if (DetermineOpenLoan(bor))
                    CheckValidAddress(bor);
            }

            SendLetterAndAddComments(data);

            DeleteFile(CsvPath);

            if (Err.HasErrors)
                ProcessingComplete("Processing Complete but there were errors in processing");
            else
                ProcessingComplete();

        }

        /// <summary>
        /// Loads the file from T:\IPAC Payments.csv
        /// </summary>
        /// <returns>List of BorrowerData</returns>
        private List<BorrowerData> GetBorrowerData()
        {
            FileCheck();
            List<BorrowerData> bor = new List<BorrowerData>();
            using (StreamReader sr = new StreamReader(CsvPath))
            {
                string header = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    List<string> line = new List<string>();
                    line = sr.ReadLine().SplitAndRemoveQuotes(",");

                    //If the first position is empty, it's a blank line. Skip it.
                    if (!string.IsNullOrEmpty(line.FirstOrDefault()))
                    {
                        BorrowerData data = new BorrowerData();
                        data.BorrowerSsn = line[0].Replace("-", "");
                        data.BorrowerSsn = data.BorrowerSsn.PadLeft(9, '0');

                        if (!string.IsNullOrEmpty(line[2]))
                            data.EffectiveDate = DateTime.Parse(line[2]).Date;
                        if (!bor.Any(p => p.BorrowerSsn == data.BorrowerSsn))
                        {
                            bor.Add(data);
                            if (Recovery.RecoveryValue.IsNullOrEmpty())
                                Eoj[EojTotal].Increment();
                        }
                    }
                }
            }
            Recovery.RecoveryValue = "EOJ DONE";
            return bor;
        }

        /// <summary>
        /// Checks the conditions for the file.
        /// </summary>
        private void FileCheck()
        {
            if (!File.Exists(CsvPath))
            {
                MessageBox.Show("There is not a file available to process.", "File does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Invalid.Delete();
                Valid.Delete();
                EndDllScript();
            }
            else if (new FileInfo(CsvPath).Length == 0)
            {
                MessageBox.Show("The file is empty", "Empty File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Invalid.Delete();
                Valid.Delete();
                EndDllScript();
            }
            else if (Directory.GetFiles(EnterpriseFileSystem.TempFolder, "*.csv").Where(p => p.ToString().Contains("IPAC Payments")).Count() > 1)
            {
                MessageBox.Show("There are multiple files to process. Please remove all invalid files", "Multiple files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Invalid.Delete();
                Valid.Delete();
                EndDllScript();
            }
        }

        /// <summary>
        /// Checks ITS26 for open loans and compares the date of the 0921A transaction
        /// </summary>
        /// <param name="bor">BorrowerData object</param>
        /// <returns>True if there are open loans within the specified dates</returns>
        private bool DetermineOpenLoan(BorrowerData bor)
        {
            bool openLoan = false;

            FastPath("TX3ZITS26" + bor.BorrowerSsn);

            //Check if on loan screen or selection screen
            if (CheckForText(1, 72, "TSX29"))
                openLoan = TargetScreenTSX29(bor);
            else if (CheckForText(1, 72, "TSX28"))
            {
                double balance = GetBalance();

                //Check if the date on the loan is less than or equal to the date in the file
                if (balance > 0 & DateValid(bor.EffectiveDate))
                    openLoan = true;
                else
                    openLoan = false;
            }

            if (!openLoan)
                Eoj[EojNoCor].Increment();

            return openLoan;
        }

        /// <summary>
        /// Gets the balance for the current borrower.
        /// </summary>
        /// <returns>Total balance of the borrowers loans.</returns>
        private double GetBalance()
        {
            bool credit = false;
            double balance = 0;
            for (int row = 8; !CheckForText(23, 2, "90007"); row++)
            {
                if (row > 20)
                {
                    row = 7;
                    Hit(ReflectionInterface.Key.F8);
                    continue;
                }
                string text = GetText(row, 60, 8);
                credit = CheckForText(row, 69, "CR");
                if (text != "" && !credit)
                    balance += double.Parse(text);
            }

            return balance;
        }

        /// <summary>
        /// Checks the target screen TSX29 to see if the borrower has open loans
        /// </summary>
        /// <param name="bor">Current Borrower</param>
        /// <returns>True if the borrower has an open loan, False if they do not.</returns>
        private bool TargetScreenTSX29(BorrowerData bor)
        {
            bool openLoan = false;
            double amount = double.TryParse(GetText(10, 16, 10), out amount) ? double.Parse(GetText(10, 16, 10)) : 0;
            if (CheckForText(11, 22, "CR"))
                return false;
            if (amount > 0)
            {
                if (DateValid(bor.EffectiveDate))
                    openLoan = true;
                else
                    openLoan = false;
            }
            return openLoan;
        }

        /// <summary>
        /// Checks to date on the 0921A transaction
        /// </summary>
        /// <param name="borDate">BorrowerData object</param>
        /// <returns>True if the date is less than or equal</returns>
        private bool DateValid(DateTime borDate)
        {
            bool valid = false;
            if (RI.ScreenCode == "TSX28")
                PutText(21, 12, "01", ReflectionInterface.Key.Enter);

            if (RI.ScreenCode == "TSX29")
            {
                Hit(ReflectionInterface.Key.F6);
                Hit(ReflectionInterface.Key.Enter);
            }

            DateTime effDate = new DateTime();
            Coordinate coord = new Coordinate();
            while (RI.MessageCode != "90007")
            {
                coord = FindText("0291A");
                if (coord == null)
                    Hit(ReflectionInterface.Key.F8);
                else
                    break;
            }

            if (coord != null)
                effDate = DateTime.Parse(GetText(coord.Row, 12, 8)).Date;

            if (effDate <= borDate)
                valid = true;

            return valid;
        }


        /// <summary>
        /// Checks TX1J to see if there is a valid address and sets the BorrowerData object with data from the sessions
        /// </summary>
        /// <param name="bor">BorrowerData object</param>
        private BorrowerData CheckValidAddress(BorrowerData bor)
        {
            FastPath("TX3ZITX1J;" + bor.BorrowerSsn);
            string file = string.Empty;
            if (CheckForText(11, 55, "Y"))
            {
                bor.HasValidAddress = true;
                Valid.Add(bor);
            }
            else
            {
                bor.HasValidAddress = false;
                Invalid.Add(bor);
                Eoj[EojNoCor].Increment();
            }

            return bor;
        }

        /// <summary>
        /// Uses EcorrBorrowerCostCenterPrinting to send a letter to everyone in the data file
        /// </summary>
        private void SendLetterAndAddComments(List<BorrowerData> data)
        {
            string letterId = "LPSNLFP";
            try
            {
                string stateCode = "State";
                string accountNumberField = "AccountNumber";
                EcorrProcessing.EcorrCostCenterPrinting(letterId, Valid.FilePathAndName, UserId, stateCode, ScriptId, accountNumberField,
                    DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, ProcessLogData.ProcessLogId);

                //We only want to generate the ecorr documents because they do not have a valid address.
                EcorrProcessing.EcorrCostCenterPrinting(letterId, Invalid.FilePathAndName, UserId, stateCode, ScriptId, accountNumberField,
                    DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, ProcessLogData.ProcessLogId, string.Empty, true);

                string letterComment = "Letter sent to borrower instructing payments will no longer be forwarded from previous servicer, send payments to CornerStone";
                string invalidAddressComment = "Acct reviewed, but invalid addresses, no correspondence sent to borrower";
                
                //Leave a comment for everyone who received a letter.
                foreach (string acctNumber in Valid.GetAccountNumbersFromFile(Valid.FilePathAndName))
                {
                    Atd22ByBalance(acctNumber, "NOFWD", letterComment, "", ScriptId, false);
                    Eoj[EojProcessed].Increment();
                }

                foreach (string acctNumber in Invalid.GetAccountNumbersFromFile(Invalid.FilePathAndName))
                {
                    Atd22ByBalance(acctNumber, "NOFWD", invalidAddressComment, "", ScriptId, false);
                    Eoj[EojProcessed].Increment();
                }

                //Delete the data files
                DeleteFile(Valid.FilePathAndName);
                DeleteFile(Invalid.FilePathAndName);

            }
            catch (Exception ex)
            {
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, "There was a problem printing letter id: " + letterId,
                    NotificationType.ErrorReport, NotificationSeverityType.Critical, ProcessLogData.ExecutingAssembly, ex);

                Eoj.Counts[EojPrintErrors].Increment();
                Err.AddRecord(EojPrintErrors, new object() { });
            }
        }

        /// <summary>
        /// Deletes a given file.  Will log a notification if the file was not deleted in 5 minutes.
        /// </summary>
        /// <param name="file">File to delete</param>
        private void DeleteFile(string file)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            while (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    if (s.ElapsedMilliseconds / 60000.00 > 5.00)
                    {
                        string message = string.Format("{0}  has been trying to delete {1} for over 5 minutes.  The script will now end, please ensure this file is removed", ScriptId, file);
                        ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ProcessLogData.ExecutingAssembly, ex);
                        Err.AddRecord(message + "ProcessLogId: " + ProcessLogData.ProcessLogId.ToString(), new object { });
                        return;
                    }
                    continue;
                }
            }
            s.Stop();
        }
    }//class
}//namespace
