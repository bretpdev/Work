using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SYSTMLTRSU
{
    class DataAccess
    {
        private LogDataAccess LDA_Manage { get; set; }
        private LogDataAccess LDA { get; set;  }
        private ProcessLogRun PLR {get;set;}
        public DataAccess(ProcessLogRun plr)
        {
            LDA_Manage = new LogDataAccess(DataAccessHelper.CurrentMode, plr.ProcessLogId, false, true);
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, plr.ProcessLogId, false, false);
            PLR = plr;
            
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "IncrementProcessingAttempts")]
        public void IncrementProcessingAttempts(LT20Data letter)
        {
            foreach (int seq in letter.RN_SEQ_REC_PRC)
            {
                LDA_Manage.Execute("IncrementProcessingAttempts", DataAccessHelper.Database.Udw, SqlParams.Single("RM_APL_PGM_PRC", letter.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", letter.RT_RUN_SRT_DTS_PRC),
                    SqlParams.Single("RN_SEQ_LTR_CRT_PRC", letter.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", letter.RM_DSC_LTR_PRC), SqlParams.Single("DF_SPE_ACC_ID", letter.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", letter.IsCoborrower), SqlParams.Single("ProcessingAttempts", letter.ProcessingAttempts));
            }
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "AddCoborrowerRecords")]
        public int AddCoborrowerRecords()
        {
            return LDA.ExecuteSingle<int>("AddCoborrowerRecords", DataAccessHelper.Database.Udw).CheckResult();
        }

        /// <summary>
        /// Gets all unprocessed records.
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "GetAllUnprocessedLt20Records")]
        public List<LT20Data> GetUnprocessedLetters()
        {
            return LDA.ExecuteList<LT20Data>("GetAllUnprocessedLt20Records", DataAccessHelper.Database.Udw).CheckResult();
        }

        /// <summary>
        /// Gets all of the letter seq for each letter
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "GetLT20RecordSequences")]
        public List<int> GetLetterRecSeq(LT20Data data)
        {
            return LDA_Manage.ExecuteList<int>("GetLT20RecordSequences", DataAccessHelper.Database.Udw, SqlParams.Single("DF_SPE_ACC_ID", data.DF_SPE_ACC_ID), SqlParams.Single("RM_APL_PGM_PRC", data.RM_APL_PGM_PRC), 
                SqlParams.Single("RT_RUN_SRT_DTS_PRC", data.RT_RUN_SRT_DTS_PRC), SqlParams.Single("RN_SEQ_LTR_CRT_PRC", data.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RM_DSC_LTR_PRC", data.RM_DSC_LTR_PRC), SqlParams.Single("IsCoborrower", data.IsCoborrower)).CheckResult();
        }

        /// <summary>
        /// Inactivates all letters in the system letter exclusion table
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "InactivateLetters")]
        public bool InactivateInvalidLetters()
        {
            return LDA.Execute("InactivateLetters", DataAccessHelper.Database.Udw);
        }

        /// <summary>
        /// Inactivates a letter for a given reason
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="reason"></param>
        [UsesSproc(DataAccessHelper.Database.Udw, "InactivateLetter")]
        public void InactivateLetter(LT20Data letter, int reason)
        {
            foreach(int seq in letter.RN_SEQ_REC_PRC)
            {
                LDA_Manage.Execute("InactivateLetter", DataAccessHelper.Database.Udw, SqlParams.Single("RM_APL_PGM_PRC", letter.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", letter.RT_RUN_SRT_DTS_PRC),
                    SqlParams.Single("RN_SEQ_LTR_CRT_PRC", letter.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", letter.RM_DSC_LTR_PRC), SqlParams.Single("SystemLetterExclusionReasonId", reason), SqlParams.Single("DF_SPE_ACC_ID", letter.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", letter.IsCoborrower));
            }
        }

        /// <summary>
        /// Updates a letter to indicate it has been processed
        /// </summary>
        /// <param name="letter"></param>
        [UsesSproc(DataAccessHelper.Database.Udw, "MarkLT20LetterPrinted")]
        public void UpdatePrinted(LT20Data letter)
        {
            foreach (int seq in letter.RN_SEQ_REC_PRC)
            {
                LDA_Manage.Execute("MarkLT20LetterPrinted", DataAccessHelper.Database.Udw, SqlParams.Single("RM_APL_PGM_PRC", letter.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", letter.RT_RUN_SRT_DTS_PRC),
                    SqlParams.Single("RN_SEQ_LTR_CRT_PRC", letter.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", letter.RM_DSC_LTR_PRC), SqlParams.Single("DF_SPE_ACC_ID", letter.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", letter.IsCoborrower));
            }
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "MarkLT20EcorrCreated")]
        public void UpdateEcorrDocCreated(LT20Data letter)
        {
            foreach (int seq in letter.RN_SEQ_REC_PRC)
            {
                LDA_Manage.Execute("MarkLT20EcorrCreated", DataAccessHelper.Database.Udw, SqlParams.Single("RM_APL_PGM_PRC", letter.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", letter.RT_RUN_SRT_DTS_PRC),
                    SqlParams.Single("RN_SEQ_LTR_CRT_PRC", letter.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", letter.RM_DSC_LTR_PRC), SqlParams.Single("DF_SPE_ACC_ID", letter.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", letter.IsCoborrower));
            }
        }

        /// <summary>
        /// Gets the LetterStoredProcedureData needed for a given letter.
        /// </summary>
        /// <param name="letter"></param>
        /// <returns>List of LetterStoredProcedureData</returns>
        [UsesSproc(DataAccessHelper.Database.Bsys, "LTDBGetSprocsForGivenLetter")]
        public List<LetterStoredProcedureData> GetSprocForGivenLetter(string letter)
        {
            List<LetterStoredProcedureData> dataRecords = new List<LetterStoredProcedureData>();
            dataRecords = LDA_Manage.ExecuteList<LetterStoredProcedureData>("LTDBGetSprocsForGivenLetter", DataAccessHelper.Database.Bsys, letter.ToSqlParameter("LetterId")).CheckResult();
            return dataRecords;
        }

        public HeaderFooterData ExecuteAddressSproc(LT20Data letter, LetterStoredProcedureData sproc)
        {
            string identifier = "";
            if (letter.EndorsersSsn.IsPopulated())
                identifier = letter.EndorsersSsn;
            else if (letter.IsCoborrower)
                identifier = letter.CoborrowerSSN;
            else
                identifier = letter.RF_SBJ_PRC;

            HeaderFooterData addrInfo = LDA_Manage.ExecuteSingle<HeaderFooterData>(sproc.StoredProcedureName,DataAccessHelper.Database.Udw,
                SqlParams.Single("BF_SSN", identifier), SqlParams.Single("IsCoborrower", letter.IsCoborrower)).CheckResult();

            if (addrInfo == null && letter.ProcessingAttempts.Value > 2)//This is the third try, error if it fails
            {
                string message = string.Format("Unable to find address information for the following {0}: {1} letter id {2}", letter.IsCoborrower ? "Coborrower" : "Borrower", letter.DF_SPE_ACC_ID, letter.RM_DSC_LTR_PRC);
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                InactivateLetter(letter, 6);
                return null;
            }
            else if(addrInfo == null)
            {
                //return and skip record
                return null;
            }
            else
                addrInfo.BarcodeAccountNumber = letter.EndorsersSsn.IsPopulated() ? letter.EndorsersSsn : letter.DF_SPE_ACC_ID; //Used to get barcode account number to be comaker if it came from LT20_coborrower

            return addrInfo;
        }

        public DataTable ExecuteLoanDetailSproc(LT20Data letter,  LetterStoredProcedureData sproc)
        {
            DataTable ld = LDA_Manage.ExecuteDataTable(sproc.StoredProcedureName,DataAccessHelper.Database.Udw, false, SqlParams.Single("LetterId", letter.RM_DSC_LTR_PRC), SqlParams.Single("BF_SSN", letter.IsCoborrower ? letter.CoborrowerSSN : letter.RF_SBJ_PRC), SqlParams.Single("IsCoborrower", letter.IsCoborrower), SqlParams.Single("RN_ATY_SEQ_PRC", letter.RN_ATY_SEQ_PRC)).CheckResult();
            
            if ((ld == null || ld.Rows.Count == 0 || (ld.Rows.Count > 0 && ld?.Rows[0]?.ItemArray?.FirstOrDefault().GetType()?.Name == "DBNull")) && letter.ProcessingAttempts.Value > 2)
            {
                string message = string.Format("Unable to determine Loan Detail information for the following {0}: {1} letter id {2}", letter.IsCoborrower ? "Coborrower" : "Borrower", letter.DF_SPE_ACC_ID, letter.RM_DSC_LTR_PRC);
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);//adding to prevent a deadlock
                InactivateLetter(letter, 6);
                return null;
            }
            else if (ld == null || ld.Rows.Count == 0 || (ld.Rows.Count > 0 && ld?.Rows[0]?.ItemArray?.FirstOrDefault().GetType()?.Name == "DBNull"))
            {
                //return and skip record
                return null;
            }

            return ld;
        }

        public Dictionary<string, string> ExecuteFormFieldsSproc(LT20Data letter, LetterStoredProcedureData sproc )
        {
            DataTable dt = LDA_Manage.ExecuteDataTable(sproc.StoredProcedureName, DataAccessHelper.Database.Udw, false, SqlParams.Single("BF_SSN", letter.IsCoborrower ? letter.CoborrowerSSN : letter.RF_SBJ_PRC), SqlParams.Single("IsCoborrower", letter.IsCoborrower), SqlParams.Single("RN_ATY_SEQ_PRC", letter.RN_ATY_SEQ_PRC)).CheckResult();
            Dictionary<string, string> fields = new Dictionary<string, string>();

            if ((dt == null || dt.Rows.Count == 0 || (dt.Rows.Count > 0 && dt?.Rows[0]?.ItemArray?.FirstOrDefault().GetType()?.Name == "DBNull")) && letter.ProcessingAttempts.Value > 2)
            {
                string message = string.Format("Unable to determine Form Field information for the following {0}: {1} letter id {2}", letter.IsCoborrower ? "Coborrower" : "Borrower", letter.DF_SPE_ACC_ID, letter.RM_DSC_LTR_PRC);
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                InactivateLetter(letter, 6);
                return null;
            }
            else if (dt == null || dt.Rows.Count == 0 || (dt.Rows.Count > 0 && dt?.Rows[0]?.ItemArray?.FirstOrDefault().GetType()?.Name == "DBNull"))
            {
                //return and skip record
                return null;
            }

            for (int col = 0; col < dt.Columns.Count; col++)
                fields.Add(dt.Columns[col].ColumnName.ToUpper(), dt.Rows[0][col].ToString());

            return fields;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "spGetAccountNumberFromSsn")]
        public string GetEndorserAccountNumber(LT20Data letterToGenerate)
        {
            return DataAccessHelper.ExecuteSingle<string>("spGetAccountNumberFromSsn", DataAccessHelper.Database.Udw, letterToGenerate.EndorsersSsn.ToSqlParameter("Ssn"));
        }
    }
}
