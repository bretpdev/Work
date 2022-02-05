using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace MauiDUDE
{
    public partial class Phone : UserControl
    {
        public Phone()
        {
            InitializeComponent();

            comboBoxMobile.SelectedItem = "U";
        }

        private void textBoxPhone1_TextChanged(object sender, EventArgs e)
        {
            if(textBoxPhone1.TextLength == 3)
            {
                textBoxPhone2.Focus();
            }
        }

        private void textBoxPhone2_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPhone1.TextLength == 3)
            {
                textBoxPhone2.Focus();
            }
        }

        private void textBoxPhone3_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPhone3.TextLength == 4)
            {
                textBoxExtension.Focus();
            }
        }

        public bool ValidUserInput()
        {
            //if any of the phone fields have data then validate the data
            if(textBoxPhone1.TextLength != 0 || textBoxPhone2.TextLength != 0 || textBoxPhone3.TextLength != 0)
            {
                //be sure that the phone is only numeric, has ten digits, and doesn't have a 0 or a 1 in the 4th digit place
                if(textBoxPhone1.TextLength != 3 || textBoxPhone2.TextLength != 3 || textBoxPhone3.TextLength != 4)
                {
                    //give the user an error message
                    WhoaDUDE.ShowWhoaDUDE("The phone number must be numeric, have 10 digits, and not have a '1' or '0' in the 1st or 4th position.", "Invalid Phone Number", true);
                    //enable phone number boxes and buttons
                    EnableControls();
                    textBoxPhone1.Focus();
                    return false;
                }
                else
                {
                    //check if the phone is valid numeric format
                    if(!textBoxPhone1.Text.IsNumeric() || !textBoxPhone2.Text.IsNumeric() || !textBoxPhone3.Text.IsNumeric() || (textBoxExtension.Text != "" && !textBoxExtension.Text.IsNumeric())
                        || textBoxPhone2.Text.Substring(0,1) == "0" || textBoxPhone2.Text.Substring(0,1) == "1" || textBoxPhone1.Text.Substring(0,1) == "0" || textBoxPhone1.Text.Substring(0,1) == "1")
                    {
                        WhoaDUDE.ShowWhoaDUDE("The phone number must be numeric, have 10 digits, and not have a '1' or '0' in the 1st or 4th position.", "Invalid Phone Number", true);
                        //enable phone number boxes and buttons
                        EnableControls();
                        textBoxPhone1.Focus();
                        return false;
                    }
                }
            }
            return true;
        }

        //enables controls on control
        private void EnableControls()
        {
            textBoxPhone1.Enabled = true;
            textBoxPhone2.Enabled = true;
            textBoxPhone3.Enabled = true;
            textBoxExtension.Enabled = true;
            comboBoxMobile.Enabled = true;
        }
    }
}
