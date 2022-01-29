using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;



namespace BILLINGFED
{
    public class BillingStatementsFed
    {
        #region Variables
        public const string LETTER_ID = "BILSTFED";
        public static ProcessLogRun LogRun { get; set; }
        public static string InterestNoticeDataFile { get; set; }
        public static string InterestStatementFile { get; set; }
        public static string InterestStatementDataFile { get; set; }
        private static ReaderWriterLockSlim InterestStatementLock { get; set; }
        public static ReaderWriterLockSlim IntStatementImagingLock { get; set; }
        private static ReaderWriterLockSlim InterestNoticeLock { get; set; }
        public static ReaderWriterLockSlim IntNoticeImagingLock { get; set; }
        public static string ScriptId
        {
            get
            {
                return "BILLINGFED";
            }
        }
        public static string DocId
        {
            get
            {
                return "CRBIL";
            }
        }
        public static string AccountNumber
        {
            get
            {
                return "AccountNumber";
            }
        }
        public static string CoBorrowerAccountNumber
        {
            get
            {
                return "CoBorrowerAccountNumber";
            }
        }
        private static int Counter { get; set; }
        public static string BillingDirectory { get; set; }
        public static DataAccess DA { get; set; }
        public static int MaxDegreeOfParallelism { get; set; }
        public static InterestHelper IH { get; set; }
        public static Dictionary<int, BillText> BillTextValues { get; set; }
        #endregion

        public BillingStatementsFed()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true, true);

            List<Process> adProcess = Process.GetProcesses().ToList();
            adProcess = adProcess.Where(p => p.ProcessName.Contains(ScriptId)).ToList();

            if (adProcess.Count() > 1)
            {
                LogRun.AddNotification("BILLINGFED is already running running.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                Environment.Exit(0);
            }

            DA = new DataAccess(LogRun);
            InterestNoticeDataFile = Path.Combine(EnterpriseFileSystem.TempFolder, "IntNoticeDataFile.txt");
            InterestStatementFile = Path.Combine(EnterpriseFileSystem.TempFolder, "InterestStatements.txt");
            InterestStatementDataFile = Path.Combine(EnterpriseFileSystem.TempFolder, "IntStatementDataFile.txt");
            IntializeLocks();
            IH = SetupIntHelper();
            PreparePrintingFiles();
        }

        public static int Main(string[] args)
        {
            bool showPrompts = (args.Any() && args.Count() > 2 && args[2].ToUpper() == "SHOWPROMPTS");
            if (!DataAccessHelper.StandardArgsCheck(args, "Billing Fed"))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return 1;
            List<Process> adProcess = Process.GetProcesses().ToList();
            adProcess = adProcess.Where(p => p.ProcessName.Contains(ScriptId)).ToList();

            if (adProcess.Count() > 1)
            {
                ReportAndEnd(showPrompts);
                return 0;
            }



            MaxDegreeOfParallelism = args.Skip(1).FirstOrDefault().ToIntNullable() ?? 50;
            if (MaxDegreeOfParallelism == 0)
                MaxDegreeOfParallelism = int.MaxValue;

            return new BillingStatementsFed().Run();
        }

        private static void ReportAndEnd(bool showPrompts)
        {
            string message = "BILLINGFED is already running running.";
            if (LogRun == null)
            {
                //Process log doesn't exist if we hit this error case sometime
                LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true, true);
            }

            LogRun.AddNotification(message, NotificationType.Other, NotificationSeverityType.Informational);
            if (showPrompts)
                Dialog.Error.Ok(message);
            else
                Console.WriteLine(message);

            LogRun.LogEnd();
        }

        /// <summary>
        /// Loads all the BillText for each report number
        /// </summary>
        private void LoadBillText()
        {
            BillTextValues = new Dictionary<int, BillText>();
            for (int i = 2; i <= 23; i++)
            {
                if (i.IsIn(2, 3, 4, 5, 6, 10, 11, 12, 13, 15, 16, 17, 18, 19, 20, 21, 22, 23))
                {
                    var billTextGet = DA.GetTextForBill(i);
                    BillTextValues.Add(i, billTextGet);
                }
            }
        }

        /// <summary>
        /// Initializes the ReaderWriterLockSlim's
        /// </summary>
        private static void IntializeLocks()
        {
            InterestNoticeLock = new ReaderWriterLockSlim();
            IntNoticeImagingLock = new ReaderWriterLockSlim();

            InterestStatementLock = new ReaderWriterLockSlim();
            IntStatementImagingLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// Sets all the properties in the InterestHelper class to the properties in the main class
        /// </summary>
        /// <returns>New instance of InterestHelper</returns>
        private InterestHelper SetupIntHelper()
        {
            return new InterestHelper(LogRun)
            {
                IntStatementLock = InterestStatementLock,
                IntStatementImagingLock = IntStatementImagingLock,
                IntNoticeLock = InterestNoticeLock,
                IntNoticeImagingLock = IntNoticeImagingLock,
                IntStatementFile = InterestStatementFile,
                IntStatementDataFile = InterestStatementDataFile,
                IntNoticeDataFile = InterestNoticeDataFile,
                AccountNumber = AccountNumber,
                CoBorrowerAccountNumber = CoBorrowerAccountNumber,
                ScriptId = ScriptId,
                Da = DA
            };
        }

        /// <summary>
        /// Loads the data from the SAS files to the database
        /// Gets all borrower data from the database and loops through each borrower
        /// </summary>
        public int Run()
        {
            string dirPath = EnterpriseFileSystem.GetPath("BillingDirectory");
            BillingDirectory = CheckRecoveryDirectories(BillingDirectory, dirPath);
            if (BillingDirectory.IsNullOrEmpty())
                return 1;
            
            Console.WriteLine("Building data objects. May take several minutes.");
            LoadBillText();
            FS.CreateDirectory(BillingDirectory);
            var borrowers = DA.GetUnprocessedBorrowerData(ScriptId).OrderBy(p => p.PrintProcessingId).ToList();
            var printBorrowers = borrowers.Where(p => p.OnEcorr == false).OrderBy(p => p.PrintProcessingId).ToList();
            var nonPrintBorrowers = borrowers.Where(p => p.OnEcorr == true).OrderBy(p => p.PrintProcessingId).ToList();
            if (!borrowers.Any())
            {
                Console.WriteLine("There are no borrowers in the database to process");
                DeleteFiles(BillingDirectory);
                Thread.Sleep(5000);
                return 0;
            }
            try
            {
                Console.WriteLine("Imaging and creating print files for borrower not on Ecorr");
                ProcessBorrowers(printBorrowers, "Image");  //Print and image people not on ecorr first
                Console.WriteLine("Imaging complete, ready to print");
                PrintingHelper ph = new PrintingHelper(DA, LogRun);
                ph.Print(BillingDirectory, printBorrowers);
                ph.PrintInterestFiles(IH, LogRun);
                Console.WriteLine("Printing complete, ready to image remaining borrowers");
                Console.WriteLine("Imaging borrowers on Ecorr");
                ProcessBorrowers(nonPrintBorrowers, "Image"); //Imaging for people on Ecorr
                Console.WriteLine("Running Ecorr process for all borrowers");
                ProcessBorrowers(borrowers, "Ecorr"); //Ecorr for all borrowers

                DeleteFiles(BillingDirectory);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification(ex.ToString(), NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
            finally
            {
                LogScriptEnd();
                CloseConnections();
            }
            return 0;
        }

        /// <summary>
        /// Closes and disposes the connections
        /// </summary>
        private void CloseConnections()
        {
            DA.CdwConn.Close();
            DA.CdwConn.Dispose();

            DA.ClsConn.Close();
            DA.ClsConn.Dispose();

            DA.EcorrConn.Close();
            DA.EcorrConn.Dispose();
        }

        /// <summary>
        /// Process all the borrowers on Ecorr
        /// </summary>
        /// <param name="borrowers"></param>
        private void ProcessBorrowers(List<Borrower> borrowers, string step)
        {
            Counter = 0;
            Parallel.ForEach(borrowers, new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, bor =>
            {
                try
                {
                    bor.LineData = DA.GetLineData(bor.PrintProcessingId);
                    bor.CoBorrowerAccountNumber = bor.LineData[0].SplitAndRemoveQuotes(",")[2];
                    bor.IsEndorser = bor.LineData[0].SplitAndRemoveQuotes(",")[27].IsPopulated() ? bor.LineData[0].SplitAndRemoveQuotes(",")[27].IsPopulated() : false;
                    if (bor.IsEndorser)
                        bor.CoBorrowerAccountNumber = bor.LineData[0].SplitAndRemoveQuotes(",")[27];
                    EcorrData ecorrInfo = EcorrProcessing.CheckEcorr(bor.AccountNumber, DA.CdwConn, DA.EcorrConn);
                    if (!bor.ImagedAt.HasValue || !bor.ArcAddedAt.HasValue || !bor.PrintedAt.HasValue || !bor.EcorrDocumentCreatedAt.HasValue)
                        Console.WriteLine($"{Counter++}; Processing borrower: {bor.AccountNumber}; Ecorr: {bor.OnEcorr}");
                    int? reportNumber = bor.ParseReportNumber();
                    switch (step)
                    {
                        case "Image":
                            if (!reportNumber.Value.IsIn(9, 15, 16, 17, 18, 19))
                                new GeneratePdfs().ImageFile(new PrintData(bor, ecorrInfo, ScriptId, "UT00801", reportNumber.Value, LETTER_ID, BillingDirectory, LogRun), DA, LogRun);
                            else if (reportNumber.Value.IsIn(16, 17))
                                new InterestStatement().ProcessInterestStatement(bor, LogRun, reportNumber.Value == 17); //This process will do the Print/Image/Ecorr
                            else if (reportNumber.Value.IsIn(18, 19))
                            {
                                bor.IsEndorser = bor.LineData[0].SplitAndRemoveQuotes(",").Count > 19 ? bor.LineData[0].SplitAndRemoveQuotes(",")[19].IsPopulated() : false;
                                if (bor.IsEndorser)
                                    bor.CoBorrowerAccountNumber = bor.LineData[0].SplitAndRemoveQuotes(",")[19];
                                new InterestNotice().ProcessInterestNotice(bor, LogRun); //This process will do the Print/Image/Ecorr
                            }
                            break;
                        case "Ecorr":
                            if (!reportNumber.Value.IsIn(9, 15, 16, 17, 18, 19))
                                new GeneratePdfs().GenerateEcorr(bor, "UT00801", ecorrInfo, reportNumber.Value, DA, LogRun);
                            break;
                    }

                    if (!bor.ArcAddedAt.HasValue)
                        AddComment(bor, reportNumber.Value, reportNumber.Value.IsIn(18, 19) ? bor.LineData.Select(p => p.SplitAndRemoveQuotes(",")[19]).FirstOrDefault() : bor.LineData.Select(p => p.SplitAndRemoveQuotes(",")[27]).FirstOrDefault());
                }
                catch (Exception ex)
                {
                    string message = $"Error in step: {step}; Borrower: {bor.AccountNumber}; Print Processing Id: {bor.PrintProcessingId}; Exception: {ex.Message}";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                    Console.WriteLine(message);
                }
            });
        }

        /// <summary>
        /// Creates all the files used to hold merge data
        /// </summary>
        private void PreparePrintingFiles()
        {
            if (!File.Exists(InterestStatementFile))//Write out the header if the file does not exist
                using (StreamW sw = new StreamW(InterestStatementFile))
                    sw.WriteLine(IH.GetInterestStatementHeader());
        }

        /// <summary>
        /// Deletes all the temp files.
        /// </summary>
        private void DeleteFiles(string dir)
        {
            Console.WriteLine("Deleting temp pdf files");
            Repeater.TryRepeatedly(() => FS.Delete(dir, true)); //Directory
            Repeater.TryRepeatedly(() => FS.Delete(InterestNoticeDataFile));
            Repeater.TryRepeatedly(() => FS.Delete(InterestStatementDataFile));
            Repeater.TryRepeatedly(() => FS.Delete(InterestStatementFile));
        }

        /// <summary>
        /// The the Eoj and Process Logger
        /// </summary>
        private void LogScriptEnd()
        {
            Console.WriteLine("Ending script and log process end");
            ProcessLogger.LogEnd(LogRun.ProcessLogId);
            Thread.Sleep(5000);
        }

        /// <summary>
        /// Checks to see if in recovery 
        /// </summary>
        private string CheckRecoveryDirectories(string dir, string dirPath)
        {
            List<string> recDirs = Directory.GetDirectories(dirPath, (ScriptId + "*")).ToList();
            if (recDirs.Count == 0)
                dir = Path.Combine(dirPath, (ScriptId + "_" + DateTime.Now.ToShortDateString().Replace("/", "_")));
            else if (recDirs.Count == 1)
                dir = recDirs.First();
            else
            {
                string errorMessage = $"The Script has found multiple recovery directories on {dirPath}.  Please review.";
                LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return null;
            }
            return dir;
        }

        /// <summary>
        /// Adds comments for each borrower to the ArcAddProcessing database
        /// </summary>
        private void AddComment(Borrower borrower, int reportNumber, string endAcctNum)
        {
            if (!borrower.ArcAddedAt.HasValue)
            {
                ArcData arc = GenerateArc(borrower, reportNumber, endAcctNum);
                for (int i = 0; i < (arc.DelinquencyArc.IsPopulated() ? 2 : 1); i++)
                {
                    if (i == 1)
                        arc.Arc = arc.DelinquencyArc;
                    ArcAddResults result = arc.AddArc();
                    if (result.ArcAdded)
                        borrower.SetArcAddedAt();
                    else
                    {
                        string message = $"Error adding ARC {arc.Arc} to ArcAdd database for borrower: {arc.AccountNumber}; Print Process Id: {borrower.PrintProcessingId}";
                        LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    }

                }
            }
        }

        /// <summary>
        /// Generates an ArcAdd object
        /// </summary>
        private ArcData GenerateArc(Borrower bor, int reportNumber, string endAcctNum)
        {
            int daysDeliq = 0;
            if (!reportNumber.IsIn(16, 17, 18, 19))
                daysDeliq = bor.LineData[0].SplitAndRemoveQuotes(",")[48].ToInt();

            bor.AccountNumber = bor.LineData[0].SplitAndRemoveQuotes(",")[2];
            CommentsAndArcs ca = new CommentsAndArcs(reportNumber, bor.AccountNumber, daysDeliq, endAcctNum);
            List<int> loanSeqs = bor.LineData.Select(p => p.SplitAndRemoveQuotes(",")[13].ToInt()).ToList();
            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = bor.AccountNumber,
                Arc = ca.Arc,
                DelinquencyArc = ca.DelinquentArc1.IsPopulated() ? ca.DelinquentArc1 : ca.DelinquentArc2.IsPopulated() ? ca.DelinquentArc2 : "",
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                Comment = ca.Comment,
                IsEndorser = false,
                IsReference = false,
                LoanPrograms = null,
                LoanSequences = loanSeqs,
                NeedBy = null,
                ProcessFrom = null,
                ProcessOn = null,
                ProcessTo = null,
                RecipientId = ca.EndorserSsn,
                RegardsCode = null,
                RegardsTo = null,
                ScriptId = ScriptId
            };

            return arc;
        }
    }
}