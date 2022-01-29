using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxR8AXCTRLLib;
using Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace MdSession
{
    public partial class SessionForm : Form
    {
        public AxRibmCtrl SessionControl { get; private set; }
        public ColorGrid ColorGridControl { get; set; }
        public Session ReflectionSession { get; private set; }
        public SessionForm(string oleGuid, string ordinal, string region, string mode)
        {
            InitializeComponent();

            this.Text = "MD Session";
            if (ordinal != "0")
                this.Text += " (" + ordinal + ")";

            DataAccessHelper.CurrentRegion = (DataAccessHelper.Region)Enum.Parse(typeof(DataAccessHelper.Region), region, true);
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)Enum.Parse(typeof(DataAccessHelper.Mode), mode, true);

            SessionControl = new AxR8AXCTRLLib.AxRibmCtrl();
            SessionControl.Dock = DockStyle.Fill;
            ReflectionPanel.Controls.Add(SessionControl);

            LoadBackgroundFunctionality();
            LoadScriptFunctionality();


            this.Shown += (o, ea) =>
            {
                ReflectionSession = ((Reflection.Session)SessionControl.GetActiveSession());
                ReflectionSession.OLEServerName = oleGuid;
                InitSession();
                BackgroundChange(ColorCodes.GetByReflectionValue((Reflection.Constants)Properties.Settings.Default.BackgroundColor));
            };
        }

        private void InitSession()
        {
            var session = ReflectionSession;
            session.ToolBarVisible = 0;
            session.Hostname = "hera.aessuccess.org";
            //re-map certain colors to match the rest of the sessions.
            session.SetColorMap((int)Reflection.Constants.rcProtNormNum, (int)Reflection.Constants.rcCyan, (int)Reflection.Constants.rcBlack);
            session.SetColorMap((int)Reflection.Constants.rcUnprotNormAlpha, (int)Reflection.Constants.rcGreen, (int)Reflection.Constants.rcBlack);
            session.SetColorMap((int)Reflection.Constants.rcProtNormAlpha, (int)Reflection.Constants.rcGrey, (int)Reflection.Constants.rcBlack);
            session.SubstituteDisplayChars = 1;
            session.TelnetEncryptionDisableCRLCheck = 1;
            session.TelnetPort = 1022;
            session.TLSSSLVersion = 12;
            session.TelnetEncryptionValidateCertChain = 0;
            session.TelnetEncryption = 1;
            session.TelnetEncryptionVerifyHostName = 0;
            session.BDTIgnoreScrollLock = 1;
        }

        #region Copy
        private void CopyMenuBarItem_Click(object sender, EventArgs e)
        {
            Copy();
        }
        private void CopyToolBarItem_Click(object sender, EventArgs e)
        {
            Copy();
        }
        private void Copy()
        {
            try
            {
                ReflectionSession.CopySelection();
            }
            catch (Exception)
            {
                //throws an exception if nothing was selected.
            }
        }
        #endregion
        #region Paste
        private void PasteMenuBarItem_Click(object sender, EventArgs e)
        {
            Paste();
        }
        private void PasteToolBarItem_Click(object sender, EventArgs e)
        {
            Paste();
        }
        private void Paste()
        {
            try
            {
                ReflectionSession.Paste();
            }
            catch (Exception)
            {
                //eat any bad paste exception (for instance, trying to paste an image to the session)
            }
        }
        #endregion
        #region Print
        private void PrintMenuBarItem_Click(object sender, EventArgs e)
        {
            Print();
        }
        private void PrintToolBarItem_Click(object sender, EventArgs e)
        {
            Print();
        }
        private void Print()
        {
            try
            {
                ReflectionSession.PrintDlg();
            }
            catch (Exception)
            {
                //PrintDlg throws an exception if the user cancels.  Seriously.
            }
        }
        #endregion
        #region Connect/Disconnect
        private void ReconnectMenuBarItem_Click(object sender, EventArgs e)
        {
            Reconnect();
        }
        private void ReconnectToolBarItem_Click(object sender, EventArgs e)
        {
            Reconnect();
        }
        private void Reconnect()
        {
            if (ReflectionSession.Connected == 0)
                ReflectionSession.Connect();
            else
                ReflectionSession.Disconnect();
            GenerateReconnectText();
        }
        private void GenerateReconnectText()
        {
            if (ReflectionSession != null)
                if (ReflectionSession.Connected == -1)
                {
                    ReconnectMenuBarItem.Text = "Disconnect";
                    ReconnectToolBarItem.Image = Properties.Resources.reconnect;
                }
                else
                {
                    ReconnectMenuBarItem.Text = "Connect";
                    ReconnectToolBarItem.Image = Properties.Resources.disconnected;
                }
        }
        private void ReconnectTimer_Tick(object sender, EventArgs e)
        {
            GenerateReconnectText();
        }
        #endregion
        #region Password Reset
        private void PasswordResetMenuBarItem_Click(object sender, EventArgs e)
        {
            PasswordReset();
        }
        private void PasswordResetToolBarItem_Click(object sender, EventArgs e)
        {
            PasswordReset();
        }
        private void PasswordReset()
        {
            Proc.Start("AesPasswordReset");
        }
        #endregion
        #region Background Change
        private void LoadBackgroundFunctionality()
        {
            ColorGridControl = new ColorGrid();
            ColorGridControl.ColorSelected += (o, c) =>
            {
                BackgroundChange(c);
                BackgroundToolBarItem.DropDown.Close();
            };
            BackgroundToolBarItem.DropDownItems.Add(new ToolStripControlHost(ColorGridControl));

            foreach (ColorCode color in ColorCodes.Codes)
            {
                var item = BackgroundColorMenuBarItem.DropDownItems.Add(color.FriendlyName);
                item.Tag = color;
                item.Click += (o, ea) => BackgroundChange(color);
            }
        }
        private void BackgroundChange(ColorCode color)
        {
            ReflectionSession.BackgndColor = (int)color.ReflectionValue;
            Properties.Settings.Default.BackgroundColor = (int)color.ReflectionValue;
            Properties.Settings.Default.Save();

            Image magenta = new Bitmap(Properties.Resources.magenta);
            using (Graphics g = Graphics.FromImage(magenta))
            {
                Brush b = new SolidBrush(color.Color);
                //image has 1-pixel border
                g.FillRectangle(b, new Rectangle(1, 1, magenta.Width - 2, magenta.Height - 2));
            }
            BackgroundToolBarItem.Image = magenta;

            foreach (ToolStripMenuItem dropdown in BackgroundColorMenuBarItem.DropDownItems)
                dropdown.Checked = dropdown.Tag == color;
        }
        private void BackgroundToolBarItem_Click(object sender, EventArgs e)
        {
            ColorCode code = ColorCodes.GetByReflectionValue((Reflection.Constants)ReflectionSession.BackgndColor);
            int index = ColorCodes.Codes.Select((o, i) => o == code ? i : -1).Max();
            index++;
            if (index >= ColorCodes.Codes.Count)
                index = 0;
            BackgroundChange(ColorCodes.Codes[index]);
        }
        #endregion
        #region Scripts
        public void LoadScriptFunctionality()
        {
            foreach (ScriptInfo si in MdScripts.Scripts)
            {
                var menuItem = ScriptsMenuBarItem.DropDownItems.Add(si.ScriptName);
                menuItem.Click += (o, ea) => si.LaunchScript(ReflectionSession);

                var toolItem = ToolBar.Items.Add(si.Icon);
                toolItem.ImageScaling = ToolStripItemImageScaling.None;
                toolItem.AutoSize = false;

                toolItem.ToolTipText = si.ScriptName;
                toolItem.Click += (o, ea) => si.LaunchScript(ReflectionSession);
            }
        }
        #endregion

        private void CommandTimer_Tick(object sender, EventArgs e)
        {
            if (ReflectionSession == null) return;
            try
            {
                switch (ReflectionSession.Caption)
                {
                    case "CMD:KILL":
                        this.Close();
                        break;
                    case "CMD:TESTMODE":
                        this.Text = this.Text.Replace("[TEST MODE] ", "");
                        ModeHelper.TestMode = true;
                        this.Text = "[TEST MODE] " + this.Text;
                        break;
                    case "CMD:LIVEMODE":
                        this.Text = this.Text.Replace("[TEST MODE] ", "");
                        ModeHelper.LiveMode = true;
                        break;
                    default:
                        return;
                }
                ReflectionSession.Caption = "";

            }
            catch (Exception ex)
            {
                //issue initially accessing the session, wait until next time
            }
        }

        private void SessionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Marshal.ReleaseComObject(ReflectionSession);
        }
    }
}
