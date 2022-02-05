using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.FileLoad;
using Uheaa.Common.ProcessLogger;

namespace OPSACTCMAA
{
    public static class Program
    {
        const string ScriptId = "OPSACTCMAA";
        const int SUCCESS = 0;
        const int ERROR = 1;

        [STAThread]
        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, "OPSACTCMAA") || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            bool result = false;
            //Load the data files into the database
            if (LoadFile(logRun))
            {
                List<Task> threads = new List<Task>();
                threads.Add(Task.Factory.StartNew(() => result = new ActivityComments(logRun).ProcessComment(), TaskCreationOptions.LongRunning));

                Task.WhenAll(threads).Wait();
                logRun.LogEnd();

                Console.WriteLine("Processing Complete");
                Thread.Sleep(5000);
            }
            
            // If result is fale, return a 1 to indicate a failed run
            return (!result) ? ERROR : SUCCESS;
        }

        /// <summary>
        /// Loads the data files for the UHEAA region.
        /// </summary>
        /// <param name="logData">ProcessLogData object</param>
        /// <returns>True: File loaded; False: File not loaded</returns>
        private static bool LoadFile(ProcessLogRun logRun)
        {
            //Load Uheaa files
            Console.WriteLine("Loading data for the Uheaa region");
            bool loadedFile = FileLoader.LoadFiles(logRun, ScriptId, DataAccessHelper.CurrentRegion, "fp", "\r", false);

            return loadedFile;
        }
    }
}