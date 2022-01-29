using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace EMAILBTCF
{
    public class EmailCampaign
    {
        public int EmailCampaignId{ get; set; }
        public string SasFile { get; set; }
        public string LetterId { get; set; }
        public string SendingAddress { get; set; }
        public string SubjectLine { get; set; }
        public string Arc { get; set; }
        public string CommentText { get; set; }
        public DateTime? WorkLastLoaded { get; set; }
    }
}
