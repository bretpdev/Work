using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Reflection;

namespace FINALBFED
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

        [UsesSproc(DataAccessHelper.Database.Cdw, "finalbfed.GetUnprocessedRecords")]
        public List<LT20Data> GetUprocessedRecords()
        {
            return LDA.ExecuteList<LT20Data>("finalbfed.GetUnprocessedRecords", DataAccessHelper.Database.Cdw).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "GetLT20RecordSequences")]
        public List<int> GetLetterRecSeq(LT20Data record)
        {
            return LDA.ExecuteList<int>("GetLT20RecordSequences", DataAccessHelper.Database.Cdw, SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID), SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC),
                SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC), SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC), SqlParams.Single("IsCoborrower", record.IsCoborrower)).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "finalbfed.InactivateLetter")]
        public void InactivateLetter(LT20Data record, int reason)
        {
            foreach (int seq in record.RN_SEQ_REC_PRC)
            {
                LDA.Execute("finalbfed.InactivateLetter", DataAccessHelper.Database.Cdw, SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC),
                    SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC), SqlParams.Single("SystemLetterExclusionReasonId", reason), SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower));
            }
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "finalbfed.AddCoborrowerRecords")]
        public int AddCoborrowerRecords()
        {
            return LDA.ExecuteSingle<int>("finalbfed.AddCoborrowerRecords", DataAccessHelper.Database.Cdw).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "finalbfed.UpdatePrinted")]
        [UsesSproc(DataAccessHelper.Database.Cdw, "finalbfed.UpdateEcorrDocumentCreatedAt")]
        public void UpdateProcessed(LT20Data record, bool onEcorr)
        {
            if (!onEcorr)
            {
                foreach (int seq in record.RN_SEQ_REC_PRC)
                {
                    LDA.Execute("finalbfed.UpdatePrinted", DataAccessHelper.Database.Cdw, SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC),
                        SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC), SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower));
                }
            }
            else
            {
                foreach (int seq in record.RN_SEQ_REC_PRC)
                {
                    LDA.Execute("finalbfed.UpdateEcorrDocumentCreatedAt", DataAccessHelper.Database.Cdw, SqlParams.Single("RM_APL_PGM_PRC", record.RM_APL_PGM_PRC), SqlParams.Single("RT_RUN_SRT_DTS_PRC", record.RT_RUN_SRT_DTS_PRC),
                            SqlParams.Single("RN_SEQ_LTR_CRT_PRC", record.RN_SEQ_LTR_CRT_PRC), SqlParams.Single("RN_SEQ_REC_PRC", seq), SqlParams.Single("RM_DSC_LTR_PRC", record.RM_DSC_LTR_PRC), SqlParams.Single("DF_SPE_ACC_ID", record.DF_SPE_ACC_ID), SqlParams.Single("IsCoborrower", record.IsCoborrower));
                }
            }
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "finalbfed.GetLoanInformation")]
        public List<LoanInformation> GetBorrowersLoanInfo(LT20Data record)
        {
            return LDA.ExecuteList<LoanInformation>("finalbfed.GetLoanInformation", DataAccessHelper.Database.Cdw, new SqlParameter("AccountNumber", record.DF_SPE_ACC_ID), new SqlParameter("IsCoborrower", record.IsCoborrower)).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "finalbfed.GetAddress")]
        public AddressInfo GetAddress(LT20Data record)
        {
            return LDA.ExecuteSingle<AddressInfo>("finalbfed.GetAddress", DataAccessHelper.Database.Cdw, new SqlParameter("AccountNumber", record.DF_SPE_ACC_ID), new SqlParameter("IsCoborrower", record.IsCoborrower)).CheckResult();
        }
    }
}