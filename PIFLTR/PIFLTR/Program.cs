using System;
using System.Linq;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace PIFLTR
{
    public class Program
    {
        public static ProcessLogRun PLR { get; set; }
        public static string ScriptId = "PIFLTR";
        public const string ConsolLetterId = "PIFCLLTR";
        public const string PifLetterId = "PIFLTR";
        public static Arguments ApplicationArgs { get; set; }
        public const int SUCCESS = 0;
        public const int FAILURE = 1;

        static int Main(string[] args)
        {
            args = args?.Select(p => Security.SanitizeString(p)).ToArray();
            var results = KvpArgValidator.ValidateArguments<Arguments>(args);
            if (results.IsValid)
            {
                ApplicationArgs = new Arguments(args);
                ApplicationArgs.SetDataAccessValues();
                
                if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), ApplicationArgs.ShowPrompts))
                    return FAILURE;

                PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion,
                   DataAccessHelper.CurrentMode, ApplicationArgs.ShowPrompts, false, true);

                bool didRun = new TaskProcessor(PLR).RunProcess();
                PLR.LogEnd();
                DataAccessHelper.CloseAllManagedConnections();

                return didRun ? SUCCESS : FAILURE;
            }
            else //If invalid args provided, write out prompt for invalid/missing args
            {
                Console.WriteLine(results.ValidationMesssage);
                Dialog.Error.Ok(results.ValidationMesssage);
                return FAILURE;
            }
        }
    }
}