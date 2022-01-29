using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace LS008
{
    public partial class ProcessSelection : Form
    {
        private List<DbData> Processes { get; set; }
        private LS008Data QData { get; set; }
        public List<DbData> SelectedProcesses { get; set; }
        public List<UserInputedCmt> ManualCmts { get; set; }
        public bool PlaceTaskOnHold { get; set; }
        public bool DoneProcessing { get; set; }
        private ReflectionInterface RI { get; set; }
        private Task HistoryThread { get; set; }

        public ProcessSelection(LS008Data qData, DataAccess da, ReflectionInterface ri)
        {
            InitializeComponent();
            RI = ri;
            DocNum.Text = qData.CorrDocNum;
            AN.Text = qData.AccountNumber;
            SSN.Text = qData.BwrSsn;
            ActSeq.Text = qData.ActivitySeq;
            TC.Text = qData.TaskControlNumber;
            Processes = da.GetAllProcesses();
            Pro.Items.AddRange(Processes.Select(p => string.Format("{0}-{1}", p.Arc, p.ProcessName)).ToArray());
            QData = qData;
            DupSel.Enabled = QData.DupDcns.Any();
            SelectedProcesses = new List<DbData>();
            ManualCmts = new List<UserInputedCmt>();
        }

        private void DocNum_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(DocNum.Text);
        }

        private void CDN_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(DocNum.Text);
        }

        private void CAN_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(AN.Text);
        }

        private void CSSN_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(SSN.Text);
        }

        private void CAS_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(ActSeq.Text);
        }

        private void CTCN_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(TC.Text);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            if (Dialog.Error.YesNo("Are you sure you want to cancel processing this task?"))
                DialogResult = DialogResult.Cancel;
        }

        private void ProcessSelection_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => QData.HistoryComments = GetHistoryComments(QData));
            ShowDup();
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

            EnableHist();

            return comments;
        }

        delegate void SetTextCallback();
        private void EnableHist()
        {
            if (this.HistCmts.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(EnableHist);
                this.Invoke(d);
            }
            else
            {
               HistCmts.Enabled = true;
            }
        }

        private void ShowDup()
        {
            if (QData.DupDcns.Any())
            {
                using (DupDcnsChooser ddc = new DupDcnsChooser(QData.DupDcns, QData.CorrDocNum))
                {
                    ddc.ShowDialog();
                    QData.DupDcns = ddc.DupDcns;
                }
            }
        }

        private void DupSel_Click(object sender, EventArgs e)
        {
            ShowDup();
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            foreach (var item in Pro.CheckedItems)
                SelectedProcesses.Add(Processes.Where(p => p.Arc == item.ToString().Substring(0, item.ToString().IndexOf("-"))).First());

            if (SelectedProcesses.Any() || ManualCmts.Any() || PlaceOnHold.Checked)
            {
                PlaceTaskOnHold = PlaceOnHold.Checked;
                DoneProcessing = LastTask.Checked;

                DialogResult = DialogResult.OK;
            }
        }

        private void Pro_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && (Pro.CheckedItems.Count == 0 || Pro.CheckedItems.Count >= 1))
                Continue.Enabled = true; 
            else if (e.NewValue == CheckState.Unchecked && Pro.CheckedItems.Count == 1 && !PlaceOnHold.Checked)
                Continue.Enabled = false;
            else
                Continue.Enabled = true;
        }

        private void AddComment_Click(object sender, EventArgs e)
        {
            using (ManualCommentTask mcmt = new ManualCommentTask(ManualCmts, QData))
            {
                if (mcmt.ShowDialog() == DialogResult.OK)
                {
                    ManualCmts = mcmt.Comments;
                    if (!Continue.Enabled)
                        Continue.Enabled = true;
                    else if (!ManualCmts.Any() && !PlaceOnHold.Checked && Pro.CheckedItems.Count == 0)
                        Continue.Enabled = false;
                   
                }
                else
                {
                    if (!PlaceOnHold.Checked || Pro.CheckedItems.Count == 0)
                        Continue.Enabled = false;
                }
            }
        }

        private void PlaceOnHold_CheckedChanged(object sender, EventArgs e)
        {
            if (Continue.Enabled)
                return;
            else if (PlaceOnHold.Checked)
                Continue.Enabled = true;
        }

        private void HistCmts_Click(object sender, EventArgs e)
        {
            HistoryThread = Task.Factory.StartNew(() => new HistoryComments(QData.HistoryComments, RI, QData.BwrSsn).ShowDialog());
        }
    }
}
