using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;

namespace ADDMOREFED
{
    public partial class ReferenceInformation : Form
    {

        #region Class Level Variables and Constructor

        public BorrowerReference ReferenceData { get; set; }
        public AddOrModifyReferenceScriptFed.UpdateMode UpdateMode { get; set; }

        List<BorrowerReference> references;
        DataAccess dataAccess;
        bool controlsEnabled = false;  //used to prevent field updates if the user changes the source after making edits

        List<TextBox> homePhoneTextBoxes;
        List<TextBox> alternatePhoneTextBoxes;
        List<TextBox> workPhoneTextBoxes;

        List<TextBox> foreignHomePhoneTextBoxes;
        List<TextBox> foreignAlternatePhoneTextBoxes;
        List<TextBox> foreignWorkPhoneTextBoxes;

        List<CheckBox> notValidCheckBoxes;
        List<CheckBox> invalidateFirstCheckBoxes;

		/// <summary>
		/// Initializes form
		/// </summary>
		/// <param name="reference">list of borrower references</param>
		/// <param name="updateMode">Add or Modify</param>
		/// <param name="referenceIndexToDisplay">Current Reference being modified</param>
        public ReferenceInformation(List<BorrowerReference> reference, AddOrModifyReferenceScriptFed.UpdateMode updateMode, int referenceIndexToDisplay)
        {
            InitializeComponent();

            references = reference;
            dataAccess = new DataAccess();
            UpdateMode = updateMode;

            //text box lists for managing phone number fields
            homePhoneTextBoxes = new List<TextBox> { homeAreaCodeTextBox, homePrefixTextBox, homeNumberTextBox };
            alternatePhoneTextBoxes = new List<TextBox> { alternateAreaCodeTextBox, alternatePrefixTextBox, alternateNumberTextBox };
            workPhoneTextBoxes = new List<TextBox> { workAreaCodeTextBox, workPrefixTextBox, workNumberTextBox };
            foreignHomePhoneTextBoxes = new List<TextBox> { homeCountryPhoneTextBox, homeCityPhoneTextBox, homeLocalPhoneTextBox };
            foreignAlternatePhoneTextBoxes = new List<TextBox> { alternateCountryPhoneTextBox, alternateCityPhoneTextBox, alternateLocalPhoneTextBox };
            foreignWorkPhoneTextBoxes = new List<TextBox> { workCountryPhoneTextBox, workCityPhoneTextBox, workLocalPhoneTextBox };

            //check box lists for managing check boxes
            notValidCheckBoxes = new List<CheckBox> {chkAddressNotValid, chkHomePhoneNotValid, chkAlternatePhoneNotValid, chkWorkPhoneNotValid,
                                                      chkForeignHomePhoneNotValid, chkForeignAlternatePhoneNotValid, chkForeignWorkPhoneNotValid, chkEmailNotValid};
            invalidateFirstCheckBoxes = new List<CheckBox> { chkAddressInvalidate, chkAlternatePhoneInvalidate, chkEmailInvalidate, chkForeignAlternatePhoneInvalidate,
                                                              chkForeignHomePhoneInvalidate, chkForeignWorkPhoneInvalidate, chkHomePhoneInvalidate, chkWorkPhoneInvalidate};

            //populate list box with references
            foreach (BorrowerReference r in references)
            {
                selectReferenceList.Items.Add(r.Demographics.Ssn + " - " + r.Demographics.FirstName + " " + r.Demographics.LastName);
            }

            //populate source combo
            sourceComboBox.Items.Add(string.Empty);
            foreach (string s in dataAccess.GetAddressSourceDescriptions())
            {
                sourceComboBox.Items.Add(s);
            }

            //populate suffix combo
            suffixComboBox.Items.Add(string.Empty);
            foreach (string s in dataAccess.GetSuffixes())
            {
                suffixComboBox.Items.Add(s);
            }

            //populate relationship combo
            relationshipComboBox.Items.Add(string.Empty);
            foreach (string s in dataAccess.GetRelationshipDescriptions())
            {
                relationshipComboBox.Items.Add(s);
            }

            //populate state combo
            stateCodeComboBox.Items.Add(string.Empty);
            foreach (string s in dataAccess.GetStateCodes())
            {
                if (s != "FC") { stateCodeComboBox.Items.Add(s); }
            }

            //populate foreign country combo
            foreignCountryComboBox.Items.Add(string.Empty);
            foreach (string c in dataAccess.GetCountryNames())
            {
                foreignCountryComboBox.Items.Add(c);
            }

            //disable controls
            EnableDisableControls(false);

            //set up the form for adding a new reference
            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Add))
            {
                SetUpForAdd();
            }

            //populate the form with data for the selected reference (if there is one)
            if (!referenceIndexToDisplay.Equals(-1))
            {
                selectReferenceLabel.Visible = false;
                selectReferenceList.Visible = false;
                selectSourceLabel.Visible = false;
                sourceComboBox.Visible = false;
                continueButton.Visible = false;
                cancelButton.Visible = false;
                addReferenceButton.Visible = false;

                possibleDuplicateLabel.Visible = true;
                modifyReferenceButton.Visible = true;
                addNewReferenceButton.Visible = true;

                selectReferenceList.SelectedIndex = referenceIndexToDisplay;
            }
        }

        #endregion

        #region Reference Information Edits

        /// <summary>
        /// Makes sure first name has a value when left
        /// </summary>
        private void firstNameTextBox_Leave(object sender, EventArgs e)
        {
            if (firstNameTextBox.Text.Equals(string.Empty))
            {
                MessageBox.Show("The first name may not be left blank.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                firstNameTextBox.Select();
            }
            else
                OpenForAdding();
        }

		/// <summary>
		/// Makes sure last name has a value when left
		/// </summary>
        private void lastNameTextBox_Leave(object sender, EventArgs e)
        {
            if (lastNameTextBox.Text.Equals(string.Empty))
            {
                MessageBox.Show("The last name may not be left blank.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                    lastNameTextBox.Text = references[selectReferenceList.SelectedIndex].Demographics.LastName;

                lastNameTextBox.Select();
            }
            else if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Add))
                OpenForAdding();
        }

		/// <summary>
		/// Makes sure relationship has a value when selecting from the dropdown
		/// </summary>
        private void relationshipComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (relationshipComboBox.Text.Equals(string.Empty))
            {
                MessageBox.Show("The relationship may not be left blank.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                relationshipComboBox.Select();
            }
            else
            {
                OpenForAdding();
                street1TextBox.Select();
            }
        }

		/// <summary>
		/// Makes sure relationship has a value when left
		/// </summary>
		private void relationshipComboBox_Leave(object sender, EventArgs e)
        {
            if (relationshipComboBox.Text.Equals(string.Empty))
            {
                MessageBox.Show("The relationship may not be left blank.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                relationshipComboBox.Select();
            }
        }

		/// <summary>
		/// Enable fields for adding new reference demographic and phone data.
		/// </summary>
        private void OpenForAdding()
        {
            if (!lastNameTextBox.Text.Equals(string.Empty) && !firstNameTextBox.Text.Equals(string.Empty) && !relationshipComboBox.Text.Equals(string.Empty))
                EnableDisableControls(true);
        }

        #endregion

        #region Address Edits

		/// <summary>
		/// Makes sure Address1 has a value when left and updates address validity
		/// </summary>
        private void street1TextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = "";
            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].Demographics.Address1.ToUpper();

            if (street1TextBox.Text.Equals(string.Empty) && UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
            {
                MessageBox.Show("The Street Address Line 1 field may not be left blank.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                street1TextBox.Text = previousValue;
                street1TextBox.Select();
            }
            else if (!street1TextBox.Text.ToUpper().Equals(previousValue))
                UpdateAddressValidityIndicator();
        }

		/// <summary>
		/// Updates address validity if street2 is changed
		/// </summary>
        private void street2TextBox_Leave(object sender, EventArgs e)
        {
            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify) && !street2TextBox.Text.ToUpper().Equals(references[selectReferenceList.SelectedIndex].Demographics.Address2.ToUpper()))
                UpdateAddressValidityIndicator();
        }

		/// <summary>
		/// Makes sure city has a value when left and updates address validity
		/// </summary>
        private void cityTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = "";
            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].Demographics.City.ToUpper();

            if (cityTextBox.Text.Equals(string.Empty) && UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
            {
                MessageBox.Show("The City field may not be left blank.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cityTextBox.Text = previousValue;
                cityTextBox.Select();
            }
            else if (!cityTextBox.Text.ToUpper().Equals(previousValue))
                UpdateAddressValidityIndicator();
        }

		/// <summary>
		/// Makes sure state has a value when left unless a foreign country is selected.  Also updates address validity
		/// </summary>
        private void stateCodeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!stateCodeComboBox.Text.Equals(string.Empty) || UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Add))
            {
                stateNameText.Text = dataAccess.GetStateNameFromStateCode(stateCodeComboBox.Text);
                foreignStateTextBox.Text = string.Empty;
                foreignStateTextBox.Enabled = false;
                foreignCountryComboBox.Text = string.Empty;
                if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify) && !stateCodeComboBox.Text.ToUpper().Equals(references[selectReferenceList.SelectedIndex].Demographics.State.ToUpper()))
                {
                    UpdateAddressValidityIndicator();
                }
            }
            else if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
            {
                MessageBox.Show("The domestic state may not be left blank unless a foreign country is selected.  Select a foreign country and the script will blank the domestic state for you.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                stateCodeComboBox.Text = references[selectReferenceList.SelectedIndex].Demographics.State;
            }
        }

		/// <summary>
		/// Makes sure zipcode has a value when left.  Also validates zipcode length for domestic and foreign zipcodes.
		/// </summary>
        private void zipCodeTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = "";
            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].Demographics.ZipCode;

            if (zipCodeTextBox.Text.Equals(string.Empty) && UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
            {
                MessageBox.Show("The Zip field may not be left blank.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                zipCodeTextBox.Text = previousValue;
                zipCodeTextBox.Select();
            }
            else if (!foreignCountryComboBox.Text.Equals(string.Empty))
            {
                if (zipCodeTextBox.Text.Length < 4)
                {
                    MessageBox.Show("Unable to update the reference’s demographics because you didn’t provide the minimum characters for the Zip code.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    zipCodeTextBox.Select();
                }
            }
            else if (!stateCodeComboBox.Text.Equals(string.Empty))
            {
                if (zipCodeTextBox.Text.Length < 5)
                {
                    MessageBox.Show("Unable to update the reference’s demographics because you didn’t provide the minimum characters for the Zip code.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    zipCodeTextBox.Select();
                }
            }
            else if (!zipCodeTextBox.Text.Equals(previousValue))
                UpdateAddressValidityIndicator();
        }

		/// <summary>
		/// Updates address validity if foreign state is updated.
		/// </summary>
        private void foreignStateTextBox_Leave(object sender, EventArgs e)
        {
			if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify) && !foreignStateTextBox.Text.ToUpper().Equals(references[selectReferenceList.SelectedIndex].Demographics.State.ToUpper()))
                UpdateAddressValidityIndicator();
        }

		/// <summary>
		/// Makes sure foreignCountry has a value when left unless state is populated.
		/// </summary>
        private void foreignCountryComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!foreignCountryComboBox.Text.Equals(string.Empty) || UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Add))
            {
                stateCodeComboBox.Text = string.Empty;
                stateNameText.Text = string.Empty;
                foreignStateTextBox.Enabled = true;
                if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify) && !foreignCountryComboBox.Text.ToUpper().Equals(references[selectReferenceList.SelectedIndex].Demographics.Country.ToUpper()))
                    UpdateAddressValidityIndicator();
            }
            else if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
            {
                MessageBox.Show("The foreign country may not be left blank unless a domestic state is selected.  Select a domestic state and the script will blank the foreign country for you.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                foreignCountryComboBox.Text = references[selectReferenceList.SelectedIndex].Demographics.Country;
            }
        }

		/// <summary>
		/// Reset the address check boxes
		/// </summary>
        private void UpdateAddressValidityIndicator()
        {
            chkAddressNotValid.Checked = false;
            chkAddressNotValid.Enabled = false;
            chkAddressInvalidate.Enabled = true;
        }

        #endregion

        #region Phone Number/E-Mail Field Event Handlers

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void homeAreaCodeTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].HomePhone.DomesticAreaCode;

            LeftPhone(homeAreaCodeTextBox, homePhoneTextBoxes, chkHomePhoneNotValid, chkHomePhoneInvalidate, foreignHomePhoneTextBoxes, 3, "three", previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void homePrefixTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
				previousValue = references[selectReferenceList.SelectedIndex].HomePhone.DomesticPrefix;

            LeftPhone(homePrefixTextBox, homePhoneTextBoxes, chkHomePhoneNotValid, chkHomePhoneInvalidate, foreignHomePhoneTextBoxes, 3, "three", previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void homeNumberTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].HomePhone.DomesticLineNumber;

            LeftPhone(homeNumberTextBox, homePhoneTextBoxes, chkHomePhoneNotValid, chkHomePhoneInvalidate, foreignHomePhoneTextBoxes, 4, "four", previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void alternateAreaCodeTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].AlternatePhone.DomesticAreaCode;

            LeftPhone(alternateAreaCodeTextBox, alternatePhoneTextBoxes, chkAlternatePhoneNotValid, chkAlternatePhoneInvalidate, foreignAlternatePhoneTextBoxes, 3, "three", previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void alternatePrefixTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].AlternatePhone.DomesticPrefix;

            LeftPhone(alternatePrefixTextBox, alternatePhoneTextBoxes, chkAlternatePhoneNotValid, chkAlternatePhoneInvalidate, foreignAlternatePhoneTextBoxes, 3, "three", previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void alternateNumberTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].AlternatePhone.DomesticLineNumber;

            LeftPhone(alternateNumberTextBox, alternatePhoneTextBoxes, chkAlternatePhoneNotValid, chkAlternatePhoneInvalidate, foreignAlternatePhoneTextBoxes, 4, "four", previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void workAreaCodeTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].WorkPhone.DomesticAreaCode;

            LeftPhone(workAreaCodeTextBox, workPhoneTextBoxes, chkWorkPhoneNotValid, chkWorkPhoneInvalidate, foreignWorkPhoneTextBoxes, 3, "three", previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void workPrefixTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].WorkPhone.DomesticPrefix;

            LeftPhone(workPrefixTextBox, workPhoneTextBoxes, chkWorkPhoneNotValid, chkWorkPhoneInvalidate, foreignWorkPhoneTextBoxes, 3, "three", previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void workNumberTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].WorkPhone.DomesticLineNumber;

            LeftPhone(workNumberTextBox, workPhoneTextBoxes, chkWorkPhoneNotValid, chkWorkPhoneInvalidate, foreignWorkPhoneTextBoxes, 4, "four", previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void homeCountryPhoneTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].HomePhone.ForeignCountryCode;

            LeftPhone(homeCountryPhoneTextBox, foreignHomePhoneTextBoxes, chkForeignHomePhoneNotValid, chkForeignHomePhoneInvalidate, homePhoneTextBoxes, 0, string.Empty, previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void homeCityPhoneTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].HomePhone.ForeignCityCode;

            LeftPhone(homeCityPhoneTextBox, foreignHomePhoneTextBoxes, chkForeignHomePhoneNotValid, chkForeignHomePhoneInvalidate, homePhoneTextBoxes, 0, string.Empty, previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void homeLocalPhoneTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].HomePhone.ForeignLocalNumber;

            LeftPhone(homeLocalPhoneTextBox, foreignHomePhoneTextBoxes, chkForeignHomePhoneNotValid, chkForeignHomePhoneInvalidate, homePhoneTextBoxes, 9, "nine", previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void alternateCountryPhoneTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].AlternatePhone.ForeignCountryCode;

            LeftPhone(alternateCountryPhoneTextBox, foreignAlternatePhoneTextBoxes, chkForeignAlternatePhoneNotValid, chkForeignAlternatePhoneInvalidate, alternatePhoneTextBoxes, 0, string.Empty, previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void alternateCityPhoneTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].AlternatePhone.ForeignCityCode;

            LeftPhone(alternateCityPhoneTextBox, foreignAlternatePhoneTextBoxes, chkForeignAlternatePhoneNotValid, chkForeignAlternatePhoneInvalidate, alternatePhoneTextBoxes, 0, string.Empty, previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void alternateLocalPhoneTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].AlternatePhone.ForeignLocalNumber;

            LeftPhone(alternateLocalPhoneTextBox, foreignAlternatePhoneTextBoxes, chkForeignAlternatePhoneNotValid, chkForeignAlternatePhoneInvalidate, alternatePhoneTextBoxes, 9, "nine", previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void workCountryPhoneTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].WorkPhone.ForeignCountryCode;

            LeftPhone(workCountryPhoneTextBox, foreignWorkPhoneTextBoxes, chkForeignWorkPhoneNotValid, chkForeignWorkPhoneInvalidate, workPhoneTextBoxes, 0, string.Empty, previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void workCityPhoneTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].WorkPhone.ForeignCityCode;

            LeftPhone(workCityPhoneTextBox, foreignWorkPhoneTextBoxes, chkForeignWorkPhoneNotValid, chkForeignWorkPhoneInvalidate, workPhoneTextBoxes, 0, string.Empty, previousValue);
        }

		/// <summary>
		/// Stores the previous number if in modify mode and calls LeftPhone for generic handling of leaving the field.
		/// </summary>
        private void workLocalPhoneTextBox_Leave(object sender, EventArgs e)
        {
            string previousValue = string.Empty;

            if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                previousValue = references[selectReferenceList.SelectedIndex].WorkPhone.ForeignLocalNumber;

            LeftPhone(workLocalPhoneTextBox, foreignWorkPhoneTextBoxes, chkForeignWorkPhoneNotValid, chkForeignWorkPhoneInvalidate, workPhoneTextBoxes, 9, "nine", previousValue);
        }

		/// <summary>
		/// Validates the email format (ensures there is an @, no periods prior to the @, and no double periods).
		/// Updates the email checkboxes based on the evaluation.
		/// </summary>
        private void emailTextBox_Leave(object sender, EventArgs e)
        {
            if (emailTextBox.Text.Equals(string.Empty))
            {
                chkEmailNotValid.Checked = true;
                chkEmailNotValid.Enabled = false;
                chkEmailInvalidate.Checked = false;
                chkEmailInvalidate.Enabled = false;
            }
            else if (
                        (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify) && !emailTextBox.Text.Equals(references[selectReferenceList.SelectedIndex].Demographics.EmailAddress))
                        || UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Add)
                    )
            {
                int atPosition = emailTextBox.Text.IndexOf("@");
                int periodPosition = emailTextBox.Text.LastIndexOf(".");
                int doublePeriodPosition = emailTextBox.Text.IndexOf("..");

                if (atPosition.Equals(-1) || periodPosition <= atPosition || doublePeriodPosition > atPosition)
                {
                    MessageBox.Show("Unable to add the e-mail address because it is in the wrong format.  Please input a valid e-mail address or leave this field blank in order to continue.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    emailTextBox.Select();
                }
                else
                {
                    chkEmailNotValid.Checked = false;
                    chkEmailNotValid.Enabled = false;
                    chkEmailInvalidate.Checked = false;
                    chkEmailInvalidate.Enabled = true;
                }
            }
        }
        #endregion

        #region Phone Number Edit Methods

		/// <summary>
		/// Takes the current text box, the list of boxes considered grouped, the phone check box values, requirements on length of field, and the original value of the field.
		/// Updates the field if the entire group has valid data.
		/// </summary>
		/// <param name="phoneTextBox">current modified field</param>
		/// <param name="phoneTextBoxes">group of fields that make up the number</param>
		/// <param name="notValid">flag for a bad number</param>
		/// <param name="invalidateFirst">Use this to clear the fields before updating</param>
		/// <param name="counterPartTextBoxes">Used to ensure that if a domestic phone is added, the foreign phone field for the same type is disabled.</param>
		/// <param name="requiredLength">field requirement for length</param>
		/// <param name="requiredDigits"># of digits needed for field</param>
		/// <param name="originalValue">value prior to modification</param>
        private void LeftPhone(TextBox phoneTextBox, List<TextBox> phoneTextBoxes, CheckBox notValid, CheckBox invalidateFirst, List<TextBox> counterPartTextBoxes, int requiredLength, string requiredDigits, string originalValue)
        {
            int totalLength = 0;

            //calculate the total length of the text in the fields for the phone number
            if (requiredLength.Equals(9))
                totalLength = DigitsInPhoneNumber(phoneTextBoxes);
            else
            //get the length of the text in the field
                totalLength = phoneTextBox.Text.Length;

            if (!requiredLength.Equals(0) && !phoneTextBox.Text.Equals(string.Empty) && totalLength < requiredLength)
            {
                if (requiredLength < 9)
                {
                    MessageBox.Show("You must either blank the field or enter all " + requiredDigits + " digits for domestic phone numbers.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    phoneTextBox.Select();
                }
                else
                    MessageBox.Show("You must either blank all of the phone number fields or enter at least nine digits for foreign phone numbers.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //processing for when the field was blanked out
            else if (phoneTextBox.Text.Equals(string.Empty))
                PhoneChanged(true, phoneTextBoxes, notValid, invalidateFirst, counterPartTextBoxes);

            else if (!phoneTextBox.Text.Equals(originalValue))
            //processing for when the field was changed but not blanked out
                PhoneChanged(false, phoneTextBoxes, notValid, invalidateFirst, counterPartTextBoxes);
        }

		/// <summary>
		/// Determine how many digits are in the phone number currently
		/// </summary>
		/// <param name="phoneTextBoxes">group of textboxes representing the current phone number being modified</param>
		/// <returns>integer number of existing digits in number</returns>
        private int DigitsInPhoneNumber(List<TextBox> phoneTextBoxes)
        {
            int totalLength = 0;

            foreach (TextBox t in phoneTextBoxes)
            {
                totalLength = totalLength + t.Text.Length;
            }

            return totalLength;
        }

		/// <summary>
		/// Changes flags based on a phone number change
		/// </summary>
		/// <param name="wasBlankedOut">True if the field was cleared</param>
		/// <param name="phoneTextBoxes">Group of textboxes representing the modified phone number</param>
		/// <param name="notValid">Flag to update.  Set to checked if the number is bad.</param>
		/// <param name="invalidateFirst">Flag to update.  Flag is cleared.</param>
		/// <param name="counterPartTextBoxes">Used to keep phone numbers in 1 area (domestic or foreign)</param>
        private void PhoneChanged(bool wasBlankedOut, List<TextBox> phoneTextBoxes, CheckBox notValid, CheckBox invalidateFirst, List<TextBox> counterPartTextBoxes)
        {
            //the field was blanked out
            if (wasBlankedOut)
            {
                //blank all of the fields for the phone number
                foreach (TextBox t in phoneTextBoxes)
                {
                    t.Text = string.Empty;
                }

                //update check boxes
                notValid.Checked = true;
                notValid.Enabled = false;
                invalidateFirst.Checked = false;
                invalidateFirst.Enabled = false;

                //enable counterpart fields
                foreach (TextBox t in counterPartTextBoxes)
                {
                    t.Enabled = true;
                }
            }
            //field was changed
            else
            {
                //update check boxes
                notValid.Checked = false;
                notValid.Enabled = false;
                invalidateFirst.Enabled = true;

                //disable counterpart fields so the user can't enter both a domestic and a foreign phone number of the same type (H, A, W)
                foreach (TextBox t in counterPartTextBoxes)
                {
                    t.Enabled = false;
                }
            }
        }

        #endregion

        #region Load References and Enable Form

        /// <summary>
        /// When the selected reference changes, update the form
        /// </summary>
        private void selectReferenceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //abort if no item was selected
            if (selectReferenceList.SelectedIndex.Equals(-1))
            {
                return;
            }

            //populate address fields
            chkAddressNotValid.Checked = (!references[selectReferenceList.SelectedIndex].Demographics.IsValidAddress.Equals("Y"));
            txtReferenceId.Text = references[selectReferenceList.SelectedIndex].Demographics.Ssn;
            firstNameTextBox.Text = references[selectReferenceList.SelectedIndex].Demographics.FirstName;
            middleNameTextBox.Text = references[selectReferenceList.SelectedIndex].Demographics.MiddleIntial;
            lastNameTextBox.Text = references[selectReferenceList.SelectedIndex].Demographics.LastName;
            suffixComboBox.Text = references[selectReferenceList.SelectedIndex].Suffix;
            if (!suffixComboBox.Text.Equals(references[selectReferenceList.SelectedIndex].Suffix))
            {
                suffixComboBox.Text = string.Empty;
            }
            relationshipComboBox.Text = dataAccess.GetRelationshipDescriptionForCode(references[selectReferenceList.SelectedIndex].Relationship);
            street1TextBox.Text = references[selectReferenceList.SelectedIndex].Demographics.Address1;
            street2TextBox.Text = references[selectReferenceList.SelectedIndex].Demographics.Address2;
            cityTextBox.Text = references[selectReferenceList.SelectedIndex].Demographics.City;
            zipCodeTextBox.Text = references[selectReferenceList.SelectedIndex].Demographics.ZipCode;
            if (references[selectReferenceList.SelectedIndex].Demographics.Country.Equals(string.Empty))
            {
                stateCodeComboBox.Text = references[selectReferenceList.SelectedIndex].Demographics.State;
                stateNameText.Text = dataAccess.GetStateNameFromStateCode(references[selectReferenceList.SelectedIndex].Demographics.State);
                foreignStateTextBox.Text = string.Empty;
                foreignCountryComboBox.Text = string.Empty;
            }
            else
            {
                stateCodeComboBox.Text = string.Empty;
                stateNameText.Text = string.Empty;
                foreignStateTextBox.Text = references[selectReferenceList.SelectedIndex].Demographics.State;
                foreignCountryComboBox.Text = references[selectReferenceList.SelectedIndex].Demographics.Country;
            }

            //populate domestic home phone fields
            homeAreaCodeTextBox.Text = references[selectReferenceList.SelectedIndex].HomePhone.DomesticAreaCode;
            homePrefixTextBox.Text = references[selectReferenceList.SelectedIndex].HomePhone.DomesticPrefix;
            homeNumberTextBox.Text = references[selectReferenceList.SelectedIndex].HomePhone.DomesticLineNumber;
            chkHomePhoneNotValid.Checked = (homeAreaCodeTextBox.Text.Equals(string.Empty) || homePrefixTextBox.Text.Equals(string.Empty) || homeNumberTextBox.Text.Equals(string.Empty) ||
                                            references[selectReferenceList.SelectedIndex].HomePhone.ValidityIndicator != "Y");

            //populate domestic alternate phone fields
            alternateAreaCodeTextBox.Text = references[selectReferenceList.SelectedIndex].AlternatePhone.DomesticAreaCode;
            alternatePrefixTextBox.Text = references[selectReferenceList.SelectedIndex].AlternatePhone.DomesticPrefix;
            alternateNumberTextBox.Text = references[selectReferenceList.SelectedIndex].AlternatePhone.DomesticLineNumber;
            chkAlternatePhoneNotValid.Checked = (alternateAreaCodeTextBox.Text.Equals(string.Empty) || alternatePrefixTextBox.Text.Equals(string.Empty) || alternateNumberTextBox.Text.Equals(string.Empty) ||
                                                 references[selectReferenceList.SelectedIndex].AlternatePhone.ValidityIndicator != "Y");

            //populate domestic work phone fields
            workAreaCodeTextBox.Text = references[selectReferenceList.SelectedIndex].WorkPhone.DomesticAreaCode;
            workPrefixTextBox.Text = references[selectReferenceList.SelectedIndex].WorkPhone.DomesticPrefix;
            workNumberTextBox.Text = references[selectReferenceList.SelectedIndex].WorkPhone.DomesticLineNumber;
            chkWorkPhoneNotValid.Checked = (workAreaCodeTextBox.Text.Equals(string.Empty) || workPrefixTextBox.Text.Equals(string.Empty) || workNumberTextBox.Text.Equals(string.Empty) ||
                                             references[selectReferenceList.SelectedIndex].WorkPhone.ValidityIndicator != "Y");

            //populate foreign home phone fields
            homeCountryPhoneTextBox.Text = references[selectReferenceList.SelectedIndex].HomePhone.ForeignCountryCode;
            homeCityPhoneTextBox.Text = references[selectReferenceList.SelectedIndex].HomePhone.ForeignCityCode;
            homeLocalPhoneTextBox.Text = references[selectReferenceList.SelectedIndex].HomePhone.ForeignLocalNumber;
            chkForeignHomePhoneNotValid.Checked = (homeCountryPhoneTextBox.Text.Equals(string.Empty) || homeCityPhoneTextBox.Text.Equals(string.Empty) || homeLocalPhoneTextBox.Text.Equals(string.Empty) ||
                                                   references[selectReferenceList.SelectedIndex].HomePhone.ValidityIndicator != "Y");

            //populate foreign alternate phone fields
            alternateCountryPhoneTextBox.Text = references[selectReferenceList.SelectedIndex].AlternatePhone.ForeignCountryCode;
            alternateCityPhoneTextBox.Text = references[selectReferenceList.SelectedIndex].AlternatePhone.ForeignCityCode;
            alternateLocalPhoneTextBox.Text = references[selectReferenceList.SelectedIndex].AlternatePhone.ForeignLocalNumber;
            chkForeignAlternatePhoneNotValid.Checked = (alternateCountryPhoneTextBox.Text.Equals(string.Empty) || alternateCityPhoneTextBox.Text.Equals(string.Empty) || alternateLocalPhoneTextBox.Text.Equals(string.Empty) ||
                                                        references[selectReferenceList.SelectedIndex].AlternatePhone.ValidityIndicator != "Y");

            //populate foreign work phone fields
            workCountryPhoneTextBox.Text = references[selectReferenceList.SelectedIndex].WorkPhone.ForeignCountryCode;
            workCityPhoneTextBox.Text = references[selectReferenceList.SelectedIndex].WorkPhone.ForeignCityCode;
            workLocalPhoneTextBox.Text = references[selectReferenceList.SelectedIndex].WorkPhone.ForeignLocalNumber;
            chkForeignWorkPhoneNotValid.Checked = (workCountryPhoneTextBox.Text.Equals(string.Empty) || workCityPhoneTextBox.Text.Equals(string.Empty) || workLocalPhoneTextBox.Text.Equals(string.Empty) ||
                                                   references[selectReferenceList.SelectedIndex].WorkPhone.ValidityIndicator != "Y");

            //populate e-mail fields
            emailTextBox.Text = references[selectReferenceList.SelectedIndex].Demographics.EmailAddress;
            chkEmailNotValid.Checked = (emailTextBox.Text.Equals(string.Empty) || !references[selectReferenceList.SelectedIndex].Demographics.IsValidEmail);

            //if a source has been selected, reenble controls as the controls which should be enabled will have changed based on the new data
            if (!sourceComboBox.Text.Equals(string.Empty))
            {
                EnableDisableControls(true);
            }
        }

		/// <summary>
		/// Update the form when the source is changed.  Note: a reference must be selected before modifying the source.
		/// </summary>
        private void sourceComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //warn the user if no item was selected and in modify mode
            if (selectReferenceList.SelectedIndex.Equals(-1) && UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
            {
                MessageBox.Show("You must select a reference to modify before selecting the source.", "Reference not Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sourceComboBox.Text = string.Empty;
            }
            //disable controls if the source is blanked out
            else if (sourceComboBox.Text.Equals(string.Empty))
            {
                EnableDisableControls(false);
                controlsEnabled = false;
            }
            //enable appropriate controls for adding a new reference
            else if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Add))
            {
                foreach (Control c in grpReference.Controls)
                {
                    c.Enabled = true;
                }
                txtReferenceId.Enabled = false;
            }
            //enable appropriate controls for modifying a reference
            else if (!controlsEnabled)
            {
                EnableDisableControls(true);
                controlsEnabled = true;
            }
        }

        /// <summary>
        /// Enable or disable group controls on form.
        /// </summary>
        /// <param name="mode">True for enable or false for disable.</param>
        private void EnableDisableControls(bool mode)
        {
            //disable the reference information controls if enabled mode = false
            if (!mode)
            {
                foreach (Control c in grpReference.Controls)
                {
                    c.Enabled = mode;
                }
            }
            //enable appropriate reference information controls for editing if enabled mode = true
            else
            {
                lastNameTextBox.Enabled = mode;
                suffixComboBox.Enabled = mode;
                firstNameTextBox.Enabled = mode;
                middleNameTextBox.Enabled = mode;
                relationshipComboBox.Enabled = mode;
            }

            //enable or disable address controls
            foreach (Control c in grpAddress.Controls)
            {
                if (c.Name != "stateNameText" && c.Name != "foreignStateTextBox")
                {
                    c.Enabled = mode;
                }
            }

            //enable or disable state and foreign state controls so only domestic or foreign information can be entered
            if (stateCodeComboBox.Text.Equals(string.Empty) && mode)
            {
                foreignStateTextBox.Enabled = mode;
            }
            else
            {
                foreignStateTextBox.Enabled = false;
            }

            //enable or disable phone controls
            foreach (Control c in grpPhone.Controls)
            {
                c.Enabled = mode;
            }

            //enable or disable phone text fields so only domestic or foreign information can be entered
            if (mode)
            {
                ResetPhoneFields(homePhoneTextBoxes, chkHomePhoneNotValid, foreignHomePhoneTextBoxes);
                ResetPhoneFields(alternatePhoneTextBoxes, chkAlternatePhoneNotValid, foreignAlternatePhoneTextBoxes);
                ResetPhoneFields(workPhoneTextBoxes, chkWorkPhoneNotValid, foreignWorkPhoneTextBoxes);
                ResetPhoneFields(foreignHomePhoneTextBoxes, chkForeignHomePhoneNotValid, homePhoneTextBoxes);
                ResetPhoneFields(foreignAlternatePhoneTextBoxes, chkForeignAlternatePhoneNotValid, alternatePhoneTextBoxes);
                ResetPhoneFields(foreignWorkPhoneTextBoxes, chkForeignWorkPhoneNotValid, workPhoneTextBoxes);
            }

            //disable the e-mail not valid field if the e-mail is blank
            if (emailTextBox.Text.Equals(string.Empty))
            {
                chkEmailNotValid.Enabled = false;
            }

            //reset all of the invalidate first check boxes to false (the user shouldn't be able click them until the data is changed)
            ResetInvalidateFirstCheckBoxes();
        }

        /// <summary>
        /// Modifies the form to be in Add mode
        /// </summary>
        private void SetUpForAdd()
        {
            this.Text = "Add a New Reference";
            selectReferenceLabel.Visible = false;
            selectReferenceList.Visible = false;
            addReferenceButton.Visible = false;

            selectSourceLabel.Location = new System.Drawing.Point(12, 14);
            selectSourceLabel.Text = "Select the address source to enable the form for editing and add a reference.";
            sourceComboBox.Location = new System.Drawing.Point(12, 76);

            EnableDisableControls(false);
            HideShowCheckBoxes(false);
        }

        /// <summary>
        /// Changes availability of each phone number field so that only a set of local or foreign phone numbers may be added, but not both.
        /// </summary>
        /// <param name="phoneTextBoxes">Phone number fields to allow access to</param>
        /// <param name="notValid">Updated to not be enabled if the phoneTextBoxes field is blank</param>
        /// <param name="counterPartPhoneTextBoxes">Phone number fields to block access to</param>
        private void ResetPhoneFields(List<TextBox> phoneTextBoxes, CheckBox notValid, List<TextBox> counterPartPhoneTextBoxes)
        {
            //home phone is blank so not valid can't be changed
            if (phoneTextBoxes[0].Text.Equals(string.Empty) || phoneTextBoxes[1].Text.Equals(string.Empty) || phoneTextBoxes[2].Text.Equals(string.Empty))
            {
                notValid.Enabled = false;
            }
            //home phone is not blank so counterpart phone should be disabled
            else
            {
                foreach (TextBox t in counterPartPhoneTextBoxes)
                {
                    t.Enabled = false;
                }
            }
        }

		/// <summary>
		/// Reset all of the invalidate first check boxes to false
		/// </summary>
        private void ResetInvalidateFirstCheckBoxes()
        {
            foreach (CheckBox c in invalidateFirstCheckBoxes)
            {
                c.Checked = false;
                c.Enabled = false;
            }
        }

        /// <summary>
        /// Change visibility of check boxes.
        /// </summary>
        /// <param name="mode">True to show and false to hide.</param>
        private void HideShowCheckBoxes(bool mode)
        {
            foreach (CheckBox c in invalidateFirstCheckBoxes)
            {
                c.Visible = mode;
            }

            foreach (CheckBox c in notValidCheckBoxes)
            {
                c.Visible = mode;
            }

            lblNot.Visible = mode;
            lblValid.Visible = mode;
            lblInvalidate.Visible = mode;
            lblFirst.Visible = mode;
        }

        #endregion

        #region Button Clicks

		/// <summary>
		/// Final validation is done before continuing.
		/// </summary>
        private void continueButton_Click(object sender, EventArgs e)
        {
            bool dataIsValid = true;

            //validate fields
            if (sourceComboBox.Text.Equals(string.Empty))
            {
                MessageBox.Show("The address source may not be left blank.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sourceComboBox.Select();
                dataIsValid = false;
            }
            else if (!stateCodeComboBox.Text.Equals(string.Empty) && zipCodeTextBox.Text.Length < 5)
            {
                MessageBox.Show("The zip code must be at least five digits for a domestic address.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                zipCodeTextBox.Select();
                dataIsValid = false;
            }
            else if (!foreignCountryComboBox.Text.Equals(string.Empty) && zipCodeTextBox.Text.Length < 4)
            {
                MessageBox.Show("The zip code must be at least four digits for a foreign address.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                zipCodeTextBox.Select();
                dataIsValid = false;
            }
            else if ((!DigitsInPhoneNumber(homePhoneTextBoxes).Equals(0) && DigitsInPhoneNumber(homePhoneTextBoxes) < 10) ||
                     (!DigitsInPhoneNumber(alternatePhoneTextBoxes).Equals(0) && DigitsInPhoneNumber(alternatePhoneTextBoxes) < 10) ||
                     (!DigitsInPhoneNumber(workPhoneTextBoxes).Equals(0) && DigitsInPhoneNumber(workPhoneTextBoxes) < 10))
            {
                MessageBox.Show("Domestic phone numbers must include all ten digits.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                homeAreaCodeTextBox.Select();
                dataIsValid = false;
            }
            else if ((!DigitsInPhoneNumber(foreignHomePhoneTextBoxes).Equals(0) && DigitsInPhoneNumber(foreignHomePhoneTextBoxes) < 9) ||
                     (!DigitsInPhoneNumber(foreignAlternatePhoneTextBoxes).Equals(0) && DigitsInPhoneNumber(foreignAlternatePhoneTextBoxes) < 9) ||
                     (!DigitsInPhoneNumber(foreignWorkPhoneTextBoxes).Equals(0) && DigitsInPhoneNumber(foreignWorkPhoneTextBoxes) < 9))
            {
                MessageBox.Show("Foreign phone numbers must include at least nine digits.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                homeCountryPhoneTextBox.Select();
                dataIsValid = false;
            }
            else if (street1TextBox.Text.Equals(string.Empty) || cityTextBox.Text.Equals(string.Empty) || (stateCodeComboBox.Text.Equals(string.Empty) && foreignCountryComboBox.Text.Equals(string.Empty)))
            {
                if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Modify))
                {
                    MessageBox.Show("The street address line 1, the city, and either the domestic state or the foreign field must be populated.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    street1TextBox.Select();
                    dataIsValid = false;
                }
                else if (chkNoAddress.CheckState.Equals(CheckState.Unchecked))
                {
                    if (MessageBox.Show("You haven’t provided an address for the reference. Would you still like to continue? The script will update the reference’s address to match the borrower’s address if you continue.", "Invalid Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
                    {
                        street1TextBox.Select();
                        dataIsValid = false;
                    }
                }
            }

            if (dataIsValid)
            {
                DialogResult = DialogResult.OK;

                //compare the new reference to existing references
                if (UpdateMode.Equals(AddOrModifyReferenceScriptFed.UpdateMode.Add))
                {
                    foreach (CheckBox c in notValidCheckBoxes)
                    {
                        c.Checked = false;
                    }

                    //compare new reference against existing references to determine if the new reference may be a duplicate
                    int duplicateReferenceIndex = GetDuplicateReferenceIndex();

                    //display the possible duplicate for the user to review
                    if (!duplicateReferenceIndex.Equals(-1))
                    {
                        ReferenceInformation reviewDuplicateReference = new ReferenceInformation(references, AddOrModifyReferenceScriptFed.UpdateMode.Modify, duplicateReferenceIndex);

                        reviewDuplicateReference.ShowDialog();

                        //reset the form so the user can modify the existing reference
                        if (reviewDuplicateReference.DialogResult.Equals(DialogResult.Yes))
                        {
                            this.Text = "Modify Reference Information";
                            selectSourceLabel.Text = "Select the address source to enable the form for editing and modify the reference.";
                            UpdateMode = AddOrModifyReferenceScriptFed.UpdateMode.Modify;
                            if (selectReferenceList.SelectedIndex.Equals(duplicateReferenceIndex))
                            {
                                selectReferenceList_SelectedIndexChanged(null, null);
                            }
                            else
                            {
                                selectReferenceList.SelectedIndex = duplicateReferenceIndex;
                            }

                            firstNameTextBox.Enabled = false;
                            middleNameTextBox.Enabled = false;
                            relationshipComboBox.Enabled = false;
                            HideShowCheckBoxes(true);
                            DialogResult = DialogResult.None;
                        }
                    }
                }

                //pass reference data from the form fields to a BorrowerReference object
                PopulateReferenceData();
            }
        }

        /// <summary>
        /// Creates a borrower reference object from the data on the form for comparison to the session later.
        /// </summary>
        private void PopulateReferenceData()
        {
            BorrowerReference tempReferenceData = new BorrowerReference();

            tempReferenceData.Demographics.Ssn = txtReferenceId.Text;
            tempReferenceData.Demographics.FirstName = firstNameTextBox.Text;
            tempReferenceData.Demographics.MiddleIntial = middleNameTextBox.Text;
            tempReferenceData.Demographics.LastName = lastNameTextBox.Text;
            tempReferenceData.Suffix = suffixComboBox.Text;
            tempReferenceData.Relationship = relationshipComboBox.Text;

            tempReferenceData.Demographics.IsValidAddress = chkAddressNotValid.CheckState.Equals(CheckState.Checked) ? false : true;
            
            tempReferenceData.AddressInvalidateFirst = chkAddressInvalidate.CheckState;

            if (chkNoAddress.CheckState.Equals(CheckState.Unchecked))
            {
                tempReferenceData.Demographics.Address1 = street1TextBox.Text;
				tempReferenceData.Demographics.Address2 = street2TextBox.Text;
                tempReferenceData.Demographics.City = cityTextBox.Text;
                
				if (!stateCodeComboBox.Text.Equals(string.Empty))
                    tempReferenceData.Demographics.State = stateCodeComboBox.Text;
                else if (!foreignStateTextBox.Text.Equals(string.Empty))
                    tempReferenceData.Demographics.State = foreignStateTextBox.Text;
                else
                    tempReferenceData.Demographics.State = string.Empty;

                tempReferenceData.Demographics.ZipCode = zipCodeTextBox.Text;
                tempReferenceData.Demographics.Country = foreignCountryComboBox.Text;
            }
            else
            {
				tempReferenceData.Demographics.Address1 = "00000";
				tempReferenceData.Demographics.Address2 = string.Empty;
                tempReferenceData.Demographics.City = "FPO";
                tempReferenceData.Demographics.State = "AP";
                tempReferenceData.Demographics.ZipCode = "00000";
                tempReferenceData.Demographics.Country = string.Empty;
                tempReferenceData.Demographics.IsValidAddress = false;
            }

            tempReferenceData.HomePhone.DomesticAreaCode = homeAreaCodeTextBox.Text;
            tempReferenceData.HomePhone.DomesticPrefix = homePrefixTextBox.Text;
            tempReferenceData.HomePhone.DomesticLineNumber = homeNumberTextBox.Text;
            tempReferenceData.HomePhone.ForeignCountryCode = homeCountryPhoneTextBox.Text;
            tempReferenceData.HomePhone.ForeignCityCode = homeCityPhoneTextBox.Text;
            tempReferenceData.HomePhone.ForeignLocalNumber = homeLocalPhoneTextBox.Text;
            tempReferenceData.HomePhone.ValidityIndicator = GetValidityIndicator(homePhoneTextBoxes, chkHomePhoneNotValid, foreignHomePhoneTextBoxes, chkForeignHomePhoneNotValid);
            tempReferenceData.HomePhoneInvalidateFirst = GetInvalidateFirstIndicator(homePhoneTextBoxes, chkHomePhoneInvalidate, foreignHomePhoneTextBoxes, chkForeignHomePhoneInvalidate);

            tempReferenceData.AlternatePhone.DomesticAreaCode = alternateAreaCodeTextBox.Text;
            tempReferenceData.AlternatePhone.DomesticPrefix = alternatePrefixTextBox.Text;
            tempReferenceData.AlternatePhone.DomesticLineNumber = alternateNumberTextBox.Text;
            tempReferenceData.AlternatePhone.ForeignCountryCode = alternateCountryPhoneTextBox.Text;
            tempReferenceData.AlternatePhone.ForeignCityCode = alternateCityPhoneTextBox.Text;
            tempReferenceData.AlternatePhone.ForeignLocalNumber = alternateLocalPhoneTextBox.Text;
            tempReferenceData.AlternatePhone.ValidityIndicator = GetValidityIndicator(alternatePhoneTextBoxes, chkAlternatePhoneNotValid, foreignAlternatePhoneTextBoxes, chkForeignAlternatePhoneNotValid);
            tempReferenceData.AlternatePhoneInvalidateFirst = GetInvalidateFirstIndicator(alternatePhoneTextBoxes, chkAlternatePhoneInvalidate, foreignAlternatePhoneTextBoxes, chkForeignAlternatePhoneInvalidate);

            tempReferenceData.WorkPhone.DomesticAreaCode = workAreaCodeTextBox.Text;
            tempReferenceData.WorkPhone.DomesticPrefix = workPrefixTextBox.Text;
            tempReferenceData.WorkPhone.DomesticLineNumber = workNumberTextBox.Text;
            tempReferenceData.WorkPhone.ForeignCountryCode = workCountryPhoneTextBox.Text;
            tempReferenceData.WorkPhone.ForeignCityCode = workCityPhoneTextBox.Text;
            tempReferenceData.WorkPhone.ForeignLocalNumber = workLocalPhoneTextBox.Text;
            tempReferenceData.WorkPhone.ValidityIndicator = GetValidityIndicator(workPhoneTextBoxes, chkWorkPhoneNotValid, foreignWorkPhoneTextBoxes, chkForeignWorkPhoneNotValid);
            tempReferenceData.WorkPhoneInvalidateFirst = GetInvalidateFirstIndicator(workPhoneTextBoxes, chkWorkPhoneInvalidate, foreignWorkPhoneTextBoxes, chkForeignWorkPhoneInvalidate);

            tempReferenceData.Demographics.EmailAddress = emailTextBox.Text;
			tempReferenceData.Demographics.IsValidEmail = chkEmailNotValid.CheckState.Equals(CheckState.Checked) ? false : true;
            tempReferenceData.EmailInvalidateFirst = chkEmailInvalidate.CheckState;

            tempReferenceData.Source = sourceComboBox.Text;

            ReferenceData = tempReferenceData;
        }

        /// <summary>
        /// Returns a Y/N indicator of the validity of phones
        /// </summary>
        /// <param name="domesticTextBoxes">Phone text boxes for domestic</param>
        /// <param name="domesticCheckBox">Current text box if domestic</param>
		/// <param name="foreignTextBoxes">Phone text boxes for foreign</param>
		/// <param name="foreignCheckBox">Current text box if foreign</param>
        /// <returns>Y or N</returns>
        private string GetValidityIndicator(List<TextBox> domesticTextBoxes, CheckBox domesticCheckBox, List<TextBox> foreignTextBoxes, CheckBox foreignCheckBox)
        {
            System.Windows.Forms.CheckState checkedState = CheckState.Unchecked;

            //determine which check box to use for phones (domestic or foreign)
            if (!domesticTextBoxes[0].Text.Equals(string.Empty))
                checkedState = domesticCheckBox.CheckState;
            else if (!foreignTextBoxes[0].Text.Equals(string.Empty) && !foreignTextBoxes[1].Equals(string.Empty) && !foreignTextBoxes[2].Equals(string.Empty))
                checkedState = foreignCheckBox.CheckState;

            //convert checked state to Compass string equivalent
            if (checkedState.Equals(CheckState.Checked))
                return "N";
            else
                return "Y";
        }

		/// <summary>
		/// Returns a CheckState of the domestic side if not empty or the foreign side if it is not empty.
		/// </summary>
		/// <param name="domesticTextBoxes">Phone text boxes for domestic</param>
		/// <param name="domesticCheckBox">Current text box if domestic</param>
		/// <param name="foreignTextBoxes">Phone text boxes for foreign</param>
		/// <param name="foreignCheckBox">Current text box if foreign</param>
		/// <returns>CheckState of either the domestic or foreign checkBox</returns>
        private System.Windows.Forms.CheckState GetInvalidateFirstIndicator(List<TextBox> domesticTextBoxes, CheckBox domesticCheckBox, List<TextBox> foreignTextBoxes, CheckBox foreignCheckBox)
        {
            if (!domesticTextBoxes[0].Text.Equals(String.Empty))
            {
                return domesticCheckBox.CheckState;
            }
            else if (!foreignTextBoxes[0].Text.Equals(string.Empty) && !foreignTextBoxes[1].Equals(string.Empty) && !foreignTextBoxes[2].Equals(string.Empty))
            {
                return foreignCheckBox.CheckState;
            }
            else
            {
                return CheckState.Unchecked;
            }
        }

        /// <summary>
		/// Reset the form when the Add Reference button is clicked
        /// </summary>
        private void addReferenceButton_Click(object sender, EventArgs e)
        {
            UpdateMode = AddOrModifyReferenceScriptFed.UpdateMode.Add;

            foreach (Control c in grpReference.Controls.OfType<TextBox>())
            {
                c.Text = string.Empty;
            }
            suffixComboBox.Text = string.Empty;
            relationshipComboBox.Text = string.Empty;

            foreach (Control c in grpAddress.Controls.OfType<TextBox>())
            {
                c.Text = string.Empty;
            }
            stateCodeComboBox.Text = string.Empty;
            foreignCountryComboBox.Text = string.Empty;

            foreach (Control c in grpPhone.Controls.OfType<TextBox>())
            {
                c.Text = string.Empty;
            }

            sourceComboBox.Text = string.Empty;
            SetUpForAdd();
        }

        #endregion

        #region Processing Methods

		/// <summary>
		/// Compare the new reference to existing references and return the index of possible duplicate existing references
		/// </summary>
		/// <returns>Integer index of an existing reference that may be a duplicate</returns>
        private int GetDuplicateReferenceIndex()
        {
            int duplicateReferenceIndex = -1;

            if (!references.Count.Equals(0))
            {
                for (int i = 0; i < references.Count; i++)
                {
                    BorrowerReference r = references[i];

					//check firstname and address1 for being the same
                    if (r.Demographics.FirstName.ToUpper().Equals(firstNameTextBox.Text.ToUpper()) &&
                        r.Demographics.Address1.SafeSubString(0, 4).ToUpper().Equals(street1TextBox.Text.SafeSubString(0, 4).ToUpper()))
                    {
                        duplicateReferenceIndex = i;
                    }
					//check first and last name being the same
                    else if (r.Demographics.FirstName.SafeSubString(0, 4).ToUpper().Equals(firstNameTextBox.Text.SafeSubString(0, 4).ToUpper()) &&
                             r.Demographics.LastName.SafeSubString(0, 4).ToUpper().Equals(lastNameTextBox.Text.SafeSubString(0, 4).ToUpper()))
                    {
                        duplicateReferenceIndex = i;
                    }
					//check homephone number as being the same
                    else if (!homeAreaCodeTextBox.Text.Equals(string.Empty) &&
                             (r.HomePhone.DomesticAreaCode.Equals(homeAreaCodeTextBox.Text) &&
                              r.HomePhone.DomesticPrefix.Equals(homePrefixTextBox.Text) &&
                              r.HomePhone.DomesticLineNumber.Equals(homeNumberTextBox.Text))
                            )
                    {
                        duplicateReferenceIndex = i;
                    }
					//check firstname, city, and state as being the same
                    else if (r.Demographics.FirstName.SafeSubString(0, 4).ToUpper().Equals(firstNameTextBox.Text.SafeSubString(0, 4).ToUpper()) &&
                             r.Demographics.City.SafeSubString(0, 4).ToUpper().Equals(cityTextBox.Text.SafeSubString(0, 4).ToUpper()) &&
                             (r.Demographics.State.SafeSubString(0, 4).ToUpper().Equals(foreignStateTextBox.Text.SafeSubString(0, 4).ToUpper()) ||
                              r.Demographics.State.Equals(stateCodeComboBox.Text)))
                    {
                        duplicateReferenceIndex = i;
                    }
                }
            }

            return duplicateReferenceIndex;
        }

        #endregion
    }
}
