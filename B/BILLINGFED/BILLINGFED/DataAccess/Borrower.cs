using System;
using System.Collections.Generic;
using System.IO;
using Uheaa.Common;
using Uheaa.Common.DocumentProcessing;

namespace BILLINGFED
{
    public class Borrower
    {
        public int PrintProcessingId { get; set; }
        public string AccountNumber { get; set; }
        public string CoBorrowerAccountNumber { get; set; }
        public string Ssn { get; set; }
        public string SourceFile { get; set; }
        public DateTime? ArcAddedAt { get; set; }
        public DateTime? ImagedAt { get; set; }
        public DateTime? PrintedAt { get; set; }
        public DateTime? EcorrDocumentCreatedAt { get; set; }
        public bool OnEcorr { get; set; }
        public bool IsEndorser { get; set; }
        public List<string> LineData { get; set; }
        private string imagingFile;
        public string GetImagingFile(DataAccess da)
        {
            if (imagingFile.IsNullOrEmpty())
            {
                string path = $"{BillingStatementsFed.BillingDirectory}\\{BillingStatementsFed.ScriptId}_{AccountNumber}_{PrintProcessingId}.pdf";
                if (!File.Exists(path))
                {
                    EcorrData ecorrInfo = EcorrProcessing.CheckEcorr(AccountNumber);
                    LineData = BillingStatementsFed.DA.GetLineData(PrintProcessingId);
                    new GeneratePdfs().GeneratePrintDocument(new PrintData(this, ecorrInfo, BillingStatementsFed.ScriptId, "UT00801", ParseReportNumber().Value, BillingStatementsFed.LETTER_ID, BillingStatementsFed.BillingDirectory, BillingStatementsFed.LogRun), da);
                }
                imagingFile = path;
            }
            return imagingFile;
        }

        public void SetImagingFile(string imagingFile)
        {
            this.imagingFile = imagingFile;
        }

        public Borrower()
        {
            LineData = new List<string>();
        }

        public void SetEcorrDocumentCreated()
        {
            EcorrDocumentCreatedAt = BillingStatementsFed.DA.SetEcorrDocumentCreated(PrintProcessingId);
        }

        public void SetImagedAt()
        {
            ImagedAt = BillingStatementsFed.DA.SetImagedAt(PrintProcessingId);
        }

        public void SetPrintedAt()
        {
            BillingStatementsFed.DA.SetPrintedAt(PrintProcessingId);
        }

        public void SetArcAddedAt()
        {
            ArcAddedAt = BillingStatementsFed.DA.SetArcProcessed(PrintProcessingId);
        }

        public int? ParseReportNumber()
        {
            int? reportNumber = null;
            if (SourceFile.Length >= 14 && SourceFile.Length <= 16) //SAS file does not have a data on the end of the name
            {
                reportNumber = SourceFile.Length == 15 ? SourceFile.Substring(SourceFile.LastIndexOf("NWS05R") + 6, 2).ToIntNullable() :
                   SourceFile.Substring(SourceFile.LastIndexOf("NWS05R") + 6, 1).ToIntNullable();
            }
            else //SAS file has a date on the end of the name
            {
                reportNumber = SourceFile.Substring(SourceFile.LastIndexOf("NWS05R") + 6, 2).ToIntNullable();
                if (!reportNumber.HasValue)
                    reportNumber = SourceFile.Substring(SourceFile.LastIndexOf("NWS05R") + 6, 1).ToIntNullable();
            }
            return reportNumber;
        }
    }
}