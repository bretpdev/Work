using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing.Drawing2D;

namespace SASM
{
    public partial class SasWindow : UserControl
    {
        private Process process;
        public Process Process
        {
            get
            {
                return process;
            }
        }
        public static List<SasWindow> AllWindows { get; internal set; }
        public SasWindow(Region region, Mode mode)
        {
            InitializeComponent();
            Start(region, mode);
            AllWindows.Add(this);
        }

        ~SasWindow()
        {
            AllWindows.Remove(this);
        }

        static SasWindow()
        {
            AllWindows = new List<SasWindow>();
        }

        private void Start(Region region, Mode mode)
        {
            string conn = "";
            string username = SettingsForm.Singleton.Username;
            string password = "";
            string title = "";
            if (region == SASM.Region.Legend)
            {
                password = SettingsForm.Singleton.LegendPassword;
                if (mode == Mode.Test)
                {
                    conn = "LegendTest.sas";
                    TitleLabel.Text = "LEGEND TEST";
                    title = "[T]LEGEND";
                }
                else if (mode == Mode.Production)
                {
                    conn = "LegendProd.sas";
                    TitleLabel.Text = "LEGEND LIVE";
                    title = "[L]LEGEND";
                }
            }
            else if (region == SASM.Region.Duster)
            {
                if (mode == Mode.Test)
                {
                    password = SettingsForm.Singleton.DusterTestPassword;
                    conn = "DusterTest.sas";
                    TitleLabel.Text = "DUSTER TEST";
                    title = "[T]DUSTER";
                }
                else if (mode == Mode.Production)
                {
                    password = SettingsForm.Singleton.DusterLivePassword;
                    conn = "DusterProd.sas";
                    title = "DUSTER LIVE";
                    TitleLabel.Text = "[L]DUSTER";
                }
            }

            string newTitle = title;
            int count = 1;
            while (AllWindows.Any(o => o.WindowName == newTitle))
            {
                count++;
                newTitle = title + " " + count;
            }
            title = newTitle;

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path, "Connections");
            path = Path.Combine(path, conn);
            string args = string.Format(@"-initstmt {0}%let USERID = '{2}';%let PASSWORD='{3}';%include '{1}';{0}", '"', path, username, password);

            process = new Process();
            process.StartInfo = new ProcessStartInfo(WindowHelper.SASDirectory, args);
            process.Start();
            Thread sensitive = new Thread(() =>
            {
                //wait until window is visible
                while (!IsWindowVisible(process.MainWindowHandle)) Thread.Sleep(1000);
                IntPtr closeButton = IntPtr.Zero;
                int milliWait = 10 * 1000; //try for ten seconds total;
                while (closeButton == IntPtr.Zero && milliWait > 0)
                {
                    closeButton = FindWindow("#32770 (Dialog)", "SAS 9.3");
                    Thread.Sleep(1000);
                    milliWait -= 1000;
                }
                if (closeButton != IntPtr.Zero)
                {
                    CloseWindow(closeButton);
                }
            });
            Pinvoker.FindChildWindow(process.MainWindowHandle);
            //setting caption before window is visible causes problems.  might as well wait as long as possible
            SetWindowText(process.MainWindowHandle, title);

            WindowNameText.Tag = true;
            WindowNameText.Text = title;
            WindowNameText.Tag = null;
        }

        private void WindowNameText_TextChanged(object sender, EventArgs e)
        {
            if (WindowNameText.Tag == null)
            {
                OkButton.Visible = CancelButton.Visible = true;
                this.Invalidate();
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            OkButton.Visible = CancelButton.Visible = false;
            SetWindowText(process.MainWindowHandle, WindowNameText.Text);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            OkButton.Visible = CancelButton.Visible = false;
            StringBuilder sb = new StringBuilder();
            GetWindowText(process.MainWindowHandle, sb, 5000);
            WindowNameText.Tag = true;
            WindowNameText.Text = sb.ToString();
            WindowNameText.Tag = null;
        }

        public string WindowName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                GetWindowText(process.MainWindowHandle, sb, 5000);
                return sb.ToString();
            }
        }

        public void Close()
        {
            if (process != null && !process.HasExited)
                process.Kill();
            this.Parent.Controls.Remove(this);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int CloseWindow(IntPtr hWnd);

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(SystemColors.ControlDark), e.ClipRectangle);
        }
        private RoundedRectangle rr;
        private void SasWindow_Paint(object sender, PaintEventArgs e)
        {
            if (rr == null)
                rr = new RoundedRectangle(3, this.DisplayRectangle);
            else
                rr.Load(this.DisplayRectangle);

            rr.Draw(e.Graphics, this.BackColor);
        }

        private class RoundedRectangle
        {
            public GraphicsPath Path { get; set; }
            private Rectangle Rect { get; set; }
            private int radius;
            private static Pen borderColor = new Pen(Color.FromArgb(96, 96, 96), 2);
            public RoundedRectangle(int radius, Rectangle rect)
            {
                this.radius = radius;
                Load(rect);
            }

            public void Draw(Graphics g, Color backColor)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.FillPath(new SolidBrush(backColor), Path);
                g.DrawPath(borderColor, Path);
            }

            public void Load(Rectangle rect)
            {
                if (Rect != null && Rect.Equals(rect))
                    return;
                Rect = rect;
                rect = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                var rad2 = radius * 2;
                Path = new GraphicsPath();
                Path.StartFigure();
                Path.AddArc(0, 0, rad2, rad2, 180, 90);
                Path.AddLine(radius, 0, rect.Width - radius, 0);
                Path.AddArc(rect.Width - rad2, 0, rad2, rad2, 270, 90);
                Path.AddLine(rect.Width, radius, rect.Width, rect.Height - radius);
                Path.AddArc(rect.Width - rad2, rect.Height - rad2, rad2, rad2, 0, 90);
                Path.AddLine(rect.Width - radius, rect.Height, radius, rect.Height);
                Path.AddArc(0, rect.Height - rad2, rad2, rad2, 90, 90);
                Path.AddLine(0, rect.Height - radius, 0, radius);
                Path.CloseFigure();
            }
        }

    }
}
