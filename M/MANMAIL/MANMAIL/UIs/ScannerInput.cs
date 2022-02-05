using System;
using System.Windows.Forms;

namespace MANMAIL
{
    public partial class ScannerInput : Form
    {
        public string ScannedInput { get; set; }

        public ScannerInput()
        {
            InitializeComponent();
            ScannedDataTxt.Focus();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            ScannedInput = ScannedDataTxt.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void ScannerInput_MouseClick(object sender, MouseEventArgs e)
        {
            ScannedDataTxt.Focus();
        }

        private void ScannedDataTxt_TextChanged(object sender, EventArgs e)
        {
            ScannedDataLbl.Text = ScannedDataTxt.Text;
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            ScannedDataTxt.Text = "";
            ScannedDataLbl.Text = "";
            ScannedDataTxt.Focus();
        }
    }
}