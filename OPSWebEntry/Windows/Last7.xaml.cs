using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for Last7.xaml
    /// </summary>
    public partial class Last7 : Window
    {
        public Last7()
        {
            InitializeComponent();
        }

        private void Last7_Loaded(object sender, RoutedEventArgs e)
        {
            Results.ItemsSource = OPSPayment.GetLast7Days();
        }
    }
}
