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

namespace DPLTRS.NotificationOfGarnishmentSuspension
{
    public partial class NOTSUSP : Form
    {
        public NOTSUSP()
        {
            InitializeComponent();
        }

        public NOTSUSPInfo GetInfo()
        {
            NOTSUSPInfo info = new NOTSUSPInfo();           

            if (ssnTextBox.Text != null && ssnTextBox.Text.Length == 9)
            {
                info.Ssn = ssnTextBox.Text;
            }

            return info;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            bool haveSsn = false;
            
            if(ssnTextBox.Text != null && ssnTextBox.Text.Length == 9)
            {
                haveSsn = true;
            }

            if(haveSsn)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                Dialog.Error.Ok("Make sure the SSN is 9 digits and you have selected a reason.");
            }
            
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
    public class NOTSUSPInfo
    {
        public string Ssn { get; set; }
    }
}
