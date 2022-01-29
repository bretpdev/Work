namespace ACDCAccess
{
	class User
	{
		public string Name { get; set; }
		public int SqlUserId { get; set; }

		public User()
		{
			Name = "";
			SqlUserId = 0;
		}
	}
}
