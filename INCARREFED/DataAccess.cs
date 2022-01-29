using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Uheaa.Common.DataAccess;

namespace INCARREFED
{
    class DataAccess
    {
        public static List<string> GetStateCodes()
        {
            return DataAccessHelper.GetContext(DataAccessHelper.Database.Csys).ExecuteQuery<string>("EXEC spGENR_GetStateCodes").ToList();
        }

        public static bool AddFutureDatedTask(CommentData cData)
        {
            string comment = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10:mm/dd/yyyy},{11},{12}", cData.IsBorrower ? "BORROWER" : "ENDORSER/STUDENT", cData.StudentAccountNumber,
                cData.BorrowerName, cData.FacilityName, cData.FacilityAddress, cData.FacilityCity, cData.FacilityState, cData.FacilityZip, cData.FacilityPhone, cData.InmateNumber, cData.ReleaseDate,
                cData.Source, cData.OtherInfo);

            try
            {
                DataAccessHelper.GetContext(DataAccessHelper.Database.Cls).ExecuteCommand("EXEC spArcAddAddRecord {0}, {1}, KJAIL, {2}, {3}, {4}, {5}", cData.BorrowerAccountNumber,cData.IsBorrower ? cData.BorrowerAccountNumber : cData.StudentAccountNumber, cData.FollowUpDate, comment, DateTime.Now, Environment.UserName);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
