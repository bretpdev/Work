using System;
using System.Collections;
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
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using System.Reflection;
using System.Globalization;
using System.Runtime.Remoting;
using Uheaa.Common;

namespace SessionTester
{
    /// <summary>
    /// Interaction logic for ScriptList.xaml
    /// </summary>
    public partial class ScriptList : UserControl
    {
        const int minRows = 20;
        public ScriptList()
        {
            InitializeComponent();
        }

        private IEnumerable<iDll> itemSource;
        public IEnumerable<iDll> ItemsSource
        {
            get { return itemSource; }
            set {
                itemSource = value;
                for (int i = itemSource.Count(); i < minRows; i++)
                {
                    FakeDll fake = new FakeDll();
                    itemSource = itemSource.Concat(fake.MakeArray());
                }
                Items.ItemsSource = itemSource; 
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var dll = (sender as Button).Tag as Dll;
            if (MessageBox.Show("Are you sure you want to remove " + dll.ContainingFolder + "?", "Really delete?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                dll.DeleteContainingFolder();
                ItemsSource = ItemsSource.Where(o => o != dll);
            }
        }

        public string Header
        {
            get { return HeaderLabel.Content.ToString(); }
            set { HeaderLabel.Content = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dll = (sender as Button).Tag as Dll;
            if (dll == null) return;
            ScriptHelper.StartScript(dll);
        }
    }
}
