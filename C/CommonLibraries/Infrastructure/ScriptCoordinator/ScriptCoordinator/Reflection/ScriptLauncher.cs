using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ScriptCoordinator
{
    public class ScriptLauncher : MarshalByRefObject
    {
        private class InvalidReflectionException : Exception { public InvalidReflectionException(Exception inner) : base("Invalid Reflection DLL", inner) { } };
        public static void LaunchScript(DllInfo info, string reflectionOleName, string scriptId)
        {
            if (string.IsNullOrEmpty(reflectionOleName))
            {
                reflectionOleName = Guid.NewGuid().ToString().Replace("-", "");
                new Uheaa.Common.Scripts.ReflectionInterface().ReflectionSession.OLEServerName = reflectionOleName;
                Thread.Sleep(10000);
            }
            object[] args = new object[] { info, reflectionOleName, scriptId, (int)DataAccessHelper.CurrentMode, (int)DataAccessHelper.CurrentRegion };

            string dllRoot = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ValidReflectionDlls");
            List<string> reflections = new List<string>();
            if (Directory.Exists(dllRoot))
                reflections = Directory.GetFiles(dllRoot).OrderBy(o => o).ToList();
            for (int currentReflectionTry = -1; currentReflectionTry < reflections.Count; currentReflectionTry++)
            {
                try
                {
                    var domain = GenerateDomain(Path.GetDirectoryName(info.Path));

                    if (currentReflectionTry >= 0)
                    {
                        FS.Copy(reflections[currentReflectionTry], Path.Combine(Path.GetDirectoryName(info.Path), "Interop.Reflection.dll"), true); //overwrite Interop.Reflection.dll with a different version
                    }
                    var assembly = typeof(ScriptLauncher).Assembly.FullName;
                    domain.CreateInstance(assembly, typeof(ScriptLauncher).FullName, false, BindingFlags.Default, null, args, null, null);
                    AppDomain.Unload(domain);
                    break;
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException is InvalidReflectionException)
                        continue; //continue and try a new version of the interop dll.
                    if (ex.InnerException is FileNotFoundException && (ex.InnerException as FileNotFoundException).FileName.StartsWith("Interop.Reflection"))
                        continue;

                    throw;
                }
            }
        }
        private static AppDomain GenerateDomain(string path)
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = path;
            AppDomain domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, setup);
            //foreach (string dll in Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*.dll"))
            //{
            //    if (dll.ToLower().EndsWith("uheaa.common.dll"))
            //        continue; //don't replace common dll
            //    string destinationDll = Path.Combine(path, Path.GetFileName(dll));
            //    FS.Copy(dll, destinationDll, true);
            //}
            string destinationExe = Path.Combine(path, Path.GetFileName(Assembly.GetExecutingAssembly().Location));
            try
            {
                FS.Copy(Assembly.GetExecutingAssembly().Location, destinationExe, true);
            }
            catch (IOException)
            {
                //eat io exception, exe is already there.
            }
            //var directory = Path.GetDirectoryName(info.Path);
            //string search = "Uheaa.*.dll";
            //if (Path.GetFileNameWithoutExtension(info.Path).ToLower() == Path.GetFileName(directory).ToLower())
            //{
            //    search = "*.dll";
            //}
            ////load every child dll manually
            //foreach (string dll in Directory.GetFiles(directory, search))
            //{
            //    if (dll.ToLower() == info.Path.ToLower())
            //        continue;
            //    domain.Load(File.ReadAllBytes(dll));
            //}
            //domain.Load(File.ReadAllBytes(Assembly.GetExecutingAssembly().Location));
            return domain;
        }

        public ScriptLauncher(DllInfo info, string reflectionOleName, string scriptId, int mode, int region)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (o, ea) =>
            {
                var dependantFile = Path.Combine(info.Path, ea.Name.Substring(0, ea.Name.IndexOf(',')) + ".dll");
                if (File.Exists(dependantFile))
                    return Assembly.LoadFrom(dependantFile);
                return null;
            };

            var types = Assembly.LoadFile(info.Path).GetTypes().ToList();
            var existingProcessLogger = Path.Combine(Path.GetDirectoryName(info.Path), "Uheaa.Common.ProcessLogger.dll");
            if (File.Exists(existingProcessLogger))
                types.AddRange(Assembly.LoadFile(existingProcessLogger).GetTypes());
            var existingScripts = Path.Combine(Path.GetDirectoryName(info.Path), "Uheaa.Common.Scripts.dll");
            if (File.Exists(existingScripts))
                types.AddRange(Assembly.LoadFile(existingScripts).GetTypes());
            var existingDa = Path.Combine(Path.GetDirectoryName(info.Path), "Uheaa.Common.DataAccess.dll");
            if (File.Exists(existingDa))
                types.AddRange(Assembly.LoadFile(existingDa).GetTypes());
            var existingQ = Path.Combine(Path.GetDirectoryName(info.Path), "Q.dll");
            if (File.Exists(existingQ))
                types.AddRange(Assembly.LoadFile(existingQ).GetTypes());


            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)mode;
            DataAccessHelper.CurrentRegion = (DataAccessHelper.Region)region;
            var assembly = Assembly.LoadFrom(info.Path);


            Reflection.Session session = null;
            if (string.IsNullOrEmpty(reflectionOleName))
                //no session given, start a new one
                session = Uheaa.Common.Scripts.ReflectionInterface.OpenExistingSession(Uheaa.Common.Scripts.ReflectionInterface.Flag.None);
            else
                //load existing session via OLE Name.  This should be a guid.
                session = (Reflection.Session)Microsoft.VisualBasic.Interaction.GetObject(reflectionOleName, null);

            object reflectionInterface = null;
            try
            {
                if (info.UsesCommon)
                {
                    var existingReflectionInterfaceType = types.Where(o => o.Name == "ReflectionInterface").FirstOrDefault();
                    reflectionInterface = Activator.CreateInstance(existingReflectionInterfaceType, new object[] { session });
                }
                else
                {
                    var existingReflectionInterfaceType = types.Where(o => o.Name == "ReflectionInterface").Last();
                    reflectionInterface = Activator.CreateInstance(existingReflectionInterfaceType, new object[] { session, DataAccessHelper.TestMode });
                    //var qInterface = new Q.ReflectionInterface(session, DataAccessHelper.TestMode);
                    //reflectionInterface = qInterface;
                }
            }
            catch (MissingMethodException ex)
            {
                throw new InvalidReflectionException(ex);
            }


            ProcessLogData pld = ProcessLogger.RegisterScript(scriptId, AppDomain.CurrentDomain, assembly); ;
            if (info.UsesCommon)
            {
                var existingPldType = types.Where(o => o.Name == "ProcessLogData").FirstOrDefault();
                if (existingPldType != null)
                {
                    var newPld = Activator.CreateInstance(existingPldType, new object[] { });
                    newPld.GetType().GetProperty("ProcessLogId").GetSetMethod().Invoke(newPld, new object[] { pld.ProcessLogId });
                    reflectionInterface.GetType().GetProperty("ProcessLogData").GetSetMethod().Invoke(reflectionInterface, new object[] { newPld });
                }
                //(reflectionInterface as Uheaa.Common.Scripts.ReflectionInterface).ProcessLogData = pld;
            }

            Action<Exception> exCheck = new Action<Exception>(ex =>
            {
                if (ex.GetType().Name.ToLower() == "enddllexception" || (ex.InnerException != null && ex.InnerException.GetType().Name.ToLower() == "enddllexception"))
                {
                    //script is finished, log end
                    ProcessLogger.LogEnd(pld.ProcessLogId);
                }
                else
                {
                    LogException(pld, assembly, ex);
                }
            });
            try
            {
                var dah = types.FirstOrDefault(o => o.Name == "DataAccessHelper");
                if (dah != null)
                {
                    dah.GetProperty("CurrentMode").GetSetMethod().Invoke(null, new object[] { (int)mode });
                    dah.GetProperty("CurrentRegion").GetSetMethod().Invoke(null, new object[] { (int)region });
                }
                var script = Activator.CreateInstanceFrom(info.Path, info.MainClassName, false, BindingFlags.Default, null, new object[] { reflectionInterface }, null, null).Unwrap();
                script.GetType().GetMethod("Main").Invoke(script, null);
            }
            catch (TargetInvocationException ex)
            {
                exCheck(ex.InnerException);
            }
            catch (Exception ex)
            {
                exCheck(ex);
            }
        }
        private void LogException(ProcessLogData pld, Assembly assembly, Exception ex)
        {
            ProcessLogger.AddNotification(pld.ProcessLogId, ex.Message, NotificationType.ErrorReport, NotificationSeverityType.Critical, assembly, ex);
            ProcessLogger.LogEnd(pld.ProcessLogId);
            Dialog.Warning.Ok(string.Format("An unexpected error has occurred.  Please contact Systems Support and reference Log Id: {0}", pld.ProcessLogId), "Error");
        }
    }
}
