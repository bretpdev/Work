using System;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSQTSKKLLR
{
    public class Program
    {
        public static ReflectionInterface RI { get; set; }
        public static ProcessLogRun PLRC { get; set; }
        public static ProcessLogRun PLRU { get; set; }
        public static RecoveryLog Recovery { get; set; }

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            string scriptId = "CSQTSKKLLR";
            if (!DataAccessHelper.StandardArgsCheck(args, scriptId, false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false) || !CheckStartingLocation())
                return;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            PLRU = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);

            LoginAndProcess(scriptId);
            Dialog.Info.Ok("Processing Complete");
            return;
        }


        private static void LoginAndProcess(string scriptId)
        {
            Recovery = new RecoveryLog(string.Format("{0}Recovery{1}", scriptId, DateTime.Now.Ticks));
            RI = new ReflectionInterface();
            LoginForm loginForm = new LoginForm();
            DialogResult loginResult = DialogResult.Abort;
            while (loginResult != DialogResult.None && loginResult != DialogResult.Cancel)
            {
                loginResult = loginForm.ShowDialog();
                if (loginResult == DialogResult.OK)
                {
                    if (!RI.Login(loginForm.UserName, loginForm.Password))
                    {
                        string message = string.Format("Failed to log in {0} using {1}.  Check that the password is up to date and the user is not locked out, then restart the script.",
                            DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? "Uheaa" : "unknown", loginForm.UserName);
                        LeaveProcessLogMessage(message);
                        return;
                    }
                }

                RunProcessing();
                PLRU.LogEnd();

                Recovery.Delete();
                RI.CloseSession();
                loginResult = DialogResult.None;
            }
        }

        /// <summary>
        /// Leaves a message in process logger using processlogrun based off of the current region
        /// </summary>
        /// <param name="message">Message to leave</param>
        /// <param name="severity">optional parameter setting the severity level. Defaults to critical</param>
        private static void LeaveProcessLogMessage(string message, NotificationSeverityType severity = NotificationSeverityType.Critical)
        {
            PLRU.AddNotification(message, NotificationType.ErrorReport, severity);
        }

        /// <summary>
        /// Force launching the application from ACDC or a local launch
        /// </summary>
        /// <returns></returns>
        private static bool CheckStartingLocation()
        {
            var location = Assembly.GetEntryAssembly().Location;
            if (location.Contains("X:"))
            {
                Dialog.Info.Ok("You are attempting to start this from a network drive. Please launch the application from a local copy or from ACDC");
                return false; //Return false to show it was started from an invalid location
            }
            return true; //Return true to show it was not started from an invalid location
        }

        /// <summary>
        /// Load the file and kill the queues
        /// </summary>
        public static void RunProcessing()
        {
            //Load all the data from the SAS File
            List<QueueData> data = GetFileData();
            //File format was bad or user doesnt have access to the queues
            if (data.Count == 0 || !VerifyUserAccess(data))
                return;

            //Process the data
            ProcessKill(data);
        }

        /// <summary>
        /// Checks the users access to the Queue/SubQueue and if they has Supervisor access
        /// </summary>
        /// <returns>false if the user cant process the queue</returns>
        public static bool VerifyUserAccess(List<QueueData> data)
        {
            string message = "";
            RI.FastPath("PROF");
            string userId = RI.GetText(2, 49, 7);

            foreach (string queue in data.Select(p => p.Queue).Distinct())
            {
                foreach (string subQueue in data.Where(p => p.Queue == queue).Select(r => r.SubQueue).Distinct())
                {
                    RI.FastPath("TX3ZITX64");
                    RI.PutText(8, 46, queue);
                    RI.PutText(10, 46, subQueue);
                    RI.PutText(12, 46, userId, Key.Enter);
                    if (!RI.CheckForText(10, 33, "W", "S"))
                    {
                        message = "You do not have access to the " + queue + " queue and the " + subQueue + " subqueue.";
                        Dialog.Warning.Ok(message, "No Access");
                        LeaveProcessLogMessage(message);
                        return false;
                    }
                }
            }

            string[] departments = { "LSV", "CSV", "DMN", "DOC", "ASV", "AUX", "BSV", "LOR", "OPA", "SKP" };
            foreach (string str in departments)
            {
                RI.FastPath("TX3ZITX41");
                RI.PutText(8, 36, str);
                RI.PutText(10, 36, userId, Key.Enter);
                if (RI.CheckForText(6, 35, userId))
                    return true;
            }
            message = "You are not set up as a supervisor.";
            Dialog.Warning.Ok(message, "No Access");
            LeaveProcessLogMessage(message);
            return false;
        }

        /// <summary>
        /// Opens the file with a Stream Reader and reads the data in one line at a time
        /// </summary>
        /// <returns>list of QueueData objects</returns>
        public static List<QueueData> GetFileData()
        {
            List<QueueData> fileData = new List<QueueData>();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = EnterpriseFileSystem.FtpFolder;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(dialog.FileName))
                {
                    //Get rid of the header row
                    string header = sr.ReadLine();
                    //Check the header row to verify the file is the correct one
                    if (!header.Contains("WF_QUE") || !header.Contains("WF_SUB_QUE") || !header.Contains("WN_CTL_TSK") || !header.Contains("PF_REQ_ACT"))
                    {
                        string message = "You have chosen an invalid file. Please restart the script and choose the correct file.";
                        Dialog.Warning.Ok(message, "Invalid File Format");
                        LeaveProcessLogMessage(message);
                        return fileData;
                    }
                    while (!sr.EndOfStream)
                    {
                        List<string> line = sr.ReadLine().SplitAndRemoveQuotes(",");
                        fileData.Add(new QueueData()
                        {
                            Queue = line[0],
                            SubQueue = line[1],
                            TaskControlNumber = line[2],
                            ActionRequest = line[3]
                        });
                    }
                }
            }
            return fileData;
        }

        /// <summary>
        /// Cancels all the queue tasks in the data file
        /// </summary>
        /// <param name="data">List of QueueData objects</param>
        public static void ProcessKill(List<QueueData> data)
        {
            int recoveryCounter = string.IsNullOrEmpty(Recovery.RecoveryValue) ? 0 : int.Parse(Recovery.RecoveryValue);
            int counter = recoveryCounter;
            int successCount = 0;

            foreach (QueueData queue in data.Skip(recoveryCounter))
            {
                RI.FastPath("TX3ZCTX6Q");
                RI.PutText(8, 41, queue.Queue);
                RI.PutText(10, 41, queue.SubQueue);
                RI.PutText(12, 41, queue.TaskControlNumber);
                RI.PutText(14, 41, queue.ActionRequest, Key.Enter);
                if (RI.MessageCode == "01648" || RI.MessageCode == "01019")
                {
                    //The task is already closed or cancelled
                    string message = string.Format("The {0} {1} queue with control number {2} and action request {3} was already closed or canceled.", queue.Queue, queue.SubQueue, queue.TaskControlNumber, queue.ActionRequest);
                    LeaveProcessLogMessage(message, NotificationSeverityType.Warning);
                }
                else if (!CancelTask())
                {
                    string message = string.Format("The {0} {1} queue with control number {2} and action request {3} failed to close.", queue.Queue, queue.SubQueue, queue.TaskControlNumber, queue.ActionRequest);
                    LeaveProcessLogMessage(message);
                }
                else
                {
                    successCount++;
                }

                Recovery.RecoveryValue = counter++.ToString();
            }

            LeaveProcessLogMessage(string.Format("Number of queues successfully closed: {0}", successCount), NotificationSeverityType.Informational);
        }

        /// <summary>
        /// Cancels the Queue Task
        /// </summary>
        /// <returns>True if canceled, false if not canceled</returns>
        public static bool CancelTask()
        {
            RI.PutText(8, 19, "X", Key.Enter);
            if (RI.MessageCode == "01005")
                return true;
            else
            {
                RI.PutText(9, 19, "CANCL", Key.Enter);
                if (RI.MessageCode == "01005")
                    return true;
                else
                    return false;
            }
        }
    }
}
