namespace I1I2CHREV
{
    partial class ManualForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SchoolFoundButton = new System.Windows.Forms.CheckBox();
            this.SchoolNotFoundButton = new System.Windows.Forms.CheckBox();
            this.Step2Box = new System.Windows.Forms.GroupBox();
            this.LetterNotSentButton = new System.Windows.Forms.CheckBox();
            this.LetterSentButton = new System.Windows.Forms.CheckBox();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.Step2Box.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "No schools were found for this borrower.";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.SchoolFoundButton);
            this.groupBox1.Controls.Add(this.SchoolNotFoundButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 87);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Review NSLDS for the last school attended.";
            // 
            // SchoolFoundButton
            // 
            this.SchoolFoundButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SchoolFoundButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.SchoolFoundButton.Location = new System.Drawing.Point(207, 25);
            this.SchoolFoundButton.Name = "SchoolFoundButton";
            this.SchoolFoundButton.Size = new System.Drawing.Size(182, 52);
            this.SchoolFoundButton.TabIndex = 7;
            this.SchoolFoundButton.Text = "I found the last school attended.";
            this.SchoolFoundButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SchoolFoundButton.UseVisualStyleBackColor = true;
            this.SchoolFoundButton.CheckedChanged += new System.EventHandler(this.SchoolFoundButton_CheckedChanged);
            // 
            // SchoolNotFoundButton
            // 
            this.SchoolNotFoundButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.SchoolNotFoundButton.Location = new System.Drawing.Point(6, 25);
            this.SchoolNotFoundButton.Name = "SchoolNotFoundButton";
            this.SchoolNotFoundButton.Size = new System.Drawing.Size(195, 52);
            this.SchoolNotFoundButton.TabIndex = 6;
            this.SchoolNotFoundButton.Text = "I did not find the last school attended.";
            this.SchoolNotFoundButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SchoolNotFoundButton.UseVisualStyleBackColor = true;
            this.SchoolNotFoundButton.CheckedChanged += new System.EventHandler(this.SchoolNotFoundButton_CheckedChanged);
            // 
            // Step2Box
            // 
            this.Step2Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Step2Box.Controls.Add(this.LetterNotSentButton);
            this.Step2Box.Controls.Add(this.LetterSentButton);
            this.Step2Box.Enabled = false;
            this.Step2Box.Location = new System.Drawing.Point(12, 142);
            this.Step2Box.Name = "Step2Box";
            this.Step2Box.Size = new System.Drawing.Size(395, 88);
            this.Step2Box.TabIndex = 4;
            this.Step2Box.TabStop = false;
            this.Step2Box.Text = "2. Send a manual letter to the last school attended.";
            // 
            // LetterNotSentButton
            // 
            this.LetterNotSentButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.LetterNotSentButton.Location = new System.Drawing.Point(6, 26);
            this.LetterNotSentButton.Name = "LetterNotSentButton";
            this.LetterNotSentButton.Size = new System.Drawing.Size(195, 52);
            this.LetterNotSentButton.TabIndex = 9;
            this.LetterNotSentButton.Text = "I won\'t send a letter to the last school attended.";
            this.LetterNotSentButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LetterNotSentButton.UseVisualStyleBackColor = true;
            this.LetterNotSentButton.CheckedChanged += new System.EventHandler(this.LetterNotSentButton_CheckedChanged);
            // 
            // LetterSentButton
            // 
            this.LetterSentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LetterSentButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.LetterSentButton.Location = new System.Drawing.Point(207, 26);
            this.LetterSentButton.Name = "LetterSentButton";
            this.LetterSentButton.Size = new System.Drawing.Size(182, 52);
            this.LetterSentButton.TabIndex = 8;
            this.LetterSentButton.Text = "I sent a letter to the last school attended.";
            this.LetterSentButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LetterSentButton.UseVisualStyleBackColor = true;
            this.LetterSentButton.CheckedChanged += new System.EventHandler(this.LetterSentButton_CheckedChanged);
            // 
            // ContinueButton
            // 
            this.ContinueButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ContinueButton.Enabled = false;
            this.ContinueButton.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContinueButton.Location = new System.Drawing.Point(12, 236);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(395, 53);
            this.ContinueButton.TabIndex = 5;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = true;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // ManualForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 297);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.Step2Box);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(435, 335);
            this.Name = "ManualForm";
            this.Text = "No Schools Found";
            this.groupBox1.ResumeLayout(false);
            this.Step2Box.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox Step2Box;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.CheckBox SchoolNotFoundButton;
        private System.Windows.Forms.CheckBox SchoolFoundButton;
        private System.Windows.Forms.CheckBox LetterNotSentButton;
        private System.Windows.Forms.CheckBox LetterSentButton;
    }
}