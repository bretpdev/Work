using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;


namespace NSLDSCONSO
{
    public class DataAccess
    {
        private LogDataAccess lda;
        private DB db = DB.Cls;
        public DataAccess(LogDataAccess lda)
        {
            this.lda = lda;
        }

        [UsesSproc(DB.Cls, "[nsldsconso].[StartDataLoadRun]")]
        public int StartDataLoadRun(int borrowerCount, string fileName)
        {
            return lda.ExecuteSingle<int>("[nsldsconso].[StartDataLoadRun]", db, Sp("BorrowerCount", borrowerCount), Sp("Filename", fileName)).Result;
        }

        [UsesSproc(DB.Cls, "[nsldsconso].[EndDataLoadRun]")]
        public void EndDataLoadRun(int dataLoadRunId)
        {
            lda.Execute("[nsldsconso].[EndDataLoadRun]", db, Sp("DataLoadRunId", dataLoadRunId));
        }

        [UsesSproc(DB.Cls, "[nsldsconso].[GetMostRecentDataLoadRun]")]
        public DataLoadRun GetMostRecentDataLoadRun()
        {
            return lda.ExecuteSingle<DataLoadRun>("[nsldsconso].[GetMostRecentDataLoadRun]", db).Result;
        }

        /// <summary>
        /// Migrates existing borrowers from OldDataLoadRunId to NewDataLoadRunId, and returns a list of the current SSNs in NewDataLoadRun
        /// </summary>
        [UsesSproc(DB.Cls, "[nsldsconso].[RecoveryMigration]")]
        public List<string> RecoveryMigration(int oldDataLoadRunId, int newDataLoadRunId)
        {
            return lda.ExecuteList<string>("[nsldsconso].[RecoveryMigration]", db, Sp("OldDataLoadRunId", oldDataLoadRunId), Sp("NewDataLoadRunId", newDataLoadRunId)).Result;
        }

        [UsesSproc(DB.Cls, "[nsldsconso].[InsertBorrower]")]
        public bool UploadBorrower(int dataLoadRunId, Borrower b)
        {
            var parms = new List<SqlParameter>();
            parms.AddRange(new SqlParameter[]
            {
                Sp("DataLoadRunId", dataLoadRunId), Sp("Ssn", b.Ssn), Sp("Name", b.Name), Sp("DateOfBirth", b.DateOfBirth), Sp("FileName", b.FileName)
            });
            parms.Add(Sp("BorrowerConsolidationLoans", b.ConsolidationLoans.ToDataTable()));
            parms.Add(Sp("BorrowerUnderlyingLoans", b.UnderlyingLoans.ToDataTable()));
            parms.Add(Sp("BorrowerUnderlyingLoanFunding", b.UnderlyingLoanFunding.ToDataTable()));
            return lda.Execute("[nsldsconso].[InsertBorrower]", db, parms.ToArray());
        }

        [UsesSproc(DB.Cls, "[nsldsconso].[CleanAndReseedTables]")]
        public bool CleanAndReseedTables()
        {
            return lda.Execute("[nsldsconso].[CleanAndReseedTables]", db);
        }

        [UsesSproc(DB.Cls, "[nsldsconso].[GetBorrowersToReport]")]
        public List<ReportBorrower> GetBorrowersToReport()
        {
            return lda.ExecuteList<ReportBorrower>("[nsldsconso].[GetBorrowersToReport]", db).Result;
        }

        [UsesSproc(DB.Cls, "[nsldsconso].[GetBorrowersWithUnmappedLoans]")]
        public List<UnmappedBorrower> GetBorrowersWithUnmappedLoans()
        {
            return lda.ExecuteList<UnmappedBorrower>("[nsldsconso].[GetBorrowersWithUnmappedLoans]", db).Result;
        }

        [UsesSproc(DB.Cls, "[nsldsconso].[GetBorrowerDetails]")]
        public BorrowerDetails GetBorrowerDetails(int borrowerId)
        {
            var details = new BorrowerDetails();
            using (var cmd = DataAccessHelper.GetCommand("[nsldsconso].[GetBorrowerDetails]", db))
            {
                cmd.Parameters.Add(Sp("BorrowerId", borrowerId));

                var reader = cmd.ExecuteReader();

                details.UnderlyingLoans = new List<BorrowerDetails.BorrowerUnderlyingLoan>();
                while (reader.Read())
                    details.UnderlyingLoans.Add(DataAccessHelper.Populate<BorrowerDetails.BorrowerUnderlyingLoan>(reader));

                reader.NextResult();
                details.AlternatelyCalculatedUnderlyingLoans = new List<BorrowerDetails.BorrowerUnderlyingLoan>();
                while (reader.Read())
                    details.AlternatelyCalculatedUnderlyingLoans.Add(DataAccessHelper.Populate<BorrowerDetails.BorrowerUnderlyingLoan>(reader));

                reader.NextResult();
                details.ConsolidationLoans = new List<BorrowerDetails.BorrowerConsolidationLoan>();
                while (reader.Read())
                    details.ConsolidationLoans.Add(DataAccessHelper.Populate<BorrowerDetails.BorrowerConsolidationLoan>(reader));

                reader.NextResult();
                details.Grsps = new List<BorrowerDetails.Grsp>();
                while (reader.Read())
                    details.Grsps.Add(DataAccessHelper.Populate<BorrowerDetails.Grsp>(reader));

                reader.NextResult();
                details.Grs2s = new List<BorrowerDetails.Grs2>();
                while (reader.Read())
                    details.Grs2s.Add(DataAccessHelper.Populate<BorrowerDetails.Grs2>(reader));

                reader.NextResult();
                reader.Read();
                details.MaxTotalAmount = reader.GetDecimal(0);
            }
            return details;
        }

        [UsesSproc(DB.Cls, "[nsldsconso].[MarkBorrowersAsReported]")]
        public bool UpdateBorrowerLoans(List<BorrowerUnderlyingLoanForUpdate> loans)
        {
            return lda.Execute("[nsldsconso].[UpdateBorrowerLoans]", db, Sp("BorrowerLoanUpdates", loans.ToDataTable()));
        }

        [UsesSproc(DB.Cls, "[nsldsconso].[MarkBorrowerAsReported]")]
        public bool MarkBorrowerAsReported(int borrowerId)
        {
            return lda.Execute("[nsldsconso].[MarkBorrowerAsReported]", db, Sp("BorrowerId", borrowerId));
        }

        [UsesSproc(DB.Cls, "[nsldsconso].[GetAllReportedNsldsLabels]")]
        public List<string> GetAllReportedNsldsLabels()
        {
            return lda.ExecuteList<string>("[nsldsconso].[GetAllReportedNsldsLabels]", db).Result;
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}
