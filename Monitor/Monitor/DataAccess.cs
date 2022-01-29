using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace Monitor
{
    class DataAccess
    {
        DataAccessHelper.Database warehouseDb;
        DataAccessHelper.Database servicingDb;
        LogDataAccess lda;
        public DataAccess(DataAccessHelper.Database warehouseDb, DataAccessHelper.Database servicingDb, LogDataAccess lda)
        {
            this.warehouseDb = warehouseDb;
            this.servicingDb = servicingDb;
            this.lda = lda;
        }
        [UsesSproc(DataAccessHelper.Database.Cdw, "monitor.GetRedisclosureExemptions")]
        [UsesSproc(DataAccessHelper.Database.Udw, "monitor.GetRedisclosureExemptions")]
        public RedisclosureExemptionData GetRedisclosureExemptions(string ssn, DateTime r0CreateDate)
        {
            return lda.ExecuteSingle<RedisclosureExemptionData>("monitor.GetRedisclosureExemptions", warehouseDb,
                Sp("Ssn", ssn), Sp("R0CreateDate", r0CreateDate)).Result;
        }

        public bool CheckMonitorSettings()
        {
            return GetMonitorSettingsInternal().Count > 0;
        }

        public MonitorSettings GetMonitorSettings()
        {
            return GetMonitorSettingsInternal().FirstOrDefault();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "monitor.GetMonitorSettings")]
        [UsesSproc(DataAccessHelper.Database.Udw, "monitor.GetMonitorSettings")]
        private List<MonitorSettings> GetMonitorSettingsInternal()
        {
            return lda.ExecuteList<MonitorSettings>("monitor.GetMonitorSettings", warehouseDb).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[monitor].[GetBorrowerRecentMonitorSkippedTask]")]
        [UsesSproc(DataAccessHelper.Database.Cls, "[monitor].[GetBorrowerRecentMonitorSkippedTask]")]
        public ArcEntry BorrowerGetRecentMonitorSkippedTask(string accountNumber)
        {
            return lda.ExecuteList<ArcEntry>("[monitor].[GetBorrowerRecentMonitorSkippedTask]", servicingDb, Sp("AccountNumber", accountNumber)).Result.SingleOrDefault();
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "spGetManagersUtId")]
        public string GetReassignmentUserId()
        {
            return lda.ExecuteSingle<string>("spGetManagersUtId", DataAccessHelper.Database.Csys, Sp("BusinessUnitId", 55)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "[monitor].[SetMonitorSettings]")]
        [UsesSproc(DataAccessHelper.Database.Cdw, "[monitor].[SetMonitorSettings]")]
        public void SaveMonitorSettings(MonitorSettings settings)
        {
            lda.Execute("[monitor].[SetMonitorSettings]", warehouseDb, Sp("MaxIncrease", settings.MaxIncrease), Sp("MaxForce", settings.MaxForce), Sp("MaxPreNote", settings.MaxPreNote), Sp("LastRecoveryPage", settings.LastRecoveryPage));
        }

        public List<MonitorReason> GetMonitorReasons()
        {
            return lda.ExecuteList<MonitorReason>("[monitor].[GetMonitorReasons]", warehouseDb).Result;
        }

        public decimal GetBorrowerTotalBalance(string ssn)
        {
            return lda.ExecuteSingle<decimal>("[monitor].[GetBorrowerTotalBalance]", warehouseDb, Sp("Ssn", ssn)).Result;
        }

        /// <summary>
        /// Starts a Run History and returns the RunHistoryId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Udw, "[monitor].[StartRunHistory]")]
        [UsesSproc(DataAccessHelper.Database.Cdw, "[monitor].[StartRunHistory]")]
        public int StartRunHistory()
        {
            return lda.ExecuteSingle<int>("[monitor].[StartRunHistory]", warehouseDb).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "[monitor].[EndRunHistory]")]
        [UsesSproc(DataAccessHelper.Database.Cdw, "[monitor].[EndRunHistory]")]
        public void EndRunHistory(int runHistoryId)
        {
            lda.Execute("[monitor].[EndRunHistory]", warehouseDb, Sp("RunHistoryId", runHistoryId));
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "[monitor].[AddEojItem]")]
        [UsesSproc(DataAccessHelper.Database.Cdw, "[monitor].[AddEojItem]")]
        public void AddEojItem(int runHistoryId, TaskEoj eoj, decimal maxIncrease)
        {
            lda.Execute("[monitor].[AddEojItem]", warehouseDb,
                Sp("RunHistoryId", runHistoryId), Sp("EojReportId", eoj.EojType), Sp("Ssn", eoj.Task.Ssn), Sp("TaskControl", eoj.Task.TaskControl),
                Sp("ActionRequest", eoj.Task.ActionRequest), Sp("R0CreateDate", eoj.R0CreateDate), Sp("MonitorReason", eoj.Task.MonitorReason),
                Sp("OldMonthlyPayment", eoj.OldMonthlyPayment), Sp("NewMonthlyPayment", eoj.NewMonthlyPayment), Sp("ForcedDisclosure", eoj.ForceDisclosure),
                Sp("MaxIncrease", maxIncrease), Sp("10CreateDate", eoj.CreateDate10), Sp("CancelReason", eoj.CancelReason));
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "[dbo].[spLTDB_GetLetterInfo]")]
        public string GetCostCenterForLetter(string letterId)
        {
            if (!LetterCostCenters.ContainsKey(letterId))
            {
                var info = lda.ExecuteSingle<LetterInfo>("[dbo].[spLTDB_GetLetterInfo]", DataAccessHelper.Database.Bsys, Sp("LetterId", letterId)).Result;
                LetterCostCenters.Add(letterId, info.CostCenter);
            }
            return LetterCostCenters[letterId];

        }
        private Dictionary<string, string> LetterCostCenters = new Dictionary<string, string>();
        public class LetterInfo
        {
            public string LetterId { get; set; }
            public string CostCenter { get; set; }
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}
