using System;
using System.Windows.Forms;
using Uheaa.Common;

namespace IDRUSERPRO
{
    public partial class AnnivesaryDate : Form
    {
        public DateTime? AnniversaryDate { get; set; }
        public AnnivesaryDate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnniversaryDate = ADate.Text.ToDateNullable();
            if (AnniversaryDate > DateTime.MaxValue || AnniversaryDate < DateTime.MinValue)
                AnniversaryDate = null;

            if (AnniversaryDate.HasValue)
                DialogResult = DialogResult.OK;
            else
                Dialog.Error.Ok("You did not enter a valid date.", "Error");
        }
    }
}