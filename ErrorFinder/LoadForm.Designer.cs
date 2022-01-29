namespace ErrorFinder
{
    partial class LoadForm
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
            this.LoadProgress = new System.Windows.Forms.ProgressBar();
            this.RefreshCheck = new System.Windows.Forms.CheckBox();
            this.GenerateCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // LoadProgress
            // 
            this.LoadProgress.Location = new System.Drawing.Point(18, 18);
            this.LoadProgress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LoadProgress.Name = "LoadProgress";
            this.LoadProgress.Size = new System.Drawing.Size(483, 35);
            this.LoadProgress.TabIndex = 0;
            // 
            // RefreshCheck
            // 
            this.RefreshCheck.AutoSize = true;
            this.RefreshCheck.Location = new System.Drawing.Point(18, 63);
            this.RefreshCheck.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RefreshCheck.Name = "RefreshCheck";
            this.RefreshCheck.Size = new System.Drawing.Size(506, 24);
            this.RefreshCheck.TabIndex = 1;
            this.RefreshCheck.Text = "Immediately refresh opsdev.EA27_BANA.dbo.Borrower_Errors_All ";
            this.RefreshCheck.UseVisualStyleBackColor = true;
            // 
            // GenerateCheck
            // 
            this.GenerateCheck.AutoSize = true;
            this.GenerateCheck.Location = new System.Drawing.Point(18, 92);
            this.GenerateCheck.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GenerateCheck.Name = "GenerateCheck";
            this.GenerateCheck.Size = new System.Drawing.Size(322, 24);
            this.GenerateCheck.TabIndex = 2;
            this.GenerateCheck.Text = "Immediately generate all CSV error views";
            this.GenerateCheck.UseVisualStyleBackColor = true;
            // 
            // LoadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 129);
            this.Controls.Add(this.GenerateCheck);
            this.Controls.Add(this.RefreshCheck);
            this.Controls.Add(this.LoadProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadForm";
            this.Text = "Loading Validation Report";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadForm_FormClosing);
            this.Load += new System.EventHandler(this.LoadForm_Load);
            this.Shown += new System.EventHandler(this.LoadForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar LoadProgress;
        private System.Windows.Forms.CheckBox RefreshCheck;
        private System.Windows.Forms.CheckBox GenerateCheck;
    }
}