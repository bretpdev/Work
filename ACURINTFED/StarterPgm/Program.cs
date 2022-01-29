using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace StarterPgm
{
    class Program
    {
        public static void Main(string[] args)
        {
            
            
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            ProcessLogRun logRun = new ProcessLogRun(ACURINTFED.DataAccess.SCRIPTID, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true, false);
            ReflectionInterface RI = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, RI, ACURINTFED.DataAccess.SCRIPTID, "BatchUheaa");

            new ACURINTFED.Accurint(RI).Main();
            RI.CloseSession();
        }
    }
}
