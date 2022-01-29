using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Data.SqlClient;

namespace IVRRequestProcessing
{
    class DataAccess : DataAccessBase
    {

        public static void AddEntryToDB(bool testMode, CheckByPhoneData data)
        {
			//TODO: Don't use NoradSqlCommand. I want to deprecate it. Besides, this code is still vulnerable to SQL injection.
            string query = string.Format("EXEC spCKPH_AddCheckByPhoneEntry '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}'",
                data.SSN, data.Name, data.DOB, data.RoutingNumber, data.BankAccountNumber, data.AccountType, data.PaymentAmount, data.EffectiveDate, data.EmailAddress, data.AccountHolderName);
            SqlCommand comm = NoradSqlCommand(testMode);
            comm.Connection.Open();
            comm.CommandText = query;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }

    }
}
