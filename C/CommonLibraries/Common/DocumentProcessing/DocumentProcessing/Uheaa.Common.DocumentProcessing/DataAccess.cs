
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace Uheaa.Common.DocumentProcessing
{
    public static class DataAccess
    {
        [UsesSproc(DataAccessHelper.Database.Bsys, "spLTDBGetPathAndName")]
        public static DocumentPathAndName GetDocumentPathAndName(string letterId, DataAccessHelper.Region? region = null)
        {
           var currentRegion = region ?? DataAccessHelper.CurrentRegion;
            DocumentPathAndName docInfo = DataAccessHelper.GetContext(DataAccessHelper.Database.Bsys).ExecuteQuery<DocumentPathAndName>(@" EXEC spLTDBGetPathAndName {0}", letterId).SingleOrDefault();
            docInfo.CalculatedPath = docInfo.OriginalDBEntry.Substring(0, docInfo.OriginalDBEntry.LastIndexOf(@"\") + 1);
            docInfo.CalculatedFileName = docInfo.OriginalDBEntry.Substring(docInfo.OriginalDBEntry.LastIndexOf(@"\") + 1, (docInfo.OriginalDBEntry.Length - docInfo.OriginalDBEntry.LastIndexOf(@"\") - 1));
            if (DataAccessHelper.CurrentMode.IsIn(DataAccessHelper.Mode.Dev, DataAccessHelper.Mode.Test, DataAccessHelper.Mode.QA) && currentRegion == DataAccessHelper.Region.CornerStone)
            {
                docInfo.CalculatedPath = docInfo.CalculatedPath.Replace(@"Z:\", @"Y:\");
            }
            else if (DataAccessHelper.CurrentMode.IsIn(DataAccessHelper.Mode.Dev, DataAccessHelper.Mode.Test, DataAccessHelper.Mode.QA) && currentRegion == DataAccessHelper.Region.Uheaa)
            {
                docInfo.CalculatedPath = docInfo.OriginalDBEntry.Substring(0, docInfo.OriginalDBEntry.LastIndexOf(@"\") + 1) + @"Test\";
            }

            return docInfo;
        }
        [UsesSproc(DataAccessHelper.Database.Bsys, "spLTDBGetPageCount")]
        public static BarcodeQueryResults GetPageCount(string letterId)
        {
            return DataAccessHelper.ExecuteSingle<BarcodeQueryResults>("spLTDBGetPageCount", DataAccessHelper.Database.Bsys, new SqlParameter("LetterId", letterId));
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "spGetSSNFromAcctNumber")]
        public static string GetSsnFromFromAcctNo(string acctNum)
        {
            return DataAccessHelper.GetContext(DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ? DataAccessHelper.Database.Cdw : DataAccessHelper.Database.Udw).ExecuteQuery<string>(@"EXEC spGetSSNFromAcctNumber {0}", acctNum).SingleOrDefault();
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "spGetCostCenterInstructions")]
        public static string GetCostCenterInstructions(string letterId)
        {
            return DataAccessHelper.GetContext(DataAccessHelper.Database.Bsys).ExecuteQuery<string>(@"EXEC spGetCostCenterInstructions {0}", letterId).Single();
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "spGetLetterName")]
        public static string GetLetterName(string letterId)
        {
            return DataAccessHelper.GetContext(DataAccessHelper.Database.Bsys).ExecuteQuery<string>(@"EXEC spGetLetterName {0}", letterId).FirstOrDefault();
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "spGetUserInfoForCentralizedPrint")]
        public static CentralizedPrintingAnd2DBarcodeInfo GetUserInfoForCentralizedPrintAnd2DObject()
        {
            var result = DataAccessHelper.GetContext(DataAccessHelper.Database.Bsys).ExecuteQuery<CentralizedPrintingAnd2DBarcodeInfo>("EXEC spGetUserInfoForCentralizedPrint {0}", Environment.UserName).SingleOrDefault();
            if (result == null)
                throw new NullReferenceException("GetUserInfoForCentralizedPrintAnd2DObject returned nothing for user " + Environment.UserName);
            return result;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "spPRNT_CreateLetterRec")]
        public static LetterRecordCreationResults CreateLetterRecordForCentralizedPrinting(string letterId, string accountNumber, string businessUnit, string domesticState, DataAccessHelper.Region? region = null)
        {
            SqlCommand comm;
            using (comm = new SqlCommand("spPRNT_CreateLetterRec", new SqlConnection(DataAccessHelper.GetContext(DataAccessHelper.Database.Bsys).Connection.ConnectionString)))
            {
                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add(new SqlParameter { ParameterName = "@LetterID", Value = letterId, Direction = ParameterDirection.Input, Size = 10 });
                comm.Parameters.Add(new SqlParameter { ParameterName = "@AcctNum", Value = accountNumber, Direction = ParameterDirection.Input, Size = 20 });
                comm.Parameters.Add(new SqlParameter { ParameterName = "@BU", Value = businessUnit, Direction = ParameterDirection.Input, Size = 50 });
                comm.Parameters.Add(new SqlParameter { ParameterName = "@Dom", Value = domesticState, Direction = ParameterDirection.Input, Size = 2 });
                comm.Parameters.Add(new SqlParameter { ParameterName = "@NewRecNum", Direction = ParameterDirection.Output, Size = 8 });
                comm.Parameters.Add(new SqlParameter { ParameterName = "@BarcodeSeqNum", Direction = ParameterDirection.Output, Size = 8 });
                comm.Connection.Open();
                comm.ExecuteScalar();

                LetterRecordCreationResults r = new LetterRecordCreationResults() { BarcodeSeqNum = long.Parse(comm.Parameters["@BarcodeSeqNum"].Value.ToString()), NewRecordIdentity = long.Parse(comm.Parameters["@NewRecNum"].Value.ToString()) };

                comm.Connection.Close();
                return r;
            }
        }
    }
}
