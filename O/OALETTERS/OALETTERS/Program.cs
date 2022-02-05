using System;
using System.Reflection;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace OALETTERS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Count() > 0 && args[0].IsNumeric())
                if (args[0].ToInt() > 0) // ACDC sends in a number which needs to be converted to a string
                    args[0] = "dev";
                else
                    args[0] = "live";

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            string scriptId = "OALETTERS";
            if (!DataAccessHelper.StandardArgsCheck(args, scriptId) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return;

            ProcessLogData logData = ProcessLogger.RegisterApplication(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());

            new ProcessLetter(logData, scriptId).Run();

            ProcessLogger.LogEnd(logData.ProcessLogId);
        }
    }
}