using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static System.Console;

namespace AESXMTDIAL
{
    public class CreateFiles
    {
        public DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }

        public CreateFiles(ProcessLogRun logRun)
        {
            LogRun = logRun;
            DA = new DataAccess(logRun.LDA);
        }

        public void Process(DateTime createdAt)
        {
            List<FileData> data = DA.GetFileData(createdAt);

            //Create DFT file 
            List<FileData> dftData = data.Where(p => p.QueueRegion == "DFT").ToList();
            WriteLine($"Adding {dftData.Count} DFT records to xmtfile8.xfr file.");
            CreateFile(dftData, "8", createdAt);

            //Create PRE file
            List<FileData> preData = data.Where(p => p.QueueRegion == "PRE").ToList();
            WriteLine($"Adding {preData.Count} PRE records to xmtfile8.xfr file.");
            CreateFile(preData, "12", createdAt);

            //Create SKP file
            List<FileData> skpData = data.Where(p => p.QueueRegion == "SKP").ToList();
            WriteLine($"Adding {skpData.Count} SKP records to xmtfile13.xfr file.");
            CreateFile(skpData, "13", createdAt);
        }

        private void CreateFile(List<FileData> data, string number, DateTime createdAt)
        {
            string fileName = $"{EnterpriseFileSystem.FtpFolder}xmtfile{number}.xfr";
            try
            {
                using StreamWriter sw = new StreamWriter(fileName);
                foreach (FileData d in data)
                {
                    sw.WriteLine($"{d.TargetId,-9}{d.QueueRegion,-3}{d.Queue,-8}{d.BorrowerSSN,-9}{"",-120} UT00204{"",-165}" +
                        $"{d.OnelinkDisposition,-2}{d.PhoneIndicator,-1}{d.PhoneNumber,17}{d.DateDialed,-8}{d.TimeDialed,-4}");
                }
            }
            catch (Exception ex)
            {
                string message = $"There was an error creating or adding records to the file: {fileName}. EX: {ex}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                WriteLine(message);
            }
        }
    }
}