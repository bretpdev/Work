using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace SPLTRCAMP
{
    public partial class Entry : FormBase
    {

        public enum Recipient
        {
            Borrower = DocumentHandling.Barcode2DLetterRecipient.lrBorrower,
            Reference = DocumentHandling.Barcode2DLetterRecipient.lrReference,
            Other = DocumentHandling.Barcode2DLetterRecipient.lrOther
        }

        public enum PageCountAndDestination
        {
            One = DocumentHandling.DestinationOrPageCount.Page1,
            Two = DocumentHandling.DestinationOrPageCount.Page2,
            Three = DocumentHandling.DestinationOrPageCount.Page3,
            Four = DocumentHandling.DestinationOrPageCount.Page4,
            DocumentServices = DocumentHandling.DestinationOrPageCount.DocServices,
            BusinessUnit = DocumentHandling.DestinationOrPageCount.BusinessUnit
        }

        private CampaignData _data;

        /// <summary>
        /// Default Constructor (DO NOT USE).
        /// </summary>
        public Entry()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Entry(CampaignData data)
        {
            InitializeComponent();
            _data = data;
            cmbLetterRecipient.DataSource = Enum.GetValues(typeof(Recipient));
            cmbPageCountAndDestination.DataSource = Enum.GetValues(typeof(PageCountAndDestination));
            campaignDataBindingSource.DataSource = data;
        }

        private void btnDataFileBrowse_Click(object sender, EventArgs e)
        {
            ofdBrowser.InitialDirectory = @"X:\PADD";
            ofdBrowser.ShowDialog();
            txtMergeFile.Text = ofdBrowser.FileName;
        }

        private void btnLetterFileBrowse_Click(object sender, EventArgs e)
        {
            ofdBrowser.InitialDirectory = @"T:\";
            ofdBrowser.ShowDialog();
            txtLetterFile.Text = ofdBrowser.FileName;
        }

        private void txtActionCode_Enter(object sender, EventArgs e)
        {
            txtActionCode.SelectAll();
        }

        private void txtARC_Enter(object sender, EventArgs e)
        {
            txtARC.SelectAll();
        }

        private void chkOneLINKComments_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOneLINKComments.Checked)
            {
                txtActionCode.Enabled = true;
            }
            else
            {
                txtActionCode.Enabled = false;
                txtActionCode.Text = "Action Code";
            }
        }

        private void chkCompassComments_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCompassComments.Checked)
            {
                txtARC.Enabled = true;
            }
            else
            {
                txtARC.Enabled = false;
                txtARC.Text = "ARC";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to run the campaign?", "Live Campaign Run", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (DataIsValid())
                {
                    _data.ActionSelected = CampaignData.Action.Run;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please ensure that you are logged into the test region.","Test Campaign Run",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            if (DataIsValid())
            {
                _data.ActionSelected = CampaignData.Action.Test;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        //check if data is valid
        private bool DataIsValid()
        {
            if (txtMergeFile.TextLength == 0)
            {
                MessageBox.Show("You must provide the path and name for the data file that the merge process will use.","Data File Path Needed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
            else if (txtLetterFile.TextLength == 0)
            {
                MessageBox.Show("You must provide the path and name for the letter file that the merge process will use.", "Letter File Path Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (chkOneLINKComments.Checked)
            {
                if (txtActionCode.TextLength != 5)
                {
                    MessageBox.Show("You must provide an action code if you want comments documented on OneLINK.", "Action Code Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            if (chkCompassComments.Checked)
            {
                if (txtARC.TextLength != 5)
                {
                    MessageBox.Show("You must provide an ARC if you want comments documented on Compass.", "ARC Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void txtActionCode_Click(object sender, EventArgs e)
        {
            txtActionCode.SelectAll();
        }

        private void txtARC_Click(object sender, EventArgs e)
        {
            txtARC.SelectAll();
        }

    }
}
