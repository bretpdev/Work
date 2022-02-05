using System.Collections.Generic;

namespace TEXTCOORD
{
    public class Campaigns
    {
        public int CampaignId { get; set; }
        public string Campaign { get; set; }
        public string CampaignCode { get; set; }
        public string Sproc { get; set; }

        public List<CampaignDisabledUiField> DisabledUiFields { get; set; } = new List<CampaignDisabledUiField>();
    }
}