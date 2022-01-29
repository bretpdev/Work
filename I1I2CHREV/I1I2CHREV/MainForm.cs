using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace I1I2CHREV
{
    public partial class MainForm : Form
    {
        public MainForm(Demographics demographics)
        {
            InitializeComponent();
            AutoDataBinder.Bind(this, demographics);
            NoLocateButton.Click += (o, ea) => SetStatus(LocationStatus.NoLocate);
            NotFoundButton.Click += (o, ea) => SetStatus(LocationStatus.NotFound);
            CancelButton.Click += (o, ea) => SetStatus(LocationStatus.Cancel);
            LocateButton.Click += (o, ea) =>
            {
                if (Address1Box.Text.IsNullOrEmpty())
                    Dialog.Warning.Ok("Please enter an Address");
                else if (CityBox.Text.IsNullOrEmpty())
                    Dialog.Warning.Ok("Please enter a City");
                else if (StateBox.Text.IsNullOrEmpty() && CountryBox.Text.IsNullOrEmpty())
                    Dialog.Warning.Ok("Please select a State or enter a Country");
                else if (ZipBox.Text.IsNullOrEmpty())
                    Dialog.Warning.Ok("Please enter a Zip Code");
                else
                    SetStatus(LocationStatus.Locate);
            };
        }

        private void SetStatus(LocationStatus status)
        {
            LocationStatus = status;
            this.Close();
        }
        public LocationStatus LocationStatus { get; private set; }
        public new LocationStatus ShowDialog()
        {
            base.ShowDialog();
            return LocationStatus;
        }

        private void ValidationOnLeave(object sender, Uheaa.Common.WinForms.SimpleValidationEventArgs e)
        {
            if (string.IsNullOrEmpty((sender as TextBox).Text))
            {
                e.Valid = false;
                e.ValidationMessage = "This field is required.";
            }
            else
                e.Valid = true;
        }

        private void StateBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CountryBox.Enabled = StateBox.Text.IsNullOrEmpty();
        }

        private void CountryBox_TextChanged(object sender, EventArgs e)
        {
            StateBox.Enabled = CountryBox.Text.IsNullOrEmpty();
        }

        private void StateBox_TextChanged(object sender, EventArgs e)
        {
            CountryBox.Enabled = StateBox.Text.IsNullOrEmpty();
        }
    }
}
