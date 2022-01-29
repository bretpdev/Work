using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace LS008
{
    public partial class ManualCommentTask : Form
    {
        public List<UserInputedCmt> Comments { get; set; }
        private LS008Data Qdata { get; set; }
        private List<int> SelectedLoanSeq { get; set; }
        public ManualCommentTask(List<UserInputedCmt> comments, LS008Data qData)
        {
            InitializeComponent();
            Comments = comments;
            SetDataSource();
            Qdata = qData;

            if (Qdata.LoanSeq.Any())
                Loans.Enabled = true;
        }

        private void SetDataSource()
        {
            ArcsToAdd.DataSource = null;
            SelectedLoanSeq = new List<int>();
            if (Comments.Any())
            {
                ArcsToAdd.DataSource = Comments;
                ArcsToAdd.Columns[0].Width = 75;
                ArcsToAdd.Columns[1].Width = 225;
                ArcsToAdd.Columns[2].Width = 225;
                ArcsToAdd.Columns[3].Width = 100;
                ArcsToAdd.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                ArcsToAdd.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                ArcsToAdd.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                ArcsToAdd.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                ArcsToAdd.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                ArcsToAdd.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                ArcsToAdd.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                ArcsToAdd.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                ArcsToAdd.Columns[1].HeaderText = "Arc Comment";
                ArcsToAdd.Columns[2].HeaderText = "LS008 Comment";
                foreach (DataGridViewColumn c in ArcsToAdd.Columns)
                    c.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F, GraphicsUnit.Pixel);
            }
        }

        private void Arc_Leave(object sender, EventArgs e)
        {
            EnableAdd();
        }

        private void EnableAdd()
        {
            if (ArcComment.Text.IsPopulated() && Arc.Text.IsNullOrEmpty())
                Add.Enabled = false;
            else if (TaskComment.Text.IsPopulated())
                Add.Enabled = true;
            else
                Add.Enabled = ((Arc.Text.IsPopulated() && ArcComment.Text.IsPopulated()) && TaskComment.Text.IsPopulated());
        }

        private void EnableCont()
        {
            Continue.Enabled = ArcsToAdd.RowCount > 0;
        }

        private void ArcComment_Leave(object sender, EventArgs e)
        {
            EnableAdd();
        }

        private void TaskComment_Leave(object sender, EventArgs e)
        {
            //EnableAdd();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (Recipid.Text.IsPopulated() && Recipid.Text.Length < 9)
            {
                Recipid.BackColor = Color.LightPink;
                return;
            }
            else
                Recipid.BackColor = SystemColors.Window;
            Comments.Add(new UserInputedCmt() { Arc = Arc.Text, ArcComment = ArcComment.Text, Ls008Comment = TaskComment.Text, LoanSeqs = SelectedLoanSeq, Recipient = Recipid.Text });
            SetDataSource();
            EnableCont();
            Arc.Clear();
            ArcComment.Clear();
            TaskComment.Clear();
            Recipid.Clear();
            Add.Enabled = false;
        }

        private int RowToDelete { get; set; }
        private void Delete_Click(object sender, EventArgs e)
        {
            if (Comments.Any())
            {
                Comments.RemoveAt(RowToDelete);
                SetDataSource();
                RowToDelete = -1;
            }
        }

        private void ArcsToAdd_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            RowToDelete = e.RowIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (LoanSelect ls = new LoanSelect(Qdata.LoanSeq.OrderBy(p => p).ToList(), SelectedLoanSeq))
            {
                if (ls.ShowDialog() == DialogResult.OK)
                    SelectedLoanSeq = ls.SelectedLoans;
                else
                    SelectedLoanSeq = new List<int>();
            }
        }

        private void ArcsToAdd_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var item = Comments[e.RowIndex];
            Comments.RemoveAt(e.RowIndex);
            SetDataSource();
            SetFields(item);
        }

        private void SetFields(UserInputedCmt data)
        {
            Arc.Text = data.Arc;
            ArcComment.Text = data.ArcComment;
            TaskComment.Text = data.Ls008Comment;
            SelectedLoanSeq = data.LoanSeqs;
            Recipid.Text = data.Recipient;
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            if (Arc.Text.IsPopulated() || ArcComment.Text.IsPopulated() || TaskComment.Text.IsPopulated() || Recipid.Text.IsPopulated())
            {
                if (!Dialog.Error.YesNo("You have an unsaved ARC/Comment.  Are you sure you want to continue?"))
                    return;
            }
            DialogResult = DialogResult.OK;
        }

        private void TaskComment_TextChanged(object sender, EventArgs e)
        {
            Add.Enabled = TaskComment.Text.IsPopulated();
        }
    }
}
