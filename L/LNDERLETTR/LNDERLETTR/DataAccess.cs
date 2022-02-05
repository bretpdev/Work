using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;

namespace LNDERLETTR
{
    public class DataAccess
    {
        private ProcessLogRun Plr { get; set; }

        public DataAccess(ProcessLogRun PLR)
        {
            Plr = PLR;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "spGetCostCenterInstructions")]
        public string GetCostCenterInstructions(string letterId)
        {
            return Plr.LDA.ExecuteSingle<string>("spGetCostCenterInstructions", DataAccessHelper.Database.Bsys, SqlParams.Single("letterId", letterId)).Result;
        }

        /// <summary>
        /// Gets Business manager for unit.
        /// </summary>

        [UsesSproc(DataAccessHelper.Database.Bsys, "GetManagerOfBusinessUnit")]
        public string ManagerId()
        {
            string buName = EnterpriseFileSystem.GetPath($"LNDERLETTR_BU", DataAccessHelper.Region.Uheaa);
            return Plr.LDA.ExecuteSingle<string>("GetManagerOfBusinessUnit", DataAccessHelper.Database.Bsys, SqlParams.Single("BusinessUnit", buName)).Result;
        }

        /// <summary>
        /// Loads all the borrowers needing a letter to the lnderletter.Letters table
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[lnderlettr].InsertLetterData")]
        public void LoadData()
        {
            Plr.LDA.Execute("[lnderlettr].InsertLetterData", DataAccessHelper.Database.Uls);
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[lnderlettr].GetLenderData")]
        public LenderData GetLenderData(string lenderId)
        {
            return Plr.LDA.ExecuteSingle<LenderData>("[lnderlettr].GetLenderData", DataAccessHelper.Database.Uls,
                SqlParams.Single("WF_ORG_LDR", lenderId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[lnderlettr].GetBorrowerData")]
        public BorrowerData GetBorrowerData(string ssn)
        {
            BorrowerData bd = null;
            try
            {
                bd = Plr.LDA.ExecuteSingle<BorrowerData>("[lnderlettr].GetBorrowerData", DataAccessHelper.Database.Uls,
                      SqlParams.Single("BF_SSN", ssn)).Result;
            }
            catch (System.Exception ex)
            {
                Plr.AddNotification(string.Format("Unable to retrieve demographic data for Borrower {0} ", ssn),
                NotificationType.HandledException, NotificationSeverityType.Informational);
            }
            return bd;
        }

        /// <summary>
        /// Gets the list of lenders that will receive a letter
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[lnderlettr].GetPendingWork")]
        public List<LetterData> GetPendingWork()
        {
            return Plr.LDA.ExecuteList<LetterData>("[lnderlettr].GetPendingWork", DataAccessHelper.Database.Uls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[lnderlettr].SetLetterCreatedAt")]
        public bool SetLetterCreatedAt(int LettersId)
        {
            return Plr.LDA.Execute("[lnderlettr].SetLetterCreatedAt", DataAccessHelper.Database.Uls, SqlParams.Single("LettersId", LettersId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[lnderlettr].SetArcAddId")]
        public bool SetArcAddId(int lettersId, long arcAddProcessingId)
        {
            return Plr.LDA.Execute("[lnderlettr].SetArcAddId", DataAccessHelper.Database.Uls,
                SqlParams.Single("LettersId", lettersId),
                SqlParams.Single("ArcAddProcessingId", arcAddProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[lnderlettr].SetQueueClosedAt")]
        public bool SetQueueClosedAt(int lettersId)
        {
            return Plr.LDA.Execute("[lnderlettr].SetQueueClosedAt", DataAccessHelper.Database.Uls, SqlParams.Single("LettersId", lettersId));
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "[dbo].spGetBUCostCenterByLetter")]
        public CoverData GetCoverSheetData(string LetterName)
        {
            return Plr.LDA.ExecuteSingle<CoverData>("[dbo].spGetBUCostCenterByLetter", DataAccessHelper.Database.Bsys,
                SqlParams.Single("LetterName", LetterName)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "[dbo].spGetAccountNumberFromSSN")]
        public string GetSpAcctId(string ssn)
        {
            return Plr.LDA.ExecuteSingle<string>("[dbo].spGetAccountNumberFromSSN", DataAccessHelper.Database.Udw,
                SqlParams.Single("Ssn", ssn)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[lnderlettr].SetErroredAt")]
        public void UpdateError(int id)
        {
            Plr.LDA.Execute("[lnderlettr].[SetErroredAt]", DataAccessHelper.Database.Uls,
                SqlParams.Single("LetterId", id));

        }
    }
}