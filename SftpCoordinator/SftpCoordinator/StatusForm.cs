using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace SftpCoordinator
{
    public partial class StatusForm : Form
    {
        public StatusForm()
        {
            InitializeComponent();
        }

        private void StopStartBox_Click(object sender, EventArgs e)
        {
            
        }

        private void SettingsBox_Click(object sender, EventArgs e)
        {
            SettingsForm fm = new SettingsForm();
            fm.ShowDialog();
        }
    }
}
