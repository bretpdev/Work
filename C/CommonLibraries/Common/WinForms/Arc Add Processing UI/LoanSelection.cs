using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.WinForms
{
    public partial class LoanSelection : Form
    {
        public List<string> SelectedLoans { get; set; }
        public LoanSelection(bool loanSeq, string accountNumber)
        {
            InitializeComponent();
            SelectedLoans = new List<string>();
            Message.Text = string.Format("Borrowers {0}", loanSeq ? "Loan Sequence(s)" : "Loan Program(s)");
            string sproc = loanSeq ? "GetBorrowersLoanSeqForArcAdd" : "GetBorrowersLoanPgmForArcAdd";
            DataAccessHelper.Database db = DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? DataAccessHelper.Database.Udw : DataAccessHelper.Database.Cdw;
            List<string> loans = new List<string>();

            loans = DataAccessHelper.ExecuteList<string>(sproc, db,  SqlParams.Single("AccountNumber", accountNumber));
            foreach (object seq in loans)
                Selection.Items.Add(seq.ToString());
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (Selection.SelectedItems.Count == 0)
            {
                MessageBox.Show("You must select at least 1 loan");
                return;
            }

            foreach (var item in Selection.SelectedItems)
                SelectedLoans.Add(item.ToString());

            DialogResult = DialogResult.OK;
        }
    }
}
