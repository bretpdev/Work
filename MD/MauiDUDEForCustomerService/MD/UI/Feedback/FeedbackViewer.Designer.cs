namespace MD
{
    partial class FeedbackViewer
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
            this.DetailsBox = new System.Windows.Forms.TextBox();
            this.MyQuestionsGrid = new System.Windows.Forms.DataGridView();
            this.FeedbackTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestedOnColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestedByColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppVersionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeedbackDetailsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.ScreenshotBox = new System.Windows.Forms.PictureBox();
            this.SubmittedLabel = new System.Windows.Forms.Label();
            this.ProcessedButton = new System.Windows.Forms.Button();
            this.ReflectionScreenshotBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.MyQuestionsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenshotBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReflectionScreenshotBox)).BeginInit();
            this.SuspendLayout();
            // 
            // DetailsBox
            // 
            this.DetailsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.DetailsBox.BackColor = System.Drawing.Color.White;
            this.DetailsBox.Location = new System.Drawing.Point(12, 194);
            this.DetailsBox.MaxLength = 4000;
            this.DetailsBox.Multiline = true;
            this.DetailsBox.Name = "DetailsBox";
            this.DetailsBox.ReadOnly = true;
            this.DetailsBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.DetailsBox.Size = new System.Drawing.Size(687, 315);
            this.DetailsBox.TabIndex = 0;
            // 
            // MyQuestionsGrid
            // 
            this.MyQuestionsGrid.AllowUserToAddRows = false;
            this.MyQuestionsGrid.AllowUserToDeleteRows = false;
            this.MyQuestionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MyQuestionsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MyQuestionsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FeedbackTypeColumn,
            this.RequestedOnColumn,
            this.RequestedByColumn,
            this.AppVersionColumn,
            this.FeedbackDetailsColumn});
            this.MyQuestionsGrid.Location = new System.Drawing.Point(12, 12);
            this.MyQuestionsGrid.Name = "MyQuestionsGrid";
            this.MyQuestionsGrid.ReadOnly = true;
            this.MyQuestionsGrid.RowHeadersVisible = false;
            this.MyQuestionsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MyQuestionsGrid.Size = new System.Drawing.Size(891, 150);
            this.MyQuestionsGrid.TabIndex = 19;
            this.MyQuestionsGrid.SelectionChanged += new System.EventHandler(this.MyQuestionsGrid_SelectionChanged);
            // 
            // FeedbackTypeColumn
            // 
            this.FeedbackTypeColumn.DataPropertyName = "FeedbackType";
            this.FeedbackTypeColumn.HeaderText = "Type";
            this.FeedbackTypeColumn.Name = "FeedbackTypeColumn";
            this.FeedbackTypeColumn.ReadOnly = true;
            // 
            // RequestedOnColumn
            // 
            this.RequestedOnColumn.DataPropertyName = "RequestedOnString";
            this.RequestedOnColumn.HeaderText = "Requested On";
            this.RequestedOnColumn.Name = "RequestedOnColumn";
            this.RequestedOnColumn.ReadOnly = true;
            this.RequestedOnColumn.Width = 150;
            // 
            // RequestedByColumn
            // 
            this.RequestedByColumn.DataPropertyName = "RequestedByString";
            this.RequestedByColumn.HeaderText = "Requested By";
            this.RequestedByColumn.Name = "RequestedByColumn";
            this.RequestedByColumn.ReadOnly = true;
            this.RequestedByColumn.Width = 130;
            // 
            // AppVersionColumn
            // 
            this.AppVersionColumn.DataPropertyName = "AppVersion";
            this.AppVersionColumn.HeaderText = "Version";
            this.AppVersionColumn.Name = "AppVersionColumn";
            this.AppVersionColumn.ReadOnly = true;
            // 
            // FeedbackDetailsColumn
            // 
            this.FeedbackDetailsColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FeedbackDetailsColumn.DataPropertyName = "FeedbackDetails";
            this.FeedbackDetailsColumn.HeaderText = "Details";
            this.FeedbackDetailsColumn.Name = "FeedbackDetailsColumn";
            this.FeedbackDetailsColumn.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14F);
            this.label1.Location = new System.Drawing.Point(8, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 22);
            this.label1.TabIndex = 20;
            this.label1.Text = "Details";
            // 
            // ScreenshotBox
            // 
            this.ScreenshotBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScreenshotBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ScreenshotBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ScreenshotBox.Location = new System.Drawing.Point(705, 194);
            this.ScreenshotBox.Name = "ScreenshotBox";
            this.ScreenshotBox.Size = new System.Drawing.Size(198, 156);
            this.ScreenshotBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ScreenshotBox.TabIndex = 21;
            this.ScreenshotBox.TabStop = false;
            this.ScreenshotBox.Click += new System.EventHandler(this.ScreenshotBox_Click);
            // 
            // SubmittedLabel
            // 
            this.SubmittedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SubmittedLabel.AutoSize = true;
            this.SubmittedLabel.Font = new System.Drawing.Font("Arial", 14F);
            this.SubmittedLabel.Location = new System.Drawing.Point(8, 525);
            this.SubmittedLabel.Name = "SubmittedLabel";
            this.SubmittedLabel.Size = new System.Drawing.Size(245, 22);
            this.SubmittedLabel.TabIndex = 22;
            this.SubmittedLabel.Text = "Submitted by blank on blank";
            this.SubmittedLabel.Visible = false;
            // 
            // ProcessedButton
            // 
            this.ProcessedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ProcessedButton.Enabled = false;
            this.ProcessedButton.Font = new System.Drawing.Font("Arial", 14F);
            this.ProcessedButton.Location = new System.Drawing.Point(705, 515);
            this.ProcessedButton.Name = "ProcessedButton";
            this.ProcessedButton.Size = new System.Drawing.Size(198, 43);
            this.ProcessedButton.TabIndex = 23;
            this.ProcessedButton.Text = "Mark as Processed";
            this.ProcessedButton.UseVisualStyleBackColor = true;
            this.ProcessedButton.Click += new System.EventHandler(this.ProcessedButton_Click);
            // 
            // ReflectionScreenshotBox
            // 
            this.ReflectionScreenshotBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReflectionScreenshotBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ReflectionScreenshotBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ReflectionScreenshotBox.Location = new System.Drawing.Point(705, 353);
            this.ReflectionScreenshotBox.Name = "ReflectionScreenshotBox";
            this.ReflectionScreenshotBox.Size = new System.Drawing.Size(198, 156);
            this.ReflectionScreenshotBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ReflectionScreenshotBox.TabIndex = 24;
            this.ReflectionScreenshotBox.TabStop = false;
            this.ReflectionScreenshotBox.Click += new System.EventHandler(this.ReflectionScreenshotBox_Click);
            // 
            // FeedbackViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 570);
            this.Controls.Add(this.ReflectionScreenshotBox);
            this.Controls.Add(this.ProcessedButton);
            this.Controls.Add(this.SubmittedLabel);
            this.Controls.Add(this.ScreenshotBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MyQuestionsGrid);
            this.Controls.Add(this.DetailsBox);
            this.MinimumSize = new System.Drawing.Size(931, 609);
            this.Name = "FeedbackViewer";
            this.Text = "Manage Feedback";
            ((System.ComponentModel.ISupportInitialize)(this.MyQuestionsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenshotBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReflectionScreenshotBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DetailsBox;
        private System.Windows.Forms.DataGridView MyQuestionsGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox ScreenshotBox;
        private System.Windows.Forms.Label SubmittedLabel;
        private System.Windows.Forms.Button ProcessedButton;
        private System.Windows.Forms.PictureBox ReflectionScreenshotBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeedbackTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestedOnColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestedByColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppVersionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeedbackDetailsColumn;
    }
}