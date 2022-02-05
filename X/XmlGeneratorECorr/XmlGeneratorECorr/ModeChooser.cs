using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XmlGeneratorECorr
{
    public partial class ModeChooser : Form
    {
        public bool Console { get; set; }
        public ModeChooser()
        {
            InitializeComponent();
        }

        private void BatchMode_Click(object sender, EventArgs e)
        {
            Console = true;
        }

        private void UserMode_Click(object sender, EventArgs e)
        {
            Console = false;
        }

    }
}
