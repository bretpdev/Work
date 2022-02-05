using Q;

namespace SPLTRCAMP
{
    public class CampaignData
    {

        public enum Action
        {
            Test,
            Run,
            None
        }

        public string DataFile { get; set; }
        public string CalculatedDataFile { get; set; }
        public string LetterFile { get; set; }
        public string LetterIDFromLTS { get; set; }
        public Entry.Recipient Recipient { get; set; }
        public string LetterDescriptionForCCCCoverSheet { get; set; }
        public Entry.PageCountAndDestination PageCountOrDestination { get; set; }
        public bool AddCommentsToOneLINK { get; set; }
        public bool AddCommentsToCompass { get; set; }
        public string ARC { get; set; }
        public string ActionCode { get; set; }
        public string Comment { get; set; }
        public Action ActionSelected { get; set; }

        public CampaignData()
        {
            DataFile = string.Empty;
            LetterFile = string.Empty;
            AddCommentsToOneLINK = false;
            AddCommentsToCompass = false;
            ARC = "ARC";
            ActionCode = "Action Code";
            Comment = string.Empty;
            ActionSelected = Action.None;
            LetterIDFromLTS = string.Empty;
            LetterDescriptionForCCCCoverSheet = string.Empty;
            Recipient = Entry.Recipient.Borrower;
            PageCountOrDestination = Entry.PageCountAndDestination.One;
        }

    }
}
