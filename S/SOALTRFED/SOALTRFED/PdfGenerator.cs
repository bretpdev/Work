using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using Uheaa.Common.DataAccess;
using System.Drawing.Text;
using System.Drawing;
using Uheaa.Common;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace SOALTRFED
{
    public class PdfGenerator
    {
        private LT20Data LetterData { get; set; }
        private DataAccess DA { get; set; }
        private ProcessLogRun PLR { get; set; }
        private DataTable LoanInfo { get; set; }
        private DataTable FinancialTrans { get; set; }
        private iTextSharp.text.Font StateMailBarcodeFont { get; set; }
        private iTextSharp.text.Font ReturnMailBarcodeFont { get; set; }

        public PdfGenerator(LT20Data letterData, DataTable loanInfo, DataTable financialTrans, DataAccess da, ProcessLogRun plr)
        {
            DA = da;
            PLR = plr;
            LetterData = letterData;
            LoanInfo = loanInfo;
            FinancialTrans = financialTrans;
            BaseFont customfont = BaseFont.CreateFont(EnterpriseFileSystem.GetPath("Barcode Font") + "\\IDAutomationDMatrix.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            StateMailBarcodeFont = new iTextSharp.text.Font(customfont, 8);
            ReturnMailBarcodeFont = new iTextSharp.text.Font(customfont, 4);
        }

        /// <summary>
        /// Creates an ecorr or printed PDF
        /// </summary>
        public void CreatePdf()
        {
            AddressInfo addressLine = DA.GetAddress(LetterData);
            EcorrData ecorrInfo = EcorrProcessing.CheckEcorr(LetterData.DF_SPE_ACC_ID);
            string ecorrFile = "";
            string cpFile = "";
            DocumentProperties prop = null;
            if (ecorrInfo.AccountNumber == null || !(ecorrInfo.LetterIndicator && ecorrInfo.ValidEmail)) //printed
            {
                if (!LetterData.EcorrDocumentCreatedAt.HasValue)
                {
                    ecorrFile = GeneratePdf(addressLine, true);
                    string path = EnterpriseFileSystem.GetPath("ECORRDocuments") + Path.GetFileName(ecorrFile);
                    bool coBorrower = LetterData.CoborrowerSSN.IsPopulated();
                    string ssn;
                    if (coBorrower)
                        ssn = LetterData.CoborrowerSSN;
                    else
                        ssn = LetterData.EndorsersSsn.IsNullOrEmpty() ? LetterData.RF_SBJ_PRC : LetterData.EndorsersSsn;

                    File.Move(ecorrFile, Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), Path.GetFileName(ecorrFile)));

                    prop = new DocumentProperties(ssn, LetterData.DF_SPE_ACC_ID, LetterData.RM_DSC_LTR_PRC, "UT00801", ecorrInfo.EmailAddress, DocumentProperties.CorrMethod.Printed, path, ecorrInfo.Format);
                    prop.InsertEcorrInformation();
                    DA.UpdateProcessed(LetterData, true);
                }

                if (addressLine.HasValidAddress && !LetterData.PrintedAt.HasValue)
                    cpFile = GeneratePdf(addressLine, false);

                if (!addressLine.HasValidAddress)
                    DA.InactivateLetter(LetterData, 5);
            }
            else
            {
                bool validEcorr = ecorrInfo.LetterIndicator && ecorrInfo.ValidEmail;
                DocumentProperties.CorrMethod ecorrMethod = validEcorr ? DocumentProperties.CorrMethod.EmailNotify : DocumentProperties.CorrMethod.Printed;

                if (!LetterData.EcorrDocumentCreatedAt.HasValue)
                {
                    ecorrFile = GeneratePdf(addressLine, true);

                    string path = EnterpriseFileSystem.GetPath("ECORRDocuments") + Path.GetFileName(ecorrFile);
                    bool coBorrower = LetterData.CoborrowerSSN.IsPopulated();
                    string ssn;
                    if (coBorrower)
                        ssn = LetterData.CoborrowerSSN;
                    else
                        ssn = LetterData.EndorsersSsn.IsNullOrEmpty() ? LetterData.RF_SBJ_PRC : LetterData.EndorsersSsn;

                    File.Move(ecorrFile, Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), Path.GetFileName(ecorrFile)));

                    prop = new DocumentProperties(ssn, LetterData.DF_SPE_ACC_ID, LetterData.RM_DSC_LTR_PRC, "UT00801", ecorrInfo.EmailAddress, ecorrMethod, path, ecorrInfo.Format);
                    prop.InsertEcorrInformation();
                    DA.UpdateProcessed(LetterData, true);

                    if (!validEcorr && !addressLine.HasValidAddress)
                        DA.InactivateLetter(LetterData, 5);
                }
            }

            if (cpFile.IsPopulated())
            {
                AddBarcodesToPdfAndPrint(cpFile, LetterData.RM_DSC_LTR_PRC, addressLine.Country.IsPopulated());
                Repeater.TryRepeatedly(() => File.Delete(cpFile));
                DA.UpdateProcessed(LetterData, false);
            }
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

        private void AddBarcodesToPdfAndPrint(string file, string letterId, bool isForeign)
        {
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{0}DataTable.pdf", Guid.NewGuid().ToBase64String()));
            //Read in the template PDF file
            using (PdfReader pdfReader = new PdfReader(file))
            {
                int numberOfPages = pdfReader.NumberOfPages;
                int duplexPages = numberOfPages / 2;
                if (duplexPages % 2 == 0)
                    duplexPages++;

                duplexPages++;

                List<string> returnMailBarcodes = DocumentProcessing.GetReturnMailBarcodes(LetterData.DF_SPE_ACC_ID, LetterData.RM_DSC_LTR_PRC, DocumentProcessing.LetterRecipient.Borrower);
                List<string> stateMailBarcodes = DocumentProcessing.GetStateMailBarcodesforPdf(LetterData.DF_SPE_ACC_ID, duplexPages, DocumentProcessing.LetterRecipient.Borrower);

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

                DocumentProcessing.PrintStateMailCoverSheet(pageCount, isForeign, LetterData.RM_DSC_LTR_PRC);
                DocumentProcessing.PrintPdf(newFile);

                Repeater.TryRepeatedly(() => File.Delete(newFile));
            }
        }

        private string GeneratePdf(AddressInfo addressLine, bool isEcorr)
        {
            string loanDetail = "";
            string financialTran = "";
            string templatePath = Path.Combine(EnterpriseFileSystem.GetPath("Correspondence"), string.Format("{0}{1}.pdf", LetterData.RM_DSC_LTR_PRC, isEcorr ? "Ecorr" : ""));
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
                    InsertAddress(addressLine, pdfReader, pdfStamper);

                    pdfStamper.Close();
                }
            }

            List<string> mergeFiles = new List<string>() { newFile, loanDetail, financialTran };
            string returnFile = PdfHelper.MergePdfs(mergeFiles, LetterData.RM_DSC_LTR_PRC, "", true, DataAccessHelper.CurrentRegion);

            foreach (string file in mergeFiles)
                Repeater.TryRepeatedly(() => File.Delete(file));

            return returnFile;
        }

        private void InsertAddress(AddressInfo addressLine, PdfReader pdfReader, PdfStamper pdfStamper)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>();

            string name = addressLine.Name;
            string address = addressLine.ToString();

            formFields.Add("HEADERNAME", name);
            formFields.Add("HEADERADDRESS", address);
            formFields.Add("HEADERACCOUNTNUMBER", LetterData.LetterAccountNumber);
            formFields.Add("HEADERDATE", DateTime.Now.ToString("MMM dd, yyyy"));



            AcroFields pdfFormFields = pdfStamper.AcroFields;
            if (formFields != null)
                foreach (var field in pdfReader.AcroFields.Fields.Where(p => p.Key.ToUpper().IsIn("HEADERNAME", "HEADERADDRESS", "HEADERACCOUNTNUMBER", "HEADERDATE")))
                {
                    if (pdfFormFields.Fields.Where(p => p.Key == field.Key).Count() > 0)
                        pdfFormFields.SetField(field.Key, formFields[field.Key.ToUpper()]);
                }
        }



        private static string CalculateFileName(string letterId, string path = null)
        {
            if (path == null)
                path = EnterpriseFileSystem.TempFolder;

            return string.Format("{0}{1}{2}.pdf", path, letterId, Guid.NewGuid());
        }
    }
}
