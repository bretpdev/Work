using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ACDCAccess
{
	partial class Research : BaseMainTabUserControl
	{
		private readonly DataAccess _dataAccess;

		public Research()
			: base()
		{
			InitializeComponent();
		}

		public Research(bool testMode)
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
			cmbFieldsToSearch.SelectedIndex = 0;
		}

		private void LoadResearchResults()
		{
			pnlResults.Controls.Clear();
			string application = "";
			string keyType = "";
			string searchKey = "";
			string searchFields = "";
			if (cmbApplication.SelectedItem != null && cmbType.SelectedItem != null)
			{
				application = cmbApplication.SelectedItem.ToString();
				keyType = cmbType.SelectedItem.ToString();
				searchKey = txtKeyWordSearch.Text;
				searchFields = cmbFieldsToSearch.Text;
			}
			IEnumerable<Key> researchResultKeys = _dataAccess.SearchForKeys(application, keyType, searchKey, searchFields);
			bool evenRow = true;
			foreach (Key k in researchResultKeys)
			{
				pnlResults.Controls.Add(new ResearchResult(k));
				//alternate colors
				evenRow = !evenRow;
				if (evenRow) { pnlResults.Controls[pnlResults.Controls.Count - 1].BackColor = Color.WhiteSmoke; }
			}
		}

		private void Research_Load(object sender, EventArgs e)
		{
			LoadResearchResults();
		}

		private void txtKeyWordSearch_TextChanged(object sender, EventArgs e)
		{
			LoadResearchResults();
		}

		private void cmbSystem_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadResearchResults();
		}

		private void cmbFieldsToSearch_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadResearchResults();
		}
	}//class
}//namespace
