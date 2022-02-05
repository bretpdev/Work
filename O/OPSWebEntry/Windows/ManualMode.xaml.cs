using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OPSWebEntry
{
    /// <summary>
    /// Interaction logic for ManualMode.xaml
    /// </summary>
    public partial class ManualMode : Window
    {
        public ManualMode()
        {
            InitializeComponent();
        }

        List<OPSPayment> payments;
        private void ManualMode_Loaded(object sender, RoutedEventArgs e)
        {
            payments = OPSPayment.GetPendingPayments();
            ManualResults.ItemsSource = payments;
        }

        private void ManualResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ManualResults.SelectedIndex >= 0)
            {
                payments = new List<OPSPayment>(new OPSPayment[] { payments[ManualResults.SelectedIndex] });
                this.IsEnabled = false;
                Cursor = Cursors.Wait;
                new Thread(() =>
                {
                    OPSManager.ProcessPayments(new Server(), payments);
                    OPSManager.Stop();
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Close();
                    }));
                }).Start();
            }
        }
    }
}
