using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace CSLTRPRFED
{
    public partial class LetterInput : Form
    {
        private List<string> LetterIds;
        public LetterInput(List<string> letterIds)
        {
            InitializeComponent();
            
            LetterIds = letterIds;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLetterId.Text))
            {
                MessageBox.Show("You did not enter a Letter Id.  Please try again.","No Letter Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!LetterIds.ContainsToUpper(txtLetterId.Text))
            {
                LetterIds.Add(txtLetterId.Text);

                lstLetterIds.DataSource = null;
                lstLetterIds.DataSource = LetterIds;
                btnOk.Visible = true;
            }
            else
            {
                MessageBox.Show("You have already entered that letter id.  Please try again.");
            }

            txtLetterId.Text = string.Empty;
        }

        private void lstLetterIds_DoubleClick(object sender, EventArgs e)
        {
            LetterIds.Remove(lstLetterIds.SelectedValue.ToString());
            lstLetterIds.DataSource = null;
            lstLetterIds.DataSource = LetterIds;

            if (LetterIds.Count < 1)
                btnOk.Visible = false;
        }

        private void txtLetterId_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnAdd_Click(null, null);
            }
        }
    }
}
