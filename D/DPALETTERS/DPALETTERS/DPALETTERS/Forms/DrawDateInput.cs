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
    public partial class DrawDateInput : Form
    {
        public DateTime? DrawDate { get; set; }
        public DrawDateInput()
        {
            InitializeComponent();
        }

        private bool ValidateInput()
        {
            DateTime? parsedDate = maskedDateTextBox.Text.ToDateNullable();
            //The draw date has to be on the 7th, 15th, or 22nd
            //The draw date has to be between today and 60 days from now
            if (parsedDate.HasValue && parsedDate.Value.Day.IsIn(7,15,22) && parsedDate.Value >= DateTime.Today && parsedDate.Value <= DateTime.Today.AddDays(60))
            {
                DrawDate = parsedDate.Value;
                return true;
            }
            else if(parsedDate.HasValue && !parsedDate.Value.Day.IsIn(7, 15, 22))
            {
                Dialog.Warning.Ok("The first draw date must be either the 7th, 15th or the 22nd in MMDDYYYY or MM/DD/YYYY format","Invalid Draw Date");
                return false;
            }
            else if (parsedDate.HasValue && (parsedDate.Value < DateTime.Today || parsedDate.Value > DateTime.Today.AddDays(60)))
            {
                Dialog.Warning.Ok("The draw date must not be a past date or more than 60 days in the future.", "Invalid Draw Date");
                return false;
            }
            else
            {
                Dialog.Warning.Ok("The date entered is not a valid date.", "Invalid Draw Date");
                return false;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(ValidateInput())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonNoDrawDate_Click(object sender, EventArgs e)
        {
            DrawDate = null;
            DialogResult = DialogResult.OK;
        }
    }
}
