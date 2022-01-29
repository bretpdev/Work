using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace FINALREV
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun) => LDA = logRun.LDA;

        [UsesSproc(BatchProcessing, "spGetDecrpytedPassword")]
        public List<string> GetPassword(string userId) => LDA.ExecuteList<string>("spGetDecrpytedPassword", BatchProcessing, SP("UserId", userId)).Result;

        /// <summary>
        /// Gets a list of all the schools for unprocessed borrowers
        /// </summary>
        /// <returns></returns>
        [UsesSproc(Uls, "finalrev.GetSchools")]
        public List<string> GetSchools(int borrowerRecordId) => LDA.ExecuteList<string>("finalrev.GetSchools", Uls, SP("BorrowerRecordId", borrowerRecordId)).Result;

        /// <summary>
        /// Checks for recovery and returns the SSN and Step it was on
        /// </summary>
        [UsesSproc(Uls, "finalrev.GetRecoveryValues")]
        public BorrowerRecord CheckUnprocessedBorrower() => LDA.ExecuteSingle<BorrowerRecord>("finalrev.GetRecoveryValues", Uls).Result;

        /// <summary>
        /// Gets borrower demographics info that matches a SystemBorrowerDemographic object
        /// </summary>
        [UsesSproc(Udw, "GetSystemBorrowerDemographics")]
        public SystemBorrowerDemographics GetDemos(string accountIdentifier) => LDA.ExecuteSingle<SystemBorrowerDemographics>("GetSystemBorrowerDemographics", Udw, SP("AccountIdentifier", accountIdentifier)).Result;

        /// <summary>
        /// Gets the number of tasks that have been processed
        /// </summary>
        /// <returns></returns>
        [UsesSproc(Uls, "finalrev.GetTaskCount")]
        public int GetTaskCount() => LDA.ExecuteSingle<int>("finalrev.GetTaskCount", Uls).Result;

        /// <summary>
        /// Updates the current recovery value
        /// </summary>
        [UsesSproc(Uls, "finalrev.InsertBorrowerRecord")]
        public int InsertBorrowerRecord(int step, string ssn) => LDA.ExecuteId<int>("finalrev.InsertBorrowerRecord", Uls, SP("Ssn", ssn), SP("Step", step));

        /// <summary>
        /// Inserts a new records that ties the borrower id to the school id
        /// </summary>
        [UsesSproc(Uls, "finalrev.InsertBorrowerSchool")]
        public void InsertBorrowerSchool(int borrowerRecordId, int schoolsId) => LDA.Execute("finalrev.InsertBorrowerSchool", Uls, SP("BorrowerRecordId", borrowerRecordId), SP("SchoolsId", schoolsId));

        /// <summary>
        /// Updates the step that just finished processing
        /// </summary>
        [UsesSproc(Uls, "finalrev.UpdateStep")]
        public void UpdateStep(int borrowerRecordId, FinalReview.RecoveryStep step) => LDA.Execute("finalrev.UpdateStep", Uls, SP("BorrowerRecordId", borrowerRecordId), SP("Step", step));

        /// <summary>
        /// Inserts a new school if it does not already exist
        /// </summary>
        [UsesSproc(Uls, "finalrev.InsertSchool")]
        public int InsertSchool(string schoolCode) => LDA.ExecuteId<int>("finalrev.InsertSchool", Uls, SP("SchoolCode", schoolCode));

        /// <summary>
        /// Updates the recovery account to processed at for historical purposes
        /// </summary>
        [UsesSproc(Uls, "finalrev.SetProcessedAt")]
        public void SetProcessedAt(int borrowerRecordId) => LDA.Execute("finalrev.SetProcessedAt", Uls, SP("BorrowerRecordId", borrowerRecordId));

        /// <summary>
        /// Gets all the available records that need to be printed
        /// </summary>
        [UsesSproc(Uls, "finalrev.GetSchoolLetterData")]
        public List<SchoolLetterData> GetSchoolLetterData() => LDA.ExecuteList<SchoolLetterData>("finalrev.GetSchoolLetterData", Uls).Result;

        /// <summary>
        /// Sets the Letter Sent date showing the letter was sent to the school for the borrower.
        /// </summary>
        /// <param name="borrowerRecordId"></param>
        [UsesSproc(Uls, "finalrev.SetSchoolLetterSent")]
        public void SetSchoolLetterSent(int borrowerRecordId) => LDA.Execute("finalrev.SetSchoolLetterSent", Uls, SP("BorrowerRecordId", borrowerRecordId));

        /// <summary>
        /// Gets a list of lender codes
        /// </summary>
        /// <returns></returns>
        [UsesSproc(Bsys, "GetLenderCodes")]
        public List<string> GetLenderCodes() => LDA.ExecuteList<string>("GetLenderCodes", Bsys).Result;

        public SqlParameter SP(string name, object value) => SqlParams.Single(name, value);
    }
}