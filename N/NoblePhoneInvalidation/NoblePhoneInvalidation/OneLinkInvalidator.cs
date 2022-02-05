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
	public class OneLinkInvalidator
	{
		public ReflectionInterface RI { get; set; }
		public ProcessLogData LogData { get; set; }
		public BatchProcessingHelper Batch { get; set; }
		public ErrorReport ErrorReportOneLink { get; set; }
		static readonly object locker = new object();
		public OneLinkInvalidator(ProcessLogData logData)
		{
			LogData = logData;
			lock (locker)
			{
				DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
				ErrorReportOneLink = new ErrorReport("OneLink Disposition Error", "ERR_BU19");
			}
		}

		/// <summary>
		/// Load a Reflection Interface.
		/// </summary>
		/// <param name="batch">BatchProcessingHelper</param>
		/// <returns>true if successful</returns>
		public bool Login(BatchProcessingHelper batch)
		{
			DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
			Batch = batch;
			RI = new ReflectionInterface();
			Console.WriteLine("Created OneLink Session. Attempting to login.");
			return RI.Login(batch.UserName, batch.Password);
		}

		/// <summary>
		/// Takes an account Identifier (ssn or ref#) and a phone number to invalidate to the oneLink system.  If the phone number is found and invalidated, returns true
		/// </summary>
		/// <param name="record">NobleData record containing phone number and account</param>
		/// <returns>true if the phone number is found and changed to invalid</returns>
		public bool InvalidatePhoneNumber(NobleData record)
		{
			bool changesMade = false;
			//Reference identifier
			try
			{
				List<string> searchResults = new List<string>();
				if (record.AccountIdentifier == null)
				{
					searchResults.AddRange(ByPhoneLookupBorrower(record));
					searchResults.AddRange(ByPhoneLookupReference(record));
					//Invalidate all if we find more that 1 result and it is a Fax or Disconnect or a single result for WN
					if ((searchResults.Any() && record.Disposition.IsIn("FX", "D")) || (searchResults.Count == 1 && record.Disposition == "WN")) //WN is not handled if it matches more than 1 account
					{
						foreach (string identifier in searchResults)
						{
							record.AccountIdentifier = identifier;
							if (record.AccountIdentifier.StartsWith("RF"))
								changesMade = OneLinkReferenceProcessor(record, changesMade);
							else if (record.AccountIdentifier.IsNumeric()) //SSN
								changesMade = OneLinkBorrowerProcessor(record, changesMade);
							else
								ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Unable to invalidate the phone number for account number{0}.\r\n {1}", record.AccountIdentifier, GetErrorMessageFromObject(record)), NotificationType.ErrorReport, NotificationSeverityType.Critical);
						}
					}
				}
				else if (record.AccountIdentifier.StartsWith("RF"))
					changesMade = OneLinkReferenceProcessor(record, changesMade);
				else if (record.AccountIdentifier.IsNumeric()) //SSN
					changesMade = OneLinkBorrowerProcessor(record, changesMade);
				else
					ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Unexpected format for account number: {0}.\r\n {1}", record.AccountIdentifier, GetErrorMessageFromObject(record)), NotificationType.ErrorReport, NotificationSeverityType.Critical);
			}
			catch (Exception ex)
			{
				ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Unable to invalidate the phone number.\r\n {0}. Exception: {1}", GetErrorMessageFromObject(record), ex.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
				lock (locker)
				{
					ErrorReportOneLink.AddRecord("Attempt to invalidate borrower phone number failed.", record);
				}
			}
			return changesMade;
		}

		/// <summary>
		/// Add records to searchResults for references with matching phone number
		/// </summary>
		/// <param name="record">Passing the phone number</param>
		/// <returns>List of strings</returns>
		private List<string> ByPhoneLookupReference(NobleData record)
		{
			List<string> searchResults = new List<string>();
			RI.FastPath("LP2CC*");
			RI.PutText(6, 45, "", true);
			RI.PutText(16, 45, record.PhoneNumber, Key.Enter, true);
			//found record. add to list search results
			if (RI.CheckForText(1, 59, "REFERENCE DEMOGRAPHICS"))//1 record
				searchResults.Add(RI.GetText(3, 14, 9));
			else if (RI.CheckForText(1, 65, "REFERENCE SELECT"))
			{
				for(int row = 6; !RI.CheckForText(22, 3, "46004"); row += 3)
				{
					if (row > 18 || !RI.CheckForText(row, 3, "  "))
					{
						RI.Hit(Key.F8); //Page to next
						row = 6;
					}
					else
					{
						int? selection = RI.GetText(row, 3, 2).ToIntNullable();
						if (selection == null)
							break;
						RI.PutText(21, 13, selection.ToString(), Key.Enter, true);
						searchResults.Add(RI.GetText(3, 14, 9));
						RI.Hit(Key.F12);
					}
				}
			}
			else
				ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Phone number {0} not found in session for account {1}. Screen: {2} Message: {3} Login: {4}", record.PhoneNumber, record.AccountIdentifier, RI.GetText(1, 59, 22), GetErrorMessageFromObject(record), Batch.UserName), NotificationType.ErrorReport, NotificationSeverityType.Informational);
			return searchResults;
		}

		/// <summary>
		/// Add records to searchResults for borrowers with matching phone number
		/// </summary>
		/// <param name="record">Passing the phone number</param>
		/// <returns>list of results</returns>
		private List<string> ByPhoneLookupBorrower(NobleData record)
		{
			List<string> searchResults = new List<string>();
			RI.FastPath("LP22C;;;;;;;"+ record.PhoneNumber);
			//found record.  add to list search results
			if (RI.CheckForText(1, 62, "PERSON DEMOGRAPHICS"))//1 record
				searchResults.Add(RI.GetText(1, 9, 9));
			else if (RI.CheckForText(1, 65, "PERSON SELECTION"))
			{
				for(int row = 5; !RI.CheckForText(22, 3, "46004"); row+=3)
				{
					if (row > 17 || !RI.CheckForText(row, 3, "  "))
					{
						RI.Hit(Key.F8); //Page to next
						row = 5;
					}
					else
					{
						int? selection = RI.GetText(row, 3, 2).ToIntNullable();
						if (selection == null)
							break;
						RI.PutText(21, 13, selection.ToString(), Key.Enter, true);
						searchResults.Add(RI.GetText(1, 9, 9));
						RI.Hit(Key.F12);
					}
				}
			}
			else
				ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Expected screen not reached for account {0}. Screen: {1} Message: {2} Login: {3}", record.AccountIdentifier, RI.GetText(1, 62, 19), GetErrorMessageFromObject(record), Batch.UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical);
			return searchResults;
		}

		/// <summary>
		/// The accountIdentifier designates a borrower that needs to have its phone numbers checked.  If the phone number passed in is found on the account, it will be set to invalid.
		/// </summary>
		/// <param name="record">Phone number to search for and Account number</param>
		/// <param name="changesMade">Passed in as false.  If the phonenumber validity flag is changed to N this will be returned as true.</param>
		/// <returns>bool   True if a phone number is updated.</returns>
		private bool OneLinkBorrowerProcessor(NobleData record, bool changesMade)
		{
			//Check borrower phone number
			RI.FastPath("LP22C" + record.AccountIdentifier);
			if (RI.CheckForText(22, 3, "47004"))
			{
				ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Attempted to invalidate borrower {0} but borrower did not exist for object {1}", record.AccountIdentifier, GetErrorMessageFromObject(record)), NotificationType.ErrorReport, NotificationSeverityType.Warning);
				lock (locker)
				{
					ErrorReportOneLink.AddRecord("Attempt to invalidate borrower phone number failed. Borrower not found.", record);
				}
				return false;
			}
			changesMade = SetInvalidateFlagBorrower(record, changesMade);

			if (changesMade)
			{
				AddInvalidationCommentBorrower(record);
			}
			return changesMade;
		}

		/// <summary>
		/// Creates a comment based on the record disposition and adds that comment to the account
		/// </summary>
		/// <param name="record">NobleData object</param>
		/// <returns>true if comment is left successfully</returns>
		private bool AddInvalidationCommentBorrower(NobleData record)
		{
			string comment = string.Empty;
			if (record.Disposition == "D")
			{
				RI.PutText(3, 9, "F");
				comment = string.Format("{0} shows disconnected and invalidated by dialer", record.PhoneNumber);
			}
			else if (record.Disposition == "FX")
			{
				RI.PutText(3, 9, "H");
				comment = string.Format("{0} invalidated as fax machine by dialer", record.PhoneNumber);
			}
			else if (record.Disposition == "WN")
			{
				RI.PutText(3, 9, "H");
				comment = string.Format("{0} dispositioned as wrong number by {1} on {2} at {3}", record.PhoneNumber, record.AgentId, record.EffectiveDate, record.EffectiveTime);
			}
			else
			{
				ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Expected disposition of 'D', 'FX', or 'WN' and received {0} for account {1}", record.Disposition, record.AccountIdentifier), NotificationType.ErrorReport, NotificationSeverityType.Warning);
				lock (locker)
				{
					ErrorReportOneLink.AddRecord("Attempt to invalidate borrower phone number failed. Invalid disposition type received.", record);
				}
			}
			RI.Hit(Key.Enter);//Posting source for the disposition code
			RI.Hit(Key.F6);
			//Leave borrower note
			return RI.AddCommentInLP50(record.AccountIdentifier, "AM", "10", "DNOBL", comment, "NDNUMINVAL");
		}

		/// <summary>
		/// Checks the record phonenumber with the reflection interface and changes valid flag to N if they match
		/// </summary>
		/// <param name="record">NobleData object</param>
		/// <param name="changesMade">Set the changes made flag if invalidation occurs</param>
		/// <returns>True if a phone number is invalidated</returns>
		private bool SetInvalidateFlagBorrower(NobleData record, bool changesMade)
		{
			for (int row = 13; row <= 15; row++)
			{
				if (RI.CheckForText(row, 12, record.PhoneNumber) && RI.CheckForText(row, 38, "Y"))
				{
					RI.PutText(row, 38, "N");
					changesMade = true;
				}
			}
			return changesMade;
		}

		/// <summary>
		/// The accountIdentifier designates a reference that needs to have its phone numbers checked.  If the phone number passed in is found on the account, it will be set to invalid.
		/// </summary>
		/// <param name="record">Phone number to search for and Account number</param>
		/// <param name="changesMade">Passed in as false.  If the phonenumber validity flag is changed to N this will be returned as true.</param>
		/// <returns>bool   True if a phone number is updated.</returns>
		private bool OneLinkReferenceProcessor(NobleData record, bool changesMade)
		{
			RI.FastPath("LP2CC;" + record.AccountIdentifier);
			if (RI.CheckForText(22, 3, "47004"))
			{
				ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Attempted to invalidate reference {0} but reference did not exist for object {1}", record.AccountIdentifier, GetErrorMessageFromObject(record)), NotificationType.ErrorReport, NotificationSeverityType.Warning);
				lock (locker)
				{
					ErrorReportOneLink.AddRecord("Attempt to invalidate reference phone number failed. Reference not found.", record);
				}
				return false;
			}
			//Phones match and phone is currently valid.  Needs to be invalidated
			changesMade = SetInvalidateFlagReference(record, changesMade);
			if (changesMade)
			{
				AddInvalidationCommentReference(record);
			}
			return changesMade;
		}

		/// <summary>
		/// Creates a comment based on the record disposition and adds that comment to the account
		/// </summary>
		/// <param name="record">NobleData object</param>
		/// <returns>true if comment is left successfully</returns>
		private bool AddInvalidationCommentReference(NobleData record)
		{
			string comment = string.Empty;
			if (record.Disposition == "D")
			{
				RI.PutText(5, 9, "O");
				comment = string.Format("{0} shows disconnected and invalidated by dialer", record.PhoneNumber);
			}
			else if (record.Disposition == "FX")//FX and WN
			{
				RI.PutText(5, 9, "O");
				comment = string.Format("{0} invalidated as fax machine by dialer", record.PhoneNumber);
			}
			else if (record.Disposition == "WN")
			{
				RI.PutText(5, 9, "O");
				comment = string.Format("{0} dispositioned as wrong number by {1} on {2} at {3}", record.PhoneNumber, record.AgentId, record.EffectiveDate, record.EffectiveTime);
			}
			else
			{
				ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Expected disposition of 'D', 'FX', or 'WN' and received {0} for account {1}", record.Disposition, record.AccountIdentifier), NotificationType.ErrorReport, NotificationSeverityType.Warning);
				lock (locker)
				{
					ErrorReportOneLink.AddRecord("Attempt to invalidate borrower phone number failed. Invalid disposition type received.", record);
				}
				return false;
			}
			RI.Hit(Key.Enter);//Posting the source for the disposition code
			RI.Hit(Key.F6);
			return RI.AddCommentInLP50(record.AccountIdentifier, "AM", "10", "DNOBL", comment, "NDNUMINVAL");
		}

		/// <summary>
		/// Checks the record phonenumber with the reflection interface and changes valid flag to N if they match
		/// </summary>
		/// <param name="record">NobleData object</param>
		/// <param name="changesMade">Set the changes made flag if invalidation occurs</param>
		/// <returns>True if a phone number is invalidated</returns>
		private bool SetInvalidateFlagReference(NobleData record, bool changesMade)
		{
			for (int row = 13; row <= 14; row++)
			{
				if (RI.CheckForText(row, 16, record.PhoneNumber) && RI.CheckForText(row, 36, "Y"))
				{
					RI.PutText(row, 36, "N");
					changesMade = true;
				}
			}
			return changesMade;
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

