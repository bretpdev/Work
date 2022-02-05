using System;

namespace DPAPOST
{
    public class FileData
    {
        public int PostingDataId { get; set; }
        public string AccountNumber { get; set; }
        public double Amount { get; set; }
        public DateTime AddedAt { get; set; }
    }
}