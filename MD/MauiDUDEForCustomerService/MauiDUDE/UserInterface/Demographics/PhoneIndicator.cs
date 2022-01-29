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

namespace MauiDUDE
{

    public partial class PhoneIndicator : Form
    {
        private Borrower Borrower { get; set; }
        private string Home { get; set; }
        private string Other { get; set; }
        private string Other2 { get; set; }


        public PhoneIndicator(Borrower borrower, string home, string other, string other2)
        {
            InitializeComponent();
            Borrower = borrower;
            Home = home;
            Other = other;
            Other2 = other2;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(checkBoxHome.Checked)
            {
                Borrower.ContactPhoneNumber = Home;
                DialogResult = DialogResult.OK;
            }
            else if(checkBoxOther.Checked)
            {
                Borrower.ContactPhoneNumber = Other;
                DialogResult = DialogResult.OK;
            }
            else if(checkBoxOther2.Checked)
            {
                Borrower.ContactPhoneNumber = Other2;
                DialogResult = DialogResult.OK;
            }
            else if(checkBoxManualInput.Checked)
            {
                if(textBoxManualInput.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("Pelase enter a manual phone number.");
                    return;
                }
                Borrower.ContactPhoneNumber = textBoxManualInput.Text;
                DialogResult = DialogResult.OK;
            }
        }

        private void PhoneIndicator_Load(object sender, EventArgs e)
        {
            if(Home == "--")
            {
                textBoxHome.Text = "";
            }
            else if(Home != "")
            {
                textBoxHome.Text = Home;
                checkBoxHome.Enabled = true;
            }

            if(Other == "--")
            {
                textBoxOther.Text = "";
            }
            else if(Other != "")
            {
                textBoxOther.Text = Other;
                checkBoxOther.Enabled = true;
            }

            if(Other2 == "--")
            {
                textBoxOther2.Text = "";
            }
            else if(Other2 != "")
            {
                textBoxOther2.Text = Other2;
                checkBoxOther2.Enabled = true;
            }
        }

        private void checkBoxHome_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxHome.Checked)
            {
                checkBoxOther.Checked = false;
                checkBoxOther2.Checked = false;
                checkBoxManualInput.Checked = false;
                buttonOK.Enabled = true;
            }
        }

        private void checkBoxOther_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOther.Checked)
            {
                checkBoxHome.Checked = false;
                checkBoxOther2.Checked = false;
                checkBoxManualInput.Checked = false;
                buttonOK.Enabled = true;
            }
        }

        private void checkBoxOther2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOther2.Checked)
            {
                checkBoxHome.Checked = false;
                checkBoxOther.Checked = false;
                checkBoxManualInput.Checked = false;
                buttonOK.Enabled = true;
            }
        }

        private void checkBoxManualInput_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxManualInput.Checked)
            {
                checkBoxHome.Checked = false;
                checkBoxOther.Checked = false;
                checkBoxOther2.Checked = false;
                buttonOK.Enabled = true;
            }
        }
    }
}
