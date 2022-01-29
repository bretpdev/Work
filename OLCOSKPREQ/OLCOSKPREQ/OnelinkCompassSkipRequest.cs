using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace OLCOSKPREQ
{
	public class OnelinkCompassSkipRequest : BatchScriptBase
	{
		[Flags]
		private enum ComparisonAndUpdateResults
		{
			None = 0,
			InvalidatedOneLinkAddress = 1,
			InvalidatedOneLinkPhone = 2,
			UpdatedCompass = 4
		}

		[Flags]
		private enum CompassFixResults
		{
			None = 0,
			OneLinkAddressExistsOnCompass = 1,
			OneLinkPhoneExistsOnCompass = 2
		}

		private ErrorReport _errorReport;

		public OnelinkCompassSkipRequest(ReflectionInterface ri)
			: base(ri, "OLCOSKPREQ")
		{
			_errorReport = new ErrorReport(ri);
		}

		public override void Main()
		{

			if (MessageBox.Show("This script requests skip activity on OneLINK for a borrower on Compass.", "OneLINK Compass Skip Request", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK) { return; }
			IEnumerable<string> ssns = GetAllSsnsInQueue75();
			if (ssns.Count() == 0)
			{
				MessageBox.Show("The queue is empty.");
				return;
			}
			WorkThe75Queue();
			VerifySkipRequests(ssns);
			_errorReport.Print();
			MessageBox.Show("The script has completed successfully!");
		}//Main()

		private void WorkThe75Queue()
		{
			while (SelectNext75Task())
			{
				//Use the hotkey from the queue task to go to the COMPASS demographics screen.
				Hit(Key.F6);
				SystemBorrowerDemographics compassDemos = ReadCompassDemographics();

				//If the demographics are valid then the borrower is not skip.
				if (compassDemos.AddrValidityIndicator == "Y" && compassDemos.PhoneValidityIndicator == "Y")
				{
					CloseTask(false);
					continue;
				}

				bool borrowerHasPreclaim = CheckForPreclaim(compassDemos.SSN);
				if (borrowerHasPreclaim) { _errorReport.Exclude(compassDemos.SSN); }

				//Hit up OneLINK to compare the demographics.
				FastPath("LP22C");
				if (Check4Text(22, 3, "47004"))
				{
					//Borrower is not found.
					CloseTask(false);
					continue;
				}
				if (Check4Text(10, 32, "S"))
				{
					//Borrower is in skip.
					_errorReport.Exclude(compassDemos.SSN);
					CloseTask(false);
					continue;
				}
				ComparisonAndUpdateResults results = CompareAndUpdateDemographics(compassDemos);

				//Request skip if COMPASS was not updated.
				bool compassWasUpdated = ((results & ComparisonAndUpdateResults.UpdatedCompass) == ComparisonAndUpdateResults.UpdatedCompass);
				bool skipRequestHadProblem = false;
				if (!compassWasUpdated)
				{
					bool compassAddressIsValid = (compassDemos.AddrValidityIndicator == "Y");
					bool compassPhoneIsValid = (compassDemos.PhoneValidityIndicator == "Y");
					bool oneLinkAddressWasInvalidated = ((results & ComparisonAndUpdateResults.InvalidatedOneLinkAddress) == ComparisonAndUpdateResults.InvalidatedOneLinkAddress);
					bool oneLinkPhoneWasInvalidated = ((results & ComparisonAndUpdateResults.InvalidatedOneLinkPhone) == ComparisonAndUpdateResults.InvalidatedOneLinkPhone);
					skipRequestHadProblem = !RequestSkip(compassDemos.SSN, borrowerHasPreclaim, compassAddressIsValid, compassPhoneIsValid, oneLinkAddressWasInvalidated, oneLinkPhoneWasInvalidated);
				}

				CloseTask(skipRequestHadProblem);
			}//while
		}//WorkThe75Queue()

		private bool SelectNext75Task()
		{
			bool foundTask = false;
			FastPath("TX3Z/ITX6X75");
			if (!Check4Text(23, 2, "01020"))
			{
				while (!foundTask && !Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 8; !Check4Text(row, 4, " "); row += 3)
					{
						if (!Check4Text(row, 75, "P"))
						{
							foundTask = true;
							PutText(21, 19, GetText(row, 4, 1), Key.Enter);
							break;
						}
					}//for
					if (!foundTask)
					{
						if (Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA"))
						{
							Hit(Key.Enter);
						}
						else
						{
							Hit(Key.F8);
						}
					}//if
				}//while
			}//if
			return foundTask;
		}//SelectNext75Task()

		private ComparisonAndUpdateResults CompareAndUpdateDemographics(SystemBorrowerDemographics compassDemos)
		{
			ComparisonAndUpdateResults updateResults = ComparisonAndUpdateResults.None;
			SystemBorrowerDemographics oneLinkDemosForUpdatingCompass = new SystemBorrowerDemographics();
			if (compassDemos.AddrValidityIndicator == "N")
			{
				//See if OneLINK has the same address (or close enough).
				bool addressMatches = (compassDemos.Addr1.SafeSubstring(0, 4) == GetText(11, 9, 4));
				bool cityMatches = (compassDemos.City.SafeSubstring(0, 4) == GetText(13, 9, 4));
				bool zipMatches = (compassDemos.Zip.SafeSubstring(0, 5) == GetText(13, 60, 5));
				if (addressMatches && cityMatches && zipMatches)
				{
					//Same address as the invalid COMPASS address. Invalidate it.
					if (Check4Text(11, 57, "Y"))
					{
						PutText(11, 57, "N");
						PutText(3, 9, "E");
						Hit(Key.F6);
						updateResults |= ComparisonAndUpdateResults.InvalidatedOneLinkAddress;
					}
				}
				else
				{
					//Different from COMPASS address.
					if (Check4Text(11, 57, "N"))
					{
						//Make the address match COMPASS, and keep it invalid.
						PutText(11, 9, compassDemos.Addr1.Replace("#", "NO "));
						PutText(13, 9, compassDemos.City);
						PutText(13, 60, compassDemos.Zip);
						PutText(11, 57, "N");
						PutText(3, 9, "E");
						Hit(Key.F6);
						updateResults |= ComparisonAndUpdateResults.InvalidatedOneLinkAddress;
					}
					else
					{
						//Use OneLINK's valid address to update COMPASS.
						oneLinkDemosForUpdatingCompass.SSN = compassDemos.SSN;
						oneLinkDemosForUpdatingCompass.Addr1 = GetText(11, 9, 20);
						oneLinkDemosForUpdatingCompass.Addr2 = GetText(12, 9, 20).Replace("_", " ").Trim();
						oneLinkDemosForUpdatingCompass.City = GetText(13, 9, 20);
						oneLinkDemosForUpdatingCompass.State = GetText(13, 52, 2);
						oneLinkDemosForUpdatingCompass.Zip = GetText(13, 60, 10);
						oneLinkDemosForUpdatingCompass.AddrValidityDate = DateTime.Parse(GetText(11, 72, 8).ToDateFormat());
					}
				}
			}//if

			if (compassDemos.PhoneValidityIndicator == "N")
			{
				if (Check4Text(14, 12, compassDemos.Phone))
				{
					//Phone valid?
					if (Check4Text(14, 38, "Y"))
					{
						PutText(14, 38, "N");
						PutText(3, 9, "E");
						UpdateMblAndConsent(compassDemos.MBL, compassDemos.Consent);
						updateResults |= ComparisonAndUpdateResults.InvalidatedOneLinkPhone;
					}
				}
				else
				{
					//Phone valid?
					if (Check4Text(14, 38, "N"))
					{
						PutText(14, 12, compassDemos.Phone);
						PutText(14, 38, "N");
						PutText(3, 9, "E");
						UpdateMblAndConsent(compassDemos.MBL, compassDemos.Consent);
						updateResults |= ComparisonAndUpdateResults.InvalidatedOneLinkPhone;
					}
					else
					{
						oneLinkDemosForUpdatingCompass.SSN = compassDemos.SSN;
						oneLinkDemosForUpdatingCompass.Phone = GetText(14, 12, 10);
						oneLinkDemosForUpdatingCompass.PhoneValidityDate = DateTime.Parse(GetText(14, 44, 8).ToDateFormat());
						oneLinkDemosForUpdatingCompass.PrimExt = GetText(14, 27, 4);
                        oneLinkDemosForUpdatingCompass.Type = string.IsNullOrEmpty(GetText(14, 58, 1).Replace("_","")) ? "U" : GetText(14, 58, 1);
                        oneLinkDemosForUpdatingCompass.Consent = string.IsNullOrEmpty(GetText(14, 68, 1).Replace("_", "")) ? "N" : GetText(14, 68, 1);	
					}
				}
			}

			//Post any updates made to LP22.
			Hit(Key.Enter);
			Hit(Key.F6);

			//if OneLink has valid info different than the invalid compass info then add to compass
			CompassFixResults fixResults = CompassFixResults.None;
			if (!string.IsNullOrEmpty(oneLinkDemosForUpdatingCompass.SSN))
			{
				fixResults = FixCompass(oneLinkDemosForUpdatingCompass);
				if (fixResults != (CompassFixResults.OneLinkAddressExistsOnCompass | CompassFixResults.OneLinkPhoneExistsOnCompass))
				{
					updateResults |= ComparisonAndUpdateResults.UpdatedCompass;
				}
			}

			//if phone or address was found, update onelink to compass address or phone and invalidate
			if (fixResults != CompassFixResults.None)
			{
				FixOneLINK(compassDemos, fixResults);
				if ((fixResults & CompassFixResults.OneLinkAddressExistsOnCompass) == CompassFixResults.OneLinkAddressExistsOnCompass)
				{
					updateResults |= ComparisonAndUpdateResults.InvalidatedOneLinkAddress;
				}
				if ((fixResults & CompassFixResults.OneLinkPhoneExistsOnCompass) == CompassFixResults.OneLinkPhoneExistsOnCompass)
				{
					updateResults |= ComparisonAndUpdateResults.InvalidatedOneLinkPhone;
				}
				updateResults &= ~ComparisonAndUpdateResults.UpdatedCompass;
			}

			return updateResults;
		}//CompareAndUpdateDemographics()

		private void VerifySkipRequests(IEnumerable<string> ssns)
		{
			foreach (string ssn in ssns)
			{
				bool skipWasRequested = false;
				FastPath("LP5CI{0}", ssn);
				if (!Check4Text(22, 3, "47004"))
				{
					//Get to the target screen if needed.
					if (Check4Text(1, 75, "SELECT")) { PutText(21, 13, "1", Key.Enter); }
					if (Check4Text(1, 54, "SKIPTRACING REQUEST DISPLAY"))
					{
						//Check that the create date is today.
						skipWasRequested = Check4Text(4, 57, DateTime.Now.ToString("MMddyyyy"));
					}
				}
				if (!skipWasRequested) { _errorReport.Add(ssn); }
			}//foreach
		}//VerifySkipRequests()

		private IEnumerable<string> GetAllSsnsInQueue75()
		{
			List<string> ssns = new List<string>();
			FastPath("TX3Z/ITX6X75");
			if (!Check4Text(23, 2, "01020"))
			{
				while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
				{
					for (int row = 8; !Check4Text(row, 4, " "); row += 3)
					{
						ssns.Add(GetText(row, 6, 9));
					}
					if (Check4Text(23, 2, "01033 PRESS ENTER TO DISPLAY MORE DATA"))
					{
						Hit(Key.Enter);
					}
					else
					{
						Hit(Key.F8);
					}
				}//while
			}//if
			return ssns;
		}//GetAllSsnsInQueue75()

		private bool CheckForPreclaim(string ssn)
		{
			bool hasPreclaim = false;
			FastPath("LC02I{0}", ssn);
			if (Check4Text(1, 66, "PRECLAIM SELECT") && GetText(7, 24, 8).IsNumeric())
			{
				DateTime statusDate = DateTime.Parse(GetText(7, 24, 8).ToDateFormat());
				DateTime threeDaysAgo = DateTime.Now.AddDays(-3).Date;
				//See if the preclaim reason is bankruptcy, monthly payment, or quarterly payment.
				bool preclaimReasonMatches = Check4Text(7, 7, "DB", "DF", "DQ");
				bool conversionReasonIsPreclaim = Check4Text(7, 21, "01");
				hasPreclaim = (preclaimReasonMatches && conversionReasonIsPreclaim && statusDate <= threeDaysAgo);
			}
			return hasPreclaim;
		}//CheckForPreclaim()

		private SystemBorrowerDemographics ReadCompassDemographics()
		{
			SystemBorrowerDemographics compassDemos = new SystemBorrowerDemographics();
			compassDemos.SSN = GetText(1, 11, 9);
			compassDemos.Addr1 = GetText(11, 10, 30);
			compassDemos.AddrValidityIndicator = GetText(11, 55, 1);
			compassDemos.City = GetText(14, 8, 20);
			compassDemos.State = GetText(14, 32, 2);
			compassDemos.Zip = GetText(14, 40, 17);
			compassDemos.Phone = GetText(17, 14, 3) + GetText(17, 23, 3) + GetText(17, 31, 4);
			compassDemos.PhoneValidityIndicator = GetText(17, 54, 1);
			compassDemos.MBL = GetText(16, 20, 1);
			compassDemos.Consent = GetText(16, 30, 1);
			return compassDemos;
		}//Demo()

		private void CloseTask(bool taskHasProblem)
		{
			FastPath("TX3Z/ITX6X75");
			PutText(21, 19, "1");
			Hit(Key.F2);
			if (taskHasProblem)
			{
				PutText(8, 19, "P");
				Hit(Key.Enter);
			}
			else
			{
				PutText(8, 19, "C");
				PutText(9, 19, "COMPL");
				Hit(Key.Enter);
				if (!Check4Text(23, 2, "01005"))
				{
					MessageBox.Show("There was a problem closing the queue task. Contact System Support.");
					EndDLLScript();
				}
			}
		}//CloseTask()

		private CompassFixResults FixCompass(SystemBorrowerDemographics oneLinkDemos)
		{
			CompassFixResults results = CompassFixResults.None;
			FastPath("TX3Z/ITX1J;{0}", oneLinkDemos.SSN);

            bool updatedPhone = false;
            bool updatedAddress = false;
			//Update the COMPASS address if we got an address from OneLINK.
			if (!string.IsNullOrEmpty(oneLinkDemos.Addr1))
			{
                if (!FixCompassAddress(oneLinkDemos))
                {
                    results |= CompassFixResults.OneLinkAddressExistsOnCompass;
                }
                else
                {
                    updatedAddress = true;
                }
			}

			//Update the COMPASS phone if we got a phone from OneLINK.
			if (!string.IsNullOrEmpty(oneLinkDemos.Phone))
			{
				string areaCode = oneLinkDemos.Phone.Substring(0, 3);
				string prefix = oneLinkDemos.Phone.Substring(3, 3);
				string local = oneLinkDemos.Phone.Substring(6, 4);
                if (!FixCompassPhone(areaCode, prefix, local, oneLinkDemos.PhoneValidityDate, oneLinkDemos.Type, oneLinkDemos.Consent))
                {
                    results |= CompassFixResults.OneLinkPhoneExistsOnCompass;
                }
                else
                {
                    updatedPhone = true;
                }
			}

			//Add an activity record if any updates were made.
            string message = string.Empty;
			if (updatedPhone)
			{
                message = "Updated skip borrower’s phone information to match OneLINK.";
			}
            if (updatedAddress)
            {
                message = "Updated skip borrower’s address information to match OneLINK.";
            }
            if (updatedAddress && updatedPhone)
            {
                message = "Updated skip borrower’s address and phone information to match OneLINK.";  
            }

            if (!string.IsNullOrEmpty(message))
            {
                ATD22AllLoans(oneLinkDemos.SSN, "KNOTE", message, false);
            }

			return results;
		}//FixCompass()

		private bool FixCompassAddress(SystemBorrowerDemographics oneLinkDemos)
		{
			//Look through address history.
			Hit(Key.F6);
			Hit(Key.F6);
			bool foundOneLinkAddress = false;
			while (!foundOneLinkAddress && !Check4Text(23, 2, "90007"))
			{
				bool addressMatches = (oneLinkDemos.Addr1.SafeSubstring(0, 4).Trim() == GetText(11, 10, 4));
				bool cityMatches = (oneLinkDemos.City.SafeSubstring(0, 4).Trim() == GetText(14, 8, 4));
				bool zipMatches = (oneLinkDemos.Zip.SafeSubstring(0, 5).Trim() == GetText(14, 40, 17));
				foundOneLinkAddress = (addressMatches && cityMatches && zipMatches);
				if (!foundOneLinkAddress) { Hit(Key.F8); }
			}
			if (foundOneLinkAddress)
			{
				//The OneLINK address has already been in COMPASS, so don't use it again.
				return false;
			}
			else
			{
				//Update the COMPASS address to match OneLINK.
				PutText(1, 4, "C", Key.Enter);
				Hit(Key.F6);
				Hit(Key.F6);
				PutText(11, 10, oneLinkDemos.Addr1, true);
				PutText(12, 10, oneLinkDemos.Addr2, true);
				PutText(14, 8, oneLinkDemos.City, true);
				PutText(14, 32, oneLinkDemos.State, true);
				PutText(14, 40, oneLinkDemos.Zip, true);
				PutText(11, 55, "Y");
				PutText(10, 32, oneLinkDemos.AddrValidityDate.ToString("MMddyy"));
				PutText(8, 18, "56");
				if (!string.IsNullOrEmpty(oneLinkDemos.State))
				{
					PutText(12, 52, "", true);
					PutText(13, 52, "", true);
					PutText(12, 77, "", true);
				}
				Hit(Key.Enter);
				return true;
			}
		}//FixCompassAddress()

		private bool FixCompassPhone(string areaCode, string prefix, string local, DateTime validityDate, string type, string consent)
		{
			//Look through phone history.
			PutText(1, 4, "I", Key.Enter);
			Hit(Key.F6);
			Hit(Key.F6);
			Hit(Key.F6);
			bool foundOneLinkPhone = false;
			while (!foundOneLinkPhone && !Check4Text(23, 2, "90007"))
			{
				foundOneLinkPhone = (Check4Text(17, 14, areaCode) && Check4Text(17, 23, prefix) && Check4Text(17, 31, local));
				if (!foundOneLinkPhone) { Hit(Key.F8); }
			}
			if (foundOneLinkPhone)
			{
				//The OneLINK phone has already been in COMPASS, so don't use it again.
				return false;
			}
			else
			{
				//Update the COMPASS phone to match OneLINK.
				PutText(1, 4, "C", Key.Enter);
				Hit(Key.F6);
				Hit(Key.F6);
				Hit(Key.F6);
				PutText(17, 14, areaCode);
				PutText(17, 23, prefix);
				PutText(17, 31, local);
				PutText(16, 20, type);
				PutText(16, 30, consent);
				PutText(16, 45, validityDate.ToString("MMddyy"));
				PutText(17, 54, "Y");
				PutText(19, 14, "56");
				Hit(Key.Enter);
				return true;
			}
		}//FixCompassPhone()

		//update onelink with compass's invalid info
		private void FixOneLINK(SystemBorrowerDemographics compassDemos, CompassFixResults compassFixResults)
		{
			FastPath("LP22C{0}", compassDemos.SSN);
			if ((compassFixResults & CompassFixResults.OneLinkAddressExistsOnCompass) == CompassFixResults.OneLinkAddressExistsOnCompass)
			{
				PutText(11, 9, compassDemos.Addr1.Replace("#", "NO "), true);
				PutText(12, 9, "", Key.EndKey);
				PutText(13, 9, compassDemos.City, true);
				PutText(13, 60, compassDemos.Zip, true);
				PutText(11, 57, "N");
				PutText(3, 9, "E");
			}
			if ((compassFixResults & CompassFixResults.OneLinkPhoneExistsOnCompass) == CompassFixResults.OneLinkPhoneExistsOnCompass)
			{
				PutText(14, 12, compassDemos.Phone);
				PutText(14, 38, "N");
				PutText(3, 9, "E");
			}
			Hit(Key.Enter);
			Hit(Key.F6);
		}//FixOneLINK()

		private bool RequestSkip(string ssn, bool borrowerHasPreclaim, bool compassAddressIsValid, bool compassPhoneIsValid, bool oneLinkAddressWasInvalidated, bool oneLinkPhoneWasInvalidated)
		{
			bool multiLoan = false;
			if (borrowerHasPreclaim && (!compassAddressIsValid || !compassPhoneIsValid)) { return false; }
			FastPath("LP5CA");
			PutText(3, 39, "700126");
			PutText(3, 48, "003");
			PutText(5, 22, "20");
			Hit(Key.Enter);
			//if 480003 is not found then there was an error
			if (Check4Text(22, 3, "04016"))
			{
				//if multiple records are added then close queue task and go to next record
				multiLoan = true;
				//if address or phone was modified then add comment.
				if (oneLinkAddressWasInvalidated || oneLinkPhoneWasInvalidated)
				{
					AddCommentInLP50(ssn, "KGNRL", "AM", "18", "Skip account - Address updated to match Compass");
				}
			}
			else if (Check4Text(22, 3, "48003"))
			{
				//if address or phone was modified then add comment.
				if (oneLinkAddressWasInvalidated || oneLinkPhoneWasInvalidated)
				{
					AddCommentInLP50(ssn, "KGNRL", "AM", "18", "Skip account - Address updated to match Compass");
				}
			}
			else
			{
				return false;
			}
			if (!multiLoan)
			{
				AddCommentInLP50(ssn, "KSKPM", "AM", "18", "Initiated Skip for Servicer");
			}
			return true;
		}//RequestSkip()

		private void UpdateMblAndConsent(string mbl, string consent)
		{
			if (mbl.Contains("M") && consent.Contains("Y")) { PutText(14, 68, "P"); }
			else if (mbl.Contains("M") && consent.Contains("N")) { PutText(14, 68, "N"); }
			else if ((mbl.Contains("Y") && consent.Contains("Y")) || (mbl.Contains("L") && consent.Contains("N"))) { PutText(14, 68, "L"); }
			else if ((mbl.Contains("U") && consent.Contains("Y")) || (mbl.Contains("U") && consent.Contains("N"))) { PutText(14, 68, "T"); }
		}
	}//class
}//namespace
