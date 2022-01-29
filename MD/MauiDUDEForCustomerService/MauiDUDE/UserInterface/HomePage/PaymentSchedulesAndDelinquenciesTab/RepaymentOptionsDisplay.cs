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
    public partial class RepaymentOptionsDisplay : UserControl
    {
        public RepaymentOptionsDisplay()
        {
            InitializeComponent();
        }

        public RepaymentOptionsDisplay(string terms, string amount, string beginDate)
        {
            labelMonths.Text = terms;
            labelRepayAmount.Text = $"${amount}";
            labelBeginDate.Text = beginDate;
        }
    }
}
