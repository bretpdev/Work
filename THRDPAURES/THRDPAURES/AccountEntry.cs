using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;

namespace THRDPAURES
{
    public partial class AccountEntry : Form
    {
        public bool IsPowerOfAttorney { get; set; }
        public string BorrowerIdentifier { get; set; }
        public string ReferenceFirstName { get; set; }
        public string ReferenceLastName { get; set; }
        public string PoaExpirationDate { get; set; }

        public AccountEntry()
        {
            InitializeComponent();
            PoaExpirationDate = "";
        }

        private void POA_Click(object sender, EventArgs e)
        {
            IsPowerOfAttorney = true;
        }

        private void TPA_Click(object sender, EventArgs e)
        {
            IsPowerOfAttorney = false;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!POA.Checked && !TPA.Checked)
            {
                MessageBox.Show("Please review the following errors: \n" + "-You must select Power of Attorney or Third Party Authorization.", "Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (RefFirstName.Text.IsNullOrEmpty() || RefLastName.Text.IsNullOrEmpty() || AccountIdentifier.Text.IsNullOrEmpty())
            {
                MessageBox.Show("You must enter an Account Number/ SSN,  Reference First Name, and Last Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            BorrowerIdentifier = AccountIdentifier.Text;
            ReferenceFirstName = RefFirstName.Text;
            ReferenceLastName = RefLastName.Text;
            DialogResult = DialogResult.OK;
        }

        private void POA_CheckedChanged(object sender, EventArgs e)
        {
            if (POA.Checked)
            {
                using (ExpirationDatePoa exDate = new ExpirationDatePoa())
                {
                    exDate.ShowDialog();
                    PoaExpirationDate = exDate.ExpirationDate == null ? "" : exDate.ExpirationDate.Replace("/", "");
                    IsPowerOfAttorney = true;
                }
            }
        }

        private void TPA_CheckedChanged(object sender, EventArgs e)
        {
            if (TPA.Checked)
            {
                PoaExpirationDate = "";
                IsPowerOfAttorney = false;
            }
        }
    }
}
