using System;
using System.Collections.Generic;
using Microsoft.Exchange.WebServices.Data;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Data.SqlClient;
using System.Linq;

namespace RTNEMLINVF
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }
        public LogDataAccess LDA { get; set; }
        public DataAccessHelper.Database DB { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
            LDA = logRun.LDA;
            DB = DataAccessHelper.Database.Uls;
        }

        /// <summary>
        /// Gets the SSN and Email Type for the email address in the UDW PD32 table
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Udw, "rtnemlinvf.GetSsnAndEmailType")]
        public List<Borrower> GetSsnFromEmailUdw(string email)
        {
            return LDA.ExecuteList<Borrower>("rtnemlinvf.GetSsnAndEmailType", DataAccessHelper.Database.Udw,
            SP("Email", email)).Result;
        }

        /// <summary>
        /// Gets the SSN for the email address in the ODW PD03 table
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Odw, "rtnemlinvf.GetSsnFromEmail")]
        public List<Borrower> GetSsnFromEmailOdw(string email)
        {
            return LDA.ExecuteList<Borrower>("rtnemlinvf.GetSsnFromEmail", DataAccessHelper.Database.Odw,
            SP("Email", email)).Result;
        }

        /// <summary>
        /// Gets the Email addresses that need to be searched for invalid emails.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "rtnemlinvf.GetEmailAddresses")]
        public List<string> GetEmailAddresses()
        {
            return LDA.ExecuteList<string>("rtnemlinvf.GetEmailAddresses", DB).Result;
        }

        /// <summary>
        /// Adds the email and borrower data to the database to be processed.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "rtnemlinvf.InsertInvalidEmail")]
        public bool AddEmailToTable(EmailMessage message, Borrower bor)
        {
            try
            {
                LDA.Execute("rtnemlinvf.InsertInvalidEmail", DB,
                    SP("Ssn", bor.Ssn),
                    SP("EmailType", bor.EmailType),
                    SP("ReceivedBy", message.ToRecipients.FirstOrDefault().Address),
                    SP("Subject", message.Subject),
                    SP("ReceivedDate", message.DateTimeReceived));
                return true;
            }
            catch (Exception ex)
            {
                LogRun.AddNotification($"There was an error adding Email: {message.ReceivedBy.Address}, Email Type: {bor.EmailType} to the InvalidEmail table for Borrower: {bor.Ssn}", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return false;
            }
        }

        /// <summary>
        /// Gets the list of emails that need to be invalidated and deleted.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "rtnemlinvf.GetInvalidEmail")]
        public List<InvalidEmail> GetInvalidEmail()
        {
            return LDA.ExecuteList<InvalidEmail>("rtnemlinvf.GetInvalidEmail", DB).Result;
        }

        /// <summary>
        /// Set the ArcAddProcessingId after the ARC has been added to ArcAddProcessing.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "rtnemlinvf.SetArcAddId")]
        public void SetArcAddId(int invalidEmailId, long arcAddId)
        {
            LDA.Execute("rtnemlinvf.SetArcAddId", DB,
                SP("InvalidEmailId", invalidEmailId),
                SP("ArcAddId", arcAddId));
        }

        /// <summary>
        /// Sets the account as invalidated in the database
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "rtnemlinvf.SetInvalidatedAt")]
        public void SetInvalidatedAt(int invalidEmailId)
        {
            LDA.Execute("rtnemlinvf.SetInvalidatedAt", DB,
                SP("InvalidEmailId", invalidEmailId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "rtnemlinvf.SetDeletedAt")]
        public void DeleteRecord(int invalidEmailId)
        {
            LDA.Execute("rtnemlinvf.SetDeletedAt", DB,
                SP("InvalidEmailId", invalidEmailId));
        }

        public SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}