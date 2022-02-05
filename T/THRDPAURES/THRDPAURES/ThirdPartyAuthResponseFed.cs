using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;

namespace THRDPAURES
{
    public class ThirdPartyAuthResponseFed : FedScript
    {
        private bool ContinueProcessing { get; set; }
        private string TaskControlNumber { get; set; }

        public ThirdPartyAuthResponseFed(ReflectionInterface ri)
            : base(ri, "THRDPAURES")
        {
            ContinueProcessing = true;
        }

        public override void Main()
        {
            if (!VerifyArcAccess())
                EndDllScript();

            do
            {
                BorrowerData data = null;
                while (data == null)
                    data = BorrowerSelection();

                CheckForExistingQueueTask(data.Ssn, data.IsPowerOfAttorney);
                AccessReferenceScreen(data.Ssn);
                bool isNewReference = false;
                ReferencesDemographics referenceDemo = CheckForPossibleReferences(data.ReferenceFirstName, data.ReferenceLastName);

                if (referenceDemo == null)
                {
                    referenceDemo = new ReferencesDemographics(data.ReferenceFirstName, data.ReferenceLastName);
                    isNewReference = true;
                }

                bool isApproved = VerifyUserInput(data.IsPowerOfAttorney);

                referenceDemo = DisplayReferenceData(referenceDemo, data, isNewReference, isApproved);

                CommentsAndLetters clHelper = new CommentsAndLetters(RI, ProcessLogData);
                clHelper.AddComment(data.AccountNumber, referenceDemo, data.ExpirationDate, isApproved, data.IsPowerOfAttorney, isNewReference);
                clHelper.PrintLetter(data, isApproved, data.IsPowerOfAttorney, UserId, ScriptId, ProcessLogData.ProcessLogId);
                CloseQueueTask(data.IsPowerOfAttorney);
            }
            while (MessageBox.Show("Processing complete, would you like to add another reference record?", "", MessageBoxButtons.YesNo) == DialogResult.Yes);
            EndDllScript();
        }

        /// <summary>
        /// Displays the form so a user can modify a references information.
        /// </summary>
        /// <param name="referenceDemo">References Information.</param>
        /// <param name="bData">Borrowers Information.</param>
        /// <param name="isNewReference">New reference indicator.</param>
        /// <param name="isApproved">Approved indicator.</param>
        /// <returns>ReferenceDemographics object with all of the references information.</returns>
        private ReferencesDemographics DisplayReferenceData(ReferencesDemographics referenceDemo, BorrowerData bData, bool isNewReference, bool isApproved)
        {

            using (AddModifyReferencesDemos d = new AddModifyReferencesDemos(referenceDemo, bData))
            {
                string referenceId = string.Empty;
                do
                {
                    if (d.ShowDialog() == DialogResult.Cancel)
                        EndDllScript();

                    referenceId = AddTheReference(bData, d.RefDemos, isNewReference, isApproved);
                }
                while (referenceId.IsNullOrEmpty());

                if (!bData.ExpirationDate.IsNullOrEmpty())
                    AddFutureDatedArc(d.RefDemos, bData, referenceId);

                return d.RefDemos;
            }
        }

        /// <summary>
        /// Adds the reference to the system.
        /// </summary>
        /// <param name="data">Borrowers Information.</param>
        /// <param name="refDemos">References Information.</param>
        /// <param name="isNewReference">New reference indicator.</param>
        /// <param name="isApproved">Approved indicator.</param>
        /// <returns>References Id</returns>
        private string AddTheReference(BorrowerData data, ReferencesDemographics refDemos, bool isNewReference, bool isApproved)
        {
            ReferenceToSystem systemHelper = new ReferenceToSystem(RI);
            if (isNewReference)
                return systemHelper.AddTheReference(refDemos, data.ExpirationDate, data.Ssn, isApproved);
            else
            {
                AccessReferenceScreen(data.Ssn);
                List<PossibleReferenceLocations> possibleLocations = LocatePossibleReferences(refDemos.FirstName, refDemos.LastName);

                if (possibleLocations.Count == 0)
                    return systemHelper.AddTheReference(refDemos, data.ExpirationDate, data.Ssn, isApproved);
                else if (possibleLocations.Count == 1)
                {
                    RI.FastPath("TX3Z/ITX1JB" + data.Ssn);
                    RI.Hit(ReflectionInterface.Key.F2);
                    RI.Hit(ReflectionInterface.Key.F4);
                    LocateReferenceByPageAndSelection(possibleLocations.ElementAt(0).Page, possibleLocations.ElementAt(0).Selection);
                    return systemHelper.UpdateTheReference(refDemos, data.ExpirationDate, data.Ssn, isApproved);
                }
                else
                {
                    if (!refDemos.Id.IsNullOrEmpty())
                        LocateReferenceById(refDemos.Id);

                    while (!RI.CheckForText(1, 71, "TXX1R-03"))
                    {
                        ShowMultipleReferences(possibleLocations);
                        MessageBox.Show("You must be on the reference demographic screen to continue.  Locate reference and press insert.");
                        RI.PauseForInsert();
                    }

                    return systemHelper.UpdateTheReference(refDemos, data.ExpirationDate, data.Ssn, isApproved);
                }
            }
        }

        private void LocateReferenceById(string id)
        {
            while (!RI.CheckForText(2, 72, "1"))
                RI.Hit(ReflectionInterface.Key.F7);

            RI.Hit(ReflectionInterface.Key.F5);
            //At this point we should be on the last screen for references, we will now traverse back to locate the reference
            for (int row = 10; !(RI.MessageCode == "90007"); row++)
            {
                if (row > 21)
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 9;
                    continue;
                }

                if (RI.GetText(row, 13, 9) == id)
                {
                    RI.PutText(22, 12, RI.GetText(row, 2, 2), ReflectionInterface.Key.Enter, true);
                    break;
                }
            }
        }

        /// <summary>
        /// Closes the queue task for the borrower.
        /// </summary>
        /// <param name="isPowerOfAttorney">Indicator if request was for Power Of Attorney</param>
        private void CloseQueueTask(bool isPowerOfAttorney)
        {
            string queueToCheck = isPowerOfAttorney ? "A3" : "T3";
            RI.FastPath("TX3Z/ITX6X");
            RI.PutText(6, 37, queueToCheck);
            RI.PutText(8, 37, "01");
            RI.PutText(10, 37, TaskControlNumber, ReflectionInterface.Key.Enter, true);
            RI.PutText(21, 18, "01");
            RI.Hit(ReflectionInterface.Key.F2);
            RI.PutText(8, 19, "C");
            RI.PutText(9, 19, "COMPL");
            RI.Hit(ReflectionInterface.Key.Enter);
        }

        /// <summary>
        /// Adds a record into the Arc Add DB
        /// </summary>
        /// <param name="refDemos">Reference Information</param>
        /// <param name="bData">Borrower Information</param>
        /// <param name="referenceId">Reference Id</param>
        [UsesSproc(DataAccessHelper.Database.Cls, "ArcAdd_AddRecord")]
        private void AddFutureDatedArc(ReferencesDemographics refDemos, BorrowerData bData, string referenceId)
        {
            ArcData arc = new ArcData()
            {
                AccountNumber = bData.AccountNumber,
                Arc = "EXTPA",
                DelinquencyArc = "",
                ArcType = ArcAddHelper.ArcType.Atd22ByBalance,
                Comment = string.Format("{0} {1} Check POA Documentation in Imaging.", refDemos.FirstName, refDemos.LastName),
                IsEndorser = false,
                IsReference = false,
                LoanPrograms = null,
                LoanSequences = null,
                NeedBy = null,
                PauseForManualComments = false,
                ProcessFrom = null,
                ProcessOn = bData.ExpirationDate.Insert(2, "-").Insert(5, "-").ToDate(),
                ProcessTo = null,
                RecipientId = null,
                RegardsCode = null,
                RegardsTo = "",
                ScriptId = ScriptId
            };

            ArcAddHelper.AddArc(arc, DataAccessHelper.Region.CornerStone);
        }

        /// <summary>
        /// Shows the user a message of possible references
        /// </summary>
        /// <param name="possibleLocations">Dictionary with the page and location of the references</param>
        private void ShowMultipleReferences(List<PossibleReferenceLocations> possibleLocations)
        {
            string referenceMessage = string.Empty;
            foreach (PossibleReferenceLocations val in possibleLocations)
                referenceMessage += string.Format("Page: {0}, Selection: {1} \n", val.Page, val.Selection);

            MessageBox.Show("Multiple possible references found, please review the following references and select the reference and press insert to continue: \n" + referenceMessage
                + "If the possible references do not match press insert from TXX1Y selection screen", "Multiple References found", MessageBoxButtons.OK);

            RI.PauseForInsert();
        }

        /// <summary>
        /// Shows the AccountEntry form
        /// </summary>
        /// <returns>Borrower Data object.</returns>
        private BorrowerData BorrowerSelection()
        {
            BorrowerData data = null;
            using (AccountEntry mode = new AccountEntry())
            {
                if (mode.ShowDialog() == DialogResult.Cancel)
                    EndDllScript();

                data = GetBorrowerDemographics(mode.BorrowerIdentifier);

                if (data == null)
                {
                    MessageBox.Show("Invalid Account Number or SSN, please try again.", "Invalid Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                data.ReferenceFirstName = mode.ReferenceFirstName;
                data.ReferenceLastName = mode.ReferenceLastName;
                data.IsPowerOfAttorney = mode.IsPowerOfAttorney;
                data.ExpirationDate = mode.PoaExpirationDate;
            }
            return data;
        }

        /// <summary>
        /// Verifies that the options the user has chosen are correct.
        /// </summary>
        /// <param name="isPowerOfAttorney">Indicator if request was for Power Of Attorney.</param>
        /// <returns>bool is the request has been approved.</returns>
        private bool VerifyUserInput(bool isPowerOfAttorney)
        {
            string type = isPowerOfAttorney ? "Power of Attorney" : "Third Party Authorization";
            string approvalMessage = string.Format("Has the {0} request been approved?", type);

            bool approved = false;

            if (!isPowerOfAttorney)
                approved = MessageBox.Show(approvalMessage, "Approved?", MessageBoxButtons.YesNo) == DialogResult.Yes;
            else
                approved = MessageBox.Show("Has the Power of Attorney been approved and has a phone number, or address been provided for the Power of Attorney?  If a phone number or address has not be provided then the request cannot be approved", "Valid Demos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

            string approvalResponse = string.Format("The {0} has been {1}, an activity comment and letter will be sent to the borrower.  Press OK to continue or Cancel the end the script",
                type, approved ? "approved" : "denied");

            if (MessageBox.Show(approvalResponse, "", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                EndDllScript();

            return approved;
        }

        /// <summary>
        /// Access a borrowers reference screen.
        /// </summary>
        /// <param name="ssn">Borrowers Ssn</param>
        private void AccessReferenceScreen(string ssn)
        {
            RI.FastPath("TX3Z/ITS26*");
            RI.PutText(8, 40, ssn, ReflectionInterface.Key.Enter, true);

            if (RI.ScreenCode == "TSX28")
            {
                decimal balance = 0;
                //Search for loans until we find one with a balance.
                for (int row = 8; RI.MessageCode != "90007"; row++)
                {
                    if (row > 20)
                    {
                        row = 7;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }

                    if (decimal.TryParse(RI.GetText(row, 59, 11), out balance) && balance > 0)
                    {
                        RI.PutText(21, 12, RI.GetText(row, 2, 2), ReflectionInterface.Key.Enter, true);
                        break;
                    }
                }

                if (balance == 0)
                {
                    MessageBox.Show("Unable to find a loan with a balance.  The script will now end.", "No Balance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //We do not need to end process logger it will be done in the method below.
                    EndDllScript();
                }
            }
            RI.Hit(ReflectionInterface.Key.F4);
        }

        /// <summary>
        /// Checks a borrowers account for possible references so we do not have to add a new one.
        /// </summary>
        /// <param name="referenceFirstName">References First Name.</param>
        /// <param name="referenceLastName">Reference Last Name.</param>
        /// <returns>ReferenceDemographics object if a matching reference is found.</returns>
        private ReferencesDemographics CheckForPossibleReferences(string referenceFirstName, string referenceLastName)
        {
            List<PossibleReferenceLocations> possibleReferenceLocation = LocatePossibleReferences(referenceFirstName, referenceLastName);

            if (possibleReferenceLocation.Count == 0)
                return null;
            else if (possibleReferenceLocation.Count == 1)
            {
                LocateReferenceByPageAndSelection(possibleReferenceLocation.ElementAt(0).Page, possibleReferenceLocation.ElementAt(0).Selection);
                if (MessageBox.Show("Is the displayed reference the reference we received the Third Party Authorization/ Power of Attorney for?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    return new ReferencesDemographics(RI);
                else
                    return null;
            }
            else
            {
                ShowMultipleReferences(possibleReferenceLocation);

                if (RI.CheckForText(1, 71, "TXX1R-03"))
                    return new ReferencesDemographics(RI);
                else
                    return null;
            }
        }

        /// <summary>
        /// Loops though a borrowers reference screen to find a matching reference.
        /// </summary>
        /// <param name="referenceFirstName">References First Name.</param>
        /// <param name="referenceLastName">References Last Name.</param>
        /// <returns>A Dictionary with the Page and Selection for possible references.</returns>
        private List<PossibleReferenceLocations> LocatePossibleReferences(string referenceFirstName, string referenceLastName)
        {
            List<PossibleReferenceLocations> possibleReferenceLocation = new List<PossibleReferenceLocations>();
            RI.Hit(ReflectionInterface.Key.F5);
            for (int row = 10; RI.MessageCode != "90007"; row++)
            {
                if (row > 21)
                {
                    row = 9;
                    RI.Hit(ReflectionInterface.Key.F8);
                    continue;
                }

                //We only want to check references that are active.
                if (!RI.CheckForText(row, 78, "A"))
                    continue;

                string selection = RI.GetText(row, 2, 2);
                string page = RI.GetText(2, 71, 2);
                RI.PutText(22, 12, selection, ReflectionInterface.Key.Enter, true);

                //They only want to compare the first 4 letters of the first and last name to determine if they are trying to add a duplicate reference.
                string systemReferenceFirstName = RI.GetText(4, 34, 4);
                string systemReferenceLastName = RI.GetText(4, 6, 4);

                if (referenceFirstName.ToUpper().Contains(systemReferenceFirstName.ToUpper()) && referenceLastName.ToUpper().Contains(systemReferenceLastName.ToUpper()))
                    possibleReferenceLocation.Add(new PossibleReferenceLocations() { Page = page, Selection = selection });

                RI.Hit(ReflectionInterface.Key.F12);
            }
            return possibleReferenceLocation;
        }

        /// <summary>
        /// Accesses a references demographic screen.
        /// </summary>
        /// <param name="page">Page reference is located on.</param>
        /// <param name="selection">Reference selection.</param>
        private void LocateReferenceByPageAndSelection(string page, string selection)
        {
            while (!RI.CheckForText(2, 72, "1"))
                RI.Hit(ReflectionInterface.Key.F7);

            RI.Hit(ReflectionInterface.Key.F5);
            //At this point we should be on the last screen for references, we will now traverse back to locate the reference
            while (RI.MessageCode != "90007")
            {
                if (RI.GetText(2, 71, 2) == page)
                {
                    RI.PutText(22, 12, selection, ReflectionInterface.Key.Enter, true);
                    return;
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
        }

        /// <summary>
        /// Checks TX6T for an existing queue task.
        /// </summary>
        /// <param name="ssn">Borrowers Ssn.</param>
        /// <param name="isPowerOfAttorney">Indicator if request is for Power Of Attorney.</param>
        private void CheckForExistingQueueTask(string ssn, bool isPowerOfAttorney)
        {
            RI.FastPath("TX3Z/ITX6T*");
            RI.PutText(6, 41, ssn, ReflectionInterface.Key.Enter);
            string queueToCheck = isPowerOfAttorney ? "A3" : "T3";

            if (RI.ScreenCode == "TXX6U")
            {
                MessageBox.Show(string.Format("Unable to locate {0} queue task.", queueToCheck), "Unable to Locate Task", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EndDllScript();
            }

            bool foundQueue = false;
            for (int row = 7; RI.MessageCode != "90007"; row += 5)
            {
                if (row > 17)
                {
                    row = 2;
                    RI.Hit(ReflectionInterface.Key.F8);
                    continue;
                }
                if (RI.CheckForText(row, 8, queueToCheck))
                {
                    foundQueue = true;
                    TaskControlNumber = RI.GetText(row, 40, 20);
                    RI.PutText(21, 18, RI.GetText(row, 3, 1), ReflectionInterface.Key.Enter, true);
                    break;
                }
            }

            if (!foundQueue)
            {
                MessageBox.Show(string.Format("Unable to locate {0} queue task.", queueToCheck), "Unable to Locate Task", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EndDllScript();
            }
        }

        /// <summary>
        /// Gets a Borrowers demographic information from TS26
        /// </summary>
        /// <param name="accountIdentifier">Borrowers account number or Ssn.</param>
        /// <returns>A borrowerData object if a borrower is found.</returns>
        public BorrowerData GetBorrowerDemographics(string accountIdentifier)
        {
            if (!VerifyBorrowerExists(accountIdentifier))
                return null;

            //We are assuming that the borrower have a valid mailing address.  If they do not this will be changed below.
            bool hasValidAddress = true;
            if (RI.CheckForText(11, 55, "N"))
            {
                RI.Hit(ReflectionInterface.Key.F6);
                RI.Hit(ReflectionInterface.Key.F6);
                hasValidAddress = HasAlternateValidAddress("B", "D");
            }
            return new BorrowerData(RI, hasValidAddress);
        }

        /// <summary>
        /// Verify a borrower can be found in TX1J.
        /// </summary>
        /// <param name="accountIdentifier">Borrowers Account Number of Ssn.</param>
        /// <returns>True if the borrower is found.</returns>
        public bool VerifyBorrowerExists(string accountIdentifier)
        {
            RI.FastPath("TX3Z/ITX1J");
            RI.PutText(5, 16, "B");

            //Clear out any data in the RI.FastPath
            RI.PutText(6, 16, "", true);
            RI.PutText(6, 20, "", true);
            RI.PutText(6, 23, "", true);

            if (accountIdentifier.Length == 9)
                RI.PutText(6, 16, accountIdentifier, ReflectionInterface.Key.Enter);
            else if (accountIdentifier.Length == 10)
                RI.PutText(6, 61, accountIdentifier, ReflectionInterface.Key.Enter);
            else
                return false;

            //The screen did not change so we know that the account was not found.
            if (RI.ScreenCode == "TXX1K")
                return false;

            return true;
        }

        /// <summary>
        /// Gets a borrowers Address from TX1J
        /// </summary>
        /// <param name="data">BorrowerData object</param>
        /// <returns>BorrowerData object with the new address.</returns>
        public BorrowerData GetAddress(BorrowerData data)
        {
            //Calls the RI.GetText method and removes any underscores
            Func<int, int, int, string> GGetTextRemoveUnderscore = (row, col, length) => RI.GetText(row, col, length).Replace("_", "");

            data.Ssn = GGetTextRemoveUnderscore(3, 12, 11);
            data.AccountNumber = GGetTextRemoveUnderscore(3, 34, 12);

            if (data.HasValidAddress)
            {
                data.Street1 = GGetTextRemoveUnderscore(11, 10, 29);
                data.Street2 = GGetTextRemoveUnderscore(12, 10, 29);
                data.ForeignState = GGetTextRemoveUnderscore(12, 52, 14);
                data.ForeignCountry = GGetTextRemoveUnderscore(12, 77, 2);
                data.City = GGetTextRemoveUnderscore(14, 8, 20);
                data.State = GGetTextRemoveUnderscore(14, 32, 2);
                data.Zip = GGetTextRemoveUnderscore(14, 40, 5);
            }
            return data;
        }

        /// <summary>
        /// Checks to see if an Alternate address exists. (Recursive)
        /// </summary>
        /// <param name="addressType">Address Type to Check</param>
        /// <param name="nextAddressType">Next Address Type to Check (Recursive).</param>
        /// <returns></returns>
        public bool HasAlternateValidAddress(string addressType, string nextAddressType = "")
        {
            RI.PutText(10, 14, addressType, ReflectionInterface.Key.Enter);
            if (!RI.CheckForText(11, 55, "Y"))
            {
                if (nextAddressType.IsNullOrEmpty())
                    return false;
                return HasAlternateValidAddress(nextAddressType);
            }
            return true;
        }

        /// <summary>
        /// Checks the Users Arc Access.
        /// </summary>
        /// <returns>True if the User has access to all of the ARC's</returns>
        public bool VerifyArcAccess()
        {
            string missingArcs = string.Empty;
            foreach (string arc in new List<string>() { "X3RDC", "X3RDD", "MREFP", "MPOAA", "MPOAD", "M1REF" })
            {
                RI.FastPath(string.Format("TX3Z/ITX68{0};{1}", UserId, arc));
                if (RI.CheckForText(1, 72, "TXX69"))
                    missingArcs += "\n" + arc;
            }

            if (!missingArcs.IsNullOrEmpty())
            {
                MessageBox.Show("You do not have access to the following ARCS:" + missingArcs, "ARC Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
