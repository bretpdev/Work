using Q;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Windows.Forms;

namespace PUTSUSPCOM
{
    public class DataAccess : DataAccessBase
    {

        /// <summary>
        /// Gets all deals from the DB.
        /// </summary>
        /// <param name="testMode">Test mode indicator.</param>
        /// <returns></returns>
        public static List<Deal> GetDeals(bool testMode)
        {
            string query = "SELECT Description, DealNumber, Owner FROM BORG_LST_PUTSuspCmtDeals";
            return BsysDataContext(testMode).ExecuteQuery<Deal>(query).ToList();
        }

    }
}
