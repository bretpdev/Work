using DocProcessingFileLoad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.FileLoad;
using Uheaa.Common.ProcessLogger;

namespace BILLING
{
    class Program
    {
        public static ProcessLogRun PLR { get; set; }

        public static string ScriptId = "BILLING";
        public static string BillingFolder { get; set; }
        private DataAccess DA { get; set; }

        public Program()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            PLR = new ProcessLogRun(Program.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DA = new DataAccess(PLR.ProcessLogId);
        }

        static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "Billing"))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return 1;

            return new Program().Run();
        }

        private static bool CheckStartingLocation()
        {
            var location = Assembly.GetEntryAssembly().Location;
            if (location.Contains("X:"))
            {
                Dialog.Info.Ok("You are attempting to start this from a network drive. This must be started locally to run.");
                return false;
            }
            return true;
        }

        public int Run()
        {
            Console.WriteLine("Loading Files");
            if (ChangePrinterToDuplex() == 1)
                return 1;

            if (FileLoader.LoadFiles(Program.PLR, Program.ScriptId, DataAccessHelper.Region.Uheaa, "print")) //Processes the ULWS014 files
            {
                BillingFolder = string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, Program.ScriptId);
                if (Directory.Exists(BillingFolder))
                    Repeater.TryRepeatedly(() => Directory.Delete(BillingFolder, true));

                Thread.Sleep(3000);//Sleep to allow the network to catch up
                Repeater.TryRepeatedly(() => Directory.CreateDirectory(BillingFolder));

                Console.WriteLine("Gathering Borrower Data");
                var getBorrowers = DA.GetUnprocessedBorrowerData(Program.ScriptId);
                if (!getBorrowers.DatabaseCallSuccessful)
                    return 1;
                List<Borrower> borrowers = getBorrowers.Result;
                GetLineData(borrowers);
                if (borrowers.Count == 0)
                    Console.WriteLine("There were no borrowers found for processing");
                else
                    Console.WriteLine("Processing Borrowers");

                var printThread = Task.Factory.StartNew(() => new PrintAndImage(DA).PrintAndImageBwrs(borrowers));//Printing and Imaging are tightly coupled for this request so I do not think it makes sense to split them out at this point
                var commentThread = Task.Factory.StartNew(() => new Comment().Start(borrowers, PLR, DA));
                Task.WhenAll(commentThread, printThread).Wait();

                Console.WriteLine("Deleting all files and folders used for processing");
                if (Directory.Exists(BillingFolder))
                    Repeater.TryRepeatedly(() => Directory.Delete(BillingFolder, true));

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
            Console.WriteLine("Ending process logger");
            PLR.LogEnd();
            Thread.Sleep(5000);
        }

        /// <summary>
        /// Load the line data for each borrower.
        /// </summary>
        private void GetLineData(List<Borrower> borrowers)
        {
            foreach (Borrower bor in borrowers)
            {
                var getLineData =  DA.GetLineData(bor.PrintProcessingId);
                if (!getLineData.DatabaseCallSuccessful)
                {
                    PLR.AddNotification(string.Format("Couldn't find line data for borrower {0}, PrintProcessingId {1}", bor.AccountNumber, bor.PrintProcessingId), NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    continue;
                }
                bor.LineData = getLineData.Result;
                if (bor.ReportNumber.IsIn(25))
                {
                    int? daysDelq = bor.LineData.SplitAndRemoveQuotes(",")[385].ToIntNullable();
                    if (daysDelq != null && daysDelq > 0)
                    {
                        List<string> lineDataList = bor.LineData.SplitAndRemoveQuotes(",");
                        lineDataList[393] = DA.GetDelqComment(daysDelq.Value).Result;
                        string comment = "";
                        foreach (string item in lineDataList)
                        {
                            if (item != null)
                                comment += (item.Contains(',') ? item.Insert(0, "\"").Insert(item.Length, "\"") : item) + ",";
                            else
                                comment += ",";
                        }
                        bor.LineData = comment.Remove(comment.LastIndexOf(','), 1);
                    }
                }
            }
        }

        /// <summary>
        /// Change the printer to duplex
        /// </summary>
        private int ChangePrinterToDuplex()
        {
            try
            {
                //Change the Printer to Duplex
                PrinterInfo info = new PrinterInfo(true);
                info.ChangePrinterSettings();
            }
            catch (Exception ex)
            {
                if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live)
                {
                    PLR.AddNotification("There was an error setting the printer to duplex", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                    return 1;
                }
            }
            return 0;
        }
    }
}