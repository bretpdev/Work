using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;
using Uheaa.Common;

namespace PRETNFRNOT
{
    public class DataAccess
    {
        LogDataAccess lda;
        public DataAccess(LogDataAccess lda)
        {
            this.lda = lda;
        }
        /// <summary>
        /// Gets a list of all the data for the pre-transfer letters
        /// </summary>
        [UsesSproc(DB.Cdw, "pretnfrnot.GetPreTransferData")]
        public List<BorrowerData> GetDataToProcess(string letterId)
        {
            return lda.ExecuteList<BorrowerData>("pretnfrnot.GetPreTransferData", DB.Cdw, Sp("LetterId", letterId)).Result;
        }

        /// <summary>
        /// Returns the paragraph for the given id
        /// </summary>
        [UsesSproc(DB.Cls, "pretnfrnot.GetPreTransferParagraph")]
        public string GetParagraph(int paragraphId)
        {
            return lda.ExecuteSingle<string>("pretnfrnot.GetPreTransferParagraph", DB.Cls, Sp("PreTransferParagraphId", paragraphId)).Result;
        }

        [UsesSproc(DB.Cdw, "pretnfrnot.AddCoborrowerRecords")]
        public int AddCoborrowerRecords(string letter)
        {
            return lda.ExecuteSingle<int>("pretnfrnot.AddCoborrowerRecords", DB.Cdw, Sp("LetterId", letter)).Result;
        }

        /// <summary>
        /// Updates the LT20 table in the warehouse showing that the letter ecorr document was created
        /// </summary>
        [UsesSproc(DB.Cdw, "pretnfrnot.SetLetterEcorrCreated")]
        public void SetLetterEcorrCreated(BorrowerData borr)
        {
            lda.Execute("pretnfrnot.SetLetterEcorrCreated", DB.Cdw,
                Sp("RM_APL_PGM_PRC", borr.RM_APL_PGM_PRC),
                Sp("RT_RUN_SRT_DTS_PRC", borr.RT_RUN_SRT_DTS_PRC),
                Sp("RN_SEQ_LTR_CRT_PRC", borr.RN_SEQ_LTR_CRT_PRC),
                Sp("RN_SEQ_REC_PRC", borr.RN_SEQ_REC_PRC),
                Sp("RM_DSC_LTR_PRC", borr.LetterId),
                Sp("DF_SPE_ACC_ID", borr.AccountNumber),
                Sp("IsCoborrower", borr.IsCoborrower));
        }

        /// <summary>
        /// Updates the LT20 table in the warehouse showing that the letter was printed
        /// </summary>
        [UsesSproc(DB.Cdw, "pretnfrnot.SetLetterPrinted")]
        public void SetPrintedAt(BorrowerData borr)
        {
            lda.Execute("pretnfrnot.SetLetterPrinted", DB.Cdw,
                Sp("RM_APL_PGM_PRC", borr.RM_APL_PGM_PRC),
                Sp("RT_RUN_SRT_DTS_PRC", borr.RT_RUN_SRT_DTS_PRC),
                Sp("RN_SEQ_LTR_CRT_PRC", borr.RN_SEQ_LTR_CRT_PRC),
                Sp("RN_SEQ_REC_PRC", borr.RN_SEQ_REC_PRC),
                Sp("RM_DSC_LTR_PRC", borr.LetterId),
                Sp("DF_SPE_ACC_ID", borr.AccountNumber),
                Sp("IsCoborrower", borr.IsCoborrower));
        }

        /// <summary>
        /// Sets the letter to inactive
        /// </summary>
        [UsesSproc(DB.Cdw, "pretnfrnot.SetLetterInvalid")]
        public void SetLetterInvalid(BorrowerData borr)
        {
            lda.Execute("pretnfrnot.SetLetterInvalid", DB.Cdw,
                Sp("RM_APL_PGM_PRC", borr.RM_APL_PGM_PRC),
                Sp("RT_RUN_SRT_DTS_PRC", borr.RT_RUN_SRT_DTS_PRC),
                Sp("RN_SEQ_LTR_CRT_PRC", borr.RN_SEQ_LTR_CRT_PRC),
                Sp("RN_SEQ_REC_PRC", borr.RN_SEQ_REC_PRC),
                Sp("RM_DSC_LTR_PRC", borr.LetterId),
                Sp("DF_SPE_ACC_ID", borr.AccountNumber),
                Sp("IsCoborrower", borr.IsCoborrower));
        }

        /// <summary>
        /// Gets the new servicer data for the region
        /// </summary>
        [UsesSproc(DB.Cls, "pretnfrnot.GetPreTransferServicer")]
        public NewServicer GetNewServicer(string regionDeconversion)
        {
            return lda.ExecuteSingle<NewServicer>("pretnfrnot.GetPreTransferServicer", DB.Cls, Sp("RegionDeconversion", regionDeconversion)).Result;
        }

        /// <summary>
        /// Gets the question and answers that will be merged into the Ecorr document
        /// </summary>
        [UsesSproc(DB.Cls, "pretnfrnot.GetPreTransferServicerQA")]
        public Questions GetQAData(string regionDeconversion, string transferDate)
        {
            transferDate = transferDate.ToDate().ToString("MM/dd/yyyy");
            return lda.ExecuteSingle<Questions>("pretnfrnot.GetPreTransferServicerQA", DB.Cls, Sp("RegionDeconversion", regionDeconversion), Sp("TransferDate", transferDate)).Result;
        }

        /// <summary>
        /// Gets the question and answers that will be merged into the Ecorr document
        /// </summary>
        [UsesSproc(DB.Cls, "pretnfrnot.GetPreTransferServicerQA_TS06BPRTCH")]
        public Questions GetQAData_TS06BPRTCH(string regionDeconversion, string transferDate)
        {
            transferDate = transferDate.ToDate().ToString("MM/dd/yyyy");
            return lda.ExecuteSingle<Questions>("pretnfrnot.GetPreTransferServicerQA_TS06BPRTCH", DB.Cls, Sp("RegionDeconversion", regionDeconversion), Sp("TransferDate", transferDate)).Result;
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}