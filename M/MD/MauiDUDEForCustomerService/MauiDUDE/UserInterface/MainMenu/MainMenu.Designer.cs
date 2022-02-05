namespace MauiDUDE
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.nonSystemCallsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noUHEAAConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.askDudeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainingModuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dictionaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutDUDEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unexpectedResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brightIdeaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demosOnlyOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonSecurityIncident = new System.Windows.Forms.Button();
            this.buttonPhysicalThreat = new System.Windows.Forms.Button();
            this.buttonWhoAmI = new System.Windows.Forms.Button();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.textBoxAccountNumberOrSSN = new System.Windows.Forms.TextBox();
            this.labelAccountNumberOrSSN = new System.Windows.Forms.Label();
            this.flowLayoutPanelPreviousBorrowers = new System.Windows.Forms.FlowLayoutPanel();
            this.labelShowPreviousBorrowers = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nonSystemCallsToolStripMenuItem,
            this.qAToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.demosOnlyOffToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(445, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // nonSystemCallsToolStripMenuItem
            // 
            this.nonSystemCallsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noUHEAAConnectionToolStripMenuItem});
            this.nonSystemCallsToolStripMenuItem.Name = "nonSystemCallsToolStripMenuItem";
            this.nonSystemCallsToolStripMenuItem.Size = new System.Drawing.Size(113, 20);
            this.nonSystemCallsToolStripMenuItem.Text = "Non-System Calls";
            // 
            // noUHEAAConnectionToolStripMenuItem
            // 
            this.noUHEAAConnectionToolStripMenuItem.Name = "noUHEAAConnectionToolStripMenuItem";
            this.noUHEAAConnectionToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.noUHEAAConnectionToolStripMenuItem.Text = "No UHEAA Connection";
            this.noUHEAAConnectionToolStripMenuItem.Click += new System.EventHandler(this.noUHEAAConnectionToolStripMenuItem_Click);
            // 
            // qAToolStripMenuItem
            // 
            this.qAToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.askDudeToolStripMenuItem,
            this.trainingModuleToolStripMenuItem});
            this.qAToolStripMenuItem.Name = "qAToolStripMenuItem";
            this.qAToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.qAToolStripMenuItem.Text = "Q && A";
            // 
            // askDudeToolStripMenuItem
            // 
            this.askDudeToolStripMenuItem.Name = "askDudeToolStripMenuItem";
            this.askDudeToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.askDudeToolStripMenuItem.Text = "Ask Dude";
            this.askDudeToolStripMenuItem.Click += new System.EventHandler(this.askDudeToolStripMenuItem_Click);
            // 
            // trainingModuleToolStripMenuItem
            // 
            this.trainingModuleToolStripMenuItem.Name = "trainingModuleToolStripMenuItem";
            this.trainingModuleToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.trainingModuleToolStripMenuItem.Text = "Training Module";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dictionaryToolStripMenuItem,
            this.aboutDUDEToolStripMenuItem,
            this.unexpectedResultsToolStripMenuItem,
            this.brightIdeaToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // dictionaryToolStripMenuItem
            // 
            this.dictionaryToolStripMenuItem.Name = "dictionaryToolStripMenuItem";
            this.dictionaryToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.dictionaryToolStripMenuItem.Text = "Dictionary";
            // 
            // aboutDUDEToolStripMenuItem
            // 
            this.aboutDUDEToolStripMenuItem.Name = "aboutDUDEToolStripMenuItem";
            this.aboutDUDEToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.aboutDUDEToolStripMenuItem.Text = "About DUDE";
            // 
            // unexpectedResultsToolStripMenuItem
            // 
            this.unexpectedResultsToolStripMenuItem.Name = "unexpectedResultsToolStripMenuItem";
            this.unexpectedResultsToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.unexpectedResultsToolStripMenuItem.Text = "Unexpected Results";
            this.unexpectedResultsToolStripMenuItem.Click += new System.EventHandler(this.unexpectedResultsToolStripMenuItem_Click);
            // 
            // brightIdeaToolStripMenuItem
            // 
            this.brightIdeaToolStripMenuItem.Name = "brightIdeaToolStripMenuItem";
            this.brightIdeaToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.brightIdeaToolStripMenuItem.Text = "Bright Idea";
            this.brightIdeaToolStripMenuItem.Click += new System.EventHandler(this.brightIdeaToolStripMenuItem_Click);
            // 
            // demosOnlyOffToolStripMenuItem
            // 
            this.demosOnlyOffToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.demosOnlyOffToolStripMenuItem.Name = "demosOnlyOffToolStripMenuItem";
            this.demosOnlyOffToolStripMenuItem.Size = new System.Drawing.Size(107, 20);
            this.demosOnlyOffToolStripMenuItem.Text = "Demos Only: Off";
            this.demosOnlyOffToolStripMenuItem.Click += new System.EventHandler(this.demosOnlyOffToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.buttonSecurityIncident);
            this.panel1.Controls.Add(this.buttonPhysicalThreat);
            this.panel1.Controls.Add(this.buttonWhoAmI);
            this.panel1.Controls.Add(this.buttonContinue);
            this.panel1.Controls.Add(this.textBoxAccountNumberOrSSN);
            this.panel1.Controls.Add(this.labelAccountNumberOrSSN);
            this.panel1.Location = new System.Drawing.Point(30, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(369, 39);
            this.panel1.TabIndex = 1;
            // 
            // buttonSecurityIncident
            // 
            this.buttonSecurityIncident.Image = global::MauiDUDE.Properties.Resources.brokenlock;
            this.buttonSecurityIncident.Location = new System.Drawing.Point(331, 5);
            this.buttonSecurityIncident.Name = "buttonSecurityIncident";
            this.buttonSecurityIncident.Size = new System.Drawing.Size(29, 30);
            this.buttonSecurityIncident.TabIndex = 5;
            this.buttonSecurityIncident.UseVisualStyleBackColor = true;
            this.buttonSecurityIncident.Click += new System.EventHandler(this.buttonSecurityIncident_Click);
            // 
            // buttonPhysicalThreat
            // 
            this.buttonPhysicalThreat.Image = global::MauiDUDE.Properties.Resources.bomb;
            this.buttonPhysicalThreat.Location = new System.Drawing.Point(296, 5);
            this.buttonPhysicalThreat.Name = "buttonPhysicalThreat";
            this.buttonPhysicalThreat.Size = new System.Drawing.Size(29, 30);
            this.buttonPhysicalThreat.TabIndex = 4;
            this.buttonPhysicalThreat.UseVisualStyleBackColor = true;
            this.buttonPhysicalThreat.Click += new System.EventHandler(this.buttonPhysicalThreat_Click);
            // 
            // buttonWhoAmI
            // 
            this.buttonWhoAmI.Image = global::MauiDUDE.Properties.Resources.search;
            this.buttonWhoAmI.Location = new System.Drawing.Point(261, 5);
            this.buttonWhoAmI.Name = "buttonWhoAmI";
            this.buttonWhoAmI.Size = new System.Drawing.Size(29, 30);
            this.buttonWhoAmI.TabIndex = 3;
            this.buttonWhoAmI.UseVisualStyleBackColor = true;
            // 
            // buttonContinue
            // 
            this.buttonContinue.Image = global::MauiDUDE.Properties.Resources.Gear;
            this.buttonContinue.Location = new System.Drawing.Point(226, 5);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(29, 30);
            this.buttonContinue.TabIndex = 2;
            this.buttonContinue.UseVisualStyleBackColor = true;
            // 
            // textBoxAccountNumberOrSSN
            // 
            this.textBoxAccountNumberOrSSN.Location = new System.Drawing.Point(118, 11);
            this.textBoxAccountNumberOrSSN.MaxLength = 10;
            this.textBoxAccountNumberOrSSN.Name = "textBoxAccountNumberOrSSN";
            this.textBoxAccountNumberOrSSN.Size = new System.Drawing.Size(100, 20);
            this.textBoxAccountNumberOrSSN.TabIndex = 1;
            // 
            // labelAccountNumberOrSSN
            // 
            this.labelAccountNumberOrSSN.AutoSize = true;
            this.labelAccountNumberOrSSN.Location = new System.Drawing.Point(13, 14);
            this.labelAccountNumberOrSSN.Name = "labelAccountNumberOrSSN";
            this.labelAccountNumberOrSSN.Size = new System.Drawing.Size(99, 13);
            this.labelAccountNumberOrSSN.TabIndex = 0;
            this.labelAccountNumberOrSSN.Text = "Account # Or SSN:";
            // 
            // flowLayoutPanelPreviousBorrowers
            // 
            this.flowLayoutPanelPreviousBorrowers.Location = new System.Drawing.Point(30, 66);
            this.flowLayoutPanelPreviousBorrowers.Name = "flowLayoutPanelPreviousBorrowers";
            this.flowLayoutPanelPreviousBorrowers.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanelPreviousBorrowers.TabIndex = 2;
            // 
            // labelShowPreviousBorrowers
            // 
            this.labelShowPreviousBorrowers.AutoSize = true;
            this.labelShowPreviousBorrowers.Location = new System.Drawing.Point(405, 53);
            this.labelShowPreviousBorrowers.Name = "labelShowPreviousBorrowers";
            this.labelShowPreviousBorrowers.Size = new System.Drawing.Size(13, 13);
            this.labelShowPreviousBorrowers.TabIndex = 3;
            this.labelShowPreviousBorrowers.Text = "+";
            this.labelShowPreviousBorrowers.Click += new System.EventHandler(this.labelShowPreviousBorrowers_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(174)))), ((int)(((byte)(231)))));
            this.ClientSize = new System.Drawing.Size(445, 73);
            this.Controls.Add(this.labelShowPreviousBorrowers);
            this.Controls.Add(this.flowLayoutPanelPreviousBorrowers);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(461, 112);
            this.Name = "MainMenu";
            this.Text = "Main Menu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainMenu_FormClosing);
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem nonSystemCallsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem demosOnlyOffToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelAccountNumberOrSSN;
        private System.Windows.Forms.Button buttonWhoAmI;
        private System.Windows.Forms.Button buttonPhysicalThreat;
        private System.Windows.Forms.Button buttonSecurityIncident;
        private System.Windows.Forms.ToolStripMenuItem noUHEAAConnectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem askDudeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trainingModuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dictionaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutDUDEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unexpectedResultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brightIdeaToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelPreviousBorrowers;
        private System.Windows.Forms.Label labelShowPreviousBorrowers;
        public System.Windows.Forms.TextBox textBoxAccountNumberOrSSN;
        public System.Windows.Forms.Button buttonContinue;
    }
}