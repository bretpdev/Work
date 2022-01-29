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

namespace NSLDSCONSO
{
    public partial class MapperForm : Form
    {
        DataAccess da;
        public MapperForm(DataAccess da)
        {
            InitializeComponent();
            this.da = da;
        }

        List<UnmappedBorrower> borrowers;
        private void MapperForm_Load(object sender, EventArgs e)
        {
            borrowers = da.GetBorrowersWithUnmappedLoans();
            if (borrowers.Any())
            {
                BorrowerList.DataSource = borrowers;
                BorrowerList.DisplayMember = "Name";
            }
            else
            {
                Dialog.Info.Ok("No Borrowers with Unmapped Loans found.", "All Loans Mapped");
                this.Close();
            }
        }

        private void BorrowerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBorrower();
        }

        BorrowerDetails selectedBorrowerDetails;
        private void LoadBorrower()
        {
            AbandonButton.Enabled = false;
            SaveButton.Enabled = false;
            var selectedBorrower = borrowers[BorrowerList.SelectedIndex];
            SsnBox.Text = selectedBorrower.Ssn;
            BorrowerIdBox.Text = selectedBorrower.BorrowerId.ToString();
            NameBox.Text = selectedBorrower.Name;
            selectedBorrowerDetails = da.GetBorrowerDetails(selectedBorrower.BorrowerId);
            ConsolidationLoansGrid.DataSource = selectedBorrowerDetails.ConsolidationLoans;
            UnderlyingLoansGrid.DataSource = selectedBorrowerDetails.UnderlyingLoans;
            foreach (DataGridViewColumn column in UnderlyingLoansGrid.Columns)
            {
                if (column.Name.IsIn("NewLoanId", "NsldsLabel"))
                    column.ReadOnly = false;
                else
                    column.ReadOnly = true;
            }
        }

        private void UnderlyingLoansGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            BorrowerList.Enabled = false;
            AbandonButton.Enabled = true;
            SaveButton.Enabled = true;
        }

        private void AbandonButton_Click(object sender, EventArgs e)
        {
            BorrowerList.Enabled = true;
            LoadBorrower();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var updates = selectedBorrowerDetails.UnderlyingLoans.Select(o => new BorrowerUnderlyingLoanForUpdate()
            {
                BorrowerUnderlyingLoanId = o.BorrowerUnderlyingLoanId,
                NewLoanId = o.NewLoanId.Trim(),
                NsldsLabel = o.NsldsLabel.Trim()
            }).ToList();
            if (da.UpdateBorrowerLoans(updates))
                Dialog.Info.Ok("Borrower Loans Updated.");
            else
                Dialog.Warning.Ok("Unable to update Borrower Loans.");
            BorrowerList.Enabled = true;
            LoadBorrower();
        }
    }
}
