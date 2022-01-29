namespace CommentAuditTracker
{
    partial class DeleteAgentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeleteAgentForm));
            this.label1 = new System.Windows.Forms.Label();
            this.AgentNameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FindButton = new System.Windows.Forms.Button();
            this.FindStatusLabel = new System.Windows.Forms.Label();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(431, 61);
            this.label1.TabIndex = 0;
            this.label1.Text = "This form should only be used for the removal of legitimately bad data.  In most " +
    "cases, the agent should be marked as inactive instead of deleted.";
            // 
            // AgentNameBox
            // 
            this.AgentNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AgentNameBox.Location = new System.Drawing.Point(12, 103);
            this.AgentNameBox.Name = "AgentNameBox";
            this.AgentNameBox.Size = new System.Drawing.Size(431, 26);
            this.AgentNameBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enter Agent Name";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(-2, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(464, 2);
            this.label3.TabIndex = 3;
            // 
            // FindButton
            // 
            this.FindButton.Location = new System.Drawing.Point(12, 135);
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(130, 36);
            this.FindButton.TabIndex = 4;
            this.FindButton.Text = "Find Agent";
            this.FindButton.UseVisualStyleBackColor = true;
            this.FindButton.Click += new System.EventHandler(this.FindButton_Click);
            // 
            // FindStatusLabel
            // 
            this.FindStatusLabel.AutoSize = true;
            this.FindStatusLabel.Location = new System.Drawing.Point(148, 144);
            this.FindStatusLabel.Name = "FindStatusLabel";
            this.FindStatusLabel.Size = new System.Drawing.Size(120, 18);
            this.FindStatusLabel.TabIndex = 6;
            this.FindStatusLabel.Text = "No Agent Found";
            this.FindStatusLabel.Visible = false;
            // 
            // DeleteButton
            // 
            this.DeleteButton.ForeColor = System.Drawing.Color.Red;
            this.DeleteButton.Location = new System.Drawing.Point(313, 135);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(130, 36);
            this.DeleteButton.TabIndex = 7;
            this.DeleteButton.Text = "Delete Agent";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Visible = false;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // DeleteAgentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 181);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.FindStatusLabel);
            this.Controls.Add(this.FindButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AgentNameBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteAgentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Delete Agents";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AgentNameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button FindButton;
        private System.Windows.Forms.Label FindStatusLabel;
        private System.Windows.Forms.Button DeleteButton;
    }
}