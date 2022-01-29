using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace I1I2FEDREV
{
    public partial class BorrowerDemo : Form
    {
        public BorrowerDemo(BorrowerInfo borrower)
        {
            InitializeComponent();
            borrowerInfoBindingSource.DataSource = borrower;
            if (borrower.AddressStatus == "VALID")
                AddressStatus.ForeColor = Color.Green;
            else
                AddressStatus.ForeColor = Color.Red;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateEntry())
                return;

            DialogResult = DialogResult.Yes;
        }

        private bool ValidateEntry()
        {
            List<string> errors = new List<string>();
            if (Street1.Text.IsNullOrEmpty())
                errors.Add("Street1");
            if (City.Text.IsNullOrEmpty())
                errors.Add("City");
            if (State.Enabled && State.Text.IsNullOrEmpty())
                errors.Add("State");
            if (!State.Enabled && Country.Text.IsNullOrEmpty())
                errors.Add("Country");
            if (Zip.Text.IsNullOrEmpty())
                errors.Add("Zip");

            if (errors.Any())
            {
                MessageBox.Show("Please enter the following errors: \n" + string.Join("\n", errors.Select(e => " - " + e).ToArray()),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void State_TextChanged(object sender, EventArgs e)
        {
            if (!State.Text.IsNullOrEmpty())
                 Country.Enabled = false;
            else
                 Country.Enabled = true;
        }

        private void Country_TextChanged(object sender, EventArgs e)
        {
            if (!Country.Text.IsNullOrEmpty())
                State.Enabled = false;
            else
                State.Enabled = true;
        }
    }
}
