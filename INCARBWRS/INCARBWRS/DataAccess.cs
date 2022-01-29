using System.Linq;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;

namespace INCARBWRS
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        public DataAccess(ProcessLogRun plr)
        {
            LDA = plr.LDA;
        }

        /// <summary>
        /// Gets a list of ContactSources
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Bsys, "ContactSourcesGetAll")]
        public List<ContactSource> GetContactSources()
        {
            return LDA.ExecuteList<ContactSource>("ContactSourcesGetAll", DataAccessHelper.Database.Bsys).Result;
        }

        /// <summary>
        /// Gets a list of the state codes
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Bsys, "StateCodesGetDomestic")]
        public List<string> GetStateCodes()
        {
            return LDA.ExecuteList<string>("StateCodesGetDomestic", DataAccessHelper.Database.Bsys).Result;
        }

        /// <summary>
        /// Checks the loan type to make sure it is in the list of approved loan types
        /// </summary>
        /// <param name="SpecificationType"></param>
        /// <param name="LoanKey"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Bsys, "LoanTypesGetByKey")]
        public bool IsLoanType(string specificLoanType, string loanKey)
        {
            List<string> types = LDA.ExecuteList<string>("LoanTypesGetByKey", DataAccessHelper.Database.Bsys, SqlParams.Single("TypeKey", loanKey)).Result;
            return types.Contains(specificLoanType.ToUpper());
        }

    }
}