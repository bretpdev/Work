using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Q;

namespace ARCADDFED
{
    class DataAccess : DataAccessBase
    {
		private bool _testMode;

		public DataAccess(bool testMode)
		{
			_testMode = testMode;
		}

		public ArcRecord GetSingleRecord()
		{
			string selectStr = "EXEC spArcAdd_GetSingleRecord";
			return ClsDataContext(_testMode).ExecuteQuery<ArcRecord>(selectStr).SingleOrDefault();
		}

		public void DeleteProcessedRecord(long recordId)
		{
			string deleteStr = "EXEC spArcAdd_DeleteProcessedRecord {0}";
			ClsDataContext(_testMode).ExecuteCommand(deleteStr, recordId);
		}

		public string GetSSNFromDataWarehouse(string acctNum)
		{
			string selectStr = "EXEC spGetSSNFromAcctNumber {0}";
			return CDWDataContext(_testMode).ExecuteQuery<string>(selectStr, acctNum).SingleOrDefault();
		}
    }
}
