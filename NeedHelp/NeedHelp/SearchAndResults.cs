using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using SubSystemShared;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Reflection;
using Uheaa.Common;

namespace NeedHelp
{
    public partial class SearchAndResults : Form
    {
        public enum SearchMethod
        {
            AllTickets,
            OpenTickets,
            UserSearchCriteria
        }

        private const int NO_SORT_COLUMN = -1;

        private SearchCriteria Criteria { get; set; }
        private int LastSortColumn { get; set; }
        private bool LastSortWasAscending { get; set; }
        private SqlUser User { get; set; }
        private readonly Color LABLE_FORE_COLOR = Color.FromArgb(249, 234, 229);

        private List<TicketType> TicketTypes { get; set; }
        private List<string> Statuses { get; set; }
        private List<string> Subjects { get; set; }
        private List<SqlUser> Assignees { get; set; }
        private List<SqlUser> Courts { get; set; }
        private List<SqlUser> Requesters { get; set; }
        private List<BusinessUnit> BusinessUnits { get; set; }
        private List<string> BusinessFunctions { get; set; }
        private List<KeyWordScope> KeywordScopes { get; set; }
        private List<SortField> SortFields { get; set; }
        private DateTime EarliestTicketCreateDate { get; set; }
        private string SubSystemName { get; set; }
        private ProcessLogRun LogRun { get; set; }

        /// <summary>
        /// DO NOT USE!!!
        /// The parameterless constructor is required for the Windows Forms Designer, but it will not work with ACDC.
        /// </summary>
        public SearchAndResults()
        {
            InitializeComponent();
        }

        public SearchAndResults(SqlUser user, List<string> commercialSubSystemNames, bool isOnlyFacilities, ProcessLogRun logRun)
        {
            InitializeComponent();
            this.Text = $"Search and Results :: Version {Assembly.GetEntryAssembly().GetName().Version} :: {DataAccessHelper.CurrentMode}";

            User = user;
            LogRun = logRun;

            //Add the commercial sub systems to the search form
            foreach (string item in commercialSubSystemNames.OrderByDescending(p => p.ToString()))
            {
                SubSystemName name = new SubSystemName(item);
                name.Clicked += new EventHandler<SubSystemName.ClickedEventArgs>(SubSystemName_Clicked);
                pnlUheaaSubSystems.Controls.Add(name);
                lblUheaaSubSystems.Visible = true;
            }

            if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev)
                isOnlyFacilities = true;

            //If they do not have access to Need Help, clear everything other than facilities, system access and incident reporting
            if (isOnlyFacilities)
            {
                //Add the facilities, system access and incident tickets to the search form for all staff outside of uheaa/cornerstone
                SubSystemName facilitiesName = new SubSystemName("Facilities");
                facilitiesName.Clicked += new EventHandler<SubSystemName.ClickedEventArgs>(SubSystemName_Clicked);
                pnlAllStaff.Controls.Add(facilitiesName);
                SubSystemName accessName = new SubSystemName("System Access");
                accessName.Clicked += new EventHandler<SubSystemName.ClickedEventArgs>(SubSystemName_Clicked);
                pnlAllStaff.Controls.Add(accessName);
                SubSystemName incidentName = new SubSystemName("Incident Reporting");
                incidentName.Clicked += new EventHandler<SubSystemName.ClickedEventArgs>(SubSystemName_Clicked);
                pnlAllStaff.Controls.Add(incidentName);
                SubSystemName buildingAccess = new SubSystemName("Building Access");
                buildingAccess.Clicked += new EventHandler<SubSystemName.ClickedEventArgs>(SubSystemName_Clicked);
                pnlAllStaff.Controls.Add(buildingAccess);
                if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live)
                {
                    pnlUheaaSubSystems.Controls.Clear();
                    lblUheaaSubSystems.Visible = false;
                }
                lblAllStaff.Visible = true;
            }
        }


        public void SetUpSearchFields(List<TicketType> ticketTypes, List<string> statuses, List<string> subjects, List<SqlUser> assignees, List<SqlUser> courts, List<SqlUser> requesters, List<BusinessUnit> businessUnits, List<string> businessFunctions, List<KeyWordScope> keywordScopes, List<SortField> sortFields, DateTime earliestTicketCreateDate, string name)
        {
            TicketTypes = ticketTypes;
            Statuses = statuses;
            Subjects = subjects;
            Assignees = assignees;
            Courts = courts;
            Requesters = requesters;
            BusinessUnits = businessUnits;
            BusinessFunctions = businessFunctions;
            KeywordScopes = keywordScopes;
            SortFields = sortFields;
            EarliestTicketCreateDate = earliestTicketCreateDate;

            gbxSearch.Enabled = true;
            Criteria = new SearchCriteria();
            dtpFrom.Value = earliestTicketCreateDate;
            Criteria.CreateDateRangeStart = earliestTicketCreateDate;
            Criteria.CreateDateRangeEnd = DateTime.Now;
            searchCriteriaBindingSource.DataSource = Criteria;

            LastSortColumn = NO_SORT_COLUMN;
            LastSortWasAscending = true;

            //Set combo box data sources.
            ticketTypeBindingSource.DataSource = ticketTypes;
            cboStatus.DataSource = statuses;
            cboSubject.DataSource = subjects;
            assignedToBindingSource.DataSource = assignees;
            courtBindingSource.DataSource = courts;
            requesterBindingSource.DataSource = requesters;
            keywordScopeBindingSource.DataSource = keywordScopes;
            sortFieldBindingSource.DataSource = sortFields;
            cboBusinessUnit.DataSource = businessUnits;
            cboFunctional.DataSource = businessFunctions;

            foreach (FlowLayoutPanel panel in new FlowLayoutPanel[] { pnlAllStaff, pnlUheaaSubSystems })
            {
                foreach (Control item in panel.Controls)
                {
                    SubSystemName tempName = item as SubSystemName;
                    if (tempName != null)
                    {
                        tempName.IsSelected = tempName.SubSystem == name;
                    }
                }
            }
        }

        public void ShowSearchResults(List<SearchResultItem> searchResults)
        {
            //Sort using the sort option if a column header hasn't been clicked.
            if (LastSortColumn == NO_SORT_COLUMN && searchResults != null)
            {
                if (Criteria.SortingOption == SortField.LastUpdateDate)
                { searchResults = searchResults.OrderByDescending(p => p.LastUpdateDate).ToList(); }
                else if (Criteria.SortingOption == SortField.Priority)
                { searchResults = searchResults.OrderByDescending(p => p.Priority).ToList(); }
                else if (Criteria.SortingOption == SortField.Status)
                { searchResults = searchResults.OrderByDescending(p => p.Status).ToList(); }
                else
                    searchResults = searchResults.OrderBy(p => p.TicketNumber).ToList();
            }

            //Remove the grid view and either display a messae of no results or reload the grid view
            if (searchResults == null || searchResults.Count() == 0)
            {
                gbxResults.Controls.Clear();
                Label emptyMessage = new Label();
                emptyMessage.Text = "No Results Found";
                emptyMessage.ForeColor = Color.Wheat;
                emptyMessage.Size = new Size(400, 100);
                emptyMessage.Font = new Font(new FontFamily("Microsoft Sans Serif"), 20, FontStyle.Bold);
                emptyMessage.Location = new Point(250, 100);
                gbxResults.Controls.Add(emptyMessage);
            }
            else
            {
                gbxResults.Controls.Clear();
                gbxResults.Controls.Add(dgvSearchResults);
            }

            //Show the results.
            searchResultItemBindingSource.Clear();
            searchResultItemBindingSource.DataSource = searchResults;
            Cursor = Cursors.Default;
        }//ShowSearchResults()

        //Convenience function containing common code for all of the "Get...SearchResult" methods.
        private SearchResultItem[] GetSearchResults()
        {
            //Go through two casts to get an List<SearchResultItem> from the DataGridView.
            BindingSource searchResultsBindingSource = dgvSearchResults.DataSource as BindingSource;
            List<SearchResultItem> searchResults = searchResultsBindingSource.DataSource as List<SearchResultItem>;
            return searchResults.ToArray();
        }

        private string GetSelectedSubSystemName()
        {
            foreach (Control control in pnlUheaaSubSystems.Controls)
                if (control is SubSystemName name && name.IsSelected) { return name.SubSystem; }
            foreach (Control control in pnlAllStaff.Controls)
                if (control is SubSystemName name && name.IsSelected) { return name.SubSystem; }

            //If no subsystem has been selected, return null.
            return null;
        }

        private void BtnCreateNew_Click(object sender, EventArgs e)
        {
            if (cboNewTicketType.Text == "")
            {
                MessageBox.Show("You must choose a ticket type to create a new ticket.", "Information Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Fire an event that the SearchAndResultsProcessor is subscribed to for creating a new ticket.
            OnNewTicket(new NewTicketEventArgs((cboNewTicketType.SelectedItem as TicketType), User));
            cboNewTicketType.SelectedIndex = -1;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            //Fire an event that the SearchAndResultsProcessor is subscribed to for performing the search.
            Cursor = Cursors.WaitCursor;
            LastSortColumn = NO_SORT_COLUMN;
            Criteria = new SearchCriteria()
            {
                AssignedTo = string.IsNullOrEmpty(cboAssignedTo.Text.Trim()) ? null : (SqlUser)cboAssignedTo.SelectedItem,
                BusinessUnit = string.IsNullOrEmpty(cboBusinessUnit.Text.Trim()) ? null : (BusinessUnit)cboBusinessUnit.SelectedItem,
                Court = string.IsNullOrEmpty(cboCourt.Text.Trim()) ? null : (SqlUser)cboCourt.SelectedItem,
                CreateDateRangeEnd = string.IsNullOrEmpty(dtpTo.Text.Trim()) ? new DateTime() : Convert.ToDateTime(dtpTo.Text).AddDays(1),
                CreateDateRangeStart = string.IsNullOrEmpty(dtpFrom.Text.Trim()) ? new DateTime() : Convert.ToDateTime(dtpFrom.Text),
                FunctionalArea = string.IsNullOrEmpty(cboFunctional.Text.Trim()) ? null : cboFunctional.Text,
                KeyWord = string.IsNullOrEmpty(txtKeyword.Text.Trim()) ? null : txtKeyword.Text,
                KeyWordSearchScope = string.IsNullOrEmpty(cboKeyword.Text.Trim()) ? null : (KeyWordScope)cboKeyword.SelectedItem,
                Requester = string.IsNullOrEmpty(cboRequester.Text.Trim()) ? null : (SqlUser)cboRequester.SelectedItem,
                SortingOption = string.IsNullOrEmpty(cboSorting.Text.Trim()) ? null : (SortField)cboSorting.SelectedItem,
                Status = string.IsNullOrEmpty(cboStatus.Text.Trim()) ? null : cboStatus.Text,
                Subject = string.IsNullOrEmpty(cboSubject.Text.Trim()) ? null : cboSubject.Text,
                TicketNumber = string.IsNullOrEmpty(txtTicketNo.Text.Trim()) ? null : txtTicketNo.Text,
                TicketType = string.IsNullOrEmpty(cboType.Text.Trim()) ? null : (TicketType)cboType.SelectedItem
            };
            searchCriteriaBindingSource = new BindingSource();
            searchCriteriaBindingSource.DataSource = Criteria;
            OnSearchRequested(new SearchRequestedEventArgs(GetSelectedSubSystemName(), SearchMethod.UserSearchCriteria, Criteria));
        }

        private void BtnOpenTickets_Click(object sender, EventArgs e)
        {
            //Fire an event that the SearchAndResultsProcessor is subscribed to for performing the search.
            Cursor = Cursors.WaitCursor;
            LastSortColumn = NO_SORT_COLUMN;
            Criteria.TicketType = (TicketType)cboType.SelectedItem;
            OnSearchRequested(new SearchRequestedEventArgs(GetSelectedSubSystemName(), SearchMethod.OpenTickets, Criteria));
        }

        private void BtnAllTickets_Click(object sender, EventArgs e)
        {
            //Fire an event that the SearchAndResultsProcessor is subscribed to for performing the search.
            Cursor = Cursors.WaitCursor;
            LastSortColumn = NO_SORT_COLUMN;
            Criteria.TicketType = (TicketType)cboType.SelectedItem;
            OnSearchRequested(new SearchRequestedEventArgs(GetSelectedSubSystemName(), SearchMethod.AllTickets, Criteria));
        }

        private void DgvSearchResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSearchResults.SelectedRows.Count == 0)
                return;
            //Fire an event that the SearchAndResultsProcessor is subscribed to for displaying a ticket.
            SearchResultItem selectedItem = (dgvSearchResults.SelectedRows[0].DataBoundItem as SearchResultItem);
            if (selectedItem == null)
                return;
            else
                SubSystemName = "Need Help";

            OnDisplayTicket(new DisplayTicketEventArgs(selectedItem.TicketCode, selectedItem.TicketNumber));
        }

        private void DgvSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SortResults(e.ColumnIndex);
        }

        private void SortResults(int selectedColumnIndex)
        {
            //Check that there are results to sort.
            List<SearchResultItem> searchResults = (searchResultItemBindingSource.DataSource as List<SearchResultItem>);
            if (searchResults == null)
                return;

            //selectedColumnIndex is the only indicator we have of which column was clicked,
            //so if the columns change, this switch block will need to be updated.
            switch (selectedColumnIndex)
            {
                case 0:
                    if (LastSortColumn == selectedColumnIndex)
                    {
                        if (LastSortWasAscending)
                            searchResults = searchResults.OrderByDescending(p => p.TicketCode).ToList();
                        else
                            searchResults = searchResults.OrderBy(p => p.TicketCode).ToList();
                        LastSortWasAscending = !LastSortWasAscending;
                    }
                    else
                    {
                        searchResults = searchResults.OrderBy(p => p.TicketCode).ToList();
                        LastSortWasAscending = true;
                    }
                    break;
                case 1:
                    if (LastSortColumn == selectedColumnIndex)
                    {
                        if (LastSortWasAscending)
                            searchResults = searchResults.OrderByDescending(p => p.TicketNumber).ToList();
                        else
                            searchResults = searchResults.OrderBy(p => p.TicketNumber).ToList();
                        LastSortWasAscending = !LastSortWasAscending;
                    }
                    else
                    {
                        searchResults = searchResults.OrderBy(p => p.TicketNumber).ToList();
                        LastSortWasAscending = true;
                    }
                    break;
                case 2:
                    if (LastSortColumn == selectedColumnIndex)
                    {
                        if (LastSortWasAscending)
                            searchResults = searchResults.OrderByDescending(p => p.Subject).ToList();
                        else
                            searchResults = searchResults.OrderBy(p => p.Subject).ToList();
                        LastSortWasAscending = !LastSortWasAscending;
                    }
                    else
                    {
                        searchResults = searchResults.OrderBy(p => p.Subject).ToList();
                        LastSortWasAscending = true;
                    }
                    break;
                case 3:
                    if (LastSortColumn == selectedColumnIndex)
                    {
                        if (LastSortWasAscending)
                            searchResults = searchResults.OrderByDescending(p => p.Status).ToList();
                        else
                            searchResults = searchResults.OrderBy(p => p.Status).ToList();
                        LastSortWasAscending = !LastSortWasAscending;
                    }
                    else
                    {
                        searchResults = searchResults.OrderBy(p => p.Status).ToList();
                        LastSortWasAscending = true;
                    }
                    break;
                case 4:
                    if (LastSortColumn == selectedColumnIndex)
                    {
                        if (LastSortWasAscending)
                            searchResults = searchResults.OrderByDescending(p => p.Priority).ToList();
                        else
                            searchResults = searchResults.OrderBy(p => p.Priority).ToList();
                        LastSortWasAscending = !LastSortWasAscending;
                    }
                    else
                    {
                        searchResults = searchResults.OrderBy(p => p.Priority).ToList();
                        LastSortWasAscending = true;
                    }
                    break;
            }
            LastSortColumn = selectedColumnIndex;
            ShowSearchResults(searchResults.ToList());
        }

        private void SubSystemName_Clicked(object sender, SubSystemName.ClickedEventArgs e)
        {
            SubSystemName = e.Name;
            gbxSearch.Enabled = true;
            cboNewTicketType.Enabled = true;
            btnCreateNew.Enabled = true;
            OnSubSystemChanged(new SubSystemChangedEventArgs(e.Name));
            Criteria = new SearchCriteria();
            Criteria.Court = User;
            if (TicketTypes.Count == 1)
                Criteria.TicketType = TicketTypes[0];
            searchCriteriaBindingSource = new BindingSource();
            searchCriteriaBindingSource.DataSource = Criteria;
            ClearControls(Criteria);
        }

        public void SubSystemDefault()
        {
            SubSystemName_Clicked(new object(), new NeedHelp.SubSystemName.ClickedEventArgs("Need Help"));
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Criteria = new SearchCriteria();
            Criteria.CreateDateRangeStart = new DateTime(1900, 1, 2);
            Criteria.CreateDateRangeEnd = new DateTime(1900, 1, 1);
            searchCriteriaBindingSource.DataSource = Criteria;
            ClearControls(Criteria);
        }

        private void ClearControls(SearchCriteria criteria)
        {
            txtTicketNo.DataBindings.Clear();
            txtTicketNo.Text = string.Empty;
            cboType.DataBindings.Clear();
            cboType.DisplayMember = "Description";
            cboType.DataSource = TicketTypes;
            if (TicketTypes.Count == 1)
                cboType.SelectedIndex = 0;
            else
                cboType.SelectedIndex = -1;
            txtKeyword.DataBindings.Clear();
            txtKeyword.Text = string.Empty;
            cboKeyword.DataBindings.Clear();
            cboKeyword.DataSource = KeywordScopes;
            cboKeyword.SelectedIndex = 0;
            cboSubject.DataBindings.Clear();
            cboSubject.DisplayMember = "";
            cboSubject.SelectedIndex = -1;
            cboCourt.DataBindings.Clear();
            cboCourt.DisplayMember = "LegalName";
            cboCourt.DataSource = Courts;
            cboCourt.SelectedIndex = -1;
            cboRequester.DataBindings.Clear();
            cboRequester.DisplayMember = "LegalName";
            cboRequester.DataSource = Requesters;
            cboRequester.SelectedIndex = -1;
            cboStatus.DataBindings.Clear();
            cboStatus.DataSource = Statuses;
            cboStatus.SelectedIndex = -1;
            cboBusinessUnit.DataBindings.Clear();
            cboBusinessUnit.DisplayMember = "Name";
            cboBusinessUnit.DataSource = BusinessUnits;
            cboBusinessUnit.SelectedIndex = -1;
            cboFunctional.DataBindings.Clear();
            cboFunctional.DataSource = BusinessFunctions;
            cboFunctional.SelectedIndex = -1;
            cboAssignedTo.DataBindings.Clear();
            cboAssignedTo.DataSource = Assignees;
            cboAssignedTo.DisplayMember = "LegalName";
            cboAssignedTo.SelectedIndex = -1;
            cboSorting.DataBindings.Clear();
            cboSorting.DataSource = SortFields;
            cboSorting.SelectedIndex = -1;
            txtTicketNo.Focus();
            searchCriteriaBindingSource = new BindingSource();
            OnSearchRequested(new SearchRequestedEventArgs(GetSelectedSubSystemName(), SearchMethod.UserSearchCriteria, criteria));
        }

        public void RefreshSearchResults(object sender, EventArgs e)
        {
            if (GetSelectedSubSystemName() != null)
            {
                searchCriteriaBindingSource.DataSource = Criteria;
                OnSearchRequested(new SearchRequestedEventArgs(GetSelectedSubSystemName(), SearchMethod.UserSearchCriteria, Criteria));
                txtTicketNo.Focus();
            }
            else
            {
                OnSearchRequested(new SearchRequestedEventArgs(null, SearchMethod.UserSearchCriteria, new SearchCriteria() { Court = User }));
                txtTicketNo.Focus();
            }
        }

        #region Custom events

        public class DisplayTicketEventArgs : EventArgs
        {
            public readonly string TicketCode;
            public readonly long TicketNumber;

            public DisplayTicketEventArgs(string ticketCode, long ticketNumber)
            {
                TicketCode = ticketCode;
                TicketNumber = ticketNumber;
            }
        }
        public event EventHandler<DisplayTicketEventArgs> DisplayTicket;
        protected virtual void OnDisplayTicket(DisplayTicketEventArgs e)
        {
            DisplayTicket?.Invoke(this, e);
        }

        public class NewTicketEventArgs : EventArgs
        {
            public readonly TicketType TicketType;
            public readonly SqlUser User;

            public NewTicketEventArgs(TicketType ticketType, SqlUser user)
            {
                TicketType = ticketType;
                User = user;
            }
        }
        public event EventHandler<NewTicketEventArgs> NewTicket;
        protected virtual void OnNewTicket(NewTicketEventArgs e)
        {
            NewTicket?.Invoke(this, e);
        }

        public class SearchRequestedEventArgs : EventArgs
        {
            public readonly string SubSystemName;
            public readonly SearchMethod SearchMethod;
            public readonly SearchCriteria SearchCriteria;

            public SearchRequestedEventArgs(string subSystemName, SearchMethod method, SearchCriteria criteria)
            {
                SubSystemName = subSystemName;
                SearchMethod = method;
                SearchCriteria = criteria;
            }
        }
        public event EventHandler<SearchRequestedEventArgs> SearchRequested;
        protected virtual void OnSearchRequested(SearchRequestedEventArgs e)
        {
            SearchRequested?.Invoke(this, e);
        }

        public class SubSystemChangedEventArgs : EventArgs
        {
            public readonly string SubSystemName;

            public SubSystemChangedEventArgs(string subSystemName)
            {
                SubSystemName = subSystemName;
            }
        }
        public event EventHandler<SubSystemChangedEventArgs> SubSystemChanged;
        protected virtual void OnSubSystemChanged(SubSystemChangedEventArgs e)
        {
            SubSystemChanged?.Invoke(this, e);
        }

        #endregion Custom events

        private void SearchAndResults_Shown(object sender, EventArgs e)
        {
            OnSearchRequested(new SearchRequestedEventArgs(null, SearchMethod.UserSearchCriteria, new SearchCriteria() { Court = User }));
        }
    }
}