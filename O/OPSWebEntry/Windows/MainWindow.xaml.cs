using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Control = System.Windows.Controls.Control;
using System.Collections;
using System.Reflection;

namespace OPSWebEntry
{

    public partial class MainWindow : TitleWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            TestMenu.Visibility = (ModeHelper.IsTest || ModeHelper.IsLiveTest) ? Visibility.Visible : Visibility.Collapsed;
            //string error = DatabaseAccessHelper.GenerateSprocAccessAlert(Assembly.GetExecutingAssembly());
            //if (!error.IsNullOrEmpty())
            //    MessageBox.Show(error);
            this.Title = this.Title;
            EventHandler handler = (sender, args) =>
            {
                this.WindowState = System.Windows.WindowState.Normal;
                this.Show();
                this.ShowInTaskbar = true;
            };

            ActivityLog.TrayIcon.DoubleClick += handler;
            ActivityLog.TrayIcon.Click += handler;
            ActivityLog.TrayIcon.BalloonTipClicked += handler;

            ActivityLogList.ItemsSource = ActivityLog.Log;

            LoadSettings();

            ActivityLog.LogNotification("Welcome to OPS Web Entry.");
        }

        bool loadingSettings = false;
        private void LoadSettings()
        {
            loadingSettings = true;

            OPSManager.Settings.Reload();
            UsernameText.Text = OPSManager.Settings.Username;
            Password.Password = OPSManager.Settings.Password;
            TrayNotificationButton.IsChecked = OPSManager.Settings.TrayNotifications;
            TrayMinimizeButton.IsChecked = OPSManager.Settings.TrayMinimize;

            loadingSettings = false;
        }

        private void SetSettings()
        {
            OPSManager.Settings.Username = UsernameText.Text;
            OPSManager.Settings.Password = Password.Password;
            OPSManager.Settings.TrayNotifications = TrayNotificationButton.IsChecked.Value;
            OPSManager.Settings.TrayMinimize = TrayMinimizeButton.IsChecked.Value;
            CurrentStatus.Sync();
        }
        private void SaveSettings()
        {
            SetSettings();
            OPSManager.Settings.Save();
        }

        private void Settings_ValueChanged(object sender, EventArgs e)
        {
            if (loadingSettings) return;
            SaveSettingsBox.IsEnabled = false;
            foreach (var control in SettingsBox.Children.Filter<IStateAwareControl>())
                if (control.IsChanged)
                    SaveSettingsBox.IsEnabled = true;
            SetSettings();
        }

        private void SettingsCancelButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var control in SettingsBox.Children.Filter<IStateAwareControl>())
                control.CancelChanges();
            LoadSettings();
            CurrentStatus.Sync();
        }

        private void SettingsSaveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var control in SettingsBox.Children.Filter<IStateAwareControl>())
                control.AcceptChanges();
            SaveSettings();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to quit OPS Web Entry?", "Quit?", MessageBoxButton.YesNo) == MessageBoxResult.No)
                e.Cancel = true;
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (OPSManager.Settings.TrayMinimize)
                this.ShowInTaskbar = false;
        }

        private void AddPayment_Click(object sender, RoutedEventArgs e)
        {
            AddRecord ar = new AddRecord();
            ar.ShowDialog();
        }

        private void ManualProcessing_Click(object sender, RoutedEventArgs e)
        {
            ManualMode mm = new ManualMode();
            mm.ShowDialog();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (loadingSettings) return;
            SaveSettingsBox.IsEnabled = true;
            SetSettings();
        }

        private void Last7_Click(object sender, RoutedEventArgs e)
        {
            Last7 last = new Last7();
            last.ShowDialog();
        }
    }
}
