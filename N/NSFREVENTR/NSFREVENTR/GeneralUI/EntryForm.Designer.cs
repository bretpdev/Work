namespace NSFREVENTR
{
    partial class EntryForm
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
            this.radOneLINK = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radCompass = new System.Windows.Forms.RadioButton();
            this.txtSSN = new System.Windows.Forms.TextBox();
            this.reversalEntryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPaymentAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbReason = new System.Windows.Forms.ComboBox();
            this.grpBatchType = new System.Windows.Forms.GroupBox();
            this.radWire = new System.Windows.Forms.RadioButton();
            this.radCash = new System.Windows.Forms.RadioButton();
            this.lblBatchType = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpDetail = new System.Windows.Forms.GroupBox();
            this.grpLoansToReverse = new System.Windows.Forms.GroupBox();
            this.radListed = new System.Windows.Forms.RadioButton();
            this.txtLoans = new System.Windows.Forms.TextBox();
            this.radALL = new System.Windows.Forms.RadioButton();
            this.lblLoansToReverse = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnDeconvertedLoans = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reversalEntryBindingSource)).BeginInit();
            this.grpBatchType.SuspendLayout();
            this.grpDetail.SuspendLayout();
            this.grpLoansToReverse.SuspendLayout();
            this.SuspendLayout();
            // 
            // radOneLINK
            // 
            this.radOneLINK.AutoSize = true;
            this.radOneLINK.Enabled = false;
            this.radOneLINK.Location = new System.Drawing.Point(91, 21);
            this.radOneLINK.Name = "radOneLINK";
            this.radOneLINK.Size = new System.Drawing.Size(69, 17);
            this.radOneLINK.TabIndex = 0;
            this.radOneLINK.Text = "OneLINK";
            this.radOneLINK.UseVisualStyleBackColor = true;
            this.radOneLINK.CheckedChanged += new System.EventHandler(this.radOneLINK_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radCompass);
            this.groupBox1.Controls.Add(this.radOneLINK);
            this.groupBox1.Location = new System.Drawing.Point(4, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 53);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "System";
            // 
            // radCompass
            // 
            this.radCompass.AutoSize = true;
            this.radCompass.Location = new System.Drawing.Point(190, 21);
            this.radCompass.Name = "radCompass";
            this.radCompass.Size = new System.Drawing.Size(68, 17);
            this.radCompass.TabIndex = 1;
            this.radCompass.Text = "Compass";
            this.radCompass.UseVisualStyleBackColor = true;
            this.radCompass.CheckedChanged += new System.EventHandler(this.radCompass_CheckedChanged);
            // 
            // txtSSN
            // 
            this.txtSSN.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.reversalEntryBindingSource, "SSN", true));
            this.txtSSN.Location = new System.Drawing.Point(106, 18);
            this.txtSSN.MaxLength = 10;
            this.txtSSN.Name = "txtSSN";
            this.txtSSN.Size = new System.Drawing.Size(100, 20);
            this.txtSSN.TabIndex = 2;
            // 
            // reversalEntryBindingSource
            // 
            this.reversalEntryBindingSource.DataSource = typeof(NSFREVENTR.ReversalEntry);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "SSN/Acct #";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Payment Amount";
            // 
            // txtPaymentAmount
            // 
            this.txtPaymentAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.reversalEntryBindingSource, "PaymentAmount", true));
            this.txtPaymentAmount.Location = new System.Drawing.Point(106, 39);
            this.txtPaymentAmount.Name = "txtPaymentAmount";
            this.txtPaymentAmount.Size = new System.Drawing.Size(100, 20);
            this.txtPaymentAmount.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Effective Date";
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.reversalEntryBindingSource, "EffectiveDate", true));
            this.maskedTextBox1.Location = new System.Drawing.Point(106, 60);
            this.maskedTextBox1.Mask = "00/00/0000";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(100, 20);
            this.maskedTextBox1.TabIndex = 10;
            this.maskedTextBox1.ValidatingType = typeof(System.DateTime);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "NSF Reason";
            // 
            // cmbReason
            // 
            this.cmbReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReason.FormattingEnabled = true;
            this.cmbReason.Location = new System.Drawing.Point(106, 79);
            this.cmbReason.Name = "cmbReason";
            this.cmbReason.Size = new System.Drawing.Size(224, 21);
            this.cmbReason.TabIndex = 12;
            // 
            // grpBatchType
            // 
            this.grpBatchType.Controls.Add(this.radWire);
            this.grpBatchType.Controls.Add(this.radCash);
            this.grpBatchType.Enabled = false;
            this.grpBatchType.Location = new System.Drawing.Point(106, 159);
            this.grpBatchType.Name = "grpBatchType";
            this.grpBatchType.Size = new System.Drawing.Size(224, 35);
            this.grpBatchType.TabIndex = 13;
            this.grpBatchType.TabStop = false;
            // 
            // radWire
            // 
            this.radWire.AutoSize = true;
            this.radWire.Location = new System.Drawing.Point(105, 12);
            this.radWire.Name = "radWire";
            this.radWire.Size = new System.Drawing.Size(47, 17);
            this.radWire.TabIndex = 1;
            this.radWire.Text = "Wire";
            this.radWire.UseVisualStyleBackColor = true;
            // 
            // radCash
            // 
            this.radCash.AutoSize = true;
            this.radCash.Location = new System.Drawing.Point(6, 12);
            this.radCash.Name = "radCash";
            this.radCash.Size = new System.Drawing.Size(49, 17);
            this.radCash.TabIndex = 0;
            this.radCash.Text = "Cash";
            this.radCash.UseVisualStyleBackColor = true;
            // 
            // lblBatchType
            // 
            this.lblBatchType.AutoSize = true;
            this.lblBatchType.Enabled = false;
            this.lblBatchType.Location = new System.Drawing.Point(6, 175);
            this.lblBatchType.Name = "lblBatchType";
            this.lblBatchType.Size = new System.Drawing.Size(62, 13);
            this.lblBatchType.TabIndex = 14;
            this.lblBatchType.Text = "Batch Type";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(11, 266);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 34);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(176, 266);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 34);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // grpDetail
            // 
            this.grpDetail.Controls.Add(this.grpLoansToReverse);
            this.grpDetail.Controls.Add(this.lblLoansToReverse);
            this.grpDetail.Controls.Add(this.label2);
            this.grpDetail.Controls.Add(this.txtSSN);
            this.grpDetail.Controls.Add(this.label1);
            this.grpDetail.Controls.Add(this.lblBatchType);
            this.grpDetail.Controls.Add(this.txtPaymentAmount);
            this.grpDetail.Controls.Add(this.grpBatchType);
            this.grpDetail.Controls.Add(this.label3);
            this.grpDetail.Controls.Add(this.cmbReason);
            this.grpDetail.Controls.Add(this.maskedTextBox1);
            this.grpDetail.Controls.Add(this.label5);
            this.grpDetail.Enabled = false;
            this.grpDetail.Location = new System.Drawing.Point(4, 54);
            this.grpDetail.Name = "grpDetail";
            this.grpDetail.Size = new System.Drawing.Size(339, 201);
            this.grpDetail.TabIndex = 17;
            this.grpDetail.TabStop = false;
            // 
            // grpLoansToReverse
            // 
            this.grpLoansToReverse.Controls.Add(this.radListed);
            this.grpLoansToReverse.Controls.Add(this.txtLoans);
            this.grpLoansToReverse.Controls.Add(this.radALL);
            this.grpLoansToReverse.Enabled = false;
            this.grpLoansToReverse.Location = new System.Drawing.Point(106, 99);
            this.grpLoansToReverse.Name = "grpLoansToReverse";
            this.grpLoansToReverse.Size = new System.Drawing.Size(224, 60);
            this.grpLoansToReverse.TabIndex = 17;
            this.grpLoansToReverse.TabStop = false;
            // 
            // radListed
            // 
            this.radListed.AutoSize = true;
            this.radListed.Location = new System.Drawing.Point(105, 12);
            this.radListed.Name = "radListed";
            this.radListed.Size = new System.Drawing.Size(85, 17);
            this.radListed.TabIndex = 1;
            this.radListed.Text = "Listed Below";
            this.radListed.UseVisualStyleBackColor = true;
            this.radListed.CheckedChanged += new System.EventHandler(this.radListed_CheckedChanged);
            // 
            // txtLoans
            // 
            this.txtLoans.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.reversalEntryBindingSource, "LoanCriteria", true));
            this.txtLoans.Enabled = false;
            this.txtLoans.Location = new System.Drawing.Point(5, 34);
            this.txtLoans.Name = "txtLoans";
            this.txtLoans.Size = new System.Drawing.Size(212, 20);
            this.txtLoans.TabIndex = 15;
            this.txtLoans.Click += new System.EventHandler(this.txtLoans_Click);
            this.txtLoans.Enter += new System.EventHandler(this.txtLoans_Enter);
            // 
            // radALL
            // 
            this.radALL.AutoSize = true;
            this.radALL.Checked = true;
            this.radALL.Location = new System.Drawing.Point(6, 12);
            this.radALL.Name = "radALL";
            this.radALL.Size = new System.Drawing.Size(36, 17);
            this.radALL.TabIndex = 0;
            this.radALL.TabStop = true;
            this.radALL.Text = "All";
            this.radALL.UseVisualStyleBackColor = true;
            this.radALL.CheckedChanged += new System.EventHandler(this.radALL_CheckedChanged);
            // 
            // lblLoansToReverse
            // 
            this.lblLoansToReverse.AutoSize = true;
            this.lblLoansToReverse.Enabled = false;
            this.lblLoansToReverse.Location = new System.Drawing.Point(6, 107);
            this.lblLoansToReverse.Name = "lblLoansToReverse";
            this.lblLoansToReverse.Size = new System.Drawing.Size(95, 13);
            this.lblLoansToReverse.TabIndex = 16;
            this.lblLoansToReverse.Text = "Loans To Reverse";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(259, 266);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 34);
            this.btnPrint.TabIndex = 18;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnDeconvertedLoans
            // 
            this.btnDeconvertedLoans.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDeconvertedLoans.Enabled = false;
            this.btnDeconvertedLoans.Location = new System.Drawing.Point(93, 266);
            this.btnDeconvertedLoans.Name = "btnDeconvertedLoans";
            this.btnDeconvertedLoans.Size = new System.Drawing.Size(77, 34);
            this.btnDeconvertedLoans.TabIndex = 19;
            this.btnDeconvertedLoans.Text = "Deconverted Loans";
            this.btnDeconvertedLoans.UseVisualStyleBackColor = true;
            this.btnDeconvertedLoans.Click += new System.EventHandler(this.btnDeconvertedLoans_Click);
            // 
            // EntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 308);
            this.Controls.Add(this.btnDeconvertedLoans);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.grpDetail);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Name = "EntryForm";
            this.Text = "NSF Entry";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reversalEntryBindingSource)).EndInit();
            this.grpBatchType.ResumeLayout(false);
            this.grpBatchType.PerformLayout();
            this.grpDetail.ResumeLayout(false);
            this.grpDetail.PerformLayout();
            this.grpLoansToReverse.ResumeLayout(false);
            this.grpLoansToReverse.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radOneLINK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radCompass;
        private System.Windows.Forms.TextBox txtSSN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPaymentAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbReason;
        private System.Windows.Forms.GroupBox grpBatchType;
        private System.Windows.Forms.RadioButton radWire;
        private System.Windows.Forms.RadioButton radCash;
        private System.Windows.Forms.Label lblBatchType;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.BindingSource reversalEntryBindingSource;
        private System.Windows.Forms.GroupBox grpDetail;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox txtLoans;
        private System.Windows.Forms.Label lblLoansToReverse;
        private System.Windows.Forms.GroupBox grpLoansToReverse;
        private System.Windows.Forms.RadioButton radListed;
        private System.Windows.Forms.RadioButton radALL;
        private System.Windows.Forms.Button btnDeconvertedLoans;
    }
}