namespace R301QUEUE
{
	class Transaction
	{
		public string LoanSequence { get; set; }
		public string Amount { get; set; }
		public bool IsCredit { get; set; }
		public string Type { get; set; }

		/*
		 * Transaction types (from ITS2C target screen):
		 * 01 DISBURSEMENT
		 * 02 CONVERSION
		 * 03 TRANSFER
		 * 04 DECONVERSION
		 * 10 PAYMENT
		 * 12 WINDFALL PROFIT
		 * 14 ORIGINATION FEE
		 * 16 INSURANCE PREMIUM
		 * 20 REFUND
		 * 26 LATE FEE
		 * 28 PRE-CLAIM FEE
		 * 50 WRITE OFF
		 * 55 CHARGE OFF
		 * 60 WRITE UP
		 * 70 CAPITALIZATION
		 * 80 PRE-CONVERSION AD
		 * 90 INTEREST ACCRUAL
		 * 
		 * Transaction sub-types (also from ITS2C target screen):
		 * 01 AUTOMATIC
		 * 02 MANUAL REQUEST
		 * 03 AUTOMATIC SYS GEN
		 * 04 AUTO-DEFAULTED ACC
		 * 10 BORROWER
		 * 11 BRWR-INT ONLY
		 * 12 BRWR-PRIN ONLY
		 * 15 COSIGNER
		 * 16 CSGN-INT ONLY
		 * 17 CSGN-PRIN ONLY
		 * 20 BORROWER BEHALF
		 * 21 BRWR BHLF-INT ONLY
		 * 22 BRWR BHLF-PRIN ONL
		 * 25 OWNER
		 * 26 OWNER-INTEREST ONL
		 * 27 OWNER-PRIN ONLY
		 * 29 PLUS INT CREDIT BB
		 * 30 GUARANTOR
		 * 31 GUARANTOR INT ONLY
		 * 32 GUARANTOR PRIN ONL
		 * 34 TIMELY PMT O FEE C
		 * 35 MILITARY
		 * 36 GOVRN PAYMENT
		 * 37 GOVRN INT
		 * 38 BB OTHER PRIN ONLY
		 * 39 BB OTHER INT/PRIN
		 * 40 SCHOOL REFUND
		 * 41 BORROWER-CANCEL
		 * 44 SUB/UNSUB CHANGE
		 * 45 RETURNED DISB
		 * 46 DISB ERROR SUB-TYP
		 * 47 DISB ERROR SUB TYP
		 * 48 48
		 * 49 PART CANCEL
		 * 50 FORGIVENESS
		 * 53 TILP SO PAMYENT
		 * 54 TILP TEACH CR PRIN
		 * 55 COLLECTION AGENCY
		 * 56 RMT RFND REV INC A
		 * 57 RMT RFND REV INC P
		 * 58 INCOR TRN TYP/SUBT
		 * 59 ADJ MADE/POST ERRO
		 * 60 RMT RFND REV-OTHER
		 * 70 CONSOL-NON-NETWORK
		 * 80 CONSOL-NETWORK
		 * 90 PURCHASE
		 * 91 NON-PURCHCASE
		 * 92 REPURCHASE
		 * 95 SALE
		 * 96 NON-SALE
		 */
	}//class
}//namespace
