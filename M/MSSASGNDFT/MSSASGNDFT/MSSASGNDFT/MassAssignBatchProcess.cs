using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;
using static System.Console;

namespace MassAssignBatch
{
    public class MassAssignBatchProcess
    {
        private List<UserRange> Ranges { get; set; }
        private List<MassAssignRangeAssignment.SqlUser> Users { get; set; }
        public ReflectionInterface RI { get; set; }
        private DataAccess DA { get; set; }
        List<ErrorMessage> EmailMessages { get; set; }
        public string OverrideQueue { get; set; }
        private int SessionCount { get; set; }

        public MassAssignBatchProcess(ProcessLogRun logRun, List<MassAssignRangeAssignment.SqlUser> users, DataAccess da, string overrideQueue = "")
        {
            Repeater.TryRepeatedly(() => RI = GetSession(logRun));
            WriteLine($"Logged into session with User ID: {RI.UserId}");
            RI.LogRun = logRun;
            Users = users;
            OverrideQueue = overrideQueue;
            DA = da;
            EmailMessages = new List<ErrorMessage>();
        }

        public int Process()
        {
            int processed = 0;
            if (RI != null)
            {
                Ranges = DA.GetCurrentRanges().OrderBy(p => p.BeginRange).ToList();
                UpdateUtIds();
                List<QueueNames> queues = DA.GetQueueList().OrderBy(p => p.QueueName).ToList();
                if (OverrideQueue.IsPopulated())
                {
                    queues.Clear();
                    queues.Add(new QueueNames() { QueueName = OverrideQueue });
                }
                foreach (QueueNames queue in queues)
                {
                    int count = DA.GetQueueCount(queue.QueueName, queue.FutureDated);
                    WriteLine($"Queue: {queue.QueueName} has {count} records to process.");
                    if (count > 0)
                        processed = ProcessQueue(queue);
                }
                if (EmailMessages.Count > 0)
                    SendMessage();
            }
            WriteLine("Processing Complete, closing session and log end.");
            RI.CloseSession();
            return processed;
        }

        /// <summary>
        /// Crates a session and logs in with a batch ID.
        /// </summary>
        public ReflectionInterface GetSession(ProcessLogRun logRun)
        {
            ReflectionInterface ri = new ReflectionInterface();
            BatchProcessingLoginHelper.Login(logRun, ri, Program.ScriptId, "BatchUheaa");
            if (ri.CheckForText(1, 39, "UHEAA") && !ri.CheckForText(1, 2, "UNSUPPORTED"))
                return ri;
            else
            {
                SessionCount++;
                if (SessionCount == 5)
                {
                    string message = $"There was an error creating a session or loggin into the session. Please try again";
                    logRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    WriteLine(message);
                }
                throw new Exception();
            }
        }

        /// <summary>
        /// Update the UT ID for each range. If the UT ID is not available, PL and remove the range from the list of ranges.
        /// </summary>
        private void UpdateUtIds()
        {
            List<UserRange> removeRange = new List<UserRange>();
            foreach (UserRange range in Ranges)
            {
                string utId = Users.Where(p => p.ID == range.UserId).FirstOrDefault().AesUserId;
                if (utId.IsNullOrEmpty() || !CheckUserInSession(utId))
                {
                    RI.LogRun.AddNotification($"The UT ID for SqlUserId: {range.UserId} is not available it the CSYS Users table.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    removeRange.Add(range);
                }
                else
                    range.AesId = utId;
            }
            //These ranges need to be removed because we do not know the UT ID to reassign each task
            foreach (UserRange range in removeRange)
                Ranges.Remove(range);
        }

        /// <summary>
        /// Checks to see if the UT ID is valid in the session
        /// </summary>
        public bool CheckUserInSession(string utId)
        {
            RI.FastPath($"LP40I{utId}");
            if (RI.GetText(22, 3, 5) == "48012") //UT ID is not in the system
                return false;
            return true;
        }

        /// <summary>
        /// Opens each queue and updates the EOJ/ERR counts
        /// </summary>
        public int ProcessQueue(QueueNames queue)
        {
            try
            {
                WriteLine($"Reassigning tasks for queue: {queue.QueueName}");
                RI.FastPath("LP8YCDFT;" + queue.QueueName + ";;;;A;;");
                if (!RI.CheckForText(22, 3, "47432", "47004"))
                {
                    RI.FastPath("LP8YCDFT;" + queue.QueueName + ";;;;A;;");
                    AssignQueue(queue);
                }
            }
            catch (Exception ex)
            {
                string message = $"There was an error reassigning all the tasks in the {queue.QueueName} queue. Exception: {ex.Message}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                WriteLine(message);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Sends an email to the manager of the BU with any errors in processing.
        /// </summary>
        private void SendMessage()
        {
            string message = "";
            string que = "";
            foreach (ErrorMessage error in EmailMessages.OrderBy(p => p.Queue))
            {
                if (que != error.Queue)
                {
                    message += $"\r\n\r\n";
                    message += $"Queue: {error.Queue}";
                    que = error.Queue;
                }
                message += $"\r\nAccount: {error.Account}; User ID: {error.UtId};  Error Message: {error.ScreenMessage}";
            }
            EmailHelper.SendMail(DataAccessHelper.TestMode, $"{DA.GetManagerOfBU()}@utahsbr.edu", "MSSASGNDFT@utahsbr.edu", "There was an error re-assigning the following accounts", message, "", EmailHelper.EmailImportance.Normal, false);
        }

        //Clear out all the user ID's first.
        private void ClearUsers(QueueNames queue)
        {
            for (int i = 7; i <= 20; i++)
            {
                //get the date information
                DateTime screenDate = (RI.GetText(i, 26, 2) + "/" + RI.GetText(i, 28, 2) + "/" + RI.GetText(i, 22, 4)).ToDateNullable() ?? DateTime.Now.Date;
                if (RI.GetText(i, 12, 9).IsNullOrEmpty() && RI.GetText(i, 2, 9).IsNullOrEmpty())
                    break;

                bool futureDated = screenDate.Date >= DateTime.Now.Date && queue.FutureDated;
                bool pastDate = screenDate.Date <= DateTime.Now.Date;
                if (RI.GetText(i, 30, 7).IsPopulated() && RI.CheckForText(i, 33, "A") && (futureDated || pastDate))
                    RI.PutText(i, 38, "", true);
            }
        }

        /// <summary>
        /// Opens the queue and determines if the queue was assigned.
        /// </summary>
        private void AssignQueue(QueueNames queue)
        {
            int pageCount = 0;
            while (!RI.AltMessageCode.IsIn("46004", "47011"))
            {
                ClearUsers(queue);
                for (int i = 7; i <= 20; i++)
                {
                    UserRange range = Assign(queue, i);
                    if (RI.GetText(i, 12, 9).IsNullOrEmpty() && RI.GetText(i, 2, 9).IsNullOrEmpty())
                        break;
                    if (range == null)
                        continue;
                }
                RI.Hit(F6);
                if (RI.AltMessageCode == "47431")
                    DetermineInvalidUser(queue, pageCount);
                else if (RI.AltMessageCode != "49000" && RI.AltMessageCode != "49007")
                    DetermineInvalidBorAcct(queue, pageCount);
                pageCount++;
                if (RI.AltMessageCode == "49007")
                    RI.Hit(F8); //Just hit once if it gets this message, no changes were made
                else
                    RI.PutText(2, 71, $"{pageCount + 1}", Enter, true);
            }
        }

        /// <summary>
        /// If there was an 47431 error, it will manually add to this page and log which accounts had an error
        /// </summary>
        private void DetermineInvalidUser(QueueNames queue, int pageCount)
        {
            ClearUsers(queue);
            for (int i = 7; i <= 20; i++)
            {
                UserRange range = Assign(queue, i);
                if (RI.GetText(i, 12, 9).IsNullOrEmpty() && RI.GetText(i, 2, 9).IsNullOrEmpty())
                    break;
                if (range == null)
                    continue;
                RI.Hit(F6);
                if (RI.AltMessageCode == "47431")
                {
                    string message = $"Invalid user id: {range.AesId}; task not assigned for borrower: XXX-XX-{RI.GetText(i, 2, 4)}; in queue: {queue.QueueName}.";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    RI.PutText(i, 38, "", true);
                }
                else
                    RI.PutText(2, 71, $"{pageCount + 1}", Enter, true);
            }
            RI.FastPath("LP8YCDFT;" + queue.QueueName + ";;;;A;;");
        }

        /// <summary>
        /// If it did not post successfully on the current page, this will manually add each line to log the account with the error
        /// </summary>
        private void DetermineInvalidBorAcct(QueueNames queue, int pageCount)
        {
            ClearUsers(queue);
            for (int i = 7; i <= 20; i++)
            {
                UserRange range = Assign(queue, i);
                if (RI.GetText(i, 12, 9).IsNullOrEmpty() && RI.GetText(i, 2, 9).IsNullOrEmpty())
                    break;
                if (range == null)
                    continue;
                RI.Hit(F6);
                if (RI.AltMessageCode != "49000")
                {
                    string account = $"XXX-XX-{RI.GetText(i, 2, 4)}";
                    string message = $"Application can not post changes for Borrower: {account} for queue: {queue.QueueName}. Please fix the problem and post manually";
                    EmailMessages.Add(new ErrorMessage() { Account = account, Queue = queue.QueueName, ScreenMessage = RI.AltMessage, UtId = range.AesId });
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    RI.PutText(i, 38, "", true);
                }
                else
                    RI.PutText(2, 71, $"{pageCount + 1}", Enter, true);
            }
            RI.FastPath("LP8YCDFT;" + queue.QueueName + ";;;;A;;");
        }

        /// <summary>
        /// Assigns the queue according to the last 2 digits of the SSN
        /// </summary>
        /// <returns>The range for the task being re-assigned</returns>
        private UserRange Assign(QueueNames queue, int i)
        {
            UserRange range = new UserRange();
            //get the date information
            if (RI.GetText(i, 12, 9).IsNullOrEmpty() && RI.GetText(i, 2, 9).IsNullOrEmpty())
                return null;
            DateTime screenDate = (RI.GetText(i, 26, 2) + "/" + RI.GetText(i, 28, 2) + "/" + RI.GetText(i, 22, 4)).ToDateNullable() ?? DateTime.Now.Date;
            //Check for active status and that the date is before today. If the date is in the future,
            //it must be a specific future dated queue.
            bool futureDated = screenDate.Date >= DateTime.Now.Date && queue.FutureDated;
            bool pastDate = screenDate.Date <= DateTime.Now.Date;
            //Assign the task if it fits the right criteria
            if (RI.CheckForText(i, 33, "A") && (futureDated || pastDate))
            {
                int num = RI.GetText(i, 9, 2).ToInt();
                range = Ranges.Where(p => num >= p.BeginRange && num <= p.EndRange).FirstOrDefault();
                if (range == null)
                    range = new UserRange() { AesId = DA.GetManagerId() };
                RI.PutText(i, 38, range.AesId);
                return range;
            }
            return null;
        }
    }
}