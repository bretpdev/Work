using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;

namespace BCSRETMAIL
{
    public partial class SearchForm : Form
    {
        public bool CodeInitiated = false;
        public QuickBorrower Borrower { get; set; }

        public SearchForm()
        {
            InitializeComponent();
        }

        private void BorrowerSearch_OnSearchResultsRetrieved(BorrowerSearchControl sender, List<QuickBorrower> results)
        {
            if (results != null && results.Count > 0)
                UpdateDOB(results);
            BorrowerResults.SetResults(results);
            for (int i = 0; i < ((DataGridView)BorrowerResults.Controls[0].Controls[0]).Columns.Count; i++)
            {
                ((DataGridView)BorrowerResults.Controls[0].Controls[0]).AutoResizeColumn(i);
            }
        }

        /// <summary>
        /// Removes the timestamp from the date of birth
        /// </summary>
        /// <param name="results"></param>
        private void UpdateDOB(List<QuickBorrower> results)
        {
            foreach (QuickBorrower bor in results)
            {
                if (bor != null && bor.DOB != null)
                    bor.DOB = bor.DOB.SplitAndRemoveQuotes(" ")[0];
            }
        }

        private void BorrowerResults_OnBorrowerChosen(object sender, QuickBorrower selected)
        {
            Borrower = selected;
            this.Hide();
        }
    }
}