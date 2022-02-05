using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace I1I2CHREV
{
    public partial class ManualForm : Form
    {
        public bool LetterWasSent
        {
            get
            {
                return LetterSentButton.Checked;
            }
        }

        public ManualForm()
        {
            InitializeComponent();
        }

        bool calledByCode = false;
        private void SchoolButtonPushed()
        {
            if (SchoolNotFoundButton.Checked)
            {
                Step2Box.Enabled = false;
                ContinueButton.Enabled = true; 
            }
            else if (SchoolFoundButton.Checked)
            {
                Step2Box.Enabled = true;
                ContinueButton.Enabled = false;
            }
            calledByCode = true;
            LetterSentButton.Checked = LetterNotSentButton.Checked = false;
            calledByCode = false;
        }

        private void SchoolNotFoundButton_CheckedChanged(object sender, EventArgs e)
        {
            SchoolFoundButton.Checked = !SchoolNotFoundButton.Checked;
            SchoolButtonPushed();
        }

        private void SchoolFoundButton_CheckedChanged(object sender, EventArgs e)
        {
            SchoolNotFoundButton.Checked = !SchoolFoundButton.Checked;
            SchoolButtonPushed();
        }

        private void LetterNotSentButton_CheckedChanged(object sender, EventArgs e)
        {
            if (calledByCode) return;
            LetterSentButton.Checked = !LetterNotSentButton.Checked;
            ContinueButton.Enabled = true;
        }

        private void LetterSentButton_CheckedChanged(object sender, EventArgs e)
        {
            if (calledByCode) return;
            LetterNotSentButton.Checked = !LetterSentButton.Checked;
            ContinueButton.Enabled = true;
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
