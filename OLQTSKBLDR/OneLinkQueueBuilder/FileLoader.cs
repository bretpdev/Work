using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace OLQBuilder
{
    class FileLoader
    {
        private DataAccess DA { get; set; }
		private ProcessLogRun LogRun { get; set; }
		private string FtpFolder { get; set; } = EnterpriseFileSystem.FtpFolder;

		public FileLoader(DataAccess DA, ProcessLogRun logRun)
        {
            this.DA = DA;
			this.LogRun = logRun;
        }

        public void LoadFiles()
        {
            List<SasInstructions> sasList = DA.GetSasList();
			var filesToProcess = CheckForFileErrorConditions(sasList);
			foreach(string file in filesToProcess)
            {
				InsertFileRecordsToDatabase(file);
            }
		}

		private List<string> CheckForFileErrorConditions(List<SasInstructions> sasList)
		{
			List<string> fileList = new List<string>();
			foreach (SasInstructions sasItem in sasList)
			{
				//See whether any files exist.
				if (Directory.GetFiles(FtpFolder, sasItem.Filename + "*").Length == 0)
				{
					//Add to the error report if needed.
					if (!sasItem.IgnoreMissingFile) 
					{ 
						AddQueueBuilderError(sasItem.Filename, "No File Found"); 
					}
				}
				else
				{
					//Delete old files if only one file should be processed.
					if (!sasItem.ProcessMultipleFiles) 
					{ 
						string result = FileSystemHelper.DeleteOldFilesReturnMostCurrent(FtpFolder, sasItem.Filename + "*"); 
						if(!result.IsNullOrEmpty())
                        {
							fileList.Add(result);
							continue;
                        }
					}

					//Check for empty files if required, deleting any empty ones found.
					if (sasItem.IgnoreEmptyFiles)
					{
						bool foundEmptyFile = false;
						foreach (string foundFile in Directory.GetFiles(FtpFolder, sasItem.Filename + "*"))
						{
							if (new FileInfo(foundFile).Length == 0)
							{
								File.Delete(foundFile);
								foundEmptyFile = true;
							}
							else
                            {
								fileList.Add(foundFile);
							}
						}
						if (foundEmptyFile) 
						{ 
							AddQueueBuilderError(sasItem.Filename, "Empty File Found"); 
						}
					}
				}
			}
			return fileList;
		}

		//Add an error record to the QUEUE_BUILDER_ERROR_FILE, which will be printed at the end of the script.
		private void AddQueueBuilderError(string fileName, string errorDescription)
		{
			LogRun.AddNotification($"ERROR File: {fileName} Internal Error: {errorDescription}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
		}


		private void InsertFileRecordsToDatabase(string fileName)
		{
			if (new FileInfo(fileName).Length == 0)
			{
				LogRun.AddNotification(string.Format("Empty File: {0}", fileName), NotificationType.EmptyFile, NotificationSeverityType.Informational);
			}
			else
			{
				List<QueueTableRecord> records = new List<QueueTableRecord>();

				using (StreamReader sr = new StreamR(fileName))
				{
					while (!sr.EndOfStream)
					{
						QueueTableRecord record = new QueueTableRecord();
						List<string> line = sr.ReadLine().SplitAndRemoveQuotes(",");
						record.TargetId = line[0];
						record.QueueName = line[1];
						record.InstitutionId = line[2];
						record.InstitutionType = line[3];
						record.DateDue = line[4].ToDateNullable();
						record.TimeDue = line[5].ToDateNullable()?.TimeOfDay;
						record.Comment = line[6];
						records.Add(record);
					}
				}

				bool success = DA.InsertToQueues(records, Path.GetFileName(fileName));
				if(!success)
                {
					LogRun.AddNotification($"Failed to insert file records to Queues table Filename: {fileName}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
					return;
                }
			}
			FS.Delete(fileName);
		}
	}
}
