using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DOCIDFED
{
    public class DataAccess
    {
        public LogDataAccess CLDA { get; set; }
        public LogDataAccess ULDA { get; set; }
        public List<Borrower> Cdw { get; set; }
        public List<Borrower> Udw { get; set; }
        public List<Borrower> Odw { get; set; }
        public List<Borrower> CdwEnd { get; set; }
        public List<Borrower> UdwEnd { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            CLDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, true, false, DataAccessHelper.Region.CornerStone);
            ULDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, true, false, DataAccessHelper.Region.Uheaa);
            CdwEnd = new List<Borrower>();
            UdwEnd = new List<Borrower>();
        }

        /// <summary>
        /// Checks all three warehouses and returns a list of all borrower demographics information
        /// </summary>
        /// <param name="accountIdentifier">SSN or Account Number</param>
        /// <returns>Borrower demographics for all regions they are in</returns>
        public List<Borrower> GetBorrowerDemographics(string accountIdentifier)
        {
            List<Borrower> bors = new List<Borrower>();
            List<Thread> threads = new List<Thread>
            {
                CornerStone(accountIdentifier),
                Uheaa(accountIdentifier),
                Onelink(accountIdentifier)
            };

            foreach (Thread t in threads)
                t.Join();

            if (Cdw != null)
            {
                foreach (Borrower bor in Cdw)
                    bor.IsFederal = true;
                bors.AddRange(Cdw);
            }

            if (Udw != null)
                bors.AddRange(Udw);

            if (Odw != null)
                bors.AddRange(Odw);

            return bors;
        }

        /// <summary>
        /// Searches CDW in its own thread for the borrower demographics
        /// </summary>
        /// <param name="accountIdentifier"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Cdw, "docid.GetBorrowerDemographics")]
        public Thread CornerStone(string accountIdentifier)
        {
            Thread ct = new Thread(() =>
            Cdw = CLDA.ExecuteList<Borrower>("docid.GetBorrowerDemographics", DataAccessHelper.Database.Cdw,
                SqlParams.Single("AccountIdentifier", accountIdentifier)).Result);
            ct.Start();

            return ct;
        }

        /// <summary>
        /// Searches UDW on its own thread for the borrower demographics
        /// </summary>
        /// <param name="accountIdentifier"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "docid.GetBorrowerDemographics")]
        public Thread Uheaa(string accountIdentifier)
        {
            Thread ut = new Thread(() =>
            Udw = ULDA.ExecuteList<Borrower>("docid.GetBorrowerDemographics", DataAccessHelper.Database.Udw,
                SqlParams.Single("AccountIdentifier", accountIdentifier)).Result);
            ut.Start();

            return ut;
        }

        /// <summary>
        /// Searches OneLink on its own thread for the borrower demographics
        /// </summary>
        /// <param name="accountIdentifier"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Odw, "GetBorrowerDemographics")]
        public Thread Onelink(string accountIdentifier)
        {
            Thread ot = new Thread(() =>
            Odw = ULDA.ExecuteList<Borrower>("GetBorrowerDemographics", DataAccessHelper.Database.Odw,
                SqlParams.Single("AccountIdentifier", accountIdentifier)).Result);
            ot.Start();

            return ot;
        }

        /// <summary>
        /// Searches all the warehouses for the case number to get the borrower name and account identifier
        /// </summary>
        /// <param name="caseNumber">Case number found on incoming document</param>
        /// <returns>All borrower and account identifiers found for the given case number</returns>
        [UsesSproc(DataAccessHelper.Database.Cdw, "docid.CaseNumberSearch")]
        [UsesSproc(DataAccessHelper.Database.Odw, "docid.CaseNumberSearch")]
        [UsesSproc(DataAccessHelper.Database.Udw, "docid.CaseNumberSearch")]
        public List<Borrower> CaseSearch(string caseNumber)
        {
            if (caseNumber.IsNullOrEmpty())
                return new List<Borrower>();
            List<Borrower> foundCases = new List<Borrower>();

            foundCases.AddRange(CLDA.ExecuteList<Borrower>("docid.CaseNumberSearch", DataAccessHelper.Database.Cdw, SqlParams.Single("CaseNumber", caseNumber)).Result);
            foundCases.AddRange(ULDA.ExecuteList<Borrower>("docid.CaseNumberSearch", DataAccessHelper.Database.Udw, SqlParams.Single("CaseNumber", caseNumber)).Result);
            foundCases.AddRange(ULDA.ExecuteList<Borrower>("docid.CaseNumberSearch", DataAccessHelper.Database.Odw, SqlParams.Single("CaseNumber", caseNumber)).Result);

            return foundCases.DistinctBy(p => p.AccountIdentifier).ToList();
        }

        /// <summary>
        /// Gets the Doc ID's for the region the borrower is in.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "docid.GetDocIds")]
        [UsesSproc(DataAccessHelper.Database.Uls, "docid.GetDocIds")]
        public List<Doc> GetDocIds(bool isFederal)
        {
            List<Doc> ids = new List<Doc>();
            if (isFederal)
                ids.AddRange(CLDA.ExecuteList<Doc>("docid.GetDocIds", DataAccessHelper.Database.Cls).Result);
            else
                ids.AddRange(CLDA.ExecuteList<Doc>("docid.GetDocIds", DataAccessHelper.Database.Uls).Result);
            return ids;
        }

        /// <summary>o
        /// Adds the processed data to the database
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "docid.InsertProcessedRecord")]
        [UsesSproc(DataAccessHelper.Database.Uls, "docid.InsertProcessedRecord")]
        public void InsertProcessedRecord(ProcessedDocuments record, int ArcAddProcessingId)
        {
            DataAccessHelper.Database db = DataAccessHelper.Database.Uls;
            LogDataAccess log = ULDA;
            if (record.IsFederal)
            {
                db = DataAccessHelper.Database.Cls;
                log = CLDA;
            }
            log.Execute("docid.InsertProcessedRecord", db,
                SqlParams.Single("AccountIdentifier", record.AccountIdentifier),
                SqlParams.Single("Document", record.Document),
                SqlParams.Single("Source", record.Source),
                SqlParams.Single("ArcAddProcessingId", ArcAddProcessingId),
                SqlParams.Single("AddedBy", Environment.UserName));
        }

        /// <summary>
        /// Gets all the processed records for both regions for the date selected
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "docid.GetProcessedRecords")]
        [UsesSproc(DataAccessHelper.Database.Uls, "docid.GetProcessedRecords")]
        public List<ProcessedDocuments> GetProcessedRecords(DateTime selectedDate)
        {
            List<ProcessedDocuments> records = new List<ProcessedDocuments>();

            records.AddRange(CLDA.ExecuteList<ProcessedDocuments>("docid.GetProcessedRecords", DataAccessHelper.Database.Cls,
                SqlParams.Single("SelectedDate", selectedDate)).Result);

            //Mark all the records found in CLS as Federal before pulling the records from CLS
            foreach (ProcessedDocuments doc in records)
                doc.IsFederal = true;

            records.AddRange(ULDA.ExecuteList<ProcessedDocuments>("docid.GetProcessedRecords", DataAccessHelper.Database.Uls,
                SqlParams.Single("SelectedDate", selectedDate)).Result);

            return records;
        }

        /// <summary>
        /// Gets the date of the earliest processed record for both regions
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "docid.GetEarliestDate")]
        [UsesSproc(DataAccessHelper.Database.Uls, "docid.GetEarliestDate")]
        public DateTime GetEarliestDate()
        {
            DateTime? ulsDate = ULDA.ExecuteSingle<DateTime?>("docid.GetEarliestDate", DataAccessHelper.Database.Uls).Result;
            DateTime? clsDate = CLDA.ExecuteSingle<DateTime?>("docid.GetEarliestDate", DataAccessHelper.Database.Cls).Result;

            return new DateTime?[] { ulsDate, clsDate }.Min().Value;
        }

        public List<Borrower> GetBorrowersForEndorser(string accountIdentifier)
        {
            List<Borrower> bors = new List<Borrower>();
            List<Thread> threads = new List<Thread>();
            threads.Add(CornerstoneEnd(accountIdentifier));
            threads.Add(UheaaEnd(accountIdentifier));
            //threads.Add(Onelink(accountIdentifier));

            foreach (Thread t in threads)
                t.Join();

            if (CdwEnd != null)
            {
                foreach (Borrower bor in CdwEnd)
                    bor.IsFederal = true;
                bors.AddRange(CdwEnd);
            }

            if (UdwEnd != null)
                bors.AddRange(UdwEnd);

            return bors;
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "docid.GetBorrowersForEndorser")]
        public Thread CornerstoneEnd(string accountIdentifier)
        {
            Thread ct = new Thread(() =>
            CdwEnd = CLDA.ExecuteList<Borrower>("docid.GetBorrowersForEndorser", DataAccessHelper.Database.Cdw,
                SqlParams.Single("Endorser", accountIdentifier)).Result);
            ct.Start();

            return ct;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "docid.GetBorrowersForEndorser")]
        public Thread UheaaEnd(string accountIdentifier)
        {
            Thread ct = new Thread(() =>
            UdwEnd = ULDA.ExecuteList<Borrower>("docid.GetBorrowersForEndorser", DataAccessHelper.Database.Udw,
                SqlParams.Single("Endorser", accountIdentifier)).Result);
            ct.Start();

            return ct;
        }

    }
}