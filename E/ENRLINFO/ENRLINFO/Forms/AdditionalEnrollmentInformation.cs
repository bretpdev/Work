using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace ENRLINFO.Forms
{
    public partial class AdditionalEnrollmentInformation : Form
    {
        List<string> EnrollmentStatus = new List<string>() { "F", "H", "L", "W", "G", "X", "D", "A" };
        public AdditionalEnrollmentData Data = new AdditionalEnrollmentData();

        public AdditionalEnrollmentInformation()
        {
            InitializeComponent();

            enrollementStatusComboBox.DataSource = EnrollmentStatus;
        }

        private void GetData()
        {
            Data = new AdditionalEnrollmentData();
            Data.EnrollmentStatus = enrollementStatusComboBox.SelectedItem.ToString();
            Data.StatusEffectiveDate = statusEffectiveMaskedDateTextBox.Text.ToDate();
            Data.SchoolCertificationDate = schoolCertMaskedDateTextBox.Text.ToDate();
            Data.AGD = agdMaskedDateTextBox.Text.ToDate();
        }

        private bool Validate()
        {
            if(enrollementStatusComboBox.SelectedIndex < 0)
            {
                Dialog.Error.Ok("You must select a enrollment status.");
                return false;
            }

            if(!statusEffectiveMaskedDateTextBox.Text.ToDateNullable().HasValue)
            {
                Dialog.Error.Ok("You must add a valid status effective date.");
                return false;
            }

            if(statusEffectiveMaskedDateTextBox.Text.ToDate() < new DateTime(1970, 1, 1))
            {
                Dialog.Error.Ok("Status effective date must be a date after 01/01/1970.");
                return false;
            }

            if (!schoolCertMaskedDateTextBox.Text.ToDateNullable().HasValue)
            {
                Dialog.Error.Ok("You must add a valid school certification date.");
                return false;
            }

            if (schoolCertMaskedDateTextBox.Text.ToDate() < new DateTime(1970, 1, 1))
            {
                Dialog.Error.Ok("School certification date must be a date after 01/01/1970.");
                return false;
            }

            if (!agdMaskedDateTextBox.Text.ToDateNullable().HasValue)
            {
                Dialog.Error.Ok("You must enter a valid anticipated graduation date(AGD).");
                return false;
            }

            if (agdMaskedDateTextBox.Text.ToDate() < new DateTime(1970, 1, 1))
            {
                Dialog.Error.Ok("The anticipated graduation date must be a date after 01/01/1970.");
                return false;
            }

            return true;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if(!Validate())
            {
                return;
            }

            GetData();
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
