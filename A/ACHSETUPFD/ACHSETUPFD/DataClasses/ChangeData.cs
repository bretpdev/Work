namespace ACHSETUPFD
{
    class ChangeData
    {
        public string AbaNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string AccountType { get; set; }
        public double AdditionalWithdrawalAmount { get; set; }
        public string BorrowerSsn { get; set; }
        public string AccountNumber { get; set; }
        public string RecipientSsn { get; set; }
        public string PersonType { get; set; }
        public string EndorserSsn { get; set; }
        public EFTSource EFT { get; set; }

        public enum EFTSource
        {
            Paper,
            Web
        }

        /// <summary>
        /// Returns PPD for Paper EFT
        /// </summary>
        public string GetEftAsString()
        {
            return "PPD";
        }

    }
}
