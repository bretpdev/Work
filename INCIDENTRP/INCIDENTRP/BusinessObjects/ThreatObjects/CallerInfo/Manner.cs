namespace INCIDENTRP
{
    public class Manner : IsEmptyBase
    {
        public bool Angry { get; set; }
        public bool BusinessLike { get; set; }
        public bool Calm { get; set; }
        public bool Coherent { get; set; }
        public bool Deliberate { get; set; }
        public bool Emotional { get; set; }
        public bool IllAtEase { get; set; }
        public bool Incoherent { get; set; }
        public bool Irrational { get; set; }
        public bool Laughing { get; set; }
        public bool Rational { get; set; }
        public bool Righteous { get; set; }
        public bool Shouting { get; set; }
        public bool Slow { get; set; }
        public bool Other { get; set; }
        public string OtherDescription { get; set; }

        public static Manner Load(DataAccess dataAccess, long ticketNumber)
        {
            return dataAccess.LoadManner(ticketNumber);
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            if (IsEmpty())
                dataAccess.DeleteManner(ticketNumber);
            else
                dataAccess.SaveManner(this, ticketNumber);
        }
    }
}
