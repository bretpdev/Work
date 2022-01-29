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
    class OneLINKProcessor : Processor
    {
        public OneLINKProcessor(ReflectionInterface ri, string scriptID, DataAccess da, ProcessLogRun logRun, PowerOfAttorneyData data)
            : base(ri, scriptID, da, logRun, data)
        { }

        /// <summary>
        /// Checks for existing reference on OneLINK and gathers it if one exists.
        /// </summary>
        public void ReferenceCheckAndGathering()
        {
            LP2CCheck();
            if (POAdata.AddReference == false && RI.CheckForText(1, 65, "REFERENCE SELECT") == false)
            {
                string message = "Is the reference displayed in the session the one we received the Power of Attorny for?";
                if (Question.YesNo(message, "Is This The One"))
                    GetReferenceDemoInfo();
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
            if (!RI.CheckForText(6, 67, "A"))
                return;

            POAdata.RefData.Ssn = RI.GetText(3, 39, 9);
            POAdata.RefData.FirstName = RI.GetText(4, 44, 12);
            POAdata.RefData.LastName = RI.GetText(4, 5, 30);
            POAdata.RefData.MiddleIntial = RI.GetText(4, 60, 1);
            POAdata.RefData.RelationshipToBorrower = GetRelationship();

            if (RI.CheckForText(8, 53, "Y"))
            {
                POAdata.RefData.Address1 = RI.GetText(8, 9, 36);
                POAdata.RefData.Address2 = RI.GetText(9, 9, 36);
                POAdata.RefData.City = RI.GetText(10, 9, 30);
                POAdata.RefData.State = RI.GetText(10, 52, 2);
                POAdata.RefData.ZipCode = RI.GetText(10, 60, 20);
            }
            else
            {
                POAdata.RefData.Address1 = "";
                POAdata.RefData.Address1 = "";
                POAdata.RefData.City = "";
                POAdata.RefData.State = "";
                POAdata.RefData.ZipCode = "";
            }

            if (RI.CheckForText(13, 36, "Y"))
                POAdata.RefData.PrimaryPhone = RI.GetText(13, 16, 10);
            else
                POAdata.RefData.PrimaryPhone = "";

            POAdata.RefData.AlternatePhone = RI.GetText(14, 16, 10);
            POAdata.RefData.ForeignPhone = RI.GetText(15, 16, 17);
            POAdata.RefData.EmailAddress = RI.GetText(17, 9, 40);
        }

        /// <summary>
        /// Determines which relationship is in the session. The session changed to a 2 char relationship
        /// status but not all statuses have been updated. Check to see if the 2 char relationship is used first
        /// and if not, use the single char relationship. This will ensure that any new relationships that might 
        /// have the same first character will use the new 2 char relationship first.
        /// </summary>
        /// <returns>Returns the last relationship found in case there are multiple like Other.</returns>
        private Relationship GetRelationship()
        {
            string sessionRelation = RI.GetText(6, 15, 2);
            if (sessionRelation.Length == 2)
                return Relationships.Where(p => p.OneLinkCode == sessionRelation).Last();
            else if (sessionRelation.Length == 1)
                return Relationships.Where(p => p.OneLinkCode.Substring(0, 1) == sessionRelation).Last();
            return new Relationship();
        }

        /// <summary>
        /// Check reference information on LP2C
        /// </summary>
        private void LP2CCheck()
        {
            RI.FastPath($"LP2CI{POAdata.BorrowerDemos.Ssn}");
            if (RI.AltMessageCode == "47004")
                POAdata.AddReference = true; //Reference does not exist: add it.
            else
            {
                if (RI.CheckForText(1, 65, "REFERENCE SELECT"))
                    HandleMulitpleReferences();
                else
                {
                    if (POAdata.RefData.LastName.StartsWith(RI.GetText(4, 5, 4)) && POAdata.RefData.FirstName.StartsWith(RI.GetText(4, 44, 4)))
                    {
                        //names match
                        POAdata.AddReference = false;
                        RI.FastPath($"LP2CI{POAdata.BorrowerDemos.Ssn}");
                    }
                    else
                        POAdata.AddReference = true;
                }
            }
        }

        /// <summary>
        /// Find active reference by name. Leaves you on the Reference Demographics screen.
        /// </summary>
        private void HandleMulitpleReferences()
        {
            int row = 7;
            int numberFound = 0;
            while (true) //Breaks when two name matches are found.
            {
                if (RI.CheckForText(row, 27, "A"))
                {
                    //if active then check names
                    RI.PutText(21, 13, RI.GetText(row - 1, 3, 1));
                    RI.Hit(Enter);
                    if (POAdata.RefData.LastName.StartsWith(RI.GetText(4, 5, 4)) && POAdata.RefData.FirstName.StartsWith(RI.GetText(4, 44, 4)))
                    {
                        //names match
                        numberFound++;
                        if (numberFound > 1)//This is the second match.
                            break;
                    }
                    RI.Hit(F12);
                }
                row += 3;
                if (row == 22)
                {
                    row = 7;
                    RI.Hit(F8);
                    if (RI.CheckForText(22, 3, "46004"))
                    {
                        //name not found.
                        POAdata.AddReference = true;
                        break;
                    }
                }
            }

            //Loop through again to select the only match.
            RI.FastPath($"LP2CI{POAdata.BorrowerDemos.Ssn}");
            if (numberFound == 1)
            {
                row = 7;
                while (true) //Breaks when a name match is found.
                {
                    if (RI.CheckForText(row, 27, "A"))
                    {
                        //Active--check names.
                        RI.PutText(21, 13, RI.GetText(row - 1, 3, 1), Enter);
                        if (POAdata.RefData.LastName.StartsWith(RI.GetText(4, 5, 4)) && POAdata.RefData.FirstName.StartsWith(RI.GetText(4, 44, 4)))
                        {
                            //names match only name
                            POAdata.AddReference = false;
                            break;
                        }
                        else
                            RI.Hit(F12);
                    }
                    row += 3;
                    if (row == 22)
                    {
                        row = 7;
                        RI.Hit(F8);
                        if (RI.CheckForText(22, 3, "46004"))
                        {
                            //Name not found. This should never happen.
                            LogRun.AddNotification("Name not found of Account: " + POAdata.BorrowerDemos.AccountNumber.ToString(), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                            throw new Exception("A catastrophic error occurred.  Please contact a member of Systems Support.");
                        }
                    }
                }
            }
            else if (numberFound > 1)
            {
                //Allow the user to select the right reference.
                string multipleMessage = "Multiple Names have been found that match your entry.";
                multipleMessage += " Please fix the duplicate reference records and press insert to start the script from the correct Reference Demographics screen.";
                Warning.Ok(multipleMessage, "Multiples Found");
                RI.PauseForInsert();
                if (!RI.CheckForText(1, 59, "REFERENCE DEMOGRAPHICS"))
                {
                    string message = "Script did not find the REFERENCE DEMOGRAPHICS screen. Ending Script.";
                    Error.Ok(message, "Wrong Screen");
                    LogRun.AddNotification(message + " Wrong Screen", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    throw new Exception(message + " Wrong Screen");
                }
                POAdata.AddReference = false;
            }
        }

        /// <summary>
        /// Adds a new reference.
        /// </summary>
        private void AddReference()
        {
            RI.FastPath($"LP2CA{POAdata.BorrowerDemos.Ssn}");
            RI.PutText(4, 5, POAdata.RefData.LastName);
            RI.PutText(4, 44, POAdata.RefData.FirstName);
            RI.PutText(4, 60, POAdata.RefData.MiddleIntial);
            RI.PutText(5, 9, "O");
            RI.PutText(6, 15, POAdata.RefData.RelationshipToBorrower.OneLinkCode);
            RI.PutText(6, 51, (POAdata.RequestApproved ? "Y" : "N"));
            RI.PutText(8, 9, POAdata.RefData.Address1);
            RI.PutText(9, 9, POAdata.RefData.Address2);
            RI.PutText(8, 53, "Y");
            RI.PutText(10, 9, POAdata.RefData.City);
            RI.PutText(10, 52, POAdata.RefData.State);
            RI.PutText(10, 60, POAdata.RefData.ZipCode);
            RI.PutText(13, 16, POAdata.RefData.PrimaryPhone);
            if (POAdata.RefData.PrimaryPhone.IsPopulated())
                RI.PutText(13, 36, "Y");
            RI.PutText(14, 16, POAdata.RefData.AlternatePhone);
            if (POAdata.RefData.AlternatePhone.IsPopulated())
                RI.PutText(14, 36, "Y");
            RI.PutText(15, 16, POAdata.RefData.ForeignPhone);
            RI.PutText(17, 9, POAdata.RefData.EmailAddress);
            RI.Hit(Enter);
            RI.Hit(F6);
            if (!RI.CheckForText(22, 3, "48003 DATA SUCCESSFULLY ADDED"))
            {
                string message = "There was a problem while adding the reference.  Please take a moment and fix it, then press <Insert>.";
                Stop.Ok(message, "POA");
                RI.PauseForInsert();
                RI.Hit(Enter);
                RI.Hit(F6);
            }
            RI.Hit(F11);
            //Enter 'S' next to each loan.
            while (!RI.CheckForText(22, 3, "46004 NO MORE DATA TO DISPLAY"))
            {
                while (RI.GetCurrentCoordinate().Row != 1)//CHANGED SessionCursorLocation().Row != 1) 
                {
                    RI.EnterText("S");
                }
                RI.Hit(Enter);
                RI.Hit(F8);
            }

            string comment = $"Reference Added with Power of Attorney {POAdata.RefData.FirstName} {POAdata.RefData.LastName} {POAdata.ExpirationDate}";
            AddArc("MREFP", "AM", "10", comment);
            if (POAdata.BorrowerDemos.IsValidAddress)
            {
                AddArc(POAdata.RequestApproved ? "MPOAA" : "MPOAD", "LT", "03");
                if (POAdata.RequestApproved)
                    POAdata.PrintApprovalLetter = true;
                else
                    POAdata.PrintDenialLetter = true;
            }
        }

        /// <summary>
        /// Adds a Onelink comment in LP50
        /// </summary>
        public void AddArc(string arc, string type, string contact, string comment = "")
        {
            ArcData arcD = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = POAdata.BorrowerDemos.Ssn,
                ActivityContact = contact,
                ActivityType = type,
                Arc = arc,
                ArcTypeSelected = ArcData.ArcType.OneLINK,
                Comment = comment,
                ScriptId = ScriptID
            };
            ArcAddResults result = arcD.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"There was an error adding an {arc} comment in LP50 for borrower: {POAdata.BorrowerDemos.AccountNumber}.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
                Error.Ok(message, "Error adding LP50 comment");
            }
        }

        /// <summary>
        /// Modify current reference data
        /// </summary>
        private void ModifyReference()
        {
            int row = 7;
            //Find and select the reference.
            RI.FastPath($"LP2CC{POAdata.BorrowerDemos.Ssn}");
            if (RI.AltMessageCode == "47004")
            {
                //Reference does not exist (should not happen).
                LogRun.AddNotification("Reference does not exist Account: " + POAdata.BorrowerDemos.AccountNumber, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                throw new Exception("Reference does not exist");
            }
            else if (RI.CheckForText(1, 65, "REFERENCE SELECT"))
            {
                while (RI.AltMessageCode != "46004")
                {
                    if (RI.CheckForText(row, 27, "A"))
                    {
                        //Active--check names.
                        RI.PutText(21, 13, RI.GetText(row - 1, 3, 1), Enter, true);
                        if (RI.CheckForText(4, 5, POAdata.RefData.LastName.SafeSubString(0, 4)) && RI.CheckForText(4, 44, POAdata.RefData.FirstName.SafeSubString(0, 4)))
                            break; //names match
                        else
                            RI.Hit(F12); //no match
                    }
                    row += 3;
                    if (row == 22)
                    {
                        row = 7;
                        RI.Hit(F8);
                    }
                }
            }
            //Once the demographic record is selected then continue from here.
            RI.Hit(F11);
            row = 6;
            while (RI.AltMessageCode != "46004")
            {
                if (RI.CheckForText(row, 2, "_", "S"))
                {
                    RI.PutText(row, 2, "S");
                    row++;
                }
                else
                {
                    row = 6;
                    RI.Hit(Enter);
                    RI.Hit(F8);
                }
            }
            RI.Hit(Enter);
            RI.Hit(F12);

            if (RI.CheckForText(8, 53, "Y") && POAdata.RequestApproved)
            {
                //3rd party authorization field was updated and address is valid: post comment.
                POAdata.AddMPOAAActionCode = true;
                POAdata.PrintApprovalLetter = true;
            }

            if (POAdata.UserModified)
            {
                RI.PutText(4, 5, POAdata.RefData.LastName, true);
                RI.PutText(4, 44, POAdata.RefData.FirstName, true);
                RI.PutText(4, 60, POAdata.RefData.MiddleIntial);
                RI.PutText(5, 9, "O");
                RI.PutText(6, 15, POAdata.RefData.RelationshipToBorrower.OneLinkCode);

                if (POAdata.UserModifiedAddress)
                {
                    RI.PutText(8, 9, POAdata.RefData.Address1, true);
                    RI.PutText(9, 9, POAdata.RefData.Address2, true);
                    RI.PutText(10, 9, POAdata.RefData.City, true);
                    RI.PutText(10, 52, POAdata.RefData.State, true);
                    RI.PutText(10, 60, POAdata.RefData.ZipCode, true);
                    RI.PutText(8, 53, "Y");
                }
                RI.PutText(6, 51, (POAdata.RequestApproved ? "Y" : "N"));
                if (POAdata.UserModifiedHomePhoneNumber)
                {
                    RI.PutText(13, 16, POAdata.RefData.PrimaryPhone);
                    RI.PutText(13, 36, "Y");
                }
                RI.PutText(14, 16, POAdata.RefData.AlternatePhone);
                if (POAdata.RefData.AlternatePhone != string.Empty)
                    RI.PutText(14, 36, "Y");
                RI.PutText(15, 16, POAdata.RefData.ForeignPhone);
                RI.PutText(17, 9, POAdata.RefData.EmailAddress);
                RI.Hit(Enter);
                RI.Hit(F6);
                if (!RI.CheckForText(22, 3, "49000", "40639"))
                {
                    string message = "There is a problem posting the record. Please fix the data and press Insert when done.";
                    Warning.Ok(message, "Posting Problem");
                    RI.PauseForInsert();
                }
                if (POAdata.RequestApproved == false && RI.CheckForText(8, 53, "Y"))
                {
                    //3rd party authorization field was not approved and address is valid: post comment.
                    POAdata.AddMPOADActionCode = true;
                    POAdata.PrintDenialLetter = true;
                }
            }
            else
            {
                if (POAdata.RequestApproved)
                {
                    //3rd party authorization field was updated and address is valid: post comment.
                    POAdata.AddMPOAAActionCode = true;
                    RI.PutText(6, 51, "Y");
                    RI.PutText(8, 53, "Y");
                    RI.Hit(Enter);
                    RI.Hit(F6);
                }
                else if (RI.CheckForText(8, 53, "Y"))
                {
                    //3rd party authorization field was not approved and address is valid: post comment.
                    POAdata.AddMPOADActionCode = true;
                }
            }
        }

        /// <summary>
        /// Setup for add or modify
        /// </summary>
        public void CoordinateReferenceAdditionOrModification()
        {
            if (POAdata.AddReference)
                AddReference();
            else
                ModifyReference();

            if (POAdata.BorrowerDemos.IsValidAddress)
            {
                if (POAdata.AddMPOAAActionCode && POAdata.RequestApproved)
                    AddArc("MPOAA", "LT", "03");
                if (POAdata.AddMPOADActionCode && !POAdata.RequestApproved)
                    AddArc("MPOAD", "LT", "03");
            }
        }
    }
}