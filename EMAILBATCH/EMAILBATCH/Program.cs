using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace EMAILBATCH
{
    class Program
    {
        public static ProcessLogRun PLR { get; set; }
        public static DataAccess DA { get; set; }
        public static readonly string ScriptId = "EMAILBATCH";
        public static int Main(string[] args)
        {
            int result = 1;
            if (DataAccessHelper.StandardArgsCheck(args, "EMAILBATCH"))
            {
                if(args.Length != 2)
                {
                    Console.WriteLine("Invalid Number of Args.");
                    return result;
                }
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
                PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);
                DA = new DataAccess(PLR.ProcessLogId);
                if (args[1].ToUpper() == "EMAIL")
                    result =  new EMAILBATCH().Process();
                else if (args[1].ToUpper() == "FILELOAD")
                   result = new FileLoader().LoadFiles();
                else
                    Console.WriteLine("Invalid Arg {0} Passed.  The Valid Args are EMAIL or FILELOAD", args[1]);
                PLR.LogEnd();
                DataAccessHelper.CloseAllManagedConnections();
                return result;
            }

            Console.WriteLine("Return Value:{0}", result);
            return result;
        }
    }
}
