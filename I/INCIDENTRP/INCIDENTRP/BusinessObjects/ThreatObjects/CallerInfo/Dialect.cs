namespace INCIDENTRP
{
    public class Dialect : IsEmptyBase
    {
        public bool English { get; set; }
        public bool RegionalAmerican { get; set; }
        public string RegionalAmericanDescription { get; set; }
        public bool ForeignAccent { get; set; }
        public string ForeignAccentDescription { get; set; }

        public static Dialect Load(DataAccess dataAccess, long ticketNumber)
        {
            return dataAccess.LoadDialect(ticketNumber);
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            if (IsEmpty())
                dataAccess.DeleteDialect(ticketNumber);
            else
                dataAccess.SaveDialect(this, ticketNumber);
        }
    }
}
