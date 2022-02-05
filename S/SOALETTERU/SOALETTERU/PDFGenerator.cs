using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace SOALETTERU
{
    public class PDFGenerator
    {
        private LT20Data LetterData { get; set; }
        private DataAccess DA { get; set; }
        private ProcessLogRun PLR { get; set; }
        private DataTable LoanInfo { get; set; }
        private DataTable FinancialTrans { get; set; }
        private Font StateMailBarcodeFont { get; set; }
        private Font ReturnMailBarcodeFont { get; set; }

        public PDFGenerator(LT20Data letterData, DataTable loanInfo, DataTable financialTrans, DataAccess da, ProcessLogRun plr)
        {
            DA = da;
            PLR = plr;
            LetterData = letterData;
            LoanInfo = loanInfo;
            FinancialTrans = financialTrans;
            BaseFont customfont = BaseFont.CreateFont(EnterpriseFileSystem.GetPath("Barcode Font") + "\\IDAutomationDMatrix.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            StateMailBarcodeFont = new Font(customfont, 8);
            ReturnMailBarcodeFont = new Font(customfont, 4);
        }

        /// <summary>
        /// Creates an ecorr or printed PDF
        /// </summary>
        public bool CreatePdf()
        {
            AddressInfo addressLine = DA.GetAddress(LetterData);
            string ecorrFile = "";
            string cpFile = "";
            DocumentProperties prop = null;
            if (LetterData.OnEcorr && !LetterData.EcorrDocumentCreatedAt.HasValue)//Borrower is on Ecorr
                ProcessEcorr(addressLine, ecorrFile, prop);
            else if (!LetterData.PrintedAt.HasValue || !LetterData.EcorrDocumentCreatedAt.HasValue)
            {
                if (!ProcessNonEcorr(addressLine, ecorrFile, cpFile, prop))//Borrower is not on Ecorr
                    return false;
            }

            return true;
        }

        private bool ProcessNonEcorr(AddressInfo addressLine, string ecorrFile, string cpFile, DocumentProperties prop)
        {
            if (!LetterData.EcorrDocumentCreatedAt.HasValue)
                ecorrFile = GeneratePdf(addressLine);
            if (addressLine.HasValidAddress)
                cpFile = GeneratePdf(addressLine);

            string path = EnterpriseFileSystem.GetPath("ECORRDocuments") + Path.GetFileName(ecorrFile);
            bool coBorrower = LetterData.CoborrowerSSN.IsPopulated();
            string ssn;
            if (coBorrower)
                ssn = LetterData.CoborrowerSSN;
            else
                ssn = LetterData.EndorsersSsn.IsNullOrEmpty() ? LetterData.RF_SBJ_PRC : LetterData.EndorsersSsn;

            if (!LetterData.EcorrDocumentCreatedAt.HasValue)
            {
                File.Move(ecorrFile, Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), Path.GetFileName(ecorrFile)));
                prop = new DocumentProperties(ssn, LetterData.DF_SPE_ACC_ID, LetterData.RM_DSC_LTR_PRC, "UT00204", LetterData.EmailAddress, DocumentProperties.CorrMethod.Printed, path);
                prop.InsertEcorrInformation();
                DA.UpdateEcorrDocCreated(LetterData);
            }


            if (!addressLine.HasValidAddress)
            {
                DA.InactivateLetter(LetterData, 5);
                return false;
            }
            if (!LetterData.PrintedAt.HasValue)
                AddBarcodesToPdfAndPrint(cpFile, LetterData.RM_DSC_LTR_PRC, !addressLine.Country.IsNullOrEmpty(), LetterData.DF_SPE_ACC_ID);

            Repeater.TryRepeatedly(() => File.Delete(cpFile));
            return true;
        }

        private void ProcessEcorr(AddressInfo addressLine, string ecorrFile, DocumentProperties prop)
        {
            ecorrFile = GeneratePdf(addressLine);
            string path = EnterpriseFileSystem.GetPath("ECORRDocuments") + Path.GetFileName(ecorrFile);
            bool coBorrower = LetterData.CoborrowerSSN.IsPopulated();
            string ssn;
            if (coBorrower)
                ssn = LetterData.CoborrowerSSN;
            else
                ssn = LetterData.EndorsersSsn.IsNullOrEmpty() ? LetterData.RF_SBJ_PRC : LetterData.EndorsersSsn;

            File.Move(ecorrFile, Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), Path.GetFileName(ecorrFile)));

            prop = new DocumentProperties(ssn, LetterData.DF_SPE_ACC_ID, LetterData.RM_DSC_LTR_PRC, "UT00204", LetterData.EmailAddress, DocumentProperties.CorrMethod.EmailNotify, path);
            prop.InsertEcorrInformation();

            DA.UpdateEcorrDocCreated(LetterData);
        }

        private void AddBarcodesToPdfAndPrint(string file, string letterId, bool isForeign, string accountNumber)
        {
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{1}{0}.pdf", Guid.NewGuid().ToBase64String(), letterId));
            //Read in the template PDF file
            using (PdfReader pdfReader = new PdfReader(file))
            {
                int numberOfPages = pdfReader.NumberOfPages;
                int duplexPages = numberOfPages / 2;

                duplexPages++;

                List<string> returnMailBarcodes = DocumentProcessing.GetReturnMailBarcodes(LetterData.DF_SPE_ACC_ID, LetterData.RM_DSC_LTR_PRC, DocumentProcessing.LetterRecipient.Borrower);
                List<string> stateMailBarcodes = DocumentProcessing.GetStateMailBarcodesforPdf(LetterData.DF_SPE_ACC_ID, duplexPages, DocumentProcessing.LetterRecipient.Borrower);

                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create)))
                {
                    int index = 0;
                    for (int page = 1; page <= numberOfPages; page += 2)
                    {
                        float linePosition = 755f;
                        PdfContentByte pdfContent = pdfStamper.GetOverContent(page);
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 15, linePosition, 0);
                        linePosition -= 8.05f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 15, linePosition, 0);
                        linePosition -= 8.05f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 15, linePosition, 0);
                        linePosition -= 8.05f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 15, linePosition, 0);
                        linePosition -= 8.05f;
                        if (page == 1)
                        {
                            float statemailPosition = 660f;
                            for (int mailIndex = 0; mailIndex < 6; mailIndex++)
                            {
                                ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(returnMailBarcodes[mailIndex], ReturnMailBarcodeFont), 280f, statemailPosition, 0);
                                statemailPosition -= 4.05f;
                            }
                        }
                    }
                }
                int pageCount = numberOfPages / 2;
                if (pageCount % 2 > 0)
                    pageCount++;

                string coverSheetDataFile = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("CoversheetFile{0}.txt", Guid.NewGuid().ToBase64String()));

                try
                {
                    using (StreamWriter sw = new StreamWriter(coverSheetDataFile, false))
                    {
                        sw.WriteCommaDelimitedLine("BU", "Description", "Cost", "Standard", "Foreign", "NumPages", "CoverComment");
                        sw.WriteCommaDelimitedLine("Borrower Services", "Statement of Accounts Borrower Letter", "MA2324", isForeign ? "0" : "1", isForeign ? "1" : "", pageCount.ToString(), "");
                    }
                }
                catch (Exception ex)
                {
                    PLR.AddNotification($"Failed to write out cover sheet data while trying to print SOA letter for account {accountNumber}. Exception encountered: {ex.Message}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }

                DocumentProcessing.PrintDocs(EnterpriseFileSystem.GetPath("CoverSheet"), "Scripted State Mail Cover Sheet", coverSheetDataFile);
                DocumentProcessing.PrintPdf(newFile);
                File.Delete(coverSheetDataFile);

                Repeater.TryRepeatedly(() => File.Delete(newFile));
                DA.UpdatePrinted(LetterData);
            }
        }

        private static string CalculateFileName(string letterId, string path = null)
        {
            if (path == null)
                path = EnterpriseFileSystem.TempFolder;

            return string.Format("{0}{1}{2}.pdf", path, letterId, Guid.NewGuid());
        }

        private string CreateLoanDetail(DataTable dt, string title, int startingPosition)
        {
            string pdfFilePath = Path.Combine(EnterpriseFileSystem.TempFolder, Guid.NewGuid() + "DataTable.pdf");
            using (Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35))
            {
                //Create Document class object and set its size to letter and give space left, right, Top, Bottom Margin
                using (PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create)))
                {
                    wri.SetTagged();
                    doc.Open();//Open Document to write

                    var font10 = FontFactory.GetFont("TIMES NEW ROMAN", 8);

                    if (dt != null)
                    {
                        //Create instance of the pdf table and set the number of column in that table
                        PdfPTable table = new PdfPTable(dt.Columns.Count);
                        PdfPCell cell = null;
                        table.HeaderRows = 1;
                        //Add Header of the pdf table
                        foreach (DataColumn col in dt.Columns)
                        {
                            cell = new PdfPCell();
                            cell.AddHeader(new PdfPHeaderCell());
                            cell.Phrase = new Phrase(new Chunk(col.ColumnName, font10));
                            table.AddCell(cell);
                        }

                        table.CompleteRow();

                        //How add the data from datatable to pdf table
                        for (int rows = 0; rows < dt.Rows.Count; rows++)
                        {
                            for (int column = 0; column < dt.Columns.Count; column++)
                            {
                                cell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font10)));
                                table.AddCell(cell);
                            }
                        }
                        table.SpacingBefore = 10f; // Give some space after the text or it may overlap the table
                        doc.Add(table); // add pdf table to the document
                    }
                    doc.AddLanguage("english");

                    //Close document and writer
                    doc.Close();
                }
            }
            return AddHeader(pdfFilePath, 1, title, startingPosition);
        }

        private string AddHeader(string file, int page, string title, int startingPosition)
        {
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{0}.pdf", Guid.NewGuid().ToBase64String()));
            //Read in the template PDF file
            using (PdfReader pdfReader = new PdfReader(file))
            {
                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create)))
                {
                    float linePosition = 755;
                    PdfContentByte pdfContent = pdfStamper.GetOverContent(page);
                    ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(title), startingPosition, linePosition, 0);
                }
            }
            Repeater.TryRepeatedly(() => File.Delete(file));
            return newFile;
        }

        private void InsertAddressInfo(AddressInfo addressLine, PdfStamper pdfStamper, PdfContentByte pdfContent)
        {
            //This is the starting Height
            int linePosition = 640;
            foreach (PropertyInfo lineData in addressLine.GetType().GetProperties())
            {
                if (lineData.GetValue(addressLine) != null && !lineData.GetValue(addressLine).ToString().IsNullOrEmpty())
                {
                    if (lineData.PropertyType == typeof(bool))
                        continue;
                    ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(lineData.GetValue(addressLine).ToString()), 71, linePosition, 0);//71 is the starting width
                    linePosition -= 13;//Move down a line
                }
            }

            linePosition = 630;//Reset for the account number and date
            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString("MMM dd, yyyy")), 420, linePosition, 0);
            linePosition -= 13;//Move down a line
            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(LetterData.LetterAccountNumber), 420, linePosition, 0);
            BaseFont customfont = BaseFont.CreateFont(EnterpriseFileSystem.GetPath("Barcode Font") + "\\GARA.TTF", BaseFont.CP1252, BaseFont.EMBEDDED);
            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(LetterData.Hours1, new Font(customfont, 9)), 94, 38, 0);//Hours of operation line 1
            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(LetterData.Hours2, new Font(customfont, 9)), 94, 29, 0);//Hours of operation line 2
        }

        private string GeneratePdf(AddressInfo addressLine)
        {
            string loanDetail = "";
            string financialTran = "";
            string templatePath = Path.Combine(EnterpriseFileSystem.GetPath("Correspondence"), string.Format("{0}.pdf", LetterData.RM_DSC_LTR_PRC));
            string newFile = CalculateFileName(LetterData.RM_DSC_LTR_PRC);
            //Read in the template PDF file
            using (PdfReader pdfReader = new PdfReader(templatePath))
            {
                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create)))
                {
                    PdfContentByte pdfContent = pdfStamper.GetOverContent(1);
                    loanDetail = CreateLoanDetail(LoanInfo, "LOAN INFORMATION", 240);
                    financialTran = CreateLoanDetail(FinancialTrans, "FINANCIAL TRANSACTIONS FOR LOAN SEQUENCES", 160);
                    InsertAddressInfo(addressLine, pdfStamper, pdfContent);

                    pdfStamper.Close();
                }
            }

            List<string> mergeFiles = new List<string>() { newFile, loanDetail, financialTran };
            string returnFile = PdfHelper.MergePdfs(mergeFiles, LetterData.RM_DSC_LTR_PRC, "", true, DataAccessHelper.CurrentRegion);

            foreach (string file in mergeFiles)
                Repeater.TryRepeatedly(() => File.Delete(file));

            return returnFile;
        }
    }
}
