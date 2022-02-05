using System;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using WinSCP;

namespace FSAMTHCALL
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        public DataAccess(int plId)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, plId, false, true);
        }

        /// <summary>
        /// Gets All Calls Made last month/since inception based upon the fullRefresh parameter
        /// </summary>
        /// /// <param name="fullRefresh"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.NobleCalls, "GetAllCallsForThePreviousMonth")]
        [UsesSproc(DataAccessHelper.Database.NobleCalls, "GetAllUnreconciledCalls")]
        public List<NobleData> GetUnreconciledCalls(bool fullRefresh)
        {
            if(fullRefresh)
                return LDA.ExecuteList<NobleData>("GetAllUnreconciledCalls", DataAccessHelper.Database.NobleCalls).CheckResult();
            else
                return LDA.ExecuteList<NobleData>("GetAllCallsForThePreviousMonth", DataAccessHelper.Database.NobleCalls).CheckResult();
        }

        /// <summary>
        /// Gets the SFTP password for Batch Processing
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "spGetDecrpytedPassword")]
        public string GetNoblePassword(string user)
        {
            return LDA.ExecuteSingle<string>("spGetDecrpytedPassword", DataAccessHelper.Database.BatchProcessing, SqlParams.Single("UserId", user)).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.NobleCalls, "GetAllReconciledCallsForPreviousMonth")]
        public List<NobleData> GetValidCalls()
        {
            return LDA.ExecuteList<NobleData>("GetAllReconciledCallsForPreviousMonth", DataAccessHelper.Database.NobleCalls).CheckResult();
        }

        /// <summary>
        /// Gets the NobleData for a given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.NobleCalls, "GetCallDataFromId")]
        public NobleData GetCallDataFromId(int id)
        {
            return LDA.ExecuteSingle<NobleData>("GetCallDataFromId", DataAccessHelper.Database.NobleCalls, SqlParams.Single("CallId", id)).CheckResult();
        }

        /// <summary>
        /// Updates NobleCallHistory with the vox file location
        /// </summary>
        /// <param name="callId"></param>
        /// <param name="location"></param>
        [UsesSproc(DataAccessHelper.Database.NobleCalls, "UpdateVoxFileLocation")]
        public void UpdateVoxFileLocation(int callId, string location)
        {
            LDA.Execute("UpdateVoxFileLocation", DataAccessHelper.Database.NobleCalls, SqlParams.Single("CallId", callId), SqlParams.Single("VoxFileLocation", location));
        }

        [UsesSproc(DataAccessHelper.Database.NobleCalls, "GetSpecialCampaignIds")]
        public List<string> GetSpecialCampaigns()
        {
            return LDA.ExecuteList<string>("GetSpecialCampaignIds", DataAccessHelper.Database.NobleCalls).CheckResult();
        }

        public SessionOptions GetSessionOptions(string userId, string password)
        {
            return new SessionOptions()
            {
                FtpMode = FtpMode.Active,
                Protocol = Protocol.Sftp,
                UserName = userId,
                Password = password,
                HostName = "172.16.3.117",

                SshHostKeyFingerprint = EnterpriseFileSystem.GetPath("FSAMTHCALL_Fingerprint"),
            };
        }
    }
}
