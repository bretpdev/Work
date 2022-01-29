using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR
{
    static class Program
    {
        const string SCRIPTID = "ACURINTR";
        const int THREADCOUNT = 4;
        public static int Main(string[] args)
        {

            if (!DataAccessHelper.StandardArgsCheck(args, SCRIPTID, false))
                return 1;
            int retVal = 0; //no errors

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            var plData = ProcessLogger.RegisterApplication(SCRIPTID, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
            //Get the list of queues and their processing data. <<Step 1>>
            var workStack = new ConcurrentStack<QueueData>(DataAccess.GetQueues());
            var inUseIds = new ConcurrentQueue<string>();
            Parallel.For(0, THREADCOUNT, i =>
            {
                Thread.Sleep(5000 * i); //launch threads every 5 seconds to allow sessions to catch a breath
                bool tryLoggingIn = true;
                var processor = new AutomateDemographics(SCRIPTID, plData);
                BatchProcessingHelper sessionLoginInfo = null;
                while (tryLoggingIn && workStack.Any())
                {
                    sessionLoginInfo = BatchProcessingHelper.GetNextAvailableId(SCRIPTID, "BatchUheaa");
                    if (inUseIds.Contains(sessionLoginInfo.UserName))
                        continue; //don't log in twice
                    if (!processor.Login(sessionLoginInfo, DataAccessHelper.Region.Uheaa))
                    {
                        ProcessLogger.AddNotification(plData.ProcessLogId, string.Format("Login to Uheaa Failed using ID: {0} Closing Session.", sessionLoginInfo.UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        processor.Close();
                    }
                    else
                    {
                        tryLoggingIn = false;
                        inUseIds.Enqueue(sessionLoginInfo.UserName);
                    }
                }

                bool keepProcessing = true;
                while (keepProcessing && workStack.Any())
                {
                    QueueData queue = null;
                    while (workStack.Any() && !workStack.TryPop(out queue)) ;

                    var result = processor.Process(queue);
                    if (!result)
                        retVal = 1; //error
                }
                if (sessionLoginInfo != null)
                    BatchProcessingHelper.CloseConnection(sessionLoginInfo);
                processor.Close();
            });

            ProcessLogger.LogEnd(plData.ProcessLogId);
            return retVal;
        }
    }
}
