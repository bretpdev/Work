using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System.Data;

namespace SOALETTERU
{
    public class DataAccess
    {
        private ProcessLogRun PLR { get; set; }
        private LogDataAccess LDA { get; set; }
        public DataAccess(ProcessLogRun plr)
        {
            PLR = plr;
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, PLR.ProcessLogId, false, true);
        }

        /// <summary>
        /// Identifies duplicate letter requests and inactivates all the letter requests
        /// other than the original requests.
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "soaletteru.InactivateDuplicateRequests")]
        public void InactivateDuplicateLetterRequests()
        {
            LDA.Execute("soaletteru.InactivateDuplicateRequests", DataAccessHelper.Database.Udw);
        }


        /// <summary>
        /// Gets all unprocessed records.
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "soaletteru.GetUnprocessedRecords")]
        public List<LT20Data> GetUnprocessedLetters()
        {
            return LDA.ExecuteList<LT20Data>("soaletteru.GetUnprocessedRecords", DataAccessHelper.Database.Udw).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "soaletteru.UpdateEcorrDocumentCreatedAt")]
        public void UpdateEcorrDocCreated(LT20Data record)
        {
            foreach (int seq in record.RN_SEQ_REC_PRC)
            {
                LDA.Execute("soaletteru.UpdateEcorrDocumentCreatedAt", DataAccessHelper.Database.Udw, SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC),
                        SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC), SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower));
            }
        }

        /// <summary>
        /// Gets all of the letter seq for each letter
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "GetLT20RecordSequences")]
        public List<int> GetLetterRecSeq(LT20Data record)
        {
            return LDA.ExecuteList<int>("GetLT20RecordSequences", DataAccessHelper.Database.Udw, SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID), SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC),
                SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC), SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC), SqlParams.Single("IsCoborrower", record.IsCoborrower)).CheckResult();
        }

        /// <summary>
        /// Inactivates a letter for a given reason
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="reason"></param>
        [UsesSproc(DataAccessHelper.Database.Udw, "soaletteru.InactivateLetter")]
        public void InactivateLetter(LT20Data record, int reason)
        {
            foreach (int seq in record.RN_SEQ_REC_PRC)
            {
                LDA.Execute("soaletteru.InactivateLetter", DataAccessHelper.Database.Udw, SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC),
                    SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC), SqlParams.Single("SystemLetterExclusionReasonId", reason), SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower));
            }
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "soaletteru.AddCoborrowerRecords")]
        public int AddCoborrowerRecords()
        {
            return LDA.ExecuteSingle<int>("soaletteru.AddCoborrowerRecords", DataAccessHelper.Database.Udw).CheckResult();
        }

        /// <summary>
        /// Updates a letter to indicate it has been processed
        /// </summary>
        /// <param name="letter"></param>
        [UsesSproc(DataAccessHelper.Database.Udw, "soaletteru.UpdatePrinted")]
        public void UpdatePrinted(LT20Data record)
        {
            foreach (int seq in record.RN_SEQ_REC_PRC)
            {
                LDA.Execute("soaletteru.UpdatePrinted", DataAccessHelper.Database.Udw, SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC),
                    SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC), SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower));
            }
        }

        /// <summary>
        /// Gets the borrowers loans for the loan detail
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "soaletteru.GetLoanInformation")]
        public DataTable GetLoanInfo(LT20Data record)
        {
            return LDA.ExecuteDataTable("soaletteru.GetLoanInformation", DataAccessHelper.Database.Udw, true, SqlParams.Single("AccountNumber", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower), SqlParams.Single("BorrowerSSN", record.BorrowerSSN)).CheckResult();
        }

        /// <summary>
        /// Gets all financial transactions for the loan detail
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "soaletteru.GetFinancialTransactions")]
        public DataTable GetFinancialTransactions(LT20Data record)
        {
            return LDA.ExecuteDataTable("soaletteru.GetFinancialTransactions", DataAccessHelper.Database.Udw, true, SqlParams.Single("AccountNumber", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower), SqlParams.Single("BorrowerSSN", record.BorrowerSSN)).CheckResult();
        }

        /// <summary>
        /// Gets the borrowers address info for the letter
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "soaletteru.GetAddress")]
        public AddressInfo GetAddress(LT20Data record)
        {
            return LDA.ExecuteSingle<AddressInfo>("soaletteru.GetAddress", DataAccessHelper.Database.Udw, SqlParams.Single("AccountNumber", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower",record.IsCoborrower)).CheckResult();
        }
    }
}
