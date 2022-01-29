using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Q;

namespace SpecialEmailCampaignFed
{
	public partial class frmCampaignDetails : Form
	{
		CampaignData _data;
        EnterpriseFileSystem Efs;
		bool _newCampaign;
		bool _testMode;
		
		public frmCampaignDetails(CampaignData details, bool newCampaign, bool testMode, EnterpriseFileSystem efs)
		{
			_testMode = testMode;
            Efs = efs;
			InitializeComponent();
			_newCampaign = newCampaign;
			_data = details;
			campaignDataBindingSource.DataSource = details;
			if (details.CornerStone)
			{
				chkCornerStone.Checked = true;
				txtArc.ReadOnly = false;
			}

            if (details.IncludeAccountNumber)
                cboIncludeAccountNum.Checked = true;
			
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnRun_Click(object sender, EventArgs e)
		{
			if (txtEmailSubject.Text == string.Empty || txtFromAdd.Text == string.Empty || txtDataFile.Text == string.Empty || txtHtmlFile.Text == string.Empty || txtCommentTxt.Text == string.Empty)
			{
				MessageBox.Show("One of the required fields is not populated.  Please investigate and try again", "Missing Info", MessageBoxButtons.OK);
			}
			else if (chkCornerStone.Checked && txtArc.Text == string.Empty)
			{
				MessageBox.Show("The Arc text field is not populate and the CornerStone check box is selected.  Please investigate and try again", "Missing Info", MessageBoxButtons.OK);
			}
			else
			{
				Program.Run(_data, chkCornerStone.Checked);
				this.Close();
			}
		}

		private void btnBrowseDataFile_Click(object sender, EventArgs e)
		{
            openFileDialogDataFile.InitialDirectory = Efs.FtpFolder;
            openFileDialogDataFile.FileName = string.Empty;

			if (openFileDialogDataFile.ShowDialog() == DialogResult.OK) 
			{
				_data.DataFile = openFileDialogDataFile.FileName;
				txtDataFile.Text = _data.DataFile;
			}
		}

		private void btnBrowseHtml_Click(object sender, EventArgs e)
		{
            openFileDialogHTMLFile.InitialDirectory = Efs.FtpFolder;
            openFileDialogHTMLFile.FileName = string.Empty;

			if (openFileDialogHTMLFile.ShowDialog() == DialogResult.OK)
			{
				_data.HTMLFile = openFileDialogHTMLFile.FileName;
				txtHtmlFile.Text = _data.HTMLFile;
			}
		}

		private void btnTest_Click(object sender, EventArgs e)
		{
			if (txtEmailSubject.Text == string.Empty || txtFromAdd.Text == string.Empty ||  txtDataFile.Text == string.Empty || txtHtmlFile.Text == string.Empty || txtCommentTxt.Text == string.Empty)
			{
				MessageBox.Show("One of the required fields is not populated.  Please investigate and try again", "Missing Info", MessageBoxButtons.OK);
			}
			else if (chkCornerStone.Checked && txtArc.Text == string.Empty)
			{
				MessageBox.Show("The Arc text field is not populate and the CornerStone check box is selected.  Please investigate and try again", "Missing Info", MessageBoxButtons.OK);
			}
			else
			{
				Program.TestRun(_data, chkCornerStone.Checked);
				this.Close();
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			_data.EmailSubjectLine = txtEmailSubject.Text;
			_data.EmailFrom = txtFromAdd.Text;
			_data.CommentText = txtCommentTxt.Text;
			_data.Arc = txtArc.Text;
			_data.DataFile = txtDataFile.Text;
			_data.HTMLFile = txtHtmlFile.Text;
			DataAccess da = new DataAccess(_testMode);
            if (da.Save(_newCampaign, _data, chkCornerStone.Checked))
            {
                //Setting this to false so if the user saves again in this instance it will update instead of says
                _data.CampID = da.GetCampaignId(_data);
                _newCampaign = false;
            }
		}

		private void chkCornerStone_CheckedChanged(object sender, EventArgs e)
		{
            _data.CornerStone = chkCornerStone.Checked;
			if (chkCornerStone.Checked)
			{
				txtArc.ReadOnly = false;
			}
			else
			{
				txtArc.Text = string.Empty;
				txtArc.ReadOnly = true;
			}
		}

        private void cboIncludeAccountNum_CheckedChanged(object sender, EventArgs e)
        {
            _data.IncludeAccountNumber = cboIncludeAccountNum.Checked;
        }
	}
}
