namespace ActiveDirectoryGroups
{
    partial class frmActiveDirectoryGroups
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
            this.cboStaffMember = new System.Windows.Forms.ComboBox();
            this.lbxGroups = new System.Windows.Forms.ListBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.userToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activeDirectoryGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmUpdateCsys = new System.Windows.Forms.ToolStripMenuItem();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboStaffMember
            // 
            this.cboStaffMember.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboStaffMember.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStaffMember.DropDownHeight = 350;
            this.cboStaffMember.Enabled = false;
            this.cboStaffMember.ForeColor = System.Drawing.Color.Black;
            this.cboStaffMember.FormattingEnabled = true;
            this.cboStaffMember.IntegralHeight = false;
            this.cboStaffMember.Location = new System.Drawing.Point(100, 38);
            this.cboStaffMember.Name = "cboStaffMember";
            this.cboStaffMember.Size = new System.Drawing.Size(380, 21);
            this.cboStaffMember.TabIndex = 0;
            this.cboStaffMember.TextChanged += new System.EventHandler(this.cboStaffMember_SelectedIndexChanged);
            // 
            // lbxGroups
            // 
            this.lbxGroups.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbxGroups.FormattingEnabled = true;
            this.lbxGroups.Location = new System.Drawing.Point(0, 83);
            this.lbxGroups.Name = "lbxGroups";
            this.lbxGroups.Size = new System.Drawing.Size(484, 628);
            this.lbxGroups.Sorted = true;
            this.lbxGroups.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblTitle.Location = new System.Drawing.Point(6, 64);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(10, 13);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = " ";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblName.Location = new System.Drawing.Point(12, 41);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(0, 13);
            this.lblName.TabIndex = 4;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userToolStripMenuItem,
            this.activeDirectoryGroupToolStripMenuItem,
            this.tsmUpdateCsys});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(484, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // userToolStripMenuItem
            // 
            this.userToolStripMenuItem.Name = "userToolStripMenuItem";
            this.userToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.userToolStripMenuItem.Text = "Users";
            this.userToolStripMenuItem.Click += new System.EventHandler(this.userToolStripMenuItem_Click);
            // 
            // activeDirectoryGroupToolStripMenuItem
            // 
            this.activeDirectoryGroupToolStripMenuItem.Name = "activeDirectoryGroupToolStripMenuItem";
            this.activeDirectoryGroupToolStripMenuItem.Size = new System.Drawing.Size(144, 20);
            this.activeDirectoryGroupToolStripMenuItem.Text = "Active Directory Groups";
            this.activeDirectoryGroupToolStripMenuItem.Click += new System.EventHandler(this.activeDirectoryGroupToolStripMenuItem_Click);
            // 
            // tsmUpdateCsys
            // 
            this.tsmUpdateCsys.Name = "tsmUpdateCsys";
            this.tsmUpdateCsys.Size = new System.Drawing.Size(87, 20);
            this.tsmUpdateCsys.Text = "Update CSYS";
            this.tsmUpdateCsys.Click += new System.EventHandler(this.tsmUpdateCsys_Click);
            // 
            // progress
            // 
            this.progress.BackColor = System.Drawing.Color.Silver;
            this.progress.Location = new System.Drawing.Point(346, 64);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(134, 18);
            this.progress.Step = 1;
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progress.TabIndex = 6;
            this.progress.Visible = false;
            // 
            // frmActiveDirectoryGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(484, 711);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lbxGroups);
            this.Controls.Add(this.cboStaffMember);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.HelpButton = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(500, 750);
            this.Name = "frmActiveDirectoryGroups";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Active Directory Groups";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboStaffMember;
		private System.Windows.Forms.ListBox lbxGroups;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem userToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem activeDirectoryGroupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tsmUpdateCsys;
        private System.Windows.Forms.ProgressBar progress;
    }
}

