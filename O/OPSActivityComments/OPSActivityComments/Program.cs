using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.FileLoad;
using Uheaa.Common.ProcessLogger;

namespace OPSActivityComments
{
    public static class Program
    {
        const string ScriptId = "OPSACTCMAA";

        [STAThread]
        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            if (!DataAccessHelper.StandardArgsCheck(args, "OPSActivityComments"))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            ProcessLogData logData = ProcessLogger.RegisterApplication(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
            bool csReturnValue = false;
            bool uheaaReturnValue = false;
            //Load the data files into the database
            if (LoadFile(logData))
            {
                List<Task> threads = new List<Task>();
                threads.Add(Task.Factory.StartNew(() => csReturnValue = new ActivityComments(logData, DataAccessHelper.Region.CornerStone).ProcessComment(), TaskCreationOptions.LongRunning));
                threads.Add(Task.Factory.StartNew(() => uheaaReturnValue = new ActivityComments(logData, DataAccessHelper.Region.Uheaa).ProcessComment(), TaskCreationOptions.LongRunning));

                Task.WhenAll(threads).Wait();

                ProcessLogger.LogEnd(logData.ProcessLogId);

                Console.WriteLine("Processing Complete");
                Thread.Sleep(5000);
            }
            //If either region is false, return a 1. Otherwise, return a 0
            return (!csReturnValue || !uheaaReturnValue) ? 1 : 0;
        }

        /// <summary>
        /// Loads the data files for both regions
        /// </summary>
        /// <param name="logData">ProcessLogData object</param>
        /// <returns>True: at least one regions file loaded; False: no files were loaded</returns>
        private static bool LoadFile(ProcessLogData logData)
        {
            //Load Uheaa files
            Console.WriteLine("Loading data for the Uheaa region");
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            bool loadedUheaa = FileLoader.LoadFiles(logData, ScriptId, 3, DataAccessHelper.Region.Uheaa, "fp", "\n", false);

            Console.WriteLine("");

            //Load Cornerstone files
            Console.WriteLine("Loading data for the Cornerstone region");
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            bool loadedCS = FileLoader.LoadFiles(logData, ScriptId, 3, DataAccessHelper.Region.CornerStone, "fp", "\n", false);
            
            return loadedUheaa || loadedCS;
        }
    }
}