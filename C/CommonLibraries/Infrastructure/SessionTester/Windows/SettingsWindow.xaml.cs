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
using System.Windows.Shapes;

namespace SessionTester
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            this.DataContext = SettingsHelper.Instance;
            CornerStonePasswordText.Password = SettingsHelper.Instance.CornerStonePassword;
            UheaaPasswordText.Password = SettingsHelper.Instance.UheaaPassword;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            SettingsHelper.Instance.RevertPendingChanges();
        }

        private void CornerStonePasswordText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SettingsHelper.Instance.CornerStonePassword = CornerStonePasswordText.Password;
        }

        private void UheaaPasswordText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SettingsHelper.Instance.UheaaPassword = UheaaPasswordText.Password;
        }

        private void SaveLoginButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsHelper.Instance.SaveLoginSettings();
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsHelper.Instance.SaveNormalSettings();
        }
    }
}
