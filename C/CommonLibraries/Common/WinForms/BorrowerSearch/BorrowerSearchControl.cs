using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;

namespace Uheaa.Common.WinForms
{
    public partial class BorrowerSearchControl : UserControl
    {
        public BorrowerSearchControl()
        {
            InitializeComponent();
        }

        public LogDataAccess LDA { get; set; }

        public delegate void SearchResultsRetrieved(BorrowerSearchControl sender, List<QuickBorrower> results);
        public event SearchResultsRetrieved OnSearchResultsRetrieved;

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
            Reset();
        }

        /// <summary>
        /// We want to be able to reset the search fields from code
        /// </summary>
        public void Reset()
        {
            FirstNameBox.Text = LastNameBox.Text =
                MiddleInitialBox.Text = DOBBox.Text = AddressBox.Text =
                CityBox.Text = StateBox.Text = ZipBox.Text =
                PhoneBox.Text = EmailBox.Text = null;
            ResetButton.Enabled = false;
            SearchCleared?.Invoke(this, new EventArgs());
        }

        private void RegionButton_Cycle(object sender)
        {
            StatusBox.Width = StatusBox.Height = ResetButton.Height;
            StatusBox.Top = ResetButton.Top;
        }

        private void BorrowerSearchControl_SizeChanged(object sender, EventArgs e)
        {
            //status box doesn't scale well with font like the rest of the controls, manually resize it
            StatusBox.Width = StatusBox.Height = ResetButton.Height;
            StatusBox.Top = ResetButton.Top;
        }

        private void BorrowerSearchControl_Layout(object sender, LayoutEventArgs e)
        {
            //status box doesn't scale well with font like the rest of the controls, manually resize it
            StatusBox.Width = StatusBox.Height = ResetButton.Height;
            StatusBox.Top = ResetButton.Top;
        }

        private void Set_Clearfields(object sender, EventArgs e)
        {
            if (searching)
                return;
            bool anyContent = false;
            foreach (Control control in this.AllChildren())
                if ((control as TextBox)?.Text.IsPopulated() == true)
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
            });
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            SearchMethod();
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
