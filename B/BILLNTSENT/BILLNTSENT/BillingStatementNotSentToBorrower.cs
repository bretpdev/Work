using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace BILLNTSENT
{
    public class BillingStatementNotSentToBorrower : BatchScript
    {
        private const string fileName = "ULWS17.LWS17R2";

        private const string EojProcessed = "Total number of records processed";
        private const string EojFileTotal = "Total number of records in the file";
        private const string EojErrors = "Error adding comment to borrower account";
        private static readonly string[] EOJ_FIELDS = { EojProcessed, EojFileTotal, EojErrors };
        private bool IsInRecovery;

        public BillingStatementNotSentToBorrower(ReflectionInterface ri)
            : base(ri, "BILLNTSENT", "ERR_BU35", "EOJ_BU35", EOJ_FIELDS, DataAccessHelper.Region.Uheaa)
        {
        }

        public override void Main()
        {
            StartupMessage("This script enters activity records for borrowers listed in file produced by SAS for which a billing statement was not sent.");

            IsInRecovery = !Recovery.RecoveryValue.IsNullOrEmpty();

            DataAccess data = new DataAccess();

            string filePath = ScriptHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, fileName + "*", ScriptHelper.FileOptions.ErrorOnMissing);

            //End the script if there are no files to process
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("There are no SAS files to process", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                EndDllScript();
            }

            //Load the file into the BorrowerData object
            List<BorrowerData> borData = LoadData(filePath);

            //End the script if the file is empty
            if (borData.Count == 0)
            {
                MessageBox.Show("The SAS file is emtpy", "Empty File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                File.Delete(filePath);
                EndDllScript();
            }

            //Add ARC's
            if (borData != null)
                ProcessArc(borData);

            if (Err.HasErrors)
            {
                List<string> emailList = DataAccess.GetEmailList();
                string to = string.Empty;
                for (int i = 0; i < emailList.Count; i++)
                {
                    to += emailList[i] += ".utahsbr.edu,";
                }
                to = to.Remove(to.LastIndexOf(','), 1);
                string subject = "Errors Found in Bill Not Sent to Borrower Script";
                string body = "There were errors found in the Bill Not Sent To Borrower script. Please check the report at " +
                    EnterpriseFileSystem.GetPath("ERR_BU35") + "BILLNTSENT.html";
                SendMail(to, "customerservice@utahsbr.edu", subject, body, "", "", "", EmailImportance.High, false);
            }

            File.Create(EnterpriseFileSystem.LogsFolder + "MBSBILLNTSENT.txt");
            ProcessingComplete();
        }

        /// <summary>
        /// Loads the sas file into a Borrower Data object
        /// </summary>
        /// <param name="filePath">The full path of the file</param>
        /// <returns>List of borrower data objects</returns>
        private List<BorrowerData> LoadData(string filePath)
        {
            List<BorrowerData> data = new List<BorrowerData>();
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        BorrowerData bData = new BorrowerData();
                        List<string> fileData = sr.ReadLine().SplitAndRemoveQuotes(",");
                        bData.AccountNumber = fileData[0];
                        bData.LoanSequence = int.Parse(fileData[1]);
                        bData.DateBilled = fileData[2];
                        bData.DateDue = fileData[3];
                        bData.BillAmount = fileData[4];
                        bData.SentIndicator = fileData[5];
                        bData.SentDescription = fileData[6];
                        bData.BillMethodType = fileData[7];
                        data.Add(bData);
                        if (!IsInRecovery)
                            Eoj.Counts[EojFileTotal].Increment();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n\r\n" + ex.InnerException);
                EndDllScript();
            }
            return data;
        }

        private void ProcessArc(List<BorrowerData> data)
        {
            foreach (BorrowerData bor in data)
            {
                if (IsInRecovery && (Recovery.RecoveryValue != (bor.AccountNumber + "," + bor.LoanSequence.ToString())))
                    continue;
                else
                {
                    //If the recovery value was found, skip the borrower/loan seq because it is done and set the recovery to false to process
                    //the rest of the file
                    if (IsInRecovery)
                    {
                        IsInRecovery = false;
                        continue;
                    }
                    string accountNo = string.Empty;
                    string com = string.Empty;
                    if (bor.BillMethodType == "2")
                        com = string.Format("Date Billed: {0}, Date Due: {1}, Bill Amount: {2:C}, {3} BORROWER REQUESTED ELECTRONIC BILLING", bor.DateBilled, bor.DateDue, bor.BillAmount, bor.SentIndicator);
                    else if (bor.BillMethodType.ToUpper() == "E")
                        com = string.Format("Date Billed: {0}, Date Due: {1}, Bill Amount: {2:C}, {3} BORROWER IS ON ACH", bor.DateBilled, bor.DateDue, bor.BillAmount, bor.SentIndicator);
                    else
                        com = string.Format("Date Billed: {0}, Date Due: {1}, Bill Amount: {2:C}, {3} {4}", bor.DateBilled, bor.DateDue, bor.BillAmount, bor.SentIndicator, bor.SentDescription);

                    if (!Atd22ByLoan(bor.AccountNumber, "BILLN", com, string.Empty, new List<int>(bor.LoanSequence), "BILLNTSENT", false))
                    {
                        if (!Atd37(bor.AccountNumber, "BILLN", com, string.Empty, new List<int>(bor.LoanSequence), "BILLNTSENT", false))
                        {
                            Err.AddRecord(EojErrors, bor);
                            Eoj[EojErrors].Increment();
                        }
                        else
                            Eoj.Counts[EojProcessed].Increment();
                    }
                    else
                        Eoj.Counts[EojProcessed].Increment();

                    Recovery.RecoveryValue = bor.AccountNumber + "," + bor.LoanSequence.ToString();
                }
            }
        }

    }//class
}//namespace
