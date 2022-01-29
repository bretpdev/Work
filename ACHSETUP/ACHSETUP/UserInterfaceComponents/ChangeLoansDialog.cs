using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ACHSETUP
{
    partial class ChangeLoansDialog : Form
    {
        private List<Loan> AvailableLoans { get; set; }
        private List<Loan> AvailableLoansOriginal { get; set; }
        private List<Loan> SelectedLoans { get; set; }
        private List<Loan> SelectedLoansOriginal { get; set; }

        /// <summary>
        /// DO NOT USE!!!
        /// The parameterless constructor is required by the Windows Forms Designer,
        /// but it won't work with the script.
        /// </summary>
        public ChangeLoansDialog()
        {
            InitializeComponent();
        }

        public ChangeLoansDialog(List<Loan> availableLoans, List<Loan> selectedLoans)
        {
            InitializeComponent();

            AvailableLoans = availableLoans;
            foreach (Loan ln in availableLoans)
                dgvAvailableLoans.Rows.Add(ln.Sequence, ln.Program, ln.Balance, ln.FirstDisbDate);
            AvailableLoansOriginal = new List<Loan>(AvailableLoans);
            SelectedLoans = selectedLoans;
            foreach (Loan ln in selectedLoans)
                dgvSelectedLoans.Rows.Add(ln.Sequence, ln.Program, ln.Balance, ln.FirstDisbDate);
            SelectedLoansOriginal = new List<Loan>(SelectedLoans);
            OK.Enabled = false; //Remains disabled until the loan lists differ from their original state.
        }

        private void Add_Click(object sender, System.EventArgs e)
        {
            foreach (DataGridViewRow row in dgvAvailableLoans.SelectedRows)
            {
                Loan selectedLoan = AvailableLoans.Where(p => p.Sequence == (int)row.Cells["SeqAvailable"].Value).Single();
                SelectedLoans.Add(selectedLoan);
                AvailableLoans.Remove(selectedLoan);
            }
            RefreshForm();
        }

        private void AddAll_Click(object sender, System.EventArgs e)
        {
            foreach (DataGridViewRow row in dgvAvailableLoans.Rows)
            {
                Loan selectedLoan = AvailableLoans.Where(p => p.Sequence == (int)row.Cells["SeqAvailable"].Value).Single();
                SelectedLoans.Add(selectedLoan);
                AvailableLoans.Remove(selectedLoan);
            }
            RefreshForm();
        }

        private void Remove_Click(object sender, System.EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSelectedLoans.SelectedRows)
            {
                Loan selectedLoan = SelectedLoans.Where(p => p.Sequence == (int)row.Cells["SeqCurrent"].Value).Single();
                AvailableLoans.Add(selectedLoan);
                SelectedLoans.Remove(selectedLoan);
            }
            RefreshForm();
        }

        private void RemoveAll_Click(object sender, System.EventArgs e)
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
                dgvAvailableLoans.Rows.Add(ln.Sequence, ln.Program, ln.Balance, ln.FirstDisbDate);

            foreach (Loan ln in SelectedLoans)
                dgvSelectedLoans.Rows.Add(ln.Sequence, ln.Program, ln.Balance, ln.FirstDisbDate);


            //Enable or disable the OK button based on whether the loan lists have changed.
            bool listsChanged = false;
            if (AvailableLoans.Count != AvailableLoansOriginal.Count)
                listsChanged = true;
            else
            {
                foreach (Loan currentLoan in AvailableLoans)
                    if (AvailableLoansOriginal.Where(p => p.Sequence == currentLoan.Sequence).Count() == 0)
                        listsChanged = true;
                foreach (Loan originalLoan in AvailableLoansOriginal)
                    if (AvailableLoans.Where(p => p.Sequence == originalLoan.Sequence).Count() == 0)
                        listsChanged = true;
            }
            OK.Enabled = listsChanged;
        }
    }
}