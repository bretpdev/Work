using System;
using Q;
using Key = Q.ReflectionInterface.Key;
using System.Collections.Generic;

namespace ARCADD
{
    public class ArcAddDatabaseProcessing : BatchScriptBase
    {
        private DataAccess _da;
        private ErrorReport _errorReport;
        private bool _hasRecordsProcesssed = false;
        private bool _hasErrors = false;

        public ArcAddDatabaseProcessing(ReflectionInterface ri)
            : base(ri, "ARCADD")
        {
            _da = new DataAccess(TestModeProperty);
            _errorReport = new ErrorReport(ri.TestMode, "ARC ADD Database Processing", "ERR_BU35", Region.UHEAA);
            ValidateRegion(Region.UHEAA);
        }

        public override void Main()
        {
            //Loops through all records found in the database
            for (ArcRecord record = _da.GetSingleRecord(); record != null; record = _da.GetSingleRecord())
            {
                ProcessRecord(record);
            }

            if (!_hasRecordsProcesssed) { NotifyAndContinue("No available records to process"); }
            if (_hasErrors)
            {
                _errorReport.Publish();
                SendErrorEmail();
            }
            ProcessingComplete();
        }

        private void ProcessRecord(ArcRecord record)
		{
			//Load borrowers SSN from the data warehouse
			string borSSN = _da.GetSSNFromDataWarehouse(record.AccountNumber);
            record.InRegardsTo = TD22.RegardsTo.Borrower;
			string recipientSSN = "";
            List<int> lnseqs = new List<int>();
            //If the recipient acct num is different from the borrower and does not start with P, load the recipient ssn from data warehouse
            if (record.AccountNumber != record.RecipientId && record.RecipientId.Substring(0, 1).ToUpper() != "P")
            {
                FastPath("TX3Z/ITX1J;" + record.RecipientId);
                recipientSSN = GetText(3, 12, 11).Replace(" ", "");
                record.InRegardsTo = GetRecipientCode();
                Hit(Key.F2);
                Hit(Key.F6);
                int row = 10;
                while (!Check4Text(23,2,"90007"))
                {
                    if(Check4Text(row,13,borSSN))
                        {
                            lnseqs.Add(Convert.ToInt32(GetText(row,9,3)));
                        }
                    row++;
                    if (GetText(row,3,1).Equals(string.Empty))
                    {
                        Hit(Key.F8);
                        row=10;
                    }
                }
            }
            else if (record.RecipientId.Substring(0, 1).ToUpper() == "P")
            {
                record.InRegardsTo = TD22.RegardsTo.Reference;
                recipientSSN = record.RecipientId;
            }

			record.Comment += " " + record.RequestedDate.ToShortDateString() + " " + record.UserId;
            string regardsToId = record.InRegardsTo == TD22.RegardsTo.Endorser ? recipientSSN : "";

            Common.CompassCommentScreenResults result = Common.CompassCommentScreenResults.ARCNotFound;
            if (record.AccountNumber != record.RecipientId && record.RecipientId.Substring(0, 1).ToUpper() != "P")
            {
                TD22 atd22 = TD22.CreateNewTd22(RI);
               result = atd22.ATD22ByLoan(borSSN, record.Arc, lnseqs, record.Comment, ScriptID, false, TD22.NO_DATE, TD22.NO_DATE, TD22.NO_DATE, recipientSSN, record.InRegardsTo, regardsToId);

            }
            else
            {
                result = ATD22ByBalance(borSSN, record.Arc, record.Comment, ScriptID, false, recipientSSN, record.InRegardsTo, regardsToId);
            }

            if (result == Common.CompassCommentScreenResults.RecipientFieldBlank)
            {
                result = ATD22ByBalance(borSSN, record.Arc, record.Comment, ScriptID, false, borSSN, record.InRegardsTo, regardsToId);
            }
            if (result == Common.CompassCommentScreenResults.CommentAddedSuccessfully)
            {
                _hasRecordsProcesssed = true;
			}
			else
			{
                _errorReport.AddRecord("Error adding comment to TD22", record);
                _hasErrors = true;
			}

			//Delete the record after it has been deleted
			_da.DeleteProcessedRecord(record.RecordId);
		}

        private void SendErrorEmail()
        {
            try
            {
                string emailRecipients = DataAccess.GetEmailForKeyString(TestModeProperty, "ARC Add Error", 0, DataAccessBase.EmailLookupOption.None);
                string subject = "ARC Add Database Processing";
                string body = "The ARC Add database processing error report generated. Please review error report.";
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
