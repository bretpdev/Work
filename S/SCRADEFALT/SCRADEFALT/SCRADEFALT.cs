using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace SCRADEFALT
{
    public class SCRADEFALT
    {
        public static ProcessLogRun LogRun { get; set; }
        public static LogDataAccess LDA { get; set; }
        public static string ScriptId { get { return "SCRADEFALT"; } }
        public ReflectionInterface RI { get; set; }
        public string ScriptFile = "UTLWD43.ULWD43R3.*";
        public List<ScraData> borrowerData = new List<ScraData>();

        /// <summary>
        /// Entry method to script
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "SCRA DEFAULT ACCOUNTS", false))
                return 1;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, true, true);
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, LogRun.ProcessLogId, true, true);
            return new SCRADEFALT().Process();
        }

        /// <summary>
        /// Log into a session and read the SCRA sas file.  Add comments to LP50 for each row in the file
        /// </summary>
        /// <returns></returns>
        public int Process()
        {
            if (!LogInToSession())
                return 1; //Failed to log in.  Error logged.

            string fileToProcess = FileSystemHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, ScriptFile);
            if(fileToProcess.IsNullOrEmpty())
            {
                LogRun.AddNotification(string.Format("File {0} not found on ftp location.  Please ensure the file exists and run the script again.", ScriptFile), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return 1; //No files to process.  We are done
            }

            borrowerData = ReadFile(fileToProcess);
            foreach (ScraData borrower in borrowerData)
            {
                if (!RI.AddCommentInLP50(borrower.SSN, "MS", "26", "DACTV", borrower.comment, ScriptId))
                    LogRun.AddNotification(string.Format("Failed to add comment to LP50 for borrower {0}.  Comment: {1} ErrorMessage {2}", borrower.SSN, borrower.comment, RI.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            Repeater.TryRepeatedly(() => File.Copy(fileToProcess, EnterpriseFileSystem.GetPath("Archive", DataAccessHelper.CurrentRegion) + fileToProcess.Substring(fileToProcess.LastIndexOf(@"\") + 1, fileToProcess.Length - fileToProcess.LastIndexOf(@"\") - 1)));
            Repeater.TryRepeatedly(() => File.Delete(fileToProcess));
            RI.CloseSession();
            LogRun.LogEnd();
            return 0;
        }

        /// <summary>
        /// Opens a reflection session and logs in using a batch user.
        /// </summary>
        /// <returns>true if successfully logged in</returns>
        public bool LogInToSession()
        {
            RI = new ReflectionInterface();
            BatchProcessingHelper oneLinkLogin = BatchProcessingHelper.GetNextAvailableId(ScriptId, "BatchUheaa");
            Console.WriteLine(string.Format("Attempting to login to Onelink session using userID {0}", oneLinkLogin.UserName));
            bool loggedIn = RI.Login(oneLinkLogin.UserName, oneLinkLogin.Password, DataAccessHelper.Region.Uheaa);
            if (!loggedIn)
            {
                LogRun.AddNotification(string.Format("Failed to log into session using userId {0}", oneLinkLogin.UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Parse SSN and comment from SAS file.
        /// </summary>
        /// <param name="fileToProcess"></param>
        /// <returns></returns>
        public List<ScraData> ReadFile(string fileToProcess)
        {
            List<ScraData> ScraBorrowers = new List<ScraData>();
            using (StreamReader sr = new StreamReader(fileToProcess))
            {
                if (new FileInfo(fileToProcess).Length == 0)
                {
                    // file is empty
                    LogRun.AddNotification(string.Format("File is Empty: {0}", fileToProcess), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return ScraBorrowers;
                }
                else
                {
                    // there is something in it
                    try
                    {
                        sr.ReadLine(); //Get rid of header
                        while (!sr.EndOfStream)
                        {
                            ScraData data = new ScraData();
                            List<string> line = sr.ReadLine().SplitAndRemoveQuotes(",");
                            if(line.Count < 2)
                            {
                                LogRun.AddNotification(string.Format("Unable to read file. File format is incorrect: {0}", fileToProcess), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                                return ScraBorrowers;
                            }
                            data.SSN = line[0];
                            data.comment = line[1];
                            ScraBorrowers.Add(data);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogRun.AddNotification(string.Format("Unable to read file. File format is incorrect: {0}", fileToProcess), NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                        return ScraBorrowers;
                    }
                }
            }
            return ScraBorrowers;
        }
    }
}
