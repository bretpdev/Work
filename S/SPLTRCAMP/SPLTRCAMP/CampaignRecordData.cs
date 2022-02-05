using Q;

namespace SPLTRCAMP
{
    public class CampaignRecordData : PersonDemographics
    {

        public string AccountNumber { get; set; }
        public string CCC { get; set; }
        public string ACSKeyline { get; set; }
        public string Gen1 { get; set; }
        public string Gen2 { get; set; }
        public string Gen3 { get; set; }
        public string SSN { get; set; }

        public CampaignRecordData()
            : base()
        {
            SSN = string.Empty;
            AccountNumber = string.Empty;
        }

    }
}
