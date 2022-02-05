using System;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace ARCADDFED
{
	public class ArcAddDatabaseProcessingFed : FedBatchScriptBase
	{
		//End-of-job fields are used as both array initializers and dictionary keys, so define them in consts.
		private const string EOJ_TOTAL_FROM_DATABASE = "Total number of records retrieved from Database";
		private const string EOJ_PROCESSED = "Total number of comments added to TD22";
		private const string EOJ_ERROR_REPORT = "Error adding comment to TD22";
		private static readonly string[] EOJ_FIELDS = { EOJ_TOTAL_FROM_DATABASE, EOJ_PROCESSED, EOJ_ERROR_REPORT };

		private DataAccess _da;

		public ArcAddDatabaseProcessingFed(ReflectionInterface ri)
			: base(ri, "ARCADDFED", "ERR_BU35", "EOJ_BU35", EOJ_FIELDS)
		{
			_da = new DataAccess(TestModeProperty);
		}

		public override void Main()
		{
			//Loops through all records found in the database
			for (ArcRecord record = _da.GetSingleRecord(); record != null; record = _da.GetSingleRecord())
			{
				Eoj.Counts[EOJ_TOTAL_FROM_DATABASE].Increment();
				//Process the record foundx
				ProcessRecord(record);
			}

			if (Eoj.Counts[EOJ_TOTAL_FROM_DATABASE] == 0) { NotifyAndContinue("No available records to process"); }
			if (Eoj.Counts[EOJ_ERROR_REPORT] > 0) { SendErrorEmail(); }
			ProcessingComplete();
		}

		private void ProcessRecord(ArcRecord record)
		{
			//Load borrowers SSN from the data warehouse
			string borSSN = _da.GetSSNFromDataWarehouse(record.AccountNumber);
			record.InRegardsTo = TD22.RegardsTo.Borrower;
			string recipientSSN = "";
			//If the recipient acct num is different from the borrower and does not start with P, load the recipient ssn from data warehouse
			if (record.AccountNumber != record.RecipientId && record.RecipientId.Substring(0, 1).ToUpper() != "P")
			{
				FastPath("TX3Z/ITX1J;" + record.RecipientId);
				recipientSSN = GetText(3, 12, 11).Replace(" ", "");
				record.InRegardsTo = GetRecipientCode();
			}
			else if (record.RecipientId.Substring(0, 1).ToUpper() == "P")
			{
				record.InRegardsTo = TD22.RegardsTo.Reference;
				recipientSSN = record.RecipientId;
			}

			record.Comment += ", " + record.RequestedDate.ToShortDateString() + ", " + record.UserId;
			string regardsToId = record.InRegardsTo == TD22.RegardsTo.Endorser ? recipientSSN : "";
			Common.CompassCommentScreenResults result = ATD22ByBalance(borSSN, record.Arc, record.Comment, ScriptID, false, recipientSSN, record.InRegardsTo, regardsToId);
			if (result == Common.CompassCommentScreenResults.RecipientFieldBlank)
			{
				result = ATD22ByBalance(borSSN, record.Arc, record.Comment, ScriptID, false, borSSN, record.InRegardsTo, regardsToId);
			}
			if (result == Common.CompassCommentScreenResults.CommentAddedSuccessfully)
			{
				//if arc added, increment processed count
				Eoj.Counts[EOJ_PROCESSED].Increment();
			}
			else
			{
				//if arc not added, increment err count and add to error report
				Eoj.Counts[EOJ_ERROR_REPORT].Increment();
				Err.AddRecord(EOJ_ERROR_REPORT, record);
			}

			//Delete the record after it has been deleted
			_da.DeleteProcessedRecord(record.RecordId);
		}

		private void SendErrorEmail()
		{
			try
			{
				string emailRecipients = DataAccessBase.GetEmailRecipientString(TestModeProperty, "ARC Add", DataAccessBase.EmailLookupOption.ErrorOnEmpty);
				string subject = "ARC Add FED Database Processing";
				string body = "The ARC Add database processing FED error report generated. Please review error reports.";
				SendMail(emailRecipients, "", subject, body, "", "", "", Common.EmailImportanceLevel.High);
			}
			catch (Exception ex)
			{
				NotifyAndContinue(ex.Message);
			}
		}

		private TD22.RegardsTo GetRecipientCode()
		{
			switch (GetText(1, 77, 2))
			{
				case "01":
					return TD22.RegardsTo.Borrower;
				case "02":
					return TD22.RegardsTo.Endorser;
				case "04":
					return TD22.RegardsTo.Student;
			}
			return TD22.RegardsTo.Borrower;
		}
	}
}
