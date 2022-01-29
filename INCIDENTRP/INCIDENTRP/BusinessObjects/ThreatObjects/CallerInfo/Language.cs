namespace INCIDENTRP
{
    public class Language : IsEmptyBase
    {
        public bool Educated { get; set; }
        public bool Uneducated { get; set; }
        public bool FoulOrProfane { get; set; }
        public bool Other { get; set; }
        public string OtherDescription { get; set; }

        public static Language Load(DataAccess dataAccess, long ticketNumber)
        {
            return dataAccess.LoadLanguage(ticketNumber);
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            if (IsEmpty())
                dataAccess.DeleteLanguage(ticketNumber);
            else
                dataAccess.SaveLanguage(this, ticketNumber);
        }
    }
}
