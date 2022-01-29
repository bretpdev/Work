using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KEYIDENCHN
{
    public partial class SupervisorPrompt : Form
    {
        public SupervisorPrompt()
        {
            InitializeComponent();
        }

        public bool IsSupervisor { get; set; }

        private void NormalUserButton_Click(object sender, EventArgs e)
        {
            IsSupervisor = false;
            this.DialogResult = DialogResult.OK;   
        }

        private void AdminButton_Click(object sender, EventArgs e)
        {
            IsSupervisor = true;
            this.DialogResult = DialogResult.OK;
        }
    }
}
