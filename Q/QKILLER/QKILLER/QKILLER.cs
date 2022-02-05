using System;
using System.Collections.Generic;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace QKILLER
{
    public class QKILLER
    {
        public ProcessLogRun LogRun { get; set; }
        public string ScriptId { get { return "QKILLER"; } }
        static readonly object locker = new object();
        static readonly object lockerPL = new object();

        /// <summary>
        /// Main entry method
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
		public static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "QKILLER", false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            new QKILLER().Run();

            return 0;
        }

        /// <summary>
        /// Starts process logger and spawns up threads for each queue.
        /// </summary>
        private void Run()
        {
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            //get list of queues to process
            List<WorkQueue> QueuesToProcess = DataAccess.GetQueues();

            foreach (WorkQueue queue in QueuesToProcess)
            {
                ReflectionInterface ri = new ReflectionInterface();
                BatchProcessingHelper batch = BatchProcessingLoginHelper.Login(LogRun, ri, ScriptId, "BatchUheaa");
                if (batch != null)
                    KillQueue(queue, batch, ri);
            }
            ProcessLogger.LogEnd(LogRun.ProcessLogId);
        }


        /// <summary>
        /// Hits LP8YC and marks the queue to be deleted.
        /// </summary>
        /// <param name="queue">queue to process</param>
        /// <returns>true if successful</returns>
        private void KillQueue(WorkQueue queue, BatchProcessingHelper batch, ReflectionInterface ri)
        {
            ri.FastPath("LP8YC" + queue.Department + ";" + queue.Queue);
            if (ri.CheckForText(22, 3, "47432 INVALID COMBINATION ENTERED"))
            {
                CloseConnectionAndSession(batch, ri);
                return;
            }
            else if (ri.CheckForText(1, 61, "QUEUE TASK SELECTION"))
            {
                ri.PutText(21, 13, "01", Key.Enter, true);
                for (int row = 7; !ri.CheckForText(22, 3, "46004 NO MORE DATA TO DISPLAY"); row++)
                {
                    if (ri.GetText(row, 2, 9) == "") //blank row encountered
                        ri.Hit(Key.F8);
                    else if (!ri.CheckForText(row, 22, DateTime.Now.ToString("yyyyMMdd")))
                    {
                        if (ri.CheckForText(row, 33, "A"))
                        {
                            ri.PutText(row, 33, "X", Key.F6, true);
                            row--;
                        }
                        else if (ri.CheckForText(row, 33, "W"))
                        {
                            ri.PutText(row, 33, "A", Key.F6, true);
                            row--;
                        }
                        else
                        {
                            if (row == 20)
                            {
                                ri.Hit(Key.F8);
                                row = 6;
                            }
                            continue; //skip the successful adding check for X
                        }

                        if (!ri.CheckForText(22, 3, "49000 DATA SUCCESSFULLY UPDATED"))
                        {
                            lock (lockerPL)
                            {
                                string message = string.Format("{6}; Failed to cancel out the queue task {0} for department: {1};  Borrower: {2}; Target: {3}; lst work: {4}; Session Message: {5}",
                                    queue.Queue, queue.Department, ri.GetText(row, 2, 9), ri.GetText(row, 12, 9), ri.GetText(row, 22, 8), ri.GetText(22, 3, 75), DateTime.Now.ToString());
                                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                            }
                        }
                    }

                    if (row == 20)
                    {
                        ri.Hit(Key.F8);
                        row = 6;
                    }
                }
            }

            CloseConnectionAndSession(batch, ri);
        }

        private void CloseConnectionAndSession(BatchProcessingHelper batch, ReflectionInterface ri)
        {
            try
            {
                BatchProcessingHelper.CloseConnection(batch);
                ri.CloseSession();
            }
            catch (Exception ex)
            {
                string message = string.Format("{0}; There was an error closing the session for user id: {1}", DateTime.Now.ToString(), batch.UserName);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
        }
    }
}