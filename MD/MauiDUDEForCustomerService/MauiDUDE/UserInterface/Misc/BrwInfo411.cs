using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public partial class BrwInfo411 : Form
    {
        public BrwInfo411()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(Borrower borrower, bool displayRegardlessOfResults)
        {
            labelSSN.Text = borrower.SSN;
            labelName.Text = borrower.FullName;

            //Change colors
            textBoxComment.ForeColor = ForeColor;

            if(borrower.Info411.Length > 0)
            {
                textBoxComment.Text = borrower.Info411;
            }
            else
            {
                textBoxComment.Text = "No Borrower Information Found";
            }

            if(displayRegardlessOfResults)
            {
                Show();
                Focus();

                return DialogResult.OK; //TODO make sure this isn't an issue
            }
            else
            {
                return base.ShowDialog();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            ProtectInfo();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            textBoxComment.BackColor = SystemColors.Window;
            textBoxComment.ReadOnly = false;
            buttonSave.Enabled = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //Protect text box
            ProtectInfo();

            BrwInfo411Processor.SaveChanges(textBoxComment.Text);

            //hide form
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ProtectInfo()
        {
            textBoxComment.BackColor = SystemColors.ScrollBar;
            textBoxComment.ReadOnly = true;
            buttonSave.Enabled = false;
            buttonClose.Focus();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ProtectInfo();
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
