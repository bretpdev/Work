namespace DirectoryCompressor
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
            this.TargetFolderBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ActivityLog = new System.Windows.Forms.TextBox();
            this.CompressButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.RunTimer = new System.Windows.Forms.Timer(this.components);
            this.FoundFilesLabel = new System.Windows.Forms.Label();
            this.BrowseDestinationButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.DestinationFolderBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TargetFolderBox
            // 
            this.TargetFolderBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TargetFolderBox.Location = new System.Drawing.Point(12, 28);
            this.TargetFolderBox.Name = "TargetFolderBox";
            this.TargetFolderBox.Size = new System.Drawing.Size(579, 23);
            this.TargetFolderBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Target Folder";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Location = new System.Drawing.Point(597, 28);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(72, 23);
            this.BrowseButton.TabIndex = 2;
            this.BrowseButton.Text = "Browse...";
            this.BrowseButton.UseCompatibleTextRendering = true;
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ActivityLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 102);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(657, 170);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Activity";
            // 
            // ActivityLog
            // 
            this.ActivityLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActivityLog.Location = new System.Drawing.Point(6, 22);
            this.ActivityLog.Multiline = true;
            this.ActivityLog.Name = "ActivityLog";
            this.ActivityLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ActivityLog.Size = new System.Drawing.Size(645, 142);
            this.ActivityLog.TabIndex = 4;
            // 
            // CompressButton
            // 
            this.CompressButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CompressButton.Location = new System.Drawing.Point(545, 279);
            this.CompressButton.Name = "CompressButton";
            this.CompressButton.Size = new System.Drawing.Size(124, 32);
            this.CompressButton.TabIndex = 4;
            this.CompressButton.Text = "Compress";
            this.CompressButton.UseVisualStyleBackColor = true;
            this.CompressButton.Click += new System.EventHandler(this.CompressButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelButton.Enabled = false;
            this.CancelButton.Location = new System.Drawing.Point(12, 279);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(124, 32);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // RunTimer
            // 
            this.RunTimer.Enabled = true;
            this.RunTimer.Interval = 500;
            this.RunTimer.Tick += new System.EventHandler(this.RunTimer_Tick);
            // 
            // FoundFilesLabel
            // 
            this.FoundFilesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FoundFilesLabel.Location = new System.Drawing.Point(355, 279);
            this.FoundFilesLabel.Name = "FoundFilesLabel";
            this.FoundFilesLabel.Size = new System.Drawing.Size(184, 32);
            this.FoundFilesLabel.TabIndex = 8;
            this.FoundFilesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BrowseDestinationButton
            // 
            this.BrowseDestinationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseDestinationButton.Location = new System.Drawing.Point(597, 73);
            this.BrowseDestinationButton.Name = "BrowseDestinationButton";
            this.BrowseDestinationButton.Size = new System.Drawing.Size(72, 23);
            this.BrowseDestinationButton.TabIndex = 11;
            this.BrowseDestinationButton.Text = "Browse...";
            this.BrowseDestinationButton.UseCompatibleTextRendering = true;
            this.BrowseDestinationButton.UseVisualStyleBackColor = true;
            this.BrowseDestinationButton.Click += new System.EventHandler(this.BrowseDestinationButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Destination Folder";
            // 
            // DestinationFolderBox
            // 
            this.DestinationFolderBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DestinationFolderBox.Location = new System.Drawing.Point(12, 73);
            this.DestinationFolderBox.Name = "DestinationFolderBox";
            this.DestinationFolderBox.Size = new System.Drawing.Size(579, 23);
            this.DestinationFolderBox.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 323);
            this.Controls.Add(this.BrowseDestinationButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DestinationFolderBox);
            this.Controls.Add(this.FoundFilesLabel);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.CompressButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TargetFolderBox);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "Directory Compressor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TargetFolderBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ActivityLog;
        private System.Windows.Forms.Button CompressButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Timer RunTimer;
        private System.Windows.Forms.Label FoundFilesLabel;
        private System.Windows.Forms.Button BrowseDestinationButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DestinationFolderBox;
    }
}

