using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHRIRDF
{
    class ACHRIRDF
    {
        ReflectionInterface ri;
        ProcessLogRun plr;
        DataAccess data;
        public ACHRIRDF(ReflectionInterface ri, ProcessLogRun plr, DataAccess data)
        {
            this.ri = ri;
            this.plr = plr;
            this.data = data;
            nextJune30 = new DateTime(DateTime.Now.Year, 6, 30);
            if (nextJune30 < DateTime.Now)
                nextJune30 = nextJune30.AddYears(1);
        }

        public void Process()
        {
            var pendingWork = data.GetPendingQueueWork();
            if (!pendingWork.DatabaseCallSuccessful)
            {
                Error("Unable to retrieve pending work from database.");
                return;
            }
            var result = pendingWork.Result;
            //Flags partial covered R/M/P rates for manual review and removes them from the population of records to be worked
            result = RemoveInvalidR2(result);
            result = SplitVariableR2(result);

            RemoveInvalidR4s(result);

            //Order population by account then by report
            result = result.OrderBy(r => r.AccountNumber).ThenByDescending(r => r.Report).ToList();

            foreach (var queue in result)
            {
                if (queue.Report.StartsWith("R2"))
                {
                    if (queue.VariableRate)
                    {
                        if (!Cts06AlreadyExists(queue))
                        {
                            WorkR2(queue);
                        }
                        //Skip processing if the record already exists
                    }
                    else
                    {
                        WorkR2(queue);
                    }
                }
                else if (queue.Report.StartsWith("R4"))
                    WorkR4(queue);
                else
                {
                    Error("Work record {0} has unrecognized Report {1}", queue.ProcessQueueId, queue.Report);
                    continue;
                }
                data.MarkQueueAsProcessed(queue.ProcessQueueId);
            }
        }

        private List<QueueRecord> RemoveInvalidR2(List<QueueRecord> result)
        {
            List<ReviewRecord> accountsToErrorLogs = new List<ReviewRecord>();
            var r2s = result.Where(o => o.Report.StartsWith("R2")).ToList();
            //Create error message for arc comment
            foreach (var r2 in r2s)
            {
                if (r2.HasPartialReducedRate.HasValue && r2.HasPartialReducedRate.Value == true)
                {
                    var rec = accountsToErrorLogs.Where(r => r.AccountNumber == r2.AccountNumber && r.BeginDate == r2.DefermentOrForbearanceBeginDate && r.EndDate == r2.DefermentOrForbearanceEndDate);
                    if(rec.Count() > 0)
                    {
                        rec.FirstOrDefault().LoanSequences.Add(r2.LoanSequence);
                        //accountsToErrorLogs[r2.AccountNumber] += $"Date period has partial reduced rate, loan sequence: {r2.LoanSequence.ToString()} begin date: {r2.DefermentOrForbearanceBeginDate.Value.ToShortDateString()}, end date: {r2.DefermentOrForbearanceEndDate.Value.ToShortDateString()};";
                    }
                    else
                    {
                        accountsToErrorLogs.Add(new ReviewRecord(r2.DefermentOrForbearanceBeginDate, r2.DefermentOrForbearanceEndDate, r2.AccountNumber, r2.LoanSequence));
                    }
                }
            }
            //add a manual review record per account
            foreach(var rec in accountsToErrorLogs)
            {
                string errorLog = $"Date period has partial reduced rate, loan sequences: {rec.GetCommaDelimitedLoanSequences()} begin date: {rec.BeginDate.Value.ToShortDateString()}, end date: {rec.EndDate.Value.ToShortDateString()};";
                new ManualReviewHelper(data, plr).FlagForManualReview(rec.AccountNumber, errorLog);
            }
            //mark records processed
            foreach (var r2 in r2s)
            {
                if (r2.HasPartialReducedRate.HasValue && r2.HasPartialReducedRate.Value == true)
                {
                    data.MarkQueueAsProcessed(r2.ProcessQueueId);
                    result.Remove(r2);
                }
            }
            return result;
        }

        /// <summary>
        /// Takes records and splits the variable interest rate loans returning a new list
        /// </summary>
        private List<QueueRecord> SplitVariableR2(List<QueueRecord> result)
        {
            List<QueueRecord> splitList = new List<QueueRecord>();
            foreach(var rec in result)
            {
                if(rec.VariableRate && rec.Report == "R2")
                {
                    bool done = false;
                    while(!done)
                    {
                        DateTime june30 = GetNextJune30(rec.DefermentOrForbearanceBeginDate.Value);
                        if(rec.DefermentOrForbearanceBeginDate > DateTime.Now)
                        {
                            done = true;
                        }
                        else if(june30 >= rec.DefermentOrForbearanceEndDate)
                        {
                            //Add and break
                            splitList.Add(new QueueRecord(rec.ProcessQueueId, rec.Report, rec.Ssn, rec.AccountNumber, rec.LoanSequence, rec.OwnerCode, rec.DefermentOrForbearanceBeginDate, june30 > rec.DefermentOrForbearanceEndDate ? rec.DefermentOrForbearanceEndDate : june30, rec.VariableRate));
                            done = true;
                        }
                        else
                        {
                            //Add and adjust start date
                            splitList.Add(new QueueRecord(rec.ProcessQueueId, rec.Report, rec.Ssn, rec.AccountNumber, rec.LoanSequence, rec.OwnerCode, rec.DefermentOrForbearanceBeginDate, june30, rec.VariableRate));
                            //Adjust start for next record
                            rec.DefermentOrForbearanceBeginDate = june30.AddDays(1);
                        }
                    }
                }
                else
                {
                    splitList.Add(rec);
                }
            }
            return splitList;
        }

        private DateTime GetNextJune30(DateTime date)
        {
            DateTime june30 = new DateTime(date.Year, 6, 30);
            if (june30 < date)
                june30 = june30.AddYears(1);
            return june30;
        }

        /// <summary>
        /// Remove all R4 records if there is already an associated R3 to process.
        /// </summary>
        private void RemoveInvalidR4s(List<QueueRecord> records)
        {
            var r4s = records.Where(o => o.Report.StartsWith("R4")).ToList();
            foreach (var r4 in r4s)
                if (records.Any(o => o.AccountNumber == r4.AccountNumber && o.Ssn == r4.Ssn && o.Report.StartsWith("R3")))
                {
                    data.MarkQueueAsProcessed(r4.ProcessQueueId);
                    records.Remove(r4);
                }
        }

        readonly string[] nextJune30Codes = new string[] { "SV", "V1", "V2", "C1", "C2", "F2" };
        DateTime nextJune30;
        public void WorkR2(QueueRecord record, bool useOriginalBeginDate = false)
        {
            LoadCts06(record);
            ri.PutText(11, CTS06ColumnData.InterestRateColumn, Math.Max(0, record.NewRate ?? 0).ToString()); //Interest Rate
            ri.PutText(11, CTS06ColumnData.ReductionCodeColumn, "S"); //RDC CDE
            var beginDate = useOriginalBeginDate ? record.DefermentOrForbearanceOriginalBeginDate.Value : record.DefermentOrForbearanceBeginDate.Value;

            if (nextJune30Codes.Contains(record.NewRateType))
            {
                if (beginDate <= nextJune30)
                    ri.PutText(11, CTS06ColumnData.BeginDateColumn, beginDate.ToString("MMddyyyy"));
                else
                    return;
            }
            else
                ri.PutText(11, CTS06ColumnData.BeginDateColumn, record.DefermentOrForbearanceBeginDate.Value.ToString("MMddyyyy"));

            if (nextJune30Codes.Contains(record.NewRateType) && record.DefermentOrForbearanceEndDate.Value > nextJune30)
                ri.PutText(11, CTS06ColumnData.EndDateColumn, nextJune30.ToString("MMddyyyy"));
            else
                ri.PutText(11, CTS06ColumnData.EndDateColumn, record.DefermentOrForbearanceEndDate.Value.ToString("MMddyyyy"));

            //if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live) TODO Commenting out for Jams
            //    Dialog.Info.Ok("Press OK to continue processing.");

            ri.Hit(ReflectionInterface.Key.Enter);

            Tsx07Finish(record);
        }

        public void WorkR3(QueueRecord record)
        {
            LoadCts06(record);
            RemoveCts06MatchingRows(record);
            WorkR2(record, true);
        }

        public void WorkR4(QueueRecord record)
        {
            LoadCts06(record);
            RemoveCts06MatchingRows(record);
            LeaveComment(record);
        }

        private void Tsx07Finish(QueueRecord record)
        {
            if (ri.MessageCode == "04235") //confirm special rates
                ri.Hit(ReflectionInterface.Key.Enter);

            if (ri.MessageCode == "01005" || ri.MessageCode == "06207" || ri.MessageCode == "90007")
                LeaveComment(record);
            else
                ProcessError(record);
        }

        private void LoadCts06(QueueRecord record)
        {
            //Load TSX07
            ri.FastPath("tx3z/cts06" + record.Ssn);
            if (ri.ScreenCode == "TSX05")
                PageHelper.Iterate(ri, (row, args) =>
                {
                    var seq = ri.GetText(row, 47, 3).ToIntNullable();
                    if (seq == record.LoanSequence)
                    {
                        var sel = ri.GetText(row, 3, 2);
                        ri.PutText(21, 18, sel, ReflectionInterface.Key.Enter);
                        args.ContinueIterating = false;
                    }
                });

            //Find NEW INTEREST RATE
            var settings = PageHelper.IterationSettings.Default();
            settings.MinRow = 12;
            settings.MaxRow = 21;
            PageHelper.Iterate(ri, (row, s) =>
            {
                if (!record.NewRate.HasValue)
                {
                    var beginDate = ri.GetText(row, CTS06ColumnData.BeginDateColumn, 10).ToDateNullable();
                    var endDate = ri.GetText(row, CTS06ColumnData.EndDateColumn, 10).ToDateNullable();
                    if (beginDate <= record.DefermentOrForbearanceBeginDate && endDate >= record.DefermentOrForbearanceBeginDate)
                    {
                        var rate = ri.GetText(row, CTS06ColumnData.InterestRateColumn, 6).ToDecimal();
                        var type = ri.GetText(row, CTS06ColumnData.TypeColumn, 2);
                        record.NewRate = rate;
                        if (!ri.CheckForText(row, CTS06ColumnData.ReductionCodeColumn, "S") && !ri.CheckForText(row, CTS06ColumnData.ReductionCodeColumn, "R"))
                            record.NewRate -= GetOwnerCodeDiscount(record.OwnerCode);
                        record.NewRateType = type;
                    }
                }
                else if (string.IsNullOrEmpty(record.NewRateType))
                    record.NewRateType = ri.GetText(row, CTS06ColumnData.TypeColumn, 2);
                else
                    settings.ContinueIterating = false;
            }, settings);
            while (ri.MessageCode != "01152")
                ri.Hit(ReflectionInterface.Key.F7); //tab back to the first page
        }

        private bool Cts06AlreadyExists(QueueRecord record)
        {
            bool foundRecord = false;
            //Load TSX07
            ri.FastPath("tx3z/cts06" + record.Ssn);
            if (ri.ScreenCode == "TSX05")
                PageHelper.Iterate(ri, (row, args) =>
                {
                    var seq = ri.GetText(row, 47, 3).ToIntNullable();
                    if (seq == record.LoanSequence)
                    {
                        var sel = ri.GetText(row, 3, 2);
                        ri.PutText(21, 18, sel, ReflectionInterface.Key.Enter);
                        args.ContinueIterating = false;
                    }
                });

            //Find Record if it exists
            var settings = PageHelper.IterationSettings.Default();
            settings.MinRow = 12;
            settings.MaxRow = 21;
            PageHelper.Iterate(ri, (row, s) =>
            {
                var beginDate = ri.GetText(row, CTS06ColumnData.BeginDateColumn, 10).ToDateNullable();
                var endDate = ri.GetText(row, CTS06ColumnData.EndDateColumn, 10).ToDateNullable();
                if (ri.CheckForText(row, CTS06ColumnData.StatusCodeColumn, "A") && ri.CheckForText(row, CTS06ColumnData.ReductionCodeColumn, "S") && beginDate == record.DefermentOrForbearanceBeginDate && endDate == record.DefermentOrForbearanceEndDate)
                {
                    foundRecord = true;
                    settings.ContinueIterating = false;
                }
                else if(ri.MessageCode == "01152" || ri.MessageCode == "90007")
                {
                    settings.ContinueIterating = false;
                }
            }, settings);
            return foundRecord;
        }

        private void RemoveCts06MatchingRows(QueueRecord record)
        {
            var settings = PageHelper.IterationSettings.Default();
            settings.MinRow = 11;
            settings.MaxRow = 21;
            PageHelper.Iterate(ri, (row, args) =>
            {
                var rdc = ri.GetText(row, CTS06ColumnData.ReductionCodeColumn, 1);
                var status = ri.GetText(row, CTS06ColumnData.StatusCodeColumn, 1);
                var beginDate = ri.GetText(row, CTS06ColumnData.BeginDateColumn, 10).Replace(" ", "/").ToDateNullable();
                if ((rdc == "S" || rdc == "R") && status == "A" && beginDate >= record.DefermentOrForbearanceOriginalBeginDate && beginDate <= record.DefermentOrForbearanceOriginalEndDate)
                {
                    var sel = ri.GetText(row, 2, 2);
                    ri.PutText(22, 17, sel);
                    ri.Hit(ReflectionInterface.Key.F11);
                }
            }, settings);
        }

        private decimal GetOwnerCodeDiscount(string ownerCode)
        {
            switch (ownerCode)
            {
                case "82976901":
                    return 0.25m;
                case "82976902":
                    return 0.33m;
                case "82976903":
                    return 1.00m;
                case "82976904":
                    return 1.50m;
                case "82976905":
                    return 1.75m;
                case "82976906":
                    return 2.00m;
                case "82976907":
                    return 2.5m;
                case "82976908":
                    return 3.00m;
            }
            return 0m;
        }

        private void LeaveComment(QueueRecord record)
        {
            if (!data.BorrowerHasUnprocessedRIRAJ(record.AccountNumber))
            {
                var arc = new ArcData(DataAccessHelper.CurrentRegion);
                arc.AccountNumber = record.AccountNumber;
                arc.Arc = "RIRAJ";
                arc.ScriptId = Program.ScriptId;
                arc.ArcTypeSelected = ArcData.ArcType.Atd22AllLoans;
                arc.ProcessOn = DateTime.Now;
                arc.Comment = "Borrower qualifies for RIR while in Defer-Forb. Adjusting rates.";
                var result = arc.AddArc();
                if (!result.ArcAdded)
                    Error("Unable to leave RIRAJ arc for borrower {0}.  {1}", record.AccountNumber, string.Join(Environment.NewLine, result.Errors));
            }
        }

        private void ProcessError(QueueRecord record)
        {
            Error("Unable to process record {0}.  {1}", record.ProcessQueueId, ri.Message);
        }

        private void Error(string message, params object[] args)
        {
            message = string.Format(message, args);
            Console.WriteLine(message);
            plr.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }
    }
}
