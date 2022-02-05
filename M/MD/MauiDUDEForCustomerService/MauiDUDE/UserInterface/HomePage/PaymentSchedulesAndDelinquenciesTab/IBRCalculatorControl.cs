using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace MauiDUDE
{
    public partial class IBRCalculatorControl : UserControl
    {
        public IBRCalculatorControl()
        {
            InitializeComponent();
        }

        public IBRCalculatorControl(List<ServicingLoanDetail> loans)
        {
            InitializeComponent();

            Proc.Start("RepaymentEstimator");
            foreach(ServicingLoanDetail loan in loans)
            {
                listView.Items.Add(new ListViewItem(new string[] { loan.LoanSeqNum.ToString(), loan.LoanType, loan.CurrentPrincipal.ToString(), loan.InterestRate.ToString() }));
            }
        }
    }
}
