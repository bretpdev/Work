using System;
using System.Reflection;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace ACS
{
    public class Program
    {
        private static readonly string scriptId = "ACS";
        public static int Main(string[] args)
        {
            int? retVal = 0;
            DateTime startDate;

            if (!DataAccessHelper.StandardArgsCheck(args, "ACS", false))
                return 1;
                        
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            
            ProcessLogRun plr = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true, true);
            DataAccess da = new DataAccess(plr);
            
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            if (args.Length == 2)
            {
                try
                {
                    startDate = Program.ConstructDate(args[1], plr);
                }
                catch(Exception ex)
                {
                    plr.AddNotification(string.Format("Invalid date string as argument: {0}", args[1]), NotificationType.HandledException, NotificationSeverityType.Critical, ex);
                    plr.LogEnd();
                    return 1;
                }
            }
            else
                startDate = DateTime.Now;

            ReflectionInterface RI = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(plr, RI, da.ScriptId, "BatchUheaa");

            if (helper != null)
            {
                Processors.Processor process = new Processors.Processor(RI, da, plr);
                retVal = process.Process(startDate);
            }
            else
            {
                plr.AddNotification("Login failure for ACS job.", NotificationType.ErrorReport, NotificationSeverityType.Critical); 
                DataAccessHelper.CloseAllManagedConnections();
                RI.CloseSession();
                plr.LogEnd();
                return 1;
            }

            helper.Connection.Close();
            DataAccessHelper.CloseAllManagedConnections();
            RI.CloseSession();
            plr.LogEnd();
            if (retVal == 0)
                return 0;
            else
                return 1;

        }

        /// <summary>
        /// ACS date format is YYMMDD or, YYMDD. Build the correct date.
        /// </summary>
        /// <param name="dateStr">NULL for current date, otherwise start date.</param>
        /// <returns></returns>
        private static DateTime ConstructDate(string dateStr, ProcessLogRun plr)
        {

            string newstr = "";
            DateTime returnVal;

            dateStr = dateStr.Replace("/", "").Replace("-", "");
            if (dateStr.Length == 6)
                newstr = dateStr.SafeSubString(0, 1) + "/" + dateStr.SafeSubString(1, 1) + "/" + dateStr.Substring(2);
            else if (dateStr.Length == 8)
                newstr = dateStr.SafeSubString(0, 2) + "/" + dateStr.SafeSubString(2, 2) + "/" + dateStr.Substring(4);
            else
                plr.AddNotification("Incorrect date format provided. Please provide date in either d-m-yyyy or dd-mm-yyyy", NotificationType.ErrorReport, NotificationSeverityType.Critical);

                returnVal = DateTime.Parse(newstr);

            return returnVal;
        }
    }
}
