namespace CommercialArchiveImport
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
            this.ZipLocationText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ZipBrowseButton = new System.Windows.Forms.Button();
            this.ResultsBrowseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ResultsLocationText = new System.Windows.Forms.TextBox();
            this.ImportButton = new System.Windows.Forms.Button();
            this.SetupGroup = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BatchNumberText = new System.Windows.Forms.TextBox();
            this.ResultsList = new System.Windows.Forms.ListBox();
            this.ResultsLabel = new System.Windows.Forms.Label();
            this.SetupGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ZipLocationText
            // 
            this.ZipLocationText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ZipLocationText.Location = new System.Drawing.Point(6, 48);
            this.ZipLocationText.Name = "ZipLocationText";
            this.ZipLocationText.Size = new System.Drawing.Size(297, 20);
            this.ZipLocationText.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Zip Location";
            // 
            // ZipBrowseButton
            // 
            this.ZipBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZipBrowseButton.Location = new System.Drawing.Point(228, 74);
            this.ZipBrowseButton.Name = "ZipBrowseButton";
            this.ZipBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.ZipBrowseButton.TabIndex = 2;
            this.ZipBrowseButton.Text = "Browse";
            this.ZipBrowseButton.UseVisualStyleBackColor = true;
            this.ZipBrowseButton.Click += new System.EventHandler(this.ZipBrowseButton_Click);
            // 
            // ResultsBrowseButton
            // 
            this.ResultsBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsBrowseButton.Location = new System.Drawing.Point(228, 129);
            this.ResultsBrowseButton.Name = "ResultsBrowseButton";
            this.ResultsBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.ResultsBrowseButton.TabIndex = 5;
            this.ResultsBrowseButton.Text = "Browse";
            this.ResultsBrowseButton.UseVisualStyleBackColor = true;
            this.ResultsBrowseButton.Click += new System.EventHandler(this.ResultsBrowseButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Results Location";
            // 
            // ResultsLocationText
            // 
            this.ResultsLocationText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsLocationText.Location = new System.Drawing.Point(6, 103);
            this.ResultsLocationText.Name = "ResultsLocationText";
            this.ResultsLocationText.Size = new System.Drawing.Size(297, 20);
            this.ResultsLocationText.TabIndex = 3;
            // 
            // ImportButton
            // 
            this.ImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ImportButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImportButton.Location = new System.Drawing.Point(224, 220);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(99, 37);
            this.ImportButton.TabIndex = 6;
            this.ImportButton.Text = "Generate";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // SetupGroup
            // 
            this.SetupGroup.Controls.Add(this.label3);
            this.SetupGroup.Controls.Add(this.BatchNumberText);
            this.SetupGroup.Controls.Add(this.label1);
            this.SetupGroup.Controls.Add(this.ZipLocationText);
            this.SetupGroup.Controls.Add(this.ResultsBrowseButton);
            this.SetupGroup.Controls.Add(this.ZipBrowseButton);
            this.SetupGroup.Controls.Add(this.label2);
            this.SetupGroup.Controls.Add(this.ResultsLocationText);
            this.SetupGroup.Location = new System.Drawing.Point(8, 12);
            this.SetupGroup.Name = "SetupGroup";
            this.SetupGroup.Size = new System.Drawing.Size(315, 198);
            this.SetupGroup.TabIndex = 7;
            this.SetupGroup.TabStop = false;
            this.SetupGroup.Text = "Setup";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Batch Number (optional)";
            // 
            // BatchNumberText
            // 
            this.BatchNumberText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BatchNumberText.Location = new System.Drawing.Point(7, 160);
            this.BatchNumberText.MaxLength = 8;
            this.BatchNumberText.Name = "BatchNumberText";
            this.BatchNumberText.Size = new System.Drawing.Size(121, 20);
            this.BatchNumberText.TabIndex = 6;
            // 
            // ResultsList
            // 
            this.ResultsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsList.FormattingEnabled = true;
            this.ResultsList.Location = new System.Drawing.Point(329, 14);
            this.ResultsList.Name = "ResultsList";
            this.ResultsList.Size = new System.Drawing.Size(413, 238);
            this.ResultsList.TabIndex = 8;
            // 
            // ResultsLabel
            // 
            this.ResultsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ResultsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResultsLabel.Location = new System.Drawing.Point(14, 220);
            this.ResultsLabel.Name = "ResultsLabel";
            this.ResultsLabel.Size = new System.Drawing.Size(204, 37);
            this.ResultsLabel.TabIndex = 9;
            this.ResultsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 263);
            this.Controls.Add(this.ResultsLabel);
            this.Controls.Add(this.ResultsList);
            this.Controls.Add(this.SetupGroup);
            this.Controls.Add(this.ImportButton);
            this.MinimumSize = new System.Drawing.Size(767, 301);
            this.Name = "MainForm";
            this.Text = "Commercial Image Importer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.SetupGroup.ResumeLayout(false);
            this.SetupGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox ZipLocationText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ZipBrowseButton;
        private System.Windows.Forms.Button ResultsBrowseButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ResultsLocationText;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.GroupBox SetupGroup;
        private System.Windows.Forms.ListBox ResultsList;
        private System.Windows.Forms.Label ResultsLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox BatchNumberText;
    }
}

