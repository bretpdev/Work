using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
namespace NBLCONTPUL
{
	public class NobleData
	{
		[DbName("rowid")]
		public string NobleRowId { get; set; }
		[DbName("call_type")]
		public string CallType { get; set; }
		[DbName("lm_filler2")]
		public string IdentifierOut { get; set; }
		[DbName("filler2")]
		public string IdentifierIn { get; set; }
		[DbName("areacode")]
		public string AreaCodeOut { get; set; }
		[DbName("ani_code")]
		public string AreaCodeIn { get; set; }
		[DbName("phone")]
		public string PhoneOut { get; set; }
		[DbName("ani_phone")]
		public string PhoneIn { get; set; }
		[DbName("appl")]
		public string Campaign { get; set; }
		[DbName("status")]
		public string Disposition { get; set; }
		[DbName("addi_status")]
		public string AgentDisposition { get; set; }
		[DbName("tsr")]
		public string AgentCode { get; set; }
		[DbName("listid")]
		public string ListId { get; set; }
		[DbName("act_date")]
		public string CallDateOut { get; set; }
		[DbName("call_date")]
		public string CallDateIn { get; set; }
		[DbName("act_time")]
		public string CallTimeOut { get; set; }
		[DbName("call_time")]
		public string CallTimeIn { get; set; }
		[DbName("time_connect")]
		public string TimeConnected { get; set; }
		[DbName("time_acw")]
		public string TimeACWOut { get; set; }
		[DbName("time_acwork")]
		public string TimeACWIn { get; set; }
		[DbName("time_hold")]
		public string TimeHoldOut { get; set; }
		[DbName("time_holding")]
		public string TimeHoldIn { get; set; }
		[DbName("lm_filler1")]
		public string Filler1Out { get; set; }
		[DbName("filler1")]
		public string Filler1In { get; set; }
		

		public string AgentCode2 { get; set; } //retrieved from BSYS
		public string AgentName { get; set; } //retrieved from BSYS
		/// <summary>
		/// Determines if the Filler2 field is 9 or 10 characters and sets the SSN = Filler2 if Filler2 is 9 characters
		/// </summary>
		public string SSN 
		{
			get { return IdentifierOut.IsPopulated() ? (IdentifierOut.Trim().Length == 9 ? IdentifierOut.Trim() : "") 
				       : IdentifierIn.IsPopulated() ? (IdentifierIn.Trim().Length == 9 ? IdentifierIn.Trim() : "") : ""; }
		}

		/// <summary>
		/// Determines if the Filler2 field is 9 or 10 characters and sets the AccountNumber = Filler2 if Filler2 is 10 characters
		/// </summary>
		public string AccountNumber 
		{
			get { return IdentifierOut.IsPopulated() ? (IdentifierOut.Trim().Length > 9 ? IdentifierOut.Trim() : "") 
				: IdentifierIn.IsPopulated() ? (IdentifierIn.Trim().Length > 9 ? IdentifierIn.Trim() : "") : ""; }
		}

		public string PhoneNumber
		{
			get { return AreaCodeOut.IsPopulated() ? AreaCodeOut + PhoneOut : AreaCodeIn.IsPopulated() ? AreaCodeIn + PhoneIn : ""; }
		}

	}
}
