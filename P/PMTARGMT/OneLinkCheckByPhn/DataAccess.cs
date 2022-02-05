using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Data.Linq;

namespace OneLinkCheckByPhn
{
    class DataAccess : DataAccessBase
    {

        public static void AddRecordToOneLINKCheckByPhoneData(bool testMode, string contactType, decimal paymentAmount)
        {
            string insertStr = "EXEC spBORG_AddRecordToOneLINKCheckByPhoneData {0}, {1}, {2}";
            BsysDataContext(testMode).ExecuteCommand(insertStr, Environment.UserName, contactType, paymentAmount);
        }

    }
}
