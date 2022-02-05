using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace NoblePhoneInvalidation
{
	public class CompassInvalidator
	{
		public ReflectionInterface RI { get; set; }
		public ProcessLogData LogData { get; set; }
		public BatchProcessingHelper Batch { get; set; }
		public ErrorReport ErrorReportUheaa { get; set; }
		public ErrorReport ErrorReportCornerStone { get; set; }
		private string BorrowerSSN { get; set; }
		private string ArcToLeave { get { return "BADPH"; } }
		private string ScriptId { get { return "NDNUMINVAL"; } }
		static readonly object locker = new object();
		public CompassInvalidator(ProcessLogData logData)
		{
			LogData = logData;
			lock (locker)
			{
				DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
				ErrorReportCornerStone = new ErrorReport("CS Compass Disposition Error", "ERR_BU10");
			}
			lock (locker)
			{
				DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
				ErrorReportUheaa = new ErrorReport("UHEAA Compass Disposition Error", "ERR_BU10");
			}
		}

		/// <summary>
		/// Load a Reflection Interface.
		/// </summary>
		/// <param name="batch">BatchProcessingHelper</param>
		/// <returns>true if successful</returns>
		public bool Login(BatchProcessingHelper batch, DataAccessHelper.Region region)
		{
			DataAccessHelper.CurrentRegion = region;
			Batch = batch;
			RI = new ReflectionInterface();
			return RI.Login(batch.UserName, batch.Password);
		}

		/// <summary>
		/// Takes a NobleData record and checks the number against the green screen.
		/// </summary>
		/// <param name="record">NobleData record from query</param>
		/// <returns>True if successful.</returns>
		public bool InvalidatePhoneNumber(NobleData record)
		{
			List<string> searchResults = new List<string>();
			List<string> phoneTypes = new List<string>(){"H","W","M","A"};
			if (record.AccountIdentifier == null)
			{
				searchResults.AddRange(CheckPhoneList(record, phoneTypes));
				if ((searchResults.Any() && record.Disposition.IsIn("FX","D")) || (searchResults.Count == 1 && record.Disposition == "WN")) //WN is not handled if it matches more than 1 account
				{
					foreach (string identifier in searchResults)
					{
						record.AccountIdentifier = identifier;
						return InvalidatePhoneNumberByAcccount(record);
					}
				}
				return false;
			}
			else if (record.AccountIdentifier.IsNumeric() || record.AccountIdentifier.StartsWith("P0"))
			{
				return InvalidatePhoneNumberByAcccount(record);
			}
			else
				return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="record"></param>
		/// <param name="borrowerSSN"></param>
		/// <returns>True if successful</returns>
		private bool InvalidatePhoneNumberByAcccount(NobleData record)
		{
			RI.FastPath("TX3Z/CTX1J;" + record.AccountIdentifier);
			if (RI.CheckForText(23, 2, "01019"))
			{
				ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Attempted to invalidate borrower {0} but borrower did not exist for object {1}", record.AccountIdentifier, GetErrorMessageFromObject(record)), NotificationType.ErrorReport, NotificationSeverityType.Warning);

				if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone)
				{
					lock (locker)
						ErrorReportCornerStone.AddRecord("Attempt to invalidate borrower phone number failed. Borrower not found.", record);
				}
				else if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
				{
					lock (locker)
						ErrorReportUheaa.AddRecord("Attempt to invalidate borrower phone number failed. Borrower not found.", record);
				}
				return false;
			}
			//borrower
			if (RI.CheckForText(1, 71, "TXX1R-01"))
				return CompassPhoneProcessor(record, BorrowerSSN, "B");
			//co-maker
			else if (RI.CheckForText(1, 71, "TXX1R-02"))
			{
				BorrowerSSN = RI.GetText(7, 11, 11).Replace(" ", "");
				return CompassPhoneProcessor(record, BorrowerSSN, "E");
			}
			else if (RI.CheckForText(1, 71, "TXX1R-03"))
			{
				BorrowerSSN = RI.GetText(7, 11, 11).Replace(" ", "");
				return CompassPhoneProcessor(record, BorrowerSSN, "R");
			}
			else
			{
				ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Expected screen TXX1R-01 -02 or -03 but entered screen {0} instead for account {1}.  Login: {2}", RI.GetText(1, 71, 8), record.AccountIdentifier, Batch.UserName), NotificationType.ErrorReport, NotificationSeverityType.Warning);
				if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone)
				{
					lock (locker)
						ErrorReportCornerStone.AddRecord("Attempt to invalidate borrower phone number failed. Unable to reach TXX1R screen for borrower.", record);
				}
				else if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
				{
					lock (locker)
						ErrorReportUheaa.AddRecord("Attempt to invalidate borrower phone number failed. Unable to reach TXX1R screen for borrower.", record);
				}
				return false;
			}
		}

		/// <summary>
		/// Search for matching account numbers for each type of phone and adds them to searchResults.
		/// </summary>
		/// <param name="record">Used to get phone number</param>
		/// <param name="phoneTypes">H, A, W, M</param>
		/// <returns>List of strings</returns>
		private List<string> CheckPhoneList(NobleData record, List<string> phoneTypes)
		{
			List<string> searchResults = new List<string>();
			foreach (string type in phoneTypes)
			{
				RI.FastPath("TX3Z/CTX1J");
				RI.PutText(17, 16, type, true);
				RI.PutText(18, 16, record.AreaCode, true);
				RI.PutText(18, 22, record.Phone.SafeSubString(0, 3), true);
				RI.PutText(18, 28, record.Phone.SafeSubString(3, 4), true);
				RI.Hit(Key.Enter);
				if (!RI.CheckForText(23, 2, "04473")) //no record found for phone number
				{
					//found record. add to list search results
					if (RI.CheckForText(1, 71, "TXX1R-01") || RI.CheckForText(1, 71, "TXX1R-02") || RI.CheckForText(1, 71, "TXX1R-03"))//1 record
						searchResults.Add(RI.GetText(3, 12, 3) + RI.GetText(3, 16, 2) + RI.GetText(3, 19, 4));
					else if (RI.CheckForText(1, 74, "TXX1L"))
					{
						for (int row = 6; !RI.CheckForText(23, 2, "01027") && !RI.CheckForText(23, 2, "90007"); row += 2)
						{
							if (row > 20 || !RI.CheckForText(row, 3, "  "))
							{
								RI.Hit(Key.F8); //Page to next
								row = 6;
							}
							else
							{
								int? selection = RI.GetText(row, 3, 2).ToIntNullable();
								if (selection == null)
									break;
								RI.PutText(22, 12, selection.ToString(), Key.Enter, true);
								searchResults.Add(RI.GetText(3, 12, 3) + RI.GetText(3, 16, 2) + RI.GetText(3, 19, 4));
								RI.Hit(Key.F12);
							}
						}
					}
				}
				else
					ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Phone number {0} not found in session for account {1}. Screen: {2} Message: {3} Login: {4}", record.PhoneNumber, record.AccountIdentifier, RI.GetText(1, 71, 8), GetErrorMessageFromObject(record), Batch.UserName), NotificationType.ErrorReport, NotificationSeverityType.Informational);
			}
			return searchResults;
		}

		/// <summary>
		/// Calls ComparePhoneCompass and based on results, notes the account.
		/// </summary>
		/// <param name="record">object created from Noble table</param>
		/// <param name="borrowerSSN">If Reference or endorser record, the borrowers ssn for noting.</param>
		/// <param name="regardTo">B E R</param>
		/// <returns>True if successful</returns>
		public bool CompassPhoneProcessor(NobleData record, string borrowerSSN, string regardsTo)
		{
			//get to phone section of green screen
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);
			bool HomeChanged = ComparePhoneCompass(record.PhoneNumber, "H");
			bool AltChanged = ComparePhoneCompass(record.PhoneNumber, "A");
			bool WorkChanged = ComparePhoneCompass(record.PhoneNumber, "W");
			bool MobileChanged = ComparePhoneCompass(record.PhoneNumber, "M");

			string comment = DetermineComment(record);
			if (comment == "")//failed to determine the comment
				return false;

			string ssn = "";
			if (record.AccountIdentifier.Length >= 10)
				ssn = DataAccess.GetSSNFromAccountNumber(record.AccountIdentifier);
			else
				ssn = record.AccountIdentifier;
			//Normal Borrower Note
			if (HomeChanged || AltChanged || WorkChanged || MobileChanged)
			{
				if (borrowerSSN.IsNullOrEmpty()) //if this is null, then it is the borrower that needs the note
				{
					if (!RI.Atd22ByBalance(record.AccountIdentifier, ArcToLeave, comment, ssn, ScriptId, false, false, false))//no pause, not reference, not endorser
						ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Failed to add ARC in TD22 for Borrower: {0} ARC: {1} comment: {2} Login: {3}", record.AccountIdentifier, "BADPH", comment, Batch.UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical);
				}
				else //Reference or Co-Maker Note
				{
					if (regardsTo == "E")
					{
						if (!RI.Atd22ByBalance(borrowerSSN, ArcToLeave, comment, ssn, ScriptId, false, false, true))//no pause, not reference, IS endorser
							ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Failed to add ARC in TD22 for Endorser: {0} ARC: {1} comment: {2} Login: {3}", record.AccountIdentifier, "BADPH", comment, Batch.UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical);
					}
					else if (regardsTo == "R")
					{
						if (!RI.Atd22ByBalance(borrowerSSN, ArcToLeave, comment, ssn, ScriptId, false, true, false))//no pause, IS reference, not endorser
							ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Failed to add ARC in TD22 for Reference: {0} ARC: {1} comment: {2} Login: {3}", record.AccountIdentifier, "BADPH", comment, Batch.UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical);
					}
					else
						ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Expected a Reference or Endorser code but instead found '{0}' for borrower '{1}'", regardsTo, borrowerSSN), NotificationType.ErrorReport, NotificationSeverityType.Critical);
				}
			}
			return true;
		}

		/// <summary>
		/// Determines the comment based on the record disposition
		/// </summary>
		/// <param name="record">Noble call record</param>
		/// <returns>string comment</returns>
		private string DetermineComment(NobleData record)
		{
			string comment = "";
			if (record.Disposition == "D")
				comment = string.Format("{0} shows disconnected and invalidated by dialer", record.PhoneNumber);
			else if (record.Disposition == "FX")
				comment = string.Format("{0} invalidated as fax machine by dialer", record.PhoneNumber);
			else if (record.Disposition == "WN")
				comment = string.Format("{0} dispositioned as wrong number by {1} on {2} at {3}", record.PhoneNumber, record.AgentId, record.EffectiveDate, record.EffectiveTime);
			else
			{
				ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Expected disposition of 'D', 'FX', or 'WN' and received {0} for account {1}", record.Disposition, record.AccountIdentifier), NotificationType.ErrorReport, NotificationSeverityType.Warning);
				if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone)
				{
					lock (locker)
					{
						ErrorReportCornerStone.AddRecord("Attempt to invalidate borrower phone number failed. Invalid disposition type received.", record);
					}
				}
				else if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
				{
					lock (locker)
					{
						ErrorReportUheaa.AddRecord("Attempt to invalidate borrower phone number failed. Invalid disposition type received.", record);
					}
				}
			}
			return comment;
		}

		/// <summary>
		/// Takes a phonenumber and a type (H,M,A,W), compares them to the green screen, invalidates if they match, and if changes are made returns true.
		/// </summary>
		/// <param name="phonenumber">phone to search for</param>
		/// <param name="phoneType">type of phone</param>
		/// <returns>True if a number is found and invalidated.</returns>
		private bool ComparePhoneCompass(string phonenumber, string phoneType)
		{
			RI.PutText(16, 14, phoneType, Key.Enter, true);
			string compareNumber = RI.GetText(17, 14, 3) + RI.GetText(17, 23, 3) + RI.GetText(17, 31, 4);
			//Phone needs to be invalidated
			if (compareNumber == phonenumber && RI.CheckForText(17, 54, "Y"))
			{
				DateTime nowDate = DateTime.Now;
				string month = nowDate.Month.ToString().PadLeft(2, '0');
				string day = nowDate.Day.ToString().PadLeft(2, '0');
				string year = nowDate.Year.ToString().Substring(2);

				RI.PutText(16, 45, month, true);
				RI.PutText(16, 48, day, true);
				RI.PutText(16, 51, year, true);
				RI.PutText(17, 54, "N");
				RI.PutText(19, 14, "41", Key.Enter);
				if(RI.CheckForText(23, 2, "01097"))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the objects header and line to be passed to process logger
		/// </summary>
		/// <param name="record">Object to get properties from</param>
		/// <returns>comma seperated header row followed by a newline and a comma sperated property list</returns>
		private static string GetErrorMessageFromObject(NobleData record)
		{
			List<string> objectList = CsvHelper.GetHeaderAndLineFromObject(record, ", ");
			string message = objectList[0] + "\r\n" + objectList[1];
			return message;
		}
	}
}
