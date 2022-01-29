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

namespace ENRLINFO
{
    public partial class EnrollmentInformation : Form
    {
        public EnrollmentData data = new EnrollmentData();

        public EnrollmentInformation()
        {
            InitializeComponent();
        }

        private void PopulateData(bool evrHistoryContainsInformation)
        {
            data = new EnrollmentData();
            data.AccountIdentifier = accountIdentifierTextBox.Text;
            data.SchoolCode = schoolCodeTextBox.Text;
            if(nsldsRadioButton.Checked)
            {
                data.Source = "I";
                data.SourceText = "NSLDS";
            }
            else if(nchRadioButton.Checked)
            {
                //data.Source = EnrollmentData.EnrollmentSource.NCH;
                data.Source = "M";
                data.SourceText = "NCH";
            }
            else //if(schoolRadioButton.Checked)
            {
                //data.Source = EnrollmentData.EnrollmentSource.School;
                data.Source = "D";
                data.SourceText = "School";
            }
            data.EVRHistoryContainsEnrollmentInformation = evrHistoryContainsInformation;
        }

        private bool Validate()
        {
            if(accountIdentifierTextBox.TextLength != 9 && accountIdentifierTextBox.TextLength != 10)
            {
                Dialog.Error.Ok("You must enter a valid 10 digit account number or 9 digit SSN.");
                return false;
            }

            if(schoolCodeTextBox.TextLength != 8)
            {
                Dialog.Error.Ok("You must enter a valid 8 digit school code.");
                return false;
            }

            if(!nsldsRadioButton.Checked && !nchRadioButton.Checked && !schoolRadioButton.Checked)
            {
                Dialog.Error.Ok("You must select a source for the enrollment information.");
                return false;
            }

            return true;
        }

        private void yesButton_Click(object sender, EventArgs e)
        {
            if(!Validate())
            {
                return;
            }

            PopulateData(true);
            DialogResult = DialogResult.Yes;
        }

        private void noButton_Click(object sender, EventArgs e)
        {
            if (!Validate())
            {
                return;
            }

            PopulateData(false);
            DialogResult = DialogResult.No;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
