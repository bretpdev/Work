using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using efs = Uheaa.Common.DataAccess.EnterpriseFileSystem;

namespace CNCRTRNSFR
{
    public class CancerTransferFile
    {
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        private const string FileName = "CNCRTRNSFR.txt";

        public CancerTransferFile(ProcessLogRun logRun)
        {
            LogRun = logRun;
            DA = new DataAccess(logRun);
        }

        public void Run()
        {
            string file = $"{efs.TempFolder}{FileName}";
            List<Borrower> bors = LoadBorrowers(file);
            if (bors != null)
                Process(bors);
#if !DEBUG
            Repeater.TryRepeatedly(() => File.Delete(file));
#endif
        }

        /// <summary>
        /// Gets a list of all borrowers from the Activity History - Imaging FED.r2 file that have a cancer deferment or forbearance
        /// </summary>
        private List<Borrower> LoadBorrowers(string file)
        {
            Console.WriteLine("Checking the to make sure the file exists and has records");
            List<Borrower> accounts = new List<Borrower>();
            if (!File.Exists(file))
            {
                string message = $"The {file} file is missing. Please replace the file and try running again.";
                Dialog.Error.Ok(message);
                LogRun.AddNotification(message, NotificationType.NoFile, NotificationSeverityType.Critical);
                return null;
            }
            else if (new FileInfo(file).Length == 0)
            {
                string message = $"The {file} file is empty.";
                Dialog.Error.Ok(message);
                LogRun.AddNotification(message, NotificationType.EmptyFile, NotificationSeverityType.Critical);
                return null;
            }
            else
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    sr.ReadLine(); //Read in header row
                    string currentSsn = ""; //The ssn is in the file multiple times, only need to check it once
                    while (!sr.EndOfStream)
                    {
                        Borrower b = new Borrower();
                        List<string> line = sr.ReadLine().SplitAndRemoveQuotes(",");
                        if (line.Count > 0 && currentSsn != line[1])
                        {
                            currentSsn = line[0];
                            b.Ssn = line[0];
                            b.SendingServicer = line[1];
                            b.ReceivingServicer = line[2];
                            b.DealId = line[3];
                            b.SaleDate = line[4].ToDate();
                            accounts.Add(b);
                        }
                    }
                }
            }
            return accounts;
        }

        ExcelHelper xlHelper;
        private void Process(List<Borrower> accounts)
        {
            Console.WriteLine($"Loading borrower accounts from {FileName}");
            List<Borrower> bors = LoadBorrowerData(accounts);
            if (bors == null || bors.Count == 0)
            {
                LogRun.AddNotification("There are no borrowers found with a cancer deferment or forbearance to process.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                return;
            }
            
            string dealId = "";

            string path = "";
            string awardId = "";
            foreach (Borrower bor in bors.OrderBy(p => p.DealId))
            {
                if (dealId != bor.DealId)
                {
                    if (xlHelper != null)
                        xlHelper.SaveAndQuit();
                    dealId = bor.DealId;
                    //Create new file to add borrowers to
                    path = $"{efs.TempFolder}CD_{bor.SendingServicer} NR_{bor.ReceivingServicer} NR_{bor.DealId}_{bor.SaleDate.ToShortDateString().Replace("/", "_")}_{DateTime.Now.Date.ToShortDateString().Replace("/", "_")}.xlsx";
                    xlHelper = new ExcelHelper(path, LogRun);
                    Console.WriteLine($"Creating new file: {path}");
                    xlHelper.AddHeader();
                    counter = 0;
                    row = 2;
                }
                if (awardId != bor.AwardId)
                    counter++;
                awardId = bor.AwardId;
                AddRecordToExcel(bor);
                row++;
            }
            xlHelper.SaveAndQuit();
        }

        int row = 2; //Always start on 2 since the header is on 1.
        int column = 1;
        int counter = 0;
        private void AddRecordToExcel(Borrower bor)
        {
            Console.WriteLine($"Adding borrower: {bor.Ssn}; Award ID: {bor.AwardId} to the file.");
            xlHelper.AddRecord(counter.ToString(), row, column);
            xlHelper.AddRecord(bor.Ssn, row, ++column);
            xlHelper.AddRecord(bor.AwardId, row, ++column);
            xlHelper.AddRecord(bor.DefermentBeginDate.ToShortDate(), row, ++column);
            xlHelper.AddRecord(bor.DefermentEndDate.ToShortDate(), row, ++column);
            xlHelper.AddRecord(bor.DefermentMonthCount.CheckMonth(), row, ++column);
            xlHelper.AddRecord(bor.ForbearanceBeginDate.ToShortDate(), row, ++column);
            xlHelper.AddRecord(bor.ForbearanceEndDate.ToShortDate(), row, ++column);
            xlHelper.AddRecord(bor.ForbearanceMonthCount.CheckMonth(), row, ++column);
            column = 1;
        }

        /// <summary>
        /// Gathers the data for each ssn in accounts
        /// </summary>
        private List<Borrower> LoadBorrowerData(List<Borrower> accounts)
        {
            List<Borrower> bors = new List<Borrower>();
            foreach (Borrower bor in accounts)
            {
                List<Borrower> loadedBorrower = DA.GetDefForbData(bor.Ssn);
                foreach (Borrower lBor in loadedBorrower)
                {
                    Console.WriteLine($"Found borrower: {bor.Ssn}; Award ID: {lBor.AwardId}");
                    lBor.SendingServicer = bor.SendingServicer;
                    lBor.ReceivingServicer = bor.ReceivingServicer;
                    lBor.DealId = bor.DealId;
                    lBor.SaleDate = bor.SaleDate;
                    bors.Add(lBor);
                }
            }
            return bors;
        }
    }

    public static class Validate
    {
        public static string ToShortDate(this DateTime d)
        {
            if (d.ToShortDateString() == "1/1/1900")
                return "";
            return d.Date.ToShortDateString();
        }

        public static string CheckMonth(this double i)
        {
            if (i == 0)
                return "";
            if (i.ToString().Contains(".") && i.ToString().Split('.')[1].Substring(1, 1).ToInt() > 5)
                i = Math.Round(i); //always round up the second decimal but not down
            return i.ToString("0.0");
        }
    }
}