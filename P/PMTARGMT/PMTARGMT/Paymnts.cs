using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace Payments
{
    public partial class Paymnts : FormBase
    {

        public PossiblePaymentDate SelectedPaymentDue { get; set; }

        public Paymnts()
        {
            InitializeComponent();
            cmbDayOfMonth.Items.Add(new DayOfTheMonth(1));
            cmbDayOfMonth.Items.Add(new DayOfTheMonth(7));
            cmbDayOfMonth.Items.Add(new DayOfTheMonth(15));
            cmbDayOfMonth.Items.Add(new DayOfTheMonth(22));
        }

        private void cmbDayOfMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbMonth.Items.Clear();
            cmbMonth.Items.AddRange(((DayOfTheMonth)cmbDayOfMonth.SelectedItem).CalculatePossibleMonths().ToArray());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //a month was selected so processing can continue
            SelectedPaymentDue = (PossiblePaymentDate) cmbMonth.SelectedItem;
        }

    }
}
