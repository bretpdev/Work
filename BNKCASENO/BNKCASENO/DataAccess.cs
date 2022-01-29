using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Reflection;

namespace BNKCASENO
{
    class DataAccess : DataAccessBase
    {

        private static DataContext _liveDocIDDataContext;
        private static DataContext _testDocIDDataContext;

        /// <summary>
        /// Creates datacontext as needed.  Ensures that only one is created to save processor and memory use.
        /// </summary>
        /// <param name="testMode">True if running in test mode.</param>
        protected static DataContext DocIDDataContext(bool testMode)
        {
            if (testMode)
            {
                if (_testDocIDDataContext == null)
                {
                    _testDocIDDataContext = new DataContext(@"Data Source=Bart\Bart;Initial Catalog=DocID;Integrated Security=SSPI;");
                }
                return _testDocIDDataContext;
            }
            else
            {
                if (_liveDocIDDataContext == null)
                {
                    _liveDocIDDataContext = new DataContext(@"Data Source=Nochouse;Initial Catalog=DocID;Integrated Security=SSPI;");
                }
                return _liveDocIDDataContext;
            }
        }

        /// <summary>
        /// Deletes all data from table.
        /// </summary>
        /// <param name="testMode"></param>
        public static void DeleteAllDataFromTable(bool testMode)
        {
            DocIDDataContext(testMode).ExecuteCommand("DELETE FROM Bankruptcy WHERE Acc IS NOT NULL");
        }

        /// <summary>
        /// Inserts record into doc id database
        /// </summary>
        /// <param name="testMode"></param>
        /// <param name="record"></param>
        public static void InsertRecordIntoTable(bool testMode, BankruptcyRecordData record)
        {
            string query = string.Format("INSERT into Bankruptcy (Acc, CaseID, FName, LName) Values ('{0}', '{1}', '{2}', '{3}')", record.AccountNumber, record.CaseNumber, record.FirstName, record.LastName);
            DocIDDataContext(testMode).ExecuteCommand(query);
        }

    }
}
