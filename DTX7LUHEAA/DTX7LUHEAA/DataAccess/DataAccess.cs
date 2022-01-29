using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DTX7LUHEAA
{
    class DataAccess 
    {
        private LogDataAccess LDA { get; set; }
        private DataAccessHelper.Database DB { get; set; }
		public DataAccess(int processlogId)
		{
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, processlogId, false, true);
            DB = DataAccessHelper.Database.Uls;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "spDTX7LGetDueDilLetters")]
        public List<LetterIDAndArcCombo> GetDueDilLetters()
		{
			return LDA.ExecuteList<LetterIDAndArcCombo>("spDTX7LGetDueDilLetters", DB).CheckResult();
		}

        [UsesSproc(DataAccessHelper.Database.Uls, "spDTX7LGetExpiredLetters")]
        public List<LetterIDAndArcCombo> GetExpiredLetters()
		{
            return LDA.ExecuteList<LetterIDAndArcCombo>("spDTX7LGetExpiredLetters", DB).CheckResult();
		}

        /// <summary>
        /// Marks the Deleted record as processed
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "DTX7LMarkRecordProcessed")]
        public void MarkProcessed(int recordId)
        {
            LDA.Execute("DTX7LMarkRecordProcessed", DB, Sp("DTX7LDeletedRecordId", recordId));
        }

        /// <summary>
        /// Inserts a record to be deleted
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "DTX7LInsertDeletedRecord")]
        public void InsertDeletedRecord(string ssn, string arc, DateTime requestDate, string letterId, bool isDueDil)
        {
            LDA.Execute("DTX7LInsertDeletedRecord", DB, Sp("Ssn", ssn), Sp("Arc", arc), Sp("RequestDate", requestDate), 
                Sp("LetterId", letterId), Sp("IsDueDiligence", isDueDil));
        }

        /// <summary>
        /// Gets all the records to delete td2a data
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "Dtx7lGetDeletedRecords")]
        public Queue<Dtx7lDeletedRecords> Populate()
        {
            return new Queue<Dtx7lDeletedRecords>(LDA.ExecuteList<Dtx7lDeletedRecords>("Dtx7lGetDeletedRecords", DB).CheckResult());
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}
