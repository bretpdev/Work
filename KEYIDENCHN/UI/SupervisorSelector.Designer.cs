namespace KEYIDENCHN
{
    partial class SupervisorSelector
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
            this.SupervisorsList = new System.Windows.Forms.ListBox();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.RememberCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // SupervisorsList
            // 
            this.SupervisorsList.FormattingEnabled = true;
            this.SupervisorsList.ItemHeight = 18;
            this.SupervisorsList.Location = new System.Drawing.Point(12, 35);
            this.SupervisorsList.Name = "SupervisorsList";
            this.SupervisorsList.Size = new System.Drawing.Size(288, 202);
            this.SupervisorsList.TabIndex = 0;
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(12, 243);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(97, 47);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(115, 243);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(185, 47);
            this.SubmitButton.TabIndex = 2;
            this.SubmitButton.Text = "Submit for Review";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // RememberCheck
            // 
            this.RememberCheck.AutoSize = true;
            this.RememberCheck.Location = new System.Drawing.Point(13, 7);
            this.RememberCheck.Name = "RememberCheck";
            this.RememberCheck.Size = new System.Drawing.Size(268, 22);
            this.RememberCheck.TabIndex = 3;
            this.RememberCheck.Text = "Remember my selected supervisor";
            this.RememberCheck.UseVisualStyleBackColor = true;
            // 
            // SupervisorSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 301);
            this.Controls.Add(this.RememberCheck);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.SupervisorsList);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SupervisorSelector";
            this.Text = "Select a Supervisor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox SupervisorsList;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.CheckBox RememberCheck;
    }
}