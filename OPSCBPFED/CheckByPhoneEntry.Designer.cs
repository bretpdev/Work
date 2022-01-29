namespace OPSCBPFED
{
    partial class CheckByPhoneEntry
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
            this.components = new System.ComponentModel.Container();
            this.oPSEntryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.txtRoutingNumber = new Uheaa.Common.WinForms.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVerifyRoutingNumber = new Uheaa.Common.WinForms.NumericTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtVerifyBankAccountNumber = new Uheaa.Common.WinForms.NumericTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBankAccountNumber = new Uheaa.Common.WinForms.NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radOtherPaymentAmount = new System.Windows.Forms.RadioButton();
            this.radScheduleAmountDue = new System.Windows.Forms.RadioButton();
            this.radTotalAmountDue = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPaymentAmount = new Uheaa.Common.WinForms.NumericTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radSavings = new System.Windows.Forms.RadioButton();
            this.radChecking = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtAccountHolderName = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtEmailAddress = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.radNone = new System.Windows.Forms.RadioButton();
            this.radLetter = new System.Windows.Forms.RadioButton();
            this.radEmail = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dtpEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this.txtSSN = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.oPSEntryBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // oPSEntryBindingSource
            // 
            this.oPSEntryBindingSource.DataSource = typeof(OPSEntry);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Routing #:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRoutingNumber
            // 
            this.txtRoutingNumber.AllowedSpecialCharacters = "";
            this.txtRoutingNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "RoutingNumber", true));
            this.txtRoutingNumber.Location = new System.Drawing.Point(152, 40);
            this.txtRoutingNumber.MaxLength = 9;
            this.txtRoutingNumber.Name = "txtRoutingNumber";
            this.txtRoutingNumber.Size = new System.Drawing.Size(272, 20);
            this.txtRoutingNumber.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Verify Routing #:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtVerifyRoutingNumber
            // 
            this.txtVerifyRoutingNumber.AllowedSpecialCharacters = "";
            this.txtVerifyRoutingNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "VerifyRoutingNumber", true));
            this.txtVerifyRoutingNumber.Location = new System.Drawing.Point(152, 62);
            this.txtVerifyRoutingNumber.MaxLength = 9;
            this.txtVerifyRoutingNumber.Name = "txtVerifyRoutingNumber";
            this.txtVerifyRoutingNumber.Size = new System.Drawing.Size(272, 20);
            this.txtVerifyRoutingNumber.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 23);
            this.label4.TabIndex = 9;
            this.label4.Text = "Verify Banking Account #:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtVerifyBankAccountNumber
            // 
            this.txtVerifyBankAccountNumber.AllowedSpecialCharacters = "";
            this.txtVerifyBankAccountNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "VerifyBankAccountNumber", true));
            this.txtVerifyBankAccountNumber.Location = new System.Drawing.Point(152, 106);
            this.txtVerifyBankAccountNumber.MaxLength = 17;
            this.txtVerifyBankAccountNumber.Name = "txtVerifyBankAccountNumber";
            this.txtVerifyBankAccountNumber.Size = new System.Drawing.Size(272, 20);
            this.txtVerifyBankAccountNumber.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 23);
            this.label5.TabIndex = 7;
            this.label5.Text = "Banking Account #:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBankAccountNumber
            // 
            this.txtBankAccountNumber.AllowedSpecialCharacters = "";
            this.txtBankAccountNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "BankAccountNumber", true));
            this.txtBankAccountNumber.Location = new System.Drawing.Point(152, 84);
            this.txtBankAccountNumber.MaxLength = 17;
            this.txtBankAccountNumber.Name = "txtBankAccountNumber";
            this.txtBankAccountNumber.Size = new System.Drawing.Size(272, 20);
            this.txtBankAccountNumber.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 222);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 23);
            this.label6.TabIndex = 10;
            this.label6.Text = "Payment Options:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radOtherPaymentAmount);
            this.groupBox1.Controls.Add(this.radScheduleAmountDue);
            this.groupBox1.Controls.Add(this.radTotalAmountDue);
            this.groupBox1.Location = new System.Drawing.Point(152, 216);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 82);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // radOtherPaymentAmount
            // 
            this.radOtherPaymentAmount.AutoSize = true;
            this.radOtherPaymentAmount.Location = new System.Drawing.Point(6, 57);
            this.radOtherPaymentAmount.Name = "radOtherPaymentAmount";
            this.radOtherPaymentAmount.Size = new System.Drawing.Size(134, 17);
            this.radOtherPaymentAmount.TabIndex = 3;
            this.radOtherPaymentAmount.TabStop = true;
            this.radOtherPaymentAmount.Text = "Other Payment Amount";
            this.radOtherPaymentAmount.UseVisualStyleBackColor = true;
            this.radOtherPaymentAmount.CheckedChanged += new System.EventHandler(this.radOtherPaymentAmount_CheckedChanged);
            // 
            // radScheduleAmountDue
            // 
            this.radScheduleAmountDue.AutoSize = true;
            this.radScheduleAmountDue.Location = new System.Drawing.Point(6, 34);
            this.radScheduleAmountDue.Name = "radScheduleAmountDue";
            this.radScheduleAmountDue.Size = new System.Drawing.Size(189, 17);
            this.radScheduleAmountDue.TabIndex = 2;
            this.radScheduleAmountDue.TabStop = true;
            this.radScheduleAmountDue.Text = "Repayment Schedule Amount Due";
            this.radScheduleAmountDue.UseVisualStyleBackColor = true;
            this.radScheduleAmountDue.CheckedChanged += new System.EventHandler(this.radMonthlyAmountDue_CheckedChanged);
            // 
            // radTotalAmountDue
            // 
            this.radTotalAmountDue.AutoSize = true;
            this.radTotalAmountDue.Checked = true;
            this.radTotalAmountDue.Location = new System.Drawing.Point(6, 11);
            this.radTotalAmountDue.Name = "radTotalAmountDue";
            this.radTotalAmountDue.Size = new System.Drawing.Size(111, 17);
            this.radTotalAmountDue.TabIndex = 1;
            this.radTotalAmountDue.TabStop = true;
            this.radTotalAmountDue.Text = "Total Amount Due";
            this.radTotalAmountDue.UseVisualStyleBackColor = true;
            this.radTotalAmountDue.CheckedChanged += new System.EventHandler(this.radTotalAmountDue_CheckedChanged);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(13, 304);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(136, 23);
            this.label7.TabIndex = 13;
            this.label7.Text = "Payment Amount:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPaymentAmount
            // 
            this.txtPaymentAmount.AllowedSpecialCharacters = ".,";
            this.txtPaymentAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "PaymentAmount", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtPaymentAmount.Enabled = false;
            this.txtPaymentAmount.Location = new System.Drawing.Point(152, 304);
            this.txtPaymentAmount.Name = "txtPaymentAmount";
            this.txtPaymentAmount.Size = new System.Drawing.Size(272, 20);
            this.txtPaymentAmount.TabIndex = 8;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radSavings);
            this.groupBox2.Controls.Add(this.radChecking);
            this.groupBox2.Location = new System.Drawing.Point(152, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(272, 61);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // radSavings
            // 
            this.radSavings.AutoSize = true;
            this.radSavings.Location = new System.Drawing.Point(6, 34);
            this.radSavings.Name = "radSavings";
            this.radSavings.Size = new System.Drawing.Size(63, 17);
            this.radSavings.TabIndex = 2;
            this.radSavings.TabStop = true;
            this.radSavings.Text = "Savings";
            this.radSavings.UseVisualStyleBackColor = true;
            // 
            // radChecking
            // 
            this.radChecking.AutoSize = true;
            this.radChecking.Location = new System.Drawing.Point(6, 11);
            this.radChecking.Name = "radChecking";
            this.radChecking.Size = new System.Drawing.Size(70, 17);
            this.radChecking.TabIndex = 1;
            this.radChecking.TabStop = true;
            this.radChecking.Text = "Checking";
            this.radChecking.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(13, 134);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 23);
            this.label8.TabIndex = 14;
            this.label8.Text = "Account Type:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 195);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 23);
            this.label9.TabIndex = 17;
            this.label9.Text = "Account Holder Name:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAccountHolderName
            // 
            this.txtAccountHolderName.AllowedSpecialCharacters = "-\' ";
            this.txtAccountHolderName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "AccountHolderName", true));
            this.txtAccountHolderName.Location = new System.Drawing.Point(152, 195);
            this.txtAccountHolderName.Name = "txtAccountHolderName";
            this.txtAccountHolderName.Size = new System.Drawing.Size(272, 20);
            this.txtAccountHolderName.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(13, 325);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(136, 23);
            this.label10.TabIndex = 19;
            this.label10.Text = "Effective Date:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtEmailAddress);
            this.groupBox3.Controls.Add(this.radNone);
            this.groupBox3.Controls.Add(this.radLetter);
            this.groupBox3.Controls.Add(this.radEmail);
            this.groupBox3.Location = new System.Drawing.Point(152, 345);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(272, 101);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.AllowedSpecialCharacters = "@._";
            this.txtEmailAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "EmailAddress", true));
            this.txtEmailAddress.Enabled = false;
            this.txtEmailAddress.Location = new System.Drawing.Point(24, 30);
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(240, 20);
            this.txtEmailAddress.TabIndex = 2;
            // 
            // radNone
            // 
            this.radNone.AutoSize = true;
            this.radNone.Checked = true;
            this.radNone.Location = new System.Drawing.Point(6, 75);
            this.radNone.Name = "radNone";
            this.radNone.Size = new System.Drawing.Size(39, 17);
            this.radNone.TabIndex = 4;
            this.radNone.TabStop = true;
            this.radNone.Text = "No";
            this.radNone.UseVisualStyleBackColor = true;
            // 
            // radLetter
            // 
            this.radLetter.AutoSize = true;
            this.radLetter.Location = new System.Drawing.Point(6, 52);
            this.radLetter.Name = "radLetter";
            this.radLetter.Size = new System.Drawing.Size(52, 17);
            this.radLetter.TabIndex = 3;
            this.radLetter.TabStop = true;
            this.radLetter.Text = "Letter";
            this.radLetter.UseVisualStyleBackColor = true;
            // 
            // radEmail
            // 
            this.radEmail.AutoSize = true;
            this.radEmail.Location = new System.Drawing.Point(6, 11);
            this.radEmail.Name = "radEmail";
            this.radEmail.Size = new System.Drawing.Size(154, 17);
            this.radEmail.TabIndex = 1;
            this.radEmail.TabStop = true;
            this.radEmail.Text = "Email (enter address below)";
            this.radEmail.UseVisualStyleBackColor = true;
            this.radEmail.CheckedChanged += new System.EventHandler(this.radEmail_CheckedChanged);
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(0, 351);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(149, 23);
            this.label11.TabIndex = 20;
            this.label11.Text = "Confirmation Options:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSubmit
            // 
            this.btnSubmit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSubmit.Location = new System.Drawing.Point(112, 464);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 11;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(251, 464);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // dtpEffectiveDate
            // 
            this.dtpEffectiveDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.oPSEntryBindingSource, "EffectiveDate", true));
            this.dtpEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEffectiveDate.Location = new System.Drawing.Point(152, 328);
            this.dtpEffectiveDate.Name = "dtpEffectiveDate";
            this.dtpEffectiveDate.Size = new System.Drawing.Size(272, 20);
            this.dtpEffectiveDate.TabIndex = 9;
            // 
            // txtSSN
            // 
            this.txtSSN.AccountNumber = null;
            this.txtSSN.AllowedSpecialCharacters = "";
            this.txtSSN.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "AccountNumber", true));
            this.txtSSN.Location = new System.Drawing.Point(152, 17);
            this.txtSSN.MaxLength = 9;
            this.txtSSN.Name = "txtSSN";
            this.txtSSN.Size = new System.Drawing.Size(272, 20);
            this.txtSSN.Ssn = null;
            this.txtSSN.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 23);
            this.label1.TabIndex = 22;
            this.label1.Text = "Account Number:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CheckByPhoneEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 495);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSSN);
            this.Controls.Add(this.dtpEffectiveDate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtAccountHolderName);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtPaymentAmount);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtVerifyBankAccountNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBankAccountNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtVerifyRoutingNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRoutingNumber);
            this.MaximumSize = new System.Drawing.Size(456, 533);
            this.MinimumSize = new System.Drawing.Size(456, 533);
            this.Name = "CheckByPhoneEntry";
            this.Text = "Check By Phone - FED";
            ((System.ComponentModel.ISupportInitialize)(this.oPSEntryBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private Uheaa.Common.WinForms.NumericTextBox txtRoutingNumber;
        private System.Windows.Forms.Label label3;
        private Uheaa.Common.WinForms.NumericTextBox txtVerifyRoutingNumber;
        private System.Windows.Forms.Label label4;
        private Uheaa.Common.WinForms.NumericTextBox txtVerifyBankAccountNumber;
        private System.Windows.Forms.Label label5;
        private Uheaa.Common.WinForms.NumericTextBox txtBankAccountNumber;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radOtherPaymentAmount;
        private System.Windows.Forms.RadioButton radScheduleAmountDue;
        private System.Windows.Forms.RadioButton radTotalAmountDue;
        private System.Windows.Forms.Label label7;
        private Uheaa.Common.WinForms.NumericTextBox txtPaymentAmount;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radSavings;
        private System.Windows.Forms.RadioButton radChecking;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private Uheaa.Common.WinForms.AlphaNumericTextBox txtAccountHolderName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox3;
        private Uheaa.Common.WinForms.AlphaNumericTextBox txtEmailAddress;
        private System.Windows.Forms.RadioButton radNone;
        private System.Windows.Forms.RadioButton radLetter;
        private System.Windows.Forms.RadioButton radEmail;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.BindingSource oPSEntryBindingSource;
        private System.Windows.Forms.DateTimePicker dtpEffectiveDate;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox txtSSN;
        private System.Windows.Forms.Label label1;
    }
}