using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ALIGNPNOTE
{
    public class PNoteReconciliation : ScriptBase
    {
        public PNoteReconciliation(ReflectionInterface ri)
            : base(ri, "ALIGNPNOTE")
        {
        }

        public override void Main()
        {
            ValidateRegion(Uheaa.Common.DataAccess.DataAccessHelper.Region.Uheaa);
            DisplayForm();
        }

        /// <summary>
        /// Registers the ProcessLogger, creates and shows the form
        /// </summary>
        private void DisplayForm()
        {
            ProcessLogData = ProcessLogger.RegisterScript(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
            ReconciliationForm recForm;
            do
            {
                List<BorrowerData> data = GetBorrowerData();
                if (data == null)
                    break;
                recForm = new ReconciliationForm(data, this);
            }
            while (recForm.ShowDialog() == DialogResult.OK);
            ProcessLogger.LogEnd(ProcessLogData.ProcessLogId); //End the ProcessLogger
        }

        /// <summary>
        /// Retreives the borrower SSN from the WY/PN queue and then pulls the rest of the data from the AlignImport database.
        /// </summary>
        /// <returns></returns>
        private List<BorrowerData> GetBorrowerData()
        {
            List<BorrowerData> data = new List<BorrowerData>();

            FastPath("TX3Z/ITX6XWY;PN;;;");
            if (RI.MessageCode == "01020")
            {
                MessageBox.Show("There are no WY/PN tasks to work.", "No Tasks", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            PutText(21, 18, "01", ReflectionInterface.Key.Enter);
            CheckForErrors(); //Checks error message 01020 and ends script if there are none;
            string ssn = "";
            if (CheckForText(1, 76, "TSX25"))
                ssn = GetText(4, 16, 11).Replace(" ", "");
            if (!ssn.IsNullOrEmpty())
            {
                data = GetBorrowerFromSession(ssn);
            }
            return data;
        }

        /// <summary>
        /// Goes to TS26 and determines if there are multiple loans to pull data for
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns>List of BorrowerData objects for each loan with selected lender code</returns>
        private List<BorrowerData> GetBorrowerFromSession(string ssn)
        {
            List<BorrowerData> borData = new List<BorrowerData>();
            FastPath("TX3Z/ITS26" + ssn);
            bool HasData = false;
            if (RI.ScreenCode == "TSX29") //Only one loan available
            {
                BorrowerData bData = new BorrowerData();
                bData.SSN = ssn;
                HasData = PullData(bData);
                if (HasData) borData.Add(bData);
            }
            else //Multiple loans to check
            {
                while (RI.MessageCode != "90007")
                {
                    for (int i = 8; i < 20; i++)
                    {
                        BorrowerData data = new BorrowerData();
                        data.SSN = ssn;
                        string row = GetText(i, 2, 2);
                        if (row.IsNullOrEmpty())
                            break;
                        else
                            PutText(21, 12, GetText(i, 2, 2), true);
                        Hit(ReflectionInterface.Key.Enter);
                        HasData = PullData(data);
                        if (HasData) borData.Add(data);
                        Hit(ReflectionInterface.Key.F12);
                    }
                    Hit(ReflectionInterface.Key.F8);
                }
            }
            return borData;
        }

        /// <summary>
        /// Get the Signed date, disbursement date and loan sequence number from the session
        /// </summary>
        /// <param name="data"></param>
        /// <returns>True if data found, false if no data with listed lender code</returns>
        private bool PullData(BorrowerData data)
        {
            //List of lender codes the loan must belong to
            List<string> guarantor = new List<string>() { "000706", "000708", "000755", "000800", "000951", "000731", "000730" };
            Hit(ReflectionInterface.Key.Enter);
            if (CheckForText(6, 36, guarantor.ToArray()))
            {
                data.Signed_Date = GetText(12, 54, 8).Length > 0 ? (DateTime?)DateTime.Parse(GetText(12, 54, 8)) : null;
                Hit(ReflectionInterface.Key.F12);
                data.Loan_Sequence = int.Parse(GetText(7, 35, 4));
                data.Disbursement_Date = DateTime.Parse(GetText(6, 18, 8));
                return true;
            }
            else //No loans found with the listed lender code
            {
                Hit(ReflectionInterface.Key.F12);
                return false;
            }
        }

        /// <summary>
        /// Check to make sure that there are no 01020 or 01848 errors
        /// </summary>
        private void CheckForErrors()
        {
            if (RI.MessageCode == "01020")
            {
                string message = "There are no WY/PN tasks to work";
                MessageBox.Show(message, "No Queue Tasks", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                EndDllScript();
            }

            if (RI.MessageCode == "01848")
            {
                string message = "There is an open WY/PN task open that needs to be closed first";
                MessageBox.Show(message, "Open Task", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                EndDllScript();
            }
        }

        /// <summary>
        /// Notifies the user that the borrower was not in the database and unassigns the task.
        /// </summary>
        private void BorrowerNotInDB()
        {
            string message = "The borrower was not in the AlignImport database. Do you want to be un-assigned from the queue?";
            if (MessageBox.Show(message, "Borrower not found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FastPath("TX3Z/ITX6T");
                PutText(21, 18, "01", ReflectionInterface.Key.F2);
                PutText(8, 19, "U", ReflectionInterface.Key.Enter);
            }
            EndDllScript();
        }

        /// <summary>
        /// Retrieves all the data that was checked as having a Pnote
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public BorrowerData SelectedData(DataGridViewRow row)
        {
            BorrowerData bor = new BorrowerData();
            try
            {
                bor.SSN = row.Cells["SSN"].Value.ToString();
                if (row.Cells["Signed_Date"].Value != null)
                    bor.Signed_Date = DateTime.Parse(row.Cells["Signed_Date"].Value.ToString());
                else
                    bor.Signed_Date = null;
                bor.Loan_Sequence = int.Parse(row.Cells["Loan_Sequence"].Value.ToString());
                bor.Disbursement_Date = DateTime.Parse(row.Cells["Disbursement_Date"].Value.ToString());
                bor.PNote_Found = Convert.ToBoolean(row.Cells["PNote_Found"].Value);
            }
            catch (Exception ex)
            {
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, ex.Message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ProcessLogData.ExecutingAssembly, ex);
            }
            return bor;
        }


        /// <summary>
        /// Determines if the loan had a pnote and adds a comment accordingly.
        /// </summary>
        /// <param name="borData"></param>
        /// <param name="selected"></param>
        public void AddComments(List<BorrowerData> borData, bool selected)
        {
            string arc = selected ? "MPROM" : "NOPNT";
            string comment = "";
            string loanSeqs = "";

            if (selected)
            {
                loanSeqs = string.Join(",", borData.Select(p => p.Loan_Sequence.ToString()).ToArray());
                comment = string.Format("Pnote for Loan Sequence(s) {0} reconciled.", loanSeqs);
                string ssn = borData.Select(p => p.SSN).FirstOrDefault();
                if (!Atd22AllLoans(ssn, arc, comment, "", ScriptId, false))
                {
                    string message = string.Format("Error adding arc when PNote was found for borrower {0}", ssn);
                    ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }
            }
            else
            {
                foreach (BorrowerData data in borData)
                {
                    string message = "";
                    if (data.Signed_Date != null)
                    {
                        message = string.Format("Error adding arc for borrower {0}, loan sequence {1}, no pnote found, pnote signed on {2}", data.SSN, data.Loan_Sequence, data.Signed_Date.Value.ToShortDateString());
                        comment = string.Format("No pnote found for loan sequence {0}, pnote was signed on {1}", data.Loan_Sequence, data.Signed_Date.Value.ToShortDateString());
                    }
                    else
                    {
                        message = string.Format("Error adding arc for borrower {0}, loan sequence {1}, no pnote found, loan disbured on {2}", data.SSN, data.Loan_Sequence, data.Disbursement_Date.ToShortDateString());
                        comment = string.Format("No pnote found for loan sequence(s) {0}. Loan was disbursed on {1}", data.Loan_Sequence, data.Disbursement_Date.ToShortDateString());
                    }

                    if (!Atd22AllLoans(data.SSN, arc, comment, "", ScriptId, false))
                        ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }
            }
        }

        /// <summary>
        /// Closes the queue task and asks the user if they want to process more queues.
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public DialogResult CloseTask()
        {
            FastPath("TX3Z/ITX6T");
            PutText(21, 18, "01", ReflectionInterface.Key.F2);
            PutText(8, 19, "C");
            PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);
            return MessageBox.Show("Would you like to continue processing other WY/PN queues?", "Continue Processing?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}