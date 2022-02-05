using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace MANMAIL
{
    public class ManualReturnMail : ScriptBase
    {
        public enum AddressRegion
        {
            Onelink,
            Compass,
            Both,
            None
        }
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        public int? BusinessUnit { get; set; }
        public bool IsDeceased { get; set; }

        public bool? IsBorrower { get; set; } = null;

        public ManualReturnMail(ReflectionInterface ri)
            : base(ri, "MANMAIL", DataAccessHelper.Region.Uheaa)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LogRun = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true, false, false);
            DA = new DataAccess(LogRun);
        }

        public override void Main()
        {
            BusinessUnit = null;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
                return;
            bool continueProcessing = true;
            bool processedOne = false; // Make sure something is processed before saying 'Process Complete'
            string barcode = "";

            while (continueProcessing)
            {
                BusinessUnit = null;
                IsBorrower = null;
                using (ScannerInput input = new ScannerInput())
                    if (input.ShowDialog() == DialogResult.OK)
                        barcode = input.ScannedInput;
                    else
                    {
                        continueProcessing = false;
                        continue;
                    }
                using (ReturnMail rm = new ReturnMail(RI, barcode, DA))
                {
                    if (rm.ShowDialog() != DialogResult.OK)
                        continue;
                    processedOne = true;

                    AddressRegion invalidatedOn = AddressRegion.None;

                    IsDeceased = rm.LetterReturnMailReason.ToLower().Contains("deceased");
                    if (rm.Letter.IsPopulated())
                        BusinessUnit = DA.GetUnitForLetter(rm.Letter);
                    bool forwarding = Dialog.Def.YesNo("Was a forwarding address provided?");
                    if (rm.SelectedAccountIdentifier.Contains("@"))
                    {
                        ProcessOneLinkReference(rm.SelectedAccountIdentifier, rm.LetterCreateDate, rm.LetterReturnDate, rm.LetterReturnMailReason, forwarding, rm.Letter);
                        invalidatedOn = AddressRegion.Onelink;
                    }
                    else if (rm.SelectedAccountIdentifier.ToUpper().Contains("P"))
                    {
                        ProcessCompassReference(rm.SelectedAccountIdentifier, rm.LetterCreateDate, rm.LetterReturnDate, rm.LetterReturnMailReason, forwarding, rm.Letter);
                        invalidatedOn = AddressRegion.Compass;
                    }
                    else
                        invalidatedOn = ProcessBorrower(rm.AccountIdentifier, rm.SelectedAccountIdentifier, rm.LetterCreateDate, rm.LetterReturnDate, rm.LetterReturnMailReason, forwarding, rm.Letter);
                    if (BusinessUnit != null)
                        CheckCABorrower(rm, invalidatedOn);
                }
            }
            if (processedOne)
                Dialog.Info.Ok("Processing Complete");
        }

        private void CheckCABorrower(ReturnMail rm, AddressRegion invalidatedOn)
        {
            bool selectedReference = rm.SelectedAccountIdentifier.ToUpper().Contains("P") || rm.SelectedAccountIdentifier.ToUpper().Contains("@");
            bool primaryReference = rm.AccountIdentifier.ToUpper().Contains("P") || rm.AccountIdentifier.ToUpper().Contains("@");
            //Don't send mail if either account is a reference
            if (!(selectedReference || primaryReference))
            {
                //Handle region processing separately becasue of both region/cross region endorser ambiguity
                List<BorrowerEmailData> compassEmailData = new List<BorrowerEmailData>();
                List<BorrowerEmailData> onelinkEmailData = new List<BorrowerEmailData>();
                if(invalidatedOn == AddressRegion.Both)
                {
                    compassEmailData = DA.GetCAEmailData(rm.SelectedAccountIdentifier, AddressRegion.Compass, IsBorrower.HasValue ? IsBorrower.Value : true);
                    onelinkEmailData = DA.GetCAEmailData(rm.SelectedAccountIdentifier, AddressRegion.Onelink, IsBorrower.HasValue ? IsBorrower.Value : true);
                }
                else if(invalidatedOn == AddressRegion.Compass)
                {
                    compassEmailData = DA.GetCAEmailData(rm.SelectedAccountIdentifier, AddressRegion.Compass, IsBorrower.HasValue ? IsBorrower.Value : true);
                }
                else if(invalidatedOn == AddressRegion.Onelink)
                {
                    onelinkEmailData = DA.GetCAEmailData(rm.SelectedAccountIdentifier, AddressRegion.Onelink, IsBorrower.HasValue ? IsBorrower.Value : true);
                }
                //Send compass emails if there are any
                foreach (BorrowerEmailData data in compassEmailData)
                {
                    bool isEndorser = data.Priority == 2 || data.Priority == 3;
                    string email = "CARTMAILUH.html";
                    DA.InsertEmailBatch(data, email, isEndorser);
                }
                //Send onelink emails if there are any
                foreach(BorrowerEmailData data in onelinkEmailData)
                {
                    bool isEndorser = data.Priority == 2 || data.Priority == 3;
                    string email = "RTRNMLEML.html";
                    DA.InsertEmailBatch(data, email, isEndorser);
                }
            }
        }

        private void ProcessCompassReference(string accountIdentifer, DateTime createDate, DateTime returnDate, string reason, bool forwarding, string letter)
        {
            if (BusinessUnit == null)
                BusinessUnit = 1;
            RI.FastPath("TX3Z/ITX1JR;" + accountIdentifer);

            if (!RI.CheckForText(1, 71, "TXX1R-03"))
            {
                Dialog.Error.Ok("Unable to locate the reference please try again.");
                return;
            }
            AddressData refAddr = GetCompassRefAddrData(accountIdentifer);
            ShowCompassRefDemos(refAddr.Ssn, accountIdentifer, forwarding, refAddr, letter, createDate, reason);
        }

        private AddressData GetCompassRefAddrData(string accountIdentifer)
        {
            AddressData refAddr = new AddressData()
            {
                Ssn = RI.GetText(7, 11, 11).Replace(" ", ""),
                AccountNumber = accountIdentifer,
                Name = string.Format("{0} {1}", RI.GetText(4, 34, 14), RI.GetText(4, 6, 23)),
                Address1 = RI.GetText(11, 10, 30),
                Address2 = RI.GetText(12, 10, 30).Replace("_", ""),
                AddressIsValid = RI.GetText(11, 55, 1),
                AddressVerifiedDate = RI.GetText(10, 32, 8).Replace(" ", @"/"),
                City = RI.GetText(14, 8, 21),
                State = RI.GetText(14, 32, 2),
                Zip = RI.GetText(14, 40, 17)
            };
            return refAddr;
        }

        private void ShowCompassRefDemos(string ssn, string accountIdentifer, bool forwarding, AddressData refAddr, string letter, DateTime createDate, string returnRea)
        {
            using (Demos d = new Demos(refAddr.Ssn, refAddr.AccountNumber, refAddr.Name, null, refAddr, forwarding, DA))
            {
                bool cSuccess = false;
                if (d.ShowDialog() != DialogResult.OK)
                    return;
                if (d.BadAddress.AddressIsValid != "N")//Invalidate the address
                    cSuccess = InvalidateCompassRefAddress(ssn, accountIdentifer, forwarding, letter, createDate, returnRea, d.BadAddress, d.ForwardingAddress);
                if (d.ForwardingAddress != null && cSuccess)//Add the new address
                    UpdateCompassRefAddress(accountIdentifer, d, createDate);
            }
        }

        private bool UpdateCompassRefAddress(string accountIdentifer, Demos d, DateTime createDate)
        {
            RI.FastPath("TX3Z/CTX1JR;" + accountIdentifer);
            RI.Hit(ReflectionInterface.Key.F6, 2);
            RI.PutText(11, 10, d.ForwardingAddress.Address1, true);
            RI.PutText(12, 10, d.ForwardingAddress.Address2, true);
            RI.PutText(14, 8, d.ForwardingAddress.City, true);
            RI.PutText(14, 32, d.ForwardingAddress.State);
            RI.PutText(14, 40, d.ForwardingAddress.Zip, true);
            RI.PutText(11, 55, "Y");
            RI.PutText(10, 32, DateTime.Now.ToString("MMddyy"), ReflectionInterface.Key.Enter);
            return RI.MessageCode == "01096";
        }

        private bool InvalidateCompassRefAddress(string ssn, string referenceId, bool forwarding, string letter, DateTime createDate, string returnRea, AddressData badAddress, AddressData newAddress)
        {
            if (!AddCompassComment(ssn, "", referenceId, forwarding, letter, createDate, returnRea, badAddress, newAddress, false))
                return false;
            RI.FastPath("TX3Z/CTX1JR;" + referenceId);
            if (RI.GetText(10, 32, 8).Replace(" ", "/").ToDate().Date < createDate.Date)
            {
                if (RI.CheckForText(1, 71, "TXX1R"))
                {
                    RI.Hit(ReflectionInterface.Key.F6, 2);
                    RI.PutText(11, 55, "N");
                    RI.PutText(10, 32, DateTime.Now.ToString("MMddyy"), ReflectionInterface.Key.Enter);
                    return true;
                }
            }
            return false;
        }

        private void AccessLp2C(string accountIdentifier, string mode)
        {
            RI.FastPath(string.Format("LP2C{0};{1}", mode, accountIdentifier));
        }

        private void ProcessOneLinkReference(string accountIdentifier, DateTime createDate, DateTime returnDate, string reason, bool forwarding, string letter)
        {
            if (BusinessUnit == null)
                BusinessUnit = 0;
            AccessLp2C(accountIdentifier, "I");

            if (RI.CheckForText(1, 67, "REFERENCE"))
            {
                Dialog.Error.Ok("Unable to locate the reference please try again.");
                return;
            }

            AddressData refAddr = GetOLRefData();
            ShowOlRefDemos(accountIdentifier, forwarding, refAddr, letter, createDate, reason);
        }

        private void ShowOlRefDemos(string accountIdentifier, bool forwarding, AddressData refAddr, string letter, DateTime createDate, string returnRea)
        {
            using (Demos d = new Demos(refAddr.Ssn, refAddr.AccountNumber, refAddr.Name, refAddr, null, forwarding, DA))
            {
                if (d.ShowDialog() != DialogResult.OK)
                    return;

                bool olSuccess = false;
                if (d.BadAddress.AddressIsValid != "N")//Invalidate the address
                    olSuccess = InvalidateOLRefAddress(accountIdentifier, forwarding, letter, createDate, returnRea, d.BadAddress, d.ForwardingAddress);
                if (d.ForwardingAddress != null && olSuccess)//Add the new address
                    UpdateOlRefAddress(accountIdentifier, d, createDate);
            }
        }

        private bool InvalidateOLRefAddress(string accountIdentifier, bool forwarding, string letter, DateTime createDate, string returnRea, AddressData badAddress, AddressData newAddress)
        {
            if (!AddOlComment("", accountIdentifier, forwarding, letter, createDate, returnRea, badAddress, newAddress, false))
                return false;
            AccessLp2C(accountIdentifier, "C");
            if (RI.CheckForText(8, 55, "MMDDCCYY") || RI.GetText(8, 55, 8).ToDate().Date < createDate)
            {
                RI.PutText(8, 53, "N");
                RI.PutText(8, 55, DateTime.Now.ToString("MMddyyyy"), ReflectionInterface.Key.Enter);
                RI.Hit(ReflectionInterface.Key.F6);
                return RI.AltMessageCode == "49000";
            }
            return false;
        }

        private bool UpdateOlRefAddress(string accountIdentifier, Demos d, DateTime createDate)
        {
            AccessLp2C(accountIdentifier, "C");
            RI.PutText(8, 9, d.ForwardingAddress.Address1, true);
            RI.PutText(9, 9, d.ForwardingAddress.Address2, true);
            RI.PutText(10, 9, d.ForwardingAddress.City, true);
            RI.PutText(10, 52, d.ForwardingAddress.State);
            RI.PutText(10, 60, d.ForwardingAddress.Zip, true);
            RI.PutText(8, 53, "Y");
            RI.PutText(8, 55, DateTime.Now.ToString("MMddyyyy"), ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F6);
            return RI.AltMessageCode == "49000";
        }

        private AddressData GetOLRefData()
        {
            AddressData refAddr = new AddressData()
            {
                Ssn = RI.GetText(3, 39, 9),
                AccountNumber = RI.GetText(3, 14, 10),
                Name = string.Format("{0} {1}", RI.GetText(4, 44, 12), RI.GetText(4, 5, 34)),
                Address1 = RI.GetText(8, 9, 36),
                Address2 = RI.GetText(9, 9, 36),
                AddressIsValid = RI.GetText(8, 53, 1),
                AddressVerifiedDate = RI.GetText(8, 55, 8),
                City = RI.GetText(10, 9, 31),
                State = RI.GetText(10, 52, 2),
                Zip = RI.GetText(10, 60, 10)
            };
            return refAddr;
        }


        /// <summary>
        /// The address data passed in should be associated with accountId2
        /// </summary>
        private bool AreSamePerson(string accountId1, string accountId2, AddressData compass, AddressData onelink)
        {
            if(accountId1.Length == accountId2.Length)
            {
                return accountId1 == accountId2;
            }
            else if(accountId1.Length == 9 && accountId2.Length == 10)
            {
                if(compass == null)
                {
                    return accountId1 == onelink.Ssn;
                }
                else
                {
                    return accountId1 == compass.Ssn;
                }
            }
            else if(accountId1.Length == 10 && accountId2.Length == 9)
            {
                if(compass == null)
                {
                    return accountId1 == onelink.AccountNumber;
                }
                else
                {
                    return accountId1 == compass.AccountNumber;
                }
            }
            return false;
        }

        private AddressRegion ProcessBorrower(string accountIdentifier, string selectedAccountIdentifer, DateTime createDate, DateTime returnDate, string reason, bool forwarding, string letter)
        {
            AddressRegion invalidatedOn = AddressRegion.None;
            AddressData oneLink = GetOnelinkAddress(selectedAccountIdentifer);
            AddressData compass = GetCompassAddress(selectedAccountIdentifer);
            if (BusinessUnit == null)
                SetBusinessUnit(oneLink, compass);

            if (oneLink == null && compass == null)
            {
                Dialog.Error.Ok("Unable to locate the borrower please try again.");
                return invalidatedOn;
            }

            IsBorrower = AreSamePerson(accountIdentifier, selectedAccountIdentifer, compass, oneLink);
            if (IsBorrower.HasValue && IsBorrower.Value)
                accountIdentifier = "";

            string ssn = oneLink == null ? compass.Ssn : oneLink.Ssn;
            string accountNumber = oneLink == null ? compass.AccountNumber : oneLink.AccountNumber;
            string name = oneLink == null ? compass.Name : oneLink.Name;

            using (Demos d = new Demos(ssn, accountNumber, name, oneLink, compass, forwarding, DA))
            {
                if (d.ShowDialog() != DialogResult.OK)
                    return invalidatedOn;

                invalidatedOn = UpdateBorrower(createDate, reason, forwarding, letter, ssn, d, accountIdentifier, IsBorrower.HasValue ? IsBorrower.Value : true);
            }
            return invalidatedOn;
        }

        /// <summary>
        /// Determines the business unit to know which letter to send.
        /// </summary>
        private void SetBusinessUnit(AddressData oneLink, AddressData compass)
        {
            if (compass == null && oneLink != null)
                BusinessUnit = 0;
            if (compass != null && oneLink == null)
                BusinessUnit = 1;
            if (compass != null && oneLink != null)
                BusinessUnit = 2;
            if (compass == null && oneLink == null)
                BusinessUnit = null;
        }

        private AddressRegion UpdateBorrower(DateTime createDate, string reason, bool forwarding, string letter, string ssn, Demos d, string accountIdentifier, bool isBorrower)
        {
            bool cSuccess = false;
            bool olSuccess = false;
            bool compassCommentSuccess = false;
            bool OneLinkCommentSuccess = false;
            DateTime? compassLastVerified = null;
            DateTime? onelinkLastVerified = null;

            if (d.SelectedRegion.IsIn(Demos.AddressRegion.Compass, Demos.AddressRegion.Both))
            {
                RI.FastPath("TX3Z/CTX1J;" + ssn);
                if (RI.CheckForText(1, 71, "TXX1R"))
                    if (accountIdentifier.IsNullOrEmpty())
                        compassCommentSuccess = AddCompassComment(ssn, "", "", forwarding, letter, createDate, reason, d.BadAddress, d.ForwardingAddress, isBorrower);
                    else
                        compassCommentSuccess = AddCompassComment(accountIdentifier, ssn, "", forwarding, letter, createDate, reason, d.BadAddress, d.ForwardingAddress, isBorrower);
            }
            if (d.SelectedRegion.IsIn(Demos.AddressRegion.Onelink, Demos.AddressRegion.Both))
            {
                RI.FastPath("LP22C" + ssn);
                if (!RI.CheckForText(1, 60, "PERSON"))
                    if (accountIdentifier.IsNullOrEmpty())
                        OneLinkCommentSuccess = AddOlComment(ssn, "", forwarding, letter, createDate, reason, d.BadAddress, d.ForwardingAddress, isBorrower);
                    else
                        OneLinkCommentSuccess = AddOlComment(accountIdentifier, ssn, forwarding, letter, createDate, reason, d.BadAddress, d.ForwardingAddress, isBorrower);
            }
            if (d.InvalidAddress)//Invalidate the address
            {
                if (compassCommentSuccess)
                    cSuccess = InvalidateCompassBwrAddr(ssn, accountIdentifier, "", forwarding, letter, createDate, reason, d.BadAddress, d.ForwardingAddress, out compassLastVerified);
                if (OneLinkCommentSuccess)
                    olSuccess = InvalidateOLBwr(ssn, "", forwarding, letter, createDate, reason, d.BadAddress, out onelinkLastVerified);
            }

            if (d.ForwardingAddress != null)//Add the new address
            {
                if (compassCommentSuccess && d.SelectedRegion.IsIn(Demos.AddressRegion.Compass, Demos.AddressRegion.Both) && compassLastVerified.HasValue && compassLastVerified.Value < createDate)
                    UpdateCompassBwrAddr(ssn, d, createDate);
                if (OneLinkCommentSuccess && d.SelectedRegion.IsIn(Demos.AddressRegion.Onelink, Demos.AddressRegion.Both) && onelinkLastVerified.HasValue && onelinkLastVerified.Value < createDate)
                    UpdateOlBwrAddr(ssn, d, createDate);
            }
            //Return the code to show which regions were invalidated
            if(d.SelectedRegion == Demos.AddressRegion.Both)
            {
                return AddressRegion.Both;
            }
            else if(d.SelectedRegion == Demos.AddressRegion.Compass)
            {
                return AddressRegion.Compass;
            }
            else if(d.SelectedRegion == Demos.AddressRegion.Onelink)
            {
                return AddressRegion.Onelink;
            }
            else
            {
                return AddressRegion.None;
            }
        }

        private bool UpdateOlBwrAddr(string ssn, Demos d, DateTime createDate)
        {
            RI.FastPath("LP22C" + ssn);
            RI.PutText(3, 9, "2");
            RI.PutText(10, 9, d.ForwardingAddress.Address1, true);
            RI.PutText(11, 9, d.ForwardingAddress.Address2, true);
            RI.PutText(12, 9, d.ForwardingAddress.City, true);
            RI.PutText(12, 60, d.ForwardingAddress.Zip, true);
            RI.PutText(12, 52, d.ForwardingAddress.State);
            RI.PutText(10, 57, "Y", ReflectionInterface.Key.F6);
            return RI.AltMessageCode == "49000";
        }

        private bool InvalidateOLBwr(string ssn, string referenceId, bool forwarding, string letter, DateTime createDate, string returnRea, AddressData badAddress, out DateTime? lastVerified)
        {
            RI.FastPath("LP22C" + ssn);
            if (!RI.CheckForText(1, 60, "PERSON"))
            {
                if (returnRea.Contains("DOA"))
                    RI.AddQueueTaskInLP9O(ssn, "DEATHFLW");

                RI.FastPath("LP22C" + ssn);
                lastVerified = RI.GetText(10, 72, 8).ToDate().Date;
                if (lastVerified < createDate)
                {
                    RI.PutText(3, 9, "2");
                    RI.PutText(10, 57, "N", ReflectionInterface.Key.F6);
                    return true;
                }
            }
            lastVerified = null;
            return false;
        }

        private bool UpdateCompassBwrAddr(string ssn, Demos d, DateTime createDate)
        {
            RI.FastPath("TX3Z/CTX1J;" + ssn);
            RI.Hit(ReflectionInterface.Key.F6, 2);
            RI.PutText(11, 10, d.ForwardingAddress.Address1, true);
            RI.PutText(12, 10, d.ForwardingAddress.Address2, true);
            RI.PutText(14, 8, d.ForwardingAddress.City, true);
            RI.PutText(14, 32, d.ForwardingAddress.State);
            RI.PutText(14, 40, d.ForwardingAddress.Zip, true);
            RI.PutText(11, 55, "Y");
            if (!RI.CheckForText(1, 9, "E"))
                RI.PutText(8, 18, "25");
            RI.PutText(10, 32, DateTime.Now.ToString("MMddyy"), ReflectionInterface.Key.Enter);
            return RI.MessageCode == "01096";
        }

        private bool AddOlComment(string ssn, string referenceId, bool forwarding, string letter, DateTime createDate, string returnRea, AddressData badAddress, AddressData newAddress, bool isBorrower)
        {
            string comment = "";
            if (!forwarding)
                comment = string.Format("RTRNMAIL, For: {0}, {1}, {2}, {3:MMddyyyy},  Received return mail from {4}.  Return Mail Reason: {5}", forwarding ? "Y" : "N", referenceId.IsNullOrEmpty() ? ssn : referenceId, letter, createDate, badAddress.ToString(), returnRea);
            else
            {
                comment = string.Format("Received return mail from {0}, New Address {1}, Return Mail Reason: {2}, forwarding address received and updated for {3}", badAddress.ToString(), newAddress.ToString(), returnRea, ssn.IsPopulated() ? "Borrower" : "Reference");
            }
            List<string> personType = new List<string>();
            if (referenceId.IsNullOrEmpty())
            {
                RI.FastPath("LP22I" + ssn);
                RI.Hit(ReflectionInterface.Key.F10);
                string name = RI.GetText(3, 40, 42);
                for (int row = 7; !RI.CheckForText(22, 3, "46004"); row++)
                {
                    if (row > 19 || RI.GetText(row, 11, 42).IsNullOrEmpty())
                    {
                        row = 6;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }
                    if (RI.GetText(row, 11, 42) == name && !RI.CheckForText(row, 7, "R"))
                        personType.Add(RI.GetText(row, 7, 1));
                }
            }
            else
                personType.Add("R");
            bool addedComment = false;
            if (personType.Contains("R"))
                addedComment = AddLp50PersonTypeR(referenceId, "LT", "90", "S4LRM", comment);
            if (personType.Contains("E") && !isBorrower)
                addedComment = AddLp50(ssn, "LT", "90", "SELRM", comment);
            if (personType.Contains("B"))
                addedComment = AddLp50(ssn, "LT", "90", "S4LRM", comment);

            return addedComment;
        }

        private bool AddLp50(string ssn, string activityType, string activityContact, string actionCode, string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = ssn,
                ActivityContact = activityContact,
                ActivityType = activityType,
                Arc = actionCode,
                ArcTypeSelected = ArcData.ArcType.OneLINK,
                Comment = comment,
                IsEndorser = false,
                IsReference = false,
                ScriptId = ScriptId
            };
            return arc.AddArc().ArcAdded;
        }

        private bool AddLp50PersonTypeR(string referenceId, string activityType, string activityContact, string actionCode, string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = referenceId,
                ActivityContact = activityContact,
                ActivityType = activityType,
                Arc = actionCode,
                ArcTypeSelected = ArcData.ArcType.OneLINK,
                Comment = comment,
                IsEndorser = false,
                IsReference = true,
                ScriptId = ScriptId
            };
            return arc.AddArc().ArcAdded;
        }

        private bool AddCompassComment(string ssn, string endorserId, string referenceId, bool forwarding, string letter, DateTime createDate, string returnRea, AddressData badAddress, AddressData newAddress, bool isBorrower)
        {
            if (newAddress == null)
                newAddress = new AddressData();
            string comment = "";
            if (referenceId != ssn)
                comment = string.Format("RTRNMAIL, For: {0}, {1}, {2}, {3:MMddyyyy},  Received return mail from {4}. New Address {6}  Return Mail Reason: {5}", forwarding ? "Y" : "N", referenceId, letter, createDate, badAddress.ToString(), returnRea, newAddress.ToString());
            else
                comment = string.Format("RTRNMAIL, For: {0}, {1}, {2}, {3:MMddyyyy},  Received return mail from {4}. New Address {6}  Return Mail Reason: {5}", forwarding ? "Y" : "N", ssn, letter, createDate, badAddress.ToString(), returnRea, newAddress.ToString());

            List<string> personType = new List<string>();
            string borrowerSSn = "";
            List<int> loanSeq = new List<int>();
            if (endorserId.IsPopulated() && endorserId != ssn)
            {
                if (ssn.ToUpper().Contains("P"))
                {
                    LogRun.AddNotification(string.Format("Account Identifier presented is not valid for person selected. P number can only invalidate P number. Provided: {0}, Selected: {1}", ssn, endorserId), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    MessageBox.Show(string.Format("Account Identifier presented is not valid for person selected. P number can only invalidate P number. Provided: {0}, Selected: {1}", ssn, endorserId), "BAD DATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new DemographicException(string.Format("Account Identifier presented is not valid for person selected. P number can only invalidate P number. Provided: {0}, Selected: {1}", ssn, endorserId));
                }
                borrowerSSn = HandleNullReference(endorserId, personType, borrowerSSn, loanSeq);
            }
            else if (referenceId.IsNullOrEmpty())
                borrowerSSn = HandleNullReference(ssn, personType, borrowerSSn, loanSeq);
            else
                personType.Add("R");

            if (loanSeq.Any())
                loanSeq = loanSeq.Distinct().ToList();

            return DetermineArc(ssn, comment, endorserId, referenceId, personType, loanSeq, borrowerSSn, isBorrower);
        }

        private bool DetermineArc(string ssn, string comment, string endorserId, string referenceId, List<string> personType, List<int> loanSeq, string borrowerSSn, bool isBorrower)
        {
            bool deceased = IsDeceased;
            bool arcAdded = false;
            IsDeceased = false;
            endorserId = endorserId ?? ssn;
            if (personType.Contains("R"))
                arcAdded = AddArcPersonTypeR(ssn, comment, referenceId, personType, loanSeq);
            else if (personType.Contains("E") && !isBorrower)
                arcAdded = AddArcPersonTypeE(ssn, comment, referenceId, personType, loanSeq, borrowerSSn, endorserId);
            else if (personType.Contains("B"))
                arcAdded = AddArcPersonTypeB(ssn, comment, referenceId, personType, loanSeq);

            if (deceased && personType.Contains("B"))
            {
                IsDeceased = deceased;
                comment = "";
                AddArcPersonTypeB(ssn, comment, referenceId, personType, loanSeq);
            }
            return arcAdded;
        }

        private string HandleNullReference(string ssn, List<string> personType, string borrowerSSn, List<int> loanSeq)
        {
            foreach (string type in new string[] { "B", "E" })
            {
                RI.FastPath(string.Format("TX3Z/ITX1J{0}{1}", type, ssn));
                if (RI.CheckForText(1, 71, "TXX1R"))
                {
                    personType.Add(type);
                    if (type == "E")
                    {
                        borrowerSSn = RI.GetText(7, 11, 11).Replace(" ", "");
                        RI.Hit(ReflectionInterface.Key.F2);
                        RI.Hit(ReflectionInterface.Key.F4);
                        for (int row = 10; RI.MessageCode != "90007"; row++)
                        {
                            if (row > 21 || RI.CheckForText(row, 2, "  "))
                            {
                                row = 9;
                                RI.Hit(ReflectionInterface.Key.F8);
                                continue;
                            }

                            if (RI.CheckForText(row, 5, "E") && RI.CheckForText(row, 13, ssn))
                                loanSeq.Add(RI.GetText(row, 9, 3).ToInt());
                        }
                    }
                }
            }

            return borrowerSSn;
        }

        private bool AddArcPersonTypeR(string ssn, string comment, string referenceId, List<string> personType, List<int> loanSeq)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = ssn,
                Arc = IsDeceased ? "G113A" : "S4LRM",
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                IsEndorser = false,
                IsReference = true,
                RegardsTo = referenceId,
                RegardsCode = "R",
                ScriptId = ScriptId
            };
            if (!arc.AddArc().ArcAdded)
                if (!RI.Atd37FirstLoan(ssn, "S4LRM", comment, ScriptId, UserId))
                {
                    string message = string.Format("Unable to add comment for Borrower/Reference {0}/{1} Arc: S4LRM Comment: {2}", ssn, referenceId, comment);
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Dialog.Error.Ok(message);
                    return false;
                }
            return true;
        }

        private bool AddArcPersonTypeB(string ssn, string comment, string recipientId, List<string> personType, List<int> loanSeq)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = ssn,
                Arc = IsDeceased ? "G113A" : "S4LRM",
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                IsEndorser = false,
                IsReference = false,
                ScriptId = ScriptId,
                RegardsTo = recipientId.IsPopulated() ? recipientId : null,
                RegardsCode = recipientId.IsPopulated() ? "E" : null
            };
            if (!arc.AddArc().ArcAdded)
                if (!RI.Atd37FirstLoan(ssn, "S4LRM", comment, ScriptId, UserId))
                {
                    string message = string.Format("Unable to add comment for Borrower/Reference {0}/{1} Arc: S4LRM Comment: {2}", ssn, recipientId, comment);
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Dialog.Error.Ok(message);
                    return false;
                }
            return true;
        }

        private bool AddArcPersonTypeE(string ssn, string comment, string referenceId, List<string> personType, List<int> loanSeq, string borrowerSsn, string endorserId)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = borrowerSsn,
                RecipientId = IsDeceased ? null : endorserId,
                Arc = IsDeceased ? "G113A" : "SELRM",
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                Comment = comment,
                IsEndorser = true,
                IsReference = false,
                LoanSequences = loanSeq,
                RegardsTo = endorserId,
                RegardsCode = "E",
                ScriptId = ScriptId
            };
            if (!arc.AddArc().ArcAdded)
                if (!RI.Atd37FirstLoan(ssn, "SELRM", comment, ScriptId, UserId))
                {
                    LogRun.AddNotification(string.Format("Unable to add comment for Endorser {0} Arc: SELRM Comment: {1}", ssn, comment), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return false;
                }
            return true;
        }

        /// <summary>
        /// Invalidates the borrowers address in Compass
        /// </summary>
        private bool InvalidateCompassBwrAddr(string ssn, string endorserId, string referenceId, bool forwarding, string letter, DateTime createDate, string returnRea, AddressData badAddress, AddressData newAddress, out DateTime? lastVerified)
        {
            if (endorserId.IsPopulated())
                RI.FastPath("TX3Z/CTX1JE;" + ssn);
            else
                RI.FastPath("TX3Z/CTX1J;" + ssn);
            if (RI.CheckForText(1, 71, "TXX1R"))
            {
                lastVerified = RI.GetText(10, 32, 8).Replace(" ", "/").ToDate().Date;
                if (lastVerified < createDate.Date)
                {
                    RI.Hit(ReflectionInterface.Key.F6, 2);
                    RI.PutText(11, 55, "N");
                    if (!RI.CheckForText(1, 9, "E"))
                        RI.PutText(8, 18, "25");
                    RI.PutText(10, 32, DateTime.Now.ToString("MMddyy"), ReflectionInterface.Key.Enter);
                    if (RI.MessageCode != "01096")
                        Dialog.Warning.Ok("There was an error invalidating the address. Please review and make updates manually.");
                    return true;
                }
            }
            lastVerified = null;
            return false;
        }

        /// <summary>
        /// Gets the borrower address in Onelink
        /// </summary>
        private AddressData GetOnelinkAddress(string accountIdentifer)
        {
            RI.FastPath("LP22I*");
            if (accountIdentifer.Length == 9)
                RI.PutText(4, 33, accountIdentifer, ReflectionInterface.Key.Enter);
            else
            {
                RI.PutText(4, 33, "", true);
                RI.PutText(6, 33, accountIdentifer, ReflectionInterface.Key.Enter);
            }

            if (RI.CheckForText(1, 60, "PERSON"))
                return null;

            return new AddressData()
            {
                Ssn = RI.GetText(3, 23, 9),
                AccountNumber = RI.GetText(3, 60, 12).Replace(" ", ""),
                Name = string.Format("{0} {1}", RI.GetText(4, 44, 12), RI.GetText(4, 5, 34)),
                Address1 = RI.GetText(10, 9, 36),
                Address2 = RI.GetText(11, 9, 36),
                AddressIsValid = RI.GetText(10, 57, 1),
                AddressVerifiedDate = RI.GetText(10, 72, 8),
                City = RI.GetText(12, 9, 31),
                State = RI.GetText(12, 52, 2),
                Zip = RI.GetText(12, 60, 10)
            }; ;
        }

        /// <summary>
        /// Gets the borrowers address in Compass
        /// </summary>
        private AddressData GetCompassAddress(string accountIdentifer)
        {
            RI.FastPath("TX3Z/ITX1J ");
            RI.PutText(5, 16, "", true);
            RI.PutText(6, 16, "", true);
            RI.PutText(6, 20, "", true);
            RI.PutText(6, 23, "", true);
            if (accountIdentifer.Length == 9)
                RI.PutText(6, 16, accountIdentifer, ReflectionInterface.Key.Enter);
            else
                RI.PutText(6, 61, accountIdentifer, ReflectionInterface.Key.Enter);

            if (RI.CheckForText(1, 72, "TXX1K"))
                return null;

            return new AddressData()
            {
                Ssn = RI.GetText(3, 12, 11).Replace(" ", ""),
                AccountNumber = RI.GetText(3, 34, 12).Replace(" ", ""),
                Name = string.Format("{0} {1}", RI.GetText(4, 34, 14), RI.GetText(4, 6, 23)),
                Address1 = RI.GetText(11, 10, 30),
                Address2 = RI.GetText(12, 10, 30).Replace("_", ""),
                AddressIsValid = RI.GetText(11, 55, 1),
                AddressVerifiedDate = RI.GetText(10, 32, 8).Replace(" ", @"/"),
                City = RI.GetText(14, 8, 21),
                State = RI.GetText(14, 32, 2),
                Zip = RI.GetText(14, 40, 17)
            };
        }
    }
}