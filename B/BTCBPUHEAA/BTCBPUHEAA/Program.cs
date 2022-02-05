using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace BTCBPUHEAA
{
    class Program
    {
        private const int FAILURE = 1;
        private const int SUCCESS = 0;
        private const string ScriptId = "BTCBPUHEAA";
        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return FAILURE;
            var PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, false);
            try
            {
                var btcp = new BatchCheckByPhone(PLR);
                if (!btcp.ValidateDrives())
                    return FAILURE; //Check that drives exist.  If any do not, end the script

                if (!btcp.ProcessCheckByPhone())
                    return FAILURE;
                return SUCCESS;
            }
            finally
            {
                PLR.LogEnd();
            }
        }

    }
}
