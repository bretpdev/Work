using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace MauiDUDE
{
    public partial class PaymentHistoryDisplay : UserControl
    {
        private FinancialTransactionWithLoanLevelDetail TransactionData;
        public PaymentHistoryDisplay()
        {
            InitializeComponent();
        }

        public PaymentHistoryDisplay(FinancialTransactionWithLoanLevelDetail transaction)
        {
            InitializeComponent();

            //TODO
            financialTransactionWithLoanLevelDetailBindingSource.DataSource = transaction;
            TransactionData = transaction;
            buttonExpand.Visible = true;
        }

        private void buttonExpand_Click(object sender, EventArgs e)
        {
            if(flowLayoutPanel.Controls.Count == 0)
            {
                flowLayoutPanel.Visible = true;
                flowLayoutPanel.Controls.Add(new PaymentHistoryDetailDisplay());
                if (TransactionData != null) //this is here so that it will behave better when the borrower has no transaction history
                {
                    foreach (FinancialTransaction transaction in TransactionData.LoanLevelTransactions)
                    {
                        PaymentHistoryDetailDisplay newControl = new PaymentHistoryDetailDisplay(transaction);
                        newControl.BackColor = Color.White;
                        flowLayoutPanel.Controls.Add(newControl);
                    }
                }
                buttonExpand.Text = "-";
            }
            else
            {
                flowLayoutPanel.Visible = false;
                flowLayoutPanel.Controls.Clear();
                buttonExpand.Text = "+";
            }
        }
    }
}
