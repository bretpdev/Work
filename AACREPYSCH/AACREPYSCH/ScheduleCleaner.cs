using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace AACREPYSCH
{
	public class ScheduleCleaner : FedBatchScriptBase
	{
		private enum Ts0nScreen
		{
			TSX0R,
			TSX0P,
			TSX0S
		}

		private const string SUCCESS_ARC = "RSCHG";
		private const string ERROR_ARC = "FUAAC";
		private const string ERROR_COMMENT = "AAC Cleanup";
		private const string SHORT_DATE_FORMAT = "MM/dd/yy";

		private readonly DataAccess _dataAccess;
		private readonly IFormatProvider _formatProvider;

		public ScheduleCleaner(ReflectionInterface ri)
			: base(ri, "AACREPYSCH", "ERR_BU35", "EOJ_BU35", new string[] { })
		{
			_dataAccess = new DataAccess(ri.TestMode);
			_formatProvider = CultureInfo.CurrentCulture;
		}

		public override void Main()
		{
			StartupMessage("This script corrects the repayment terms on newly loaded loans. Click OK to continue, or Cancel to quit.");
			if (!UserHasAccessToArc(UserID, SUCCESS_ARC)) { NotifyAndEnd("You need access to the {0} ARC before running this script.", SUCCESS_ARC); }
			if (!UserHasAccessToArc(UserID, ERROR_ARC)) { NotifyAndEnd("You need access to the {0} ARC before running this script.", ERROR_ARC); }

			WorkQueue();
			ProcessingComplete();
		}//Main()

		private void WorkQueue()
		{
			bool openTask = false;

			const string QUEUE_PATH = "TX3Z/ITX6XSP;01";
			FastPath(QUEUE_PATH);
			if (Check4Text(23, 2, "01020 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA"))
			{ return; }
			else if (!Check4Text(23, 2, "     "))
			{ NotifyAndEnd("error accessing queue, {0}", GetText(23, 2, 78)); }

			//Work all tasks in the queue.
			string[] unworkableSchedules = { "IB", "IL", "C1", "C2", "C3", "PG", "PL" };
			for (; !Check4Text(23, 2, "01020 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA"); FastPath(QUEUE_PATH))
			{
				openTask = Check4Text(8, 75, "W"); //task open
				//Select the first queue task.
				string ssn = GetText(8, 6, 9);

				PutText(21, 18, "1", Key.Enter);
				//Check for concurrency problems (i.e., someone else is working the same queue and selected the first task before we did).
				while (Check4Text(23, 2, "01029 RECORD UPDATED OR DELETED BY ANOTHER USER"))
				{
					Hit(Key.F5);
					ssn = GetText(8, 6, 9);
					PutText(21, 18, "1", Key.Enter);
				}

				//Get active repayment schedules from ITS2X.
				IEnumerable<RepaymentSchedule> activeRepaymentSchedules = GetRepaymentSchedules(ssn);

				bool borrowerHasUnworkableSchedules = activeRepaymentSchedules.Count() < 1 || (activeRepaymentSchedules.Select(p => p.Type).Intersect(unworkableSchedules).Count() > 0);

				//Update the repayment schedules on ATS0N if we can.
				bool scheduleUpdated = false;
				//activeRepaymentSchedules.Select(p => p.Type).Distinct().Count() == 1
				if (activeRepaymentSchedules.Count() > 0 && !borrowerHasUnworkableSchedules && !openTask)
				{
					scheduleUpdated = UpdateRepaymentSchedule(ssn, activeRepaymentSchedules);
				}

				//Close the task.
				FastPath(QUEUE_PATH);
				PutText(21, 18, "1", Key.F2);
				PutText(8, 19, "C"); //TASK STATUS
				PutText(9, 19, "COMPL"); //ACTION RESPONSE
				Hit(Key.Enter);
				if (Check4Text(23, 2, "01644"))
				{
					PutText(9, 19, "", true);
					Hit(Key.Enter);
				}

				//Leave an ARC according to whether the schedule was updated.
				if (scheduleUpdated)
				{
					List<int> loanSequences = activeRepaymentSchedules.SelectMany(p => p.Loans.Select(q => q.Sequence)).ToList();
					ATD22ByLoan(ssn, SUCCESS_ARC, "AAC Cleanup completed", loanSequences, false);
				}
				else
				{
					ATD22ByBalance(ssn, ERROR_ARC, ERROR_COMMENT, false);
				}
			}//for
		}//WorkQueue()

		#region ITS2X
		private List<Loan> GetRepaymentLoans()
		{
			Debug.Assert(Check4Text(1, 71, "TSX3W"), "You must be on TSX3W to get the loans for a repayment schedule");
			Hit(Key.F4);
			List<Loan> loans = new List<Loan>();
			while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
			{
				Loan loan = new Loan();
				loan.FirstDisbursementDate = DateTime.ParseExact(GetText(6, 17, 8), SHORT_DATE_FORMAT, _formatProvider);
				loan.Sequence = int.Parse(GetText(7, 42, 4));
				//For loans with more than one screen of repayment levels, hitting F8 goes to the next screen
				//of repayment levels before going to the next loan. We don't care about repayment levels,
				//so check that we've actually picked up a new loan before adding it to the list.
				if (!loans.Select(p => p.Sequence).Contains(loan.Sequence)) { loans.Add(loan); }
				Hit(Key.F8);
			}//while
			return loans;
		}//GetRepaymentLoans()

		private List<RepaymentSchedule> GetRepaymentSchedules(string ssn)
		{
			FastPath("TX3Z/ITS2X{0}", ssn);
			List<RepaymentSchedule> schedules = new List<RepaymentSchedule>();
			if (Check4Text(1, 71, "TSX3W"))
			{
				//Target screen.
				RepaymentSchedule schedule = new RepaymentSchedule();
				schedule.Type = GetText(9, 75, 2);
				schedule.FirstDueDate = DateTime.ParseExact(GetText(7, 19, 8), SHORT_DATE_FORMAT, _formatProvider);

				Hit(Key.F4);
				schedule.HasMoreThan1Loan = Check4Text(2, 74, "+");
				schedule.HasMoreThan1Tier = Check4Text(13, 74, "+");
				schedule.IsALevelSch = Check4Text(18, 9, " ");
				if (schedule.IsALevelSch)
				{
					if (int.Parse(GetText(16, 47, 3)) <= 1)
					{
						schedule.IsNotFirstTeir = true;
					}
				}
				else
				{
					schedule.IsNotFirstTeir = false;
				}

				List<Loan> loans = new List<Loan>();

				int rowTSX56 = 16;
				int targetYear = DateTime.Now.Year;
				if (schedule.HasMoreThan1Tier && !schedule.HasMoreThan1Loan && !schedule.IsNotFirstTeir)
				{
					RepaymentSchedule rSch = new RepaymentSchedule();
					DateTime firstDue = schedule.FirstDueDate;
					string type = schedule.Type;
					rSch.Type = type;
					rSch.FirstDueDate = firstDue;
					rSch.LoanPgm = GetText(5, 16, 6);
					while (targetYear >= int.Parse(string.Format("20{0}", GetText(rowTSX56, 28, 2))) && targetYear <= int.Parse(string.Format("20{0}", GetText(rowTSX56, 28, 2))))
					{
						rowTSX56++;
						if (rowTSX56 > 21)
						{
							Hit(Key.F8);
							if (!Check4Text(23, 2, "90007"))
							{
								rowTSX56 = 16;
							}
						}
					}

					rSch.PaymentAmount = double.Parse(GetText(rowTSX56, 60, 11).Replace("$", "").Replace(",", ""));
					Loan loan = new Loan();
					loan.FirstDisbursementDate = DateTime.ParseExact(GetText(6, 17, 8), SHORT_DATE_FORMAT, _formatProvider);
					loan.Sequence = int.Parse(GetText(7, 42, 4));
					//For loans with more than one screen of repayment levels, hitting F8 goes to the next screen
					//of repayment levels before going to the next loan. We don't care about repayment levels,
					//so check that we've actually picked up a new loan before adding it to the list.
					if (!loans.Select(p => p.Sequence).Contains(loan.Sequence)) { loans.Add(loan); }
					rSch.Loans = loans;
					rSch.Loan = loan;
					schedules.Add(rSch);

					Hit(Key.F12);
					Hit(Key.F12);
				}
				else if (schedule.HasMoreThan1Loan && !schedule.HasMoreThan1Tier && schedule.IsALevelSch && !schedule.IsNotFirstTeir)
				{
					while (!Check4Text(23, 2, "90007"))
					{
						RepaymentSchedule rSch = new RepaymentSchedule();
						DateTime firstDue = schedule.FirstDueDate;
						string type = schedule.Type;
						rSch.Type = type;
						rSch.FirstDueDate = firstDue;

						rSch.LoanPgm = GetText(5, 16, 6);
						rSch.PaymentAmount = double.Parse(GetText(16, 60, 11).Replace("$", "").Replace(",", ""));
						Loan loan = new Loan();
						loan.FirstDisbursementDate = DateTime.ParseExact(GetText(6, 17, 8), SHORT_DATE_FORMAT, _formatProvider);
						loan.Sequence = int.Parse(GetText(7, 42, 4));
						rSch.Loans = loans;
						rSch.Loan = loan;
						loans.Add(loan);
						schedules.Add(rSch);
						Hit(Key.F8);
					}

					Hit(Key.F12);
					Hit(Key.F12);
				}
				else if (schedule.HasMoreThan1Loan && schedule.HasMoreThan1Tier && !schedule.IsALevelSch && !schedule.IsNotFirstTeir)
				{

					while (!Check4Text(23, 2, "90007"))
					{
						RepaymentSchedule rSch = new RepaymentSchedule();
						DateTime firstDue = schedule.FirstDueDate;
						string type = schedule.Type;
						rSch.Type = type;
						rSch.FirstDueDate = firstDue;
						Hit(Key.F6);//move to the teir level
						rSch.LoanPgm = GetText(5, 16, 6);

						while (targetYear >= int.Parse(string.Format("20{0}", GetText(rowTSX56, 28, 2))) && !(targetYear <= int.Parse(string.Format("20{0}", GetText(rowTSX56 + 1, 28, 2)))))
						{
							rowTSX56++;
							if (rowTSX56 > 21)
							{
								Hit(Key.F8);
								if (!Check4Text(23, 2, "90007"))
								{
									rowTSX56 = 16;
								}
							}
						}

						rSch.PaymentAmount = double.Parse(GetText(rowTSX56, 60, 11).Replace("$", "").Replace(",", ""));
						Loan loan = new Loan();
						loan.FirstDisbursementDate = DateTime.ParseExact(GetText(6, 17, 8), SHORT_DATE_FORMAT, _formatProvider);
						loan.Sequence = int.Parse(GetText(7, 42, 4));
						//For loans with more than one screen of repayment levels, hitting F8 goes to the next screen
						//of repayment levels before going to the next loan. We don't care about repayment levels,
						//so check that we've actually picked up a new loan before adding it to the list.
						if (!loans.Select(p => p.Sequence).Contains(loan.Sequence)) { loans.Add(loan); }
						rSch.Loans = loans;
						rSch.Loan = loan;
						schedules.Add(rSch);
						Hit(Key.F5);//refresh the screen
						Hit(Key.F6);//move back to the loan level
						Hit(Key.F8);//move to the next loan
					}

					Hit(Key.F12);
					Hit(Key.F12);
				}
				else if (!schedule.HasMoreThan1Loan && !schedule.HasMoreThan1Tier && schedule.IsALevelSch && !schedule.IsNotFirstTeir)
				{
					RepaymentSchedule rSch = new RepaymentSchedule();
					DateTime firstDue = schedule.FirstDueDate;
					string type = schedule.Type;
					rSch.Type = type;
					rSch.FirstDueDate = firstDue;
					rSch.LoanPgm = GetText(5, 16, 6);

					rSch.PaymentAmount = double.Parse(GetText(16, 60, 11).Replace("$", "").Replace(",", ""));
					Loan loan = new Loan();
					loan.FirstDisbursementDate = DateTime.ParseExact(GetText(6, 17, 8), SHORT_DATE_FORMAT, _formatProvider);
					loan.Sequence = int.Parse(GetText(7, 42, 4));
					//For loans with more than one screen of repayment levels, hitting F8 goes to the next screen
					//of repayment levels before going to the next loan. We don't care about repayment levels,
					//so check that we've actually picked up a new loan before adding it to the list.
					if (!loans.Select(p => p.Sequence).Contains(loan.Sequence)) { loans.Add(loan); }
					rSch.Loans = loans;
					rSch.Loan = loan;
					schedules.Add(rSch);

					Hit(Key.F12);
					Hit(Key.F12);

				}

				else if (!schedule.HasMoreThan1Loan && !schedule.HasMoreThan1Tier && schedule.IsALevelSch && schedule.IsNotFirstTeir)
				{
					RepaymentSchedule rSch = new RepaymentSchedule();
					DateTime firstDue = schedule.FirstDueDate;
					string type = schedule.Type;
					rSch.Type = type;
					rSch.FirstDueDate = firstDue;
					rSch.LoanPgm = GetText(5, 16, 6);

					rSch.PaymentAmount = double.Parse(GetText(17, 60, 11).Replace("$", "").Replace(",", ""));
					Loan loan = new Loan();
					loan.FirstDisbursementDate = DateTime.ParseExact(GetText(6, 17, 8), SHORT_DATE_FORMAT, _formatProvider);
					loan.Sequence = int.Parse(GetText(7, 42, 4));
					//For loans with more than one screen of repayment levels, hitting F8 goes to the next screen
					//of repayment levels before going to the next loan. We don't care about repayment levels,
					//so check that we've actually picked up a new loan before adding it to the list.
					if (!loans.Select(p => p.Sequence).Contains(loan.Sequence)) { loans.Add(loan); }
					rSch.Loans = loans;
					rSch.Loan = loan;
					schedules.Add(rSch);

					Hit(Key.F12);
					Hit(Key.F12);

				}



				Hit(Key.F8);
				return schedules;

			}



			else if (Check4Text(1, 72, "TSX2Y"))
			{
				//Selection screen.
				while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 8; row < 21; row++)
					{
						//Select active schedules.
						if (Check4Text(row, 7, "A"))
						{
							RepaymentSchedule schedule = new RepaymentSchedule();
							schedule.Type = GetText(row, 11, 2);
							//schedule.PaymentAmount = double.Parse(GetText(row, 15, 10).Replace(",", ""));
							schedule.FirstDueDate = DateTime.ParseExact(GetText(row, 40, 8), SHORT_DATE_FORMAT, _formatProvider);
							PutText(21, 14, GetText(row, 3, 2), Key.Enter, true);
							Debug.Assert(Check4Text(1, 71, "TSX3W"), "You must be on TSX3W to get the loans for a repayment schedule");
							Hit(Key.F4);
							schedule.HasMoreThan1Loan = Check4Text(2, 74, "+");
							schedule.HasMoreThan1Tier = Check4Text(13, 74, "+");
							schedule.IsALevelSch = Check4Text(18, 9, " ");
							if (schedule.IsALevelSch)
							{
								if (int.Parse(GetText(16, 47, 3)) <= 1)
								{
									schedule.IsNotFirstTeir = true;
								}
							}
							else
							{
								schedule.IsNotFirstTeir = false;
							}

							List<Loan> loans = new List<Loan>();

							int rowTSX56 = 16;
							int targetYear = DateTime.Now.Year;
							if (schedule.HasMoreThan1Tier && !schedule.HasMoreThan1Loan && !schedule.IsNotFirstTeir)
							{
								RepaymentSchedule rSch = new RepaymentSchedule();
								DateTime firstDue = schedule.FirstDueDate;
								string type = schedule.Type;
								rSch.Type = type;
								rSch.FirstDueDate = firstDue;
								rSch.LoanPgm = GetText(5, 16, 6);
								while (targetYear >= int.Parse(string.Format("20{0}", GetText(rowTSX56, 28, 2))) && targetYear <= int.Parse(string.Format("20{0}", GetText(rowTSX56, 28, 2))))
								{
									rowTSX56++;
									if (rowTSX56 > 21)
									{
										Hit(Key.F8);
										if (!Check4Text(23, 2, "90007"))
										{
											rowTSX56 = 16;
										}
									}
								}

								rSch.PaymentAmount = double.Parse(GetText(rowTSX56, 60, 11).Replace("$", "").Replace(",", ""));
								Loan loan = new Loan();
								loan.FirstDisbursementDate = DateTime.ParseExact(GetText(6, 17, 8), SHORT_DATE_FORMAT, _formatProvider);
								loan.Sequence = int.Parse(GetText(7, 42, 4));
								//For loans with more than one screen of repayment levels, hitting F8 goes to the next screen
								//of repayment levels before going to the next loan. We don't care about repayment levels,
								//so check that we've actually picked up a new loan before adding it to the list.
								if (!loans.Select(p => p.Sequence).Contains(loan.Sequence)) { loans.Add(loan); }
								rSch.Loans = loans;
								rSch.Loan = loan;
								schedules.Add(rSch);

								Hit(Key.F12);
								Hit(Key.F12);
							}
							else if (schedule.HasMoreThan1Loan && !schedule.HasMoreThan1Tier && schedule.IsALevelSch && !schedule.IsNotFirstTeir)
							{
								while (!Check4Text(23, 2, "90007"))
								{
									RepaymentSchedule rSch = new RepaymentSchedule();
									DateTime firstDue = schedule.FirstDueDate;
									string type = schedule.Type;
									rSch.Type = type;
									rSch.FirstDueDate = firstDue;

									rSch.LoanPgm = GetText(5, 16, 6);
									rSch.PaymentAmount = double.Parse(GetText(16, 60, 11).Replace("$", "").Replace(",", ""));
									Loan loan = new Loan();
									loan.FirstDisbursementDate = DateTime.ParseExact(GetText(6, 17, 8), SHORT_DATE_FORMAT, _formatProvider);
									loan.Sequence = int.Parse(GetText(7, 42, 4));
									rSch.Loans = loans;
									rSch.Loan = loan;
									loans.Add(loan);
									schedules.Add(rSch);
									Hit(Key.F8);
								}

								Hit(Key.F12);
								Hit(Key.F12);
							}
							else if (schedule.HasMoreThan1Loan && schedule.HasMoreThan1Tier && !schedule.IsALevelSch && !schedule.IsNotFirstTeir)
							{

								while (!Check4Text(23, 2, "90007"))
								{
									RepaymentSchedule rSch = new RepaymentSchedule();
									DateTime firstDue = schedule.FirstDueDate;
									string type = schedule.Type;
									rSch.Type = type;
									rSch.FirstDueDate = firstDue;
									Hit(Key.F6);//move to the teir level
									rSch.LoanPgm = GetText(5, 16, 6);

									while (targetYear >= int.Parse(string.Format("20{0}", GetText(rowTSX56, 28, 2))) && !(targetYear <= int.Parse(string.Format("20{0}", GetText(rowTSX56 + 1, 28, 2)))))
									{
										rowTSX56++;
										if (rowTSX56 > 21)
										{
											Hit(Key.F8);
											if (!Check4Text(23, 2, "90007"))
											{
												rowTSX56 = 16;
											}
										}
									}

									rSch.PaymentAmount = double.Parse(GetText(rowTSX56, 60, 11).Replace("$", "").Replace(",", ""));
									Loan loan = new Loan();
									loan.FirstDisbursementDate = DateTime.ParseExact(GetText(6, 17, 8), SHORT_DATE_FORMAT, _formatProvider);
									loan.Sequence = int.Parse(GetText(7, 42, 4));
									//For loans with more than one screen of repayment levels, hitting F8 goes to the next screen
									//of repayment levels before going to the next loan. We don't care about repayment levels,
									//so check that we've actually picked up a new loan before adding it to the list.
									if (!loans.Select(p => p.Sequence).Contains(loan.Sequence)) { loans.Add(loan); }
									rSch.Loans = loans;
									rSch.Loan = loan;
									schedules.Add(rSch);
									Hit(Key.F5);//refresh the screen
									Hit(Key.F6);//move back to the loan level
									Hit(Key.F8);//move to the next loan
								}

								Hit(Key.F12);
								Hit(Key.F12);
							}
							else if (!schedule.HasMoreThan1Loan && !schedule.HasMoreThan1Tier && schedule.IsALevelSch && !schedule.IsNotFirstTeir)
							{
								RepaymentSchedule rSch = new RepaymentSchedule();
								DateTime firstDue = schedule.FirstDueDate;
								string type = schedule.Type;
								rSch.Type = type;
								rSch.FirstDueDate = firstDue;
								rSch.LoanPgm = GetText(5, 16, 6);

								rSch.PaymentAmount = double.Parse(GetText(16, 60, 11).Replace("$", "").Replace(",", ""));
								Loan loan = new Loan();
								loan.FirstDisbursementDate = DateTime.ParseExact(GetText(6, 17, 8), SHORT_DATE_FORMAT, _formatProvider);
								loan.Sequence = int.Parse(GetText(7, 42, 4));
								//For loans with more than one screen of repayment levels, hitting F8 goes to the next screen
								//of repayment levels before going to the next loan. We don't care about repayment levels,
								//so check that we've actually picked up a new loan before adding it to the list.
								if (!loans.Select(p => p.Sequence).Contains(loan.Sequence)) { loans.Add(loan); }
								rSch.Loans = loans;
								rSch.Loan = loan;
								schedules.Add(rSch);

								Hit(Key.F12);
								Hit(Key.F12);

							}
							else if (!schedule.HasMoreThan1Loan && !schedule.HasMoreThan1Tier && !schedule.IsALevelSch && !schedule.IsNotFirstTeir)
							{
								RepaymentSchedule rSch = new RepaymentSchedule();
								DateTime firstDue = schedule.FirstDueDate;
								string type = schedule.Type;
								rSch.Type = type;
								rSch.FirstDueDate = firstDue;
								rSch.LoanPgm = GetText(5, 16, 6);

								rSch.PaymentAmount = double.Parse(GetText(16, 60, 11).Replace("$", "").Replace(",", ""));
								Loan loan = new Loan();
								loan.FirstDisbursementDate = DateTime.ParseExact(GetText(6, 17, 8), SHORT_DATE_FORMAT, _formatProvider);
								loan.Sequence = int.Parse(GetText(7, 42, 4));
								//For loans with more than one screen of repayment levels, hitting F8 goes to the next screen
								//of repayment levels before going to the next loan. We don't care about repayment levels,
								//so check that we've actually picked up a new loan before adding it to the list.
								if (!loans.Select(p => p.Sequence).Contains(loan.Sequence)) { loans.Add(loan); }
								rSch.Loans = loans;
								rSch.Loan = loan;
								schedules.Add(rSch);

								Hit(Key.F12);
								Hit(Key.F12);
							}

							else if (!schedule.HasMoreThan1Loan && !schedule.HasMoreThan1Tier && schedule.IsALevelSch && schedule.IsNotFirstTeir)
							{
								RepaymentSchedule rSch = new RepaymentSchedule();
								DateTime firstDue = schedule.FirstDueDate;
								string type = schedule.Type;
								rSch.Type = type;
								rSch.FirstDueDate = firstDue;
								rSch.LoanPgm = GetText(5, 16, 6);

								rSch.PaymentAmount = double.Parse(GetText(17, 60, 11).Replace("$", "").Replace(",", ""));
								Loan loan = new Loan();
								loan.FirstDisbursementDate = DateTime.ParseExact(GetText(6, 17, 8), SHORT_DATE_FORMAT, _formatProvider);
								loan.Sequence = int.Parse(GetText(7, 42, 4));
								//For loans with more than one screen of repayment levels, hitting F8 goes to the next screen
								//of repayment levels before going to the next loan. We don't care about repayment levels,
								//so check that we've actually picked up a new loan before adding it to the list.
								if (!loans.Select(p => p.Sequence).Contains(loan.Sequence)) { loans.Add(loan); }
								rSch.Loans = loans;
								rSch.Loan = loan;
								schedules.Add(rSch);

								Hit(Key.F12);
								Hit(Key.F12);
							}
						}//if
					}//for
					Hit(Key.F8);
				}//while
			}

			return schedules;
		}//GetRepaymentSchedules()


		#endregion ITS2X

		#region ATS0N
		private bool UpdateGraduatedSchedule(int dueDay, double targetPaymentAmount, bool commit)
		{
			Debug.Assert(Check4Text(1, 72, "TSX0T"), "Need to be on TSX0T before this code runs.");
			Hit(Key.F12);
			Hit(Key.Enter);
			PutText(8, 14, "FG");
			PutText(8, 27, dueDay.ToString());
			int maxTerm = int.Parse(GetText(22, 26, 3));
			PutText(9, 23, maxTerm.ToString());
			Hit(Key.Enter);
			if (Check4Text(23, 2, "06419") || Check4Text(23, 2, "02876") || Check4Text(23, 2, "02877"))
			{
				return false;
			}
			if (Check4Text(23, 2, "01840 PRESS PF4 TO ADD REPAYMENT SCHEDULE"))
			{
				double paymentAmount = double.Parse(GetText(22, 68, 10).Replace(",", ""));
				if (paymentAmount > targetPaymentAmount + 10)
				{
					//Commit the schedule change. (We can't bring the payment down to what it should be.)
					if (commit)
					{
						Hit(Key.F4);
						Hit(Key.F4);
						Hit(Key.F12);
					}
					return true;
				}
				else
				{
					//Reduce the term one month at a time until the payment comes up to what it should be.
					while (Math.Abs(paymentAmount - targetPaymentAmount) > 10)
					{
						maxTerm--;
						Hit(Key.F12);
						Hit(Key.Enter);
						PutText(8, 14, "FG");
						PutText(8, 27, dueDay.ToString());
						PutText(9, 23, maxTerm.ToString());
						Hit(Key.Enter);
						if (Check4Text(23, 2, "06419") || Check4Text(23, 2, "02876"))
						{
							return false;
						}
						paymentAmount = double.Parse(GetText(22, 68, 10).Replace(",", ""));
					}
					if (commit)
					{
						Hit(Key.F4);
						Hit(Key.F4);
					}
					else
					{
						Hit(Key.F12);
					}
					return true;
				}
			}
			else
			{
				Hit(Key.F12);
				return false;
			}
		}//TryGraduatedSchedule()

		private bool UpdateMultipleSchedules(IEnumerable<RepaymentSchedule> schedules, bool commit, Ts0nScreen screen)
		{
			Debug.Assert(Check4Text(1, 72, "TSX0S"), "Need ot be on TSX0S to Continue");

			//Go into each schedule and build a dictionary of interest rates, along with the selections (schedules) that have each rate.
			Dictionary<string, List<Selection>> interestRates = new Dictionary<string, List<Selection>>();
			for (int selectionPage = 1; !Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"); selectionPage++)
			{
				for (int row = 11; Check4Text(row, 3, "_", "X"); row++)
				{
					PutText(row, 3, "X", Key.Enter);
					if (Check4Text(23, 2, "02877"))
					{
						return false;
					}
					string rate = GetText(10, 42, 5);
					if (!interestRates.ContainsKey(rate))
					{
						interestRates.Add(rate, new List<Selection>());
					}
					interestRates[rate].Add(new Selection(selectionPage, row));
					//Back out.
					Hit(Key.F12);
					//We are unsure what page the session will take us to when we back out.  We are moving to the first page so we know exactly where we are at.
					while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
					{
						Hit(Key.F7);
					}
					//Moving to the current page 
					for (int currentPage = 1; currentPage < selectionPage; currentPage++)
					{
						Hit(Key.F8);
					}
					//Screen leaves last selection selected so we need to clear it
					PutText(row, 3, "", true);
				}//for
				Hit(Key.F8);
			}//for



			foreach (string interestRate in interestRates.Keys)
			{
				//Back up to the first page and go through each interest rate, selecting all of the schedules with that rate.
				while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					Hit(Key.F7);
				}
				clearTs0n();
				Hit(Key.F5);//refresh the screen to get rid of any error messages
				for (int page = 1; !Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"); page++)
				{
					//clearTs0n();
					for (int row = 11; Check4Text(row, 3, "_", "X"); row++)
					{
						string selection = interestRates[interestRate].Count(p => p.Page == page && p.Row == row) > 0 ? "X" : string.Empty;
						PutText(row, 3, selection, true);
					}
					Hit(Key.F8);
				}

				Hit(Key.Enter);

				//Get the disbursement dates for all the loans in the group. (Need to be on TSX0R at this point.)
				List<TS0NData> disbursementDatesAndLoanPgm = new List<TS0NData>();
				while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{

					for (int row = 10; Check4Text(row, 3, "X"); row++)
					{
						TS0NData tData = new TS0NData();
						tData.DisbDate = (DateTime.Parse(GetText(row, 5, 10)));
						tData.LoanPgm = GetText(5, 15, 6);
						disbursementDatesAndLoanPgm.Add(tData);
					}
					Hit(Key.F8);
				}//while
				//Sum the payment amount for schedules that have loans in the above list.
				double schedulePaymentAmount = 0;
				foreach (RepaymentSchedule schedule in schedules)
				{
					foreach (TS0NData data in disbursementDatesAndLoanPgm)
					{
						if (data.DisbDate == schedule.Loan.FirstDisbursementDate && data.LoanPgm == schedule.LoanPgm)
						{
							schedulePaymentAmount += schedule.PaymentAmount;
							break;
						}
					}
				}

				if (!UpdateSingleSchedule(schedules.First().Type, schedules.First().FirstDueDate.Day, schedulePaymentAmount, commit, screen))
				{
					return false;
				}
				else
				{
					Hit(Key.F12);
					if (commit)
					{
						if (screen == Ts0nScreen.TSX0P)
						{
							Hit(Key.F12);
							Hit(Key.F12);
							Hit(Key.Enter);
							//if bwr has a TSX0P screen we need to go back to that screen and reselect the same loan pgm to process other schedules if they exsist on TSX0S
						}
						else if (screen == Ts0nScreen.TSX0S)
						{
							FastPath("TX3Z/ATS0N");
							//This will return to TSX0S to select other schedules we need to fastpath to clear the prevois selection
						}
					}//end if
				}//end else
			}//end foreach
			return true;
		}//TryMultipleScheduleUpdates(

		private void clearTs0n()
		{
			for (int row = 11; row < 23 && Check4Text(row, 3, "_", "X"); row++)
			{
				PutText(row, 3, "_");
			}

			Hit(Key.F8);

			while (!Check4Text(23, 2, "90007"))
			{
				for (int row = 11; row < 23 && Check4Text(row, 3, "_", "X"); row++)
				{
					PutText(row, 3, "_");
				}

				Hit(Key.F8);
			}

			Hit(Key.Enter);

		}

		private bool UpdateLevelSchedule(int dueDay, double targetPaymentAmount, bool commit)
		{
			Debug.Assert(Check4Text(1, 72, "TSX0T"), "Need to be on TSX0T before this code runs.");
			Hit(Key.F12);
			Hit(Key.Enter);
			PutText(8, 14, "FS");
			PutText(8, 27, dueDay.ToString());
			PutText(14, 9, targetPaymentAmount.ToString());
			Hit(Key.Enter);
			if (Check4Text(23, 2, "06419") || Check4Text(23, 2, "02876") || Check4Text(23, 2, "02877"))
			{
				return false;
			}
			if (Check4Text(23, 2, "01840 PRESS PF4 TO ADD REPAYMENT SCHEDULE"))
			{
				//Check whether the resulting payment amount is close enough to what we want.
				double paymentAmount = double.Parse(GetText(22, 68, 10).Replace(",", ""));
				if (Math.Abs(paymentAmount - targetPaymentAmount) <= 10)
				{
					if (commit)
					{
						Hit(Key.F4);
						Hit(Key.F4);
					}
					else
					{
						Hit(Key.F12);
					}
					return true;
				}
				else
				{
					//Commit the schedule change. (I guess we don't attempt to adjust level payments.)
					Hit(Key.F4);
					Hit(Key.F4);
					return false;
				}
			}
			else
			{
				Hit(Key.F12);
				return false;
			}
		}//TryLevelSchedule()

		private bool UpdateSingleSchedule(string scheduleType, int dueDay, double targetPaymentAmount, bool commit, Ts0nScreen screen)
		{
			if (!Check4Text(1, 72, "TSX0R"))
			{
				return false;
			}
			Hit(Key.Enter);
			if (Check4Text(23, 2, "06419") || Check4Text(23, 2, "02876") || Check4Text(23, 2, "02877"))
			{
				return false;
			}
			if (scheduleType == "FS") { return UpdateLevelSchedule(dueDay, targetPaymentAmount, commit); }
			else if (scheduleType == "FG") { return UpdateGraduatedSchedule(dueDay, targetPaymentAmount, commit); }
			else
			{
				PutText(8, 14, scheduleType);
				PutText(8, 27, dueDay.ToString());
				Hit(Key.Enter);
				if (Check4Text(23, 2, "06419") || Check4Text(23, 2, "02876"))
				{
					return false;
				}
				if (Check4Text(23, 2, "01840 PRESS PF4 TO ADD REPAYMENT SCHEDULE"))
				{
					double paymentAmount = double.Parse(GetText(22, 68, 10).Replace(",", ""));
					if (Math.Abs(paymentAmount - targetPaymentAmount) <= 10)
					{
						if (commit)
						{
							Hit(Key.F4);
							Hit(Key.F4);
						}
						else
						{
							Hit(Key.F12);
						}
						return true;
					}
					else if (scheduleType == "L")
					{
						return UpdateLevelSchedule(dueDay, targetPaymentAmount, commit);
					}
					else if (scheduleType == "G")
					{
						return UpdateGraduatedSchedule(dueDay, targetPaymentAmount, commit);
					}
				}
				Hit(Key.F12);
				return false;
			}
		}//TryScheduleUpdate()

		private bool UpdateRepaymentSchedule(string ssn, IEnumerable<RepaymentSchedule> activeRepaymentSchedules)
		{
			FastPath("TX3Z/ATS0N{0}", ssn);
			switch (GetText(1, 72, 5))
			{
				case "TSX0P":
					if (UpdateMulipleLoanPgms(activeRepaymentSchedules, false, Ts0nScreen.TSX0P))
					{
						return UpdateMulipleLoanPgms(activeRepaymentSchedules, true, Ts0nScreen.TSX0P);
					}
					else
					{
						return false;
					}
				case "TSX0S":
					if (UpdateMultipleSchedules(activeRepaymentSchedules, false, Ts0nScreen.TSX0S))
					{
						return UpdateMultipleSchedules(activeRepaymentSchedules, true, Ts0nScreen.TSX0S);
					}
					else
					{
						return false;
					}
				case "TSX0R":
					//We should land here only if there's a single schedule.
					if (activeRepaymentSchedules.Count() > 1)
					{
						return false;
					}
					else
					{
						RepaymentSchedule schedule = activeRepaymentSchedules.Single();
						PutText(10, 3, "X");
						return UpdateSingleSchedule(schedule.Type, schedule.FirstDueDate.Day, schedule.PaymentAmount, true, Ts0nScreen.TSX0R);
					}
				default:
					return false;
			}
		}//UpdateRepaymentSchedule()

		private bool UpdateMulipleLoanPgms(IEnumerable<RepaymentSchedule> activeRepaymentSchedules, bool commit, Ts0nScreen screen)
		{
			Debug.Assert(Check4Text(1, 72, "TSX0P", "Need to be on TSX0P to run this"));
			for (int row = 8; !Check4Text(row, 4, "  "); row++)
			{
				PutText(21, 12, GetText(row, 4, 2), Key.Enter, true);
				if (!UpdateMultipleSchedules(activeRepaymentSchedules, commit, screen)) { return false; }
				Hit(Key.F12);
			}
			return true;
		}

		#endregion ATS0N
	}//class
}//namespace
