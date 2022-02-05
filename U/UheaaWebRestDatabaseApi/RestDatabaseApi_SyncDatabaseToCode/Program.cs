using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace RestDatabaseApi_SyncDatabaseToCode
{
    class Program
    {
        static void Main(string[] args)
        {
            string scriptLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            scriptLocation = Path.Combine(scriptLocation, "restdatabaseapi_sync_" + DateTime.Now.ToString("yyyy_mm_dd_hh_MM_ss") + ".sql");
            var generatedScript = new CompiledScript();

            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
            var da = new DataAccess();
            var codeControllers = GetControllersFromCode();
            var dbControllers = da.GetControllers(true);

            foreach (var codeController in codeControllers.OrderBy(o => o.ControllerId))
            {
                var matchingDbController = dbControllers.SingleOrDefault(o => o.ControllerId == codeController.ControllerId);
                if (matchingDbController == null)
                    generatedScript.InsertController(codeController);
                else if (matchingDbController.ControllerName != codeController.ControllerName)
                    generatedScript.RenameController(codeController, matchingDbController.ControllerName);
                foreach (var action in codeController.ActionNames)
                {
                    bool hasMatching = matchingDbController?.ActionNames?.Any(o => o == action) ?? false;
                    if (!hasMatching)
                        generatedScript.InsertControllerAction(codeController, action);
                    else
                        matchingDbController.ActionNames.Remove(action);
                }
                if (matchingDbController != null)
                {
                    dbControllers.Remove(matchingDbController);
                    foreach (var remainingDbAction in matchingDbController?.ActionNames)
                        generatedScript.RetireControllerAction(matchingDbController, remainingDbAction);
                }
            }
            foreach (var remainingDbController in dbControllers)
                generatedScript.RetireController(remainingDbController);

            File.WriteAllLines(scriptLocation, generatedScript.GetCompiledScriptLines());
            Process.Start(scriptLocation);
        }

        static List<ControllerInfo> GetControllersFromCode()
        {
            List<ControllerInfo> controllers = new List<ControllerInfo>();
            var baseControllerType = typeof(RestDatabaseApi.RestDatabaseApiControllerBase);
            var baseDbType = typeof(RestDatabaseApi.DbControllerBase);
            RestDatabaseApi.RestDatabaseApiControllerBase.SkipAccessCheck = true;
            var assembly = Assembly.GetAssembly(baseControllerType);
            //find all controllers
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && type.IsSubclassOf(baseControllerType) && !type.IsAbstract)
                {
                    var info = new ControllerInfo();
                    controllers.Add(info);
                    info.ControllerName = type.Name;
                    var instance = (RestDatabaseApi.RestDatabaseApiControllerBase)Activator.CreateInstance(type); //passing true to constructor skips access checks
                    info.ControllerId = instance.ControllerId;
                    if (!string.IsNullOrEmpty(instance.ControllerFriendlyName))
                        info.ControllerName = instance.ControllerFriendlyName;
                    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly))
                    {
                        if (!method.IsSpecialName) //filters auto-generated methods like property getters and setters
                            info.ActionNames.Add(method.Name);
                    }
                    if (type.IsSubclassOf(baseDbType))
                        info.ActionNames.Add("Execute"); //all db controllers have an execute method
                }
            }
            return controllers;
        }


    }
}
