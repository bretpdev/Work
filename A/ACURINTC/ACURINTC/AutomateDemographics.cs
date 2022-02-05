using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ACURINTC
{
    public class AutomateDemographics
    {
        private string ScriptId { get; set; }
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        const string SKIPWORKADD = "skipworkadd";
        const string SKIPTASKCLOSE = "skiptaskclose";
        const string SKIPPROCESSING = "skipprocessing";
        public static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "ACURINTC", false))
                return 1;

            List<string> arguments = new List<string>(args.Skip(1).Select(o => o.ToLower()));
            bool skipWorkAdd = false;
            if (arguments.Contains(SKIPWORKADD))
            {
                skipWorkAdd = true;
                arguments.Remove(SKIPWORKADD);
            }
            bool skipTaskClose = false;
            if (arguments.Contains(SKIPTASKCLOSE))
            {
                skipTaskClose = true;
                arguments.Remove(SKIPTASKCLOSE);
            }
            bool skipProcessing = false;
            if(arguments.Contains(SKIPPROCESSING))
            {
                skipProcessing = true;
                arguments.Remove(SKIPPROCESSING);
            }
            string overrideAccount = arguments.FirstOrDefault(o => o.StartsWith("accountnumber:"));
            if (overrideAccount != null)
            {
                arguments.Remove(overrideAccount);
                overrideAccount = overrideAccount.Split(':')[1];
            }
            string overrideQueue = arguments.SingleOrDefault();

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
                return new AutomateDemographics().Process(skipWorkAdd, skipTaskClose, skipProcessing, overrideQueue, overrideAccount);
        }

        private int Process(bool skipWorkAdd, bool skipTaskClose, bool skipProcessing, string overrideQueue = null, string overrideAccount = null)
        {
            int returnValue = 0;
            ScriptId = "ACURINTC";
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, false);
            Console.WriteLine("Working with Process Log Id #" + LogRun.ProcessLogId);
            DA = new DataAccess(LogRun);

            List<QueueInfo> allQueues = DA.GetQueues();
            Console.WriteLine("Found {0} queues to process.", allQueues.Count);
            if (overrideQueue != null)
            {
                Console.WriteLine("Limiting queues to single override queue {0} from args.", overrideQueue);
                allQueues = allQueues.Where(o => o.Queue.ToLower() == overrideQueue.ToLower()).ToList();
            }
            if (skipWorkAdd)
            {
                Console.WriteLine("Skipping Work Add.");
            }
            else
            {
                foreach (var queue in allQueues)
                {
                    switch (queue.ParserId)
                    {
                        case Parser.CompassComma:
                            DA.LoadCompassCommaData(queue.Queue, queue.SubQueue);
                            break;
                        case Parser.CompassPdem:
                            DA.LoadCompassPdemData(queue.Queue, queue.SubQueue);
                            break;
                        default:
                            LogRun.AddNotification("Parser Not Supported: Unable to load work for QueueInfoId " + queue.QueueInfoId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                            continue;
                    }
                    Console.WriteLine("Finished loading new work for " + queue.Queue + ";" + queue.SubQueue);
                }
            }
            List<PendingDemos> compassData = DA.GetCompassQueueData().ToList();
            Console.WriteLine("Found " + compassData.Count + " record(s) to process");
            if (overrideAccount != null)
            {
                compassData = compassData.Where(o => o.AccountNumber == overrideAccount).ToList();
                Console.WriteLine("Limiting accounts to #{0}.  {1} accounts found.", overrideAccount, compassData.Count);
            }
            if(!skipProcessing)
            {
                using (var rm = new ReflectionManager(ScriptId, LogRun))
                {
                    Parallel.ForEach(allQueues, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, data =>
                    {
                    //if (DataAccessHelper.TestMode)
                    //{
                    //    var q = data.Queue + ";" + data.SubQueue;
                    //    if (!compassData.Any(o => o.Queue == data.Queue))
                    //    {
                    //        Console.WriteLine("No results for queue {0}, skipping automatically.", q);
                    //        return;
                    //    }
                    //    Console.WriteLine("Getting confirmation to process " + q);
                    //    if (!Dialog.Info.YesNo(string.Format("{0};{1} - Would you like to process?", data.Queue, data.SubQueue)))
                    //    {
                    //        Console.WriteLine(q + " skipped.");
                    //        return;
                    //    }
                    //}
                    var ri = rm.GetAvailableReflectionSession();
                        if (ri == null)
                            return;  //none available, a very bad sign
                    try
                        {
                            QueueProcessor qp = new QueueProcessor(LogRun, ScriptId, DA, ri.Session, ri.Login.UserName);
                            var records = compassData.Where(o => o.Queue == data.Queue).ToList();
                            Console.WriteLine("Beginning Queue {0} using {2}, {1} records to process.", data.Queue, records.Count, ri.Login.UserName);
                            qp.Process(data, records, skipTaskClose);
                            Console.WriteLine("Finished Queue {0}", data.Queue);
                        }
                        finally
                        {
                            rm.ReleaseReflectionSession(ri);
                        }
                    });
                }
            }
            else
            {
                Console.WriteLine("Skipping Processing.");
            }


            ProcessLogger.LogEnd(LogRun.ProcessLogId);
            //if (DataAccessHelper.TestMode)
            //{
            //    Console.WriteLine("Press any key to exit.");
            //    Console.ReadKey();
            //}
            return returnValue;
        }
    }
}