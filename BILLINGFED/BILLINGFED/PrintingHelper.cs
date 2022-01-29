using System;
using System.Collections.Generic;
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
    public class PrintingHelper
    {
        private DataAccess da;
        private ProcessLogRun LogRun { get; set; }
        public PrintingHelper(DataAccess da, ProcessLogRun logRun)
        {
            this.da = da;
            LogRun = logRun;
        }

        /// <summary>
        /// Merges all the PDF's and sends them to the printer.
        /// </summary>
        public void Print(string dir, List<Borrower> borrowers)
        {
            List<Borrower> printBorrowers = borrowers.Where(p => !p.PrintedAt.HasValue && !p.OnEcorr).ToList();
            List<string> files = GetFileNamesForPrinting(printBorrowers);
            if (Directory.Exists(dir))
            {
                //Order by Account Numbers which is position 4
                files = files.OrderBy(p => p.SplitAndRemoveQuotes("_")[5]).ToList();
                List<int> printIds = new List<int>();
                if (files.Count > 0)
                {
                    int filesSent = 0;
                    List<string> filesToProcess = new List<string>();
                    List<Task> threads = new List<Task>();
                    for (int i = 0; i < files.Count(); i++)
                    {
                        filesToProcess.Add(files[i]);
                        //Grabs the Print Processing ID 
                        printIds.Add(files[i].SplitAndRemoveQuotes("_")[5].Replace(".pdf", "").ToInt());
                        if (filesToProcess.Count() == 50)
                        {
                            PrintFiles(dir, borrowers, printIds, filesToProcess, threads, true);
                            filesToProcess = new List<string>();
                            printIds = new List<int>();
                            filesSent += 1;
                            if (filesSent % 10 == 0)
                                Task.WhenAll(threads).Wait(); //Send 10 to the printer at a time.
                        }
                        else if (filesToProcess.Count > 0 && files.Count - 1 == i)
                            PrintFiles(dir, borrowers, printIds, filesToProcess, threads, false);
                    }
                    Task.WhenAll(threads).Wait();
                }
            }
        }

        /// <summary>
        /// Gets the files that need to be printed. Creates any files that are not created.
        /// </summary>
        private List<string> GetFileNamesForPrinting(List<Borrower> printBorrowers)
        {
            List<string> files = new List<string>();
            ReaderWriterLockSlim writeLock = new ReaderWriterLockSlim();
            Parallel.ForEach(printBorrowers, new ParallelOptions { MaxDegreeOfParallelism = BillingStatementsFed.MaxDegreeOfParallelism }, bor =>
            {
                int? reportNumber = bor.ParseReportNumber();
                if (reportNumber.IsIn(2, 3, 4, 5, 10, 11, 12, 13, 22, 23)) //These are the only files that get printed besides the Interest docs
                {
                    string imagingFile = bor.GetImagingFile(da);//Will create the file if it does not exist.
                    try
                    {
                        writeLock.EnterWriteLock();
                        files.Add(imagingFile);
                    }
                    catch (Exception ex)
                    {
                        string message = $"Error writing merge data to the Imaging File: {imagingFile}.";
                        LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                    }
                    finally
                    {
                        writeLock.ExitWriteLock();
                    }
                }
            });
            return files;
        }

        /// <summary>
        /// Create a PrintHelper object and call the print method. If there are less than 50 bills, don't thread them.
        /// </summary>
        /// <param name="dir">The directory where the files are</param>
        /// <param name="borrowers">All the borrowers to be processed</param>
        /// <param name="printIds">The print ID's of the borrowers being processed in the batch of 50</param>
        /// <param name="filesToProcess">The borrowers being processed in the batch of 50</param>
        /// <param name="threads">The list of threads that are being added to</param>
        /// <param name="useThread">Whether or not to add the current list to the thread.</param>
        private void PrintFiles(string dir, List<Borrower> borrowers, List<int> printIds, List<string> filesToProcess, List<Task> threads, bool useThread)
        {
            string first = filesToProcess.First().SplitAndRemoveQuotes("_")[5].Replace(".pdf", "");
            string last = filesToProcess.Last().SplitAndRemoveQuotes("_")[5].Replace(".pdf", "");
            if (useThread)
                threads.Add(Task.Factory.StartNew(() => CreateAndPrintPdf(first, last, filesToProcess, dir, borrowers, printIds, BillingStatementsFed.DA)));
            else //This is the last file and does not need to be in the thread
                CreateAndPrintPdf(first, last, filesToProcess, dir, borrowers, printIds, BillingStatementsFed.DA);
        }

        /// <summary>
        /// Merges the PDF's created for billing and send them to the printer, then updates the borrower records in the database
        /// </summary>
        /// <param name="first">The first account number found in the file</param>
        /// <param name="last">The last account number found in the file</param>
        /// <param name="filesToProcess">50 or less files that need to be merged, printed and updated</param>
        /// <param name="dir">The directory where the files exist</param>
        /// <param name="borrowers">All borrowers in billing for the day</param>
        /// <param name="printIds">The print processing id's for the borrowers in the filesToProcess list</param>
        /// <param name="Da">DataAccess object</param>
        private void CreateAndPrintPdf(string first, string last, List<string> filesToProcess, string dir, List<Borrower> borrowers, List<int> printIds, DataAccess Da)
        {
            Console.WriteLine($"Sending borrowers {first} to {last} to printer");
            string printingfile = CreateMergeFile(filesToProcess, first, last, dir);
            if (printingfile.IsNullOrEmpty())
                return;
            if (!PrintPdf(printingfile, filesToProcess, first, last, dir))
                return;
            Console.WriteLine($"Setting borrowers {first} to {last} as printed");
            List<Borrower> bors = borrowers.Where(p => p.PrintProcessingId.IsIn(printIds.ToArray())).ToList();
            foreach (Borrower bor in bors)
                if (!bor.PrintedAt.HasValue)
                    bor.SetPrintedAt();
            string printPath = EnterpriseFileSystem.GetPath("BillingPrintFiles", DataAccessHelper.CurrentRegion);
            if (!Directory.Exists(printPath))
            {
                FS.CreateDirectory(printPath);
            }
            Repeater.TryRepeatedly(() => FS.Copy(printingfile, Path.Combine(printPath, Path.GetFileName(printingfile)), true));
            Repeater.TryRepeatedly(() => FS.Delete(printingfile));
            foreach (string file in filesToProcess)
                Repeater.TryRepeatedly(() => FS.Delete(file));
        }

        /// <summary>
        /// Prints the print file.
        /// </summary>
        private bool PrintPdf(string printingfile, List<string> filesToProcess, string first, string last, string dir)
        {
            string message = $"There was an error printing the file {printingfile} containing the print processing id's between {first} and {last}";
            try
            {
                if (printingfile.IsNullOrEmpty()) //Create the file if it does not exist
                    printingfile = CreateMergeFile(filesToProcess, first, last, dir);
                DocumentProcessing.PrintPdf(printingfile); //Returns 1 if successful
            }
            catch (Exception ex)
            {
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tries to create the printing file 5 times and process logs if it can't create the file.
        /// </summary>
        private string CreateMergeFile(List<string> filesToProcess, string first, string last, string dir)
        {
            string file = "";
            int retryCount = 0;
            string message = $"There was an error creating the printing file for print processing id's {first} through {last}";
            do
            {
                try
                {
                    file = PdfHelper.MergePdfsBilling(filesToProcess, "", "", false, DataAccessHelper.Region.CornerStone, Path.Combine(dir, $"CSBilling_{first}_{last}.pdf"));
                    ++retryCount; //Try to create the document 5 times then process log it
                }
                catch (Exception ex)
                {
                    if (retryCount == 5)
                    {
                        LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                        return null;
                    }
                }
            } while ((file.IsNullOrEmpty() || !File.Exists(file)) && retryCount < 5);

            if (file.IsNullOrEmpty() || !File.Exists(file))
            {
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return null;
            }

            return file;
        }

        /// <summary>
        /// Prints the interest notices and interest statements
        /// </summary>
        public void PrintInterestFiles(InterestHelper ih, ProcessLogRun logRun)
        {
            Console.WriteLine("Printing Interest Notice");
            if (ih.PrintFileData.Any()) //The first line is a header so it has to be greater than 1
            {
                PrintInterestNotices(ih);
            }

            Console.WriteLine("Printing Interest Statement");
            if (File.Exists(ih.IntStatementFile) && FS.ReadAllLines(ih.IntStatementFile).Count() > 1) //The first line is a header so it has to be greater than 1
            {
                PrintingHelper ph = new PrintingHelper(da, logRun);
                PrintInterestStatements(ih.IntStatementFile, ih.ScriptId, ih.CoBorrowerAccountNumber, ih.Da);
                Repeater.TryRepeatedly(() => FS.Delete(ih.IntStatementFile));
            }

        }

        /// <summary>
        /// Prints the interest notices and updates the accounts in the file to printed
        /// </summary>
        /// <param name="InterestNoticeFile">Interest notice print file created during processing</param>
        /// <param name="ScriptId">Script Id</param>
        /// <param name="AccountNumber">Account Number field name in file</param>
        /// <param name="Da">DataAccess object</param>
        public void PrintInterestNotices(InterestHelper ih)
        {
            int count = ih.PrintFileData.Count;
            List<List<string>> splitFiles = new List<List<string>>();
            //If < 600 then it will take everything, if more the 600 will be broken into groups of 6
            int take = count / (count > 600 ? 6 : 1);
            for (int skip = 0; count > skip; skip += take)
            {
                List<string> fileToAdd = new List<string>() { InterestHelper.GetInterestNoticeHeader() };
                fileToAdd.AddRange((ih.PrintFileData.Skip(skip).Take(take).ToList()));
                splitFiles.Add(fileToAdd);
            }

            Parallel.ForEach(splitFiles, new ParallelOptions() { MaxDegreeOfParallelism = int.MaxValue }, printFile =>
            {
                List<int> processingIds = new List<int>();
                processingIds.AddRange(printFile.Select(p => p.SplitAndRemoveQuotes(",")[0].ToInt()));
                string printingFile = Path.Combine(EnterpriseFileSystem.TempFolder, $"InterestNotices_{Guid.NewGuid().ToBase64String()}.txt");
                FS.WriteAllLines(printingFile, printFile.ToArray());
                DocumentProcessing.CostCenterPrinting("INNOSCHFED", printingFile, "State", ih.ScriptId, ih.AccountNumber, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, false, true);
                foreach (int id in processingIds)
                    ih.Da.SetPrintedAt(id);
            });
        }

        /// <summary>
        /// Prints the interest statements and updates the accounts found in the file
        /// </summary>
        /// <param name="InterestStatementFile">Interest Statement print file created during processing</param>
        /// <param name="ScriptId">Script Id</param>
        /// <param name="AccountNumber">Account Number field name in the file</param>
        /// <param name="Da">DataAccess object</param>
        public void PrintInterestStatements(string InterestStatementFile, string ScriptId, string AccountNumber, DataAccess Da)
        {
            List<int> processingIds = new List<int>();
            using (StreamR sr = new StreamR(InterestStatementFile))
            {
                string header = sr.ReadLine();
                while (!sr.EndOfStream)
                    processingIds.Add(sr.ReadLine().SplitAndRemoveQuotes(",")[0].ToInt());
            }
            DocumentProcessing.CostCenterPrinting("INTBILFED", InterestStatementFile, "State", ScriptId, AccountNumber, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, false, true);
            foreach (int id in processingIds)
                Da.SetPrintedAt(id);
        }
    }
}