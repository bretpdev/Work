using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace DASFORBFED
{
    class Program
    {
        static readonly string ScriptId = "DASFORBFED";
        public static string ForbType {get;set;}
        public static int Main(string[] args)
        {
            int runResult = 1; //Default to failure code
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false))
                return runResult;
            var regionArg = args.Skip(1).FirstOrDefault();
            if (regionArg == null)
                Console.WriteLine("Please pass either Uheaa or CornerStone with your arguments.");
            else
            {
                if (args.Skip(2).Any())
                    ForbType = args[2];
                else
                    ForbType = "40";
                DataAccessHelper.CurrentRegion = (DataAccessHelper.Region)Enum.Parse(typeof(DataAccessHelper.Region), regionArg, true);
                var plr = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
                ReflectionInterface ri = null;
                BatchProcessingHelper login = null;
                try
                {
                    ri = new ReflectionInterface();
                    string loginType = "Batch" + DataAccessHelper.CurrentRegion.ToString();
                    login = BatchProcessingLoginHelper.Login(plr, ri, "DASFORBFED", loginType, true);

                    runResult = new DisasterForbearanceAddFed(plr, (m) => Console.WriteLine(m)).ProcessWork(ri);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Processing Work: " + ex.Message);
                    plr.AddNotification("Error Processing Work", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                }
                finally
                {
                    if (ri != null)
                        ri.CloseSession();
                    if (login != null)
                        BatchProcessingHelper.CloseConnection(login);
                    plr.LogEnd();
                }
            }
            return runResult;
        }
    }
}