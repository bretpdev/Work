using System;
using System.Drawing;
using System.Windows.Forms;

namespace OLDEMOS
{
    public partial class BrwInfo411 : Form
    {
        public BrwInfo411()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(Borrower borrower)
        {
            labelSSN.Text = borrower.Ssn;
            labelName.Text = borrower.FullName;

            //Change colors
            textBoxComment.ForeColor = ForeColor;

            if (borrower.Info411.Length > 0)
                textBoxComment.Text = borrower.Info411;
            else
                textBoxComment.Text = "No Borrower Information Found";

            return base.ShowDialog();
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            ProtectInfo();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonChange_Click(object sender, EventArgs e)
        {
            textBoxComment.BackColor = SystemColors.Window;
            textBoxComment.ReadOnly = false;
            buttonSave.Enabled = true;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            //Protect text box
            ProtectInfo();

            BwrInfo411Processor.SaveChangesToSystems(textBoxComment.Text);

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

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            ProtectInfo();
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}