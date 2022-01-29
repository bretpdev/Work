using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace MDIntermediary
{
    public partial class AlternateAddressForm : Form
    {
        string accountNumber;
        private List<AlternateAddress> AltAddresses;
        private List<AlternateAddress> Deleted = new List<AlternateAddress>();
        private DataAccessHelper.Region region;
        public AlternateAddressForm(DataAccessHelper.Region region, string accountNumber)
        {
            InitializeComponent();

            this.region = region;
            this.accountNumber = accountNumber;
            AltAddresses = AlternateAddress.GetAltAddresses(region, accountNumber);
            Rebind();
        }

        private void LoadAddress(AlternateAddress address)
        {
            var bind = new Action<TextBox, string>((t, s) =>
            {
                t.DataBindings.Clear();
                t.DataBindings.Add(new Binding("Text", address, s));
            });
            bind(Address1Box, "Address1");
            bind(Address2Box, "Address2");
            bind(CityBox, "City");
            bind(StateBox, "State");
            bind(ZipBox, "Zip");
            bind(CountryBox, "Country");
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void AddressesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var address = AddressesList.SelectedItem as AlternateAddress;
            bool exists = address != null;
            DeleteButton.Enabled = exists;
            Address1Box.Enabled = Address2Box.Enabled = CityBox.Enabled = StateBox.Enabled = CountryBox.Enabled = ZipBox.Enabled = exists;
            LoadAddress(address);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var address = AddressesList.SelectedItem as AlternateAddress;
            if (address.AlternateAddressId != 0)
                Deleted.Add(address);
            AltAddresses.Remove(address);
            Rebind();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AltAddresses.Add(new AlternateAddress() { Address1 = "NEW ADDRESS", AccountNumber = accountNumber });
            Rebind();
            AddressesList.SelectedIndex = AltAddresses.Count - 1;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            foreach (var address in AltAddresses)
            {
                if (address.AlternateAddressId == 0) //add
                    AlternateAddress.AddNew(region, address);
                else
                    AlternateAddress.Update(region, address);
            }
            foreach (var deleted in Deleted)
            {
                AlternateAddress.Delete(region, deleted.AlternateAddressId);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void Rebind()
        {
            AddressesList.DataSource = AltAddresses.ToArray();
        }

        private void Address1Box_TextChanged(object sender, EventArgs e)
        {
            //Rebind();
        }
    }
}
