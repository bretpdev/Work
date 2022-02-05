using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PMTHISTFED
{
    public partial class MainFrm : Form
    {

        public List<Loan> UserSelectedLoans { get; set; }
        public bool SelectedAllLoans { get; set; }

        /// <summary>
        /// Default constructor (do not use).
        /// </summary>
        public MainFrm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
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
                lvwLoans.Sorting = SortOrder.Ascending;
                lvwLoans.Sort();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtSSN.Text.Length < 10)
            {
                MessageBox.Show("You must provide an account number.", "Account Number Not Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (lvwLoans.Items.Count == UserSelectedLoans.Count)
                {
                    SelectedAllLoans = true;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

    }
}
