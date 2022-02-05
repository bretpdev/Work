using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
namespace BTCBPUHEAA
{
    class DataAccess
    {
        ProcessLogRun PLR { get; set; }
        public DataAccess(ProcessLogRun plr) => PLR = plr;
        /// <summary>
        /// Goes to the database and pulls a list of Pending Payments
        /// </summary>
        /// <returns>List of payment objects</returns>
        [UsesSproc(DataAccessHelper.Database.Norad, "spCKPH_GetCheckByPhonesToBeProcessed")]
        public List<Payment> GetPendingPayments()
        {
            return PLR.LDA.ExecuteList<Payment>("spCKPH_GetCheckByPhonesToBeProcessed", DataAccessHelper.Database.Norad).Result;
        }

        /// <summary>
        /// Goes to the database and sets processed date
        /// </summary>
        /// <returns>List of payment objects</returns>
        [UsesSproc(DataAccessHelper.Database.Norad, "spCKPH_UpdateRecordToProcessed")]
        public void UpdateProcessedDate(IEnumerable<int> recordNumbers)
        {
            foreach (int recordNumber in recordNumbers)
            {
                try
                {
                    PLR.LDA.Execute("spCKPH_UpdateRecordToProcessed", DataAccessHelper.Database.Norad, new SqlParameter("ID", recordNumber));
                }
                catch (Exception)
                {
                    PLR.AddNotification(string.Format("Failed to update processed date for record number {0} in NORAD.dbo.CKPH_DAT_OPSCheckByPhone", recordNumber), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
            }
        }

        [UsesSproc(DataAccessHelper.Database.Norad, "spCKPH_SetFileName")]
        public void SetFileName(Payment payment, string fileName, string folder)
        {
            PLR.LDA.Execute("spCKPH_SetFileName", DataAccessHelper.Database.Norad, new SqlParameter("ID", payment.RecNo), new SqlParameter("FileName", folder + "\"" + fileName));
        }
    }
}
