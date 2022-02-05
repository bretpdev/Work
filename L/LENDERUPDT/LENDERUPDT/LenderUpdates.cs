using System;
using Uheaa.Common.DataAccess;

namespace LENDERUPDT
{
	public class LenderUpdates
	{
		[DbName("LenderUpdateId")]
		public int LenderUpdateId { get; set; }
		[DbName("MOD")]
		public string Mod { get; set; }
		[DbName("LenderId")]
		public string LenderId { get; set; }
		[DbName("FullName")]
		public string FullName { get; set; }
		[DbName("ShortName")]
		public string ShortName { get; set; }
		[DbName("Address1")]
		public string Address1 { get; set; }
		[DbName("Address2")]
		public string Address2 { get; set; }
		[DbName("City")]
		public string City { get; set; }
		[DbName("State")]
		public string State { get; set; }
		[DbName("Zip")]
		public string Zip { get; set; }
		[DbName("Valid")]
		public bool Valid { get; set; }
		[DbName("DateVerified")]
		public DateTime DateVerified { get; set; }
		[DbName("Type")]
		public string Type { get; set; }
		[DbName("AddedAt")]
		public DateTime AddedAt { get; set; }
		[DbName("AddedBy")]
		public string AddedBy { get; set; }
	}
}
