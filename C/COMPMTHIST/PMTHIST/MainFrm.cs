using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace COMPMTHIST
{
    public partial class MainFrm : Form
    {

        public List<Loan> UserSelectedLoans { get; set; }
        public MainFrm(BorrowerPaymentInformation bpi)
        {
            InitializeComponent();
            borrowerPaymentInformationBindingSource.DataSource = bpi;
            if (bpi.Loans.Count > 0)
            {
                grpLoanInformation.Enabled = true;
                grpBorrowerInformation.Enabled = false;
                foreach (Loan ln in bpi.Loans)
                {
                    lvwLoans.Items.Add(ln);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtSSN.Text.Length < 9)
            {
                MessageBox.Show("You must provide an SSN.", "SSN Not Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnGetPaymentData_Click(object sender, EventArgs e)
        {
            if (lvwLoans.SelectedItems.Count == 0)
            {
                MessageBox.Show("You must select a loan.", "Loan Not Selected",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                UserSelectedLoans = (from ln in lvwLoans.SelectedItems.Cast<Loan>()
                                    select ln).ToList();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserSelectedLoans = (from ln in lvwLoans.Items.Cast<Loan>()
                                 select ln).ToList();

            DialogResult = DialogResult.OK;
        } 
    }
}
