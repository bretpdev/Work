using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DataWarehouseLoad
{
    class Program
    {
        public static ProcessLogData LogData { get; set; }
        static int Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;//Default to dev mode this will get set below
            if (args.Length != 2)
            {
                WriteToConsole(string.Format("Expecting 2 command lines args but found {0}", args.Length));
                return 1;
            }

            if (!DataAccessHelper.StandardArgsCheck(args, "DataWarehouseLoad", DataAccessHelper.TestMode))
                return 1;

            if (args[1].ToUpper() == "CORNERSTONE")
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            else if (args[1].ToUpper() == "UHEAA")
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            else
            {
                WriteToConsole(string.Format("Unknown region {0} passed.  The region must be UHEAA or CornerStone", args[1]));
                return 1;
            }

            LogData = ProcessLogger.RegisterApplication(DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ? "UTNWDW1.L" : "UTLWDW1.L", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.TestMode);

            int returnVal = new SasProcessor().StartProcess();
            ProcessLogger.LogEnd(LogData.ProcessLogId);
            return returnVal;
        }

        public static void WriteToConsole(string message)
        {
            Console.WriteLine(message);
            //if (DataAccessHelper.TestMode)
            //    Dialog.Error.Ok(message, "Error");
        }
    }
}
