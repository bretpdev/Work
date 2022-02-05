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

namespace TPERM
{
    public partial class ExistingReferenceSelection : Form
    {
        public PossibleReferenceMatch SelectedReference { get; set; }
        private List<PossibleReferenceMatch> Matches { get; set; }
        public ExistingReferenceSelection(List<PossibleReferenceMatch> matches)
        {
            InitializeComponent();
            Matches = matches;
            ReferelceSelection.DataSource = Matches;
            ReferelceSelection.Columns[4].Visible = false;
            ReferelceSelection.Columns[0].HeaderText = "Reference Num";
            ReferelceSelection.Columns[1].HeaderText = "First Name";
            ReferelceSelection.Columns[2].HeaderText = "Last Name";
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            int selectedCount = 0;
            int selectedIndex = -1;
            foreach (DataGridViewRow row in ReferelceSelection.Rows)
            {
                if ((bool)row.Cells[3].Value)
                {
                    selectedCount++;
                    selectedIndex = row.Index;
                }
            }

            if (selectedCount > 1)
            {
                Dialog.Error.Ok("You can only select 1 comaker that signed the form.");
                return;
            }
            if (selectedIndex != -1)
                SelectedReference = Matches[selectedIndex];
            DialogResult = DialogResult.OK;
        }
    }
}
