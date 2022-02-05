using System;
using System.Windows.Forms;

namespace SCHDEMOUP
{
    partial class frmDemoRun : Form
    {
        private ProcessingOption _option;

        public frmDemoRun(ProcessingOption option)
        {
            InitializeComponent();
            _option = option;
        }

        public frmDemoRun()
        {
            InitializeComponent();
        }

        private void btnRunUpdates_Click(object sender, EventArgs e)
        {
            _option.SelectedOption = ProcessingOption.Option.Update;
            this.DialogResult = DialogResult.OK;
        }

        private void btnLive_Click(object sender, EventArgs e)
        {
            _option.SelectedOption = ProcessingOption.Option.Live;
            this.DialogResult = DialogResult.OK;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            _option.SelectedOption = ProcessingOption.Option.Test;
            this.DialogResult = DialogResult.OK;
        }
    }
}
