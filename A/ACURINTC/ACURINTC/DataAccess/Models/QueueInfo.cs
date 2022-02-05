namespace ACURINTC
{
	public class QueueInfo
	{
        public int QueueInfoId { get; set; }
		public string Queue { get; set; }
		public string SubQueue { get; set; }
		public Parser ParserId { get; set; }
		public Processor ProcessorId { get; set; }
		public string DemographicsReviewQueue { get; set; }
		public string ForeignReviewQueue { get; set; }
	}
}