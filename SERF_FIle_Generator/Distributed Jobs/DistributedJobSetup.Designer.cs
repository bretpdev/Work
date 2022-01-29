namespace SERF_File_Generator
{
    partial class DistributedJobSetup
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
            this.WorkspaceLocationText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.JobsList = new System.Windows.Forms.ListBox();
            this.JoinButton = new System.Windows.Forms.Button();
            this.NewJobButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Distributed Workspace Location";
            // 
            // WorkspaceLocationText
            // 
            this.WorkspaceLocationText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WorkspaceLocationText.Location = new System.Drawing.Point(13, 30);
            this.WorkspaceLocationText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.WorkspaceLocationText.Name = "WorkspaceLocationText";
            this.WorkspaceLocationText.Size = new System.Drawing.Size(544, 26);
            this.WorkspaceLocationText.TabIndex = 2;
            this.WorkspaceLocationText.Text = "Q:\\Support Services\\Evan\\DistributionWorkspace";
            this.WorkspaceLocationText.TextChanged += new System.EventHandler(this.WorkspaceLocationText_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Available Jobs";
            // 
            // JobsList
            // 
            this.JobsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.JobsList.FormattingEnabled = true;
            this.JobsList.ItemHeight = 18;
            this.JobsList.Location = new System.Drawing.Point(14, 80);
            this.JobsList.Name = "JobsList";
            this.JobsList.Size = new System.Drawing.Size(544, 94);
            this.JobsList.TabIndex = 5;
            this.JobsList.SelectedIndexChanged += new System.EventHandler(this.JobsList_SelectedIndexChanged);
            // 
            // JoinButton
            // 
            this.JoinButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.JoinButton.Enabled = false;
            this.JoinButton.Location = new System.Drawing.Point(382, 180);
            this.JoinButton.Name = "JoinButton";
            this.JoinButton.Size = new System.Drawing.Size(176, 38);
            this.JoinButton.TabIndex = 6;
            this.JoinButton.Text = "Join Selected Job";
            this.JoinButton.UseVisualStyleBackColor = true;
            this.JoinButton.Click += new System.EventHandler(this.JoinButton_Click);
            // 
            // NewJobButton
            // 
            this.NewJobButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewJobButton.Enabled = false;
            this.NewJobButton.Location = new System.Drawing.Point(13, 180);
            this.NewJobButton.Name = "NewJobButton";
            this.NewJobButton.Size = new System.Drawing.Size(176, 38);
            this.NewJobButton.TabIndex = 7;
            this.NewJobButton.Text = "Start New Job";
            this.NewJobButton.UseVisualStyleBackColor = true;
            this.NewJobButton.Click += new System.EventHandler(this.NewJobButton_Click);
            // 
            // DistributedJobSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 231);
            this.Controls.Add(this.NewJobButton);
            this.Controls.Add(this.JoinButton);
            this.Controls.Add(this.JobsList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.WorkspaceLocationText);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DistributedJobSetup";
            this.Text = "Start/Join Distributed Job";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox WorkspaceLocationText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox JobsList;
        private System.Windows.Forms.Button JoinButton;
        private System.Windows.Forms.Button NewJobButton;
    }
}