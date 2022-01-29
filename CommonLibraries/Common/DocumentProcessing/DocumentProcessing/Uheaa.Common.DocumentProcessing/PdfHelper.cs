using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using Uheaa.Common.DataAccess;
using iTextSharp.text.xml.xmp;

namespace Uheaa.Common.DocumentProcessing
{
    public static class PdfHelper
    {
        private static iTextSharp.text.Font StateMailBarcodeFont { get; set; }

        static PdfHelper()
        {
            BaseFont customfont = BaseFont.CreateFont($"{EnterpriseFileSystem.GetPath("Barcode Font")}\\IDAutomationDMatrix.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            StateMailBarcodeFont = new iTextSharp.text.Font(customfont, 8);
        }
        private static string CalculateFileName(string letterId, string path = null)
        {
            if (path == null)
                path = EnterpriseFileSystem.TempFolder;

            return string.Format($"{path}{letterId}{Guid.NewGuid()}.pdf");
        }

        public static int GetNumberOfPagesInPdf(string pdfFile)
        {
            using (PdfReader reader = new PdfReader(pdfFile))
            {
                return reader.NumberOfPages;
            }
        }

        public static string MergePdfsBilling(List<string> files, string letterId, string accountNumber, bool tagPdf, DataAccessHelper.Region? region = null, string saveAs = null)
        {
            string outputFile = saveAs == null ? CalculateFileName(letterId, Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation", region)))
                : saveAs;
            List<PdfReader> readers = new List<PdfReader>();
            using (FileStream outputPdfStream = new FStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (Document document = new Document(PageSize.A4))
                {
                    using (PdfCopy copy = new PdfCopy(document, outputPdfStream))
                    {
                        if (tagPdf)
                        {
                            copy.SetTagged();
                            copy.SetLanguage("English");
                            copy.SetFullCompression();
                        }
                        document.Open();

                        foreach (string file in files)
                        {
                            PdfReader r = new PdfReader(file);
                            readers.Add(r);
                            copy.AddDocument(r);
                        }
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

        public static string GenerateEcorrBill(string templatePath, string accountNumber, string borrowerSsn, DocumentProperties.CorrMethod ecorrMethod, string userId, EcorrData data, DataTable loanDetail, Dictionary<string, string> formFields, DateTime dueDate, string totalDue, string billSeq, DateTime billCreateDate, string coBorrowerAccountNumber)
        {
            string letterId = Path.GetFileNameWithoutExtension(templatePath);
            List<string> filesToMerge = new List<string>();
            string localFile = CalculateFileName(letterId);
            FS.Copy(templatePath, localFile);
            filesToMerge.Add(WriteFormValues(formFields, localFile, letterId, null));
            filesToMerge.Add(AddTable(loanDetail));

            string finalFile = MergePdfs(filesToMerge, letterId, accountNumber, true);

            foreach (string file in filesToMerge)
                FS.Delete(file);

            if (accountNumber == coBorrowerAccountNumber)
                DocumentProperties.GenerateEcorrBillWithPDF(borrowerSsn, accountNumber, letterId, userId, data.EmailAddress, ecorrMethod, finalFile, dueDate, totalDue, billSeq, billCreateDate, data.Format);
            else
                DocumentProperties.GenerateEcorrBillWithPDF(borrowerSsn, accountNumber, letterId, userId, data.EmailAddress, ecorrMethod, finalFile, dueDate, totalDue, billSeq, billCreateDate, data.Format, coBorrowerAccountNumber);

            FS.Delete(localFile);
            return finalFile;
        }

        /// <summary>
        /// Generates a PDF for Ecorr.
        /// </summary>
        /// <param name="templatePath">Template path for the PDF</param>
        /// <param name="accountNumber">Borrowers or Endorsers Account Number</param>
        /// <param name="borrowerSsn">Borrowers Ssn (Even if creating an endorsers letter)</param>
        /// <param name="ecorrMethod">Ecorr Method P for paper E for Electronic</param>
        /// <param name="userId">AES User Id</param>
        /// <param name="data">Ecorr Data Object</param>
        /// <param name="address">List of address strings</param>
        /// <param name="loanDetail">Loan Detail if the borrower has one</param>
        /// <param name="formFields">Form fields</param>
        /// <returns>Path of the generated document.</returns>
        public static string GenerateEcorrPdf(string templatePath, string accountNumber, string borrowerSsn, DocumentProperties.CorrMethod ecorrMethod, string userId, EcorrData data, List<string> address, DataTable loanDetail = null, Dictionary<string, string> formFields = null, List<MutliLineMergeField> multiLineFields = null, DataAccessHelper.Region? region = null, string coBorrowerAccountNumber = null)
        {
            var currentRegion = region ?? DataAccessHelper.CurrentRegion;
            string letterId = Path.GetFileNameWithoutExtension(templatePath);
            List<string> filesToMerge = new List<string>();
            string localFile = CalculateFileName(letterId);
            FS.Copy(templatePath, localFile);

            bool hasFormFields = formFields != null || multiLineFields != null;

            if (hasFormFields)
                localFile = WriteFormValues(formFields, localFile, letterId, multiLineFields);

            filesToMerge.Add(InsertAddressInformation(address, accountNumber, localFile, letterId, hasFormFields, region));

            if (loanDetail != null)
                filesToMerge.Add(AddTable(loanDetail));

            Forms form = DocumentProcessing.HasForm(letterId);
            if (form != null)
                filesToMerge.Add(form.PathAndForm);

            string finalFile = string.Empty;
            //if (filesToMerge.Count > 1)
            //{
            finalFile = MergePdfs(filesToMerge, letterId, accountNumber, true, currentRegion);
            string formPath = form != null ? form.PathAndForm : "";
            foreach (string file in filesToMerge.Where(p => p != formPath))
                FS.Delete(file);
            //}
            //else
            //    finalFile = filesToMerge.FirstOrDefault();

            string path = EnterpriseFileSystem.GetPath("ECORRDocuments", currentRegion) + Path.GetFileName(finalFile);
            string ssn = !borrowerSsn.IsNullOrEmpty() ? borrowerSsn : DataAccessHelper.ExecuteSingle<string>("spGetSSNFromAcctNumber", currentRegion == DataAccessHelper.Region.CornerStone ? DataAccessHelper.Database.Cdw : DataAccessHelper.Database.Udw, data.AccountNumber.ToSqlParameter("AccountNumber"));
            DocumentProperties doc = new DocumentProperties(ssn, accountNumber, letterId, userId, data.EmailAddress, ecorrMethod, path, data.Format, currentRegion);
            doc.InsertEcorrInformation();

            while (File.Exists(localFile))
                FS.Delete(localFile);

            return finalFile;

        }

        /// <summary>
        /// Generates a PDF for Ecorr.
        /// </summary>
        /// <param name="templatePath">Template path for the PDF</param>
        /// <param name="accountNumber">Borrowers or Endorsers Account Number</param>
        /// <param name="borrowerSsn">Borrowers Ssn (Even if creating an endorsers letter)</param>
        /// <param name="ecorrMethod">Ecorr Method P for paper E for Electronic</param>
        /// <param name="userId">AES User Id</param>
        /// <param name="data">Ecorr Data Object</param>
        /// <param name="address">List of address strings</param>
        /// <param name="loanDetail">Loan Detail if the borrower has one</param>
        /// <param name="formFields">Form fields</param>
        /// <returns>Path of the generated document.</returns>
        public static string GenerateEcorrPdf(string templatePath, string accountNumber, string borrowerSsn, DocumentProperties.CorrMethod ecorrMethod, string userId, EcorrData data, List<string> address, string letterAccountNumber, DataTable loanDetail = null, Dictionary<string, string> formFields = null, List<MutliLineMergeField> multiLineFields = null, DataAccessHelper.Region? region = null)
        {
            var currentRegion = region ?? DataAccessHelper.CurrentRegion;
            string letterId = Path.GetFileNameWithoutExtension(templatePath);
            List<string> filesToMerge = new List<string>();
            string localFile = CalculateFileName(letterId);
            FS.Copy(templatePath, localFile);

            bool hasFormFields = formFields != null || multiLineFields != null;

            if (hasFormFields)
                localFile = WriteFormValues(formFields, localFile, letterId, multiLineFields);

            filesToMerge.Add(InsertAddressInformation(address, letterAccountNumber, localFile, letterId, hasFormFields, region));

            if (loanDetail != null)
                filesToMerge.Add(AddTable(loanDetail));

            Forms form = DocumentProcessing.HasForm(letterId);
            if (form != null)
                filesToMerge.Add(form.PathAndForm);

            string finalFile = string.Empty;
            //if (filesToMerge.Count > 1)
            //{
            finalFile = MergePdfs(filesToMerge, letterId, accountNumber, true, currentRegion);
            string formPath = form != null ? form.PathAndForm : "";
            foreach (string file in filesToMerge.Where(p => p != formPath))
                FS.Delete(file);
            //}
            //else
            //    finalFile = filesToMerge.FirstOrDefault();

            string path = EnterpriseFileSystem.GetPath("ECORRDocuments", currentRegion) + Path.GetFileName(finalFile);
            string ssn = !borrowerSsn.IsNullOrEmpty() ? borrowerSsn : DataAccessHelper.ExecuteSingle<string>("spGetSSNFromAcctNumber", currentRegion == DataAccessHelper.Region.CornerStone ? DataAccessHelper.Database.Cdw : DataAccessHelper.Database.Udw, data.AccountNumber.ToSqlParameter("AccountNumber"));
            DocumentProperties doc = new DocumentProperties(ssn, accountNumber, letterId, userId, data.EmailAddress, ecorrMethod, path, data.Format, currentRegion);
            doc.InsertEcorrInformation();

            while (File.Exists(localFile))
                FS.Delete(localFile);

            return finalFile;

        }

        private static string AddBarcodesToPdf(string file, string letterId, bool isForeign)
        {
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{0}.pdf", Guid.NewGuid().ToBase64String()));
            //Read in the template PDF file
            using (PdfReader pdfReader = new PdfReader(file))
            {
                int numberOfPages = pdfReader.NumberOfPages;
                int duplexPages = numberOfPages / 2;
                if (duplexPages % 2 == 0)
                    duplexPages++;

                duplexPages++;

                //List<string> returnMailBarcodes = DocumentProcessing.GetReturnMailBarcodes(LetterData.AccountNumber, LetterData.LetterId, DocumentProcessing.LetterRecipient.Borrower);
                List<string> stateMailBarcodes = DocumentProcessing.GetStateMailBarcodesforPdf("", duplexPages, DocumentProcessing.LetterRecipient.Borrower);

                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FStream(newFile, FileMode.Create)))
                {
                    int index = 0;
                    for (int page = 1; page <= numberOfPages; page += 2)
                    {
                        float linePosition = page == 1 ? 755f : 600f;
                        float horizontalPosition = page == 1 ? 15f : 720f;
                        PdfContentByte pdfContent = pdfStamper.GetOverContent(page);
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), horizontalPosition, linePosition, 0);
                        linePosition -= (float)8.05;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), horizontalPosition, linePosition, 0);
                        linePosition -= (float)8.05;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), horizontalPosition, linePosition, 0);
                        linePosition -= (float)8.05;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(stateMailBarcodes[index++], StateMailBarcodeFont), horizontalPosition, linePosition, 0);
                        linePosition -= (float)8.05;
                        //if (page == 1)
                        //{
                        //    float statemailPosition = 660;
                        //    for (int mailIndex = 0; mailIndex < 6; mailIndex++)
                        //    {
                        //        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(returnMailBarcodes[mailIndex], ReturnMailBarcodeFont), 280, statemailPosition, 0);
                        //        statemailPosition -= (float)4.05;
                        //    }
                        //}
                    }
                }
                int pageCount = numberOfPages / 2;
                if (pageCount % 2 > 0)
                    pageCount++;


            }

            Repeater.TryRepeatedly(() => FS.Delete(file));

            return newFile;
        }

        public static string GenerateUS06BLCNTM(List<string> lenderAddress, List<US06BLCNTMBwrAddr> bwrs)
        {
            string templatePath = Path.Combine(EnterpriseFileSystem.GetPath("US06BLCNTM"), "US06BLCNTM.pdf");
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, Guid.NewGuid().ToBase64String() + ".pdf");
            using (PdfReader pdfReader = new PdfReader(templatePath))
            {
                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FStream(newFile, FileMode.Create)))
                {
                    PdfContentByte pdfContent = pdfStamper.GetOverContent(1);
                    //This is the starting Height
                    int linePosition = 640;
                    foreach (string line in lenderAddress)
                    {
                        if (line.IsPopulated())
                        {
                            ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(line), 71, linePosition, 0);//71 is the starting width
                            linePosition -= 13;//Move down a line
                        }
                    }
                }
            }

            List<string> files = new List<string>();
            files.Add(newFile);
            int skip = 0;
            int take = 3;
            while (skip < bwrs.Count)
            {
                files.Add(CreateUS06BLCNTMForm(bwrs.Skip(skip).Take(take).ToList()));
                skip += take;
            }

            string returnFile = PdfHelper.MergePdfs(files, "", "", false, null, Path.Combine(EnterpriseFileSystem.TempFolder, Guid.NewGuid().ToBase64String() + ".pdf"));

            foreach (string file in files)
                Repeater.TryRepeatedly(() => FS.Delete(file));

            returnFile = AddBarcodesToPdf(returnFile, "US06BLCNTM", false);

            return returnFile;
        }

        public static string CreateUS06BLCNTMForm(List<US06BLCNTMBwrAddr> dts)
        {
            string templatePath = Path.Combine(EnterpriseFileSystem.GetPath("US06BLCNTM"), "US06BLCNTM_Form.pdf");
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, Guid.NewGuid().ToBase64String() + ".pdf");
            using (PdfReader pdfReader = new PdfReader(templatePath))
            {
                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FStream(newFile, FileMode.Create)))
                {
                    PdfContentByte pdfContent = pdfStamper.GetOverContent(1);

                    //This is the starting Height
                    float last4 = 559f;
                    float address = 505f;
                    float phone = 459.5f;
                    float name = 559;

                    Font f = new Font(Font.FontFamily.HELVETICA, 9);

                    foreach (US06BLCNTMBwrAddr dt in dts)
                    {
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.Name, f), 335, name, 0);//71 is the starting width
                        name -= 11f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.PrevName, f), 335, name, 0);//71 is the starting width


                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.SsnLastFour, f), 159, last4, 0);//71 is the starting width

                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.Address1, f), 22, address, 0);//71 is the starting width
                        address -= 11f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.Address2, f), 22, address, 0);//71 is the starting width
                        address -= 11f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(string.Format("{0} {1}, {2}", dt.City, dt.State, dt.Zip), f), 22, address, 0);//71 is the starting width

                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.Phone, f), 87, phone, 0);//71 is the starting width
                        phone -= 18f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.AltPhone1, f), 72, phone, 0);//71 is the starting width
                        phone -= 18f;
                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.AltPhone2, f), 72, phone, 0);//71 is the starting width
                        phone -= 18f;

                        ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(dt.EmailAddress, f), 50, phone, 0);//71 is the starting width

                        phone -= 124;
                        address -= 155;
                        name -= 167;
                        last4 -= 178;
                    }
                }
            }

            return newFile;
        }



        /// <summary>
        /// Adds data into the form fields for the given template.
        /// </summary>
        /// <param name="formFields">Dictionary with the Key being the field name in the document, and the value being the value to go into the document</param>
        /// <param name="template">Path of the template PDF</param>
        /// <param name="letterId">Letter Tracking Id</param>
        /// <returns>Path of the new document created</returns>
        private static string WriteFormValues(Dictionary<string, string> formFields, string template, string letterId, List<MutliLineMergeField> multiLineFields)
        {
            string outFile = CalculateFileName(letterId);
            using (PdfReader pdfReader = new PdfReader(template))
            {
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FStream(outFile, FileMode.Create), PdfWriter.VERSION_1_5))
                {
                    //pdfStamper.FormFlattening = true;
                    //pdfStamper.SetFullCompression();
                    AcroFields pdfFormFields = pdfStamper.AcroFields;
                    if (formFields != null)
                        foreach (var field in pdfReader.AcroFields.Fields.Where(p => !p.Key.ToUpper().IsIn("CLOSINGPARAGRAPH", "HEADERNAME", "HEADERADDRESS", "HEADERACCOUNTNUMBER", "HEADERDATE")))
                        {
                            string[] multiFields = multiLineFields != null ? multiLineFields.Select(p => p.PdfMergeFieldName).ToArray() : null;
                            if (pdfFormFields.Fields.Where(p => p.Key == field.Key).Count() > 0 && (multiFields == null || !multiFields.Contains(field.Key)))
                                pdfFormFields.SetField(field.Key, formFields[field.Key.ToUpper()]);
                        }

                    if (multiLineFields != null)
                        foreach (MutliLineMergeField field in multiLineFields.Where(p => p.PdfMergeFieldName.ToUpper() != "CLOSINGPARAGRAPH"))
                            pdfFormFields.SetField(field.PdfMergeFieldName, string.Join(Environment.NewLine, field.DataFileValues.ToArray()));
                }
            }

            FS.Delete(template);
            return outFile;
        }

        private static string InsertAddressInfoFormFields(List<string> addressLine, string accountNumber, string template, string letterId, bool hasFormFields = false, DataAccessHelper.Region? region = null)
        {
            addressLine = addressLine.Where(p => !string.IsNullOrEmpty(p)).ToList(); //remove any blank lines

            Dictionary<string, string> formFields = new Dictionary<string, string>();

            string name = addressLine.First();
            string address = string.Join(Environment.NewLine, addressLine.Skip(1).ToList());

            formFields.Add("HEADERNAME", name);
            formFields.Add("HEADERADDRESS", address);
            formFields.Add("HEADERACCOUNTNUMBER", accountNumber);
            formFields.Add("HEADERDATE", DateTime.Now.ToString("MMM dd, yyyy"));


            string outFile = CalculateFileName(letterId, Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation", region)));
            using (PdfReader pdfReader = new PdfReader(template))
            {
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FStream(outFile, FileMode.Create), PdfWriter.VERSION_1_5))
                {
                    AcroFields pdfFormFields = pdfStamper.AcroFields;
                    if (formFields != null)
                        foreach (var field in pdfReader.AcroFields.Fields.Where(p => p.Key.ToUpper().IsIn("HEADERNAME", "HEADERADDRESS", "HEADERACCOUNTNUMBER", "HEADERDATE")))
                        {
                            if (pdfFormFields.Fields.Where(p => p.Key == field.Key).Count() > 0)
                                pdfFormFields.SetField(field.Key, formFields[field.Key.ToUpper()]);
                        }
                }
            }

            if (hasFormFields)
                FS.Delete(template);

            return outFile;
        }

        /// <summary>
        /// Insert the borrower name and address into a relative position in the PDF document.
        /// </summary>
        /// <param name="addressLine">List with name and address</param>
        /// <param name="accountNumber">Borrower Account Number</param>
        /// <param name="templatePath">Path of the template PDF</param>
        /// <param name="letterId">Letter Tracking Id</param>
        /// <param name="hasFormFields">Indicator if we have already created a file with merge fields</param>
        /// <returns>Path of new PDF</returns>
        private static string InsertAddressInformation(List<string> addressLine, string accountNumber, string templatePath, string letterId, bool hasFormFields = false, DataAccessHelper.Region? region = null)
        {
            return InsertAddressInfoFormFields(addressLine, accountNumber, templatePath, letterId, hasFormFields, region);

            //string newFile = CalculateFileName(letterId, Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation")));
            ////Read in the template PDF file
            //using (PdfReader pdfReader = new PdfReader(templatePath))
            //{

            //    //This will create the new PDF
            //    using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FStream(newFile, FileMode.Create)))
            //    {
            //        PdfContentByte t = pdfStamper.GetOverContent(1);
            //        //This is the starting Height
            //        int count = 665;
            //        foreach (string lineData in addressLine.Where(p => !string.IsNullOrEmpty(p)))
            //        {
            //            //71 is the starting width
            //            ColumnText.ShowTextAligned(
            //                t,
            //                Element.ALIGN_LEFT,
            //                new Phrase(lineData),
            //                71, count, 0
            //              );
            //            //Move down a line
            //            count -= 13;
            //        }

            //        //Reset for the account number and date
            //        count = 630;


            //        ColumnText.ShowTextAligned(
            //                   t,
            //                   Element.ALIGN_LEFT,
            //                   new Phrase(DateTime.Now.ToString("MMM dd, yyyy")),
            //                   420, count, 0
            //                 );


            //        count -= 13;

            //        ColumnText.ShowTextAligned(
            //                   t,
            //                   Element.ALIGN_LEFT,
            //                   new Phrase(accountNumber),
            //                   420, count, 0
            //                 );

            //        pdfStamper.Close();

            //    }
            //}

            ////If it has form fields we have already created one pdf, we want to delete that one.
            //if (hasFormFields)
            //    FS.Delete(templatePath);

            //return newFile;
        }

        /// <summary>
        /// Add a loan detail table to a page.
        /// </summary>
        /// <param name="dt">Data table with loan detail info</param>
        /// <returns>Path of the new PDf created</returns>
        public static string AddTable(DataTable dt)
        {
            string pdfFilePath = Path.Combine(EnterpriseFileSystem.TempFolder, Guid.NewGuid() + "DatasTable.pdf");
            var letterOrientation = dt.Columns.Count > 4 ? iTextSharp.text.PageSize.LETTER.Rotate() : iTextSharp.text.PageSize.LETTER;
            using (Document doc = new Document(letterOrientation, 0, 0, 42, 35))
            {
                //Create Document class object and set its size to letter and give space left, right, Top, Bottom Margin
                using (PdfWriter wri = PdfWriter.GetInstance(doc, new FStream(pdfFilePath, FileMode.Create)))
                {
                    wri.SetTagged();
                    doc.Open();//Open Document to write

                    var font10 = FontFactory.GetFont("ARIAL", 8);
                    var font10b = FontFactory.GetFont("ARIAL", 8);
                    font10b.SetStyle("bold");

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

                        doc.AddLanguage("english");
                    }

                    //Close document and writer
                    doc.Close();
                }
            }
            int x = dt.Columns.Count > 4 ? 330 : 240;
            int y = dt.Columns.Count > 4 ? 585 : 755;
            return AddHeader(pdfFilePath, 1, "Loan Information", x, y);
        }

        private static string AddHeader(string file, int page, string title, int startingPosition, int linePosition = 755)
        {
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{0}DataTable.pdf", Guid.NewGuid().ToBase64String()));
            //Read in the template PDF file
            using (PdfReader pdfReader = new PdfReader(file))
            {
                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FStream(newFile, FileMode.Create)))
                {
                    PdfContentByte pdfContent = pdfStamper.GetOverContent(page);
                    ColumnText.ShowTextAligned(pdfContent, Element.ALIGN_LEFT, new Phrase(title), startingPosition, linePosition, 0);
                }
            }
            Repeater.TryRepeatedly(() => FS.Delete(file));
            return newFile;
        }


        /// <summary>
        /// Merges a List of PDF files into one.
        /// </summary>
        /// <param name="files">List of files to merge</param>
        /// <param name="saveAs">Path to save the PDF as</param>
        /// <returns>Path the PDF was saved as</returns>
        public static string MergePdfs(List<string> files, string saveAs)
        {
            return MergePdfs(files, string.Empty, string.Empty, false, null, saveAs);
        }


        /// <summary>
        /// Merges a list of PDF documents into one
        /// </summary>
        /// <param name="files">List of pdf files to merge</param>
        /// <param name="letterId">Letter we are creating</param>
        /// <param name="accountNumber">Borrowers Account Number</param>
        /// <param name="saveAs">Path to save the new PDF</param>
        /// <returns>Path for new PDF</returns>
        public static string MergePdfs(List<string> files, string letterId, string accountNumber, bool tagPdf, DataAccessHelper.Region? region = null, string saveAs = null)
        {
            //UNDONE this will be needed for 508 but they are not ready for it yet.  Do not delete
            string outputFile = saveAs == null ? CalculateFileName(letterId, Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation", region)))
                : saveAs;
            string templateFile = Path.Combine(EnterpriseFileSystem.GetPath("Correspondence", region), letterId + ".pdf");
            PdfReader reader = new PdfReader(templateFile);
            string auth = "";
            string sub = "";
            string title = "";
            string keywords = "";
            try
            {
                auth = reader.Info["Author"];
                sub = reader.Info["Subject"];
                title = reader.Info["Title"];
                keywords = reader.Info["Keywords"];
            }
            catch
            {
                //do nothing some docs do not have these properties
            }



            var t = reader;
            List<PdfReader> readers = new List<PdfReader>();
            using (FileStream outputPdfStream = new FStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (Document document = new Document(PageSize.A4))
                {

                    using (PdfCopy copy = new PdfCopy(document, outputPdfStream))
                    {
                        if (tagPdf)
                        {
                            copy.SetTagged();
                            copy.SetLanguage("English");

                            copy.SetFullCompression();
                        }
                        document.Open();

                        foreach (string file in files)
                        {
                            PdfReader r = new PdfReader(file);
                            readers.Add(r);
                            copy.AddDocument(r);
                        }
                    }

                    document.AddAuthor(auth);
                    document.AddTitle(title);
                    document.AddSubject(sub);
                    document.AddKeywords(keywords);

                    document.Close();
                    document.Dispose();
                }

                outputPdfStream.Dispose();
            }

            foreach (PdfReader r in readers)
                r.Dispose();

            string fileToDelete = outputFile;
            using (var reader1 = new PdfReader(outputFile))
            {
                outputFile = saveAs == null ? CalculateFileName(letterId, Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation", region)))
                : saveAs;
                using (var stamper = new PdfStamper(reader1, new FStream(outputFile, FileMode.Create)))
                {
                    var info = reader.Info;
                    info["Author"] = auth;
                    info["Title"] = title;
                    info["Subject"] = sub;
                    info["Keywords"] = keywords;
                    stamper.AddViewerPreference(PdfName.DISPLAYDOCTITLE, new PdfBoolean(true));
                    stamper.MoreInfo = info;

                    using (var ms = new MemoryStream())
                    {
                        var xmp = new XmpWriter(ms, info);
                        stamper.XmpMetadata = ms.ToArray();
                        xmp.Close();
                    }
                }
            }
            FS.Delete(fileToDelete);

            return outputFile;

            //string outputFile = saveAs == null ? CalculateFileName(letterId, Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation", region)))
            //    : saveAs;
            //List<PdfReader> readers = new List<PdfReader>();
            //using (FileStream outputPdfStream = new FStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    using (Document document = new Document(PageSize.A4))
            //    {
            //        using (PdfCopy copy = new PdfCopy(document, outputPdfStream))
            //        {
            //            if (tagPdf)
            //            {
            //                copy.SetTagged();
            //                copy.SetLanguage("English");
            //                copy.SetFullCompression();
            //            }
            //            document.Open();

            //            foreach (string file in files)
            //            {
            //                PdfReader r = new PdfReader(file);
            //                readers.Add(r);
            //                copy.AddDocument(r);
            //            }
            //        }

            //        document.Close();
            //        document.Dispose();
            //    }

            //    outputPdfStream.Dispose();
            //}

            //foreach (PdfReader r in readers)
            //    r.Dispose();

            //return outputFile;
        }
    }
}
