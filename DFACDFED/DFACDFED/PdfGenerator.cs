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

namespace DFACDFED
{
    class PdfGenerator
    {
        private BorrowerInfo BorrowerInfo { get; set; }
        private iTextSharp.text.Font StateMailBarcodeFont { get; set; }
        private iTextSharp.text.Font ReturnMailBarcodeFont { get; set; }

        DataAccess da;
        public PdfGenerator(BorrowerInfo info, DataAccess da)
        {
            this.BorrowerInfo = info;
            this.da = da;
            BaseFont customfont = BaseFont.CreateFont(@"C:\Windows\Fonts\" + "IDAutomationDMatrix.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            StateMailBarcodeFont = new iTextSharp.text.Font(customfont, 8);
            ReturnMailBarcodeFont = new iTextSharp.text.Font(customfont, 4);
        }

        public enum PdfResult
        {
            None,
            Ecorr,
            Printed
        }

        /// <summary>
        /// Creates an ecorr or printed PDF
        /// </summary>
        public PdfResult CreatePdf()
        {

            AddressInfo addressLine = da.GetAddressInfo(BorrowerInfo.Letter.AccountNumber);
            if(addressLine == null)
            {
                da.InactivateSystemLetter(BorrowerInfo.Letter.LetterSequence, BorrowerInfo.Letter.LetterId, BorrowerInfo.Letter.AccountNumber);
                return PdfResult.None;
            }
            EcorrData ecorrInfo = EcorrProcessing.CheckEcorr(BorrowerInfo.Letter.AccountNumber);
            string ecorrFile = "";
            string cpFile = "";
            DocumentProperties prop = null;
            bool emailed = false;
            if ((ecorrInfo != null && ecorrInfo.LetterIndicator && ecorrInfo.ValidEmail))
            {
                emailed = true;
                ecorrFile = GeneratePdf(addressLine, true);
                string path = EnterpriseFileSystem.GetPath("ECORRDocuments") + Path.GetFileName(ecorrFile);
                prop = new DocumentProperties(BorrowerInfo.Letter.Ssn, BorrowerInfo.Letter.AccountNumber, BorrowerInfo.Letter.LetterId, "UT00801", ecorrInfo.EmailAddress, DocumentProperties.CorrMethod.EmailNotify, path, ecorrInfo.Format);
                prop.InsertEcorrInformation();
                File.Move(ecorrFile, Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), Path.GetFileName(ecorrFile)));
            }
            else if (ecorrInfo != null)
            {
                ecorrFile = GeneratePdf(addressLine, true);
                if (addressLine.HasValidAddress)
                    cpFile = GeneratePdf(addressLine, false);


                string path = EnterpriseFileSystem.GetPath("ECORRDocuments") + Path.GetFileName(ecorrFile);
                prop = new DocumentProperties(BorrowerInfo.Letter.Ssn, BorrowerInfo.Letter.AccountNumber, BorrowerInfo.Letter.LetterId, "UT00801", ecorrInfo.EmailAddress, DocumentProperties.CorrMethod.Printed, path, ecorrInfo.Format);
                prop.InsertEcorrInformation();
                File.Move(ecorrFile, Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), Path.GetFileName(ecorrFile)));

                if (!addressLine.HasValidAddress)
                {
                    da.InactivateSystemLetter(BorrowerInfo.Letter.LetterSequence, BorrowerInfo.Letter.LetterId, BorrowerInfo.Letter.AccountNumber);
                    return PdfResult.None;
                }
            }
            else
            {
                if (addressLine.HasValidAddress)
                    cpFile = GeneratePdf(addressLine, false);

                if (!addressLine.HasValidAddress)
                {
                    da.InactivateSystemLetter(BorrowerInfo.Letter.LetterSequence, BorrowerInfo.Letter.LetterId, BorrowerInfo.Letter.AccountNumber);
                    return PdfResult.None;
                }
            }


            if (!cpFile.IsNullOrEmpty())
            {
                AddBarcodesToPdfAndPrint(cpFile, BorrowerInfo.Letter.LetterId, !addressLine.Country.IsNullOrEmpty());
                Repeater.TryRepeatedly(() => File.Delete(cpFile));
            }

            return emailed ? PdfResult.Ecorr : PdfResult.Printed;
        }

        private void AddBarcodesToPdfAndPrint(string file, string letterId, bool isForeign)
        {
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{0}DataTable.pdf", Guid.NewGuid().ToBase64String()));
            //Read in the template PDF file
            using (PdfReader pdfReader = new PdfReader(file))
            {
                int numberOfPages = pdfReader.NumberOfPages;
                int duplexPages = numberOfPages / 2;

                duplexPages++;

                List<string> returnMailBarcodes = DocumentProcessing.GetReturnMailBarcodes(BorrowerInfo.Letter.AccountNumber, BorrowerInfo.Letter.LetterId, DocumentProcessing.LetterRecipient.Borrower);
                List<string> stateMailBarcodes = DocumentProcessing.GetStateMailBarcodesforPdf(BorrowerInfo.Letter.AccountNumber, duplexPages, DocumentProcessing.LetterRecipient.Borrower);

                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create)))
                {
                    int index = 0;
                    for (int page = 1; page <= numberOfPages; page += 2)
                    {
                        float linePosition = 755;
                        PdfContentByte pdfContent = pdfStamper.GetOverContent(page);
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 15, linePosition, 0);
                        linePosition -= (float)8.05;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 15, linePosition, 0);
                        linePosition -= (float)8.05;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 15, linePosition, 0);
                        linePosition -= (float)8.05;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 15, linePosition, 0);
                        linePosition -= (float)8.05;
                        if (page == 1)
                        {
                            float statemailPosition = 660;
                            for (int mailIndex = 0; mailIndex < 6; mailIndex++)
                            {
                                ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(returnMailBarcodes[mailIndex], ReturnMailBarcodeFont), 280, statemailPosition, 0);
                                statemailPosition -= (float)4.05;
                            }
                        }
                    }
                }
                int pageCount = numberOfPages / 2;
                if (pageCount % 2 > 0)
                    pageCount++;

                DocumentProcessing.PrintStateMailCoverSheet(pageCount, isForeign, BorrowerInfo.Letter.LetterId);
                DocumentProcessing.PrintPdf(newFile);

                Repeater.TryRepeatedly(() => File.Delete(newFile));
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
            string pdfFilePath = Path.Combine(EnterpriseFileSystem.TempFolder, Guid.NewGuid() + "DatasTable.pdf");
            using (Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35))
            {
                //Create Document class object and set its size to letter and give space left, right, Top, Bottom Margin
                using (PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create)))
                {
                    wri.SetTagged();
                    doc.Open();//Open Document to write

                    var font10 = FontFactory.GetFont("ARIAL", 8);

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
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{0}DataTable.pdf", Guid.NewGuid().ToBase64String()));
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
            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(BorrowerInfo.Letter.AccountNumber), 420, linePosition, 0);
        }

        private string GeneratePdf(AddressInfo addressLine, bool isEcorr)
        {
            string loanDetail = "";
            string templatePath = Path.Combine(EnterpriseFileSystem.GetPath("Correspondence"), string.Format("{0}{1}.pdf", "DFACDFED", isEcorr ? "Ecorr" : ""));
            string newFile = CalculateFileName(BorrowerInfo.Letter.LetterId);
            //Read in the template PDF file
            using (PdfReader pdfReader = new PdfReader(templatePath))
            {
                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create)))
                {
                    PdfContentByte pdfContent = pdfStamper.GetOverContent(1);
                    loanDetail = CreateLoanDetail(da.GenerateTable(BorrowerInfo.Loans), "LOAN INFORMATION", 240);
                    InsertAddressInfo(addressLine, pdfStamper, pdfContent);

                    pdfStamper.Close();
                }
            }

            List<string> mergeFiles = new List<string>() { newFile, loanDetail };
            string returnFile = PdfHelper.MergePdfs(mergeFiles, BorrowerInfo.Letter.LetterId, "", true, CalculateFileName(BorrowerInfo.Letter.LetterId));

            foreach (string file in mergeFiles)
                Repeater.TryRepeatedly(() => File.Delete(file));

            return returnFile;
        }
    }

}
