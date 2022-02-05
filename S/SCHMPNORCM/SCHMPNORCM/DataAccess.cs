using System.Data.Linq;
using System.Linq;
using Q;

namespace SCHMPNORCM
{
    class DataAccess : DataAccessBase
    {
		private DataContext _bsys;

		public DataAccess(bool testMode)
		{
			_bsys = BsysDataContext(testMode);
		}

		public bool IsUtahSchool(string schoolId)
		{
			string query = string.Format("SELECT COUNT(*) FROM GENR_LST_UTSchools WHERE UTSchoolID = '{0}'", schoolId);
			int resultCount = _bsys.ExecuteQuery<int>(query).Single();
			return (resultCount > 0);
		}//IsUtahSchool()
    }//class
}//namespace
