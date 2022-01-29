using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SASM
{
    public partial class MainForm : Form
    {
        Properties.Settings settings = Properties.Settings.Default;
        public MainForm()
        {
            InitializeComponent();
        }

        private bool ToggleLegendTunnel()
        {
            if (WindowHelper.LegendTunnel == null)
            {
                if (WindowHelper.OpenLegend())
                {
                    Thread.Sleep(1000);
                    LegendButton.Image = Properties.Resources.Actions_dialog_ok_apply_icon_large;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                WindowHelper.CloseLegend();
                LegendButton.Image = Properties.Resources.Actions_media_playback_start_icon;
            }
            return true;
        }

        private bool ToggleDusterLiveTunnel()
        {
            if (WindowHelper.DusterLiveTunnel == null)
            {
                if (WindowHelper.OpenDusterLive())
                {
                    Thread.Sleep(1000);
                    DusterLiveButton.Image = Properties.Resources.Actions_dialog_ok_apply_icon_large;

                    return true;
                }
                else
                    return false;
            }
            else
            {
                WindowHelper.CloseDusterLive();
                DusterLiveButton.Image = Properties.Resources.Actions_media_playback_start_icon;
            }
            return true;
        }

        private bool ToggleDusterTestTunnel()
        {
            if (WindowHelper.DusterTestTunnel == null)
            {
                if (WindowHelper.OpenDusterTest())
                {
                    Thread.Sleep(1000);
                    DusterTestButton.Image = Properties.Resources.Actions_dialog_ok_apply_icon_large;

                    return true;
                }
                else
                    return false;
            }
            else
            {
                WindowHelper.CloseDusterTest();
                DusterTestButton.Image = Properties.Resources.Actions_media_playback_start_icon;
            }
            return true;
        }

        private void LegendButton_Click(object sender, EventArgs e)
        {
            ToggleLegendTunnel();
        }

        private void DusterButton_Click(object sender, EventArgs e)
        {
            ToggleDusterLiveTunnel();
        }

        private void DusterTestButton_Click(object sender, EventArgs e)
        {
            ToggleDusterTestTunnel();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm.Singleton.ShowDialog(this);
        }

        private void AddWindowButton_Click(object sender, EventArgs e)
        {
            var pos = Cursor.Position;
            //MessageBox.Show(pos.X + "," + pos.Y);
            AddMenu.Show(pos);
        }

        private void AddLegendTest()
        {
            if (WindowHelper.LegendTunnel == null)
                ToggleLegendTunnel();
            SasWindow win = new SasWindow(SASM.Region.Legend, Mode.Test);
            SasWindowsPanel.Controls.Add(win);
        }

        private void AddLegendLive()
        {
            if (WindowHelper.LegendTunnel == null)
                ToggleLegendTunnel();
            SasWindow win = new SasWindow(SASM.Region.Legend, Mode.Production);
            SasWindowsPanel.Controls.Add(win);
        }

        private void AddDusterLive()
        {
            if (WindowHelper.DusterLiveTunnel == null)
                ToggleDusterLiveTunnel();
            SasWindow win = new SasWindow(SASM.Region.Duster, Mode.Production);
            SasWindowsPanel.Controls.Add(win);
        }

        private void AddDusterTest()
        {
            if (WindowHelper.DusterLiveTunnel == null)
                ToggleDusterLiveTunnel();
            SasWindow win = new SasWindow(SASM.Region.Duster, Mode.Production);
            SasWindowsPanel.Controls.Add(win);
        }

        private void LegendTestMenu_Click(object sender, EventArgs e)
        {
            AddLegendTest();
        }

        private void LegendLiveMenu_Click(object sender, EventArgs e)
        {
            AddLegendLive();
        }

        private void DusterMenu_Click(object sender, EventArgs e)
        {
            AddDusterLive();
        }

        private void DusterTestMenu_Click(object sender, EventArgs e)
        {
            AddDusterTest();
        }

        private void AllMenu_Click(object sender, EventArgs e)
        {
            AddLegendLive();
            AddLegendTest();
            AddDusterLive();
            AddDusterLive();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            //    TrayIcon.ShowBalloonTip(7000, "SASM", string.Join(Environment.NewLine, message), ToolTipIcon.Info);

            //This menu shows at the top-left corner of the screen the first time, regardless of what position you give it.
            //showing and hiding it here fixes this problem.
            AddMenu.Show();
            AddMenu.Hide();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.ShowInTaskbar = this.WindowState != FormWindowState.Minimized;
        }

        private void ShowMenu_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SasWindowsPanel.Controls.Count > 0)
            {
                var result = MessageBox.Show("Would you like to automatically close all remaining SAS windows?", "Close Windows?", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (result == DialogResult.Yes)
                {
                    foreach (SasWindow sas in SasWindowsPanel.Controls)
                        sas.Close();
                }
            }
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
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
        public void SyncTray()
        {
            var temp = "temp";
            var tempItems = TrayMenu.Items.Cast<ToolStripItem>().Where(t => (string)t.Tag == temp).ToArray();
            foreach (var item in tempItems)
                TrayMenu.Items.Remove(item);
            TrayMenu.Items.Add(new ToolStripSeparator() { Tag = temp });
            TrayMenu.Items.Add(new ToolStripLabel() { Text = "SAS Windows", Tag = temp, Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold) });
            foreach (var window in SasWindow.AllWindows)
            {
                var item = new ToolStripMenuItem() { Text = window.WindowName, Tag = temp, Margin = new Padding(10, 0, 0, 0) };
                item.Click += (s, ea) =>
                {
                    ShowWindow(window.Process.MainWindowHandle, ShowWindowCommands.Show);
                    SetForegroundWindow(window.Process.MainWindowHandle);
                };
                TrayMenu.Items.Add(item);
            }
            TrayMenu.Invalidate();
        }

        private void SasWindowsPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            SyncTray();
        }

        private void SasWindowsPanel_ControlRemoved(object sender, ControlEventArgs e)
        {
            SyncTray();
        }


    }
}
