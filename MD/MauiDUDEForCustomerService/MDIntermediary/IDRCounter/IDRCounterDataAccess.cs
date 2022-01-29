using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MDIntermediary
{
    public class IDRCounterDataAccess
    {
        DataAccessHelper.Database db;
        public IDRCounterDataAccess()
        {
            this.db = DataAccessHelper.Database.Udw;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "[idrcounter].[GetIDRCount]") ]
        public List<IDRCountRecord> GetIDRCounterRecords(string accountNumber)
        {
            return DataAccessHelper.ExecuteList<IDRCountRecord>("[idrcounter].[GetIDRCount]", db, SqlParams.Single("AccountNumber", accountNumber));
        }
    }
}
