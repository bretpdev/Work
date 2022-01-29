using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace PRETNFRNOT
{
    class PrintLetter
    {
        const string NewServicer = "[[[NewServicer]]]";
        const string AltNewServicer = "[[[New Servicer]]]";
        const string PaymentAddress = "[[[PaymentAddress]]]";
        const string Phone = "[[[Phone]]]";
        const string CorrespondenceAddress = "[[[CorrespondenceAddress]]]";
        const string TransferDate = "[[[TransferDate]]]";
        const string Website = "[[[Website]]]";
        public ProcessLogRun PL { get; set; }
        public DataAccess Da { get; set; }

        public int Start(string letter, DataAccess da, ProcessLogRun logRun)
        {
            PL = logRun;
            Da = da;
            Console.WriteLine("Processing letter id {0}", letter);
            int rowsAdded = Da.AddCoborrowerRecords(letter);
            Console.WriteLine(string.Format("Added {0} letter records to account for coborrower processing.", rowsAdded));

            int errorCount = 0;
            List<BorrowerData> unprocessedRecords = Da.GetDataToProcess(letter);
            foreach (BorrowerData bor in unprocessedRecords)
            {
                Console.WriteLine("Processing borrower: {0} for letter id {1}", bor.AccountNumber, letter);
                if (bor.OnEcorr && bor.ValidEcorrEmail == "Y" && bor.Email.IsPopulated())
                {
                    errorCount = Email(bor, letter);
                }
                else
                {
                    errorCount = Print(bor, letter);
                    if (errorCount == 0)
                        Da.SetPrintedAt(bor); // No issues found, set as printed
                }

                int ecorrResult = Ecorr(bor, letter);
                if (ecorrResult == 0)
                    Da.SetLetterEcorrCreated(bor);

                errorCount += ecorrResult;
                if (errorCount > 0)
                    Da.SetLetterInvalid(bor);
            }
            Console.WriteLine("Finished processing letter id: {0}", letter);
            return errorCount;
        }

        /// <summary>
        /// Create an html string to merge all the fields then send an email
        /// </summary>
        private int Email(BorrowerData bor, string letter)
        {
            try
            {
                string html = "";
                using (StreamReader sr = new StreamReader(string.Format("{0}{1}.html", EnterpriseFileSystem.GetPath("EmailCampaigns"), letter)))
                {
                    html = sr.ReadToEnd();
                }

                string paragraph = GetParagraphMergeField(bor, letter);

                html = html.Replace("[[[FirstName]]]", bor.FirstName)
                           .Replace("[[[LastName]]]", bor.LastName)
                           .Replace("[[[Date]]]", DateTime.Now.ToString().ToLongDateNoDOW())
                           .Replace("[[[Address1]]]", bor.Address1)
                           .Replace("</br>[[[Address2]]]", bor.Address2.IsPopulated() ? "</br>" + bor.Address2 : "")
                           .Replace("[[[City]]]", bor.City)
                           .Replace("[[[State]]]", bor.State)
                           .Replace("[[[Zip]]]", bor.Zip)
                           .Replace("[[[Country]]]", bor.Country)
                           .Replace("[[[AccountNumber]]]", bor.AccountNumber)
                           .Replace("[[[Paragraph]]]", paragraph)
                           .Replace("''", "&#39;") //trim double apostrophies to single 
                           .Replace("There's no action for you to take.", "<b>There's no action for you to take.</b>"); 

                html = FormatParagraph(html, bor);

                EmailHelper.SendMailBatch(DataAccessHelper.TestMode, bor.Email, "customerservice@mycornerstoneloan.org", "IMPORTANT NOTICE", html, "", "", "", EmailHelper.EmailImportance.Normal, true);
            }
            catch (Exception ex)
            {
                string message = string.Format("There was an error sending the email for letter {0} for borrower {1}", letter, bor.AccountNumber);
                return LogError(message, ex);
            }

            return 0;
        }

        private string GetParagraphMergeField(BorrowerData bor, string letter)
        {
            int? paragraphId = GetParagraphId(letter, bor, true);
            return string.Format("{0}", paragraphId.HasValue ? Da.GetParagraph(paragraphId.Value) : "");

        }

        /// <summary>
        /// Sets up the files to be printed
        /// </summary>
        /// <param name="bor">The current BorrowerData object</param>
        /// <param name="letter">The letter being used for the print job.</param>
        /// <returns>0 for Success, 1 if there were errors</returns>
        private int Print(BorrowerData bor, string letter)
        {
            try
            {
                if (!bor.ValidAddress)
                {
                    string message = string.Format("The address for borrower: {0} is invalid and letter: {1} was not printed", bor.AccountNumber, bor.LetterId);
                    return LogError(message);
                }

                string dataFile = CreateDataFile(letter, bor, false);

                if (dataFile.IsPopulated())
                {
                    string doc = DocumentProcessing.AddBarcodesForBatchProcessing(dataFile, "AccountNumber", letter, true, DocumentProcessing.LetterRecipient.Borrower);
                    DocumentProcessing.PrintFederalStateMailCoverSheet(doc, letter, "State");
                    DocumentProcessing.PrintDocs(string.Format("{0}\\", EnterpriseFileSystem.GetPath("Correspondence")), letter, doc);

                    if (File.Exists(dataFile))
                        Repeater.TryRepeatedly(() => File.Delete(dataFile));
                    if (File.Exists(doc))
                        Repeater.TryRepeatedly(() => File.Delete(doc));
                }
                else
                {
                    string message = string.Format("There was an error getting the merge fields for letter {0} for borrower: {1}", letter, bor.AccountNumber);
                    return LogError(message);
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Error printing the {0} letter for borrower :{1}", letter, bor.AccountNumber);
                return LogError(message, ex);
            }
            return 0;
        }

        private int LogError(string message, Exception ex = null)
        {
            PL.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            return 1;
        }

        /// <summary>
        /// Generates the Ecorr document
        /// </summary>
        private int Ecorr(BorrowerData bor, string letter)
        {
            try
            {
                EcorrData ecorrData = EcorrProcessing.CheckEcorr(bor.AccountNumber);
                string path = string.Format("{0}\\{1}.pdf", EnterpriseFileSystem.GetPath("Correspondence"), letter);
                PdfHelper.GenerateEcorrPdf(path, bor.AccountNumber, bor.Ssn, DocumentProperties.CorrMethod.Printed, Environment.UserName, ecorrData, GetAddressInfo(bor, letter), bor.LetterAccountNumber, null, GetFormFields(bor, letter, true));
            }

            catch (Exception ex)
            {
                string message = string.Format("There was an error creating the {0} Ecorr document for borrower: {1}.", letter, bor.AccountNumber);
                return LogError(message, ex);
            }
            return 0;
        }

        /// <summary>
        /// Creates a data file to be sent to Centralized Printing
        /// </summary>
        /// <param name="letter">The letter to be used for printing</param>
        /// <param name="bor">The current BorrowerData object</param>
        /// <returns>The file path of the data file, empty if there were errors.</returns>
        private string CreateDataFile(string letter, BorrowerData bor, bool isEcorr)
        {
            string dataFile = string.Format("{0}{1}DataFile.txt", EnterpriseFileSystem.TempFolder, letter);
            using (StreamWriter sw = new StreamWriter(dataFile, false))
            {
                sw.WriteLine("KeyLine, AccountNumber, FirstName, LastName, Address1, Address2, City, State, Zip, Country, ServicerName, Website, Phone, PaymentAddress, CorrespondenceAddress, TransferDate, Paragraph, Paragraph1, Paragraph2");
                int? paragraphId = GetParagraphId(letter, bor, isEcorr);
                string mergeFields = BuildMergeData(bor, paragraphId, isEcorr);
                if (mergeFields.IsNullOrEmpty())
                    return ""; //No merge fields available, process log and end
                sw.WriteLine(mergeFields);
            }
            return dataFile;
        }

        /// <summary>
        /// Determines which letter to use to build to retrieve the data for the merge fields
        /// </summary>
        /// <param name="bor">The current BorrowerData object</param>
        /// <param name="letter">The letter to be used for printing</param>
        /// <returns></returns>
        private int? GetParagraphId(string letter, BorrowerData bor, bool isEcorr)
        {
            int? paragraphid = null;
            switch (letter)
            {
                case "TS06BSPLIT":
                case "TS06BPSTFR":
                case "TS06BPRTCH":
                    paragraphid = GetFinalPaymentData(bor, letter);
                    break;
                case "TS06BTRNCL":
                    paragraphid = GetDelayCancelData(bor, letter);
                    break;
                default:
                    PL.AddNotification(string.Format("Invalid letter id: {0}, merge fields not available for borrower: {1}", letter, bor.AccountNumber), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return null;
            }
            return paragraphid + (isEcorr ? 6 : 0); //Ecorr version is 6 indexes higher than printed version.
        }

        /// <summary>
        /// Determines which paragraph to be merged into the letter for the Final Payment letters
        /// </summary>
        /// <param name="bor">The current BorrowerData object</param>
        /// <param name="letter">The letter to be used for printing</param>
        /// <returns>The data merge fields for the data file</returns>
        private int? GetFinalPaymentData(BorrowerData bor, string letter)
        {
            int? paragraph = null;
            string message = "";
            if (bor.AchStatus == "A" && bor.AchSuspensionReason.IsNullOrEmpty())
                paragraph = 1;
            else if (bor.AchStatus == "A" && bor.AchSuspensionReason.IsPopulated())
                paragraph = null;
            else if (bor.AchStatus != "A" && Convert.ToDateTime(bor.LastPaymentDate) > DateTime.Now.AddMonths(-6))
            { //Most recent payment in the last 6 months
                if (bor.LastPaymentSource == "T" && bor.LastPaymentSubSource == "08")
                    paragraph = 2;
                else if (bor.LastPaymentSource == "T" && bor.LastPaymentSubSource == "09")
                    paragraph = 3;
                else if (bor.LastPaymentSource == "W")
                    paragraph = 3;
                else if (bor.LastPaymentSource == "S")
                    paragraph = 4;
                else if (bor.LastPaymentSource == "E")
                    paragraph = null;
                else if (bor.LastPaymentSource == "M")
                    paragraph = null;
                else if (bor.LastPaymentSource.IsNullOrEmpty())
                    paragraph = null;
                else
                    message = string.Format("There was not a final payment code available for letter {0} for borrower: {1}", letter, bor.AccountNumber);
            }
            else if (bor.LastPaymentDate.IsNullOrEmpty() || Convert.ToDateTime(bor.LastPaymentDate).Date < DateTime.Now.AddMonths(-6).Date)
                paragraph = null;
            else
                message = string.Format("There was not a final payment code available for letter {0} for borrower: {1}", letter, bor.AccountNumber);

            if (message.IsPopulated())
            {
                PL.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return null;
            }
            return paragraph;
        } 

        /// <summary>
        /// Determines which paragraph to be merged into the letter for the Delay/Cancel letters
        /// </summary>
        /// <param name="bor">The current BorrowerData object</param>
        /// <param name="letter">The letter to be used for printing</param>
        /// <returns></returns>
        private int? GetDelayCancelData(BorrowerData bor, string letter)
        {
            int? paragraphId = null;
            if (bor.DelayCancelCode == "DY")
                paragraphId = 5;
            else if (bor.DelayCancelCode == "CY")
                paragraphId = 6;
            else //There was a delay/cancel code not recognized
            {
                string message = string.Format("There was not a Delay/Cancel code available for letter {0} for borrower: {1}", letter, bor.AccountNumber);
                PL.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return null;
            }
            return paragraphId;
        }

        /// <summary>
        /// Creates the data string for the merge fields
        /// </summary>
        /// <param name="bor">The current BorrowerData object</param>
        /// <param name="paragraphId">The number corresponding to the ID field in the database</param>
        /// <returns>Data merge fields to be added to the printing data file.</returns>
        private string BuildMergeData(BorrowerData bor, int? paragraphId, bool isEcorr)
        {
            string keyline = DocumentProcessing.ACSKeyLine(bor.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string paragraph = paragraphId.HasValue ? Da.GetParagraph(paragraphId.Value) : "";
            NewServicer ns = Da.GetNewServicer(bor.RegionDeconversion);
            paragraph = FormatParagraph(paragraph, bor);
            List<string> paragraphs = paragraphId.HasValue ? paragraph.Split('^').ToList() : new List<string> { "", "", "" };
            while (paragraphs.Count < 3)
                paragraphs.Add("");

            return string.Format("\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\", \"{10}\", {11}, {12}, \"{13}\", \"{14}\", \"{15}\", \"{16}\", \"{17}\", \"{18}\"",
                keyline, bor.AccountNumber, bor.FirstName, bor.LastName, bor.Address1, bor.Address2, bor.City, bor.State, bor.Zip, bor.Country,
                ns?.ServicerName, ns?.Website, ns?.Phone, ns?.PaymentAddress, ns?.CorrespondenceAddress,
                bor.TransferDate?.ToLongDateNoDOW(), paragraphs[0].Replace("\r\n", "\v"), paragraphs[1].Replace("\r\n", "\v"), paragraphs[2].Replace("\r\n", "\v"));
        }

        private string FormatParagraph(string paragraph, BorrowerData bor, int numExtraBrackets = 3)
        {
            if (paragraph.Contains("[[[TransferDate]]]") && bor.TransferDate != null) //If this resolves as null, replace will break in the return statement
            {
                paragraph = paragraph.Replace("[[[TransferDate]]]", bor.TransferDate.ToDate().ToString("MM/dd/yyyy"));
            }

            NewServicer ns = Da.GetNewServicer(bor.RegionDeconversion);
            return paragraph.Replace("[[[NewServicer]]]", ns.ServicerName)
                            .Replace("[[[AltNewServicer]]]", ns.ServicerName)
                            .Replace("[[[PaymentAddress]]]", ns.PaymentAddress)
                            .Replace("[[[Phone]]]", ns.Phone)
                            .Replace("[[[CorrespondenceAddress]]]", ns.CorrespondenceAddress)
                            .Replace("[[[Website]]]", ns.Website);

        }

        /// <summary>
        /// Gets the address information to merge into the Ecorr pdf
        /// </summary>
        /// <param name="bor"></param>
        /// <param name="letter"></param>
        /// <returns></returns>
        private List<string> GetAddressInfo(BorrowerData bor, string letter)
        {
            return new List<string>()
            {
                string.Format("{0} {1}", bor.FirstName, bor.LastName),
                bor.Address1,
                bor.Address2,
                string.Format("{0} {1} {2}", bor.City, bor.State, bor.Zip),
                bor.Country
            };
        }

        /// <summary>
        /// Builds the data to be merged into the Ecorr document
        /// </summary>
        private Dictionary<string, string> GetFormFields(BorrowerData bor, string letter, bool isEcorr)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            NewServicer ns = Da.GetNewServicer(bor.RegionDeconversion);
            int? paragraphId = GetParagraphId(letter, bor, isEcorr);
            string paragraph = paragraphId.HasValue ? Da.GetParagraph(paragraphId.Value) : "";
            paragraph = FormatParagraph(paragraph, bor);

            string noHtml = Regex.Replace(paragraph, @"<[^>]+>|&nbsp;", "").Trim(); //Strip out all HTML tags
            //Make sure all the form fields are uppercase
            fields.Add("HEADERACCOUNTNUMBER", bor.LetterAccountNumber);
            fields.Add("FIRSTNAME", bor.FirstName);
            fields.Add("HEADERDATE", DateTime.Now.ToLongDateString().ToLongDateNoDOW());
            fields.Add("PARAGRAPH", noHtml);
            if (letter.IsIn("TS06BSPLIT", "TS06BPRTCH"))
                GetQuestionFields(fields, bor, ns, letter);
            if (letter.IsIn("TS06BPSTFR"))
                fields.Add("TRANSFERDATE", bor.TransferDate.ToDate().ToString("MM/dd/yyyy"));

            return fields;
        }

        /// <summary>
        /// Gets all the question and answer fields to be merged into the Ecorr document
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="bor"></param>
        private void GetQuestionFields(Dictionary<string, string> fields, BorrowerData bor, NewServicer ns, string letter)
        {
            //Make sure all the form fields are uppercase
            Questions q = null;
            if (letter == "TS06BPRTCH")
                q = Da.GetQAData_TS06BPRTCH(bor.RegionDeconversion, bor.TransferDate.ToLongDateNoDOW());
            else
                q = Da.GetQAData(bor.RegionDeconversion, bor.TransferDate.ToLongDateNoDOW());
            fields.Add("IMPORTANTNOTICE", q.ImportantNotice);
            fields.Add("YOURACTIONS", q.YourActions);
            fields.Add("Q1", q.Q1);
            fields.Add("A1", q.A1);
            fields.Add("Q2", q.Q2);
            fields.Add("A2", q.A2);
            fields.Add("Q3", q.Q3);
            fields.Add("A3", q.A3);
            fields.Add("Q4", q.Q4);
            fields.Add("A4", q.A4);
            fields.Add("Q5", q.Q5);
            fields.Add("A5", q.A5);
            fields.Add("WEBSITE", ns.Website);
            fields.Add("PHONE", ns.Phone);
            fields.Add("PAYMENTADDRESS", ns.PaymentAddress);
            fields.Add("CORRESPONDENCEADDRESS", ns.CorrespondenceAddress);
        }
    }
}