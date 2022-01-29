using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace COVIDFORB
{
    public class ArcProcessor
    {
        public ProcessLogRun logRun { get; set; }
        public DataAccess DA { get; set; }

        public ArcProcessor(ProcessLogRun logRun)
        {
            this.logRun = logRun;
            DA = new DataAccess(logRun);
        }

        public void Process()
        {
            AddAggregateArcs();
            //AddClaimsPopulationArcs(); This is being removed since they already have NH tickets working on it
        }

        private int? AddLetter(ArcProcessingRecord record)
        {
            string letterId = "COVIDFORUH";
            string ssn = DA.GetSsnFromAccountNumber(record.AccountNumber);
            string keyline = DocumentProcessing.ACSKeyLine(ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            var demos = DA.GetDemos(record.AccountNumber);
            var ecorr = EcorrProcessing.CheckEcorr(record.AccountNumber, DataAccessHelper.Region.Uheaa);
            string suffix = demos.Suffix.IsNullOrEmpty() ? "" : " " + demos.Suffix;
            string letterData = $"{keyline},{record.AccountNumber},{demos.FirstName} {demos.LastName}{suffix},\"{demos.Address1}\",\"{demos.Address2}\",\"{demos.City}\",\"{demos.State}\",\"{demos.ZipCode}\",\"{demos.Country}\"";
            string costCenter = "MA2324";
            if (!demos.IsValidAddress && !ecorr.LetterIndicator)
            {
                DA.SetInvalidAddressNoEcorr(record.ForbearanceProcessingId);
                return -100; //setting arbitrary number to be used later.  This is not the best way but it will work
            }
            var printProcessingId = EcorrProcessing.AddRecordToPrintProcessing(Program.ScriptId, letterId, letterData, record.AccountNumber, costCenter, DataAccessHelper.Region.Uheaa);

            //Existing print processing id or failure
            if (!printProcessingId.HasValue)
            {
                logRun.AddNotification($"Failed to add print processing record for account {record.AccountNumber}, it may already exist.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
            return printProcessingId;
        }

        //private void AddClaimsPopulationArcs()
        //{
        //    var population = DA.GetClaimsManualReviewPopulation();
        //    var groupedPopulation = population.GroupBy(p => p.AccountNumber);

        //    foreach(var borrower in groupedPopulation)
        //    {
        //        var prio = borrower.Min(p => p.Priority);
        //        var prioRecord = borrower.Where(p => p.Priority == prio).FirstOrDefault();
        //        ArcProcessingRecord record = new ArcProcessingRecord() { AccountNumber = prioRecord.AccountNumber, ArcComment = prioRecord.Comment, BusinessUnitId = 1 }; //There should be only 1 business unit id so we just set it to 1
        //        AddArcError(new List<ArcProcessingRecord> { record }, DA, logRun, prioRecord.Comment, null, false);
        //    }
        //}

        public void AddAggregateArcs()
        {
            var arcProcessingRecords = DA.GetUnprocessedForbArcs();

            //Group by BusinessUnitId and Account Number
            var groupedSuccess = arcProcessingRecords.Where(p => !p.Failure).GroupBy(p => new { p.AccountNumber, p.BusinessUnitId });
            var groupedFailure = arcProcessingRecords.Where(p => p.Failure).GroupBy(p => new { p.AccountNumber, p.BusinessUnitId });


            foreach (var group in groupedSuccess)
            {
                List<ArcProcessingRecord> records = group.ToList();
                var minStart = group.Min(p => p.StartDate);
                var maxEnd = group.Max(p => p.EndDate);
                string successComment = "Forbearance added for ForbearanceProcessingIds: " + string.Join(",", records.Select(p => p.ForbearanceProcessingId)) + ", Start Date: " + minStart.ToShortDateString() + ", End Date: " + maxEnd.ToShortDateString() + " sent COVIDFORUH letter";
                AddArcSuccess(records, DA, logRun, successComment, null);
            }

            foreach (var group in groupedFailure)
            {
                List<ArcProcessingRecord> records = group.ToList();
                string failureComment = $"{records.First().ArcComment}";
                AddArcError(records, DA, logRun, failureComment, null);
            }
        }

        public static void SetProcessedSuccess(ForbProcessingRecord record, DataAccess da, ProcessLogRun logRun)
        {
            string comment = string.Format("Forbearance added for {0} through {1} Forbearance Type: {2}", record.StartDate.ToShortDateString(), record.EndDate.ToShortDateString(), record.ForbearanceType);
            bool set = da.SetProcessedOn(record.ForbearanceProcessingId, comment, false);
            if (!set)
            {
                logRun.AddNotification("Unable to set record processed, ForbearanceProcessingId: " + record.ForbearanceProcessingId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }
        }

        public static void SetProcessedFailure(ForbProcessingRecord record, DataAccess da, ProcessLogRun logRun, string comment)
        {
            bool set = da.SetProcessedOn(record.ForbearanceProcessingId, comment, true);
            if (!set)
            {
                logRun.AddNotification("Unable to set record processed, ForbearanceProcessingId: " + record.ForbearanceProcessingId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }
        }

        public void AddArcError(List<ArcProcessingRecord> records, DataAccess da, ProcessLogRun logRun, string comment, List<int> loans, bool setArcAddId = true)
        {
            string newComment = comment.SafeSubString(0, comment.IndexOf("SSRS report") + 12) + $" Start Date: {records.Select( p=> p.StartDate).Min().ToShortDateString()}, End Date: {records.Select(p => p.EndDate).Max().ToShortDateString()}";

            BusinessUnits bu = da.GetBusinessUnit(records.First().BusinessUnitId);
            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion);
            {
                arc.ArcTypeSelected = loans == null ? ArcData.ArcType.Atd22AllLoans : ArcData.ArcType.Atd22ByLoan;
                arc.ResponseCode = null;
                arc.AccountNumber = records.First().AccountNumber;
                arc.RecipientId = null;
                arc.Arc = bu.ARC;
                arc.ScriptId = Program.ScriptId;
                arc.ProcessOn = DateTime.Now;
                arc.Comment = newComment;
                arc.ProcessFrom = null;
                arc.ProcessTo = null;
                arc.NeedBy = null;
                arc.RegardsTo = null;
                arc.RegardsCode = null;
                arc.LoanSequences = loans;
            }

            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                logRun.AddNotification("Unable to add arc for ForbearanceProcessingIds: " + string.Join(",", records.Select(p => p.ForbearanceProcessingId)), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }
            if (setArcAddId)
            {
                foreach (var record in records)
                {
                    bool set = da.SetArcAddProcessingId(record.ForbearanceProcessingId, result.ArcAddProcessingId, null); //we don't need to send a letter for errors
                    if (!set)
                    {
                        logRun.AddNotification("Unable to set record processed, ForbearanceProcessingId: " + record.ForbearanceProcessingId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        return;
                    }
                }
            }
        }

        public void AddArcSuccess(List<ArcProcessingRecord> records, DataAccess da, ProcessLogRun logRun, string comment, List<int> loans)
        {
            var demos = DA.GetDemos(records.First().AccountNumber);
            var ecorr = EcorrProcessing.CheckEcorr(records.First().AccountNumber, DataAccessHelper.Region.Uheaa);
            string message = "Sent COVIDFORUH letter";
            if (!demos.IsValidAddress && !ecorr.LetterIndicator)
            {
                message = "Did not send COVIDFORUH letter.  Borrower has invalid address and No Ecorr.";
            }
            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion);
            {
                arc.ArcTypeSelected = loans == null ? ArcData.ArcType.Atd22AllLoans : ArcData.ArcType.Atd22ByLoan;
                arc.ResponseCode = null;
                arc.AccountNumber = records.First().AccountNumber;
                arc.RecipientId = null;
                arc.Arc = "FBAPV";
                arc.ScriptId = Program.ScriptId;
                arc.ProcessOn = DateTime.Now;
                arc.Comment = comment ?? string.Format("Forbearance added for {0} through {1} Forbearance Type: {2}.  {3}", records.First().StartDate.ToShortDateString(), records.Last().EndDate.ToShortDateString(), records.First().ForbearanceType, message);
                arc.ProcessFrom = null;
                arc.ProcessTo = null;
                arc.NeedBy = null;
                arc.RegardsTo = null;
                arc.RegardsCode = null;
                arc.LoanSequences = loans;
            }

            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                logRun.AddNotification("Unable to add arc for ForbearanceProcessingIds: " + string.Join(",", records.Select(p => p.ForbearanceProcessingId)), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }
            int? printProcessingId = AddLetter(records.First());
            foreach (var record in records)
            {
                bool set = false;
                if (printProcessingId == -100)
                {
                    set= DA.SetInvalidAddressNoEcorr(record.ForbearanceProcessingId);
                }
                else
                {
                    set = da.SetArcAddProcessingId(record.ForbearanceProcessingId, result.ArcAddProcessingId, printProcessingId);
                }
                if (!set)
                {
                    logRun.AddNotification("Unable to set record processed, ForbearanceProcessingId: " + record.ForbearanceProcessingId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return;
                }
            }
        }
    }
}
