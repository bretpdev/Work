using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace Uheaa
{
    [ComVisible(true)]
    [Guid("F90D02D0-746C-4FDC-972E-D6A67C362F5E")]
    [ClassInterface(ClassInterfaceType.None)]
    public class ScriptSync : IScriptSync
    {
        private const string EnterpriseProgramFiles = @"C:\Enterprise Program Files\";

        [ComVisible(true)]
        public bool SyncAndStartWithErrorPopup(Reflection.Session session, int mode, string scriptId)
        {
            return SyncAndStart(session, mode, scriptId, true);
        }

        [ComVisible(true)]
        public bool SyncAndStart(Reflection.Session session, int mode, string scriptId, bool errorPopup = true)
        {
            DateTime start = DateTime.Now;
            IInstantiationWrapper<object> script = null;
            DataAccessHelper.Region region = DataAccessHelper.Region.None;
            try
            {
                if (scriptId.ToLower().EndsWith(".dll")) scriptId = scriptId.Substring(0, scriptId.Length - 4);
                script = InstantiateScript(session, mode, scriptId);
                region = script.Script.Area.IsFed ? DataAccessHelper.Region.CornerStone : DataAccessHelper.Region.Uheaa;
                bool result = RunScript(script.ScriptResult);
                ProcessLogger.LogScriptRun(start, DateTime.Now, scriptId, region);
                return result;
            }
            catch (Exception ex)
            {
                Assembly assembly = script.ReflectionInfo == null ? null : script.ReflectionInfo.Assembly;
                int scriptRunId = ProcessLogger.LogScriptException(start, DateTime.Now, scriptId, region, ex, assembly);
                ErrorForm ef = new ErrorForm(scriptId, ex, scriptRunId);
                ef.ShowDialog();
                return false;
            }
        }

        public void SyncScript(string scriptId)
        {
            FindScript(scriptId).Sync();
        }

        public Script FindScript(string scriptId)
        {
            Areas a = Areas.Current;
            //these are individual statements and checks to make debugging easier.
            var commonFed = a.CommonFed.FindScript(scriptId);
            if (commonFed != null) return commonFed;

            var commonFfel = a.CommonFfel.FindScript(scriptId);
            if (commonFfel != null) return commonFfel;

            var tempFed = a.TempFed.FindScript(scriptId);
            if (tempFed != null) return tempFed;

            var tempFfel = a.TempFfel.FindScript(scriptId);
            if (tempFfel != null) return tempFfel;

            var qFed = a.QFed.FindScript(scriptId);
            if (qFed != null) return qFed;

            var qFfel = a.QFfel.FindScript(scriptId);
            if (qFfel != null) return qFfel;

            return null;
        }

        public IInstantiationWrapper<object> InstantiateScript(Reflection.Session session, int mode, string scriptId)
        {
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)mode;
            return InstantiateScript(session, mode, FindScript(scriptId));
        }
        public IInstantiationWrapper<object> InstantiateScript(Reflection.Session session, int mode, Script script)
        {
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)mode;
            if (script is QScript)
                return InstantiateQScript(session, (QScript)script, DataAccessHelper.TestMode);
            else
                return InstantiateTempOrCommonScript(session, script);
        }

        public IInstantiationWrapper<IHasMain> InstantiateTempOrCommonScript(Reflection.Session session, Script script)
        {
            return InstantiateScript<IHasMain>(script, new ReflectionInterface(session));
        }

        public IInstantiationWrapper<Q.ScriptBase> InstantiateQScript(Reflection.Session session, QScript script, bool testMode)
        {
            return InstantiateScript<Q.ScriptBase>(script, new Q.ReflectionInterface(session, testMode));
        }

        private IInstantiationWrapper<T> InstantiateScript<T>(Script script, params object[] scriptArgs) where T : class
        {
            script.Sync();
            ReflectionResults rr = null;
            try
            {
                rr = MainClassFinder.FindMainClass(script);
                var scriptBase = Activator.CreateInstanceFrom(script.File.LocalFullPath, rr.MainClassName, true, BindingFlags.Default, null, scriptArgs, null, null);
                var unwrapped = scriptBase.Unwrap();
                return new InstantiationWrapper<T>((T)unwrapped, rr, script);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException == null) throw;
                //the StupRegionSpecifiedException exception is thrown by the ValidateRegion function in the ScriptSessionBase if the user is not logged in to the correct region
                if (ex.InnerException is Q.StupRegionSpecifiedException)
                    throw new IncorrectRegionException(ex.InnerException);
                else if (ex.InnerException is StupRegionException)
                    throw new IncorrectRegionException(ex.InnerException);
            }
            catch (MissingMethodException ex)
            {
                if (Regex.IsExactMatch(ex.Message, @"Constructor on type '.*' not found."))
                    throw new InvalidConstructorException(ex);
                throw;
            }
            return new InstantiationWrapper<T>(null, rr, script);
        }

        private bool RunScript(object o)
        {
            try
            {
                var common = o as IHasMain;
                var q = o as Q.ScriptBase;
                if (common != null) common.Main();
                if (q != null) q.Main();
            }
            catch (Exception ex)
            {
                if (ex is Uheaa.Common.Scripts.EndDLLException || ex is Q.EndDLLException)
                    return true; //these exceptions signal that the script is finished running.
                throw;
            }
            return false;
        }
    }

}
