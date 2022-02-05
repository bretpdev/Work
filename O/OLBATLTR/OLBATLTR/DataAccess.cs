using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Q;

namespace OLBATLTR
{
	public class DataAccess : DataAccessBase
	{
		public static List<SasDetail> GetSasDetails(bool testMode)
		{
			string query = "SELECT ActionCode, BaseFileName, CommentText, LetterId FROM OLBL_REF_SasDetail";
			return BsysDataContext(testMode).ExecuteQuery<SasDetail>(query).ToList();
		}//GetSasDetails()
	}//class
}//namespace
