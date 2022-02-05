using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace MD
{
    public partial class TrainingModulesForm : BaseForm
    {
        public TrainingModulesForm()
        {
            InitializeComponent();
            LoadModules();
        }

        private List<string> moduleLocations;
        const string networkLocation = @"X:\Training Modules\";
        public void LoadModules()
        {
            moduleLocations = new List<string>
            (
                Directory.GetFiles(networkLocation, "*.ppt").Union(Directory.GetFiles(networkLocation, "*.pptx")).Distinct()
            );
            ModulesList.DataSource = moduleLocations.Select(file => Path.GetFileNameWithoutExtension(file)).ToList();
        }

        private void ModulesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LaunchButton.Enabled = ModulesList.SelectedIndex != -1;
        }

        private void ModulesList_DoubleClick(object sender, EventArgs e)
        {
            LaunchButton.PerformClick();
        }

        private void LaunchButton_Click(object sender, EventArgs e)
        {
            if(moduleLocations.Count <= ModulesList.SelectedIndex || ModulesList.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a training module before continuing.", "Please select an option", MessageBoxButtons.OK);
                return;
            }

            string module = moduleLocations[ModulesList.SelectedIndex];
            
            new Thread(() =>
                {
                    string newName = TempHelper.GetPath(Guid.NewGuid().ToString() + ".pps");
                    using (NotifyIcon ni = new NotifyIcon())
                    {
                        ni.Visible = true;
                        ni.Icon = Properties.Resources.waveicon;
                        ni.ShowBalloonTip(5000, "Slideshow Starting", "Press Escape to end the slideshow early", ToolTipIcon.Info);
                        FS.Copy(module, newName);
                        Proc.Start(newName);
                    }
                }).Start();
        }
    }
}
