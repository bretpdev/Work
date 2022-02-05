namespace CommentAuditTracker
{
    partial class AgentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgentForm));
            this.FilterBox = new System.Windows.Forms.GroupBox();
            this.ResultsGroup = new System.Windows.Forms.GroupBox();
            this.AgentsPanel = new System.Windows.Forms.Panel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.NewAgentButton = new System.Windows.Forms.Button();
            this.FocusThief = new System.Windows.Forms.TextBox();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.AdvancedMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteAgentsMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.UtIdFilterBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.FullNameFilterBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.ActiveInactiveFilterBox = new CommentAuditTracker.ActiveInactiveAllCycleButton();
            this.FilterBox.SuspendLayout();
            this.ResultsGroup.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // FilterBox
            // 
            this.FilterBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterBox.Controls.Add(this.UtIdFilterBox);
            this.FilterBox.Controls.Add(this.ActiveInactiveFilterBox);
            this.FilterBox.Controls.Add(this.FullNameFilterBox);
            this.FilterBox.Location = new System.Drawing.Point(12, 30);
            this.FilterBox.Name = "FilterBox";
            this.FilterBox.Size = new System.Drawing.Size(700, 50);
            this.FilterBox.TabIndex = 1;
            this.FilterBox.TabStop = false;
            this.FilterBox.Text = "Filter";
            // 
            // ResultsGroup
            // 
            this.ResultsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsGroup.Controls.Add(this.AgentsPanel);
            this.ResultsGroup.Location = new System.Drawing.Point(12, 86);
            this.ResultsGroup.Name = "ResultsGroup";
            this.ResultsGroup.Size = new System.Drawing.Size(700, 278);
            this.ResultsGroup.TabIndex = 2;
            this.ResultsGroup.TabStop = false;
            this.ResultsGroup.Text = "Results";
            // 
            // AgentsPanel
            // 
            this.AgentsPanel.AutoScroll = true;
            this.AgentsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AgentsPanel.Location = new System.Drawing.Point(3, 22);
            this.AgentsPanel.Name = "AgentsPanel";
            this.AgentsPanel.Size = new System.Drawing.Size(694, 253);
            this.AgentsPanel.TabIndex = 0;
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Enabled = false;
            this.SaveButton.Location = new System.Drawing.Point(565, 370);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(144, 39);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.Text = "Save Changes";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.Enabled = false;
            this.CancelButton.Location = new System.Drawing.Point(363, 370);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(196, 39);
            this.CancelButton.TabIndex = 4;
            this.CancelButton.Text = "Cancel / Undo Changes";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // NewAgentButton
            // 
            this.NewAgentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewAgentButton.Location = new System.Drawing.Point(15, 370);
            this.NewAgentButton.Name = "NewAgentButton";
            this.NewAgentButton.Size = new System.Drawing.Size(182, 39);
            this.NewAgentButton.TabIndex = 5;
            this.NewAgentButton.Text = "Add New Agent";
            this.NewAgentButton.UseVisualStyleBackColor = true;
            this.NewAgentButton.Click += new System.EventHandler(this.NewAgentButton_Click);
            // 
            // FocusThief
            // 
            this.FocusThief.Location = new System.Drawing.Point(-100, -100);
            this.FocusThief.Multiline = true;
            this.FocusThief.Name = "FocusThief";
            this.FocusThief.Size = new System.Drawing.Size(0, 0);
            this.FocusThief.TabIndex = 0;
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AdvancedMenuButton});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MainMenu.Size = new System.Drawing.Size(724, 24);
            this.MainMenu.TabIndex = 6;
            this.MainMenu.Text = "menuStrip1";
            // 
            // AdvancedMenuButton
            // 
            this.AdvancedMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteAgentsMenuButton});
            this.AdvancedMenuButton.Name = "AdvancedMenuButton";
            this.AdvancedMenuButton.Size = new System.Drawing.Size(72, 20);
            this.AdvancedMenuButton.Text = "&Advanced";
            // 
            // DeleteAgentsMenuButton
            // 
            this.DeleteAgentsMenuButton.Name = "DeleteAgentsMenuButton";
            this.DeleteAgentsMenuButton.Size = new System.Drawing.Size(147, 22);
            this.DeleteAgentsMenuButton.Text = "&Delete Agents";
            this.DeleteAgentsMenuButton.Click += new System.EventHandler(this.DeleteAgentsMenuButton_Click);
            // 
            // UtIdFilterBox
            // 
            this.UtIdFilterBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UtIdFilterBox.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Italic);
            this.UtIdFilterBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.UtIdFilterBox.Location = new System.Drawing.Point(445, 18);
            this.UtIdFilterBox.MaxLength = 7;
            this.UtIdFilterBox.Name = "UtIdFilterBox";
            this.UtIdFilterBox.Size = new System.Drawing.Size(140, 26);
            this.UtIdFilterBox.TabIndex = 1;
            this.UtIdFilterBox.Text = "UT ID";
            this.UtIdFilterBox.Watermark = "UT ID";
            this.UtIdFilterBox.TextChanged += new System.EventHandler(this.Trigger_Search);
            // 
            // FullNameFilterBox
            // 
            this.FullNameFilterBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FullNameFilterBox.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Italic);
            this.FullNameFilterBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FullNameFilterBox.Location = new System.Drawing.Point(10, 18);
            this.FullNameFilterBox.Name = "FullNameFilterBox";
            this.FullNameFilterBox.Size = new System.Drawing.Size(429, 26);
            this.FullNameFilterBox.TabIndex = 0;
            this.FullNameFilterBox.Text = "Full Name";
            this.FullNameFilterBox.Watermark = "Full Name";
            this.FullNameFilterBox.TextChanged += new System.EventHandler(this.Trigger_Search);
            // 
            // ActiveInactiveFilterBox
            // 
            this.ActiveInactiveFilterBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveInactiveFilterBox.Font = new System.Drawing.Font("Arial", 12F);
            this.ActiveInactiveFilterBox.Location = new System.Drawing.Point(591, 18);
            this.ActiveInactiveFilterBox.Name = "ActiveInactiveFilterBox";
            this.ActiveInactiveFilterBox.SelectedIndex = 1;
            this.ActiveInactiveFilterBox.SelectedValue = CommentAuditTracker.ActiveInactiveAll.Active;
            this.ActiveInactiveFilterBox.Size = new System.Drawing.Size(99, 26);
            this.ActiveInactiveFilterBox.TabIndex = 2;
            this.ActiveInactiveFilterBox.UseVisualStyleBackColor = true;
            this.ActiveInactiveFilterBox.Cycle += new Uheaa.Common.WinForms.CycleButton<CommentAuditTracker.ActiveInactiveAll>.OnCycle(this.ActiveInactiveFilterBox_Cycle);
            // 
            // AgentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 421);
            this.Controls.Add(this.FocusThief);
            this.Controls.Add(this.NewAgentButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ResultsGroup);
            this.Controls.Add(this.FilterBox);
            this.Controls.Add(this.MainMenu);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AgentForm";
            this.Text = "Comment Audit Tracker";
            this.Load += new System.EventHandler(this.AgentForm_Load);
            this.Shown += new System.EventHandler(this.AgentForm_Shown);
            this.FilterBox.ResumeLayout(false);
            this.FilterBox.PerformLayout();
            this.ResultsGroup.ResumeLayout(false);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.WatermarkTextBox FullNameFilterBox;
        private System.Windows.Forms.GroupBox FilterBox;
        private System.Windows.Forms.GroupBox ResultsGroup;
        private Uheaa.Common.WinForms.WatermarkTextBox UtIdFilterBox;
        private ActiveInactiveAllCycleButton ActiveInactiveFilterBox;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button NewAgentButton;
        private System.Windows.Forms.Panel AgentsPanel;
        private System.Windows.Forms.TextBox FocusThief;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem AdvancedMenuButton;
        private System.Windows.Forms.ToolStripMenuItem DeleteAgentsMenuButton;
    }
}

