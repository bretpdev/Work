using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DayFileRec
{
	partial class frmStatus : Form
	{
		public frmStatus()
		{
			InitializeComponent();
			Reconciler.NoteworthyEvent += Reconciler_NoteworthyEvent;
		}

		#region Event handlers
		private void btnAction_Click(object sender, System.EventArgs e)
		{
			if (btnAction.Text == "Start")
			{
				Run();
			}
			else
			{
				this.Close();
			}
		}

		private void dtpDownloadDate_ValueChanged(object sender, System.EventArgs e)
		{
			//Don't allow future dates.
			if (dtpDownloadDate.Value > DateTime.Now) { dtpDownloadDate.Value = DateTime.Now; }
		}

		private void radArchive_CheckedChanged(object sender, System.EventArgs e)
		{
			//Enable date selection only when searching Archive.
			lblDownloadDate.Enabled = radArchive.Checked;
			dtpDownloadDate.Enabled = radArchive.Checked;
		}

		private void radFtp_CheckedChanged(object sender, EventArgs e)
		{
			//Don't use past dates when searching FTP.
			if (radFtp.Checked) { dtpDownloadDate.Value = DateTime.Now; }
		}

		private void Reconciler_NoteworthyEvent(object sender, Reconciler.NoteworthyEventArgs e)
		{
			txtStatus.AppendText(string.Format("{0}{1}", e.Message, Environment.NewLine));
			Application.DoEvents();
		}
		#endregion Event handlers

		private void Run()
		{
			Cursor = Cursors.WaitCursor;
			txtStatus.Cursor = Cursor;
			btnAction.Enabled = false;
			try
			{
				string sasFolder = (radArchive.Checked ? DataAccess.SasFolder.ARCHIVE : DataAccess.SasFolder.FTP);
				txtStatus.Clear();
				Reconciler.Reconcile(dtpDownloadDate.Value, sasFolder);
			}
			finally
			{
				btnAction.Text = "Close";
				btnAction.Enabled = true;
				Cursor = Cursors.Default;
				txtStatus.Cursor = Cursor;
			}
		}//Run()
	}//class
}//namespace
