namespace Uheaa.Common.Baa
{
    partial class UI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.SaveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScreenshotEnterMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ScreenshotPutTextMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ClosingMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.LogBrowser = new System.Windows.Forms.WebBrowser();
            this.ClosingTimer = new System.Windows.Forms.Timer(this.components);
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveMenu,
            this.settingsToolStripMenuItem,
            this.ClosingMenu});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1118, 24);
            this.MainMenu.TabIndex = 1;
            this.MainMenu.Text = "menuStrip1";
            // 
            // SaveMenu
            // 
            this.SaveMenu.Name = "SaveMenu";
            this.SaveMenu.Size = new System.Drawing.Size(66, 20);
            this.SaveMenu.Text = "&Save Log";
            this.SaveMenu.Click += new System.EventHandler(this.SaveMenu_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScreenshotEnterMenu,
            this.ScreenshotPutTextMenu});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "S&ettings";
            // 
            // ScreenshotEnterMenu
            // 
            this.ScreenshotEnterMenu.Name = "ScreenshotEnterMenu";
            this.ScreenshotEnterMenu.Size = new System.Drawing.Size(238, 22);
            this.ScreenshotEnterMenu.Text = "Take screenshot after ENTER";
            // 
            // ScreenshotPutTextMenu
            // 
            this.ScreenshotPutTextMenu.Name = "ScreenshotPutTextMenu";
            this.ScreenshotPutTextMenu.Size = new System.Drawing.Size(238, 22);
            this.ScreenshotPutTextMenu.Text = "Take screenshot after PUTTEXT";
            // 
            // ClosingMenu
            // 
            this.ClosingMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ClosingMenu.Name = "ClosingMenu";
            this.ClosingMenu.Size = new System.Drawing.Size(142, 20);
            this.ClosingMenu.Text = "Closing in 60 seconds...";
            this.ClosingMenu.Visible = false;
            this.ClosingMenu.Click += new System.EventHandler(this.ClosingMenu_Click);
            // 
            // TrayIcon
            // 
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "BAA - {0}";
            this.TrayIcon.Visible = true;
            this.TrayIcon.Click += new System.EventHandler(this.TrayIcon_Click);
            // 
            // LogBrowser
            // 
            this.LogBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogBrowser.Location = new System.Drawing.Point(0, 24);
            this.LogBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.LogBrowser.Name = "LogBrowser";
            this.LogBrowser.Size = new System.Drawing.Size(1118, 706);
            this.LogBrowser.TabIndex = 2;
            // 
            // ClosingTimer
            // 
            this.ClosingTimer.Enabled = true;
            this.ClosingTimer.Interval = 500;
            this.ClosingTimer.Tick += new System.EventHandler(this.ClosingTimer_Tick);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 730);
            this.Controls.Add(this.LogBrowser);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.Name = "UI";
            this.Text = "{0} - Business Analyst Assistant";
            this.Shown += new System.EventHandler(this.UI_Shown);
            this.Resize += new System.EventHandler(this.UI_Resize);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.WebBrowser LogBrowser;
        private System.Windows.Forms.ToolStripMenuItem SaveMenu;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ScreenshotEnterMenu;
        private System.Windows.Forms.ToolStripMenuItem ScreenshotPutTextMenu;
        private System.Windows.Forms.ToolStripMenuItem ClosingMenu;
        private System.Windows.Forms.Timer ClosingTimer;
    }
}