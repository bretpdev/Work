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

namespace LS008
{
    public partial class LoanSelect : Form
    {
        public List<int> SelectedLoans { get; set; }
        public LoanSelect(List<int> seq, List<int> selectedSeq)
        {
            InitializeComponent();
            SelectedLoans = new List<int>();
            foreach(int loanSeq in seq)
            {
                Loans.Items.Add(loanSeq, selectedSeq.Contains(loanSeq));
            }
            //Loans.Items.AddRange(seq.Select(x => x.ToString()).ToArray());
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            if (Loans.CheckedItems.Count == 0)
                DialogResult = DialogResult.Cancel;
            else
            {
                foreach(var loans in Loans.CheckedItems)
                {
                    SelectedLoans.Add(loans.ToString().ToInt());
                }

                DialogResult = DialogResult.OK;
            }
        }
    }
}
