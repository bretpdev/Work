namespace ACCURINT
{
	class UserInput
	{
		public enum Selection
		{
			CreateRequestFile,
			GetResponseFile,
			ProcessResponseFile,
			RunAll,
			SendInputFile,
			SendRequestFile
		}

		public Selection Selected { get; set; }
        public bool OnlyProcessFirstSelection { get; set; }
	}
}
