using SubSystemShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace NeedHelp
{
    class SubSystemManager
    {
        private List<SubSystemBase> SubSystems { get; set; }
        List<SqlUser> CurrentEmployees { get; set; }
        List<SqlUser> UheaaEmployees { get; set; }

        public bool OnlyFacilitiesLoaded
        {
            get
            {
                SubSystemBase generalHelp = SubSystems.SingleOrDefault(p => p.Name == "Need Help");
                return (generalHelp != null && generalHelp.IsOnlyFacilities);
            }
        }

        /// <summary>
        /// Downloads all subsystems from the network, instantiates the ones the user is authorized to use,
        /// and creates a SubSystemManager with properties representing values from all subsystems.
        /// </summary>
        /// <param name="mode">True if running in test mode.</param>
        /// <param name="userSid">The Active Directory security ID of the logged-in user.</param>
        public static SubSystemManager Initialize(SqlUser user, string role, ProcessLogRun logRun)
        {
            SubSystemManager manager = new SubSystemManager();
            manager.SubSystems = manager.LoadSubSystems(user, role, logRun).Where(p => p.HasAccess).ToList();
            manager.CurrentEmployees = new DataAccess(logRun.LDA).GetSqlUsers(false).OrderBy(p => p.FirstName + " " + p.LastName).ToList();
            manager.UheaaEmployees = new List<SqlUser>(manager.CurrentEmployees);
            return manager;
        }

        public void CreateNewTicket(TicketType type)
        {
            //Find the subsystem that owns this ticket type and have it create a new ticket.
            SubSystemBase subSystem = SubSystems.Where(p => p.GetTicketTypes().Contains(type)).Single();
            subSystem.CreateNewTicket(type, UheaaEmployees);
        }

        public void DisplayTicket(string code, long number, ProcessLogRun logRun)
        {
            //Find the subsystem that owns this ticket type and have it display the ticket.
            SubSystemBase subSystem = SubSystems.Single(p => p.GetTicketTypes().Select(q => q.Abbreviation.ToUpper()).Contains(code.ToUpper()));
            subSystem.DisplayTicket(code, number, UheaaEmployees);
        }

        public List<SearchResultItem> SearchForAllTickets(string subSystemName, SearchCriteria criteria)
        {
            List<SearchResultItem> allTickets = new List<SearchResultItem>();
            if (subSystemName.IsIn("Building Access", "Facilities", "System Access"))
                subSystemName = "Need Help";
            foreach (SubSystemBase subSystem in SubSystems.Where(p => string.IsNullOrEmpty(subSystemName) || p.Name == subSystemName))
            {
                List<SearchResultItem> tickets = subSystem.SearchForAllTickets(criteria);
                allTickets.AddRange(tickets);
            }
            return allTickets;
        }

        public List<SearchResultItem> SearchForOpenTickets(string subSystemName, SearchCriteria criteria)
        {
            List<SearchResultItem> openTickets = new List<SearchResultItem>();
            if (subSystemName.IsIn("Building Access", "Facilities", "System Access"))
                subSystemName = "Need Help";
            foreach (SubSystemBase subSystem in SubSystems.Where(p => string.IsNullOrEmpty(subSystemName) || p.Name == subSystemName))
            {
                List<SearchResultItem> tickets = subSystem.SearchForOpenTickets(criteria);
                openTickets.AddRange(tickets);
            }
            return openTickets;
        }

        public List<SearchResultItem> SearchForTickets(string subSystemName, SearchCriteria criteria)
        {
            List<SearchResultItem> tickets = new List<SearchResultItem>();
            if (subSystemName.IsIn("Building Access", "Facilities", "System Access"))
                subSystemName = "Need Help";
            foreach (SubSystemBase subSystem in SubSystems.Where(p => string.IsNullOrEmpty(subSystemName) || p.Name == subSystemName))
            {
                List<SearchResultItem> ticket = subSystem.SearchForTickets(criteria);
                tickets.AddRange(ticket);
            }
            return tickets;
        }

        private List<SubSystemBase> LoadSubSystems(SqlUser user, string role, ProcessLogRun logRun)
        {
            List<SubSystemBase> subSystems = new List<SubSystemBase>();
            List<string> systems = new List<string>() { "NHGeneral.dll", "INCIDENTRP.exe" };

            //Load all subsystems as federal at first, and then
            //check each one to see if we also need a commercial instance.
            var files = Directory.GetFiles(Properties.Resources.SubsystemLocalDirectory).Where(p => p.EndsWith(systems[0]) || p.EndsWith(systems[1]));
#if DEBUG
            files = Directory.GetFiles(Properties.Resources.SubsystemLocalDirectoryTest).Where(p => p.EndsWith(systems[0]) || p.EndsWith(systems[1]));
#endif
            foreach (string dll in files)
            {
                string theNamespace = dll.Substring(dll.LastIndexOf('\\') + 1).Replace(".dll", "").Replace(".exe", "");
                object[] commercialConstructorArguments = new object[] { user, role, logRun };
                ObjectHandle commercialSubSystemHandle = Activator.CreateInstanceFrom(dll, theNamespace + ".SubSystem", true, BindingFlags.Default, null, commercialConstructorArguments, null, (new Object[] { }), null);
                SubSystemBase commercialSubSystem = (commercialSubSystemHandle.Unwrap() as SubSystemBase);
                commercialSubSystem.RefreshSearchResults += new EventHandler<RefreshSearchEventArgs>((object sender, RefreshSearchEventArgs e) => OnRefreshSearchResults(e));
                subSystems.Add(commercialSubSystem);
            }

            return subSystems;
        }

        public List<string> GetCommercialSubSystemNames()
        {
            return SubSystems.Select(p => p.Name).ToList();
        }

        public List<SqlUser> GetAssignees(string subSystemName)
        {
            List<SqlUser> assignees = new List<SqlUser>(UheaaEmployees);
            assignees.AddRange(SubSystems.Single(p => p.Name == subSystemName).GetFormerEmployeeAssignees());
            return assignees;
        }

        public List<SqlUser> GetCourts(string subSystemName)
        {
            List<SqlUser> courts = new List<SqlUser>(UheaaEmployees);
            courts.AddRange(SubSystems.Single(p => p.Name == subSystemName).GetFormerEmployeeCourts());
            return courts;
        }

        public List<SqlUser> GetRequesters(string subSystemName)
        {
            List<SqlUser> requesters = new List<SqlUser>(UheaaEmployees);
            requesters.AddRange(SubSystems.Single(p => p.Name == subSystemName).GetFormerEmployeeRequesters());
            return requesters;
        }

        public DateTime GetEarliestTicketCreateDate(string subSystemName)
        {
            return SubSystems.Single(p => p.Name == subSystemName).GetEarliestTicketCreateDate();
        }

        public List<string> GetStatuses(string subSystemName)
        {
            return SubSystems.Single(p => p.Name == subSystemName).GetStatuses();
        }

        public List<string> GetSubjects(string subSystemName)
        {
            return SubSystems.Single(p => p.Name == subSystemName).GetSubjects();
        }

        public List<TicketType> GetTicketTypes(string subSystemName)
        {
            if (subSystemName == "Facilities")
            {
                List<TicketType> type = new List<TicketType>();
                type.Add(new TicketType() { Abbreviation = "FAC", Description = "Facilities" });
                return type;
            }
            else if (subSystemName == "System Access")
            {
                List<TicketType> type = new List<TicketType>();
                type.Add(new TicketType() { Abbreviation = "SASR", Description = "System Access Request" });
                return type;
            }
            else if (subSystemName == "Building Access")
            {
                List<TicketType> type = new List<TicketType>();
                type.Add(new TicketType() { Abbreviation = "BAC", Description = "Building Access Request" });
                return type;
            }
            else
            {
                List<TicketType> type = new List<TicketType>();
                type.AddRange(SubSystems.Single(p => p.Name == subSystemName).GetTicketTypes());
                type.Insert(0, new TicketType());
                return type;
            }
        }

        #region Custom events

        public event EventHandler<RefreshSearchEventArgs> RefreshSearchResults;
        protected virtual void OnRefreshSearchResults(RefreshSearchEventArgs e)
        {
            RefreshSearchResults?.Invoke(this, e);
        }

        #endregion Custom events
    }
}