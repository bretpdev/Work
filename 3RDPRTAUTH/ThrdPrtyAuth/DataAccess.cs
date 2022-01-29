using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System.Collections.Generic;

namespace ThrdPrtyAuth
{
    public class DataAccess
    {
        public ProcessLogRun PLR { get; set; }
        public string ScriptId { get { return "AUXLTRS"; } }

        public DataAccess(ProcessLogRun plr)
        {
            PLR = plr;
        }


        [UsesSproc(DataAccessHelper.Database.Uls, "[print].InsertOneLinkPrintProcessingRecord")]
        public int? AddOneLink(string scriptId, string letterId, string letterData, string accountNumber, string costCenter)
        {
            return PLR.LDA.ExecuteSingle<int>("[print].[InsertOneLinkPrintProcessingRecord]", DataAccessHelper.Database.Uls,
                SqlParams.Single("ScriptId", scriptId),
                SqlParams.Single("LetterId", letterId),
                SqlParams.Single("LetterData", letterData),
                SqlParams.Single("AccountNumber", accountNumber),
                SqlParams.Single("CostCenter", costCenter)
                ).Result;
        }


    }
}
