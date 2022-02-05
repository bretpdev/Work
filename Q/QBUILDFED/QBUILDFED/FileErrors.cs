using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace QBUILDFED
{
    public class FileErrors
    {
        public ProcessLogRun LogRun { get; set; }

        public FileErrors(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        public bool HasErrors { get; set; }

        public void AddError(string fileName, string message)
        {
            HasErrors = true;
            LogRun.AddNotification($"{message} {fileName}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
        }

        /// <summary>
        /// Checks the file to make sure all of the conditions are correct to process it.
        /// </summary>
        public void CheckForFileErrorConditions(List<SasInstructions> sasList)
        {
            foreach (SasInstructions sas in sasList)
            {
                string[] foundFiles = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, sas.FileName);

                if (!sas.MissingFileIsOk && foundFiles.Length == 0)
                    AddError(sas.FileName, "No File Found");
                else
                {
                    if (!sas.ProcessMultipleFiles && foundFiles.Length > 1)
                    {
                        FileSystemHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, sas.FileName);
                        foundFiles = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, sas.FileName);
                    }
                    if (!sas.EmptyFileIsOk)
                    {
                        string firstEmptyFile = foundFiles.FirstOrDefault(p => new FileInfo(p).Length == 0);
                        if (!string.IsNullOrEmpty(firstEmptyFile))
                            AddError(sas.FileName, "Empty File Found");
                    }
                }
            }
        }

        /// <summary>
        /// Checks the file to make sure it is in the correct format.
        /// </summary>
        public bool CheckFileFormat(string file)
        {
            using (StreamReader sr = new StreamR(file))
            {
                if (sr.ReadLine().SplitAndRemoveQuotes(",").Count != 10)
                {
                    AddError(file, "The file is not in the correct format");
                    return false;
                }
            }
            return true;
        }
    }
}