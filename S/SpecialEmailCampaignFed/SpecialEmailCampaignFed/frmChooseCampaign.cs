using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Q;

namespace SpecialEmailCampaignFed
{
	public partial class frmChooseCampaign : Form
	{
		bool TestMode;
        EnterpriseFileSystem Efs;

		public frmChooseCampaign(IEnumerable<CampaignData> data, bool testMode)
		{
			InitializeComponent();
			TestMode = testMode;
            Efs = new EnterpriseFileSystem(TestMode, ScriptSessionBase.Region.CornerStone);

			foreach (CampaignData emailData in data)
			{
				string[] items = { emailData.EmailSubjectLine, emailData.DataFile, emailData.HTMLFile };
				lvwExsistingCamp.Items.Add(emailData.CampID.ToString()).SubItems.AddRange(items);
			}
		}

		private void btnOpen_Click(object sender, EventArgs e)
		{
			if (lvwExsistingCamp.SelectedItems.Count == 0)
			{
				MessageBox.Show("You must select a campaign", "Select Campaign", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			else
			{
				int campId = Convert.ToInt32(lvwExsistingCamp.SelectedItems[0].Text);
				
				DataAccess da = new DataAccess(TestMode);
                CampaignData selectedData = da.GetCampaignData(campId).SingleOrDefault();
				
				frmCampaignDetails details = new frmCampaignDetails(selectedData, false, TestMode, Efs);
                this.Dispose();
				details.ShowDialog();
			}
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			frmCampaignDetails frmDetails = new frmCampaignDetails(new CampaignData(), true, TestMode, Efs);
			frmDetails.ShowDialog();
			this.Close();
		}
	}
}