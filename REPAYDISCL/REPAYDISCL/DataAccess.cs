using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;

namespace REPAYDISCL
{
    public static class DataAccess
    {
        private static SqlConnection UlsConn { get; set; }
        private static SqlConnection BsysConn { get; set; }

        static DataAccess()
        {
            UlsConn = new SqlConnection(DataAccessHelper.GetConnectionString(DataAccessHelper.Database.Uls, DataAccessHelper.CurrentMode));
            BsysConn = new SqlConnection(DataAccessHelper.GetConnectionString(DataAccessHelper.Database.Bsys, DataAccessHelper.CurrentMode));
            UlsConn.Open();
            BsysConn.Open();
        }
        /// <summary>
        /// Gets a list of all unprocessed borrowers
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetPendingAccountsForRepaymentDisc")]
        public static List<Borrower> GetUnprocessedBorrowerData(string ScriptId)
        {
            return DataAccessHelper.ExecuteList<Borrower>("[print].GetPendingAccountsForRepaymentDisc", UlsConn);
        }

        /// <summary>
        /// Gets the line data for the current borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetLineDataForAccount")]
        public static List<string> GetLineData(int printProcessingId)
        {
            return DataAccessHelper.ExecuteList<string>("[print].GetLineDataForAccount", UlsConn, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Sets the printing complete for the current borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetPrintingCompleteWithTable")]
        public static void SetPrintComplete(DataTable printProcIds )
        {
            DataAccessHelper.Execute("[print].SetPrintingCompleteWithTable", UlsConn, SqlParams.Single("PrintProcessingId", printProcIds));
        }

        /// <summary>
        /// Sets the Comment complete for the current borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetArcAddComplete")]
        public static void SetCommentComplete(int printProcId)
        {
            DataAccessHelper.Execute("[print].SetArcAddComplete", UlsConn, SqlParams.Single("PrintProcessingId", printProcId));
        }

        /// <summary>
        /// Sets the Imaging complete for the current borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetImagedAtWithTable")]
        public static void SetImageComplete(DataTable printProcIds)
        {
            DataAccessHelper.Execute("[print].SetImagedAtWithTable", UlsConn, SqlParams.Single("PrintProcessingId", printProcIds));
        }


        /// <summary>
        /// Sets the data record date when the document is created.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetDocumentCreatedAtWithTable")]
        public static void SetDocumentCreated(DataTable printProcIds)
        {
            DataAccessHelper.Execute("[print].SetDocumentCreatedAtWithTable", UlsConn, SqlParams.Single("PrintProcessingId", printProcIds));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetDocIds")]
        public static List<string> GetAllDocIds()
        {
            return DataAccessHelper.ExecuteList<string>("[print].GetDocIds", DataAccessHelper.Database.Uls);
        }

        /// <summary>
        /// Closes the Sql Connections
        /// </summary>
        public static void CloseConnections()
        {
            UlsConn.Close();
            UlsConn.Dispose();
            BsysConn.Close();
            BsysConn.Dispose();
        }
    }
}