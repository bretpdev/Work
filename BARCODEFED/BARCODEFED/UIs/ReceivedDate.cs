using System;
using System.Windows.Forms;

namespace BARCODEFED
{
    public partial class ReceivedDate : Form
    {
        public ReceivedDate()
        {
            InitializeComponent();
            selectedDate.Value = DateTime.Now;
            selectedDate.MaxDate = DateTime.Now.Date;
        }

        private void Select_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}