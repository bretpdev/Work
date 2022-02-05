using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace UHECORPRT
{
    class FileLoader
    {
        private DataAccess DA { get; set; }
        private DataAccessHelper.Database DB
        {
            get
            {
                return DataAccessHelper.Database.Uls;
            }
        }

        public FileLoader()
        {
        }

        public int RunFileLoader(DataAccess da)
        {
            DA = da;
            List<ScriptData> scriptsToLoad = DA.GetScriptsToLoad();
            bool result = true;
            foreach (ScriptData script in scriptsToLoad)
            {
                bool tempResult = ProcessFiles(script);

                if (result && !tempResult)//If one file fails we want it to indicate the job was a failure.
                    result = tempResult;
            }

            return result ? 0 : 1;
        }

        private bool ProcessFiles(ScriptData scripInfo)
        {
            string filePath = "";
            bool result = true;
            DirectoryInfo path = new DirectoryInfo(EnterpriseFileSystem.FtpFolder);
            if (!scripInfo.ProcessAllFiles)
            {
                filePath = FileSystemHelper.DeleteOldFilesReturnMostCurrent(path.ToString(), scripInfo.SourceFile);
                result = ProcessFile(filePath, scripInfo);
            }
            else
            {
                var files = path.GetFiles(scripInfo.SourceFile);
                foreach (FileInfo f in files)
                {
                    bool tempResult = ProcessFile(f.FullName, scripInfo);
                    if (result && !tempResult)//If one fails mark them all to fail.
                        result = tempResult;
                }
            }
            return result;
        }

        private bool ProcessFile(string filePath, ScriptData scripInfo)
        {
            string fileName = Path.GetFileName(filePath);
            if (filePath.IsNullOrEmpty())
                ProcessLogger.AddNotification(Program.PL.ProcessLogId, string.Format("Missing file {0}", scripInfo.SourceFile), NotificationType.NoFile, NotificationSeverityType.Critical);
            else
            {
                //Check database to see if filename has been processed before
                bool processedPrior = DataAccessHelper.ExecuteSingle<int>(string.Format("[print].CheckFileProcessed"), DB, SqlParams.Single("SourceFile", fileName)) > 0;

                if (processedPrior)
                    ProcessLogger.AddNotification(Program.PL.ProcessLogId, string.Format("File {0} has already been processed.", filePath), NotificationType.ErrorReport, NotificationSeverityType.Informational);

                if (new FileInfo(filePath).Length == 0)
                {
                    ProcessLogger.AddNotification(Program.PL.ProcessLogId, string.Format("File {0} was empty", filePath), NotificationType.EmptyFile, NotificationSeverityType.Informational);
                    Repeater.TryRepeatedly(() => File.Delete(filePath));
                    return true;
                }

                //Loads data from the file into [print]._BulkLoad table for files that were found and have not been processed before
                if (filePath.IsPopulated() && !processedPrior)
                {
                    int lineCount = 0;
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        while (!sr.EndOfStream)
                        {
                            if (lineCount > 2)
                                break;
                            sr.ReadLine();
                            lineCount++;
                        }
                    }
                    if (lineCount == 2)
                    {
                        using (StreamWriter sw = new StreamWriter(filePath, true))
                        {
                            sw.Write(Environment.NewLine);
                        }
                    }
                    BulkLoadImport(filePath);
                    if (FileImportValidation(filePath))
                    {
                        Console.WriteLine("Loading data into ULS.print.PrintProcessing");
                        DataAccessHelper.Execute(string.Format("[print].LoadPrintData"), DB, 300 /*5 minute timeout*/,
                            SqlParams.Single("ScriptDataId", scripInfo.ScriptDataId),
                            SqlParams.Single("SourceFile", fileName),
                            SqlParams.Single("AddedBy", Environment.UserName));
#if !DEBUG
                    Repeater.TryRepeatedly(() => File.Delete(filePath));
#endif
                    }
                    else
                    {
                        ProcessLogger.AddNotification(Program.PL.ProcessLogId, string.Format("File {0} failed to fully load into _BulkLoad.  Please clear out _BulkLoad and try again", filePath), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        return false;
                    }
                }
            }

            return true;
        }

        private void BulkLoadImport(string filePath)
        {
            //determine where to put the data based on the TestMode
            string server = DataAccessHelper.TestMode ? "opsdev" : "uheaasqldb";
            DataAccessHelper.Execute(string.Format("[print].Delete_BulkLoad"), DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ? DataAccessHelper.Database.Cls :
                DB);
            string BCPFinal = string.Format("uls.[print]._BulkLoad in {0} -S {1} -c -T -F{2}", filePath, server, 2);
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            var exeRun = Process.Start(EnterpriseFileSystem.GetPath("BulkLoadBCP"), BCPFinal);
            exeRun.WaitForExit();

        }

        private bool FileImportValidation(string filePath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe", string.Format("/C findstr /R /N \"^\" {0} | find /C \":\"", filePath));
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            var exeRun = Process.Start(startInfo);
            exeRun.WaitForExit();
            int line = exeRun.StandardOutput.ReadLine().ToInt();
            Console.WriteLine(string.Format("{0} Lines Found in {1}", line, filePath));
            line = line < 0 ? 0 : (line - 1);//ACCOUNT FOR THE HEADER ROW
            int databaseRecords = DataAccessHelper.ExecuteSingle<int>(string.Format("[print].GetBulkLoadCount"), DB);
            if (databaseRecords == line)
                return true;
            else
                return false;
        }
    }
}
