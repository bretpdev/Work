using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace DEFMNTPPC
{
    class Program
    {
        public static string ScriptId = "DEFMNTPPC";
        public static ProcessLogRun PLR { get; set; }

        static int Main(string[] args)
        {
            int returnVal = 0;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if(!DataAccessHelper.StandardArgsCheck(args, ScriptId) && !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return 0;

            PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = new ReflectionInterface();
            
            var login = BatchProcessingLoginHelper.Login(PLR, ri, ScriptId, "BatchUheaa");
            if (login != null)
            {
                DefermentPPC def = new DefermentPPC(ri, PLR, login.UserName);
                returnVal = def.Process();
            }
            else
                returnVal = 1;

            PLR.LogEnd();

            return returnVal;
        }
    }
}
