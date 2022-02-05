using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace NeedHelp
{
    class SearchAndResultsProcessor
    {
        private SearchAndResults SearchForm { get; set; }
        private SubSystemManager SubSystemManager { get; set; }
        private SqlUser User { get; set; }
        private string Role { get; set; }
        private ProcessLogRun LogRun { get; set; }

        //Combo box lists that are common to all subsystems:
        List<string> BusinessFunctions { get; set; }
        List<BusinessUnit> BusinessUnits { get; set; }
        List<KeyWordScope> KeywordScopes { get; set; }
        List<SortField> SortFields { get; set; }

        public SearchAndResultsProcessor(SqlUser user, string role, ProcessLogRun logRun)
        {
            User = user;
            Role = role;
            LogRun = logRun;
        }

        /// <summary>
        /// Main starting point for the search and results display functionality.
        /// </summary>
        public void Start()
        {
#if !DEBUG
            SplashScreen ss = null;
            Thread t = new Thread(() =>
            {
                Application.EnableVisualStyles();
                ss = new SplashScreen();
                Application.Run(ss);
            });
            t.Start();
#endif

            SubSystemManager = SubSystemManager.Initialize(User, Role, LogRun);
            SubSystemManager.RefreshSearchResults += new EventHandler<RefreshSearchEventArgs>(RefreshSearchResults);

            //Initialize the search form, subscribe to its custom events, and display the form.
            SearchForm = new SearchAndResults(User, SubSystemManager.GetCommercialSubSystemNames(), SubSystemManager.OnlyFacilitiesLoaded, LogRun);
            SearchForm.DisplayTicket += new EventHandler<SearchAndResults.DisplayTicketEventArgs>(DisplayTicket);
            SearchForm.NewTicket += new EventHandler<SearchAndResults.NewTicketEventArgs>(CreateNewTicket);
            SearchForm.SearchRequested += new EventHandler<SearchAndResults.SearchRequestedEventArgs>(Search);
            SearchForm.SubSystemChanged += new EventHandler<SearchAndResults.SubSystemChangedEventArgs>(ChangeSubSystem);
            SearchForm.SubSystemDefault();
#if !DEBUG
            t.Abort();
#endif
            Application.Run(SearchForm);
        }

        void ChangeSubSystem(object sender, SearchAndResults.SubSystemChangedEventArgs e)
        {
            //Make sure the common combo box lists are loaded.
            DataAccess da = new DataAccess(LogRun.LDA);
            if (BusinessFunctions == null)
            {
                BusinessFunctions = da.GetBusinessFunctions().ToList();
                BusinessFunctions.Insert(0, "");
            }

            if (KeywordScopes == null) { KeywordScopes = GetKeyWordScopes(); }
            if (SortFields == null) { SortFields = GetSortFields(); }

            string subSystemName = e.SubSystemName;
            if (e.SubSystemName.IsIn("Facilities", "System Access", "Building Access"))
            {
                subSystemName = "Need Help";
            }

            //The rest of the combo box contents are determined by the subsystems.
            List<SqlUser> assignees = SubSystemManager.GetAssignees(subSystemName).ToList();
            BusinessUnits = new List<BusinessUnit>(da.GetBusinessUnits());
            List<SqlUser> courts = SubSystemManager.GetCourts(subSystemName).ToList();
            List<SqlUser> requesters = SubSystemManager.GetRequesters(subSystemName).ToList();
            List<string> statuses = SubSystemManager.GetStatuses(subSystemName).ToList();
            List<string> subjects = SubSystemManager.GetSubjects(subSystemName).ToList();
            List<TicketType> ticketTypes = SubSystemManager.GetTicketTypes(e.SubSystemName).ToList();
            DateTime earliestTicketCreateDate = SubSystemManager.GetEarliestTicketCreateDate(subSystemName);

            //Put a blank item at the beginning of each list.
            assignees.Insert(0, new SqlUser());
            BusinessUnits.Insert(0, new BusinessUnit());
            courts.Insert(0, new SqlUser());
            requesters.Insert(0, new SqlUser());
            statuses.Insert(0, "");
            subjects.Insert(0, "");
            SearchForm.SetUpSearchFields(ticketTypes, statuses, subjects, assignees, courts, requesters, BusinessUnits, BusinessFunctions, KeywordScopes, SortFields, earliestTicketCreateDate, e.SubSystemName);
        }

        private void CreateNewTicket(object sender, SearchAndResults.NewTicketEventArgs e)
        {
            SubSystemManager.CreateNewTicket(e.TicketType);
        }

        private void DisplayTicket(object sender, SearchAndResults.DisplayTicketEventArgs e)
        {
            SubSystemManager.DisplayTicket(e.TicketCode, e.TicketNumber, LogRun);
        }

        private void RefreshSearchResults(object sender, RefreshSearchEventArgs e)
        {
            SearchForm.RefreshSearchResults(sender, e);
        }

        private List<KeyWordScope> GetKeyWordScopes()
        {
            List<KeyWordScope> keywordScopes = new List<KeyWordScope>();
            keywordScopes.Add(KeyWordScope.None);
            keywordScopes.Add(KeyWordScope.Subject);
            keywordScopes.Add(KeyWordScope.Issue);
            keywordScopes.Add(KeyWordScope.History);
            keywordScopes.Add(KeyWordScope.All);
            return keywordScopes;
        }

        private List<SortField> GetSortFields()
        {
            List<SortField> sortFields = new List<SortField>();
            sortFields.Add(SortField.None);
            sortFields.Add(SortField.Priority);
            sortFields.Add(SortField.LastUpdateDate);
            sortFields.Add(SortField.Status);
            return sortFields;
        }

        private void Search(object sender, SearchAndResults.SearchRequestedEventArgs e)
        {
            List<SearchResultItem> searchResults;
            switch (e.SearchMethod)
            {
                case SearchAndResults.SearchMethod.AllTickets:
                    searchResults = SubSystemManager.SearchForAllTickets(e.SubSystemName, e.SearchCriteria).ToList();
                    break;
                case SearchAndResults.SearchMethod.OpenTickets:
                    searchResults = SubSystemManager.SearchForOpenTickets(e.SubSystemName, e.SearchCriteria).ToList();
                    break;
                default:
                    searchResults = SubSystemManager.SearchForTickets(e.SubSystemName, e.SearchCriteria).ToList();
                    break;
            }
            SearchForm.ShowSearchResults(searchResults);

            if (!string.IsNullOrEmpty(e.SearchCriteria.TicketNumber) && searchResults.Count() == 1)
                DisplayTicket(this, new SearchAndResults.DisplayTicketEventArgs(searchResults.Single().TicketCode, searchResults.Single().TicketNumber));
        }
    }
}