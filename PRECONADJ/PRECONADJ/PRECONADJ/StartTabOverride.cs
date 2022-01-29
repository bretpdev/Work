using System;
using System.Windows.Forms;
using Uheaa.Common;

namespace PRECONADJ
{
    public partial class StartTabOverride : Form
    {
        public int? StartAfterValue { get; set; } = 0;

        public StartTabOverride()
        {
            InitializeComponent();
        }

        private void startAtCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(startAtCheckBox.Checked)
                startAtUpDown.Enabled = true;
            else if(!startAtCheckBox.Checked)
            {
                startAtUpDown.Text = "";
                startAtUpDown.Enabled = false;
            }
        }

        private void startAtUpDown_ValueChanged(object sender, EventArgs e)
        {
            StartAfterValue = startAtUpDown.Value.ToString().ToIntNullable().Value;
        }
    }
}