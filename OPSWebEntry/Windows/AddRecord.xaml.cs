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
using Uheaa.Common;

namespace OPSWebEntry
{
    /// <summary>
    /// Interaction logic for AddRecord.xaml
    /// </summary>
    public partial class AddRecord : TitleWindow
    {
        public AddRecord()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OPSPayment payment = new OPSPayment();
                payment.SSN = SSNText.Text;
                payment.Name = NameText.Text;
                payment.DOB = DOBText.Text.ToDate();
                payment.ABA = ABAText.Text;
                payment.BankAccountNumber = LegacyCryptography.Encrypt(AccountNumberText.Text, LegacyCryptography.Keys.NoradOPS);
                payment.AccountType = (AccountTypeBox.SelectedItem as ComboBoxItem).Content.ToString()[0].ToString();
                payment.Amount = AmountText.Text.ToDecimal();
                payment.EffectiveDate = EffectiveDateText.Text.ToDate();
                payment.AccountHolderName = HolderNameText.Text;
                payment.Insert();
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                //TODO: Real error checking.
                MessageBox.Show("There was an error adding your payment.  Please ensure all fields are valid.  Error Message: " + ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
