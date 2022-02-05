namespace IPACPMTFED
{
    partial class Processing
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
            this.processingTxt = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // processingTxt
            // 
            this.processingTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.processingTxt.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processingTxt.Location = new System.Drawing.Point(12, 9);
            this.processingTxt.Name = "processingTxt";
            this.processingTxt.Size = new System.Drawing.Size(569, 35);
            this.processingTxt.TabIndex = 0;
            this.processingTxt.Text = "Reading the csv file please wait...";
            this.processingTxt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(12, 66);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(569, 31);
            this.progress.TabIndex = 1;
            // 
            // Processing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 105);
            this.ControlBox = false;
            this.Controls.Add(this.progress);
            this.Controls.Add(this.processingTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Processing";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label processingTxt;
        private System.Windows.Forms.ProgressBar progress;

    }
}