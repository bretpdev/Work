namespace GitCloner
{
    partial class MainForm
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
            this.ApiKeyBox = new System.Windows.Forms.TextBox();
            this.CloneLocationBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.CleanBox = new System.Windows.Forms.CheckBox();
            this.BranchesBox = new System.Windows.Forms.CheckBox();
            this.CloneButton = new System.Windows.Forms.Button();
            this.ProgressGroup = new System.Windows.Forms.GroupBox();
            this.ProgressBox = new System.Windows.Forms.TextBox();
            this.ManageLink = new System.Windows.Forms.LinkLabel();
            this.ProgressGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "API Key";
            // 
            // ApiKeyBox
            // 
            this.ApiKeyBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ApiKeyBox.Location = new System.Drawing.Point(12, 30);
            this.ApiKeyBox.Name = "ApiKeyBox";
            this.ApiKeyBox.Size = new System.Drawing.Size(471, 26);
            this.ApiKeyBox.TabIndex = 1;
            this.ApiKeyBox.TextChanged += new System.EventHandler(this.Setting_Changed);
            // 
            // CloneLocationBox
            // 
            this.CloneLocationBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CloneLocationBox.Location = new System.Drawing.Point(12, 80);
            this.CloneLocationBox.Name = "CloneLocationBox";
            this.CloneLocationBox.Size = new System.Drawing.Size(436, 26);
            this.CloneLocationBox.TabIndex = 3;
            this.CloneLocationBox.TextChanged += new System.EventHandler(this.Setting_Changed);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Clone Location";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Location = new System.Drawing.Point(454, 80);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(29, 26);
            this.BrowseButton.TabIndex = 4;
            this.BrowseButton.Text = "...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // CleanBox
            // 
            this.CleanBox.AutoSize = true;
            this.CleanBox.Location = new System.Drawing.Point(12, 118);
            this.CleanBox.Name = "CleanBox";
            this.CleanBox.Size = new System.Drawing.Size(103, 22);
            this.CleanBox.TabIndex = 5;
            this.CleanBox.Text = "Clean First";
            this.CleanBox.UseVisualStyleBackColor = true;
            this.CleanBox.CheckedChanged += new System.EventHandler(this.Setting_Changed);
            // 
            // BranchesBox
            // 
            this.BranchesBox.AutoSize = true;
            this.BranchesBox.Location = new System.Drawing.Point(121, 118);
            this.BranchesBox.Name = "BranchesBox";
            this.BranchesBox.Size = new System.Drawing.Size(145, 22);
            this.BranchesBox.TabIndex = 6;
            this.BranchesBox.Text = "Include Branches";
            this.BranchesBox.UseVisualStyleBackColor = true;
            this.BranchesBox.Visible = false;
            this.BranchesBox.CheckedChanged += new System.EventHandler(this.Setting_Changed);
            // 
            // CloneButton
            // 
            this.CloneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloneButton.Location = new System.Drawing.Point(373, 112);
            this.CloneButton.Name = "CloneButton";
            this.CloneButton.Size = new System.Drawing.Size(110, 32);
            this.CloneButton.TabIndex = 7;
            this.CloneButton.Text = "Clone";
            this.CloneButton.UseVisualStyleBackColor = true;
            this.CloneButton.Click += new System.EventHandler(this.CloneButton_Click);
            // 
            // ProgressGroup
            // 
            this.ProgressGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressGroup.Controls.Add(this.ProgressBox);
            this.ProgressGroup.Location = new System.Drawing.Point(12, 146);
            this.ProgressGroup.Name = "ProgressGroup";
            this.ProgressGroup.Size = new System.Drawing.Size(471, 322);
            this.ProgressGroup.TabIndex = 8;
            this.ProgressGroup.TabStop = false;
            this.ProgressGroup.Text = "Progress";
            // 
            // ProgressBox
            // 
            this.ProgressBox.BackColor = System.Drawing.Color.White;
            this.ProgressBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgressBox.Location = new System.Drawing.Point(3, 22);
            this.ProgressBox.Multiline = true;
            this.ProgressBox.Name = "ProgressBox";
            this.ProgressBox.ReadOnly = true;
            this.ProgressBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ProgressBox.Size = new System.Drawing.Size(465, 297);
            this.ProgressBox.TabIndex = 0;
            // 
            // ManageLink
            // 
            this.ManageLink.AutoSize = true;
            this.ManageLink.Location = new System.Drawing.Point(418, 9);
            this.ManageLink.Name = "ManageLink";
            this.ManageLink.Size = new System.Drawing.Size(65, 18);
            this.ManageLink.TabIndex = 9;
            this.ManageLink.TabStop = true;
            this.ManageLink.Text = "Manage";
            this.ManageLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ManageLink_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 480);
            this.Controls.Add(this.ManageLink);
            this.Controls.Add(this.ProgressGroup);
            this.Controls.Add(this.CloneButton);
            this.Controls.Add(this.BranchesBox);
            this.Controls.Add(this.CleanBox);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.CloneLocationBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ApiKeyBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Git Cloner";
            this.ProgressGroup.ResumeLayout(false);
            this.ProgressGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ApiKeyBox;
        private System.Windows.Forms.TextBox CloneLocationBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.CheckBox CleanBox;
        private System.Windows.Forms.CheckBox BranchesBox;
        private System.Windows.Forms.Button CloneButton;
        private System.Windows.Forms.GroupBox ProgressGroup;
        private System.Windows.Forms.TextBox ProgressBox;
        private System.Windows.Forms.LinkLabel ManageLink;
    }
}

