using System;
using System.Collections.Generic;

namespace FEDECORPRT
{
    public class PrintProcessingData
    {
        public int PrintProcessingId { get; set; }
        public string AccountNumber { get; set; }
        public string BF_SSN { get; set; } 
        public string SourceFile { get; set; }
        public int? ArcAddProcessingId { get; set; }
        public bool ArcNeeded { get; set; }
        public DateTime? ImagedAt { get; set; }
        public bool ImagingNeeded { get; set; }
        public DateTime? EcorrDocumentCreatedAt { get; set; }
        public DateTime? PrintedAt { get; set; }
        public bool OnEcorr { get; set; }
        public string LetterData { get; set; }
        public string WordDataFile { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? BillCreateDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? BillTotalDue { get; set; }
        public int? BillSeq { get; set; }
        public Dictionary<string, string> Barcodes { get; set; }
        public string BarcodeHeader { get; set; }
        public string LetterDataConst { get; set; }
        public string CostCenter { get; set; }
        public string BorrowerSsn { get; set; }

        public PrintProcessingData()
        {
            
        }
        public void MarkEcorrDone(DataAccess da, bool isCoBorrower)
        {
            if(!isCoBorrower)
            {
                da.MarkEcorrDone(this.PrintProcessingId);
            }
            else
            {
                da.MarkEcorrDoneCoBorrower(this.PrintProcessingId);
            }
        }

        public void MarkImagingDone(DataAccess da, bool isCoBorrower)
        {
            if (!isCoBorrower)
            {
                da.MarkImaged(this.PrintProcessingId);
            }
            else
            {
                da.MarkImagedCoBorrower(this.PrintProcessingId);
            }
        }

        public void MarkArcAdded(int arcAddProcessingId, DataAccess da)
        {
            da.MarkArcAdded(this.PrintProcessingId, arcAddProcessingId);
        }

        public void MarkPrintingDone(DataAccess da, bool isCoBorrower)
        {
            if(!isCoBorrower)
            {
                da.MarkPrintingComplete(this.PrintProcessingId);
            }
            else
            {
                da.MarkPrintingCompleteCoBorrower(this.PrintProcessingId);
            }
        }
    }
}