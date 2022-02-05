using System.Diagnostics;
using System.IO;
using System.Reflection;
using Uheaa.Common.DataAccess;

namespace OLDEMOS
{
    public static class TestHelper
    {
        private const string TestSourceLocation = @"\\uheaa-fs\DEVSEASCS\Codebase\Applications\OLDEMOS\";
        private const string ExeName = "OLDEMOS.exe";

        private static string SourceExePath
        {
            get
            {
                return Path.Combine(TestSourceLocation, ExeName);
            }
        }

        public static bool IsTesting
        {
            get
            {
                return !Assembly.GetExecutingAssembly().Location.ToLower().StartsWith(EnterpriseFileSystem.GetPath("EnterpriseProgramFiles", DataAccessHelper.Region.Uheaa).ToLower().Replace("test\\", ""));
            }
        }

        public static bool NewVersionAvailable
        {
            get
            {
                if (!IsTesting)
                    return false;
                string oldVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return CurrentTestVersion != oldVersion;
            }
        }

        public static string CurrentTestVersion
        {
            get
            {
                if (!File.Exists(SourceExePath)) return Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return FileVersionInfo.GetVersionInfo(SourceExePath).FileVersion;
            }
        }

        public static void Update()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path, "CopyAndStart.exe");
            Process.Start(path, $"{TestSourceLocation} {ExeName}");

            UIHelper.Instance.QuitApplication();
        }
    }
}