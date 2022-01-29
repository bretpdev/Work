using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace PEPSFED
{
    public class Program
    {
        public static string ScriptId = "PEPS-FED";
        public static Args ApplicationArgs { get; set; }
        public static ProcessLogRun PLR { get; set; }
        [STAThread]
        static int Main(string[] args)
        {
            int returnVal = 0;
            if (args.Length == 0)
            {
                Console.WriteLine("Missing KVP Args.");
                if (DataAccessHelper.TestMode)
                    Dialog.Error.Ok("Missing KVP Args."); 
                return 1;
            }
            AffiliationData d = new AffiliationData();
            Console.WriteLine(d.ToString());
            var results = KvpArgValidator.ValidateArguments<Args>(args);
            if (results.IsValid)
            {
                ApplicationArgs = new Args(args);
                ApplicationArgs.SetDataAccessValues();

                if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), ApplicationArgs.ShowPrompts))
                    return 1;

                PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.CurrentMode, false, true);

                returnVal = new Peps(ApplicationArgs).Process();
                Console.WriteLine("Return Value: {0}", returnVal);
                DataAccessHelper.CloseAllManagedConnections();
            }
            else
            {
                Console.WriteLine(results.ValidationMesssage); 
                return 1;
            }
            PLR.LogEnd();
            return returnVal;
        }
    }
}
