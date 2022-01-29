using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TCPAFEDPS
{
    public partial class ModeChooser : Form
    {
        public bool BatchProcessing { get; set; }
        public ModeChooser()
        {
            InitializeComponent();
            Batch.Focus();
        }

        private void Batch_Click(object sender, EventArgs e)
        {
            BatchProcessing = true;
            DialogResult = DialogResult.OK;
        }

        private void Manual_Click(object sender, EventArgs e)
        {
            BatchProcessing = false;
            DialogResult = DialogResult.OK;
        }
    }
}
