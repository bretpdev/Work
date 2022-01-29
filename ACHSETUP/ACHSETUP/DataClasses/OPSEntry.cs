using System;

namespace ACHSETUP
{
    public class OPSEntry
    {
        public enum ConfirmationOptions
        {
            Email,
            Letter,
            None
        }

        public string RoutingNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string PaymentAmount { get; set; }
        public ACHRecord.BankAccountType AcctType { get; set; }
        public string AccountHolderName { get; set; }
        public string EffectiveDate { get; set; }

        /// <summary>
        /// Calculates the Account Type based off the current value in AcctType (enum).
        /// </summary>
        public string CalculatedAccountType
        {
            get
            {
                if (AcctType == ACHRecord.BankAccountType.Checking)
                    return "Checking";
                else
                    return "Savings";
            }
        }
        public string DOB { get; set; }
        public string FullName { get; set; }
        public string SSN { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public OPSEntry()
        {
            RoutingNumber = string.Empty;
            BankAccountNumber = string.Empty;
            PaymentAmount = string.Empty;
            AccountHolderName = string.Empty;
            EffectiveDate = DateTime.Today.ToString("MM/dd/yyyy");

        }
    }
}