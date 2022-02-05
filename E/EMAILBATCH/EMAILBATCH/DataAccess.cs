using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace EMAILBATCH
{
    class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        public DataAccess(int processLogId)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, processLogId, false, true);
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "emailbatch.GetCampaignsToLoad")]
        public List<EmailCampaigns>GetAllCampaigns()
        {
            return LDA.ExecuteList<EmailCampaigns>("emailbatch.GetCampaignsToLoad", DataAccessHelper.Database.Uls).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[emailbatch].GetEmailData")]
        public List<EmailProcessingData> GetAll()
        {
            return LDA.ExecuteList<EmailProcessingData>("emailbatch.GetEmailData", DataAccessHelper.Database.Uls).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "emailbatch.UpdateEmailSentAt")]
        public void UpdateEmailProcessedAt(int emailProcessingId)
        {
            LDA.Execute("emailbatch.UpdateEmailSentAt", DataAccessHelper.Database.Uls, SqlParams.Single("EmailProcessingId", emailProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "spGetSSNFromAcctNumber")]
        public string GetSSNFromAccountNumber(string accountNumber)
        {
            return LDA.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Udw, SqlParams.Single("AccountNumber", accountNumber)).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "emailbatch.UpdateArcAddProcessingId")]
        public void UpdateArcAddedAt(int emailProcessingId, int arcAddProcessingId)
        {
            LDA.Execute("emailbatch.UpdateArcAddProcessingId", DataAccessHelper.Database.Uls, SqlParams.Single("EmailProcessingId", emailProcessingId), SqlParams.Single("ArcAddProcessingId", arcAddProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "emailbatch.AddProcessNotification")]
        public void AddProcessNotificationId(int emailProcessingId, long processNotficationId)
        {
            LDA.Execute("emailbatch.AddProcessNotification", DataAccessHelper.Database.Uls, SqlParams.Single("EmailProcessingId", emailProcessingId), SqlParams.Single("ProcessNotificationId", processNotficationId));
        }
    }
}
