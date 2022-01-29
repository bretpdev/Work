using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monitor
{
    public partial class LaunchForm : Form
    {
        public bool WasAutoLaunched { get; private set; }
        const int autoLaunchSeconds = 60;
        public LaunchForm()
        {
            InitializeComponent();
            autoLaunchMessage = AutoLaunchTimerLabel.Text;
            autoLaunchTime = DateTime.Now.AddSeconds(autoLaunchSeconds);
        }

        DateTime autoLaunchTime;
        string autoLaunchMessage;
        private void AutoLaunchTimer_Tick(object sender, EventArgs e)
        {
            int secondsLeft = (int)(autoLaunchTime - DateTime.Now).TotalSeconds;
            AutoLaunchTimerLabel.Text = string.Format(autoLaunchMessage, secondsLeft);
            if (secondsLeft <= 0)
            {
                WasAutoLaunched = true;
                this.Close();
            }
        }

        private void AdjustSettingsButton_Click(object sender, EventArgs e)
        {
            WasAutoLaunched = false;
            this.Close();
        }

        private void BeginProcessingButton_Click(object sender, EventArgs e)
        {
            WasAutoLaunched = true;
            this.Close();
        }
    }
}
