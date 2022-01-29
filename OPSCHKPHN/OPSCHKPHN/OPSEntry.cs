using System;
using System.Globalization;
using System.Windows.Forms;
using Uheaa.Common;

namespace OPSCHKPHN
{
	public class OPSEntry
	{

		public enum ValidityCheckResult
		{
			InvalidSetFocusToSSN,
			InvalidSetFocusToEffectiveDate,
			Valid
		}

		public enum PaymentOption
		{
			TotalAmountDue,
			MonthlyAmountDue,
			OtherPaymentAmount
		}

		public enum AccountType
		{
			Checking,
			Savings
		}

		public enum ConfirmationOptions
		{
			Letter,
			None
		}

		public string AccNumOrSSN { get; set; }
		public string AccountNumber { get; set; }
		public string RoutingNumber { get; set; }
		public string VerifyRoutingNumber { get; set; }
		public string BankAccountNumber { get; set; }
		public string VerifyBankAccountNumber { get; set; }
		public PaymentOption PayOpt { get; set; }
		public string PaymentAmount { get; set; }
		public AccountType AcctType { get; set; }
		public string AccountHolderName { get; set; }
		public string EffectiveDate { get; set; }
		public ConfirmationOptions ConfOpt { get; set; }
		public string AppendToTotalAmountDue { get; set; }
		public string AppendToMonthlyAmountDue { get; set; }
		public bool CalledByDUDE { get; set; }
		public string RPF { get; set; }
		/// <summary>
		/// Calculates the Account Type based off the current value in AcctType (enum).
		/// </summary>
		public string CalculatedAccountType
		{
			get
			{
				if (AcctType == AccountType.Checking)
				{
					return "Checking";
				}
				else
				{
					return "Savings";
				}
			}
		}
		public string TS24DOB { get; set; }
		public string TS24Name { get; set; }
		public string TS24SSN { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public OPSEntry()
		{
			AccNumOrSSN = string.Empty;
			RoutingNumber = string.Empty;
			VerifyRoutingNumber = string.Empty;
			BankAccountNumber = string.Empty;
			VerifyBankAccountNumber = string.Empty;
			PaymentAmount = string.Empty;
			AccountHolderName = string.Empty;
			EffectiveDate = DateTime.Today.ToString("MM/dd/yyyy");
			AppendToTotalAmountDue = "$0.00";
			AppendToMonthlyAmountDue = "$0.00";
			CalledByDUDE = false;
			AccountNumber = string.Empty;
			RPF = string.Empty;
		}

		/// <summary>
		/// Does data validation after entry form.
		/// </summary>
		/// <returns></returns>
		public ValidityCheckResult ValidDataCheck()
		{
			const string REQUIRED_DATE_FORMAT = @"^\d{2}/\d{2}/\d{4}$"; //"MM/dd/yyyy"

			PaymentAmount = PaymentAmount.Replace(",", "");
			string errorMsg = "";
			if (!System.Text.RegularExpressions.Regex.IsMatch(EffectiveDate, REQUIRED_DATE_FORMAT))
				errorMsg = string.Join(Environment.NewLine, "Effective date must be populated (format: MM/DD/YYYY)");
			else if (EffectiveDate != string.Empty)
			{
				DateTime testVal = new DateTime();
				if (DateTime.TryParseExact(EffectiveDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out testVal))
				{
					if (DateTime.Parse(EffectiveDate) < DateTime.Today || DateTime.Parse(EffectiveDate) >= DateTime.Today.AddDays(29)) //check if effective date is valid
						errorMsg = string.Join(Environment.NewLine, errorMsg, "The effective date must be today or after today, and must be less than 29 days in the future.");
					if (DateTime.Parse(EffectiveDate).DayOfWeek.IsIn(DayOfWeek.Saturday, DayOfWeek.Sunday)) //check if the effective data falls on a weekend
					{
						if (MessageBox.Show("The effective date you selected falls on a weekend.  The date has been changed to the following Monday.  Is this OK?  If yes, press yes.  If no, press no and you will be returned to the entry box to change the date.", "Effective Falls On Weekend", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
							return ValidityCheckResult.InvalidSetFocusToEffectiveDate;
						if (DateTime.Parse(EffectiveDate).DayOfWeek == DayOfWeek.Saturday)
							EffectiveDate = DateTime.Parse(EffectiveDate).AddDays(2).ToString("MM/dd/yyyy");
						else
							EffectiveDate = DateTime.Parse(EffectiveDate).AddDays(1).ToString("MM/dd/yyyy");
					}
				}
				else
					errorMsg = string.Join(Environment.NewLine, errorMsg, "Date not recognized as a valid date.");
			}
			if (double.Parse(PaymentAmount) > 25000.00 || double.Parse(PaymentAmount) <= 1.00) //check if payment amount is valid amount
				errorMsg = string.Join(Environment.NewLine, errorMsg, "The Payment Amount field must be less that $25,000.00, and greater than $1.00.");
			if (RoutingNumber != VerifyRoutingNumber) //check if double entry routing numbers match
				errorMsg = string.Join(Environment.NewLine, errorMsg, "The routing numbers don't match.");
			if (BankAccountNumber != VerifyBankAccountNumber) //check if double entry bank account numbers match
				errorMsg = string.Join(Environment.NewLine, errorMsg, "The banking account numbers don't match.");
			if (BankAccountNumber.IsNullOrEmpty() || VerifyBankAccountNumber.IsNullOrEmpty())
				errorMsg = string.Join(Environment.NewLine, errorMsg, "The banking account number cannot be blank.");
			if (!RoutingNumber.StartsWith("0") && !RoutingNumber.StartsWith("1") && !RoutingNumber.StartsWith("2") && !RoutingNumber.StartsWith("3")) //check if valid routing number is provided
				errorMsg = string.Join(Environment.NewLine, errorMsg, "The routing number must begin with a 0, 1, 2 or 3.");
			if (RoutingNumber.Length != 9 || VerifyRoutingNumber.Length != 9)
				errorMsg = string.Join(Environment.NewLine, errorMsg, "The routing number must be 9 digits.");
			if (AccountHolderName.IsNullOrEmpty())
				errorMsg = string.Join(Environment.NewLine, errorMsg, "The Account Holder Name must not be blank.");
			if (AccountNumber.IsNullOrEmpty() && AccNumOrSSN.IsNullOrEmpty())
				errorMsg = string.Join(Environment.NewLine, errorMsg, "The Account Number must not be blank.");
			if (AccountNumber.Length < 9 && AccNumOrSSN.Length < 9)
				errorMsg = string.Join(Environment.NewLine, errorMsg, "The Account Number field must have at least 9 characters.");
			if (errorMsg == "")
			{
				PaymentAmount = double.Parse(PaymentAmount).ToString("####0.00");//everything is good to go
				return ValidityCheckResult.Valid;
			}
			else
			{
				string msg = string.Join(Environment.NewLine,
					"In order to continue the following conditions must be met:",
					"- SSN or Account number must be at least 9 digits",
					"- Routing number must have a 9 digit number",
					"- Account number must be populated with numeric characters",
					"- Payment amount must be a dollar amount",
					"- Checking or Savings must be selected",
					"- Account Holder Name must be filled in",
					"- Effective date must be populated (format: MM/DD/YYYY)",
					"- Confirmation options must be selected",
					"",
					"The Following have not been met:",
					errorMsg);

				MessageBox.Show(msg, "Errors Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return ValidityCheckResult.InvalidSetFocusToSSN;
			}
		}
	}
}
