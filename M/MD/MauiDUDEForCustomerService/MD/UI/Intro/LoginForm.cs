using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace MD
{
    public partial class LoginForm : BaseForm
    {
        const string TestLockFileName = "_test_force_token_remove_during_production";
        public LoginForm()
        {
            InitializeComponent();
            LoadQuickUtIdPanel();
            if (System.IO.File.Exists(TestLockFileName))
            {
                TestModeCheckButton.IsChecked = true;
                TestModeCheckButton.Enabled = false;
            }
            this.Async(() =>
            {
                try
                {
                    string warning = LoginHelper.GetLoginWarningMessage();
                    if (WarningLabel.Text != warning)
                        this.BeginInvoke(() => WarningLabel.Text = warning);
                }
                catch (COMException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            });
            this.Shown += (o, ea) =>
            {
                this.BringToFront();
                WinApiHelper.BringWindowToTop(this.Handle);
            };
        }

        public void DemoLogin(string username, bool testMode, string accountNumber)
        {
            if (testMode)
                ModeHelper.TestMode();
            else
                ModeHelper.LiveMode();
            var landing = Hlpr.UI.CreateForm<LandingForm>();
            if (Hlpr.Login.Login(this))
            {
                landing.Show();
                landing.LoadBorrower(accountNumber, true);
                return;
            }
        }

        bool quickMode;
        public void Initialize(bool quickMode)
        {
            this.quickMode = quickMode;
            if (quickMode)
            {
                WinApiHelper.ForceTopMost(this);
                this.Controls.Remove(CommonMenu);
                this.Load += (o, ea) => this.BringToFront();
                TestModeCheckButton.Enabled = false;
                if (Hlpr.Login.LoginMode == LoginMode.Calls)
                    LoginProcessingButton.Visible = false;
                else if (Hlpr.Login.LoginMode == LoginMode.Processing)
                    LoginCallsButton.Visible = false;
            }
        }

        private void LoadQuickUtIdPanel()
        {
            UtIdBox.TextBox.TextChanged += (o, ea) => RefreshLinks();
            QuickUtIdPanel.Controls.Clear();
            Color link = base.CurrentForeColor ?? Color.FromArgb(20, 20, 20);
            Color active = base.CurrentForeColor ?? Color.FromArgb(60, 60, 60);
            Color visited = base.CurrentForeColor ?? Color.FromArgb(20, 100, 20);
            foreach (string utid in UtIdHelper.CachedUtIds)
            {
                LinkLabel label = new LinkLabel()
                {
                    Text = utid,
                    LinkColor = link,
                    ActiveLinkColor = active,
                    VisitedLinkColor = visited,
                    LinkBehavior = LinkBehavior.HoverUnderline,
                    AutoSize = true,
                    Margin = new Padding(0),
                    TabStop = false
                };
                label.Click += (o, ea) =>
                {
                    UtIdBox.UtId = utid;
                    RefreshLinks();
                };
                QuickUtIdPanel.Controls.Add(label);
            }
            RefreshLinks();
        }

        protected override void ContrastApplied()
        {
            LoadQuickUtIdPanel();
        }

        private void RefreshLinks()
        {
            foreach (LinkLabel ll in QuickUtIdPanel.Controls.Cast<LinkLabel>())
                ll.LinkVisited = (ll.Text == UtIdBox.UtId);
        }

        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {
            SyncButtons();
        }

        private void UtIdBox_UtIdChanged(object sender, EventArgs e)
        {
            SyncButtons();
        }

        private void SyncButtons()
        {
            bool validLength = PasswordBox.Text.Length == 8;
            bool validId = UtIdBox.IsValid;
            LoginProcessingButton.Enabled = LoginCallsButton.Enabled = validLength && validId;
        }

        private void LoginCallsButton_Click(object sender, EventArgs e)
        {
            Hlpr.Login.LoginMode = LoginMode.Calls;
            Login();
        }

        private void LoginProcessingButton_Click(object sender, EventArgs e)
        {
            Hlpr.Login.LoginMode = LoginMode.Processing;
            Login();
        }

        private void Login()
        {
            if (TestModeCheckButton.IsChecked)
            {
                if (Dialog.Info.YesNo("Are you sure you want to log in to test mode?"))
                {
                    ModeHelper.TestMode();
                }
                else
                    return;
            }
            else
                ModeHelper.LiveMode();
            string username = UtIdBox.UtId;
            //string password = PasswordBox.Text; //Not used so removing to prevent from showing up as a heartbleed vulnerability
            if (Hlpr.Login.Login(this, this))
            {
                this.Hide();
                if (!quickMode)
                {
                    var landing = Hlpr.UI.CreateForm<LandingForm>();
                    landing.Show();
                }
                this.Close();
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}
