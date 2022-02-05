namespace ADDMOREFED
{
    partial class PromptForAccountNumber
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
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnContinue = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.accountNumberTextBox = new Uheaa.Common.WinForms.NumericTextBox();
			this.referenceIdTextBox = new Uheaa.Common.WinForms.NumericTextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Location = new System.Drawing.Point(12, 18);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(5);
			this.label1.Size = new System.Drawing.Size(268, 54);
			this.label1.TabIndex = 1;
			this.label1.Text = "Please provide the borrower’s account # or provide the reference’s ID and click ‘" +
    "Continue’.  Click ‘Cancel’ to end the script.";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 86);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Borrower Acct #";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(71, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Reference ID";
			// 
			// btnContinue
			// 
			this.btnContinue.Location = new System.Drawing.Point(46, 158);
			this.btnContinue.Name = "btnContinue";
			this.btnContinue.Size = new System.Drawing.Size(75, 23);
			this.btnContinue.TabIndex = 2;
			this.btnContinue.Text = "Continue";
			this.btnContinue.UseVisualStyleBackColor = true;
			this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(168, 158);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(177, 115);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(14, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "P";
			// 
			// accountNumberTextBox
			// 
			this.accountNumberTextBox.AllowedSpecialCharacters = "";
			this.accountNumberTextBox.Location = new System.Drawing.Point(180, 83);
			this.accountNumberTextBox.Name = "accountNumberTextBox";
			this.accountNumberTextBox.Size = new System.Drawing.Size(100, 20);
			this.accountNumberTextBox.TabIndex = 8;
			// 
			// referenceIdTextBox
			// 
			this.referenceIdTextBox.AllowedSpecialCharacters = "";
			this.referenceIdTextBox.Location = new System.Drawing.Point(193, 112);
			this.referenceIdTextBox.Name = "referenceIdTextBox";
			this.referenceIdTextBox.Size = new System.Drawing.Size(87, 20);
			this.referenceIdTextBox.TabIndex = 9;
			// 
			// PromptForAccountNumber
			// 
			this.AcceptButton = this.btnContinue;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 193);
			this.Controls.Add(this.referenceIdTextBox);
			this.Controls.Add(this.accountNumberTextBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnContinue);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "PromptForAccountNumber";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Text = "Add or Modify Reference Script - FED";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
		private Uheaa.Common.WinForms.NumericTextBox accountNumberTextBox;
		private Uheaa.Common.WinForms.NumericTextBox referenceIdTextBox;
    }
}