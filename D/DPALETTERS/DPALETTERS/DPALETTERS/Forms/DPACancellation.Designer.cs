namespace DPALETTERS.Forms
{
    partial class DPACancellation
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
            this.label = new System.Windows.Forms.Label();
            this.accountIdentifierTextBoxBorrower = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.accountIdentifierTextBoxCosigner = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.radioButtonOtherReason = new System.Windows.Forms.RadioButton();
            this.radioButtonStoppedPayments = new System.Windows.Forms.RadioButton();
            this.radioButtonBankAccountClosed = new System.Windows.Forms.RadioButton();
            this.radioButtonNSF = new System.Windows.Forms.RadioButton();
            this.radioButtonBorrowersRequest = new System.Windows.Forms.RadioButton();
            this.radioButtonConsolidated = new System.Windows.Forms.RadioButton();
            this.radioButtonRehabilitated = new System.Windows.Forms.RadioButton();
            this.radioButtonPIF = new System.Windows.Forms.RadioButton();
            this.textBoxOtherReason = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.radioButtonDPACANP = new System.Windows.Forms.RadioButton();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point(13, 13);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(194, 60);
            this.label.TabIndex = 0;
            this.label.Text = "Enter the borrower\'s SSN or account number and the cosigner\'s SSN or account numb" +
    "er (if applicable) and select the cancellation reason.";
            // 
            // accountIdentifierTextBoxBorrower
            // 
            this.accountIdentifierTextBoxBorrower.AccountNumber = null;
            this.accountIdentifierTextBoxBorrower.AllowedSpecialCharacters = "";
            this.accountIdentifierTextBoxBorrower.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountIdentifierTextBoxBorrower.Location = new System.Drawing.Point(107, 76);
            this.accountIdentifierTextBoxBorrower.MaxLength = 10;
            this.accountIdentifierTextBoxBorrower.Name = "accountIdentifierTextBoxBorrower";
            this.accountIdentifierTextBoxBorrower.Size = new System.Drawing.Size(100, 20);
            this.accountIdentifierTextBoxBorrower.Ssn = null;
            this.accountIdentifierTextBoxBorrower.TabIndex = 1;
            // 
            // accountIdentifierTextBoxCosigner
            // 
            this.accountIdentifierTextBoxCosigner.AccountNumber = null;
            this.accountIdentifierTextBoxCosigner.AllowedSpecialCharacters = "";
            this.accountIdentifierTextBoxCosigner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountIdentifierTextBoxCosigner.Location = new System.Drawing.Point(107, 102);
            this.accountIdentifierTextBoxCosigner.MaxLength = 10;
            this.accountIdentifierTextBoxCosigner.Name = "accountIdentifierTextBoxCosigner";
            this.accountIdentifierTextBoxCosigner.Size = new System.Drawing.Size(100, 20);
            this.accountIdentifierTextBoxCosigner.Ssn = null;
            this.accountIdentifierTextBoxCosigner.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Borrower SSN/AN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Cosigner SSN/AN";
            // 
            // panel
            // 
            this.panel.Controls.Add(this.radioButtonDPACANP);
            this.panel.Controls.Add(this.radioButtonOtherReason);
            this.panel.Controls.Add(this.radioButtonStoppedPayments);
            this.panel.Controls.Add(this.radioButtonBankAccountClosed);
            this.panel.Controls.Add(this.radioButtonNSF);
            this.panel.Controls.Add(this.radioButtonBorrowersRequest);
            this.panel.Controls.Add(this.radioButtonConsolidated);
            this.panel.Controls.Add(this.radioButtonRehabilitated);
            this.panel.Controls.Add(this.radioButtonPIF);
            this.panel.Location = new System.Drawing.Point(12, 128);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(200, 189);
            this.panel.TabIndex = 5;
            // 
            // radioButtonOtherReason
            // 
            this.radioButtonOtherReason.AutoSize = true;
            this.radioButtonOtherReason.Location = new System.Drawing.Point(4, 146);
            this.radioButtonOtherReason.Name = "radioButtonOtherReason";
            this.radioButtonOtherReason.Size = new System.Drawing.Size(91, 17);
            this.radioButtonOtherReason.TabIndex = 7;
            this.radioButtonOtherReason.TabStop = true;
            this.radioButtonOtherReason.Text = "Other Reason";
            this.radioButtonOtherReason.UseVisualStyleBackColor = true;
            this.radioButtonOtherReason.CheckedChanged += new System.EventHandler(this.radioButtonOtherReason_CheckedChanged);
            // 
            // radioButtonStoppedPayments
            // 
            this.radioButtonStoppedPayments.AutoSize = true;
            this.radioButtonStoppedPayments.Location = new System.Drawing.Point(4, 126);
            this.radioButtonStoppedPayments.Name = "radioButtonStoppedPayments";
            this.radioButtonStoppedPayments.Size = new System.Drawing.Size(114, 17);
            this.radioButtonStoppedPayments.TabIndex = 6;
            this.radioButtonStoppedPayments.TabStop = true;
            this.radioButtonStoppedPayments.Text = "Stopped Payments";
            this.radioButtonStoppedPayments.UseVisualStyleBackColor = true;
            // 
            // radioButtonBankAccountClosed
            // 
            this.radioButtonBankAccountClosed.AutoSize = true;
            this.radioButtonBankAccountClosed.Location = new System.Drawing.Point(4, 107);
            this.radioButtonBankAccountClosed.Name = "radioButtonBankAccountClosed";
            this.radioButtonBankAccountClosed.Size = new System.Drawing.Size(128, 17);
            this.radioButtonBankAccountClosed.TabIndex = 5;
            this.radioButtonBankAccountClosed.TabStop = true;
            this.radioButtonBankAccountClosed.Text = "Bank Account Closed";
            this.radioButtonBankAccountClosed.UseVisualStyleBackColor = true;
            // 
            // radioButtonNSF
            // 
            this.radioButtonNSF.AutoSize = true;
            this.radioButtonNSF.Location = new System.Drawing.Point(4, 87);
            this.radioButtonNSF.Name = "radioButtonNSF";
            this.radioButtonNSF.Size = new System.Drawing.Size(46, 17);
            this.radioButtonNSF.TabIndex = 4;
            this.radioButtonNSF.TabStop = true;
            this.radioButtonNSF.Text = "NSF";
            this.radioButtonNSF.UseVisualStyleBackColor = true;
            // 
            // radioButtonBorrowersRequest
            // 
            this.radioButtonBorrowersRequest.AutoSize = true;
            this.radioButtonBorrowersRequest.Location = new System.Drawing.Point(4, 67);
            this.radioButtonBorrowersRequest.Name = "radioButtonBorrowersRequest";
            this.radioButtonBorrowersRequest.Size = new System.Drawing.Size(117, 17);
            this.radioButtonBorrowersRequest.TabIndex = 3;
            this.radioButtonBorrowersRequest.TabStop = true;
            this.radioButtonBorrowersRequest.Text = "Borrower\'s Request";
            this.radioButtonBorrowersRequest.UseVisualStyleBackColor = true;
            // 
            // radioButtonConsolidated
            // 
            this.radioButtonConsolidated.AutoSize = true;
            this.radioButtonConsolidated.Location = new System.Drawing.Point(4, 46);
            this.radioButtonConsolidated.Name = "radioButtonConsolidated";
            this.radioButtonConsolidated.Size = new System.Drawing.Size(86, 17);
            this.radioButtonConsolidated.TabIndex = 2;
            this.radioButtonConsolidated.TabStop = true;
            this.radioButtonConsolidated.Text = "Consolidated";
            this.radioButtonConsolidated.UseVisualStyleBackColor = true;
            // 
            // radioButtonRehabilitated
            // 
            this.radioButtonRehabilitated.AutoSize = true;
            this.radioButtonRehabilitated.Location = new System.Drawing.Point(4, 25);
            this.radioButtonRehabilitated.Name = "radioButtonRehabilitated";
            this.radioButtonRehabilitated.Size = new System.Drawing.Size(87, 17);
            this.radioButtonRehabilitated.TabIndex = 1;
            this.radioButtonRehabilitated.TabStop = true;
            this.radioButtonRehabilitated.Text = "Rehabilitated";
            this.radioButtonRehabilitated.UseVisualStyleBackColor = true;
            // 
            // radioButtonPIF
            // 
            this.radioButtonPIF.AutoSize = true;
            this.radioButtonPIF.Location = new System.Drawing.Point(4, 4);
            this.radioButtonPIF.Name = "radioButtonPIF";
            this.radioButtonPIF.Size = new System.Drawing.Size(41, 17);
            this.radioButtonPIF.TabIndex = 0;
            this.radioButtonPIF.TabStop = true;
            this.radioButtonPIF.Text = "PIF";
            this.radioButtonPIF.UseVisualStyleBackColor = true;
            // 
            // textBoxOtherReason
            // 
            this.textBoxOtherReason.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOtherReason.Enabled = false;
            this.textBoxOtherReason.Location = new System.Drawing.Point(12, 323);
            this.textBoxOtherReason.Multiline = true;
            this.textBoxOtherReason.Name = "textBoxOtherReason";
            this.textBoxOtherReason.Size = new System.Drawing.Size(200, 66);
            this.textBoxOtherReason.TabIndex = 6;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.Location = new System.Drawing.Point(12, 410);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(90, 30);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(122, 410);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(90, 30);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // radioButtonDPACANP
            // 
            this.radioButtonDPACANP.AutoSize = true;
            this.radioButtonDPACANP.Enabled = false;
            this.radioButtonDPACANP.Location = new System.Drawing.Point(4, 164);
            this.radioButtonDPACANP.Name = "radioButtonDPACANP";
            this.radioButtonDPACANP.Size = new System.Drawing.Size(114, 17);
            this.radioButtonDPACANP.TabIndex = 8;
            this.radioButtonDPACANP.TabStop = true;
            this.radioButtonDPACANP.Text = "Manual DPACANP";
            this.radioButtonDPACANP.UseVisualStyleBackColor = true;
            this.radioButtonDPACANP.Visible = false;
            // 
            // DPACancellation
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 449);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxOtherReason);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.accountIdentifierTextBoxCosigner);
            this.Controls.Add(this.accountIdentifierTextBoxBorrower);
            this.Controls.Add(this.label);
            this.MinimumSize = new System.Drawing.Size(236, 451);
            this.Name = "DPACancellation";
            this.Text = "DPA Cancellation";
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox accountIdentifierTextBoxBorrower;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox accountIdentifierTextBoxCosigner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.RadioButton radioButtonStoppedPayments;
        private System.Windows.Forms.RadioButton radioButtonBankAccountClosed;
        private System.Windows.Forms.RadioButton radioButtonNSF;
        private System.Windows.Forms.RadioButton radioButtonBorrowersRequest;
        private System.Windows.Forms.RadioButton radioButtonConsolidated;
        private System.Windows.Forms.RadioButton radioButtonRehabilitated;
        private System.Windows.Forms.RadioButton radioButtonPIF;
        private System.Windows.Forms.TextBox textBoxOtherReason;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.RadioButton radioButtonOtherReason;
        private System.Windows.Forms.RadioButton radioButtonDPACANP;
    }
}