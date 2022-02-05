using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;

namespace CFSARPTFED
{
    class DataAccess
    {
        /// <summary>
        /// Will gather all of the reports from the dbo.CreateFsaReports table.
        /// </summary>
        /// <returns></returns>
        public static List<ReportData> GetReportData()
        {
            return DataAccessHelper.ExecuteList<ReportData>("spGetFsaReports", DataAccessHelper.Database.Cls);
        }

        /// <summary>
        /// Gets the FY to date numbers for the previous month.
        /// </summary>
        /// <param name="dl">bool indicator if for DL or FFEL loans</param>
        /// <returns></returns>
        public static LastMonthData GetYearToDateNumbers(bool dl)
        {
            //The FY for fed starts in Oct.
            if (DateTime.Now.Month != 11)
                return DataAccessHelper.ExecuteSingle<LastMonthData>("spCreateFsaReportsGetYearToDateNumbers", DataAccessHelper.Database.Cls, new SqlParameter("CornerStone", dl));
            else
                return new LastMonthData();
        }

        /// <summary>
        /// Updates the FY to date data in the dbo.BankruptcyReport database
        /// </summary>
        /// <param name="borrowers">Number of borrowers</param>
        /// <param name="loans">Number of loans</param>
        /// <param name="dollars">Total amount</param>
        /// <param name="dl">bool indicator if for DL or FFEL</param>
        public static void UpdateTotals(int borrowers, int loans, decimal dollars, bool dl)
        {
            using (SqlCommand cmd = DataAccessHelper.GetCommand("spCreateFsaReportsUpdateTotals", DataAccessHelper.Database.Cls))
            {
                cmd.Parameters.AddRange(new SqlParameter[] {
                    new SqlParameter("Borrowers", borrowers), 
                    new SqlParameter("Loans", loans),
                    new SqlParameter("Dollars", dollars),
                    new SqlParameter("CornerStone", dl)});
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
        }
    }
}
