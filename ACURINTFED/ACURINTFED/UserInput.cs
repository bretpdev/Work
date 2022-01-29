namespace ACURINTFED
{
	class UserInput
	{
		public enum Selection
		{
			Continue,
			CreateRequestFile,
			GetResponseFile,
			ProcessResponseFile,
			SendRequestFile
		}

		public Selection Selected { get; set; }
	}//class
}//namespace
