using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace EA80Reconciliation.UserControls
{
    public partial class EA27Control : UserControl
    {
		ProcessLogData LogData;
        OpenFolderDialog ofd = new OpenFolderDialog();
		OpenFileDialog fileDialog = new OpenFileDialog();

		public EA27Control()
		{
			InitializeComponent();
			EA27LocationText.DataBindings.Add(new Binding("Text", Properties.Settings.Default, "EA27Location"));
			EA80FolderLocationText.DataBindings.Add(new Binding("Text", Properties.Settings.Default, "EA80FolderLocation"));
		}

        public EA27Control(ProcessLogData logData) : this()
        {
			LogData = logData;
        }

		private void StartReconciliationButton_Click(object sender, EventArgs e)
		{
			SettingsGroup.Enabled = StartReconciliationButton.Enabled = false;
			//Verify that file locations are valid to use.
			if (FileSystemHelper.CheckFile(EA27LocationText.Text) && FileSystemHelper.CheckDirectory(EA80FolderLocationText.Text))
			{
				var comparer = new EA80Reconciliation(LogData);
				comparer.Reconcile(EA27LocationText.Text, EA80FolderLocationText.Text, LoanSaleDatePicker.Value);
			}

			SettingsGroup.Enabled = StartReconciliationButton.Enabled = true;
		}

		private void EA27LocationBrowse_Click(object sender, EventArgs e)
		{
			if (fileDialog.ShowDialog() == DialogResult.OK)
				Properties.Settings.Default.EA27Location = fileDialog.FileName;
		}

		private void EA80FolderLocationBrowse_Click(object sender, EventArgs e)
		{
			if (ofd.ShowDialog(this.Handle, false) == DialogResult.OK)
				Properties.Settings.Default.EA80FolderLocation = ofd.Folder;
		}
    }
}
