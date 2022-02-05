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
using Uheaa.Common.WinForms;

namespace DPALETTERS
{
    public partial class BorrowerInput : Form
    {
        private bool Optional { get; set; } = false;
        private string LabelText { get; set; } = "Enter an SSN or account number, or click Cancel to quit.";
        public BorrowerInput(bool optional, string overrideText)
        {
            InitializeComponent();
            Optional = optional;
            if(!overrideText.IsNullOrEmpty())
            {
                LabelText = overrideText;
            }
            label.Text = LabelText;
        }
        
        private bool ValidateInput()
        {
            //if the input is optional, return true
            if(Optional && accountIdentifierTextBox.TextLength == 0)
            {
                return true;
            }

            if (!AccountIdentifierTextBox.ValidateInput(accountIdentifierTextBox.Text))
            {
                Dialog.Warning.Ok("The value provided is not a valid account number or ssn","Bad Input");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e) 
        {
            if (ValidateInput())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
