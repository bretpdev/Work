using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
	abstract class ComparePathBase : ProcessingPathBase
	{
		protected enum PhoneType
		{
			Alternate,
			Home,
			Mobile,
			Work
		}

		protected ComparePathBase(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, GeneralRecovery recovery, ErrorReport errorReport, ProcessLogRun logRun, DataAccess da, string userid, string scriptId)
			: base(ri, demographicsReviewQueue, foreignDemographicsQueue, recovery, errorReport, logRun, da, userid, scriptId)
		{
		}

		public abstract void Process(QueueTask task, QueueData queueDetails, bool pauseOnQueueClosingError);

		protected abstract bool DemographicsPassReview(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError, bool borrowerExistsOnOnelink);

		protected virtual bool UpdateCompass(QueueData queueData, QueueTask task) { throw new NotImplementedException(); } //This is required for the compiler

		protected abstract bool UpdateOneLink(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError);

		/// <summary>
		/// Checks to see if a borrower is on Onelink
		/// </summary>
		/// <param name="ssn"></param>
		/// <returns>True if the borrower is in Onelink.  False if they are not.</returns>
		protected bool BorrowerExistsOnOnelink(string ssn)
		{
			RI.FastPath("LP22I" + ssn);
			return RI.CheckForText(1, 69, "DEMOGRAPHICS");
		}

		#region Address
		/// <summary>
		/// Determine whether the address is foreign or not
		/// </summary>
		/// <param name="taskDemographics"></param>
		/// <param name="borroerExistsOnOnelink"></param>
		/// <returns></returns>
		protected bool AddressIsForeign(AccurintRDemographics taskDemographics, bool borroerExistsOnOnelink)
		{
			//Check the task demographics first, since that's quick and easy.
			if (taskDemographics.Country.IsPopulated())
				return true;

			//Check the current system demographics next.
			//Make sure we at least got an SSN or account number.
			if (taskDemographics.AccountNumber.IsNullOrEmpty() && taskDemographics.Ssn.IsNullOrEmpty())
			{
				throw new Exception("SSN and account number are both missing.");
			}
			//Hit up LP22 to get the borrower's legal address.
			if (taskDemographics.Ssn.IsPopulated() && taskDemographics.Ssn.Length == 9)
			{
				if (borroerExistsOnOnelink)
					RI.FastPath(string.Format("LP22I{0};;;;L", taskDemographics.Ssn));
				else
					RI.FastPath("TX3Z/ITX1JB" + taskDemographics.Ssn);
			}
			else
			{
				if (borroerExistsOnOnelink)
					RI.FastPath(string.Format("LP22I;;;L;;;{0}", taskDemographics.AccountNumber));
				else
				{
					RI.FastPath("TX3Z/ITX1JB*");
					RI.PutText(6, 61, taskDemographics.AccountNumber);
				}
			}
			//See if the COUNTRY field is populated.
			if (borroerExistsOnOnelink)
				return (RI.GetText(11, 55, 25).Length > 0);
			else
				return (RI.GetText(12, 77, 2).Replace("__", "").Length > 0);
		}

		/// <summary>
		/// Check to see if we have had this address before and marked it as invalid
		/// </summary>
		/// <param name="taskDemographics"></param>
		/// <returns></returns>
		protected bool AddressIsInvalidInOneLinkWithinPastYear(AccurintRDemographics taskDemographics)
		{
			//Make sure we at least got an SSN or account number.
			if (taskDemographics.AccountNumber.IsNullOrEmpty() && taskDemographics.Ssn.IsNullOrEmpty())
			{
				throw new Exception("SSN and account number are both missing.");
			}

			//Get the borrower's SSN if needed.
			if (taskDemographics.Ssn.IsNullOrEmpty() || taskDemographics.Ssn.Length < 9)
			{
				RI.FastPath(string.Format("LP22I;;;L;;;{0}", taskDemographics.AccountNumber));
				taskDemographics.Ssn = RI.GetText(3, 23, 9);
			}

			//Hit up LP2J to get the borrower's address history.
			RI.FastPath(string.Format("LP2JI{0};X;;;Y", taskDemographics.Ssn));
			if (RI.CheckForText(22, 3, "47004")) 
				return false; 

			//Check whether we got the selection screen or the target screen.
			if (RI.CheckForText(1, 75, "SELECT"))
			{
				//Selection screen. Select all.
				RI.PutText(3, 13, "X", Key.Enter);

				//Look through the historical addresses for a match.
				for (int dateRow = 4; dateRow <= 22; dateRow += 6)
				{
					//Move on to the next page if we're done with the current one.
					if (!RI.CheckForText(dateRow, 63, "DATE"))
					{
						RI.Hit(Key.F8);
						dateRow = 4;
					}
					//Stop looking when we run out of address history.
					if (RI.CheckForText(22, 3, "46004")) 
						break;

					//Stop looking once we get a date that's older than a year.
					string date = RI.GetText(dateRow, 68, 2) + RI.GetText(dateRow, 70, 2) + RI.GetText(dateRow, 72, 4);
					if (date == "MMDDCCYY") 
						break; 

					int addressMonth = int.Parse(RI.GetText(dateRow, 68, 2));
					int addressDay = int.Parse(RI.GetText(dateRow, 70, 2));
					int addressYear = int.Parse(RI.GetText(dateRow, 72, 4));
					DateTime addressDate = new DateTime(addressYear, addressMonth, addressDay);
					if (DateTime.Now.AddYears(-1) > addressDate) 
						break; 

					//Check the address validity indicator.
					if (RI.CheckForText(dateRow + 1, 80, "Y")) 
						continue;

					//Check for an exact match on the state.
					if (!RI.CheckForText(dateRow + 3, 45, taskDemographics.State)) 
						continue;

					//Check for a match on the first five digits of the zip code.
					if (!RI.CheckForText(dateRow + 3, 52, taskDemographics.ZipCode.SafeSubString(0, 5))) 
						continue;

					//Check that the address in question's numeric groups are
					//all represented in the on-screen address, and vice-versa.
					List<string> addressNumberGroups = taskDemographics.Address1.GetNumericGroups();
					List<string> onScreenNumberGroups = (RI.GetText(dateRow + 1, 9, 35) + RI.GetText(dateRow + 2, 9, 35)).GetNumericGroups();
					bool numbersAllMatch = true;
					foreach (string addressNumber in addressNumberGroups)
					{
						if (!onScreenNumberGroups.Contains(addressNumber))
						{
							numbersAllMatch = false;
							break;
						}
					}
					if (!numbersAllMatch) 
						continue;
					foreach (string onScreenNumber in onScreenNumberGroups)
					{
						if (!addressNumberGroups.Contains(onScreenNumber))
						{
							numbersAllMatch = false;
							break;
						}
					}
					if (!numbersAllMatch)
						continue;

					//If none of the above checks fail and we get to this point, we have a match.
					return true;
				}//for
			}
			else
			{
				//Target screen. Check the one record.
				string date = RI.GetText(4, 68, 2) + RI.GetText(4, 70, 2) + RI.GetText(4, 72, 4);
				if (date == "MMDDCCYY")
					return false;

				int addressMonth = int.Parse(RI.GetText(4, 68, 2));
				int addressDay = int.Parse(RI.GetText(4, 70, 2));
				int addressYear = int.Parse(RI.GetText(4, 72, 4));
				DateTime addressDate = new DateTime(addressYear, addressMonth, addressDay);
				if (DateTime.Now.AddYears(-1) > addressDate)
					return false;
				if (RI.CheckForText(5, 80, "Y"))
					return false;
				if (!RI.CheckForText(7, 45, taskDemographics.State))
					return false;
				if (!RI.CheckForText(7, 52, taskDemographics.ZipCode.SafeSubString(0, 5)))
					return false;
				List<string> addressNumberGroups = taskDemographics.Address1.GetNumericGroups();
				List<string> onScreenNumberGroups = (RI.GetText(5, 9, 35) + RI.GetText(6, 9, 35)).GetNumericGroups();
				foreach (string addressNumber in addressNumberGroups)
				{
					if (!onScreenNumberGroups.Contains(addressNumber))
						return false;
				}
				foreach (string onScreenNumber in onScreenNumberGroups)
				{
					if (!addressNumberGroups.Contains(onScreenNumber))
						return false;
				}
				return true;
			}

			//If we didn't return true before now, then no matches were found.
			return false;
		}

		/// <summary>
		/// Compare new address to active address in compass.
		/// </summary>
		/// <param name="taskDemographics"></param>
		/// <returns></returns>
		protected bool AddressMatchesCompass(AccurintRDemographics taskDemographics)
		{
			//Hit up TX1J to get the borrower's address.
			RI.FastPath(string.Format("TX3Z/CTX1JB;{0}", taskDemographics.Ssn));
			if (RI.CheckForText(23, 2, "01080"))
			{
				RI.FastPath(string.Format("TX3Z/CTX1JS;{0}", taskDemographics.Ssn));
				if (RI.CheckForText(23, 2, "01019", "01222"))
				{
					RI.FastPath(string.Format("TX3Z/CTX1JE;{0}", taskDemographics.Ssn));
				}
			}
			if (!RI.CheckForText(1, 71, "TXX"))
			{
				throw new Exception(RI.GetText(23, 2, 77));
			}

			//The column for address type is different depending on the person type.
			int addressTypeColumn = (RI.CheckForText(1, 71, "TXX1R-02", "TXX1R-04") ? 13 : 14);

			//Go to the legal address if it's not already displayed.
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);
			if (!RI.CheckForText(10, addressTypeColumn, "L"))
			{
				RI.PutText(10, addressTypeColumn, "L", Key.Enter);
			}

			//Only valid addresses can be considered a match.
			if (RI.CheckForText(11, 55, "N"))
				return false;

			//Compare the state, zip, and street address.
			if (!RI.CheckForText(14, 32, taskDemographics.State))
				return false;
			if (!RI.CheckForText(14, 40, taskDemographics.ZipCode.SafeSubString(0, 5)))
				return false;
			List<string> addressNumberGroups = taskDemographics.Address1.GetNumericGroups();
			List<string> onScreenNumberGroups = (RI.GetText(11, 10, 35) + RI.GetText(12, 10, 35)).GetNumericGroups();
			foreach (string addressNumber in addressNumberGroups)
			{
				if (!onScreenNumberGroups.Contains(addressNumber))
					return false;
			}
			foreach (string onScreenNumber in onScreenNumberGroups)
			{
				if (!addressNumberGroups.Contains(onScreenNumber))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Compare new address to active address in one link
		/// </summary>
		/// <param name="taskDemographics"></param>
		/// <returns></returns>
		protected virtual bool AddressMatchesOneLink(AccurintRDemographics taskDemographics)
		{
			//Make sure we at least got an SSN or account number.
			if (taskDemographics.AccountNumber.IsNullOrEmpty() && (taskDemographics.Ssn.IsNullOrEmpty()))
			{
				throw new Exception("SSN and account number are both missing.");
			}

			//Hit up LP22 to get the borrower's legal address.
			if (taskDemographics.Ssn.IsPopulated() && taskDemographics.Ssn.Length == 9)
				RI.FastPath(string.Format("LP22I{0};;;;L", taskDemographics.Ssn));
			else
				RI.FastPath(string.Format("LP22I;;;L;;;{0}", taskDemographics.AccountNumber));

			//Compare the address in question to the borrower's legal address.
			if (!RI.CheckForText(12, 60, taskDemographics.ZipCode.SafeSubString(0, 5)))
				return false;
			if (!RI.CheckForText(12, 52, taskDemographics.State))
				return false;
			List<string> addressNumberGroups = taskDemographics.Address1.GetNumericGroups();
			List<string> onScreenNumberGroups = (RI.GetText(10, 9, 35) + RI.GetText(11, 9, 35)).GetNumericGroups();
			foreach (string addressNumber in addressNumberGroups)
			{
				if (!onScreenNumberGroups.Contains(addressNumber))
					return false;
			}
			foreach (string onScreenNumber in onScreenNumberGroups)
			{
				if (!addressNumberGroups.Contains(onScreenNumber))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Check to see if the current address in the address history for the account.
		/// </summary>
		/// <param name="task"></param>
		/// <returns></returns>
		protected bool CompareAddressHistory(QueueTask task)
		{
			RI.FastPath("TX3Z/ITX1JB" + task.Demographics.Ssn);
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);

			if (!RI.CheckForText(10, 14, "L"))
				RI.PutText(10, 14, "L", Key.Enter);

			RI.Hit(Key.F7);
			bool hasPreviousAddress = false;
			while (!RI.CheckForText(23, 2, "90007"))
			{
				if (AddressMatchesCompass(task.Demographics))
				{
					hasPreviousAddress = true;
					break;
				}
			}

			return hasPreviousAddress;
		}
		#endregion Address

		#region Phone
		/// <summary>
		/// Check to see if the number is a foreign number
		/// </summary>
		/// <param name="ssnOrAccountNumber"></param>
		/// <param name="borrowerExistsOnOnelink"></param>
		/// <returns></returns>
		protected bool PhoneNumberIsForeign(string ssnOrAccountNumber, bool borrowerExistsOnOnelink)
		{
			//Hit up LP22 to get the borrower's legal address.
			if (ssnOrAccountNumber.Length == 9)
			{
				if (borrowerExistsOnOnelink)
					RI.FastPath(string.Format("LP22I{0};;;;L", ssnOrAccountNumber));
				else
					RI.FastPath("TX3Z/ITX1JB" + ssnOrAccountNumber);
			}
			else
			{
				if (borrowerExistsOnOnelink)
					RI.FastPath(string.Format("LP22I;;;L;;;{0}", ssnOrAccountNumber));
				else
				{
					RI.FastPath("TX3Z/ITX1JB*");
					RI.PutText(6, 61, ssnOrAccountNumber);
				}
			}
			//See if the COUNTRY field is populated.
			if (borrowerExistsOnOnelink)
				return (RI.GetText(16, 14, 17).Length > 0);
			else
				return !(RI.GetText(18, 15, 3).Replace("__", "").Length > 0);
		}

		/// <summary>
		/// Check if we have had this number marked bad in the last year
		/// </summary>
		/// <param name="ssnOrAccountNumber"></param>
		/// <param name="phoneNumber"></param>
		/// <returns></returns>
		protected bool PhoneNumberIsInvalidInOneLinkWithinPastYear(string ssnOrAccountNumber, string phoneNumber)
		{
			//Get the borrower's SSN if needed.
			string ssn = ssnOrAccountNumber;
			if (ssnOrAccountNumber.Length == 10)
			{
				RI.FastPath(string.Format("LP22I;;;L;;;{0}", ssnOrAccountNumber));
				ssn = RI.GetText(3, 23, 9);
			}

			//Hit up LP2J to get the borrower's phone history.
			RI.FastPath(string.Format("LP2JI{0};X;;Y", ssn));
			if (RI.CheckForText(22, 3, "47004"))
				return false;

			//Check whether we got the selection screen or the target screen.
			if (RI.CheckForText(1, 75, "SELECT"))
			{
				//Selection screen. Select all.
				RI.PutText(3, 13, "X", Key.Enter);

				//Look through the historical phone numbers for a match.
				for (int dateRow = 4; dateRow <= 22; dateRow += 6)
				{
					//Move on to the next page if we're done with the current one.
					if (!RI.CheckForText(dateRow, 54, "DATE"))
					{
						RI.Hit(Key.F8);
						dateRow = 4;
					}
					//Stop looking when we run out of address history.
					if (RI.CheckForText(22, 3, "46004"))
						break;

					//Stop looking once we get a date that's older than a year.
					int monthColumn = 59;
					int dayColumn = 61;
					int yearColumn = 63;
					//AES didn't implement the changes to LP2J when the other OneLINK changes happened,
					//so check which version we're working with. Once they put that in place, we can just go with the new position (above).
					if (RI.CheckForText(dateRow + 4, 2, "HOME"))
					{
						monthColumn = 24;
						dayColumn = 26;
						yearColumn = 28;
					}

					try
					{
						string date = RI.GetText(dateRow, monthColumn, 2) + RI.GetText(dateRow, dayColumn, 2) + RI.GetText(dateRow, yearColumn, 4);
						if (date == "MMDDCCYY")
							break;

						int addressMonth = int.Parse(RI.GetText(dateRow, monthColumn, 2));
						int addressDay = int.Parse(RI.GetText(dateRow, dayColumn, 2));
						int addressYear = int.Parse(RI.GetText(dateRow, yearColumn, 4));
						DateTime addressDate = new DateTime(addressYear, addressMonth, addressDay);

						if (DateTime.Now.AddYears(-1) > addressDate) 
							break;

						//Check for a matching phone number (either HOME or OTHER) with a validity indicator of N.
						int primaryPhoneColumn = 10;
						//AES didn't implement the changes to LP2J when the other OneLINK changes happened,
						//so check which version we're working with. Once they put that in place, we can just go with the new position (above).
						if (RI.CheckForText(dateRow + 4, 2, "HOME")) 
							primaryPhoneColumn = 7; //Old position.
						if ((RI.CheckForText(dateRow + 4, primaryPhoneColumn, phoneNumber) && RI.CheckForText(dateRow + 4, 38, "N")) || (RI.CheckForText(dateRow + 4, 49, phoneNumber) && RI.CheckForText(dateRow + 4, 80, "N")))
							return true;
					}
					catch (FormatException)
					{
						continue;
					}
				}
			}
			else
			{
				//Target screen. Check the one record.
				//AES didn't implement the changes to LP2J when the other OneLINK changes happened,
				//so check which version we're working with. Once they put that in place, we can just go with the new position ("else" block).
				if (RI.CheckForText(8, 2, "HOME"))
				{
					string date = RI.GetText(4, 24, 2) + RI.GetText(4, 26, 2) + RI.GetText(4, 28, 4);
					if (date == "MMDDCCYY") 
						return false;

					int addressMonth = int.Parse(RI.GetText(4, 24, 2));
					int addressDay = int.Parse(RI.GetText(4, 26, 2));
					int addressYear = int.Parse(RI.GetText(4, 28, 4));
					DateTime addressDate = new DateTime(addressYear, addressMonth, addressDay);
					if (DateTime.Now.AddYears(-1) > addressDate)
						return false;
					if ((RI.CheckForText(8, 7, phoneNumber) && RI.CheckForText(8, 38, "N")) || (RI.CheckForText(8, 49, phoneNumber) && RI.CheckForText(8, 80, "N")))
						return true;
				}
				else
				{
					string date = RI.GetText(4, 24, 2) + RI.GetText(4, 26, 2) + RI.GetText(4, 28, 4);
					if (date == "MMDDCCYY") 
						return false;

					int addressMonth = int.Parse(RI.GetText(4, 24, 2));
					int addressDay = int.Parse(RI.GetText(4, 26, 2));
					int addressYear = int.Parse(RI.GetText(4, 28, 4));
					DateTime addressDate = new DateTime(addressYear, addressMonth, addressDay);
					if (DateTime.Now.AddYears(-1) > addressDate)
						return false;
					if ((RI.CheckForText(6, 10, phoneNumber) && RI.CheckForText(6, 32, "N")) || (RI.CheckForText(6, 52, phoneNumber) && RI.CheckForText(6, 74, "N")))
						return true;
				}
			}

			//If we didn't return True within the loop, then no matches were found.
			return false;
		}

		/// <summary>
		/// Check compass to determine if our new number is the same as the current one
		/// </summary>
		/// <param name="ssnOrAccountNumber"></param>
		/// <param name="phoneNumber"></param>
		/// <param name="phoneType"></param>
		/// <returns></returns>
		protected bool PhoneNumberMatchesCompass(string ssnOrAccountNumber, string phoneNumber, PhoneType phoneType)
		{
			//Hit up TX1J to get the borrower's address.
			RI.FastPath(string.Format("TX3Z/CTX1JB;{0}", ssnOrAccountNumber));
			if (RI.CheckForText(23, 2, "01080"))
			{
				RI.FastPath(string.Format("TX3Z/CTX1JS;{0}", ssnOrAccountNumber));
				if (RI.CheckForText(23, 2, "01019", "01222"))
					RI.FastPath(string.Format("TX3Z/CTX1JE;{0}", ssnOrAccountNumber));
			}
			if (!RI.CheckForText(1, 71, "TXX"))
			{
				throw new Exception(RI.GetText(23, 2, 77));
			}

			//Go to the correct phone type if it's not already displayed.
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);
			bool phoneMatches = false;
			foreach (string type in new List<string>() { "H", "A", "W", "M" })
			{
				if (!RI.CheckForText(16, 14, type)) 
					RI.PutText(16, 14, type, Key.Enter);

				//Compare the phone number to what's in COMPASS,
				//and check whether the COMPASS phone number is marked as valid.
				string compassPhone = RI.GetText(17, 14, 3) + RI.GetText(17, 23, 3) + RI.GetText(17, 31, 4);
				phoneMatches = (compassPhone == phoneNumber && RI.CheckForText(17, 54, "Y"));
				if (phoneMatches)
					break;
			}
			return phoneMatches;
		}

		/// <summary>
		/// Check to see if our new number matches the onelink number
		/// </summary>
		/// <param name="ssnOrAccountNumber"></param>
		/// <param name="phoneNumber"></param>
		/// <returns></returns>
		protected virtual bool PhoneNumberMatchesOneLink(string ssnOrAccountNumber, string phoneNumber)
		{
			//Hit up LP22 to get the borrower's current demographics.
			if (ssnOrAccountNumber.Length == 9)
				RI.FastPath(string.Format("LP22I{0};;;;L", ssnOrAccountNumber));
			else
				RI.FastPath(string.Format("LP22I;;;L;;;{0}", ssnOrAccountNumber));
			//Compare the phone number in question to the domestic phone on the system.
			if (RI.CheckForText(13, 12, phoneNumber)) 
				return true; 
			//Compare the phone number in question to the alternate phone on the system.
			if (RI.CheckForText(14, 12, phoneNumber)) 
				return true;
			//Return False if neither one matched.
			return false;
		}

		/// <summary>
		/// Check our phone history for the new number
		/// </summary>
		/// <param name="ssn"></param>
		/// <param name="phoneNumber"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		protected int? CompareHistoryPhone(string ssn, string phoneNumber, PhoneType type)
		{
			RI.FastPath("TX3Z/CTX1JB" + ssn);
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);

			RI.Hit(Key.F8);
			int? matchCode = null;
			while (!RI.CheckForText(23, 2, "90007"))
			{

				matchCode = PhoneNumberMatchesCompass(ssn, phoneNumber, type, true);
				if (matchCode.HasValue)
					break;
				RI.Hit(Key.F8);
			}
			return matchCode;
		}

		/// <summary>
		/// Check compass and compare if the number is the same
		/// </summary>
		/// <param name="ssnOrAccountNumber"></param>
		/// <param name="phoneNumber"></param>
		/// <param name="phoneType"></param>
		/// <param name="history"></param>
		/// <returns></returns>
		protected int? PhoneNumberMatchesCompass(string ssnOrAccountNumber, string phoneNumber, PhoneType phoneType, bool history = false)
		{
			//Hit up TX1J to get the borrower's address.
			if (!history)
				RI.FastPath(string.Format("TX3Z/CTX1JB;{0}", ssnOrAccountNumber));
			if (RI.CheckForText(23, 2, "01080"))
			{
				RI.FastPath(string.Format("TX3Z/CTX1JS;{0}", ssnOrAccountNumber));
				if (RI.CheckForText(23, 2, "01019", "01222"))
					RI.FastPath(string.Format("TX3Z/CTX1JE;{0}", ssnOrAccountNumber));
			}
			if (!RI.CheckForText(1, 71, "TXX"))
			{
				throw new Exception(RI.GetText(23, 2, 77));
			}

			//Go to the correct phone type if it's not already displayed.
			if (!history)
			{
				RI.Hit(Key.F6);
				RI.Hit(Key.F6);
				RI.Hit(Key.F6);
			}

			string phoneTypeCode = "H";
			switch (phoneType)
			{
				case PhoneType.Alternate:
					phoneTypeCode = "A";
					break;
				case PhoneType.Work:
					phoneTypeCode = "W";
					break;
			}
			if (!RI.CheckForText(16, 14, phoneTypeCode))
			{
				RI.PutText(16, 14, phoneTypeCode, Key.Enter);
				RI.Hit(Key.F8);
			}

			//Compare the phone number to what's in COMPASS,
			//and check whether the COMPASS phone number is marked as valid.
			string compassPhone = RI.GetText(17, 14, 3) + RI.GetText(17, 23, 3) + RI.GetText(17, 31, 4);
			if (!history)
			{
				if (compassPhone == phoneNumber && RI.CheckForText(17, 54, "Y"))
					return 0;
			}
			else
			{
				if (RI.GetText(16, 45, 8).Replace("_", "").Replace(" ", "") == "")
					return null;
				DateTime temp = DateTime.Parse(RI.GetText(16, 45, 8).Replace(" ", "/"));
				TimeSpan t = DateTime.Now.Subtract(temp);

				if (compassPhone == phoneNumber && RI.CheckForText(17, 54, "Y"))
					return 0;
				else if (compassPhone == phoneNumber && RI.CheckForText(17, 54, "N") && t.Days < 365)
					return 1;
				else
					return null;
			}

			//to make the complier happy.
			return null;
		}
		#endregion Phone
	}
}
