using System.Collections.Generic;
using Uheaa.Common.DataAccess;

namespace WORKRAQUE
{
    public static class DataAccess
    {
        /// <summary>
        /// Gets all the FFEL loan types, leaving out the private types
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Bsys, "LoanTypesGetByKey")]
        public static List<string> GetLoanTypes()
        {
            return DataAccessHelper.ExecuteList<string>("LoanTypesGetByKey", DataAccessHelper.Database.Bsys,
                SqlParams.Single("TypeKey", "FFEL"));
        }
    }
}