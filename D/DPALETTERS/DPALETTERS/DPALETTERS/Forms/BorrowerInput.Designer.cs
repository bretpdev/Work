namespace DPALETTERS
{
    partial class BorrowerInput
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label = new System.Windows.Forms.Label();
            this.accountIdentifierTextBox = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.Location = new System.Drawing.Point(12, 89);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(264, 89);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point(13, 2);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(190, 38);
            this.label.TabIndex = 0;
            this.label.Text = "Enter an SSN or account number, or click Cancel to quit.";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // accountIdentifierTextBox
            // 
            this.accountIdentifierTextBox.AccountNumber = null;
            this.accountIdentifierTextBox.AllowedSpecialCharacters = "";
            this.accountIdentifierTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountIdentifierTextBox.Location = new System.Drawing.Point(12, 44);
            this.accountIdentifierTextBox.MaxLength = 10;
            this.accountIdentifierTextBox.Name = "accountIdentifierTextBox";
            this.accountIdentifierTextBox.Size = new System.Drawing.Size(327, 20);
            this.accountIdentifierTextBox.Ssn = null;
            this.accountIdentifierTextBox.TabIndex = 1;
            // 
            // BorrowerInput
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 116);
            this.Controls.Add(this.accountIdentifierTextBox);
            this.Controls.Add(this.label);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.MinimumSize = new System.Drawing.Size(231, 137);
            this.Name = "BorrowerInput";
            this.Text = "Enter SSN or AN";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label;
        public Uheaa.Common.WinForms.AccountIdentifierTextBox accountIdentifierTextBox;
    }
}