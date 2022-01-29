using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DataWarehouseLoad
{
    class SasProcessor
    {
        private Queue<LocalLoadData> FilesToProcess { get; set; }
        private DataAccessHelper.Database Db { get; set; }

        public SasProcessor()
        {
            Db = DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ? DataAccessHelper.Database.Cdw : DataAccessHelper.Database.Udw;
            FilesToProcess = new Queue<LocalLoadData>(DataAccessHelper.ExecuteList<LocalLoadData>("GetLocalLoadFiles", Db));
        }

        public int StartProcess()
        {
            if (!CheckAllFilesExist())
                return 1;

            if (!ProcessFiles())
                return 1;

            return 0;
        }

        private bool ProcessFiles()
        {
            ReaderWriterLock locker = new ReaderWriterLock();
            List<Task> threads = new List<Task>();
            List<int> returnValues = new List<int>();
            Task thread1 = null;
            Task thread2 = null;
            Task thread3 = null;
            while (FilesToProcess.Count > 0)
            {
                if (thread1 == null || thread1.IsCompleted)
                {
                    LocalLoadData data = GetItem(locker);
                    if (data != null)
                        thread1 = Task.Factory.StartNew(() => ProcessFile(data, locker, returnValues));
                }
                if (thread2 == null || thread2.IsCompleted)
                {

                    LocalLoadData data = GetItem(locker);
                    if (data != null)
                        thread2 = Task.Factory.StartNew(() => ProcessFile(data, locker, returnValues));
                }
                if (thread3 == null || thread3.IsCompleted)
                {
                    LocalLoadData data = GetItem(locker);
                    if (data != null)
                        thread3 = Task.Factory.StartNew(() => ProcessFile(data, locker, returnValues));
                }

                Thread.Sleep(2000);
            }

            //wait for the jobs to finish on the last run
            if (thread1 != null)
                thread1.Wait();
            if (thread2 != null)
                thread2.Wait();
            if (thread3 != null)
                thread3.Wait();

            if (FilesToProcess.Count != 0)
            {
                bool success = ProcessFiles();
                if (!success)
                    returnValues.Add(1);

            }

            return returnValues.Sum() == 0;
        }


        private LocalLoadData GetItem(ReaderWriterLock locker)
        {
            locker.AcquireWriterLock(int.MaxValue);
            LocalLoadData data = null;
            if (FilesToProcess.Any())
                data = FilesToProcess.Dequeue();

            locker.ReleaseWriterLock();
            return data;
        }

        private void ProcessFile(LocalLoadData file, ReaderWriterLock locker, List<int> returnValues)
        {
            string sasPath = Path.Combine(EnterpriseFileSystem.GetPath("SAS_CODE"), file.SasCodeName);
            SasCoordinator.SasRegion region = DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ? SasCoordinator.SasRegion.Legend : SasCoordinator.SasRegion.Duster;
            bool complete = true;
            try
            {
                string dir = Path.Combine(EnterpriseFileSystem.GetPath("SAS_RPT"), string.Format("SAS_{0}{1}", file.LocalLoadFile, file.ReportNumber));
                List<string> files = Directory.GetFiles(dir).ToList();
                foreach (string sasFile in files.OrderBy(p => new FileInfo(p).LastWriteTime).ToList())
                {
                    string movedFile = Path.Combine(EnterpriseFileSystem.GetPath("SAS_RPT"), Path.GetFileName(sasFile));
                    File.Move(Path.Combine(dir, sasFile), movedFile);
                    Console.WriteLine("Starting job {0}", sasPath);
                    SasCoordinator.Coordinator corr = new SasCoordinator.Coordinator("", "", "", sasPath, region, Program.LogData);
                    corr.Coordinate();
                    if (File.Exists(movedFile))
                        Repeater.TryRepeatedly(() => File.Delete(movedFile));
                }

                if(!Directory.GetFiles(dir).Any())
                    Repeater.TryRepeatedly(() => Directory.Delete(dir));

            }
            catch (Exception ex)
            {
                complete = false;
                file.TryCount += 1;
                string message = string.Format("Job {0}/ File {1} was not processed successfully please check process logger and reference Process Log Id {2}. This is attempt number {3} ", file.SasCodeName, file.GetFileName(), Program.LogData.ProcessLogId, file.TryCount);
                Program.WriteToConsole(message);
                ProcessLogger.AddNotification(Program.LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly(), ex);
                if (file.TryCount >= 5)
                {
                    AddReturnVal(locker, returnValues, 1);
                    return;
                }
                else
                {
                    if (Directory.GetFiles(EnterpriseFileSystem.GetPath("SAS_RPT"), file.GetFileName()).ToList().Any())
                    {
                        FilesToProcess.Enqueue(file);
                        AddReturnVal(locker, returnValues, 1);
                    }
                }
            }

            if (complete)
            {
                DataAccessHelper.Execute("UpdateDatawarehouseRun", Db, SqlParams.Single("LocalLoadDataID", file.LocalLoadDataID));
                List<string> files = Directory.GetFiles(EnterpriseFileSystem.GetPath("SAS_RPT"), file.GetFileName()).ToList();
                foreach (string fileToDelete in files)
                {
                    if (File.Exists(fileToDelete))
                        Repeater.TryRepeatedly(() => File.Delete(fileToDelete));
                }

                AddReturnVal(locker, returnValues, 0);
            }
        }

        private static void AddReturnVal(ReaderWriterLock locker, List<int> returnValues, int val)
        {
            locker.AcquireWriterLock(int.MaxValue);
            returnValues.Add(val);
            locker.ReleaseWriterLock();
        }

        private bool CheckAllFilesExist()
        {
            List<string> missingFiles = new List<string>();
            foreach (LocalLoadData file in FilesToProcess)
            {
                string dir = Path.Combine(EnterpriseFileSystem.GetPath("SAS_RPT"), string.Format(@"SAS_{0}{1}\", file.LocalLoadFile, file.ReportNumber));
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                List<string> files = Directory.GetFiles(EnterpriseFileSystem.GetPath("SAS_RPT"), file.GetFileName()).ToList();
                if (!files.Any())
                    missingFiles.Add(file.GetFileName());
                else
                {
                    foreach(string fileToProcess in files)
                    {
                        try
                        {
                            File.Move(fileToProcess, Path.Combine(dir, Path.GetFileName(fileToProcess)));
                        }
                        catch (IOException ex)
                        {
                            if(!ex.Message.Contains("Cannot create a file when that file already exists."))
                            {//If the file is already out there we do not need to remove it.
                                throw ex;
                            }
                            else
                            {
                                File.Delete(fileToProcess);
                            }
                        }
                    }
                }
            }

            if (missingFiles.Any())
            {
                string message = string.Format("The following files were missing from {0} \r\n {1}", EnterpriseFileSystem.GetPath("SAS_RPT"), string.Join(Environment.NewLine, missingFiles));
                Program.WriteToConsole(message);
                EmailHelper.SendMail(DataAccessHelper.TestMode, "SSehlp@utahsbr.edu", Db.ToString() + "@utahsbr.edu", Db.ToString() + " Load files missing", message, string.Empty, string.Empty, EmailHelper.EmailImportance.High, true);
                return false;
            }

            return true;
        }
    }
}
