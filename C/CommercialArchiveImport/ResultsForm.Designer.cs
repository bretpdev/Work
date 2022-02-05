namespace CommercialArchiveImport
{
    partial class ResultsForm
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
            this.EojButton = new CommercialArchiveImport.PaleButton();
            this.EojFolderButton = new CommercialArchiveImport.PaleButton();
            this.CopyResultsButton = new CommercialArchiveImport.PaleButton();
            this.ImagingServerButton = new CommercialArchiveImport.PaleButton();
            this.RemainingImagesButton = new CommercialArchiveImport.PaleButton();
            this.ViewResultsButton = new CommercialArchiveImport.PaleButton();
            this.SuspendLayout();
            // 
            // EojButton
            // 
            this.EojButton.BackgroundImage = global::CommercialArchiveImport.Properties.Resources.file;
            this.EojButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.EojButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EojButton.Location = new System.Drawing.Point(12, 8);
            this.EojButton.Name = "EojButton";
            this.EojButton.Size = new System.Drawing.Size(128, 128);
            this.EojButton.TabIndex = 0;
            this.EojButton.Text = "End of Job Report";
            this.EojButton.UseVisualStyleBackColor = true;
            this.EojButton.Click += new System.EventHandler(this.EojButton_Click);
            // 
            // EojFolderButton
            // 
            this.EojFolderButton.BackgroundImage = global::CommercialArchiveImport.Properties.Resources.folder;
            this.EojFolderButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.EojFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EojFolderButton.Location = new System.Drawing.Point(13, 142);
            this.EojFolderButton.Name = "EojFolderButton";
            this.EojFolderButton.Size = new System.Drawing.Size(128, 128);
            this.EojFolderButton.TabIndex = 1;
            this.EojFolderButton.Text = "View End of Job Folder";
            this.EojFolderButton.UseVisualStyleBackColor = true;
            this.EojFolderButton.Click += new System.EventHandler(this.EojFolderButton_Click);
            // 
            // CopyResultsButton
            // 
            this.CopyResultsButton.BackgroundImage = global::CommercialArchiveImport.Properties.Resources.move;
            this.CopyResultsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CopyResultsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CopyResultsButton.Location = new System.Drawing.Point(146, 8);
            this.CopyResultsButton.Name = "CopyResultsButton";
            this.CopyResultsButton.Size = new System.Drawing.Size(128, 128);
            this.CopyResultsButton.TabIndex = 2;
            this.CopyResultsButton.Text = "Copy Results to Load Folder";
            this.CopyResultsButton.UseVisualStyleBackColor = true;
            this.CopyResultsButton.Click += new System.EventHandler(this.CopyResultsButton_Click);
            // 
            // ImagingServerButton
            // 
            this.ImagingServerButton.BackgroundImage = global::CommercialArchiveImport.Properties.Resources.networkfolder;
            this.ImagingServerButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ImagingServerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImagingServerButton.Location = new System.Drawing.Point(146, 142);
            this.ImagingServerButton.Name = "ImagingServerButton";
            this.ImagingServerButton.Size = new System.Drawing.Size(128, 128);
            this.ImagingServerButton.TabIndex = 3;
            this.ImagingServerButton.Text = "View Load Folder";
            this.ImagingServerButton.UseVisualStyleBackColor = true;
            this.ImagingServerButton.Click += new System.EventHandler(this.ImagingServerButton_Click);
            // 
            // RemainingImagesButton
            // 
            this.RemainingImagesButton.BackgroundImage = global::CommercialArchiveImport.Properties.Resources.zip;
            this.RemainingImagesButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RemainingImagesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemainingImagesButton.Location = new System.Drawing.Point(281, 8);
            this.RemainingImagesButton.Name = "RemainingImagesButton";
            this.RemainingImagesButton.Size = new System.Drawing.Size(128, 128);
            this.RemainingImagesButton.TabIndex = 6;
            this.RemainingImagesButton.Text = "Remaining Images";
            this.RemainingImagesButton.UseVisualStyleBackColor = true;
            this.RemainingImagesButton.Click += new System.EventHandler(this.RemainingImagesButton_Click);
            // 
            // ViewResultsButton
            // 
            this.ViewResultsButton.BackgroundImage = global::CommercialArchiveImport.Properties.Resources.folder;
            this.ViewResultsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ViewResultsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ViewResultsButton.Location = new System.Drawing.Point(280, 142);
            this.ViewResultsButton.Name = "ViewResultsButton";
            this.ViewResultsButton.Size = new System.Drawing.Size(128, 128);
            this.ViewResultsButton.TabIndex = 7;
            this.ViewResultsButton.Text = "View Results Folder";
            this.ViewResultsButton.UseVisualStyleBackColor = true;
            this.ViewResultsButton.Click += new System.EventHandler(this.ViewResultsButton_Click);
            // 
            // ResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(421, 281);
            this.Controls.Add(this.ViewResultsButton);
            this.Controls.Add(this.EojFolderButton);
            this.Controls.Add(this.EojButton);
            this.Controls.Add(this.ImagingServerButton);
            this.Controls.Add(this.CopyResultsButton);
            this.Controls.Add(this.RemainingImagesButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResultsForm";
            this.Text = "Results";
            this.Load += new System.EventHandler(this.ResultsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CommercialArchiveImport.PaleButton EojButton;
        private CommercialArchiveImport.PaleButton EojFolderButton;
        private CommercialArchiveImport.PaleButton ImagingServerButton;
        private CommercialArchiveImport.PaleButton CopyResultsButton;
        private PaleButton RemainingImagesButton;
        private PaleButton ViewResultsButton;
    }
}