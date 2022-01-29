using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Q;

namespace BatchLoginDatabase
{
	class DataAccess : DataAccessBase
	{
		private bool _testMode;
		private DataContext _batchProcessing;

		public DataAccess(bool testMode)
		{
			_testMode = testMode;
			_batchProcessing = BatchProcessingDataContext(testMode);
		}

		public void DeleteOldHistory()
		{
			try
			{
				_batchProcessing.ExecuteCommand(@"EXEC spBLDBDeleteOldHistory");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<UserIdsAndPasswords> GetUsersAndPasswordFromDb()
		{
			return _batchProcessing.ExecuteQuery<UserIdsAndPasswords>(@"EXEC spBLDBGetUserIdsAndPasswords").ToList();
		}

		public void AddUserIdsAndPasswords(UserIdsAndPasswords uData)
		{
			_batchProcessing.ExecuteCommand(@"EXEC spAddUserIdAndPassword {0},{1},{2}", uData.UserNameId, uData.NewPassword, uData.Notes);
		}

		public void DeleteUserIdsAndPasswords(UserIdsAndPasswords uData, LoginCredentais lData)
		{
			_batchProcessing.ExecuteCommand(@"EXEC spUpdateHistoryTable {0},{1}", uData.UserNameId, lData.UserId);
			_batchProcessing.ExecuteCommand(@"EXEC spDeleteUserIdsAndPasswords {0}", uData.UserNameId);
		}

		public void UpdateUserIdsAndPasswords(UserIdsAndPasswords uData, LoginCredentais lData)
		{
			_batchProcessing.ExecuteCommand(@"EXEC spUpdateHistoryTable {0},{1}", uData.UserNameId, lData.UserId);
			_batchProcessing.ExecuteCommand(@"EXEC spUpdateUserIdsAndPasswords {0},{1},{2}", uData.UserNameId, uData.NewPassword, uData.Notes);
		}
	}
}
