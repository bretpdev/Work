using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;

namespace BCSRETMAIL
{
    public partial class ForwardingAddress : Form
    {
        private Account Compass { get; set; }
        private Account Onelink { get; set; }
        public Account NewAddress { get; set; }
        public Account InvalidAddress { get; set; }
        public bool OlInvalidate { get; set; }
        public bool OlForward { get; set; }
        public bool CpInvalidate { get; set; }
        public bool CpForward { get; set; }


        public ForwardingAddress(List<Account> accounts, Account newAddress, DataAccess da)
        {
            InitializeComponent();
            State.SelectedIndex = -1;
            NewAddress = newAddress;
            Compass = accounts.Where(p => p.System == BarcodeInfo.SystemType.Compass).FirstOrDefault();
            Onelink = accounts.Where(p => p.System == BarcodeInfo.SystemType.Onelink).FirstOrDefault();
            GetStateCodes(da);
            LoadLabels();
        }

        private void GetStateCodes(DataAccess da)
        {
            List<string> states = da.GetStateCodes();
            states.Insert(0, "");
            State.DataSource = states;
            InvalidState.DataSource = new List<string>(states);
        }

        private void LoadLabels()
        {
            if (Compass != null)
            {
                CompassVisible();
                CompassDemos.Enabled = true;
                CompassAddress1.Text = Compass.Address1;
                CompassAddress2.Text = Compass.Address2;
                CompassCityStZip.Text = $"{Compass.City}, {Compass.State} {Compass.ZipCode}";
                CompassAddressValid.Text = Compass.IsValidAddress;
                CompassAddressDate.Text = Compass.AddressValidityDate;
            }
            if (Onelink != null)
            {
                OnelinkVisible();
                OnelinkDemos.Enabled = true;
                OnelinkAddress1.Text = Onelink.Address1;
                OnelinkAddress2.Text = Onelink.Address2;
                OnelinkCityStZip.Text = $"{Onelink.City}, {Onelink.State} {Onelink.ZipCode}";
                OnelinkAddressValid.Text = Onelink.IsValidAddress;
                OnelinkAddressDate.Text = Onelink.AddressValidityDate;
            }
            AccountNumber.Text = Compass?.AccountNumber ?? Onelink?.AccountNumber;
            BorrowerName.Text = $"{Compass?.FirstName ?? Onelink?.FirstName} {Compass?.LastName ?? Onelink?.LastName}";
            if (NewAddress != null)
            {
                Address1.Text = NewAddress.Address1;
                Address2.Text = NewAddress.Address2;
                City.Text = NewAddress.City;
                State.Text = NewAddress.State;
                Zip.Text = NewAddress.ZipCode;
            }
        }

        private void CompassVisible()
        {
            CompassAddress1.Visible = true;
            CompassAddress2.Visible = true;
            CompassCityStZip.Visible = true;
            CompassAddressValid.Visible = true;
            CompassAddressDate.Visible = true;
        }

        private void OnelinkVisible()
        {
            OnelinkAddress1.Visible = true;
            OnelinkAddress2.Visible = true;
            OnelinkCityStZip.Visible = true;
            OnelinkAddressValid.Visible = true;
            OnelinkAddressDate.Visible = true;
        }

        private void CompassForward_Click(object sender, EventArgs e)
        {
            CpForward = true;
            OlForward = false;
            CompassInvalidate.Enabled = false;
            OnelinkInvalidate.Enabled = true;
            CpForward = true;
            Address1.Text = Compass.Address1;
            Address2.Text = Compass.Address2;
            City.Text = Compass.City;
            Zip.Text = Compass.ZipCode;
            State.Text = Compass.State;
            this.Refresh();
        }

        private void OnelinkForward_Click(object sender, EventArgs e)
        {
            OlForward = true;
            CpForward = false;
            OnelinkInvalidate.Enabled = false;
            CompassInvalidate.Enabled = true;
            OlForward = true;
            Address1.Text = Onelink.Address1;
            Address2.Text = Onelink.Address2;
            City.Text = Onelink.City;
            Zip.Text = Onelink.ZipCode;
            State.Text = Onelink.State;
            this.Refresh();
        }

        private void CompassInvalidate_Click(object sender, EventArgs e)
        {
            CpInvalidate = true;
            OlInvalidate = false;
            CompassForward.Enabled = false;
            OnelinkForward.Enabled = true;
            CpInvalidate = true;
            InvalidAddress1.Text = Compass.Address1;
            InvalidAddress2.Text = Compass.Address2;
            InvalidCity.Text = Compass.City;
            InvalidState.Text = Compass.State;
            InvalidZip.Text = Compass.ZipCode;
            this.Refresh();
        }

        private void OnelinkInvalidate_Click(object sender, EventArgs e)
        {
            OlInvalidate = true;
            CpInvalidate = false;
            OnelinkForward.Enabled = false;
            CompassForward.Enabled = true;
            OlInvalidate = true;
            InvalidAddress1.Text = Onelink.Address1;
            InvalidAddress2.Text = Onelink.Address2;
            InvalidCity.Text = Onelink.City;
            InvalidState.Text = Onelink.State;
            InvalidZip.Text = Onelink.ZipCode;
            this.Refresh();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                NewAddress.Address1 = Address1.Text;
                NewAddress.Address2 = Address2.Text;
                NewAddress.City = City.Text;
                NewAddress.State = State.Text;
                NewAddress.ZipCode = Zip.Text;
                if (InvalidAddress1.Text.IsPopulated() && NewAddress.Address1.IsPopulated())
                {
                    InvalidAddress = new Account
                    {
                        Address1 = InvalidAddress1.Text,
                        Address2 = InvalidAddress2.Text,
                        City = InvalidCity.Text,
                        State = InvalidState.Text,
                        ZipCode = InvalidZip.Text
                    };
                }
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateFields()
        {
            if ((Address1.Text.IsPopulated() && !(City.Text.IsPopulated() && State.Text.IsPopulated() && Zip.Text.IsPopulated()))
                || (City.Text.IsPopulated() && !(Address1.Text.IsPopulated() && State.Text.IsPopulated() && Zip.Text.IsPopulated()))
                || (State.Text.IsPopulated() && !(Address1.Text.IsPopulated() && City.Text.IsPopulated() && Zip.Text.IsPopulated()))
                || ((Zip.Text.IsPopulated() && !(Address1.Text.IsPopulated() && City.Text.IsPopulated() && State.Text.IsPopulated())) || (Zip.Text.IsPopulated() && Zip.Text.Length < 5)))
            {
                Dialog.Error.Ok("Please provide all required fields when updating the forwarding address.");
                return false;
            }
            if ((InvalidAddress1.Text.IsPopulated() && !(InvalidCity.Text.IsPopulated() && InvalidState.Text.IsPopulated() && InvalidZip.Text.IsPopulated()))
                || (InvalidCity.Text.IsPopulated() && !(InvalidAddress1.Text.IsPopulated() && InvalidState.Text.IsPopulated() && InvalidZip.Text.IsPopulated()))
                || (InvalidState.Text.IsPopulated() && !(InvalidAddress1.Text.IsPopulated() && InvalidCity.Text.IsPopulated() && InvalidZip.Text.IsPopulated()))
                || ((InvalidZip.Text.IsPopulated() && !(InvalidAddress1.Text.IsPopulated() && InvalidCity.Text.IsPopulated() && InvalidState.Text.IsPopulated())) || (InvalidZip.Text.IsPopulated() && InvalidZip.Text.Length < 5)))
            {
                Dialog.Error.Ok("Please provide all required fields when updating the address to invalidate.");
                return false;
            }
            return true;
        }

        private void Rules_Click(object sender, EventArgs e)
        {
            using AddressHygiene hygiene = new AddressHygiene();
            hygiene.ShowDialog();
        }

        private void ForwardClear_Click(object sender, EventArgs e)
        {
            CompassInvalidate.Enabled = true;
            OnelinkInvalidate.Enabled = true;
            CpForward = false;
            OlForward = false;
            if (CpInvalidate)
            {
                CompassForward.Enabled = false;
                OnelinkForward.Enabled = true;
            }
            if (OlInvalidate)
            {
                OnelinkForward.Enabled = false;
                CompassForward.Enabled = true;
            }
            Address1.Text = "";
            Address2.Text = "";
            City.Text = "";
            State.SelectedIndex = 0;
            Zip.Text = "";
        }

        private void InvalidateClear_Click(object sender, EventArgs e)
        {
            CompassForward.Enabled = true;
            OnelinkForward.Enabled = true;
            CpInvalidate = false;
            OlInvalidate = false;
            if (CpForward)
            {
                CompassInvalidate.Enabled = false;
                OnelinkInvalidate.Enabled = true;
            }
            if (OlForward)
            {
                OnelinkInvalidate.Enabled = false;
                CompassInvalidate.Enabled = true;
            }
            InvalidAddress1.Text = "";
            InvalidAddress2.Text = "";
            InvalidCity.Text = "";
            InvalidState.SelectedIndex = 0;
            InvalidZip.Text = "";
        }

        private void Address1_TextChanged(object sender, EventArgs e)
        {
            SetForwardingColors();
        }

        private void Address2_TextChanged(object sender, EventArgs e)
        {
            SetForwardingColors();
        }

        private void City_TextChanged(object sender, EventArgs e)
        {
            SetForwardingColors();
        }

        private void State_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetForwardingColors();
        }

        private void Zip_TextChanged(object sender, EventArgs e)
        {
            SetForwardingColors();
        }

        public void SetForwardingColors()
        {
            SetAddress1Color();
            SetAddress2Color();
            SetCityColor();
            SetStateColor();
            SetZipColor();
        }

        public void SetAddress1Color()
        {
            if (Address1.Text.IsNullOrEmpty() && !(Address2.Text.IsPopulated() || City.Text.IsPopulated() || State.Text.IsPopulated() || Zip.Text.IsPopulated()))
                Address1.BackColor = SystemColors.Window;
            else if (Address1.Text.IsNullOrEmpty() && (Address2.Text.IsPopulated() || City.Text.IsPopulated() || State.Text.IsPopulated() || Zip.Text.IsPopulated()))
                Address1.BackColor = Color.LightPink;
            else
                Address1.BackColor = Color.LightGreen;
        }

        public void SetAddress2Color()
        {
            if (Address2.Text.IsNullOrEmpty() && !(Address1.Text.IsPopulated() || City.Text.IsPopulated() || State.Text.IsPopulated() || Zip.Text.IsPopulated()))
                Address2.BackColor = SystemColors.Window;
            else if (Address2.Text.IsPopulated())
                Address2.BackColor = Color.LightGreen;
        }

        public void SetCityColor()
        {
            if (City.Text.IsNullOrEmpty() && !(Address1.Text.IsPopulated() || Address2.Text.IsPopulated() || State.Text.IsPopulated() || Zip.Text.IsPopulated()))
                City.BackColor = SystemColors.Window;
            else if (City.Text.IsNullOrEmpty() && (Address1.Text.IsPopulated() || Address2.Text.IsPopulated() || State.Text.IsPopulated() || Zip.Text.IsPopulated()))
                City.BackColor = Color.LightPink;
            else
                City.BackColor = Color.LightGreen;
        }

        public void SetStateColor()
        {
            if (State.Text.IsNullOrEmpty() && !(Address1.Text.IsPopulated() || Address2.Text.IsPopulated() || City.Text.IsPopulated() || Zip.Text.IsPopulated()))
                State.BackColor = SystemColors.Window;
            else if (State.Text.IsNullOrEmpty() && (Address1.Text.IsPopulated() || Address2.Text.IsPopulated() || City.Text.IsPopulated() || Zip.Text.IsPopulated()))
                State.BackColor = Color.LightPink;
            else
                State.BackColor = Color.LightGreen;
        }

        public void SetZipColor()
        {
            if (Zip.Text.IsNullOrEmpty() && !(Address1.Text.IsPopulated() || Address2.Text.IsPopulated() || City.Text.IsPopulated() || State.Text.IsPopulated()))
                Zip.BackColor = SystemColors.Window;
            else if ((Zip.Text.IsNullOrEmpty() && (Address1.Text.IsPopulated() || Address2.Text.IsPopulated() || City.Text.IsPopulated() || State.Text.IsPopulated())) || Zip.Text.Length < 5)
                Zip.BackColor = Color.LightPink;
            else if (Zip.Text.Length >= 5)
                Zip.BackColor = Color.LightGreen;
        }

        private void InvalidAddress1_TextChanged(object sender, EventArgs e)
        {
            SetInvalidColors();
        }

        private void InvalidAddress2_TextChanged(object sender, EventArgs e)
        {
            SetInvalidColors();
        }

        private void InvalidCity_TextChanged(object sender, EventArgs e)
        {
            SetInvalidColors();
        }

        private void InvalidState_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetInvalidColors();
        }

        private void InvalidZip_TextChanged(object sender, EventArgs e)
        {
            SetInvalidColors();
        }

        public void SetInvalidColors()
        {
            SetInvalidAddress1Color();
            SetInvalidAddress2Color();
            SetInvalidCityColor();
            SetInvalidStateColor();
            SetInvalidZipColor();
        }

        public void SetInvalidAddress1Color()
        {
            if (InvalidAddress1.Text.IsNullOrEmpty() && !(InvalidAddress2.Text.IsPopulated() || InvalidCity.Text.IsPopulated() || InvalidState.Text.IsPopulated() || InvalidZip.Text.IsPopulated()))
                InvalidAddress1.BackColor = SystemColors.Window;
            else if (InvalidAddress1.Text.IsNullOrEmpty() && (InvalidAddress2.Text.IsPopulated() || InvalidCity.Text.IsPopulated() || InvalidState.Text.IsPopulated() || InvalidZip.Text.IsPopulated()))
                InvalidAddress1.BackColor = Color.LightPink;
            else
                InvalidAddress1.BackColor = Color.LightGreen;
        }

        public void SetInvalidAddress2Color()
        {
            if (InvalidAddress2.Text.IsNullOrEmpty() && !(InvalidAddress1.Text.IsPopulated() || InvalidCity.Text.IsPopulated() || InvalidState.Text.IsPopulated() || InvalidZip.Text.IsPopulated()))
                InvalidAddress2.BackColor = SystemColors.Window;
            else if (InvalidAddress2.Text.IsPopulated())
                InvalidAddress2.BackColor = Color.LightGreen;
        }

        public void SetInvalidCityColor()
        {
            if (InvalidCity.Text.IsNullOrEmpty() && !(InvalidAddress1.Text.IsPopulated() || InvalidAddress2.Text.IsPopulated() || InvalidState.Text.IsPopulated() || InvalidZip.Text.IsPopulated()))
                InvalidCity.BackColor = SystemColors.Window;
            else if (InvalidCity.Text.IsNullOrEmpty() && (InvalidAddress1.Text.IsPopulated() || InvalidAddress2.Text.IsPopulated() || InvalidState.Text.IsPopulated() || InvalidZip.Text.IsPopulated()))
                InvalidCity.BackColor = Color.LightPink;
            else
                InvalidCity.BackColor = Color.LightGreen;
        }

        public void SetInvalidStateColor()
        {
            if (InvalidState.Text.IsNullOrEmpty() && !(InvalidAddress1.Text.IsPopulated() || InvalidAddress2.Text.IsPopulated() || InvalidCity.Text.IsPopulated() || InvalidZip.Text.IsPopulated()))
                InvalidState.BackColor = SystemColors.Window;
            else if (InvalidState.Text.IsNullOrEmpty() && (InvalidAddress1.Text.IsPopulated() || InvalidAddress2.Text.IsPopulated() || InvalidCity.Text.IsPopulated() || InvalidZip.Text.IsPopulated()))
                InvalidState.BackColor = Color.LightPink;
            else
                InvalidState.BackColor = Color.LightGreen;
        }

        public void SetInvalidZipColor()
        {
            if (InvalidZip.Text.IsNullOrEmpty() && !(InvalidAddress1.Text.IsPopulated() || InvalidAddress2.Text.IsPopulated() || InvalidCity.Text.IsPopulated() || InvalidState.Text.IsPopulated()))
                InvalidZip.BackColor = SystemColors.Window;
            else if ((InvalidZip.Text.IsNullOrEmpty() && (InvalidAddress1.Text.IsPopulated() || InvalidAddress2.Text.IsPopulated() || InvalidCity.Text.IsPopulated() || InvalidState.Text.IsPopulated())) || InvalidZip.Text.Length < 5)
                InvalidZip.BackColor = Color.LightPink;
            else if (InvalidZip.Text.Length >= 5)
                InvalidZip.BackColor = Color.LightGreen;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            NewAddress = null;
        }
    }
}