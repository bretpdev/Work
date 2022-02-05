namespace ACURINTC
{
	public class RejectAction
	{
        public int RejectActionId { get; set; }
		public RejectReason RejectReasonId { get; set; }
        public DemographicsSource DemographicsSourceId { get; set; }
		public string ActionCodeAddress { get; set; }
		public string ActionCodePhone { get; set; }
		public string ActionCodeEmail { get; set; }
	}
}