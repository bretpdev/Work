using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Uheaa.Common.DataAccess;

namespace MD
{
    public static class TestHelper
    {
        private const string TestSourceLocation = @"\\uheaa-fs\DEVSEASCS\Codebase\Applications\MD\";
        private const string ExeName = "MD.exe";
        private static string SourceExePath
        {
            get { return Path.Combine(TestSourceLocation, ExeName); }
        }
        public static bool IsTesting
        {
            //"c:\\enterprise program files\\"
            //TODO, UNDO Making reference Cornerstone since the filepath is the same as uheaa and the start runs in live, so until the uheaa keys are created it won't work on test
            get { return !Assembly.GetExecutingAssembly().Location.ToLower().StartsWith(EnterpriseFileSystem.GetPath("EnterpriseProgramFiles", DataAccessHelper.Region.CornerStone).ToLower().Replace("test\\","")); }
        }
        public static bool NewVersionAvailable
        {
            get
            {
                if (!IsTesting) return false;
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
            //TODO REMOVE If this is no longer necessary
            //#if DEBUG
            //            Proc.Start("MDCopyStartDebug", $"{TestSourceLocation} {ExeName}");
            //#else
            //            Proc.Start("MDCopyStart", $"{TestSourceLocation} {ExeName}");
            //#endif

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path, "CopyAndStart.exe");
            Process.Start(path, $"{TestSourceLocation} {ExeName}");

            UIHelper.Instance.QuitApplication();
        }
    }
}
