using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace DPLTRS.Payoff
{
    public partial class PayoffDateForm : Form
    {
        public DateTime PayoffDate { get; set; }
        public PayoffDateForm()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            if(payOffDateTextBox.Text != null && payOffDateTextBox.Text != "")
            {
                DateTime dt;
                bool success = DateTime.TryParseExact(payOffDateTextBox.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                if(success)
                {
                    PayoffDate = dt;
                    DialogResult = DialogResult.OK;
                    return;
                }
                else
                {
                    Dialog.Info.Ok("Please enter a valid date.");
                }
            }
        }
    }
}
