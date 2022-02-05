using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ECORRSLFED
{
    class LetterGenerator
    {
        private ProcessLogRun PLR { get; set; }
        private const string FileNamePattern = "SystemLetterGeneratorFed.";
        private DataAccess DA { get; set; }

        private string AddressHeaderLine
        {
            get
            {
                return "Name,Address1,Address2,City,State,Zip,Country,ForeignState,AccountNumber,BarcodeAccountNumber,KeyLine,CostCenter";
            }
        }

        public LetterGenerator(ProcessLogRun plr)
        {
            PLR = plr;
            DA = new DataAccess(PLR);
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


            if (!DA.InactivateInvalidLetters())
                return 1;

            Console.WriteLine("Adding coborrower records to database for currently unprocessed borrower records.");
            int rowsAdded = DA.AddCoborrowerRecords();
            int condensedRowsAdded = DA.AddCoborrowerRecordsCondensed();
            Console.WriteLine(string.Format("Added {0} letter records to account for coborrower processing.", rowsAdded + condensedRowsAdded));

            Console.WriteLine("Getting unprocessed records.  This can take a minute.");
            List<LT20Data> unprocessedRecords = GetRecords();
            
            ProcessNonEcorr(unprocessedRecords.Where(p => (!p.OnEcorr || p.Recipient.IsPopulated()) && !p.PrintedAt.HasValue).ToList());
            ProcessEcorr(unprocessedRecords.Where(p => !p.EcorrDocumentCreatedAt.HasValue && p.Recipient.IsNullOrEmpty()).ToList());//We generate an ecorr doc for all borrower regardless of whether they are one ecorr or not.

            return 0;
        }

        private void ProcessEcorr(List<LT20Data> unprocessedRecords)
        {
            foreach (string letter in unprocessedRecords.Select(p => p.RM_DSC_LTR_PRC.Trim()).Distinct())
            {
                ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
                List<LT20Data> data = unprocessedRecords.Where(q => q.RM_DSC_LTR_PRC.Trim() == letter).ToList();
                Parallel.ForEach(data, new ParallelOptions() { MaxDegreeOfParallelism = Program.NumberOfThreads }, letterToGenerate =>
               {
                   Console.WriteLine("Processing Borrower:{0}", letterToGenerate.DF_SPE_ACC_ID);
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

                   try
                   {
                       locker.EnterWriteLock();
                       GenerateLetterDataFiles(letterToGenerate, letter, false, "", true);
                       locker.ExitWriteLock();
                       if (!letterToGenerate.InvalidatedEcorr && !letterToGenerate.SkipProcessingForRun)
                       {
                           string templatePath = EcorrProcessing.GetEcorrTemplate(letterToGenerate.RM_DSC_LTR_PRC);
                           EcorrData ecorrData = EcorrProcessing.CheckEcorr(letterToGenerate.EndorsersAccountNumber.IsNullOrEmpty() ? letterToGenerate.DF_SPE_ACC_ID : letterToGenerate.EndorsersAccountNumber);
                           bool coBorrower = letterToGenerate.CoborrowerSSN.IsPopulated();
                           string ssn;
                           string accountNumberXML = letterToGenerate.DF_SPE_ACC_ID;
                           if (coBorrower)
                               ssn = letterToGenerate.CoborrowerSSN;
                           else if (letterToGenerate.EndorsersSsn.IsPopulated())
                           {
                               ssn = letterToGenerate.RF_SBJ_PRC;
                               accountNumberXML = letterToGenerate.EndorsersAccountNumber;
                               letterToGenerate.LetterAccountNumber = letterToGenerate.EndorsersAccountNumber;
                           }
                           
                           else
                               ssn = letterToGenerate.RF_SBJ_PRC;

                           if (letterToGenerate.OnEcorr) //On Ecorr
                               PdfHelper.GenerateEcorrPdf(templatePath, accountNumberXML, ssn, DocumentProperties.CorrMethod.EmailNotify,
                                  "UT00801", ecorrData, letterToGenerate.Parameters.AddressLine.ToEcorrList(), letterToGenerate.LetterAccountNumber, letterToGenerate.Parameters.LoanDetail, letterToGenerate.Parameters.FormFields, null, null);
                           else
                               PdfHelper.GenerateEcorrPdf(templatePath, accountNumberXML, ssn, DocumentProperties.CorrMethod.Printed,
                                    "UT00801", ecorrData, letterToGenerate.Parameters.AddressLine.ToEcorrList(), letterToGenerate.LetterAccountNumber, letterToGenerate.Parameters.LoanDetail, letterToGenerate.Parameters.FormFields, null, null);
                       }
                   }
                   catch (Exception ex)
                   {
                       if (locker.IsWriteLockHeld)
                           locker.ExitWriteLock();
                       string message = string.Format("Unable to generate Letter:{0} for Borrower:{1}", letterToGenerate.RM_DSC_LTR_PRC, letterToGenerate.DF_SPE_ACC_ID);
                       Console.WriteLine(message);
                       Program.PL.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                       return; 
                   }
                   if (!letterToGenerate.InvalidatedEcorr && !letterToGenerate.SkipProcessingForRun)
                   {
                       locker.EnterWriteLock();
                       DA.UpdateEcorrDocCreated(letterToGenerate);
                       locker.ExitWriteLock();
                   }
               });
            }
        }

        private string GetPrintingFileName(string letter, string guid)
        {
            return Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{0}{1}_{2}.txt", FileNamePattern, letter, guid));
        }

        private void ProcessNonEcorr(List<LT20Data> unprocessedRecords)
        {
            foreach (string letter in unprocessedRecords.Select(p => p.RM_DSC_LTR_PRC.Trim()).Distinct())
            {
                string guid = Guid.NewGuid().ToBase64String();
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
                    if (!letterToGenerate.ProcessingAttemptsUpdated)
                    {
                        letterToGenerate.ProcessingAttempts = (letterToGenerate.ProcessingAttempts ?? (int?)0).Value + 1;
                        DA.IncrementProcessingAttempts(letterToGenerate);
                        letterToGenerate.ProcessingAttemptsUpdated = true;
                    }

                    GenerateLetterDataFiles(letterToGenerate, letter, true, guid);
                }

                data.RemoveAll(d => d.SkipProcessingForRun);
                string printingDataFile = GetPrintingFileName(letter, guid);
                if (File.Exists(printingDataFile))
                {
                    Console.WriteLine("Printing {0}", letter);
                    var recipient = DocumentProcessing.LetterRecipient.Borrower;
                    if (data.First().Recipient == "Reference")
                        recipient = DocumentProcessing.LetterRecipient.Reference;
                    else if (data.First().Recipient == "Other")
                        recipient = DocumentProcessing.LetterRecipient.Other;
                    printingDataFile = DocumentProcessing.AddBarcodesForBatchProcessing(printingDataFile, "BarcodeAccountNumber", letter, false, recipient);
                    DocumentProcessing.CostCenterPrinting(letter, printingDataFile, "State", Program.ScriptId, "AccountNumber", recipient, DocumentProcessing.CostCenterOptions.None, false, "CostCenter");
                    var bwrs = data.Where(p => !p.InvalidatedPrint).ToList();
                    Parallel.ForEach(bwrs, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, borrower =>
                    {
                        DA.UpdatePrinted(borrower);
                    });

                    Repeater.TryRepeatedly(() => File.Delete(printingDataFile));
                }
            }
        }

        private void GenerateLetterDataFiles(LT20Data letterToGenerate, string letter, bool notOnEcorr, string guid, bool ecorrProcessing = false)
        {
            List<LetterStoredProcedureData> sprocs = DA.GetSprocForGivenLetter(letterToGenerate.RM_DSC_LTR_PRC);

            if (!sprocs.Any() && letterToGenerate.ProcessingAttempts > 2)
            {
                PLR.AddNotification(string.Format("Letter:{0} has no sprocs assigned in BSYS.LTDB_SystemLettersStoredProcedures", letterToGenerate.RM_DSC_LTR_PRC), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                letterToGenerate.InvalidatedPrint = true;
                letterToGenerate.InvalidatedEcorr = true;
                return;
            }

            if (!sprocs.Any())
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
                if(!letterToGenerate.SkipProcessingForRun)
                    PLR.AddNotification($"Letter:{letterToGenerate.RM_DSC_LTR_PRC}, Account Number:{letterToGenerate.DF_SPE_ACC_ID} returns no results from sproc, retrying.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                letterToGenerate.SkipProcessingForRun = true;
                return;//The letter will be tried again on the next run
            }

            if (!letterToGenerate.Parameters.AddressLine.HasValidAddress && notOnEcorr)
            {
                DA.InactivateLetter(letterToGenerate, 5);
                letterToGenerate.InvalidatedPrint = true;
                return;
            }
            if (!ecorrProcessing)
                GenerateFiles(letterToGenerate, letterToGenerate.Parameters, guid);

            return;
        }

        private string GenerateFiles(LT20Data letterToGenerate, LetterParameters parameters, string guid)
        {
            string fileName = GetPrintingFileName(letterToGenerate.RM_DSC_LTR_PRC, guid);
            letterToGenerate.DataFile = fileName;
            bool needsHeader = !File.Exists(fileName);
            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                if (needsHeader)
                    sw.WriteLine(GetLetterHeader(parameters));
                sw.WriteLine(GetDataLine(parameters, letterToGenerate.RF_SBJ_PRC));
            }

            return fileName;
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
                    //Update the ecorr flag of endoresers on Ecorr for endorser letters in the borrower table
                    if (!record.IsCoborrower)
                    {
                        DA.UpdateEndorserEcorrOnRecord(record);
                    }
                }

                Console.WriteLine("Getting letter Data for Account {0} LetterId {1}", record.DF_SPE_ACC_ID, record.RM_DSC_LTR_PRC);
            });
            return unprocessedRecords;
        }

        /// <summary>
        /// Gets the data line for a printed file.
        /// </summary>
        /// <param name="letterParams">All of the information needed to generate the letter</param>
        /// <param name="ssn">Borrowers Ssn</param>
        /// <returns>Line to be written to the data file</returns>
        private string GetDataLine(LetterParameters letterParams, string ssn)
        {
            List<string> dataLines = new List<string>();
            dataLines.Add(letterParams.AddressLine.ToString()); //address
            dataLines.Add(DocumentProcessing.ACSKeyLine(ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal)); //keyline
            dataLines.Add("MA4481"); //cost center
            if (letterParams.LoanDetail != null)
                dataLines.Add(GetLoanDetailData(letterParams.LoanDetail));

            if (letterParams.FormFields != null)
                dataLines.Add(GetFormFieldsData(letterParams.FormFields));

            return string.Join(",", dataLines);
        }

        /// <summary>
        /// Gets all of the form fields
        /// </summary>
        /// <param name="formFields">Form fields gathered from CDW</param>
        /// <returns>string with all of the fields values</returns>
        private string GetFormFieldsData(Dictionary<string, string> formFields)
        {
            return string.Join(",", formFields.OrderBy(p => p.Key).Select(q => q.Value).ToArray());
        }

        /// <summary>
        /// Gets the loan detail data and return is as a string
        /// </summary>
        /// <param name="loanDetail"></param>
        /// <returns></returns>
        private string GetLoanDetailData(DataTable loanDetail)
        {
            string line = string.Empty;

            int rowCount = 1;
            foreach (DataRow row in loanDetail.Rows)
            {
                if (rowCount > 30)
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

            //Insert the empty Commas
            if (loanDetail.Rows.Count < 30)//Insert the empty Commas
            {
                for (int rows = (loanDetail.Rows.Count * loanDetail.Columns.Count) + 1; rows < (30 * loanDetail.Columns.Count); rows++)
                    line += @",";
            }
            if (rowCount > 30)
                line = line.Remove(line.Length - 1, 1);
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
            {
                header += GetLoanDetailHeader(letterParams.LoanDetail);
            }

            if (letterParams.FormFields != null)
            {
                if (!header.EndsWith(","))
                    header += ",";
                header += GetFormFieldsHeader(letterParams.FormFields);
            }

            return header;
        }

        /// <summary>
        /// Gets the loan detail part of the header.
        /// </summary>
        /// <param name="loanDetail">Loan detail data</param>
        /// <returns>string with the loan detail header</returns>
        private string GetLoanDetailHeader(DataTable loanDetail)
        {
            List<string> baseHeader = new List<string>();
            foreach (DataColumn col in loanDetail.Columns)
                baseHeader.Add(col.ColumnName);

            string header = string.Empty;
            for (int count = 1; count < 31; count++)
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
            return string.Join(",", formFields.OrderBy(p => p.Key).Select(q => q.Key).ToArray());
        }
    }
}
