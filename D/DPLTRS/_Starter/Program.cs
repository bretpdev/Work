using DPLTRS.NotificationOfSatisfaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        public static string ScriptId = "DPLTRS";

        static void Main(string[] args)
        {
            //StartRehabilitation(args);
            //StartPaidThroughConsolidation(args);
            //StartPayoff(args);
            //StartBreakdown(args);
            //StartCurrent(args);
            //StartNotification(args);
            Start15FormulaInfo(args);
        }

        public static void StartRehabilitation(string[] args)
        {
            //Check that first arg is DEV or LIVE
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
            {
                return;
            }

            //Check that the user running the script has access to execute the SQL
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                return;
            }

            //The second argument should be the region as UHEAA or CORNERSTONE
            if (args.Length < 2 || (args[1].ToUpper() != "UHEAA" && args[1].ToUpper() != "CORNERSTONE"))
            {
                return;
            }

            //Set Current Region
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;//= args[1] == "UHEAA" ? DataAccessHelper.Region.Uheaa : DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = null;
            //Create Session In Test
#if DEBUG
            ri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? "BatchUheaa" : "BatchCornerstone");
#endif
            new DPLTRS.Rehabilitation.LoanManagementLetters_Rehabilitation(ri).Main();

            logRun.LogEnd();
        }

        public static void StartPaidThroughConsolidation(string[] args)
        {
            //Check that first arg is DEV or LIVE
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
            {
                return;
            }

            //Check that the user running the script has access to execute the SQL
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                return;
            }

            //The second argument should be the region as UHEAA or CORNERSTONE
            if (args.Length < 2 || (args[1].ToUpper() != "UHEAA" && args[1].ToUpper() != "CORNERSTONE"))
            {
                return;
            }

            //Set Current Region
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;//= args[1] == "UHEAA" ? DataAccessHelper.Region.Uheaa : DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = null;
            //Create Session In Test
#if DEBUG
            ri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? "BatchUheaa" : "BatchCornerstone");
#endif
            new DPLTRS.PaidThroughConsolidation.LoanManagementLetters_PaidThroughConsolidation(ri).Main();

            logRun.LogEnd();
        }

        public static void StartPayoff(string[] args)
        {
            //Check that first arg is DEV or LIVE
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
            {
                return;
            }

            //Check that the user running the script has access to execute the SQL
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                return;
            }

            //The second argument should be the region as UHEAA or CORNERSTONE
            if (args.Length < 2 || (args[1].ToUpper() != "UHEAA" && args[1].ToUpper() != "CORNERSTONE"))
            {
                return;
            }

            //Set Current Region
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;//= args[1] == "UHEAA" ? DataAccessHelper.Region.Uheaa : DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = null;
            //Create Session In Test
#if DEBUG
            ri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? "BatchUheaa" : "BatchCornerstone");
#endif
            new DPLTRS.Payoff.Payoff(ri).Main();

            logRun.LogEnd();
        }

        public static void StartBreakdown(string[] args)
        {
            //Check that first arg is DEV or LIVE
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
            {
                return;
            }

            //Check that the user running the script has access to execute the SQL
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                return;
            }

            //The second argument should be the region as UHEAA or CORNERSTONE
            if (args.Length < 2 || (args[1].ToUpper() != "UHEAA" && args[1].ToUpper() != "CORNERSTONE"))
            {
                return;
            }

            //Set Current Region
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;//= args[1] == "UHEAA" ? DataAccessHelper.Region.Uheaa : DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = null;
            //Create Session In Test
#if DEBUG
            ri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? "BatchUheaa" : "BatchCornerstone");
#endif
            new DPLTRS.Breakdown.Breakdown(ri).Main();

            logRun.LogEnd();
        }

        public static void StartCurrent(string[] args)
        {
            //Check that first arg is DEV or LIVE
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
            {
                return;
            }

            //Check that the user running the script has access to execute the SQL
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                return;
            }

            //The second argument should be the region as UHEAA or CORNERSTONE
            if (args.Length < 2 || (args[1].ToUpper() != "UHEAA" && args[1].ToUpper() != "CORNERSTONE"))
            {
                return;
            }

            //Set Current Region
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;//= args[1] == "UHEAA" ? DataAccessHelper.Region.Uheaa : DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = null;
            //Create Session In Test
#if DEBUG
            ri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? "BatchUheaa" : "BatchCornerstone");
#endif
            new DPLTRS.Current.Current(ri).Main();

            logRun.LogEnd();
        }

        public static void StartNotification(string[] args)
        {
            //Check that first arg is DEV or LIVE
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
            {
                return;
            }

            //Check that the user running the script has access to execute the SQL
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                return;
            }

            //The second argument should be the region as UHEAA or CORNERSTONE
            if (args.Length < 2 || (args[1].ToUpper() != "UHEAA" && args[1].ToUpper() != "CORNERSTONE"))
            {
                return;
            }

            //Set Current Region
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;//= args[1] == "UHEAA" ? DataAccessHelper.Region.Uheaa : DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = null;
            //Create Session In Test
#if DEBUG
            ri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? "BatchUheaa" : "BatchCornerstone");
#endif
            new LoanManagementLetters_NotificationOfSatisfaction(ri).Main();

            logRun.LogEnd();
        }

        public static void Start15FormulaInfo(string[] args)
        {
            //Check that first arg is DEV or LIVE
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
            {
                return;
            }

            //Check that the user running the script has access to execute the SQL
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                return;
            }

            //The second argument should be the region as UHEAA or CORNERSTONE
            if (args.Length < 2 || (args[1].ToUpper() != "UHEAA" && args[1].ToUpper() != "CORNERSTONE"))
            {
                return;
            }

            //Set Current Region
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;//= args[1] == "UHEAA" ? DataAccessHelper.Region.Uheaa : DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = null;
            //Create Session In Test
#if DEBUG
            ri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? "BatchUheaa" : "BatchCornerstone");
#endif
            new DPLTRS._15FormulaInformation.LoanManagementLetters_15FormulaInformation(ri).Main();

            logRun.LogEnd();
        }
    }
}
