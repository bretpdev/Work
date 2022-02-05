using System;
using Uheaa.Common.DataAccess;

namespace EMAILBTCF
{
    public class LineData
    {
        public int LineDataId { get; set; }
        public int EmailCampaignId { get; set; }
        public string AccountNumber { get; set; }
        public string Recipient { get; set; }
        [DbName("LineData")]
        public string Data { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? EmailSentAt { get; set; }
        public DateTime? ArcProcessedAt { get; set; }

    }
}