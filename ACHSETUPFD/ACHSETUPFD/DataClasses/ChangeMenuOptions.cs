namespace ACHSETUPFD
{
	class ChangeMenuOptions
	{
		public enum ChangeOption
		{
			AddRemove,
			Modify
		}

		public ChangeOption Selection { get; set; }
	}
}