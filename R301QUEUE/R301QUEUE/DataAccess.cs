using System.Data.Linq;
using System.Linq;
using Q;

namespace R301QUEUE
{
	class DataAccess : DataAccessBase
	{
		private readonly DataContext _bsys;

		public DataAccess(bool testMode)
		{
			_bsys = BsysDataContext(testMode);
		}

		public string GetReassignmentId(string userId)
		{
			string query = string.Format("SELECT AssignID FROM R301_LST_Users WHERE UserID = '{0}'", userId);
			return _bsys.ExecuteQuery<string>(query).FirstOrDefault();
		}//GetReassignmentId()
	}//class
}//namespace
