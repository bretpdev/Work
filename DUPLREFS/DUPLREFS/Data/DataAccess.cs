using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DUPLREFS
{
    public class DataAccess
    {
        ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "spGENR_GetStateCodes")]
        public List<string> GetStates()
        {
            return LogRun.LDA.ExecuteList<string>("spGENR_GetStateCodes", DataAccessHelper.Database.Bsys,
                SP("IncludeTerritories", false)).Result;
        }

        #region table loaders
        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.InsertBorrowerRecord")]
        public int? InsertBorrowerRecord(string accountNumber, string userId)
        {
            return LogRun.LDA.ExecuteIdNullable<int?>("duplrefs.InsertBorrowerRecord", DataAccessHelper.Database.Ols,
                SP("AccountNumber", accountNumber), SP("UserId", userId));
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.UpdateReferenceRecord")]
        public bool UpdateReferenceRecord(Reference reference)
        {
            return LogRun.LDA.Execute("duplrefs.UpdateReferenceRecord", DataAccessHelper.Database.Ols,
                SP("ReferenceQueueId", reference.ReferenceQueueId),
                SP("RefId", reference.RefId),
                SP("RefName", reference.RefName),
                SP("RefAddress1", reference.RefAddress1),
                SP("RefAddress2", reference.RefAddress2),
                SP("RefCity", reference.RefCity),
                SP("RefState", reference.RefState),
                SP("RefZip", reference.RefZip),
                SP("RefCountry", reference.RefCountry),
                SP("RefPhone", reference.RefPhone),
                SP("RefStatus", reference.RefStatus),
                SP("ValidAddress", reference.ValidAddress),
                SP("ValidPhone", reference.ValidPhone),
                SP("DemosChanged", reference.DemosChanged),
                SP("ZipChanged", reference.ZipChanged),
                SP("Duplicate", reference.Duplicate),
                SP("PossibleDuplicate", reference.PossibleDuplicate));
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.InsertReferenceRecords")]
        public bool InsertReferenceRecords(Borrower bwr)
        {
            bool result = LogRun.LDA.Execute("duplrefs.InsertReferenceRecords", DataAccessHelper.Database.Ols, SP("BorrowerQueueId", bwr.BorrowerQueueId), SP("ReferenceRecords", bwr.References.ToDataTable()));
            if (!result)
                LogRun.AddNotification($"Failed to insert references for account {bwr.AccountNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            return result;
        }
        #endregion

        #region setters
        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.SetBorrowerProcessedAt")]
        public bool SetBorrowerProcessedAt(int? borrowerQueueId)
        {
            return LogRun.LDA.Execute("duplrefs.SetBorrowerProcessedAt", DataAccessHelper.Database.Ols,
                SP("BorrowerQueueId", borrowerQueueId));
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.SetReferenceProcessedAt")]
        public bool SetReferenceProcessedAt(int? referenceQueueId)
        {
            return LogRun.LDA.Execute("duplrefs.SetReferenceProcessedAt", DataAccessHelper.Database.Ols,
                SP("ReferenceQueueId", referenceQueueId));
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.SetReferenceLp2fProcessedAt")]
        public bool SetReferenceLp2fProcessedAt(int? referenceQueueId)
        {
            return LogRun.LDA.Execute("duplrefs.SetReferenceLp2fProcessedAt", DataAccessHelper.Database.Ols,
                SP("ReferenceQueueId", referenceQueueId));
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.SetReferenceArcAddId")]
        public bool SetReferenceArcAddProcessingId(int? referenceQueueId, int? arcAddProcessingId)
        {
            return LogRun.LDA.Execute("duplrefs.SetReferenceArcAddId", DataAccessHelper.Database.Ols,
                SP("ReferenceQueueId", referenceQueueId), SP("ArcAddProcessingId", arcAddProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.SetDemosChanged")]
        public bool SetDemosChanged(int? referenceQueueId, bool wasUpdated)
        {
            bool result = LogRun.LDA.Execute("duplrefs.SetDemosChanged", DataAccessHelper.Database.Ols,
                SP("ReferenceQueueId", referenceQueueId), SP("DemosChanged", wasUpdated));
            if (!result)
                LogRun.AddNotification($"Failed to update DemosChanged to {(wasUpdated ? "true" : "false")} for ReferenceQueueId: {referenceQueueId}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            
            return result;
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.SetManuallyWorked")]
        public bool SetManuallyWorked(int? referenceQueueId, bool manuallyWorked)
        {
            return LogRun.LDA.Execute("duplrefs.SetManuallyWorked", DataAccessHelper.Database.Ols,
                SP("ReferenceQueueId", referenceQueueId), SP("ManuallyWorked", manuallyWorked));
        }
        #endregion

        #region recovery
        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.GetReferences")]
        public List<Reference> GetReferences(int? borrowerQueueId)
        {
            return LogRun.LDA.ExecuteList<Reference>("duplrefs.GetReferences", DataAccessHelper.Database.Ols,
                SP("BorrowerQueueId", borrowerQueueId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.GetRecoveryData")]
        public RecoveryData GetRecoveryData(string accountNumber)
        {
            return LogRun.LDA.ExecuteSingle<RecoveryData>("duplrefs.GetRecoveryData", DataAccessHelper.Database.Ols,
                SP("AccountNumber", accountNumber)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.GetUnprocessedBorrower")]
        public Borrower GetUnprocessedBorrower(int? borrowerQueueId)
        {
            return LogRun.LDA.ExecuteSingle<Borrower>("duplrefs.GetUnprocessedBorrower", DataAccessHelper.Database.Ols,
                SP("BorrowerQueueId", borrowerQueueId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.InactivateRecord")]
        public bool InactivateRecord(int? borrowerQueueId)
        {
            return LogRun.LDA.ExecuteSingle<bool>("duplrefs.InactivateRecord", DataAccessHelper.Database.Ols,
                SP("BorrowerQueueId", borrowerQueueId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "duplrefs.CleanUp")]
        public bool CleanUp()
        {
            return LogRun.LDA.Execute("duplrefs.CleanUp", DataAccessHelper.Database.Ols);
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
