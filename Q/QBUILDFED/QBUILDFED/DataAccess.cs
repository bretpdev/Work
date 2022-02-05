using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace QBUILDFED
{
    public class DataAccess 
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }

        /// <summary>
        /// Gets all of the Sas files the script needs to process.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "spGetQueueBuilderFiles")]
		public List<SasInstructions> GetSasList()
		{
			return LDA.ExecuteList<SasInstructions>("spGetQueueBuilderFiles", DataAccessHelper.Database.Cls).Result;
		}

        /// <summary>
        /// Gets an the borrowers ssn from their account number
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "spGetSSNFromAcctNumber")]
        public string GetSsnFromAccountNumber(string accountNumber)
        {
            return LDA.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Cdw, accountNumber.ToSqlParameter("AccountNumber")).Result;
        }

        /// <summary>
        /// Gets an the borrowers account number from their ssn
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "spGetAccountNumberFromSsn")]
        public  string GetAccountNumberFromSsn(string ssn)
        {
            return LDA.ExecuteSingle<string>("spGetAccountNumberFromSsn", DataAccessHelper.Database.Cdw, ssn.ToSqlParameter("SSN")).Result;
        }
	}
}