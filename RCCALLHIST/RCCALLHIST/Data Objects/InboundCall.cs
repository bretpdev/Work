using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCALLHIST
{
    public class InboundCall
    {
        public long RowId { get; set; }
        public int CallType { get; set; }
        public string ListId { get; set; }
        public string CampaignId { get; set; }
        public string AccountNumber { get; set; }
        public string AreaCode { get; set; }
        public string Phone { get; set; }
        public string AdditionalStatus { get; set; }
        public string Status { get; set; }
        public string AgentId { get; set; }
        public DateTime StartTime { get; set; }
        public string VoxFileName { get; set; }
        public int TimeConnect { get; set; }
        public int TimeACW { get; set; }
        public int TimeHold { get; set; }
        public int AgentHold { get; set; }
        public int SessionSeqNum { get; set; }
        public int NodeId { get; set; }
        public int ProfileId { get; set; }
        public string DialerField1 { get; set; }
        public string DialerField2 { get; set; }
        public string DialerField3 { get; set; }
        public string DialerField4 { get; set; }
        public string DialerField5 { get; set; }
        public string DialerField6 { get; set; }
        public string DialerField7 { get; set; }
        public string DialerField8 { get; set; }
        public string DialerField9 { get; set; }
        public string DialerField10 { get; set; }
    }
}
