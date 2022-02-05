namespace ACURINTR
{
	//Projection class for the DEMS_DAT_Queues table in BSYS.
	public class QueueData
	{
		public string Queue { get; set; }
		public string Department { get; set; }
		public string System { get; set; }
		public string Parser { get; set; }
		public string Processor { get; set; }
		public string DemographicsReviewQueue { get; set; }
		public string ForeignReviewQueue { get; set; }

		/// <summary>
		/// Container for constants representing valid values for System.
		/// </summary>
		/// <remarks>
		/// The values of these constants must exactly match the distinct values
		/// of the System column from the DEMS_DAT_Queues table in BSYS.
		/// </remarks>
		public class Systems
		{
			public const string COMPASS = "COMPASS";
			public const string ONELINK = "OneLINK";
		}
	}
}