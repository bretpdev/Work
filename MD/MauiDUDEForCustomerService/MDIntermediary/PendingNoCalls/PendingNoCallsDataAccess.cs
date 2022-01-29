using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MDIntermediary
{
    public class PendingNoCallsDataAccess
    {
        string accountnumber;
        DataAccessHelper.Database db;
        public PendingNoCallsDataAccess(string accountnumber)
        {
            this.accountnumber = accountnumber;
            this.db = DataAccessHelper.Database.Udw;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "[pendingnocalls].[GetNextEligibleCallSuspensionDate]")]
        public DateTime GetNextEligibleCallSuspensionDate()
        {
            return DataAccessHelper.ExecuteSingle<DateTime>("[pendingnocalls].[GetNextEligibleCallSuspensionDate]", db, SqlParams.Single("AccountNumber", accountnumber))
                .Date;
        }
        [UsesSproc(DataAccessHelper.Database.Udw, "[pendingnocalls].[BorrowerCallSuspensionInsert]")]
        public void AddCallSuspension(DateTime start, DateTime end)
        {
            DataAccessHelper.Execute("[pendingnocalls].[BorrowerCallSuspensionInsert]", db, SqlParams.Single("AccountNumber", accountnumber),
                SqlParams.Single("StartDate", start), SqlParams.Single("EndDate", end));
        }
    }
}
