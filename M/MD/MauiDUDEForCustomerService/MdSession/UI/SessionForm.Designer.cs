namespace MdSession
{
    partial class SessionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionForm));
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.ReflectionPanel = new System.Windows.Forms.Panel();
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyMenuBarItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteMenuBarItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PrintMenuBarItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReconnectMenuBarItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PasswordResetMenuBarItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BackgroundColorMenuBarItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScriptsMenuBarItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReconnectTimer = new System.Windows.Forms.Timer(this.components);
            this.CommandTimer = new System.Windows.Forms.Timer(this.components);
            this.ToolBarPanel = new System.Windows.Forms.Panel();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.PrintToolBarItem = new System.Windows.Forms.ToolStripButton();
            this.CopyToolBarItem = new System.Windows.Forms.ToolStripButton();
            this.PasteToolBarItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ReconnectToolBarItem = new System.Windows.Forms.ToolStripButton();
            this.PasswordResetToolBarItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.BackgroundToolBarItem = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuBar.SuspendLayout();
            this.ToolBarPanel.SuspendLayout();
            this.ToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.AutoScroll = true;
            this.ContentPanel.Size = new System.Drawing.Size(691, 417);
            // 
            // ReflectionPanel
            // 
            this.ReflectionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReflectionPanel.Location = new System.Drawing.Point(0, 47);
            this.ReflectionPanel.Name = "ReflectionPanel";
            this.ReflectionPanel.Size = new System.Drawing.Size(816, 434);
            this.ReflectionPanel.TabIndex = 1;
            // 
            // MenuBar
            // 
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.connectionToolStripMenuItem,
            this.BackgroundColorMenuBarItem,
            this.ScriptsMenuBarItem});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MenuBar.Size = new System.Drawing.Size(816, 24);
            this.MenuBar.TabIndex = 2;
            this.MenuBar.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyMenuBarItem,
            this.PasteMenuBarItem,
            this.PrintMenuBarItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.fileToolStripMenuItem.Text = "&Edit";
            // 
            // CopyMenuBarItem
            // 
            this.CopyMenuBarItem.Name = "CopyMenuBarItem";
            this.CopyMenuBarItem.Size = new System.Drawing.Size(102, 22);
            this.CopyMenuBarItem.Text = "&Copy";
            this.CopyMenuBarItem.Click += new System.EventHandler(this.CopyMenuBarItem_Click);
            // 
            // PasteMenuBarItem
            // 
            this.PasteMenuBarItem.Name = "PasteMenuBarItem";
            this.PasteMenuBarItem.Size = new System.Drawing.Size(102, 22);
            this.PasteMenuBarItem.Text = "&Paste";
            this.PasteMenuBarItem.Click += new System.EventHandler(this.PasteMenuBarItem_Click);
            // 
            // PrintMenuBarItem
            // 
            this.PrintMenuBarItem.Name = "PrintMenuBarItem";
            this.PrintMenuBarItem.Size = new System.Drawing.Size(102, 22);
            this.PrintMenuBarItem.Text = "P&rint";
            this.PrintMenuBarItem.Click += new System.EventHandler(this.PrintMenuBarItem_Click);
            // 
            // connectionToolStripMenuItem
            // 
            this.connectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ReconnectMenuBarItem,
            this.PasswordResetMenuBarItem});
            this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            this.connectionToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.connectionToolStripMenuItem.Text = "&Connection";
            // 
            // ReconnectMenuBarItem
            // 
            this.ReconnectMenuBarItem.Name = "ReconnectMenuBarItem";
            this.ReconnectMenuBarItem.Size = new System.Drawing.Size(155, 22);
            this.ReconnectMenuBarItem.Text = "Disconnect";
            this.ReconnectMenuBarItem.Click += new System.EventHandler(this.ReconnectMenuBarItem_Click);
            // 
            // PasswordResetMenuBarItem
            // 
            this.PasswordResetMenuBarItem.Name = "PasswordResetMenuBarItem";
            this.PasswordResetMenuBarItem.Size = new System.Drawing.Size(155, 22);
            this.PasswordResetMenuBarItem.Text = "&Password Reset";
            this.PasswordResetMenuBarItem.Click += new System.EventHandler(this.PasswordResetMenuBarItem_Click);
            // 
            // BackgroundColorMenuBarItem
            // 
            this.BackgroundColorMenuBarItem.Name = "BackgroundColorMenuBarItem";
            this.BackgroundColorMenuBarItem.Size = new System.Drawing.Size(115, 20);
            this.BackgroundColorMenuBarItem.Text = "&Background Color";
            // 
            // ScriptsMenuBarItem
            // 
            this.ScriptsMenuBarItem.Checked = true;
            this.ScriptsMenuBarItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ScriptsMenuBarItem.Name = "ScriptsMenuBarItem";
            this.ScriptsMenuBarItem.Size = new System.Drawing.Size(54, 20);
            this.ScriptsMenuBarItem.Text = "&Scripts";
            // 
            // ReconnectTimer
            // 
            this.ReconnectTimer.Enabled = true;
            this.ReconnectTimer.Tick += new System.EventHandler(this.ReconnectTimer_Tick);
            // 
            // CommandTimer
            // 
            this.CommandTimer.Enabled = true;
            this.CommandTimer.Tick += new System.EventHandler(this.CommandTimer_Tick);
            // 
            // ToolBarPanel
            // 
            this.ToolBarPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolBarPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ToolBarPanel.Controls.Add(this.ToolBar);
            this.ToolBarPanel.Location = new System.Drawing.Point(-1, 24);
            this.ToolBarPanel.Name = "ToolBarPanel";
            this.ToolBarPanel.Size = new System.Drawing.Size(824, 26);
            this.ToolBarPanel.TabIndex = 5;
            // 
            // ToolBar
            // 
            this.ToolBar.AutoSize = false;
            this.ToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PrintToolBarItem,
            this.CopyToolBarItem,
            this.PasteToolBarItem,
            this.toolStripSeparator1,
            this.ReconnectToolBarItem,
            this.PasswordResetToolBarItem,
            this.toolStripSeparator2,
            this.BackgroundToolBarItem,
            this.toolStripSeparator3});
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Padding = new System.Windows.Forms.Padding(10, 0, 1, 0);
            this.ToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.ToolBar.Size = new System.Drawing.Size(822, 24);
            this.ToolBar.TabIndex = 5;
            this.ToolBar.Text = "toolStrip1";
            // 
            // PrintToolBarItem
            // 
            this.PrintToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PrintToolBarItem.Image = global::MdSession.Properties.Resources.print;
            this.PrintToolBarItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.PrintToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PrintToolBarItem.Name = "PrintToolBarItem";
            this.PrintToolBarItem.Size = new System.Drawing.Size(24, 21);
            this.PrintToolBarItem.Text = "Print";
            this.PrintToolBarItem.Click += new System.EventHandler(this.PrintToolBarItem_Click);
            // 
            // CopyToolBarItem
            // 
            this.CopyToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CopyToolBarItem.Image = global::MdSession.Properties.Resources.copy;
            this.CopyToolBarItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CopyToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CopyToolBarItem.Name = "CopyToolBarItem";
            this.CopyToolBarItem.Size = new System.Drawing.Size(24, 21);
            this.CopyToolBarItem.Text = "Copy";
            this.CopyToolBarItem.Click += new System.EventHandler(this.CopyToolBarItem_Click);
            // 
            // PasteToolBarItem
            // 
            this.PasteToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PasteToolBarItem.Image = global::MdSession.Properties.Resources.paste;
            this.PasteToolBarItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.PasteToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PasteToolBarItem.Name = "PasteToolBarItem";
            this.PasteToolBarItem.Size = new System.Drawing.Size(24, 21);
            this.PasteToolBarItem.Text = "Paste";
            this.PasteToolBarItem.Click += new System.EventHandler(this.PasteToolBarItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 24);
            // 
            // ReconnectToolBarItem
            // 
            this.ReconnectToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ReconnectToolBarItem.Image = global::MdSession.Properties.Resources.reconnect;
            this.ReconnectToolBarItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ReconnectToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ReconnectToolBarItem.Name = "ReconnectToolBarItem";
            this.ReconnectToolBarItem.Size = new System.Drawing.Size(24, 21);
            this.ReconnectToolBarItem.Text = "Connect/Disconnect";
            this.ReconnectToolBarItem.Click += new System.EventHandler(this.ReconnectToolBarItem_Click);
            // 
            // PasswordResetToolBarItem
            // 
            this.PasswordResetToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PasswordResetToolBarItem.Image = global::MdSession.Properties.Resources.passwordreset;
            this.PasswordResetToolBarItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.PasswordResetToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PasswordResetToolBarItem.Name = "PasswordResetToolBarItem";
            this.PasswordResetToolBarItem.Size = new System.Drawing.Size(24, 21);
            this.PasswordResetToolBarItem.Text = "Password Reset";
            this.PasswordResetToolBarItem.Click += new System.EventHandler(this.PasswordResetToolBarItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 24);
            // 
            // BackgroundToolBarItem
            // 
            this.BackgroundToolBarItem.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BackgroundToolBarItem.Image = global::MdSession.Properties.Resources.magenta;
            this.BackgroundToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BackgroundToolBarItem.Name = "BackgroundToolBarItem";
            this.BackgroundToolBarItem.Size = new System.Drawing.Size(32, 21);
            this.BackgroundToolBarItem.ButtonClick += new System.EventHandler(this.BackgroundToolBarItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 24);
            // 
            // SessionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 481);
            this.Controls.Add(this.ToolBarPanel);
            this.Controls.Add(this.MenuBar);
            this.Controls.Add(this.ReflectionPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SessionForm";
            this.Text = "MD Session";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SessionForm_FormClosing);
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            this.ToolBarPanel.ResumeLayout(false);
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.Panel ReflectionPanel;
        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyMenuBarItem;
        private System.Windows.Forms.ToolStripMenuItem PasteMenuBarItem;
        private System.Windows.Forms.ToolStripMenuItem PrintMenuBarItem;
        private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReconnectMenuBarItem;
        private System.Windows.Forms.ToolStripMenuItem PasswordResetMenuBarItem;
        private System.Windows.Forms.ToolStripMenuItem ScriptsMenuBarItem;
        private System.Windows.Forms.Timer ReconnectTimer;
        private System.Windows.Forms.Timer CommandTimer;
        private System.Windows.Forms.Panel ToolBarPanel;
        private System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripButton PrintToolBarItem;
        private System.Windows.Forms.ToolStripButton CopyToolBarItem;
        private System.Windows.Forms.ToolStripButton PasteToolBarItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ReconnectToolBarItem;
        private System.Windows.Forms.ToolStripButton PasswordResetToolBarItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSplitButton BackgroundToolBarItem;
        private System.Windows.Forms.ToolStripMenuItem BackgroundColorMenuBarItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

    }
}

