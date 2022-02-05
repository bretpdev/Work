namespace INCIDENTRP
{
    public class ThreatInfo : IsEmptyBase
    {
        public string WordingOfThreat { get; set; }
        public string NatureOfCall { get; set; }
        public string AdditionalRemarks { get; set; }

        public static ThreatInfo Load(DataAccess dataAccess, long ticketNumber)
        {
            return dataAccess.LoadThreatInfo(ticketNumber);
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            if (IsEmpty())
                dataAccess.DeleteThreatInfo(ticketNumber);
            else
                dataAccess.SaveThreatInfo(this, ticketNumber);
        }
    }
}
