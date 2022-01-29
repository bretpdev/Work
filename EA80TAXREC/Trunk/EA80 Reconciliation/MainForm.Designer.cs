namespace EA80Reconciliation
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
			this.Tabs = new System.Windows.Forms.TabControl();
			this.EA27Compare = new System.Windows.Forms.TabPage();
			this.ResultsList = new System.Windows.Forms.ListBox();
			this.ProgressLabel = new System.Windows.Forms.Label();
			this.CopyAllButton = new System.Windows.Forms.Button();
			this.EA27FormControl = new UserControls.EA27Control(LogData);
			this.Tabs.SuspendLayout();
			this.EA27Compare.SuspendLayout();
			this.SuspendLayout();
			// 
			// Tabs
			// 
			this.Tabs.Alignment = System.Windows.Forms.TabAlignment.Left;
			this.Tabs.Controls.Add(this.EA27Compare);
			this.Tabs.ItemSize = new System.Drawing.Size(50, 160);
			this.Tabs.Location = new System.Drawing.Point(12, 3);
			this.Tabs.Multiline = true;
			this.Tabs.Name = "Tabs";
			this.Tabs.SelectedIndex = 0;
			this.Tabs.Size = new System.Drawing.Size(571, 427);
			this.Tabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.Tabs.TabIndex = 0;
			this.Tabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Tabs_DrawItem);
			// 
			// EA27Compare
			// 
			this.EA27Compare.Controls.Add(this.EA27FormControl);
			this.EA27Compare.Location = new System.Drawing.Point(164, 4);
			this.EA27Compare.Name = "EA27Compare";
			this.EA27Compare.Padding = new System.Windows.Forms.Padding(3);
			this.EA27Compare.Size = new System.Drawing.Size(403, 419);
			this.EA27Compare.TabIndex = 6;
			this.EA27Compare.Text = "EA27 Compare";
			this.EA27Compare.UseVisualStyleBackColor = true;
			// 
			// ResultsList
			// 
			this.ResultsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ResultsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.ResultsList.FormattingEnabled = true;
			this.ResultsList.HorizontalScrollbar = true;
			this.ResultsList.Location = new System.Drawing.Point(589, 46);
			this.ResultsList.Name = "ResultsList";
			this.ResultsList.Size = new System.Drawing.Size(462, 355);
			this.ResultsList.TabIndex = 1;
			this.ResultsList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ResultsList_DrawItem);
			this.ResultsList.DoubleClick += new System.EventHandler(this.ResultsList_DoubleClick);
			this.ResultsList.Resize += new System.EventHandler(this.ResultsList_Resize);
			// 
			// ProgressLabel
			// 
			this.ProgressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ProgressLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ProgressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ProgressLabel.Location = new System.Drawing.Point(589, 3);
			this.ProgressLabel.Name = "ProgressLabel";
			this.ProgressLabel.Size = new System.Drawing.Size(462, 42);
			this.ProgressLabel.TabIndex = 27;
			this.ProgressLabel.Text = "Results";
			this.ProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// CopyAllButton
			// 
			this.CopyAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CopyAllButton.Enabled = false;
			this.CopyAllButton.Location = new System.Drawing.Point(783, 407);
			this.CopyAllButton.Name = "CopyAllButton";
			this.CopyAllButton.Size = new System.Drawing.Size(75, 23);
			this.CopyAllButton.TabIndex = 28;
			this.CopyAllButton.Text = "Copy All";
			this.CopyAllButton.UseVisualStyleBackColor = true;
			this.CopyAllButton.Click += new System.EventHandler(this.CopyAllButton_Click);
			// 
			// EA27FormControl
			// 
			this.EA27FormControl.Location = new System.Drawing.Point(2, 3);
			this.EA27FormControl.Name = "EA27FormControl";
			this.EA27FormControl.Size = new System.Drawing.Size(398, 350);
			this.EA27FormControl.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1059, 435);
			this.Controls.Add(this.CopyAllButton);
			this.Controls.Add(this.ProgressLabel);
			this.Controls.Add(this.ResultsList);
			this.Controls.Add(this.Tabs);
			this.MinimumSize = new System.Drawing.Size(1075, 473);
			this.Name = "MainForm";
			this.Text = "EA80 TAXX Reconciliation";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Tabs.ResumeLayout(false);
			this.EA27Compare.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.TabControl Tabs;
		private System.Windows.Forms.ListBox ResultsList;
		private System.Windows.Forms.Label ProgressLabel;
		private System.Windows.Forms.Button CopyAllButton;
        private System.Windows.Forms.TabPage EA27Compare;
        private UserControls.EA27Control EA27FormControl;
    }
}

