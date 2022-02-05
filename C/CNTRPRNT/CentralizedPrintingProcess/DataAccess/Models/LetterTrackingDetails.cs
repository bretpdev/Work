namespace CentralizedPrintingProcess
{
	public class LetterTrackingDetails
	{
		public string LetterId { get; set; }
		public bool SpecialHandling { get; set; }
		public decimal Pages { get; set; }
		public string Instructions { get; set; }
		public string CostCenter { get; set; }
		public bool Duplex { get; set; }
	}
}