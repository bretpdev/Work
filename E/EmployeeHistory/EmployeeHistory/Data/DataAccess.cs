using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace EmployeeHistory
{
    class DataAccess
    {
        private LogDataAccess LDA;
        public DataAccess(LogDataAccess lda)
        {
            this.LDA = lda;
        }
        [UsesSproc(DB.EmployeeHistory, "AvatierHistoryInsert")]
        public void AvatierHistoryInsert(AvatierHistory history)
        {
            LDA.Execute("AvatierHistoryInsert", DB.EmployeeHistory,
                Sp("EmployeeId", history.EmployeeId),
                Sp("UserGuid", history.UserGuid),
                Sp("Role", history.Role),
                Sp("Title", history.Title),
                Sp("Department", history.Department),
                Sp("ManagerEmployeeId", history.ManagerEmployeeId),
                Sp("FirstName", history.FirstName),
                Sp("MiddleName", history.MiddleName),
                Sp("LastName", history.LastName),
                Sp("HireDate", history.HireDate),
                Sp("TerminationDate", history.TerminationDate),
                Sp("UpdateTypeId", (int)history.UpdateTypeId)
            );
        }

        [UsesSproc(DB.EmployeeHistory, "AvatierHistoryGetMostRecent")]
        public AvatierHistory AvatierHistoryGetMostRecent(string userGuid)
        {
            return LDA.ExecuteList<AvatierHistory>("AvatierHistoryGetMostRecent", DB.EmployeeHistory, Sp("UserGuid", userGuid)).Result.SingleOrDefault();
        }

        public string GetEncryptedCredential(int credentialId)
        {
            var encryptedCredential = DataAccessHelper.ExecuteList<string>("GetEncryptedCredential", DB.EmployeeHistory, Sp("EncryptedCredentialId", credentialId));
            if (encryptedCredential.Count != 1)
                return null;
            return new EncryptionHelper().Decrypt(encryptedCredential[0]);
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}