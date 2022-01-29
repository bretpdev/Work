using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMAILBTCF
{
    public class CampaignDataForUpload
    {
        public CampaignDataForUpload(CampaignData cd)
        {
            this.Recipient = cd.Recipient;
            this.AccountNumber = cd.AccountNumber;
            this.FirstName = cd.FirstName;
            this.LastName = cd.LastName;
        }
        public string Recipient { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
