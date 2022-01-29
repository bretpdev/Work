using System;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SMBALWO
{
    class Program
    {
        public static string ScriptId = "SMBALWO";
        public static Args ApplicationArgs { get; set; }
        private static ProcessLogRun PLR { get; set; }

        [STAThread]
        static int Main(string[] args)
        {
            int returnVal = 0;
            if (args.Length == 0)
            {
                Console.WriteLine("Missing KVP Args.");
                return 1;
            }
            var results = KvpArgValidator.ValidateArguments<Args>(args);
            if (results.IsValid)
            {
                ApplicationArgs = new Args(args);
                ApplicationArgs.SetDataAccessValues();

                if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), ApplicationArgs.ShowPrompts))
                    return 1;

                PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
                returnVal =  new SmallBalanceWriteOff(PLR, ApplicationArgs.LoadData).Process();
                PLR.LogEnd();
                DataAccessHelper.CloseAllManagedConnections();
            }

            return returnVal;
        }
    }
}