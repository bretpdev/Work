using System;
using System.Linq;

namespace INCIDENTRP
{
	public class ActionTaken
	{
		//Valid values for Action, according to the LST_Action table:
		public const string ASKED_CALLER_TO_RETURN_CORRESPONDENCE = "Asked Caller to Return Correspondence";
		public const string CONTACTED_IT_INFORMATION_SECURITY_OFFICE = "Contacted IT/Information Security Office";
		public const string CONTACTED_LAW_ENFORCEMENT = "Contacted Law Enforcement";
		public const string CORRECTED_DATA = "Corrected Data";
		public const string DELETED_FILES = "Deleted Files";
		public const string LOGGED_OFF_SYSTEM = "Logged Off System";
		public const string NOTIFIED_AFFECTED_INDIVIDUAL = "Notified Affected Individual";
		public const string REBOOTED_SYSTEM = "Rebooted System";
		public const string REMOVED_SYSTEM_FROM_NETWORK = "Removed System from Network";
		public const string SHUT_DOWN_SYSTEM = "Shut Down System";

		public bool ActionWasTaken { get; set; }
		public string Action { get; set; }
		public DateTime DateTime { get; set; }
		public string PersonContacted { get; set; }

		public ActionTaken(string action)
		{
			Action = action;
			DateTime = DateTime.Now;
		}

		public bool IsComplete()
		{
			//DateTime will never be null, so the only thing we can check is PersonContacted
			//for the actions where contacting a person is required.
			string[] actionsRequiringPersonContact = { CONTACTED_IT_INFORMATION_SECURITY_OFFICE, CONTACTED_LAW_ENFORCEMENT, NOTIFIED_AFFECTED_INDIVIDUAL };
			if (actionsRequiringPersonContact.Contains(Action))
                return !string.IsNullOrEmpty(PersonContacted);
			return true;
		}

		public static ActionTaken Load(DataAccess dataAccess, long ticketNumber, string ticketType, string action)
		{
			return dataAccess.LoadActionTaken(ticketNumber, ticketType, action);
		}

		public void Save(DataAccess dataAccess, long ticketNumber, string ticketType)
		{
			if (ActionWasTaken)
			 dataAccess.SaveActionTaken(this, ticketNumber, ticketType); 
			else
			 dataAccess.DeleteActionTaken(Action, ticketNumber, ticketType); 
		}
	}
}
