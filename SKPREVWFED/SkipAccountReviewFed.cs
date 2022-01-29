using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace SKPREVWFED
{
    public class SkipAccountReviewFed : FedBatchScript
    {
        private const string EojTotalTasks = "Total number of JN/01 tasks initially available";
        private const string EojTotalTaskCompleted = "Total number of JN/01 tasks completed";
        private const string EojNumberOfMrTasks = "Total number of MR/01 tasks created";
        private const string EojErrors = "Total number of errors received";

        public SkipAccountReviewFed(ReflectionInterface ri)
            : base(ri, "SKPREVWFED", "ERR_BU35", "EOJ_BU10", new List<string> { EojTotalTasks, EojTotalTaskCompleted, EojNumberOfMrTasks, EojErrors })
        {
        }

        public override void Main()
        {
            while (CheckForQueueTasks())
            {
                ScriptData data = SelectTask();

                if (HasHighBalance())
                    CreateMRTask(data);

                CloseTask(data);
            }
        }

        /// <summary>
        /// Closes out the current task.
        /// </summary>
        /// <param name="data">Borrower data for the current task.</param>
        private void CloseTask(ScriptData data)
        {
            FastPath("TX3Z/ITX6XJN01");
            PutText(21, 18, "01");
            Hit(ReflectionInterface.Key.F2);
            PutText(8, 19, "C");
            PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);

            if (RI.MessageCode == "01644")
            {
                PutText(6, 19, "", ReflectionInterface.Key.Enter, true);
                if (RI.MessageCode != "01005")
                    ReassignTask(data);
            }

            Eoj.Counts[EojTotalTaskCompleted].Increment();
        }

        /// <summary>
        /// Checks to see if the borrower has a balance >= 30,000.
        /// </summary>
        /// <returns>Ture if the balance is >= 30,000.</returns>
        private bool HasHighBalance()
        {
            FastPath("TX3Z/ITS24");
            string balance = GetText(19, 41, 12).Replace(",", "");
            if (balance.ToDouble() >= 30000.00)
                return true;

            return false;
        }

        /// <summary>
        /// Reassigns the task to the manager of delinquency management.
        /// </summary>
        /// <param name="data">Borrower data for the current task.</param>
        [UsesSproc(DataAccessHelper.Database.Csys, "spGetManagersUtId")]
        private void ReassignTask(ScriptData data)
        {
            FastPath("TX3Z/CTX6J");
            PutText(7, 42, "JN");
            PutText(8, 42, "01");
            PutText(9, 42, "", true);
            PutText(13, 42, UserId, ReflectionInterface.Key.Enter);

            string userId = DataAccessHelper.ExecuteSingle<string>("spGetManagersUtId", DataAccessHelper.Database.Csys, "10".ToSqlParameter("BusinessUnitId"));
            PutText(8, 15, userId, ReflectionInterface.Key.Enter);

            if (RI.MessageCode != "01005")
            {
                string message = string.Format("Unable to reassign task for Borrower: {0} Recipient: {1} Relationship: {2}", data.BorrowersSsn, data.RecipientSsn, data.RelationshipCode);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                EndDllScript();
            }
        }

        /// <summary>
        /// Create a MR queue task for the current borrowers.
        /// </summary>
        /// <param name="data">Borrower data for the current task.</param>
        private void CreateMRTask(ScriptData data)
        {
            string arc = data.RelationshipCode == "B" ? "SKARB" : "SKARE";
            if (!Atd22ByBalance(data.BorrowersSsn, arc, string.Empty, data.RecipientSsn, ScriptId, false))
            {
                Eoj[EojErrors].Increment();
                ReassignTask(data);
            }
            else
                Eoj.Counts[EojNumberOfMrTasks].Increment();
        }

        /// <summary>
        /// Checks to see if there are any queue tasks to process.
        /// </summary>
        /// <returns>True if queue tasks are available to process.</returns>
        private bool CheckForQueueTasks()
        {
            FastPath("TX3Z/ITX6XJN01");

            if (RI.ScreenCode == "TXX6Y")
                ProcessingComplete();
            if (!CheckForText(8, 75, "W"))
                Eoj[EojTotalTasks].Increment();

            return true;
        }

        /// <summary>
        /// Selects the next task.
        /// </summary>
        /// <returns>Borrowers information for the selected task.</returns>
        private ScriptData SelectTask()
        {
            //Select the first task.
            PutText(21, 18, "01", ReflectionInterface.Key.Enter, true);

            if (RI.ScreenCode == "TXX71")
            {
                string message = string.Format("Unable to select task, {0}", RI.Message);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                EndDllScript();
            }

            Func<int, int, int, string> GetTextRemoveSpace = (row, col, length) => GetText(row, col, length).Replace(" ", "");

            return new ScriptData()
            {
                BorrowersSsn = GetTextRemoveSpace(4, 16, 11),
                RecipientSsn = GetTextRemoveSpace(5, 16, 11),
                RelationshipCode = GetText(6, 16, 1)
            };
        }
    }
}
