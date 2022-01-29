namespace AutomatedImageImporter
{
    partial class StartForm
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
            this.SourceBrowseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SourceZipText = new System.Windows.Forms.TextBox();
            this.MonitorBrowseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.MonitorFolderText = new System.Windows.Forms.TextBox();
            this.LineNumberPicker = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BeginButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LineNumberPicker)).BeginInit();
            this.SuspendLayout();
            // 
            // SourceBrowseButton
            // 
            this.SourceBrowseButton.Location = new System.Drawing.Point(299, 54);
            this.SourceBrowseButton.Name = "SourceBrowseButton";
            this.SourceBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.SourceBrowseButton.TabIndex = 8;
            this.SourceBrowseButton.Text = "Browse...";
            this.SourceBrowseButton.UseVisualStyleBackColor = true;
            this.SourceBrowseButton.Click += new System.EventHandler(this.SourceBrowseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Source Zip";
            // 
            // SourceZipText
            // 
            this.SourceZipText.Location = new System.Drawing.Point(11, 28);
            this.SourceZipText.Name = "SourceZipText";
            this.SourceZipText.Size = new System.Drawing.Size(363, 20);
            this.SourceZipText.TabIndex = 6;
            // 
            // MonitorBrowseButton
            // 
            this.MonitorBrowseButton.Location = new System.Drawing.Point(299, 109);
            this.MonitorBrowseButton.Name = "MonitorBrowseButton";
            this.MonitorBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.MonitorBrowseButton.TabIndex = 11;
            this.MonitorBrowseButton.Text = "Browse...";
            this.MonitorBrowseButton.UseVisualStyleBackColor = true;
            this.MonitorBrowseButton.Click += new System.EventHandler(this.MonitorBrowseButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Imaging Monitor Folder";
            // 
            // MonitorFolderText
            // 
            this.MonitorFolderText.Location = new System.Drawing.Point(11, 83);
            this.MonitorFolderText.Name = "MonitorFolderText";
            this.MonitorFolderText.Size = new System.Drawing.Size(363, 20);
            this.MonitorFolderText.TabIndex = 9;
            this.MonitorFolderText.Text = "\\\\imgdevkofax\\ascent$\\FederalImport\\Monitor";
            // 
            // LineNumberPicker
            // 
            this.LineNumberPicker.Location = new System.Drawing.Point(62, 117);
            this.LineNumberPicker.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.LineNumberPicker.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.LineNumberPicker.Name = "LineNumberPicker";
            this.LineNumberPicker.Size = new System.Drawing.Size(62, 20);
            this.LineNumberPicker.TabIndex = 12;
            this.LineNumberPicker.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(130, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "index lines at a time.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Process";
            // 
            // BeginButton
            // 
            this.BeginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BeginButton.Location = new System.Drawing.Point(280, 147);
            this.BeginButton.Name = "BeginButton";
            this.BeginButton.Size = new System.Drawing.Size(97, 36);
            this.BeginButton.TabIndex = 15;
            this.BeginButton.Text = "Begin";
            this.BeginButton.UseVisualStyleBackColor = true;
            this.BeginButton.Click += new System.EventHandler(this.BeginButton_Click);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 195);
            this.Controls.Add(this.BeginButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LineNumberPicker);
            this.Controls.Add(this.MonitorBrowseButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MonitorFolderText);
            this.Controls.Add(this.SourceBrowseButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SourceZipText);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartForm";
            this.Text = "Automated Image Importer";
            this.Load += new System.EventHandler(this.StartForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LineNumberPicker)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SourceBrowseButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SourceZipText;
        private System.Windows.Forms.Button MonitorBrowseButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox MonitorFolderText;
        private System.Windows.Forms.NumericUpDown LineNumberPicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BeginButton;
    }
}