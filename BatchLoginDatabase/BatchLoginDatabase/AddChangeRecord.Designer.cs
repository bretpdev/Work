namespace BatchLoginDatabase
{
    partial class AddChangeRecord
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
            this.LoginTypesCombo = new System.Windows.Forms.ComboBox();
            this.Delete = new System.Windows.Forms.Button();
            this.Save = new Uheaa.Common.WinForms.ValidationButton();
            this.Notes = new Uheaa.Common.WinForms.WatermarkRequiredTextBox();
            this.Password = new Uheaa.Common.WinForms.WatermarkRequiredTextBox();
            this.UserId = new Uheaa.Common.WinForms.WatermarkRequiredTextBox();
            this.ConfirmPassword = new Uheaa.Common.WinForms.WatermarkRequiredTextBox();
            this.ShowPasswords = new System.Windows.Forms.CheckBox();
            this.AllowedLogins = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.AllowedLogins)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(51, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Login Type:";
            // 
            // LoginTypesCombo
            // 
            this.LoginTypesCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoginTypesCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginTypesCombo.FormattingEnabled = true;
            this.LoginTypesCombo.Location = new System.Drawing.Point(147, 78);
            this.LoginTypesCombo.Name = "LoginTypesCombo";
            this.LoginTypesCombo.Size = new System.Drawing.Size(182, 26);
            this.LoginTypesCombo.TabIndex = 4;
            this.LoginTypesCombo.SelectionChangeCommitted += new System.EventHandler(this.LoginTypesCombo_SelectionChangeCommitted);
            // 
            // Delete
            // 
            this.Delete.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Delete.Enabled = false;
            this.Delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Delete.Location = new System.Drawing.Point(12, 294);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(108, 32);
            this.Delete.TabIndex = 8;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Save
            // 
            this.Save.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Save.Location = new System.Drawing.Point(221, 294);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(108, 32);
            this.Save.TabIndex = 0;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.OnValidate += new Uheaa.Common.WinForms.ValidationButton.ValidationHandler(this.Save_OnValidate);
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Notes
            // 
            this.Notes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Notes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.Notes.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Notes.Location = new System.Drawing.Point(12, 142);
            this.Notes.MaxLength = 100;
            this.Notes.Multiline = true;
            this.Notes.Name = "Notes";
            this.Notes.Size = new System.Drawing.Size(319, 146);
            this.Notes.TabIndex = 7;
            this.Notes.Text = "NOTES";
            this.Notes.Watermark = "NOTES";
            // 
            // Password
            // 
            this.Password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.Password.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Password.Location = new System.Drawing.Point(179, 12);
            this.Password.MaxLength = 128;
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(150, 26);
            this.Password.TabIndex = 2;
            this.Password.Text = "Password";
            this.Password.Watermark = "Password";
            this.Password.Enter += new System.EventHandler(this.Password_Enter);
            // 
            // UserId
            // 
            this.UserId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.UserId.ForeColor = System.Drawing.SystemColors.WindowText;
            this.UserId.Location = new System.Drawing.Point(19, 12);
            this.UserId.MaxLength = 128;
            this.UserId.Name = "UserId";
            this.UserId.Size = new System.Drawing.Size(150, 26);
            this.UserId.TabIndex = 1;
            this.UserId.Text = "User Id";
            this.UserId.Watermark = "User Id";
            // 
            // ConfirmPassword
            // 
            this.ConfirmPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConfirmPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.ConfirmPassword.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ConfirmPassword.Location = new System.Drawing.Point(179, 44);
            this.ConfirmPassword.Name = "ConfirmPassword";
            this.ConfirmPassword.Size = new System.Drawing.Size(150, 26);
            this.ConfirmPassword.TabIndex = 3;
            this.ConfirmPassword.Text = "Confirm Password";
            this.ConfirmPassword.Watermark = "Confirm Password";
            this.ConfirmPassword.Enter += new System.EventHandler(this.ConfirmPassword_Enter);
            // 
            // ShowPasswords
            // 
            this.ShowPasswords.Appearance = System.Windows.Forms.Appearance.Button;
            this.ShowPasswords.AutoSize = true;
            this.ShowPasswords.Location = new System.Drawing.Point(71, 44);
            this.ShowPasswords.Name = "ShowPasswords";
            this.ShowPasswords.Size = new System.Drawing.Size(98, 23);
            this.ShowPasswords.TabIndex = 9;
            this.ShowPasswords.Text = "Show Passwords";
            this.ShowPasswords.UseVisualStyleBackColor = true;
            this.ShowPasswords.CheckedChanged += new System.EventHandler(this.ShowPasswords_CheckedChanged);
            // 
            // AllowedLogins
            // 
            this.AllowedLogins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AllowedLogins.Enabled = false;
            this.AllowedLogins.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AllowedLogins.Location = new System.Drawing.Point(147, 110);
            this.AllowedLogins.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AllowedLogins.Name = "AllowedLogins";
            this.AllowedLogins.Size = new System.Drawing.Size(182, 26);
            this.AllowedLogins.TabIndex = 5;
            this.AllowedLogins.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Allowed Logins:";
            // 
            // AddChangeRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 338);
            this.Controls.Add(this.ShowPasswords);
            this.Controls.Add(this.ConfirmPassword);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.AllowedLogins);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.Notes);
            this.Controls.Add(this.LoginTypesCombo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.UserId);
            this.Name = "AddChangeRecord";
            this.ShowIcon = false;
            this.Text = "AddChangeRecord";
            ((System.ComponentModel.ISupportInitialize)(this.AllowedLogins)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.WatermarkRequiredTextBox UserId;
        private Uheaa.Common.WinForms.WatermarkRequiredTextBox Password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox LoginTypesCombo;
        private Uheaa.Common.WinForms.WatermarkRequiredTextBox Notes;
        private Uheaa.Common.WinForms.ValidationButton Save;
        private System.Windows.Forms.Button Delete;
        private Uheaa.Common.WinForms.WatermarkRequiredTextBox ConfirmPassword;
        private System.Windows.Forms.CheckBox ShowPasswords;
        private System.Windows.Forms.NumericUpDown AllowedLogins;
        private System.Windows.Forms.Label label3;

    }
}