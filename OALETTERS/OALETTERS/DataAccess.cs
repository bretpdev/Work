using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.DataAccess;

namespace OALETTERS
{
    public class DataAccess
    {
        /// <summary>
        /// Gets the borrower demographics data
        /// </summary>
        /// <param name="accountIdentifier"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "GetOABorrower")]
        public BorrowerData GetBorrowerData(string accountIdentifier)
        {
            return DataAccessHelper.ExecuteList<BorrowerData>("GetOABorrower", DataAccessHelper.Database.Udw,
                SqlParams.Single("AccountIdentifier", accountIdentifier)).FirstOrDefault();
        }

        /// <summary>
        /// Gets a list of the state codes
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetStateCodes")]
        public List<string> GetStateCodes()
        {
            return DataAccessHelper.ExecuteList<string>("spGENR_GetStateCodes", DataAccessHelper.Database.Csys);
        }

        /// <summary>
        /// Gets the list of available operational accounting letters
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "GetOALetters")]
        public List<LetterTypes> GetLetters()
        {
            return DataAccessHelper.ExecuteList<LetterTypes>("GetOALetters", DataAccessHelper.Database.Uls);
        }

        /// <summary>
        /// Gets the cost center code for the letter being printed
        /// </summary>
        /// <param name="letterType"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Bsys, "spGetCostCenterCodeForLetter")]
        public string GetCostCenter(string letterType)
        {
            return DataAccessHelper.ExecuteSingle<string>("spGetCostCenterCodeForLetter", DataAccessHelper.Database.Bsys,
                SqlParams.Single("LetterType", letterType));
        }
    }
}