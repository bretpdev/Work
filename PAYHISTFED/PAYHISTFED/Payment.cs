using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace PAYHISTFED
{
	public class Payment
	{
		//I don't trust the SAS file to always have valid values for most of these properties,
		//so rather than give them proper types, most of them are declared as strings.
		//If they need a different format on the Crystal report, it can be set in the SAS code.
		
		public string TransactionEffectiveDate { get; set; }
		public string PostEffectiveDate { get; set; }
		public string ApplicationDate { get; set; }
		public string TransactionType { get; set; }
		public string TransactionAmount { get; set; }
		public string Principal { get; set; }
		public string Interest { get; set; }
		public string Fees { get; set; }
		public string Balance { get; set; }

		public static Payment Parse(string sasLine, ProcessLogData logData)
		{
			const int EXPECTED_FIELD_COUNT = 16;
			List<string> fields = sasLine.SplitAndRemoveQuotes(",");
			if (fields.Count != EXPECTED_FIELD_COUNT)
			{
				string message = string.Format("A SAS line for borrower #{0} has the wrong number of fields. It should have {1}, but only {2} were found.", fields[0], EXPECTED_FIELD_COUNT, fields.Count);
                ProcessLogger.AddNotification(logData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
				throw new FormatException(message);
			}
			Payment payment = new Payment();
			//payment.BorrowerNumber = int.Parse(fields[0]);
			//payment.SSN = fields[1];
			//payment.Name = fields[2];
			//payment.LoanProgram = fields[3];
   //         payment.DisbursementDate = fields[4];
			payment.TransactionEffectiveDate = fields[5];
			payment.PostEffectiveDate = fields[6];
			payment.ApplicationDate = fields[7];
			payment.TransactionType = fields[8];
			payment.TransactionAmount = fields[9];
			payment.Principal = fields[10];
			payment.Interest = fields[11];
			payment.Fees = fields[12];
			payment.Balance = fields[13];
			return payment;
		}
	}
}