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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void ProjectsTable_RowSelected(DataGridViewRow row)
        {
            int projectId = (int)row.Cells["ProjectId"].Value;
            string name = (string)row.Cells["Name"].Value;
            ProjectFilesTable.HiddenColumns["ProjectId"] = projectId;
            ProjectFilesLabel.Text = "Project \"" + name + "\" Files";
        }
    }
}
