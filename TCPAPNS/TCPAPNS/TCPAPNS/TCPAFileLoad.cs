using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static System.Console;

namespace TCPAPNS
{
    public class TCPAFileLoad
    {
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        private bool OneLink { get; set; }

        public string RowTerminator
        {
            get { return "-r\r\n"; }
            set { RowTerminator = value; }
        }

        private readonly string[] CompassFiles = new string[] { "ULWS62.LWS62R2" };
        private readonly string[] OneLinkFiles = new string[] { "ULWS62.LWS62R3", "ULWS62.LWS62R4" };

        public TCPAFileLoad(ProcessLogRun logRun, bool oneLink)
        {
            LogRun = logRun;
            DA = new DataAccess(logRun.LDA, oneLink);
            OneLink = oneLink;
        }

        public bool LoadFiles()
        {
            string filePattern = "flag_*";
            string path = EnterpriseFileSystem.FtpFolder;

            List<string> filesToProcess = new List<string>();
            if (OneLink)
                foreach (string str in OneLinkFiles)
                    filesToProcess.AddRange(Directory.GetFiles(path, filePattern + str + "*"));
            else
                foreach (string str in CompassFiles)
                    filesToProcess.AddRange(Directory.GetFiles(path, filePattern + str + "*"));

            if (filesToProcess.Count == 0)
            {
                string message = "No files in FTP matching pattern flag_*";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                WriteLine(message);
                return true;
            }

            HandleFiles(path, filesToProcess);
            return true;
        }

        private void HandleFiles(string path, List<string> files)
        {
            foreach (string file in files)
            {
                string filePath = FileSystemHelper.DeleteOldFilesReturnMostCurrent(path, Path.GetFileName(file));
                try
                {
                    if (new FileInfo(filePath).Length == 0)
                    {
                        LogRun.AddNotification($"File was empty. Deleting file: {filePath}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        Repeater.TryRepeatedly(() => FS.Delete(filePath));
                    }
                    else
                    {
                        WriteLine($"Importing data from file: {filePath}");
                        BulkLoadImport(filePath);
                        if (ValidateFileImport(filePath))
                        {
                            DA.LoadData(Path.GetFileName(filePath));
                            DA.InactivateInvalidRecords();
                            Repeater.TryRepeatedly(() => FS.Delete(filePath));
                        }
                        else
                        {
                            string message = $"Failed to import file, line count in file did not match line count loaded to database: {filePath}";
                            LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                            WriteLine(message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = $"Failed to import file: {filePath}: Received Exception {ex.Message}";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    WriteLine(message);
                }
            }
        }

        /// <summary> 
        /// takes a file path to a single file and loads it's data into the database using the BCP command line tool. 
        /// </summary> 
        /// <param name="filePath">File to load into the database.  Should be a full path.</param> 
        /// <param name="schemaName">Schema to load the file into. I.E. billing loads into the the [print] schema</param>  
        private void BulkLoadImport(string filePath)
        {
            //determine where to put the data based on the TestMode 
            string server = DataAccessHelper.TestMode ? "opsdev" : "uheaasqldb";

            DA.ClearBulkLoad();

            string BCPFinal;
            if (OneLink)
                BCPFinal = $"uls.[{Program.ScriptId}]._OneLinkBulkLoad in {filePath} -S {server} -c -T -F2 {RowTerminator}";
            else
                BCPFinal = $"uls.[{Program.ScriptId}]._BulkLoad in {filePath} -S {server} -c -T -F2 {RowTerminator}";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            var exeRun = Proc.Start("BulkLoadBCP", BCPFinal);
            exeRun.WaitForExit();
        }

        private bool ValidateFileImport(string filePath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe", $"/C findstr /R /N \"^\" {filePath} | find /C \":\"");
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            var exeRun = Process.Start(startInfo);
            exeRun.WaitForExit();
            int line = exeRun.StandardOutput.ReadLine().ToInt();

#if DEBUG
            WriteLine($"{line} Lines Found in {filePath}");
#endif

            //Need to remove the header row to compare the counts
            line--;
            if (line < 0)
                line = 0;

            int databaseRecords = DA.GetBulkLoadCount();
            return (databaseRecords == line);
        }

    }
}
