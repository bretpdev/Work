using System;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;



namespace LENDERUPDT
{
	public class DataAccess
	{
        private LogDataAccess LDA { get; set; }

        public string ScriptId { get { return "LENDERUPDT"; }  }

        public DataAccess(int plId)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, plId, false, true);
        }

		[UsesSproc(DataAccessHelper.Database.Uls, "GetUnprocessedLenderUpdates")]
		public List<LenderUpdates> GetUnprocessedLenderUpdates()
		{
			return LDA.ExecuteList<LenderUpdates>("GetUnprocessedLenderUpdates", DataAccessHelper.Database.Uls).Result;
		}

		[UsesSproc(DataAccessHelper.Database.Uls, "SetLenderUpdateProcessed")]
		public void SetLenderUpdateProcessed(LenderUpdates record)
		{
			LDA.Execute("SetLenderUpdateProcessed", DataAccessHelper.Database.Uls, new SqlParameter("LenderUpdateId", record.LenderUpdateId));
		}
	}
}
