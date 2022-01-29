using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace ACHSETUPFD
{
	partial class ChangeLoansDialog : Form
	{
        private List<Loan> AvailableLoans { get; set; }
        private List<Loan> AvailableOriginalLoans { get; set; }
        private List<Loan> SelectedLoans { get; set; }
        private List<Loan> SelectedOriginalLoans { get; set; }
        
		public ChangeLoansDialog(List<Loan> availableLoans, List<Loan> selectedLoans)
		{
			InitializeComponent();

			AvailableLoans = availableLoans;
            foreach (Loan ln in availableLoans)
            {
                dgvAvailableLoans.Rows.Add(ln.Sequence,ln.Program,ln.Balance,ln.FirstDisbDate);   
            }
			AvailableOriginalLoans = new List<Loan>(AvailableLoans);
			SelectedLoans = selectedLoans;
            foreach (Loan ln in selectedLoans)
            {
                dgvSelectedLoans.Rows.Add(ln.Sequence, ln.Program, ln.Balance, ln.FirstDisbDate);
            }
			SelectedOriginalLoans = new List<Loan>(SelectedLoans);
			btnOk.Enabled = false; //Remains disabled until the loan lists differ from their original state.
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			foreach (DataGridViewRow row in dgvAvailableLoans.SelectedRows)
			{
				Loan selectedLoan = AvailableLoans.Where(p => p.Sequence == (int)row.Cells["SeqAvailable"].Value).Single();
				SelectedLoans.Add(selectedLoan);
				AvailableLoans.Remove(selectedLoan);
			}
			RefreshForm();
		}

		private void btnAddAll_Click(object sender, System.EventArgs e)
		{
			foreach (DataGridViewRow row in dgvAvailableLoans.Rows)
			{
                Loan selectedLoan = AvailableLoans.Where(p => p.Sequence == (int)row.Cells["SeqAvailable"].Value).Single();
				SelectedLoans.Add(selectedLoan);
				AvailableLoans.Remove(selectedLoan);
			}
			RefreshForm();
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			foreach (DataGridViewRow row in dgvSelectedLoans.SelectedRows)
			{
				Loan selectedLoan = SelectedLoans.Where(p => p.Sequence == (int)row.Cells["SeqCurrent"].Value).Single();
				AvailableLoans.Add(selectedLoan);
				SelectedLoans.Remove(selectedLoan);
			}
			RefreshForm();
		}

		private void btnRemoveAll_Click(object sender, System.EventArgs e)
		{
			foreach (DataGridViewRow row in dgvSelectedLoans.Rows)
			{
                Loan selectedLoan = SelectedLoans.Where(p => p.Sequence == (int)row.Cells["SeqCurrent"].Value).Single();
				AvailableLoans.Add(selectedLoan);
				SelectedLoans.Remove(selectedLoan);
			}
			RefreshForm();
		}

		private void RefreshForm()
		{

            //Update the contents of the DataGridViews.
            dgvAvailableLoans.Rows.Clear();
            dgvSelectedLoans.Rows.Clear();

            foreach (Loan ln in AvailableLoans)
            {
                dgvAvailableLoans.Rows.Add(ln.Sequence, ln.Program, ln.Balance, ln.FirstDisbDate);
            }

            foreach (Loan ln in SelectedLoans)
            {
                dgvSelectedLoans.Rows.Add(ln.Sequence, ln.Program, ln.Balance, ln.FirstDisbDate);
            }
            

			//Enable or disable the OK button based on whether the loan lists have changed.
			bool listsChanged = false;
			if (AvailableLoans.Count != AvailableOriginalLoans.Count)
			{
				listsChanged = true;
			}
			else
			{
				foreach (Loan currentLoan in AvailableLoans)
				{
					if (AvailableOriginalLoans.Where(p => p.Sequence == currentLoan.Sequence).Count() == 0) { listsChanged = true; }
				}
				foreach (Loan originalLoan in AvailableOriginalLoans)
				{
					if (AvailableLoans.Where(p => p.Sequence == originalLoan.Sequence).Count() == 0) { listsChanged = true; }
				}
			}
			btnOk.Enabled = listsChanged;
		}
	}
}