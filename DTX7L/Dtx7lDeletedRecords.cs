using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace DTX7L
{
    class Dtx7lDeletedRecords
    {
        public int DTX7LDeletedRecordId { get; set; }
        public string Ssn { get; set; }
        public string Arc { get; set; }
        public DateTime RequestDate { get; set; }
        public string LetterId { get; set; }
        public bool IsDueDiligence { get; set; }

        /// <summary>
        /// Marks the Deleted record as processed
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "DTX7LMarkRecordProcessed")]
        public void MarkProcessed()
        {
            DataAccessHelper.Execute("DTX7LMarkRecordProcessed", DataAccessHelper.Database.Uls, SqlParams.Single("DTX7LDeletedRecordId", this.DTX7LDeletedRecordId));
        }

        /// <summary>
        /// Gets all the records to delete td2a data
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "Dtx7lGetDeletedRecords")]
        public static Queue<Dtx7lDeletedRecords> Populate()
        {
            return new Queue<Dtx7lDeletedRecords>(DataAccessHelper.ExecuteList<Dtx7lDeletedRecords>("Dtx7lGetDeletedRecords", DataAccessHelper.Database.Uls));
        }
    }
}
