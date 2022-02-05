using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace TRDPRTYRES
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda) =>
            LDA = lda;

        /// <summary>
        /// Gets the list of states
        /// </summary>
        [UsesSproc(Csys, "spGENR_GetStateCodes")]
        public List<string> GetStateCodes() =>
            LDA.ExecuteList<string>("spGENR_GetStateCodes", Csys).Result;

        /// <summary>
        /// Checks the Print Processing tables for a matching letter added on the same date
        /// Only one letter is allowed per day for the borrower with the same letter id, letter data and script id
        /// </summary>
        /// <returns>True: A letter was found for today; No: No letters added for today</returns>
        [UsesSproc(Uls, "[print].CheckForCurrentLetter")]
        public bool CheckForExistingLetter(string accountNumber, string letterId, string scriptId, string letterData) =>
            LDA.ExecuteSingle<int>("[print].CheckForCurrentLetter", Uls,
                SP("AccountNumber", accountNumber),
                SP("LetterId", letterId),
                SP("LetterData", letterData),
                SP("ScriptId", scriptId)).Result > 0;

        [UsesSproc(Odw, "GetSystemBorrowerDemographics")]
        public SystemBorrowerDemographics GetOnelinkDemos(string ssn) =>
            LDA.ExecuteSingle<SystemBorrowerDemographics>("GetSystemBorrowerDemographics", Odw, SP("AccountIdentifier", ssn)).Result;

        [UsesSproc(Udw, "GetSystemBorrowerDemographics")]
        public SystemBorrowerDemographics GetCompassDemos(string ssn) =>
            LDA.ExecuteSingle<SystemBorrowerDemographics>("GetSystemBorrowerDemographics", Udw, SP("AccountIdentifier", ssn)).Result;

        [UsesSproc(Odw, "trdprtyres.GetReferences")]
        public List<References> GetOnelinkReferences(string ssn) =>
            LDA.ExecuteList<References>("trdprtyres.GetReferences", Odw, SP("AccountIdentifier", ssn)).Result;

        [UsesSproc(Udw, "trdprtyres.GetReferences")]
        public List<References> GetCompassReferences(string ssn) =>
            LDA.ExecuteList<References>("trdprtyres.GetReferences", Udw, SP("AccountIdentifier", ssn)).Result;

        public bool CheckLoanStatus(string ssn) =>
            LDA.ExecuteSingle<int>("trdprtyres.CheckLoanStatus", Odw, SP("BF_SSN", ssn)).Result > 0;

        [UsesSproc(Udw, "trdprtyres.HasOpenLoans")]
        public bool HasOpenLoans(string ssn) =>
            LDA.ExecuteSingle<decimal>("trdprtyres.HasOpenLoans", Udw, SP("AccountIdentifier", ssn)).Result > 0;

        [UsesSproc(Uls, "trdprtyres.GetSources")]
        public List<Sources> GetSources(bool isOneLink) =>
            LDA.ExecuteList<Sources>("trdprtyres.GetSources", Uls, SP("IsOnelink", isOneLink)).Result;

        [UsesSproc(Uls, "trdprtyres.GetRelationships")]
        public List<Relationships> GetRelationships(bool isOneLink) =>
            LDA.ExecuteList<Relationships>("trdprtyres.GetRelationships", Uls, SP("IsOnelink", isOneLink)).Result;

        private SqlParameter SP(string name, object value) => SqlParams.Single(name, value);
    }
}