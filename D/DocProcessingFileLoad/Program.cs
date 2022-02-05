using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Diagnostics;

namespace DocProcessingFileLoad
{
	class PrintProcessing
    {
		/// <summary>
		/// Executes a stored procedure to pull a list of FileNames objects from the database by scriptid
		/// </summary>
		/// <param name="plData">ProcessLogData Object</param>
		/// <param name="scriptId">string of the scriptId to search the database for</param>
		[UsesSproc(DataAccessHelper.Database.Cls, "[print].GetFileNamesForScript")]
		public static bool LoadPrintFiles(ProcessLogData plData, string scriptId)
		{
				//TODO:add to CLS proj
				List<FileNames> filesToProcess = DataAccessHelper.ExecuteList<FileNames>("[print].GetFileNamesForScript", DataAccessHelper.Database.Cls, new SqlParameter("ScriptId", scriptId));
				//Goes to the ftp folder, grabs all files with the naming convention (deleting old files)
				return PrintProcessingFileLoad(filesToProcess, plData);
		}

		/// <summary>
		/// Takes a list of FileNames objects and sends them to ProcessImportFiles
		/// </summary>
		/// <param name="filesToProcess">List of filename objects to process</param>
		/// <param name="plData">Process Log Data</param>
		private static bool PrintProcessingFileLoad(List<FileNames> filesToProcess, ProcessLogData plData)
		{
			DirectoryInfo path = new DirectoryInfo(EnterpriseFileSystem.FtpFolder);
			//Search the directory for each file
			return ProcessImportFiles(path, filesToProcess, plData);
		}

		/// <summary>
		/// Takes a list of ints representing the valid rfiles and a directory info path.  Checks to see if a file is processed and loads data into the database.
		/// </summary>
		/// <param name="path">DirectoryInfo object to the path where all files to process are located.</param>
		/// <param name="filesToProcess">list of FileNames Objects</param>
		/// <param name="plData">Process Log Data</param>
		private static bool ProcessImportFiles(DirectoryInfo path, List<FileNames> filesToProcess, ProcessLogData plData)
		{
			bool processComplete = true;
			foreach (FileNames file in filesToProcess)
			{
				string filePath = "";
				if (!file.ProcessAllFiles)
				{
					//if we only want the most recent file then delete the rest and use the most current
					filePath = FileSystemHelper.DeleteOldFilesReturnMostCurrent(path.ToString(), file.FileName +".*");
					try
					{
						ProcessFile(plData, file.ScriptFileId, filePath, file.FileName);
					}
					catch(Exception ex) 
					{
 						processComplete = false;
						ProcessLogger.AddNotification(plData.ProcessLogId, string.Format("Failed to import file: {0}", filePath), NotificationType.FileFormatProblem, NotificationSeverityType.Critical);
					}
				}
				else
				{
					//need to get all files in the path that have the filename
					var files = path.GetFiles(file.FileName +".*");
					try
					{
						foreach (FileInfo f in files)
						{
							ProcessFile(plData, file.ScriptFileId, f.FullName, file.FileName);
						}
					}
					catch(Exception ex)
					{
						processComplete = false;
						ProcessLogger.AddNotification(plData.ProcessLogId, string.Format("Failed to import a file in filegroup: {0}", file.FileName + ".*"), NotificationType.FileFormatProblem, NotificationSeverityType.Critical);
					}
				}
			}
			return processComplete;
		}

		/// <summary>
		/// Takes 1 file to process and adds it to the database.
		/// </summary>
		/// <param name="plData">ProcessLogData object</param>
		/// <param name="fileName">filename string</param>
		/// <param name="filePath">Location of the file</param>
		[UsesSproc(DataAccessHelper.Database.Cls, "[print].CheckFileProcessed")]
		[UsesSproc(DataAccessHelper.Database.Cls, "[print].LoadPrintData")]
		private static void ProcessFile(ProcessLogData plData, int scriptFileId, string filePath, string name)
		{
			string fileName = Path.GetFileName(filePath);
			if (filePath.IsNullOrEmpty())
				ProcessLogger.AddNotification(plData.ProcessLogId, string.Format("Missing file {0}", name), NotificationType.NoFile, NotificationSeverityType.Warning);
			else
			{
				//Check database to see if filename has been processed before
				bool processedPrior = DataAccessHelper.ExecuteSingle<int>("[print].CheckFileProcessed", DataAccessHelper.Database.Cls, new SqlParameter("SourceFile", fileName)) > 0;

				if (processedPrior)
					ProcessLogger.AddNotification(plData.ProcessLogId, string.Format("File {0} has already been processed.", filePath), NotificationType.ErrorReport, NotificationSeverityType.Informational);

				//Loads data from the file into [print]._BulkLoad table for files that were found and have not been processed before
				if (filePath.IsPopulated() && !processedPrior)
				{
					BulkLoadImport(filePath);
					DataAccessHelper.Execute("[print].LoadPrintData", DataAccessHelper.Database.Cls, new SqlParameter("ScriptFileId", scriptFileId), new SqlParameter("SourceFile", fileName), new SqlParameter("CreatedBy", Environment.UserName));
				}
			}
		}

		/// <summary>
		/// takes a file path to a single file and loads it's data into the database using the BCP command line tool.
		/// </summary>
		/// <param name="filePath">File to load into the database.  Should be a full path.</param>
		private static void BulkLoadImport(string filePath)
		{
			//determine where to put the data based on the TestMode
			string server = DataAccessHelper.TestMode ? "opsdev" : "nochouse";
			string BCPFinal = string.Format("cls.[Print]._BulkLoad in {0} -S {1} -c -T -F2", filePath, server);
			//TODO: efs key
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.CreateNoWindow = true;
			var exeRun = Process.Start(@"C:\Program Files\Microsoft SQL Server\110\Tools\Binn\bcp.exe", BCPFinal);
			exeRun.WaitForExit();

		}

		private class FileNames
		{
			public bool ProcessAllFiles { get; set; }
			public string FileName { get; set; }
			public int ScriptFileId { get; set; }
		}
    }
}
