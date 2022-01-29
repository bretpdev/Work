using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMAILBTCF
{
    public class CampaignData
    {
        public int CampaignDataId { get; set; }
        public int EmailCampaignId { get; set; }
        public string Recipient { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? EmailSentAt { get; set;}
        public DateTime? ArcProcessedAt { get; set; }
        public string LineData { get; set; }
    }
}
