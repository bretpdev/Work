using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EBILL
{
	class RequestRecord
	{
        public int EbillId { get; set; }
        public string SSN { get; set; }
        public string LoanSequence { get; set; }
		public string BillingPreference { get; set; }
		public string Email { get; private set; }
        public bool UpdateSucceeded { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool ArcAdded { get; set; }
        public DateTime? ArcAddedAt { get; set; }
		
		public static List<RequestRecord> FromFile(string fileName)
		{
			List<RequestRecord> records = new List<RequestRecord>();
			foreach (string fileLine in File.ReadAllLines(fileName))
			{
				RequestRecord record = new RequestRecord();
				//0-1 is the client ID, which the script doesn't need.
				record.SSN = fileLine.Substring(2, 9);
				//11 is a space.
				record.LoanSequence = fileLine.Substring(12, 4); //Left-padded with zeros, just like on the screens we care about.
				//16-21 is the loan program, which the script doesn't need.
				//22-30 is the balance, which the script doesn't need.
				record.BillingPreference = fileLine.Substring(31, 1);
				record.Email = fileLine.Substring(32).Trim();

				//A borrower may have changed their e-bill preference or e-mail address multiple times in one day, which creates multiple file lines for the same loan.
				//AES won't guard against that or filter out the invalid lines, so we need to do it ourselves.
				//Add the new record if we don't have this loan yet; update the e-bill preference and e-mail address if we do.
				RequestRecord existingLoan = records.SingleOrDefault(p => p.SSN == record.SSN && p.LoanSequence == record.LoanSequence);
				if (existingLoan == null)
					records.Add(record);
				else
				{
					existingLoan.BillingPreference = record.BillingPreference;
					existingLoan.Email = record.Email;
				}
			}
			return records;
		}//FromFile()
	}//class
}//namespace
