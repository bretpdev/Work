using System;
using System.Text;

namespace BTCHCBP
{
	class Payment
	{
		//The database has some char columns, so trim any string properties as they come in. While we're at it, don't allow null strings.
		public int RecNo { get; set; }
		public DateTime EffectiveDate { get; set; }
		private string _accountType;
		public string AccountType { get { return _accountType; } set { _accountType = Sanitize(value); } }
		private string _routingNumber;
		public string RoutingNumber { get { return _routingNumber; } set { _routingNumber = Sanitize(value); } }
		private string _bankAccountNumber;
		public string BankAccountNumber { get { return _bankAccountNumber; } set { _bankAccountNumber = Sanitize(value); } }
		public int PaymentAmount { get; set; }
		private string _cornerStoneAccountNumber;
		public string CornerStoneAccountNumber { get { return _cornerStoneAccountNumber; } set { _cornerStoneAccountNumber = Sanitize(value); } }
		private string _accountHolderName;
		public string AccountHolderName { get { return _accountHolderName; } set { _accountHolderName = Sanitize(value); } }


		public string GetAutoPostRecord(int batchNumber, int recordNumber)
		{
			//The CheckByPhone table makes certain guarantees about string lengths at the time of writing,
			//but we're checking them here to guard against future table changes that can violate the file layout.
			if (CornerStoneAccountNumber.Length != 10) { throw new FormatException("CornerStone account number"); }

			StringBuilder recordBuilder = new StringBuilder();
			recordBuilder.Append("DETAIL01");
			recordBuilder.AppendFormat("{0:0000}", batchNumber);
			recordBuilder.AppendFormat("{0:0000000}", recordNumber);
			recordBuilder.Append("".PadRight(9, ' '));
			recordBuilder.Append("10");
			recordBuilder.Append("10");
			recordBuilder.AppendFormat("{0:MMddyyyy}", EffectiveDate);
			recordBuilder.AppendFormat("{0:00000000}", PaymentAmount);
			recordBuilder.Append("".PadRight(60, ' '));
			recordBuilder.Append(CornerStoneAccountNumber);
			recordBuilder.Append("".PadRight(80, ' '));
			recordBuilder.Append("KU");
			return recordBuilder.ToString();
		}//GetAutoPostRecord()

		public string GetPayGovRecord()
		{
			//The CheckByPhone table makes certain guarantees about string lengths at the time of writing,
			//but we're checking them here to guard against future table changes that can violate the file layout.
			if (AccountType.Length != 1) { throw new FormatException("account type"); }
			if (RoutingNumber.Length != 9) { throw new FormatException("routing number"); }
			if (BankAccountNumber.Length > 17 || BankAccountNumber.Length < 1) { throw new FormatException("bank account number"); }
			if (CornerStoneAccountNumber.Length != 10) { throw new FormatException("CornerStone account number"); }
            if (AccountHolderName.Length > 23) { AccountHolderName = ShortenAccountHolderName(AccountHolderName); }
            if (AccountHolderName.Length > 23 || AccountHolderName.Length < 1) { throw new FormatException("account holder name"); }

			StringBuilder recordBuilder = new StringBuilder();
			recordBuilder.AppendFormat("{0:yyMMdd}", EffectiveDate);
			recordBuilder.Append(AccountType);
			recordBuilder.Append(RoutingNumber);
			recordBuilder.Append(BankAccountNumber.PadRight(17, ' '));
			recordBuilder.AppendFormat("{0:0000000000}", PaymentAmount);
			recordBuilder.Append("KU");
			recordBuilder.Append(CornerStoneAccountNumber);
			recordBuilder.Append("".PadRight(3, ' '));
			recordBuilder.Append(AccountHolderName.PadRight(23, ' '));
			return recordBuilder.ToString();
		}//GetPayGovRecord()

		public string GetPayGovRecordArchive()
		{
			//The CheckByPhone table makes certain guarantees about string lengths at the time of writing,
			//but we're checking them here to guard against future table changes that can violate the file layout.
			if (AccountType.Length != 1) { throw new FormatException("account type"); }
			if (RoutingNumber.Length != 9) { throw new FormatException("routing number"); }
			if (BankAccountNumber.Length > 17 || BankAccountNumber.Length < 1) { throw new FormatException("bank account number"); }
			if (CornerStoneAccountNumber.Length != 10) { throw new FormatException("CornerStone account number"); }
            if (AccountHolderName.Length > 23) { AccountHolderName = ShortenAccountHolderName(AccountHolderName); }
			if (AccountHolderName.Length > 23 || AccountHolderName.Length < 1) { throw new FormatException("account holder name"); }

			StringBuilder recordBuilder = new StringBuilder();
			recordBuilder.AppendFormat("{0:yyMMdd}", EffectiveDate);
			recordBuilder.Append("X");
			recordBuilder.Append("XXXXXXXXX");
			recordBuilder.Append("XXXXXXXXXXXXXXXXX");
			recordBuilder.AppendFormat("{0:0000000000}", PaymentAmount);
			recordBuilder.Append("KU");
			recordBuilder.Append(CornerStoneAccountNumber);
			recordBuilder.Append("".PadRight(3, ' '));
			recordBuilder.Append(AccountHolderName.PadRight(23, ' '));
			return recordBuilder.ToString();
		}//GetPayGovRecord()

		private string Sanitize(string rawData)
		{
			return (rawData == null ? "" : rawData.Trim());
		}

        //shorten account holder name to first initial and last name
        private string ShortenAccountHolderName(string rawName)
        {
            string[] names = rawName.Split(' ');
            if (names.GetUpperBound(0) < 2)
            {
                return names[0].Substring(0,1) + " " + names[names.GetUpperBound(0)];
            }
            else 
            {
                return names[0].Substring(0, 1) + " " + names[names.GetUpperBound(0) - 1] + " " + names[names.GetUpperBound(0)];
            }
        }
	}//class
}//namespace
