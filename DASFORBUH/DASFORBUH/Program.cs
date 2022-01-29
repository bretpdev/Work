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

namespace DASFORBUH
{
    class Program
    {
        static readonly string ScriptId = "DASFORBUH";
        public static string ForbType {get;set;}
        public static int Main(string[] args)
        {
            int runResult = 1; //Default to failure code

            //the first argument is the mode dev/live
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false))
                return runResult;

            //the second argument is the forbearance type and is optional
            if (args.Skip(1).Any())
                ForbType = args[1];
            else
                ForbType = "40";

            //This is a uheaa only script
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            var plr = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = null;
            BatchProcessingHelper login = null;
            try
            {
                ri = new ReflectionInterface();
                string loginType = "Batch" + DataAccessHelper.CurrentRegion.ToString();
                login = BatchProcessingLoginHelper.Login(plr, ri, ScriptId, loginType, true);

                runResult = new DisasterForbearanceAdd(plr, (m) => Console.WriteLine(m)).Process(ri);
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
            return runResult;
        }
    }
}