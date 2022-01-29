using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using LNDERLETTR.Helpers;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace LNDERLETTR
{
    public class ProcessQueue
    {
        private ProcessLogRun LogRun { get; set; }
        private Populations Pops { get; set; }
        private AddArc ArcAdd { get; set; }
        private int BANA { get; } = 1;
        private int UHEA { get; } = 2;
        private int OPEN { get; } = 3;
        private int CLSD { get; } = 4;

        public string ScriptId
        {
            get
            {
                return "LNDERLETTR";
            }
        }
        public List<PrintingFiles> FilesToPrint { get; set; }
        public DataAccess DA { get; set; }
        
        private List<int> queueMarked;

        public ProcessQueue(ProcessLogRun logRun)
        {
            LogRun = logRun;
            FilesToPrint = new List<PrintingFiles>();
            Pops = new Populations();
            DA = new DataAccess(LogRun);
            queueMarked = new List<int>();
            ArcAdd = new AddArc(DA, LogRun, Pops);
            
        }
        public int initPrinter(ProcessLogRun plr, string theMode)
        {

            //#if DEBUG
            if (theMode.ToUpper() == "DEV") 
            {
                if (Uheaa.Common.Dialog.Info.YesNo("Do you want to change the printer setting automatically?"))
                {
                    Console.WriteLine("Setting printer manually.");
                    PrinterInfo pi = new PrinterInfo();
                    while (pi.Duplex != 2)
                    {
                        pi.Duplex = 1;
                        try
                        {
                            pi.ChangePrinterSettings(true);
                        }
                        catch (Exception ex)
                        {
                            plr.AddNotification("Error setting debug printer", NotificationType.HandledException, NotificationSeverityType.Critical, ex);
                            return 1;
                        }
                        Thread.Sleep(10000);
                    }
                }
            }
            else
            {
                try
                {
                    PrinterInfo pi = new PrinterInfo();
                    pi.ChangePrinterSettings(true);
                }
                catch (Exception ex)
                {
                    plr.AddNotification("Error setting live printer", NotificationType.HandledException, NotificationSeverityType.Critical, ex);
                    return 1;
                }
            }
                return 0;
        }
        
        /// <summary>
        /// Load the data from fp.LineData and determine which step to run for each file
        /// </summary>
        /// <returns>0 if no errors, 1 if errors in processing</returns>
        public int Process()
        {
            Console.WriteLine("Loading data into ULS.[lnderlettr].letters. (InsertLetterData)");
            DA.LoadData();

            string managerId = DA.ManagerId();

            List<LetterData> processData = DA.GetPendingWork(); //List of all accounts that need to be processed

            if (processData.Count == 0)
            {
                LogRun.AddNotification("There are no records to process.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                return 0;
            }
            
            ReflectionInterface ri = new ReflectionInterface();
            var log = BatchProcessingLoginHelper.Login(LogRun, ri, "LNDERLETTR", "BatchUheaa");
            if (log == null)
            {
                string message = string.Format("Unable to login with ID: {0}", log.UserName);
                ProcessLogger.AddNotification(LogRun.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return 1;
            }
            Thread.Sleep(2500);
            
            //Generate letters for specified lenders.
            Console.WriteLine("Populations have been sorted, printing BANA letter.");
            List<LetterData> borrowers = processData.Where(p => p.Population == BANA && (p.LetterCreatedAt == null || p.QueueClosedAt == null || p.ArcAddProcessingId == null)).GroupBy(p => new { p.Ssn, p.LenderId}).Select(p => p.First()).ToList();
            if(borrowers.Count > 0)
                GenerateLenderLetters(borrowers, ri, managerId);

            Console.WriteLine("Printing Open Lender letters.");
            borrowers = processData.Where(p => p.Population == OPEN && (p.LetterCreatedAt == null || p.QueueClosedAt == null || p.ArcAddProcessingId == null)).GroupBy(p => new { p.Ssn, p.LenderId }).Select(p => p.First()).ToList();
            if (borrowers.Count > 0)
                GenerateLenderLetters(borrowers, ri, managerId);

            Console.WriteLine("Marking Arcs and Queues for Uheaa Lender population.");
            borrowers = processData.Where(p => p.Population == UHEA && (p.LetterCreatedAt == null || p.QueueClosedAt == null || p.ArcAddProcessingId == null)).GroupBy(p => new { p.Ssn, p.LenderId }).Select(p => p.First()).ToList();
            if (borrowers.Count > 0)
                CloseQueueAddArc(borrowers, managerId, ri);

            Console.WriteLine("Marking Arcs and Queues for Closed Lender population.");
            borrowers = processData.Where(p => p.Population == CLSD && (p.LetterCreatedAt == null || p.QueueClosedAt == null || p.ArcAddProcessingId == null)).GroupBy(p => new { p.Ssn, p.LenderId }).Select(p => p.First()).ToList();
            if (borrowers.Count > 0)
                CloseQueueAddArc(borrowers, managerId, ri);

            Console.WriteLine("All records processed, exiting program.");
            ri.CloseSession();
            return 0;
        }


        /// <summary>
        /// Gets a distinct list of letters that need to be generated for each lender
        /// </summary>  
        private void GenerateLenderLetters(List<LetterData> letters, ReflectionInterface ri, string manager)
        {
            List<LetterData> newLetters = CreateLenderData(letters);
            if(newLetters.Count > 0)
                Console.WriteLine("Processing lender letters");
            else
            {
                LogRun.AddNotification("There are no letters to process.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                return;
            }

            foreach (LenderData l in newLetters.Select(p => p.LenderInfo).ToList())
            {
                var print = newLetters.Where(p => p.LenderInfo.LenderId == l.LenderId && (p.LetterCreatedAt == null || l.UnprintedBorrowers.Count > 0)).Select(p => p.LenderInfo).FirstOrDefault();

                if (print != null)
                {
                    PrintingFiles pf = GenerateLetter(print);
                    if (!PrintLetter(pf))
                    {
                        LogRun.AddNotification($"Letter for {print.LenderName} not successful.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        break;
                    }

                    Console.WriteLine($"Setting borrowers as printed");
                    foreach (BorrowerData bd in print.UnprintedBorrowers)
                    {
                        if (!DA.SetLetterCreatedAt(bd.LettersId))
                            LogRun.AddNotification($"Error at SetLetterCreatedAt, letter: {bd.LettersId}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                }

                var arcLetters = newLetters.Where(p => p.LenderInfo.LenderId == l.LenderId && p.ArcAddProcessingId == null).ToList();
                markArcs(arcLetters);

                var queLetters = newLetters.Where(p => p.LenderInfo.LenderId == l.LenderId && p.QueueClosedAt == null).ToList();
                markQueues(ri, manager, queLetters);
            }
        }

        private void markQueues(ReflectionInterface ri, string manager, List<LetterData> letters)
        {
            foreach (LetterData letter in letters)
            {
                foreach (BorrowerData bd in letter.LenderInfo.AllBorrowers.Where(p => p.QueId == null))
                {
                    CloseQueue(ri, bd.Ssn, bd.LettersId, letter, LogRun, manager);
                }
            }
        }

        private void markArcs(List<LetterData> letters) 
        {
            foreach (LetterData letter in letters)
            {
                
                foreach (BorrowerData bd in letter.LenderInfo.AllBorrowers.Where(o => o.ArcId == null).ToList())
                {
                    ArcAdd.AddArcForLettersId(bd.AccountNumber, bd.LettersId, letter);
                }
            }
        }

        private List<LetterData> CreateLenderData(List<LetterData> letters) 
        {
            List<LetterData> newLetters = new List<LetterData>();
            //foreach (LetterData letter in letters.Select( p => p.LenderId).Distinct().ToList()) //Load the lender/borrower data into the dictionary
            foreach (string lenderId in letters.Select(p => p.LenderId).Distinct().ToList()) //Load the lender/borrower data into the dictionary
            {
                var letter = letters.First(o => o.LenderId == lenderId);

                LenderData lData;
                lData = DA.GetLenderData(lenderId);

                // Gather data for Lender:
                lData.UnprintedBorrowers = new List<BorrowerData>();
                lData.AllBorrowers = new List<BorrowerData>();

                foreach (LetterData ltr in letters.Where(p => p.LenderId == lenderId))
                {
                    BorrowerData bd = DA.GetBorrowerData(ltr.Ssn);
                    bd.LettersId = ltr.LettersId;
                    bd.ArcId = ltr.ArcAddProcessingId;
                    bd.QueId = ltr.QueueClosedAt;

                    if (bd != null && ltr.LetterCreatedAt == null)
                    {
                        lData.UnprintedBorrowers.Add(bd); //Get all the borrowers for the current lender
                    }

                    lData.AllBorrowers.Add(bd);
                }
                letter.LenderInfo = lData;
                letter.ArcAddProcessingId = null;
                letter.QueueClosedAt = null;
                newLetters.Add(letter);
            }


            return newLetters;
        }

        
        /// <summary>
        /// Creates the pdf document will all the borrowers data and adds it to a PrintingFiles object
        /// </summary>
        /// <param name="lender">The Lender the letter will be sent to</param>
        private PrintingFiles GenerateLetter(LenderData lender)
        {
            List<string> lenderAddress = LoadLenderAddress(lender);
            List<US06BLCNTMBwrAddr> borAddress = LoadBorrowerAddress(lender.UnprintedBorrowers);
            PrintingFiles pf = new PrintingFiles();
            pf.LenderName = lender.LenderName.Trim();
            pf.LenderId = lender.LenderId;
            pf.FilePath = US06BLCNTM.GenerateUS06BLCNTM(lenderAddress, borAddress, DA);
            return pf;
        }

        /// <summary>
        /// Creates a list of US06BLCNTMBwrAddr objects for each lender
        /// </summary>
        /// <param name="lender">The lender being processed</param>
        /// <returns>List<LNDERLETTR.US06BLCNTMBwrAddr> objects</returns>
        /// Prefaced with namespace; this used to be a common code object, but was changed to stay
        /// in the local project.
        private List<LNDERLETTR.US06BLCNTMBwrAddr> LoadBorrowerAddress(List<BorrowerData> borrowers)
        {
            List<LNDERLETTR.US06BLCNTMBwrAddr> borAddress = new List<LNDERLETTR.US06BLCNTMBwrAddr>();
            foreach (BorrowerData bor in borrowers)
            {
            
                LNDERLETTR.US06BLCNTMBwrAddr borAddr = new LNDERLETTR.US06BLCNTMBwrAddr();
                borAddr.Name = string.Format("{0} {1} {2}", bor.FirstName.Trim(), bor.MiddleInitial.Trim(), bor.LastName.Trim());
                borAddr.Address1 = bor.Address1.Trim();
                borAddr.Address2 = (bor.Address2 ?? "").Trim();
                borAddr.City = bor.City.Trim();
                borAddr.State = bor.State.Trim();
                borAddr.Zip = bor.ZipCode.Trim();
                borAddr.Province = bor.ForeignState.Trim();
                if(!borAddr.Province.IsNullOrEmpty())
                {
                    borAddr.Province += " ";
                }
                
                borAddr.Country = bor.ForeignCountry.Trim();
                //If phone and email are not provided, leave them as null values
                borAddr.Phone = (bor.HomePhone ?? "").Trim();
                borAddr.AltPhone1 = (bor.AlternatePhone ?? "").Trim();
                borAddr.AltPhone2 = (bor.WorkPhone ?? "").Trim();
                borAddr.EmailAddress = (bor.Email ?? "").Trim();
                if (bor.FirstNameHst.IsPopulated())
                    borAddr.PrevName = string.Format("{0} {1} {2}", bor.FirstNameHst.Trim(), (bor.MiddleInitialHst ?? "").Trim(), (bor.LastNameHst ?? "").Trim());
                borAddr.Ssn = bor.Ssn.Trim();
                borAddress.Add(borAddr);
            }
            return borAddress;
        }

        /// <summary>
        /// Load the lender address just how it should appear on the letter.
        /// </summary>
        /// <param name="lender"></param>
        /// <returns></returns>
        private List<string> LoadLenderAddress(LenderData lender)
        {
            List<string> lenderAddress = new List<string>();
            lenderAddress.Add(lender.LenderName.Trim());
            lenderAddress.Add(lender.Address1.Trim());
            lenderAddress.Add(lender.Address2.Trim());
            lenderAddress.Add($"{lender.City.Trim()}, {lender.State.Trim()} {lender.Zip.Trim()}");
            lenderAddress.Add(lender.Country.Trim());
            return lenderAddress;
        }


        /// <summary>
        /// Prints the letters that were generated during processing.
        /// </summary>
        private bool PrintLetter(PrintingFiles pf)
        {
            try
            {
                US06BLCNTM.PrintPdf(pf.FilePath);
                Repeater.TryRepeatedly(() => File.Delete(pf.FilePath));
            }
            catch (Exception ex)
            {
                LogRun.AddNotification($"There was an error printing file: {pf.FilePath} for Lender: {pf.LenderName} Lender Id: {pf.LenderId}", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Completes the Queue by adding COMPL/CANCL to the Queue
        /// </summary>
        /// <param name="lender">Lender object with Lender/Borrower data</param>
        private bool CloseQueue(ReflectionInterface RI, string ssn, int id, LetterData ld, ProcessLogRun plr, string manager) 
        {
            //Get only the borrowers that have not been processed
            if (!queueMarked.Contains(ld.LettersId))
            {
                string wildcardString = ssn + "*";
                Console.WriteLine("Closing queue task for letter id: {0}", id);
                RI.FastPath(string.Format("TX3Z/ITX6XJR;01;{0}", wildcardString));
                
                if (RI.MessageCode == "80014" || RI.MessageCode == "01020")
                {
                    LogError("JR", ssn, RI.Message); // JR is only queue
                    DA.UpdateError(id);    
                    return false;
                }
                RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter); // Make sure to hit Enter and not F2

                if (ld.Population == BANA || (ld.Population == OPEN && ld.ValidLenderAddress == "Y"))
                    RI.CloseCompassQueue("JR", "C", "COMPL");
                else if (ld.Population == UHEA || ld.Population == CLSD && (ld.ValidLenderAddress == "N" && ld.Address.Contains("X-CLOSED")))
                    RI.CloseCompassQueue("JR", "X", "CANCL");
                else
                    RI.ReAssignQueueTask("JR", "01", RI.UserId, manager);
                
                if (RI.MessageCode != "01005")
                {
                    ld.QueueClosedAt = DateTime.Now;
                    LogError("JR", ld.Ssn, RI.Message);
                    return false;
                }

                //ld.QueueClosedAt = DateTime.Now;
                DA.SetQueueClosedAt(id);
                queueMarked.Add(id);
            }
            return true;
        }

        public void LogError(string queue, string borrower, string message)
        {
            string errorMessage = $"Error completing {queue} queue for borrower: {borrower}; Message: {message}";
            LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }

        private void CloseQueueAddArc(List<LetterData> letterData, string manager, ReflectionInterface ri)
        {
            List<LetterData> letters = CreateLenderData(letterData);
            markArcs(letters);
            markQueues(ri, manager, letters);
        }
    }
}