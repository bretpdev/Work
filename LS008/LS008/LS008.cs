using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace LS008
{
    public class LS008 : ScriptBase
    {
        private DataAccess DA { get; set; }
        public LS008(ReflectionInterface ri)
            : base(ri, "LS008", DataAccessHelper.Region.Pheaa)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DA = new DataAccess(ProcessLogData.ProcessLogId);
        }

        public override void Main()
        {
            LS008Data qData = GetNextQueue();
            while (qData != null)
            {
                using (ProcessSelection ps = new ProcessSelection(qData, DA, RI))
                {
                    if (ps.ShowDialog() != DialogResult.OK)
                        return;

                    bool arcsAdded = ProcessCannedComents(ps.SelectedProcesses, qData);

                    if (arcsAdded)
                        arcsAdded = ProcessManualComents(ps.ManualCmts, qData);
                    if (ps.PlaceTaskOnHold || !arcsAdded)
                        PlaceTaskOnHold(qData);
                    else
                    {
                        CloseTask();
                        CommentAndCloseDupTasks(qData);
                    }
                    if (ps.DoneProcessing)
                        break;
                }

                qData = GetNextQueue();
            }

            ProcessLogger.LogEnd(ProcessLogData.ProcessLogId);
        }

        private void CommentAndCloseDupTasks(LS008Data qData)
        {
            foreach (var task in qData.DupDcns.Where(p => p.Selected))
            {
                string seq = task.ActivitySeq;
                CommentTask(qData, string.Format("Dup Task See Seq:{0}", qData.ActivitySeq), seq);
                AccessQueue(task.TaskControlNumber);
                RI.PutText(21, 18, "01", ReflectionInterface.Key.F2);
                RI.PutText(8, 19, "C");
                RI.PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);
            }
        }

        private void CommentTask(LS008Data qData, string comment, string seq)
        {
            RI.FastPath("TX3Z/ITD2A*");
            RI.PutText(4, 16, qData.BwrSsn);
            RI.PutText(12, 65, seq, ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F4);
            RI.PutText(8, 5, comment, ReflectionInterface.Key.Enter);
        }

        private bool ProcessCannedComents(List<DbData> arcsToAdd, LS008Data qData)
        {
            foreach (var record in arcsToAdd)
            {
                if (!AddArcs(qData, record.Arc, record.ArcMessageText ?? string.Empty))
                    return false;
                CommentTask(qData, record.OriginalCommentText, qData.ActivitySeq);
            }

            return true;
        }
        private bool ProcessManualComents(List<UserInputedCmt> arcsToAdd, LS008Data qData)
        {
            foreach (var record in arcsToAdd)
            {
                if (record.Arc.IsPopulated())
                    if(!AddArcs(qData, record.Arc, record.ArcComment, record.LoanSeqs, record.Recipient))
                        return false;
                if (record.Ls008Comment.IsPopulated())
                    CommentTask(qData, record.Ls008Comment, qData.ActivitySeq);
            }

            return true;
        }


        private void CloseTask()
        {
            RI.FastPath("TX3Z/ITX6XSB;01;;LS008");
            bool selectedTask = false;
            for (int row = 8; RI.MessageCode != "90007"; row += 3)
            {
                if (RI.CheckForText(row, 4, " ") || row > 20)
                {
                    row = 5;
                    RI.Hit(ReflectionInterface.Key.F8);
                    continue;
                }
                if (RI.CheckForText(row, 75, "W"))
                {
                    RI.PutText(21, 18, RI.GetText(row, 4, 1), ReflectionInterface.Key.F2);
                    selectedTask = true;
                    break;
                }
            }

            if (!selectedTask)
                return;

            RI.PutText(8, 19, "C");
            RI.PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);
        }

        private bool AddArcs(LS008Data qData, string arc, string arcMessageText, List<int> loanSeq = null, string recipId = "")
        {
            bool arcResult = false;
            if (qData.HasNoLoans)
            {
                //Dialog.Error.Ok("The script will now end please send this task to Jarom, he needs to step through the code to make sure it is coded correctly.");
                //throw new Exception(string.Format("{0} {1}", qData.AccountNumber, qData.TaskControlNumber));
                arcResult = RI.ATD42AllLoans(qData.BwrSsn, arc, arcMessageText ?? string.Empty);
            }
            else
            {
                if (loanSeq != null && loanSeq.Any())
                    arcResult = RI.Atd22ByLoan(qData.BwrSsn, arc, arcMessageText ?? string.Empty, recipId, loanSeq, string.Empty, false);
                else
                    arcResult = RI.Atd22ByBalance(qData.BwrSsn, arc, arcMessageText ?? string.Empty, recipId, string.Empty, false, false);
            }

            bool hold = false;
            if (!arcResult)
            {
                string message = string.Format("The following ARC was not added. ARC:{0} ACCT:{1} ERRORCODE:{2} COMMENT{3}.  Do you want to place this task on hold?",
                    arc, qData.AccountNumber, RI.Message, arcMessageText ?? string.Empty);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                hold = Dialog.Error.YesNo(message);
            }

            return hold;
        }

        private void PlaceTaskOnHold(LS008Data qData)
        {
            ManualComments cmts = new ManualComments(string.Empty, 70);
            cmts.SetLabel("Please enter the reason the task is being placed on hold.");
            cmts.ShowDialog();
            PlaceOnHold(qData, qData.TaskControlNumber, cmts.Comment, qData.ActivitySeq);

            foreach (var record in qData.DupDcns)
                PlaceOnHold(qData, record.TaskControlNumber, cmts.Comment, record.ActivitySeq);

            if (Dialog.Info.YesNo("Do you want to email this task to the Supervisor group?"))
            {
                string body = string.Format("{0}, {1}, {2}, {3}", qData.BorrowerName, qData.AccountNumber, qData.CorrDocNum, cmts.Comment); //GetEmailBody(qData, cmts.Comment);
                EmailHelper.SendMail(DataAccessHelper.TestMode, "borrcorrcontractor@utahsbr.edu", "LS008@utahsbr.edu", "LS008 Hold Task", body, string.Empty, string.Empty, EmailHelper.EmailImportance.High, true);
            }

            DA.InsertHoldData(qData, DateTime.Now.AddDays(2), UserId, cmts.Comment);
        }

        private void PlaceOnHold(LS008Data qData, string taskNumber, string comment, string actSeq)
        {
            AccessQueue(string.Format("{0}*", taskNumber));

            RI.PutText(21, 18, "01", ReflectionInterface.Key.F2);
            RI.PutText(8, 19, "H", ReflectionInterface.Key.Enter);
            RI.FastPath("TX3Z/CTX6J");
            RI.PutText(7, 42, "SB");
            RI.PutText(8, 42, "01");
            RI.PutText(10, 42, "LS008");
            RI.PutText(9, 42, (taskNumber));
            RI.PutText(9, 76, "A", ReflectionInterface.Key.Enter);
            RI.PutText(21, 2, string.Format("UHEAA {0}", comment), ReflectionInterface.Key.Enter, true);
            RI.Hit(ReflectionInterface.Key.F12);
            CommentTask(qData, string.Format("***task on hold {0:MM/dd/yyyy} {1}", DateTime.Now, comment), actSeq);
        }

        private string GetEmailBody(LS008Data qData, string reason)
        {
            List<string> lines = new List<string>();
            lines.Add(string.Format("ACCT: {0}", qData.AccountNumber));
            lines.Add(string.Format("DCN: {0}", qData.CorrDocNum));
            lines.Add(string.Format("REASON: {0}", reason));
            return string.Join("<br>", lines);
        }

        private LS008Data GetNextQueue()
        {
            AccessQueue();

            if (!RI.CheckForText(1, 74, "TXX71"))
                return null;

            if (!RI.GetText(8, 75, 1).IsIn("W", "A"))
            {
                Dialog.Error.Ok("You do not have any tasks assigned.");
                return null;
            }
            LS008Data qData = new LS008Data() { TaskControlNumber = RI.GetText(8, 6, 20) };
            string tempSSN = RI.GetText(8, 6, 9);
            RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter, true);
            if (RI.MessageCode == "01848")
            {
                Dialog.Error.Ok(RI.Message);
                return null;
            }
            qData.BwrSsn = RI.GetText(4, 16, 11).Replace(" ", "");
            if (RI.MessageCode == "50108")//Borrower has no loans
            {
                qData.BwrSsn = tempSSN;
                qData.HasNoLoans = true;
            }


            var demos = RI.GetDemographicsFromTx1j(qData.BwrSsn);
            qData.AccountNumber = demos.AccountNumber;
            qData.BorrowerName = string.Format("{0} {1}", demos.FirstName + (demos.MiddleIntial.IsPopulated() ? " " + demos.MiddleIntial : ""), demos.LastName);

            RI.FastPath("TX3Z/ITD2A*");
            RI.PutText(4, 16, qData.BwrSsn);
            RI.PutText(12, 65, qData.ActivitySeq, ReflectionInterface.Key.Enter);
            if (RI.ScreenCode != "TDX2D")
            {
                Dialog.Error.Ok(string.Format("Unable to locate ARC in TD2A for Task Control Number {0}.  Please review and try again.", qData.TaskControlNumber));
                return null;
            }

            qData.CorrDocNum = RI.GetText(15, 64, 18);
            qData.DupDcns = GetPossibleDupTasks(qData);
            qData.LoanSeq = GetLoanSeq(qData);
            //qData.HistoryComments = GetHistoryComments(qData);

            return qData;
        }

        private List<HistoryCommentData> GetHistoryComments(LS008Data qData)
        {
            List<HistoryCommentData> comments = new List<HistoryCommentData>();
            RI.FastPath(string.Format("TX3Z/ITD2A{0}", qData.BwrSsn));
            RI.PutText(21, 16, DateTime.Now.AddDays(-60).ToString("MMddyy"));
            RI.PutText(21, 30, DateTime.Now.ToString("MMddyy"), ReflectionInterface.Key.Enter);
            RI.PutText(5, 14, "X", ReflectionInterface.Key.Enter);
            while (RI.MessageCode != "90007")
            {
                comments.Add(new HistoryCommentData(RI));
                RI.Hit(ReflectionInterface.Key.F8);
            }

            return comments;
        }

        private List<int> GetLoanSeq(LS008Data qData)
        {
            List<int> loans = new List<int>();
            RI.FastPath("TX3Z/ITS26" + qData.BwrSsn);
            if (!RI.ScreenCode.IsIn("TSX29", "TSX28"))
                return new List<int>();
            if (RI.ScreenCode == "TSX29")
                loans.Add(RI.GetText(7, 35, 4).ToInt());
            else
            {
                for (int row = 8; RI.MessageCode != "90007"; row++)
                {
                    if (row > 20 || RI.CheckForText(row, 3, " "))
                    {
                        row = 7;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }

                    loans.Add(RI.GetText(row, 14, 4).ToInt());
                }
            }

            return loans;
        }

        private List<DupDcns> GetPossibleDupTasks(LS008Data qData)
        {
            AccessQueue(qData.BwrSsn + "*");
            if (RI.MessageCode == "01020")
                return null;

            List<DupDcns> possibleDups = new List<DupDcns>();

            for (int row = 11; RI.MessageCode != "90007"; row += 3)
            {
                if (row > 20 || RI.CheckForText(row, 4, " "))
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 5;
                    continue;
                }

                possibleDups.Add(new DupDcns() { TaskControlNumber = RI.GetText(row, 6, 19) });
            }

            foreach (var item in possibleDups)
            {
                RI.FastPath("TX3Z/ITD2A*");
                RI.PutText(4, 16, qData.BwrSsn);
                RI.PutText(12, 65, item.ActivitySeq, ReflectionInterface.Key.Enter);
                if (RI.ScreenCode != "TDX2D")
                {
                    Dialog.Error.Ok(string.Format("Unable to locate ARC in TD2A for Task Control Number {0}.  Please review and try again.", qData.TaskControlNumber));
                    return null;
                }

                item.Dcn = RI.GetText(15, 64, 18);
            }

            return possibleDups;
        }

        private void AccessQueue(string ssn = "")
        {
            RI.FastPath("TX3ZITX6X");
            RI.PutText(6, 37, "SB", true);
            RI.PutText(8, 37, "01", true);
            RI.PutText(10, 37, ssn, true);
            RI.PutText(12, 37, "LS008", ReflectionInterface.Key.Enter, true);
        }
    }
}
