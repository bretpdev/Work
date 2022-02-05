using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Word = Microsoft.Office.Interop.Word;

namespace CentralizedPrintingProcess
{
    public class Printing : PrintingBase
    {
        private ReflectionInterface RI { get; set; }

        public Printing(MiscDat miscData, ProcessLogRun plr, IJobStatus js, IEmailHandler eh, DataAccess da) : base(miscData, plr, js, eh, da) { }

        public override string PrintDirectory
        {
            get { return MiscDat.LetterDirectory; }
        }

        /// <summary>
        /// Determine whether printing has occured today or not.  If so, do nothing, if not, print and then update the database
        /// </summary>
        public override void ProcessPrinting()
        {
            //Create session and log in with batch id
            RI = new ReflectionInterface();
            BatchProcessingLoginHelper.Login(PLR, RI, MiscDat.ScriptId, "BatchUheaa");
            if (!RI.IsLoggedIn)
            {
                LogError("An error was encountered while trying to log in to OneLINK and COMPASS.  Please contact Systems Support.");
                return;
            }
            //purging of network folder
            foreach (string fileName in Directory.GetFiles(MiscDat.LetterDirectory).Where(p => File.GetCreationTime(p) < DateTime.Now.AddDays(-30)))
            {
                File.Delete(fileName);
            }
            PrintRecords();
            RI.CloseSession();
        }

        public static PrinterInfo SetPrinterSiding(LetterRecord record)
        {
            PrinterInfo pInfo = new PrinterInfo();
            pInfo.ChangePrinterSettings(record.Duplex);
            Thread.Sleep(5000);
            return pInfo;
        }

        private void PrintRecords()
        {
            List<LetterRecord> detail = DA.GetUnprocessedRecords().ToList();
            List<LetterRecord> summary = DA.GetUnprocessedRecordsSummary().ToList();
            int count = 0;
            foreach (LetterRecord currentRec in summary.OrderBy(p => p.Duplex))
            {
                List<LetterRecord> matchingRecords = detail.Where(p => p.MatchesSummary(currentRec)).ToList();
                if (matchingRecords.Count != currentRec.SummaryCount.ToIntNullable())
                {
                    LogError($"Found {matchingRecords.Count} detail records but expected {currentRec.SummaryCount} for summary: {currentRec}");
                    continue;
                }

                if (!DataAccessHelper.TestMode)
                    SetPrinterSiding(currentRec);

                PrintBatch(matchingRecords);
                count++;
                JS.LogItem("Commercial group {0}/{1} processed.", count, summary.Count);
            }
        }

        /// <summary>
        /// Print cover sheet and group of records
        /// </summary>
        /// <param name="Records"> List of Print records </param>
        private void PrintBatch(List<LetterRecord> Records)
        {
            Dictionary<LetterRecord, string> documentsToPrint = new Dictionary<LetterRecord, string>();

            foreach (LetterRecord record in Records)
            {
                string documentToPrint = ProcessRecord(record);
                if (documentToPrint != null)
                    documentsToPrint[record] = documentToPrint;
            }

            if (documentsToPrint.Any())
            {
                //print cost center printing cover sheet
                var rec = Records[0];
                string coverdata = EnterpriseFileSystem.TempFolder.ToString() + "coversheet.txt";
                //write out file to call printDocs
                using (StreamWriter coverSheet = new StreamWriter(coverdata))
                {
                    string[] headers = new string[7];
                    headers[0] = "BU";
                    headers[1] = "Description";
                    headers[2] = "Cost";
                    headers[3] = "Standard";
                    headers[4] = "Foreign";
                    headers[5] = "NumPages";
                    headers[6] = "CoverComment";
                    string[] parameters = new string[7];
                    parameters[0] = "Document Services";
                    parameters[1] = rec.LetterID;
                    parameters[2] = rec.UHEAACostCenter;
                    parameters[3] = (rec.DomesticCalc == "Y") ? documentsToPrint.Count.ToString() : "0";
                    parameters[4] = (rec.DomesticCalc != "Y") ? documentsToPrint.Count.ToString() : "0";
                    parameters[5] = (rec.Instructions != null) ? "0" : rec.PagesToPrint;
                    parameters[6] = rec.Instructions;
                    coverSheet.WriteCommaDelimitedLine(headers);
                    coverSheet.WriteCommaDelimitedLine(parameters);
                }
                DocumentProcessing.PrintDocs(EnterpriseFileSystem.GetPath("CoverSheet", DataAccessHelper.Region.Uheaa), "Scripted State Mail Cover Sheet", coverdata);

                foreach (var record in documentsToPrint.Keys)
                {
                    PrintDocument(documentsToPrint[record]);
                    DA.MarkLetterRecordAsPrinted(record.SeqNum);
                }
            }
        }

        private void PrintDocument(string document)
        {
            //print actual documents
            Word.Application DocPrint = new Word.Application();
            DocPrint.Visible = false;
            DocPrint.Documents.Add();
            object m = System.Type.Missing;

            Console.WriteLine("Printing Document " + document);
            DocPrint.PrintOut(false, m, m, m, m, m, m, m, m, m, m, m, document, m, m, m, m, m, m);
            //warning is using correct version of function
            DocPrint.Application.Quit(false);
            DocPrint.Quit(false);
            Marshal.FinalReleaseComObject(DocPrint);
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Log into session and leave LP50, LP90, and ATD22 for error
        /// </summary>
        /// <param name="record">LetterRecord</param>
        /// <param name="recordData">Comma Seperated version of the record object</param>
        private void LogErrorToSession(LetterRecord record, string recordData)
        {
            //add comments to systems
            string comment = $"Document {record.LetterID} missing from central printing.";
            string ssn;
            try
            {
                ssn = RI.GetDemographicsFromLP22(record.AccountNumber.Replace(" ", "")).Ssn;
            }
            catch (DemographicException)
            {
                RI.Atd22AllLoans(record.AccountNumber.Replace(" ", ""), "MDCS1", comment, "", "CNTRPRNT", false);
                //if onelink queue task can't be added then try COMPASS
                if (!RI.Atd22AllLoans(record.AccountNumber.Replace(" ", ""), DA.ARCsAndQueuesForBusinessUnits(record.BusinessUnit, CentralizedPrintingErrorType.CPrintingErrArc), recordData, "", "CNTRPRNT", false))
                {
                    //if COMPASS queue task can't be added then send and email
                    string message = $"There was an error in adding a queue task for [[{record}]] in COMPASS.  Please take the necessary steps to ensure the follow up queue task is created. {RI.Message}";
                    EH.AddEmail(message, "Error in adding queue task for failed printing", record.BusinessUnit, "CentralPrintingSysError");
                    LogError(message, NotificationSeverityType.Critical);
                }
                return;
            }

            RI.AddCommentInLP50(ssn, "LT", "18", "MDCS1", comment, "CNTRPRNT");

            //try and add queue tasks                    
            if (!RI.AddQueueTaskInLP9O(ssn, DA.ARCsAndQueuesForBusinessUnits(record.BusinessUnit, CentralizedPrintingErrorType.OLPrintingErrQueue), null, recordData, "", "", ""))
            {
                //if onelink queue task can't be added then try COMPASS
                if (!RI.Atd22AllLoans(ssn, DA.ARCsAndQueuesForBusinessUnits(record.BusinessUnit, CentralizedPrintingErrorType.CPrintingErrArc), recordData, "", "CNTRPRNT", false))
                {
                    //if COMPASS queue task can't be added then send and email
                    string message = $"There was an error in adding a queue task for [[{record}]] in COMPASS.  Please take the necessary steps to ensure the follow up queue task is created.  {RI.Message}";
                    EH.AddEmail(message, "Error in adding queue task for failed printing", record.BusinessUnit, "CentralPrintingSysError");
                    LogError(message, NotificationSeverityType.Critical);
                }
            }
        }

        /// <summary>
        /// Adds an error email if one doesnt already exist and process logs the error
        /// </summary>
        /// <param name="record">LetterRecord</param>
        /// <param name="recordData">comma seperated version of the object</param>
        private void AddErrorEmail(LetterRecord record, string recordData)
        {
            string message = recordData + " failed to generate. Please ensure that proper measures are taken to deliver the letter to the borrower quickly, and to prevent the problem from happening again.";
            //check if letter ID is already had a email sent for it
            if (!qcErrorEmailSentFor.Contains(record.LetterID))
            {
                EH.AddEmail(message, "Document failing to generate", record.BusinessUnit, "CentralPrintingQCError");
                //add letter ID to array list so multiple email aren't sent out
                qcErrorEmailSentFor.Add(record.LetterID);
            }
            LogError(message, NotificationSeverityType.Critical);
        }

        /// <summary>
        /// Processes a record for Ecorr, and returns a path if printing is needed.
        /// </summary>
        private string ProcessRecord(LetterRecord record)
        {
            bool doEcorr = true;
            string retPath = null;
            string fileName = Path.Combine(MiscDat.LetterDirectory, $"{record.LetterID}_{record.SeqNum}.doc");
            if (!File.Exists(fileName))
                fileName += "x"; //docx

            string skipBu = "Loan Management";
            if (record.BusinessUnit == skipBu)
            {
                JS.LogItem("Skipping Ecorr portion of " + skipBu + " document " + record.ToString());
                DA.MarkRecordEcorrStatus(record.SeqNum, false, null);
                doEcorr = false;
            }
            if (record.EcorrDocumentCreatedAt != null)
                doEcorr = false;

            if (File.Exists(fileName))
            {
                retPath = fileName;
                if (doEcorr)
                {
                    string ecorrDestination = Path.Combine(MiscDat.EcorrDirectory, Path.GetFileNameWithoutExtension(fileName) + ".pdf");
                    var ecorr = DA.GetBorrowerEcorrInfo(record.AccountNumber.Replace(" ", ""));
                    if (ecorr == null)
                    {
                        JS.LogItem("Unable to find UDW Ecorr and SSN info for record: " + record.ToString());
                    }
                    else
                    {
                        int? letterId = DA.GetLetterId(record.LetterID);
                        if (letterId == null)
                        {
                            LogError("Unable to retrieve Ecorr Letter ID for Letter: " + record.LetterID);
                            DA.MarkRecordEcorrStatus(record.SeqNum, false, null);
                        }
                        else
                        {
                            string corrMethod = "EmailNotify";
                            bool isOnEcorr = true;
                            if (!ecorr.OptedIntoEcorrLetters || !ecorr.EmailAddressIsValid)
                            {
                                corrMethod = "Printed";
                                isOnEcorr = false;
                            }
                            else
                                retPath = null;

                            Console.WriteLine("Creating Ecorr Document " + fileName + " : " + record.ToString());
                            Word.Application PdfApp = new Word.Application();
                            PdfApp.Visible = false;
                            object m = System.Type.Missing;
                            object fileNameAsObject = fileName;
                            object pdfExt = Word.WdSaveFormat.wdFormatPDF;
                            var document = PdfApp.Documents.Open(ref fileNameAsObject, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m);
                            fileNameAsObject = ecorrDestination;
                            document.SaveAs2(ref fileNameAsObject, ref pdfExt, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m, ref m);

                            document.Close();
                            PdfApp.Application.Quit(false);
                            PdfApp.Quit(false);
                            Marshal.FinalReleaseComObject(PdfApp);
                            Thread.Sleep(1000);

                            JS.LogItem("Finished Creating Ecorr Document " + fileName + " : " + record.ToString());

                            DA.AddEcorrDocumentDetails(letterId.Value, @"\bulk\FILENET_UHEAA_UT\InboundRequest\{0}\" + Path.GetFileName(ecorrDestination), ecorr.Ssn, DateTime.Now, ecorr.AccountNumber, RI.UserId, corrMethod, DateTime.Now, ecorr.EmailAddress, DateTime.Now);
                            DA.MarkRecordEcorrStatus(record.SeqNum, isOnEcorr, DateTime.Now);
                        }
                    }
                }
            }
            else if (doEcorr || record.PrintedAt == null) //MS Doc doesnt exist and we need it
            {
                //convert data row data into string
                string recordData = record.ToShortString();
                AddErrorEmail(record, recordData);
                //only add system stuff if error not handled yet
                LogErrorToSession(record, recordData);
                DA.MarkRecordDeleted(record.SeqNum);
            }

            if (record.PrintedAt == null)
                return retPath;
            else
                return null;
        }
    }
}