using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace BILLINGFED
{
    class GeneratePdfs
    {
        public static bool GenerateAndImagePdf(Borrower bor, EcorrData ecorrInfo, bool doEcorr, string ScriptId, string UserId, int reportNumber, string letterId, string directory, ProcessLogData logData)
        {
            bool didProcess = true;
            if (doEcorr)
            {
                if (!bor.DocumentCreatedAt.HasValue)
                {
                    GenerateEcorr(bor, UserId, ecorrInfo, reportNumber, true);
                    BillingStatementsFed.Da.SetPrinted(bor.PrintProcessingId);
                }
                if (!bor.ImagedAt.HasValue)
                    GeneratePrinted(bor, ScriptId, reportNumber, letterId, directory, logData, 0, true, false); //This will do the imaging piece
            }
            else
            {
                List<Task> tasks = new List<Task>();
                if (!bor.DocumentCreatedAt.HasValue)
                {
                    if (ecorrInfo != null)
                        tasks.Add(new TaskFactory().StartNew(() => GenerateEcorr(bor, UserId, ecorrInfo, reportNumber, false), TaskCreationOptions.LongRunning));
                    tasks.Add(new TaskFactory().StartNew(() => didProcess = GeneratePrinted(bor, ScriptId, reportNumber, letterId, directory, logData), TaskCreationOptions.LongRunning));

                    Task.WhenAll(tasks).Wait();
                    bor.SetDocumentCreated();
                }
                else if (bor.DocumentCreatedAt.HasValue && !bor.ImagedAt.HasValue)
                    GeneratePrinted(bor, ScriptId, reportNumber, letterId, EnterpriseFileSystem.TempFolder, logData, 0, false, false);
            }
            return didProcess;
        }

        private static bool GeneratePrinted(Borrower data, string ScriptId, int reportNumber, string letterId, string directory, ProcessLogData logData, int retryCount = 0, bool isOnEcorr = false, bool doPrint = true)
        {
            List<BorrowerData> bData = BorrowerData.GetDataFromLine(data.PrintProcessingId, data.SourceFile, letterId, data.AccountNumber, reportNumber);
            string imagingFile = string.Format("{0}{1}_{2}_{3}.pdf", isOnEcorr ? EnterpriseFileSystem.TempFolder : directory + @"\", ScriptId, data.AccountNumber, Guid.NewGuid().ToBase64String());

            int take = 12;
            for (int skip = 0; skip < bData.Count; skip += 12)
            {
                try
                {
                    GenerateCrytalReport(data, logData, isOnEcorr, bData, imagingFile, take, skip);
                    ImageFile(data, imagingFile, isOnEcorr, logData);
                    if (doPrint)
                        BillingStatementsFed.AddToCoverSheetFile(bData.First().StateCode);
                }
                catch (InvalidNumberOfPagesInAPdfException ex)
                {
                    if (retryCount == 5)
                    {
                        string message = string.Format("Unable to generate printed bill for the following borrower {0}, PrintProcessingId {1}", data.AccountNumber, data.PrintProcessingId);
                        ProcessLogger.AddNotification(logData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, logData.ExecutingAssembly, ex);
                        break;
                    }
                    string consoleMessage = string.Format("Invalid number of pages, trying again.  Try Count: {0}", retryCount);
                    Console.WriteLine(consoleMessage);
                    Thread.Sleep(5000);
                    retryCount++;
                    GeneratePrinted(data, ScriptId, reportNumber, letterId, directory, logData, retryCount);
                }
                catch (CrystalDecisions.Shared.CrystalReportsException ex)
                {
                    if (ex.InnerException.Message == "Not enough memory for operation.")
                    {
                        string message = string.Format("{0}; recursing; {1}; count: {2}", ex.InnerException.Message, data.AccountNumber, skip);
                        Console.WriteLine(message);
                        Thread.Sleep(10000);
                        GeneratePrinted(data, ScriptId, reportNumber, letterId, directory, logData);
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Format("Unable to generate printed bill for the following borrower {0}, PrintProcessingId {1}", data.AccountNumber, data.PrintProcessingId);
                    ProcessLogger.AddNotification(logData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, logData.ExecutingAssembly, ex);
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Generates the report using Crystal Reports
        /// </summary>
        private static void GenerateCrytalReport(Borrower data, ProcessLogData logData, bool isOnEcorr, List<BorrowerData> bData, string imagingFile, int take, int skip)
        {
            using (BillingStatement report = new BillingStatement())
            {
                report.SetDataSource(bData.Skip(skip).Take(take));
                report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, imagingFile);
                report.Dispose(); //Attempting to avoid Not Enough Memoery for Operation error.
            }

            int numberOfPages = PdfHelper.GetNumberOfPagesInPdf(imagingFile);
            if (numberOfPages != 2)
            {
                Repeater.TryRepeatedly(() => File.Delete(imagingFile));
                throw new InvalidNumberOfPagesInAPdfException(string.Format("Expecting 2 pages in the pdf found {0}", numberOfPages));
            }
        }

        /// <summary>
        /// Images the file
        /// </summary>
        private static void ImageFile(Borrower data, string imagingFile, bool isOnEcorr, ProcessLogData logData)
        {
            try
            {
                if (!data.ImagedAt.HasValue)
                {
                    DocumentProcessing.ImageFile(imagingFile, BillingStatementsFed.DocId, data.Ssn);
                    data.SetImagedAt();
                }
                if (isOnEcorr)
                    Repeater.TryRepeatedly(() => File.Delete(imagingFile));
            }
            catch (Exception ex)
            {
                string message = string.Format("Error imaging bill for account: {0};", data.AccountNumber);
                ProcessLogger.AddNotification(logData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, logData.ExecutingAssembly, ex);
            }
        }

        private static string GenerateEcorr(Borrower data, string UserId, EcorrData ecorrInfo, int reportNumber, bool isEcorr, int retryCount = 0)
        {
            string ecorrDoc = "";
            try
            {
                List<string> fields = data.LineData.First().SplitAndRemoveQuotes(",");
                DateTime dueDate = fields[24].ToDate();
                string totalDue = (fields[25].ToDecimal() + fields[26].ToDecimal()).ToString();
                string billSeq = fields[31];
                DateTime billCreateDate = fields[19].ToDate();

                //Borrower is on Ecorr
                DataTable loanDetail = GetLoanDetail(data.LineData);
                Dictionary<string, string> formFields = GetFormFields(data.LineData, reportNumber);
                ecorrDoc = PdfHelper.GenerateEcorrBill(Path.Combine(EnterpriseFileSystem.GetPath("Correspondence"), "EBILLFED.pdf"), data.AccountNumber, data.Ssn, isEcorr ? DocumentProperties.CorrMethod.EmailNotify : DocumentProperties.CorrMethod.Printed,
                    UserId, ecorrInfo, loanDetail, formFields, dueDate, totalDue, billSeq, billCreateDate);
                data.SetDocumentCreated();
            }
            catch (OutOfMemoryException ex)
            {
                if (retryCount == 5)
                    throw;
                //GC.Collect(); //Attempting to manually garbage collect to stop running out of memory.
                Thread.Sleep(60000);
                string message = string.Format("{0}; recursing; {1}", ex.InnerException.Message, data.AccountNumber);
                Console.WriteLine(message);
                GenerateEcorr(data, UserId, ecorrInfo, reportNumber, isEcorr, ++retryCount);
            }
            return ecorrDoc;
        }

        private static Dictionary<string, string> GetFormFields(List<string> dataLines, int reportNumber)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>();
            List<string> oneLine = dataLines.First().SplitAndRemoveQuotes(",");

            formFields.Add("NAME", string.Format("{0} {1}", oneLine[3], oneLine[5]));//Borrowers Name
            formFields.Add("ACCOUNTNUMBER", oneLine[2]);//Borrowers Account Number
            formFields.Add("BILLDATE", oneLine[19]);
            formFields.Add("DATEDUE", oneLine[24]);
            formFields.Add("DATERECEIVED", oneLine[20]);
            formFields.Add("AMOUNTTOPRINCIPAL", string.Format("$ {0:0.00}", dataLines.Sum(p => p.SplitAndRemoveQuotes(",")[21].ToDecimal())));
            formFields.Add("AMOUNTTOINTEREST", string.Format("$ {0:0.00}", dataLines.Sum(p => p.SplitAndRemoveQuotes(",")[22].ToDecimal())));
            formFields.Add("AMOUNTPAID", string.Format("$ {0:0.00}", dataLines.Sum(p => p.SplitAndRemoveQuotes(",")[23].ToDecimal())));
            formFields.Add("AMOUNTPASTDUE", string.Format("$ {0:0.00}", oneLine[25].ToDecimal()));
            formFields.Add("MONTHLYINSTALL", string.Format("$ {0:0.00}", oneLine[35].ToDecimal()));
            formFields.Add("TOTALAMOUNTDUE", string.Format("$ {0:0.00}", oneLine[26].ToDecimal()));
            formFields.Add("DUEDATE", oneLine[24]);
            formFields.Add("ACCOUNTNUMBER1", oneLine[2]);
            formFields.Add("AMOUNTPASTDUE1", string.Format("$ {0:0.00}", oneLine[25].ToDecimal()));
            formFields.Add("MONTHLYINSTALL1", string.Format("$ {0:0.00}", oneLine[35].ToDecimal()));
            formFields.Add("TOTALAMOUNTDUE1", string.Format("$ {0:0.00}", oneLine[26].ToDecimal()));
            formFields.Add("DUEBY", oneLine[24]);
            BillText bill = BillingStatementsFed.Da.GetTextForBill(reportNumber);
            formFields.Add("SPECIALMESSAGETITLE", bill.FirstSpecialMessageTitle);
            formFields.Add("SPECIALMESSAGE", bill.FirstSpecialMessageBody.IsPopulated() ? string.Format(bill.FirstSpecialMessageBody, oneLine[18]) : "");
            formFields.Add("SPECIALMESSAGETITLE2", bill.SecondSpecialMessageTitle);
            formFields.Add("SPECIALMESSAGE2", bill.SecondSpecialMessageBody);
            formFields.Add("TOTALPRINCIPALPAID", string.Format("$ {0:0.00}", (dataLines.Sum(s => s.SplitAndRemoveQuotes(",")[32].ToDecimal()))));
            formFields.Add("TOTALINTERESTPAID", string.Format("$ {0:0.00}", (dataLines.Sum(s => s.SplitAndRemoveQuotes(",")[33].ToDecimal()))));
            formFields.Add("TOTALAMOUNTPAID", string.Format("$ {0:0.00}", (dataLines.Sum(s => s.SplitAndRemoveQuotes(",")[34].ToDecimal()))));

            return formFields;
        }

        private static DataTable GetLoanDetail(List<string> dataLines)
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
            });

            foreach (string line in dataLines)
            {
                List<string> fields = line.SplitAndRemoveQuotes(",");
                string[] dataRow = new string[]
                {
                    fields[12], //Loan Program
                    fields[14].ToDate().ToString("MM/dd/yyyy"), //First Disbursed
                    string.Format("{0} %", fields[15]), //Interest rate
                    string.Format("$ {0:0.00}", fields[16].ToDecimal()), //Original Principal
                    string.Format("$ {0:0.00}", fields[17].ToDecimal()), //Current Balance
                    string.Format("$ {0:0.00}", fields[32].ToDecimal()), //Total principal paid
                    string.Format("$ {0:0.00}", fields[33].ToDecimal()), //Total interest paid
                    string.Format("$ {0:0.00}", fields[34].ToDecimal()) //Total amount paid
                };

                dt.Rows.Add(dataRow);
            }
            return dt;
        }
    }
}