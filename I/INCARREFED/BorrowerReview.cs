using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace INCARREFED
{
    public partial class BorrowerReview : Form
    {
        private CommentData Data;
        public BorrowerReview(CommentData cData)
        {
            InitializeComponent();
            Data = cData;
            commentDataBindingSource.DataSource = Data;
            dtpReleaseDate.Value = Data.ReleaseDate;
            if (Data.IsBorrower)
                lblIsBwr.Text = "BORROWER";
            else
                lblIsBwr.Text = "STUDENT/ENDORSER";

            if (!Data.IsBorrower)
                txtEndAcctNum.Enabled = true;

            cmbSource.Items.AddRange(new string[] { "Prison/Jail Letter", "Prison/Jail Call", "Prison/Jail Web" });
            cmbState.Items.AddRange(DataAccess.GetStateCodes().ToArray());
            cmbState.SelectedItem = cData.FacilityState;
            lblActualAcctNum.Text = Data.BorrowerAccountNumber;
            dtpFollowUpDate.MinDate = DateTime.Now;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtEndAcctNum.Enabled && txtEndAcctNum.Text.IsNullOrEmpty())
            {
                MessageBox.Show("An Endorser/Student's account number is required.");
            }
            else if (txtFacName.Text.IsNullOrEmpty() || txtAddress.Text.IsNullOrEmpty() || txtFacCity.Text.IsNullOrEmpty() || cmbState.Text.IsNullOrEmpty() || txtZip.Text.IsNullOrEmpty() || txtPhoneNum.Text.IsNullOrEmpty())
            {
                MessageBox.Show("The Facility Name, Address, City, State,  Zip, and/or Phone Number is required.");
            }
            else if (cmbSource.Text.ToString().IsNullOrEmpty())
            {
                MessageBox.Show("A Source is required.");
            }
            else
            {
                if (dtpFollowUpDate.Value.Date == DateTime.Now.Date || dtpReleaseDate.Value.Date == DateTime.Now.Date)
                {
                    if (MessageBox.Show(string.Format("Are you sure the follow up date and or release date is {0:MM/dd/yyyy}?", DateTime.Now.Date),"Verify", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }

                Data.Source = cmbSource.Text.ToString();
                Data.FacilityName = txtFacName.Text;
                Data.FacilityAddress = txtAddress.Text;
                Data.FacilityCity = txtFacCity.Text;
                Data.FacilityState = cmbState.Text;
                Data.FacilityZip = txtZip.Text;
                Data.FacilityPhone = txtPhoneNum.Text;
                Data.InmateNumber = txtInmateNumber.Text;
                Data.ReleaseDate = dtpReleaseDate.Value;
                Data.FollowUpDate = dtpFollowUpDate.Value;
                Data.OtherInfo = txtOtherInfo.Text;

                DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
