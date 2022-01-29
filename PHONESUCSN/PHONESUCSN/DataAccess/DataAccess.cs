using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace PHONESUCSN
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun) =>
            LogRun = logRun;

        /// <summary>
        /// Loads the Endorser succession data
        /// </summary>
        [UsesSproc(Uls, "phonesucsn.LoadSuccessionDataEndorser")]
        public void LoadSuccessionDataEndorser() =>
            LogRun.LDA.Execute("phonesucsn.LoadSuccessionDataEndorser", Uls);

        /// <summary>
        /// Loads the Endorser duplicate data
        /// </summary>
        [UsesSproc(Uls, "phonesucsn.LoadDuplicateDataEndorser")]
        public void LoadDuplicateDataEndorser() =>
            LogRun.LDA.Execute("phonesucsn.LoadDuplicateDataEndorser", Uls);

        /// <summary>
        /// Gets the succession data from the PhoneSuccession table in ULS
        /// </summary>
        [UsesSproc(Uls, "phonesucsn.GetSuccessionData")]
        public List<PhoneData> GetSuccessionData(int count) =>
            LogRun.LDA.ExecuteList<PhoneData>("phonesucsn.GetSuccessionData", Uls, Sp("Count", count)).Result;

        /// <summary>
        /// Gets the duplicate data from the PhoneSuccession table in ULS
        /// </summary>
        [UsesSproc(Uls, "phonesucsn.GetDuplicateData")]
        public List<PhoneData> GetDuplicateData(int count) =>
            LogRun.LDA.ExecuteList<PhoneData>("phonesucsn.GetDuplicateData", Uls, Sp("Count", count)).Result;

        /// <summary>
        /// Sets the current record as processed after the number has been updated in the session
        /// </summary>
        [UsesSproc(Uls, "phonesucsn.SetProcessedAt")]
        public void MarkProcessed(int phoneSuccessionId) =>
            LogRun.LDA.Execute("phonesucsn.SetProcessedAt", Uls,
                Sp("PhoneSuccessionId", phoneSuccessionId));

        /// <summary>
        /// Sets the current record as invalidated after the number has been invalidated in the session
        /// </summary>
        [UsesSproc(Uls, "phonesucsn.SetInvalidatedAt")]
        public void MarkInvalidatedAt(int phoneSuccessionId) =>
            LogRun.LDA.Execute("phonesucsn.SetInvalidatedAt", Uls,
                Sp("PhoneSuccessionId", phoneSuccessionId));

        /// <summary>
        /// Updates the record as having an error in processing
        /// </summary>
        [UsesSproc(Uls, "phonesucsn.SetError")]
        public void LogError(int phoneSuccessionId) =>
            LogRun.LDA.Execute("phonesucsn.SetError", Uls,
                Sp("PhoneSuccessionId", phoneSuccessionId));

        /// <summary>
        /// Gets the borrower ssn for the endorser
        /// </summary>
        [UsesSproc(Udw, "GetBorrowerForEndorser")]
        public string GetBorrowerSsn(string ssn) =>
            LogRun.LDA.ExecuteSingle<string>("GetBorrowerForEndorser", Udw, Sp("Endorser", ssn)).Result;

        private SqlParameter Sp(string name, object value) =>
            SqlParams.Single(name, value);
    }
}