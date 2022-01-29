using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;
using static Uheaa.Common.Dialog;
using System.Text;

namespace DPAPOST
{
    public class DPAPosting : ScriptBase
    {
        private DataAccess DA { get; set; }
        private string PostingFile { get; set; }

        public DPAPosting(ReflectionInterface ri)
            : base(ri, "DPAPOST")
        {
            RI.LogRun = ri.LogRun ?? new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            PostingFile = EnterpriseFileSystem.GetPath("dpapost");
        }

        public override void Main()
        {
            if (!Info.OkCancel("This is the DPA Posting Script. Press OK to continue.", "DPA Posting"))
                return;
            DA = new DataAccess(RI.LogRun.LDA);
            List<FileData> data = DA.GetRecords();
            if (data.Count > 0)
            {
                bool? shouldProcess = Question.YesNoCancel($"There are {data.Count} records that have not been processed in the amount of {data.Sum(p => p.Amount):C} from {string.Join(" and ", data.Select(p => p.AddedAt.ToShortDateString()).Distinct())}. " +
                    "Do you want process these records before running the new file? Click NO to delete these records and process the new file or Cancel to end the script.", "DPA Posting Script");
                if (!shouldProcess.HasValue) //User chose to cancel
                    return;
                else if (!shouldProcess.Value) //User chose No and wants the records set to deleted
                    DeleteRecords(data);
                else if (shouldProcess.Value)
                    ProcessData(data);
            }
            List<FileData> records = GetRecords();
            if (records.Count > 0)
            {
                bool? isCorrect = Info.YesNoCancel($"There are {records.Count} record(s) in the file in the amount of {records.Sum(p => p.Amount):C}, is this correct?");
                if (isCorrect.HasValue)
                {
                    if (records.Count > 0 && isCorrect.Value)
                    {
                        data = LoadRecords(records);
                        if (data.Count > 0)
                            ProcessData(data);
                    }
                }
            }

            RI.LogRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
        }

        /// <summary>
        /// Deletes all the records in the list of FileData
        /// </summary>
        private void DeleteRecords(List<FileData> data)
        {
            foreach (FileData record in data)
                DA.DeleteRecord(record.PostingDataId);
        }

        /// <summary>
        /// Checks for a file to process. If file exists and has data, it is loaded to the table then pulled back out to process.
        /// </summary>
        private List<FileData> GetRecords()
        {
            List<FileData> records = new List<FileData>();
            string message;
            if (!File.Exists(PostingFile)) //The file was not found
            {
                message = $"The {PostingFile} file is missing. Please verify the file exists and run the script again.";
                RI.LogRun.AddNotification(message, NotificationType.NoFile, NotificationSeverityType.Critical);
                Error.Ok(message);
                return new List<FileData>();
            }
            if (new FileInfo(PostingFile).Length == 0) //There are no records in the file
            {
                message = $"There are no records in the {PostingFile} file to process today. Please review the file if you feel this is in error.";
                RI.LogRun.AddNotification(message, NotificationType.EmptyFile, NotificationSeverityType.Warning);
                Info.Ok(message);
                return new List<FileData>();
            }
            if (new FileInfo(PostingFile).CreationTime < DateTime.Now.AddDays(-4))
            {
                message = $"The file is older than 4 days old and can't be run. Please contact Systems Support for assistance.";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Info.Ok(message);
                return new List<FileData>();
            }
            int count = 0;
            using StreamReader sr = new StreamReader(PostingFile);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line.IsPopulated())
                {
                    List<string> record = line.SplitAndRemoveQuotes(",");
                    if (record[3].ToDouble() == 0)
                        continue; //Header row, do not process
                    records.Add(new FileData() { AccountNumber = $"{new StringBuilder(10).Append("000000000", 0, 10 - record[1].Length)}{record[1]}", Amount = record[3].ToDouble() });
                    count++;
                }
            }
            sr.Close();
            if (count == 0)
            {
                message = $"There are no records in the {PostingFile} file to process today. Please review the file if you feel this is in error.";
                RI.LogRun.AddNotification(message, NotificationType.EmptyFile, NotificationSeverityType.Warning);
                Info.Ok(message);
                return new List<FileData>();
            }
            return records;
        }

        /// <summary>
        /// Loads the records from the file and pulls them back out of the database for processing.
        /// </summary>
        private List<FileData> LoadRecords(List<FileData> records)
        {
            try
            {
                foreach (FileData record in records)
                    DA.InsertRecord(record);
                List<FileData> data = DA.GetRecords();
                if (data.Count > 0)
                    return data;
                else
                {
                    string message = "There was an error adding or getting the data out of the database. Check to make sure the borrowers in your file have not been processed or contact Systems Support for assistance.";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Error.Ok(message);
                    return new List<FileData>();
                }
            }
            catch (Exception ex)
            {
                string message = "There was an error adding or getting the data from the database. Please contact Systems Support for assistance.";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                Error.Ok(message);
                return new List<FileData>();
            }
            finally
            {
#if !DEBUG
            Repeater.TryRepeatedly(() => FS.Delete(PostingFile));
#endif
            }
        }

        /// <summary>
        /// Creates
        /// </summary>
        /// <param name="data"></param>
        private void ProcessData(List<FileData> data)
        {
            if (CreateBatch(data))
            {
                string batchId = GetBatchId();
                if (SelectBatch(batchId))
                    AddDataToBatch(data, batchId);
            }
        }

        /// <summary>
        /// Creates a new batch in LC38
        /// </summary>
        private bool CreateBatch(List<FileData> data)
        {
            RI.FastPath($"LC38A{DateTime.Now.Date:MMddyyyy}");
            RI.PutText(9, 32, $"{data.Count}", true);
            RI.PutText(9, 42, $"{data.Sum(p => p.Amount):#.##}", true);
            RI.PutText(9, 72, "BR", Enter);
            if (RI.AltMessageCode != "49000")
            {
                string message = $"There was an error creating a batch in LC38A with batch count: {data.Count}, batch total: {data.Sum(p => p.Amount)}, batch type: BR; Session Error: {RI.AltMessage}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Error.Ok(message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the newly created batch id
        /// </summary>
        private string GetBatchId()
        {
            while (RI.AltMessageCode != "46004")
                RI.Hit(F8);

            int row = 21;
            while (RI.CheckForText(row, 2, " "))
                row--;

            return RI.GetText(row, 6, 12);
        }

        /// <summary>
        /// Selects the batch so records can be added
        /// </summary>
        private bool SelectBatch(string batchId)
        {
            RI.FastPath($"LC38C{batchId}");
            if (RI.CheckForText(9, 6, batchId))
            {
                RI.PutText(9, 2, "X", Enter);
                if (RI.CheckForText(3, 13, batchId))
                    return true;
            }
            string message = $"There was an error selecting the batch id: {batchId}. Try running the script again or contact System Support for assistance.";
            RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            Error.Ok(message);
            return false;
        }

        /// <summary>
        /// Adds data from the file to the new batch
        /// </summary>
        private void AddDataToBatch(List<FileData> data, string batchId)
        {
            int row = 9;
            foreach (FileData fileData in data)
            {
                RI.PutText(row, 2, fileData.AccountNumber, true);
                RI.PutText(row, 18, fileData.Amount.ToString(), true);
                RI.PutText(row, 34, $"{DateTime.Now.Date:MMddyyyy}", Enter, true);
                row++;
                if (RI.AltMessageCode.IsIn("44068", "49000"))
                {
                    RI.Hit(Enter);
                    DA.SetProcessed(fileData.PostingDataId);
                }
                else //Error if there isn't a success code
                {
                    string message = $"There was an error posting the amount: {fileData.Amount} for borrower: {fileData.AccountNumber}, Session Error: {RI.AltMessage}";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Error.Ok(message);
                    DA.SetErrorPosting(fileData.PostingDataId);
                    row--; //Count back down 1 to use the row with the error.
                }
            }
            RI.Hit(F12);
            RI.FastPath($"LC38C{batchId}");
            RI.PutText(9, 2, "X", F2);

            string finishedMessage;
            if (RI.AltMessageCode == "44034")
                finishedMessage = "The batch entry is complete and batch totals have been verified\r\n\r\n Do you want to reivew the report?";
            else
                finishedMessage = $"Batch entry is complete but the batch totals do NOT match. \r\n\r\nSession Error: {RI.AltMessage}\r\n\r\n Do you want to reivew the error report?";
            if (Info.YesNo(finishedMessage, "Processing Complete"))
                System.Diagnostics.Process.Start(EnterpriseFileSystem.GetPath("dpapostreport", DataAccessHelper.Region.Uheaa));
        }
    }
}