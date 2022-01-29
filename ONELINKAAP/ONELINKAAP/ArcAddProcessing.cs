using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ONELINKAAP
{
    public class ArcAddProcessing
    {
        public ProcessLogRun LogRun { get; set; }
        public List<Thread> Threads { get; set; }
        private readonly string ScriptId;
        private readonly DataAccessHelper.Region Region;
        private readonly DataAccessHelper.Database Db;
        private DataAccess DA { get; set; }

        public ArcAddProcessing(ProcessLogRun logRun, DataAccessHelper.Region region, string scriptId)
        {
            LogRun = logRun;
            ScriptId = scriptId;
            Region = region;
            Db = DataAccessHelper.Database.Uls;
            DA = new DataAccess(LogRun.ProcessLogId, Region, Db);
        }

        /// <summary>
        /// Checks for available Batch ID and creates a new ArcProcessor.
        /// </summary>
        /// <param name="sCount">Session count</param>
        public int ProcessArcs(int sCount)
        {
            DA.CleanUpArcs();
            int sessionCount = 0;
            Threads = new List<Thread>();
            for (int count = 1; count <= sCount; count++)
            {
                if (DA.GetArcCount() == 0)
                    break;
                ArcProcessor processor = new ArcProcessor(LogRun, Region, ScriptId, DA);
                if (!StartProcess(count, processor))
                    count--;
                sessionCount++;
                if (sessionCount == sCount && Threads.Count() == 0)
                {
                    EmailHelper.SendMail(DataAccessHelper.TestMode, "sshelp@utahsbr.edu", "ArcAddProcessing@utahsbr.edu", "ONELINK AAP, Error logging into sessions", "ONELINK AAP, There was an error logging into the session for 1 or more sessions. This could be the result of many problems including expired passwords or UT ID trying to log in more than the allowed number of times.", "", EmailHelper.EmailImportance.High, true);
                    return 1;
                }
            }

            foreach (Thread t in Threads)
                t.Join();

            return 0;
        }

        /// <summary>
        /// Creates a new thread, opens a connection to the database and starts the process
        /// </summary>
        /// <return>Decrements the count if the session didn't login with the Batch ID</return>
        private bool StartProcess(int count, ArcProcessor processor)
        {
            try
            {
                if (processor.LoginInfo != null)
                {
                    Console.WriteLine(string.Format("{0}; Creating {1} thread {2}", DateTime.Now.ToString(), Region.ToString(), count));
                    Thread thread = new Thread(() => processor.Process(count));
                    thread.Start();
                    Threads.Add(thread); //Add all the threads to a list to be joined together.
                    return true;
                }
                else
                {
                    Console.WriteLine(string.Format("{1}: {2} Thread {0} did not log in properly, closing session.", count, DateTime.Now.ToString(), Region));
                    processor.RI.CloseSession();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{3}: Error creating and using {2} session for thread {0}\r\nEX:{1}", count, ex.Message, Region, DateTime.Now.ToString()));
            }
            return false;
        }

    }
}
