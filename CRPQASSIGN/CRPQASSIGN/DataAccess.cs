using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CRPQASSIGN
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        public DataAccess(int plId)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, plId, false, true);
        }

        public List<Queues> GetAllQueues()
        {
            return LDA.ExecuteList<Queues>("[crpqassign].GetQueues", DataAccessHelper.Database.Pls).CheckResult();
        }

        public List<Users> GetAllUsers()
        {
            return LDA.ExecuteList<Users>("[crpqassign].GetAllUsers", DataAccessHelper.Database.Pls).CheckResult();
        }
    }
}
