using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using Uheaa.Common.ProcessLogger;
using System.Threading.Tasks;

namespace Uheaa.Common.WinForms
{
    public partial class DynamicBorrowerSearchControl : UserControl
    {
        public DynamicBorrowerSearchControl()
        {
            InitializeComponent();
        }

        public LogDataAccess LDA { get; set; }

        public delegate void SearchResultsRetrieved(DynamicBorrowerSearchControl sender, List<QuickBorrower> results);
        public event SearchResultsRetrieved OnSearchResultsRetrieved;

        const string Category = "Search Fields";
        #region Enabled
        [Browsable(true), Category(Category), DefaultValue(true)]
        public bool FirstNameEnabled
        {
            get { return GetFieldEnabled(FirstNameBox); }
            set { SetFieldEnabled(FirstNameBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(true)]
        public bool LastNameEnabled
        {
            get { return GetFieldEnabled(LastNameBox); }
            set { SetFieldEnabled(LastNameBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(true)]
        public bool MiddleInitialEnabled
        {
            get { return GetFieldEnabled(MiddleInitialBox); }
            set { SetFieldEnabled(MiddleInitialBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(true)]
        public bool DOBEnabled
        {
            get { return GetFieldEnabled(DOBBox); }
            set { SetFieldEnabled(DOBBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(true)]
        public bool AddressEnabled
        {
            get { return GetFieldEnabled(AddressBox); }
            set { SetFieldEnabled(AddressBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(true)]
        public bool CityEnabled
        {
            get { return GetFieldEnabled(CityBox); }
            set { SetFieldEnabled(CityBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(true)]
        public bool StateEnabled
        {
            get { return GetFieldEnabled(StateBox); }
            set { SetFieldEnabled(StateBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(true)]
        public bool ZipEnabled
        {
            get { return GetFieldEnabled(ZipBox); }
            set { SetFieldEnabled(ZipBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(true)]
        public bool PhoneEnabled
        {
            get { return GetFieldEnabled(PhoneBox); }
            set { SetFieldEnabled(PhoneBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(true)]
        public bool EmailEnabled
        {
            get { return GetFieldEnabled(EmailBox); }
            set { SetFieldEnabled(EmailBox, value); }
        }
        private bool GetFieldEnabled(Control c)
        {
            return c.Visible;
        }
        private void SetFieldEnabled(Control c, bool isEnabled)
        {
            c.Visible = isEnabled;
        }
        #endregion
        #region Width
        [Browsable(true), Category(Category), DefaultValue(1d)]
        public double FirstNameWidth
        {
            get { return GetFieldWidth(FirstNameBox); }
            set { SetFieldWidth(FirstNameBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(1d)]
        public double LastNameWidth
        {
            get { return GetFieldWidth(LastNameBox); }
            set { SetFieldWidth(LastNameBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(1d)]
        public double MiddleInitialWidth
        {
            get { return GetFieldWidth(MiddleInitialBox); }
            set { SetFieldWidth(MiddleInitialBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(1d)]
        public double DOBWidth
        {
            get { return GetFieldWidth(DOBBox); }
            set { SetFieldWidth(DOBBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(1d)]
        public double AddressWidth
        {
            get { return GetFieldWidth(AddressBox); }
            set { SetFieldWidth(AddressBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(1d)]
        public double CityWidth
        {
            get { return GetFieldWidth(CityBox); }
            set { SetFieldWidth(CityBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(1d)]
        public double StateWidth
        {
            get { return GetFieldWidth(StateBox); }
            set { SetFieldWidth(StateBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(1d)]
        public double ZipWidth
        {
            get { return GetFieldWidth(ZipBox); }
            set { SetFieldWidth(ZipBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(1d)]
        public double PhoneWidth
        {
            get { return GetFieldWidth(PhoneBox); }
            set { SetFieldWidth(PhoneBox, value); }
        }
        [Browsable(true), Category(Category), DefaultValue(1d)]
        public double EmailWidth
        {
            get { return GetFieldWidth(EmailBox); }
            set { SetFieldWidth(EmailBox, value); }
        }
        private double GetFieldWidth(Control c)
        {
            var widthMod = c.Tag as double? ?? 1;
            return widthMod;
        }
        private void SetFieldWidth(Control c, double width)
        {
            var widthMod = c.Tag as double? ?? 1;
            c.Width = (int)(c.Width / widthMod);
            c.Tag = width;
            c.Width = (int)(c.Width * width);
        }
        #endregion


        bool searching = false;
        private void SearchMethod()
        {
            Task.Run(() =>
            {
                DisableEntry(true);
                searching = true;
                try
                {
                    this.BeginInvoke(() =>
                    {
                        StatusBox.BackgroundImage = Properties.Resources.search;
                        MainToolTip.SetToolTip(StatusBox, "Searching...");
                    });
                    SearchBorrower template = GetSearchTemplate();
                    List<QuickBorrower> results = null;
                    if (ValidTemplate(template))
                        results = QuickBorrower.Search(template, RegionButton.SelectedValue, LDA, RegionButton.UheaaEnabled, RegionButton.OnelinkEnabled);
                    this.BeginInvoke(new Action(() => OnSearchResultsRetrieved(this, results)));
                    this.BeginInvoke(() => StatusBox.BackgroundImage = null);
                }
                catch (SqlException)
                {
                    this.BeginInvoke(() =>
                    {
                        StatusBox.BackgroundImage = Properties.Resources.error;
                        MainToolTip.SetToolTip(StatusBox, "Couldn't connect to search database.");
                    });
                }
                catch (Exception) { } //thread abort, do nothing
                finally
                {
                    searching = false;
                    DisableEntry(false);
                }
            });
        }

        public void BeginInvoke(Action a)
        {
            base.BeginInvoke(a);
        }

        protected SearchBorrower GetSearchTemplate()
        {
            SearchBorrower template = new SearchBorrower();
            template.Address = AddressBox.Text.Trim();
            template.City = CityBox.Text.Trim();
            template.StateCode = StateBox.Text;
            template.Zip = ZipBox.Text.Trim();
            template.FirstName = FirstNameBox.Text.Trim();
            template.LastName = LastNameBox.Text.Trim();
            template.MiddleInitial = MiddleInitialBox.Text.Trim();
            string dob = DOBBox.Text.Trim();
            if (dob.Length == 8 && dob.ToIntNullable().HasValue)
                template.DOB = dob.Substring(0, 2) + "/" + dob.Substring(2, 2) + "/" + dob.Substring(4, 4);
            else
            {
                DateTime tryer = DateTime.Now;
                if (DateTime.TryParse(dob, out tryer))
                    template.DOB = tryer.ToString("MM/dd/yyyy");
            }
            string phone = "";
            foreach (char c in PhoneBox.Text) //remove all special characters
                if (char.IsNumber(c)) phone += c;
            template.Phone = phone;
            template.Email = EmailBox.Text;
            return template;
        }

        /// <summary>
        /// Check to see if the template has enough information to warrant a search (4 or more characters in all fields)
        /// </summary>
        protected bool ValidTemplate(SearchBorrower template)
        {
            return (template.FirstName + template.LastName + template.MiddleInitial + template.DOB + template.Zip +
                template.Address + template.City + template.Phone + template.Email).Length >= 4;
        }

        public event EventHandler SearchCleared;

        private void ResetButton_Click(object sender, EventArgs e)
        {
            FirstNameBox.Text = LastNameBox.Text =
                MiddleInitialBox.Text = DOBBox.Text = AddressBox.Text =
                CityBox.Text = StateBox.Text = ZipBox.Text =
                PhoneBox.Text = EmailBox.Text = null;
            ResetButton.Enabled = false;
            SearchCleared?.Invoke(this, new EventArgs());
        }

        private void Set_Clearfields(object sender, EventArgs e)
        {
            if (searching)
                return;
            bool anyContent = false;
            foreach (Control control in this.AllChildren())
                if ((control as WatermarkTextBox)?.Text.IsPopulated() == true)
                    anyContent = true;
            ResetButton.Enabled = SearchButton.Enabled = anyContent;
        }

        private void Enter_Pressed(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchMethod();
            }
        }

        private void DisableEntry(bool disable)
        {
            this.BeginInvoke(() =>
            {
                EmailBox.ReadOnly = PhoneBox.ReadOnly = FirstNameBox.ReadOnly = ZipBox.ReadOnly = StateBox.ReadOnly =
                    CityBox.ReadOnly = AddressBox.ReadOnly = DOBBox.ReadOnly = MiddleInitialBox.ReadOnly = LastNameBox.ReadOnly = disable;
                ResetButton.Enabled = RegionButton.Enabled = !disable;
                SearchButton.Visible = !disable;
                StatusBox.Visible = disable;
            });
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            SearchMethod();
        }

        public void Reset()
        {
            ResetButton.PerformClick();
        }

        public bool UheaaEnabled
        {
            get
            {
                return RegionButton.UheaaEnabled;
            }
            set
            {
                RegionButton.UheaaEnabled = value;
            }
        }

        public bool OnelinkEnabled
        {
            get
            {
                return RegionButton.OnelinkEnabled;
            }
            set
            {
                RegionButton.OnelinkEnabled = value;
            }
        }
    }
}
