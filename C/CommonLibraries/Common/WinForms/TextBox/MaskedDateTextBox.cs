using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Uheaa.Common.WinForms
{


    public class MaskedDateTextBox : NumericMaskedTextBox
    {
        public MaskedDateTextBox()
        {
            Mask = "00/00/0000";
            Font = new Font("Microsoft Sans Serif", 12);
            Width = 87;
            this.Leave += MaskedDateTextBox_Leave;
        }

        void MaskedDateTextBox_Leave(object sender, EventArgs e)
        {
            DateTime val = new DateTime();
            bool valid = DateTime.TryParse(this.Text, out val);
            if (valid || this.Text.Trim('/', ' ').IsNullOrEmpty())
                return;
            else
            {
                MessageBox.Show("Please enter a valid date.");
                this.Focus();
            }
        }
    }
}
