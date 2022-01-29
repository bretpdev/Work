using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIntermediary
{
    public partial class PendingNoCallsForm : Form
    {
        PendingNoCallsDataAccess data;
        public PendingNoCallsForm(PendingNoCallsDataAccess data)
        {
            InitializeComponent();
            this.data = data;
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            DateTime start = StartBox.Value;
            DateTime end = EndBox.Value;
            if (start >= end)
            {
                MessageBox.Show("Start Date must be before End Date");
                return;
            }
            if (TotalBusinessDays(start, end) > 10)
            {
                MessageBox.Show("Start and End Date cannot be more than 10 business days apart.");
                return;
            }
            DateTime next = data.GetNextEligibleCallSuspensionDate();
            if (next > start)
            {
                MessageBox.Show("Start date is too early, the next eligible start date is " + next.ToShortDateString());
                return;
            }
            data.AddCallSuspension(start, end);
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int TotalBusinessDays(DateTime start, DateTime end)
        {
            return Enumerable.Range(0, (int)(end.Date - start.Date).TotalDays)
                .Select(o => start.AddDays(o)).Where(o => o.DayOfWeek != DayOfWeek.Saturday && o.DayOfWeek != DayOfWeek.Sunday).Count();

        }
    }
}
