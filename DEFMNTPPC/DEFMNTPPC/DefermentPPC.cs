using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;

namespace DEFMNTPPC
{
    class DefermentPPC
    {
        private ReflectionInterface RI { get; set; }
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        private RecoveryLog Recovery { get; set; }

        public DefermentPPC(ReflectionInterface ri, ProcessLogRun logRun, string userId)
        {
            RI = ri;
            LogRun = logRun;
            DA = new DataAccess(logRun);
            Recovery = new RecoveryLog(string.Format("{0}_{1}.txt", Program.ScriptId, userId));
        }

        public int Process()
        {
            if (Recovery.RecoveryValue.IsPopulated())
                RecoveryProcessing();

            List<string> files = GetAllFiles();
            if (files == null)
                return 1;
            foreach (string file in files)
            {
                List<PPCData> data = LoadData(file);
                ProcessList(data, file);
                File.Delete(file);
            }


            Recovery.Delete();
            return 0;

        }

        private void ProcessList(List<PPCData> data, string file)
        {
            foreach (PPCData item in data)
            {
                ProcessRecord(item);
                Recovery.RecoveryValue = string.Format("{0},{1}", file, item.FileLineNumber);
            }
        }

        private void ProcessRecord(PPCData record)
        {
            RI.FastPath(string.Format("TX3Z/LG36A{0};{1}", record.SSN, record.CLUID));
            RI.PutText(4, 24, record.OldBeginDate.ToString("YYYYMMDD"));
            RI.PutText(4, 42, record.NewBeginDate.ToString("YYYYMMDD"));
            RI.PutText(4, 61, record.NewEndDate.ToString("YYYYMMDD"));
            RI.PutText(4, 79, record.DefermentType, ReflectionInterface.Key.Enter);

            if(!RI.CheckForText(1,49, "PAST PERIOD CHANGE SELECT SCREEN"))
            {
                //TODO process log
            }
            else//TODO check spec for LP50 comment even on failure
            {
                //TODO add Arc
            }
        }

        public void RecoveryProcessing()
        {
            List<string> recoveryItems = Recovery.RecoveryValue.SplitAndRemoveQuotes(",");
            List<PPCData> data = LoadData(recoveryItems[0]);
            ProcessList(data.Skip(recoveryItems[1].ToInt()).ToList(), recoveryItems[0]);
            File.Delete(recoveryItems[0]);
        }

        private List<PPCData> LoadData(string file)
        {
            List<PPCData> data = new List<PPCData>();

            using (StreamReader sr = new StreamReader(file))
            {
                int lineCounter = 0;
                while (!sr.EndOfStream)
                {
                    List<string> lineData = sr.ReadLine().SplitAndRemoveQuotes(",");
                    data.Add(new PPCData()
                    {
                        AccountNumber = lineData[0],
                        CLUID = lineData[1],
                        OldBeginDate = lineData[2].ToDate(),
                        NewBeginDate = lineData[3].ToDate(),
                        OldEndDate = lineData[4].ToDate(),
                        NewEndDate = lineData[5].ToDate(),
                        ChangeDate = lineData[6].ToDate(),
                        DefermentType = lineData[7],
                        SSN = DA.GetSsn(lineData[0]),
                        FileLineNumber = lineCounter

                    });
                    lineCounter++;
                }
            }

            return data;
        }



        //public List<PPCData> ReadFiles(List<string> files)
        //{
        //    List<PPCData> data = new List<PPCData>();
        //    foreach (string file in files)
        //    {
        //        using (StreamReader sr = new StreamReader(Path.Combine(EnterpriseFileSystem.FtpFolder, file)))
        //        {
        //            while (sr.EndOfStream)
        //            {
        //                List<string> val = sr.ReadLine().SplitAndRemoveQuotes(",");
        //                data.Add(new PPCData() { AccountNumber = val[0], Name = val[1] });
        //            }
        //        }

        //    }

        //    return data;
        //}

        private List<string> GetAllFiles()
        {
            List<string> files = new List<string>();
            string path = Path.Combine(EnterpriseFileSystem.FtpFolder);
            files = Directory.GetFiles(path).ToList();
            files = files.Where(p => p.Contains("LWO83R2")).ToList();

            //files = Directory.GetFiles(string.Format(@"{0}{1}", EnterpriseFileSystem.FtpFolder, "ULWO83.LWO83R2*")).ToList();
            if (files.Count == 0)
            {
                LogRun.AddNotification(string.Format("Unable to find ULWO83.LWO83R2* in {0}", EnterpriseFileSystem.FtpFolder), NotificationType.NoFile, NotificationSeverityType.Critical);
                return null;
            }
            else
            {
                List<string> dataFiles = new List<string>();
                foreach (string file in files)
                {
                    if (new FileInfo(file).Length == 0)
                    {
                        string message = string.Format("File {0} is empty", file);
                        LogRun.AddNotification(message, NotificationType.EmptyFile, NotificationSeverityType.Warning);
                        Console.WriteLine(message);
                        File.Delete(file);
                    }
                    else
                        dataFiles.Add(file);
                }
                return dataFiles;
            }
        }
    }
}
