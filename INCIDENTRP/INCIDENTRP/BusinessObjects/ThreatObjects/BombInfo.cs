namespace INCIDENTRP
{
    public class BombInfo : IsEmptyBase
    {
        public string Location { get; set; }
        public string DetonationTime { get; set; }
        public string Appearance { get; set; }
        public string WhoPlacedAndWhy { get; set; }
        public string CallerName { get; set; }

        public static BombInfo Load(DataAccess dataAccess, long ticketNumber)
        {
            return dataAccess.LoadBombInfo(ticketNumber);
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            if (IsEmpty())
                dataAccess.DeleteBombInfo(ticketNumber);
            else
                dataAccess.SaveBombInfo(this, ticketNumber);
        }
    }
}
