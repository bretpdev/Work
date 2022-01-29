namespace IMGHISTFED
{
    partial class ActivityForm
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.counter = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Imaging = new System.Windows.Forms.Button();
            this.Tdrive = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.VersionNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 104);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(459, 41);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 0;
            // 
            // counter
            // 
            this.counter.AutoSize = true;
            this.counter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.counter.Location = new System.Drawing.Point(314, 71);
            this.counter.Name = "counter";
            this.counter.Size = new System.Drawing.Size(0, 20);
            this.counter.TabIndex = 2;
            this.counter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(253, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Send to Imaging or T Drive?";
            // 
            // Imaging
            // 
            this.Imaging.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Imaging.Location = new System.Drawing.Point(239, 198);
            this.Imaging.Name = "Imaging";
            this.Imaging.Size = new System.Drawing.Size(106, 33);
            this.Imaging.TabIndex = 3;
            this.Imaging.Text = "Image";
            this.Imaging.UseVisualStyleBackColor = true;
            this.Imaging.Click += new System.EventHandler(this.Imaging_Click);
            // 
            // Tdrive
            // 
            this.Tdrive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Tdrive.Location = new System.Drawing.Point(365, 198);
            this.Tdrive.Name = "Tdrive";
            this.Tdrive.Size = new System.Drawing.Size(106, 33);
            this.Tdrive.TabIndex = 4;
            this.Tdrive.Text = "T Drive";
            this.Tdrive.UseVisualStyleBackColor = true;
            this.Tdrive.Click += new System.EventHandler(this.Tdrive_Click);
            // 
            // Cancel
            // 
            this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.Location = new System.Drawing.Point(16, 198);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(106, 33);
            this.Cancel.TabIndex = 6;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(44, 18);
            this.label3.MaximumSize = new System.Drawing.Size(420, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(393, 40);
            this.label3.TabIndex = 7;
            this.label3.Text = "This is the Activity History Report--Imaging FED script. Choose where to store th" +
    "e files, or Cancel to quit.";
            // 
            // VersionNumber
            // 
            this.VersionNumber.AutoSize = true;
            this.VersionNumber.Location = new System.Drawing.Point(379, 244);
            this.VersionNumber.Name = "VersionNumber";
            this.VersionNumber.Size = new System.Drawing.Size(35, 13);
            this.VersionNumber.TabIndex = 8;
            this.VersionNumber.Text = "label1";
            // 
            // ActivityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 266);
            this.Controls.Add(this.VersionNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Imaging);
            this.Controls.Add(this.Tdrive);
            this.Controls.Add(this.counter);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(504, 300);
            this.MinimumSize = new System.Drawing.Size(504, 300);
            this.Name = "ActivityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Activity History Report Federal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ActivityForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label counter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Imaging;
        private System.Windows.Forms.Button Tdrive;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label VersionNumber;
    }
}