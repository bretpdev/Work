using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLoggerRC;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace RCEMAILPUL
{
    class DataAccess
    {
        public const string SOURCE = "sendgrid";

        LogDataAccess LDA;
        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }

        [UsesSproc(DB.Voyager, "rcemailpul.AddSendGridHistory")]
        public SendGridHistoryRecord AddOrUpdate(SendGridMessage message)
        {
            var result = LDA.ExecuteList<SendGridHistoryRecord>("rcemailpul.AddSendGridHistory", DB.Voyager,
                Sp("FromEmail", message.from_email),
                Sp("MsgId", message.msg_id),
                Sp("Subject", message.subject),
                Sp("ToEmail", message.to_email),
                Sp("Status", message.status),
                Sp("OpensCount", message.opens_count),
                Sp("ClicksCount", message.clicks_count),
                Sp("LastEventTime", message.last_event_time)
             );
            var existingHistory = result.Result.SingleOrDefault();
            return existingHistory;
        }

        public List<EmailAndSource> GetUnsubscribedEmails()
        {
            return LDA.ExecuteList<EmailAndSource>("rcemailpul.GetUnsubscribedEmails", DB.Voyager).Result;
        }

        public bool AddUnsubscribedEmail(string email)
        {
            return LDA.Execute("rcemailpul.AddUnsubscribedEmail", DB.Voyager, Sp("EmailAddress", email), Sp("Source", SOURCE));
        }

        public bool RemoveUnsubscribedEmail(string email)
        {
            return LDA.Execute("rcemailpul.RemoveUnsubscribedEmail", DB.Voyager, Sp("EmailAddress", email), Sp("Source", SOURCE));
        }

        private SqlParameter Sp(string name, object val) => SqlParams.Single(name, val);
    }
}
