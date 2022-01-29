using System;
using Uheaa.Common.DataAccess;

namespace NDNUMINVAL
{
    public class NobleData
    {
        public int? CallType { get; set; }
        public string AccountIdentifier { get; set; }
        public string AreaCode { get; set; }
        public string Phone { get; set; }
		public string Campaign { get; set; }
		public string Disposition { get; set; }
		public string AdditionalDisposition { get; set; }
		public string AgentId { get; set; }
		public DateTime? EffectiveDate { get; set; }
		public string ListId { get; set; }

        public string PhoneNumber
        {
            get { return AreaCode + Phone; }
        }
		public override string ToString()
		{
 			return string.Join(" ", CallType.HasValue ? CallType.Value.ToString() : "", ListId, Campaign, AccountIdentifier, AreaCode, Phone, AdditionalDisposition, Disposition, AgentId, EffectiveDate.HasValue ? EffectiveDate.Value.ToString("M/d/yyyy H:mm") : "");
		}
    }
}

