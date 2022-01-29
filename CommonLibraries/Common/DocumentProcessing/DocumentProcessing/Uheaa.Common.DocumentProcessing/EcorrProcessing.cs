using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Word = Microsoft.Office.Interop.Word;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.DocumentProcessing;
using Microsoft.Office.Interop.Word;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;
using System.Data.SqlClient;

namespace Uheaa.Common.DocumentProcessing
{
    public static class EcorrProcessing
    {
        public static void EcorrCostCenterPrinting(string letterId, string dataFile, string userId, string stateCodeFieldName, string scriptId, string acctNumFieldName, DocumentProcessing.LetterRecipient letterRecipient, DocumentProcessing.CostCenterOptions options, int processLogId, string costCenterFieldName = "", bool onlyGenerateEcorrPdf = false)
        {
            EcorrCostCenterPrinting(letterId, dataFile, userId, stateCodeFieldName, scriptId, acctNumFieldName, letterRecipient, options, processLogId, -1, costCenterFieldName, onlyGenerateEcorrPdf);
        }

        [Obsolete("Use AddRecordToPrintProcessing", false)]
        public static bool AddUheaaPrintRecord(int processLogId, string accountNumber, string scriptId, string letterId, string lineData, bool isOnEcorr, bool isEndorser)
        {
            try
            {
                DataAccessHelper.ExecuteSingle<int>("[print].InsertPrintData", DataAccessHelper.Database.Uls, SqlParams.Single("Letter", letterId), SqlParams.Single("ScriptId", scriptId),
                    SqlParams.Single("AccountNumber", accountNumber), SqlParams.Single("LetterData", lineData), SqlParams.Single("IsOnEcorr", isOnEcorr), SqlParams.Single("IsEndorser", isEndorser),
                    SqlParams.Single("CreatedBy", Environment.UserName));
            }
            catch (Exception ex)
            {
                ProcessLogger.ProcessLogger.AddNotification(processLogId, "Unable to add print record see exception for more detail.", NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly(), ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// E-Corr Cost Center Printing.
        /// </summary>
        /// <param name="letterId">Letter to generate.</param>
        /// <param name="dataFile">Data file to process.</param>
        /// <param name="stateCodeFieldName">State Code Field Name.</param>
        /// <param name="scriptId">Script Id.</param>
        /// <param name="acctNumFieldName">Account NUmber Field Name.</param>
        /// <param name="letterRecipient">Letter Recipient.</param>
        /// <param name="options">Cost Center Options</param>
        /// <param name="borrowerSsnFieldName">Borrower SSN field (Only needed for Endorsers).</param>
        /// <param name="costCenterFieldName">Cost Center Field Code Name.</param>
        public static void EcorrCostCenterPrinting(string letterId, string dataFile, string userId, string stateCodeFieldName, string scriptId, string acctNumFieldName, DocumentProcessing.LetterRecipient letterRecipient, DocumentProcessing.CostCenterOptions options, int processLogId, int borrowerSsnIndex, string costCenterFieldName = "", bool onlyGenerateEcorrPdf = false)
        {
            PrintingDialog d = new PrintingDialog(letterId);
            d.StartDisplay();

            RecoveryLog recovery = new RecoveryLog(string.Format("{0}_{1}_{2}_CostCenterPrintingRecovery", userId, scriptId, letterId));

            throw new NotImplementedException("EcorrCostCenterPrinting is not supported in UHEAA Please use:");

            recovery.Delete();
            d.EndDisplay();

        }



        /// <summary>
        /// Processes a datafile to create the PDF's needed for E-Corr. FED ONLY
        /// </summary>
        /// <param name="file">File to process.</param>
        /// <param name="scriptId">Script Id.</param>
        /// <param name="letterId">Letter To Generate.</param>
        /// <param name="userId">User Id.</param>
        /// <param name="accountNumberField">Account Number Field Code.</param>
        /// <param name="recovery">Recovery Object</param>
        /// <returns>Data File containing accounts that need to be printed.</returns>
        private static string GenerateEcorrDocuments(string file, string scriptId, string letterId, string userId, string accountNumberField, RecoveryLog recovery, int processLogId, int borrowerSsnIndex, bool onlyGenerateEcorr = false)
        {
            int recoveryLineNumber = 0;
            int.TryParse(recovery.RecoveryValue.SplitAndRemoveQuotes(",")[0], out recoveryLineNumber);

            //Checks recovery, if it exists there it will get the printing file that was created when the process first started.
            string printingFile = recoveryLineNumber != 0 ? recovery.RecoveryValue.SplitAndRemoveQuotes(",")[1] :
                Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{0}_{2}_printing_file{1}.txt", letterId, Guid.NewGuid().ToString(), scriptId));

            using (StreamWriter printingStream = new StreamW(printingFile, true))
            {
                List<string> fileLines = FS.ReadAllLines(file).ToList();

                //Get the header line, it will be used to get the index's of each fields
                string headerLine = fileLines.First();
                List<string> header = headerLine.SplitAndRemoveQuotes(",");

                int accountNumberIndex = header.IndexOf(accountNumberField);

                if (recoveryLineNumber == 0)
                    printingStream.WriteLine(headerLine);

                //This is the path where the template PDF resides.
                string templatePath = GetEcorrTemplate(letterId);

                ReaderWriterLockSlim tLock = new ReaderWriterLockSlim();


                Parallel.ForEach(fileLines.Skip(1 + recoveryLineNumber).ToList(), new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, line =>
                {

                    LetterHeaderFields fields = LetterHeaderFields.Populate(letterId);
                    header = header.Select(p => p.ToUpper()).ToList();
                    List<string> addressLine = GetAddressLine(header, fields, line);
                    fields.MergeFields = SetMergeFields(fields, header, line.SplitAndRemoveQuotes(","));
                    string accountNumber = line.SplitAndRemoveQuotes(",")[accountNumberIndex];
                    System.Data.DataTable loanDetail = GetLoanDetail(fields.DataTable, line.SplitAndRemoveQuotes(","), header);
                    string borrowerSsn = string.Empty;

                    if (borrowerSsnIndex == -1)
                    {
                        //tLock.EnterWriteLock();
                        Repeater.TryRepeatedly(() => borrowerSsn = DataAccessHelper.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Cdw, accountNumber.ToSqlParameter("AccountNumber")));
                        //tLock.ExitWriteLock();
                    }
                    else
                        borrowerSsn = line.SplitAndRemoveQuotes(",")[borrowerSsnIndex];


                    EcorrData data = null;
                    Repeater.TryRepeatedly(() => data = CheckEcorr(accountNumber));

                    if (data != null && data.LetterIndicator && data.ValidEmail) //On Ecorr
                        PdfHelper.GenerateEcorrPdf(templatePath, accountNumber, borrowerSsn, DocumentProperties.CorrMethod.EmailNotify, userId, data, addressLine, loanDetail, fields.MergeFields);
                    else  // Not on Ecorr but we 
                    {
                        //Generate the document to go to ecorr but also print the letter because the borrower does not have a valid email.
                        PdfHelper.GenerateEcorrPdf(templatePath, accountNumber, borrowerSsn, DocumentProperties.CorrMethod.Printed, userId, data, addressLine, loanDetail, fields.MergeFields);

                        if (!onlyGenerateEcorr && data.Format == EcorrData.CorrespondenceFormat.Standard)
                        {
                            //Lock up the printing stream resource so that there is not contention when writing to it
                            tLock.EnterWriteLock();
                            printingStream.WriteLine(line);
                            tLock.ExitWriteLock();
                        }
                    }


                    tLock.EnterWriteLock();
                    recovery.RecoveryValue = string.Format("{0},{1}", ++recoveryLineNumber, printingFile);
                    tLock.ExitWriteLock();

                });
            }

            return printingFile;
        }

        private static List<MutliLineMergeField> SetMultiLineMergeFields(LetterHeaderFields fields, List<string> header, List<string> line)
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

        /// <summary>
        /// Takes the header and sets all of the merge fields keys and values.
        /// </summary>
        /// <param name="fields">Object that has a dictionary with all the keys for the merge fields.</param>
        /// <param name="header">Header of the file we are processing.</param>
        /// <param name="line">Current line of the file we are processing.</param>
        /// <returns>Dictionary with each key and value being a merge field in the letter.</returns>
        private static Dictionary<string, string> SetMergeFields(LetterHeaderFields fields, List<string> header, List<string> line)
        {
            if (fields.MergeFields == null || fields.MergeFields.Count == 0)
                return null;

            Dictionary<string, string> mergeFields = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> item in fields.MergeFields)
                mergeFields.Add(item.Key, line[header.IndexOf(item.Key)]);

            return mergeFields;
        }

        /// <summary>
        /// Creates a data table for all loan detail information.
        /// </summary>
        /// <param name="dataTableHeaders">Headers of the datat table.</param>
        /// <param name="lineData">Line data from the file we are processing.</param>
        /// <param name="lineHeader">Header info from the file we are processing.</param>
        /// <returns>Data table with all of the borrowers loan detail info.</returns>
        private static System.Data.DataTable GetLoanDetail(List<string> dataTableHeaders, List<string> lineData, List<string> lineHeader)
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

        /// <summary>
        /// Gets the address line from the given file data.
        /// </summary>
        /// <param name="header">Header of the file.</param>
        /// <param name="fields">Fields to look for.</param>
        /// <param name="line">Current line of the file we are processing.</param>
        /// <returns>List with the address information for the current borrower.</returns>
        private static List<string> GetAddressLine(List<string> header, LetterHeaderFields fields, string line)
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
            {
                cityStateZip += linedata[header.IndexOf(item)] + " ";
            }

            addressLines.Add(cityStateZip);

            foreach (string foreignAddress in fields.ForeignFields)
            {
                string item = linedata[header.IndexOf(foreignAddress)];
                if (!item.IsNullOrEmpty())
                    addressLines.Add(item);
            }

            return addressLines;
        }

        /// <summary>
        /// Concatenates the name of the borrower.
        /// </summary>
        /// <param name="header">Header of the file.</param>
        /// <param name="fields">Name fields</param>
        /// <param name="linedata">Line of the file.</param>
        /// <returns>string with name</returns>
        private static string ConcatenateName(List<string> header, LetterHeaderFields fields, List<string> linedata)
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

        /// <summary>
        /// Checks to see if a borrower is on ecorr.
        /// </summary>
        /// <param name="accountNumber">Borrowers account number.</param>
        /// <returns>Returns Object if borrower is on ecorr, return null if borrower is not on ecorr.</returns>
        public static EcorrData CheckEcorr(string accountNumber, DataAccessHelper.Region? region = null)
        {
            var currentRegion = region ?? DataAccessHelper.CurrentRegion;
            EcorrData data = new EcorrData();
            if (currentRegion == DataAccessHelper.Region.Uheaa)
            {
                try
                {
                    data = DataAccessHelper.ExecuteSingle<EcorrData>("GetEcorrInformation", DataAccessHelper.Database.Udw, accountNumber.ToSqlParameter("AccountNumber"));
                }
                catch (Exception ex)
                {
                    data.EmailAddress = "Ecorr@UHEAA.org";
                }
            }
            else
            {
                try
                {
                    data = DataAccessHelper.ExecuteSingle<EcorrData>("GetEcorrInformation", DataAccessHelper.Database.Cdw, accountNumber.ToSqlParameter("AccountNumber"));
                }
                catch (Exception ex)
                {
                    data.EmailAddress = "Ecorr@MyCornerStoneLoan.org";
                }
                finally
                {
                    data.Format = (EcorrData.CorrespondenceFormat)DataAccessHelper.ExecuteSingle<int>("GetBorrowersAltFormat", DataAccessHelper.Database.ECorrFed, accountNumber.ToSqlParameter("AccountNumber"));
                }
            }

            return data;
        }

        /// <summary>
        /// Checks to see if a borrower is on ecorr.
        /// </summary>
        /// <param name="accountNumber">Borrowers account number.</param>
        /// <param name="cdwConn">The SqlConnection for the CDW database</param>
        /// <param name="ecorrConn">The SqlConnection for the Ecorr database</param>
        /// <returns>Returns Object if borrower is on ecorr, return null if borrower is not on ecorr.</returns>
        public static EcorrData CheckEcorr(string accountNumber, SqlConnection cdwConn, SqlConnection ecorrConn, DataAccessHelper.Region? region = null)
        {
            var currentRegion = region ?? DataAccessHelper.CurrentRegion;
            EcorrData data = new EcorrData();
            if (currentRegion == DataAccessHelper.Region.Uheaa)
                return null;
            try
            {
                data = DataAccessHelper.ExecuteSingle<EcorrData>("GetEcorrInformation", cdwConn, accountNumber.ToSqlParameter("AccountNumber"));
            }
            catch (InvalidOperationException)//ExecuteSingle throws this exception when no results come back.
            {
                data.EmailAddress = "Ecorr@MyCornerStoneLoan.org";
            }
            finally
            {
                data.Format = (EcorrData.CorrespondenceFormat)DataAccessHelper.ExecuteSingle<int>("GetBorrowersAltFormat", ecorrConn, accountNumber.ToSqlParameter("AccountNumber"));
            }

            return data;
        }

        /// <summary>
        /// This method will add a record to PrintProcess to be printed
        /// </summary>
        /// <param name="scriptId"></param>
        /// <param name="letterId"></param>
        /// <param name="letterData"></param>
        /// <param name="accountNumber">Borrowers Account Number</param>
        /// <param name="costCenter"></param>
        public static int? AddRecordToPrintProcessing(string scriptId, string letterId, string letterData, string accountNumber, string costCenter, DataAccessHelper.Region? region = null, string runBy = null, bool nonBorrower = false)
        {
            if ((region ?? DataAccessHelper.CurrentRegion) == DataAccessHelper.Region.Uheaa)
            {
                if (nonBorrower)
                    return DataAccessHelper.ExecuteIdNullable("print.InsertPrintProcessingRecordNonBwr", DataAccessHelper.Database.Uls, SqlParams.Single("ScriptId", scriptId), SqlParams.Single("LetterId", letterId), SqlParams.Single("LetterData", letterData), SqlParams.Single("AccountNumber", accountNumber), SqlParams.Single("CostCenter", costCenter), SqlParams.Single("RunBy", runBy));
                else
                    return DataAccessHelper.ExecuteIdNullable("print.InsertPrintProcessingRecord", DataAccessHelper.Database.Uls, SqlParams.Single("ScriptId", scriptId), SqlParams.Single("LetterId", letterId), SqlParams.Single("LetterData", letterData), SqlParams.Single("AccountNumber", accountNumber), SqlParams.Single("CostCenter", costCenter), SqlParams.Single("RunBy", runBy));
            }
            else
            {
                if (nonBorrower)
                    return DataAccessHelper.ExecuteIdNullable("print.InsertPrintProcessingRecordNonBwr", DataAccessHelper.Database.Cls, SqlParams.Single("ScriptId", scriptId), SqlParams.Single("LetterId", letterId), SqlParams.Single("LetterData", letterData), SqlParams.Single("AccountNumber", accountNumber), SqlParams.Single("CostCenter", costCenter), SqlParams.Single("RunBy", runBy));
                else
                    return DataAccessHelper.ExecuteIdNullable("print.InsertPrintProcessingRecord", DataAccessHelper.Database.Cls, SqlParams.Single("ScriptId", scriptId), SqlParams.Single("LetterId", letterId), SqlParams.Single("LetterData", letterData), SqlParams.Single("AccountNumber", accountNumber), SqlParams.Single("CostCenter", costCenter), SqlParams.Single("RunBy", runBy));
            }

        }

        /// <summary>
        /// This method will add a record to PrintProcess to be printed
        /// </summary>
        /// <param name="scriptId"></param>
        /// <param name="letterId"></param>
        /// <param name="letterData"></param>
        /// <param name="accountNumber">Borrowers Account Number</param>
        /// <param name="costCenter"></param>
        public static int? AddOneLinkRecordToPrintProcessing(string scriptId, string letterId, string letterData, string accountNumber, string costCenter, string runBy = null)
        {
            return DataAccessHelper.ExecuteIdNullable("print.InsertOneLinkPrintProcessingRecord", DataAccessHelper.Database.Uls, SqlParams.Single("ScriptId", scriptId), SqlParams.Single("LetterId", letterId), SqlParams.Single("LetterData", letterData), SqlParams.Single("AccountNumber", accountNumber), SqlParams.Single("CostCenter", costCenter), SqlParams.Single("RunBy", runBy));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptId"></param>
        /// <param name="letterId"></param>
        /// <param name="letterData"></param>
        /// <param name="accountNumber"></param>
        /// <param name="costCenter"></param>
        public static int? AddCoBwrRecordToPrintProcessing(string scriptId, string letterId, string letterData, string accountNumber, string costCenter, string borrowerSsn, DataAccessHelper.Region? region = null, string runBy = null)
        {
            if ((region ?? DataAccessHelper.CurrentRegion) == DataAccessHelper.Region.Uheaa)
                return DataAccessHelper.ExecuteIdNullable("print.InsertPrintProcessingRecordCoBwr", DataAccessHelper.Database.Uls, SqlParams.Single("ScriptId", scriptId), SqlParams.Single("LetterId", letterId), SqlParams.Single("LetterData", letterData), SqlParams.Single("AccountNumber", accountNumber), SqlParams.Single("CostCenter", costCenter), SqlParams.Single("BorrowerSsn", borrowerSsn), SqlParams.Single("RunBy", runBy));
            else
                return DataAccessHelper.ExecuteIdNullable("print.InsertPrintProcessingRecordCoBwr", DataAccessHelper.Database.Cls, SqlParams.Single("ScriptId", scriptId), SqlParams.Single("LetterId", letterId), SqlParams.Single("LetterData", letterData), SqlParams.Single("AccountNumber", accountNumber), SqlParams.Single("CostCenter", costCenter), SqlParams.Single("BorrowerSsn", borrowerSsn), SqlParams.Single("RunBy", runBy));
        }

        /// <summary>
        /// Creates a PDF for e-corr if the borrower is not on e-corr also creates centralized printing document.  If you pass a docId then it will image that document as well.
        /// </summary>
        /// <param name="accountNumber">Borrower or Endorsers account number</param>
        /// <param name="borrowerSsn">Borrower or endorsers SSN</param>
        /// <param name="letterId">Letter Tracking Id</param>
        /// <param name="dataFile">Data file</param>
        /// <param name="userId">AES User Id of User running the script</param>
        /// <param name="scriptId">Script Id</param>
        /// <param name="accountNumberField">Account Number Field in the data file</param>
        /// <param name="stateCodeField">State Code Field in the data file</param>
        /// <param name="demos">SystemBorroerDemo object</param>
        /// <param name="docId">Imaging Doc Id (only use if you want the document imaged)</param>
        /// <returns>Path of the document created</returns>
        public static string EcorrCentralizedPrintingAndImage(string accountNumber, string borrowerSsn, string letterId, string dataFile, string userId, string scriptId, string accountNumberField, string stateCodeField, SystemBorrowerDemographics demos, int processLogId, bool onlyGenertateEcorrPdf = false, string docId = null, DataAccessHelper.Region? region = null)
        {
            PrintingDialog d = new PrintingDialog(letterId);
            d.StartDisplay();

            string saveAs = string.Empty;
            var currentRegion = region ?? DataAccessHelper.CurrentRegion;

            if (currentRegion == DataAccessHelper.Region.CornerStone)
            {
                List<string> fileLines = FS.ReadAllLines(dataFile).ToList();
                string templatePath = GetEcorrTemplate(letterId, currentRegion);
                string headerLine = fileLines.First();
                List<string> header = headerLine.SplitAndRemoveQuotes(",");
                header = header.Select(p => p.ToUpper()).ToList();
                string line = "";
                foreach (string item in fileLines.Skip(1))
                {
                    line += item + "\r\n\r\n";
                }

                LetterHeaderFields fields = LetterHeaderFields.Populate(letterId);

                List<string> addressLine = GetAddressLine(header, fields, line);
                fields.MergeFields = SetMergeFields(fields, header, line.SplitAndRemoveQuotes(","));
                System.Data.DataTable loanDetail = GetLoanDetail(fields.DataTable, line.SplitAndRemoveQuotes(","), header);
                fields.MultiLineFields = SetMultiLineMergeFields(fields, header, line.SplitAndRemoveQuotes(","));

                EcorrData data = null;
                if (accountNumber.IsPopulated())
                    data = CheckEcorr(accountNumber, currentRegion);
                if (data != null && data.LetterIndicator && data.ValidEmail)
                    saveAs = PdfHelper.GenerateEcorrPdf(templatePath, accountNumber, borrowerSsn, DocumentProperties.CorrMethod.EmailNotify, userId, data, addressLine, loanDetail, fields.MergeFields, fields.MultiLineFields, currentRegion);
                else if (data == null || (data != null && (!data.LetterIndicator || !data.ValidEmail)))
                {
                    if (data == null)
                        data = new EcorrData() { AccountNumber = accountNumber, EbillIndicator = false, EmailAddress = "", LetterIndicator = false, TaxIndicator = false, ValidEmail = false, Format = EcorrData.CorrespondenceFormat.Standard };
                    //Generate the document to go to ecorr but also print the letter because the borrower does not have a valid email.
                    saveAs = PdfHelper.GenerateEcorrPdf(templatePath, accountNumber, borrowerSsn, DocumentProperties.CorrMethod.Printed, userId, data, addressLine, loanDetail, fields.MergeFields, fields.MultiLineFields, currentRegion);
                    if (!onlyGenertateEcorrPdf && data.Format == EcorrData.CorrespondenceFormat.Standard)
                        saveAs = DocumentProcessing.GenerateCentralizedPrintingDocument(letterId, dataFile, accountNumberField, stateCodeField, demos, currentRegion);
                }
                else
                {
                    if (data == null)
                        data = new EcorrData() { AccountNumber = accountNumber, EbillIndicator = false, EmailAddress = "", LetterIndicator = false, TaxIndicator = false, ValidEmail = false, Format = EcorrData.CorrespondenceFormat.Standard };

                    saveAs = PdfHelper.GenerateEcorrPdf(templatePath, accountNumber, borrowerSsn, DocumentProperties.CorrMethod.Printed, userId, data, addressLine, loanDetail, fields.MergeFields, fields.MultiLineFields);

                    if (!onlyGenertateEcorrPdf && data.Format == EcorrData.CorrespondenceFormat.Standard)
                        saveAs = DocumentProcessing.GenerateCentralizedPrintingDocument(letterId, dataFile, accountNumberField, stateCodeField, demos, currentRegion);
                }
            }
            else
            {
                saveAs = DocumentProcessing.GenerateCentralizedPrintingDocument(letterId, dataFile, accountNumberField, stateCodeField, demos, currentRegion);
            }

            if (!docId.IsNullOrEmpty())
                DocumentProcessing.ImageFile(saveAs, docId, demos.Ssn);

            d.EndDisplay();

            return saveAs;
        }

        public static string GetEcorrTemplate(string letterId, DataAccessHelper.Region? region = null)
        {

            DocumentPathAndName docInfo = DataAccess.GetDocumentPathAndName(letterId, region);
            string templatePath = Path.Combine(Path.GetDirectoryName(docInfo.CalculatedPath), letterId + ".pdf");
            return templatePath;
        }

        /// <summary>
        /// Creates an XML file for a given set of E-Corr Data.
        /// </summary>
        /// <param name="data">E-corr Data to create file for.</param>
        /// <param name="xmlPath">Where to save the XML file.</param>
        /// <param name="pdfPathLocation">Where the corresponding PDF is stored.</param>
        public static void GenerateXmlFileForECorr(DocumentProperties data, string xmlPath, string pdfPathLocation)
        {
            using (XmlTextWriter writer = new XmlTextWriter(xmlPath, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented; // if you want it indented
                writer.WriteStartDocument(); // <?xml version="1.0" encoding="utf-16"?>
                writer.WriteStartElement("InputRequest");
                writer.WriteAttributeString("xmlns", "urn:com/aes/imaging/input");

                //<SUBTAG>value</SUBTAG>
                writer.WriteStartElement(@"RoutingInformation");
                writer.WriteAttributeString("applicationId", "KUINBOUND");
                writer.WriteAttributeString("clientId", "KU");

                string clientUUID = string.Format("IN_{0:yyyyMMddhhmmss}_{1}", DateTime.Now, DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live ? "KUECORR" : "KUECORR_TEST_1");
                writer.WriteAttributeString("clientUUID", clientUUID);
                writer.WriteAttributeString("retryCount", "0");
                writer.WriteAttributeString("clientIpAddress", "10.10.41.167");//TODO verify this is correct
                writer.WriteAttributeString("password", "");
                writer.WriteAttributeString("userId", "");
                writer.WriteStartElement("Environment");
                writer.WriteString(DataAccessHelper.CurrentMode.ToString().ToUpper());
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement(@"Document");
                writer.WriteAttributeString("Processed", "false");
                writer.WriteStartElement("DocumentStorageInformation");
                writer.WriteStartElement("ObjectStore");
                writer.WriteString("CORNERSTONE");
                writer.WriteEndElement();
                writer.WriteStartElement("DocumentLibrary");
                writer.WriteString("CORNERSTONEEeCorr");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("DocumentLocation");
                writer.WriteStartElement("Path");
                writer.WriteString(pdfPathLocation);
                writer.WriteEndElement();
                writer.WriteEndElement();

                WriteDetailsForEcorrXmlFile(data, writer);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                //writer.Close();
            }
        }

        /// <summary>
        /// Writes the details for an XML file for E-Corr
        /// </summary>
        /// <param name="docData"></param>
        /// <param name="writer"></param>
        private static void WriteDetailsForEcorrXmlFile(DocumentProperties docData, XmlTextWriter writer)
        {
            writer.WriteStartElement("DocumentProperties");
            foreach (PropertyInfo item in docData.GetType().GetProperties())
            {
                writer.WriteStartElement("DocumentProperty");
                writer.WriteStartElement("Name");
                writer.WriteString(item.Name);
                writer.WriteEndElement();
                writer.WriteStartElement("value");
                writer.WriteString(docData.GetType().GetProperty(item.Name).GetValue(docData, null).ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}