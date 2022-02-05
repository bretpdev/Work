using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace UHECORPRT
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
        public bool InValidAddress { get; set; }
        public bool DoNotProcessEcorr { get; set; }
        public string CostCenter { get; set; }
        public string LetterDataConst { get; set; }

        public void MarkEcorrDone(DataAccess DA, bool isCoBorrower)
        {
            if(!isCoBorrower)
            {
                DA.MarkEcorrDone(this.PrintProcessingId);
            }
            else
            {
                DA.MarkEcorrDoneCoBorrower(this.PrintProcessingId);
            }
        }

        public void MarkImagingDone(DataAccess DA, bool isCoBorrower)
        {
            if(!isCoBorrower)
            {
                DA.MarkImaged(this.PrintProcessingId);
            }
            else
            {
                DA.MarkImagedCoBorrower(this.PrintProcessingId);
            }
        }

        public void MarkArcAdded(DataAccess DA, int arcAddProcessingId)
        {
            DA.MarkArcAdded(this.PrintProcessingId, arcAddProcessingId);
        }

        public void MarkPrintingDone(DataAccess DA, bool isCoBorrower)
        {
            if(!isCoBorrower)
            {
                DA.MarkPrintingComplete(this.PrintProcessingId);
            }
            else
            {
                DA.MarkPrintingCompleteCoBorrower(this.PrintProcessingId);
            }
        }
    }
}
