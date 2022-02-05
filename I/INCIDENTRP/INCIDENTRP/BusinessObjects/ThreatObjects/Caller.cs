namespace INCIDENTRP
{
    public class Caller : IsEmptyBase
    {
        //Valid values for Sex, according to the LST_Sex table:
        public const string FEMALE = "Female";
        public const string MALE = "Male";
        public const string UNSURE = "Unsure";

        public string CallDuration { get; set; }
        public bool RecognizedTheCallersVoice { get; set; }
        public bool CallerIsFamiliarWithUheaa { get; set; }
        public string FamiliaritySpecifics { get; set; }
        public string Sex { get; set; }
        public string Age { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string AccountNumber { get; set; }
        public Voice Voice { get; set; }
        public Language Language { get; set; }
        public Dialect Dialect { get; set; }
        public Manner Manner { get; set; }
        public BackgroundNoise BackgroundNoise { get; set; }

        public Caller()
        {
            Sex = UNSURE;
            Voice = new Voice();
            Language = new Language();
            Dialect = new Dialect();
            Manner = new Manner();
            BackgroundNoise = new BackgroundNoise();
        }

        public static Caller Load(DataAccess dataAccess, long ticketNumber)
        {
            Caller caller = dataAccess.LoadCaller(ticketNumber);
            caller.Voice = Voice.Load(dataAccess, ticketNumber);
            caller.Language = Language.Load(dataAccess, ticketNumber);
            caller.Dialect = Dialect.Load(dataAccess, ticketNumber);
            caller.Manner = Manner.Load(dataAccess, ticketNumber);
            caller.BackgroundNoise = BackgroundNoise.Load(dataAccess, ticketNumber);
            return caller;
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            //Save our own basic properties.
            if (IsEmpty())
                dataAccess.DeleteCaller(ticketNumber);
            else
                dataAccess.SaveCaller(this, ticketNumber);

            //Call on member objects to save themselves.
            Voice.Save(dataAccess, ticketNumber);
            Language.Save(dataAccess, ticketNumber);
            Dialect.Save(dataAccess, ticketNumber);
            Manner.Save(dataAccess, ticketNumber);
            BackgroundNoise.Save(dataAccess, ticketNumber);
        }
    }
}
