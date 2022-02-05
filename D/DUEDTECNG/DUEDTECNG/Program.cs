using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DUEDTECNG
{
    class Program
    {
        const int FAILURE = 1;
        const int SUCCESS = 0;
        private static ProcessLogRun PLR { get; set; }
        private static DataAccess DA { get; set; }
        public static string ScriptId = "DUEDTECNG";
        public static List<Thread> Threads { get; set; }
        static int Main(string[] args)
        {
            var argResults = KvpArgValidator.ValidateArguments<Args>(args);
            if (!argResults.IsValid)
            {
                Console.WriteLine(argResults.ValidationMesssage);
                return FAILURE;
            }

            var parsedArgs = new Args(args);
            DataAccessHelper.CurrentMode = parsedArgs.Mode;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
                return FAILURE;


            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            var LDA = new LogDataAccess(DataAccessHelper.CurrentMode, PLR.ProcessLogId, false, true);
            DA = new DataAccess(LDA);

            var result = Run(parsedArgs);
            DataAccessHelper.CloseAllManagedConnections();
            PLR.LogEnd();
            return result;
        }

        public static int Run(Args args)
        {
            if (args.SkipWorkAdd)
                Console.WriteLine("Skipping Work-Add Step.");
            else
            {
                Console.WriteLine("Adding new work...");
                int recordCount = 0;
                int workCount = 0;
                int? remainingWorkLimit = null;
                if (args.WorkAddLimit.HasValue)
                {
                    remainingWorkLimit = args.WorkAddLimit;
                    Console.WriteLine("Limiting Work Add to {0} records.", args.WorkAddLimit);
                }
                foreach (var target in args.TargetDueDates)
                {
                    workCount++;
                    var results = DA.AddNewWork(target, args.NewDueDate, remainingWorkLimit);
                    Console.WriteLine("Added work {0}/{1}", workCount, args.TargetDueDates.Count);
                    recordCount += results;
                    if (remainingWorkLimit.HasValue)
                        remainingWorkLimit -= results;
                    if (remainingWorkLimit == 0)
                    {
                        Console.WriteLine("Work Add Limit reached, skipping remaining steps.");
                        break;
                    }
                }
                Console.WriteLine(recordCount + " new record(s) added.");
            }
            if (args.SkipWorkProcess)
                Console.WriteLine("Skipping Work-Process Step");
            else
            {
                if (DataAccessHelper.TestMode && args.AccountIdentifiers.Any())
                    foreach (var accountIdentifier in args.AccountIdentifiers)
                        DA.MarkAccountUnprocessed(accountIdentifier);

                Console.WriteLine("Pulling available work...");
                var totalWork = DA.GetAvailableWork();
                if (args.AccountIdentifiers.Any())
                {
                    Console.WriteLine("Limiting to account identifiers " + string.Join(",", args.AccountIdentifiers));
                    totalWork = totalWork.Where(o => o.AccountNumber.IsIn(args.AccountIdentifiers.ToArray()) || o.Ssn.IsIn(args.AccountIdentifiers.ToArray())).ToList();
                }
                Console.WriteLine("Found {0} record(s) to process.", totalWork.Count);


                string loginType = "BatchCornerstone";
                AppSettings settings = DA.GetAppSettings();
                var worker = new ThreadedBatchReflectionWorker<BorrowerData>(ScriptId, loginType, PLR, totalWork, args.ThreadCount, (bData, ri) =>
                {
                    try
                    {
                        var processor = new RediscloseSchedule(ri, PLR, DA, settings, bData);
                        var successful = processor.RediscloseLoans();
                        DA.MarkDueDateAsProcessed(bData.DueDateChangeId, !successful);
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("Unexpected error while processing Account {0}, DueDateChangeId {1}.", bData.AccountNumber, bData.DueDateChangeId);
                        PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                    }
                });
                worker.DoWork();

                if (!worker.SuccessfullyLoggedIn)
                {   //the worker is finished but there is still work, so we probably couldn't find a login.
                    string message = string.Format("{0} had trouble logging in with batch ID {1}", ScriptId, loginType);
                    EmailHelper.SendMail(DataAccessHelper.TestMode, "sshelp@utahsbr.edu", Environment.UserName + "@utahsbr.edu", "Batch Login Issue", message, "", "", EmailHelper.EmailImportance.High, true);
                    return FAILURE;
                }
            }
            return SUCCESS;
        }
    }
}
