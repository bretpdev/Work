using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace KEYIDENCHN
{
    public class KeyIdentifierChange : FedBatchScript
    {
        const int MaxCommentLength = 154;
        public KeyIdentifierChange(ReflectionInterface ri)
            : base(ri, "KEYIDENCHN", "ERR_BU35", "EOJ_BU35", new string[] { })
        {
            acct = new AccountInfo(this, RI);
            new Thread(Supervisor.LoadSupervisors).Start();
        }
        bool IsSupervisor { get { return super != null; } }
        SupervisorInfo super = null;
        AccountInfo acct;
        VerificationInfo verifiedChanges;
        Supervisor reviewSupervisor;
        bool approval = false;
        public override void Main()
        {
            CheckSupervisorAccess();
            SelectQueueTask();
            LoadBorrower();
            if (KeyIdentifierUpdate())
            {
                CloseQueue();
                ProcessingComplete();
            }
        }

        /// <summary>
        /// Check if borrower is a supervisor and if they want to run in supervisor mode
        /// </summary>
        private void CheckSupervisorAccess()
        {
            if (DataAccess.CurrentUserHasSupervisorAccess())
            {
                using (SupervisorPrompt sp = new SupervisorPrompt())
                    if (sp.ShowDialog() == DialogResult.OK)
                        super = sp.IsSupervisor ? new SupervisorInfo(this) : null;
                    else
                        EndDllScript();
            }
        }

        /// <summary>
        /// Navigate to and select an applicable queue task
        /// 
        /// </summary>
        private void SelectQueueTask()
        {
            FastPath("TX3Z/ITX6XS6;01");
            if (RI.MessageCode == "01020")
                NotifyAndEnd("There are no S6/01 queue tasks available.");
            if (!CheckForText(8, 75, "W"))
                NotifyAndEnd("You have not selected an S6/01 queue task to work. Please select a task and start this script again.");
            if (IsSupervisor)
                super.LoadQueueTaskComment(RI);
            PutText(21, 19, "1", ReflectionInterface.Key.Enter);
            if (!CheckForText(1, 76, "TSX25"))
                NotifyAndEnd("A queue task was selected, but the session did not navigate to the TSX25 screen.  Unable to proceed.");
            var totalBal = RI.GetText(19, 42, 14).ToDecimalNullable();
            if (totalBal.HasValue && totalBal <= 0)
                NotifyAndEnd("Borrower has no open loans");
        }

        /// <summary>
        /// Load Borrower Information
        /// </summary>
        private void LoadBorrower()
        {
            acct.LoadAccount();
            FastPath("TX3Z/ITX1J;" + acct.SSN);
            if (RI.ScreenCode == "01019")
                NotifyAndEnd("Borrower not found on TX1J. Please review and start this script again");
            acct.LoadDemographics();
        }

        /// <summary>
        /// Display the Key Identifier interface and allow user to make changes
        /// </summary>
        private bool KeyIdentifierUpdate()
        {
            using (var form = new KeyIdentifierUpdateForm(acct, super))
            {
                bool showAgain = true;
                while (showAgain)
                {
                    showAgain = false;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        verifiedChanges = form.VerifiedChanges;
                        reviewSupervisor = form.ReviewSupervisor;
                        approval = form.Approval;
                    }
                    else
                        return false;

                    if (reviewSupervisor == null)
                    {
                        if (LeaveComments())
                        {
                            if (approval)
                                UpdateSystem();
                        }
                        else
                            showAgain = true;
                    }
                    else if (approval)
                    {
                        if (SupervisorReview())
                            LeaveComments();
                        else
                            showAgain = true;
                    }
                }
            }
            return true;
        }

        private void UpdateSystem()
        {
            FastPath("TX3Z/CTX1JB;" + acct.SSN);
            //enter current date
            PutText(3, 61, DateTime.Now.ToString("MMddyy"));

            //enter changes
            PutIfPop(4, 34, 13, verifiedChanges.FirstName);
            PutIfPop(4, 53, 13, verifiedChanges.MiddleName);
            if (verifiedChanges.RemoveMiddleName)
                PutText(4, 53, new string(' ', 13));
            PutIfPop(4, 6, 23, verifiedChanges.LastName);
            PutIfPop(4, 72, 4, verifiedChanges.Suffix);
            if (verifiedChanges.RemoveSuffix)
                PutText(4, 72, new string(' ', 13));
            PutIfPop(20, 6, 8, verifiedChanges.DOB.Replace("/", ""));

            RI.Hit(ReflectionInterface.Key.Enter);
            if (RI.MessageCode == "01709") //possible duplicate exists
                RI.Hit(ReflectionInterface.Key.Enter);
            if (!RI.MessageCode.IsNullOrEmpty() && RI.MessageCode != "01093")
                NotifyAndEnd("Error {0} received while updating system.  Please review and update manually", RI.MessageCode);
        }

        private bool LeaveComments()
        {
            string comment = "";
            if (reviewSupervisor != null)
            {
                comment = "Key identifier request sent for supervisor review";
            }
            else
            {
                if (super != null)
                    comment = super.Comment;
                using (ManualComments manualCmts = new ManualComments(comment))
                {
                    manualCmts.MaxCharactersAllowed = MaxCommentLength;
                    manualCmts.CommentRequired = true;
                    if (manualCmts.ShowDialog() == DialogResult.OK)
                        comment = manualCmts.Comment;
                    else
                        return false;
                }
            }
            if (!RI.Atd22ByBalance(acct.SSN, "LSKEY", comment, acct.SSN, "KEYIDENCHN", false, false))
                NotifyAndEnd("Error {0}.  Please add LSKEY comment manually and close queue task.", RI.MessageCode);
            return true;
        }

        private bool SupervisorReview()
        {
            string comment = null;
            using (ManualComments manualCmts = new ManualComments(comment))
            {
                manualCmts.MaxCharactersAllowed = MaxCommentLength;
                manualCmts.CommentRequired = true;
                if (manualCmts.ShowDialog() == DialogResult.OK)
                    comment = manualCmts.Comment;
                else
                    return false;
            }
            comment = string.Join(",", new string[] { verifiedChanges.FirstName, verifiedChanges.MiddleName, verifiedChanges.LastName, verifiedChanges.Suffix, verifiedChanges.DOB, comment });

            if (!RI.Atd22ByBalance(acct.SSN, "MAKEY", comment, acct.SSN, "KEYIDENCHN", false, false))
                NotifyAndEnd("Error {0}.  Please create Supervisor Review task manually using the MAKEY arc, add LSKEY comment manually, and close queue task.", RI.MessageCode);

            FastPath("TX3Z/CTX6J;");
            PutText(7, 42, "S6");
            PutText(8, 42, "01");
            PutText(10, 42, "MAKEY");
            RI.Hit(ReflectionInterface.Key.Enter);
            if (RI.MessageCode == "01487")
                NotifyAndEnd("Error {0}.  Please manually reassign S6/01 queue task for Supervisor Review, add LSKEY comment manually, and close queue task.", RI.MessageCode);
            if (RI.ScreenCode == "TXX6N")
            {
                int selection = 1;
                bool found = false;
                while (!found && RI.MessageCode != "90007")
                {
                    selection = 1;
                    for (int row = 9; row < 19; row += 2)
                    {
                        string foundSsn = GetText(row, 38, 9);
                        bool userFieldBlank = GetText(row + 1, 20, 1).IsNullOrEmpty();
                        if (foundSsn == acct.SSN && userFieldBlank)
                        {
                            found = true;
                            break;
                        }
                        selection++;
                    }
                    if (!found)
                        RI.Hit(ReflectionInterface.Key.F8);
                }
                PutText(21, 18, selection.ToString().PadLeft(2, '0'), ReflectionInterface.Key.Enter);
            }
            PutText(8, 15, reviewSupervisor.UtId, ReflectionInterface.Key.Enter);
            if (RI.MessageCode != "01005")
                NotifyAndEnd("Error {0}.  Please manually reassign S6/01 queue task for Supervisor Review, add LSKEY comment manually, and close queue task.", RI.MessageCode);
            return true;
        }

        private void CloseQueue()
        {
            FastPath("TX3Z/ITX6XS6;01");
            int selection = 1;
            for (int row = 8; row <= 18; row += 3)
            {
                if (CheckForText(row, 75, "W"))
                    break;
                selection++;
            }
            PutText(21, 18, selection.ToString().PadLeft(2, '0'), ReflectionInterface.Key.F2);
            PutText(8, 19, "C");
            PutText(9, 19, "COMPL");
            RI.Hit(ReflectionInterface.Key.Enter);
            if (RI.MessageCode == "01644")
            {
                RI.PutText(9, 19, "", ReflectionInterface.Key.EndKey); //blank out field
                RI.Hit(ReflectionInterface.Key.Enter);
            }
            if (!RI.MessageCode.IsNullOrEmpty() && RI.MessageCode != "01005") //01005 == success
                NotifyAndEnd("Error {0}.  Please manually close queue task.", RI.MessageCode);
        }


        private void PutIfPop(int row, int col, int length, string val)
        {
            if (!val.IsNullOrEmpty())
                PutText(row, col, val.PadRight(length, ' '));
        }

        public new void NotifyAndEnd(string format, params object[] args)
        {
            base.NotifyAndEnd(format, args);
        }
    }
}
