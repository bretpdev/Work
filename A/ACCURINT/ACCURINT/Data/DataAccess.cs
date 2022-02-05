using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace ACCURINT
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }

        [UsesSproc(DB.Bsys, "GetManagerOfBusinessUnit")]
        public string GetBuManager()
        {
            string buName = EnterpriseFileSystem.GetPath($"ACCURINT_BU", DataAccessHelper.Region.Uheaa);
            return DataAccessHelper.ExecuteSingle<string>("GetManagerOfBusinessUnit", DataAccessHelper.Database.Bsys, SqlParams.Single("BusinessUnit", buName));
        }

        [UsesSproc(DB.BatchProcessing, "spGetDecrpytedPassword")]
        public string GetBatchProcessingPassword(string userId)
        {
            return DataAccessHelper.ExecuteSingle<string>("spGetDecrpytedPassword", DataAccessHelper.Database.BatchProcessing, SqlParams.Single("UserId", userId));
        }

        #region Both Regions
        [UsesSproc(DB.Uls, "accurint.AddNewWork")]
        public bool AddNewWork(int runId)
        {
            return LDA.ExecuteSingle<bool>("[accurint].AddNewWork", DB.Uls, SP("RunId", runId)).Result;
        }

        [UsesSproc(DB.Uls, "accurint.CreateNewRun")]
        public RunInfo CreateRun()
        {
            return LDA.ExecuteSingle<RunInfo>("accurint.CreateNewRun", DB.Uls).Result;
        }

        [UsesSproc(DB.Uls, "accurint.DeleteRun")]
        public bool DeleteRun(int runId)
        {
            return LDA.ExecuteSingle<bool>("[accurint].DeleteRun", DB.Uls, SP("RunId", runId)).Result;
        }

        [UsesSproc(DB.Uls, "accurint.GetRunInfo")]
        public RunInfo GetRunInfo()
        {
            return LDA.ExecuteSingle<RunInfo>("[accurint].GetRunInfo", DB.Uls).Result;
        }

        [UsesSproc(DB.Uls, "accurint.GetRecordsSentCount")]
        public int GetRecordsSentCount(int runId)
        {
            return LDA.ExecuteSingle<int>("[accurint].GetRecordsSentCount", DB.Uls, SP("RunId", runId)).Result;
        }

        [UsesSproc(DB.Uls, "accurint.SetRequestFileInfo")]
        public bool SetRequestFileInfo(int runId, int recordsSent)
        {
            return LDA.Execute("[accurint].SetRequestFileInfo", DB.Uls, SP("RunId", runId), SP("RecordsSent", recordsSent));
        }

        [UsesSproc(DB.Uls, "accurint.SetRequestFileUploaded")]
        public bool SetRequestUploaded(int runId, string requestFileName)
        {
            return LDA.Execute("[accurint].SetRequestFileUploaded", DB.Uls, SP("RunId", runId), SP("RequestFileName", requestFileName));
        }

        [UsesSproc(DB.Uls, "accurint.SetResponseFilesDownloaded")]
        public bool SetResponseFilesDownloaded(int runId, DateTime? responseFileDownloadedAt)
        {
            return LDA.Execute("[accurint].SetResponseFilesDownloaded", DB.Uls, SP("RunId", runId), SP("ResponseFilesDownloadedAt", responseFileDownloadedAt));
        }

        [UsesSproc(DB.Uls, "accurint.SetResponseFilesProcessed")]
        public bool SetResponseFilesProcessed(int runId)
        {
            return LDA.Execute("[accurint].SetResponseFilesProcessed", DB.Uls, SP("RunId", runId));
        }

        [UsesSproc(DB.Uls, "accurint.UpdateRecordsReceivedCount")]
        public bool UpdateRecordsReceivedCount(int runId, int recordsReceived)
        {
            return LDA.Execute("[accurint].UpdateRecordsReceivedCount", DB.Uls, SP("RunId", runId), SP("RecordsReceived", recordsReceived));
        }

        [UsesSproc(DB.Uls, "accurint.AddNewResponseFile")]
        public bool AddNewResponseFile(int runId, string responseFileName)
        {
            return LDA.Execute("[accurint].AddNewResponseFile", DB.Uls, SP("RunId", runId), SP("ResponseFileName", responseFileName));
        }

        [UsesSproc(DB.Uls, "accurint.GetResponseFiles")]
        public List<ResponseFile> GetResponseFiles(int runId)
        {
            return LDA.ExecuteList<ResponseFile>("[accurint].GetResponseFiles", DB.Uls, SP("RunId", runId)).Result;
        }

        [UsesSproc(DB.Uls, "accurint.SetResponseFileProcessed")]
        public bool SetResponseFileProcessed(int responseFileId)
        {
            return LDA.Execute("[accurint].SetResponseFileProcessed", DB.Uls, SP("ResponseFileId", responseFileId));
        }

        [UsesSproc(DB.Uls, "accurint.SetArchivedResponseFileName")]
        public bool SetArchivedResponseFileName(int responseFileId, string archivedFileName)
        {
            return LDA.Execute("[accurint].SetArchivedResponseFileName", DB.Uls, SP("ResponseFileId", responseFileId), SP("ArchivedFileName", archivedFileName));
        }

        [UsesSproc(DB.Uls, "accurint.AddSentDemos")]
        public bool AddSentDemos(int demosId, string region, string accountNumber, string sentAddress1, string sentAddress2, string sentCity, string sentState, string sentZipCode, string sentPhoneNumber, bool sentValidity)
        {
            return LDA.Execute("[accurint].AddSentDemos", DB.Uls, SP("DemosId", demosId), SP("Region", region), SP("AccountNumber", accountNumber), SP("SentAddress1", sentAddress1), SP("SentAddress2", sentAddress2), SP("SentCity", sentCity), SP("SentState", sentState), SP("SentZipCode", sentZipCode), SP("SentPhoneNumber", sentPhoneNumber ?? ""), SP("SentValidity", sentValidity));
        }

        [UsesSproc(DB.Uls, "accurint.AddReceivedDemos")]
        public bool AddReceivedDemos(int demosId, string region, string receivedAddress1, string receivedAddress2, string receivedCity, string receivedState, string receivedZipCode, string receivedPhoneNumber)
        {
            return LDA.Execute("[accurint].AddReceivedDemos", DB.Uls, SP("DemosId", demosId), SP("Region", region), SP("ReceivedAddress1", receivedAddress1), SP("ReceivedAddress2", receivedAddress2), SP("ReceivedCity", receivedCity), SP("ReceivedState", receivedState), SP("ReceivedZipCode", receivedZipCode), SP("ReceivedPhoneNumber", receivedPhoneNumber ?? ""));
        }
        #endregion

        #region OneLINK
        [UsesSproc(DB.Uls, "[accurint].OL_GetUnprocessedRecords")]
        public List<OneLinkDemosRecord> GetUnprocessedOLRecords(int runId)
        {
            return LDA.ExecuteList<OneLinkDemosRecord>("[accurint].OL_GetUnprocessedRecords", DB.Uls, SP("RunId", runId)).Result;
        }

        [UsesSproc(DB.Uls, "[accurint].OL_SetAddedToRequestFile")]
        public bool SetOLRecordAddedToRequestFile(int demosId)
        {
            return LDA.Execute("[accurint].OL_SetAddedToRequestFile", DB.Uls, SP("DemosId", demosId));
        }

        [UsesSproc(DB.Uls, "[accurint].OL_SetRequestCommentAdded")]
        public bool SetOneLinkRequestCommentAdded(int demosId)
        {
            return LDA.Execute("[accurint].OL_SetRequestCommentAdded", DB.Uls, SP("DemosId", demosId));
        }

        [UsesSproc(DB.Uls, "[accurint].OL_SetDeleted")]
        public bool SetOLDeleted(int demosId)
        {
            return LDA.Execute("[accurint].OL_SetDeleted", DB.Uls, SP("DemosId", demosId));
        }

        [UsesSproc(DB.Uls, "[accurint].OL_SetAddressTaskQueueId")]
        public bool SetOLAddressTaskQueueId(int demosId, int addressTaskQueueId)
        {
            return LDA.Execute("[accurint].OL_SetAddressTaskQueueId", DB.Uls, SP("AddressTaskQueueId", addressTaskQueueId), SP("DemosId", demosId));
        }

        [UsesSproc(DB.Uls, "[accurint].OL_SetPhoneTaskQueueId")]
        public bool SetOLPhoneTaskQueueId(int demosId, int phoneTaskQueueId)
        {
            return LDA.Execute("[accurint].OL_SetPhoneTaskQueueId", DB.Uls, SP("PhoneTaskQueueId", phoneTaskQueueId), SP("DemosId", demosId));
        }

        [UsesSproc(DB.Uls, "[accurint].OL_SetTaskCompleted")]
        public bool SetOLTaskCompleted(int demosId)
        {
            return LDA.Execute("[accurint].OL_SetTaskCompleted", DB.Uls, SP("DemosId", demosId));
        }

        [UsesSproc(DB.Uls, "[accurint].OL_AddSpecialRequestDemoRecord")]
        public OneLinkDemosRecord AddSpecialRequestDemoRecord(string accountNumber, string workGroup, string department, int runId)
        {
            return LDA.ExecuteSingle<OneLinkDemosRecord>("[accurint].OL_AddSpecialRequestDemoRecord", DB.Uls, SP("AccountNumber", accountNumber), SP("WorkGroup", workGroup), SP("Department", department), SP("RunId", runId)).Result;
        }
        #endregion

        #region UHEAA
        [UsesSproc(DB.Uls, "[accurint].UH_GetUnprocessedRecords")]
        public List<UheaaDemosRecord> GetUnprocessedUHRecords(int runId)
        {
            return LDA.ExecuteList<UheaaDemosRecord>("[accurint].UH_GetUnprocessedRecords", DB.Uls, SP("RunId", runId)).Result;
        }

        [UsesSproc(DB.Uls, "[accurint].UH_SetAddedToRequestFile")]
        public bool SetUHRecordAddedToRequestFile(int demosId)
        {
            return LDA.Execute("[accurint].UH_SetAddedToRequestFile", DB.Uls, SP("DemosId", demosId));
        }

        [UsesSproc(DB.Uls, "[accurint].UH_SetRequestArcId")]
        public bool SetUHRequestArcId(int demosId, int requestArcId)
        {
            return LDA.Execute("[accurint].UH_SetRequestArcId", DB.Uls, SP("DemosId", demosId), SP("RequestArcId", requestArcId));
        }

        [UsesSproc(DB.Uls, "[accurint].UH_SetResponseAddressArcId")]
        public bool SetUHResponseAddressArcId(int demosId, int responseAddressArcId)
        {
            return LDA.Execute("[accurint].UH_SetResponseAddressArcId", DB.Uls, SP("DemosId", demosId), SP("ResponseAddressArcId", responseAddressArcId));
        }

        [UsesSproc(DB.Uls, "[accurint].UH_SetResponsePhoneArcId")]
        public bool SetUHResponsePhoneArcId(int demosId, int responsePhoneArcId)
        {
            return LDA.Execute("[accurint].UH_SetResponsePhoneArcId", DB.Uls, SP("DemosId", demosId), SP("ResponsePhoneArcId", responsePhoneArcId));
        }

        [UsesSproc(DB.Uls, "[accurint].UH_SetDeleted")]
        public bool SetUHDeleted(int demosId)
        {
            return LDA.Execute("[accurint].UH_SetDeleted", DB.Uls, SP("DemosId", demosId));
        }

        [UsesSproc(DB.Uls, "[accurint].UH_SetTaskCompleted")]
        public bool SetUHTaskCompleted(int demosId)
        {
            return LDA.Execute("[accurint].UH_SetTaskCompleted", DB.Uls, SP("DemosId", demosId));
        }
        #endregion

        /// <summary>
        /// SQL parameterization wrapper: 
        /// parameterizes a string as the field name and
        /// an object as the value to be used for DB calls.
        /// </summary>
        public SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}