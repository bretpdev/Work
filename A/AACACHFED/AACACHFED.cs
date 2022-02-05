using System;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;

namespace AACACHFED
{
    public class AACACHFED : FedBatchScript
    {
        private const string SCRIPT_ID = "AACACHFED";
        private const string SAS_FILE_NAME = "CornerStone AAC ACH";
        private const string EojTotalRecords = "Total number of borrowers in the SAS file";
        private const string EojTotalComplete = "Total number of borrowers successfully processed";
        private const string EojTotalError = "Total number of borrowers with errors";
        private static readonly string[] EojFields = { EojTotalRecords, EojTotalComplete, EojTotalError };
        private bool _hasErrorReport = false;
        private List<int> LoanSeqAchAddedTo { get; set; }

        public AACACHFED(ReflectionInterface ri)
            : base(ri, "AACACHFED","ERR_BU01","EOJ_BU01", EojFields)
        {
            LoanSeqAchAddedTo = new List<int>();
        }

        public override void Main()
        {
            StartupMessage("This script loads direct debit records to the system for recently loaded federal accounts. Click OK to continue, or Cancel to quit.");

            //prompt the user to select the file to process
            string sasFile = PromptForFile();

            //process the file
            ProcessFile(sasFile);

            //delete the file and end the script
            File.Delete(sasFile);

            if (_hasErrorReport)
                ProcessingComplete("Script Completed with errors please see error report.");
            else
                ProcessingComplete();
        }//Main

        //prompt the user to select the file to be processed
        private string PromptForFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = DialogResult.Cancel;

            //prompt the user to select the file from the temp folder if in recovery mode or the FTP folder if not in recovery mode
            if (!string.IsNullOrEmpty(Recovery.RecoveryValue))
            {
                MessageBox.Show("The script is running in recovery mode.  Select the file to resume processing from the temporary folder.", "Recovery Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dialog.InitialDirectory = EnterpriseFileSystem.TempFolder;
            }
            else
            {
                dialog.InitialDirectory = EnterpriseFileSystem.FtpFolder;
            }

            //prompt the user until a valid result is obtained
            while (result != DialogResult.OK)
            {
                //prompt the user
                result = dialog.ShowDialog();

                //end the script if the dialog box is canceled
                if (result == DialogResult.Cancel)
                    EndDllScript();

                //verify the file is an AAC ACH file
                if (!dialog.FileName.Contains(SAS_FILE_NAME))
                {
                    MessageBox.Show("Invalid file. Please choose a 'CornerStone AAC ACH' file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result = DialogResult.Cancel;
                }
            }

            //warn user if file empty
            FileInfo fi = new FileInfo(dialog.FileName);
            if (fi.Length == 0)
            {
                MessageBox.Show("CornerStone AAC ACH SAS file is empty.", "File Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EndDllScript();
            }

            //move the file to the temp folder for processing if not in recovery mode
            string newFileName = string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, dialog.SafeFileName);
            if (string.IsNullOrEmpty(Recovery.RecoveryValue))
            {
                try
                {
                    fi.MoveTo(newFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("There was an error while moving the file.  Resolve the problem and run the script again.{0}{0}{1}", Environment.NewLine, ex.Message), "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EndDllScript();
                }
            }

            return newFileName;
        }//PromptForFile


        //process the file
        private void ProcessFile(string fileName)
        {
            using (StreamReader fileReader = new StreamReader(fileName))
            {
                AchData record;
                int lineNumber = 0;
                int recoveryLineNumber = 0;
                int recoveryProcessNumber = 0;
                string ssnToProcess = string.Empty;

                //skip header row
                fileReader.ReadLine();
                lineNumber++;

                //Skip to the last completed record if in recovery.
                if (!string.IsNullOrEmpty(Recovery.RecoveryValue))
                {
                    recoveryLineNumber = int.Parse(Recovery.RecoveryValue.Split(',')[0]);
                    recoveryProcessNumber = int.Parse(Recovery.RecoveryValue.Split(',')[1]);
                    do
                    {
                        record = new AchData(fileReader.ReadLine());
                        lineNumber++;
                    } while (lineNumber != recoveryLineNumber);
                }//if
                else
                {
                    record = new AchData(fileReader.ReadLine());
                    lineNumber++;
                }

                //just in case the last row was in the recovery file and there isn't really anything to process
                if (fileReader.EndOfStream) { return; }
                bool keepProcesing = true;

                //Process the remaining records.
                while (keepProcesing)
                {
                    //set up new values for borrower to process
                    recoveryLineNumber = lineNumber;
                    List<AchData> data = new List<AchData>();
                    ssnToProcess = record.Ssn;

                    //add records to list until a record for a new ssn is read in
                    while (ssnToProcess == record.Ssn)
                    {
                        data.Add(record);
                        if (!fileReader.EndOfStream)
                        {
                            record = new AchData(fileReader.ReadLine());
                            lineNumber++;
                        }
                        else
                        {
                            keepProcesing = false;
                            break;
                        }
                    }

                    Eoj.Counts[EojTotalRecords].Increment();


                    //add record to error report if data missing
                    if (data[0].IsMissingData)
                    {
                        HandleError("Required fields were missing from the file.", data[0], false);
                    }
                    else
                    {
                        bool continueProcessing = true;
                        //update the first screen of TS7O                       
                        if (recoveryProcessNumber < 2)
                        {
                            continueProcessing = UpdateFirstScreen(data);
                            Recovery.RecoveryValue = string.Format("{0},1", recoveryLineNumber);
                        }
                        //continue processing if the first page of TS7O was updated
                        if (continueProcessing)
                        {
                            //update the second screen of TS7O
                            if (recoveryProcessNumber < 2)
                            {
                                continueProcessing = UpdateSecondScreen(data);
                                Recovery.RecoveryValue = string.Format("{0},2", recoveryLineNumber);
                            }
                            //continue processing if the second page of TS7O was updated
                            if (continueProcessing)
                            {
                                //update the billing method
                                if (recoveryProcessNumber < 3)
                                {
                                    if (!string.IsNullOrEmpty(data[0].Email)) { UpdateBillingMethod(data.Where(p => !string.IsNullOrEmpty(p.LoanSeq))); }
                                    Recovery.RecoveryValue = string.Format("{0},3", recoveryLineNumber);
                                }
                                //add an activity record
                                if (recoveryProcessNumber < 4)
                                {
                                    List<int> loans = data.Where(p => !string.IsNullOrEmpty(p.LoanSeq)).Select(q => int.Parse(q.LoanSeq)).ToList();
                                    if (!Atd22ByLoan(data[0].Ssn, "AAUTO", "Autopay was successfully added from EA27", string.Empty,loans, ScriptId, false))
                                    {
                                        HandleError(string.Format("CornerStone AAC ACH Errors {0}",DateTime.Now), data[0], false);
                                    }
                                    Eoj.Counts[EojTotalComplete].Increment();
                                    Recovery.RecoveryValue = string.Format("{0},4", recoveryLineNumber);
                                }
                            }
                        }
                    }

                    Recovery.RecoveryValue = string.Format("{0},4", recoveryLineNumber);
                    recoveryProcessNumber = 0;

                }//while
            }//using

            Recovery.Delete();
        }//ProcessFile()


        //update the first screen of TS7O
        private bool UpdateFirstScreen(List<AchData> data)
        {
            FastPath(string.Format("TX3ZATS7O{0}", data[0].Ssn));
            PutText(12, 42, data[0].AbaRoutingNumber);
            PutText(14, 42, data[0].BankAccountNumber);
            PutText(16, 42, data[0].DayDue);
            Hit(ReflectionInterface.Key.Enter);
            if (!CheckForText(1, 72, "TSX7K"))
            {
                HandleError(GetText(23, 2, 78), data[0], false);
                return false;
            }
            return true;
        }//UpdateFirstScreen


        //update the second screen of TS7O
        private bool UpdateSecondScreen(List<AchData> data)
        {
            PutText(9, 18, data[0].BankAccountType);
            PutText(12, 57, "Y");
            PutText(13, 57, data[0].EftSource);
            bool noLoansSelected = true;
            bool loansNotOnTS7O = false;

            int row = 17;
            while (true)
            {
                foreach (AchData d in data)
                {
                    if (CheckForText(row, 23, d.FirstDisbDate.ToString("MM/dd/yyyy")) && CheckLoanProgram(row, d.LoanProgram))
                    {
                        PutText(row, 3, "A");
                        d.LoanSeq = GetText(row, 10, 4);
                        noLoansSelected = false;
                        break;
                    }
                }
                if (!CheckForText(row, 3, "A")) { loansNotOnTS7O = true; }
                row++;
                if (row == 22 || !CheckForText(row, 3, "_"))
                {
                    Hit(ReflectionInterface.Key.F8);
                    if (CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
                    {
                        break;
                    }
                    else
                    {
                        row = 17;
                    }
                }
            }//End while

            Hit(ReflectionInterface.Key.Enter);
            bool successfulUpdate = CheckForText(23, 2, "01004 RECORD SUCCESSFULLY ADDED");

            if (successfulUpdate)
            {
                PutText(10, 18, "P", ReflectionInterface.Key.Enter);
            }

            if (noLoansSelected)
            {
                HandleError("There were no eligible loans for ACH", data[0], true);
                return false;
            }
            else if (loansNotOnTS7O)
            {
                HandleError("Not all loans were eligible for ACH", data[0], true);
                List<int> loans = data.Where(p => !string.IsNullOrEmpty(p.LoanSeq)).Select(q => int.Parse(q.LoanSeq)).ToList();
                Atd22ByLoan(data[0].Ssn, "AAUTO", "Autopay was successfully added from EA27", string.Empty,loans, ScriptId, false);
                return false;
            }
            else if (!successfulUpdate)
            {
                HandleError("Error adding ACH", data[0], true);
                return false;
            }

            return true;

        }//UpdateSecondScreen


        //check to see if the loan program matches
        private bool CheckLoanProgram(int row, string loanProgram)
        {
            if (loanProgram == "CNS")
            {
                return (CheckForText(row, 41, "UNCNS") || CheckForText(row, 41, "SUBCNS"));
            }
            else if (loanProgram == "SPC")
            {
                return (CheckForText(row, 41, "SUBSPC") || CheckForText(row, 41, "UNSPC"));
            }
            else
            {
                return CheckForText(row, 41, loanProgram);
            }
        }//CheckLoanProgram


        //update the billing method on TX7C
        private void UpdateBillingMethod(IEnumerable<AchData> data)
        {
            FastPath("TX3ZCTS7C" + data.First().Ssn);
            if (CheckForText(1, 72, "TSX3S"))
            {
                int row = 7;
                while (true)
                {
                    foreach (AchData d in data)
                    {
                        if (GetText(row, 20, 4).Replace("0", string.Empty) == d.LoanSeq.Replace(" ","").Replace("0",""))
                        {
                            PutText(22, 19, GetText(row, 3, 2), ReflectionInterface.Key.Enter, true);
                            SetBillingPreference(d);
                            Hit(ReflectionInterface.Key.F12);
                            break;
                        }
                    }
                    row++;
                    if (row == 22 || GetText(row, 4, 1) == string.Empty)
                    {
                        Hit(ReflectionInterface.Key.F8);
                        if (CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
                        {
                            break;
                        }
                        else
                        {
                            row = 7;
                        }
                    }
                }
            }//if

            //target screen.
            if (CheckForText(1, 72, "TSX7D"))
            {
                SetBillingPreference(data.First());
            }
        }//UpdateBillingMethod


        //set billing preference on TS7C
        private void SetBillingPreference(AchData data)
        {
            PutText(19, 42, "Y");
            if (data.HasNoGracePeriod && CheckForText(18, 19, "_")) { PutText(18, 19, "0"); }
            if (CheckForText(14, 48, "_")) { PutText(14, 48, "N"); }
            Hit(ReflectionInterface.Key.Enter);
        }//SetBillingPreference


        //handle errors
        private void HandleError(string errMessage, AchData data, bool addArc)
        {
            bool arcAdded = false;
            data.AbaRoutingNumber = DataAccessHelper.ExecuteSingle<string>("EncryptString", DataAccessHelper.Database.ProcessLogs, new SqlParameter("text", data.AbaRoutingNumber));
            data.BankAccountNumber = DataAccessHelper.ExecuteSingle<string>("EncryptString", DataAccessHelper.Database.ProcessLogs, new SqlParameter("text", data.BankAccountNumber));

            //add activity record and error report if needed and record results            
            if (addArc)
            {
                arcAdded = Atd22AllLoans(data.Ssn, "APUPD", errMessage,string.Empty,ScriptId, false);

                ErrorDataBu errData = new ErrorDataBu(data);
                string message = string.Format("Account: {0} ABA: {1} Amount: {2} Additional Amount: {3} DueDate: {4} Source {5} Aes Account NUmber {6} Type {7}", errData.Acct, errData.ABA, errData.Amt, errData.Add_Amt, errData.Due, errData.Source, data.AccountNumber, errData.Type);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, string.Format("{0} {1}", errMessage, message), NotificationType.ErrorReport, NotificationSeverityType.Warning);
                _hasErrorReport = true;
            }

            //add to error report if activity record not added
            if (!arcAdded)
            {
                ErrorData errData = new ErrorData(data);
                string message = string.Format("Aes Account Number: {0}, Routing NUmber: {1} Account NUmber:{2} Account Type: {3} Monthly Installment: {4} Additional Amount: {5} Due Date: {6} Source {7} Award Type {8} Loan Type {9} First Disb Date: {10}", data.AccountNumber, errData.ABA_Routing_Number, errData.Bank_Account_Number, errData.Bank_Account_Type, errData.Monthly_Installment, errData.Additional_Amount, errData.Due_Date, errData.Source_of_Application, errData.Award_Type, errData.Loan_Type, errData.First_Disbursement_Date);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, string.Format("{0} {1}", errMessage, message), NotificationType.ErrorReport, NotificationSeverityType.Warning);
                _hasErrorReport = true;
            }

            Eoj.Counts[EojTotalError].Increment();
        }//HandleError
    }
}
