using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uheaa.Common.Baa
{
    public partial class UI : Form
    {
        BaaReflectionInterface ri;
        ReflectionLog log;
        HtmlDocument doc;
        ReflectionLogSettings settings;
        string scriptId = "";
        public UI(ReflectionLog log, string scriptId, ReflectionLogSettings settings, BaaReflectionInterface ri)
        {
            InitializeComponent();
            this.scriptId = scriptId;
            this.settings = settings;
            this.log = log;
            this.ri = ri;
            this.log.EventLogged += e =>
            {
                if (IsHandleCreated)
                    LogEvent(e);
                else
                    waitingEvents.Add(e);
            };
            this.log.GetHtmlText = () =>
            {
                return this.doc.GetElementsByTagName("html")[0].OuterHtml;
            };
            BindBox(ScreenshotEnterMenu, l => l.ScreenshotAfterEnter);
            BindBox(ScreenshotPutTextMenu, l => l.ScreenshotAfterPutText);
            LogBrowser.DocumentCompleted += (o, ea) =>
            {
                this.doc = LogBrowser.Document;
                this.doc.Write(writeCache);
                writeCache = null;
            };
            LogBrowser.Navigate("about:blank");
            this.doc = LogBrowser.Document;
            Write("<h1>" + DateTime.Now.ToShortDateString() + " - Started Script " + scriptId + "</h1>");

            this.Text = string.Format(this.Text, scriptId);
            TrayIcon.Text = string.Format(TrayIcon.Text, scriptId);
            TrayIcon.ShowBalloonTip(1000, scriptId + " - BAA", "Started Script " + scriptId, ToolTipIcon.None);
        }

        private void BindBox(ToolStripMenuItem menu, Expression<Func<ReflectionLogSettings, bool>> setting)
        {
            menu.Checked = setting.Compile().Invoke(settings);
            menu.Click += (o, ea) =>
            {
                menu.Checked = !menu.Checked;
                var prop = (setting.Body as MemberExpression).Member as PropertyInfo;
                prop.SetValue(settings, menu.Checked);
            };
        }

        string writeCache = "";
        private void Write(string text)
        {
            if (this.doc == null)
                writeCache += text;
            else
                this.doc.Write(text);
            if (doc.Body.All.Count > 0)
                doc.Body.All[doc.Body.All.Count - 1].ScrollIntoView(false);
        }

        List<ReflectionLog.LogEvent> waitingEvents = new List<ReflectionLog.LogEvent>();
        DateTime lastLoggedEvent = DateTime.Now;
        private void LogEvent(ReflectionLog.LogEvent e)
        {
            this.BeginInvoke(new Action(() =>
            {
                Write("<br />");
                if (e is ReflectionLog.TextLogEvent)
                {
                    Write(e.Timestamp.ToLongTimeString() + " - " + (e as ReflectionLog.TextLogEvent).Text);
                }
                else if (e is ReflectionLog.ImageLogEvent)
                {
                    var img = (e as ReflectionLog.ImageLogEvent).Image;
                    if (img != null)
                    {
                        Write("<br />");
                        Write(e.Timestamp.ToLongTimeString());
                        Write("<br />");
                        Write(ImageHelper.GetHtmlImage(img));
                        Write("<br />");
                    }
                }
            }));
            lastLoggedEvent = DateTime.Now;
        }

        bool startingUp = true;
        protected override void SetVisibleCore(bool value)
        {
            if (startingUp)
            {
                base.SetVisibleCore(false);
                startingUp = false;
            }
            else
                base.SetVisibleCore(value);
        }

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            if (this.Visible)
                this.Hide();
            else
            {
                this.Show();
                this.BringToFront();
            }
        }

        private void UI_Shown(object sender, EventArgs e)
        {
            foreach (var ev in waitingEvents.ToList())
            {
                LogEvent(ev);
                waitingEvents.Remove(ev);
            }
        }

        private void SaveMenu_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog() { Filter = "HTML|*.htm" })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(dialog.FileName, LogBrowser.DocumentText);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to save file. " + ex.ToString());
                    }
                }
            }
        }

        DateTime? closeTime = null;
        const int timeoutSeconds = 60; //1 minute wait
        const int closeDelaySeconds = 10;
        private void ClosingTimer_Tick(object sender, EventArgs e)
        {
            if (closeTime != null)
            {
                int seconds = (int)(closeTime.Value - DateTime.Now).TotalSeconds;
                ClosingMenu.Text = "Closing in " + (timeoutSeconds - seconds) + " seconds...";
                if (closeTime <= DateTime.Now)
                {
                    ri.WriteToDisk();
                    Application.Exit();
                }
            }
            else
            {
                var elapsedSeconds = (DateTime.Now - lastLoggedEvent).TotalSeconds;
                if (elapsedSeconds > timeoutSeconds)
                {
                    TrayIcon.ShowBalloonTip(1000, scriptId + " - BAA", "No script activity in the last " + (int)elapsedSeconds + " seconds, BAA closing in " + closeDelaySeconds + ".", ToolTipIcon.None);
                    closeTime = DateTime.Now.AddSeconds(closeDelaySeconds);
                    ClosingMenu.Visible = true;
                }
                else
                {
                    ClosingMenu.Visible = false;
                    closeTime = null;
                }
            }
        }

        private void ClosingMenu_Click(object sender, EventArgs e)
        {
            closeTime = null;
            lastLoggedEvent = DateTime.Now;
            ClosingTimer.Enabled = false; //don't try to auto-close again
            ClosingMenu.Visible = false;
        }

        private void UI_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Visible = false;
        }
    }
}
