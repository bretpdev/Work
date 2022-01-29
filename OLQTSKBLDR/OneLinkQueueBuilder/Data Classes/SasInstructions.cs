namespace OLQBuilder
{
    class SasInstructions
    {
		public string Filename { get; set; }

		public bool IgnoreEmptyFiles { get; set; }

		public bool IgnoreMissingFile { get; set; }

		public bool ProcessMultipleFiles { get; set; }

		public SasInstructions(QueueBuilderList qb)
		{
			Filename = qb.FileName;
			IgnoreEmptyFiles = (qb.Empty.ToUpper() == "Y");
			IgnoreMissingFile = (qb.NoFile.ToUpper() == "Y");
			ProcessMultipleFiles = (qb.MultiFile.ToUpper() == "Y");
		}
    }

	/// <summary>
	/// Projection class for reading straight from the BSYS table.
	/// </summary>
    class QueueBuilderList
    {
        public string FileName { get; set; }
        public string Empty { get; set; }
        public string NoFile { get; set; }
        public string MultiFile { get; set; }
	}
}
