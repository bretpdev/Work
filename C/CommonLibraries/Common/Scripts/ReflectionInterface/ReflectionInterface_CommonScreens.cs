using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.Scripts
{
    public partial class ReflectionInterface
    {
        private List<ArcSeverityTracking> arcErrorInfo;
        public List<ArcSeverityTracking> ArcErrorInfo
        {
            get
            {
                if (arcErrorInfo == null)
                    arcErrorInfo = DataAccessHelper.ExecuteList<ArcSeverityTracking>("arcaddproc.GetArcSeverityInfo", DataAccessHelper.Database.CentralData);

                return arcErrorInfo;
            }
        }
        private bool AccessTd22(string ssnOrAccountNum, string arc)
        {
            FastPath(string.Format("TX3Z/ATD22{0};{1}", ssnOrAccountNum, arc));
            return CheckForText(1, 72, "TDX24");
        }

        private void EnterRecipientId(string recipientId)
        {
            PutText(6, 32, recipientId);
        }

        private bool SelectAllLoans(bool checkAbends)
        {
            for (int row = 11; !CheckForText(23, 2, "90007"); row++)
            {
                if (row > 18)
                {
                    Hit(ReflectionInterface.Key.F8);
                    if (checkAbends && CheckScreenAbend())
                        return false;
                    if (GetText(23, 2, 5).IsIn(ArcErrorInfo.Select(p => p.ErrorCode).ToArray()))
                        return false;
                    else
                        row = 10;
                }

                if (CheckForText(row, 3, "_"))
                {
                    PutText(row, 3, "X");
                }
            }
            return true;
        }

        private void FillInFromTo(DateTime from, DateTime to)
        {
            PutText(5, 54, from.ToString("MMddyy"));
            PutText(5, 67, to.ToString("MMddyy"));
        }

        private void FillInNeededBy(DateTime neededBy)
        {
            PutText(6, 13, neededBy.ToString("MMddyy"));
        }

        private void FillInRegards(string code, string id)
        {
            PutText(7, 19, code);
            PutText(7, 36, id);
        }

        private bool SelectLoansByProgram(bool checkAbends, params string[] loanPrograms)
        {
            for (int row = 11; !CheckForText(23, 2, "90007"); row++)
            {
                if (row > 18)
                {
                    Hit(ReflectionInterface.Key.F8);
                    if (checkAbends && CheckScreenAbend())
                        return false;
                    if (GetText(23, 2, 5).IsIn(ArcErrorInfo.Select(p => p.ErrorCode).ToArray()))
                        return false;
                    row = 10;
                    continue;
                }
                if (CheckForText(row, 5, "  "))
                    break;
                else if (loanPrograms.Contains(GetText(row, 61, 6)))
                    PutText(row, 3, "X");
            }

            return true;
        }


        public string CloseCompassQueue(string queue, string status, string actionResponse)
        {
            FastPath("TX3Z/ITX6X" + queue);
            bool selectedTask = false;
            for (int row = 8; MessageCode != "90007"; row += 3)
            {
                if (CheckForText(row, 4, " ") || row > 20)
                {
                    row = 5;
                    Hit(Key.F8);
                    continue;
                }
                if (CheckForText(row, 75, "W"))
                {
                    PutText(21, 18, GetText(row, 4, 1), ReflectionInterface.Key.F2);
                    selectedTask = true;
                    break;
                }
            }

            if (!selectedTask)
                return string.Empty;

            PutText(8, 19, status);
            PutText(9, 19, actionResponse, ReflectionInterface.Key.Enter);

            return Message;
        }

        public bool Atd22ByLoanProgram(string acctNumOrSsn, string arc, string comment, string recipientId, string scriptId, bool pauseForManualComments, bool checkAbends, params string[] loanPrograms)

        {
            if (!AccessTd22(acctNumOrSsn, arc))
                return false;

            EnterRecipientId(recipientId);
            if (!SelectLoansByProgram(checkAbends, loanPrograms))
                return false;

            return EnterComment(comment, scriptId, pauseForManualComments, checkAbends);
        }

        public bool Atd22ByLoanProgram(string acctNumOrSsn, string arc, string comment, string recipientId, string scriptId, bool pauseForManualComments, params string[] loanPrograms)
        {
            return Atd22ByLoanProgram(acctNumOrSsn, arc, comment, recipientId, scriptId, pauseForManualComments, false, loanPrograms);
        }

        public bool Atd22AllLoans(string ssnOrAccountNum, string arc, string comment, string recipientId, string scriptId, bool pauseForManualComments, string regardsToCode = "", string regardsToId = "", DateTime? from = null, DateTime? to = null, DateTime? neededBy = null, bool checkAbends = false)
        {
            if (!AccessTd22(ssnOrAccountNum, arc))
                return false;

            if (from.HasValue && to.HasValue)
                FillInFromTo(from.Value, to.Value);

            if (neededBy.HasValue)
                FillInNeededBy(neededBy.Value);

            if (regardsToCode.IsPopulated() && regardsToId.IsPopulated())
                FillInRegards(regardsToCode, regardsToId);

            EnterRecipientId(recipientId);
            if (!SelectAllLoans(checkAbends))
                return false;

            return EnterComment(comment, scriptId, pauseForManualComments, checkAbends);
        }

        public bool Atd22AllLoans(string ssnOrAccountNum, string arc, string comment, string recipientId, string scriptId, bool pauseForManualComments, string regardsToCode = "", string regardsToId = "", DateTime? from = null, DateTime? to = null, DateTime? neededBy = null)
        {
            return Atd22AllLoans(ssnOrAccountNum, arc, comment, recipientId, scriptId, pauseForManualComments, regardsToCode, regardsToId, from, to, neededBy, false);

        }

        public bool Atd22AllLoans(string ssnOrAccountNum, string arc, string comment, string recipientId, string scriptId, bool pauseForManualComments)
        {
            return Atd22AllLoans(ssnOrAccountNum, arc, comment, recipientId, scriptId, pauseForManualComments, null, null, null, null, null);
        }

        public bool Atd22ByBalance(string ssnOrAccountNum, string arc, string comment, string recipientId, string scriptId, bool pauseForManualComments, bool reference, bool endorser, bool checkAbends)
        {
            if (!AccessTd22(ssnOrAccountNum, arc))
                return false;

            if (reference)
            {
                PutText(6, 32, GetText(4, 16, 11).Replace(" ", ""));
                PutText(7, 19, "R");
                PutText(7, 36, recipientId);
            }
            else if (endorser)
            {
                PutText(6, 32, GetText(4, 16, 11).Replace(" ", ""));
                PutText(7, 19, "E");
                PutText(7, 36, recipientId);
            }
            else
                PutText(6, 32, recipientId);

            for (int row = 11; !CheckForText(23, 2, "90007"); row++)
            {
                if (row > 18)
                {
                    Hit(ReflectionInterface.Key.F8);
                    if (checkAbends && CheckScreenAbend())
                        return false;
                    if (GetText(23, 2, 5).IsIn(ArcErrorInfo.Select(p => p.ErrorCode).ToArray()))
                        return false;
                    row = 10;
                    continue;
                }
                if (CheckForText(row, 5, "  "))
                    break;
                else if (double.Parse(GetText(row, 68, 10).Replace(",", "")) > 0.00 && GetText(row, 78, 2).IsNullOrEmpty())
                    PutText(row, 3, "X");
            }

            return EnterComment(comment, scriptId, pauseForManualComments, checkAbends);
        }

        public bool Atd22ByBalance(string ssnOrAccountNum, string arc, string comment, string recipientId, string scriptId, bool pauseForManualComments, bool reference, bool endorser)
        {
            return Atd22ByBalance(ssnOrAccountNum, arc, comment, recipientId, scriptId, pauseForManualComments, reference, endorser, false);
        }

        public bool Atd22ByBalance(string ssnOrAccountNum, string arc, string comment, string recipientId, string scriptId, bool pauseForManualComments, bool reference)
        {
            return Atd22ByBalance(ssnOrAccountNum, arc, comment, recipientId, scriptId, pauseForManualComments, reference, false);
        }

        public bool Atd22ByLoan(string ssnOrAccountNum, string arc, string comment, string recipientId, List<int> loanSequenceNumbers, string scriptId, bool pauseForManualComments, string regardsToCode = "", string regardsToId = "", DateTime? from = null, DateTime? to = null, DateTime? neededBy = null, bool checkAbends = false)
        {
            if (!AccessTd22(ssnOrAccountNum, arc))
                return false;

            if (from.HasValue && to.HasValue)
                FillInFromTo(from.Value, to.Value);

            if (neededBy.HasValue)
                FillInNeededBy(neededBy.Value);

            if (!regardsToCode.IsNullOrEmpty() && !regardsToId.IsNullOrEmpty())
                FillInRegards(regardsToCode, regardsToId);

            EnterRecipientId(recipientId);

            if (!SelectLoansBySequence(loanSequenceNumbers, checkAbends))
                return false;

            return EnterComment(comment, scriptId, pauseForManualComments, checkAbends);
        }

        public bool Atd22ByLoan(string ssnOrAccountNum, string arc, string comment, string recipientId, List<int> loanSequenceNumbers, string scriptId, bool pauseForManualComments, string regardsToCode = "", string regardsToId = "", DateTime? from = null, DateTime? to = null, DateTime? neededBy = null)
        {
            return Atd22ByLoan(ssnOrAccountNum, arc, comment, recipientId, loanSequenceNumbers, scriptId, pauseForManualComments, regardsToCode, regardsToId, from, to, neededBy, false);
        }

        public bool Atd22ByLoan(string ssnOrAccountNum, string arc, string comment, string recipientId, List<int> loanSequenceNumbers, string scriptId, bool pauseForManualComments)
        {
            return Atd22ByLoan(ssnOrAccountNum, arc, comment, recipientId, loanSequenceNumbers, scriptId, pauseForManualComments, null, null, null, null, null);
        }

        private bool SelectLoansBySequence(List<int> loanSequenceNumbers, bool checkAbends)
        {
            int nextPageCounter = 0;

            foreach (int loan in loanSequenceNumbers)
            {
                string seq = loan.ToString().Length > 1 ? loan.ToString().Insert(0, "0") : loan.ToString().Insert(0, "00");
                Coordinate location = new Coordinate();
                while (MessageCode != "90007")
                {
                    for (int row = 11; row < 19; row++)
                    {
                        if (CheckForText(row, 5, seq))
                        {
                            location.Row = row;
                            location.Column = 3;
                            break;
                        }
                    }
                    if (location.Row == 0)
                    {
                        Hit(ReflectionInterface.Key.F8);
                        if (checkAbends && CheckScreenAbend())
                            return false;
                        if (GetText(23, 2, 5).IsIn(ArcErrorInfo.Select(p => p.ErrorCode).ToArray()))
                            return false;
                        nextPageCounter++;
                    }
                    else
                    {
                        PutText(location.Row, location.Column, "X");
                        break;
                    }
                }
                //Hit F7 to go back to the first page
                for (int i = 0; i < nextPageCounter; i++)
                {
                    Hit(ReflectionInterface.Key.F7);
                    Hit(Key.F5);
                }

                nextPageCounter = 0;
            }

            return true;
        }

        public string GetAccountNumberFromSsn(string ssn)
        {
            FastPath("TX3Z/ITX1J;" + ssn);
            if (!CheckForText(1, 71, "TXX1R"))
                return null;
            return GetText(3, 34, 12).Replace(" ", "");
        }

        /// <summary>
        /// Gets demographic information from screen TX1J.
        /// </summary>
        /// <param name="ssnOrAcctNum">The SSN or Account Number.</param>
        /// <returns>A populated SystemBorrowerDemograhics object.</returns>
        public SystemBorrowerDemographics GetDemographicsFromTx1j(string ssnOrAcctNum)
        {
            if (ssnOrAcctNum.Length == 9)
            {
                FastPath("TX3Z/ITX1J;" + ssnOrAcctNum);
            }
            else
            {
                FastPath("TX3Z/ITX1J;");
                PutText(6, 61, ssnOrAcctNum, ReflectionInterface.Key.Enter);
            }

            if (!CheckForText(1, 71, "TXX1R"))
            {
                throw new DemographicException("The given SSN or Account number couldn't be found on the system.  Please contact Systems Support.");
            }

            SystemBorrowerDemographics demos = new SystemBorrowerDemographics();

            demos.AccountNumber = GetText(3, 34, 12).Replace(" ", "");
            demos.Ssn = GetText(3, 12, 11).Replace(" ", "");
            demos.FirstName = GetText(4, 34, 13);
            demos.MiddleIntial = GetText(4, 53, 13);
            demos.LastName = GetText(4, 6, 23);
            demos.DateOfBirth = GetText(20, 6, 10).Replace(" ", "/");
            demos.Address1 = GetText(11, 10, 29);
            demos.Address2 = GetText(12, 10, 29)[0] == '_' ? "" : GetText(12, 10, 29);
            demos.City = GetText(14, 8, 20);
            demos.State = GetText(14, 32, 2);
            demos.Suffix = GetText(4, 72, 4)[0] == '_' ? "" : GetText(4, 72, 4);

            if (!CheckForText(13, 52, "_"))
            {
                demos.State = GetText(12, 52, 15);
                demos.Country = GetText(13, 52, 24);
            }

            demos.ZipCode = GetText(14, 40, 17);
            if (demos.ZipCode.Length > 5)
            {
                demos.ZipCode.Insert(4, "-");
            }

            demos.IsValidAddress = CheckForText(11, 55, "Y");
            demos.AddressValidityDate = GetText(10, 32, 8).Replace(" ", "/");

            demos.PrimaryPhoneType = GetText(16, 14, 1);
            demos.PrimaryMblIndicator = GetText(16, 20, 1);
            demos.PrimaryConsentIndicator = GetText(16, 30, 1);

            demos.PhoneValidityDate = GetText(16, 45, 8).Replace(" ", "/");
            demos.IsPrimaryPhoneValid = CheckForText(17, 54, "Y");
            demos.PrimaryPhone = GetText(17, 14, 3) + GetText(17, 23, 3) + GetText(17, 31, 4);
            demos.ForeignPhone = GetText(18, 15, 3) + GetText(18, 24, 5) + GetText(18, 36, 11);

            Hit(ReflectionInterface.Key.F6);
            Hit(ReflectionInterface.Key.F6);
            Hit(ReflectionInterface.Key.F6);
            PutText(16, 14, "A", ReflectionInterface.Key.Enter);
            if (!CheckForText(23, 2, "01105"))
            {
                demos.AlternatePhoneType = GetText(16, 14, 1);
                demos.AlternateMblIndicator = GetText(16, 20, 1);
                demos.AlternateConsentIndicator = GetText(16, 30, 1);
                demos.IsAlternatePhoneValid = CheckForText(17, 54, "Y");
                demos.AlternamtePhoneValidityDate = GetText(16, 45, 8).Replace(" ", "/");
                demos.AlternatePhone = GetText(17, 14, 3) + GetText(17, 23, 3) + GetText(17, 31, 4);
            }

            Hit(ReflectionInterface.Key.F2);
            Hit(ReflectionInterface.Key.F10);

            if (!CheckForText(14, 10, "___") && CheckForText(1, 72, "TXX4V"))
            {
                demos.IsValidEmail = CheckForText(12, 14, "Y");
                demos.EmailValidityDate = GetText(11, 17, 8).Replace(" ", "/");
                demos.EmailAddress = GetText(14, 10, 59);

                for (int row = 15; !CheckForText(row, 10, "___"); row++)
                {
                    if (row > 18) { break; }
                    demos.EmailAddress += GetText(row, 10, 59);
                }

            }

            return demos;
        }

        /// <summary>
        /// Get demographics from LP22
        /// </summary>
        /// <param name="accountIdentifier">The SSN (9 chars) or Account Number (10 chars)</param>
        /// <returns>A populated SystemBorrowerDemographics object</returns>
        public SystemBorrowerDemographics GetDemographicsFromLP22(string accountIdentifier)
        {
            //access LP22
            if (accountIdentifier.Length == 10)
                FastPath("LP22I;;;;L;;" + accountIdentifier);
            else if (accountIdentifier.Length == 9)
                FastPath("LP22I" + accountIdentifier);
            else
                throw new DemographicException("A valid 10 digit Account Number or 9 digit Social Security Number wasn't provided.");

            //Check that the LP22 target screen is displayed.
            if (!CheckForText(1, 62, "PERSON DEMOGRAPHICS"))
                throw new DemographicException("A valid 10 digit Account Number or 9 digit Social Security Number wasn't provided.");

            //Define, populate, and return a BorrowerDemographics object.
            SystemBorrowerDemographics demog = new SystemBorrowerDemographics();
            demog.Ssn = GetText(3, 23, 9);
            demog.AccountNumber = GetText(3, 60, 12);
            demog.LastName = GetText(4, 5, 35);
            demog.MiddleIntial = GetText(4, 60, 1);
            demog.FirstName = GetText(4, 44, 12);
            demog.Address1 = GetText(10, 9, 35);
            demog.Address2 = GetText(11, 9, 35);
            demog.City = GetText(12, 9, 35);
            demog.State = GetText(12, 52, 2).Replace("FC", "");
            demog.Country = GetText(11, 54, 25);
            demog.ZipCode = GetText(12, 60, 5) + GetText(12, 65, 4);
            demog.IsValidAddress = GetText(10, 57, 1) == "Y";
            Func<string, string> ParseDate = (input) =>
            { //convert to mm/dd/yyyy
                return input.Substring(0, 2) + "/" + input.Substring(2, 2) + "/" + input.Substring(4, 4);
            };
            if (!CheckForText(11, 72, "MMDDCCYY"))
                demog.AddressValidityDate = ParseDate(GetText(10, 72, 8));
            if (!CheckForText(4, 72, "MMDDCCYY"))
                demog.DateOfBirth = ParseDate(GetText(4, 72, 8));

            demog.PrimaryPhone = GetText(13, 12, 10);
            demog.IsPrimaryPhoneValid = GetText(13, 38, 1) == "Y";
            demog.AlternatePhone = GetText(14, 12, 10);
            demog.IsAlternatePhoneValid = GetText(14, 38, 1) == "Y";
            if (!CheckForText(14, 44, "MMDDCCYY"))
                demog.PhoneValidityDate = ParseDate(GetText(13, 44, 8));

            demog.EmailAddress = GetText(19, 9, 56);
            demog.IsValidEmail = GetText(18, 56, 1) == "Y";
            if (!CheckForText(18, 71, "MMDDCCYY"))
                demog.EmailValidityDate = ParseDate(GetText(18, 71, 8));
            Hit(Key.F2);
            Hit(Key.F10);
            Hit(Key.F10);
            string alternateBillingName = GetText(9, 38, 40);
            if ((demog.FirstName + " " + demog.LastName) != alternateBillingName)
                demog.AKA = "AKA " + alternateBillingName;
            return demog;
        }

        private bool EnterComment(string comment, string scriptId, bool pauseForManualComment, bool checkAbends)
        {
            comment = comment ?? "";
            if (pauseForManualComment)
            {
                using (ManualComments manualCmts = new ManualComments(comment))
                {
                    if (manualCmts.ShowDialog() == DialogResult.OK)
                    {
                        comment = manualCmts.Comment;
                    }
                }
            }

            if (!scriptId.IsNullOrEmpty())
            {
                if (comment.EndsWith("."))
                    comment = comment.Substring(0, comment.Length - 1);
                comment = string.Format("{0}.  {{ {1} }}", comment, scriptId);
            }

            if (comment.Length <= 154)
            {
                PutText(21, 2, comment, ReflectionInterface.Key.Enter);
                if (checkAbends && CheckScreenAbend())
                    return false;
            }
            else
            {
                if (comment.Length > 1234)
                {
                    throw new Exception("The requested comment will not fit on TD22");
                }

                PutText(21, 2, comment.Substring(0, 154), ReflectionInterface.Key.Enter);
                if (!CheckForText(23, 2, "02860"))
                {
                    return false;
                }
                Hit(ReflectionInterface.Key.F4);

                for (int segmentStart = 154; segmentStart <= comment.Length - 1; segmentStart += 72)
                {
                    EnterText(comment.SafeSubString(segmentStart, 72));
                }

                Hit(ReflectionInterface.Key.Enter);
                if (checkAbends && CheckScreenAbend())
                    return false;
            }

            if (CheckForText(23, 2, "02860", "02114"))
            {
                return true;
            }

            return false;
        }

        public bool AddCommentInLP50(string accountIdenifier, string actionCode, string activityType, string activityContactType, string comment, string scriptId, string associatedPersonID = null, string documentID = null, DateTime? activityDateTime = null, DateTime? activityCloseDate = null, string uniqueID = null, string institutionID = null, string userID = null, DateTime? claimPackageCreateDate = null, string userIDClaimPackage = null)
        {
            FastPath("LP50A");
            Hit(ReflectionInterface.Key.EndKey);

            if (associatedPersonID != null && associatedPersonID.Length > 3 && associatedPersonID.SafeSubString(0, 3).ToUpper().Contains("RF@"))
            {
                PutText(3, 13, "", true);
                PutText(3, 48, associatedPersonID);
            }
            else
                PutText(3, 13, accountIdenifier);

            PutText(9, 20, actionCode, ReflectionInterface.Key.Enter);

            if (!CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY"))
                return false;

            PutText(7, 2, activityType);
            PutText(7, 5, activityContactType);

            if (!documentID.IsNullOrEmpty())
                PutText(7, 60, documentID);
            if (activityDateTime != null)
                PutText(7, 15, activityDateTime.Value.ToString("MMddyyyy/hh:mm"));
            if (activityCloseDate != null)
                PutText(8, 15, activityCloseDate.Value.ToString("MMddyyyy"));
            if (!uniqueID.IsNullOrEmpty())
                PutText(8, 34, uniqueID);
            if (!institutionID.IsNullOrEmpty())
                PutText(8, 60, institutionID);
            if (!userID.IsNullOrEmpty())
                PutText(8, 69, userID);
            if (claimPackageCreateDate != null)
                PutText(10, 36, claimPackageCreateDate.Value.ToString("MMddyyyy/hhmmss"), true);
            if (!userIDClaimPackage.IsNullOrEmpty())
                PutText(11, 26, userIDClaimPackage);

            if (comment.Length + scriptId.Length < 70)
            {
                comment = string.Format("{0}. {{ {1} }}", comment, scriptId);
                PutText(13, 2, comment, ReflectionInterface.Key.F6);
            }
            else
            {
                if (comment.Length + scriptId.Length > 585)
                    throw new Exception("The requested comment will not fit on LP50");

                PutText(13, 2, comment.SafeSubString(0, 75));

                for (int segmentStart = 75; segmentStart <= comment.Length - 1; segmentStart += 75)
                    EnterText(comment.SafeSubString(segmentStart, 75));

                EnterText(string.Format(" {{ {0} }}", scriptId));

                Hit(ReflectionInterface.Key.F6);
            }

            return CheckForText(22, 3, "48003") || CheckForText(22, 3, "48081"); // 48003 = Success; 48081 = Alternative success code with info attached
        }

        /// <summary>
        /// Adds a comment to LP50
        /// </summary>
        /// <param name="ssn">SSN for borrower/Reference Id for reference</param>
        /// <param name="activityType">Activity Type</param>
        /// <param name="activityContact">Contact Type</param>
        /// <param name="actionCode">Action code to use</param>
        /// <param name="comment">Comment must be less than 585 characters</param>
        /// <param name="scriptId">Script Id</param>
        /// <returns></returns>
        public bool AddCommentInLP50(string ssn, string activityType, string activityContact, string actionCode, string comment, string scriptId)
        {
            if (ssn.IsNullOrEmpty())
                return false;
            FastPath("LP50A");
            Hit(ReflectionInterface.Key.EndKey);
            PutText(3, 48, "", true);
            if (ssn.SafeSubString(0, 3).ToUpper().Contains("RF@"))
            {
                PutText(3, 13, "", true);
                PutText(3, 48, ssn);
            }
            else
                PutText(3, 13, ssn);

            PutText(9, 20, actionCode, ReflectionInterface.Key.Enter);

            if (!CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY"))
                return false;

            PutText(7, 2, activityType);
            PutText(7, 5, activityContact);

            comment = comment ?? "";
            if (comment.Length + scriptId.Length < 70)
            {
                comment = string.Format("{0}. {{ {1} }}", comment, scriptId);
                PutText(13, 2, "", Key.EndKey);
                PutText(14, 2, "", Key.EndKey);
                PutText(15, 2, "", Key.EndKey);
                PutText(16, 2, "", Key.EndKey);
                PutText(17, 2, "", Key.EndKey);
                PutText(18, 2, "", Key.EndKey);
                PutText(13, 2, comment, ReflectionInterface.Key.F6);
            }
            else
            {
                if (comment.Length + scriptId.Length > 585)
                    throw new Exception("The requested comment will not fit on LP50");

                PutText(13, 2, comment.SafeSubString(0, 75));

                for (int segmentStart = 75; segmentStart <= comment.Length - 1; segmentStart += 75)
                    EnterText(comment.SafeSubString(segmentStart, 75));

                EnterText(string.Format(" {{ {0} }}", scriptId));

                Hit(ReflectionInterface.Key.F6);
            }

            return CheckForText(22, 3, "48003") || CheckForText(22, 3, "48081"); // 48003 = Success; 48081 = Alternative success code with info attached

        }

        /// <summary>
        /// Adds a comment in LP50, appending the text to the default comment.
        /// </summary>
        /// <param name="ssn">SSN for borrower/Reference Id for reference</param>
        /// <param name="activityType">Activity Type</param>
        /// <param name="activityContact">Contact Type</param>
        /// <param name="actionCode">Action code to use</param>
        /// <param name="comment">Comment must be less than 585 characters</param>
        /// <param name="scriptId">Script Id</param>
        /// <returns></returns>
        public bool AppendCommentInLP50(string ssn, string activityType, string activityContact, string actionCode, string comment, string scriptId)
        {
            if (ssn.IsNullOrEmpty())
                return false;
            FastPath("LP50A");
            Hit(ReflectionInterface.Key.EndKey);
            PutText(3, 48, "", true);
            if (ssn.SafeSubString(0, 3).ToUpper().Contains("RF@"))
            {
                PutText(3, 13, "", true);
                PutText(3, 48, ssn);
            }
            else
                PutText(3, 13, ssn);

            PutText(9, 20, actionCode, ReflectionInterface.Key.Enter);

            if (!CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY"))
                return false;

            PutText(7, 2, activityType);
            PutText(7, 5, activityContact);
            

            Coordinate startingLocation = new Coordinate() { Row = 13, Column = 2 };
            Coordinate endingLocation = new Coordinate() { Row = 20, Column = 76 };
            Coordinate lastCharacter = FindLastCharacter(startingLocation, endingLocation);
            
            Coordinate appendLocation;
            if (lastCharacter == null) // No text found, so start comment at beginning of comment field 
            {
                appendLocation = startingLocation;
                comment = comment ?? ""; // If null, blank comment
            }
            else
            {
                appendLocation = new Coordinate() { Row = lastCharacter.Row, Column = lastCharacter.Column + 1 };
                comment = comment == null ? "" : $" {comment}"; // Add space to beginning if not null, otherwise blank comment
            }
            
            if (appendLocation.Column >= endingLocation.Column && appendLocation.Row < endingLocation.Row) // Wrap text to next line if necessary
            {
                appendLocation.Column = startingLocation.Column;
                appendLocation.Row++;
            }

            int defaultCommentLength = ((appendLocation.Row - startingLocation.Row) * 75) + (appendLocation.Column - startingLocation.Column); // 75 = length of each comment line in the session
            if (comment.Length + scriptId.Length + defaultCommentLength > 585)
                throw new Exception("The requested comment will not fit on LP50");

            int firstLineLength = endingLocation.Column - appendLocation.Column + 1; // Calculate remaining space on line 
            if (appendLocation.Column < 77 && appendLocation.Row < 21) // Comment box goes from (13,2) to (20,76). Verify this starts within that box.
                PutText(appendLocation.Row, appendLocation.Column, comment.SafeSubString(0, firstLineLength));

            for (int segmentStart = firstLineLength; segmentStart <= comment.Length - 1; segmentStart += 75)
                EnterText(comment.SafeSubString(segmentStart, 75));

            EnterText(string.Format(" {{ {0} }}", scriptId));

            Hit(ReflectionInterface.Key.F6);

            return CheckForText(22, 3, "48003") || CheckForText(22, 3, "48081"); // 48003 = Success; 48081 = Alternative success code with info attached
        }

        /// <summary>
        /// Finds the coordinates for the last character for a given range
        /// in the session. If no text is found, returns null.
        /// </summary>
        /// <param name="start">Starting coordinates</param>
        /// <param name="end">Ending coordinates</param>
        /// <returns></returns>
        private Coordinate FindLastCharacter(Coordinate start, Coordinate end)
        {
            if (start.Row > end.Row || start.Column > end.Column)
                throw new Exception("Invalid coordinates provided. Unable to find location in session");

            int numberOfRows = end.Row - start.Row + 1;
            int numberOfColumns = end.Column - start.Column + 1;
            Coordinate location = null;

            for (int i = 0; i < numberOfRows; i++)
            {
                string text = GetText(start.Row + i, start.Column, numberOfColumns).Replace("_","");
                if (i == 0 && string.IsNullOrWhiteSpace(text))
                    return location; // No text found
                else if (!string.IsNullOrWhiteSpace(text))
                {
                    location = new Coordinate();
                    location.Row = start.Row + i;
                    location.Column = start.Column + text.Length - 1;
                }
            }

            return location;

        }

        /// <summary>
        /// Add a queue task to LP9O
        /// </summary>
        /// <param name="ssn">Borrower SSN</param>
        /// <param name="workGroup">Workgroup ID</param>
        /// <param name="dueDate">Date due</param>
        /// <param name="comment1">First comment line</param>
        /// <param name="comment2">Second comment line</param>
        /// <param name="comment3">Third comment line</param>
        /// <param name="comment4">Fourth comment line</param>
        /// <returns></returns>
        public bool AddQueueTaskInLP9O(string ssn, string workGroup, DateTime? dueDate = null, string comment1 = null, string comment2 = null, string comment3 = null, string comment4 = null)
        {
            //Try to access LP90 up to 3 times
            for (int i = 0; i < 3; i++)
            {
                FastPath("LP9OA" + ssn + ";;" + workGroup);
                if (WaitForText(22, 3, "44000", 5)) //wait 5 seconds for a response
                    break;
            }
            //See if we got in.
            if (!CheckForText(1, 61, "OPEN ACTIVITY DETAIL"))
                return false;
            //Add the queue task.
            if (dueDate.HasValue)
                PutText(11, 25, dueDate.Value.ToString("MMddyyyy"));
            string comment = string.Format("{0} {1} {2} {3}", comment1, comment2, comment3, comment4);
            if (comment.Length > 200)
                throw new Exception("The requested comment will not fit on LP9OA");

            PutText(16, 12, comment);
            Hit(ReflectionInterface.Key.F6);
            return WaitForText(22, 3, "48003", 2);
        }

        /// <summary>
        /// Attempts to add a cmoment on TD37 for the first app listed.
        /// </summary>
        public bool Atd37FirstLoan(string ssn, string arc, string comment, string scriptId, string utId, DateTime? neededBy = null)
        {
            //TODO: make sure we hit LP40 before adding comments if needed?
            FastPath("TX3Z/ATD37" + ssn);
            if (ScreenCode != "TDX38")
                return false;
            //find arc
            while (ReflectionSession.FindText(arc, 8, 8) == 0) //0 is not found
            {
                Hit(Key.F8);
                if (MessageCode == "90007")
                    return false;
            }
            //select arc
            PutText(ReflectionSession.FoundTextRow, ReflectionSession.FoundTextColumn - 5, "01", Key.Enter);
            if (ScreenCode != "TDX39")
                return false;
            if (neededBy.HasValue)
            {
                try
                {
                    PutText(6, 13, neededBy.Value.ToString("MMddyy"));
                }
                catch (Exception ex)
                {
                    //ignore exception if we're unable to set a neededBy date.
                }
            }
            //mark the first app
            PutText(11, 18, "X");
            if (MessageCode == "01490" || MessageCode == "01764")
                return false;
            //TODO: logic for comments larger than 154 characters
            PutText(21, 2, String.Format("{0}  {1}{2}{3} /{4}", comment, "{", scriptId, "}", utId));
            Hit(Key.Enter);
            if (MessageCode != "02860")
                return false;

            return true;
        }

        /// <summary>
        /// Checks to see if there is an abend on the screen. If there is, it process logs the error and returns true.
        /// </summary>
        public bool CheckScreenAbend()
        {
            Thread.Sleep(1000);
            if (CheckForText(4, 17, "E") && CheckForText(4, 28, "R") && CheckForText(4, 41, "R") && CheckForText(4, 54, "O") && CheckForText(4, 66, "R"))
            {
                Hit(Key.PageUp);
                StringBuilder sb = new StringBuilder();
                if (CheckForText(1, 32, "ABEND"))
                {
                    for (int i = 1; i <= 24; i++)
                    {
                        if (GetText(i, 1, 80).IsPopulated())
                            sb.AppendLine(GetText(i, 12, 60));
                    }
                }
                string message = string.Format("There was an ABEND in screen {0}. Abend Information:\r\n{1}", GetText(3, 28, 4), sb.ToString());
                LogRun.AddNotification(message, ProcessLogger.NotificationType.ErrorReport, ProcessLogger.NotificationSeverityType.Critical);
                return true;
            }
            return false;
        }
    }
}