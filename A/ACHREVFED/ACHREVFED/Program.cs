using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHREVFED
{
    class Program
    {
        const string ScriptId = "ACHREVFED";
        public static int Main(string[] args)
        {
            var argResults = KvpArgValidator.ValidateArguments<Args>(args);
            if (!argResults.IsValid)
            {
                Console.WriteLine(argResults.ValidationMesssage);
#if DEBUG
                Console.ReadKey();
#endif
            }
            else
            {
                var parsedArgs = new Args(args);
                DataAccessHelper.CurrentMode = parsedArgs.Mode;
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
                if (DatabaseAccessHelper.StandardSprocAccessCheck(Assembly.GetExecutingAssembly()))
                {
                    var plr = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.CurrentMode, false, true);
                    var lda = new LogDataAccess(DataAccessHelper.CurrentMode, plr.ProcessLogId, false, true);
                    var da = new DataAccess(lda);
                    var ri = new ReflectionInterface();
                    var login = BatchProcessingLoginHelper.Login(plr, ri, ScriptId, "BatchCornerstone");

                    new AchReviewFed(ScriptId, plr, ri, da).Process();

                    plr.LogEnd();
                    BatchProcessingHelper.CloseConnection(login);
                }
            }
            return 0;
        }
    }
}
