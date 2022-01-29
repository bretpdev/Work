using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace CLMPMTPST
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }

        [UsesSproc(DB.Uls, "clmpmtpst.AddClaimPayments")]
        public bool AddClaimPayments()
        {
            return LDA.ExecuteSingle<bool>("clmpmtpst.AddClaimPayments", DB.Uls).Result;
        }

        [UsesSproc(DB.Uls, "clmpmtpst.AddError")]
        public bool AddError()
        {
            return LDA.Execute("clmpmtpst.AddError", DB.Uls);
        }

        [UsesSproc(DB.Uls, "clmpmtpst.Cleanup")]
        public bool Cleanup()
        {
            return LDA.Execute("clmpmtpst.Cleanup", DB.Uls);
        }

        [UsesSproc(DB.Uls, "clmpmtpst.GetUnprocessedClaimPayments")]
        public List<ClaimPayment> GetUnprocessedClaimPayments()
        {
            return LDA.ExecuteList<ClaimPayment>("clmpmtpst.GetUnprocessedClaimPayments", DB.Uls).Result;
        }

        [UsesSproc(DB.Uls, "clmpmtpst.SetDeletedAt")]
        public bool SetDeleted(int claimPaymentId)
        {
            return LDA.Execute("clmpmtpst.SetDeletedAt", DB.Uls, SP("ClaimPaymentId", claimPaymentId));
        }

        [UsesSproc(DB.Uls, "clmpmtpst.SetProcessedAt")]
        public bool SetProcessed(int claimPaymentId)
        {
            return LDA.Execute("clmpmtpst.SetProcessedAt", DB.Uls, SP("ClaimPaymentId", claimPaymentId));
        }

        /// <summary>
        /// SQL parameterization wrapper: 
        /// parameterizes a string as the field name and
        /// an object as the value to be used for DB calls.
        /// </summary>
        public SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}
