using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SASM
{
    public static class WindowHelper
    {
        public static string SASDirectory { get; internal set; }
        static WindowHelper()
        {
            SASDirectory = SasFinder.FindSasExecutable();

            AppDomain.CurrentDomain.ProcessExit += (o, e) =>
            {
                if (DusterLiveTunnel != null)
                    CloseDusterLive();
                if (LegendTunnel != null)
                    CloseLegend();
                if (DusterTestTunnel != null)
                    CloseDusterTest();
            };
        }

        public static Process DusterLiveTunnel { get; internal set; }
        public static Process DusterTestTunnel { get; set; }
        public static Process LegendTunnel { get; internal set; }
        static string[] plinkVariations = new string[]
        {
            @"C:\Program Files (x86)\AES Link\plink.exe",
            @"C:\Program Files\AES Link\plink.exe",
            @"C:\Program Files (x86)\AES\AESLink.NET\plink.exe",
            @"C:\Program Files\AES\AESLink.NET\plink.exe",
        };
        private static Process OpenTunnel(Region host, string password)
        {
            if (string.IsNullOrEmpty(SettingsForm.Singleton.LegendPassword) || string.IsNullOrEmpty(SettingsForm.Singleton.Username))
                if (SettingsForm.Singleton.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return null;
            string hostPort = "5555";
            if (host == Region.Duster)
                hostPort = "5556";
            if (host == Region.DusterTest)
                hostPort = "5557";
            string args = string.Format("-pw {0} -L {3}:host{2}.aessuccess.org:4502 {1}@host{2}.aessuccess.org", 
                password, 
                SettingsForm.Singleton.Username, 
                ((int)host).ToString(),
                hostPort);
            var plinky = plinkVariations.FirstOrDefault(o => File.Exists(o));

            if (plinky == null)
            {
                MessageBox.Show("Unable to find PLINK.EXE at any of the following: " + string.Join(";", plinkVariations));
                return null;
            }
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(plinky, args)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            if (process.HasExited)
            {
                if (SettingsForm.Singleton.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    return OpenTunnel(host, password);
            }
            //ShowWindow(GetStdHandle(-11), ShowWindowCommands.Hide);
            return process;
        }

        public static bool OpenLegend()
        {
            return (LegendTunnel = OpenTunnel(Region.Legend, SettingsForm.Singleton.LegendPassword)) != null;
        }

        public static void CloseLegend()
        {
            if (!LegendTunnel.HasExited)
                LegendTunnel.Kill();
            LegendTunnel = null;
        }

        public static bool OpenDusterLive()
        {
            return (DusterLiveTunnel = OpenTunnel(Region.Duster, SettingsForm.Singleton.DusterLivePassword)) != null;
        }

        public static void CloseDusterLive()
        {
            if (!DusterLiveTunnel.HasExited)
                DusterLiveTunnel.Kill();
            DusterLiveTunnel = null;
        }

        public static bool OpenDusterTest()
        {
            return (DusterTestTunnel = OpenTunnel(Region.DusterTest, SettingsForm.Singleton.DusterLivePassword)) != null;
        }

        public static void CloseDusterTest()
        {
            if (!DusterTestTunnel.HasExited)
                DusterTestTunnel.Kill();
            DusterTestTunnel = null;
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
