using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Reflection;
using System.Data;

namespace SOALTRFED
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

        [UsesSproc(DataAccessHelper.Database.Cdw, "soaltrfed.GetUnprocessedRecords")]
        public List<LT20Data> GetUprocessedRecords()
        {
            return LDA.ExecuteList<LT20Data>("soaltrfed.GetUnprocessedRecords", DataAccessHelper.Database.Cdw).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "GetLT20RecordSequences")]
        public List<int> GetLetterRecSeq(LT20Data record)
        {
            return LDA.ExecuteList<int>("GetLT20RecordSequences", DataAccessHelper.Database.Cdw, SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID), SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC), SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC),
                SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC), SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("IsCoborrower", record.IsCoborrower)).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "soaltrfed.InactivateLetter")]
        public void InactivateLetter(LT20Data record, int reason)
        {
            foreach (int seq in record.RN_SEQ_REC_PRC)
            {
                LDA.Execute("soaltrfed.InactivateLetter", DataAccessHelper.Database.Cdw, SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC),
                    SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC), SqlParams.Single("SystemLetterExclusionReasonId", reason), SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower));
            }
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "soaltrfed.AddCoborrowerRecords")]
        public int AddCoborrowerRecords()
        {
            return LDA.ExecuteSingle<int>("soaltrfed.AddCoborrowerRecords", DataAccessHelper.Database.Cdw).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "soaltrfed.UpdatePrinted")]
        [UsesSproc(DataAccessHelper.Database.Cdw, "soaltrfed.UpdateEcorrDocumentCreatedAt")]
        public void UpdateProcessed(LT20Data record, bool onEcorr)
        {
            if (!onEcorr)
            {
                foreach (int seq in record.RN_SEQ_REC_PRC)
                {
                    LDA.Execute("soaltrfed.UpdatePrinted", DataAccessHelper.Database.Cdw, SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC),
                        SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC), SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower));
                }
            }
            else
            {
                foreach (int seq in record.RN_SEQ_REC_PRC)
                {
                    LDA.Execute("soaltrfed.UpdateEcorrDocumentCreatedAt", DataAccessHelper.Database.Cdw, SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC),
                            SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC), SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower));
                }
            }
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "soaltrfed.GetLoanInformation")]
        public DataTable GetLoanInfo(LT20Data record)
        {
            return LDA.ExecuteDataTable("soaltrfed.GetLoanInformation", DataAccessHelper.Database.Cdw, true, SqlParams.Single("AccountNumber", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower), SqlParams.Single("BorrowerSSN", record.BorrowerSSN)).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "soaltrfed.GetFinancialTransactions")]
        public DataTable GetFinancialTransactions(LT20Data record)
        {
            return LDA.ExecuteDataTable("soaltrfed.GetFinancialTransactions", DataAccessHelper.Database.Cdw, true, SqlParams.Single("AccountNumber", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower), SqlParams.Single("BorrowerSSN", record.BorrowerSSN)).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "soaltrfed.GetAddress")]
        public AddressInfo GetAddress(LT20Data record)
        {
            return LDA.ExecuteSingle<AddressInfo>("soaltrfed.GetAddress", DataAccessHelper.Database.Cdw, SqlParams.Single("AccountNumber", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower)).CheckResult();
        }
    }
}
