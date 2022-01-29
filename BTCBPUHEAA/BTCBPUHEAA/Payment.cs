using System;
using System.Text;
using Uheaa.Common.DataAccess;

namespace BTCBPUHEAA
{
    class Payment
    {
        [DbName("ID")]
        public int RecNo { get; set; }
        [DbName("EffectiveDate")]
        public DateTime EffectiveDate { get; set; }
        [DbName("AccountType")]
        public string AccountType { get; set; }
        [DbName("ABA")]
        public string RoutingNumber { get; set; }
        [DbName("BankAccountNumber")]
        public string BankAccountNumber { get; set; }
        [DbName("Amount")]
        public decimal PaymentAmount { get; set; }
        [DbName("AccountNumber")]
        public string AccountNumber { get; set; }
        [DbName("Name")]
        public string Name { get; set; }
        [DbName("DOB")]
        public DateTime DateOfBirth { get; set; }
        [DbName("AccountHolderName")]
        public string AccountHolderName { get; set; }


        public string GetAutoPostRecord(int batchNumber, int recordNumber)
        {
            //The CheckByPhone table makes certain guarantees about string lengths at the time of writing,
            //but we're checking them here to guard against future table changes that can violate the file layout.
            ValidateData();
            string region = "UT";

            StringBuilder recordBuilder = new StringBuilder();
            recordBuilder.Append("DETAIL01");
            recordBuilder.AppendFormat("{0:0000}", batchNumber);
            recordBuilder.AppendFormat("{0:0000000}", recordNumber);
            recordBuilder.Append(AccountNumber); //Account number field stores SSN for uheaa

            recordBuilder.Append("10");
            recordBuilder.Append("10");
            recordBuilder.AppendFormat("{0:MMddyyyy}", EffectiveDate);
            recordBuilder.Append(PaymentAmount.ToString().Replace(".", "").PadLeft(8, '0'));
            recordBuilder.Append("".PadRight(60, ' '));
            recordBuilder.Append("".PadRight(10, ' ')); //Uheaa doesnt store account number

            recordBuilder.Append("".PadRight(80, ' '));
            recordBuilder.Append(region);
            return recordBuilder.ToString();
        }

        public string GetTelpayRecord()
        {
            //The CheckByPhone table makes certain guarantees about string lengths at the time of writing,
            //but we're checking them here to guard against future table changes that can violate the file layout.
            ValidateData();

            StringBuilder recordBuilder = new StringBuilder();
            recordBuilder.AppendFormat("{0:yyMMdd}", EffectiveDate);
            recordBuilder.Append(AccountType.ToUpper());
            recordBuilder.Append(RoutingNumber);
            recordBuilder.Append(BankAccountNumber.PadRight(17, ' '));
            recordBuilder.Append(PaymentAmount.ToString().Replace(".", "").PadLeft(10, '0'));
            recordBuilder.Append("UT");
            recordBuilder.Append(AccountNumber);
            recordBuilder.Append("".PadRight(3, ' '));
            recordBuilder.Append(AccountHolderName.PadRight(23, ' '));
            return recordBuilder.ToString();
        }

        private void ValidateData()
        {
            if (RoutingNumber.Length != 9)
                throw new FormatException("Routing number must be 9 digits.");
            if (BankAccountNumber.Length > 17 || BankAccountNumber.Length < 1)
                throw new FormatException("Bank account number must be between 1 and 17 digits.");
            if (AccountNumber.Length != 9 && AccountNumber.Length != 10)
                throw new FormatException("Account identifier must be 9 or 10 digits.");
            if (AccountHolderName.Length > 23)
                AccountHolderName = ShortenAccountHolderName(AccountHolderName);
            if (AccountHolderName.Length > 23 || AccountHolderName.Length < 1)
                throw new FormatException("Account holder name must be between 1 and 23 characters in length.");
        }

        public string GetTelpayRecordArchive()
        {
            //The CheckByPhone table makes certain guarantees about string lengths at the time of writing,
            //but we're checking them here to guard against future table changes that can violate the file layout.
            ValidateData();

            StringBuilder recordBuilder = new StringBuilder();
            recordBuilder.AppendFormat("{0:yyMMdd}", EffectiveDate);
            recordBuilder.Append("X");
            recordBuilder.Append("XXXXXXXXX");
            recordBuilder.Append("XXXXXXXXXXXXXXXXX");
            recordBuilder.Append(PaymentAmount.ToString().Replace(".", "").PadLeft(10, '0'));
            recordBuilder.Append("  ");
            recordBuilder.Append(AccountNumber);
            recordBuilder.Append("".PadRight(3, ' '));
            recordBuilder.Append(AccountHolderName.PadRight(23, ' '));
            return recordBuilder.ToString();
        }

        public string GetPayGovRecord()
        {
            //The CheckByPhone table makes certain guarantees about string lengths at the time of writing,
            //but we're checking them here to guard against future table changes that can violate the file layout.
            ValidateData();

            StringBuilder recordBuilder = new StringBuilder();
            recordBuilder.AppendFormat("{0:yyMMdd}", EffectiveDate);
            recordBuilder.Append(AccountType);
            recordBuilder.Append(RoutingNumber);
            recordBuilder.Append(BankAccountNumber.PadRight(17, ' '));
            recordBuilder.AppendFormat("{0:0000000000}", PaymentAmount);
            recordBuilder.Append("KU");
            recordBuilder.Append(AccountNumber);
            recordBuilder.Append("".PadRight(3, ' '));
            recordBuilder.Append(AccountHolderName.PadRight(23, ' '));
            return recordBuilder.ToString();
        }

        public string GetPayGovRecordArchive()
        {
            //The CheckByPhone table makes certain guarantees about string lengths at the time of writing,
            //but we're checking them here to guard against future table changes that can violate the file layout.
            ValidateData();

            StringBuilder recordBuilder = new StringBuilder();
            recordBuilder.AppendFormat("{0:yyMMdd}", EffectiveDate);
            recordBuilder.Append("X");
            recordBuilder.Append("XXXXXXXXX");
            recordBuilder.Append("XXXXXXXXXXXXXXXXX");
            recordBuilder.AppendFormat("{0:0000000000}", PaymentAmount);
            recordBuilder.Append("KU");
            recordBuilder.Append(AccountNumber);
            recordBuilder.Append("".PadRight(3, ' '));
            recordBuilder.Append(AccountHolderName.PadRight(23, ' '));
            return recordBuilder.ToString();
        }

        //shorten account holder name to first initial and last name
        private string ShortenAccountHolderName(string rawName)
        {
            string[] names = rawName.Split(' ');
            string newName = "";
            if (names.GetUpperBound(0) < 2)
                newName = names[0].Substring(0, 1) + " " + names[names.GetUpperBound(0)];
            else
                newName = names[0].Substring(0, 1) + " " + names[names.GetUpperBound(0) - 1] + " " + names[names.GetUpperBound(0)];

            if (newName.Length > 23)
                return newName.Substring(0, 22);
            else
                return newName;
        }
    }
}
