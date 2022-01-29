namespace SftpCoordinator
{
    partial class Dashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.SettingsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ProjectsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PathTypesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ProjectSnapshotMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AdvancedLabel = new System.Windows.Forms.Label();
            this.RunHistoryList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LastRunText = new System.Windows.Forms.TextBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.AdvancedPanel = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.EndCheck = new System.Windows.Forms.CheckBox();
            this.StartCheck = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.EndDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.StartDate = new System.Windows.Forms.DateTimePicker();
            this.DetailsButton = new System.Windows.Forms.Button();
            this.IncludeEmptyButton = new Uheaa.Common.WinForms.YesNoButton();
            this.RunNowContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RunNowSubItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.AdvancedPanel.SuspendLayout();
            this.RunNowContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingsMenu,
            this.ProjectsMenu,
            this.PathTypesMenu,
            this.ProjectSnapshotMenu});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(472, 24);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "menuStrip1";
            // 
            // SettingsMenu
            // 
            this.SettingsMenu.Name = "SettingsMenu";
            this.SettingsMenu.Size = new System.Drawing.Size(61, 20);
            this.SettingsMenu.Text = "&Settings";
            this.SettingsMenu.Click += new System.EventHandler(this.SettingsMenu_Click);
            // 
            // ProjectsMenu
            // 
            this.ProjectsMenu.Name = "ProjectsMenu";
            this.ProjectsMenu.Size = new System.Drawing.Size(61, 20);
            this.ProjectsMenu.Text = "&Projects";
            this.ProjectsMenu.Click += new System.EventHandler(this.ProjectsMenu_Click);
            // 
            // PathTypesMenu
            // 
            this.PathTypesMenu.Name = "PathTypesMenu";
            this.PathTypesMenu.Size = new System.Drawing.Size(158, 20);
            this.PathTypesMenu.Text = "Path Types && &Destinations";
            this.PathTypesMenu.Click += new System.EventHandler(this.PathTypesMenu_Click);
            // 
            // ProjectSnapshotMenu
            // 
            this.ProjectSnapshotMenu.Name = "ProjectSnapshotMenu";
            this.ProjectSnapshotMenu.Size = new System.Drawing.Size(108, 20);
            this.ProjectSnapshotMenu.Text = "Project &Snapshot";
            this.ProjectSnapshotMenu.Click += new System.EventHandler(this.ProjectSnapshotMenu_Click);
            // 
            // AdvancedLabel
            // 
            this.AdvancedLabel.AutoSize = true;
            this.AdvancedLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AdvancedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdvancedLabel.Location = new System.Drawing.Point(8, 122);
            this.AdvancedLabel.Name = "AdvancedLabel";
            this.AdvancedLabel.Size = new System.Drawing.Size(136, 20);
            this.AdvancedLabel.TabIndex = 9;
            this.AdvancedLabel.Text = "Last 100 Runs (+)";
            this.AdvancedLabel.Click += new System.EventHandler(this.AdvancedLabel_Click);
            this.AdvancedLabel.MouseEnter += new System.EventHandler(this.AdvancedLabel_MouseEnter);
            this.AdvancedLabel.MouseLeave += new System.EventHandler(this.AdvancedLabel_MouseLeave);
            // 
            // RunHistoryList
            // 
            this.RunHistoryList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunHistoryList.FormattingEnabled = true;
            this.RunHistoryList.ItemHeight = 16;
            this.RunHistoryList.Location = new System.Drawing.Point(12, 145);
            this.RunHistoryList.Name = "RunHistoryList";
            this.RunHistoryList.Size = new System.Drawing.Size(445, 212);
            this.RunHistoryList.TabIndex = 8;
            this.RunHistoryList.SelectedIndexChanged += new System.EventHandler(this.RunHistoryList_SelectedIndexChanged);
            this.RunHistoryList.DoubleClick += new System.EventHandler(this.RunHistoryList_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LastRunText);
            this.groupBox1.Controls.Add(this.RunButton);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(445, 82);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Last Run";
            // 
            // LastRunText
            // 
            this.LastRunText.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LastRunText.Location = new System.Drawing.Point(6, 31);
            this.LastRunText.Name = "LastRunText";
            this.LastRunText.ReadOnly = true;
            this.LastRunText.Size = new System.Drawing.Size(275, 38);
            this.LastRunText.TabIndex = 1;
            this.LastRunText.Text = "1/1/2012 9:45 AM";
            this.LastRunText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RunButton
            // 
            this.RunButton.ContextMenuStrip = this.RunNowContextMenu;
            this.RunButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunButton.Location = new System.Drawing.Point(287, 31);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(146, 38);
            this.RunButton.TabIndex = 0;
            this.RunButton.Text = "Run Now";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // AdvancedPanel
            // 
            this.AdvancedPanel.Controls.Add(this.label3);
            this.AdvancedPanel.Controls.Add(this.IncludeEmptyButton);
            this.AdvancedPanel.Controls.Add(this.EndCheck);
            this.AdvancedPanel.Controls.Add(this.StartCheck);
            this.AdvancedPanel.Controls.Add(this.label2);
            this.AdvancedPanel.Controls.Add(this.EndDate);
            this.AdvancedPanel.Controls.Add(this.label1);
            this.AdvancedPanel.Controls.Add(this.StartDate);
            this.AdvancedPanel.Location = new System.Drawing.Point(12, 363);
            this.AdvancedPanel.Name = "AdvancedPanel";
            this.AdvancedPanel.Size = new System.Drawing.Size(448, 75);
            this.AdvancedPanel.TabIndex = 10;
            this.AdvancedPanel.TabStop = false;
            this.AdvancedPanel.Text = "Advanced";
            this.AdvancedPanel.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(283, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 20);
            this.label3.TabIndex = 27;
            this.label3.Text = "Include Empty Runs";
            // 
            // EndCheck
            // 
            this.EndCheck.AutoSize = true;
            this.EndCheck.Location = new System.Drawing.Point(185, 19);
            this.EndCheck.Name = "EndCheck";
            this.EndCheck.Size = new System.Drawing.Size(15, 14);
            this.EndCheck.TabIndex = 25;
            this.EndCheck.UseVisualStyleBackColor = true;
            this.EndCheck.CheckedChanged += new System.EventHandler(this.EndCheck_CheckedChanged);
            // 
            // StartCheck
            // 
            this.StartCheck.AutoSize = true;
            this.StartCheck.Checked = true;
            this.StartCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StartCheck.Location = new System.Drawing.Point(56, 19);
            this.StartCheck.Name = "StartCheck";
            this.StartCheck.Size = new System.Drawing.Size(15, 14);
            this.StartCheck.TabIndex = 24;
            this.StartCheck.UseVisualStyleBackColor = true;
            this.StartCheck.CheckedChanged += new System.EventHandler(this.StartCheck_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(141, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 20);
            this.label2.TabIndex = 23;
            this.label2.Text = "End";
            // 
            // EndDate
            // 
            this.EndDate.CustomFormat = "MM/dd/yyyy hh:mm tt";
            this.EndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDate.Location = new System.Drawing.Point(145, 39);
            this.EndDate.Name = "EndDate";
            this.EndDate.Size = new System.Drawing.Size(136, 20);
            this.EndDate.TabIndex = 22;
            this.EndDate.ValueChanged += new System.EventHandler(this.EndDate_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "Start";
            // 
            // StartDate
            // 
            this.StartDate.CustomFormat = "MM/dd/yyyy hh:mm tt";
            this.StartDate.Enabled = false;
            this.StartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDate.Location = new System.Drawing.Point(10, 39);
            this.StartDate.Name = "StartDate";
            this.StartDate.Size = new System.Drawing.Size(129, 20);
            this.StartDate.TabIndex = 20;
            this.StartDate.ValueChanged += new System.EventHandler(this.StartDate_ValueChanged);
            // 
            // DetailsButton
            // 
            this.DetailsButton.Enabled = false;
            this.DetailsButton.Location = new System.Drawing.Point(357, 119);
            this.DetailsButton.Name = "DetailsButton";
            this.DetailsButton.Size = new System.Drawing.Size(100, 23);
            this.DetailsButton.TabIndex = 11;
            this.DetailsButton.Text = "Run Details >>";
            this.DetailsButton.UseVisualStyleBackColor = true;
            this.DetailsButton.Click += new System.EventHandler(this.DetailsButton_Click);
            // 
            // IncludeEmptyButton
            // 
            this.IncludeEmptyButton.Location = new System.Drawing.Point(324, 36);
            this.IncludeEmptyButton.Name = "IncludeEmptyButton";
            this.IncludeEmptyButton.SelectedIndex = 1;
            this.IncludeEmptyButton.SelectedValue = false;
            this.IncludeEmptyButton.Size = new System.Drawing.Size(75, 23);
            this.IncludeEmptyButton.TabIndex = 26;
            this.IncludeEmptyButton.UseVisualStyleBackColor = true;
            this.IncludeEmptyButton.Cycle += new Uheaa.Common.WinForms.CycleButton<bool>.OnCycle(this.IncludeEmptyButton_Cycle);
            // 
            // RunNowContextMenu
            // 
            this.RunNowContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunNowSubItem});
            this.RunNowContextMenu.Name = "RunNowContextMenu";
            this.RunNowContextMenu.Size = new System.Drawing.Size(288, 26);
            // 
            // RunNowSubItem
            // 
            this.RunNowSubItem.Name = "RunNowSubItem";
            this.RunNowSubItem.Size = new System.Drawing.Size(287, 22);
            this.RunNowSubItem.Text = "Run Now and open all applicable folders";
            this.RunNowSubItem.Click += new System.EventHandler(this.RunNowSubItem_Click);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 450);
            this.Controls.Add(this.DetailsButton);
            this.Controls.Add(this.AdvancedPanel);
            this.Controls.Add(this.AdvancedLabel);
            this.Controls.Add(this.RunHistoryList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.MaximizeBox = false;
            this.Name = "Dashboard";
            this.Text = "SFTP Coordinator";
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.AdvancedPanel.ResumeLayout(false);
            this.AdvancedPanel.PerformLayout();
            this.RunNowContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem SettingsMenu;
        private System.Windows.Forms.ToolStripMenuItem ProjectSnapshotMenu;
        private System.Windows.Forms.Label AdvancedLabel;
        private System.Windows.Forms.ListBox RunHistoryList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox LastRunText;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.GroupBox AdvancedPanel;
        private System.Windows.Forms.CheckBox EndCheck;
        private System.Windows.Forms.CheckBox StartCheck;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker EndDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker StartDate;
        private Uheaa.Common.WinForms.YesNoButton IncludeEmptyButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem ProjectsMenu;
        private System.Windows.Forms.ToolStripMenuItem PathTypesMenu;
        private System.Windows.Forms.Button DetailsButton;
        private System.Windows.Forms.ContextMenuStrip RunNowContextMenu;
        private System.Windows.Forms.ToolStripMenuItem RunNowSubItem;

    }
}