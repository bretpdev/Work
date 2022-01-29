using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using System.Reflection;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace NDNUMINVAL
{
    public class CompassInvalidator : _BaseInvalidator
    {
        private string BorrowerSSN { get; set; }
        private string ArcToLeave { get { return "BADPH"; } }
        private string ScriptId { get { return "NDNUMINVAL"; } }

        /// <summary>
        /// Takes a NobleData record and checks the number against the green screen.
        /// </summary>
        /// <param name="record">NobleData record from query</param>
        /// <returns>True if successful.</returns>
        public override bool InvalidatePhoneNumber(NobleData record)
        {
            List<string> searchResults = new List<string>();
            List<string> phoneTypes = new List<string>() { "H", "W", "M", "A" };
            if (record.AccountIdentifier == null)
            {
                searchResults.AddRange(CheckPhoneList(record, phoneTypes));
                if ((searchResults.Any() && record.Disposition.IsIn(Disposition.Fax, Disposition.Invalid, Disposition.NotInService)) || (searchResults.Count == 1 && record.Disposition == Disposition.WrongNumber)) //Wrong Number is not handled if it matches more than 1 account
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
                ProcessLogger.AddNotification(PLR.ProcessLogId, string.Format("Attempted to invalidate borrower {0} but borrower did not exist in the session for object {1}", record.AccountIdentifier, GetErrorMessageFromObject(record)), NotificationType.ErrorReport, NotificationSeverityType.Informational);
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
                PLR.AddNotification(string.Format("Expected screen TXX1R-01 -02 or -03 but entered screen {0} instead for account {1}.  Login: {2}", RI.GetText(1, 71, 8), record.AccountIdentifier, UserName), NotificationType.ErrorReport, NotificationSeverityType.Warning);
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
                ssn = DA.GetSSNFromAccountNumber(record.AccountIdentifier);
            else
                ssn = record.AccountIdentifier;
            //Normal Borrower Note
            if (HomeChanged || AltChanged || WorkChanged || MobileChanged)
            {
                if (borrowerSSN.IsNullOrEmpty() || regardsTo == "B") //if this is null, then it is the borrower that needs the note
                {
                    ArcData arcToAdd = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = record.AccountIdentifier,
                        Arc = ArcToLeave,
                        ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                        Comment = comment,
                        IsEndorser = false,
                        IsReference = false,
                        LoanPrograms = null,
                        NeedBy = null,
                        ProcessFrom = null,
                        ProcessOn = null,
                        ProcessTo = null,
                        RecipientId = null,
                        RegardsCode = null,
                        RegardsTo = null,
                        ScriptId = ScriptId
                    };
                    ArcAddResults result = arcToAdd.AddArc();
                    if (!result.ArcAdded)
                        PLR.AddNotification(string.Format("Failed to add Arc into arc add database for borrower: {0} ARC: {1} comment: {2} Login: {3}", record.AccountIdentifier, "BADPH", comment, UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical, result.Ex);
                }
                else //Reference or Co-Maker Note
                {
                    if (regardsTo == "E")
                    {
                        ArcData arcToAdd = new ArcData(DataAccessHelper.CurrentRegion)
                        {
                            AccountNumber = borrowerSSN,
                            Arc = ArcToLeave,
                            ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                            Comment = comment,
                            IsEndorser = true,
                            IsReference = false,
                            LoanPrograms = null,
                            NeedBy = null,
                            ProcessFrom = null,
                            ProcessOn = null,
                            ProcessTo = null,
                            RecipientId = null,
                            RegardsCode = "E",
                            RegardsTo = ssn,
                            ScriptId = ScriptId
                        };
                        ArcAddResults result = arcToAdd.AddArc();
                        if (!result.ArcAdded)
                            PLR.AddNotification(string.Format("Failed to add Arc into arc add database for Endorser: {0} ARC: {1} comment: {2} Login: {3}", record.AccountIdentifier, "BADPH", comment, UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical, result.Ex);
                    }
                    else if (regardsTo == "R")
                    {
                        ArcData arcToAdd = new ArcData(DataAccessHelper.CurrentRegion)
                        {
                            AccountNumber = borrowerSSN,
                            Arc = ArcToLeave,
                            ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                            Comment = comment,
                            IsEndorser = false,
                            IsReference = true,
                            LoanPrograms = null,
                            NeedBy = null,
                            ProcessFrom = null,
                            ProcessOn = null,
                            ProcessTo = null,
                            RecipientId = null,
                            RegardsCode = null,
                            RegardsTo = null,
                            ScriptId = ScriptId
                        };
                        ArcAddResults result = arcToAdd.AddArc();
                        if (!result.ArcAdded)
                            PLR.AddNotification(string.Format("Failed to add Arc into arc add database for Reference: {0} ARC: {1} comment: {2} Login: {3}", record.AccountIdentifier, "BADPH", comment, UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical, result.Ex);
                    }
                    else
                        PLR.AddNotification(string.Format("Expected a Reference or Endorser code but instead found '{0}' for borrower '{1}'", regardsTo, borrowerSSN), NotificationType.ErrorReport, NotificationSeverityType.Warning);
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
            if (record.Disposition == Disposition.Invalid || record.Disposition == Disposition.NotInService)
                comment = string.Format("{0} shows disconnected and invalidated by dialer", record.PhoneNumber);
            else if (record.Disposition == Disposition.Fax)
                comment = string.Format("{0} invalidated as fax machine by dialer", record.PhoneNumber);
            else if (record.Disposition == Disposition.WrongNumber)
            {
                string effectiveDate = record.EffectiveDate.HasValue ? record.EffectiveDate.Value.ToString("M/d/yyyy H:mm") : "";
                comment = $"{record.PhoneNumber} dispositioned as wrong number by {record.AgentId} on {effectiveDate}";
            }
            else
            {
                ProcessLogger.AddNotification(PLR.ProcessLogId, $"Expected disposition of { Disposition.Invalid}, { Disposition.NotInService}, { Disposition.Fax}, or { Disposition.WrongNumber} and received {record.Disposition} for account {record.AccountIdentifier}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
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
                if (RI.CheckForText(23, 2, "01097"))
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
