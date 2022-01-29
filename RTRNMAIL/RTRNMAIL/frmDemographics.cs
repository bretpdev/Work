using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace RTRNMAIL
{
	partial class frmDemographics : FormBase
	{
		private readonly bool TEST_MODE;
		private MailingAddress _forwardingAddress;

		/// <summary>
		/// DO NOT USE!!! The no-parameter constructor is requred by Visual Studio's Form Designer, but it will not work with the script.
		/// </summary>
		public frmDemographics()
		{
			InitializeComponent();
		}

		public frmDemographics(bool testMode, IdentifyingInfo recipientInfo, out MailingAddress forwardingAddress)
		{
			InitializeComponent();

			TEST_MODE = testMode;

			//Show the recipient's identifying information.
			lblAcctNo.Text = recipientInfo.AccountNumber;
			lblName.Text = recipientInfo.FullName;
			lblSsn.Text = recipientInfo.SSN;

			//Load up the combo box values.
			cmbState.DataSource = DataAccess.GetStateCodes();

			//Create a new MailingAddress for our out parameter and bind it to the form fields.
			forwardingAddress = new MailingAddress();
			_forwardingAddress = forwardingAddress;
			ForwardingAddressBindingSource.DataSource = _forwardingAddress;
		}

		private void btnContinue_Click(object sender, EventArgs e)
		{
			//Validate the selected demographics.
			List<string> errors = GetValidationErrors();
			if (errors.Count > 0)
			{
				StringBuilder messageBuilder = new StringBuilder();
				messageBuilder.Append("Please correct the following errors:");
				foreach (string error in errors)
				{
					messageBuilder.Append(Environment.NewLine);
					messageBuilder.Append(error);
				}
				MessageBox.Show(messageBuilder.ToString(), "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//Close the form with a result of "OK" if we're ready to go.
			DialogResult = DialogResult.OK;
		}//btnContinue_Click()

		private void btnDaRules_Click(object sender, EventArgs e)
		{
			new frmDaRules(TEST_MODE).ShowDialog();
		}//btnDaRules_Click()

		private List<string> GetValidationErrors()
		{
			char[] invalidCharacters = { '!', '@', '$', '%', '^', '&', '*', '(', ')', '-', '<', '>', ',', '.', '"', ';', ':', '~', '`', '?' };
			HashSet<string> errors = new HashSet<string>();

			//Address 1
			if (string.IsNullOrEmpty(_forwardingAddress.Address1))
			{
				errors.Add("Address 1 must be populated.");
			}
			else
			{
				foreach (char letter in _forwardingAddress.Address1)
				{
					if (invalidCharacters.Contains(letter))
					{
						errors.Add(string.Format("The Post Office doesn't allow \"{0}\" in address lines. Please remove the invalid character for address line 1.", letter.ToString()));
					}
				}//foreach
			}

			//Address 2
			if (!string.IsNullOrEmpty(_forwardingAddress.Address2))
			{
				foreach (char letter in _forwardingAddress.Address2)
				{
					if (invalidCharacters.Contains(letter))
					{
						errors.Add(string.Format("The Post Office doesn't allow \"{0}\" in address lines. Please remove the invalid character for address line 2.", letter.ToString()));
					}
				}//foreach
			}

			//City
			if (string.IsNullOrEmpty(_forwardingAddress.City))
			{
				errors.Add("City must be populated.");
			}

			//State
			if (string.IsNullOrEmpty(_forwardingAddress.State))
			{
				errors.Add("State must be populated.");
			}

			//Zip code
			if (string.IsNullOrEmpty(_forwardingAddress.Zip))
			{
				errors.Add("Zip Code must be populated.");
			}
			else
			{
				if (_forwardingAddress.Zip.Length != 5 && _forwardingAddress.Zip.Length != 9)
				{
					errors.Add("The zip code must be either 5 or 9 digits long.");
				}
				else if (_forwardingAddress.Zip.EndsWith("0000"))
				{
					errors.Add("The last four digits of the zip code can't be zeros.");
				}
			}

			return errors.ToList();
		}//GetValidationErrors()
	}//class
}//namespace
