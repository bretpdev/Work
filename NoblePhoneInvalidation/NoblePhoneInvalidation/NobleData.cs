using Uheaa.Common.DataAccess;

namespace NoblePhoneInvalidation
{
    public class NobleData
    {
		[DbName("call_type")]
        public string CallType { get; set; }
		[DbName("lm_filler2")]
        public string AccountIdentifier { get; set; }
		[DbName("areacode")]
        public string AreaCode { get; set; }
		[DbName("phone")]
        public string Phone { get; set; }
		[DbName("appl")]
		public string Campaign { get; set; }
		[DbName("status")]
		public string Disposition { get; set; }
		[DbName("addi_status")]
		public string AdditionalDisposition { get; set; }
		[DbName("tsr")]
		public string AgentId { get; set; }
		[DbName("act_date")]
		public string EffectiveDate { get; set; }
		[DbName("act_time")]
		public string EffectiveTime { get; set; }
		[DbName("listid")]
		public string ListId { get; set; }

        public string PhoneNumber
        {
            get { return AreaCode + Phone; }
        }
		public override string ToString()
		{
 			return string.Join(" ", CallType, ListId, Campaign, AccountIdentifier, AreaCode, Phone, AdditionalDisposition, Disposition, AgentId, EffectiveDate, EffectiveTime);
		}
    }
}

