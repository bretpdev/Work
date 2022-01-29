namespace EBILLFED
{
	class RequestRecord
	{
		private readonly string _billingPreference;
		public string BillingPreference { get { return _billingPreference; } }

		private readonly string _email;
		public string Email { get { return _email; } }

		private readonly string _loanSequence;
		public string LoanSequence { get { return _loanSequence; } }

        private readonly string _loanProgram;
        public string LoanProgram { get { return _loanProgram; } }

		private readonly string _ssn;
		public string SSN { get { return _ssn; } }

        private readonly bool _hasNoGracePeriod = false;
        public bool HasNoGracePeriod { get { return _hasNoGracePeriod; } }

		//Private constructor forces the use of the Parse() method.
		private RequestRecord(string ssn, string loanSequence, string billingPreference, string email, string loanProgram)
		{
			_billingPreference = billingPreference;
			_email = email;
			_loanSequence = loanSequence;
			_ssn = ssn;
            //set no grace period indicator
            if (loanProgram == "DLPLUS" || loanProgram == "DLPLGB" || loanProgram == "DLSSPL" || loanProgram == "DLUSPL" || loanProgram == "DLUCNS" || loanProgram == "DLSCNS" || loanProgram == "PLUS" || loanProgram == "PLUSGB" || loanProgram == "UNCNS" || loanProgram == "SUBCNS" || loanProgram == "SUBSPC" || loanProgram == "UNSPC")
            {
                _hasNoGracePeriod = true;
            }
		}

		public static RequestRecord Parse(string fileLine)
		{
			//0-1 is the client ID, which the script doesn't need.
			string ssn = fileLine.Substring(2, 9);
			//11 is a space.
			string loanSequence = fileLine.Substring(12, 4); //Left-padded with zeros, just like on the screens we care about.
            string loanProgram = fileLine.Substring(16, 6).Trim();
			//22-30 is the balance, which the script doesn't need.
			string billingPreference = fileLine.Substring(31, 1);
			string email = fileLine.Substring(32).Trim();
			return new RequestRecord(ssn, loanSequence, billingPreference, email, loanProgram);
		}//Parse()
	}//class
}//namespace
