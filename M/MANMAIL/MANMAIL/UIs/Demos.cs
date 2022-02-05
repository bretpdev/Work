using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;

namespace MANMAIL
{
    public partial class Demos : Form
    {
        private bool Forwarding { get; set; }
        public bool InvalidAddress { get; set; }
        public AddressData BadAddress { get; set; }
        public AddressData ForwardingAddress { get; set; }
        public DataAccess DA { get; set; }
        public AddressRegion SelectedRegion { get; set; }

        public enum AddressRegion
        {
            Onelink,
            Compass,
            Both
        }

        public Demos(string ssn, string accountNumber, string name, AddressData olDemos, AddressData cDemos, bool forwarding, DataAccess da)
        {
            InitializeComponent();
            DA = da;
            Forwarding = forwarding;
            SSN.Text = ssn;
            AccountNumber.Text = accountNumber;
            BorName.Text = name;
            SetOlDemos(olDemos);
            SetCompassDemos(cDemos);
            ForwardingAddress = null;
            SetStates();
        }

        private void SetStates()
        {
            List<string> states = DA.GetStates();
            states.Insert(0, "");
            State.DataSource = states;
        }

        private void SetOlDemos(AddressData demos)
        {
            if (demos == null)
            {
                OLSelect.Enabled = false;
                return;
            }

            OA1.Text = demos.Address1;
            OA2.Text = demos.Address2;
            OCSZ.Text = string.Format("{0}|{1}|{2}", demos.City, demos.State, demos.Zip);
            OAV.Text += demos.AddressIsValid;
            OAED.Text += demos.AddressVerifiedDate;
        }

        private void SetCompassDemos(AddressData demos)
        {
            if (demos == null)
            {
                CSelect.Enabled = false;
                return;
            }

            CA1.Text = demos.Address1;
            CA2.Text = demos.Address2;
            CCSZ.Text = string.Format("{0}|{1}|{2}", demos.City, demos.State, demos.Zip);
            CAV.Text += demos.AddressIsValid;
            CAED.Text += demos.AddressVerifiedDate;
        }

        private void OLSelect_Click(object sender, EventArgs e)
        {
            Address1.Text = OA1.Text;
            Address2.Text = OA2.Text;
            List<string> cityStateZip = OCSZ.Text.SplitAndRemoveQuotes("|");
            City.Text = cityStateZip[0];
            State.Text = cityStateZip[1];
            Zip.Text = cityStateZip[2];
            Valid.Text = OAV.Text.Last().ToString();
            SetRegion(AddressRegion.Onelink);
        }

        private void CSelect_Click(object sender, EventArgs e)
        {
            Address1.Text = CA1.Text;
            Address2.Text = CA2.Text;
            List<string> cityStateZip = CCSZ.Text.SplitAndRemoveQuotes("|");
            City.Text = cityStateZip[0];
            State.Text = cityStateZip[1];
            Zip.Text = cityStateZip[2];
            Valid.Text = CAV.Text.Last().ToString();
            SetRegion(AddressRegion.Compass);
        }

        private void SetRegion(AddressRegion region)
        {
            if (OA1.Text == CA1.Text && OA2.Text == CA2.Text && OCSZ.Text == CCSZ.Text)
                SelectedRegion = AddressRegion.Both;
            else
                SelectedRegion = region;
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            if (Address1.Text.IsNullOrEmpty() || City.Text.IsNullOrEmpty() || State.Text.IsNullOrEmpty() || Zip.Text.IsNullOrEmpty() || Zip.Text.Length < 5)
            {
                Dialog.Error.Ok("You must enter a valid address.");
                return;
            }

            if (IAddress1.Text == "Invalid Address 1")
            {
                if (Dialog.Info.YesNo("Is this the address the return mail was received from?"))
                {
                    if (Valid.Text == "Y"  && !InvalidAddress)
                    {
                        Dialog.Info.Ok("The address will be invalidated.");
                        InvalidAddress = true;
                    }
                    if (!Forwarding)
                        DialogResult = DialogResult.OK;

                    SetInvalidAddress();
                    if(Forwarding)
                        Dialog.Info.Ok("Please enter the forwarding address");
                }
            }
            else
            {
                SetForwardingAddress();
                DialogResult = DialogResult.OK;
            }
        }

        private void SetInvalidAddress()
        {
            IAddress1.Text = Address1.Text;
            IAddress2.Text = Address2.Text;
            ICityStateZip.Text = string.Format("{0}|{1}|{2}", City.Text, State.Text, Zip.Text);
            SetBadAddress();

            Address1.Text = "";
            Address2.Text = "";
            City.Text = "";
            State.SelectedIndex = -1;
            Zip.Text = "";
        }

        private void SetForwardingAddress()
        {
            ForwardingAddress = new AddressData()
            {
                Address1 = Address1.Text,
                Address2 = Address2.Text,
                City = City.Text,
                State = State.Text,
                Zip = Zip.Text,
                Ssn = SSN.Text,
                AccountNumber = AccountNumber.Text
            };
        }

        private void SetBadAddress()
        {
            BadAddress = new AddressData()
            {
                Address1 = Address1.Text,
                Address2 = Address2.Text,
                City = City.Text,
                State = State.Text,
                Zip = Zip.Text,
                AddressIsValid = Valid.Text,
                Ssn = SSN.Text,
                AccountNumber = AccountNumber.Text
            };
        }

        private void Address1_TextChanged(object sender, EventArgs e)
        {
            if (Address1.Text.Contains("!", "@", "$", "%", "^", "&", "*", "(", ")", "-", "+", "=", "<", ">", ",", ".", @"""", ";", ":", "~", "`", "?"))
                Address1.Text = Address1.Text.Replace("", "!", "@", "$", "%", "^", "&", "*", "(", ")", "-", "+", "=", "<", ">", ",", ".", @"""", ";", ":", "~", "`", "?");
        }

        private void Address2_TextChanged(object sender, EventArgs e)
        {
            if (Address2.Text.Contains("!", "@", "$", "%", "^", "&", "*", "(", ")", "-", "+", "=", "<", ">", ",", ".", @"""", ";", ":", "~", "`", "?"))
               Address2.Text = Address2.Text.Replace("", "!", "@", "$", "%", "^", "&", "*", "(", ")", "-", "+", "=", "<", ">", ",", ".", @"""", ";", ":", "~", "`", "?");
        }

        private void Rules_Click(object sender, EventArgs e)
        {
            using (TheRules dr = new TheRules())
            {
                dr.ShowDialog();
            }
        }
    }
}