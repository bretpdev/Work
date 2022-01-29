using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetTester
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            var settings = Properties.Settings.Default;
            UsernameBox.DataBindings.Add("Text", settings, "SessionUsername", true, DataSourceUpdateMode.OnPropertyChanged);
            PasswordBox.DataBindings.Add("Text", settings, "SessionPassword", true, DataSourceUpdateMode.OnPropertyChanged);
            DownloadLocationBox.DataBindings.Add("Text", settings, "DownloadLocation", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            var ofd = new Uheaa.Common.OpenFolderDialog();
            if (ofd.ShowDialog(this.Handle, false) == System.Windows.Forms.DialogResult.OK)
            {
                DownloadLocationBox.Text = ofd.Folder;
            }
        }
    }
}
