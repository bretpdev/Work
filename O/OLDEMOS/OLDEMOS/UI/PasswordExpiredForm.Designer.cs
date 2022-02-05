namespace OLDEMOS
{
    partial class PasswordExpiredForm
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
            this.PasswordText = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ShowPasswordCheck = new System.Windows.Forms.CheckBox();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.ConfirmLabel = new System.Windows.Forms.Label();
            this.ConfirmText = new System.Windows.Forms.TextBox();
            this.NewPasswordLabel = new System.Windows.Forms.Label();
            this.MessagePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.MessageOneLabel = new System.Windows.Forms.Label();
            this.MessageTwoLabel = new System.Windows.Forms.Label();
            this.LengthCheck = new ReadOnlyCheckbox();
            this.UppercaseCheck = new ReadOnlyCheckbox();
            this.LowerCaseCheck = new ReadOnlyCheckbox();
            this.NumberCheck = new ReadOnlyCheckbox();
            this.PreviousCheck = new ReadOnlyCheckbox();
            this.Line = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.MessagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PasswordText
            // 
            this.PasswordText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PasswordText.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordText.Location = new System.Drawing.Point(129, 13);
            this.PasswordText.Margin = new System.Windows.Forms.Padding(4);
            this.PasswordText.MaxLength = 8;
            this.PasswordText.Name = "PasswordText";
            this.PasswordText.PasswordChar = '*';
            this.PasswordText.Size = new System.Drawing.Size(247, 23);
            this.PasswordText.TabIndex = 1;
            this.PasswordText.TextChanged += new System.EventHandler(this.PasswordText_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ShowPasswordCheck);
            this.panel1.Controls.Add(this.CancelButton);
            this.panel1.Controls.Add(this.OkButton);
            this.panel1.Controls.Add(this.ConfirmLabel);
            this.panel1.Controls.Add(this.ConfirmText);
            this.panel1.Controls.Add(this.NewPasswordLabel);
            this.panel1.Controls.Add(this.PasswordText);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 179);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(388, 116);
            this.panel1.TabIndex = 2;
            // 
            // ShowPasswordCheck
            // 
            this.ShowPasswordCheck.AutoSize = true;
            this.ShowPasswordCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowPasswordCheck.Location = new System.Drawing.Point(27, 74);
            this.ShowPasswordCheck.Name = "ShowPasswordCheck";
            this.ShowPasswordCheck.Size = new System.Drawing.Size(102, 17);
            this.ShowPasswordCheck.TabIndex = 7;
            this.ShowPasswordCheck.Text = "Show Password";
            this.ShowPasswordCheck.UseVisualStyleBackColor = true;
            this.ShowPasswordCheck.CheckedChanged += new System.EventHandler(this.ShowPasswordCheck_CheckedChanged);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(241, 74);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(66, 32);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.Location = new System.Drawing.Point(312, 74);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(66, 32);
            this.OkButton.TabIndex = 5;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // ConfirmLabel
            // 
            this.ConfirmLabel.AutoSize = true;
            this.ConfirmLabel.Location = new System.Drawing.Point(1, 46);
            this.ConfirmLabel.Name = "ConfirmLabel";
            this.ConfirmLabel.Size = new System.Drawing.Size(122, 16);
            this.ConfirmLabel.TabIndex = 4;
            this.ConfirmLabel.Text = "Confirm Password";
            // 
            // ConfirmText
            // 
            this.ConfirmText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConfirmText.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfirmText.Location = new System.Drawing.Point(129, 44);
            this.ConfirmText.Margin = new System.Windows.Forms.Padding(4);
            this.ConfirmText.MaxLength = 8;
            this.ConfirmText.Name = "ConfirmText";
            this.ConfirmText.PasswordChar = '*';
            this.ConfirmText.Size = new System.Drawing.Size(247, 23);
            this.ConfirmText.TabIndex = 3;
            this.ConfirmText.TextChanged += new System.EventHandler(this.PasswordText_TextChanged);
            this.ConfirmText.Enter += new System.EventHandler(this.ConfirmText_Enter);
            this.ConfirmText.Leave += new System.EventHandler(this.ConfirmText_Leave);
            // 
            // NewPasswordLabel
            // 
            this.NewPasswordLabel.AutoSize = true;
            this.NewPasswordLabel.Location = new System.Drawing.Point(24, 15);
            this.NewPasswordLabel.Name = "NewPasswordLabel";
            this.NewPasswordLabel.Size = new System.Drawing.Size(99, 16);
            this.NewPasswordLabel.TabIndex = 2;
            this.NewPasswordLabel.Text = "New Password";
            // 
            // MessagePanel
            // 
            this.MessagePanel.Controls.Add(this.MessageOneLabel);
            this.MessagePanel.Controls.Add(this.MessageTwoLabel);
            this.MessagePanel.Controls.Add(this.LengthCheck);
            this.MessagePanel.Controls.Add(this.UppercaseCheck);
            this.MessagePanel.Controls.Add(this.LowerCaseCheck);
            this.MessagePanel.Controls.Add(this.NumberCheck);
            this.MessagePanel.Controls.Add(this.PreviousCheck);
            this.MessagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessagePanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.MessagePanel.Location = new System.Drawing.Point(0, 0);
            this.MessagePanel.Name = "MessagePanel";
            this.MessagePanel.Size = new System.Drawing.Size(388, 179);
            this.MessagePanel.TabIndex = 3;
            // 
            // MessageOneLabel
            // 
            this.MessageOneLabel.AutoSize = true;
            this.MessageOneLabel.Location = new System.Drawing.Point(3, 0);
            this.MessageOneLabel.Name = "MessageOneLabel";
            this.MessageOneLabel.Size = new System.Drawing.Size(374, 32);
            this.MessageOneLabel.TabIndex = 0;
            this.MessageOneLabel.Text = "Please provide a new password and then enter it again to confirm.";
            // 
            // MessageTwoLabel
            // 
            this.MessageTwoLabel.AutoSize = true;
            this.MessageTwoLabel.Location = new System.Drawing.Point(3, 32);
            this.MessageTwoLabel.Name = "MessageTwoLabel";
            this.MessageTwoLabel.Size = new System.Drawing.Size(312, 16);
            this.MessageTwoLabel.TabIndex = 1;
            this.MessageTwoLabel.Text = "The rules for your new password are as follows: ";
            // 
            // LengthCheck
            // 
            this.LengthCheck.AutoSize = true;
            this.LengthCheck.Checked = true;
            this.LengthCheck.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.LengthCheck.Location = new System.Drawing.Point(9, 51);
            this.LengthCheck.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.LengthCheck.Name = "LengthCheck";
            this.LengthCheck.Size = new System.Drawing.Size(245, 20);
            this.LengthCheck.TabIndex = 2;
            this.LengthCheck.Text = "Must be eight characters in length.";
            this.LengthCheck.ThreeState = true;
            this.LengthCheck.UseVisualStyleBackColor = true;
            // 
            // UppercaseCheck
            // 
            this.UppercaseCheck.AutoSize = true;
            this.UppercaseCheck.Checked = true;
            this.UppercaseCheck.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.UppercaseCheck.Location = new System.Drawing.Point(9, 77);
            this.UppercaseCheck.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.UppercaseCheck.Name = "UppercaseCheck";
            this.UppercaseCheck.Size = new System.Drawing.Size(363, 20);
            this.UppercaseCheck.TabIndex = 3;
            this.UppercaseCheck.Text = "Must contain one upper case letter (A-Z) or (@, #, $).";
            this.UppercaseCheck.ThreeState = true;
            this.UppercaseCheck.UseVisualStyleBackColor = true;
            // 
            // LowerCaseCheck
            // 
            this.LowerCaseCheck.AutoSize = true;
            this.LowerCaseCheck.Checked = true;
            this.LowerCaseCheck.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.LowerCaseCheck.Location = new System.Drawing.Point(9, 103);
            this.LowerCaseCheck.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.LowerCaseCheck.Name = "LowerCaseCheck";
            this.LowerCaseCheck.Size = new System.Drawing.Size(279, 20);
            this.LowerCaseCheck.TabIndex = 4;
            this.LowerCaseCheck.Text = "Must contain one lower case letter (a-z).";
            this.LowerCaseCheck.ThreeState = true;
            this.LowerCaseCheck.UseVisualStyleBackColor = true;
            // 
            // NumberCheck
            // 
            this.NumberCheck.AutoSize = true;
            this.NumberCheck.Checked = true;
            this.NumberCheck.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.NumberCheck.Location = new System.Drawing.Point(9, 129);
            this.NumberCheck.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.NumberCheck.Name = "NumberCheck";
            this.NumberCheck.Size = new System.Drawing.Size(323, 20);
            this.NumberCheck.TabIndex = 5;
            this.NumberCheck.Text = "The second character must be a number (0-9).";
            this.NumberCheck.ThreeState = true;
            this.NumberCheck.UseVisualStyleBackColor = true;
            // 
            // PreviousCheck
            // 
            this.PreviousCheck.AutoSize = true;
            this.PreviousCheck.Checked = true;
            this.PreviousCheck.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.PreviousCheck.Location = new System.Drawing.Point(9, 155);
            this.PreviousCheck.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.PreviousCheck.Name = "PreviousCheck";
            this.PreviousCheck.Size = new System.Drawing.Size(328, 20);
            this.PreviousCheck.TabIndex = 6;
            this.PreviousCheck.Text = "Must not use any of the previous 10 passwords.";
            this.PreviousCheck.ThreeState = true;
            this.PreviousCheck.UseVisualStyleBackColor = true;
            // 
            // Line
            // 
            this.Line.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Line.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Line.Location = new System.Drawing.Point(0, 177);
            this.Line.Name = "Line";
            this.Line.Size = new System.Drawing.Size(388, 2);
            this.Line.TabIndex = 9;
            // 
            // PasswordExpiredForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 295);
            this.Controls.Add(this.Line);
            this.Controls.Add(this.MessagePanel);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(404, 333);
            this.Name = "PasswordExpiredForm";
            this.Text = "Password Expired";
            this.Load += new System.EventHandler(this.PasswordExpiredForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.MessagePanel.ResumeLayout(false);
            this.MessagePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox PasswordText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label NewPasswordLabel;
        private System.Windows.Forms.TextBox ConfirmText;
        private System.Windows.Forms.CheckBox ShowPasswordCheck;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Label ConfirmLabel;
        private System.Windows.Forms.FlowLayoutPanel MessagePanel;
        private System.Windows.Forms.Label MessageOneLabel;
        private System.Windows.Forms.Label MessageTwoLabel;
        private ReadOnlyCheckbox LengthCheck;
        private ReadOnlyCheckbox UppercaseCheck;
        private ReadOnlyCheckbox LowerCaseCheck;
        private ReadOnlyCheckbox NumberCheck;
        private ReadOnlyCheckbox PreviousCheck;
        private System.Windows.Forms.Label Line;
        private System.Windows.Forms.Button CancelButton;
    }
}