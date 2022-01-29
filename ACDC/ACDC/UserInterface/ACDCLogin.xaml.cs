using System.Windows;
using System.Windows.Controls;

namespace ACDC
{
    partial class ACDCLogin : UserControl
    {
		public ACDCLogin()
		{
			InitializeComponent();
			txtUserName.Focus();
		}

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
			ACDCMain mainWindow = Window.GetWindow(this) as ACDCMain;
			mainWindow.AuthenticateUser();
			mainWindow.MakeOptionsVisibleAccordingToAccess();
        }
    }
}
