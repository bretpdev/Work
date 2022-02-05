using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace BILLINGFED
{
    public class GeneratePdfs
    {
        private iTextSharp.text.Font StateMailBarcodeFont { get; set; }
        private iTextSharp.text.Font ReturnMailBarcodeFont { get; set; }
        private iTextSharp.text.Font Arial { get; set; }
        private iTextSharp.text.Font ArialLoan { get; set; }
        private iTextSharp.text.Font ScanLine { get; set; }

        public GeneratePdfs()
        {
            BaseFont customfont = BaseFont.CreateFont($"{EnterpriseFileSystem.GetPath("Barcode Font")}\\IDAutomationDMatrix.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            BaseFont customfontScanLine = BaseFont.CreateFont($"{EnterpriseFileSystem.GetPath("Barcode Font")}\\OCRAEXT.TTF", BaseFont.CP1252, BaseFont.EMBEDDED);
            StateMailBarcodeFont = new iTextSharp.text.Font(customfont, 8);
            ReturnMailBarcodeFont = new iTextSharp.text.Font(customfont, 4);
            ScanLine = new iTextSharp.text.Font(customfontScanLine, 12);
        }

        private PdfContentByte AddBarcodesToFirstPage(List<string> returnMailBarcodes, PdfStamper pdfStamper)
        {
            PdfContentByte pdfContent = pdfStamper.GetOverContent(1);//Adding state mail barcode to the first page.

            float returnMailYCoord = 95f;
            for (int mailIndex = 0; mailIndex < 6; mailIndex++)
            {
                ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(returnMailBarcodes[mailIndex], ReturnMailBarcodeFont), 220, returnMailYCoord, 0);
                returnMailYCoord -= 4.05f;
            }
            return pdfContent;
        }

        private void Generate(Bill data, string fileName, int reportNumber)
        {
            string billFile = Path.Combine(EnterpriseFileSystem.TempFolder, $"{Guid.NewGuid().ToBase64String()}Bill.pdf");
            using (PdfReader pdfReader = new PdfReader(Path.Combine(EnterpriseFileSystem.GetPath("Correspondence"), "BILSTFEDprint.pdf")))
            {
                int numberOfPages = pdfReader.NumberOfPages;
                int duplexPages = numberOfPages / 2;
                if (duplexPages % 2 == 0)
                    duplexPages++;
                duplexPages++;
                List<string> returnMailBarcodes = DocumentProcessing.GetReturnMailBarcodes(data.CoBorrowerAccountNumber, "BILSTFED", DocumentProcessing.LetterRecipient.Borrower);

                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FStream(billFile, FileMode.Create)))
                {
                    PdfContentByte pdfContent = AddBarcodesToFirstPage(returnMailBarcodes, pdfStamper);
                    AddBillLevelData(data, pdfContent);
                    AddSpecialMessages(data, reportNumber, pdfContent);
                }
            }
            AddLoanDetail(data, fileName, billFile, data.CoBorrowerAccountNumber);
        }

        private void AddLoanDetail(Bill data, string fileName, string billFile, string accountNumber)
        {
            DataTable loanDetail = GetLoanDetail(data.LineData);
            string loanPage = AddTable(loanDetail, data.AccountNumber);
            string finalFile = Path.Combine(EnterpriseFileSystem.TempFolder, $"{Guid.NewGuid().ToBase64String()}.pdf");
            new GeneratePdfs().MergePdfs(new List<string>() { billFile, loanPage }, finalFile);
            AddBarcodesToDocument(accountNumber, fileName, finalFile);
            Repeater.TryRepeatedly(() => FS.Delete(billFile));
            Repeater.TryRepeatedly(() => FS.Delete(loanPage));
            Repeater.TryRepeatedly(() => FS.Delete(finalFile));
        }

        private string MergePdfs(List<string> files, string saveAs)
        {
            string outputFile = saveAs;
            List<PdfReader> readers = new List<PdfReader>();
            using (FStream outputPdfStream = new FStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (Document document = new Document(PageSize.A4))
                {
                    using (PdfCopy copy = new PdfCopy(document, outputPdfStream))
                    {
                        copy.CompressionLevel = 9;
                        copy.SetFullCompression();
                        document.Open();

                        foreach (string file in files)
                        {
                            PdfReader r = new PdfReader(file);
                            readers.Add(r);
                            copy.AddDocument(r);
                        }

                        if (copy.PageNumber % 2 == 0)//add a blank document to the end if needed.
                            copy.AddPage(PageSize.A4, 0);
                    }
                    document.Close();
                    document.Dispose();
                }
                outputPdfStream.Dispose();
            }

            foreach (PdfReader r in readers)
                r.Dispose();

            return outputFile;
        }

        private void AddSpecialMessages(Bill data, int reportNumber, PdfContentByte pdfContent)
        {
            BillText bill = BillingStatementsFed.BillTextValues[reportNumber];
            string secondSpecialMessage = BillText.GetSecondMessageSpecialText(data.LineData, bill, reportNumber);
            float message1YCoord = bill.Message1YCoord;
            float message2YCoord = bill.Message2YCoord;
            string firstSpecialMessage = bill.FirstSpecialMessage;
            if (bill.FirstSpecialMessage.Contains("{0}"))
                firstSpecialMessage = string.Format(bill.FirstSpecialMessage, data.DaysPastDue);
            List<string> firstMessage = firstSpecialMessage.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> secondMessage = secondSpecialMessage.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            FontData firstMessageFont = FontData.GetFontFromId(bill.Message1FontTypeId);
            iTextSharp.text.Font font1 = new iTextSharp.text.Font((Font.FontFamily)firstMessageFont.EnumValue, firstMessageFont.FontSize, (firstMessageFont.IsBold ? 1 : 0));
            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(bill.FirstSpecialMessageTitle, font1), bill.Message1XCoord, message1YCoord + firstMessageFont.FontSize, 0);
            foreach (string line in firstMessage)
            {
                ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(line, font1), bill.Message1XCoord, message1YCoord, 0);
                message1YCoord -= firstMessageFont.FontSize;
            }

            FontData secondMessageFont = FontData.GetFontFromId(bill.Message1FontTypeId);
            iTextSharp.text.Font font2 = new iTextSharp.text.Font((Font.FontFamily)secondMessageFont.EnumValue, secondMessageFont.FontSize, (secondMessageFont.IsBold ? 1 : 0));
            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(bill.SecondSpecialMessageTitle, font2), bill.Message2XCoord, message2YCoord + secondMessageFont.FontSize, 0);
            foreach (string line in secondMessage)
            {
                ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(line, font2), bill.Message2XCoord, message2YCoord, 0);
                message2YCoord -= secondMessageFont.FontSize;
            }
        }

        private void AddBillLevelData(Bill data, PdfContentByte pdfContent)
        {
            foreach (BillData item in data.BillLevelData)
            {
                List<string> values = item.Value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                foreach (string value in values)
                {
                    iTextSharp.text.Font font = new iTextSharp.text.Font((Font.FontFamily)item.EnumValue, item.FontSize, (item.IsBold ? 1 : 0));
                    if (font.Family.ToString() == "SCANLINE")
                        font = ScanLine;
                    ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(value, font), item.XCoord, item.YCoord, 0);
                    item.YCoord -= item.FontSize;
                }
            }
        }

        private string RotateDocument(string file)
        {
            PdfReader reader = new PdfReader(file);
            int n = reader.NumberOfPages;
            PdfDictionary page;
            PdfNumber rotate;
            for (int p = 1; p <= n; p++)
            {
                page = reader.GetPageN(p);
                rotate = page.GetAsNumber(PdfName.ROTATE);
                if (rotate == null)
                    page.Put(PdfName.ROTATE, new PdfNumber(90));
                else
                    page.Put(PdfName.ROTATE, new PdfNumber((rotate.IntValue + 270) % 360));
            }

            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, $"{Guid.NewGuid().ToBase64String()}.pdf");
            PdfStamper stamper = new PdfStamper(reader, new FStream(newFile, FileMode.Create));
            stamper.Close();
            reader.Close();
            return newFile;
        }

        private string AddHeader(string file, string title, int startingPosition, string accountNumber)
        {
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, $"{Guid.NewGuid().ToBase64String()}.pdf");
            //Read in the template PDF file
            AddTitle(file, title, newFile);

            string rotatedFile = RotateDocument(newFile);

            Repeater.TryRepeatedly(() => FS.Delete(file));
            Repeater.TryRepeatedly(() => FS.Delete(newFile));
            return rotatedFile;
        }

        private void AddBarcodesToDocument(string accountNumber, string finalFile, string rotatedFile)
        {
            using (PdfReader pdfReader = new PdfReader(rotatedFile))
            {
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FStream(finalFile, FileMode.Create)))
                {
                    int numberOfPages = pdfReader.NumberOfPages;
                    int duplexPages = numberOfPages / 2;
                    if (duplexPages % 2 == 0)
                        duplexPages++;

                    duplexPages++;
                    PdfContentByte pdfContent = pdfStamper.GetOverContent(1);
                    List<string> stateMailBarcodes = DocumentProcessing.GetStateMailBarcodesforPdf(accountNumber, (pdfReader.NumberOfPages / 2), DocumentProcessing.LetterRecipient.Borrower);

                    int index = 0;
                    for (int page = 1; page <= numberOfPages; page += 2)
                    {
                        float linePosition = 755;
                        pdfContent = pdfStamper.GetOverContent(page);
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 15, linePosition, 0);
                        linePosition -= (float)8.05;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 15, linePosition, 0);
                        linePosition -= (float)8.05;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 15, linePosition, 0);
                        linePosition -= (float)8.05;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), 15, linePosition, 0);
                        linePosition -= (float)8.05;
                    }
                }
            }
        }

        private void AddTitle(string file, string title, string newFile)
        {
            using (PdfReader pdfReader = new PdfReader(file))
            {
                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FStream(newFile, FileMode.Create)))
                {
                    PdfContentByte pdfContent = pdfStamper.GetOverContent(1);
                    ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(title, new iTextSharp.text.Font(Font.FontFamily.TIMES_ROMAN, 14, 1)), 350, 575, 0);
                }
            }
        }

        /// <summary>
        /// Add a loan detail table to a page.
        /// </summary>
        /// <param name="dt">Data table with loan detail info</param>
        /// <returns>Path of the new PDf created</returns>
        private string AddTable(DataTable dt, string accountNumber)
        {
            string pdfFilePath = Path.Combine(EnterpriseFileSystem.TempFolder, Guid.NewGuid() + ".pdf");
            var letterOrientation = dt.Columns.Count > 4 ? iTextSharp.text.PageSize.LETTER.Rotate() : iTextSharp.text.PageSize.LETTER;
            using (Document doc = new Document(letterOrientation, 0, 0, 42, 35))
            {
                //Create Document class object and set its size to letter and give space left, right, Top, Bottom Margin
                using (PdfWriter wri = PdfWriter.GetInstance(doc, new FStream(pdfFilePath, FileMode.Create)))
                {
                    doc.Open();//Open Document to write
                    var font10 = new iTextSharp.text.Font(Font.FontFamily.TIMES_ROMAN, 8);
                    var font10b = new iTextSharp.text.Font(Font.FontFamily.TIMES_ROMAN, 8, 1);

                    if (dt != null)
                    {
                        //Create instance of the pdf table and set the number of column in that table
                        PdfPTable table = new PdfPTable(dt.Columns.Count);
                        PdfPCell cell = null;
                        table.HeaderRows = 1;
                        table.Summary = "Loan Information";
                        //Add Header of the pdf table
                        foreach (DataColumn col in dt.Columns)
                        {
                            cell = new PdfPCell();
                            cell.AddHeader(new PdfPHeaderCell());
                            cell.HorizontalAlignment = 1; //center
                            cell.VerticalAlignment = 2;//center
                            cell.BackgroundColor = new BaseColor(211, 211, 211);
                            cell.Phrase = new Phrase(new Chunk(col.ColumnName, font10b));
                            table.AddCell(cell);
                        }
                        table.CompleteRow();

                        //How add the data from datatable to pdf table
                        for (int rows = 0; rows < dt.Rows.Count; rows++)
                        {
                            for (int column = 0; column < dt.Columns.Count; column++)
                            {
                                cell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font10)));
                                decimal val = 0;
                                if (decimal.TryParse(dt.Rows[rows][column].ToString().Replace("$", ""), out val))
                                    cell.HorizontalAlignment = 2;//Align right
                                else
                                    cell.HorizontalAlignment = 1;

                                cell.VerticalAlignment = 2;//center
                                table.AddCell(cell);
                            }
                        }
                        table.SpacingBefore = 15f; // Give some space after the text or it may overlap the table
                        doc.Add(table); // add pdf table to the document
                    }

                    //Close document and writer
                    doc.Close();
                }
            }


            string rotatedFile = AddHeader(pdfFilePath, "Loan Information", 350, accountNumber);

            return rotatedFile;
        }

        /// <summary>
        /// Generates the bill using iTextSharp code Reports
        /// </summary>
        /// <returns>True if document was created</returns>
        public void GeneratePrintDocument(PrintData pData, DataAccess da)
        {
            var imagingFile = $"{pData.Directory}\\{pData.ScriptId}_{pData.Bor.AccountNumber}_{pData.Bor.PrintProcessingId}.pdf";
            pData.Bor.SetImagingFile(imagingFile);
            Dictionary<string, int> fileHeader = da.GetHeaderData();
            Bill data = new Bill(pData.Bor.AccountNumber, pData.Bor.LineData, da, pData.Bor.CoBorrowerAccountNumber);
            data.LoadBillData(fileHeader, pData.Bor.LineData.First());
            Generate(data, pData.Bor.GetImagingFile(da), pData.ReportNumber);
        }

        /// <summary>
        /// Images the file
        /// </summary>
        public void ImageFile(PrintData pData, DataAccess da, ProcessLogRun logRun)
        {
            try
            {
                if (!pData.Bor.ImagedAt.HasValue)
                {
                    //ImagingFile is lazy loaded if the file does not exist it will create it for you.
                    DocumentProcessing.ImageFile(pData.Bor.GetImagingFile(da), BillingStatementsFed.DocId, pData.Bor.Ssn);
                    pData.Bor.SetImagedAt();
                    if (pData.Bor.OnEcorr || pData.ReportNumber.IsIn(20, 21))
                        Repeater.TryRepeatedly(() => FS.Delete(pData.Bor.GetImagingFile(da)));
                }
            }
            catch (Exception ex)
            {
                string message = $"Error imaging bill for account: {pData.Bor.AccountNumber}; Print Processing Id: {pData.Bor.PrintProcessingId}";
                logRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
        }

        /// <summary>
        /// Creates a Ecorr bill
        /// </summary>
        /// <param name="bor">Borrower object</param>
        /// <param name="UserId">User Id of the person running the application</param>
        /// <param name="ecorrInfo">EcorrData object</param>
        /// <param name="reportNumber">The SAS job being processed</param>
        /// <param name="isEcorr">Boolean showing is borrower is on Ecorr or not</param>
        /// <param name="retryCount">Starts at 0 and increments by 1 every time there is an error</param>
        /// <returns>True is success Ecorr document created</returns>
        public void GenerateEcorr(Borrower bor, string UserId, EcorrData ecorrInfo, int reportNumber, DataAccess da, ProcessLogRun logRun, int retryCount = 0)
        {
            if (!bor.EcorrDocumentCreatedAt.HasValue)
            {
                try
                {
                    if (!CheckForSsn(bor, da, logRun))
                        return;
                    List<string> fields = bor.LineData.First().SplitAndRemoveQuotes(",");
                    DateTime dueDate = fields[24].ToDate();
                    string totalDue = (fields[26].ToDecimal()).ToString();
                    string billSeq = fields[31];
                    DateTime billCreateDate = fields[19].ToDate();
                    if (bor.IsEndorser)
                    {
                        bor.Ssn = da.GetSsnFromAcctNum(bor.LineData[0].SplitAndRemoveQuotes(",")[2]);
                        bor.AccountNumber = bor.LineData[0].SplitAndRemoveQuotes(",")[2];
                    }

                    DataTable loanDetail = GetLoanDetail(bor.LineData);
                    Dictionary<string, string> formFields = GetFormFields(bor.LineData, reportNumber);
                    PdfHelper.GenerateEcorrBill(Path.Combine(EnterpriseFileSystem.GetPath("Correspondence"), "BILSTFED.pdf"), bor.AccountNumber, bor.Ssn, bor.OnEcorr ? DocumentProperties.CorrMethod.EmailNotify : DocumentProperties.CorrMethod.Printed,
                        UserId, ecorrInfo, loanDetail, formFields, dueDate, totalDue, billSeq, billCreateDate, bor.CoBorrowerAccountNumber);
                }
                catch (OutOfMemoryException ex)
                {
                    if (retryCount == 5)
                        throw;

                    Thread.Sleep(60000);
                    Console.WriteLine($"{ex.InnerException.Message}; recursing; {bor.AccountNumber}");
                    GenerateEcorr(bor, UserId, ecorrInfo, reportNumber, da, logRun, ++retryCount);
                }
                bor.SetEcorrDocumentCreated();
            }
        }

        /// <summary>
        /// Checks to see if the Borrower SSN is provided for the Ecorr process.
        /// </summary>
        /// <param name="bor">Borrower object</param>
        /// <param name="da">DataAccess object</param>
        /// <param name="logData">ProcessLogData object</param>
        /// <returns>True if the SSN was found and provided; False if the SSN was not found.</returns>k
        private bool CheckForSsn(Borrower bor, DataAccess da, ProcessLogRun logRun)
        {
            if (bor.Ssn.IsNullOrEmpty())
                bor.Ssn = da.GetSsnFromAcctNum(bor.AccountNumber);
            if (bor.Ssn.IsNullOrEmpty())
            {
                string message = $"Error retrieving SSN from PD10 in CDW for borrower: {bor.AccountNumber}; PringProcessing Id: {bor.PrintProcessingId}; Unable to generate Ecorr document and insert Ecorr data into the EcorrFed.DocumentDetails table.";
                logRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Console.WriteLine(message);
                return false;
            }
            return true;
        }

        private Dictionary<string, string> GetFormFields(List<string> dataLines, int reportNumber)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>();
            List<string> oneLine = dataLines.First().SplitAndRemoveQuotes(",");

            formFields.Add("NAME", $"{oneLine[3]} {oneLine[5]}");//Borrowers Name
            formFields.Add("DATE", DateTime.Now.Date.ToString().ToLongDateNoDOW());
            formFields.Add("ACCOUNTNUMBER", oneLine[2]);//Borrowers Account Number
            formFields.Add("BILLDATE", oneLine[19]);
            formFields.Add("DATEDUE", oneLine[24]);
            formFields.Add("DATERECEIVED", oneLine[20]);
            formFields.Add("AMOUNTTOPRINCIPAL", oneLine[37]);
            formFields.Add("AMOUNTTOINTEREST", oneLine[38]);
            formFields.Add("AMOUNTPAID", oneLine[39]);
            formFields.Add("AMOUNTPASTDUE", oneLine[25]);
            formFields.Add("MONTHLYINSTALL", oneLine[52]);
            formFields.Add("TOTALAMOUNTDUE", oneLine[26]);
            formFields.Add("DUEDATE", oneLine[24]);
            formFields.Add("ACCOUNTNUMBER1", (oneLine.Count > 27 && oneLine[27].IsPopulated()) ? oneLine[27] : oneLine[2]);
            formFields.Add("AMOUNTPASTDUE1", oneLine[25]);
            formFields.Add("INSTALLMENTAMOUNT", oneLine[35]);
            formFields.Add("TOTALAMOUNTDUE1", oneLine[26]);
            formFields.Add("DUEBY", oneLine[24]);
            formFields.Add("AMOUNTTOFEES", oneLine[49]);
            BillText bill = BillingStatementsFed.BillTextValues[reportNumber];
            formFields.Add("SPECIALMESSAGETITLE", bill.FirstSpecialMessageTitle);
            formFields.Add("SPECIALMESSAGE", bill.FirstSpecialMessage.IsPopulated() ? string.Format(bill.FirstSpecialMessage.Replace("\r\n", " "), oneLine[48]) : "");
            formFields.Add("SPECIALMESSAGETITLE2", bill.SecondSpecialMessageTitle);
            formFields.Add("SPECIALMESSAGE2", BillText.GetSecondMessageText(BillText.GetSecondMessageSpecialText(dataLines, bill, reportNumber).Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList()));
            formFields.Add("TOTALPRINCIPALPAID", oneLine[40]);
            formFields.Add("TOTALINTERESTPAID", oneLine[41]);
            formFields.Add("TOTALAMOUNTPAID", oneLine[42]);
            formFields.Add("WA_TOT_BRI_OTS", oneLine[36]);
            formFields.Add("AGG_TOT_PRN_BAL", oneLine[43]);

            return formFields;
        }

        private DataTable GetLoanDetail(List<string> dataLines)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Loan Program"),
                new DataColumn("First Disbursed"),
                new DataColumn("Interest Rate"),
                new DataColumn("Original Principal"),
                new DataColumn("Current Balance"),
                new DataColumn("Total Principal Paid"),
                new DataColumn("Total Interest Paid"),
                new DataColumn("Total Amount Paid"),
                new DataColumn("Months Remaining")
            });
            foreach (string line in dataLines)
            {
                List<string> fields = line.SplitAndRemoveQuotes(",");
                string[] dataRow = new string[]
                {
                    fields[12], //Loan Program
                    fields[14].ToDate().ToString("MM/dd/yyyy"), //First Disbursed
                    fields[15], //Interest rate
                    fields[16], //Original Principal
                    fields[17], //Current Balance
                    fields[32], //Total principal paid
                    fields[33], //Total interest paid
                    fields[34], //Total amount paid
                    fields[50] //Months Remaining
                };

                dt.Rows.Add(dataRow);
            }
            return dt;
        }
    }
}