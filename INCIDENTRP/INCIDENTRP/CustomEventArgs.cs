using System;
using System.Windows.Forms;

namespace INCIDENTRP
{
	class CancelTicketEventArgs : EventArgs
	{
		public readonly string TicketType;
		public readonly DateTime CreateDateTime;
		public readonly string AccountNumber;

		public CancelTicketEventArgs(string ticketType, DateTime createDateTime, string accountNumber)
		{
			TicketType = ticketType;
			CreateDateTime = createDateTime;
			AccountNumber = accountNumber;
		}
	}

	class NextFlowStepEventArgs : EventArgs
	{
		public readonly Ticket Ticket;
		public readonly string UpdateText;
		public readonly Form DisplayForm;

		public NextFlowStepEventArgs(Ticket ticket, string updateText, Form displayForm)
		{
			Ticket = ticket;
			UpdateText = updateText;
			DisplayForm = displayForm;
		}
	}

	class SaveTicketEventArgs : EventArgs
	{
		public readonly Ticket Ticket;
		public readonly Form DisplayForm;

		public SaveTicketEventArgs(Ticket ticket, Form displayForm)
		{
			Ticket = ticket;
			DisplayForm = displayForm;
		}
	}

	class UpdateTicketEventArgs : EventArgs
	{
		public readonly Ticket Ticket;
		public readonly string UpdateText;
		public readonly Form DisplayForm;

		public UpdateTicketEventArgs(Ticket ticket, string updateText, Form dispalyForm)
		{
			Ticket = ticket;
			UpdateText = updateText;
			DisplayForm = dispalyForm;
		}
	}

	class ValidityChangedEventArgs : EventArgs
	{
		public readonly bool IsValid;

		public ValidityChangedEventArgs(bool isValid)
		{
			IsValid = isValid;
		}
	}
}