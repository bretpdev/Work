using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LNSLSSNFED
{
    public partial class UserInput : Form
    {
        public bool formSplit { get; set; }
        public bool formPslf { get; set; }
        public string formSaleId { get; set; }
        public bool formDlo { get; set; }
        public bool formLnc { get; set; }

        public UserInput()
        {
            InitializeComponent();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            if (accountNo.Text.Length == 7 && (split.IsChecked || pslf.IsChecked))
            {
                formSplit = split.IsChecked;
                formPslf = pslf.IsChecked;
                formSaleId = accountNo.Text;
                formDlo = dlo.Checked;
                formLnc = lnc.Checked;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void split_Click(object sender, EventArgs e)
        {
            if (split.IsChecked)
                pslf.IsChecked = false;
        }

        private void pslf_Click(object sender, EventArgs e)
        {
            if (pslf.IsChecked)
                split.IsChecked = false;
        }

        private void dlo_Click(object sender, EventArgs e)
        {
            if (dlo.Checked)
                lnc.Checked = false;
        }

        private void lnc_Click(object sender, EventArgs e)
        {
            if (lnc.Checked)
                dlo.Checked = false;
        }
    }
}
