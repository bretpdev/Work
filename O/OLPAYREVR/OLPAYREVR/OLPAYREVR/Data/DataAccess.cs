using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace OLPAYREVR
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "olpayrevr.AddRecord")]
        public bool AddRecord(Payment data, DateTime? createdAt)
        {
            return LogRun.LDA.Execute("olpayrevr.AddRecord", DataAccessHelper.Database.Ols,
                SP("Ssn", data.Ssn),
                SP("PaymentAmount", data.PaymentAmount),
                SP("PaymentEffectiveDate", data.PaymentEffectiveDate),
                SP("PaymentType", data.PaymentType),
                SP("CreatedAt", createdAt));
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "olpayrevr.GetUnprocessedRecords")]
        public List<Payment> GetUnprocessedRecords()
        {
            return LogRun.LDA.ExecuteList<Payment>("olpayrevr.GetUnprocessedRecords", DataAccessHelper.Database.Ols).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "olpayrevr.UpdateTransactionInfo")]
        public bool AddDbTransactionInfo()
        {
            return LogRun.LDA.Execute("olpayrevr.UpdateTransactionInfo", DataAccessHelper.Database.Ols);
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "olpayrevr.SetProcessed")]
        public bool SetProcessed(int? processingQueueId, bool hadError = false, string errorDescription = null)
        {
            return LogRun.LDA.Execute("olpayrevr.SetProcessed", DataAccessHelper.Database.Ols, SP("ProcessingQueueId", processingQueueId), SP("HadError", hadError), SP("ErrorDescription", errorDescription.SafeSubString(0, 499)));
        }

        public SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}
