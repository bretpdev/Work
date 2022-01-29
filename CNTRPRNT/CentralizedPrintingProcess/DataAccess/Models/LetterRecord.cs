using System;

namespace CentralizedPrintingProcess
{
    public class LetterRecord : PropsToStringBase
    {
        public string UHEAACostCenter { get; set; }
        public string PagesToPrint { get; set; }
        public bool Duplex { get; set; }
        public string DomesticCalc { get; set; }
        public string SummaryCount { get; set; }
        public string LetterID { get; set; }
        public string BusinessUnit { get; set; }
        public int SeqNum { get; set; }
        public string AccountNumber { get; set; }
        public string Instructions { get; set; }
        public DateTime? PrintedAt { get; set; }
        public DateTime? EcorrDocumentCreatedAt { get; set; }

        public string ToShortString()
        {
            return string.Join(",", UHEAACostCenter ?? "", PagesToPrint, Duplex, DomesticCalc, LetterID, BusinessUnit, SeqNum.ToString(), AccountNumber, SummaryCount);
        }

        public bool MatchesSummary(LetterRecord summary)
        {
            return LetterID == summary.LetterID
                && DomesticCalc == summary.DomesticCalc
                && UHEAACostCenter == summary.UHEAACostCenter
                && Duplex == summary.Duplex
                && PagesToPrint == summary.PagesToPrint
                && Instructions == summary.Instructions;
        }
    }
}