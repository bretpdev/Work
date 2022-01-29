namespace ACHSETUP
{
    public class ARCTracker
    {
        public string RequestedDate { get; set; }
        public bool Nulled { get; set; }

        public ARCTracker(string tRequestedDate)
        {
            RequestedDate = tRequestedDate.Insert(6, "20");
        }
    }
}