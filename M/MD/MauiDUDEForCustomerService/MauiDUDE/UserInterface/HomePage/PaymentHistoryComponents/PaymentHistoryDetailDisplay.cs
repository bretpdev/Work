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
    public partial class PaymentHistoryDetailDisplay : UserControl
    {
        public PaymentHistoryDetailDisplay()
        {
            InitializeComponent();
        }

        public PaymentHistoryDetailDisplay(FinancialTransaction transaction)
        {
            InitializeComponent();

            financialTransactionBindingSource.DataSource = transaction;
        }
    }
}
