using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACHSETUPFD
{
    /// <summary>
    /// Create a dialog to let the user select the desired endorser
    /// in the event that there exists more than one endorser
    /// </summary>
    public partial class SelectEndorser : Form
    {
        public List<DataClasses.EndorserRecord> recs;

        public SelectEndorser(ref List<DataClasses.EndorserRecord> recs)
        {
            InitializeComponent();
            this.recs = recs;
            foreach (var rec in recs)
            {
                string[] arr = new string[2];
                arr[0] = rec.DM_PRS_LST;
                arr[1] = rec.DM_PRS_1;
                ListViewItem itm = new ListViewItem(arr);
                listView1.Items.Add(itm);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listItemSelected())
            {
                recs = new List<DataClasses.EndorserRecord> { recs[listView1.SelectedIndices[0]] };
                DialogResult = DialogResult.OK;
            }
        }

        private bool listItemSelected()
        {
            if(listView1.SelectedItems.Count == 1)
            {
                return true;
            }
            else if(listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Please select an endorser.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Please select only one endorser.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return false;
        }
    }
}
