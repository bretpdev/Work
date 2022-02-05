using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace IDRUSERPRO
{
    public partial class ConsolidationLoans : Form
    {
        public List<Ts26Loans> Loans { get; set; }

        public ConsolidationLoans(List<Ts26Loans> loans)
        {
            InitializeComponent();
            Loans = loans;
            for (int loanId = 0; loanId < Loans.Count; loanId++)
            {
                Ts26Loans loan = loans[loanId];
                if (loan.LoanType.Trim().IsIn("DLSCNS", "DLUCNS", "DLSSPL", "DLUSPL", "UNCNS", "SUBCNS", "CNSLDN", "SUBSPC", "SPCNSL", "UNSPC"))
                {
                    string[] items = { loan.LoanType, loan.LoanSeq, loan.DisbDate };
                    var listItem = lsvConsolLoans.Items.Add((loanId + 1).ToString());
                    listItem.SubItems.AddRange(items);
                    if (!loan.IsEligible)
                        listItem.Checked = true;
                }
            }

            if (lsvConsolLoans.Items.Count < 1)
                lblLoans.Text = "Borrower does not have consolidation loans";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (lsvConsolLoans.Items.Count >= 1)
            {
                List<int> selectedIndices = lsvConsolLoans.CheckedIndices.Cast<int>().ToList();

                if (!selectedIndices.Any())
                    if (!Dialog.Def.YesNo("Are you sure all loans are eligible?"))
                        return;

                foreach (ListViewItem item in lsvConsolLoans.Items)
                {
                    var loanSequence = item.SubItems[2].Text;
                    var matchingLoan = Loans.Single(o => o.LoanSeq == loanSequence);
                    matchingLoan.IsEligible = !item.Checked;
                }

            }
            DialogResult = DialogResult.OK;

        }
    }
}
