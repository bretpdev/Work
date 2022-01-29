using System.Collections.Generic;
using System.Linq;
using Q;
using AesSystem = Q.Common.AesSystem;
using Key = Q.ReflectionInterface.Key;

namespace RTRNMAIL
{
	class SessionAccess : ScriptSessionBase
	{
		/// <summary>
		/// Bit-wise flags for possible loan statuses.
		/// </summary>
		public enum LoanStatus
		{
			None = 0,
			Closed = 1,
			Open = 2
		}

		private readonly string SCRIPT_ID;

		public SessionAccess(ReflectionInterface ri, string scriptId)
			: base(ri)
		{
			SCRIPT_ID = scriptId;
		}

		public AesSystem DetermineApplicableSystems(string recipientId)
		{
			AesSystem applicableSystems = AesSystem.None;
			SessionAccess.LoanStatus loanStatuses = GetCompassLoanStatuses(recipientId);
			if ((loanStatuses & SessionAccess.LoanStatus.Closed) == SessionAccess.LoanStatus.Closed || (loanStatuses == SessionAccess.LoanStatus.None)) { applicableSystems |= AesSystem.OneLink; }
			if ((loanStatuses & SessionAccess.LoanStatus.Open) == SessionAccess.LoanStatus.Open) { applicableSystems |= AesSystem.Compass; }
			return applicableSystems;
		}//DetermineWhichSystemToUpdate()

		public IdentifyingInfo GetRecipientInfo(string recipientId)
		{
			recipientId = recipientId.ToUpper();
			if (recipientId.StartsWith("RF@"))
			{
				return GetOneLinkReferenceInfo(recipientId);
			}
			else if (recipientId.StartsWith("P"))
			{
				return GetCompassReferenceInfo(recipientId);
			}
			else
			{
				return GetBorrowerInfo(recipientId);
			}
		}//GetRecipientInfo()

		public List<string> GetPersonTypes(string borrowerId)
		{
			//Person types only apply to borrowers, so return an empty list if a reference ID was passed in.
			if (borrowerId.StartsWith("RF@") || borrowerId.StartsWith("P")) { return new List<string>(); }

			if (borrowerId.Length == 9)
			{
				FastPath("LP22I" + borrowerId);
			}
			else
			{
				FastPath("LP22I;;;;;;" + borrowerId);
			}
			if (!Check4Text(1, 62, "PERSON DEMOGRAPHICS")) { return new List<string>(); }

			HashSet<string> personTypes = new HashSet<string>();
			Hit(Key.F10);
			if (!Check4Text(22, 3, "44024"))
			{
				int row = 7;
				while (!Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY"))
				{
					if (Check4Text(row, 11, GetText(3, 40, 42))) { personTypes.Add(GetText(row, 7, 1)); }
					row++;
					if (row > 18)
					{
						Hit(Key.F8);
						row = 7;
					}
				}//while
			}
			return personTypes.ToList();
		}//GetPersonTypes()

		private IdentifyingInfo GetBorrowerInfo(string borrowerId)
		{
			if (!Check4Text(1, 62, "PERSON DEMOGRAPHICS"))
			{
				if (borrowerId.Length == 10)
				{
					//Account number
					FastPath("LP22I;;;;L;;" + borrowerId);
				}
				else
				{
					//SSN
					FastPath("LP22I" + borrowerId);
				}
			}
			if (!Check4Text(1, 62, "PERSON DEMOGRAPHICS")) { return null; }
			IdentifyingInfo info = new IdentifyingInfo();
			info.SSN = GetText(3, 23, 9);
			info.AccountNumber = GetText(3, 60, 12).Replace(" ", "");
			info.FullName = string.Format("{0} {1}", GetText(4, 44, 12), GetText(4, 5, 35));
			return info;
		}//GetBorrowerInfo()

		private LoanStatus GetCompassLoanStatuses(string recipientId)
		{
			string[] CLOSED_STATUSES = { "DECONVERTED", "PAID IN FULL", "CLAIM PAID" };
			LoanStatus loanStatuses = LoanStatus.None;

			string ssn = recipientId;
			if (recipientId.StartsWith("P"))
			{
				FastPath("TX3Z/ITX1J;" + recipientId);
				ssn = GetText(7, 11, 11).Replace(" ", "");
			}
			else if (recipientId.StartsWith("RF@"))
			{
				FastPath("LP2CI;" + recipientId);
				ssn = GetText(3, 39, 9);
			}

			FastPath("TX3Z/ITS26" + ssn);
			if (Check4Text(1, 72, "TSX29"))
			{
				//Target screen.
				loanStatuses = (Check4Text(3, 10, CLOSED_STATUSES) ? LoanStatus.Closed : LoanStatus.Open);
			}
			else if (Check4Text(1, 72, "TSX28"))
			{
				//Selection screen.
				int row = 8;
				while (!Check4Text(row, 2, "  ") && !Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					//Select the next loan.
					PutText(21, 12, GetText(row, 2, 2), Key.Enter, true);
					//Get the loan status.
					loanStatuses |= (Check4Text(3, 10, CLOSED_STATUSES) ? LoanStatus.Closed : LoanStatus.Open);
					//Stop looking if we've found both open and closed loans.
					bool foundOpenLoan = ((loanStatuses & LoanStatus.Open) != 0);
					bool foundClosedLoan = ((loanStatuses & LoanStatus.Closed) != 0);
					if (foundOpenLoan && foundClosedLoan) { break; }
					//Back out and move on to the next row.
					Hit(Key.F12);
					row++;
					if (row > 19)
					{
						Hit(Key.F8);
						row = 8;
					}
				}//while
			}

			return loanStatuses;
		}//GetCompassLoanStatuses()

		private IdentifyingInfo GetCompassReferenceInfo(string referenceId)
		{
			if (!Check4Text(2, 33, "BORROWER DEMOGRAPHICS")) { FastPath("TX3Z/ITX1J;" + referenceId); }
			if (!Check4Text(2, 33, "BORROWER DEMOGRAPHICS")) { return null; }
			IdentifyingInfo info = new IdentifyingInfo();
			info.SSN = GetText(3, 12, 11).Replace(" ", "");
			info.AccountNumber = referenceId;
			info.FullName = string.Format("{0} {1}", GetText(4, 34, 12), GetText(4, 6, 35));
			return info;
		}//GetCompassReferenceInfo()

		private IdentifyingInfo GetOneLinkReferenceInfo(string referenceId)
		{
			if (!Check4Text(1, 59, "REFERENCE DEMOGRAPHICS")) { FastPath("LP2CI;" + referenceId); }
			if (!Check4Text(1, 59, "REFERENCE DEMOGRAPHICS")) { return null; }
			IdentifyingInfo info = new IdentifyingInfo();
			info.SSN = GetText(3, 39, 9);
			info.AccountNumber = referenceId;
			info.FullName = string.Format("{0} {1}", GetText(4, 44, 12), GetText(4, 5, 35));
			return info;
		}//GetOneLinkReferenceInfo()
	}//class
}//namespace
