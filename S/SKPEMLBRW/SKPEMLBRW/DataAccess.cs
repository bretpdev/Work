using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SKPEMLBRW
{
    public class DataAccess
    {
        LogDataAccess LDA { get; set; }
        private DataAccessHelper.Database Database { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, true);
            Database = DataAccessHelper.Database.Uls;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "skpemlbrw.GetReassignmentId")]
        public string GetReassignmentId()
        {
            return LDA.ExecuteList<string>("skpemlbrw.GetReassignmentId", DataAccessHelper.Database.Bsys).Result.FirstOrDefault();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[skpemlbrw].[InsertToEmailBatch]")]
        public int? InsertToEmailBatch(int emailCampaignId, BorrowerData borrowerData)
        {
            object result = LDA.ExecuteSingle<object>("[skpemlbrw].[InsertToEmailBatch]", DataAccessHelper.Database.Uls, SqlParams.Single("EmailCampaignId", emailCampaignId), SqlParams.Single("AccountNumber", borrowerData.AccountNumber), SqlParams.Single("EmailData", $"{borrowerData.AccountNumber},{borrowerData.BorrowerName},{borrowerData.Email}"), SqlParams.Single("ArcNeeded", true)).Result;
            if(result == null)
            {
                return null;
            }
            else
            {
                return result.ToString().ToIntNullable();
            }
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "skpemlbrw.GetCampaignId")]
        public int GetCampaignId()
        {
            return LDA.ExecuteSingle<int>("skpemlbrw.GetCampaignId", DataAccessHelper.Database.Uls).Result;
        }
    }
}
