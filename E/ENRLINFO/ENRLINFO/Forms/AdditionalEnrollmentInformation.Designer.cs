namespace ENRLINFO.Forms
{
    partial class AdditionalEnrollmentInformation
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
            this.enrollementStatusComboBox = new System.Windows.Forms.ComboBox();
            this.enrollmentStatusLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.statusEffectiveDateLabel = new System.Windows.Forms.Label();
            this.statusEffectiveMaskedDateTextBox = new Uheaa.Common.WinForms.MaskedDateTextBox();
            this.schoolCertMaskedDateTextBox = new Uheaa.Common.WinForms.MaskedDateTextBox();
            this.schoolCertDateLabel = new System.Windows.Forms.Label();
            this.agdLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.agdMaskedDateTextBox = new Uheaa.Common.WinForms.MaskedDateTextBox();
            this.SuspendLayout();
            // 
            // enrollementStatusComboBox
            // 
            this.enrollementStatusComboBox.FormattingEnabled = true;
            this.enrollementStatusComboBox.Location = new System.Drawing.Point(121, 25);
            this.enrollementStatusComboBox.Name = "enrollementStatusComboBox";
            this.enrollementStatusComboBox.Size = new System.Drawing.Size(87, 21);
            this.enrollementStatusComboBox.TabIndex = 0;
            // 
            // enrollmentStatusLabel
            // 
            this.enrollmentStatusLabel.AutoSize = true;
            this.enrollmentStatusLabel.Location = new System.Drawing.Point(4, 30);
            this.enrollmentStatusLabel.Name = "enrollmentStatusLabel";
            this.enrollmentStatusLabel.Size = new System.Drawing.Size(89, 13);
            this.enrollmentStatusLabel.TabIndex = 7;
            this.enrollmentStatusLabel.Text = "Enrollment Status";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(14, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(189, 13);
            this.titleLabel.TabIndex = 6;
            this.titleLabel.Text = "Enter the enrollment information below.";
            // 
            // statusEffectiveDateLabel
            // 
            this.statusEffectiveDateLabel.AutoSize = true;
            this.statusEffectiveDateLabel.Location = new System.Drawing.Point(4, 58);
            this.statusEffectiveDateLabel.Name = "statusEffectiveDateLabel";
            this.statusEffectiveDateLabel.Size = new System.Drawing.Size(108, 13);
            this.statusEffectiveDateLabel.TabIndex = 8;
            this.statusEffectiveDateLabel.Text = "Status Effective Date";
            // 
            // statusEffectiveMaskedDateTextBox
            // 
            this.statusEffectiveMaskedDateTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.statusEffectiveMaskedDateTextBox.Location = new System.Drawing.Point(121, 50);
            this.statusEffectiveMaskedDateTextBox.Mask = "00/00/0000";
            this.statusEffectiveMaskedDateTextBox.Name = "statusEffectiveMaskedDateTextBox";
            this.statusEffectiveMaskedDateTextBox.Size = new System.Drawing.Size(87, 26);
            this.statusEffectiveMaskedDateTextBox.TabIndex = 1;
            // 
            // schoolCertMaskedDateTextBox
            // 
            this.schoolCertMaskedDateTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.schoolCertMaskedDateTextBox.Location = new System.Drawing.Point(121, 81);
            this.schoolCertMaskedDateTextBox.Mask = "00/00/0000";
            this.schoolCertMaskedDateTextBox.Name = "schoolCertMaskedDateTextBox";
            this.schoolCertMaskedDateTextBox.Size = new System.Drawing.Size(87, 26);
            this.schoolCertMaskedDateTextBox.TabIndex = 2;
            // 
            // schoolCertDateLabel
            // 
            this.schoolCertDateLabel.AutoSize = true;
            this.schoolCertDateLabel.Location = new System.Drawing.Point(6, 89);
            this.schoolCertDateLabel.Name = "schoolCertDateLabel";
            this.schoolCertDateLabel.Size = new System.Drawing.Size(88, 13);
            this.schoolCertDateLabel.TabIndex = 9;
            this.schoolCertDateLabel.Text = "School Cert Date";
            // 
            // agdLabel
            // 
            this.agdLabel.AutoSize = true;
            this.agdLabel.Location = new System.Drawing.Point(7, 118);
            this.agdLabel.Name = "agdLabel";
            this.agdLabel.Size = new System.Drawing.Size(30, 13);
            this.agdLabel.TabIndex = 10;
            this.agdLabel.Text = "AGD";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(17, 145);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(85, 35);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(110, 145);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(85, 35);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // agdMaskedDateTextBox
            // 
            this.agdMaskedDateTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.agdMaskedDateTextBox.Location = new System.Drawing.Point(121, 114);
            this.agdMaskedDateTextBox.Mask = "00/00/0000";
            this.agdMaskedDateTextBox.Name = "agdMaskedDateTextBox";
            this.agdMaskedDateTextBox.Size = new System.Drawing.Size(87, 26);
            this.agdMaskedDateTextBox.TabIndex = 3;
            // 
            // AdditionalEnrollmentInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 192);
            this.Controls.Add(this.agdMaskedDateTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.agdLabel);
            this.Controls.Add(this.schoolCertDateLabel);
            this.Controls.Add(this.schoolCertMaskedDateTextBox);
            this.Controls.Add(this.statusEffectiveMaskedDateTextBox);
            this.Controls.Add(this.statusEffectiveDateLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.enrollmentStatusLabel);
            this.Controls.Add(this.enrollementStatusComboBox);
            this.MinimumSize = new System.Drawing.Size(231, 231);
            this.Name = "AdditionalEnrollmentInformation";
            this.Text = "Add Enrollment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox enrollementStatusComboBox;
        private System.Windows.Forms.Label enrollmentStatusLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label statusEffectiveDateLabel;
        private Uheaa.Common.WinForms.MaskedDateTextBox statusEffectiveMaskedDateTextBox;
        private Uheaa.Common.WinForms.MaskedDateTextBox schoolCertMaskedDateTextBox;
        private System.Windows.Forms.Label schoolCertDateLabel;
        private System.Windows.Forms.Label agdLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private Uheaa.Common.WinForms.MaskedDateTextBox agdMaskedDateTextBox;
    }
}