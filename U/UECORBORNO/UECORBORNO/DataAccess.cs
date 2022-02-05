using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace UECORBORNO
{
    class DataAccess
    {
        private LogDataAccess LDR { get; set; }

        public DataAccess(int plID)
        {
            LDR = new LogDataAccess(DataAccessHelper.CurrentMode, plID, true, true);
        }

        /// <summary>
        /// Updated the emailed indicator to the current date and time
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.EcorrUheaa, "UpdateEmailedAt")]
        public void UpdateEmailedIndicator(int id)
        {
            LDR.Execute("UpdateEmailedAt", DataAccessHelper.Database.EcorrUheaa, SqlParams.Single("DocumentDetailsId", id));
        }

        /// <summary>
        /// Gets all unprocessed emails
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.EcorrUheaa, "GetEmailNotificationData")]
        public List<EmailData> GetEmailData()
        {
            return LDR.ExecuteList<EmailData>("GetEmailNotificationData", DataAccessHelper.Database.EcorrUheaa).CheckResult();
        }
    }
}
