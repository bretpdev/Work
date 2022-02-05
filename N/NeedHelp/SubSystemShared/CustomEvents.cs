using System;

namespace SubSystemShared
{
	public class ChangeTicketEventArgs : EventArgs
	{
		public readonly string TicketCode;
		public readonly long TicketNumber;

		public ChangeTicketEventArgs(string ticketCode, long ticketNumber)
		{
			TicketCode = ticketCode;
			TicketNumber = ticketNumber;
		}
	}

	public class RefreshSearchEventArgs : EventArgs
	{
		public RefreshSearchEventArgs() 
		{
		}
	}
}//namespace