namespace CommonTesting
{
    partial class ValidationTest
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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.alphaTextBox1 = new Uheaa.Common.WinForms.AlphaTextBox();
            this.stateSelector2 = new Uheaa.Common.WinForms.StateSelector();
            this.stateSelector1 = new Uheaa.Common.WinForms.StateSelector();
            this.accountIdentifierTextBox1 = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.accountNumberTextBox1 = new Uheaa.Common.WinForms.AccountNumberTextBox();
            this.ValidationButton = new Uheaa.Common.WinForms.ValidationButton();
            this.ssnTextBox1 = new Uheaa.Common.WinForms.SsnTextBox();
            this.numericTextBox1 = new Uheaa.Common.WinForms.NumericTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.alphaNumericTextBox1 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Numeric Textbox";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SSN Textbox";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "AccountNumber Textbox";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 240);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(236, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "AccountIdentifier Textbox (SSN or AccountNum)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 290);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "State Selector";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 342);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(146, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "State Selector (allows blanks)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Alpha TextBox";
            // 
            // alphaTextBox1
            // 
            this.alphaTextBox1.Location = new System.Drawing.Point(12, 119);
            this.alphaTextBox1.Name = "alphaTextBox1";
            this.alphaTextBox1.Size = new System.Drawing.Size(100, 20);
            this.alphaTextBox1.TabIndex = 13;
            // 
            // stateSelector2
            // 
            this.stateSelector2.FormattingEnabled = true;
            this.stateSelector2.Location = new System.Drawing.Point(15, 358);
            this.stateSelector2.Name = "stateSelector2";
            this.stateSelector2.Size = new System.Drawing.Size(43, 21);
            this.stateSelector2.TabIndex = 12;
            // 
            // stateSelector1
            // 
            this.stateSelector1.FormattingEnabled = true;
            this.stateSelector1.Location = new System.Drawing.Point(15, 306);
            this.stateSelector1.Name = "stateSelector1";
            this.stateSelector1.Size = new System.Drawing.Size(43, 21);
            this.stateSelector1.TabIndex = 9;
            // 
            // accountIdentifierTextBox1
            // 
            this.accountIdentifierTextBox1.AccountNumber = null;
            this.accountIdentifierTextBox1.Location = new System.Drawing.Point(12, 256);
            this.accountIdentifierTextBox1.MaxLength = 10;
            this.accountIdentifierTextBox1.Name = "accountIdentifierTextBox1";
            this.accountIdentifierTextBox1.Size = new System.Drawing.Size(100, 20);
            this.accountIdentifierTextBox1.Ssn = null;
            this.accountIdentifierTextBox1.TabIndex = 8;
            // 
            // accountNumberTextBox1
            // 
            this.accountNumberTextBox1.Location = new System.Drawing.Point(12, 206);
            this.accountNumberTextBox1.MaxLength = 10;
            this.accountNumberTextBox1.Name = "accountNumberTextBox1";
            this.accountNumberTextBox1.Size = new System.Drawing.Size(100, 20);
            this.accountNumberTextBox1.Ssn = null;
            this.accountNumberTextBox1.TabIndex = 5;
            // 
            // ValidationButton
            // 
            this.ValidationButton.Location = new System.Drawing.Point(456, 469);
            this.ValidationButton.Name = "ValidationButton";
            this.ValidationButton.Size = new System.Drawing.Size(99, 35);
            this.ValidationButton.TabIndex = 4;
            this.ValidationButton.Text = "Validate";
            this.ValidationButton.UseVisualStyleBackColor = true;
            // 
            // ssnTextBox1
            // 
            this.ssnTextBox1.Location = new System.Drawing.Point(12, 158);
            this.ssnTextBox1.MaxLength = 9;
            this.ssnTextBox1.Name = "ssnTextBox1";
            this.ssnTextBox1.Size = new System.Drawing.Size(100, 20);
            this.ssnTextBox1.Ssn = null;
            this.ssnTextBox1.TabIndex = 2;
            // 
            // numericTextBox1
            // 
            this.numericTextBox1.Location = new System.Drawing.Point(12, 75);
            this.numericTextBox1.Name = "numericTextBox1";
            this.numericTextBox1.Size = new System.Drawing.Size(100, 20);
            this.numericTextBox1.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "AlphaNumeric Textbox";
            // 
            // alphaNumericTextBox1
            // 
            this.alphaNumericTextBox1.Location = new System.Drawing.Point(12, 25);
            this.alphaNumericTextBox1.Name = "alphaNumericTextBox1";
            this.alphaNumericTextBox1.Size = new System.Drawing.Size(100, 20);
            this.alphaNumericTextBox1.TabIndex = 16;
            // 
            // ValidationTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 516);
            this.Controls.Add(this.alphaNumericTextBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.alphaTextBox1);
            this.Controls.Add(this.stateSelector2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.stateSelector1);
            this.Controls.Add(this.accountIdentifierTextBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.accountNumberTextBox1);
            this.Controls.Add(this.ValidationButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ssnTextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericTextBox1);
            this.Name = "ValidationTest";
            this.Text = "Validation Tests";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.NumericTextBox numericTextBox1;
        private System.Windows.Forms.Label label1;
        private Uheaa.Common.WinForms.SsnTextBox ssnTextBox1;
        private System.Windows.Forms.Label label2;
        private Uheaa.Common.WinForms.ValidationButton ValidationButton;
        private Uheaa.Common.WinForms.AccountNumberTextBox accountNumberTextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox accountIdentifierTextBox1;
        private Uheaa.Common.WinForms.StateSelector stateSelector1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Uheaa.Common.WinForms.StateSelector stateSelector2;
        private Uheaa.Common.WinForms.AlphaTextBox alphaTextBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private Uheaa.Common.WinForms.AlphaNumericTextBox alphaNumericTextBox1;
    }
}

