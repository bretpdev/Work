using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    public class SubSystem : SubSystemBase
    {
        private DataAccess DataAccess { get; set; }
        private NeedHelpTickets TicketForm { get; set; }
        private Ticket ActiveTicket { get; set; }

        #region ComboBoxLists
        private List<SqlUser> UserList { get; set; }
        private List<string> UrgencyList { get; set; }
        private List<string> CategoryList { get; set; }
        private List<TextValueOption> UrgencyListForFacilities { get; set; }
        private List<TextValueOption> CategoryListForFacilities { get; set; }
        private List<string> FunctionalAreaList { get; set; }
        private List<BusinessUnit> BusinessUnitList { get; set; }
        private List<string> CauseList { get; set; }
        private List<string> SubjectList { get; set; }
        private List<string> SystemList { get; set; }
        #endregion

        protected override string BaseAbbreviation { get { return "NH"; } }
        protected override string BaseName { get { return "Need Help"; } }

        public override bool HasAccess
        {
            get
            {
                IsOnlyFacilities = !DataAccess.GetUserKeyAssignment(Role, "Uheaa Need Help Access");
                return true;
            }
        }

        public SubSystem(SqlUser user, string role, ProcessLogRun logRun)
            : base(user, role, logRun)
        {
            DataAccess = new DataAccess(logRun);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketType"></param>
        /// <param name="ticketNumber"></param>
        public override void DisplayTicket(string ticketCode, long ticketNumber, List<SqlUser> users)
        {
            UserList = users.OrderBy(p => p.FirstName).Distinct().ToList();
            //Check if the current active tickets ticketcode is the same ticket type
            bool isNewTicketCode = false;
            if (ActiveTicket != null) { isNewTicketCode = ActiveTicket.Data.TheTicketData.TicketCode != ticketCode; }
            //Get the active ticket
            ActiveTicket = new Ticket(ticketCode, ticketNumber, User, UserList, LogRun);
            //Load the combo boxes to be passed into the form
            LoadComboBoxLists();
            //If the form is already open, close it first unless the ticketcode is the same
            if ((TicketForm == null || TicketForm.IsDisposed) || isNewTicketCode)
            {
                if (TicketForm != null && isNewTicketCode) { TicketForm.Dispose(); }
                //Create a new ticket form and pass in the ticket and necessary combo boxes
                switch (ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator)
                {
                    case "Interface1":
                        TicketForm = new NeedHelpTickets(DataAccess, User, ActiveTicket, BusinessUnitList, CategoryList, FunctionalAreaList, CauseList, SubjectList, SystemList, UrgencyList, UserList, Role, LogRun);
                        break;
                    case "Interface2":
                        TicketForm = new NeedHelpTickets(DataAccess, User, ActiveTicket, BusinessUnitList, CategoryList, SubjectList, SystemList, UrgencyList, UserList, Role, LogRun);
                        break;
                    case "Interface3":
                        TicketForm = new NeedHelpTickets(DataAccess, User, ActiveTicket, CategoryListForFacilities, SubjectList, UrgencyListForFacilities, UserList, Role, LogRun);
                        break;
                    case "Interface4":
                        TicketForm = new NeedHelpTickets(DataAccess, User, ActiveTicket, BusinessUnitList, CategoryList, SubjectList, SystemList, UrgencyList, UserList, Role, LogRun);
                        break;
                    case "Interface5":
                        TicketForm = new NeedHelpTickets(DataAccess, User, ActiveTicket, BusinessUnitList, CategoryList, SubjectList, SystemList, UrgencyList, UserList, Role, LogRun);
                        break;
                }
                TicketForm.RefreshSearchResults += new EventHandler<RefreshSearchEventArgs>((object sender, RefreshSearchEventArgs e) => OnRefreshSearchResults(e));
            }
            else
            {
                TicketForm.BindNewTicket(ActiveTicket);
            }
            TicketForm.Show();
        }

        public override void CreateNewTicket(TicketType ticketType, List<SqlUser> users)
        {
            UserList = users.OrderBy(p => p.FirstName).Distinct().ToList();
            //Create the active ticket
            ActiveTicket = new Ticket(ticketType.Abbreviation, User, UserList, LogRun);
            //Load the combo boxes to be passed into the form
            LoadComboBoxLists();
            //If the form is already open, close it first
            if (TicketForm != null)
                TicketForm.Dispose();
            //Create a new ticket form and pass in the ticket and necessary combo boxes
            switch (ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator)
            {
                case "Interface1":
                    TicketForm = new NeedHelpTickets(DataAccess, User, ActiveTicket, BusinessUnitList, CategoryList, FunctionalAreaList, CauseList, SubjectList, SystemList, UrgencyList, UserList, Role, LogRun);
                    break;
                case "Interface2":
                    TicketForm = new NeedHelpTickets(DataAccess, User, ActiveTicket, BusinessUnitList, CategoryList, SubjectList, SystemList, UrgencyList, UserList, Role, LogRun);
                    break;
                case "Interface3":
                    TicketForm = new NeedHelpTickets(DataAccess, User, ActiveTicket, CategoryListForFacilities, SubjectList, UrgencyListForFacilities, UserList, Role, LogRun);
                    break;
                case "Interface4":
                    TicketForm = new NeedHelpTickets(DataAccess, User, ActiveTicket, BusinessUnitList, CategoryList, SubjectList, SystemList, UrgencyList, UserList, Role, LogRun);
                    break;
                case "Interface5":
                    TicketForm = new NeedHelpTickets(DataAccess, User, ActiveTicket, BusinessUnitList, CategoryList, SubjectList, SystemList, UrgencyList, UserList, Role, LogRun);
                    break;
            }
            TicketForm.RefreshSearchResults += new EventHandler<RefreshSearchEventArgs>((object sender, RefreshSearchEventArgs e) => OnRefreshSearchResults(e));
            TicketForm.Show();
        }

        public override DateTime GetEarliestTicketCreateDate()
        {
            return DataAccess.GetEarliestCreateDate();
        }

        public override List<SqlUser> GetFormerEmployeeAssignees()
        {
            return new List<SqlUser>();
        }

        public override List<SqlUser> GetFormerEmployeeCourts()
        {
            return new List<SqlUser>();
        }

        public override List<SqlUser> GetFormerEmployeeRequesters()
        {
            return new List<SqlUser>();
        }

        public override List<TicketType> GetTicketTypes()
        {
            List<TicketType> ticketTypes = new List<TicketType>();
            ticketTypes.AddRange(DataAccess.GetTicketTypes());
            return ticketTypes;
        }

        public override List<string> GetStatuses()
        {
            List<string> status = DataAccess.GetStatus();
            return status;
        }

        public override List<string> GetSubjects()
        {
            List<string> subjects = new List<string>();
            subjects.AddRange(DataAccess.GetNeedHelpSubjects());
            return subjects;
        }

        public override List<SearchResultItem> SearchForAllTickets(SearchCriteria criteria)
        {
            List<SearchResultItem> results = new List<SearchResultItem>();
            results.AddRange(DataAccess.GetAllTickets(criteria));
            return results;
        }

        public override List<SearchResultItem> SearchForOpenTickets(SearchCriteria criteria)
        {
            List<SearchResultItem> results = new List<SearchResultItem>();
            results.AddRange(DataAccess.GetOpenTicket(criteria));
            return results;
        }

        public override List<SearchResultItem> SearchForTickets(SearchCriteria criteria)
        {
            List<SearchResultItem> results = new List<SearchResultItem>();
            results.AddRange(DataAccess.TicketSearchingProcessor(criteria));
            return results;
        }

        public void LoadComboBoxLists()
        {
            //loads lists for DB for all lists that are used by all UIs
            switch (ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator)
            {
                //Genral
                case "Interface1":
                    BusinessUnitList = DataAccess.GetBusinessUnits();
                    CategoryList = DataAccess.GetCategoryOptions();
                    FunctionalAreaList = DataAccess.GetFunctionalArea();
                    CauseList = DataAccess.GetCause();
                    SubjectList = DataAccess.GetSubjectList();
                    SystemList = DataAccess.GetListOfSystems();
                    UrgencyList = DataAccess.GetUrgencyOptions();
                    break;
                //Change Form
                case "Interface2":
                    BusinessUnitList = DataAccess.GetBusinessUnits();
                    CategoryList = DataAccess.GetCategoryOptions();
                    SubjectList = DataAccess.GetSubjectList();
                    SystemList = DataAccess.GetListOfSystems();
                    UrgencyList = DataAccess.GetUrgencyOptions();
                    break;
                //Facilities
                case "Interface3":
                    CategoryListForFacilities = DataAccess.GetCategoryOptionsForFacilities();
                    SubjectList = DataAccess.GetSubjectList();
                    UrgencyListForFacilities = DataAccess.GetUrgencyOptionsForFacilities();
                    break;
                //Change Form
                case "Interface4":
                    BusinessUnitList = DataAccess.GetBusinessUnits();
                    CategoryList = DataAccess.GetCategoryOptions();
                    SubjectList = DataAccess.GetSubjectList();
                    SystemList = DataAccess.GetListOfSystems();
                    UrgencyList = DataAccess.GetUrgencyOptions();
                    break;
                //DCR Request
                case "Interface5":
                    BusinessUnitList = DataAccess.GetBusinessUnits();
                    CategoryList = DataAccess.GetCategoryOptions();
                    SubjectList = DataAccess.GetSubjectList();
                    SystemList = DataAccess.GetListOfSystems();
                    UrgencyList = DataAccess.GetUrgencyOptions();
                    break;
            }
        }
    }
}