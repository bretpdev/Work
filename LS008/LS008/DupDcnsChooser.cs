using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LS008
{
    public partial class DupDcnsChooser : Form
    {
        public List<DupDcns> DupDcns { get; set; }
        public DupDcnsChooser(List<DupDcns> possibleDups, string curDcn)
        {
            InitializeComponent();
            CurDcn.Text = curDcn;
            DupDcns = new List<DupDcns>();
            DCNS.DataSource = possibleDups;
            DCNS.Columns[0].Visible = false;
            DCNS.Columns[1].Visible = false;

        }

        private void CDN_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(CurDcn.Text);
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DCNS.Rows)
                DupDcns.Add(new DupDcns() {TaskControlNumber = row.Cells[0].Value.ToString(), Dcn = row.Cells[2].Value.ToString(), Selected = (bool)row.Cells[3].Value });

            DialogResult = DialogResult.OK;
        }
    }
}
