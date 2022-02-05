using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;

namespace DOCIDUHEAA
{
    public partial class EndorserSearch : Form
    {
        public DataAccess DA { get; set; }
        public List<Borrower> Bors { get; set; }
        public Borrower SelectedBorrower { get; set; }

        public EndorserSearch(DataAccess da, List<Borrower> borrowers)
        {
            InitializeComponent();
            DA = da;
            Bors = borrowers;
            LoadAccounts();
        }

        /// <summary>
        /// Adds all the borrowers for the given case number to a linked label
        /// </summary>
        private void LoadAccounts()
        {
            foreach (Borrower bor in Bors)
            {
                LinkLabel l = new LinkLabel();
                l.Text = $"{bor.Name} - {bor.AccountIdentifier}";
                l.AutoSize = true;
                l.Visible = true;
                l.LinkClicked += Lbl_Click;
                Accounts.Controls.Add(l);
            }
        }

        private void Lbl_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectedBorrower = Bors.Where(p => p.Name.StartsWith(((LinkLabel)sender).Text.SplitAndRemoveQuotes("-")[0])).FirstOrDefault();
            this.Hide();
        }
    }
}