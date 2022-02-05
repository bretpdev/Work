using System;
using System.Drawing;
using Uheaa.Common;
using Uheaa.Common.WinForms;

namespace PWRATRNY
{
    public class NonRequiredMaskedDateTextBox : NumericMaskedTextBox
    {
        public NonRequiredMaskedDateTextBox()
        {
            Mask = "00/00/0000";
            Font = new Font("Microsoft Sans Serif", 12);
            Width = 87;
            Leave += MaskedDateTextBox_Leave;
        }

        void MaskedDateTextBox_Leave(object sender, EventArgs e)
        {
            if (Text.ToDateNullable() != null)
                return;
            ResetText();
        }
    }
}