using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;
using static Uheaa.Common.Dialog;

namespace PWRATRNY
{
    class CompassProcessor : Processor
    {
        public CompassProcessor(ReflectionInterface ri, string scriptID, DataAccess da, ProcessLogRun logRun, PowerOfAttorneyData data)
            : base(ri, scriptID, da, logRun, data)
        { }

        /// <summary>
        /// Send user a message if they cant leave the arc required.
        /// </summary>
        /// <param name="arc">5 digit arc code</param>
        /// <param name="comment">Comment to leave.</param>
        private void UserDoesNotHaveAccessToNeededARC(string arc, string comment)
        {
            string message = $"You don't have access to the {0} ARC, or there was an error adding the {arc} ARC to the borrower's account." +
                "  If you believe you do not have access to the necessary ARCs please contact manager to submit access for 'MREFP', 'MPOAA' and 'MPOAD' ARCs.";
            Warning.Ok(message, "Need Access To ARC(s)");
            RI.FastPath($"TX3Z/ATC00{POAdata.BorrowerDemos.Ssn}");
            RI.PutText(19, 38, "4", Enter);
            RI.PutText(12, 10, comment, Enter);
        }

        /// <summary>
        /// Add a new reference to the account.
        /// </summary>
        private void AddReference()
        {
            RI.FastPath("TX3Z/ATX1JR;;;");
            RI.PutText(4, 6, POAdata.RefData.LastName);
            RI.PutText(4, 34, POAdata.RefData.FirstName);
            RI.PutText(4, 53, POAdata.RefData.MiddleIntial);
            RI.PutText(7, 11, POAdata.BorrowerDemos.Ssn);
            RI.PutText(8, 49, POAdata.RequestApproved ? "Y" : "N");
            RI.PutText(8, 15, POAdata.RefData.RelationshipToBorrower.CompassCode);
            RI.PutText(11, 10, POAdata.RefData.Address1);
            RI.PutText(12, 10, POAdata.RefData.Address2);
            RI.PutText(11, 55, "Y");
            RI.PutText(14, 8, POAdata.RefData.City);
            RI.PutText(14, 32, POAdata.RefData.State);
            RI.PutText(14, 40, POAdata.RefData.ZipCode);
            RI.PutText(16, 20, "U");
            RI.PutText(16, 30, "N");
            RI.PutText(17, 14, POAdata.RefData.PrimaryPhone);
            if (POAdata.RefData.PrimaryPhone.IsPopulated())
                RI.PutText(17, 54, "Y");
            RI.PutText(18, 15, POAdata.RefData.ForeignPhone);
            RI.PutText(19, 14, "43");
            RI.Hit(Enter);
            if (RI.CheckForText(23, 2, "90003 THIS FIELD MUST NOT BE LEFT BLANK - DATA MUST BE ENTERED"))
                RI.PutText(8, 33, $"{DateTime.Today:MMddyy}", Enter);
            if (RI.CheckForText(23, 2, "01079 POSSIBLE DUPLICATE EXISTS"))
                RI.Hit(Enter);

            if (RI.MessageCode != "01004")
            {
                string message = "There was a problem while adding the reference.";
                message += "  Please take a moment and fix it, then press <Insert>.";
                Stop.Ok(message, "POA");
                RI.PauseForInsert();
                RI.Hit(Enter);
            }

            if (POAdata.RefData.EmailAddress.IsPopulated())
            {
                RI.Hit(F2);
                RI.Hit(F10);
                if (!RI.CheckForText(10, 14, "H"))
                    RI.PutText(10, 14, "H");
                RI.PutText(14, 10, "", true);
                RI.PutText(15, 10, "", true);
                RI.PutText(16, 10, "", true);
                RI.PutText(17, 10, "", true);
                RI.PutText(18, 10, "", true);
                RI.PutText(14, 10, POAdata.RefData.EmailAddress);
                RI.PutText(11, 17, $"{DateTime.Now:MMddyy}");
                RI.PutText(12, 14, "Y");
                RI.PutText(9, 20, POAdata.RefData.RelationshipToBorrower.CompassCode);
                RI.Hit(Enter);

                if (RI.MessageCode != "01004")
                {
                    Stop.Ok("There was a problem while adding the reference.  Please take a moment and fix it, then press <Insert>.", "POA");
                    RI.PauseForInsert();
                    RI.Hit(Enter);
                }
                //Go back one screen and switch the hotkeys back.
                RI.Hit(F12);
                RI.Hit(F2);
            }

            if (POAdata.RefData.AlternatePhone.IsPopulated())
            {
                RI.Hit(F6, 3);
                RI.PutText(16, 14, "A", Enter);
                RI.Hit(Enter);
                RI.PutText(16, 20, "U");
                RI.PutText(16, 30, "N");
                RI.PutText(17, 14, POAdata.RefData.AlternatePhone);
                RI.PutText(19, 14, "43");
                RI.PutText(17, 54, "Y");
                RI.Hit(Enter);

                if (RI.MessageCode != "01100")
                {
                    string message = "There was a problem while adding the reference.";
                    message += "  Please take a moment and fix it, then press <Insert>.";
                    Stop.Ok(message, "POA");
                    RI.PauseForInsert();
                    RI.Hit(Enter);
                }
            }

            string comment = $"Reference added with Power of Attorney {POAdata.RefData.FirstName} {POAdata.RefData.LastName} Expiration: {(POAdata.ExpirationDate ?? "")}";
            if (!AddArc("MREFP", comment))
                UserDoesNotHaveAccessToNeededARC("MREFP", comment);

            if (POAdata.BorrowerDemos.IsValidAddress)
            {
                if (POAdata.RequestApproved)
                {
                    if (!AddArc("MPOAA"))
                        UserDoesNotHaveAccessToNeededARC("MPOAA", "Approved Power Of Attorney");
                    POAdata.PrintApprovalLetter = true;
                }
                else
                {
                    if (!AddArc("MPOAD"))
                        UserDoesNotHaveAccessToNeededARC("MPOAD", "Denied Power Of Attorney");
                    POAdata.PrintDenialLetter = true;
                }
            }
        }

        /// <summary>
        /// Adds Atd22AllLoans ARC to borrower account
        /// </summary>
        private bool AddArc(string arc, string comment = "")
        {
            ArcData arcD = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = POAdata.BorrowerDemos.Ssn,
                Arc = arc,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                ScriptId = ScriptID
            };
            ArcAddResults result = arcD.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"There was an error adding an {arc} ARC to borrower: {POAdata.BorrowerDemos.AccountNumber}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
                Error.Ok(message);
            }
            return result.ArcAdded;
        }

        /// <summary>
        /// Change an existing reference.
        /// </summary>
        private void ModifyReference()
        {
            RI.PutText(1, 4, "C", Enter);

            if (POAdata.UserModifiedReferenceName)
            {
                RI.PutText(4, 6, POAdata.RefData.LastName);
                RI.PutText(4, 34, POAdata.RefData.FirstName);
                RI.PutText(4, 53, POAdata.RefData.MiddleIntial);
                RI.PutText(3, 61, $"{DateTime.Today:MMddyy}", Enter);
            }

            RI.Hit(F6);
            if (POAdata.UserModifiedRelationship)
                RI.PutText(8, 15, POAdata.RefData.RelationshipToBorrower.CompassCode);
            string oAuth = RI.GetText(8, 49, 1);
            bool cmodAuth = false;
            if (POAdata.RequestApproved)
            {
                cmodAuth = (oAuth != "Y");
                POAdata.PrintApprovalLetter = true;
                RI.PutText(8, 49, "Y");
                RI.PutText(8, 33, $"{DateTime.Today:MMddyy}");
            }
            else if (oAuth != "N")
            {
                RI.PutText(8, 49, "N");
                POAdata.PrintDenialLetter = true;
            }
            RI.Hit(Enter);

            RI.Hit(F6);
            if (POAdata.UserModifiedAddress)
            {
                RI.PutText(11, 10, POAdata.RefData.Address1, true);
                RI.PutText(12, 10, POAdata.RefData.Address2, true);
                RI.PutText(14, 8, POAdata.RefData.City, true);
                RI.PutText(14, 32, POAdata.RefData.State, true);
                RI.PutText(14, 40, POAdata.RefData.ZipCode, true);
                RI.PutText(11, 55, "Y");
                RI.PutText(10, 32, $"{DateTime.Today:MMddyy}", Enter);
            }
            if (!RI.CheckForText(11, 55, "Y"))
                cmodAuth = false;

            RI.Hit(F6);
            if (POAdata.UserModifiedHomePhoneNumber)
            {
                RI.PutText(16, 20, "U");
                RI.PutText(16, 30, "N");
                RI.PutText(17, 14, POAdata.RefData.PrimaryPhone);
                RI.PutText(17, 54, "Y");
                RI.PutText(16, 45, $"{DateTime.Today:MMddyy}");
                RI.PutText(19, 14, "43");
                RI.Hit(Enter);
            }
            if (POAdata.UserModifiedAltPhone)
            {
                RI.PutText(16, 14, "A");
                RI.Hit(Enter);
                RI.PutText(16, 20, "U");
                RI.PutText(16, 30, "N");
                RI.PutText(17, 14, POAdata.RefData.AlternatePhone);
                RI.PutText(19, 14, "43");
                RI.PutText(17, 54, "Y");
                RI.PutText(16, 45, $"{DateTime.Today:MMddyy}");
                RI.Hit(Enter);
            }
            if (POAdata.UserModifiedForeignPhone)
            {
                RI.PutText(16, 14, "A");
                RI.Hit(Enter);
                RI.PutText(16, 20, "U");
                RI.PutText(16, 30, "N");
                RI.PutText(16, 45, $"{DateTime.Today:MMddyy}");
                RI.Hit(Tab);
                RI.Hit(EndKey);
                RI.Hit(Tab);
                RI.Hit(EndKey);
                RI.Hit(Tab);
                RI.Hit(EndKey);
                RI.PutText(18, 15, POAdata.RefData.ForeignPhone);
                RI.PutText(19, 14, "43");
                RI.PutText(17, 54, "Y");
                RI.Hit(Enter);
            }
            if (POAdata.UserModifiedEmail)
            {
                RI.Hit(F2);
                RI.Hit(F10);
                if (!RI.CheckForText(10, 14, "H"))
                    RI.PutText(10, 14, "H", Enter);
                RI.PutText(9, 20, POAdata.RefData.RelationshipToBorrower.CompassCode);
                RI.PutText(11, 17, $"{DateTime.Today:MMddyy}");
                RI.PutText(12, 14, "Y");
                RI.PutText(14, 10, "", true);
                RI.PutText(15, 10, "", true);
                RI.PutText(16, 10, "", true);
                RI.PutText(17, 10, "", true);
                RI.PutText(18, 10, "", true);
                RI.PutText(14, 10, POAdata.RefData.EmailAddress);
                RI.Hit(Enter);
                RI.Hit(F12);
                RI.Hit(F2);
            }

            //Add comments.
            if (POAdata.BorrowerDemos.IsValidAddress)
            {
                if (cmodAuth)
                {
                    if (!AddArc("MPOAA"))
                        UserDoesNotHaveAccessToNeededARC("MPOAA", "Approved Power Of Attorney");
                }
                else if (POAdata.RequestApproved == false)
                {
                    if (!AddArc("MPOAD"))
                        UserDoesNotHaveAccessToNeededARC("MPOAD", "Denied Power Of Attorney");
                }
            }
        }

        /// <summary>
        /// Set up for add or modify
        /// </summary>
        public void CoordinateReferenceAdditionOrModification()
        {
            RI.FastPath($"TX3Z/ITS24{POAdata.BorrowerDemos.Ssn}");
            if (!RI.CheckForText(1, 76, "TSX25"))
                return; //Borrower isn't on COMPASS.

            //Figure out home many matches there are.
            int foundCount = FindCompassReferenceCount(POAdata.BorrowerDemos.Ssn, POAdata.RefData.FirstName, POAdata.RefData.LastName, false);
            if (foundCount > 1)
            {
                string duplicateMessage = "More than one possible reference record was found for this name.";
                duplicateMessage += " Please correct the duplicate record and press Insert from the correct Reference Demographics record.";
                Warning.Ok(duplicateMessage, "Duplicate Record Fouond");
                RI.PauseForInsert();
                while (!RI.CheckForText(1, 71, "TXX1R"))
                {
                    string failMessage = "This is not a demographics record. Try again and press Insert when done.";
                    if (Warning.OkCancel(failMessage, "Wrong Screen"))
                    {
                        LogRun.AddNotification(failMessage + " WrongScreen", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        throw new Exception("Bad screen reached");
                    }
                    RI.PauseForInsert();
                }
            }
            if (foundCount == 1) //Find one match again and target.
                foundCount = FindCompassReferenceCount(POAdata.BorrowerDemos.Ssn, POAdata.RefData.FirstName, POAdata.RefData.LastName, true);
            if (foundCount == 0)
                AddReference();
            else
            {
                string message = "Is this the record you received Power of Attorney for?";
                if (Question.YesNo(message, "POA"))
                    ModifyReference();
                else
                    AddReference();
            }
        }

        /// <summary>
        /// Determines whether the SSN has a loan with a balance
        /// </summary>
        /// <param name="ssn">borrower</param>
        /// <returns>true or false</returns>
        public bool HasLoanBalance(string ssn)
        {
            RI.FastPath($"TX3Z/ITS2O{ssn}");
            if (RI.MessageCode.IsIn("01527", "50108"))
                return false;
            RI.PutText(7, 26, $"{DateTime.Today:MMddyy}"); //Requested payoff date
            RI.PutText(9, 16, "X"); //Select all
            RI.PutText(9, 54, "Y"); //Include outstanding late fees
            RI.Hit(Enter);
            RI.Hit(Enter);
            return (RI.GetText(12, 29, 10).ToDouble() > 0);
        }

        /// <summary>
        /// Finds the number of references for the borrower
        /// </summary>
        /// <param name="borrowerSsn">Social to search for</param>
        private int FindCompassReferenceCount(string borrowerSsn, string referenceFirstName, string referenceLastName, bool selectFirstMatch)
        {
            RI.FastPath($"TX3Z/ITX1J;{borrowerSsn}");
            RI.Hit(F2);
            RI.Hit(F4);
            int foundCount = 0;
            int limit = (selectFirstMatch ? 1 : 2);
            int row = 10;
            while (RI.MessageCode != "90007" && foundCount < limit)
            {
                if (!RI.CheckForText(row, 3, " "))
                {
                    if (RI.CheckForText(row, 5, "R") && RI.CheckForText(row, 78, "A"))
                    {
                        string longName = RI.GetText(row, 23, 30);
                        string[] name = longName.Split(',');
                        if (name[0] == referenceLastName && name[1].StartsWith(referenceFirstName.SafeSubString(0, 4)))
                        {
                            foundCount++;
                            if (selectFirstMatch)
                                RI.PutText(22, 12, RI.GetText(row, 2, 2), Enter);
                        }
                    }
                }
                else
                {
                    //Go to the next page.
                    RI.Hit(F8);
                    row = 10;
                }
                row++;
            }
            return foundCount;
        }

        /// <summary>
		/// Checks for existing reference on OneLINK and gathers it if one exists.
		/// </summary>
		public void ReferenceCheckAndGathering()
        {
            ITX1JCheck();
            if (POAdata.AddReference == false && RI.CheckForText(2, 33, "REFERENCE DEMOGRAPHICS"))
            {
                string message = "Is the reference displayed we received the Power of Attorney for?";
                if (Question.YesNo(message, "Is This The One"))
                    GetReferenceDemoInfo();
            }
        }

        public void ITX1JCheck()
        {
            CompassPath($"ITX1J;{POAdata.BorrowerDemos.Ssn}");
            if (RI.MessageCode == "01019")
            {
                //Borrower does not exist
                LogRun.AddNotification($"Borrower does not exist in ITX1J: {POAdata.BorrowerDemos.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                throw new Exception($"Borrower does not exist in ITX1J: {POAdata.BorrowerDemos.AccountNumber}");
            }
            else
            {
                RI.Hit(F2);
                RI.Hit(F4);
                if (RI.CheckForText(2, 30, "BORROWER RELATIONSHIP"))
                    HandleReferencePaging();
                else
                    POAdata.AddReference = true;
            }
        }

        /// <summary>
		/// Grab reference demographics.
		/// </summary>
		private void GetReferenceDemoInfo()
        {
            //gets demographic information for reference.
            if (!RI.CheckForText(7, 66, "A"))
                return;

            POAdata.RefData.Ssn = RI.GetText(7, 11, 11).Replace(" ", "");
            POAdata.RefData.FirstName = RI.GetText(4, 34, 12).Replace("_", "");
            POAdata.RefData.LastName = RI.GetText(4, 6, 22).Replace("_", "");
            POAdata.RefData.MiddleIntial = RI.GetText(4, 53, 1).Replace("_", "");
            //Check if there is a match in the list.
            List<Relationship> tempRelationships = (from r in Relationships
                                                    where RI.CheckForText(8, 15, r.CompassCode)
                                                    select r).ToList();
            //If a match is found then use it; otherwise, leave the default value.
            if (tempRelationships.Count != 0)
            {
                //Choose the last because there are mulitple records associated with OT
                //and the last one is Other which is the most correct response at this point.
                POAdata.RefData.RelationshipToBorrower = tempRelationships.Last();
            }

            if (RI.CheckForText(11, 55, "Y"))
            {
                POAdata.RefData.Address1 = RI.GetText(11, 10, 30).Replace(" ", "");
                POAdata.RefData.Address2 = RI.GetText(12, 10, 30).Replace(" ", "");
                POAdata.RefData.City = RI.GetText(14, 8, 20).Replace(" ", "");
                POAdata.RefData.State = RI.GetText(14, 32, 2).Replace(" ", "");
                POAdata.RefData.ZipCode = RI.GetText(14, 40, 16).Replace(" ", "");
            }
            else
            {
                POAdata.RefData.Address1 = "";
                POAdata.RefData.Address1 = "";
                POAdata.RefData.City = "";
                POAdata.RefData.State = "";
                POAdata.RefData.ZipCode = "";
            }

            if (RI.CheckForText(17, 54, "Y"))
                POAdata.RefData.PrimaryPhone = RI.GetText(17, 14, 3) + RI.GetText(17, 23, 3) + RI.GetText(17, 31, 4);
            else
                POAdata.RefData.PrimaryPhone = "";

            POAdata.RefData.AlternatePhone = "";
            POAdata.RefData.ForeignPhone = (RI.GetText(18, 15, 3) + RI.GetText(18, 24, 4) + RI.GetText(18, 36, 11)).Replace(" ", "").Replace("_", "");
            RI.Hit(F2);
            RI.Hit(F10);
            POAdata.RefData.EmailAddress = $"{RI.GetText(14, 10, 60).Replace("_", "")}{RI.GetText(15, 10, 60).Replace("_", "")}";
        }

        /// <summary>
		/// Find active reference by name. Leaves you on the Reference Demographics screen.
		/// </summary>
		private void HandleReferencePaging()
        {
            int row = 10;
            while (true) //Breaks when a match is found.
            {
                if (RI.CheckForText(row, 78, "A"))
                {
                    //if active then check names
                    RI.PutText(22, 12, RI.GetText(row, 2, 2).TrimRight(" "), true);
                    RI.Hit(Enter);
                    if (POAdata.RefData.LastName.StartsWith(RI.GetText(4, 6, 4)) && POAdata.RefData.FirstName.StartsWith(RI.GetText(4, 34, 4)))
                    {
                        //names match
                        POAdata.AddReference = false;
                        return;
                    }
                    RI.Hit(F12);
                }
                row++;
                if (row == 22)
                {
                    row = 7;
                    RI.Hit(F8);
                    if (RI.CheckForText(23, 2, "90007"))
                    {
                        //name not found.
                        POAdata.AddReference = true;
                        return;
                    }
                }
            }
        }

        public void CompassPath(string input)
        {
            RI.Hit(Clear);
            RI.PutText(1, 1, input, F1, false);
        }
    }
}