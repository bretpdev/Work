using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Linq;

using System.IO;
using NAudio.Wave;


namespace FSAMTHCALL
{
    public class Program
    {
        public static string ScriptId = "FSAMTHCALL";
        static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            int executionMode = ValidateCustomArgs(args);

            //Use for testing: Skips sample file reconciliation when in dev mode if argument is set
            bool reconcileOverride = false;
            if(args.Length > 2)
            {
                foreach(object o in args)
                {
                    if(o.ToString().ToUpper() == "OVERRIDESKIPRECONCILE")
                    {
                        reconcileOverride = true;
                    }
                }
            }

            ProcessLogRun plRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            int returnValue = 0;

            switch (executionMode)
            {
                case 1: //full refresh
                    returnValue = new SampleFile(plRun).Reconcile(true);
                    break;
                case 2: //monthly lookback refresh
                    returnValue = new SampleFile(plRun).Reconcile(false);
                    break;
                case 3: //Generate samplefile for AES to pick calls 
                    returnValue = new SampleFile(plRun).CreateExcelFile(reconcileOverride);
                    break;
                case 4: //Process returnfile from AES to get recordings
                    returnValue = new ReturnFile(plRun).ProcessReturnFile();
                    break;
                case 0:
                default:
                    plRun.AddNotification("Invalid command line arguments passed to script. Required arguments: \r\n args[0] VALUES: 'live' 'dev' \r\n args[1] VALUES: 'reconcile' 'samplefile' 'returnfile' \r\n Optional arguements: args[2] VALUES: 'full'", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    break;
            }

            plRun.LogEnd();
            if (executionMode == 0) //Invalid args were passed
                return 1;

            Console.WriteLine(returnValue);
            Console.WriteLine("Closing Database Connections");
            return returnValue;
        }

        /// <summary>
        /// Handles all args passed in by command line and sets an integer for run mode.
        /// </summary>
        /// <param name="args">args[0] (live,dev) args[1] (reconcile,samplefile,returnfile)</param>
        public static int ValidateCustomArgs(string[] args)
        {
            //reconcile should run every day and reconcile any unreconciled calls for the week
            //samplefile will generate a sample file of records that AES can look at
            //returnfile will get the recordings for the calls requested by AES
            int returnValue = -1;

            if (args.Length < 2)
            {
                Console.WriteLine("Missing required arguments.  Required arguements: \r\n args[0] VALUES: 'live' 'dev' \r\n args[1] VALUES: 'reconcile' 'samplefile' 'returnfile' \r\n Optional arguements: args[2] VALUES: 'full'");
                return returnValue;
            }

            bool showPopups = args[1].ToUpper() == "RETURNFILE";
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, showPopups) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), showPopups)) //Defaults the manual run to pop up error messages
                return returnValue;
            bool fullRefresh = args.Skip(2).SingleOrDefault()?.ToUpper() == "FULL";
            string invalidArgMessage = "Full refresh parameter was passed in but will be ignored in this execution mode.";
            switch (args[1].ToUpper())
            {
                case "RECONCILE":
                    returnValue = fullRefresh ? 1 : 2;
                    break;
                case "SAMPLEFILE":
                    returnValue = 3;
                    if (fullRefresh)
                        Console.WriteLine(invalidArgMessage);
                    break;
                case "RETURNFILE":
                    returnValue = 4;
                    if (fullRefresh)
                        Console.WriteLine(invalidArgMessage);
                    break;
                default:
                    returnValue = -1;
                    break;
            }

            return returnValue;
        }

       


          
        

      
    }
}
