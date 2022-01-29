
namespace DeskAudits
{
    partial class AuditMainScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuditMainScreen));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.TabsControl = new System.Windows.Forms.TabControl();
            this.SearchTab = new System.Windows.Forms.TabPage();
            this.SubmitTab = new System.Windows.Forms.TabPage();
            this.AuditSearch = new DeskAudits.AuditSearch();
            this.AuditSubmission = new DeskAudits.AuditSubmission();
            this.TabsControl.SuspendLayout();
            this.SearchTab.SuspendLayout();
            this.SubmitTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabsControl
            // 
            this.TabsControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabsControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.TabsControl.Controls.Add(this.SubmitTab);
            this.TabsControl.Controls.Add(this.SearchTab);
            this.TabsControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.TabsControl.Location = new System.Drawing.Point(0, 16);
            this.TabsControl.Name = "TabsControl";
            this.TabsControl.SelectedIndex = 0;
            this.TabsControl.Size = new System.Drawing.Size(946, 482);
            this.TabsControl.TabIndex = 0;
            // 
            // SearchTab
            // 
            this.SearchTab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SearchTab.Controls.Add(this.AuditSearch);
            this.SearchTab.Location = new System.Drawing.Point(4, 36);
            this.SearchTab.Name = "SearchTab";
            this.SearchTab.Padding = new System.Windows.Forms.Padding(3);
            this.SearchTab.Size = new System.Drawing.Size(938, 442);
            this.SearchTab.TabIndex = 0;
            this.SearchTab.Text = "Lookup Audits";
            this.SearchTab.UseVisualStyleBackColor = true;
            // 
            // SubmitTab
            // 
            this.SubmitTab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SubmitTab.Controls.Add(this.AuditSubmission);
            this.SubmitTab.Location = new System.Drawing.Point(4, 36);
            this.SubmitTab.Name = "SubmitTab";
            this.SubmitTab.Padding = new System.Windows.Forms.Padding(3);
            this.SubmitTab.Size = new System.Drawing.Size(938, 442);
            this.SubmitTab.TabIndex = 1;
            this.SubmitTab.Text = "Log Audit";
            this.SubmitTab.UseVisualStyleBackColor = true;
            // 
            // AuditSearch
            // 
            this.AuditSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AuditSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AuditSearch.BackColor = System.Drawing.SystemColors.ControlLight;
            this.AuditSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.AuditSearch.Location = new System.Drawing.Point(0, 0);
            this.AuditSearch.Margin = new System.Windows.Forms.Padding(6);
            this.AuditSearch.Name = "AuditSearch";
            this.AuditSearch.Size = new System.Drawing.Size(942, 464);
            this.AuditSearch.TabIndex = 3;
            // 
            // AuditSubmission
            // 
            this.AuditSubmission.AutoSize = true;
            this.AuditSubmission.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AuditSubmission.BackColor = System.Drawing.SystemColors.ControlLight;
            this.AuditSubmission.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AuditSubmission.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AuditSubmission.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.AuditSubmission.Location = new System.Drawing.Point(3, 3);
            this.AuditSubmission.Margin = new System.Windows.Forms.Padding(6);
            this.AuditSubmission.Name = "AuditSubmission";
            this.AuditSubmission.Size = new System.Drawing.Size(930, 434);
            this.AuditSubmission.TabIndex = 0;
            // 
            // AuditMainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 514);
            this.Controls.Add(this.TabsControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(963, 553);
            this.Name = "AuditMainScreen";
            this.Text = "Desk Audits";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AuditMainScreen_FormClosed);
            this.TabsControl.ResumeLayout(false);
            this.SearchTab.ResumeLayout(false);
            this.SubmitTab.ResumeLayout(false);
            this.SubmitTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabControl TabsControl;
        private System.Windows.Forms.TabPage SearchTab;
        private System.Windows.Forms.TabPage SubmitTab;
        private AuditSubmission AuditSubmission;
        private AuditSearch AuditSearch;
    }
}

