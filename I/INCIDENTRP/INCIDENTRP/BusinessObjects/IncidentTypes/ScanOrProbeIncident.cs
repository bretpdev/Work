namespace INCIDENTRP
{
	public class ScanOrProbeIncident : IncidentBase
	{
		public bool UnauthorizedProgramOrSnifferDevice { get; set; }
		public bool PrioritySystemAlarmOrIndicationFromIds { get; set; }
		public bool UnauthorizedPortScan { get; set; }
		public bool UnauthroizedVulnerabilityScan { get; set; }

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveScanOrProbeIncident(this, ticketNumber);
		}
	}
}
