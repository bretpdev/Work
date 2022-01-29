using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;

namespace SessionTester
{
    public static class ScriptHelper
    {
        private const string FedName = "FedScripts";
        private const string FfelName = "FfelScripts";
        private static readonly string[] ScriptNames = new string[] { FedName, FfelName };

        public static IEnumerable<Dll> FfelScripts { get { return GetScript(FfelName, DataAccessHelper.Region.Uheaa); } }
        public static IEnumerable<Dll> FedScripts { get { return GetScript(FedName, DataAccessHelper.Region.CornerStone); } }
        private static IEnumerable<Dll> GetScript(string subFolder, DataAccessHelper.Region region)
        {
            string folder = Path.Combine(UheaaRoot, subFolder);
            return GetDlls(folder, region);
        }

        public static string UheaaRoot
        {
            get
            {
                string thisLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string currentLocation = thisLocation;
                while (Directory.GetDirectories(currentLocation).Where(o => ScriptNames.Contains(Path.GetFileName(o))).Count() < 2)
                    currentLocation = Directory.GetParent(currentLocation).FullName;
                return currentLocation;
            }
        }

        private static IEnumerable<Dll> GetDlls(string directory, DataAccessHelper.Region region)
        {
            if (directory.Contains("\\obj\\"))
                return new Dll[] { }; //don't include obj dlls or common code
            List<Dll> dlls = new List<Dll>();
            string blah = Directory.GetFiles(directory).Where(f => f.EndsWith(".dll") && !f.Contains("Uheaa.Common")).FirstOrDefault();
            foreach (string file in Directory.GetFiles(directory).Where(f => f.EndsWith(".dll") && !f.Contains("Uheaa.Common"))) //only load dlls, don't load common dlls
            {
                Dll dll = new Dll(file, region);
                if (dll.IsScript)
                    dlls.Add(dll);
            }
            foreach (string folder in Directory.GetDirectories(directory))
            {
                dlls.AddRange(GetDlls(folder, region));
            }
            return dlls.OrderByDescending(d => d.LastModified);
        }

        public static void StartScript(Dll dll)
        {
            var settings = SettingsHelper.Instance;
            bool cornerStone = dll.Region == DataAccessHelper.Region.CornerStone;

            using (ManagedReflectionInterface ri = new ManagedReflectionInterface())
            {
                Func<string> getUsername = () => cornerStone ? settings.CornerStoneUsername : settings.UheaaUsername;
                Func<string> getPassword = () => cornerStone ? settings.CornerStonePassword : settings.UheaaPassword;

                var assembly = Assembly.LoadFrom(dll.Path);
                ri.ProcessLogData = ProcessLogger.RegisterScript(dll.ScriptId, AppDomain.CurrentDomain, assembly);

                bool retry = true;
                while (retry)
                {
                    if (!ri.Login(getUsername(), getPassword(), dll.Region))
                    {
                        SettingsWindow sw = new SettingsWindow();
                        retry = sw.ShowDialog() ?? false;
                    }
                    else
                        retry = false;
                }
                try
                {
                    IHasMain script = (IHasMain)(Activator.CreateInstanceFrom(dll.Path, dll.ClassName, true, BindingFlags.Default, null, new object[] { ri }, CultureInfo.CurrentCulture, null) as ObjectHandle).Unwrap();
                    script.Main();
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException is Uheaa.Common.Scripts.EndDLLException)
                        ri.Dispose();
                }
            }
        }
    }
}
