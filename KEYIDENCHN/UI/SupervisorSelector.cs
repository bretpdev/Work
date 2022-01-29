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
    public partial class SupervisorSelector : Form
    {
        public SupervisorSelector()
        {
            InitializeComponent();
            LoadSupervisors();
        }

        List<Supervisor> supervisors;
        private void LoadSupervisors()
        {
            supervisors = Supervisor.CachedSupervisors;
            SupervisorsList.DataSource = supervisors;

            string utid = Properties.Settings.Default.RememberedSupervisorUtId;
            if (!string.IsNullOrEmpty(utid))
            {
                RememberCheck.Checked = true;
                int index = supervisors.Select((s, i) => s.UtId == utid ? i : 0).Max();
                SupervisorsList.SelectedIndex = index;
            }
        }

        public Supervisor SelectedSupervisor { get; private set; }
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            int index = SupervisorsList.SelectedIndex;
            SelectedSupervisor = supervisors[index];
            if (RememberCheck.Checked)
                Properties.Settings.Default.RememberedSupervisorUtId = SelectedSupervisor.UtId;
            else
                Properties.Settings.Default.RememberedSupervisorUtId = null;
            Properties.Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
        }
    }
}
