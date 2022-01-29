using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace PRISON
{
    public class Prison : ScriptBase
    {
        public Prison(ReflectionInterface ri)
            : base(ri, "PRISON")
        {
        }

        public override void Main()
        {
            // warn the user of the purpose of the script
            string startupMessage = "This script will complete tasks in the DPRISON queue. Click OK to proceed or Cancel to quit.";
            if (!Dialog.Info.OkCancel(startupMessage, ScriptId))
                return;

            // access DPRISON queue
            RI.FastPath("LP9ACDPRISON");

            // warn the user and end the script if no more tasks are found
            if (RI.CheckForText(22, 3, "47423", "47420"))
            {
                string message = "There are no more tasks in the DPRISON queue.  Processing is Complete.";
                Dialog.Info.Ok(message, ScriptId);
                EndDllScript();
            }
            // warn the user and end the script if a task from a queue other than DPRISON is displayed
            if (!RI.CheckForText(1, 9, "DPRISON"))
            {
                string message = string.Format("You have an unresolved task in the {0} queue. You must complete the task before working the DPRISON queue.", GetText(1, 9, 8));
                Dialog.Error.Ok(message, ScriptId);
                EndDllScript();
            }
            // Work the queue 
            while (!RI.CheckForText(22, 3, "46004"))
            {
                string ssn = GetText(17, 70, 9);
                if (ssn.Length != 9 || ssn.ToIntNullable() == null)
                {
                    Dialog.Error.Ok("Expected an SSN at 17, 70, but instead found: " + ssn);
                    EndDllScript();
                }
                if (CheckForActiveGarnishment(ssn))
                {
                    string activityComment = "account reviewed for potential incarcerated borrower deferment, task created in LEMPRISN to review AWG status";
                    CreateQueueTask(ssn, "LEMPRISN", "", activityComment, "40");
                }
                else
                {
                    RI.FastPath("LP22I" + ssn);
                    string message = "Click OK to complete the task if LP22 has been updated; otherwise, click Cancel to return the task to a skip tracer.";
                    if (Dialog.Info.OkCancel(message, ScriptId))
                    {
                        EnterDeferment(ssn);
                    }
                    else
                    {
                        string queueComment = "incarcerated borrower information not complete on LP22, complete and create new task in SKIPPRSN for review";
                        string activityComment = "incarcerated borrower information not updated on LP22, task created in SKIPPRSN for skip tracer to update LP22";
                        CreateQueueTask(ssn, "SKIPPRSN", queueComment, activityComment, "10");
                    }
                }
                // Close this task and get the next one
                RI.FastPath("LP9ACDPRISON");
                RI.Hit(Key.F6);
                RI.Hit(Key.F8);
            }
            Dialog.Info.Ok("Processing Complete", ScriptId);
        }

        // check for an open GG record
        private bool CheckForActiveGarnishment(string ssn)
        {
            RI.FastPath("LC67I" + ssn + "GG");
            if (RI.CheckForText(21, 3, "SEL"))
                RI.PutText(21, 13, "01", Key.Enter);
            if (RI.CheckForText(1, 70, "AWG"))
            {
                while (!RI.CheckForText(22, 3, "46004"))
                {
                    //Check the reason withdrawn and inactive reason.
                    if (RI.CheckForText(8, 19, "  ") && RI.CheckForText(16, 63, "  "))
                    {
                        return true;
                    }
                    RI.Hit(Key.F8);
                }
            }
            return false;
        }

        // enter a deferment
        private void EnterDeferment(string ssn)
        {
            bool nonSubrogatedFound = false;
            bool nonAuxStatusSkip = false;
            RI.FastPath("LC05C");
            RI.PutText(21, 13, "01", Key.Enter);
            bool createdFollowUpTask = false;
            DateTime dueDate = DateTime.Today;

            while (!RI.CheckForText(22, 3, "46004"))
            {
                string claimId = GetText(21, 11, 4);
                // if loan is not subrogated
                bool auxStatusSkip = false;
                if (RI.CheckForText(19, 73, "MMDDCCYY"))
                {
                    nonSubrogatedFound = true;
                    bool auxStatusIsBlank = RI.CheckForText(4, 26, "__");
                    RI.Hit(Key.F10);
                    if (auxStatusIsBlank && RI.CheckForText(17, 2, "06") == false) { auxStatusSkip = true; } // set flag to bypass all other LC05 processing
                }
                if (auxStatusSkip == false) // check flag of additional
                {
                    nonAuxStatusSkip = true;
                    RI.Hit(Key.F10);
                    RI.Hit(Key.F10);
                    if (RI.CheckForText(4, 10, "03") && RI.CheckForText(4, 26, "__") && RI.CheckForText(19, 73, "MMDDCCYY"))
                    {
                        // go to page 2
                        RI.Hit(Key.F10);
                        // enter deferment info
                        RI.PutText(17, 2, "06");
                        RI.PutText(18, 2, DateTime.Today.ToString("MMddyyyy"));
                        if (dueDate.Date != DateTime.Today)
                        {
                            RI.PutText(18, 11, dueDate.ToString("MMddyyyy"), Key.Enter);
                        }
                        dueDate = ReviewAccount(ssn, createdFollowUpTask, dueDate, claimId);
                        createdFollowUpTask = true;
                    }
                    else if (RI.CheckForText(4, 10, "03") && RI.CheckForText(4, 26, "05") & RI.CheckForText(19, 73, "MMDDCCYY"))
                    {
                        // go to page 2
                        RI.Hit(Key.F10);
                        if (RI.CheckForText(17, 2, "06"))
                        {
                            if (dueDate.Date != DateTime.Today) { RI.PutText(18, 11, dueDate.ToString("MMddyyyy"), Key.Enter); }
                            dueDate = ReviewAccount(ssn, createdFollowUpTask, dueDate, claimId);
                            createdFollowUpTask = true;
                        }
                    }
                }
                RI.FastPath("LC05C");
                RI.PutText(21, 13, "01", Key.Enter);
                while (!RI.CheckForText(21, 11, claimId))
                    RI.Hit(Key.F8);  // go to next loan
                // go to next loan
                RI.Hit(Key.F8);
            }
            if (nonSubrogatedFound == false)
            {
                string message = "The borrower's loans have been transferred to ED and the prison queue task will be completed.";
                Dialog.Info.Ok(message, ScriptId);
                string comment = "Prison deferment not added due to subrogated status";
                RI.AddCommentInLP50(ssn, "AM", "10", "KPRIS", comment, ScriptId);
                return;
            }
            if (nonAuxStatusSkip == false)
            {
                string message = "The borrower's loans are in a current deferment status.  Please resolve that status before processing.  The prison queue task will be completed without adding the prison deferment.";
                Dialog.Warning.Ok(message, ScriptId);
                string comment = "Prison deferment not added due to borrower's status";
                RI.AddCommentInLP50(ssn, "AM", "10", "KPRIS", comment, ScriptId);
                return;
            }
            if (createdFollowUpTask == false)
            {
                string actionCode = "DD023";
                string activityType = "AM";
                string contactType = "10";
                string comment = "reviewed for incarcerated borrower, borrower no longer incarcerated, no updates required";
                RI.AddCommentInLP50(ssn, activityType, contactType, actionCode, comment, ScriptId);
            }
        }

        private DateTime ReviewAccount(string ssn, bool createdFollowUpTask, DateTime dueDate, string claimId)
        {
            while (!RI.CheckForText(22, 3, "49000"))
            {
                PauseForInsert();
                dueDate = DateTime.Parse(GetText(18, 11, 8).ToDateFormat());
                RI.Hit(Key.Enter);
            }
            ApplyWorkGroup23ToClaim(ssn, claimId);
            if (createdFollowUpTask == false)
            {
                // create follow up task in DJAILBRD
                string queue = "SKIPPRSN";
                string queueComment = "review due for status of incarcerated borrower";
                RI.AddQueueTaskInLP9O(ssn, queue, dueDate, queueComment);
                // add activity record to LP50
                string activityCode = "DD023";
                string activityType = "AM";
                string contactType = "10";
                StringBuilder commentBuilder = new StringBuilder();
                commentBuilder.Append("reviewed for incarcerated borrower;");
                commentBuilder.AppendFormat(" LC05: defrmnt 06 from {0:MM/dd/yyyy} to {1:MM/dd/yyyy};", DateTime.Today, dueDate);
                commentBuilder.AppendFormat(" LC34: wg to 23; SKIPPRSN task for (0:MM/dd/yyyy)", dueDate);
                RI.AddCommentInLP50(ssn, activityType, contactType, activityCode, commentBuilder.ToString(), ScriptId);
            }
            return dueDate;
        }

        // put loan in WG000023
        private void ApplyWorkGroup23ToClaim(string ssn, string claimId)
        {
            // access LC34
            RI.FastPath("LC34C" + ssn + ";01");
            // enter workgroup
            RI.PutText(5, 7, "WG000023");
            // select the loan
            int row = 9;
            while (RI.CheckForText(row, 3, "_"))
            {
                if (RI.CheckForText(row, 5, claimId))
                {
                    RI.PutText(row, 3, "X");
                    break;
                }
                else
                {
                    row++;
                }
                if (row == 21)
                {
                    RI.Hit(Key.F8);
                    row = 9;
                }
            }
            RI.Hit(Key.Enter);
            if (RI.CheckForText(22, 3, "47002"))
            {
                string message = string.Format("Loan{0} not found.  Click OK to continue.", claimId);
                MessageBox.Show(message, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // add queue tasks
        private void CreateQueueTask(string ssn, string queue, string queueComment, string activityComment, string contactType)
        {
            RI.FastPath("LP9OA" + ssn + ";;" + queue);
            if (RI.CheckForText(22, 3, "44000"))
            {
                RI.PutText(11, 25, DateTime.Today.ToString("MMddyyyy"));
                RI.PutText(16, 12, queueComment);
                RI.Hit(Key.Enter);
                RI.Hit(Key.F6);
                Thread.Sleep(2000);
            }
            else
            {
                string message = "Unable to add task.  Wait for the script to finish and then enter the task manually.";
                MessageBox.Show(message, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            RI.AddCommentInLP50(ssn, "AM", contactType, "KPRIS", activityComment, ScriptId);
        }
    }
}
