using System;
using System.Windows.Forms;
using SCHDEMOUP.Data;

namespace SCHDEMOUP
{
    partial class frmDemoData : Form
    {
        public frmDemoData(bool testMode, LPSCSchoolData schoolData, DepartmentCodes codes)
        {
            InitializeComponent();
            DataAccess da = new DataAccess(testMode);
            cboState.Items.AddRange(da.GetListOfStates());
            schoolData.SchoolCode = codes.SchoolCode;
            schoolData.CompassDepartment = codes.CompassDepartment;
            schoolData.OneLinkDepartment = codes.OnelinkDepartment;
            schoolData.Date = codes.SubmitDate;
            lPSCSchoolDataBindingSource.DataSource = schoolData;
        }

        /// <summary>
        /// Validates the data on the form and then returns dialogresult of OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ValidateFields()) { this.DialogResult = DialogResult.OK; }
        }

        /// <summary>
        /// Validates the data on the form and then returns dialogresult of YES
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnComplete_Click(object sender, EventArgs e)
        {
            if (ValidateFields()) { this.DialogResult = DialogResult.Yes; }
        }

        /// <summary>
        /// Checks all the input data for missing data or too much data.
        /// </summary>
        /// <returns></returns>
        public bool ValidateFields()
        {
            if ((txtFirstName.Text != string.Empty && txtLastName.Text == string.Empty) || (txtFirstName.Text == string.Empty && txtLastName.Text != string.Empty))
            {
                MessageBox.Show("You must enter both contact names.", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if ((txtForeignState.Text != string.Empty && txtForeignCountry.Text == string.Empty) || (txtForeignState.Text == string.Empty && txtForeignCountry.Text != string.Empty))
            {
                MessageBox.Show("Both foreign state and country must be entered if either is entered.", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if (txtForeignState.Text == string.Empty && cboState.Text == string.Empty)
            {
                MessageBox.Show("You must enter either a domestic state code or the foreign state and country.", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if ((txtPhone.Text != string.Empty || txtExtension.Text != string.Empty || txtFax.Text != string.Empty) && (txtPhoneIC.Text != string.Empty || txtPhoneCNY.Text != string.Empty || txtPhoneCity.Text != string.Empty || txtPhoneLocal.Text != string.Empty || txtForeignExt.Text != string.Empty || txtFaxIC.Text != string.Empty || txtFaxCNY.Text != string.Empty || txtFaxCity.Text != string.Empty || txtFaxLocal.Text != string.Empty))
            {
                MessageBox.Show("You cannot have both domestic and federal phone numbers", "Fix Phone Numbers", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if ((txtPhone.Text == string.Empty && txtExtension.Text == string.Empty && txtFax.Text == string.Empty) && (txtPhoneIC.Text == string.Empty && txtPhoneCNY.Text == string.Empty && txtPhoneCity.Text == string.Empty && txtPhoneLocal.Text == string.Empty && txtForeignExt.Text == string.Empty && txtFaxIC.Text == string.Empty && txtFaxCNY.Text == string.Empty && txtFaxCity.Text == string.Empty && txtFaxLocal.Text == string.Empty))
            {
                MessageBox.Show("A phone number is required.", "Phone Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if (cboState.Text != string.Empty && (txtForeignState.Text != string.Empty || txtForeignCountry.Text != string.Empty))
            {
                MessageBox.Show("You cannot have both domestic and foreign addresses", "Fix Address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if (cboState.Text == string.Empty && (txtForeignState.Text == string.Empty && txtForeignCountry.Text == string.Empty))
            {
                MessageBox.Show("A state is required", "State Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else { return true; }
        }

    }//Class
}//Namespace