using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACTDUTRPT
{
    public partial class ActiveDutyReportProcess
    {
        public const string ScriptId = "ACTDUTRPT";
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        public LogDataAccess LDA { get; set; }
        public IReflectionInterface RI { get; set; }
        private readonly DateTime defaultBeginDate = new DateTime(2008, 08, 14);
        private readonly DateTime defaultEndDate = new DateTime(2099, 12, 31);
        private readonly string defaultNotify = DateTime.Today.ToString("MMddyyyy");
        private readonly DateTime oneYearFuture = DateTime.Now.AddYears(1);

        public ActiveDutyReportProcess(ProcessLogRun logRun)
        {
            LogRun = logRun;
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, true);
            DA = new DataAccess(LDA);
            RI = new ReflectionInterface();
        }

        public int Process()
        {
            string dbRegion =  "BatchUheaa";

            try
            {
                if (BatchProcessingLoginHelper.Login(LogRun, RI, ScriptId, dbRegion, true) == null)
                    throw new Exception();
                return ActiveDutyProcessing();
            }
            catch (Exception ex)
            {
                LogRun.AddNotification($"Failed to log in for all users in {dbRegion} type. Restart this script when Ids become available.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                return 1;
            }
            finally
            {
                RI.CloseSession();
                LogRun.LogEnd();
            }
        }

        public int ActiveDutyProcessing()
        {
            List<ActiveDutyRecord> RecordsToProcess = DA.GetUnprocessedRecords();
            if (RecordsToProcess.Count == 0)
            {
                LogRun.AddNotification("No records to process. Ending script.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                return 0;
            }

            RecordsToProcess.ForEach(o =>
            {
                if (o.ProcessingSSN.IsNullOrEmpty() && o.EndorserSSN.IsNullOrEmpty())
                    o.ProcessingSSN = o.BorrowerSSN;
                else if (o.ProcessingSSN.IsNullOrEmpty() && o.EndorserSSN.IsPopulated())
                    o.ProcessingSSN = o.EndorserSSN;
            });


            foreach (var borrowerRecords in RecordsToProcess.GroupBy(p => p.ProcessingSSN))
                ProcessBorrower(borrowerRecords);

            return 0;
        }


        public void ProcessBorrower(IGrouping<string, ActiveDutyRecord> borrowerRecords)
        {
            List<ActiveDutyRecord> recordsByBorrower = borrowerRecords.ToList();
            if (!recordsByBorrower.Any()) //no records
                return;

            BenefitsProcessing(recordsByBorrower);
        }

        public void ErrorProcessing(ActiveDutyRecord record, string arc)
        {
            AddProcessLogger(record);
            AddArc(record, arc);
            SetErrorDate(record);
        }

        public void AddProcessLogger(ActiveDutyRecord record)
        {
            string errorMessage = "";
            NotificationSeverityType severity = NotificationSeverityType.Informational;

            if (!record.SessionErrorMessage.IsPopulated())
            {
                if (record.EndorserAccountNumber.IsPopulated())
                    errorMessage = string.Format("An unknown error occured while processing a record for coborrower account number: {0} in regards to borrower account number: {1}. ", record.EndorserAccountNumber, record.AccountNumber);
                else
                    errorMessage = string.Format("An unknown error occured while processing Account number: {0}. ", record.AccountNumber);

                severity = NotificationSeverityType.Critical;
            }
            else
            {
                if (record.EndorserAccountNumber.IsPopulated())
                    errorMessage = string.Format("Error handling TXCX benefit updates for coborrower account number: {0} in regards to borrower account number: {1}. ", record.EndorserAccountNumber, record.AccountNumber);
                else
                    errorMessage = string.Format("Error handling TXCX benefit updates for Acccount number: {0}. ", record.AccountNumber);

                errorMessage += string.Join(Environment.NewLine, record.SessionErrorMessage);
            }
            int processNotification = LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, severity);
        }

        public void AddArc(ActiveDutyRecord record, string arcToAdd, string comment = "", bool specialCase = false)
        {
            if (record.EndorserAccountNumber.IsPopulated())
                comment = string.Format("TXCX dates need to be updated for the following coborrower account number: {0} in regards to borrower account number: {1}. TXCX Begin {2}, TXCX End {3}.", record.EndorserAccountNumber, record.AccountNumber, record.TXCXBeginDate.Value.Date.ToShortDateString(), record.TXCXEndDate.Value.Date.ToShortDateString());
            else
                comment = string.Format("TXCX dates need to be updated for the following account number: {0}. TXCX Begin {1}, TXCX End {2}.", record.AccountNumber, record.TXCXBeginDate.Value.Date.ToShortDateString(), record.TXCXEndDate.Value.Date.ToShortDateString());

            ArcData arcRecord = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = record.AccountNumber,
                Arc = arcToAdd,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                IsEndorser = false,
                IsReference = false,
                RecipientId = "",
                RegardsTo = "",
                ScriptId = ScriptId
            };

            ArcAddResults result = arcRecord.AddArc();
            if (!result.ArcAdded)
                LogRun.AddNotification(string.Format("Unable to add arc and comment to the following borrower: {0}, ARC: {1}, Comment: {2}.", record.AccountNumber, arcToAdd, comment), NotificationType.ErrorReport, NotificationSeverityType.Critical, result.Ex);
        }

        public void SetErrorDate(ActiveDutyRecord record)
        {
            DA.SetProcessedTXCX(record, false);
        }
    }
}