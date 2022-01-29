namespace INCIDENTRP
{
    public class Voice : IsEmptyBase
    {
        public bool Distinct { get; set; }
        public bool Distorted { get; set; }
        public bool Fast { get; set; }
        public bool High { get; set; }
        public bool Hoarse { get; set; }
        public bool Lisp { get; set; }
        public bool Nasal { get; set; }
        public bool Slow { get; set; }
        public bool Slurred { get; set; }
        public bool Stuttering { get; set; }
        public bool Other { get; set; }
        public string OtherDescription { get; set; }

        public static Voice Load(DataAccess dataAccess, long ticketNumber)
        {
            return dataAccess.LoadVoice(ticketNumber);
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            if (IsEmpty())
                dataAccess.DeleteVoice(ticketNumber);
            else
                dataAccess.SaveVoice(this, ticketNumber);
        }
    }
}
