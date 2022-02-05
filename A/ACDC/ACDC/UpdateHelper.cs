using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace ACDC
{
    public static class UpdateHelper
    {
        private static string SourceLocation
        {
            get
            {
                return string.Format(EnterpriseFileSystem.GetPath("ACDC_Network"), DataAccessHelper.CurrentMode.ToString());
            }
        }
        private const string ExeName = "ACDC.exe";
        private static string SourceExePath
        {
            get { return Path.Combine(SourceLocation, ExeName); }
        }

        public static bool NewVersionAvailable
        {
            get
            {
                var copyAndStartNetwork = new FileInfo(new Uri(Path.Combine(SourceLocation, "CopyAndStart.exe")).LocalPath);
                var copyAndStartLocal = new FileInfo(Path.Combine(EnterpriseFileSystem.GetPath("ACDC_Folder"), "CopyAndStart.exe"));
                //Update Copy and Start if needed
                if (CheckIfNewVersion(copyAndStartNetwork, copyAndStartLocal))
                {
                    File.Copy(copyAndStartNetwork.FullName, copyAndStartLocal.FullName, true);
                }
                return CheckIfNewVersion();
                //return CheckIfNewVersion(2, 3, 4); //Change the major, minor or build to be higher than the current to do an update.
            }
        }

        /// <summary>
        /// Checks the Major if it is greater than the current. If not, checks the minor if it is greater than the current
        /// and if not it checks the build if it is greater than the current. If any are greater, there is an update available.
        /// </summary>
        private static bool CheckIfNewVersion(FileInfo source = null, FileInfo dest = null)
        {
            if (source == null)
                source = new FileInfo(new Uri(SourceExePath).LocalPath);
            if (dest == null)
                dest = new FileInfo(Path.Combine(EnterpriseFileSystem.GetPath("ACDC_Folder"), ExeName));
            if (source.LastWriteTime != dest.LastWriteTime)
                return true;  //network location is newer
            else if (source.Length != dest.Length)
                return true;  //files aren't same size
            else if (source.Extension.ToLower().IsIn(".exe", ".dll"))
            {
                string sourceVersion = FileVersionInfo.GetVersionInfo(source.FullName).FileVersion;
                string destVersion = FileVersionInfo.GetVersionInfo(dest.FullName).FileVersion;
                if (sourceVersion != destVersion)
                    return true; //everything else is identical, but network is clearly on a different assembly version
            }
            return false;
            //int oldMajor = Assembly.GetExecutingAssembly().GetName().Version.Major;
            //if (major != oldMajor)
            //    return true;
            //int oldMinor = Assembly.GetExecutingAssembly().GetName().Version.Minor;
            //if (minor != oldMinor)
            //    return true;
            //int oldBuild = Assembly.GetExecutingAssembly().GetName().Version.Build;
            //if (build != oldBuild)
            //    return true;
            //return false;
        }

        private static int CurrentMajor
        {
            get
            {
                if (!File.Exists(new Uri(SourceExePath).LocalPath)) return Assembly.GetExecutingAssembly().GetName().Version.Major;
                return FileVersionInfo.GetVersionInfo(new Uri(SourceExePath).LocalPath).FileMajorPart;
            }
        }

        private static int CurrentMinor
        {
            get
            {
                if (!File.Exists(new Uri(SourceExePath).LocalPath)) return Assembly.GetExecutingAssembly().GetName().Version.Minor;
                return FileVersionInfo.GetVersionInfo(new Uri(SourceExePath).LocalPath).FileMinorPart;
            }
        }

        private static int CurrentBuild
        {
            get
            {
                if (!File.Exists(new Uri(SourceExePath).LocalPath)) return Assembly.GetExecutingAssembly().GetName().Version.Build;
                return FileVersionInfo.GetVersionInfo(new Uri(SourceExePath).LocalPath).FileBuildPart;
            }
        }

        /// <summary>
        /// Creates a new ProcessStartInfo object, calls the CopyAndStart.exe and exits this application while
        /// a new version is pulled down and loaded.
        /// </summary>
        public static void Update()
        {
            string myLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ProcessStartInfo proc = new ProcessStartInfo(Path.Combine(myLocation, "CopyAndStart.exe"));
            //It is necessary to add "" around the path as well as an ending \
            proc.Arguments = "\"" + new Uri(SourceLocation).LocalPath + "\\\"" + " " + ExeName + " " + DataAccessHelper.CurrentMode.ToString();
            proc.UseShellExecute = false;
            Process.Start(proc);
            Environment.Exit(0);
        }
    }
}