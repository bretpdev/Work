namespace INVOSAMPFD
{
    partial class StatusForm
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
            this.FileLocationBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.SubStatusLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FileLocationBox
            // 
            this.FileLocationBox.Location = new System.Drawing.Point(15, 30);
            this.FileLocationBox.Name = "FileLocationBox";
            this.FileLocationBox.ReadOnly = true;
            this.FileLocationBox.Size = new System.Drawing.Size(717, 26);
            this.FileLocationBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Data File";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(15, 62);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(146, 37);
            this.BrowseButton.TabIndex = 2;
            this.BrowseButton.Text = "Select File...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // GenerateButton
            // 
            this.GenerateButton.Enabled = false;
            this.GenerateButton.Location = new System.Drawing.Point(586, 62);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(146, 37);
            this.GenerateButton.TabIndex = 3;
            this.GenerateButton.Text = "Generate Report";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SubStatusLabel);
            this.groupBox1.Controls.Add(this.StatusLabel);
            this.groupBox1.Controls.Add(this.ProgressBar);
            this.groupBox1.Location = new System.Drawing.Point(12, 105);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(720, 146);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(6, 25);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(705, 43);
            this.ProgressBar.TabIndex = 0;
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.Location = new System.Drawing.Point(6, 71);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(251, 25);
            this.StatusLabel.TabIndex = 1;
            this.StatusLabel.Text = "Waiting for File Selection";
            // 
            // SubStatusLabel
            // 
            this.SubStatusLabel.AutoSize = true;
            this.SubStatusLabel.Location = new System.Drawing.Point(15, 105);
            this.SubStatusLabel.Name = "SubStatusLabel";
            this.SubStatusLabel.Size = new System.Drawing.Size(208, 18);
            this.SubStatusLabel.TabIndex = 2;
            this.SubStatusLabel.Text = "Sub-status information (1/25)";
            // 
            // StatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 268);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.FileLocationBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "StatusForm";
            this.Text = "FSA Monthly Invoice Report";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FileLocationBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label SubStatusLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.ProgressBar ProgressBar;
    }
}