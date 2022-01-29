using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ECORRSLFED
{
    class DataAccess 
    {
        private LogDataAccess LDA_Manage { get; set; }
        private LogDataAccess LDA { get; set; }
        private ProcessLogRun PLR { get; set; }
        public DataAccess(ProcessLogRun plr)
        {
            LDA_Manage = new LogDataAccess(DataAccessHelper.CurrentMode, plr.ProcessLogId, false, true);
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, plr.ProcessLogId, false, false);
            PLR = plr;

        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "IncrementProcessingAttempts")]
        public void IncrementProcessingAttempts(LT20Data letter)
        {
            foreach (int seq in letter.RN_SEQ_REC_PRC)
            {
                LDA_Manage.Execute("IncrementProcessingAttempts", DataAccessHelper.Database.Cdw, SqlParams.Single("RM_APL_PGM_PRC", letter.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", letter.RT_RUN_SRT_DTS_PRC),
                    SqlParams.Single("RN_SEQ_LTR_CRT_PRC", letter.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", letter.RM_DSC_LTR_PRC), SqlParams.Single("DF_SPE_ACC_ID", letter.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", letter.IsCoborrower), SqlParams.Single("ProcessingAttempts", letter.ProcessingAttempts));
            }
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "AddCoborrowerRecords")]
        public int AddCoborrowerRecords()
        {
            return LDA.ExecuteSingle<int>("AddCoborrowerRecords", DataAccessHelper.Database.Cdw).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "AddCoborrowerRecordsCondensed")]
        public int AddCoborrowerRecordsCondensed()
        {
            return LDA.ExecuteSingle<int>("AddCoborrowerRecordsCondensed", DataAccessHelper.Database.Cdw).CheckResult();
        }

        /// <summary>
        /// Gets all unprocessed records.
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Cdw, "GetAllUnprocessedLt20Records")]
        public List<LT20Data> GetUnprocessedLetters()
        {
            return LDA.ExecuteList<LT20Data>("GetAllUnprocessedLt20Records", DataAccessHelper.Database.Cdw).CheckResult();
        }

        /// <summary>
        /// Gets all of the letter seq for each letter
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Cdw, "GetLT20RecordSequences")]
        public List<int> GetLetterRecSeq(LT20Data data)
        {
            return LDA_Manage.ExecuteList<int>("GetLT20RecordSequences", DataAccessHelper.Database.Cdw, SqlParams.Single("DF_SPE_ACC_ID", data.DF_SPE_ACC_ID), SqlParams.Single("RM_APL_PGM_PRC", data.RM_APL_PGM_PRC),
                SqlParams.Single("RT_RUN_SRT_DTS_PRC", data.RT_RUN_SRT_DTS_PRC), SqlParams.Single("RN_SEQ_LTR_CRT_PRC", data.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RM_DSC_LTR_PRC", data.RM_DSC_LTR_PRC), SqlParams.Single("IsCoborrower", data.IsCoborrower)).CheckResult();
        }

        //[UsesSproc(DataAccessHelper.Database.Cls, "CondenseDuplicateLetters")]
        //public bool CondenseDuplicateLetters()
        //{
        //    return LDA.Execute("CondenseDuplicateLetters", DataAccessHelper.Database.Cls);
        //}

        /// <summary>
        /// Inactivates all letters in the system letter exclusion table
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Cdw, "ecorrslfed.GetInactivationStoredProcedures")]
        public bool InactivateInvalidLetters()
        {
            List<bool> results = new List<bool>();
            List<string> sprocs = LDA.ExecuteList<string>("ecorrslfed.GetInactivationStoredProcedures", DataAccessHelper.Database.Cdw).Result;
            if (sprocs.Any())
            {
                foreach (string sproc in sprocs)
                {
                    results.Add(LDA.Execute(sproc, DataAccessHelper.Database.Cdw));
                }

                if(results.Any(p => p == false)) //Consider any sproc failure as a failure overall
                    return false;

                return true;
            }
            else
                return true;
        }

        /// <summary>
        /// Inactivates a letter for a given reason
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="reason"></param>
        [UsesSproc(DataAccessHelper.Database.Cdw, "InactivateLetter")]
        public void InactivateLetter(LT20Data letter, int reason)
        {
            foreach (int seq in letter.RN_SEQ_REC_PRC)
            {
                LDA_Manage.Execute("InactivateLetter", DataAccessHelper.Database.Cdw, SqlParams.Single("RM_APL_PGM_PRC", letter.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", letter.RT_RUN_SRT_DTS_PRC),
                    SqlParams.Single("RN_SEQ_LTR_CRT_PRC", letter.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", letter.RM_DSC_LTR_PRC), SqlParams.Single("SystemLetterExclusionReasonId", reason), SqlParams.Single("DF_SPE_ACC_ID", letter.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", letter.IsCoborrower));
            }
        }

        /// <summary>
        /// Updates a letter to indicate it has been processed
        /// </summary>
        /// <param name="letter"></param>
        [UsesSproc(DataAccessHelper.Database.Cdw, "MarkLT20LetterPrinted")]
        public void UpdatePrinted(LT20Data letter)
        {
            foreach (int seq in letter.RN_SEQ_REC_PRC)
            {
                LDA_Manage.Execute("MarkLT20LetterPrinted", DataAccessHelper.Database.Cdw, SqlParams.Single("RM_APL_PGM_PRC", letter.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", letter.RT_RUN_SRT_DTS_PRC),
                    SqlParams.Single("RN_SEQ_LTR_CRT_PRC", letter.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", letter.RM_DSC_LTR_PRC), SqlParams.Single("DF_SPE_ACC_ID", letter.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", letter.IsCoborrower));
            }
        }

        /// <summary>
        /// Updates a letter to indicate it has been processed
        /// </summary>
        /// <param name="letter"></param>
        [UsesSproc(DataAccessHelper.Database.Cdw, "MarkLT20EcorrCreated")]
        public void UpdateEcorrDocCreated(LT20Data letter)
        {
            foreach (int seq in letter.RN_SEQ_REC_PRC)
            {
                LDA_Manage.Execute("MarkLT20EcorrCreated", DataAccessHelper.Database.Cdw, SqlParams.Single("RM_APL_PGM_PRC", letter.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", letter.RT_RUN_SRT_DTS_PRC),
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

        /// <summary>
        /// Gets the EndorserAccountNumber needed for a given letter.
        /// </summary>
        /// <param name="letter"></param>
        /// <returns>string</returns>
        [UsesSproc(DataAccessHelper.Database.Cdw, "spGetAccountNumberFromSsn")]
        public string GetEndorserAccountNumber(LT20Data letterToGenerate)
        {
            return DataAccessHelper.ExecuteSingle<string>("spGetAccountNumberFromSsn", DataAccessHelper.Database.Cdw, letterToGenerate.EndorsersSsn.ToSqlParameter("Ssn"));
        }

        public HeaderFooterData ExecuteAddressSproc(LT20Data letter, LetterStoredProcedureData sproc)
        {
            HeaderFooterData addrInfo = LDA_Manage.ExecuteSingle<HeaderFooterData>(sproc.StoredProcedureName, DataAccessHelper.Database.Cdw,
                SqlParams.Single("AccountNumber", letter.EndorsersAccountNumber.IsNullOrEmpty() ? letter.DF_SPE_ACC_ID : letter.EndorsersAccountNumber), SqlParams.Single("IsCoborrower", letter.IsCoborrower)).CheckResult();
            if (addrInfo == null && letter.ProcessingAttempts.Value > 2)
            {
                var severity = NotificationSeverityType.Critical;
                if (IsCODBorrower(letter.RF_SBJ_PRC))
                    severity = NotificationSeverityType.Informational;
                string message = string.Format("Unable to find address information for the following {0}: {1} letter id {2}", letter.IsCoborrower ? "Coborrower" : "Borrower", letter.DF_SPE_ACC_ID, letter.RM_DSC_LTR_PRC);
                PLR.AddNotification(message, NotificationType.ErrorReport, severity);
                InactivateLetter(letter, 6);
                return null;
            }
            else if(addrInfo == null)
            {
                //Skipping record
                return null;
            }
            else
                addrInfo.BarcodeAccountNumber = letter.DF_SPE_ACC_ID; //Used to get barcode account number to be comaker if it came from LT20_coborrower

            return addrInfo;
        }

        /// <summary>
        /// Checks to see if this borrower is a COD borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "CheckIfCODBorrower")]
        public bool IsCODBorrower(string ssn)
        {
            return LDA.ExecuteSingle<int>("CheckIfCODBorrower", DataAccessHelper.Database.Cdw, SqlParams.Single("SSN", ssn)).CheckResult() > 0;
        }

        public DataTable ExecuteLoanDetailSproc(LT20Data letter, LetterStoredProcedureData sproc)
        {
            DataTable ld = LDA_Manage.ExecuteDataTable(sproc.StoredProcedureName, DataAccessHelper.Database.Cdw, false, SqlParams.Single("AccountNumber", letter.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", letter.IsCoborrower)).CheckResult();

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

        public Dictionary<string, string> ExecuteFormFieldsSproc(LT20Data letter, LetterStoredProcedureData sproc)
        {
            string accountNumber = letter.DF_SPE_ACC_ID;
            if (letter.EndorsersAccountNumber.IsPopulated())
                accountNumber = letter.EndorsersAccountNumber;
            DataTable dt = LDA_Manage.ExecuteDataTable(sproc.StoredProcedureName, DataAccessHelper.Database.Cdw, false, SqlParams.Single("AccountNumber", accountNumber), SqlParams.Single("IsCoborrower", letter.IsCoborrower)).CheckResult();
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

        [UsesSproc(DataAccessHelper.Database.Cdw, "UpdateEndorserEcorrOnRecord")]
        internal void UpdateEndorserEcorrOnRecord(LT20Data record)
        {
            foreach (int seq in record.RN_SEQ_REC_PRC)
            {
                LDA_Manage.Execute("UpdateEndorserEcorrOnRecord", DataAccessHelper.Database.Cdw,
                    SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID),
                    SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC),
                    SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC),
                    SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC),
                    SqlParams.Single("RN_SEQ_REC_PRC", seq),
                    SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC),
                    SqlParams.Single("OnEcorr", record.OnEcorr));
            }
        }
    }
}
