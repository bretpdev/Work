using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace DOCIDUHEAA
{
    public class DataAccess
    {
        public LogDataAccess ULDA { get; set; }
        public List<Borrower> UdwBorrowers { get; set; }
        public List<Borrower> OdwBorrowers { get; set; }
        public List<Borrower> UdwEnd { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            ULDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, true, false, DataAccessHelper.Region.Uheaa);
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
                Uheaa(accountIdentifier),
                Onelink(accountIdentifier)
            };

            foreach (Thread t in threads)
                t.Join();

            if (UdwBorrowers != null)
                bors.AddRange(UdwBorrowers);

            if (OdwBorrowers != null)
                bors.AddRange(OdwBorrowers);

            return bors;
        }

        /// <summary>
        /// Searches UDW on its own thread for the borrower demographics
        /// </summary>
        /// <param name="accountIdentifier"></param>
        /// <returns></returns>
        [UsesSproc(Udw, "docid.GetBorrowerDemographics")]
        public Thread Uheaa(string accountIdentifier)
        {
            Thread ut = new Thread(() =>
            UdwBorrowers = ULDA.ExecuteList<Borrower>("docid.GetBorrowerDemographics", Udw,
                SP("AccountIdentifier", accountIdentifier)).Result);
            ut.Start();

            return ut;
        }

        /// <summary>
        /// Searches OneLink on its own thread for the borrower demographics
        /// </summary>
        /// <param name="accountIdentifier"></param>
        /// <returns></returns>
        [UsesSproc(Odw, "GetBorrowerDemographics")]
        public Thread Onelink(string accountIdentifier)
        {
            Thread ot = new Thread(() =>
            OdwBorrowers = ULDA.ExecuteList<Borrower>("GetBorrowerDemographics", Odw,
                SP("AccountIdentifier", accountIdentifier)).Result);
            ot.Start();

            return ot;
        }

        /// <summary>
        /// Searches all the warehouses for the case number to get the borrower name and account identifier
        /// </summary>
        /// <param name="caseNumber">Case number found on incoming document</param>
        /// <returns>All borrower and account identifiers found for the given case number</returns>
        [UsesSproc(Odw, "docid.CaseNumberSearch")]
        [UsesSproc(Udw, "docid.CaseNumberSearch")]
        public List<Borrower> CaseSearch(string caseNumber)
        {
            if (caseNumber.IsNullOrEmpty())
                return new List<Borrower>();
            List<Borrower> foundCases = new List<Borrower>();

            foundCases.AddRange(ULDA.ExecuteList<Borrower>("docid.CaseNumberSearch", Udw, SP("CaseNumber", caseNumber)).Result);
            foundCases.AddRange(ULDA.ExecuteList<Borrower>("docid.CaseNumberSearch", Odw, SP("CaseNumber", caseNumber)).Result);

            return foundCases.DistinctBy(p => p.AccountIdentifier).ToList();
        }

        /// <summary>
        /// Gets the Doc ID's for the region the borrower is in.
        /// </summary>
        [UsesSproc(Uls, "docid.GetDocIds")]
        public List<Doc> GetDocIds() =>
            ULDA.ExecuteList<Doc>("docid.GetDocIds", Uls).Result;

        /// <summary>o
        /// Adds the processed data to the database
        /// </summary>
        [UsesSproc(Uls, "docid.InsertProcessedRecord")]
        public void InsertProcessedRecord(ProcessedDocuments record, int ArcAddProcessingId) =>
            ULDA.Execute("docid.InsertProcessedRecord", Uls,
                SP("AccountIdentifier", record.AccountIdentifier),
                SP("Document", record.Document),
                SP("Source", record.Source),
                SP("ArcAddProcessingId", ArcAddProcessingId),
                SP("AddedBy", Environment.UserName));


        /// <summary>
        /// Gets all the processed records for both regions for the date selected
        /// </summary>
        [UsesSproc(Uls, "docid.GetProcessedRecords")]
        public List<ProcessedDocuments> GetProcessedRecords(DateTime selectedDate) =>
            ULDA.ExecuteList<ProcessedDocuments>("docid.GetProcessedRecords", Uls,
                SP("SelectedDate", selectedDate)).Result;

        /// <summary>
        /// Gets the date of the earliest processed record for both regions
        /// </summary>
        [UsesSproc(Uls, "docid.GetEarliestDate")]
        public DateTime GetEarliestDate() =>
            ULDA.ExecuteSingle<DateTime?>("docid.GetEarliestDate", Uls).Result.Value;

        [UsesSproc(Udw, "docid.GetBorrowersForEndorser")]
        public List<Borrower> GetBorrowersForEndorser(string accountIdentifier) =>
            ULDA.ExecuteList<Borrower>("docid.GetBorrowersForEndorser", Udw,
                SP("Endorser", accountIdentifier)).Result;

        private SqlParameter SP(string name, object value) => SqlParams.Single(name, value);
    }
}