using System;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using System.Collections.Generic;
using System.Reflection;

namespace ADDMOREFED
{
    public class AddOrModifyReferenceScriptFed : FedScript
    {
		private DataAccess dataAccess { get; set; }
		private string ssn { get; set; }
		private bool referenceAdded { get; set; }
		private string accountNumber { get; set; }
		private string referenceId { get; set; }
		public ProcessLogData plData { get; set; }
        public enum UpdateMode
        {
            Add,
            Modify
        };

        //constructor for Reflection session
        public AddOrModifyReferenceScriptFed(ReflectionInterface ri)
            : base(ri, "ADDMOREFED")
        {
            dataAccess = new DataAccess();
        }

        public override void Main()
        {
			//Validate sproc access, set up region and process logger.  Kills program if failure
			plData = ProcessLogger.RegisterApplication("ADDMOREFED", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
			//loops allowing modification of references until user hits cancel or hits no during the "add another reference" prompt
			AddOrModifyReference();
        }

		/// <summary>
		/// Adds or Modifies a reference based on user input on the reference information form
		/// </summary>
		private void AddOrModifyReference()
		{
			//loop control variables
			bool isFinished = false;

			//loop until the user is finished adding or modifying references
			while (!isFinished)
			{
				List<BorrowerReference> references = new List<BorrowerReference>();
				//initialize the form for getting the account or reference number
				PromptForAccountNumber promptForm = new PromptForAccountNumber();

				//stays in this function until valid account or reference data is provided or the user hits cancel
				PromptForValidData(references, promptForm);

				//display the reference information form where the user will add or modify the reference information
				ReferenceInformation updateForm;
				//determines whether the reference is new or being modified.
				if (!references.Count.Equals(0))
					updateForm = new ReferenceInformation(references, UpdateMode.Modify, -1);
				else
					updateForm = new ReferenceInformation(references, UpdateMode.Add, -1);

				referenceAdded = false;
				//continue to display the update form until TX1J is successfully updated or the user cancels
				ValidateTX1JUpdate(updateForm);

				//prompt the user to add or modify another reference or to end
				if (MessageBox.Show("Add or Modify Reference Script has finished running. Would you like to add or modify another reference?", "Add or Modify Another Reference?", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
					isFinished = true;
			}
		}

		/// <summary>
		/// Takes information from the ReferenceInformation form and updates TX1J
		/// </summary>
		/// <param name="updateForm">Form data from the ReferenceInformation form</param>
		/// <returns>True if update is completed successfully</returns>
		private bool ValidateTX1JUpdate(ReferenceInformation updateForm)
		{
			bool tx1jUpdated = false;
			
			while (!tx1jUpdated)
			{
				updateForm.ShowDialog();

				//cancel
				if (updateForm.DialogResult.Equals(DialogResult.Cancel))
				{
					Dialog.Info.Ok("Cancel button pressed.  Closing program.");
					System.Environment.Exit(1);
				}
				else
				{
					//go to update TX1J
					if (UpdateTx1J(updateForm.ReferenceData, updateForm.UpdateMode))
					{
						//add a comment for adding the new reference or modifying it
						string arc = updateForm.UpdateMode.Equals(UpdateMode.Add) ? "M1REF": "MODRF";

						if (!RI.Atd22AllLoans(ssn, arc, CompiledComment(updateForm.ReferenceData, updateForm.UpdateMode), string.Empty, "ADDMOREFED", false))
						{
							string message = string.Format("Unable to leave arc {1} comment regarding the changes made to the reference’s demographics for ssn {0}.", ssn, arc);
							MessageBox.Show(message, "ARC Add Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							ProcessLogger.AddNotification(plData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
						}
						tx1jUpdated = true;
					}
				}
			}
			return tx1jUpdated;
		}

		/// <summary>
		/// Validates the data entered on the Search form
		/// </summary>
		/// <param name="entryIsValid">Control variable.  This will be false until the function completed</param>
		/// <param name="references">List for references to add to once and entry is found to be valid</param>
		/// <param name="promptForm">Form data from PromptForAccountNumber form</param>
		/// <returns>True if form data is valid</returns>
		private bool PromptForValidData(List<BorrowerReference> references, PromptForAccountNumber promptForm)
		{
			bool entryIsValid = false;
			//prompt the user for an account number or a reference ID until valid data is entered
			while (!entryIsValid)
			{
				promptForm.ShowDialog();
				//cancel
				if (promptForm.DialogResult.Equals(DialogResult.Cancel))
				{
					Dialog.Info.Ok("Cancel button pressed.  Closing program.");
					System.Environment.Exit(1);
				}

				//get values from the form
				accountNumber = promptForm.AccountNumber;
				referenceId = promptForm.ReferenceId;

				//if an account number was entered
				if (!accountNumber.Equals(string.Empty))
					entryIsValid = LocateAccount(entryIsValid, references, accountNumber);
				else //if a reference ID was entered
					entryIsValid = LocateReference(entryIsValid, references, referenceId);
			}
			return entryIsValid;
		}

		/// <summary>
		/// Loads into the account provided.  If unable returns false
		/// </summary>
		/// <param name="entryIsValid">Control variable.  Pass as false</param>
		/// <param name="references">List of references to add a new reference to if the account is found</param>
		/// <returns>True if account is found and reference is added</returns>
		private bool LocateAccount(bool entryIsValid, List<BorrowerReference> references, string accountNumber)
		{
			//verify the borrower is valid
			if (VerifiedBorrower(accountNumber))
			{
				//access reference selection screen
				RI.Hit(Key.F4);

				//select all active references and gather demographic data
				int row = 10;
				while (!RI.CheckForText(23, 2, "90007"))
				{
					//get the demographic data if the record is an active reference
					if (RI.CheckForText(row, 5, "R") && RI.CheckForText(row, 78, "A"))
					{
						RI.PutText(22, 12, RI.GetText(row, 2, 2), Key.Enter, true);
						AddReferenceToList(references);
						RI.Hit(Key.F12);
					}
					row++;
					//We have reached the end of the page or references
					if (RI.GetText(row, 3, 1).Equals(string.Empty) || row.Equals(22))
					{
						RI.Hit(Key.F8);
						row = 10;
					}
				}
				entryIsValid = true;
			}
			return entryIsValid;
		}

		/// <summary>
		/// Verifies that the referenceId provided is valid and able to be modified.
		/// </summary>
		/// <param name="entryIsValid">Control variable.  Pass as false</param>
		/// <param name="references">List of references to add to</param>
		/// <param name="referenceId">Reference to be located</param>
		/// <returns>True if the reference is added to the references list</returns>
		private bool LocateReference(bool entryIsValid, List<BorrowerReference> references, string referenceId)
		{
			//verify reference exists
			RI.FastPath("TX3Z/ITX1JR;P" + referenceId);
			if (RI.CheckForText(1, 71, "TXX1R"))
			{
				//check that the reference is active
				if (!RI.CheckForText(7, 66, "A"))
					MessageBox.Show("Unable to modify the reference because the reference is not active.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else
				{
					AddReferenceToList(references);
					RI.FastPath("TX3Z/ITX1JB;" + RI.GetText(7, 11, 11).Replace(" ", ""));
					string accountNumber = RI.GetText(3, 34, 12).Replace(" ", "");
					if (VerifiedBorrower(accountNumber))
						entryIsValid = true;
				}
			}
			else
				MessageBox.Show("Unable to locate the reference.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return entryIsValid;
		}

        /// <summary>
        /// Checks the reflection interface to verify that a borrower is a valid selection
        /// </summary>
		/// <param name="accountNumber">accountNumber to verify exists</param>
		/// <returns>True if the borrower exists and is able to have a reference added.</returns>
        private bool VerifiedBorrower(string accountNumber)
        {
            bool returnValue = true;

            RI.FastPath("TX3Z/ITS26" + accountNumber);
			if (RI.CheckForText(1, 72, "T1X07"))
            {
                MessageBox.Show("Unable to locate the borrower", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                returnValue = false;
            }
            else if (RI.CheckForText(1, 72, "TSX28"))
            {
                ssn = RI.GetText(4, 16, 11).Replace("-", string.Empty);
                RI.PutText(21, 12, "01", Key.Enter);
            }

            if (RI.CheckForText(1, 72, "TSX29"))
            {
                ssn = RI.GetText(5, 16, 11).Replace(" ", string.Empty);
                if (Convert.ToDecimal(RI.GetText(10, 16, 10)) <= 0)
                {
                    MessageBox.Show("Unable to add or modify a reference because the borrower doesn’t have a balance. Please input another borrower’s account # or reference ID”.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    returnValue = false;
                }
            }
            else if (returnValue)
            {
                MessageBox.Show("Unable to locate the borrower.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                returnValue = false;
            }

            return returnValue;
        } 

		/// <summary>
		/// add a new reference to the list of references
		/// </summary>
		/// <param name="references">List to add a new reference to</param>
        private void AddReferenceToList(List<BorrowerReference> references)
        {
            BorrowerReference tempReference = new BorrowerReference();

            //get demographics including e-mail
			string ssn = RI.GetText(3, 12, 11).Replace(" ", "");
            tempReference.Demographics = RI.GetDemographicsFromTx1j(ssn);
            RI.Hit(Key.F12);
            RI.Hit(Key.F2);
			//TX1J
            tempReference.Demographics.MiddleIntial = RI.GetText(4, 53, 13).Replace("_", string.Empty);
            tempReference.Suffix = RI.GetText(4, 72, 4).Replace("_", string.Empty);
            tempReference.Relationship = RI.GetText(8, 15, 2);

            //get home phone
			tempReference.HomePhone = GetPhoneDataFromTX1J(RI);

            //navigate to phone fields
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);

            //get alternate phone
            RI.PutText(16, 14, "A", Key.Enter);
            if (!RI.CheckForText(23, 2, "01105"))
				tempReference.AlternatePhone = GetPhoneDataFromTX1J(RI);
            else
                tempReference.AlternatePhone = new Phone();

            //get work phone
            RI.PutText(16, 14, "W", Key.Enter);
            if (!RI.CheckForText(23, 2, "01105"))
				tempReference.WorkPhone = GetPhoneDataFromTX1J(RI);
            else
                tempReference.WorkPhone = new Phone();
            //add reference
            references.Add(tempReference);
        }

		/// <summary>
		/// Retrieve Phone data from the session TX1J screen
		/// </summary>
		/// <param name="RI">Session to use for phone data retrieval</param>
		/// <returns>Phone Object</returns>
		private Phone GetPhoneDataFromTX1J(ReflectionInterface RI)
		{
			Phone bwrPhone = new Phone();
			bwrPhone.PhoneType = RI.GetText(16, 14, 1);
			bwrPhone.MBLIndicator = RI.GetText(16, 20, 1);
			bwrPhone.ConsentIndicator = RI.GetText(16, 30, 1);
			bwrPhone.VerifiedDate = RI.GetText(16, 45, 8).Replace(" ","/").ToDate();
			bwrPhone.DomesticAreaCode = RI.GetText(17, 14, 3).Replace("_", "");
			bwrPhone.DomesticPrefix = RI.GetText(17, 23, 3).Replace("_", "");
			bwrPhone.DomesticLineNumber = RI.GetText(17, 31, 4).Replace("_", "");
			bwrPhone.ValidityIndicator = RI.GetText(17, 54, 1);
			bwrPhone.ForeignCountryCode = RI.GetText(18, 15, 3).Replace("_", "");
			bwrPhone.ForeignCityCode = RI.GetText(18, 24, 5).Replace("_", "");
			bwrPhone.ForeignLocalNumber = RI.GetText(18, 36, 11).Replace("_", "");

			bwrPhone.Extension = RI.GetText(18, 53, 5).Replace("_", "");
			if (RI.CheckForText(17, 14, "_"))
				bwrPhone.Extension = RI.GetText(17, 40, 5).Replace("_", "");

			return bwrPhone;
		} 

		/// <summary>
		/// Update the reflection interface with the new reference
		/// </summary>
		/// <param name="reference">Reference to add or modify</param>
		/// <param name="mode">add or modify</param>
		/// <returns>true if reference is added successfully</returns>
        private bool UpdateTx1J(BorrowerReference reference, UpdateMode mode)
        {
			bool modifyRefStarter = true;
			bool addNewRefName = true;
			//change mode to modify if the script already added the reference and is returning to correct bad data entered by the user in the form
            if (mode.Equals(UpdateMode.Add) && referenceAdded) 
				mode = UpdateMode.Modify;

            //get source code from description
            string sourceCode = dataAccess.GetAddressSourceCodeForDescription(reference.Source);

            //get borrower demographics if the reference demographics were left blank
            if (reference.Demographics.Address1.Equals(String.Empty))
				GetMissingDemographics(reference);

            //begin addding a new reference
			//adds the name and relationship of the new reference into tx1j
            if (mode.Equals(UpdateMode.Add))
				addNewRefName = AddNewReferenceName(reference);
            else
            //begin modifying the reference
            {
				//get reference ID if the script is already on CTX1J because the user had to correct some bad data entered into the form
                if (RI.CheckForText(1, 71, "TXX1R-03")) 
					reference.Demographics.Ssn = RI.GetText(3, 12, 11).Replace(" ", "");
				//changes name, invalidates address and gets reference ready for updates
				modifyRefStarter = ModifyReferenceStarter(reference, sourceCode);
            }

            //update address.  Returns true if successful or if no change
			bool modifyAddress = ModifyAddress(reference);

			//set indicator that reference has already been added in case script needs to return to update bad data entered by the user in the form
            if (mode.Equals(UpdateMode.Add)) 
				referenceAdded = true;

            //get reference ID assigned by the system when new refrence is added
			reference.Demographics.Ssn = RI.GetText(3, 12, 11).Replace(" ", "");

			RI.Hit(Key.F6);
            if (mode.Equals(UpdateMode.Add))
            {
				RI.Hit(Key.F6);
				RI.Hit(Key.F6);
            }

            //update phones
			bool homePhoneChange = PhoneChange(reference.HomePhone, sourceCode, "H");
			bool altPhoneChange = PhoneChange(reference.AlternatePhone, sourceCode, "A");
			bool workPhoneChange = PhoneChange(reference.WorkPhone, sourceCode, "W");

            //update e-mail address
			bool emailChange = EmailChange(reference, sourceCode);
			//Check all for failures and return true if every update was successful
			return (modifyRefStarter && addNewRefName && modifyAddress && homePhoneChange && altPhoneChange && workPhoneChange && emailChange);
        }

		/// <summary>
		/// Compares the email in tx1j with the email from the form and if different, updates tx1j.
		/// </summary>
		/// <param name="reference">Reference to have the email reviewed for</param>
		/// <param name="sourceCode">where did the update come from</param>
		/// <returns>True if email is updated successfully or no change.  False if update unsuccessful.</returns>
		private bool EmailChange(BorrowerReference reference, string sourceCode)
		{
			if (!reference.Demographics.EmailAddress.Equals(string.Empty))
			{
				//Navigate from default tx1j to the email screen
				RI.Hit(Key.F2);
				RI.Hit(Key.F10);

				//check all email lines for an email.
				string email = string.Concat(RI.GetText(14, 10, 60), RI.GetText(15, 10, 60), RI.GetText(16, 10, 60), RI.GetText(17, 10, 60), RI.GetText(18, 10, 14)).Replace("_", "");
				bool validityInd = false;
				if (RI.GetText(12, 14, 1) == "Y")
					validityInd = true;

				if (!reference.Demographics.EmailAddress.Equals(email) || !reference.Demographics.IsValidEmail.Equals(validityInd))
				{
					//update the email source, last verified date, valid flag, null out email lines and put email on first available line
					RI.PutText(9, 20, sourceCode);
					RI.PutText(11, 17, DateTime.Now.ToString("MMddyy"));
					RI.PutText(12, 14, reference.Demographics.IsValidEmail.ToString());
					RI.PutText(14, 10, string.Empty, true);
					RI.PutText(15, 10, string.Empty, true);
					RI.PutText(16, 10, string.Empty, true);
					RI.PutText(17, 10, string.Empty, true);
					RI.PutText(18, 10, string.Empty, true);
					RI.PutText(14, 10, reference.Demographics.EmailAddress);
					if (!UpdateVerified("01005", "01004")) { return false; }
				}
			}
			return true;
		}

		/// <summary>
		/// Takes a Phone object and updates the tx1j screen with the information from the object if it is different than current information.
		/// </summary>
		/// <param name="phone">Phone object to use for updating</param>
		/// <param name="sourceCode">where the update came from</param>
		/// <param name="phoneFlag">"A","H","W" for alternate, home, or work phone number</param>
		/// <returns>True for an successful update or no update.  False for an unsuccessful update.</returns>
		private bool PhoneChange(Phone phone, string sourceCode, string phoneFlag)
		{
			RI.PutText(16, 14, phoneFlag, Key.Enter);
			//check all phone fields.  if any have changed all need to be updated.
			if (
					(!phone.DomesticAreaCode.Equals(string.Empty) && phone.DomesticAreaCode != RI.GetText(17, 14, 3).Replace("_", "")) ||
					(!phone.DomesticPrefix.Equals(string.Empty) && phone.DomesticPrefix != RI.GetText(17, 23, 3).Replace("_", "")) ||
					(!phone.DomesticLineNumber.Equals(string.Empty) && phone.DomesticLineNumber != RI.GetText(17, 31, 4).Replace("_", "")) ||
					(!RI.CheckForText(17, 54, "_") && phone.ValidityIndicator != RI.GetText(17, 54, 1)) ||
					(!phone.ForeignCountryCode.Equals(string.Empty) && phone.ForeignCountryCode != RI.GetText(18, 15, 3).Replace("_", "").Replace(" ", "")) ||
					(!phone.ForeignCityCode.Equals(string.Empty) && phone.ForeignCityCode != RI.GetText(18, 24, 5).Replace("_", "").Replace(" ", "")) ||
					(!phone.ForeignLocalNumber.Equals(string.Empty) && phone.ForeignLocalNumber != RI.GetText(18, 36, 11).Replace("_", "").Replace(" ", ""))
				)
			{
				RI.PutText(16, 20, "U");
				RI.PutText(16, 30, "N");
				RI.PutText(16, 45, DateTime.Now.ToString("MMddyy"));
				RI.PutText(16, 78, "A");
				RI.PutText(17, 14, phone.DomesticAreaCode, true);
				RI.PutText(17, 23, phone.DomesticPrefix, true);
				RI.PutText(17, 31, phone.DomesticLineNumber, true);
				RI.PutText(17, 54, phone.ValidityIndicator, true);
				RI.PutText(18, 15, phone.ForeignCountryCode, true);
				RI.PutText(18, 24, phone.ForeignCityCode, true);
				RI.PutText(18, 36, phone.ForeignLocalNumber, true);
				RI.PutText(19, 14, sourceCode);
				if (!UpdateVerified("01097", "01100")) { return false; }
			}
			return true;
		}

		/// <summary>
		/// Compares address fields in tx1j to address fields in form and updates on differences
		/// </summary>
		/// <param name="reference">Reference to update</param>
		/// <returns>True if successful or no change.  False if unsuccessful change</returns>
		private bool ModifyAddress(BorrowerReference reference)
		{
			bool addr1Change = Address1Change(reference);
			bool addr2Change = Address2Change(reference);
			bool cityChange = CityChange(reference);
			bool stateChange = StateChange(reference);
			bool zipcodeChange = ZipCodeChange(reference);
			bool countryChange = CountryChange(reference);
			bool validityChange = AddressValidityChange(reference);

			bool addrUpdated = (addr1Change || addr2Change || cityChange || stateChange || zipcodeChange || countryChange || validityChange);

			if (addrUpdated)
			{
				RI.PutText(10, 32, DateTime.Now.ToString("MMddyy"));
				RI.PutText(11, 55, reference.Demographics.IsValidAddress.ToString(), true);
				if (!UpdateVerified("01096", "01004")) { return false; }
			}
			return true;
		}

		/// <summary>
		/// Compares IsValidAddress from tx1j and the form and if different, updates tx1j
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>True if update is successful.  False if no update was made or if it failed</returns>
		private bool AddressValidityChange(BorrowerReference reference)
		{
			try
			{
				if (!reference.Demographics.IsValidAddress.Equals(RI.GetText(11, 55, 1)))
				{
					RI.PutText(11, 55, reference.Demographics.IsValidAddress.ToString(), true);
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Compares Country from tx1j and the form and if different, updates tx1j
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>True if update is successful.  False if no update was made or if it failed</returns>
		private bool CountryChange(BorrowerReference reference)
		{
			try
			{
				if (!reference.Demographics.Country.ToUpper().Equals(RI.GetText(13, 52, 25).Replace("_", "")))
				{
					RI.PutText(13, 52, reference.Demographics.Country, true);
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Compares ZipCode from tx1j and the form and if different, updates tx1j
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>True if update is successful.  False if no update was made or if it failed</returns>
		private bool ZipCodeChange(BorrowerReference reference)
		{
			try
			{
				if (!reference.Demographics.ZipCode.Replace("-", "").ToUpper().Equals(RI.GetText(14, 40, 17).Replace("_", "")))
				{
					RI.PutText(14, 40, reference.Demographics.ZipCode.Replace("-", ""), true);
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Compares Domestic and Foreign State from tx1j and the form and if different, updates tx1j
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>True if update is successful.  False if no update was made or if it failed</returns>
		private bool StateChange(BorrowerReference reference)
		{
			try
			{
				//Domestic state change
				if (reference.Demographics.Country.Equals(string.Empty) && !reference.Demographics.State.ToUpper().Equals(RI.GetText(14, 32, 2).Replace("_", "")))
				{
					RI.PutText(14, 32, reference.Demographics.State, true);
					RI.PutText(12, 52, string.Empty, true);
					RI.PutText(12, 77, string.Empty, true);
					RI.PutText(13, 52, string.Empty, true);
					return true;
				}
				//foreign state change
				else if (!reference.Demographics.Country.Equals(string.Empty) && !reference.Demographics.State.ToUpper().Equals(RI.GetText(12, 52, 15).Replace("_", "")))
				{
					RI.PutText(12, 52, reference.Demographics.State, true);
					RI.PutText(14, 32, string.Empty, true);
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Compares City from tx1j and the form and if different, updates tx1j
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>True if update is successful.  False if no update was made or if it failed</returns>
		private bool CityChange(BorrowerReference reference)
		{
			try
			{
				if (!reference.Demographics.City.ToUpper().Equals(RI.GetText(14, 8, 20).Replace("_", "")))
				{
					RI.PutText(14, 8, reference.Demographics.City, true);
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Compares Address2 from tx1j and the form and if different, updates tx1j
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>True if update is successful.  False if no update was made or if it failed</returns>
		private bool Address2Change(BorrowerReference reference)
		{
			try
			{
				if (!reference.Demographics.Address2.ToUpper().Equals(RI.GetText(12, 10, 30).Replace("_", "")))
				{
					RI.PutText(12, 10, reference.Demographics.Address2, true);
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Compares Address1 from tx1j and the form and if different, updates tx1j
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>True if update is successful.  False if no update was made or if it failed</returns>
		private bool Address1Change(BorrowerReference reference)
		{
			try
			{
				if (!reference.Demographics.Address1.ToUpper().Equals(RI.GetText(11, 10, 30).Replace("_", "")))
				{
					RI.PutText(11, 10, reference.Demographics.Address1, true);
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Performs checks for first, middle, last, suffix, and relationship and updates them if needed.  Also invalidates address, phone, and email to prep for the updates to those fields.
		/// </summary>
		/// <param name="reference">Reference to change</param>
		/// <param name="sourceCode">source for change to information on file</param>
		/// <returns>true if successful</returns>
		private bool ModifyReferenceStarter(BorrowerReference reference, string sourceCode)
		{
			RI.FastPath("TX3ZCTX1JR" + reference.Demographics.Ssn);
			//true unless update fails for all the following fields (no change is considered true)
			bool firstNameChange = FirstNameChange(reference);
			bool middleInitialChange = MiddleInitialChange(reference);
			bool lastNameChange = LastNameChange(reference);
			bool suffixChange = SuffixChange(reference);
			RI.Hit(Key.F6);
			bool relationshipChange = RelationshipChange(reference);
			RI.Hit(Key.F6);

			bool invalidate = InvalidateAddressAndPhone(reference, sourceCode);

			//return to the top of the screen so the script is in position for the next steps
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);
			return (firstNameChange && middleInitialChange && lastNameChange && suffixChange && relationshipChange && invalidate);
		}

		/// <summary>
		/// For modified references only, address, phone and email are invalidated to start off with
		/// </summary>
		/// <param name="reference">Reference to modify</param>
		/// <param name="sourceCode">code for source of new info</param>
		/// <returns>True if invalidation is successful</returns>
		private bool InvalidateAddressAndPhone(BorrowerReference reference, string sourceCode)
		{
			//invalidate the address
			bool invalidateAddress = InvalidateAddress(reference);
			RI.Hit(Key.F6);

			//invalidate the home phone
			bool invalidateHomePhone = InvalidateHomePhone(reference, sourceCode);

			//invalidate the alternate phone
			bool invalidateAltPhone = InvalidateAlternatePhone(reference, sourceCode);

			//invalidate the work phone
			bool invalidateWorkPhone = InvalidateWorkPhone(reference, sourceCode);

			//invalidate the e-mail address
			bool invalidateEmail = InvalidateEmail(reference, sourceCode);

			return (invalidateAddress && invalidateHomePhone && invalidateAltPhone && invalidateWorkPhone && invalidateEmail);
		}

		/// <summary>
		/// Sets the email to invalid.
		/// </summary>
		/// <param name="reference">Reference to modify</param>
		/// <param name="sourceCode">Source of invalidity</param>
		/// <returns>true if successful in invalidation</returns>
		private bool InvalidateEmail(BorrowerReference reference, string sourceCode)
		{
			if (reference.EmailInvalidateFirst.Equals(CheckState.Checked))
			{
				RI.Hit(Key.F2);
				RI.Hit(Key.F10);
				RI.PutText(9, 20, sourceCode);
				RI.PutText(11, 17, DateTime.Now.ToString("MMddyy"));
				RI.PutText(12, 14, "N");
				if (!UpdateVerified("01005", "01004"))
				{ return false; }
				RI.Hit(Key.F12);
				RI.Hit(Key.F2);
			}
			return true;
		}

		/// <summary>
		/// Sets the work phone to invalid.
		/// </summary>
		/// <param name="reference">Reference to modify</param>
		/// <param name="sourceCode">Source of invalidity</param>
		/// <returns>true if successful in invalidation</returns>
		private bool InvalidateWorkPhone(BorrowerReference reference, string sourceCode)
		{
			if (reference.WorkPhoneInvalidateFirst.Equals(CheckState.Checked))
			{
				RI.PutText(16, 14, "W", Key.Enter);
				RI.PutText(17, 54, "N");
				RI.PutText(16, 45, DateTime.Now.ToString("MMddyy"));
				RI.PutText(19, 14, sourceCode);
				if (!UpdateVerified("01097", "01100"))
				{ return false; }
			}
			return true;
		}

		/// <summary>
		/// Sets the AlternatePhone to invalid.
		/// </summary>
		/// <param name="reference">Reference to modify</param>
		/// <param name="sourceCode">Source of invalidity</param>
		/// <returns>true if successful in invalidation</returns>
		private bool InvalidateAlternatePhone(BorrowerReference reference, string sourceCode)
		{
			if (reference.AlternatePhoneInvalidateFirst.Equals(CheckState.Checked))
			{
				RI.PutText(16, 14, "A", Key.Enter);
				RI.PutText(17, 54, "N");
				RI.PutText(16, 45, DateTime.Now.ToString("MMddyy"));
				RI.PutText(19, 14, sourceCode);
				if (!UpdateVerified("01097", "01100"))
				{ return false; }
			}
			return true;
		}

		/// <summary>
		/// Sets the HomePhone to invalid.
		/// </summary>
		/// <param name="reference">Reference to modify</param>
		/// <param name="sourceCode">Source of invalidity</param>
		/// <returns>true if successful in invalidation</returns>
		private bool InvalidateHomePhone(BorrowerReference reference, string sourceCode)
		{
			if (reference.HomePhoneInvalidateFirst.Equals(CheckState.Checked))
			{
				RI.PutText(17, 54, "N");
				RI.PutText(16, 45, DateTime.Now.ToString("MMddyy"));
				RI.PutText(19, 14, sourceCode);
				if (!UpdateVerified("01097", "01100"))
				{ return false; }
			}
			return true;
		}

		/// <summary>
		/// Sets the address to invalid.
		/// </summary>
		/// <param name="reference">Reference to modify</param>
		/// <param name="sourceCode">Source of invalidity</param>
		/// <returns>true if successful in invalidation</returns>
		private bool InvalidateAddress(BorrowerReference reference)
		{
			if (reference.AddressInvalidateFirst.Equals(CheckState.Checked))
			{
				RI.PutText(11, 55, "N", true);
				RI.PutText(10, 32, DateTime.Now.ToString("MMddyy"));
				if (!UpdateVerified("01096", "01004"))
				{ return false; }
			}
			return true;
		}

		/// <summary>
		///  Checks the relationship in the session against the form relationship and updates the session if they are different
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>Returns true for a successful relationship change or no change.  Returns false for an unsuccessful relationship change.</returns>
		private bool RelationshipChange(BorrowerReference reference)
		{
			//update the relationship if it was changed
			if (!dataAccess.GetRelationshipCodeForDescription(reference.Relationship).Equals(RI.GetText(8, 15, 2)))
			{
				RI.PutText(8, 15, dataAccess.GetRelationshipCodeForDescription(reference.Relationship), true);
				if (!UpdateVerified("01094", "01004"))
				{ return false; }
			}
			return true;
		}

		/// <summary>
		///  Checks the suffix in the session against the form suffix and updates the session if they are different
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>Returns true for a successful suffix change or no change.  Returns false for an unsuccessful suffix change.</returns>
		private bool SuffixChange(BorrowerReference reference)
		{
			//update the suffix if it was changed
			if (!reference.Suffix.Replace(".", "").ToUpper().Equals(RI.GetText(4, 72, 4).Replace("_", "")))
			{
				RI.PutText(4, 72, reference.Suffix.Replace(".", ""), true);
				if (!UpdateVerified("01093", "01004"))
				{ return false; }
			}
			return true;
		}

		/// <summary>
		///  Checks the last name in the session against the form last name and updates the session if they are different
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>Returns true for a successful name change or no change.  Returns false for an unsuccessful name change.</returns>
		private bool LastNameChange(BorrowerReference reference)
		{
			//update the last name if it was changed
			if (!reference.Demographics.LastName.ToUpper().Equals(RI.GetText(4, 6, 23)))
			{
				RI.PutText(4, 6, reference.Demographics.LastName, true);
				if (!UpdateVerified("01093", "01004"))
				{ return false; }
			}
			return true;
		}

		/// <summary>
		///  Checks the middle name in the session against the form middle name and updates the session if they are different
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>Returns true for a successful name change or no change.  Returns false for an unsuccessful name change.</returns>
		private bool MiddleInitialChange(BorrowerReference reference)
		{
			//update the middle name if it was changed
			if (!reference.Demographics.MiddleIntial.ToUpper().Equals(RI.GetText(4, 53, 14)))
			{
				RI.PutText(4, 53, reference.Demographics.MiddleIntial, true);
				if (!UpdateVerified("01093", "01004"))
				{ return false; }
			}
			return true;
		}

		/// <summary>
		/// Checks the first name in the session against the form first name and updates the session if they are different
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>Returns true for a successful name change or no change.  Returns false for an unsuccessful name change.</returns>
		private bool FirstNameChange(BorrowerReference reference)
		{
			//update the first name if it was changed
			if (!reference.Demographics.FirstName.ToUpper().Equals(RI.GetText(4, 34, 14)))
			{
				RI.PutText(4, 34, reference.Demographics.FirstName, true);
				if (!UpdateVerified("01093", "01004"))
				{ return false; }
			}
			return true;
		}

		/// <summary>
		/// Creates a new reference, giving it a name and relationship
		/// </summary>
		/// <param name="reference">reference to add</param>
		/// <returns>true if successful</returns>
		private bool AddNewReferenceName(BorrowerReference reference)
		{
			RI.FastPath("TX3ZATX1JR");
			RI.PutText(7, 11, ssn);
			RI.PutText(4, 6, reference.Demographics.LastName, true);
			RI.PutText(4, 34, reference.Demographics.FirstName, true);
			RI.PutText(4, 53, reference.Demographics.MiddleIntial, true);
			RI.PutText(4, 72, reference.Suffix, true);
			RI.PutText(8, 15, dataAccess.GetRelationshipCodeForDescription(reference.Relationship), true);
			return true;
		}

		/// <summary>
		/// Takes a reference and updates it to have demographics if they were left blank
		/// </summary>
		/// <param name="reference">BorrowerReference object to update</param>
		private void GetMissingDemographics(BorrowerReference reference)
		{
			SystemBorrowerDemographics borrower = RI.GetDemographicsFromTx1j(accountNumber);

			reference.Demographics.Address1 = borrower.Address1;
			reference.Demographics.Address2 = borrower.Address2;
			reference.Demographics.City = borrower.City;
			reference.Demographics.State = borrower.State;
			reference.Demographics.ZipCode = borrower.ZipCode;
			reference.Demographics.Country = borrower.Country;
			reference.Demographics.IsValidAddress = borrower.IsValidAddress;
		}

        /// <summary>
        /// Verify that the update occured
        /// </summary>
        /// <param name="updatedCode">reflection interface code for a success on update (differs based on field being updated ex. 01093) </param>
		/// <param name="addedCode">reflection interfaces code for success on an add (differs based on field being updated ex. 01004)</param>
        /// <returns>true if update was successful</returns>
        private bool UpdateVerified(string updatedCode, string addedCode)
        {
            //commit changes
			RI.Hit(Key.Enter);

            //hit enter again to confirm the entry is not a duplicate
			if (RI.CheckForText(23, 2, "01079"))
            {
				RI.Hit(Key.Enter);
            }

            //return true if the specified code is displayed
			if (RI.CheckForText(23, 2, updatedCode) || RI.CheckForText(23, 2, addedCode))
            {
                return true;
            }
            //prompt the user and return false if the invalid source code error code is displayed
			else if (RI.CheckForText(23, 2, "90002"))
            {
                MessageBox.Show("Unable to update demographics because the source code is invalid.  You may have tried to update phone information using a source code that is only valid for address information.  Select a different source code and try again.", "Demographics Update Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
			else if (RI.CheckForText(23, 2, "90039"))
            {
                MessageBox.Show("Unable to update demographics because the source code is invalid. You may have tried to update email information using a source code that is only valid for address information. Select a different source code and try again.", "E-mail Update Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
			//prompt the user and return false if the specified error code is not displayed
            else
            {
                MessageBox.Show("Unable to update demographics.", "Demographics Update Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Creates a comment for the changes made to the reference
        /// </summary>
        /// <param name="reference">reference that was added</param>
        /// <param name="mode">Add or modify</param>
        /// <returns>concatenated comment string</returns>
        private string CompiledComment(BorrowerReference reference, UpdateMode mode)
        {
            //set up modify comment
            string comment = "Updated reference " + reference.Demographics.Ssn + " ";

            //reset comment for add
            if (mode.Equals(UpdateMode.Add))
            {
                comment = "Added reference " + reference.Demographics.Ssn + " " +
                          reference.Demographics.FirstName + " " +
                          reference.Demographics.MiddleIntial + " " +
                          reference.Demographics.LastName + " " +
                          reference.Suffix + " ";
            }

            //add demographic, phone, and e-mail text to comment (the same for modify or add)
            comment = comment +
                      reference.Demographics.Address1 + " " +
                      reference.Demographics.Address2 + " " +
                      reference.Demographics.City + " " +
                      reference.Demographics.State + " " +
                      reference.Demographics.ZipCode + " " +
                      reference.Demographics.Country +
                      " H: " +
                        reference.HomePhone.DomesticAreaCode + reference.HomePhone.DomesticPrefix + reference.HomePhone.DomesticLineNumber +
                        reference.HomePhone.ForeignCountryCode + reference.HomePhone.ForeignCityCode + reference.HomePhone.ForeignLocalNumber +
                      " A: " +
                        reference.AlternatePhone.DomesticAreaCode + reference.AlternatePhone.DomesticPrefix + reference.AlternatePhone.DomesticLineNumber +
                        reference.AlternatePhone.ForeignCountryCode + reference.AlternatePhone.ForeignCityCode + reference.AlternatePhone.ForeignLocalNumber +
                      " W: " +
                        reference.WorkPhone.DomesticAreaCode + reference.WorkPhone.DomesticPrefix + reference.WorkPhone.DomesticLineNumber +
                        reference.WorkPhone.ForeignCountryCode + reference.WorkPhone.ForeignCityCode + reference.WorkPhone.ForeignLocalNumber +
                      " EMAIL: " + reference.Demographics.EmailAddress;

            return comment;
        }
    }
}
