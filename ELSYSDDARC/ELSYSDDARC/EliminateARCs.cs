using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.FileLoad;
using Uheaa.Common.ProcessLogger;

namespace ELSYSDDARC
{
    class EliminateARCs
    {
        public static string ScriptId { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        public List<Thread> Threads { get; set; }

        public EliminateARCs(string scriptId, ProcessLogRun logRun, DataAccess da)
        {
            LogRun = logRun;
            ScriptId = scriptId;
            DA = da;
        }

        /// <summary>
        /// Finds the current data file, creates the sessions and calls the ProcessFile method
        /// </summary>
        /// <returns>0 if run successful, 1 if there was an error in processing</returns>
        public int Run()
        {
            if (FileLoader.LoadFiles(LogRun, ScriptId, DataAccessHelper.CurrentRegion, "fp"))
            {
                int sessionCount = 0;
                Threads = new List<Thread>();
                for (int count = 1; count <= 10; count++)
                {
                    if (DA.GetRecordCount(ScriptId) == 0)
                        continue;
                    Processor processor = new Processor(LogRun, DA, ScriptId);
                    if (!processor.LoginInfo.UserName.IsNullOrEmpty())
                    {
                        if (!StartProcess(count, processor))
                            count--;
                        sessionCount++;
                        if (sessionCount == 10 && Threads.Count == 0)
                        {
                            EmailHelper.SendMail(DataAccessHelper.TestMode, "sshelp@utahsbr.edi", "ELSYSDDARC@utahsbr.edu", "Reset Batch Passwords", "The Batch passwords in the Batch Login Database need to be changed.", "", "", EmailHelper.EmailImportance.High, true);
                            return 1;
                        }
                    }
                }

                foreach (Thread t in Threads)
                    t.Join();
            }
            return 0;
        }

        /// <summary>
        /// Creates a new thread, opens a conection to the database and starts the process
        /// </summary>
        /// <param name="count">The current sesion count</param>
        /// <param name="processor">The processor to process with</param>
        /// <returns>Decrements the count if the session didn't log in with the Batch ID</returns>
        private bool StartProcess(int count, Processor processor)
        {
            try
            {
                if (processor.Login())
                {
                    Console.WriteLine(string.Format("Creating {0} thread {1}", DataAccessHelper.CurrentRegion.ToString(), count));
                    Thread thread = new Thread(() => processor.ProcessData(count));
                    thread.Start();
                    Threads.Add(thread);
                    return true;
                }
                else
                {
                    Console.WriteLine(string.Format("Thread {0} did not log in properly, closing session."));
                    processor.CloseSession();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error creating and using session for thread {0}\r\nEX:{1}", count, ex.Message));
            }
            return false;
        }
    }
}