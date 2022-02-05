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

namespace DTX7L
{
    class Dtx7lErrorLettersRecords
    {
        public static ProcessLogData LogData { get; set; }
        public static readonly string ScriptId = "DTX7L";
        public static Queue<LetterIDAndArcCombo> ExpiredLetters { get; set; }
        public static Queue<LetterIDAndArcCombo> DueDilLetters { get; set; }
        public static Queue<Dtx7lDeletedRecords> DeletedRecords { get; set; }
        private static List<Thread> Threads { get; set; }

        [UsesSproc(DataAccessHelper.Database.Uls, "spDTX7LGetExpiredLetters")]
        [UsesSproc(DataAccessHelper.Database.Uls, "spDTX7LGetDueDilLetters")]
        static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return 1;
#if !DEBUG 
            if (DataAccessHelper.TestMode && !EverTemp.EnsureTDrive(args))
            {
                Console.WriteLine("App started from network drive.  Restarting on T drive.");
                return 0;
            }
#endif

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            LogData = ProcessLogger.RegisterApplication(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());

            ExpiredLetters = new Queue<LetterIDAndArcCombo>(DataAccessHelper.ExecuteList<LetterIDAndArcCombo>("spDTX7LGetExpiredLetters", DataAccessHelper.Database.Uls));
            DueDilLetters = new Queue<LetterIDAndArcCombo>(DataAccessHelper.ExecuteList<LetterIDAndArcCombo>("spDTX7LGetDueDilLetters", DataAccessHelper.Database.Uls));
            int returnCode = RunApp();
            ProcessLogger.LogEnd(LogData.ProcessLogId);
            return returnCode;
        }

        private static int RunApp(bool addResponse = false)
        {
            int sessionCount = 0;
            Threads = new List<Thread>();
            ReaderWriterLockSlim threadLock = new ReaderWriterLockSlim();
            for (int count = 1; count <= 4; count++)
            {
                SessionProcessor processor = new SessionProcessor(LogData, threadLock, ScriptId);
                if (!StartProcess(count, processor, addResponse))
                    count--;
                sessionCount++;
                if (sessionCount == 10 && Threads.Count() == 0)
                {
                    EmailHelper.SendMail(DataAccessHelper.TestMode, "sshelp@utahsbr.edu", "DTX7L@utahsbr.edu", "Reset Batch Passwords", "The Batch passwords in the Batch Login Database need to be changed.", "", "", EmailHelper.EmailImportance.High, true);
                    return 1;
                }
            }

            foreach (Thread t in Threads)
                t.Join();

            DeletedRecords = Dtx7lDeletedRecords.Populate();
            if (!addResponse)//Rerun though the process but this time it will process TD2a
                RunApp(true);

#if DEBUG
            Console.ReadKey();
#endif
            return 0;
        }

        /// <summary>
        /// Creates a new thread, opens a connection to the database and starts the process
        /// </summary>
        /// <return>Decrements the count if the session didn't login with the Batch ID</return>
        private static bool StartProcess(int count, SessionProcessor processor, bool addResponseCode = false)
        {
            try
            {
                if (processor.Login())
                {
                    Console.WriteLine(string.Format("Creating {1} thread {0}", count, DataAccessHelper.CurrentRegion.ToString()));
                    Thread thread = null;
                    if (!addResponseCode)
                        thread = new Thread(() => processor.ProcessTX7L(count));
                    else
                        thread = new Thread(() => processor.ProcessResponseCodes(count));
                    thread.Start();
                    Threads.Add(thread); //Add all the threads to a list to be joined together.
                    return true;
                }
                else
                {
                    Console.WriteLine(string.Format("Thread {0} did not log in properly, closing session.", count));
                    processor.RI.CloseSession();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error using session for thread {0}\r\nEX:{1}", count, ex.Message));
            }
            return false;
        }
    }
}
