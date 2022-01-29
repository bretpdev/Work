using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;


namespace NDNUMINVAL
{
    public class OneLinkInvalidator :_BaseInvalidator
    {
        static readonly object locker = new object();
        /// <summary>
        /// Takes an account Identifier (ssn or ref#) and a phone number to invalidate to the oneLink system.  If the phone number is found and invalidated, returns true
        /// </summary>
        /// <param name="record">NobleData record containing phone number and account</param>
        /// <returns>true if the phone number is found and changed to invalid</returns>
        public override bool InvalidatePhoneNumber(NobleData record)
        {
            bool changesMade = false;
            //Reference identifier
            try
            {
                List<string> searchResults = new List<string>();
                if (record.AccountIdentifier.IsNullOrEmpty())
                {
                    searchResults.AddRange(ByPhoneLookupBorrower(record));
                    searchResults.AddRange(ByPhoneLookupReference(record));
                    //Invalidate all if we find more that 1 result and it is a Fax or Disconnect or a single result for Wrong Number
                    if ((searchResults.Any() && record.Disposition.IsIn(Disposition.Fax, Disposition.Invalid, Disposition.NotInService)) || (searchResults.Count == 1 && record.Disposition == Disposition.WrongNumber)) //Wrong Number is not handled if it matches more than 1 account
                    {
                        foreach (string identifier in searchResults)
                        {
                            record.AccountIdentifier = identifier;
                            if (record.AccountIdentifier.StartsWith("RF"))
                                changesMade = OneLinkReferenceProcessor(record, changesMade);
                            else if (record.AccountIdentifier.IsNumeric()) //SSN
                                changesMade = OneLinkBorrowerProcessor(record, changesMade);
                            else
                                PLR.AddNotification(string.Format("Unable to invalidate the phone number for account number{0}.\r\n {1}", record.AccountIdentifier, GetErrorMessageFromObject(record)), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        }
                    }
                }
                else if (record.AccountIdentifier.StartsWith("RF"))
                    changesMade = OneLinkReferenceProcessor(record, changesMade);
                else if (record.AccountIdentifier.IsNumeric()) //SSN
                    changesMade = OneLinkBorrowerProcessor(record, changesMade);
            }
            catch (Exception ex)
            {
                PLR.AddNotification(string.Format("Unable to invalidate the phone number.\r\n {0}. Exception: {1}", GetErrorMessageFromObject(record), ex.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
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
                for (int row = 6; !RI.CheckForText(22, 3, "46004"); row += 3)
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
            RI.FastPath("LP22C;;;;;;;" + record.PhoneNumber);
            //found record.  add to list search results
            if (RI.CheckForText(1, 62, "PERSON DEMOGRAPHICS"))//1 record
                searchResults.Add(RI.GetText(1, 9, 9));
            else if (RI.CheckForText(1, 65, "PERSON SELECTION"))
            {
                for (int row = 5; !RI.CheckForText(22, 3, "46004"); row += 3)
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
                PLR.AddNotification(string.Format("No borrower found having phone number: {0} Message: {1}", record.PhoneNumber, GetErrorMessageFromObject(record)), NotificationType.ErrorReport, NotificationSeverityType.Informational);
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
            if (record.AccountIdentifier.Length >= 10)
                record.AccountIdentifier = RI.GetDemographicsFromLP22(record.AccountIdentifier).Ssn;

            //Check borrower phone number
            RI.FastPath("LP22C" + record.AccountIdentifier);
            if (RI.CheckForText(22, 3, "47004"))
            {
                PLR.AddNotification(string.Format("Attempted to invalidate borrower {0} but borrower did not exist in the session for object {1}", record.AccountIdentifier, GetErrorMessageFromObject(record)), NotificationType.ErrorReport, NotificationSeverityType.Informational);
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
            if (record.Disposition.IsIn(Disposition.Invalid, Disposition.NotInService))
            {
                RI.PutText(3, 9, "F");
                comment = string.Format("{0} shows disconnected and invalidated by dialer", record.PhoneNumber);
            }
            else if (record.Disposition == Disposition.Fax)
            {
                RI.PutText(3, 9, "H");
                comment = string.Format("{0} invalidated as fax machine by dialer", record.PhoneNumber);
            }
            else if (record.Disposition == Disposition.WrongNumber)
            {
                RI.PutText(3, 9, "H");
                string effectiveDate = record.EffectiveDate.HasValue ? record.EffectiveDate.Value.ToString("M/d/yyyy H:mm") : "";
                comment = $"{record.PhoneNumber} dispositioned as wrong number by {record.AgentId} on {effectiveDate}";
            }
            else
            {
                PLR.AddNotification($"Expected disposition of { Disposition.Invalid}, { Disposition.NotInService}, { Disposition.Fax}, or { Disposition.WrongNumber} and received {record.Disposition} for account {record.AccountIdentifier}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
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
                PLR.AddNotification(string.Format("Attempted to invalidate reference {0} but reference did not exist for object {1}", record.AccountIdentifier, GetErrorMessageFromObject(record)), NotificationType.ErrorReport, NotificationSeverityType.Informational);
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
            if (record.Disposition.IsIn(Disposition.Invalid, Disposition.NotInService))
            {
                RI.PutText(5, 9, "O");
                comment = $"{record.PhoneNumber} shows disconnected and invalidated by dialer";
            }
            else if (record.Disposition == Disposition.Fax)//Fax/Dialup 
            {
                RI.PutText(5, 9, "O");
                comment = $"{record.PhoneNumber} invalidated as fax machine by dialer";
            }
            else if (record.Disposition == Disposition.WrongNumber)
            {
                RI.PutText(5, 9, "O");
                string effectiveDate = record.EffectiveDate.HasValue ? record.EffectiveDate.Value.ToString("M/d/yyyy H:mm") : "";
                comment = $"{record.PhoneNumber} dispositioned as wrong number by {record.AgentId} on {effectiveDate}";
            }
            else
            {
                PLR.AddNotification($"Expected disposition of {Disposition.Invalid}, {Disposition.NotInService}, {Disposition.Fax}, or {Disposition.WrongNumber} and received {record.Disposition} for account {record.AccountIdentifier}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
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

