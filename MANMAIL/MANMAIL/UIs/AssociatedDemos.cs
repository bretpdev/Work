using System;
using System.Windows.Forms;
using Uheaa.Common;

namespace MANMAIL
{
    public partial class AssociatedDemos : UserControl
    {
        public string AccountIdentifier { get; set; }
        public event EventHandler OnUserControllButtonClicked;

        public AssociatedDemos(AssociatedAccounts account)
        {
            InitializeComponent();
            AcctIdentifier.Text = account.AccountIdentifier;
            AccountName.Text = account.Name;
            Address1.Text = account.Address1;
            if (account.Address2.IsNullOrEmpty())
                Address2.Text = string.Format("{0}, {1} {2}", account.City, account.State, account.Zip);
            else
            {
                Address2.Text = account.Address2;
                CityStateZip.Text = string.Format("{0}, {1} {2}", account.City, account.State, account.Zip);
            }
            AccountIdentifier = account.AccountIdentifier;
            RegionLbl.Text = account.Region;

            SelectCbo.Click += (s, e) => OnUserControllButtonClicked?.Invoke(this, e);
        }
    }
}