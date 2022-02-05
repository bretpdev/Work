namespace CPINTRTLPD
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
            this.GuarantorBox = new System.Windows.Forms.TextBox();
            this.GuarantorCheck = new System.Windows.Forms.CheckBox();
            this.OwnerCheck = new System.Windows.Forms.CheckBox();
            this.OwnerBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BondCheck = new System.Windows.Forms.CheckBox();
            this.BondBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.InputFileCheck = new System.Windows.Forms.CheckBox();
            this.InputFileBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.ReviewButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Copy to Guarantor (6 characters)";
            // 
            // GuarantorBox
            // 
            this.GuarantorBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GuarantorBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.GuarantorBox.Location = new System.Drawing.Point(12, 28);
            this.GuarantorBox.MaxLength = 6;
            this.GuarantorBox.Name = "GuarantorBox";
            this.GuarantorBox.Size = new System.Drawing.Size(219, 23);
            this.GuarantorBox.TabIndex = 1;
            this.GuarantorBox.TextChanged += new System.EventHandler(this.GuarantorBox_TextChanged);
            // 
            // GuarantorCheck
            // 
            this.GuarantorCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GuarantorCheck.AutoCheck = false;
            this.GuarantorCheck.AutoSize = true;
            this.GuarantorCheck.Location = new System.Drawing.Point(237, 32);
            this.GuarantorCheck.Name = "GuarantorCheck";
            this.GuarantorCheck.Size = new System.Drawing.Size(15, 14);
            this.GuarantorCheck.TabIndex = 2;
            this.GuarantorCheck.TabStop = false;
            this.GuarantorCheck.UseVisualStyleBackColor = true;
            // 
            // OwnerCheck
            // 
            this.OwnerCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OwnerCheck.AutoCheck = false;
            this.OwnerCheck.AutoSize = true;
            this.OwnerCheck.Location = new System.Drawing.Point(238, 77);
            this.OwnerCheck.Name = "OwnerCheck";
            this.OwnerCheck.Size = new System.Drawing.Size(15, 14);
            this.OwnerCheck.TabIndex = 5;
            this.OwnerCheck.TabStop = false;
            this.OwnerCheck.UseVisualStyleBackColor = true;
            // 
            // OwnerBox
            // 
            this.OwnerBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OwnerBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.OwnerBox.Location = new System.Drawing.Point(12, 72);
            this.OwnerBox.MaxLength = 8;
            this.OwnerBox.Name = "OwnerBox";
            this.OwnerBox.Size = new System.Drawing.Size(219, 23);
            this.OwnerBox.TabIndex = 4;
            this.OwnerBox.TextChanged += new System.EventHandler(this.OwnerBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(208, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Copy to Owner (6-8 characters)";
            // 
            // BondCheck
            // 
            this.BondCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BondCheck.AutoCheck = false;
            this.BondCheck.AutoSize = true;
            this.BondCheck.Location = new System.Drawing.Point(238, 121);
            this.BondCheck.Name = "BondCheck";
            this.BondCheck.Size = new System.Drawing.Size(15, 14);
            this.BondCheck.TabIndex = 8;
            this.BondCheck.TabStop = false;
            this.BondCheck.UseVisualStyleBackColor = true;
            // 
            // BondBox
            // 
            this.BondBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BondBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.BondBox.Location = new System.Drawing.Point(12, 116);
            this.BondBox.MaxLength = 8;
            this.BondBox.Name = "BondBox";
            this.BondBox.Size = new System.Drawing.Size(219, 23);
            this.BondBox.TabIndex = 7;
            this.BondBox.TextChanged += new System.EventHandler(this.BondBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Copy to Bond (6-8 characters)";
            // 
            // ContinueButton
            // 
            this.ContinueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ContinueButton.Enabled = false;
            this.ContinueButton.Location = new System.Drawing.Point(144, 231);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(109, 32);
            this.ContinueButton.TabIndex = 9;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = true;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // InputFileCheck
            // 
            this.InputFileCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InputFileCheck.AutoCheck = false;
            this.InputFileCheck.AutoSize = true;
            this.InputFileCheck.Location = new System.Drawing.Point(238, 165);
            this.InputFileCheck.Name = "InputFileCheck";
            this.InputFileCheck.Size = new System.Drawing.Size(15, 14);
            this.InputFileCheck.TabIndex = 12;
            this.InputFileCheck.TabStop = false;
            this.InputFileCheck.UseVisualStyleBackColor = true;
            // 
            // InputFileBox
            // 
            this.InputFileBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputFileBox.Location = new System.Drawing.Point(12, 160);
            this.InputFileBox.Name = "InputFileBox";
            this.InputFileBox.Size = new System.Drawing.Size(219, 23);
            this.InputFileBox.TabIndex = 11;
            this.InputFileBox.TextChanged += new System.EventHandler(this.InputFileBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "Input File Location";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(12, 189);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(92, 24);
            this.BrowseButton.TabIndex = 13;
            this.BrowseButton.Text = "Browse...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(0, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(265, 1);
            this.label5.TabIndex = 14;
            // 
            // ReviewButton
            // 
            this.ReviewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ReviewButton.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReviewButton.Location = new System.Drawing.Point(12, 231);
            this.ReviewButton.Name = "ReviewButton";
            this.ReviewButton.Size = new System.Drawing.Size(109, 32);
            this.ReviewButton.TabIndex = 15;
            this.ReviewButton.Text = "Review Input";
            this.ReviewButton.UseVisualStyleBackColor = true;
            this.ReviewButton.Click += new System.EventHandler(this.ReviewButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 275);
            this.Controls.Add(this.ReviewButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.InputFileCheck);
            this.Controls.Add(this.InputFileBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.BondCheck);
            this.Controls.Add(this.BondBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.OwnerCheck);
            this.Controls.Add(this.OwnerBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.GuarantorCheck);
            this.Controls.Add(this.GuarantorBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Copy Interest Rate LPDs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox GuarantorBox;
        private System.Windows.Forms.CheckBox GuarantorCheck;
        private System.Windows.Forms.CheckBox OwnerCheck;
        private System.Windows.Forms.TextBox OwnerBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox BondCheck;
        private System.Windows.Forms.TextBox BondBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.CheckBox InputFileCheck;
        private System.Windows.Forms.TextBox InputFileBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ReviewButton;
    }
}