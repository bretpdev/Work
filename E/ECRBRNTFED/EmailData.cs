using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace ECRBRNTFED
{
    class EmailData
    {
        public string AccountNumber { get; set; }
        public string EmailAddress { get; set; }
        public string EmailSubjectLine { get; set; }

        public EmailData()
        {
        }
        
        /// <summary>
        /// Updated the emailed indicator to the current date and time
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.ECorrFed, "UpdateEmailedAt")]
        public void UpdateEmailedIndicator()
        {
            DataAccessHelper.Execute("UpdateEmailedAt", Program.Conn, SqlParams.Single("AccountNumber", AccountNumber), SqlParams.Single("EmailAddress", EmailAddress),
                SqlParams.Single("EmailSubjectLine", EmailSubjectLine));
        }

        /// <summary>
        /// Gets all unprocessed emails
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.ECorrFed, "GetEmailNotificationData")]
        public static List<EmailData> GetEmailData()
        {
            return DataAccessHelper.ExecuteList<EmailData>("GetEmailNotificationData", DataAccessHelper.Database.ECorrFed);
        }
    }
}
