using System;
using System.Collections.Generic;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace SSCR
{
    public class Program
    {
        public const string ScriptId = "SSCR";
        public static int ERROR = 1;
        public static int SUCCESS = 0;
        public static ReflectionInterface RI { get; set; }
        public static bool Single { get; set; } = false;

        public static int Main(string[] args)
        {
            WriteLine($"SSCR Script: {Assembly.GetExecutingAssembly().GetName().Version}");

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false))
                return ERROR;

            if (args.Length > 1)
            {
                Single = true;
                Dialog.Info.Ok("When using the Single mode, you can click on the console window and hit enter to process the next record.");
            }

            RI = CreateSession();

            if (RI != null)
            {
                try
                {
                    UpdateSchoolStatus();
                    RI.LogRun.LogEnd();
                    RI.CloseSession();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    string message = $"There was an error processing the SSCR script. EX: {ex}";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                    WriteLine(message);
                }
            }
            return ERROR;
        }

        /// <summary>
        /// Create and log into session
        /// </summary>
        private static ReflectionInterface CreateSession()
        {
            ReflectionInterface ri = new ReflectionInterface();
            ri.LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            BatchProcessingLoginHelper.Login(ri.LogRun, ri, ScriptId, "BatchUheaa");
            if (!ri.IsLoggedIn)
            {
                string message = "There were no available batch id's and the session could not log in. Please try again later.";
                ri.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                ri.CloseSession();
                ri.LogRun.LogEnd();
                return null;
            }
            WriteLine($"Logged into session using {ri.UserId}");
            return ri;
        }

        private static void UpdateSchoolStatus()
        {
            List<string> schools = new DataAccess(RI.LogRun.LDA).GetSchools();
            WriteLine($"{schools.Count} schools to process.");
            foreach (string school in schools)
            {
                WriteLine($"Updating SSCR status for School Code: {school}.");
                RI.FastPath($"LPSCC{school};GEN"); //Updated the GEN department updates all other departments as well
                if (RI.AltMessageCode == "47004") //The school code is invalid
                {
                    string message = $"The School Code: {school} is invalid and the SSCR indicator was not updated for this school.";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }
                RI.Hit(F10);
                if (!RI.CheckForText(16, 45, "SSCR")) //Could not get to LPCS screen
                {
                    string message = $"There was an error getting to screen LPCS for School Code: {school}. The SSCR indicator was not updated for this school.";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }
                RI.PutText(16, 61, "N");
                RI.PutText(16, 63, "07012014", Enter, true);
                if (RI.AltMessageCode != "49000") //The SSCR status could not be updated
                {
                    string message = $"There was an error update the SSCR status for school code: {school}. Screen Code: {RI.AltMessage}";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }
                if (Single)
                    ReadKey();
            }
        }
    }
}