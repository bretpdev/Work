using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace NSFREVENTR
{
    public partial class UserChoiceOfPayments : FormBase
    {

        private double _userProvidedAmount;

        public List<COMPASSPayment> SelectedPayments { get; set; }

        public UserChoiceOfPayments(double userProvidedAmount, List<COMPASSPayment> possiblePayments)
        {
            InitializeComponent();
            _userProvidedAmount = userProvidedAmount;
            lvwPayments.Items.AddRange(possiblePayments.ToArray());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lvwPayments.SelectedItems.Count == 0)
            {   
                //must selecte at least one payment
                MessageBox.Show("You must select at least one payment.","No Payment Selected",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else if (lvwPayments.SelectedItems.Count > 2)
            {
                //cannot select more than 2 payments
                MessageBox.Show("You cannot select more that 2 payments at a time.  Please try again", "Too Many Payments Selected",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                double summedTotal = 0;
                foreach(ListViewItem lvi in lvwPayments.SelectedItems)
                {
                    summedTotal = summedTotal + double.Parse(((COMPASSPayment)lvi).RemittanceAmount);
                }
                if (_userProvidedAmount != summedTotal)
                {
                    MessageBox.Show("The summed total of the payments you selected don't equal the payment amount you provided.", "Please Try Again", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SelectedPayments = new List<COMPASSPayment>(lvwPayments.SelectedItems.OfType<COMPASSPayment>());
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

    }
}
