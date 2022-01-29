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

namespace DTX7LUHEAA
{
    class Program
    {
        public static ProcessLogRun PLR { get; set; }
        public static readonly string ScriptId = "DTX7LUHEAA";
        public static Queue<LetterIDAndArcCombo> ExpiredLetters { get; set; }
        public static Queue<LetterIDAndArcCombo> DueDilLetters { get; set; }
        public static Queue<Dtx7lDeletedRecords> DeletedRecords { get; set; }
        private static List<Thread> Threads { get; set; }
        public static Args ApplicationArgs { get; set; }
        private static DataAccess DA { get; set; }

        /// <summary>
        /// ARGS:
        /// Mode (Required)
        /// ShowPrompts (Defaults to True)
        /// NumberOfThreads (Defaults to 4)
        /// </summary>
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Dialog.Error.Ok("Missing KVP Args.");
                return 1;
            }

            var results = KvpArgValidator.ValidateArguments<Args>(args);
            if (results.IsValid)
            {
                ApplicationArgs = new Args(args);
                ApplicationArgs.SetDataAccessValues();

                if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), ApplicationArgs.ShowPrompts))
                    return 1;

                PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion,
                    DataAccessHelper.CurrentMode, ApplicationArgs.ShowPrompts, true, true);

                DA = new DataAccess(PLR.ProcessLogId);

                var expiredLetters = DA.GetExpiredLetters();
                var dueDilLetters = DA.GetDueDilLetters();
                if (ApplicationArgs.LetterIds.Any())
                {
                    expiredLetters = expiredLetters.Where(o => o.LetterID.IsIn(ApplicationArgs.LetterIds.ToArray())).ToList();
                    dueDilLetters = dueDilLetters.Where(o => o.LetterID.IsIn(ApplicationArgs.LetterIds.ToArray())).ToList();
                }
                if (ApplicationArgs.MaxExpiredRecordsToProcess.HasValue)
                    expiredLetters = expiredLetters.Take(ApplicationArgs.MaxExpiredRecordsToProcess.Value).ToList();
                if (ApplicationArgs.MaxDueDilRecordsToProcess.HasValue)
                    dueDilLetters = dueDilLetters.Take(ApplicationArgs.MaxDueDilRecordsToProcess.Value).ToList();
                ExpiredLetters = new Queue<LetterIDAndArcCombo>(expiredLetters);
                DueDilLetters = new Queue<LetterIDAndArcCombo>(dueDilLetters);
                int returnCode = RunApp();
                PLR.LogEnd();
                DataAccessHelper.CloseAllManagedConnections();
                return returnCode;
            }
            else
            {
                Console.WriteLine(results.ValidationMesssage);
                Dialog.Error.Ok(results.ValidationMesssage);
                return 1;
            }
        }

        private static int RunApp(bool addResponse = false)
        {
            int sessionCount = 0;
            Threads = new List<Thread>();
            ReaderWriterLockSlim threadLock = new ReaderWriterLockSlim();
            for (int count = 1; count <= ApplicationArgs.NumberOfThreads; count++)
            {
                SessionProcessor processor = new SessionProcessor(PLR,DA, threadLock, ScriptId);
                if (!StartProcess(count, processor, addResponse))
                    count--;
                sessionCount++;
                if (sessionCount == 10 && Threads.Count() == 0)
                {
                    EmailHelper.SendMail(DataAccessHelper.TestMode, "sshelp@utahsbr.edu", "DTX7L@utahsbr.edu", "Reset Batch Passwords", "The Batch passwords in the Batch Login Database need to be changed.", "", EmailHelper.EmailImportance.High, true);
                    return 1;
                }
            }

            foreach (Thread t in Threads)
                t.Join();

            if (!addResponse)//Rerun though the process but this time it will process TD2a
            {
                DeletedRecords = DA.Populate();
                RunApp(true);
            }

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
                    PLR.AddNotification(string.Format("Thread {0} did not log in properly, closing session.", count), NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    processor.RI.CloseSession();
                }
            }
            catch (Exception ex)
            {
                PLR.AddNotification(string.Format("Error using session for thread {0}\r\nEX:{1}", count, ex.Message), NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
            return false;
        }
    }
}
