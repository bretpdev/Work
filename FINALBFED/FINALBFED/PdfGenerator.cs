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
using Uheaa.Common.Scripts;
using Uheaa.Common.DocumentProcessing;
using System.Reflection;
using System.Data.SqlClient;
using Uheaa.Common.ProcessLogger;

namespace FINALBFED
{
    public class PdfGenerator
    {
        private LT20Data LetterData { get; set; }
        private DataAccess DA { get; set; }
        private ProcessLogRun PLR { get; set; }
        private List<LoanInformation> LoanInfo { get; set; }
        private iTextSharp.text.Font StateMailBarcodeFont { get; set; }
        private iTextSharp.text.Font ReturnMailBarcodeFont { get; set; }

        public PdfGenerator(LT20Data letterData, List<LoanInformation> loanInfo, DataAccess da, ProcessLogRun plr)
        {
            DA = da;
            PLR = plr;
            LetterData = letterData;
            LoanInfo = loanInfo;
            BaseFont customfont = BaseFont.CreateFont(EnterpriseFileSystem.GetPath("Barcode Font") + "\\IDAutomationDMatrix.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            StateMailBarcodeFont = new iTextSharp.text.Font(customfont, 8);
            ReturnMailBarcodeFont = new iTextSharp.text.Font(customfont, 4);
        }

        /// <summary>
        /// Creates the PDF determines if the borrower is on ecorr or not.
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
                    DA.UpdateProcessed(LetterData, true); //ecorr update
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
                    DA.UpdateProcessed(LetterData, true); //ecorr update
                }

                if (!validEcorr && !addressLine.HasValidAddress)
                {
                    DA.InactivateLetter(LetterData, 5);
                }
            }

            if (cpFile.IsPopulated())
            {
                AddBarcodesToPdfAndPrint(cpFile, LetterData.RM_DSC_LTR_PRC, addressLine.Country.IsPopulated());
                Repeater.TryRepeatedly(() => File.Delete(cpFile));
                DA.UpdateProcessed(LetterData, false); //ecorr update
            }
        }

        private string CreateLoanDetail(List<DataTable> dts)
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

                    if (dts != null)
                    {
                        foreach (DataTable dt in dts)
                        {
                            //Create instance of the pdf table and set the number of column in that table
                            PdfPTable table = new PdfPTable(dt.Columns.Count);
                            PdfPCell cell = null;
                            table.HeaderRows = 1;

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
                    }
                    //Close document and writer
                    doc.Close();
                }
            }

            return pdfFilePath;
        }

        private string CreateLoanDetail()
        {
            List<DataTable> t = new List<DataTable>();
            foreach (LoanInformation li in LoanInfo)
                t.Add(li.CreateDataTable());

            List<string> files = new List<string>();
            int skip = 0;
            int take = 6;
            while (skip < t.Count)
            {
                files.Add(CreateLoanDetail(t.Skip(skip).Take(take).ToList()));
                skip += take;
            }

            string returnFile = PdfHelper.MergePdfs(files, LetterData.RM_DSC_LTR_PRC, "", true, DataAccessHelper.CurrentRegion);

            foreach (string file in files)
                Repeater.TryRepeatedly(() => File.Delete(file));

            return returnFile;
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
            string templatePath = Path.Combine(EnterpriseFileSystem.GetPath("Correspondence"), string.Format("{0}{1}.pdf", LetterData.RM_DSC_LTR_PRC, isEcorr ? "Ecorr" : ""));
            string newFile = CalculateFileName(LetterData.RM_DSC_LTR_PRC);
            //Read in the template PDF file
            using (PdfReader pdfReader = new PdfReader(templatePath))
            {
                //This will create the new PDF
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create)))
                {
                    PdfContentByte pdfContent = pdfStamper.GetOverContent(1);
                    loanDetail = CreateLoanDetail();
                    InsertAddress(addressLine, pdfReader, pdfStamper);
                }
            }


            //InsertAddressInfo(addressLine, pdfStamper, pdfContent);



            List<string> mergeFiles = new List<string>() { newFile, loanDetail };
            string returnFile = PdfHelper.MergePdfs(mergeFiles, "TS06BFNBIL", "", true, DataAccessHelper.CurrentRegion);

            foreach (string file in mergeFiles)
                Repeater.TryRepeatedly(() => File.Delete(file));

            File.Delete(newFile);

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
