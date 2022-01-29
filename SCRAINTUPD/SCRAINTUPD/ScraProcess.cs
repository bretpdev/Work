using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace SCRAINTUPD
{
    public partial class ScraProcess
    {
        public string ScriptId { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        public LogDataAccess LDA { get; set; }
        public IReflectionInterface RI { get; set; }
        public DataAccessHelper.Database DB { get; set; }
        public DataAccessHelper.Region Region { get; set; }
        public DateTime defaultBeginDate = new DateTime(2008, 08, 14);
        public DateTime defaultEndDate = new DateTime(2099, 12, 31);
        public DateTime oneYearFuture = DateTime.Now.AddYears(1);
        public DateTime today = DateTime.Now;

        public ScraProcess(ProcessLogRun logRun, DataAccessHelper.Region region)
        {
            ScriptId = "SCRAINTUPD";
            LogRun = logRun;
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, true);
            DB = DataAccessHelper.Database.Uls;
            DA = new DataAccess(LDA, DB);
            RI = new ReflectionInterface();
            Region = region;
        }

        public int Process()
        {
            string dbRegion = "BatchUheaa";

            if (BatchProcessingLoginHelper.Login(LogRun, RI, ScriptId, dbRegion) == null)
            {
                LogRun.AddNotification(string.Format("Failed to log in for all users in {0} type. Restart this script when Ids become available.", "BatchUheaa"), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                LogRun.LogEnd();
                return 1;
            }

            int result = ScraProcessing();
            RI.CloseSession();
            LogRun.LogEnd();
            return result;
        }

        public int ScraProcessing()
        {
            List<ScraRecord> RecordsToProcess = DA.GetUnprocessedRecords();
            if (RecordsToProcess.Count == 0)
            {
                LogRun.AddNotification("No records to process. Ending script.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                return 0;
            }

            foreach (var borrowerRecords in RecordsToProcess.GroupBy(p => p.BorrowerSSN))
                ProcessBorrower(borrowerRecords);

            return 0;
        }


        public void ProcessBorrower(IGrouping<string, ScraRecord> borrowerRecords)
        {
            List<ScraRecord> recordsByBorrower = borrowerRecords.ToList();
            recordsByBorrower = RemoveSpecialBypass(recordsByBorrower);
            if (!recordsByBorrower.Any()) //no records left after removing special bypass
                return;
            recordsByBorrower = InterestProcessing(recordsByBorrower);
            if (recordsByBorrower == null) 
                return;

            if (!recordsByBorrower.All(p => p.LN72RegRate < 6.00m && p.ScriptAction == "E")) //Special case scenario for interest only processing
            {
                recordsByBorrower = RediscloseProcessing(recordsByBorrower);
                if (recordsByBorrower == null || recordsByBorrower.Any(p => !p.TS0NUpdatedAt.HasValue))
                    return;

                if (!recordsByBorrower.All(p => p.CalcSchedules))
                {
                    recordsByBorrower = ScheduleProcessing(recordsByBorrower);
                    if (recordsByBorrower == null)
                        return;
                }
            }
            else
            {
                foreach (ScraRecord record in recordsByBorrower)
                    DA.SetAllSpecial(record);

                AddArc(recordsByBorrower, "PSCRA", "SCRA interest rates updated.  Exempt from disclosure processing.");
                return;
            }
            AddArc(recordsByBorrower, "PSCRA");
        }

        private List<ScraRecord> RemoveSpecialBypass(List<ScraRecord> recordsForBorrower)
        {
            foreach (var record in recordsForBorrower.ToArray())
            {
                if (record.SpecialBypass)
                {
                    DA.SetRecordSpecialBypass(record.ScriptProcessingId);
                    recordsForBorrower.Remove(record);
                }
            }
            return recordsForBorrower;
        }

        public void ErrorProcessing(List<ScraRecord> records, string arc)
        {
            AddProcessLogger(records);
            AddArc(records, arc);
            SetErrorDate(records);
        }

        public void AddProcessLogger(List<ScraRecord> records)
        {
            string errorMessage = "";
            NotificationSeverityType severity = NotificationSeverityType.Informational;

            if (!records.Any(p => p.SessionErrorMessage.IsPopulated()))
            {
                errorMessage = string.Format("An unknown error occured while processing Account number:{0}", records.FirstOrDefault().AccountNumber);
                severity = NotificationSeverityType.Critical;
            }
            else
            {
                foreach (ScraRecord record in records.Where(p => p.SessionErrorMessage.IsPopulated()))
                {
                    if (!record.TS06UpdatedSuccessfully)
                        errorMessage = string.Format("Error handling TS06 interest updates for Acccount number:{0}. Session message: {1}", record.AccountNumber, record.SessionErrorMessage);
                    else if (!record.TS0NUpdatedSuccessfully)
                        errorMessage = string.Format("Error handling TS0N redisclosure updates for Acccount number:{0}. Session message: {1}", record.AccountNumber, record.SessionErrorMessage);
                    else if (!record.CalcSchedules)
                        errorMessage = string.Format("Error applying new repayment schedule for Acccount number:{0}. Session message: {1}", record.AccountNumber, record.SessionErrorMessage);
                }
            }
            int processNotification = LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, severity);

            foreach (ScraRecord record in records)
                DA.AddProcessLoggerMapping(record, processNotification, LogRun.ProcessLogId);
        }

        public void AddArc(List<ScraRecord> records, string arcToAdd, string comment = "", bool specialCase = false)
        {
            if (arcToAdd == "NSCRA")
                comment = SetCommentForNSCRA(records) + " " + comment;
            else if (arcToAdd == "MSCRA")
                comment = "SCRA Rates Adjusted. Needs new RPS. " + comment;
            else if (arcToAdd == "PSCRA" && specialCase && comment != "SCRA interest rates updated.  Exempt from disclosure processing.")
                comment = "SCRA Rates Adjusted, borrower exempt from redisclosure. " + comment;
            else if (arcToAdd == "PSCRA" && !specialCase && comment != "SCRA interest rates updated.  Exempt from disclosure processing.")
                comment = "SCRA Rates Adjusted and Redisclosed. " + comment;

            ArcData arcRecord = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = records.FirstOrDefault().AccountNumber,
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
            if (result.ArcAdded)
            {
                foreach (ScraRecord record in records)
                {
                    record.ArcAddProcessingId = result.ArcAddProcessingId;
                    DA.SetArcAddId(record);
                }

            }
            else
                LogRun.AddNotification(string.Format("Unable to add arc and comment to the following borrower: {0} , ARC: {1}, Comment: {2}.", records.FirstOrDefault().AccountNumber, arcToAdd, comment), NotificationType.ErrorReport, NotificationSeverityType.Critical, result.Ex);
        }

        public static string SetCommentForNSCRA(List<ScraRecord> records)
        {
            //var dictionary = records.ToDictionary(o => Tuple.Create(o.DODBeginDate, o.DODEndDate));
            Dictionary<Tuple<DateTime?, DateTime?>, List<short>> dictionary = new Dictionary<Tuple<DateTime?, DateTime?>, List<short>>();
            foreach (ScraRecord record in records)
            {
                var key = Tuple.Create<DateTime?, DateTime?>(record.DODBeginDate, record.DODEndDate);
                if (!dictionary.ContainsKey(key))
                {
                    List<short> sequences = new List<short>();
                    dictionary.Add(key, sequences); //get start and end dates by loan sequence
                }
                dictionary[key].Add(record.LoanSequence);
            }
            
            var loanGroup = dictionary.Select(p => string.Format("Loans {0}: DOD Begin {1}, DOD End {2}.", string.Join(";", p.Value.Select(q => q.ToString()).OrderBy(r => r.ToIntNullable()).ToArray()), p.Key.Item1.Value.ToString("MM/dd/yyyy"), p.Key.Item2.Value.ToString("MM/dd/yyyy")));
            return string.Format("{0}\r\n.\r\nNeeds TS06 and new RPS.", string.Join("\r\n", loanGroup));
        }

        public void SetErrorDate(List<ScraRecord> records)
        {
            foreach (ScraRecord record in records)
                DA.SetErroredAtDate(record);
        }
    }
}