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

namespace LS008
{
    public partial class HistoryComments : Form
    {
        private List<HistoryCommentData> AllComments { get; set; }
        private ReflectionInterface RI { get; set; }
        private string BwrSsn { get; set; }
        public HistoryComments(List<HistoryCommentData> allComments, ReflectionInterface ri, string bwrSSN)
        {
            InitializeComponent();
            AllComments = allComments;
            SetDataSource(AllComments);
            RI = ri;
            BwrSsn = bwrSSN;
        }

        private void SetDataSource(List<HistoryCommentData> comments)
        {
            Comments.DataSource = null;
            Comments.DataSource = comments;
            Comments.Columns[0].Width = 250;
            Comments.Columns[1].Width = 85;
            Comments.Columns[2].Width = 65;
            Comments.Columns[3].Width = 65;
            Comments.Columns[4].Width = 600;
            Comments.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Comments.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Comments.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Comments.Columns[4].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            SetDataSource(AllComments.Where(p => p.Description.Contains(Search.Text.ToUpper())).ToList());
        }

        private void Comments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string seq = Comments.Rows[e.RowIndex].Cells[2].Value.ToString();
            RI.FastPath("TX3Z/ITD2A*");
            RI.PutText(4, 16, BwrSsn);
            RI.PutText(12, 65, seq, ReflectionInterface.Key.Enter);
        }
    }
}
