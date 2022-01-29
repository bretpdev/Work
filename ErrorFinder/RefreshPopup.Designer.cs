namespace ErrorFinder
{
    partial class RefreshPopup
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
            this.ErrorList = new System.Windows.Forms.ListBox();
            this.RefreshProgress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // ErrorList
            // 
            this.ErrorList.FormattingEnabled = true;
            this.ErrorList.ItemHeight = 20;
            this.ErrorList.Location = new System.Drawing.Point(18, 63);
            this.ErrorList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ErrorList.Name = "ErrorList";
            this.ErrorList.Size = new System.Drawing.Size(498, 104);
            this.ErrorList.TabIndex = 2;
            // 
            // RefreshProgress
            // 
            this.RefreshProgress.Location = new System.Drawing.Point(18, 18);
            this.RefreshProgress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RefreshProgress.Name = "RefreshProgress";
            this.RefreshProgress.Size = new System.Drawing.Size(500, 35);
            this.RefreshProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.RefreshProgress.TabIndex = 3;
            // 
            // RefreshPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 185);
            this.Controls.Add(this.RefreshProgress);
            this.Controls.Add(this.ErrorList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RefreshPopup";
            this.Text = "Refreshing opsdev.EA27_BANA.dbo.Borrower_Errors_All";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadForm_FormClosing);
            this.Shown += new System.EventHandler(this.RefreshPopup_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ErrorList;
        private System.Windows.Forms.ProgressBar RefreshProgress;
    }
}