using System.Collections.Generic;
using System;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System.Reflection;
using Uheaa.Common.FileLoad;
using System.Threading.Tasks;
using System.Linq;

namespace COQTSKBLDR
{
    public class CompassQueueTaskBuilder
    {
        public DataAccess DA { get; set; }
        public ProcessLogRun PLR { get; set; }
        public string ScriptId = "COQTSKBLDR";
        public CompassQueueTaskBuilder()
        {
            PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true, true);
            DA = new DataAccess(PLR);
        }

        public int Run(string[] args)
        {
            ProcessFiles(args);

            PLR.LogEnd();
            Console.WriteLine("Closing all database connections and ending application.");
            return 0;
        }

        private void ProcessFiles(string[] args)
        {
            FileLoader.LoadFiles(PLR, ScriptId, DataAccessHelper.CurrentRegion, "fp", "\r\n", false);
            List<FileProcessingRecord> records = DA.GetUnprocessedRecords(ScriptId);
            List<CriticalIfMissing> files = DA.GetRequiredFiles(ScriptId);

            foreach (CriticalIfMissing file in files)
            {
                if (file.LastAdded.Date <= System.DateTime.Today.Date)
                    PLR.AddNotification($"No data was loaded into the ULS.fp.FileProcessing table from the {file.SourceFile} file today.", NotificationType.NoFile, NotificationSeverityType.Informational);
            }

            foreach (FileProcessingRecord row in records)
            {
                List<string> parsedLine = row.LineData.SplitAndRemoveQuotes(",");
                row.ParsedLineData = ParseQueueBuilderRecord(parsedLine);
            }

            Parallel.ForEach(records, new ParallelOptions { MaxDegreeOfParallelism = args.Length > 1 ? args[1].ToInt() : 4 }, record =>
            {
                WorkFile(record);
            });
        }

        private QueueBuilderRecord ParseQueueBuilderRecord(List<string> parsedLine)
        {
            QueueBuilderRecord record = new QueueBuilderRecord();
            record.Ssn = parsedLine[0];
            record.Arc = parsedLine[1];
            record.DateFrom = parsedLine[2].ToDateNullable();
            record.DateTo = parsedLine[3].ToDateNullable();
            record.NeededByDate = parsedLine[4].ToDateNullable();
            record.RecipientId = parsedLine[5];
            record.RegardsToText = parsedLine[6];
            switch (parsedLine[6])
            {
                case "B":
                    record.RegardsTo = RegardsTo.Borrower;
                    break;
                case "E":
                    record.RegardsTo = RegardsTo.Endorser;
                    break;
                case "R":
                    record.RegardsTo = RegardsTo.Reference;
                    break;
                case "S":
                    record.RegardsTo = RegardsTo.Student;
                    break;
                default:
                    record.RegardsTo = RegardsTo.None;
                    break;
            }
            record.RegardsToId = parsedLine[7];
            if (parsedLine[8] != "ALL")
            {
                record.LoanSequences = parsedLine[8].SplitAndRemoveQuotes(",").Select(int.Parse).ToList();
            }
            record.Comment = parsedLine[9].SafeSubString(0, 152);

            return record;
        }

        private void WorkFile(FileProcessingRecord record)
        {
            ArcData arcToAdd = CreateArc(record.ParsedLineData);
            if (!arcToAdd.AddArc().ArcAdded) //adds the arc and checks the result.
                PLR.AddNotification(string.Format("Error adding arc record to database for borrower {1}.  The arc {0} will need to be dropped manually.", record.ParsedLineData.Arc, record.ParsedLineData.Ssn), NotificationType.ErrorReport, NotificationSeverityType.Critical);

            DA.UpdateProcessed(record.FileProcessingId);
        }

        public ArcData CreateArc(QueueBuilderRecord record)
        {
            try
            {
                return new ArcData(DataAccessHelper.CurrentRegion)
                {
                    AccountNumber = record.Ssn,
                    Arc = record.Arc,
                    DelinquencyArc = null,
                    ArcTypeSelected = DetermineArcType(record),
                    Comment = record.Comment,
                    IsEndorser = record.RegardsTo == RegardsTo.Endorser,
                    IsReference = record.RegardsTo == RegardsTo.Reference,
                    LoanPrograms = null,
                    LoanSequences = record.LoanSequences,
                    NeedBy = record.NeededByDate,
                    ProcessFrom = record.DateFrom,
                    ProcessOn = System.DateTime.Now,
                    ProcessTo = record.DateTo,
                    RecipientId = string.IsNullOrWhiteSpace(record.RecipientId) ? null : record.RecipientId,
                    RegardsCode = string.IsNullOrWhiteSpace(record.RegardsToText) ? null : record.RegardsToText,
                    RegardsTo = string.IsNullOrWhiteSpace(record.RegardsToId) ? null : record.RegardsToId,
                    ScriptId = ScriptId,
                    ResponseCode = null
                };
            }
            catch (Exception ex)
            {
                PLR.AddNotification(string.Format("Error creating arc record for borrower {1} to be added to arc add processing.  The arc {0} will need to be dropped manually.", record.Arc, record.Ssn), NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                return null;
            }
        }

        /// <summary>
        /// Identifies the ArcType that should be used for the ARC.
        /// </summary>
        private ArcData.ArcType DetermineArcType(QueueBuilderRecord record)
        {
            if (record.RegardsTo == RegardsTo.Endorser && record.RegardsToText == "E")
                return record.LoanSequences == null ? ArcData.ArcType.Atd22AllLoansRegards : ArcData.ArcType.Atd22ByLoanRegards;
            else
                return record.LoanSequences == null ? ArcData.ArcType.Atd22ByBalance : ArcData.ArcType.Atd22ByLoan;
        }
    }
}
