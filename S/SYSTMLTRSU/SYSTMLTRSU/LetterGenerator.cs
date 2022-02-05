using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Word = Microsoft.Office.Interop.Word;
using DT = System.Data.DataTable; //Word also has a DataTable object
using DMATRIXLib;
using Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SYSTMLTRSU
{
    public class LetterGenerator
    {
        private ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }
        private const string FileNamePattern = "SYSTMLTRSU.";
        private string AddressHeaderLine
        {
            get
            {
                return "Name,Address1,Address2,City,State,Zip,Country,ForeignState,AccountNumber,BarcodeAccountNumber,Hours1,Hours2,KeyLine,CostCenter";
            }
        }

        public LetterGenerator(ProcessLogRun plr)
        {
            PLR = plr;
            DA = new DataAccess(PLR);
            string normalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Templates\Normal.dotm");
            if (File.Exists(normalPath))
            {
                try
                {
                    File.Delete(normalPath);
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Please close all open Microsoft Word applications and then retry", "Close Microsoft Word", System.Windows.Forms.MessageBoxButtons.OK);
                    throw e;
                }
            }
        }

        /// <summary>
        /// Prints and Ecorrs all System letters
        /// </summary>
        /// <returns></returns>
        public int Generate()
        {
            if (DataAccessHelper.TestMode)
            {
                if (Uheaa.Common.Dialog.Info.YesNo("Do you want to change the printer setting automatically?"))
                {
                    PrinterInfo pi = new PrinterInfo();
                    while (pi.Duplex != 2)
                    {
                        pi.ChangePrinterSettings(true);
                        Thread.Sleep(10000);
                    }
                }
            }
            else
            {
                PrinterInfo pi = new PrinterInfo();
                while (pi.Duplex != 2)
                {
                    pi.ChangePrinterSettings(true);
                    Thread.Sleep(10000);
                }
            }
            if (!DoRecovery())
                return 1;

            if (!DA.InactivateInvalidLetters())
                return 1;


            Console.WriteLine("Adding coborrower records to database for currently unprocessed borrower records.");
            int rowsAdded = DA.AddCoborrowerRecords();
            Console.WriteLine(string.Format("Added {0} letter records to account for coborrower processing.", rowsAdded));

            Console.WriteLine("Getting unprocessed records.  This can take a minute.");
            List<LT20Data> unprocessedRecords = GetRecords();
            ProcessNonEcorr(unprocessedRecords.Where(p => (!p.OnEcorr || p.Recipient.IsPopulated()) && !p.PrintedAt.HasValue).ToList());
            ProcessEcorr(unprocessedRecords.Where(p => !p.EcorrDocumentCreatedAt.HasValue && p.Recipient.IsNullOrEmpty()).ToList());//We generate an ecorr doc for all borrower regardless of whether they are one ecorr or not.

            return 0;
        }

        private void ProcessEcorr(List<LT20Data> unprocessedRecords)
        {
            Queue<Application> word = new Queue<Application>();
            for (int wordCount = 0; wordCount < Program.NumberOfWinwords; wordCount++)
            {
                word.Enqueue(new Microsoft.Office.Interop.Word.Application());
            }
            foreach (string letter in unprocessedRecords.Select(p => p.RM_DSC_LTR_PRC).Distinct())
            {
                Parallel.ForEach(unprocessedRecords.Where(q => q.RM_DSC_LTR_PRC == letter).ToList(), new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, letterToGenerate =>
                {
                    if (letterToGenerate.InvalidLoanStatus)
                    {
                        DA.InactivateLetter(letterToGenerate, 2);
                        letterToGenerate.InvalidatedEcorr = true;
                        return;
                    }

                    //Update processing attempts if it hasn't been updated on this run
                    if (!letterToGenerate.ProcessingAttemptsUpdated)
                    {
                        letterToGenerate.ProcessingAttempts = (letterToGenerate.ProcessingAttempts ?? (int?)0).Value + 1;
                        DA.IncrementProcessingAttempts(letterToGenerate);
                        letterToGenerate.ProcessingAttemptsUpdated = true;
                    }

                    GenerateLetterDataFiles(letterToGenerate, letter, false);
                });


                List<List<LT20Data>> processingGroups = PrepEcorrFiles(unprocessedRecords.Where(p => !p.InvalidatedEcorr && p.RM_DSC_LTR_PRC == letter && !p.SkipProcessingForRun).ToList());
                DocumentPathAndName docInfo = Uheaa.Common.DocumentProcessing.DataAccess.GetDocumentPathAndName(letter);
                string templatePath = Path.Combine(docInfo.CalculatedPath, docInfo.CalculatedFileName);
                
                string dir = Path.Combine(EnterpriseFileSystem.TempFolder, Program.ScriptId);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                
                ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
                Parallel.ForEach(processingGroups, new ParallelOptions { MaxDegreeOfParallelism = Program.NumberOfWinwords }, group =>
                {
                    locker.EnterWriteLock();
                    var wordApp = word.Dequeue();
                    locker.ExitWriteLock();
                    ProcessDocuments(templatePath, group, dir, locker, wordApp);
                    locker.EnterWriteLock();
                    word.Enqueue(wordApp);
                    locker.ExitWriteLock();
                });

               

                if (Directory.Exists(dir))
                {
                    var result = Repeater.TryRepeatedly(() => Directory.Delete(dir, true));
                    CheckRepeater(result, string.Format("Unable to delete directory {0}", dir), false);
                }
            }

            while(word.Any())
            {
                var wordApp = word.Dequeue();
                Console.WriteLine("Closing All Word Docs.");
                wordApp.Application.Quit();
                wordApp.Quit();
                Marshal.FinalReleaseComObject(wordApp);
                Thread.Sleep(1000);
            }

            GC.Collect();
            // GC.WaitForPendingFinalizers();
            Console.WriteLine("Done.{0}", Environment.NewLine);
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Uses word to process multiple files with one word object
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="group"></param>
        /// <param name="dir"></param>
        private void ProcessDocuments(string templatePath, List<LT20Data> group, string dir, ReaderWriterLockSlim locker, Microsoft.Office.Interop.Word.Application wordApp)
        {
            string templateFile = Path.Combine(dir, string.Format("{0}_{1}", Guid.NewGuid().ToBase64String(), Path.GetFileName(templatePath)));//Create a unique file.
            File.Copy(templatePath, templateFile);
            object fileName = templateFile;
            object refFalse = false;
            object refTrue = true;
            object pause = false;
            object missing = System.Type.Missing;
            object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
            string lastFileProcessed = "";
            var doc = wordApp.Application.Documents.Open(ref fileName, ref missing, ref refTrue);
            wordApp.Visible = false;
            wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            object saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
            bool hadError = false;
            foreach (LT20Data data in group)
            {
                try
                {
                    object saveAs = CreateDocument(data, lastFileProcessed, doc, pause, format, wordApp, missing, refTrue);
                    DoEcorr(data, saveAs);
                    lastFileProcessed = data.DataFile;

                }
                catch (Exception ex)//Handle any problems and process log so that 1 document crashing will not take down the entire process.
                {
                    hadError = true;
                    ProcessLogger.AddNotification(Program.PL.ProcessLogId, string.Format("Unable to generate Letter {1} for AccountNumberId:{0}", data.DF_SPE_ACC_ID, data.RM_DSC_LTR_PRC), NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly(), ex);
                    break;
                }
                finally
                {
                    ((Word._Document)wordApp.ActiveDocument).Close(ref saveChanges);//This is the newly merge document and not the template file
                }
            }

            Console.Write("Closing Doc.{0}", Environment.NewLine);
            if(!hadError)
            ((Word._Document)doc).Close(ref refFalse);//this is the template file
            
            doc = null;
            Parallel.ForEach(group, new ParallelOptions() { MaxDegreeOfParallelism = int.MaxValue }, data =>
            {
                Repeater.TryRepeatedly(() => File.Delete(data.DataFile));
            });
        }

        private void DoEcorr(LT20Data data, object saveAs)
        {
            DocumentProperties.CorrMethod ecorr = data.OnEcorr ? DocumentProperties.CorrMethod.EmailNotify : DocumentProperties.CorrMethod.Printed;
            string path = EnterpriseFileSystem.GetPath("ECORRDocuments") + Path.GetFileName(saveAs.ToString());
            bool coBorrower = data.CoborrowerSSN.IsPopulated();
            string ssn;
            string accountNumberXML = data.DF_SPE_ACC_ID;
            if (coBorrower)
                ssn = data.CoborrowerSSN;
            else if (data.EndorsersSsn.IsPopulated())
            {
                ssn = data.RF_SBJ_PRC;
                accountNumberXML = data.EndorsersAccountNumber;
                data.LetterAccountNumber = data.EndorsersAccountNumber;
            }
            else
                ssn = data.RF_SBJ_PRC;

            if(ssn.IsNullOrEmpty())
                ssn = DataAccessHelper.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ? DataAccessHelper.Database.Cdw : DataAccessHelper.Database.Udw, accountNumberXML.ToSqlParameter("AccountNumber"));

            DocumentProperties docprop = new DocumentProperties(ssn, accountNumberXML, data.RM_DSC_LTR_PRC, "UT00204", data.EmailAddress, ecorr, path);
            docprop.InsertEcorrInformation();
            DA.UpdateEcorrDocCreated(data);
        }

        private object CreateDocument(LT20Data data, string lastFileProcessed, Document doc, object pause, object format, Word.Application wordApp, object missing, object refTrue)
        {
            Console.WriteLine("Processing {0}", data.DF_SPE_ACC_ID);
            object mergeType = Word.WdMergeSubType.wdMergeSubTypeOther;
            wordApp.ActiveDocument.MailMerge.OpenDataSource(data.DataFile, ref missing, ref missing, ref refTrue, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
               ref missing, ref missing, ref mergeType);


            //wordApp.ActiveDocument.MailMerge.OpenDataSource(data.DataFile, ref missing, ref missing, ref refTrue);
            if (lastFileProcessed.IsPopulated())
                Repeater.TryRepeatedly(() => File.Delete(lastFileProcessed));//No Need to care if this does not work it will get cleaned up in the end if it is not done here.

            doc.MailMerge.SuppressBlankLines = true;//When the merge field is blank it will move it up (Address2 is a good example)
            doc.MailMerge.Destination = Microsoft.Office.Interop.Word.WdMailMergeDestination.wdSendToNewDocument;
            doc.MailMerge.Execute(ref pause);

            object saveAs = Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), string.Format("{0}_{1}.pdf", data.RM_DSC_LTR_PRC, Guid.NewGuid().ToBase64String()));
            var result = Repeater.TryRepeatedly(() => wordApp.ActiveDocument.SaveAs(ref saveAs, ref format));
            object doNotSave = false;
            CheckRepeater(result, string.Format("Unable to save {0} for AccountNumber:{1}", saveAs, data.DF_SPE_ACC_ID), false);
            return saveAs;
        }

        private void CheckRepeater(RepeatResults<Exception> result, string message, bool throwEx = true)
        {
            if (!result.Successful)
            {
                foreach (var ex in result.CaughtExceptions)
                    ProcessLogger.AddNotification(Program.PL.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly(), ex);
                if (throwEx)
                    throw result.CaughtExceptions.First();
            }
        }

        private List<List<LT20Data>> PrepEcorrFiles(List<LT20Data> unprocessedRecords)
        {
            List<LT20Data> allFiles = unprocessedRecords;
            List<List<LT20Data>> processingGroups = new List<List<LT20Data>>();

            int take = 50;//Number of word documents that will open and process
            if (allFiles.Count < take)//if initial population is < 50 set take to 50
                take = allFiles.Count;

            for (int skip = 0; skip < allFiles.Count; skip += take)
                processingGroups.Add(allFiles.Skip(skip).Take(take).ToList());

            return processingGroups;
        }

        private void ProcessNonEcorr(List<LT20Data> unprocessedRecords)
        {
            foreach (string letter in unprocessedRecords.Select(p => p.RM_DSC_LTR_PRC.Trim()).Distinct())
            {
                List<LT20Data> data = unprocessedRecords.Where(q => q.RM_DSC_LTR_PRC.Trim() == letter).ToList();
                foreach (LT20Data letterToGenerate in data)
                {
                    if (letterToGenerate.InvalidLoanStatus)
                    {
                        DA.InactivateLetter(letterToGenerate, 2);
                        letterToGenerate.InvalidatedPrint = true;
                        continue;
                    }

                    //Update processing attempts if it hasn't been updated on this run
                    if(!letterToGenerate.ProcessingAttemptsUpdated)
                    {
                        letterToGenerate.ProcessingAttempts = (letterToGenerate.ProcessingAttempts ?? (int?)0).Value + 1;
                        DA.IncrementProcessingAttempts(letterToGenerate);
                        letterToGenerate.ProcessingAttemptsUpdated = true;
                    }

                    GenerateLetterDataFiles(letterToGenerate, letter, true);
                }

                string printingDataFile = GetPrintingFileName(letter);
                data.RemoveAll(d => d.SkipProcessingForRun);
                if (File.Exists(printingDataFile))
                {
                    var recipient = DocumentProcessing.LetterRecipient.Borrower;
                    if (data.First().Recipient == "Reference")
                        recipient = DocumentProcessing.LetterRecipient.Reference;
                    else if (data.First().Recipient == "Other")
                        recipient = DocumentProcessing.LetterRecipient.Other;
                    printingDataFile = DocumentProcessing.AddBarcodesForBatchProcessing(printingDataFile, "BarcodeAccountNumber", letter, false, recipient);
                    DocumentProcessing.UheaaCostCenterPrinting(Program.ScriptId, letter, printingDataFile, "CostCenter", "State", false, true);
                    var bwrs = data.Where(p => !p.InvalidatedPrint).ToList();
                    Parallel.ForEach(bwrs, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, borrower =>
                    {
                        DA.UpdatePrinted(borrower);
                    });

                    Repeater.TryRepeatedly(() => File.Delete(printingDataFile));
                }
            }
        }

        private string GetPrintingFileName(string letter)
        {
            return Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{0}{1}", FileNamePattern, letter));
        }

        private void GenerateLetterDataFiles(LT20Data letterToGenerate, string letter, bool massPrint)
        {
            List<LetterStoredProcedureData> sprocs = DA.GetSprocForGivenLetter(letterToGenerate.RM_DSC_LTR_PRC);

            if (!sprocs.Any() && letterToGenerate.ProcessingAttempts > 2)
            {
                PLR.AddNotification(string.Format("Letter:{0} has no sprocs assigned in BSYS.LTDB_SystemLettersStoredProcedures", letterToGenerate.RM_DSC_LTR_PRC), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                letterToGenerate.InvalidatedPrint = true;
                letterToGenerate.InvalidatedEcorr = true;
                return;
            }

            if(!sprocs.Any())
            {
                letterToGenerate.SkipProcessingForRun = true;
                PLR.AddNotification(string.Format("Letter:{0} has no sprocs assigned in BSYS.LTDB_SystemLettersStoredProcedures", letterToGenerate.RM_DSC_LTR_PRC), NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return;
            }


            if (letterToGenerate.Parameters == null)//this will not be null if the borrower was not on ecorr
                letterToGenerate.Parameters = GetLetterParameters(letterToGenerate, sprocs);

            if (letterToGenerate.Parameters == null && letterToGenerate.ProcessingAttempts > 2)
            {
                DA.InactivateLetter(letterToGenerate, 6);
                letterToGenerate.InvalidatedPrint = true;
                letterToGenerate.InvalidatedEcorr = true;
                return;//This letter has been inactivated and process logged.
            }

            if (letterToGenerate.Parameters == null)
            {
                letterToGenerate.SkipProcessingForRun = true;
                PLR.AddNotification($"Letter:{letterToGenerate.RM_DSC_LTR_PRC}, Account Number:{letterToGenerate.DF_SPE_ACC_ID} returns no results from sproc, retrying.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return;//The letter will be tried again on the next run
            }

            if (!letterToGenerate.Parameters.AddressLine.HasValidAddress && massPrint)
            {
                DA.InactivateLetter(letterToGenerate, 5);
                letterToGenerate.InvalidatedPrint = true;
                return;
            }

            GenerateFiles(letterToGenerate, letterToGenerate.Parameters, massPrint);
        }

        private void GenerateFiles(LT20Data letterToGenerate, LetterParameters parameters, bool massPrint)
        {
            string dir = Path.Combine(EnterpriseFileSystem.TempFolder, Program.ScriptId, letterToGenerate.RM_DSC_LTR_PRC);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string fileName = "";
            if (!massPrint)
                fileName = Path.Combine(dir, string.Format("{0}_{1}.txt", letterToGenerate.RM_DSC_LTR_PRC, Guid.NewGuid()));
            else
                fileName = GetPrintingFileName(letterToGenerate.RM_DSC_LTR_PRC);

            letterToGenerate.DataFile = fileName;
            bool hasHeader = File.Exists(fileName);

            using (StreamWriter sw = new StreamWriter(fileName, massPrint))
            {

                if (!hasHeader)
                    sw.WriteLine(GetLetterHeader(parameters));

                sw.WriteLine(GetDataLine(parameters, letterToGenerate.RF_SBJ_PRC));
            }

            if (!massPrint)
                DocumentProcessing.AddBarcodesForBatchProcessing(letterToGenerate.DataFile, "BarcodeAccountNumber", letterToGenerate.RM_DSC_LTR_PRC, false, DocumentProcessing.LetterRecipient.Borrower);
        }

        /// <summary>
        /// Gets the data line for a printed file.
        /// </summary>
        /// <param name="letterParams">All of the information needed to generate the letter</param>
        /// <param name="ssn">Borrowers Ssn</param>
        /// <returns>Line to be written to the data file</returns>
        public string GetDataLine(LetterParameters letterParams, string ssn)
        {

            string line = letterParams.AddressLine.ToString();
            line += "," + DocumentProcessing.ACSKeyLine(ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal); //Add the KeyLine
            line += ",MA2324";//Add in the cost center

            if (letterParams.LoanDetail != null)
                line += "," + GetLoanDetailData(letterParams.LoanDetail);

            if (letterParams.FormFields != null)
                line += GetFormFieldsData(letterParams.FormFields);

            return line;
        }

        /// <summary>
        /// Gets the loan detail data and return is as a string
        /// </summary>
        /// <param name="loanDetail"></param>
        /// <returns></returns>
        private string GetLoanDetailData(DT loanDetail)
        {
            string line = string.Empty;
            int rowCount = 1;
            foreach (DataRow row in loanDetail.Rows)
            {
                if (rowCount > 28)
                    break;
                for (int columnIndex = 0; columnIndex < loanDetail.Columns.Count; columnIndex++)
                {
                    //Sometimes a merge field needs a comma so the record will contain the symbol ¬ which is generated with altl + 170. We will replace it with a comma and surround it with "" so the merge field maintains the comma.
                    string newLine = "";
                    if (row[columnIndex].ToString().Contains("$"))
                        newLine = string.Format("\"{0:C}\",", row[columnIndex].ToString().Replace("¬", ",").Replace("\"", "").Replace("$", "").ToDecimal());
                    else
                        newLine = string.Format("\"{0}\",", row[columnIndex].ToString().Replace("¬", ",").Replace("\"", ""));

                    line += newLine;
                }
                rowCount++;
            }
            if (rowCount < 29)
                for (int rows = (rowCount * loanDetail.Columns.Count); rows < (29 * loanDetail.Columns.Count); rows++)
                    line += @",";//Insert the empty Commas

            line = line.Substring(0, line.Length - 1);//get rid of the extra ,
            return line;
        }

        /// <summary>
        /// Gets all of the form fields
        /// </summary>
        /// <param name="formFields">Form fields gathered from CDW</param>
        /// <returns>string with all of the fields values</returns>
        private string GetFormFieldsData(Dictionary<string, string> formFields)
        {
            string line = string.Empty;
            foreach (KeyValuePair<string, string> item in formFields.OrderBy(p => p.Key))
            {
                if (item.Value.Contains(@""""))
                    line += "," + item.Value;
                else
                    line += "," + @"""" + item.Value + @"""";
            }

            return line;
        }

        /// <summary>
		/// Gets the header needed for the printing data file
		/// </summary>
		/// <param name="letterParams">letter Parameters</param>
		/// <returns>string with the header in one line</returns>
		private string GetLetterHeader(LetterParameters letterParams)
        {
            string header = string.Empty;
            header += AddressHeaderLine;
            if (letterParams.LoanDetail != null)
                header += GetLoanDetailHeader(letterParams.LoanDetail);

            if (letterParams.FormFields != null)
                header += GetFormFieldsHeader(letterParams.FormFields);

            return header;
        }

        /// <summary>
        /// Gets the loan detail part of the header.
        /// </summary>
        /// <param name="loanDetail">Loan detail data</param>
        /// <returns>string with the loan detail header</returns>
        private string GetLoanDetailHeader(DT loanDetail)
        {
            List<string> baseHeader = new List<string>();
            foreach (DataColumn col in loanDetail.Columns)
                baseHeader.Add(col.ColumnName);

            string header = string.Empty;
            for (int count = 1; count < 29; count++)
                foreach (string value in baseHeader)
                    header += string.Format(",{0}{1}", value, count);

            return header;
        }

        /// <summary>
        /// Gets the form fields header
        /// </summary>
        /// <param name="formFields">Form fields data</param>
        /// <returns>string of all of the form fields headers</returns>
        private string GetFormFieldsHeader(Dictionary<string, string> formFields)
        {
            string header = string.Empty;
            foreach (KeyValuePair<string, string> item in formFields.OrderBy(p => p.Key))
                header += "," + item.Key;

            return header;
        }

        private LetterParameters GetLetterParameters(LT20Data letterToGenerate, List<LetterStoredProcedureData> sprocs)
        {
            LetterParameters parameters = new LetterParameters();
            if (!letterToGenerate.EndorsersSsn.IsNullOrEmpty())
            {
                if (letterToGenerate.EndorsersSsn.Contains("P") || letterToGenerate.EndorsersSsn.Length != 9) //handles school codes, p numbers etc
                    letterToGenerate.EndorsersAccountNumber = letterToGenerate.EndorsersSsn;
                else
                    letterToGenerate.EndorsersAccountNumber = DA.GetEndorserAccountNumber(letterToGenerate);
            }

            foreach (LetterStoredProcedureData sproc in sprocs)
            {
                if (sproc.ReturnType.ToUpper() == "ADDRESS")
                {
                    parameters.AddressLine = DA.ExecuteAddressSproc(letterToGenerate, sproc);
                    if (parameters.AddressLine == null)//this has already been process logged or it's being skipped to be processed again
                        return null;
                }
                else if (sproc.ReturnType.ToUpper() == "LOAN_DETAIL")
                {
                    parameters.LoanDetail = DA.ExecuteLoanDetailSproc(letterToGenerate, sproc);
                    if (parameters.LoanDetail == null)//this has already been process logged or it's being skipped to be processed again
                        return null;
                }
                else if (sproc.ReturnType.ToUpper() == "FORM_FIELDS")
                {
                    parameters.FormFields = DA.ExecuteFormFieldsSproc(letterToGenerate, sproc);
                    if (parameters.FormFields == null)//this has already been process logged or it's being skipped to be processed again
                        return null;
                }
                else
                {
                    string message = string.Format("An unknown stored procedure return type {0} was encountered for letter {1} on account {2}, Please review.", sproc.ReturnType, letterToGenerate.RM_DSC_LTR_PRC, letterToGenerate.DF_SPE_ACC_ID);
                    PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    DA.InactivateLetter(letterToGenerate, 6);
                    return null;
                }
            }
            return parameters;
        }

        private bool DoRecovery()
        {
            List<string> recoveryFile = GetPrintingFile();
            var filesToDelete = recoveryFile;
            foreach (string file in filesToDelete)
            {
                File.Delete(file);
            }

            if (filesToDelete.Any())
                recoveryFile = GetPrintingFile();
            if (recoveryFile.Count != 0)
            {
                if (recoveryFile.Count == 1)
                {
                    string file = recoveryFile.First();
                    string letterId = Path.GetExtension(file).Replace(".", "");
                    if (!DoCostCenterPrinting(letterId, file))
                        return false;
                }
                else
                {
                    string message = string.Format("Multiple recovery files {0} were found in {1}.  Please review and try again.", FileNamePattern, EnterpriseFileSystem.TempFolder);
                    PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
		/// Checks the printing files in the T drive and calls cost center printing common code.
		/// </summary>
		/// <param name="letterId">Letter we are processing</param>
		/// <param name="file">File to process.  This is parameter should only be used be a recovery process.</param>
		private bool DoCostCenterPrinting(string letterId, string file = null)
        {
            string actualFile = file;
            if (file.IsNullOrEmpty())
            {
                List<string> files = GetPrintingFile();
                if (files.Count == 0)//If the file is not there that should not be a problem.
                    return true;
                else if (files.Count > 1)
                {
                    string message = string.Format("There are multiple printing files {0} found while processing letter {1}.  The application will now end.", FileNamePattern, letterId);
                    PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return false;
                }

                actualFile = files.First();
            }
            actualFile = DocumentProcessing.AddBarcodesForBatchProcessing(actualFile, "BarcodeAccountNumber", letterId, false, DocumentProcessing.LetterRecipient.Borrower);
            DocumentProcessing.UheaaCostCenterPrinting(Program.ScriptId, letterId, actualFile, "CostCenter", "State", false, true);

            File.Delete(actualFile);
            return true;
        }

        /// Get any cost center printing files that have been created.
		/// </summary>
		/// <returns></returns>
		private List<string> GetPrintingFile()
        {
            return Directory.GetFiles(EnterpriseFileSystem.TempFolder, string.Format("{0}*", FileNamePattern)).ToList();
        }

        private List<LT20Data> GetRecords()
        {
            List<LT20Data> unprocessedRecords = DA.GetUnprocessedLetters();
            Parallel.ForEach(unprocessedRecords, new ParallelOptions() { MaxDegreeOfParallelism = int.MaxValue }, record =>
            {
                record.RN_SEQ_REC_PRC = DA.GetLetterRecSeq(record);
                record.RM_DSC_LTR_PRC = record.RM_DSC_LTR_PRC.Trim();
                if (record.Recipient.IsPopulated())
                    record.OnEcorr = false;
                else if (record.EndorsersSsn.IsPopulated())
                {
                    record.EndorsersAccountNumber = DA.GetEndorserAccountNumber(record);
                    EcorrData endrEcorr = EcorrProcessing.CheckEcorr(record.EndorsersAccountNumber.IsNullOrEmpty() ? record.DF_SPE_ACC_ID : record.EndorsersAccountNumber);
                    record.OnEcorr = endrEcorr.LetterIndicator && endrEcorr.ValidEmail;
                }

                Console.WriteLine("Getting letter Data for Account {0} LetterId {1}", record.DF_SPE_ACC_ID, record.RM_DSC_LTR_PRC);
            });
            return unprocessedRecords;
        }
    }
}
