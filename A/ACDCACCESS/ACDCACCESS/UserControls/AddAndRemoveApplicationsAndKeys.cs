using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ACDCAccess
{
	partial class AddAndRemoveApplicationsAndKeys : BaseMainTabUserControl
	{
		private readonly DataAccess _dataAccess;

		public AddAndRemoveApplicationsAndKeys()
			: base()
		{
			InitializeComponent();
		}

		public AddAndRemoveApplicationsAndKeys(bool testMode)
			: base(testMode)
		{
			InitializeComponent();
			_dataAccess = new DataAccess(testMode);
			List<string> types = _dataAccess.GetKeyTypes().ToList();
			types.Insert(0, DataAccess.COMBOBOX_DEFAULT_SELECTION);
			cmbType.DataSource = types;
			List<string> applications = _dataAccess.GetApplications().ToList();
			applications.Insert(0, DataAccess.COMBOBOX_DEFAULT_SELECTION);
			cmbApplication.DataSource = applications;
		}

		private void btnAddKeyToSystem_Click(object sender, EventArgs e)
		{
			if (cmbApplication.SelectedItem.ToString() == DataAccess.COMBOBOX_DEFAULT_SELECTION || txtKey.TextLength == 0 ||
				cmbType.SelectedItem.ToString() == DataAccess.COMBOBOX_DEFAULT_SELECTION || txtDescription.TextLength == 0)
			{
				MessageBox.Show("You must provide all fields to successfully add a key to the system.", "All Data Needed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try
			{
				Key k = new Key();
				k.Application = cmbApplication.SelectedItem.ToString();
				k.Name = txtKey.Text;
				k.Type = cmbType.SelectedItem.ToString();
				k.Description = txtDescription.Text;
				_dataAccess.AddKey(k);
				MessageBox.Show("The provided key has been added to the specified system.", "Key Added", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				//update user interface
				txtKey.Clear();
				txtDescription.Clear();
				pnlExistingKeys.Controls.Clear();
				IEnumerable<Key> existingKeys = _dataAccess.GatherExistingKeysForApplication(cmbApplication.SelectedItem.ToString());
				bool evenRow = true;
				foreach (Key ek in existingKeys)
				{
					pnlExistingKeys.Controls.Add(new SummaryKeyInfo(ek));
					//alternate colors
					evenRow = !evenRow;
					if (evenRow) { pnlExistingKeys.Controls[pnlExistingKeys.Controls.Count - 1].BackColor = Color.WhiteSmoke; }
				}
			}
			catch (KeyAlreadyExistsException)
			{
				MessageBox.Show("The key provided already exists for the specified system.", "Key Already Exists On System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void cmbSystem_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbApplication.SelectedItem.ToString() == DataAccess.COMBOBOX_DEFAULT_SELECTION)
			{
				pnlExistingKeys.Controls.Clear();
			}
			else
			{
				pnlExistingKeys.Controls.Clear();
				IEnumerable<Key> existingKeys = _dataAccess.GatherExistingKeysForApplication(cmbApplication.SelectedItem.ToString());
				bool evenRow = true;
				foreach (Key k in existingKeys)
				{
					pnlExistingKeys.Controls.Add(new SummaryKeyInfo(k));
					//alternate colors
					evenRow = !evenRow;
					if (evenRow) { pnlExistingKeys.Controls[pnlExistingKeys.Controls.Count - 1].BackColor = Color.WhiteSmoke; }
				}
			}
		}
	}//class
}//namespace
