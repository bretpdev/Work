using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Q;

namespace RegentsApplicationDownload.DataAccess
{
    public class BsysDataAccess : DataAccessBase
    {
        public enum ErrorType
        {
            DownloadError,
            RuntimeError
        }

        public static string GetDistributionList(ErrorType errorType)
        {
            string typeKey = string.Empty;
            switch (errorType)
            {
                case ErrorType.DownloadError:
                    typeKey = "RegentsDLerror";
                    break;
                case ErrorType.RuntimeError:
                    typeKey = "RegentsAppDLerr";
                    break;
                default:
                    string message = string.Format("Invalid ErrorType ({0}) passed to {1}", errorType.ToString(), MethodBase.GetCurrentMethod().Name);
                    Debug.Assert(false, message);
                    break;
            }

            string query = string.Format("SELECT WinUName FROM GENR_REF_MiscEmailNotif WHERE TypeKey = '{0}'", typeKey);
			string[] addresses = BsysDataContext(RegentsScholarshipBackEnd.Constants.TEST_MODE).ExecuteQuery<string>(query).Select(p => p + "@utahsbr.edu").ToArray();
            return string.Join(",", addresses);
        }//GetDistributionList()
    }//class
}//namespace
