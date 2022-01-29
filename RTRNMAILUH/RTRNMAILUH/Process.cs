using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;
using static RTRNMAILUH.DemographicsProcessor;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace RTRNMAILUH
{
    public class Process
    {
        private readonly DemographicsProcessor DemoProc;
        public const int SUCCESS = 0;
        public const int ERROR = 1;
        public ReflectionInterface RI { get; set; }
        public DataAccess DA { get; set; }

        public Process(ReflectionInterface ri)
        {
            RI = ri;
            DA = new DataAccess(RI.LogRun);
            DemoProc = new DemographicsProcessor(ri, DA);
        }

        public int Run(bool pauseBetweenRecords)
        {
            int hadError = 0;
            List<BarcodeData> dataRecords = DA.GetRecordsToProcess();
            if (dataRecords.Count == 0)
            {
                WriteLine("There are no records to process at this time.");
                return SUCCESS;
            }
            foreach (BarcodeData data in dataRecords)
            {
                WriteLine($"Processing BarcodeDataId: {data.BarcodeDataId} for account: {data.AccountIdentifier}");
                if (data.AccountIdentifier.ToUpper().StartsWith("P"))
                {
                    data.BorrowerSsn = GetBorSsnForReference(data.AccountIdentifier);
                    data.IsReference = true;
                }
                SystemBorrowerDemographics demos = DA.GetDemos(data.AccountIdentifier);
                if (demos == null || demos.AccountNumber.IsNullOrEmpty())
                {
                    string message = $"The SSN or Account Number for {data.AccountIdentifier} was not found in the system. Barcode Data Id: {data.BarcodeDataId}";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    WriteLine(message);
                    continue;
                }
                data.Ssn ??= demos.Ssn;
                data.AccountNumber ??= demos.AccountNumber.Trim();
                List<string> personTypes = DemoProc.GetPersonTypes(data);
                DemographicsUpdateStatus updateStatus = data.IsReference ? DemoProc.InvalidateReference(data) : DemoProc.InvalidateBorrower(data);
                if (updateStatus != DemographicsUpdateStatus.Failed)
                {
                    if (data.ArcAddProcessingId == 0)
                        hadError += AddComment(data, personTypes, updateStatus);
                    if (data.Address1.IsPopulated())
                        hadError += UpdateAddress(data);
                    //Send email to everyone associated on the account that lives in CA.
                    SendCAMail(data.AccountNumber);
                    DA.SetProcessed(data.BarcodeDataId);
                }
                CheckForPause(pauseBetweenRecords);
            }

            return hadError > 0 ? ERROR : SUCCESS;
        }

        public void CheckForPause(bool pauseBetweenRecords)
        {
            if (pauseBetweenRecords)
            {
                WriteLine("Please review the account then set this console in focus and hit enter to process the next account");
                ReadKey();
            }
        }

        /// <summary>
        /// Invalidates the Compass address
        /// </summary>
        private int AddComment(BarcodeData data, List<string> personTypes, DemographicsUpdateStatus updateStatus)
        {
            string ADD_NEW_ADDRESS = "Received return mail from post office, updated address with provided forwarding address.";
            string INVALIDATED_COMMENT = "Received return mail from post office, address invalidated.";
            string SKIPPED_COMMENT = "Received return mail from post office, address validity is more recent than the date letter was sent.";
            string comment = data.Comment.IsPopulated() ? ADD_NEW_ADDRESS : INVALIDATED_COMMENT;
            if (data.Comment.IsPopulated())
            {
                comment = data.Comment;
                SKIPPED_COMMENT = data.Comment;
            }
            
            switch (updateStatus)
            {
                case DemographicsUpdateStatus.Updated:
                    return DemoProc.AddComment(data, comment, personTypes) ? SUCCESS : ERROR;
                case DemographicsUpdateStatus.Skipped:
                    return DemoProc.AddComment(data, SKIPPED_COMMENT, personTypes) ? SUCCESS : ERROR;
                case DemographicsUpdateStatus.Failed:
                    SendEmail(data);
                    return ERROR;
                default:
                    return ERROR;
            }
        }

        /// <summary>
        /// Sends email to manager of doc services informing them there was an error invalidating 
        /// </summary>
        private void SendEmail(BarcodeData data)
        {
            string recipient = DA.GetDocServicesManagerEmail();
            string subject = "Barcode Scanner Script Compass Demographic Invalidation Failed";
            string body;
            if (!RI.CheckForText(2, 33, "BORROWER DEMOGRAPHICS"))
            {
                string accountNumber = "BAD ACCOUNT NUMBER FORMAT";
                if (data.AccountIdentifier.Length == 10 || data.IsReference)
                    accountNumber = data.AccountIdentifier;
                else if (data.AccountIdentifier.Length == 9)
                    accountNumber = "borrower ending with " + data.AccountIdentifier.Substring(5, 4);
                body = $"The Barcode Scanner script was unable to update a Compass demographic record for {accountNumber}. Unable to obtain demographics, CTX1J unreachable";
            }
            else
                body = $"The Barcode Scanner script was unable to update a Compass demographic record for {RI.GetText(4, 34, 13)} {RI.GetText(4, 53, 1)} {RI.GetText(4, 6, 23)} ({RI.GetText(3, 34, 12).Replace(" ", "")}).";

            EmailHelper.SendMail(DataAccessHelper.TestMode, recipient, "ReturnMail@utahsbr.edu", subject, body, "", EmailHelper.EmailImportance.High, false);
            WriteLine($"Error email sent to {recipient} informing them of errors invalidating address for barcode data id: {data.BarcodeDataId}.");
        } 

        public void SendCAMail(string accountNumber)
        {
            foreach (BorrowerEmailData rec in DA.GetCAEmailData(accountNumber))
            {
                if (rec.Priority.IsIn(0, 1)) //Priority 0 gets the account for the passed in account number, 1 is always borrower
                    DA.InsertEmailBatch(rec, false, rec.AccountNumber);
                else if (rec.Priority.IsIn(2, 3)) //Priority 2/3 should always be coborrowers/endorsers
                {
                    if (DA.InsertEmailBatch(rec, true, rec.AccountNumber))
                        AddEmailComment(rec);
                }
            }
        }

        private void AddEmailComment(BorrowerEmailData data)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = data.BorSsn,
                Arc = "CAEEM",
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = "CA Returned Mail Email sent to borrower for endorser",
                IsEndorser = true,
                RecipientId = data.EndSsn,
                RegardsCode = "E",
                RegardsTo = data.EndSsn,
                ScriptId = Program.ScriptId
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"There was an error adding a CAEEM ARC for borrower {data.AccountNumber} for returned mail from endorser.";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
            }
        }

        public int UpdateAddress(BarcodeData data)
        {
            string account = data.IsReference ? data.AccountIdentifier : data.Ssn;
            RI.FastPath($"TX3Z/CTX1J;{account}");
            if (RI.CheckForText(1, 71, "TXX1R"))
            {
                RI.Hit(F6, 2);
                RI.PutText(11, 10, data.Address1, true);
                RI.PutText(12, 10, data.Address2, true);
                RI.PutText(14, 8, data.City, true);
                RI.PutText(14, 32, data.State);
                RI.PutText(14, 40, data.Zip, true);
                RI.PutText(11, 55, "Y");
                if (!RI.GetText(1, 9, 1).IsIn("E", "R"))
                    RI.PutText(8, 18, "25");
                RI.PutText(10, 32, DateTime.Now.ToString("MMddyy"), Enter);

                if (RI.MessageCode != "01096")
                {
                    string message = $"There was an error updating the {(data.IsReference ? "reference" : "borrower")} address for account: {data.AccountIdentifier}, Barcode Data Id: {data.BarcodeDataId}";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    WriteLine(message);
                    return ERROR;
                }
                WriteLine($"Forwarding address updated for barcode data id: {data.BarcodeDataId}");
                return SUCCESS;
            }
            return ERROR;
        }

        /// <summary>
        /// Needs to get the Borrower SSN for the reference
        /// </summary>
        private string GetBorSsnForReference(string referenceId)
        {
            RI.FastPath($"TX3Z/ITX1JR{referenceId}");
            if (RI.CheckForText(1, 71, "TXX1R"))
                return RI.GetText(7, 11, 11).Replace(" ", ""); //SSN of the borrower the reference is for
            return null;
        }
    }
}