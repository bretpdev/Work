using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace BTCHPWUPD
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        private LogDataAccess LDADev { get; set; }
        public DataAccess(int processlogId)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, processlogId, false, true);
            LDADev = new LogDataAccess(DataAccessHelper.Mode.Dev, processlogId, false, true);
        }
        
        /// <summary>
        /// Gets all user ids that are designates as batch ids
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "GetAllBatchUsers")]
        public List<UserIdData> GetUserIds()
        {
            return LDA.ExecuteList<UserIdData>("GetAllBatchUsers", DataAccessHelper.Database.BatchProcessing).CheckResult();
        }

        /// <summary>
        /// Updates a user name with a new password.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "UpdateBatchPassword")]
        public void UpdatePassword(string userId, string password)
        {
            LDA.Execute("UpdateBatchPassword", DataAccessHelper.Database.BatchProcessing, SqlParams.Single("UserName", userId), SqlParams.Single("Password", password));
            if(!DataAccessHelper.TestMode)//Update test when running in Live
            {
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
                LDADev.Execute("UpdateBatchPassword", DataAccessHelper.Database.BatchProcessing, SqlParams.Single("UserName", userId), SqlParams.Single("Password", password));
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
            }
        }

        /// <summary>
        /// Adds a new base word and format.  This will inactivate the old format.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "AddNewBaseWordAndFormat")]
        public void UpdateBaseWordAndFormat(string baseword, string format)
        {
            LDA.Execute("AddNewBaseWordAndFormat", DataAccessHelper.Database.BatchProcessing, SqlParams.Single("BaseWord", baseword), SqlParams.Single("Format", format));
        }

        /// <summary>
        /// Gets the baseword and format.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "GetBaseWordAndFormat")]
        public BaseWordData GetBaseWordAndFormat()
        {
            return LDA.ExecuteSingle<BaseWordData>("GetBaseWordAndFormat", DataAccessHelper.Database.BatchProcessing).CheckResult();
        }
    }
}
