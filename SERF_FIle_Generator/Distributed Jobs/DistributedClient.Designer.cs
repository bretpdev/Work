namespace SERF_File_Generator
{
    partial class DistributedClient
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
            this.WorkspaceText = new System.Windows.Forms.TextBox();
            this.JobText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FinishedList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.StopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Workspace";
            // 
            // WorkspaceText
            // 
            this.WorkspaceText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WorkspaceText.BackColor = System.Drawing.SystemColors.Window;
            this.WorkspaceText.Location = new System.Drawing.Point(12, 30);
            this.WorkspaceText.Name = "WorkspaceText";
            this.WorkspaceText.ReadOnly = true;
            this.WorkspaceText.Size = new System.Drawing.Size(371, 26);
            this.WorkspaceText.TabIndex = 1;
            // 
            // JobText
            // 
            this.JobText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.JobText.BackColor = System.Drawing.SystemColors.Window;
            this.JobText.Location = new System.Drawing.Point(12, 80);
            this.JobText.Name = "JobText";
            this.JobText.ReadOnly = true;
            this.JobText.Size = new System.Drawing.Size(371, 26);
            this.JobText.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Job";
            // 
            // FinishedList
            // 
            this.FinishedList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FinishedList.FormattingEnabled = true;
            this.FinishedList.ItemHeight = 18;
            this.FinishedList.Location = new System.Drawing.Point(12, 130);
            this.FinishedList.Name = "FinishedList";
            this.FinishedList.Size = new System.Drawing.Size(371, 184);
            this.FinishedList.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 109);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(330, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Finished Batches (chronologically descending)";
            // 
            // StopButton
            // 
            this.StopButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StopButton.Location = new System.Drawing.Point(12, 320);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(371, 32);
            this.StopButton.TabIndex = 6;
            this.StopButton.Text = "Stop Participating";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // DistributedClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 364);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.FinishedList);
            this.Controls.Add(this.JobText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.WorkspaceText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DistributedClient";
            this.Text = "Distributed Job - Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox WorkspaceText;
        private System.Windows.Forms.TextBox JobText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox FinishedList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button StopButton;
    }
}