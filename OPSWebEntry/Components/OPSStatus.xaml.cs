using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Uheaa.Common;

namespace OPSWebEntry
{
    /// <summary>
    /// Interaction logic for OPSStatus.xaml
    /// </summary>
    public partial class OPSStatus : UserControl
    {
        public OPSStatus()
        {
            InitializeComponent();
            OPSManager.StatusChanged += () => Sync();
            Sync();
        }

        public void Sync()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                StatusLabel.Content = OPSManager.Status.GetDescription().Description;
                if (OPSManager.Status == OPSStates.NotRunning)
                    StartStopButton.Content = "Start";
                else
                    StartStopButton.Content = "Stop";
                StartStopButton.IsEnabled = !string.IsNullOrEmpty(OPSManager.Settings.Username) && !string.IsNullOrEmpty(OPSManager.Settings.Password);
            }));
        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            OPSManager.Toggle();
        }
    }
}
