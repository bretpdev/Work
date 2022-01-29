using DocProcessingFileLoad;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace REPAYDISCL
{
    public class RepaymentDisclosureStatements
    {
        private const string PrintingOnlyFile = "Print";
        private const string ImageOnlyFile = "Image";
        private List<string> DocIds { get; set; }
        public ProcessLogData LogData { get; set; }
        public string ScriptId
        {
            get
            {
                return "REPAYDISCL";
            }
        }
        public static string ImagingFolder { get; set; }

        public RepaymentDisclosureStatements()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            LogData = ProcessLogger.RegisterApplication(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
            DocIds = DataAccess.GetAllDocIds();
            foreach (string docid in DocIds)
            {
                List<string> files = Directory.GetFiles(EnterpriseFileSystem.TempFolder).Where(p => p.Contains(docid)).ToList();
                foreach (string file in files)
                    Repeater.TryRepeatedly(() => File.Delete(file));//There is no recovery for printing so delete anything that is there
            }
        }

        [STAThread]
        public static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!DataAccessHelper.StandardArgsCheck(args, "Repayment Disclosure"))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            return new RepaymentDisclosureStatements().Process();
        }

        private void ChangePrinterToDuplex()
        {
            //if (Dialog.Def.YesNo("Do you want to change the printer settings autoamtically?"))
            //{
                //Change the Printer to Duplex
                PrinterInfo info = new PrinterInfo(true);
                info.ChangePrinterSettings();
            //}
        }

        /// <summary>
        /// Copy the word docs down to the T: drive, create a new thread for Print/Image/Comments and process borrowers
        /// </summary>
        private int Process()
        {
            Console.WriteLine("Loading Files");
           ChangePrinterToDuplex();

            if (PrintProcessing.LoadPrintFiles(LogData, ScriptId, 11, DataAccessHelper.Region.Uheaa)) //Processes the ULWS06 files
            {
                ImagingFolder = string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, ScriptId);
                if (!Directory.Exists(ImagingFolder))
                    Repeater.TryRepeatedly(() => Directory.CreateDirectory(ImagingFolder));

                Console.WriteLine("Gathering Borrower Data");
                List<Borrower> borrowers = DataAccess.GetUnprocessedBorrowerData(ScriptId);
                GetLineData(borrowers);

                Console.WriteLine("Processing Borrowers");
                var printThread = Task.Factory.StartNew(() => PrintAndImage(borrowers));//Printing and Imaging are tightly coupled for this request so I do not think it makes sense to split them out at this point
                var commentThread = Task.Factory.StartNew(() => Comment(borrowers));
                Task.WhenAll(printThread, commentThread).Wait();

                if (Directory.Exists(ImagingFolder))
                    Repeater.TryRepeatedly(() => Directory.Delete(ImagingFolder, true));
                ProcessComplete();

                return 0;
            }
            return 1; //Something happened in the LoadPrintFiles
        }

        /// <summary>
        /// Publish EOJ and ERR reports, log end of process and create completed log
        /// </summary>
        private void ProcessComplete()
        {
            DataAccess.CloseConnections();
            ProcessLogger.LogEnd(LogData.ProcessLogId);
            Console.WriteLine("Complete");
        }

        /// <summary>
        /// Load the line data for each borrower.
        /// </summary>
        private void GetLineData(List<Borrower> borrowers)
        {
            Parallel.ForEach(borrowers, new ParallelOptions() {MaxDegreeOfParallelism = int.MaxValue}, bor => 
            {
                bor.LineData = DataAccess.GetLineData(bor.PrintProcessingId);
            });
        }

        /// <summary>
        /// Print the repayment disclosures
        /// </summary>
        /// <param name="borrowers"></param>
        private void PrintAndImage(List<Borrower> borrowerData)
        {
            ProcessData(borrowerData);
        }

        private void ProcessData(List<Borrower> borrowers)
        {
            List<string> filesToProcess = CreateDataFiles(borrowers);
            Print(filesToProcess.Where(p => !p.Contains(ImageOnlyFile)).ToList());
            DataTable ids = Borrower.GetPrintProcessingIds(borrowers);
            DataAccess.SetPrintComplete(ids);//Since printing is an all or nothing we can mass update the print processing table
            DataAccess.SetDocumentCreated(ids);
            Image(filesToProcess.Where(p => !p.Contains(PrintingOnlyFile)).ToList());
            DataAccess.SetImageComplete(ids);//Since Imaging is an all or nothing we can mass update the print processing table

            foreach (string file in filesToProcess)//Clean up the files
                Repeater.TryRepeatedly(() => File.Delete(file));
            int printCount = 0;
            int imageCount = 0;
            foreach (Borrower bwr in borrowers.Where(p => p.DocumentCreatedAt == null || p.PrintedAt == null))
            {
                printCount += bwr.LineData.Count;
            }
            foreach (Borrower bwr in borrowers.Where(p => p.ImagedAt == null))
            {
                imageCount += bwr.LineData.Count;
            }
           
            DataTable dt = new DataTable();
            dt.Columns.Add(string.Format("Repayment Disclosure totals: {0}", DateTime.Now.ToString("MM-dd-yyyy")));
            dt.Rows.Add(string.Format("Number of documents printed {0}", printCount));
            dt.Rows.Add(string.Format("Number of documents imaged {0}", imageCount));

            string numberFile = PdfHelper.AddTable(dt);

            DocumentProcessing.PrintPdf(numberFile);
            Thread.Sleep(30000);

            Repeater.TryRepeatedly(() => File.Delete(numberFile));
        }

        private void Image(List<string> filesToProcess)
        {
            foreach (string file in filesToProcess)
            {
                int index = (file.IndexOf('.') + 1);//Getting the DocId from the file
                int length = file.LastIndexOf('.') - index;
                DocumentProcessing.ImageDocs(ScriptId, "DF_SPE_ACC_ID", file.Substring(index, length), Path.GetExtension(file).Remove(0, 1), file, DocumentProcessing.LetterRecipient.Borrower);
            }
        }

        private void Print(List<string> filesToProcess)
        {
            foreach (string file in filesToProcess)
            {
                DocumentProcessing.CostCenterPrinting(Path.GetExtension(file).Remove(0, 1), file, "DOM_FGN_ST", ScriptId, "DF_SPE_ACC_ID", DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, "COST_CENTER_CODE");

                if (file.Contains(PrintingOnlyFile))//Deletes the print only file Imaging is already done
                    Repeater.TryRepeatedly(() => File.Delete(file));
            }

        }

        /// <summary>
        /// Creates a datafile to print the cover sheet
        /// </summary>
        private List<string> CreateDataFiles(List<Borrower> borrowers)
        {
            List<string> filesToProcesses = new List<string>();
            foreach (Borrower bor in borrowers)
            {
                if ((bor.DocumentCreatedAt == null || bor.PrintedAt == null) && bor.ImagedAt == null)
                    filesToProcesses.Add(AddToDataFile(bor));
                else if ((bor.DocumentCreatedAt == null || bor.PrintedAt == null) && bor.ImagedAt != null)
                    filesToProcesses.Add(AddToDataFile(bor, PrintingOnlyFile));
                else if ((bor.DocumentCreatedAt != null || bor.PrintedAt != null) && bor.ImagedAt == null)
                    filesToProcesses.Add(AddToDataFile(bor, ImageOnlyFile));
                else
                    continue;//The arc needs to be done but will be done in another thread
            }

            return filesToProcesses = filesToProcesses.Distinct().ToList();
        }

        private string AddToDataFile(Borrower bor, string step = "All")
        {
            string file = string.Format("{0}{3}.{1}.{2}", EnterpriseFileSystem.TempFolder, bor.DocId, bor.LetterId, step);
            if (!File.Exists(file))
            {
                PrepMergeFile(file, bor.FileHeader);
            }

            using (StreamWriter sw = new StreamWriter(file, true))
            {
                foreach (string line in bor.LineData)
                    sw.WriteLine(line);
            }

            return file;
        }

        private void PrepMergeFile(string file, string header)
        {
            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.WriteLine(header);
            }
        }

        /// <summary>
        /// Adds the borrower comment to the ArcAdd database.
        /// </summary>
        private void Comment(List<Borrower> borrowers)
        {
            foreach (Borrower bor in borrowers.Where(p => p.ArcAddedAt == null))
            {
                if (bor.Arc != null)
                {
                    ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = bor.AccountNumber,
                        Arc = bor.Arc,
                        Comment = "",
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                        ScriptId = ScriptId
                    };
                    ArcAddResults result = arc.AddArc();
                    if (!result.ArcAdded)
                        ProcessLogger.AddNotification(LogData.ProcessLogId, string.Join("\r\n", result.Errors), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                DataAccess.SetCommentComplete(bor.PrintProcessingId);
            }
        }
    }
}