using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace LENDERLTRS
{
    public partial class LenderForm : Form
    {
        public LenderData UpdatedData { get; set; }

        public LenderForm(LenderData data)
        {
            InitializeComponent();
            LoadForm(data);
            LenderType.SelectedIndex = -1;
            if (data.Valid)
                DisableForm();
        }

        private void DisableForm()
        {
            FullName.Enabled = false;
            ShortName.Enabled = false;
            Street1.Enabled = false;
            Street2.Enabled = false;
            City.Enabled = false;
            State.Enabled = false;
            ZipCode.Enabled = false;
            StaOpen.Checked = true;
            StaOpen.Enabled = false;

            // Do not allow BANA to be closed.
            //if(LenderId.Text == "814817")
                StaClosed.Enabled = false;
        }

        private void LoadForm(LenderData data)
        {
            Mod.Text = data.Mod;
            LenderId.Text = data.LenderId;
            FullName.Text = data.FullName;
            ShortName.Text = data.ShortName;
            Street1.Text = data.Address1;
            Street2.Text = data.Address2;
            City.Text = data.City;
            State.Text = data.State;
            ZipCode.Text = data.Zip;
            if (Mod.Text != "A")
                LenderType.Enabled = false;
            else
                LenderType.DataSource = new List<string>() { "01 – COMMERCIAL LENDER", "02 – CREDIT UNION", "03 – EDUCATIONAL INSTIT", "04 – FEDERAL SAV & LOAN", "05 – SECONDARY MARKET", "06 – MUTUAL SAVINGS", "07 – STATE SAV & LOAN", "08 – INSURANCE COMPANY", "09 – PRIVATE INSTITUTION", "10 – WAREHOUSE" };
        }

        private bool ValidateInput()
        {
            List<string> errors = new List<string>();
            if (!StaOpen.Checked && !StaClosed.Checked)
                errors.Add("You must select a status.");
            if (Mod.Text == "A" && LenderType.SelectedIndex == -1)
                errors.Add("You must select a Lender Type when MOD equals A");
            if (State.Text.Length != 2)
                errors.Add("The State must be 2 characters.");
            if (ZipCode.Text.Length < 5)
                errors.Add("The Zip Code must be at least 5 numbers.");

            if (errors.Any())
            {
                Dialog.Error.Ok(string.Format("Please review the following errors: {1} {0}", string.Join(Environment.NewLine, errors), Environment.NewLine));
                return false;
            }

            return true;
        }

        private void StaOpen_CheckedChanged(object sender, EventArgs e)
        {
            if (StaOpen.Checked)
            {
                if (Street1.Text.Contains("X-CLOSED-"))
                    Street1.Text = Street1.Text.Replace("X-CLOSED-_", "");

                StaClosed.Checked = false;
            }

        }

        private void StaClosed_CheckedChanged(object sender, EventArgs e)
        {
            if (StaClosed.Checked)
            {
                if (!Street1.Text.Contains("X-CLOSED-"))
                    Street1.Text = "X-CLOSED-_" + Street1.Text;

                StaOpen.Checked = false;
            }
        }

        private void Continue_OnValidate(object sender, Uheaa.Common.WinForms.ValidationEventArgs e)
        {
            if (!ValidateInput())
                return;

            if (e.FormIsValid)
            {
                UpdatedData = new LenderData()
                {
                    Mod = Mod.Text,
                    LenderId = LenderId.Text,
                    Valid = StaOpen.Checked,
                    FullName = FullName.Text,
                    ShortName = ShortName.Text,
                    Address1 = Street1.Text,
                    Address2 = Street2.Text,
                    City = City.Text,
                    State = State.Text,
                    Zip = ZipCode.Text,
                    Type = LenderType.Text.IsNullOrEmpty() ? null : LenderType.Text.Substring(0, 2)
                };

                if(FullName.Enabled == true)
                    UpdatedData.Valid = false;

                DialogResult = DialogResult.OK;
            }
        }

        private void Unlocated_OnValidate(object sender, Uheaa.Common.WinForms.ValidationEventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void Unlocated_Click(object sender, EventArgs e)
        {

        }
    }
}
