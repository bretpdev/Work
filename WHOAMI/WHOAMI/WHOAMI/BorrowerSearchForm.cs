using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.WinForms;

namespace WHOAMI
{
    public partial class BorrowerSearchForm : Form
    {
        public BorrowerSearchForm()
        {
            InitializeComponent();
            this.Text = $"{this.Text} :: Version:{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private List<QuickBorrower> CurrentResults = null;
        private void BorrowerSearch_OnSearchResultsRetrieved(BorrowerSearchControl sender, List<QuickBorrower> results)
        {
            CurrentResults = results;
            BorrowerResults.SetResults(results);
        }

        public QuickBorrower SelectedBorrower { get; set; }
        private void BorrowerResults_OnBorrowerChosen(object sender, QuickBorrower selected)
        {
            SelectedBorrower = selected;
            if (SelectedBorrower != null)
                this.DialogResult = DialogResult.OK;
        }

        private void BorrowerResults_OnSelectionChanged(object sender, QuickBorrower selected)
        {
            SelectedBorrower = selected;
            if (SelectedBorrower != null)
                this.DialogResult = DialogResult.OK;
        }
    }
}