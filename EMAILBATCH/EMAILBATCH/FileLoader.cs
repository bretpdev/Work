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

namespace EMAILBATCH
{
    class FileLoader
    {
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

        public int LoadFiles()
        {
            List<EmailCampaigns> campaignsToLoad = Program.DA.GetAllCampaigns();
            bool result = true;
            foreach (EmailCampaigns campaign in campaignsToLoad)
            {
                bool tempResult = ProcessFiles(campaign);

                if (result && !tempResult)//If one file fails we want it to indicate the job was a failure.
                    result = tempResult;
            }

            return result ? 0 : 1;
        }

        private bool ProcessFiles(EmailCampaigns campaign)
        {
            string filePath = "";
            bool result = true;
            DirectoryInfo path = new DirectoryInfo(EnterpriseFileSystem.FtpFolder);
            if (!campaign.ProcessAllFiles)
            {
                filePath = FileSystemHelper.DeleteOldFilesReturnMostCurrent(path.ToString(), campaign.SourceFile);
                result = ProcessFile(filePath, campaign);
            }
            else
            {
                var files = path.GetFiles(campaign.SourceFile);
                foreach (FileInfo f in files)
                {
                    bool tempResult = ProcessFile(f.FullName, campaign);
                    if (result && !tempResult)//If one fails mark them all to fail.
                        result = tempResult;
                }

                if (!files.Any())
                {
                    if (!campaign.OKIfMissing)
                        Program.PLR.AddNotification(string.Format("Missing file {0}", campaign.SourceFile), NotificationType.NoFile, NotificationSeverityType.Warning);
                }
            }
            return result;
        }

        private bool ProcessFile(string filePath, EmailCampaigns campaign)
        {
            string fileName = Path.GetFileName(filePath);
            if (filePath.IsNullOrEmpty())
            {
                if (!campaign.OKIfMissing)
                    Program.PLR.AddNotification(string.Format("Missing file {0}", campaign.SourceFile), NotificationType.NoFile, NotificationSeverityType.Warning);
            }
            else
            {
                if (new FileInfo(filePath).Length == 0)
                {
                    if (!campaign.OKIfEmpty)
                        Program.PLR.AddNotification(string.Format("File {0} was empty", filePath), NotificationType.EmptyFile, NotificationSeverityType.Informational);
                    Repeater.TryRepeatedly(() => File.Delete(filePath));
                    return true;
                }
                //Check database to see if filename has been processed before
                bool processedPrior = DataAccessHelper.ExecuteSingle<int>(string.Format("[emailbatch].CheckFileProcessed"), DB, SqlParams.Single("SourceFile", fileName)) > 0;

                if (processedPrior)
                    Program.PLR.AddNotification(string.Format("File {0} has already been processed.", filePath), NotificationType.ErrorReport, NotificationSeverityType.Informational);



                //Loads data from the file into [emailbatch]._BulkLoad table for files that were found and have not been processed before
                if (filePath.IsPopulated() && !processedPrior)
                {
                    int lineCount = 0;
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        while (!sr.EndOfStream)
                        {
                            sr.ReadLine();//process the line we read...
                        }

                        //back 2 bytes from end of file
                        sr.BaseStream.Seek(-2, SeekOrigin.End);

                        int s1 = sr.Read(); //read the char before last
                        int s2 = sr.Read(); //read the last char 
                        if (s2 != 10 && s1 != 13)//checking for the last line to be a carrage return  
                            lineCount = 2;
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
                        Console.WriteLine("Loading data into ULS.emailbatch.EmailData");
                        DataAccessHelper.Execute(string.Format("[emailbatch].LoadEmailData"), DB, 300 /*5 minute timeout*/,
                            SqlParams.Single("EmailCampaignId", campaign.EmailCampaignId),
                            SqlParams.Single("SourceFile", fileName),
                            SqlParams.Single("AddedBy", Environment.UserName));
#if !DEBUG
                    Repeater.TryRepeatedly(() => File.Delete(filePath));
#endif
                    }
                    else
                    {
                        Program.PLR.AddNotification(string.Format("File {0} failed to fully load into _BulkLoad.  Please clear out _BulkLoad and try again", filePath), NotificationType.ErrorReport, NotificationSeverityType.Critical);
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
            DataAccessHelper.Execute(string.Format("[emailbatch].Delete_BulkLoad"), DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ? DataAccessHelper.Database.Cls :
                DB);
            string BCPFinal = string.Format(@"uls.[emailbatch]._BulkLoad in ""{0}"" -S {1} -c -T -F{2}", filePath, server, 2);
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            var exeRun = Process.Start(EnterpriseFileSystem.GetPath("BulkLoadBCP"), BCPFinal);
            exeRun.WaitForExit();

        }

        private bool FileImportValidation(string filePath)
        {
            int line = File.ReadAllLines(filePath).Count();
            line = line < 0 ? 0 : (line - 1);//ACCOUNT FOR THE HEADER ROW
            int databaseRecords = DataAccessHelper.ExecuteSingle<int>(string.Format("[emailbatch].GetBulkLoadCount"), DB);
            if (databaseRecords == line)
                return true;
            else
                return false;
        }
    }

}
