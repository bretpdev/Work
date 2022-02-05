namespace INCIDENTRP
{
	public class PhysicalDamageLossOrTheftIncident : IncidentBase
	{
		public bool DataWasEncrypted { get; set; }
		public bool DesktopWasDamaged { get; set; }
		public bool DesktopWasLost { get; set; }
		public bool DesktopWasStolen { get; set; }
		public bool LaptopWasDamaged { get; set; }
		public bool LaptopWasLost { get; set; }
		public bool LaptopWasStolen { get; set; }
		public bool MicrofilmWithRecordsContainingPiiWasLost { get; set; }
		public bool MicrofilmWithRecordsContainingPiiWasStolen { get; set; }
		public bool MobileComunicationDeviceWasLost { get; set; }
		public bool MobileComunicationDeviceWasStolen { get; set; }
		public bool PaperRecordWithPiiWasLost { get; set; }
		public bool PaperRecordWithPiiWasStolen { get; set; }
		public bool RemovableMediaWithPiiWasLost { get; set; }
		public bool RemovableMediaWithPiiWasStolen { get; set; }
		public bool WindowOrDoorWasDamaged { get; set; }

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SavePhysicalDamageLossOrTheftIncident(this, ticketNumber);
		}
	}
}
