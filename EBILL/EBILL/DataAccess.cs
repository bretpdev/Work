using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace EBILL
{
	class DataAccess 
	{
        /// <summary>
        /// Gets all of the borrowers LoanSeqs that were updated.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "GetSuccessfulEBillLoanSequences")]
		public List<int> GetSucessfulLoanSequences(string ssn, string billingPreference)
		{
            return DataAccessHelper.ExecuteList<int>("GetSuccessfulEBillLoanSequences", DataAccessHelper.Database.Uls, SqlParams.Single("SSN", ssn), SqlParams.Single("BillingPreference", billingPreference));
		}

        /// <summary>
        /// Marks a record as processed the Arc
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "MarkEbillArcAdded")]
        public void MarkArcAdded(string ssn, string billingPreference)
        {
            DataAccessHelper.Execute("MarkEbillArcAdded", DataAccessHelper.Database.Uls, SqlParams.Single("SSN", ssn), SqlParams.Single("BillingPreference", billingPreference));
        }

        /// <summary>
        /// Marks a records preferences updated.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "MarkEBillUpdateSucceeded")]
		public void MarkUpdateSucceeded(int ebillId)
		{
            DataAccessHelper.Execute("MarkEBillUpdateSucceeded", DataAccessHelper.Database.Uls, SqlParams.Single("EbillId", ebillId));
		}

        /// <summary>
        /// Saves all records to the database
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "AddEBillRecord")]
		public void SaveRecords(List<RequestRecord> records)
		{
			foreach (RequestRecord record in records)
                record.EbillId = DataAccessHelper.ExecuteId("AddEBillRecord", DataAccessHelper.Database.Uls, SqlParams.Single("SSN", record.SSN), SqlParams.Single("LoanSequence", record.LoanSequence.ToInt()), SqlParams.Single("BillingPreference", record.BillingPreference), SqlParams.Single("Email", record.Email) );
		}

        /// <summary>
        /// Gets all records to process.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "GetAllEbillRecords")]
        public List<RequestRecord> GetAllUnprocessedRecords()
        {
            return DataAccessHelper.ExecuteList<RequestRecord>("GetAllEbillRecords", DataAccessHelper.Database.Uls);
        }

        /// <summary>
        /// Marks a records had and error.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "MarkEbillRecordError")]
        public void MarkError(int ebillId)
        {
            DataAccessHelper.Execute("MarkEbillRecordError", DataAccessHelper.Database.Uls, SqlParams.Single("EbillId", ebillId));
        }
	}
}