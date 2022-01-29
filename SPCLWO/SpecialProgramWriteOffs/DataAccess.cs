using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Q;

namespace SpecialProgramWriteOffs
{
    public class DataAccess : DataAccessBase
    {
        /// <summary>
        /// Retrieves the Windows user names from the GENR_REF_MiscEmailNotif table
        /// where the TypeKey is 'SpecialWriteOffErrors'.
        /// </summary>
        /// <param name="testMode">True if running in test mode.</param>
        public static List<string> GetErrorRecipients(bool testMode)
        {
            string query = "SELECT WinUName FROM GENR_REF_MiscEmailNotif WHERE TypeKey = 'SpecialWriteOffErrors'";
            return BsysDataContext(testMode).ExecuteQuery<string>(query).ToList();
        }
    }//class
}//namespace
