using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace DPALETTERS
{
    public partial class ValidateDrawAmount : Form
    {
        private decimal ExpectedDrawAmount{ get; set; }
        public decimal? DrawAmount { get; set; } = null;
        public ValidateDrawAmount(decimal drawAmount)
        {
            InitializeComponent();

            ExpectedDrawAmount = drawAmount;
            textBoxDrawAmount.Text = string.Format("{0:C}", drawAmount);
        }

        private bool ValidateInput()
        {
            decimal? newDrawAmount = textBoxDrawAmount.Text.ToDecimalNullable();
            if(newDrawAmount.HasValue && newDrawAmount.Value >= ExpectedDrawAmount)
            {
                DrawAmount = newDrawAmount.Value;
                return true;
            }
            else if(newDrawAmount.HasValue && newDrawAmount.Value < ExpectedDrawAmount)
            {
                Dialog.Warning.Ok("The draw amount cannot be less than the sum of the expected payment amounts for all open loans.","Invalid Draw Amount");
                textBoxDrawAmount.Text = string.Format("{0:C}", ExpectedDrawAmount);
                return false;
            }
            else
            {
                Dialog.Warning.Ok("The draw amount is not a valid number.", "Invalid Draw Amount");
                textBoxDrawAmount.Text = string.Format("{0:C}", ExpectedDrawAmount);
                return false;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(ValidateInput())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
