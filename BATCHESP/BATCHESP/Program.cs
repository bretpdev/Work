using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace BATCHESP
{
    class Program
    {
        public const string ScriptId = "BATCHESP";
        const int SUCCESS = 0;
        const int ERROR = 1;

        public static int Main(string[] args)
        {
            int returnCode = SUCCESS;
            var argResults = KvpArgValidator.ValidateArguments<Args>(args);
            if (!argResults.IsValid)
                return ERROR;
            else
            {
                var parsedArgs = new Args(args);
                DataAccessHelper.CurrentMode = parsedArgs.Mode;
                if (!DatabaseAccessHelper.StandardSprocAccessCheck(Assembly.GetExecutingAssembly()))
                    return ERROR;
                ReaderWriterLockSlim taskLock = new ReaderWriterLockSlim();
                returnCode += ProcessUheaa(parsedArgs, taskLock);
            }
            DataAccessHelper.CloseAllManagedConnections();
            return returnCode;
        }

        /// <summary>
        /// Intializes tasks by loading them into the ULS ESP tables, then implements multithreading to process tasks.
        /// </summary>
        private static int ProcessRegion(Args parsedArgs, ReaderWriterLockSlim taskLock, DataAccessHelper.Region region, string batchLoginType)
        {
            int returnCode = ERROR;
            var plr = new ProcessLogRun(Program.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), region, DataAccessHelper.CurrentMode, true, false);
            LoadResults loadResults = new BATCHESP(region, batchLoginType, parsedArgs, plr, 1).LoadTasks();
            List<EspEnrollment> tasksToWork = loadResults.TasksToWork;
            ConcurrentQueue<EspEnrollment> work = new ConcurrentQueue<EspEnrollment>(tasksToWork);
            int numberOfThreads = loadResults.NumberOfThreads;
            int errorCount = 0;
            List<Task> taskList = new List<Task>();
            if (loadResults.LoadedSuccessfully)
            {
                Console.WriteLine("Spinning up threads to process tasks.");
                int threadId = 1;
                for (int threadNum = 0; threadNum < numberOfThreads; threadNum++)
                {
                    var newTask = Task.Run(() =>
                    {
                        taskLock.EnterWriteLock();
                        Console.WriteLine($"Logging into Session with thread: {threadId}.");
                        var processor = new BATCHESP(region, batchLoginType, parsedArgs, plr, threadId++);
                        processor.Login();
                        taskLock.ExitWriteLock();

                        EspEnrollment espTask;

                        while (work.TryDequeue(out espTask))
                        {
                            Console.WriteLine($"Now processing the task {espTask.TaskControlNumber} for account {espTask.AccountNumber}.");
                            bool? success = processor.ProcessTask(espTask);
                            if (success.HasValue && !success.Value)
                            {
                                taskLock.EnterWriteLock();
                                errorCount++;
                                taskLock.ExitWriteLock();
                            }
                            else if (!success.HasValue)
                            {
                                return; // End this processing thread if COMException occurs or Batch ID doesn't have access to queues
                            }
                        }
                        processor.EndLoginHelper(processor.threadId);
                    });
                    taskList.Add(newTask);
                }
            }

            Task.WaitAll(taskList.ToArray());
            Console.WriteLine("The script run has finished. Logging the end of the script run.");
            plr.LogEnd();

            if (errorCount == 0)
                returnCode = SUCCESS;
            return returnCode;
        }

        /// <summary>
        /// Processes the respective region.
        /// </summary>
        private static int ProcessUheaa(Args parsedArgs, ReaderWriterLockSlim taskLock)
        {
            return ProcessRegion(parsedArgs, taskLock, DataAccessHelper.Region.Uheaa, "BatchUheaa");
        }
    }
}

