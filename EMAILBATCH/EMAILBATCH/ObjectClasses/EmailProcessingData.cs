using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace EMAILBATCH
{
    class EmailProcessingData
    {
        public int EmailProcessingId { get; set; }
        public int EmailCampaignId { get; set; }
        public string AccountNumber { get; set; }
        public string EmailData { get; set; }
        public string HTMLFile { get; set; }
        public string ActualFile { get; set; }
        public string FromAddress { get; set; }
        public string SubjectLine { get; set; }
        public string Arc { get; set; }
        public string Comment { get; set; }
        public string ActivityType { get; set; }
        public string ActivityContact { get; set; }
        public bool ArcNeeded { get; set; }
        public int? ArcAddProcessingId { get; set; }
        public DateTime? EmailSentAt { get; set; }
        public string Name
        {
            get
            {
                if (EmailData.IsNullOrEmpty())
                    return null;
                else
                    return EmailData.SplitAndRemoveQuotes(",")[1];
            }
        }
        public string Recipient
        {
            get
            {
                if (EmailData.IsNullOrEmpty())
                    return null;
                else
                    if(DataAccessHelper.TestMode)
                    return Environment.UserName + "@utahsbr.edu";
                return EmailData.SplitAndRemoveQuotes(",")[2];
            }
        }
    }
}
