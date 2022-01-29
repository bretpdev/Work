using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SasCoordinator
{
    public class SasTunnelHelper
    {
        public SasSoftwareHelper SoftwareHelper { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string SysParm { get; set; }
        private ProcessLogData Data { get; set; }
        private bool JobIsLocal { get { return string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Username); } }
        public SasTunnelHelper(string username, string password, string sysparm, bool testMode, SasSoftwareHelper softwareHelper, ProcessLogData data)
            :this(sysparm, testMode, softwareHelper, data)
        {
            Username = username;
            Password = password;
        }
        public SasTunnelHelper(string sysparm, bool testMode, SasSoftwareHelper softwareHelper, ProcessLogData data)
        {
            SysParm = sysparm;
            SoftwareHelper = softwareHelper;
            Data = data;
        }

        public Process Tunnel { get; internal set; }
        public SasRegion? TunnelRegion { get; internal set; }
        public void OpenTunnel(SasRegion region)
        {
            TunnelRegion = region;
            if (!JobIsLocal)
            {
                ProcessLogger.AddNotification(Data.ProcessLogId, "Opening tunnel " + region.ToString(), NotificationType.EndOfJob, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly(), null);
                string args = string.Format("-pw {0} -L {3}:host{2}.aessuccess.org:4502 {1}@host{2}.aessuccess.org",
                    Password,
                    Username,
                    ((int)region).ToString(),
                    region == SasRegion.Duster ? "5555" : "");
                var process = new Process();
                process.StartInfo = new ProcessStartInfo(SoftwareHelper.AesLinkLocation, args)
                {
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                process.Start();
                
                Tunnel = process;
                ProcessLogger.AddNotification(Data.ProcessLogId, "Finished opening tunnel " + region.ToString(), NotificationType.EndOfJob, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly(), null);
            }
        }

        public void CloseTunnel()
        {
            if (Tunnel != null)
            {
                Tunnel.Kill();
                string output = Tunnel.StandardOutput.ReadToEnd();
                ProcessLogger.AddNotification(Data.ProcessLogId, "Tunnel Results:\r\n" + output, NotificationType.EndOfJob, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly());
            }
        }

        public bool ExecuteScript(string scriptLocation)
        {
            string connectionCode = SoftwareHelper.GetConnectionCode(TunnelRegion.Value, false); // Defaulting to live
            if (JobIsLocal)
                connectionCode = ""; //don't attempt to connect for local jobs
            string scriptCode = File.ReadAllText(scriptLocation);
            string logDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Guid.NewGuid().ToString());
            Directory.CreateDirectory(logDir);
            string logFile = Path.Combine(logDir, Path.GetFileNameWithoutExtension(scriptLocation) + ".log");
            string finalFile = Path.Combine(logDir, "final.txt");
            string finalStatement = GetFinalScriptCode(finalFile);
            string path = SoftwareHelper.CreateTempFile(
                string.Join(Environment.NewLine, connectionCode, scriptCode, finalStatement));
            string format = @"-initstmt {0}%let USERID = '{2}';%let PASSWORD='{3}';{0} -sysin {0}{1}{0} -icon -log {0}{4}{0}";
            string args = string.Format(format, '"', path, Username, Password, logFile);
            if (!string.IsNullOrEmpty(SysParm))
                args += string.Format(" -sysparm {0}{1}{0}", '"', SysParm);


            var process = new Process();
            process.StartInfo = new ProcessStartInfo(SoftwareHelper.FindSasExecutable(), args);
            process.Start();
            process.WaitForExit();

            bool success = File.Exists(finalFile);
            if (success)
                File.Delete(finalFile);
            ProcessLogger.AddNotification(Data.ProcessLogId, File.ReadAllText(logFile), NotificationType.EndOfJob, NotificationSeverityType.Informational);
            foreach (var file in Directory.GetFiles(logDir))
                Repeater.TryRepeatedly(() => File.Delete(file));
            Repeater.TryRepeatedly(() => Directory.Delete(logDir));
            return success;
        }

        private string GetFinalScriptCode(string path)
        {
            string code = 
            @"DATA _NULL_;
            FILE '[path]' DELIMITER=',' DSD DROPOVER LRECL=32767;
            PUT 'finished' ;
            RUN;";
            return code.Replace("[path]", path);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);
        enum ShowWindowCommands : int
        {
            /// <summary>
            /// Hides the window and activates another window.
            /// </summary>
            Hide = 0,
            /// <summary>
            /// Activates and displays a window. If the window is minimized or 
            /// maximized, the system restores it to its original size and position.
            /// An application should specify this flag when displaying the window 
            /// for the first time.
            /// </summary>
            Normal = 1,
            /// <summary>
            /// Activates the window and displays it as a minimized window.
            /// </summary>
            ShowMinimized = 2,
            /// <summary>
            /// Maximizes the specified window.
            /// </summary>
            Maximize = 3, // is this the right value?
            /// <summary>
            /// Activates the window and displays it as a maximized window.
            /// </summary>       
            ShowMaximized = 3,
            /// <summary>
            /// Displays a window in its most recent size and position. This value 
            /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except 
            /// the window is not activated.
            /// </summary>
            ShowNoActivate = 4,
            /// <summary>
            /// Activates the window and displays it in its current size and position. 
            /// </summary>
            Show = 5,
            /// <summary>
            /// Minimizes the specified window and activates the next top-level 
            /// window in the Z order.
            /// </summary>
            Minimize = 6,
            /// <summary>
            /// Displays the window as a minimized window. This value is similar to
            /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the 
            /// window is not activated.
            /// </summary>
            ShowMinNoActive = 7,
            /// <summary>
            /// Displays the window in its current size and position. This value is 
            /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the 
            /// window is not activated.
            /// </summary>
            ShowNA = 8,
            /// <summary>
            /// Activates and displays the window. If the window is minimized or 
            /// maximized, the system restores it to its original size and position. 
            /// An application should specify this flag when restoring a minimized window.
            /// </summary>
            Restore = 9,
            /// <summary>
            /// Sets the show state based on the SW_* value specified in the 
            /// STARTUPINFO structure passed to the CreateProcess function by the 
            /// program that started the application.
            /// </summary>
            ShowDefault = 10,
            /// <summary>
            ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread 
            /// that owns the window is not responding. This flag should only be 
            /// used when minimizing windows from a different thread.
            /// </summary>
            ForceMinimize = 11
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);
    }

}
