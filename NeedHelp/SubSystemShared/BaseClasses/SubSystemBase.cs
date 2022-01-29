using System;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;

namespace SubSystemShared
{
	/// <summary>
	/// Base class for all subsystems.
	/// Classes that inherit this class must be called "SubSystem" in order for the plug-in architecture to work.
	/// They must also be in a namespace whose name is exactly the same as the DLL that is generated when compiled.
	/// </summary>
    abstract public class SubSystemBase
    {

        protected SqlUser User { get; set; }
        protected string Role { get; set; }

        public bool IsOnlyFacilities { get; set; }
        public ProcessLogRun LogRun { get; set; }
		/// <summary>
		/// Checks whether the user is authorized to use the subsystem.
		/// </summary>
		abstract public bool HasAccess { get; }
		public string Name
		{
			get
			{
				return BaseName;
			}
		}
		protected abstract string BaseAbbreviation { get; }
		protected abstract string BaseName { get; }

		public SubSystemBase(SqlUser user, string role, ProcessLogRun logRun)
        {
			User = user;
            Role = role;
            IsOnlyFacilities = false;
            LogRun = logRun;
        }

        /// <summary>
		/// Displays the selected ticket in the subsystem user interface.
        /// </summary>
        abstract public void DisplayTicket(string ticketCode, long ticketNumber, List<SqlUser> users);

        /// <summary>
		/// Creates a new ticket in subsystem.
        /// </summary>
		abstract public void CreateNewTicket(TicketType ticketType, List<SqlUser> users);

		/// <summary>
		/// Searches the subsystem for all tickets.
		/// </summary>
		abstract public List<SearchResultItem> SearchForAllTickets(SearchCriteria criteria);

		/// <summary>
		/// Searches the subsystem for open tickets.
		/// </summary>
		abstract public List<SearchResultItem> SearchForOpenTickets(SearchCriteria criteria);

        /// <summary>
		/// Searches the subsystem for tickets that match the criteria.
        /// </summary>
        abstract public List<SearchResultItem> SearchForTickets(SearchCriteria criteria);

		/// <summary>
		/// Gets the earliest date a ticket was created in this subsystem.
		/// </summary>
		abstract public DateTime GetEarliestTicketCreateDate();

		/// <summary>
		/// Gets former employees who have tickets assigned to them.
		/// </summary>
        abstract public List<SqlUser> GetFormerEmployeeAssignees();

		/// <summary>
		/// Gets former employees who have tickets in their courts.
		/// </summary>
        abstract public List<SqlUser> GetFormerEmployeeCourts();

		/// <summary>
		/// Gets former employees who requested tickets.
		/// </summary>
        abstract public List<SqlUser> GetFormerEmployeeRequesters();

		/// <summary>
        /// Gets all ticket types for subsystem
        /// </summary>
		abstract public List<TicketType> GetTicketTypes();

		/// <summary>
		/// Gets all subjects of existing tickets.
		/// </summary>
		abstract public List<string> GetSubjects();

		/// <summary>
		/// Gets all statuses of existing tickets.
		/// </summary>
		abstract public List<string> GetStatuses();

		#region Custom events

		public event EventHandler<RefreshSearchEventArgs> RefreshSearchResults;
		protected virtual void OnRefreshSearchResults(RefreshSearchEventArgs e)
		{
            RefreshSearchResults?.Invoke(this, e);
        }

		#endregion Custom events
	}
}