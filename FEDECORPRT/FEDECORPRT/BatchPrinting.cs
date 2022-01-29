using DMATRIXLib;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Word = Microsoft.Office.Interop.Word;

namespace FEDECORPRT
{
    public partial class BatchPrinting
    {
        private int NumberOfProcesses { get; set; }
        private DataAccess DA { get; set; }
        private Dictionary<string, string> internalNames;
        private ProcessLogRun PL { get; set; }
        public BatchPrinting(int numberOfProcesses, DataAccess da, ProcessLogRun pl)
        {
            NumberOfProcesses = numberOfProcesses;
            DA = da;
            PL = pl;
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
        /// Starts the Printing Process for all Scripts
        /// </summary>
        /// <returns>0 is success 1 is an error occured</returns>
        public int RunPrinting()
        {
            if (DataAccessHelper.TestMode)
            {
                if (Uheaa.Common.Dialog.Info.YesNo("Do you want to change the printer setting automatically?"))
                {
                    PrinterInfo pi = new PrinterInfo();
                    pi.ChangePrinterSettings(true);
                    Thread.Sleep(5000);
                }
            }
            else
            {
                PrinterInfo pi = new PrinterInfo();
                pi.ChangePrinterSettings(true);
                Thread.Sleep(5000);
            }

            List<ScriptData> processedScripts = new List<ScriptData>();
            ScriptData file = DA.PopulateScriptData(new List<ScriptData>());

            Queue<Application> word = new Queue<Application>();
            if (file != null)
            {
                for (int wordCount = 0; wordCount < NumberOfProcesses; wordCount++)
                {
                    word.Enqueue(new Microsoft.Office.Interop.Word.Application());
                }
            }

            while (file != null && !processedScripts.Where(p => p.ScriptDataId == file.ScriptDataId).Any())
            {
                Console.WriteLine("Processing ScriptId {0}", file.ScriptDataId);
                LoadAdditionalFileData(file);
                string dir = InitializeDirectory(file);
                List<List<PrintProcessingData>> processingGroups;
                ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
                DocumentPathAndName docInfo = Uheaa.Common.DocumentProcessing.DataAccess.GetDocumentPathAndName(file.Letter);
                string templatePath = Path.Combine(docInfo.CalculatedPath, docInfo.CalculatedFileName);

                if (file.LetterDataForBorrowers.Any())//There was nothing to process move to the next.
                {
                    //file = GetNextFile(processedScripts, file);
                    //continue;
                    AddArcs(file);
                    Print(file, dir, false);//We are doing batch printing for speed.
                    processingGroups = PrepImagingFiles(file, false);

                    Parallel.ForEach(processingGroups, new ParallelOptions { MaxDegreeOfParallelism = NumberOfProcesses }, group =>
                    {
                        locker.EnterWriteLock();
                        var wordApp = word.Dequeue();
                        locker.ExitWriteLock();
                        Image(file, dir, templatePath, group, wordApp, false);
                        locker.EnterWriteLock();
                        word.Enqueue(wordApp);
                        locker.ExitWriteLock();
                    });

                    string ecorrTemplatePath = Path.Combine(Path.GetDirectoryName(docInfo.CalculatedPath), file.Letter + ".pdf");

                    if (!file.DoNotProcessEcorr)
                    {
                        Parallel.ForEach(file.LetterDataForBorrowers, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, data =>
                        {
                            if (!data.EcorrDocumentCreatedAt.HasValue)
                                DoEcorr(file, data, ecorrTemplatePath, false);
                        });
                    }
                    //PROCESS COBORROWERS HERE
                    if (file.CheckForCoBorrower)
                    {
                        internalNames = GetInternalHeaderNamesHelper();
                        Parallel.ForEach(file.LetterDataForBorrowers, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, data =>
                        {
                            //ADD COBORROWERS HERE
                            List<CoBorrowerInformation> cbi = GetCoBorrowers(data.BF_SSN);
                            InsertCoBorrowerInformation(cbi, file, data, internalNames);
                        });
                    }
                }
                LoadAdditionalCoBorrowerFileData(file);
                //Reset the file header so it is no longer has the barcode headers
                file.FileHeader = file.FileHeaderConst;

                if (file.LetterDataForCoBorrowers == null || !file.LetterDataForCoBorrowers.Any())
                {
                    file = GetNextFile(processedScripts, file);
                    continue;
                }

                //Set the letterdata here in case it was added manually
                //foreach (PrintProcessingData p in file.LetterDataForCoBorrowers)
                //{
                //    p.LetterDataConst = p.LetterData;
                //}

                Print(file, dir, true);

                processingGroups = PrepImagingFiles(file, true);
                Parallel.ForEach(processingGroups, new ParallelOptions { MaxDegreeOfParallelism = NumberOfProcesses }, group =>
                {
                    locker.EnterWriteLock();
                    var wordApp = word.Dequeue();
                    locker.ExitWriteLock();
                    Image(file, dir, templatePath, group, wordApp, true);
                    locker.EnterWriteLock();
                    word.Enqueue(wordApp);
                    locker.ExitWriteLock();
                });

                DoEcorrCoBorrower(file);
                
                

            file = CleanUp(processedScripts, file, dir);
            }

            while (word.Any())
            {
                var wordApp = word.Dequeue();
                Console.WriteLine("Closing All Word Docs.");
                wordApp.Application.Quit(false);
                wordApp.Quit(false);
                
                Marshal.FinalReleaseComObject(wordApp);
                Thread.Sleep(1000);
            }

            GC.Collect();
            Console.WriteLine("Done.{0}", Environment.NewLine);

            return 0;
        }

        private void AddArcs(ScriptData file)
        {
            Console.WriteLine("Adding ARCS");
            if (file.Arcs.Any())
            {
                Parallel.ForEach(file.LetterDataForBorrowers, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, bwr =>
                {
                    if (bwr.ArcNeeded && !bwr.ArcAddProcessingId.HasValue)
                        AddArcs(file, bwr);
                });
            }
        }

        private void LoadAdditionalFileData(ScriptData file)
        {
            Console.WriteLine("Getting Additional Data Needed.");
            file.GetArcData(DA);
            file.LetterDataForBorrowers = DA.PopulatePrintProcessingData(file.ScriptDataId);
        }

        private ScriptData CleanUp(List<ScriptData> processedScripts, ScriptData file, string dir)
        {
            var result = Repeater.TryRepeatedly(() => Directory.Delete(dir, true));
            CheckRepeater(result, string.Format("Unable to delete directory {0}", dir), false);
            file.SetLastRun();
            processedScripts.Add(file);
            file = DA.PopulateScriptData(processedScripts);
            return file;
        }

        private void Image(ScriptData file, string dir, string templatePath, List<PrintProcessingData> group, Word.Application wordApp, bool isCoBorrower)
        {
            if (file.DocIdName.IsPopulated() && group.Where(data => data.ImagingNeeded && !data.ImagedAt.HasValue).Any())
            {
                string templateFile = Path.Combine(dir, string.Format("{0}_{1}", Guid.NewGuid().ToBase64String(), Path.GetFileName(templatePath)));//Create a unique file.
                File.Copy(templatePath, templateFile);
                object fileName = templateFile;
                object refFalse = false;
                object refTrue = true;
                object pause = false;
                object missing = System.Type.Missing;
                object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                wordApp.Visible = false;
                string lastFileProcessed = "";
                var doc = wordApp.Application.Documents.Open(ref fileName, ref missing);//Open in read only
                bool createdFile = false;
                foreach (PrintProcessingData data in group)
                {
                    try
                    {
                        if (file.DocIdName.IsPopulated() && data.ImagingNeeded && !data.ImagedAt.HasValue)
                        {
                            createdFile = CreateAndImageDocument(file, lastFileProcessed, doc, data, pause, format, wordApp, dir, isCoBorrower);
                            ((Word._Document)wordApp.ActiveDocument).Close(ref refFalse);//This is the newly merge document and not the template file
                        }

                    }
                    catch (Exception ex)//Handle any problems and process log so that 1 document crashing will not take down the entire process.
                    {
                        ProcessLogger.AddNotification(Program.PL.ProcessLogId, string.Format("Unable to generate document for PrintProcessing Id:{0}", data.PrintProcessingId), NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly(), ex);
                    }
                }

                try
                {
                    if (createdFile)
                        ((Word._Document)doc).Close(ref refFalse);//this is the template file
                }
                catch (Exception ex)
                {
                    ProcessLogger.AddNotification(Program.PL.ProcessLogId, string.Format("Unable to close a word doc {0}", fileName.ToString()), NotificationType.ErrorReport, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly(), ex);
                }
            }
        }

        private bool CreateAndImageDocument(ScriptData file, string lastFileProcessed, Document doc, PrintProcessingData data, object pause, object format, Word.Application wordApp, string dir, bool isCoborrower)
        {
            Console.WriteLine("Imaging PrintProcessingID: {0}; ScriptDataID: {1}; Letter: {2}", data.PrintProcessingId, file.ScriptDataId, file.Letter);
            doc.MailMerge.OpenDataSource(data.WordDataFile);
            if (lastFileProcessed.IsPopulated())
            {
                //No Need to care if this does not work it will get cleaned up in the end if it is not done here.
                Repeater.TryRepeatedly(() => File.Delete(lastFileProcessed));
            }

            doc.MailMerge.SuppressBlankLines = true;
            doc.MailMerge.Destination = Microsoft.Office.Interop.Word.WdMailMergeDestination.wdSendToNewDocument;
            doc.MailMerge.Execute(ref pause);
            string uniqueId = Guid.NewGuid().ToString();
            string imagingFolder = EnterpriseFileSystem.GetPath("IMAGING");
            string destination = string.Format("{0}{1}_{2}.pdf", imagingFolder, data.BF_SSN, uniqueId);
            object saveAs = destination;
            var result = Repeater.TryRepeatedly(() => wordApp.ActiveDocument.SaveAs(ref saveAs, ref format));
            CheckRepeater(result, string.Format("Unable to save {0} for PrintProcessingId:{1}", saveAs, data.PrintProcessingId));
            Image(file, data, saveAs, isCoborrower);
            return true;
        }

        private List<List<PrintProcessingData>> PrepImagingFiles(ScriptData file, bool isCoBorrower)
        {
            List<PrintProcessingData> ppd;
            if (!isCoBorrower)
            {
                ppd = file.LetterDataForBorrowers;
            }
            else
            {
                ppd = file.LetterDataForCoBorrowers;
            }
            List<List<PrintProcessingData>> processingGroups = new List<List<PrintProcessingData>>();
            int take = 50;//Number of word documents that will open and process
            if (ppd.Count < take)//if initial population is < 50 set take to 50
                take = ppd.Count;
            for (int skip = 0; skip < ppd.Count; skip += take)
                processingGroups.Add(ppd.Skip(skip).Take(take).ToList());

            return processingGroups;
        }

        private ScriptData GetNextFile(List<ScriptData> processedScripts, ScriptData file)
        {
            file.SetLastRun();
            processedScripts.Add(file);
            file = DA.PopulateScriptData(processedScripts);
            return file;
        }

        private string InitializeDirectory(ScriptData file)
        {
            string dir = Path.Combine(EnterpriseFileSystem.GetPath("C Drive"), file.Letter);
            if (Directory.Exists(dir))//Delete everything files will get recreated for unprocessed borrowers
            {
                var result = Repeater.TryRepeatedly(() => Directory.Delete(dir, true));
                CheckRepeater(result, string.Format(@"Unable to delete directory {0}", dir));
            }

            var result1 = Repeater.TryRepeatedly(() => Directory.CreateDirectory(dir));
            CheckRepeater(result1, string.Format(@"Unable to create directory {0}", dir));
            return dir;
        }

        private static void CheckRepeater(RepeatResults<Exception> result, string message, bool throwEx = true)
        {
            if (!result.Successful)
            {
                foreach (var ex in result.CaughtExceptions)
                    ProcessLogger.AddNotification(Program.PL.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly(), ex);

                if (throwEx)
                    throw result.CaughtExceptions.First();
            }
        }

        private Dictionary<string, string> GetBarcodeData(string letterId, string accountNumber)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            BarcodeQueryResults queryResults = Uheaa.Common.DocumentProcessing.DataAccess.GetPageCount(letterId);
            int pageCount = 1;
            if (queryResults.Pages != 1)
            {
                pageCount = int.Parse(queryResults.Pages.ToString()) / 2;
                if (queryResults.Pages % 2 > 0)
                    pageCount = pageCount + 1;
            }

            return new Dictionary<string, string>() { { GetBarCodeHeader(pageCount), EncodeBarcodes(letterId, accountNumber, pageCount) } };
        }

        private string EncodeBarcodes(string letterId, string accountNumber, int pageCount)
        {
            char[] barcodeDelimiter = { '\r', '\n' };
            long dataFileLineNum = 0;
            List<string> barcodeData = new List<string>();

            foreach (string returnMailBarcode in Encode(string.Format("{0}{1}{2:MMddyyyy}", accountNumber, letterId.PadLeft(10), DateTime.Now), 0, 0, 0).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
                barcodeData.Add(returnMailBarcode);

            for (int pageNum = 1; pageNum <= pageCount; pageNum++)
            {
                int leadingDigit = pageNum == 1 ? 1 : 0;
                foreach (string stateMailBarcode in Encode(string.Format("{0}{1}0{2}{3:00000#}", leadingDigit, pageCount, pageNum, dataFileLineNum), 0, 0, 0).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
                    barcodeData.Add(stateMailBarcode);
            }

            barcodeData.Add(DateTime.Now.ToString("MM/dd/yyyy"));
            return string.Join(",", barcodeData) + ",";
        }

        private string Encode(string dataToEncode, int procTilde, int encMode, int prefFormat)
        {
            string encodedFont = "";
            Datamatrix DMFontEncoder = new Datamatrix();
            DMFontEncoder.FontEncode(dataToEncode, procTilde, encMode, prefFormat, out encodedFont);
            return encodedFont;
        }

        private static string GetBarCodeHeader(int pageCount)
        {
            string barcodedFields = "BC1,BC2,BC3,BC4,BC5,BC6,SMBC1,SMBC2,SMBC3,SMBC4,";
            if (pageCount > 1)
                barcodedFields = barcodedFields + "SMBC_Pg2_Ln1,SMBC_Pg2_Ln2,SMBC_Pg2_Ln3,SMBC_Pg2_Ln4,";
            if (pageCount > 2)
                barcodedFields = barcodedFields + "SMBC_Pg3_Ln1,SMBC_Pg3_Ln2,SMBC_Pg3_Ln3,SMBC_Pg3_Ln4,";
            if (pageCount > 3)
                barcodedFields = barcodedFields + "SMBC_Pg4_Ln1,SMBC_Pg4_Ln2,SMBC_Pg4_Ln3,SMBC_Pg4_Ln4,";
            if (pageCount > 4)
                barcodedFields = barcodedFields + "SMBC_Pg5_Ln1,SMBC_Pg5_Ln2,SMBC_Pg5_Ln3,SMBC_Pg5_Ln4,";
            if (pageCount > 5)
                barcodedFields = barcodedFields + "SMBC_Pg6_Ln1,SMBC_Pg6_Ln2,SMBC_Pg6_Ln3,SMBC_Pg6_Ln4,";
            if (pageCount > 6)
                barcodedFields = barcodedFields + "SMBC_Pg7_Ln1,SMBC_Pg7_Ln2,SMBC_Pg7_Ln3,SMBC_Pg7_Ln4,";

            barcodedFields = barcodedFields + "StaticCurrentDate,";

            return barcodedFields;
        }
    }
}