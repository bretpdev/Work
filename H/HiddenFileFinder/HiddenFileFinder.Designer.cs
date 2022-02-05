﻿namespace HiddenFileFinder
{
    partial class HiddenFileFinder
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtScanRoot = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select a directory  to scan";
            // 
            // txtScanRoot
            // 
            this.txtScanRoot.Location = new System.Drawing.Point(149, 10);
            this.txtScanRoot.Name = "txtScanRoot";
            this.txtScanRoot.Size = new System.Drawing.Size(464, 20);
            this.txtScanRoot.TabIndex = 1;
            this.txtScanRoot.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtScanRoot_KeyUp);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(619, 8);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lstResults
            // 
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new System.Drawing.Point(16, 37);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(678, 459);
            this.lstResults.TabIndex = 3;
            this.lstResults.DoubleClick += new System.EventHandler(this.lstResults_DoubleClick);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(16, 511);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(678, 20);
            this.txtStatus.TabIndex = 4;
            // 
            // HiddenFileFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 543);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.lstResults);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtScanRoot);
            this.Controls.Add(this.label1);
            this.Name = "HiddenFileFinder";
            this.Text = "Hidden File Finder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtScanRoot;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ListBox lstResults;
        private System.Windows.Forms.TextBox txtStatus;
    }
}

