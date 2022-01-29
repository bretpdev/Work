namespace CentralizedPrintingProcess
{
    public class PendingFaxConfirmation
    {
        public int FaxHandle { get; set; }
        public string CvrDoc { get; set; }
        public string FaxDoc { get; set; }
        public FaxRecord FaxRecord { get; set; }
        public PendingFaxConfirmation(int faxHandle, string cvrDoc, string faxDoc, FaxRecord record)
        {
            FaxHandle = faxHandle;
            CvrDoc = cvrDoc;
            FaxDoc = faxDoc;
            FaxRecord = record;
        }
    }
}