using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace SKPEMLBRW
{
    public class SkipEmailBorrower : ScriptBase
    {
        public static readonly string QueueTask = "KSKEMAIL";
        public ProcessLogRun logRun { get; set; }
        public DataAccess DA { get; set; }

        public enum QueueTaskResult
        {
            OpenQueueTask,
            Success,
            BadQueueComment,
            Failure
        }

        public SkipEmailBorrower(ReflectionInterface ri) : base(ri, "SKPEMLBRW")
        {
            logRun = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false);
            DA = new DataAccess(logRun);
        }

        public override void Main()
        {
            if(Dialog.Info.OkCancel($"This script processes {QueueTask} queue tasks and sends email messages to skip borrowers.  Click cancel to end the script.") == false)
            {
                return;
            }

            int emailCampaignId = DA.GetCampaignId();

            if (emailCampaignId == 0)
            {
                logRun.AddNotification($"unable to get campaign id from table for SKIPEML.html, please contect systems support.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return;
            }

            BorrowerData borrowerData = new BorrowerData();
            var result = QueueTaskFound(borrowerData);
            string reassignmentId = DA.GetReassignmentId();

            //There is an open queue task that needs to be resolved
            if (result == QueueTaskResult.OpenQueueTask)
            {
                return;
            }

            while (result != QueueTaskResult.Failure && result != QueueTaskResult.OpenQueueTask)
            {
                if(result == QueueTaskResult.BadQueueComment)
                {
                    Thread.Sleep(3000);
                    ReassignTask(borrowerData, reassignmentId);
                }
                else
                {
                    int? emailIdentity = DA.InsertToEmailBatch(emailCampaignId, borrowerData);
                    if (!emailIdentity.HasValue || emailIdentity.Value <= 0)
                    {
                        logRun.AddNotification($"Unable to add email for account number: {borrowerData.AccountNumber}, email: {borrowerData.Email}, It was detected that the record may have been a duplicate. Please contect systems support if you believe this to have not added in error.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        Thread.Sleep(3000);
                        ReassignTask(borrowerData, reassignmentId);
                    }
                    else
                    {
                        Thread.Sleep(5000);
                        //email sent successfully, add a comment saying it was added
                        HotkeyTransitionAddCommentInLP50(borrowerData.Ssn, "EM", "03", "KEMAL", "Email information sent to email batch", ScriptId);
                        Thread.Sleep(3000);
                        CompleteOrReassignTask(borrowerData, reassignmentId);
                    }
                }

                //Get next queue task if there is one
                borrowerData = new BorrowerData();
                result = QueueTaskFound(borrowerData);
            }

            Dialog.Info.Ok(result == QueueTaskResult.OpenQueueTask ? "Open Queue Task Found, Ending Processing" : "Processing Complete.");
            return;
        }

        public QueueTaskResult QueueTaskFound(BorrowerData borrowerData)
        {
            string queueTaskText = "";

            RI.FastPath($"LP9AC{QueueTask}");

            if (RI.CheckForText(1,71,"QUEUE TASK"))
            {
                if(!RI.CheckForText(3,24, QueueTask))
                {
                    //another queue's task came back that the user needs to resolve first
                    Dialog.Error.Ok("You have an unresolved task in another queue. Please resolve the task and start the script again.");
                    return QueueTaskResult.OpenQueueTask;
                }
                queueTaskText = RI.GetText(12, 11, 60) + RI.GetText(13, 11, 60) + RI.GetText(14, 11, 60) + RI.GetText(15, 11, 60);
                queueTaskText = queueTaskText.Replace(" ", "");

                //We can get these safely since they are not from the comment
                borrowerData.AccountNumber = RI.GetText(17, 52, 12).Replace(" ", "");
                borrowerData.Ssn = RI.GetText(17, 70, 9);
                borrowerData.BorrowerName = RI.GetText(17, 6, 39).Trim();

                //The task comment is poorly formatted, manually reassign the task to be reviewed
                if (queueTaskText == "" || queueTaskText.Split(':').Length < 2)
                {
                    return QueueTaskResult.BadQueueComment;
                }

                borrowerData.Email = queueTaskText.Split(':')[1].Split(',')[0];
              
                return QueueTaskResult.Success;
            }
            return QueueTaskResult.Failure;
        }

        public void CompleteOrReassignTask(BorrowerData borrowerData, string reassignmentId)
        {
            //RI.FastPath($"LP9AC{QueueTask}");
            //RI.PutText(1, 9, "", ReflectionInterface.Key.EndKey);
            //RI.PutText(1, 2, $"LP9AC{QueueTask}", ReflectionInterface.Key.Enter, true);
            //we're using hotkeys because when we go to close the task the script is not finding the comment as it should
            RI.Hit(ReflectionInterface.Key.F3);
            RI.Hit(ReflectionInterface.Key.F2);

            RI.Hit(ReflectionInterface.Key.F6);
            if(!RI.CheckForText(22,3,"49000 DATA SUCCESSFULLY UPDATED"))
            {
                RI.FastPath($"LP8YCSKP;{QueueTask};;{borrowerData.Ssn};;W");
                //This extra step is not necessary and causes looping on multiple tasks
                //RI.PutText(7, 33, "A", ReflectionInterface.Key.Enter);
                //RI.Hit(ReflectionInterface.Key.F6);
                //RI.FastPath($"LP8YCSKP;{QueueTask};;{borrowerData.Ssn}");
                RI.PutText(7, 33, "A" + reassignmentId, ReflectionInterface.Key.Enter);
                RI.Hit(ReflectionInterface.Key.F6);
                AddCommentInLP50(borrowerData.Ssn, "EM", "35", "SKPEM", "", ScriptId);
            }
        }

        public void ReassignTask(BorrowerData borrowerData, string reassignmentId)
        {
            RI.FastPath($"LP8YCSKP;{QueueTask};;{borrowerData.Ssn};;W");
            //This extra step is not necessary and causes looping on multiple tasks
            //RI.PutText(7, 33, "A", ReflectionInterface.Key.Enter);
            //RI.Hit(ReflectionInterface.Key.F6);
            //RI.FastPath($"LP8YCSKP;{QueueTask};;{borrowerData.Ssn}");
            RI.PutText(7, 33, "A" + reassignmentId, ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F6);
            AddCommentInLP50(borrowerData.Ssn, "EM", "35", "SKPEM", "", ScriptId);
        }

        public bool HotkeyTransitionAddCommentInLP50(string ssn, string activityType, string activityContact, string actionCode, string comment, string scriptId)
        {
            if (ssn.IsNullOrEmpty())
                return false;

            //We are on LP9A, we're using hotkeys because when we go to close the task the script is not finding the comment as it should
            RI.Hit(ReflectionInterface.Key.F2);
            RI.Hit(ReflectionInterface.Key.F7);
            //RI.FastPath("LP50A");

            RI.Hit(ReflectionInterface.Key.EndKey);
            RI.PutText(3, 48, "", true);
            if (ssn.SafeSubString(0, 3).ToUpper().Contains("RF@"))
            {
                RI.PutText(3, 13, "", true);
                RI.PutText(3, 48, ssn);
            }
            else
                RI.PutText(3, 13, ssn);

            RI.PutText(9, 20, actionCode, ReflectionInterface.Key.Enter);

            if (!RI.CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY"))
                return false;

            RI.PutText(7, 2, activityType);
            RI.PutText(7, 5, activityContact);

            if (comment.Length + scriptId.Length < 70)
            {
                comment = string.Format("{0}. {{ {1} }}", comment, scriptId);
                RI.PutText(13, 2, "", ReflectionInterface.Key.EndKey);
                RI.PutText(14, 2, "", ReflectionInterface.Key.EndKey);
                RI.PutText(15, 2, "", ReflectionInterface.Key.EndKey);
                RI.PutText(16, 2, "", ReflectionInterface.Key.EndKey);
                RI.PutText(17, 2, "", ReflectionInterface.Key.EndKey);
                RI.PutText(18, 2, "", ReflectionInterface.Key.EndKey);
                RI.PutText(13, 2, comment, ReflectionInterface.Key.F6);
            }
            else
            {
                if (comment.Length + scriptId.Length > 585)
                    throw new Exception("The requested comment will not fit on LP50");

                RI.PutText(13, 2, comment.SafeSubString(0, 75));

                for (int segmentStart = 75; segmentStart <= comment.Length - 1; segmentStart += 75)
                    RI.EnterText(comment.SafeSubString(segmentStart, 75));

                RI.EnterText(string.Format(" {{ {0} }}", scriptId));

                RI.Hit(ReflectionInterface.Key.F6);
            }

            return RI.CheckForText(22, 3, "48003");
        }
    }
}
