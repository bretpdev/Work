using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace MDIntermediary
{
    public class AcpProcessor
    {
        private Uheaa.Common.Scripts.ReflectionInterface ri;
        private int pld;
        public AcpProcessor(Reflection.Session session, int processLogId)
        {
            this.ri = new Uheaa.Common.Scripts.ReflectionInterface(session);
            this.pld = processLogId;
        }
        public void Process(string comment, IBorrower borrower, DataAccessHelper.Region region)
        {
            var acp = borrower.AcpResponses;
            //If no queue or user was working the queue in another region then return directly through TC00
            if (string.IsNullOrEmpty(acp.Queue.Queue) || acp.Queue.QueueRegion != region)
            {
                //attempt to access ACP
                ri.FastPath("TX3Z/ATC00" + borrower.SSN);
                if (ri.MessageCode != "01019" && ri.MessageCode != "03363")
                {
                    string sel = acp.EntryInfo.AcpSelection.Trim();
                    if (acp.Selection.RecipientTarget != CallRecipientTarget.Borrower && acp.Selection.CallType == CallType.OutgoingCall)
                    {
                        sel = "4"; //override
                       
                        comment = string.Format("CALLED {0} at {1} {2}",
                            acp.Selection.RecipientTarget.ToString(),
                            borrower.ContactPhoneNumber, acp.Selection.DescriptionValue) + " - " + comment;
                    }
                    ri.PutText(19, 38, sel, Key.Enter);
                }
                else
                {
                    if (borrower.NeedsDeconArc)
                        EnterAcpComments(comment, borrower.SSN, region, borrower.NeedsDeconArc, null, acp.Selection.RecipientTarget, borrower.AccountNumber);
                    return; //no more processing to do
                }
            }
            else
            {
                //return to TC00 through TX6X because queue was picked up when MD started
                ri.FastPath(String.Format("TX3Z/ITX6X{0};{1}", acp.Queue.Queue, acp.Queue.SubQueue));
                ri.PutText(21, 18, "1", Key.Enter);  //select task in working status
                ri.Hit(Key.F4);
            }
            ProcessCornerstone(comment, borrower); //TODO This does not look like it only applies to Cornerstone

            var bankruptcyInfo = borrower.AcpResponses.Bankruptcy;
            var deceasedInfo = borrower.AcpResponses.Deceased;
            if (deceasedInfo != null)
                comment += deceasedInfo.GenerateComment();

            //get through more of ACP
            while (!new string[] { "TCX06", "TCX0I", "TCX0C" }.Contains(ri.ScreenCode) && !ri.CheckForText(1, 74, "TCX0D"))
            {
                var entryInfo = borrower.AcpResponses.EntryInfo;
                if (ri.CheckForText(1, 74, "TCX02"))
                {
                    //enter gathered information
                    if (ri.CheckForText(8, 2, "_"))
                        ri.PutText(8, 2, "X");
                    else if (ri.CheckForText(13, 2, "_"))
                        ri.PutText(13, 2, "X");
                }
                else if (ri.CheckForText(1, 74, "TCX04"))
                {
                    //Enter the phone number selected in the Demo popup
                    if (!string.IsNullOrEmpty(borrower.ContactPhoneNumber) && entryInfo.AcpSelection.Replace("0", "").Trim() == "2")
                    {
                        ri.PutText(7, 17, "");
                        ri.Hit(Key.EndKey);
                        ri.PutText(7, 17, borrower.ContactPhoneNumber);
                    }
                    ri.PutText(22, 35, entryInfo.DiscussionOption);
                }
                else if (ri.CheckForText(1, 72, "TCX14"))
                {
                    //Enter gathered info
                    if (ri.CheckForText(10, 6, "BK"))
                        ri.PutText(22, 13, "1");
                    else if (ri.CheckForText(16, 6, "BK"))
                        ri.PutText(22, 13, "2");
                    else
                        ri.PutText(22, 13, "");
                }
                else if (ri.CheckForText(1, 72, "TCX0A"))
                {
                    ri.Hit(Key.Enter);
                }
                else if (ri.CheckForText(1, 74, "TCX13"))
                {
                    //Note screen
                    EnterAcpComments(comment, borrower.SSN, region, borrower.NeedsDeconArc, bankruptcyInfo, acp.Selection.RecipientTarget, borrower.AccountNumber);
                    return;
                }
                else
                {
                    throw new Exception(string.Format("Was expecting TCX02, TCX04, TCX14 or TCX0A and got something else. Session Message:{0}; Borrower:{1}", ri.Message, borrower.AccountNumber));
                }
                ri.Hit(Key.Enter);
            }

            //handle the last final screens of ACP
            //switch keys until 
            if (ri.CheckForText(1, 74, "TCX0D"))
            {

                if (bankruptcyInfo != null)
                {
                    HandleAllegedBankruptcy(bankruptcyInfo, borrower, "TCX0D", region, acp.Selection.RecipientTarget);
                }
                else
                {
                    while (!ri.CheckForText(24, 21, "CMTS"))
                        ri.Hit(Key.F2);
                }
            }
            else if (ri.ScreenCode == "TCX0I" || ri.ScreenCode == "TCX06")
            {
                if (ri.ScreenCode == "TCX0I" && deceasedInfo != null)
                {
                    //deceased information was provided
                    ri.PutText(8, 17, deceasedInfo.ThirdPartyKnowsBorrower);
                    ri.PutText(9, 17, deceasedInfo.SpeakingTo);
                    ri.PutText(11, 17, deceasedInfo.LetterOfPermissionOnFile);
                    ri.PutText(13, 17, "Y", Key.Enter); //3rd party says the borrower is deceased
                    ri.PutText(8, 17, deceasedInfo.DateOfDeath.ToString("MMddyy"));
                    ri.PutText(9, 17, deceasedInfo.DeathOccurredInformation);
                    ri.PutText(11, 17, deceasedInfo.AbleToSendOriginalDeathCertificate);
                    ri.PutText(12, 17, deceasedInfo.ClosestLivingRelativeInformation);
                    ri.PutText(15, 17, deceasedInfo.NameOfFuneralHome, Key.Enter);
                }
                else if (ri.ScreenCode == "TCX06" && bankruptcyInfo != null)
                {
                    HandleAllegedBankruptcy(bankruptcyInfo, borrower, "TCX06", region, acp.Selection.RecipientTarget);
                }
                else
                {
                    while (!ri.CheckForText(24, 29, "CMTS"))
                        ri.Hit(Key.F2);
                }
            }
            if (!ri.CheckForText(1, 74, "TCX13"))
                ri.Hit(Key.F4);

            //enter comments
            EnterAcpComments(comment, borrower.SSN, region, borrower.NeedsDeconArc, bankruptcyInfo, acp.Selection.RecipientTarget, borrower.AccountNumber);

        }

        private void ProcessCornerstone(string comment, IBorrower borrower)
        {
            Tcx22Entry(borrower.AcpResponses.Selection, borrower);
            Tcx27Entry(borrower.DemographicsVerifications);
            if (ri.ScreenCode == "TCX26")
                ri.PutText(20, 15, "02", Key.Enter);
            if (ri.ScreenCode == "TCX28")
                ri.Hit(Key.Enter);
        }

        /// <summary>
        /// Perform all Tcx22 Entries.
        /// </summary>
        private void Tcx22Entry(AcpSelectionResult resultToEnter, IBorrower borrower)
        {
            if (ri.ScreenCode != "TCX22")
                return;
            var ac = resultToEnter.ActivityCodeSelection;
            var cc = resultToEnter.ContactCodeSelection;
            if (ri.GetText(6, 16, 1).Trim() != "") //contact phone needed
                ri.PutText(6, 16, borrower.ContactPhoneNumber);
            if (resultToEnter.CallStatusType == null || resultToEnter.CallStatusType == CallStatusType.CallSuccessful)
            {
                if (cc == ContactCode.FromBorrower || cc == ContactCode.ToBorrower)
                    ri.PutText(8, 3, "X");
                else if (resultToEnter.RecipientTargetReference != null)
                {
                    bool recipientFound = false;
                    while (!recipientFound)
                    {
                        for (int row = 8; row <= 14; row++)
                        {
                            if (ri.CheckForText(row, 11, resultToEnter.RecipientTargetReference.ReferenceId))
                            {
                                ri.PutText(row, 3, "X");
                                recipientFound = true;
                                break;
                            }
                        }
                        if (!recipientFound)
                            ri.Hit(Key.F8);
                    }
                }
                else
                {
                    if (cc == ContactCode.FromGuarantor || cc == ContactCode.ToGuarantor)
                        ri.PutText(16, 3, "X");
                    else if (cc == ContactCode.FromSchool || cc == ContactCode.ToSchool)
                        ri.PutText(16, 25, "X");
                    else if (cc == ContactCode.FromCreditBureau || cc == ContactCode.ToCreditBureau)
                        ri.PutText(16, 39, "X");
                    else if (resultToEnter.RecipientInfo.Authorized)// if (cc == ContactCode.From3rdParty || cc == ContactCode.To3rdParty)
                        ri.PutText(15, 39, "X");
                    else
                        ri.PutText(15, 3, "X");
                    ri.PutText(20, 25, resultToEnter.RecipientInfo.RecipientName);
                    ri.PutText(21, 16, resultToEnter.RecipientInfo.Relationship);
                    ri.PutText(22, 24, resultToEnter.RecipientInfo.ContactPhoneNumber);
                }
            }
            else
            {
                if (resultToEnter.CallStatusType == CallStatusType.AnsweringMachine)
                    ri.PutText(18, 3, "X");
                else if (resultToEnter.CallStatusType == CallStatusType.NoAnswer)
                    ri.PutText(17, 56, "X");
                else if (resultToEnter.CallStatusType == CallStatusType.OutOfService)
                    ri.PutText(17, 3, "X");
                else if (resultToEnter.CallStatusType == CallStatusType.PhoneBusy)
                    ri.PutText(18, 25, "X");
                else if (resultToEnter.CallStatusType == CallStatusType.WrongNumber)
                    ri.PutText(17, 39, "X");
            }
            ri.Hit(Key.Enter);
            if (ri.ScreenCode == "TCX26")
                ri.PutText(20, 15, "02", Key.Enter);
        }
        /// <summary>
        /// Perform all Tcx27 Entries.
        /// </summary>
        private void Tcx27Entry(DemographicVerifications ver)
        {
            while (ri.ScreenCode == "TCX27")
            {
                ri.PutText(9, 2, ver.Address.ToVRN());
                ri.PutText(16, 2, ver.HomePhone.ToVRN());
                ri.PutText(17, 2, ver.OtherPhone.ToVRN());
                ri.PutText(18, 2, ver.OtherPhone2.ToVRN());
                ri.PutText(19, 2, "N"); //Default to a N since other phone 3 has been removed.
                ri.PutText(20, 2, ver.Email.ToVRN());
                ri.Hit(Key.Enter);
            }
        }

        private void ProcessUheaa(string comment, IBorrower borrower)
        {
            var verifications = borrower.DemographicsVerifications;
            //check if Demos screen comes up and handle it if it does
            if (ri.ScreenCode == "TCX05")
            { //address verification screen 
                //enter demographic validation indicators
                ri.PutText(22, 22, "N");
                Action<int, VerificationSelection> set = new Action<int, VerificationSelection>(
                    (col, val) => ri.PutText(22, col, val.IsValid() ? "Y" : "N"));
                set(41, verifications.Address);
                set(60, verifications.HomePhone);
                set(79, verifications.Email);
                ri.Hit(Key.Enter);
                bool keepLooking = true;
                while (keepLooking)
                {
                    if (ri.MessageCode == "88009") //if phone indicator doesn't work then change
                        ri.PutText(22, 60, "N", Key.Enter);
                    else if (ri.Message == "88471") //if email indicator doesn't work then change
                        ri.PutText(22, 79, "N", Key.Enter);
                    else if (ri.ScreenCode == "88005") //if address indicator doesn't work then change
                        ri.PutText(22, 41, "N", Key.Enter);
                    else
                        keepLooking = false;
                }
            }
        }

        /// <summary>
        /// Enters comments into the ACP comments screen
        /// </summary>
        public void EnterAcpComments(string activityComments, string ssn, DataAccessHelper.Region region, bool needsDeconArc, AcpBankruptcyInfo info, CallRecipientTarget? target, string accountNumber)
        {
            if (info != null && info.OfficiallyFiled == "N")
            {
                activityComments += string.Format(" Alleged Bankruptcy, Officially Filed: {0} Attorney Information: {1} EndorserSsn: {2}", info.OfficiallyFiled ?? "", info.AttorneyInformation ?? "", info.EndIdentifier ?? "");
                //if (info.EndIndicator) this was previously commented out in MD
                //{
                //    activityComments += string.Format(" EndorserFiled: {0} EndorserIdentifier: {1}", info.EndIndicator, info.EndIdentifier.Trim());
                //}
            }
            else if(info != null && target.HasValue && target == CallRecipientTarget.Endorser && info.OfficiallyFiled == "Y")
            {
                AddEndorserBankruptcyArc(info, accountNumber, region);
            }

            string message = string.Format("Begginning ACP comments for SSN {0}.  Needs DECON Arc Comment: {1}.  Full comment text: {2}", ssn, needsDeconArc, activityComments);
            ProcessLogger.AddNotification(pld, message, NotificationType.EndOfJob, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly(), null);

            if (needsDeconArc)
                ri.Atd22AllLoans(ssn, "DECON", activityComments, "", "", false);
            else
            {
                if (ri.ScreenCode == "TCX28")
                    ri.Hit(Key.Enter);

                int maxLength = 156;
                if (activityComments.Length > maxLength)
                {
                    ri.PutText(18, 2, activityComments.SafeSubString(0, maxLength), Key.Enter);
                    ri.FastPath("TX3ZITD2A" + ssn);
                    ri.PutText(8, 31, "X");
                    ri.PutText(7, 60, "X");
                    ri.PutText(21, 56, DateTime.Now.ToString("MMddyy"));
                    ri.PutText(21, 70, DateTime.Now.ToString("MMddyy"), Key.Enter);
                    //if selection screen is displayed then select first option
                    if (ri.ScreenCode == "TDX2C")
                        ri.PutText(7, 2, "X", Key.Enter);
                    ri.Hit(Key.F4);
                    //enter the rest on the expanded comments screen
                    for (int i = maxLength; i < activityComments.Length; i += 260)
                        ri.ReflectionSession.TransmitANSI(activityComments.SafeSubString(i, 260));
                    ri.Hit(Key.Enter);
                }
                else
                {
                    ri.PutText(18, 2, activityComments, Key.Enter);
                }
            }
        }

        /// <summary>
        /// Handles incomplete bankruptcy information using arc add and does ACP for complete information
        /// </summary>
        /// <param name="info">bankruptcy info</param>
        /// <param name="PreviousScreenCode">Must provide TCX0D or TCX06</param>
        private void HandleAllegedBankruptcy(AcpBankruptcyInfo bankruptcyInfo, IBorrower borrower, string PreviousScreenCode, DataAccessHelper.Region region, CallRecipientTarget? target)
        {
            if(bankruptcyInfo != null)
            {
                //If all information is present do normal ACP protocol
                if ((bankruptcyInfo.CourtInformation ?? "").Length != 0 && (bankruptcyInfo.AttorneyInformation ?? "").Length != 0 && (bankruptcyInfo.Chapter ?? "").Length != 0 && (bankruptcyInfo.DocketNumber ?? "").Length != 0 && (bankruptcyInfo.FilingDate.Replace("/", "").Trim() ?? "").Length != 0 && !bankruptcyInfo.EndIndicator && (!target.HasValue || target == CallRecipientTarget.Borrower))
                {
                    if (PreviousScreenCode == "TCX0D")
                    {
                        //bankruptcy information was provided
                        ri.PutText(15, 40, "3", Key.Enter); //general 
                        ri.PutText(20, 13, "11", Key.Enter); //claims bankruptcy
                        ri.PutText(7, 17, bankruptcyInfo.OfficiallyFiled);
                        ri.PutText(8, 17, bankruptcyInfo.AttorneyInformation);
                        ri.PutText(10, 17, bankruptcyInfo.CourtInformation);
                        ri.PutText(12, 17, bankruptcyInfo.Chapter.Trim().PadLeft(2, '0'));
                        if (bankruptcyInfo.FilingDate.ToDateNullable().HasValue)
                            ri.PutText(12, 23, bankruptcyInfo.FilingDate.ToDate().ToString("MMddyy"));
                        ri.PutText(13, 17, bankruptcyInfo.DocketNumber, Key.Enter);
                        if (ri.MessageCode == "90002")
                            Dialog.Error.Ok("Bankruptcy info not added.");
                    }
                    if (PreviousScreenCode == "TCX06")
                    {
                        //bankruptcy information was provided
                        ri.PutText(22, 33, "4", Key.Enter); //claims bankruptcy 
                        ri.PutText(7, 17, bankruptcyInfo.OfficiallyFiled);
                        ri.PutText(8, 17, bankruptcyInfo.AttorneyInformation);
                        ri.PutText(10, 17, bankruptcyInfo.CourtInformation);
                        ri.PutText(12, 17, bankruptcyInfo.Chapter.Trim().PadLeft(2, '0'));
                        if (bankruptcyInfo.FilingDate.ToDateNullable().HasValue)
                            ri.PutText(12, 23, bankruptcyInfo.FilingDate.ToDate().ToString("MMddyy"));
                        ri.PutText(13, 17, bankruptcyInfo.DocketNumber, Key.Enter);
                        if (ri.MessageCode == "90002")
                            Dialog.Error.Ok("Bankruptcy info not added.");
                    }
                }
                //Use Arc Add if not all information is present
                else if (bankruptcyInfo.OfficiallyFiled == "Y" && !bankruptcyInfo.EndIndicator && (!target.HasValue || target == CallRecipientTarget.Borrower))
                {
                    ArcData arc = new ArcData(region)
                    {
                        AccountNumber = borrower.AccountNumber.Replace(" ", ""),
                        Arc = "G501A",
                        Comment = "Attorney Information: " + (bankruptcyInfo.AttorneyInformation ?? "") + " Court Information: " + (bankruptcyInfo.CourtInformation ?? "") + " Chapter: " + (bankruptcyInfo.Chapter ?? "").Trim() + " Filing Date: " + (bankruptcyInfo.FilingDate ?? "") + " Docket Number: " + (bankruptcyInfo.DocketNumber ?? ""),
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                        ScriptId = "MD"
                    };
                    ArcAddResults result = arc.AddArc();
                    if (!result.ArcAdded)
                    {
                        Dialog.Error.Ok("Bankruptcy info arc(G501A) not added.");
                    }
                }
                else if (bankruptcyInfo.EndIndicator && (!target.HasValue || target == CallRecipientTarget.Borrower) && bankruptcyInfo.OfficiallyFiled == "Y")
                {
                    ArcData arc = new ArcData(region)
                    {
                        AccountNumber = borrower.AccountNumber.Replace(" ", ""),
                        Arc = "G501A",
                        Comment = "CoBorrower/Endorser claims Bankruptcy, EndorserIdentifier: " + bankruptcyInfo.EndIdentifier + " Attorney Information: " + (bankruptcyInfo.AttorneyInformation ?? "") + " Court Information: " + (bankruptcyInfo.CourtInformation ?? "") + " Chapter: " + (bankruptcyInfo.Chapter ?? "").Trim() + " Filing Date: " + (bankruptcyInfo.FilingDate ?? "") + " Docket Number: " + (bankruptcyInfo.DocketNumber ?? ""),
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                        ScriptId = "MD"
                    };
                    ArcAddResults result = arc.AddArc();
                    if (!result.ArcAdded)
                    {
                        Dialog.Error.Ok("Bankruptcy info arc(G501A) not added.");
                    }
                }
            }
        }

        public void AddEndorserBankruptcyArc(AcpBankruptcyInfo bankruptcyInfo, string accountNumber, DataAccessHelper.Region region)
        {
            ArcData arc = new ArcData(region)
            {
                AccountNumber = accountNumber.Replace(" ", ""),
                Arc = "G501A",
                Comment = "CoBorrower/Endorser claims Bankruptcy, EndorserIdentifier: " + bankruptcyInfo.EndIdentifier + " Attorney Information: " + (bankruptcyInfo.AttorneyInformation ?? "") + " Court Information: " + (bankruptcyInfo.CourtInformation ?? "") + " Chapter: " + (bankruptcyInfo.Chapter ?? "").Trim() + " Filing Date: " + (bankruptcyInfo.FilingDate ?? "") + " Docket Number: " + (bankruptcyInfo.DocketNumber ?? ""),
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                ScriptId = "MD",
                //RecipientId = bankruptcyInfo.EndIdentifier.Replace(" ", "") previously commented out
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                Dialog.Error.Ok("Bankruptcy info arc(G501A) not added.");
            }
        }

    }
}
