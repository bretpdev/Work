using SubSystemShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    abstract partial class BaseNeedHelpTicketDetail : UserControl
    {
        protected TicketAndFlowData Data
        {
            get
            {
                if (TheTicket == null)
                { return null; }
                else
                { return TheTicket.Data; }
            }
        }
        public Ticket TheTicket { get; set; }
        protected string UserID { get; set; }
        public string DataFile { get; set; }
        public ProcessLogRun LogRun { get; set; }

        public BaseNeedHelpTicketDetail()
        {
        }

        public abstract TicketData GetModifiedTicketData(Ticket activeTicket, List<SqlUser> users, NotifyType.Type type);

        public abstract void BindNewTicket(Ticket activeTicket);
        public abstract void SetIssueToReadOnly(bool isReadOnly);
        public abstract void SetEmailRecipientDataSource(List<SqlUser> emailUser);
        public abstract void AddUploadedFile(AttachedFile file);

        public event EventHandler<PriorityOptionEventArgs> CategoryChanged;
        protected virtual void OnCategoryChanged(object sender, PriorityOptionEventArgs e)
        {
            if (CategoryChanged != null) { CategoryChanged(sender, e); }
        }

        public event EventHandler<PriorityOptionEventArgs> UrgencyChanged;
        protected virtual void OnUrgencyChanged(object sender, PriorityOptionEventArgs e)
        {
            if (UrgencyChanged != null) { UrgencyChanged(sender, e); }
        }

        public class PriorityOptionEventArgs : EventArgs
        {
            public readonly string Category;
            public readonly string Urgency;
            public readonly string TicketType;

            public PriorityOptionEventArgs(string category, string urgency, string ticketType)
            {
                Category = category;
                Urgency = urgency;
                TicketType = ticketType;
            }
        }

        public event EventHandler<EmailRecipientEvantArgs> EmailRecipientChanged;
        protected virtual void OnEmailRecipientChanged(object sender, EmailRecipientEvantArgs e)
        {
            if (EmailRecipientChanged != null) { EmailRecipientChanged(sender, e); }
        }

        public class EmailRecipientEvantArgs : EventArgs
        {
            public readonly List<SqlUser> EmailUsers;

            public EmailRecipientEvantArgs(List<SqlUser> emailUsers)
            {
                EmailUsers = emailUsers;
            }
        }

        protected class DisplayUser
        {
            public string LegalName { get; set; }
            public int ID { get; set; }

            public DisplayUser(SqlUser user)
            {
                LegalName = user.FirstName + " " + user.LastName;
                ID = user.ID;
            }
        }

        public virtual void SetPriority() { }

        protected void LoadHistoryData()
        {
            DataFile = EnterpriseFileSystem.TempFolder + "History.txt";
            using (StreamWriter sw = new StreamWriter(DataFile, false))
            {
                sw.WriteLine("CurrentDate,TicketNumber,TicketType,History");
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", DateTime.Now.ToShortDateString(), TheTicket.Data.TheTicketData.TicketID, TheTicket.Data.TheTicketData.TicketCode, TheTicket.Data.TheTicketData.TicketID + " - " + TheTicket.Data.TheTicketData.TicketCode +
                       "Ticket " + TheTicket.Data.TheTicketData.History.Replace("\r\n\r\n", "\r\n").Replace("\"", "\"\"")));
            }
        }

        protected SqlUser GetUSingleUser(string fullName, List<SqlUser> users)
        {
            SqlUser user = new SqlUser();
            try
            {
                user = users.Where(p => p.LegalName == fullName).SingleOrDefault();
            }
            catch (Exception ex)
            {
                string message = string.Format("{0} is in CSYS more than once or they have not been set up in CSYS yet. Contact Systems Support to have them fix this issue.", fullName);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                return null;
            }
            return user;
        }
    }
}