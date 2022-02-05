using System;
using System.Collections.Generic;
using System.Threading;
using Uheaa.Common.ProcessLogger;

namespace DIACTCMTS
{
    class UheaaProcessing
    {
        private List<Thread> Threads { get; set; }
        public static Queue<NobleCallHistoryData> Data { get; set; }
        private ProcessLogRun LogRun { get; set; }
        private ReaderWriterLockSlim ThreadLock { get; set; }
        private DataAccess DA { get; set; }

        public UheaaProcessing(ProcessLogRun logRun, DataAccess da)
        {
            DA = da;
            List<NobleCallHistoryData> dbData = DA.Populate("CompassUheaa");
            Data = new Queue<NobleCallHistoryData>(dbData);
            ThreadLock = new ReaderWriterLockSlim();
            LogRun = logRun;
        }

        /// <summary>
        /// Starts the UHEAA process
        /// </summary>
        /// <returns></returns>
        public int Process()
        {
            Threads = new List<Thread>();
            for (int thread = 0; thread < 5; thread++)
            {
                ArcProcessing processor = new ArcProcessing(LogRun, ThreadLock, DA);
                if (!StartProcess(thread, processor))
                {
                    thread--;
                }
            }

            foreach (Thread t in Threads)
                t.Join();

            Console.WriteLine("Uheaa Complete");
            return 0;
        }

        /// <summary>
        /// Creates a new thread, opens a connection to the database and starts the process
        /// </summary>
        /// <return>Decrements the count if the session didn't login with the Batch ID</return>
        private bool StartProcess(int count, ArcProcessing processor)
        {
            try
            {
                Console.WriteLine($"Creating thread {count}");
                Thread thread = new Thread(() => processor.Process(count));
                thread.Start();
                Threads.Add(thread); //Add all the threads to a list to be joined together.
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error using thread {count}\r\nEX:{ex.Message}");
                LogRun.AddNotification("An unknown error occured", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
            return false;
        }
    }
}