using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace RTRNMAILOL
{
    public class Process
    {
        private readonly DemographicsProcessor DemoProc;
        public ReflectionInterface RI { get; set; }
        public static int SUCCESS { get; set; } = 0;
        public static int ERROR { get; set; } = 1;
        public DataAccess DA { get; set; }

        public Process(ReflectionInterface ri)
        {
            RI = ri;
            DA = new DataAccess(ri.LogRun);
            DemoProc = new DemographicsProcessor(ri, DA);
        }

        public int Run(bool pauseBetweenRecords)
        {
            int hadError = 0;
            List<BarcodeData> dataRecords = DA.GetRecordsToProcess();
            foreach (BarcodeData data in dataRecords)
            {
                WriteLine($"Processing BarcodeDataId: {data.BarcodeDataId} for account: {data.AccountIdentifier}");
                if (data.AccountIdentifier.ToUpper().StartsWith("RF@"))
                {
                    RI.FastPath($"LP2CI;{data.AccountIdentifier}");
                    data.BorrowerSsn = RI.GetText(3, 39, 9);
                    data.IsReference = true;
                }
                SystemBorrowerDemographics demos = DA.GetDemos(data.BorrowerSsn ?? data.AccountIdentifier);
                if (demos == null || demos.AccountNumber.IsNullOrEmpty())
                {
                    string message = $"The SSN or Account Number for {data.AccountIdentifier} was not found in the system. Barcode Data Id: {data.BarcodeDataId}";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    WriteLine(message);
                    continue;
                }
                data.Ssn ??= demos.Ssn;
                data.AccountNumber ??= demos.AccountNumber;
                List<string> personTypes = DemoProc.GetPersonTypes(data);
                DemographicsProcessor.DemographicsUpdateStatus updateStatus = data.IsReference ? DemoProc.InvalidateReference(data) : DemoProc.InvalidateBorrower(data);
                if (updateStatus != DemographicsProcessor.DemographicsUpdateStatus.Failed)
                {
                    hadError += AddComment(data, personTypes, updateStatus, data.LetterId.ToUpper().Contains("ULDSBDLDF1"));
                    if (data.LetterId.ToUpper() == "ULDSBDLDF1" && !data.IsReference)
                        DemoProc.MarkDefaultInvalid(data.Ssn);
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

        private void CheckForPause(bool pauseBetweenRecords)
        {
            if (pauseBetweenRecords)
            {
                WriteLine("Please review the account then set this console in focus and hit enter to process the next account");
                ReadKey();
            }
        }

        /// <summary>
        /// Invalidates borrower or reference address
        /// </summary>
        private int AddComment(BarcodeData data, List<string> personTypes, DemographicsProcessor.DemographicsUpdateStatus updateStatus, bool isDefault)
        {
            string ADD_NEW_ADDRESS = "Received return mail from post office, updated address with provided forwarding address.";
            string INVALIDATED_COMMENT = "Received return mail from post office, address invalidated.";
            string SKIPPED_COMMENT = "Received return mail from post office, address validity is more recent than the date letter was sent.";

            if (isDefault)
            {
                INVALIDATED_COMMENT += " ULDSBDLDF1 indicator was changed to Y.";
                SKIPPED_COMMENT += " ULDSBDLDF1 indicator was changed to Y.";
            }

            string comment = data.Comment.IsPopulated() ? ADD_NEW_ADDRESS : INVALIDATED_COMMENT;
            if (data.Comment.IsPopulated())
            {
                comment = data.Comment;
                SKIPPED_COMMENT = data.Comment;
            }
            switch (updateStatus)
            {
                case DemographicsProcessor.DemographicsUpdateStatus.Updated:
                    return DemoProc.AddComment(data, comment, personTypes) ? SUCCESS : ERROR;
                case DemographicsProcessor.DemographicsUpdateStatus.Skipped:
                    return DemoProc.AddComment(data, SKIPPED_COMMENT, personTypes) ? SUCCESS : ERROR;
                case DemographicsProcessor.DemographicsUpdateStatus.Failed:
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
            string subject = "Barcode Scanner Script OneLINK Demographic Invalidation Failed";
            string body;
            if (!RI.CheckForText(2, 33, "BORROWER DEMOGRAPHICS"))
            {
                string accountNumber = "BAD ACCOUNT NUMBER FORMAT";
                if (data.AccountIdentifier.Length == 10 || data.IsReference)
                    accountNumber = data.AccountIdentifier;
                else if (data.AccountIdentifier.Length == 9)
                    accountNumber = "borrower ending with " + data.AccountIdentifier.Substring(5, 4);
                body = $"The Barcode Scanner script was unable to update a OneLINK demographic record for {accountNumber}. Unable to obtain demographics, LP22 unreachable";
            }
            else
                body = $"The Barcode Scanner script was unable to update a OneLINK demographic record for {RI.GetText(4, 34, 13)} {RI.GetText(4, 53, 1)} {RI.GetText(4, 6, 23)} ({RI.GetText(3, 34, 12).Replace(" ", "")}).";

            EmailHelper.SendMail(DataAccessHelper.TestMode, recipient, "ReturnMail@utahsbr.edu", subject, body, "", EmailHelper.EmailImportance.High, false);
        }

        /// <summary>
        /// Determines if a borrower or reference to update address
        /// </summary>
        private int UpdateAddress(BarcodeData data)
        {
            if (data.IsReference)
                return DemoProc.UpdateReferenceAddress(data);
            else
                return DemoProc.UpdateBorrowerAddress(data);
        }

        /// <summary>
        /// Sends an email to the borrower and all their references that live in CA
        /// </summary>
        public void SendCAMail(string accountNumber)
        {
            foreach (BorrowerEmailData rec in DA.GetCAEmailData(accountNumber))
            {
                if (rec.Priority.IsIn(0, 1)) //Priority 0 gets the account for the passed in account number, 1 is always borrower
                    DA.InsertEmailBatch(rec, false, rec.AccountNumber); //The EmailData always has borrower account number
                else if (rec.Priority.IsIn(2, 3))//Priority 2/3 should always be coborrowers/endorsers
                {
                    if (DA.InsertEmailBatch(rec, true, accountNumber)) //The account number here will be the endorsers account number
                        AddEmailComment(rec);
                }
            }
        }

        private void AddEmailComment(BorrowerEmailData data)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = data.BorSsn,
                Arc = "D030A",
                ArcTypeSelected = ArcData.ArcType.OneLINK,
                ActivityContact = "90",
                ActivityType = "LT",
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
                string message = $"There was an error adding a D030A ARC for borrower {data.AccountNumber} for returned mail from endorser.";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
            }
        }
    }
}