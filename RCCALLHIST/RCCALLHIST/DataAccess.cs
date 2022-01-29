using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLoggerRC;

namespace RCCALLHIST
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }
        public LogDataAccess LDA { get; set; }
        public static readonly object locker = new object();
        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, LogRun.ProcessLogId, false, true);
        }

        [UsesSproc(DataAccessHelper.Database.UnexsysReports, "rccallhist.GetOutboundBasePopulation")]
        public List<OutboundCall> GetOutboundCalls()
        {
            List<CampaignPrefixes> prefixes = GetCampaignPrefixes();
            return LDA.ExecuteList<OutboundCall>("rccallhist.GetOutboundBasePopulation", DataAccessHelper.Database.UnexsysReports, SqlParams.Single("Campaigns", prefixes.ToDataTable())).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Voyager, "rccallhist.InsertOutboundCalls")]
        public bool InsertOutboundCalls(List<OutboundCall> calls)
        {
            return LDA.Execute("rccallhist.InsertOutboundCalls", DataAccessHelper.Database.Voyager, SqlParams.Single("OutboundCalls", calls.ToDataTable()));
        }

        [UsesSproc(DataAccessHelper.Database.NobleCalls, "rccallhist.GetInboundCalls")]
        public List<InboundCall> GetInboundCalls()
        {
            return LDA.ExecuteList<InboundCall>("rccallhist.GetInboundCalls", DataAccessHelper.Database.NobleCalls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.NobleCalls, "rccallhist.GetCampaignPrefixes")]
        public List<CampaignPrefixes> GetCampaignPrefixes()
        {
            return LDA.ExecuteList<CampaignPrefixes>("rccallhist.GetCampaignPrefixes", DataAccessHelper.Database.NobleCalls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Voyager, "rccallhist.InsertInboundCalls")]
        public bool InsertInboundCalls(List<InboundCall> calls)
        {
            return LDA.Execute("rccallhist.InsertInboundCalls", DataAccessHelper.Database.Voyager, SqlParams.Single("InboundCalls", calls.ToDataTable()));
        }
    }
}
