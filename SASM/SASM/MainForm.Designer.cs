namespace SASM
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.SasWindowsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DusterLiveButton = new System.Windows.Forms.Button();
            this.LegendButton = new System.Windows.Forms.Button();
            this.AddWindowButton = new System.Windows.Forms.Button();
            this.AddMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.LegendTestMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.LegendLiveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.DusterLiveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AllMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AddItemMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.DusterTestButton = new System.Windows.Forms.Button();
            this.DusterTestMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.AddMenu.SuspendLayout();
            this.TrayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SasWindowsPanel
            // 
            this.SasWindowsPanel.AutoScroll = true;
            this.SasWindowsPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.SasWindowsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SasWindowsPanel.Location = new System.Drawing.Point(3, 16);
            this.SasWindowsPanel.Name = "SasWindowsPanel";
            this.SasWindowsPanel.Size = new System.Drawing.Size(487, 155);
            this.SasWindowsPanel.TabIndex = 0;
            this.SasWindowsPanel.WrapContents = false;
            this.SasWindowsPanel.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.SasWindowsPanel_ControlAdded);
            this.SasWindowsPanel.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.SasWindowsPanel_ControlRemoved);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.SasWindowsPanel);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(493, 174);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SAS Windows";
            // 
            // DusterLiveButton
            // 
            this.DusterLiveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DusterLiveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DusterLiveButton.Image = global::SASM.Properties.Resources.Actions_media_playback_start_icon;
            this.DusterLiveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DusterLiveButton.Location = new System.Drawing.Point(146, 191);
            this.DusterLiveButton.Name = "DusterLiveButton";
            this.DusterLiveButton.Size = new System.Drawing.Size(128, 43);
            this.DusterLiveButton.TabIndex = 4;
            this.DusterLiveButton.Text = "Duster";
            this.DusterLiveButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DusterLiveButton.UseVisualStyleBackColor = true;
            this.DusterLiveButton.Click += new System.EventHandler(this.DusterButton_Click);
            // 
            // LegendButton
            // 
            this.LegendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LegendButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LegendButton.Image = global::SASM.Properties.Resources.Actions_media_playback_start_icon;
            this.LegendButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LegendButton.Location = new System.Drawing.Point(12, 191);
            this.LegendButton.Name = "LegendButton";
            this.LegendButton.Size = new System.Drawing.Size(128, 43);
            this.LegendButton.TabIndex = 3;
            this.LegendButton.Text = "Legend";
            this.LegendButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LegendButton.UseVisualStyleBackColor = true;
            this.LegendButton.Click += new System.EventHandler(this.LegendButton_Click);
            // 
            // AddWindowButton
            // 
            this.AddWindowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddWindowButton.ContextMenuStrip = this.AddMenu;
            this.AddWindowButton.Image = global::SASM.Properties.Resources.Actions_list_add_icon;
            this.AddWindowButton.Location = new System.Drawing.Point(463, 192);
            this.AddWindowButton.Name = "AddWindowButton";
            this.AddWindowButton.Size = new System.Drawing.Size(42, 43);
            this.AddWindowButton.TabIndex = 2;
            this.AddWindowButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AddWindowButton.UseVisualStyleBackColor = true;
            this.AddWindowButton.Click += new System.EventHandler(this.AddWindowButton_Click);
            // 
            // AddMenu
            // 
            this.AddMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LegendTestMenu,
            this.LegendLiveMenu,
            this.DusterLiveMenu,
            this.DusterTestMenu,
            this.AllMenu});
            this.AddMenu.Name = "contextMenuStrip1";
            this.AddMenu.Size = new System.Drawing.Size(153, 136);
            // 
            // LegendTestMenu
            // 
            this.LegendTestMenu.Name = "LegendTestMenu";
            this.LegendTestMenu.Size = new System.Drawing.Size(152, 22);
            this.LegendTestMenu.Text = "Legend TEST";
            this.LegendTestMenu.Click += new System.EventHandler(this.LegendTestMenu_Click);
            // 
            // LegendLiveMenu
            // 
            this.LegendLiveMenu.Name = "LegendLiveMenu";
            this.LegendLiveMenu.Size = new System.Drawing.Size(152, 22);
            this.LegendLiveMenu.Text = "Legend LIVE";
            this.LegendLiveMenu.Click += new System.EventHandler(this.LegendLiveMenu_Click);
            // 
            // DusterLiveMenu
            // 
            this.DusterLiveMenu.Name = "DusterLiveMenu";
            this.DusterLiveMenu.Size = new System.Drawing.Size(152, 22);
            this.DusterLiveMenu.Text = "Duster LIVE";
            this.DusterLiveMenu.Click += new System.EventHandler(this.DusterMenu_Click);
            // 
            // AllMenu
            // 
            this.AllMenu.Name = "AllMenu";
            this.AllMenu.Size = new System.Drawing.Size(152, 22);
            this.AllMenu.Text = "[ALL]";
            this.AllMenu.Click += new System.EventHandler(this.AllMenu_Click);
            // 
            // AddItemMenu
            // 
            this.AddItemMenu.DropDown = this.AddMenu;
            this.AddItemMenu.Name = "AddItemMenu";
            this.AddItemMenu.Size = new System.Drawing.Size(103, 22);
            this.AddItemMenu.Text = "&Add";
            // 
            // SettingsButton
            // 
            this.SettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsButton.Image = global::SASM.Properties.Resources.Categories_applications_system_icon;
            this.SettingsButton.Location = new System.Drawing.Point(415, 191);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(42, 43);
            this.SettingsButton.TabIndex = 5;
            this.SettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SettingsButton.UseVisualStyleBackColor = true;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // TrayIcon
            // 
            this.TrayIcon.ContextMenuStrip = this.TrayMenu;
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "SASM";
            this.TrayIcon.Visible = true;
            this.TrayIcon.DoubleClick += new System.EventHandler(this.TrayIcon_DoubleClick);
            // 
            // TrayMenu
            // 
            this.TrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowMenu,
            this.AddItemMenu});
            this.TrayMenu.Name = "TrayMenu";
            this.TrayMenu.Size = new System.Drawing.Size(104, 48);
            // 
            // ShowMenu
            // 
            this.ShowMenu.Name = "ShowMenu";
            this.ShowMenu.Size = new System.Drawing.Size(103, 22);
            this.ShowMenu.Text = "&Show";
            this.ShowMenu.Click += new System.EventHandler(this.ShowMenu_Click);
            // 
            // DusterTestButton
            // 
            this.DusterTestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DusterTestButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DusterTestButton.Image = global::SASM.Properties.Resources.Actions_media_playback_start_icon;
            this.DusterTestButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DusterTestButton.Location = new System.Drawing.Point(280, 192);
            this.DusterTestButton.Name = "DusterTestButton";
            this.DusterTestButton.Size = new System.Drawing.Size(128, 43);
            this.DusterTestButton.TabIndex = 6;
            this.DusterTestButton.Text = "Duster Test";
            this.DusterTestButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DusterTestButton.UseVisualStyleBackColor = true;
            this.DusterTestButton.Click += new System.EventHandler(this.DusterTestButton_Click);
            // 
            // DusterTestMenu
            // 
            this.DusterTestMenu.Name = "DusterTestMenu";
            this.DusterTestMenu.Size = new System.Drawing.Size(152, 22);
            this.DusterTestMenu.Text = "Duster TEST";
            this.DusterTestMenu.Click += new System.EventHandler(this.DusterTestMenu_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 244);
            this.Controls.Add(this.DusterTestButton);
            this.Controls.Add(this.DusterLiveButton);
            this.Controls.Add(this.SettingsButton);
            this.Controls.Add(this.LegendButton);
            this.Controls.Add(this.AddWindowButton);
            this.Controls.Add(this.groupBox1);
            this.MaximumSize = new System.Drawing.Size(19200, 282);
            this.MinimumSize = new System.Drawing.Size(393, 282);
            this.Name = "MainForm";
            this.Text = "SAS Manager";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.groupBox1.ResumeLayout(false);
            this.AddMenu.ResumeLayout(false);
            this.TrayMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel SasWindowsPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button AddWindowButton;
        private System.Windows.Forms.Button LegendButton;
        private System.Windows.Forms.Button DusterLiveButton;
        private System.Windows.Forms.Button SettingsButton;
        private System.Windows.Forms.ContextMenuStrip AddMenu;
        private System.Windows.Forms.ToolStripMenuItem LegendTestMenu;
        private System.Windows.Forms.ToolStripMenuItem LegendLiveMenu;
        private System.Windows.Forms.ToolStripMenuItem DusterLiveMenu;
        private System.Windows.Forms.ToolStripMenuItem AllMenu;
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenuStrip TrayMenu;
        private System.Windows.Forms.ToolStripMenuItem ShowMenu;
        private System.Windows.Forms.ToolStripMenuItem AddItemMenu;
        private System.Windows.Forms.Button DusterTestButton;
        private System.Windows.Forms.ToolStripMenuItem DusterTestMenu;
    }
}

