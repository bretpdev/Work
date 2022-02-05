using System.Collections.Generic;

namespace TCPAPNS
{
    public class FileProcessingRecord
    {
        //Set by sql call directly
        public int FileProcessingId { get; set; }
        public string AccountNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Date { get; set; }
        public bool MobileIndicator { get; set; }
        public string LineData { get; set; }
        public bool HasConsentArc { get; set; }
        public bool HasConsentArcOneLink { get; set; }
        public string SourceFile { get; set; }
        //Set by sql call indirectly
        public List<string> PhoneTypes { get; set; }
    }
}