using System;
using System.Windows.Forms;
using Q;

namespace AACLOCKFED
{
	partial class BatchEntry : FormBase
	{
		private readonly Batches _batches;

		public BatchEntry(Batches batches)
		{
			InitializeComponent();
			_batches = batches;
			batchesBindingSource.DataSource = batches;
		}

		private void AddBatch(string batchNumber)
		{
			if (txtBatches.TextLength > 0) { txtBatches.AppendText(Environment.NewLine); }
			txtBatches.AppendText(batchNumber);
			txtNewBatch.Clear();
			txtNewBatch.Focus();
		}

		private void txtNewBatch_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) { AddBatch(txtNewBatch.Text); }
		}

		private void btnContinue_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(_batches.NewlineSeparatedValues)) { return; }
			_batches.Action = (radLock.Checked ? LockAndUnlock.Action.Lock : LockAndUnlock.Action.Unlock);
			this.DialogResult = DialogResult.OK;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			AddBatch(txtNewBatch.Text);
		}

		private void radLock_CheckedChanged(object sender, EventArgs e)
		{
			btnContinue.Enabled = (radLock.Checked || radUnlock.Checked);
		}

		private void radUnlock_CheckedChanged(object sender, EventArgs e)
		{
			btnContinue.Enabled = (radLock.Checked || radUnlock.Checked);
		}
	}//class
}//namespace
