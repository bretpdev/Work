using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public partial class PreviousBorrowerDisplay : UserControl
    {
        public PreviousBorrowerDisplay()
        {
            InitializeComponent();
        }

        public PreviousBorrowerDisplay(PreviousBorrower borrower, int number)
        {
            InitializeComponent();

            previousBorrowerBindingSource.DataSource = borrower;
            labelPound.Text = number.ToString();
            buttonGO.Visible = true;
        }

        private void buttonGO_Click(object sender, EventArgs e)
        {
            ((MainMenu)ParentForm).textBoxAccountNumberOrSSN.Text = labelSSN.Text;
            ((MainMenu)ParentForm).buttonContinue.PerformClick();
        }
    }
}
