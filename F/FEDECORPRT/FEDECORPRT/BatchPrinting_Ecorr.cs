using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;

namespace FEDECORPRT
{
    partial class BatchPrinting
    {
        private void DoEcorr(ScriptData file, PrintProcessingData data, string templatePath, bool isCoBorrower)
        {
            Console.WriteLine("Creating Ecorr Doc for PrintProcessingId:{0}", data.PrintProcessingId);
            LetterHeaderFields fields = LetterHeaderFields.Populate(file.Letter);
            List<string> header = file.FileHeader.SplitAndRemoveQuotes(",").ToList().Select(p => p.ToUpper()).ToList();
            List<string> addressLine = GetAddressLine(header, fields, data.LetterData);
            fields.MergeFields = SetMergeFields(fields, header, data.LetterData.SplitAndRemoveQuotes(","));
            DataTable loanDetail = GetLoanDetail(fields.DataTable, data.LetterData.SplitAndRemoveQuotes(","), header);
            fields.MultiLineFields = SetMultiLineMergeFields(fields, header, data.LetterData.SplitAndRemoveQuotes(","));
            string ssn;
            EcorrData ecorrInfo;
            string documentAccountNumber = "";
            if(!isCoBorrower)
            {
                ssn = file.IsEndorser ? data.LetterData.SplitAndRemoveQuotes(",")[file.EndorsersBorrowerSSNIndex.Value + (file.BarcodeOffset.HasValue == true ? file.BarcodeOffset.Value : 0)] : data.BF_SSN;
                ecorrInfo = EcorrProcessing.CheckEcorr(data.AccountNumber);
                if (ssn.IsPopulated())
                {
                    documentAccountNumber = data.AccountNumber = DA.GetAccountNumberFromSsn(ssn); //Grab the account number for the borrower from the endorser ssn
                }  
            }
            else
            {
                ssn =  data.BF_SSN;
                ecorrInfo = EcorrProcessing.CheckEcorr(data.AccountNumber);
                if (ssn.IsPopulated())
                    documentAccountNumber = DA.GetAccountNumberFromSsn(data.BorrowerSsn); //Grab the account number for the borrower from the endorser ssn
            }
            GeneratePdfAndDoEcorr(templatePath, data.AccountNumber, ssn, data.OnEcorr ? DocumentProperties.CorrMethod.EmailNotify : DocumentProperties.CorrMethod.Printed,
                "UT00801", ecorrInfo, addressLine, documentAccountNumber, loanDetail, fields.MergeFields, fields.MultiLineFields, DataAccessHelper.Region.CornerStone);
            data.MarkEcorrDone(DA, isCoBorrower);
        }

        private List<MutliLineMergeField> SetMultiLineMergeFields(LetterHeaderFields fields, List<string> header, List<string> line)
        {
            List<MutliLineMergeField> values = new List<MutliLineMergeField>();

            foreach (MutliLineMergeField field in fields.MultiLineFields)
            {
                List<string> dataFileValues = new List<string>();
                foreach (string value in field.DataFileHeaders)
                {
                    int headerIndex = header.IndexOf(value.ToUpper());
                    dataFileValues.Add(line[headerIndex]);
                }

                values.Add(new MutliLineMergeField()
                {
                    PdfMergeFieldName = field.PdfMergeFieldName,
                    DataFileHeaders = field.DataFileHeaders,
                    DataFileValues = dataFileValues
                });
            }

            return values;
        }

        private static string CalculateFileName(string letterId)
        {
            return string.Format("{0}{1}{2}.pdf", EnterpriseFileSystem.GetPath("ECORRLocation"), letterId, Guid.NewGuid());
        }

        public void GeneratePdfAndDoEcorr(string templatePath, string accountNumber, string borrowerSsn, DocumentProperties.CorrMethod ecorrMethod, string userId, EcorrData data, List<string> address, string documentAccountNumber, DataTable loanDetail = null, Dictionary<string, string> formFields = null, List<MutliLineMergeField> multiLineFields = null, DataAccessHelper.Region? region = null)
        {
            var currentRegion = region ?? DataAccessHelper.CurrentRegion;
            string letterId = Path.GetFileNameWithoutExtension(templatePath);
            List<string> filesToMerge = new List<string>();
            string localFile = CalculateFileName(letterId);
            File.Copy(templatePath, localFile);

            filesToMerge.Add(WriteFormValues(formFields, localFile, letterId, multiLineFields, address, documentAccountNumber, loanDetail));

            Forms form = DocumentProcessing.HasForm(letterId);
            if (form != null)
                filesToMerge.Add(form.PathAndForm);

            string finalFile = string.Empty;
            if (filesToMerge.Count > 1)
            {
                finalFile = PdfHelper.MergePdfs(filesToMerge, letterId, accountNumber, true, currentRegion);
                string formPath = form != null ? form.PathAndForm : "";
                foreach (string file in filesToMerge.Where(p => p != formPath))
                    File.Delete(file);
            }
            else
                finalFile = filesToMerge.FirstOrDefault();

            string path = EnterpriseFileSystem.GetPath("ECORRDocuments", currentRegion) + Path.GetFileName(finalFile);
            DocumentProperties doc = new DocumentProperties(borrowerSsn, accountNumber, letterId, userId, data.EmailAddress, ecorrMethod, path, data.Format, currentRegion);
            doc.InsertEcorrInformation();

            while (File.Exists(localFile))
                File.Delete(localFile);
        }

        private string WriteFormValues(Dictionary<string, string> formFields, string template, string letterId, List<MutliLineMergeField> multiLineFields, List<string> addressLine, string accountNumber, DataTable dt)
        {
            addressLine = addressLine.Where(p => !string.IsNullOrEmpty(p)).ToList(); //remove any blank lines
            string name = addressLine.First();
            string address = string.Join(Environment.NewLine, addressLine.Skip(1).ToList());
            if (formFields == null)
                formFields = new Dictionary<string, string>();
            formFields.Add("HEADERNAME", name);
            formFields.Add("HEADERADDRESS", address);
            formFields.Add("HEADERACCOUNTNUMBER", accountNumber);
            formFields.Add("HEADERDATE", DateTime.Now.ToString("MMM dd, yyyy"));
            string outFile = CalculateFileName(letterId);
            using (PdfReader pdfReader = new PdfReader(template))
            {
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(outFile, FileMode.Create), PdfWriter.VERSION_1_5))
                {
                    AcroFields pdfFormFields = pdfStamper.AcroFields;
                    if (formFields != null)
                        foreach (var field in pdfReader.AcroFields.Fields)
                        {
                            string[] multiFields = multiLineFields != null ? multiLineFields.Select(p => p.PdfMergeFieldName).ToArray() : null;
                            if (pdfFormFields.Fields.Where(p => p.Key == field.Key).Count() > 0 && (multiFields == null || !multiFields.Contains(field.Key)))
                                pdfFormFields.SetField(field.Key, formFields[field.Key.ToUpper()]);
                        }

                    if (multiLineFields != null)
                        foreach (MutliLineMergeField field in multiLineFields)
                            pdfFormFields.SetField(field.PdfMergeFieldName, string.Join(Environment.NewLine, field.DataFileValues.ToArray()));
                }
            }
            string dtFile = "";
            if (dt != null)
            {
                dtFile = CalculateFileName(letterId);
                AddLoanDetail(dt, dtFile);
                string newOutFile = PdfHelper.MergePdfs(new List<string>() { outFile, dtFile }, letterId, accountNumber, true, DataAccessHelper.Region.CornerStone);
                File.Delete(outFile);
                File.Delete(dtFile);
                outFile = newOutFile;
            }
            File.Delete(template);
            return outFile;
        }

        public string AddLoanDetail(DataTable dt, string pdfFilePath)
        {
            var letterOrientation = dt.Columns.Count > 4 ? iTextSharp.text.PageSize.LETTER.Rotate() : iTextSharp.text.PageSize.LETTER;
            using (Document doc = new Document(letterOrientation, 0, 0, 42, 35))
            {
                //Create Document class object and set its size to letter and give space left, right, Top, Bottom Margin
                using (PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Append)))
                {
                    wri.SetTagged();
                    doc.Open();//Open Document to write
                    

                    var font10 = FontFactory.GetFont("Times New Roman", 8);
                    var font10b = FontFactory.GetFont("Times New Roman", 8);
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
                        doc.AddHeader("Loan Information", "Loan Information");
                        doc.Add(table); // add pdf table to the document
                        doc.AddLanguage("english");
                        
                    }
                    //Close document and writer
                    doc.Close();
                }
            }

            return pdfFilePath;
        }

        private DataTable GetLoanDetail(List<string> dataTableHeaders, List<string> lineData, List<string> lineHeader)
        {
            if (dataTableHeaders.Count == 0)
                return null;
            System.Data.DataTable dt = new System.Data.DataTable();
            foreach (string header in dataTableHeaders)
                dt.Columns.Add(header);

            bool doneProcessing = false;
            for (int position = 1; !doneProcessing; position++)
            {
                if (position > 36)
                    break;
                List<string> dataFields = new List<string>();
                foreach (string header in dataTableHeaders)
                {
                    int index = lineHeader.IndexOf(header + position.ToString());
                    if (index < lineData.Count && index > 0)
                    {
                        string data = lineData[index];
                        if (data.IsNullOrEmpty()) //Once we get to the first empty field we are done 
                            continue;

                        dataFields.Add(data);
                    }
                    else
                        doneProcessing = true;
                }

                if (dataFields.Count != 0)
                    dt.Rows.Add(dataFields.ToArray());
            }

            return dt;
        }

        private Dictionary<string, string> SetMergeFields(LetterHeaderFields fields, List<string> header, List<string> line)
        {
            if (fields.MergeFields == null || fields.MergeFields.Count == 0)
                return null;

            Dictionary<string, string> mergeFields = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> item in fields.MergeFields)
                mergeFields.Add(item.Key, line[header.IndexOf(item.Key)]);

            return mergeFields;
        }

        private List<string> GetAddressLine(List<string> header, LetterHeaderFields fields, string line)
        {
            List<string> addressLines = new List<string>();
            List<string> linedata = line.SplitAndRemoveQuotes(",");
            string name = ConcatenateName(header, fields, linedata);

            addressLines.Add(name);
            foreach (string address in fields.AddressLine)
            {
                string addrLine = linedata[header.IndexOf(address)];
                if (!addrLine.IsNullOrEmpty())
                    addressLines.Add(addrLine);
            }

            string cityStateZip = string.Empty;
            foreach (string item in fields.CityStateZip)
                cityStateZip += linedata[header.IndexOf(item)] + " ";

            addressLines.Add(cityStateZip);

            foreach (string foreignAddress in fields.ForeignFields)
            {
                string item = linedata[header.IndexOf(foreignAddress)];
                if (!item.IsNullOrEmpty())
                    addressLines.Add(item);
            }

            return addressLines;
        }

        private string ConcatenateName(List<string> header, LetterHeaderFields fields, List<string> linedata)
        {
            string name = string.Empty;
            foreach (string field in fields.Name)
            {
                int index = header.IndexOf(field);
                if (index < 0)
                    Dialog.Info.Ok(field);
                name += linedata[index] + " ";
            }
            return name;
        }
    }
}