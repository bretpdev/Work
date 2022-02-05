using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcorrLetterSetup
{
    public partial class ModeSelector : Form
    {
        public bool TestMode { get; set; }
        public ModeSelector()
        {
            InitializeComponent();
        }

        private void Test_Click(object sender, EventArgs e)
        {
            TestMode = true;
        }

        private void Live_Click(object sender, EventArgs e)
        {
            TestMode = false;
        }

    }
}
