using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace R301QUEUE
{
	public class R301Queue : ScriptBase
	{
		private string _reassignmentUserId;
		private readonly TransactionReport _transactionReport;
		private string _userId;

		public R301Queue(ReflectionInterface ri)
			: base(ri, "R301QUEUE")
		{
			_transactionReport = new TransactionReport(ri.TestMode);
		}

		public override void Main()
		{
			string startupMessage = "This script will work tasks in the R301 queue. Click OK to continue, or Cancel to quit.";
			if (MessageBox.Show(startupMessage, ScriptID, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK) { EndDLLScript(); }

			_userId = GetUserIDFromLP40();
			//_userId = "UT00044"; //For test
			_reassignmentUserId = new DataAccess(RI.TestMode).GetReassignmentId(_userId);
			if (string.IsNullOrEmpty(_reassignmentUserId))
			{
				MessageBox.Show("You are not authorized to use this script. Ending Script.");
				return;
			}

			List<QueueData> queueTasks = null;
			try
			{
				queueTasks = GetQueueTasks();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				return;
			}

			WorkQueue(queueTasks);

			_transactionReport.Print();
			_transactionReport.Delete();

			MessageBox.Show("Process Complete!");
		}//Main()

		private void AddCommentInTC00(string ssn, string comment)
		{
			FastPath("TX3Z/ATC00{0}", ssn);
			PutText(19, 38, "4", Key.Enter);
			PutText(12, 10, comment, Key.Enter);
		}//AddCommentInTC00()

		private void AssignQueueTask(string SSN, string userId)
		{
			FastPath("TX3Z/CTX6J");
			PutText(7, 42, "R3");
			PutText(8, 42, "01");
			PutText(9, 42, SSN + "*");
			PutText(9, 76, "L", Key.Enter);

			if (Check4Text(2, 27, "ASSIGN/REPRIORITIZE TASKS SELECTION"))
			{
				while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 9; !Check4Text(row, 2, "  "); row += 2)
					{
						PutText(21, 18, GetText(row, 2, 2), Key.Enter, true);
						PutText(8, 15, userId, Key.Enter);
						Hit(Key.F12);
					}
					Hit(Key.F8);
				}//while
			}
			else
			{
				PutText(8, 15, userId, Key.Enter);
			}
		}//AssignQueueTask()

		private void CloseR301Tasks(string ssn)
		{
			string queueScreen = string.Format("TX3Z/ITX6T{0}", ssn);
			const string NO_RECORDS_FOUND = "01020 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA";
			for (FastPath(queueScreen); !Check4Text(23, 2, NO_RECORDS_FOUND); FastPath(queueScreen))
			{
				bool foundTask = false;
				while (!foundTask && !Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 7; row < 16; row += 4)
					{
						if (Check4Text(row, 8, "R3") && Check4Text(row, 24, "01"))
						{
							foundTask = true;
							PutText(21, 18, GetText(row, 2, 2), Key.Enter, true);
							if (Check4Text(1, 72, "J0X01"))
							{
								FastPath(queueScreen);
								PutText(21, 18, GetText(row, 2, 2), Key.F2, true);
							}
							else
							{
								PutText(9, 12, "Y");
								EnterText("0");
								Hit(Key.EndKey);
								Hit(Key.Enter);
								Hit(Key.F11);
							}
							PutText(8, 19, "C", Key.Enter);
							break;
						}//if
					}//for
					if (!foundTask) { Hit(Key.F8); }
				}//while
			}//for
		}//CloseR301Tasks()

		private void CloseR304Tasks(string ssn)
		{
			string queueScreen = string.Format("TX3Z/ITX6T{0}", ssn);
			const string NO_RECORDS_FOUND = "01020 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA";
			for (FastPath(queueScreen); !Check4Text(23, 2, NO_RECORDS_FOUND); FastPath(queueScreen))
			{
				bool foundTask = false;
				while (!foundTask && !Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 7; row < 16; row += 4)
					{
						if (Check4Text(row, 8, "R3") && Check4Text(row, 24, "04") && Check4Text(row + 1, 76, "W"))
						{
							foundTask = true;
							PutText(21, 18, GetText(row, 2, 2), Key.F2, true);
							PutText(8, 19, "C", Key.Enter);
							break;
						}
					}//for
					if (!foundTask) { Hit(Key.F8); }
				}//while
			}//for
		}//CloseR304Tasks()

		private AccountInfo GetAccountInfo(string ssn)
		{
			AccountInfo account = new AccountInfo();
			FastPath("TX3Z/ITS26{0}", ssn);
			if (Check4Text(1, 72, "TSX28"))
			{
				//Selection screen.
				while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 8; !Check4Text(row, 3, " "); row++)
					{
						if (Check4Text(row, 69, "CR"))
						{
							account.HasCredit = true;
						}
						else
						{
							account.Balance += double.Parse(GetText(row, 59, 10).Replace(",", ""));
						}
					}
					Hit(Key.F8);
				}//while
			}
			else if (Check4Text(1, 72, "TSX29"))
			{
				//Target screen.
				if (Check4Text(11, 22, "CR"))
				{
					account.HasCredit = true;
				}
				else
				{
					account.Balance = double.Parse(GetText(11, 12, 10).Replace(",", ""));
				}
			}
			return account;
		}//GetAccountInfo()

		private List<QueueData> GetQueueTasks()
		{
			FastPath("TX3Z/ITX6XR301");
			if (Check4Text(23, 2, "80014")) { throw new Exception("You do not have access to the R301 Queue. Contact System Support."); }
			if (Check4Text(23, 2, "01020")) { throw new Exception("The queue is empty. Process Complete."); }

			List<QueueData> queueTasks = new List<QueueData>();
			while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
			{
				for (int row = 8; !Check4Text(row, 47, "          "); row += 3)
				{
					QueueData task = new QueueData();
					task.SSN = GetText(row, 6, 9);
					task.LoanSequence = GetText(row, 15, 4);
					task.Status = GetText(row, 75, 1);
					task.DateRequested = DateTime.Parse(GetText(row, 47, 10));
					queueTasks.Add(task);
				}
				Hit(Key.F8);
			}//while
			return queueTasks;
		}//GetQueueTasks()

		private void WorkQueue(IEnumerable<QueueData> queueTasks)
		{
			DateTime twentyDaysAgo = DateTime.Now.AddDays(-20);
			foreach (QueueData task in queueTasks.Where(p => p.Status != "A" && p.DateRequested < twentyDaysAgo && !p.WasWorked))
			{
				//Work the task if this borrower doesn't have any tasks requested within the last 20 days.
				if (!queueTasks.Any(p => p.SSN == task.SSN && p.DateRequested > twentyDaysAgo))
				{
					WorkTask(task.SSN, task.LoanSequence);
					task.WasWorked = true;
				}
			}
			if (!queueTasks.Any(p => p.WasWorked))
			{
				MessageBox.Show("There was no task found that could be worked. Ending script.");
				EndDLLScript();
			}
		}//WorkQueue()

		private void WorkTask(string ssn, string loanSequence)
		{
			AccountInfo account = GetAccountInfo(ssn);

			if (!account.HasCredit)
			{
				CloseR301Tasks(ssn);
				CloseR304Tasks(ssn);
				string comment = string.Format("Completed R301 queue task. All loans have zero balace at time of review. {0}{1}{2}", "{", ScriptID, "}");
				AddCommentInTC00(ssn, comment);
			}

			if (account.Balance > 0)
			{
				AssignQueueTask(ssn, _reassignmentUserId);
			}
			else if (account.HasCredit)
			{
				ProcessCredit(ssn, loanSequence, account.Balance);
			}
		}//WorkTask()

		#region Credit processing
		private void CompleteTask(string ssn, string loanSequence, double totalCreditBalance, IEnumerable<string> loanSequences)
		{
			FastPath("TX3Z/ITX6T{0}", ssn);
			bool foundTask = false;
			while (!foundTask && !Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
			{
				for (int row = 7; !Check4Text(row, 3, " "); row++)
				{
					string currentSequence = GetText(row, 49, 4);
					if (Check4Text(row, 8, "R3") && Check4Text(row, 24, "01") && loanSequences.Any(p => p.PadLeft(4, '0') == currentSequence))
					{
						foundTask = true;
						PutText(21, 18, GetText(row, 2, 2), Key.Enter, true);
						break;
					}
				}//for
				if (!foundTask) { Hit(Key.F8); }
			}//while

			if (foundTask)
			{
				//Complete the task.
				string writeUpAmount = GetText(7, 20, 14);
				PutText(9, 12, "Y0");
				Hit(Key.EndKey);
				Hit(Key.Enter);
				Hit(Key.F11);
				PutText(9, 19, "COMPL");
				PutText(8, 19, "C", Key.Enter);
				if (!Check4Text(23, 2, "01005 RECORD SUCCESSFULLY CHANGED"))
				{
					PutText(9, 19, "", true);
					Hit(Key.Enter);
				}
				//Add an activity comment.
				string comment = string.Format("Completed R301 queue task.  Processed Write Up - {0}, {1}, Total Write up Amount for all loans is {2}", loanSequence, writeUpAmount, totalCreditBalance);
				List<int> writeOffLoanSequences = new List<int>();
				writeOffLoanSequences.Add(int.Parse(loanSequence));
				ATD22ByLoan(ssn, "R3WUP", comment, writeOffLoanSequences, ScriptID, false);
			}
		}//CompleteTask()

		private string DetermineReassignmentId(IEnumerable<Transaction> transactions)
		{
			string[] baseTransactions = { "1010", "1027", "1029", "1036", "1037", "1038", "1039", "1041", "5002" };
			string[] consolTransactions = { "1070", "1080" };
			bool firstTransactionIsBase = (baseTransactions.Contains(transactions.First().Type));

			if (firstTransactionIsBase && transactions.Any(p => p.IsCredit && consolTransactions.Contains(p.Type)))
			{
				return "UT00049";
			}
			else if (HasPrior1010(transactions))
			{
				return _reassignmentUserId;
			}
			else if (transactions.First().Type == "1010")
			{
				return "";
			}
			else
			{
				return "UT00296";
			}
		}//DetermineReassignmentId()

		private void GenerateRefund(string ssn, string loanSequence, double accountBalance)
		{
			FastPath("TX3Z/ITX6T{0}", ssn);
			bool foundTask = false;
			while (!foundTask && !Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
			{
				for (int row = 7; !Check4Text(row, 3, " "); row++)
				{
					if (Check4Text(row, 8, "R3") && Check4Text(row, 24, "01") && Check4Text(row, 49, loanSequence))
					{
						foundTask = true;
						PutText(21, 18, GetText(row, 2, 2), Key.Enter, true);
						break;
					}
				}//for
				if (!foundTask) { Hit(Key.F8); }
			}//while

			if (foundTask)
			{
				//Complete the task if it doesn't need to be reassigned.
				string refundAmount = GetText(7, 20, 14);
				if (accountBalance != double.Parse(refundAmount))
				{
					AssignQueueTask(ssn, "UT00296");
					return;
				}

				if (Check4Text(12, 25, "_________", "         ", "828476"))
				{
					PutText(19, 13, "1");
					Hit(Key.F4);
					Hit(Key.F11);
				}

				PutText(9, 12, "Y");
				Hit(Key.Enter);
				Hit(Key.F11);
				PutText(8, 19, "C", Key.Enter);
				if (!Check4Text(23, 2, "01005 RECORD SUCCESSFULLY CHANGED"))
				{
					PutText(9, 19, "", true);
					Hit(Key.Enter);
				}

				//Create an ARC for the refund.
				string comment = string.Format("Completed R301 queue task.  Process refund to borrower for  {0}", refundAmount);
				List<int> refundLoanSequences = new List<int>();
				refundLoanSequences.Add(int.Parse(loanSequence));
				ATD22ByLoan(ssn, "R3RFX", comment, refundLoanSequences, ScriptID, false);
				_transactionReport.AddRecord(GetDemographicsFromTX1J(ssn), refundAmount);
			}
		}//GenerateRefund()

		private FinancialActivity GetFinancialActivityForCreditLoans(string ssn)
		{
			FinancialActivity activity = new FinancialActivity();
			FastPath("TX3Z/ITS2C{0}", ssn);
			Hit(Key.Enter);
			if (Check4Text(1, 72, "TSX2M"))
			{
				//Selection screen.
				while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 7; !Check4Text(row, 3, " "); row++)
					{
						if (Check4Text(row, 69, "CR"))
						{
							activity.TotalCreditBalance += double.Parse(GetText(row, 57, 12));
							PutText(21, 18, GetText(row, 2, 2), Key.Enter, true);
							activity.Transactions.AddRange(ReadAllTransactions());
							Hit(Key.F12);
						}
						else if (double.Parse(GetText(row, 59, 10).Replace(",", "")) > 0)
						{
							activity.LoanExistsWithPositiveBalance = true;
						}
					}//for
					Hit(Key.F8);
				}//while
			}
			else
			{
				//Target screen.
				if (Check4Text(7, 78, "CR"))
				{
					activity.TotalCreditBalance = double.Parse(GetText(7, 67, 11));
					activity.Transactions.AddRange(ReadAllTransactions());
				}
				else if (double.Parse(GetText(7, 67, 11)) > 0)
				{
					activity.LoanExistsWithPositiveBalance = true;
				}
			}
			return activity;
		}//GetFinancialActivityForCreditLoans()

		private bool HasPrior1010(IEnumerable<Transaction> transactions)
		{
			string[] baseTransactions = { "1025", "1027", "1029", "1036", "1037", "1038", "1039", "5002" };
			bool firstTransactionIsBase = (baseTransactions.Contains(transactions.First().Type));
			return (firstTransactionIsBase && transactions.Any(p => p.IsCredit) && transactions.Any(p => p.Type == "1010"));
		}//HasPrior1010()

		private IEnumerable<WriteUpLoan> PerformWriteUp(string ssn, string loanSequence, double totalCreditBalance)
		{
			List<WriteUpLoan> writeUpLoans = new List<WriteUpLoan>();
			FastPath("TX3Z/ATS1V{0}", GetDemographicsFromTX1J(ssn).AccountNumber.Replace(" ", ""));
			while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
			{
				for (int row = 8; row < 18; row++)
				{
					if (!Check4Text(row, 11, "  "))
					{
						WriteUpLoan newLoan = new WriteUpLoan();
						newLoan.Sequence = GetText(row, 11, 2);
						newLoan.CreditAmount = GetText(row, 49, 11).Replace("$", "").Replace(",", "");
						writeUpLoans.Add(newLoan);
						PutText(row, 4, "X");
					}
				}//for
				//Enter and post comment.
				string comment = "Completed R301 queue task.  Processed write up loan seq ";
				IEnumerable<string> loanAmounts = writeUpLoans.Select(p => string.Format("{0} amount {1}", p.Sequence, p.CreditAmount));
				comment += string.Join(", ", loanAmounts.ToArray());
				comment += " {" + ScriptID + "}";
				PutText(19, 2, comment, Key.F6);
				//Check for more loans.
				Hit(Key.F8);
			}//while
			return writeUpLoans;
		}//PerformWriteUp()

		private void ProcessCredit(string ssn, string loanSequence, double accountBalance)
		{
			FinancialActivity activity = GetFinancialActivityForCreditLoans(ssn);

			//See if the task needs to be reassigned.
			string reassignmentId = DetermineReassignmentId(activity.Transactions);
			if (!string.IsNullOrEmpty(reassignmentId))
			{
				AssignQueueTask(ssn, reassignmentId);
				return;
			}

			//Process according to balance.
			if (activity.LoanExistsWithPositiveBalance)
			{
				return;
			}
			else if (activity.TotalCreditBalance < 5)
			{
				IEnumerable<WriteUpLoan> writeUpLoans = PerformWriteUp(ssn, loanSequence, activity.TotalCreditBalance);
				CompleteTask(ssn, loanSequence, activity.TotalCreditBalance, writeUpLoans.Select(p => p.Sequence));
			}
			else
			{
				string[] consolTransactions = { "1070", "1080" };
				bool hasBorrowerOrOwnerCredit = activity.Transactions.Any(p => p.IsCredit && p.LoanSequence == loanSequence && new string[] { "1010", "1027", "1029" }.Contains(p.Type));
				bool hasConsolCredit = activity.Transactions.Any(p => p.IsCredit && p.LoanSequence == loanSequence && consolTransactions.Contains(p.Type));
				if (hasBorrowerOrOwnerCredit && !hasConsolCredit)
				{
					GenerateRefund(ssn, loanSequence, accountBalance);
				}
				else
				{
					CloseR301Tasks(ssn);
				}
			}
		}//ProcessCredit()

		private List<Transaction> ReadAllTransactions()
		{
			List<Transaction> transactions = new List<Transaction>();
			while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
			{
				for (int transactionRow = 11; !Check4Text(transactionRow, 33, "    "); transactionRow++)
				{
					Transaction newTransaction = new Transaction();
					newTransaction.LoanSequence = GetText(5, 29, 4);
					newTransaction.Amount = GetText(transactionRow, 39, 12);
					newTransaction.IsCredit = Check4Text(transactionRow, 51, "CR");
					newTransaction.Type = GetText(transactionRow, 33, 4);
					transactions.Add(newTransaction);
				}
				Hit(Key.F8);
			}//while
			return transactions;
		}//ReadAllTransactions()
		#endregion
	}//class
}//namespace
