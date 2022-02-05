using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Windows.Forms;
using ACURINTR.DemographicsProcessors;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACURINTR
{
    public class AutomateDemographics
    {
        private ReflectionInterface RI { get; set; }
        private string ScriptId { get; set; }
        private string UserId { get; set; }
        private RecoveryLog Recovery { get; set; }
        //private ProcessLogData LogData { get; set; }
        private BatchProcessingHelper Batch { get; set; }
        public ProcessLogRun PLR { get; set; }

        public static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "ACURINTR", false))
                return 1;

            string overrideQueue = args.Skip(1).SingleOrDefault();

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            try
            {
                return new AutomateDemographics().Process(overrideQueue);
            }
            catch (EarlyTerminationException)
            {
                return 1;
            }
        }

        private int Process(string overrideQueue = null)
        {
            int result = 0;
            ScriptId = "ACURINTR";
            Recovery = new RecoveryLog("AcurintRrecoveryFile");
            PLR = new ProcessLogRun("ACURINTR", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true);
            Assembly assembly = Assembly.GetExecutingAssembly();
            RI = new ReflectionInterface();
            BatchProcessingHelper sessionLoginInfo = BatchProcessingLoginHelper.Login(PLR, RI, "ACURINTR", "BatchUheaa");
            UserId = sessionLoginInfo.UserName;
            if (sessionLoginInfo == null)
            {
                PLR.AddNotification($"Login to Uheaa Failed using ID: {sessionLoginInfo.UserName} Closing Session.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                RI.CloseSession();
                return 1;
            }

            bool pauseOnQueueClosingError = false;

            //Grab some variables that we only need to set once.
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            //Get the list of queues and their processing data. <<Step 1>>
            List<QueueData> allQueues = new DataAccess(PLR).GetQueues();
            if (overrideQueue != null)
                allQueues = allQueues.Where(o => o.Queue.ToLower() == overrideQueue.ToLower()).ToList();

            //See if we're in recovery.
            if (Recovery.RecoveryValue.IsPopulated())
            {
                //Get the QueueData item that matches the recovery value and finish processing it.
                QueueData recoveryQueue = allQueues.Where(p => p.Queue == Recovery.RecoveryValue.Split(',')[0]).SingleOrDefault();
                if (recoveryQueue != null)
                {
                    ProcessQueue(recoveryQueue, UserId, assemblyName, pauseOnQueueClosingError);
                    //Remove the recovery queue from the list of queues so we don't try to process it again.
                    allQueues.Remove(recoveryQueue);
                }
                Recovery.Delete();
            }

            //Loop through each queue in the database. <<Step 2>>
            foreach (QueueData queueData in allQueues.OrderBy(x => x.Queue))
            {
                var processingResult = ProcessQueue(queueData, UserId, assemblyName, pauseOnQueueClosingError);
                result = processingResult == 1 ? processingResult : result; //set the result to 1 if any fail
            }

            PLR.LogEnd();
            Recovery.Delete();
            RI.CloseSession();
            return result;
        }

        /// <summary>
        /// Start processorBase for the queue that we pass in.
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="userId"></param>
        /// <param name="assemblyName"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        private int ProcessQueue(QueueData queueData, string userId, string assemblyName, bool pauseOnQueueClosingError)
        {
            try
            {
                //Instantiate the class specified by the queue data and process the queue.
                string fullyQualifiedProcessor = string.Format("{0}.DemographicsProcessors.{1}", assemblyName, queueData.Processor);
                ProcessorBase queueProcessor = (ProcessorBase)Activator.CreateInstance(assemblyName, fullyQualifiedProcessor, false, BindingFlags.Default, null, new Object[] { RI, userId, ScriptId, Recovery, PLR }, null, new Object[] { }).Unwrap();
                queueProcessor.Process(queueData, assemblyName, pauseOnQueueClosingError);
                return 0;
            }
            catch (Exception ex)
            {
                string message = $"{RI.UserId}: There was an error processing {queueData.System} queue: {queueData.Queue}";
                PLR.AddNotification(assemblyName + " " + message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return 1;
            }
        }


    }
}
