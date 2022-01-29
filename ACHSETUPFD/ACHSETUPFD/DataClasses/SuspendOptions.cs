namespace ACHSETUPFD
{
	class SuspendOptions
	{
		public enum Requestor
		{
			Borrower,
			Staff
		}

		public Requestor SelectedRequestor { get; set; }
		public int NumberOfBills { get; set; }
	}
}
