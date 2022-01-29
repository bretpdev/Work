namespace PWRATRNY
{
    public class UserPOAEntry
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ExpirationDate { get; set; }

		/// <summary>
		/// Only use this property until the system translation has been done.
		/// Only use SSN and AccountNumber variables once translation is complete.
		/// </summary>
		public string UserEnteredAccountNumberOrSSN { get; set; }

		public UserPOAEntry()
		{
			UserEnteredAccountNumberOrSSN = "";
			FirstName = "";
			LastName = "";
			ExpirationDate = "";
		}
	}
}