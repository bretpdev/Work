using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace CentralizedPrintingProcess
{
    public partial class EmailHandler : UserControl, IEmailHandler
    {
        private readonly BindingList<Email> Emails = new BindingList<Email>();
        private DateTime lastAutoSend = DateTime.Now;
        private const int AutoSendInterval = 15 * 60 * 1000;
        public DataAccess DataAccess { get; set; }

        public EmailHandler()
        {
            InitializeComponent();
            EmailsGrid.AutoGenerateColumns = false;
            EmailsGrid.DataSource = Emails;
            Emails.ListChanged += (o, ea) => SetButtonStatus();
            SetButtonStatus();
        }

        const string PENDING = "PENDING";
        const string SENT = "SENT";
        private class Email : INotifyPropertyChanged
        {
            public string Recipients { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public string Attachments { get; set; }
            public string Status { get; private set; }

            public Email()
            {
                Status = PENDING;
            }
            public void SetStatus(string status)
            {
                Status = status;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Status"));
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }

        /// <summary>
		/// Notify Business Unit of Centralized Printing Process Completion
		/// </summary>
		/// <param name="body">Body of email</param>
		/// <param name="subject">subject of email</param>
		/// <param name="businessUnit">business unit name</param>
		/// <param name="bsysEmailKey">email key</param>
		/// <param name="attachments">Name of attachments</param>
        public void AddEmail(string body, string subject, string businessUnit = "", string bsysEmailKey = "", string attachments = "")
        {
            string recipients = DataAccess.GetEmailRecipients(bsysEmailKey, businessUnit);
            Invoke(new Action(() =>
            {
                Emails.Add(new Email() { Title = subject, Body = body, Recipients = recipients, Attachments = attachments });
                SetButtonStatus();
            }));
        }

        public void SendAllEmails()
        {
            Invoke(() =>
            {
                foreach (var email in Emails.Where(o => o.Status == PENDING))
                {
                    EmailHelper.SendMail(DataAccessHelper.TestMode, email.Recipients, "CentralizedPrintingProcess@utahsbr.edu", email.Title, email.Body, "", EmailHelper.EmailImportance.Normal, true);
                    email.SetStatus(SENT);
                }
                lastAutoSend = DateTime.Now;
                SetButtonStatus();
            });
        }

        private void SetButtonStatus()
        {
            Invoke(() =>
            {
                var count = Emails.Count(o => o.Status == PENDING);
                SendButton.Text = $"Send {count} Pending Emails Now";
                SendButton.Enabled = count > 0;
            });
        }

        private void Invoke(Action a)
        {
            if (IsHandleCreated)
                base.Invoke(a);
            else
                a();
        }

        private void AutoSendTimer_Tick(object sender, EventArgs e)
        {
            var timeLeft = lastAutoSend.AddMilliseconds(AutoSendInterval) - DateTime.Now;
            AutoEmailsLabel.Text = $"All pending emails will be automatically sent in {(int)timeLeft.TotalMinutes:00}:{(int)(timeLeft.TotalSeconds % 60):00}.";
            if (timeLeft.TotalMilliseconds <= 0)
            {
                SendAllEmails();
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            SendAllEmails();
        }
    }
}