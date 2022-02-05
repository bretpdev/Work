using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Q;

namespace BTCHCBP
{
    class DataAccess : DataAccessBase
    {
		//Keep a single instance of the DataContext so we can use a transaction on it.
		private readonly DataContext CLS;

		public DataAccess(bool testMode)
		{
			CLS = ClsDataContext(testMode);
		}

		public List<Payment> GetPendingPayments()
		{
			return CLS.ExecuteQuery<Payment>("EXEC spCheckByPhoneGetPendingRecords").ToList();
		}//GetPendingPayments()

		public void UpdateProcessedDate(IEnumerable<int> recordNumbers)
		{
			CLS.Connection.Open();
			CLS.Transaction = CLS.Connection.BeginTransaction();
			try
			{
				foreach (int recordNumber in recordNumbers)
				{
					CLS.ExecuteCommand("EXEC spCheckByPhoneUpdateProcessedDate {0}", recordNumber);
				}
				CLS.Transaction.Commit();
			}
			catch (Exception)
			{
				CLS.Transaction.Rollback();
				throw;
			}
			finally
			{
				CLS.Connection.Close();
			}
		}//UpdateProcessedDate()
    }//class
}//namespace
