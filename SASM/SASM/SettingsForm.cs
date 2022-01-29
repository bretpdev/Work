using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SASM
{
    public partial class SettingsForm : Form
    {
        private static SettingsForm form;
        public static SettingsForm Singleton
        {
            get
            {
                if (form == null || form.IsDisposed)
                    form = new SettingsForm();
                return form;
            }
        }

        Properties.Settings settings = Properties.Settings.Default;

        private SettingsForm()
        {
            InitializeComponent();

            UsernameText.Text = Username = settings.Username;
            DusterLiveText.Text = DusterLivePassword = settings.DusterLivePassword;
            DusterTestText.Text = DusterTestPassword = settings.DusterTestPassword;
            LegendText.Text = LegendPassword = settings.LegendPassword;
        }

        public string Username { get; set; }
        public string DusterLivePassword { get; set; }
        public string DusterTestPassword { get; set; }
        public string LegendPassword { get; set; }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Username = UsernameText.Text;
            DusterLivePassword = DusterLiveText.Text;
            DusterTestPassword = DusterTestText.Text;
            LegendPassword = LegendText.Text;

            settings.Username = UsernameText.Text;
            settings.DusterLivePassword = DusterLiveText.Text;
            settings.LegendPassword = LegendText.Text;
            settings.DusterTestPassword = DusterTestText.Text;
            settings.Save();

            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
    }
}
