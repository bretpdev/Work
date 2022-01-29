using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;

namespace Uheaa.Common.WinForms
{
    public partial class SimpleBorrowerSearchControl : UserControl
    {
        public SimpleBorrowerSearchControl()
        {
            InitializeComponent();
        }

        public LogDataAccess LDA { get; set; }
        public delegate void SearchResultsRetrieved(SimpleBorrowerSearchControl sender, List<QuickBorrower> results);
        public event SearchResultsRetrieved OnSearchResultsRetrieved;

        private void SearchButton_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void NameBox_TextChanged(object sender, EventArgs e)
        {
            SearchButton.Enabled = Math.Max(FirstNameBox.Text.Length, LastNameBox.Text.Length) >= 4;
        }

        private void Enter_Pressed(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Search();
            }
        }

        private void Search()
        {
            SearchButton.Enabled = FirstNameBox.Enabled = LastNameBox.Enabled = false;
            Task.Run(new Action(() =>
            {
                var results = QuickBorrower.QuickSearch(FirstNameBox.Text, LastNameBox.Text, LDA);
                this.BeginInvoke(new Action(() =>
                {
                    OnSearchResultsRetrieved?.Invoke(this, results);
                    SearchButton.Enabled = FirstNameBox.Enabled = LastNameBox.Enabled = true;
                    this.Focus();
                }));
            }));

        }

        public void ResetFields()
        {
            FirstNameBox.Text = "";
            LastNameBox.Text = "";
        }

        private void Key_Press(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }
    }
}
