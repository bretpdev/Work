using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace MDIntermediary
{
    public class ActivityCommentHelper
    {
        public ActivityCommentHelper()
        {
            LoadContactCodes();
            LoadActivityCodes();
        }
        #region Dictionaries
        /// <summary>
        /// Links the ActivityCode enum with its corresponding string values.
        /// </summary>
        public Dictionary<ActivityCode, string> ActivityCodes = new Dictionary<ActivityCode, string>();
        /// <summary>
        /// Links the ContactCode enum with its corresponding string values, as well as some extra source codes.
        /// </summary>
        public Dictionary<ContactCode, ContactSource> ContactCodes = new Dictionary<ContactCode, ContactSource>();
        /// <summary>
        /// Load the Contact Codes dictionary
        /// </summary>
        private void LoadContactCodes()
        {
            ContactCodes = new Dictionary<ContactCode, ContactSource>();
            Action<ContactCode, string, string, string> acc = new Action<ContactCode, string, string, string>(
                (cc, code, lp22, tx1j) => ContactCodes[cc] = new ContactSource(code, lp22, tx1j));
            acc(ContactCode.ToAttorney, "33", "1", "43");
            acc(ContactCode.FromAttorney, "34", "1", "43");
            acc(ContactCode.ToBorrower, "03", "F", "41");
            acc(ContactCode.FromBorrower, "04", "F", "41");
            acc(ContactCode.ToComaker, "93", "K", "42");
            acc(ContactCode.FromComaker, "94", "K", "42");
            acc(ContactCode.ToCreditBureau, "83", "3", "11");
            acc(ContactCode.FromCreditBureau, "84", "3", "11");
            acc(ContactCode.ToDmv, "91", "6", "26");
            acc(ContactCode.FromDmv, "92", "6", "26");
            acc(ContactCode.ToEmployer, "81", "4", "23");
            acc(ContactCode.FromEmployer, "82", "4", "23");
            acc(ContactCode.ToEndorser, "69", "K", "42");
            acc(ContactCode.FromEndorser, "70", "K", "42");
            acc(ContactCode.ToFamily, "11", "1", "43");
            acc(ContactCode.FromFamily, "12", "1", "43");
            acc(ContactCode.ToGuarantor, "29", "K", "56");
            acc(ContactCode.FromGuarantor, "30", "K", "56");
            acc(ContactCode.ToLender, "05", "K", "31");
            acc(ContactCode.FromLender, "06", "K", "31");
            acc(ContactCode.ToMisc, "95", "K", "31");
            acc(ContactCode.FromMisc, "96", "K", "31");
            acc(ContactCode.ToPostOffice, "89", "2", "25");
            acc(ContactCode.FromPostOffice, "90", "2", "25");
            acc(ContactCode.ToPrison, "85", "K", "45");
            acc(ContactCode.FromPrison, "86", "K", "45");
            acc(ContactCode.ToReference, "27", "1", "43");
            acc(ContactCode.FromReference, "28", "1", "43");
            acc(ContactCode.ToSchool, "07", "D", "43");
            acc(ContactCode.FromSchool, "08", "D", "43");
            acc(ContactCode.ToUheaa, "09", "K", "31");
            acc(ContactCode.FromUheaa, "10", "K", "31");
            acc(ContactCode.To3rdParty, "TO", "1", "43");
            acc(ContactCode.From3rdParty, "TI", "1", "43");
        }
        /// <summary>
        /// Load the Activity Codes dictionary.
        /// </summary>
        private void LoadActivityCodes()
        {
            ActivityCodes = new Dictionary<ActivityCode, string>();
            ActivityCodes[ActivityCode.AccountMaintenance] = "AM";
            ActivityCodes[ActivityCode.Claim] = "CL";
            ActivityCodes[ActivityCode.CourtDocument] = "CD";
            ActivityCodes[ActivityCode.Email] = "EM";
            ActivityCodes[ActivityCode.Fax] = "FA";
            ActivityCodes[ActivityCode.Form] = "FO";
            ActivityCodes[ActivityCode.Letter] = "LT";
            ActivityCodes[ActivityCode.Misc] = "MS";
            ActivityCodes[ActivityCode.OfficeVisit] = "OV";
            ActivityCodes[ActivityCode.TelephoneContact] = "TC";
            ActivityCodes[ActivityCode.FailedCall] = "TT";
        }
        #endregion
        #region ComboBoxItems
        /// <summary>
        /// Get available Call Types (Incoming or Outgoing).  Also includes a default selection.
        /// </summary>
        public ComboBoxItem<CallType>[] GetCallTypes()
        {
            return new ComboBoxItem<CallType>[]
            {
                new ComboBoxItem<CallType>(),
                new ComboBoxItem<CallType>(CallType.IncomingCall, "Incoming Call"),
                new ComboBoxItem<CallType>(CallType.OutgoingCall, "Outgoing Call")
            };
        }
        /// <summary>
        /// Get available Call Recipient Types (Borrower, Endorser, etc).  Also includes a default selection.
        /// </summary>
        public RecipientComboBoxItem[] GetCallRecipientTarget(IBorrower borrower)
        {
            List<RecipientComboBoxItem> items = new List<RecipientComboBoxItem>();
            items.Add(new RecipientComboBoxItem());
            items.Add(new RecipientComboBoxItem(CallRecipientTarget.Borrower, "Borrower"));
            if (BorrowerHasEndorsers(borrower.AccountNumber))
                items.Add(new RecipientComboBoxItem(CallRecipientTarget.Endorser, "Endorser"));
            items.Add(new RecipientComboBoxItem(CallRecipientTarget.ThirdParty, "Third Party"));
            if (borrower.References.Any())
            {
                foreach (var reference in borrower.References.Where(o => o.StatusIndicator == "A").GroupBy(o => o.FullName).Select(o => o.First())) //distinct by full name
                {
                    items.Add(new RecipientComboBoxItem(CallRecipientTarget.Reference,
                        "Reference - " + reference.FullName + " - " + (reference.AuthorizedThirdPartyIndicator == "Y" ? "Authorized" : "Unauthorized"), reference));
                }
            }
            return items.ToArray();

        }
        /// <summary>
        /// Get available Call Statuses (Successful, No Answer, etc).  Also includes a default selection.
        /// </summary>
        public ComboBoxItem<CallStatusType>[] GetCallStatuses()
        {
            return new ComboBoxItem<CallStatusType>[]
            {
                new ComboBoxItem<CallStatusType>(),
                new ComboBoxItem<CallStatusType>(CallStatusType.CallSuccessful, "Successful Call"),
                new ComboBoxItem<CallStatusType>(CallStatusType.NoAnswer, "No Answer"),
                new ComboBoxItem<CallStatusType>(CallStatusType.AnsweringMachine, "Answering Machine or Service"),
                new ComboBoxItem<CallStatusType>(CallStatusType.WrongNumber, "Wrong Number"),
                new ComboBoxItem<CallStatusType>(CallStatusType.PhoneBusy, "Phone Busy"),
                new ComboBoxItem<CallStatusType>(CallStatusType.OutOfService, "Out of Service/Disconnected Number")
            };
        }
        /// <summary>
        /// Get available activity codes (Account Maintenance, Fax, Email, etc).  Also includes a default selection
        /// </summary>
        public ComboBoxItem<ActivityCode>[] GetActivityCodes()
        {
            //there are a few more activity codes in the enum, but they aren't available for processing mode
            return new ComboBoxItem<ActivityCode>[]
            {
                new ComboBoxItem<ActivityCode>(),
                new ComboBoxItem<ActivityCode>(ActivityCode.AccountMaintenance, "Account Maintenance"),
                new ComboBoxItem<ActivityCode>(ActivityCode.CourtDocument, "Court Document"),
                new ComboBoxItem<ActivityCode>(ActivityCode.Claim, "Claim"),
                new ComboBoxItem<ActivityCode>(ActivityCode.Email, "Email"),
                new ComboBoxItem<ActivityCode>(ActivityCode.Fax, "Fax"),
                new ComboBoxItem<ActivityCode>(ActivityCode.Form, "Form"),
                new ComboBoxItem<ActivityCode>(ActivityCode.Letter, "Letter"),
                new ComboBoxItem<ActivityCode>(ActivityCode.Misc, "Miscellaneous"),
                new ComboBoxItem<ActivityCode>(ActivityCode.OfficeVisit, "Office Visit")
            };
        }
        /// <summary>
        /// Get available contact codes (To: Comaker, From: Endorser, etc).  Also includes a default selection.
        /// </summary>
        public ComboBoxItem<ContactCode>[] GetContactCodes()
        {
            return new ComboBoxItem<ContactCode>[]
            {
                new ComboBoxItem<ContactCode>(),
                new ComboBoxItem<ContactCode>(ContactCode.ToAttorney,"To: Attorney"),
                new ComboBoxItem<ContactCode>(ContactCode.FromAttorney,"From: Attorney"),
                new ComboBoxItem<ContactCode>(ContactCode.ToBorrower,"To: Borrower"),
                new ComboBoxItem<ContactCode>(ContactCode.FromBorrower,"From: Borrower"),
                new ComboBoxItem<ContactCode>(ContactCode.ToComaker,"To: Co-Maker"),
                new ComboBoxItem<ContactCode>(ContactCode.FromComaker,"From: Co-Maker"),
                new ComboBoxItem<ContactCode>(ContactCode.ToCreditBureau,"To: Credit Bureau"),
                new ComboBoxItem<ContactCode>(ContactCode.FromCreditBureau,"From: Credit Bureau"),
                new ComboBoxItem<ContactCode>(ContactCode.ToDmv,"To: DMV"),
                new ComboBoxItem<ContactCode>(ContactCode.FromDmv,"From: DMV"),
                new ComboBoxItem<ContactCode>(ContactCode.ToEmployer,"To: Employer"),
                new ComboBoxItem<ContactCode>(ContactCode.FromEmployer,"From: Employer"),
                new ComboBoxItem<ContactCode>(ContactCode.ToEndorser,"To: Endorser"),
                new ComboBoxItem<ContactCode>(ContactCode.FromEndorser,"From: Endorser"),
                new ComboBoxItem<ContactCode>(ContactCode.ToFamily,"To: Family"),
                new ComboBoxItem<ContactCode>(ContactCode.FromFamily,"From: Family"),
                new ComboBoxItem<ContactCode>(ContactCode.ToGuarantor,"To: Guarantor"),
                new ComboBoxItem<ContactCode>(ContactCode.FromGuarantor,"From: Guarantor"),
                new ComboBoxItem<ContactCode>(ContactCode.ToLender,"To: Lender"),
                new ComboBoxItem<ContactCode>(ContactCode.FromLender,"From: Lender"),
                new ComboBoxItem<ContactCode>(ContactCode.ToMisc,"To: Miscellaneous"),
                new ComboBoxItem<ContactCode>(ContactCode.FromMisc,"From: Miscellaneous"),
                new ComboBoxItem<ContactCode>(ContactCode.ToPostOffice,"To: Post Office"),
                new ComboBoxItem<ContactCode>(ContactCode.FromPostOffice,"From: Post Office"),
                new ComboBoxItem<ContactCode>(ContactCode.ToPrison,"To: Prison"),
                new ComboBoxItem<ContactCode>(ContactCode.FromPrison,"From: Prison"),
                new ComboBoxItem<ContactCode>(ContactCode.ToReference,"To: Reference"),
                new ComboBoxItem<ContactCode>(ContactCode.FromReference,"From: Reference"),
                new ComboBoxItem<ContactCode>(ContactCode.ToSchool,"To: School"),
                new ComboBoxItem<ContactCode>(ContactCode.FromSchool,"From: School"),
                new ComboBoxItem<ContactCode>(ContactCode.ToUheaa,"To: UHEAA"),
                new ComboBoxItem<ContactCode>(ContactCode.FromUheaa,"From: UHEAA"),
                new ComboBoxItem<ContactCode>(ContactCode.To3rdParty,"To: Third Party"),
                new ComboBoxItem<ContactCode>(ContactCode.From3rdParty, "From: Third Party")
            };
        }
        #endregion
        /// <summary>
        /// Determine the ActivityCode, ContactCode, and Description based on the given selected options.
        /// </summary>
        public AcpSelectionResult CalculateSelection(CallType callType, CallRecipientTarget recipientTarget, Reference recipientTargetReference, CallStatusType? resultType, AcpRecipientInfo recipientInfo)
        {
            ActivityCode ac = ActivityCode.TelephoneContact;
            ContactCode cc = (ContactCode)(-1);
            string description = null;
            if (callType == CallType.IncomingCall || callType == CallType.OfficeVisit)
            {
                if (recipientTarget == CallRecipientTarget.Borrower)
                    cc = ContactCode.FromBorrower;
                else if (recipientTarget == CallRecipientTarget.Endorser)
                    cc = ContactCode.FromEndorser;
                else if (recipientTarget == CallRecipientTarget.Reference)
                    cc = ContactCode.FromReference;
                else if (recipientTarget == CallRecipientTarget.ThirdParty)
                    cc = ContactCode.From3rdParty;
            }
            else if (callType == CallType.OutgoingCall)
            {
                if (resultType == CallStatusType.CallSuccessful)
                {
                    if (recipientTarget == CallRecipientTarget.Borrower)
                        cc = ContactCode.ToBorrower;
                    else if (recipientTarget == CallRecipientTarget.Endorser)
                        cc = ContactCode.ToEndorser;
                    else if (recipientTarget == CallRecipientTarget.Reference)
                        cc = ContactCode.ToReference;
                    else if (recipientTarget == CallRecipientTarget.ThirdParty)
                        cc = ContactCode.To3rdParty;
                }
                else
                {
                    ac = ActivityCode.FailedCall;
                    cc = ContactCode.ToBorrower;
                    if (resultType == CallStatusType.AnsweringMachine)
                        description = "Answering Machine/Service";
                    else if (resultType == CallStatusType.NoAnswer)
                        description = "No Answer";
                    else if (resultType == CallStatusType.OutOfService)
                        description = "Disconnected Phone / Out of Service";
                    else if (resultType == CallStatusType.PhoneBusy)
                        description = "Phone Busy";
                    else if (resultType == CallStatusType.WrongNumber)
                        description = "Wrong Number";
                }
            }
            if (callType == CallType.OfficeVisit)
                ac = ActivityCode.OfficeVisit;
            var result = CalculateSelection(ac, cc);
            result.CallType = callType;
            result.RecipientTarget = recipientTarget;
            result.RecipientTargetReference = recipientTargetReference;
            result.CallStatusType = resultType;
            result.DescriptionValue = description;
            result.RecipientInfo = recipientInfo;
            return result;
        }

        /// <summary>
        /// Determine the ActivityCode, ContactCode, and Description based on the given selected options.
        /// </summary>
        public AcpSelectionResult CalculateSelection(ActivityCode activityCode, ContactCode contactCode)
        {
            var result = new AcpSelectionResult();
            result.ActivityCodeSelection = activityCode;
            result.ContactCodeSelection = contactCode;

            result.ActivityCodeValue = ActivityCodes[activityCode];

            result.ContactCodeValue = ContactCodes[contactCode].ContactCode;
            result.Lp22SourceValue = ContactCodes[contactCode].Lp22Source;
            result.Tx1jSourceValue = ContactCodes[contactCode].Tx1jSource;

            return result;
        }

        //[UsesSproc(DataAccessHelper.Database.Cdw, "[spMD_BorrowerHasEndorsers]")]
        [UsesSproc(DataAccessHelper.Database.Udw, "[spMD_BorrowerHasEndorsers]")]
        public static bool BorrowerHasEndorsers(string accountNumber)
        {
            return DataAccessHelper.ExecuteSingle<bool>("[spMD_BorrowerHasEndorsers]",
                DataAccessHelper.Database.Udw,
                SqlParams.Single("BorrowerAccountNumber", accountNumber));
        }

    }
}
