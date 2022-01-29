using System.Collections.Generic;

namespace R301QUEUE
{
	class FinancialActivity
	{
		public bool LoanExistsWithPositiveBalance;
		public double TotalCreditBalance;
		private readonly List<Transaction> _transactions;
		public List<Transaction> Transactions { get { return _transactions; } }

		public FinancialActivity()
		{
			LoanExistsWithPositiveBalance = false;
			TotalCreditBalance = 0;
			_transactions = new List<Transaction>();
		}
	}//class
}//namespace
