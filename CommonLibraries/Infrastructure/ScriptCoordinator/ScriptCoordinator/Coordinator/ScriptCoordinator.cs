using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace ScriptCoordinator
{
    public class ScriptCoordinator
    {
        #region Constants
#if !PHEAAMODE
        const string LiveFfelPath = @"X:\Sessions\UHEAA Codebase\";
        const string TestFfelPath = @"X:\PADU\UHEAACodeBase\";
#else
        const string LiveFfelPath = @"X:\Sessions\PheaaAutomation\";
        const string TestFfelPath = @"X:\PADU\PheaaAutomation\";
#endif
        const string LiveFedPath = @"Z:\Codebase\Scripts\";
        const string TestFedPath = @"Y:\Codebase\Scripts\";
        const string LiveLocalPath = @"C:\Enterprise Program Files\Nexus\";
        const string TestLocalPath = @"C:\Enterprise Program Files\Nexus\Test\";
#endregion
        public Args Args { get; set; }
        private readonly ProcessLogData PLD;
        private readonly string LocalPath;
        private readonly string NetworkPath;
        private readonly string NetworkScriptPath;
        private readonly bool CopyEntireNetworkPath;
        public ScriptCoordinator(Args args)
        {
            Args = args;
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)Enum.Parse(typeof(DataAccessHelper.Mode), Args.DataMode, true);
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            PLD = ProcessLogger.RegisterApplication("ScriptCoordinator", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
#region Find Network Script
            LocalPath = Args.LiveMode ? LiveLocalPath : TestLocalPath;
            string ffelDirectory = Args.LiveMode ? LiveFfelPath : TestFfelPath;
            string ffelScriptDirectory = Path.Combine(ffelDirectory, Args.ScriptId);
            string ffelScriptFile = ffelScriptDirectory + ".dll";
            string fedDirectory = Args.LiveMode ? LiveFedPath : TestFedPath;
            string fedScriptDirectory = Path.Combine(fedDirectory, Args.ScriptId);
            string fedScriptFile = fedScriptDirectory + ".dll";

            if (Directory.Exists(ffelScriptDirectory) || Directory.Exists(fedScriptDirectory))
            {
                if (Directory.Exists(ffelScriptDirectory))
                    NetworkPath = ffelScriptDirectory;
                else
                    NetworkPath = fedScriptDirectory;
                NetworkScriptPath = Path.Combine(NetworkPath, Args.ScriptId + ".dll");
                LocalPath = Path.Combine(LocalPath, Args.ScriptId);
                CopyEntireNetworkPath = true;
            }
            else if (File.Exists(ffelScriptFile) || File.Exists(fedScriptFile))
            {
                if (File.Exists(ffelScriptFile))
                    NetworkScriptPath = Path.Combine(ffelDirectory, args.ScriptId + ".dll");
                else
                    NetworkScriptPath = Path.Combine(fedDirectory, args.ScriptId + ".dll");
                NetworkPath = ffelDirectory;
                CopyEntireNetworkPath = false;
            }
            else
            {
                throw new Exception(string.Format("Could not locate Script {0} in Mode {1}", Args.ScriptId, Args.DataMode));
            }
#endregion
        }
        public void Finish()
        {
            ProcessLogger.LogEnd(PLD.ProcessLogId);
        }

        public void LaunchScript()
        {
            var path = Path.Combine(LocalPath, Args.ScriptId + ".dll");
            var results = MainClassFinder.FindMainClass(path, Args.ClassName);
            if (results.MainClassName != null)
            {
                ProcessLogger.AddNotification(PLD.ProcessLogId, "Starting Script: " + Args.ScriptId, NotificationType.EndOfJob, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly(), null);
                try
                {
                    ScriptLauncher.LaunchScript(results, Args.ReflectionOleName, Args.ScriptId);
                }
                catch (CannotUnloadAppDomainException ex)
                {
                    ProcessLogger.AddNotification(PLD.ProcessLogId, string.Format("Unable to unload appdomain for script {0}.  Most likely cause is a thread that hasn't finished running in your script.", Args.ScriptId), NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly(), ex);
                }
            }
        }

        /// <summary>
        /// Cache all Uheaa.Common.* and Q code.  Additionally cache all Interop.*.dll files.
        /// </summary>
        public CacheResults CacheCommonCode()
        {
            string[] searchStrings = { "Uheaa.*.dll", "Q.dll", "Interop.*.dll"}; 
            List<Tuple<string, Exception>> failedFiles = new List<Tuple<string, Exception>>();
            var files = searchStrings.SelectMany(o => Directory.GetFiles(NetworkPath, o));
            if (CopyEntireNetworkPath)
                files = Directory.GetFiles(NetworkPath);
            foreach (string networkFile in files)
            {
                var networkInfo = new FileInfo(networkFile);
                var exception = CacheIfNecessary(networkInfo.FullName, Path.Combine(LocalPath, networkInfo.Name));
                if (exception != null)
                    failedFiles.Add(new Tuple<string, Exception>(networkInfo.Name, exception));
            }
            if (failedFiles.Any())
                return CacheResults.CacheFailure(PLD, NetworkPath, LocalPath, failedFiles);
            return CacheResults.Success();
        }

        /// <summary>
        /// Cache the script supplied in the Args.
        /// </summary>
        public CacheResults CacheScript()
        {
            var local = Path.Combine(LocalPath, Args.ScriptId.ToUpper() + ".dll");
            var exception = CacheIfNecessary(NetworkScriptPath, local);
            if (exception != null)
                return CacheResults.CacheFailure(PLD, NetworkPath, local, new Tuple<string, Exception>(NetworkScriptPath, exception));
            else
                return CacheResults.Success();
        }

        /// <summary>
        /// Compare the dates of a network and local file, and copy locally if necessary.
        /// </summary>
        private Exception CacheIfNecessary(string networkPath, string localPath)
        {
            var networkInfo = new FileInfo(networkPath);
            var localInfo = new FileInfo(localPath);
            if (localInfo.Exists)
                if (localInfo.LastWriteTime >= networkInfo.LastWriteTime)
                    return null; //cache is current, no need to copy
            try
            {
                if (!Directory.Exists(localInfo.DirectoryName))
                    FS.CreateDirectory(localInfo.DirectoryName);
                FS.Copy(networkInfo.FullName, localInfo.FullName, true);
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }
    }
}
