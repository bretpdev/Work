using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DIACTCMTS
{
    class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }
        public LogDataAccess LDA { get; set; }
        public static readonly object locker = new object();
        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, LogRun.ProcessLogId, false, true);
        }

        #region InboundCalls
        /// <summary>
        /// Gets all of the calling campaigns for the given region
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public void InboundLoad()
        {
            Console.WriteLine("Pulling inbound data from Noble");
            List<InboundData> inboundRecords = GetInboundRecords();
            LoadInboundData(inboundRecords);
        }

        /// <summary>
        /// Gets the records 
        /// </summary>
        /// <param name="campaigns"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.NobleCalls, "GetInboundCalls")]
        private List<InboundData> GetInboundRecords()
        {
            return LDA.ExecuteList<InboundData>("GetInboundCalls", DataAccessHelper.Database.NobleCalls).CheckResult(); 
        }

        /// <summary>
        /// Loads the data into the NobleCallHistory table
        /// 
        /// </summary>
        /// <param name="batchRecords">List of the DbData records</param>
        [UsesSproc(DataAccessHelper.Database.NobleCalls, "InsertInboundCallHistory")]
        private void LoadInboundData(List<InboundData> batchRecords)
        {
            Console.WriteLine("Loading data into NobleCalls.NobleCallHistory on {0}", DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev ? "OPSDEV" : "UheaaSqlDB");

            Parallel.ForEach(batchRecords, item =>
            {
                item.CleanseData();
            });
            LDA.Execute("InsertInboundCallHistory", DataAccessHelper.Database.NobleCalls, SqlParams.Single("Data", batchRecords.ToDataTable()));
        }
        #endregion

        #region OutboundCalls
        /// <summary>
        /// Gets all of the calling campaigns for the given region
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.NobleCalls, "GetCallCampaigns")]
        public void OutboundLoad()
        {
            //List<CallCampaigns> callCampaigns = LDA.ExecuteList<CallCampaigns>("GetCallCampaigns", DataAccessHelper.Database.NobleCalls, SqlParams.Single("Region", "OneLink")).CheckResult();
            List<CampaignPrefixes> campaignPrefixes = LDA.ExecuteList<CampaignPrefixes>("GetCampaignPrefixes", DataAccessHelper.Database.NobleCalls, SqlParams.Single("ScriptId", DialerActivityComments.ScriptId)).Result;
            
            //string campaigns = $"'{string.Join("','", callCampaigns)}'";
            Console.WriteLine("Pulling outbound data from Noble");
            List<OutboundBasePopulation> basePopulation = GetOutboundBasePopulation(campaignPrefixes);
            List<OutboundData> batchRecords = GetOutboundRecords(basePopulation);
            LoadOutboundData(batchRecords);
        }

        /// <summary>
        /// Gets the records 
        /// </summary>
        /// <param name="campaigns"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.UnexsysReports, "GetOutboundBasePopulation")]
        private List<OutboundBasePopulation> GetOutboundBasePopulation(List<CampaignPrefixes> campaignPrefixes)
        {
            return LDA.ExecuteList<OutboundBasePopulation>("GetOutboundBasePopulation", DataAccessHelper.Database.UnexsysReports, SqlParams.Single("Campaigns", campaignPrefixes.ToDataTable())).CheckResult();
        }

        /// <summary>
        /// Gets the records 
        /// </summary>
        /// <param name="campaigns"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.NobleCalls, "GetOutboundCalls")]
        private List<OutboundData> GetOutboundRecords(List<OutboundBasePopulation> basePopulation)
        {
            //Parallel.ForEach(basePopulation, item =>
            //{
            //    item.AccountIdentifier = item.AccountIdentifier.IsPopulated() ? item.AccountIdentifier.Trim().Replace(" ", "") : item.AccountIdentifier;
            //    item.AccountIdentifier = item.AccountIdentifier.IsPopulated() ? item.AccountIdentifier.Trim().Replace("|", "") : item.AccountIdentifier;
            //    item.AccountIdentifier = item.AccountIdentifier.IsPopulated() ? item.AccountIdentifier.Trim().Replace("-", "") : item.AccountIdentifier;
            //});
            return LDA.ExecuteList<OutboundData>("GetOutboundCalls", DataAccessHelper.Database.NobleCalls, SqlParams.Single("OutboundPopulation", basePopulation.ToDataTable())).CheckResult();
        }

        /// <summary>
        /// Loads the data into the NobleCallHistory table
        /// </summary>
        /// <param name="batchRecords">List of the DbData records</param>
        /// <param name="isInbound">Determines if the call was an inbound or outbound call</param>
        [UsesSproc(DataAccessHelper.Database.NobleCalls, "InsertCallHistory")]
        private void LoadOutboundData(List<OutboundData> batchRecords)
        {

            Console.WriteLine("Loading data into NobleCalls.NobleCallHistory on {0}", DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev ? "OPSDEV" : "UheaaSqlDB");
            Parallel.ForEach(batchRecords, item =>
            {
                item.CleanseData();
            });
            LDA.Execute("InsertCallHistory", DataAccessHelper.Database.NobleCalls, SqlParams.Single("Data", batchRecords.ToDataTable()));
        }
        #endregion

        #region NobleCallHistoryData

        [UsesSproc(DataAccessHelper.Database.NobleCalls, "UpdateProcessedAt")]
        public void UpdateProcessedAt(long arcAddProcessingId, int nobleCallHistoryId)
        {
            LDA.Execute("UpdateProcessedAt", DataAccessHelper.Database.NobleCalls, SqlParams.Single("ArcAddProcessingId", arcAddProcessingId), SqlParams.Single("NobleCallHistoryId", nobleCallHistoryId));
        }

        [UsesSproc(DataAccessHelper.Database.NobleCalls, "GetCallsForRegion")]
        public List<NobleCallHistoryData> Populate(string region)
        {
            return LDA.ExecuteList<NobleCallHistoryData>("GetCallsForRegion", DataAccessHelper.Database.NobleCalls, SqlParams.Single("Region", region)).CheckResult();
        }
        #endregion

        #region ArcComments
        /// <summary>
        /// Gets the Arc and Comment for a noble disposition code
        /// </summary>
        /// <param name="dispositionCode"></param>
        /// <param name="accountNumber"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.NobleCalls, "GetArcAndCommentForResultCode")]
        public ArcCommentResponse GetDataFromDisposition(string dispositionCode, string accountNumber)
        {
            try
            {
                if (dispositionCode.IsPopulated())
                {
                    ArcCommentResponse result = LDA.ExecuteSingle<ArcCommentResponse>("GetArcAndCommentForResultCode", DataAccessHelper.Database.NobleCalls, SqlParams.Single("DispositionCode", dispositionCode)).CheckResult();
                    if (result.Arc.IsPopulated())
                        return result;
                    else
                        throw new Exception();
                }
                else
                    throw new Exception();
            }
            catch (Exception)
            {
                string message = $"A phone attempt was made on account {(accountNumber.IsPopulated() ? accountNumber : "UNKNOWN")},  but an unknown disposition code '{dispositionCode}' was found";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return new ArcCommentResponse() { Arc = "DDPHN", Comment = $"A phone attempt was made but an unknown disposition code {dispositionCode} was found", ResponseCode = "NOCTC" };
            }
        }
        #endregion
    }
}
