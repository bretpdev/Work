using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace RHBRWINPC
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        /// <summary>
        /// Pulls the available data from the remote warehouse and loads any unprocessed records into the local table in Ols
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Ols, "[rhbrwinpc].[GetAvailableLetterData]")]
        public void LoadNewLetters() => LogRun.LDA.Execute("[rhbrwinpc].[GetAvailableLetterData]", DataAccessHelper.Database.Ols);

        /// <summary>
        /// Gets a list of all the accounts that need to be processed
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Ols, "[rhbrwinpc].[GetLetters]")]
        public List<LetterData> GetLetterData() => LogRun.LDA.ExecuteList<LetterData>("[rhbrwinpc].[GetLetters]", DataAccessHelper.Database.Ols).Result;

        /// <summary>
        /// Updates the letter to show it's been printed
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Ols, "[rhbrwinpc].[SetPrintedAt]")]
        public void SetPrintedAt(int lettersId) => LogRun.LDA.Execute("[rhbrwinpc].[SetPrintedAt]", DataAccessHelper.Database.Ols,
                    SqlParams.Single("LettersId", lettersId));

        /// <summary>
        /// Sets the ARC as processed for the account
        /// </summary>
        /// <param name="lettersId"></param>
        [UsesSproc(DataAccessHelper.Database.Ols, "[rhbrwinpc].[SetArcAddId]")]
        public void SetArcAddId(int lettersId, int arcAddProcessingId) => LogRun.LDA.Execute("[rhbrwinpc].[SetArcAddId]", DataAccessHelper.Database.Ols,
                SqlParams.Single("LettersId", lettersId),
                SqlParams.Single("ArcAddProcessingId", arcAddProcessingId));
    }
}