namespace BATCHESP
{
    /// <summary>
    /// Reasons displayed in Session concerning why a deferment cannot be submitted
    /// with its current settings (such as date range).
    /// </summary>
    public class NonSelectionReason
    {
        public int NonSelectionReasonId { get; set; }
        public string Reason { get; set; }
        public string Course { get; set; }
    }
}