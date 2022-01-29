namespace ENRLINFO
{
    partial class EnrollmentInformation
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
            this.accountIdentifierTextBox = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.accountIdentifierLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.schoolCodeLabel = new System.Windows.Forms.Label();
            this.schoolCodeTextBox = new System.Windows.Forms.TextBox();
            this.sourcePanel = new System.Windows.Forms.Panel();
            this.schoolRadioButton = new System.Windows.Forms.RadioButton();
            this.nchRadioButton = new System.Windows.Forms.RadioButton();
            this.nsldsRadioButton = new System.Windows.Forms.RadioButton();
            this.sourceLabel = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.yesButton = new System.Windows.Forms.Button();
            this.noButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.sourcePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // accountIdentifierTextBox
            // 
            this.accountIdentifierTextBox.AccountNumber = null;
            this.accountIdentifierTextBox.AllowedSpecialCharacters = "";
            this.accountIdentifierTextBox.Location = new System.Drawing.Point(81, 47);
            this.accountIdentifierTextBox.MaxLength = 10;
            this.accountIdentifierTextBox.Name = "accountIdentifierTextBox";
            this.accountIdentifierTextBox.Size = new System.Drawing.Size(180, 20);
            this.accountIdentifierTextBox.Ssn = null;
            this.accountIdentifierTextBox.TabIndex = 0;
            // 
            // accountIdentifierLabel
            // 
            this.accountIdentifierLabel.AutoSize = true;
            this.accountIdentifierLabel.Location = new System.Drawing.Point(12, 50);
            this.accountIdentifierLabel.Name = "accountIdentifierLabel";
            this.accountIdentifierLabel.Size = new System.Drawing.Size(63, 13);
            this.accountIdentifierLabel.TabIndex = 7;
            this.accountIdentifierLabel.Text = "SSN/Acct#";
            // 
            // titleLabel
            // 
            this.titleLabel.Location = new System.Drawing.Point(13, 13);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(248, 34);
            this.titleLabel.TabIndex = 6;
            this.titleLabel.Text = "Enter the SSN/Acct# and select the source of the enrollment information.";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // schoolCodeLabel
            // 
            this.schoolCodeLabel.AutoSize = true;
            this.schoolCodeLabel.Location = new System.Drawing.Point(15, 78);
            this.schoolCodeLabel.Name = "schoolCodeLabel";
            this.schoolCodeLabel.Size = new System.Drawing.Size(56, 13);
            this.schoolCodeLabel.TabIndex = 8;
            this.schoolCodeLabel.Text = "School Cd";
            // 
            // schoolCodeTextBox
            // 
            this.schoolCodeTextBox.Location = new System.Drawing.Point(81, 74);
            this.schoolCodeTextBox.MaxLength = 8;
            this.schoolCodeTextBox.Name = "schoolCodeTextBox";
            this.schoolCodeTextBox.Size = new System.Drawing.Size(180, 20);
            this.schoolCodeTextBox.TabIndex = 1;
            // 
            // sourcePanel
            // 
            this.sourcePanel.Controls.Add(this.schoolRadioButton);
            this.sourcePanel.Controls.Add(this.nchRadioButton);
            this.sourcePanel.Controls.Add(this.nsldsRadioButton);
            this.sourcePanel.Controls.Add(this.sourceLabel);
            this.sourcePanel.Location = new System.Drawing.Point(18, 100);
            this.sourcePanel.Name = "sourcePanel";
            this.sourcePanel.Size = new System.Drawing.Size(243, 100);
            this.sourcePanel.TabIndex = 2;
            // 
            // schoolRadioButton
            // 
            this.schoolRadioButton.AutoSize = true;
            this.schoolRadioButton.Location = new System.Drawing.Point(7, 69);
            this.schoolRadioButton.Name = "schoolRadioButton";
            this.schoolRadioButton.Size = new System.Drawing.Size(58, 17);
            this.schoolRadioButton.TabIndex = 3;
            this.schoolRadioButton.TabStop = true;
            this.schoolRadioButton.Text = "School";
            this.schoolRadioButton.UseVisualStyleBackColor = true;
            // 
            // nchRadioButton
            // 
            this.nchRadioButton.AutoSize = true;
            this.nchRadioButton.Location = new System.Drawing.Point(7, 45);
            this.nchRadioButton.Name = "nchRadioButton";
            this.nchRadioButton.Size = new System.Drawing.Size(48, 17);
            this.nchRadioButton.TabIndex = 2;
            this.nchRadioButton.TabStop = true;
            this.nchRadioButton.Text = "NCH";
            this.nchRadioButton.UseVisualStyleBackColor = true;
            // 
            // nsldsRadioButton
            // 
            this.nsldsRadioButton.AutoSize = true;
            this.nsldsRadioButton.Location = new System.Drawing.Point(7, 21);
            this.nsldsRadioButton.Name = "nsldsRadioButton";
            this.nsldsRadioButton.Size = new System.Drawing.Size(61, 17);
            this.nsldsRadioButton.TabIndex = 1;
            this.nsldsRadioButton.TabStop = true;
            this.nsldsRadioButton.Text = "NSLDS";
            this.nsldsRadioButton.UseVisualStyleBackColor = true;
            // 
            // sourceLabel
            // 
            this.sourceLabel.AutoSize = true;
            this.sourceLabel.Location = new System.Drawing.Point(4, 4);
            this.sourceLabel.Name = "sourceLabel";
            this.sourceLabel.Size = new System.Drawing.Size(41, 13);
            this.sourceLabel.TabIndex = 0;
            this.sourceLabel.Text = "Source";
            // 
            // infoLabel
            // 
            this.infoLabel.Location = new System.Drawing.Point(18, 207);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(243, 36);
            this.infoLabel.TabIndex = 9;
            this.infoLabel.Text = "Does the EVR contain a history of enrollment information?";
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(31, 239);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(100, 40);
            this.yesButton.TabIndex = 3;
            this.yesButton.Text = "YES";
            this.yesButton.UseVisualStyleBackColor = true;
            this.yesButton.Click += new System.EventHandler(this.yesButton_Click);
            // 
            // noButton
            // 
            this.noButton.Location = new System.Drawing.Point(139, 239);
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size(100, 40);
            this.noButton.TabIndex = 4;
            this.noButton.Text = "NO";
            this.noButton.UseVisualStyleBackColor = true;
            this.noButton.Click += new System.EventHandler(this.noButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(84, 286);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 35);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // EnrollmentInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 333);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.noButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.sourcePanel);
            this.Controls.Add(this.schoolCodeTextBox);
            this.Controls.Add(this.schoolCodeLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.accountIdentifierLabel);
            this.Controls.Add(this.accountIdentifierTextBox);
            this.MinimumSize = new System.Drawing.Size(289, 372);
            this.Name = "EnrollmentInformation";
            this.Text = "Enrollment Information";
            this.sourcePanel.ResumeLayout(false);
            this.sourcePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.AccountIdentifierTextBox accountIdentifierTextBox;
        private System.Windows.Forms.Label accountIdentifierLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label schoolCodeLabel;
        private System.Windows.Forms.TextBox schoolCodeTextBox;
        private System.Windows.Forms.Panel sourcePanel;
        private System.Windows.Forms.RadioButton schoolRadioButton;
        private System.Windows.Forms.RadioButton nchRadioButton;
        private System.Windows.Forms.RadioButton nsldsRadioButton;
        private System.Windows.Forms.Label sourceLabel;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button noButton;
        private System.Windows.Forms.Button cancelButton;
    }
}