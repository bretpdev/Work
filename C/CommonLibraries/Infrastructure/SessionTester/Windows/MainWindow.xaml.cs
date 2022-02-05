using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace SessionTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            var ffel = ScriptHelper.FfelScripts;
            InitializeComponent();
            FfelList.ItemsSource = ffel;
            FfelList.Header = "Commercial";

            var fed = ScriptHelper.FedScripts;
            FedList.ItemsSource = fed;
            FedList.Header = "Federal";

            if (SettingsHelper.Instance.AutoStartEnabled)
            {
                var youngest = ffel.Concat(fed).OrderByDescending(o => o.LastModified).FirstOrDefault();
                if (youngest != null)
                {
                    string format = "Starting {0} in {1} seconds...";
                    MenuItem mi = new MenuItem();
                    mi.Foreground = Brushes.Green;
                    mi.FontWeight = FontWeights.Bold;
                    var separator = new Separator();
                    separator.Width = 4;
                    MainMenu.Items.Add(separator);
                    bool cancel = false;
                    Thread t = new Thread(() =>
                    {
                        int secondsLeft = SettingsHelper.Instance.AutoStartSeconds;

                        while (secondsLeft > 0 && !cancel)
                        {
                            try
                            {
                                mi.Dispatcher.Invoke(new Action(() => mi.Header = string.Format(format, youngest.DisplayName, secondsLeft)));
                            }
                            catch (TaskCanceledException)
                            {
                                cancel = true;
                                break;
                            }
                            Sleep.ForOneSecond();
                            secondsLeft--;
                        }
                        if (!cancel)
                        {
                            mi.Dispatcher.Invoke(new Action(() =>
                            {
                                MainMenu.Items.Remove(mi);
                                MainMenu.Items.Remove(separator);
                            }));
                            ScriptHelper.StartScript(youngest);
                        }
                    });
                    t.SetApartmentState(ApartmentState.STA);
                    var cancelItem = new MenuItem() { Header = "Cancel", Foreground = Brushes.Black, FontWeight = FontWeights.Normal };
                    cancelItem.Click += (o, ea) =>
                    {
                        cancel = true;
                        MainMenu.Items.Remove(mi);
                        MainMenu.Items.Remove(separator);
                    };
                    var startItem = new MenuItem() { Header = "Start Now", Foreground = Brushes.Black, FontWeight = FontWeights.Normal };
                    startItem.Click += (o, ea) =>
                    {
                        cancel = true;
                        MainMenu.Items.Remove(mi);
                        MainMenu.Items.Remove(separator);
                        ScriptHelper.StartScript(youngest);
                    };

                    mi.Items.Add(cancelItem);
                    mi.Items.Add(startItem);
                    MainMenu.Items.Add(mi);
                    

                    t.Start();
                }
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
        }
    }
}
