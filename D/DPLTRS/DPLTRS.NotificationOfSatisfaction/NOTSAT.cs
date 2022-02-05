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

namespace DPLTRS.NotificationOfSatisfaction
{
    public partial class NOTSAT : Form
    {
        public NOTSAT()
        {
            InitializeComponent();
        }

        public NOTSATInfo GetInfo()
        {
            NOTSATInfo info = new NOTSATInfo();
            if (radioButton1.Checked)
            {
                info.Reason = NOTSATReason.SmallBalance;
            }
            else if (radioButton2.Checked)
            {
                info.Reason = NOTSATReason.EmployerRequest;
            }
            else if (radioButton3.Checked)
            {
                info.Reason = NOTSATReason.Bankruptcy;
            }
            else if (radioButton4.Checked)
            {
                info.Reason = NOTSATReason.Management;
            }

            if (ssnTextBox.Text != null && ssnTextBox.Text.Length == 9)
            {
                info.Ssn = ssnTextBox.Text;
            }

            return info;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            bool haveReason = false;
            bool haveSsn = false;

            if (radioButton1.Checked)
            {
                haveReason = true;
            }
            else if(radioButton2.Checked)
            {
                haveReason = true;
            }
            else if(radioButton3.Checked)
            {
                haveReason = true;
            }
            else if(radioButton4.Checked)
            {
                haveReason = true;
            }
            
            if(ssnTextBox.Text != null && ssnTextBox.Text.Length == 9)
            {
                haveSsn = true;
            }

            if(haveReason && haveSsn)
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

        public enum NOTSATReason
        {
            SmallBalance,
            EmployerRequest,
            Bankruptcy,
            Management
        }
    }
    public class NOTSATInfo
    {
        public string Ssn { get; set; }
        public NOTSAT.NOTSATReason? Reason { get; set; } = null;
    }
}
