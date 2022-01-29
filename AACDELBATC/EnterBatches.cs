using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace AACDELBATC
{
    public partial class EnterBatches : Form
    {
        private List<MajorBatchInfo> BatchData;
        public EnterBatches(List<MajorBatchInfo> batchData)
        {
            InitializeComponent();
            BatchData = batchData;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!lstBatches.Items.Contains(txtMajorBatch.Text) && !txtMajorBatch.Text.IsNullOrEmpty())
            {
                lstBatches.Items.Add(txtMajorBatch.Text);
                txtMajorBatch.Text = string.Empty;
                btnContinue.Enabled = true;
            }
            else
                MessageBox.Show("The entered major batch is already in the list to be deleted or no batch number was provided.  Please try again.");
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (lstBatches.Items.Count < 1)
            {
                MessageBox.Show("You must enter at least one major batch.  Please try again.", "Enter a batch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (string item in lstBatches.Items)
            {
                BatchData.Add(new MajorBatchInfo() { MajorBatchToDelete = item });
            }

            DialogResult = DialogResult.OK;
        }

        private void lstBatches_DoubleClick(object sender, EventArgs e)
        {
            lstBatches.Items.RemoveAt(lstBatches.SelectedIndex);
            if (lstBatches.Items.Count < 1)
                btnContinue.Enabled = false;
        }

        private void txtMajorBatch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                btnAdd_Click(sender, null);
            }
        }
    }
}
