using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Q;

namespace DocIdCornerStone
{
	class DataAccess : DataAccessBase
	{
		private DataContext CLS
		{
			get { return ClsDataContext(_testMode); }
		}

		private readonly bool _testMode;
		private string _userName;

		public DataAccess(bool testMode)
		{
			_testMode = testMode;
		}

		public IEnumerable<DocumentDetail> GetDocumentDetails()
		{
			return CLS.ExecuteQuery<DocumentDetail>("EXEC spDocIdGetDocumentDetails").ToList();
		}

		public IEnumerable<ProcessingSummary> GetReportSummary()
		{
			return CLS.ExecuteQuery<ProcessingSummary>("EXEC spDocIdGetProcessingSummary").ToList();
		}

		public IEnumerable<ProcessingUser> GetReportUsers()
		{
			return CLS.ExecuteQuery<ProcessingUser>("EXEC spDocIdGetProcessingUsers").ToList();
		}

		public void SetProcessedRecord(string docId, string source)
		{
			if (string.IsNullOrEmpty(_userName))
			{
				IEnumerable<SqlUser> sqlUsers = CSYSDataContext(_testMode).ExecuteQuery<SqlUser>("EXEC spSYSA_GetSqlUsers");
				_userName = sqlUsers.Single(p => p.WindowsUserName == Environment.UserName).ToString();
			}
			CLS.ExecuteCommand("EXEC spDocIdSetProcessed {0}, {1}, {2}", _userName, docId, source);
		}
	}//class
}//namespace
