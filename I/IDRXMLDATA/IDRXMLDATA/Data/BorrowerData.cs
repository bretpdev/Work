using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace IDRXMLDATA
{
	public class BorrowerData
	{
		public int BorrowerID { get; set; }
		[DbName("BF_SSN")]
		public string SSN { get; set; }
		[DbName("DF_SPE_ACC_ID")]
		public string AccountNumber { get; set; }
		[DbName("DM_PRS_1")]
		public string FirstName { get; set; }
		[DbName("DM_PRS_LST")]
		public string LastName { get; set; }
		[DbName("DM_PRS_MID")]
		public string MiddleName { get; set; }
        public string Region { get; set; }
		public int SpouseId { get; set; }
	}
}
