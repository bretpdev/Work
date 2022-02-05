namespace MauiDUDE
{
    partial class BankruptcyACPQuestionsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxEndorser = new System.Windows.Forms.TextBox();
            this.checkBoxEndorser = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBoxExtendedInfo = new System.Windows.Forms.GroupBox();
            this.textBoxDocketNumber = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.maskedTextBoxFilingDate = new System.Windows.Forms.MaskedTextBox();
            this.textBoxChapter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxCourtInformation = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxAttorneyInformation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxBankruptcyOffiallyFiled = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBoxExtendedInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxEndorser);
            this.groupBox1.Controls.Add(this.checkBoxEndorser);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.groupBoxExtendedInfo);
            this.groupBox1.Controls.Add(this.textBoxAttorneyInformation);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxBankruptcyOffiallyFiled);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(7, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(594, 428);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bankruptcy Information";
            // 
            // textBoxEndorser
            // 
            this.textBoxEndorser.Enabled = false;
            this.textBoxEndorser.Location = new System.Drawing.Point(47, 394);
            this.textBoxEndorser.MaxLength = 9;
            this.textBoxEndorser.Name = "textBoxEndorser";
            this.textBoxEndorser.Size = new System.Drawing.Size(161, 20);
            this.textBoxEndorser.TabIndex = 8;
            // 
            // checkBoxEndorser
            // 
            this.checkBoxEndorser.AutoSize = true;
            this.checkBoxEndorser.Location = new System.Drawing.Point(28, 398);
            this.checkBoxEndorser.Name = "checkBoxEndorser";
            this.checkBoxEndorser.Size = new System.Drawing.Size(15, 14);
            this.checkBoxEndorser.TabIndex = 7;
            this.checkBoxEndorser.UseVisualStyleBackColor = true;
            this.checkBoxEndorser.CheckedChanged += new System.EventHandler(this.checkBoxEndorser_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 381);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(222, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "6. ENDORSER IS FILING(ENDORSER SSN)";
            // 
            // groupBoxExtendedInfo
            // 
            this.groupBoxExtendedInfo.Controls.Add(this.textBoxDocketNumber);
            this.groupBoxExtendedInfo.Controls.Add(this.label6);
            this.groupBoxExtendedInfo.Controls.Add(this.maskedTextBoxFilingDate);
            this.groupBoxExtendedInfo.Controls.Add(this.textBoxChapter);
            this.groupBoxExtendedInfo.Controls.Add(this.label5);
            this.groupBoxExtendedInfo.Controls.Add(this.textBoxCourtInformation);
            this.groupBoxExtendedInfo.Controls.Add(this.label4);
            this.groupBoxExtendedInfo.Location = new System.Drawing.Point(6, 199);
            this.groupBoxExtendedInfo.Name = "groupBoxExtendedInfo";
            this.groupBoxExtendedInfo.Size = new System.Drawing.Size(583, 175);
            this.groupBoxExtendedInfo.TabIndex = 5;
            this.groupBoxExtendedInfo.TabStop = false;
            this.groupBoxExtendedInfo.Text = "(THE REMAINING WILL BE ASKED ONLY IF THE BORROWER HAS ALREADY FILED.) ";
            // 
            // textBoxDocketNumber
            // 
            this.textBoxDocketNumber.Location = new System.Drawing.Point(22, 148);
            this.textBoxDocketNumber.MaxLength = 12;
            this.textBoxDocketNumber.Name = "textBoxDocketNumber";
            this.textBoxDocketNumber.Size = new System.Drawing.Size(180, 20);
            this.textBoxDocketNumber.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(193, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "5. WHAT IS THE DOCKET NUMBER?";
            // 
            // maskedTextBoxFilingDate
            // 
            this.maskedTextBoxFilingDate.Location = new System.Drawing.Point(66, 107);
            this.maskedTextBoxFilingDate.Mask = "00/00/0000";
            this.maskedTextBoxFilingDate.Name = "maskedTextBoxFilingDate";
            this.maskedTextBoxFilingDate.Size = new System.Drawing.Size(86, 20);
            this.maskedTextBoxFilingDate.TabIndex = 4;
            // 
            // textBoxChapter
            // 
            this.textBoxChapter.Location = new System.Drawing.Point(22, 107);
            this.textBoxChapter.MaxLength = 2;
            this.textBoxChapter.Name = "textBoxChapter";
            this.textBoxChapter.Size = new System.Drawing.Size(37, 20);
            this.textBoxChapter.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(363, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "4. WHAT IS THE BANKRUPTCY CHAPTER AND THE DATE OF FILING?";
            // 
            // textBoxCourtInformation
            // 
            this.textBoxCourtInformation.Location = new System.Drawing.Point(22, 40);
            this.textBoxCourtInformation.MaxLength = 80;
            this.textBoxCourtInformation.Multiline = true;
            this.textBoxCourtInformation.Name = "textBoxCourtInformation";
            this.textBoxCourtInformation.Size = new System.Drawing.Size(551, 47);
            this.textBoxCourtInformation.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(387, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "3. WHAT IS THE NAME, CITY, AND STATE OF THE COURT WHERE FILED?";
            // 
            // textBoxAttorneyInformation
            // 
            this.textBoxAttorneyInformation.Location = new System.Drawing.Point(28, 145);
            this.textBoxAttorneyInformation.MaxLength = 80;
            this.textBoxAttorneyInformation.Multiline = true;
            this.textBoxAttorneyInformation.Name = "textBoxAttorneyInformation";
            this.textBoxAttorneyInformation.Size = new System.Drawing.Size(554, 47);
            this.textBoxAttorneyInformation.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(570, 28);
            this.label3.TabIndex = 3;
            this.label3.Text = "2. WE MUST BE NOTIFIED IN WRITING THAT THE BORROWER HAS FILED.  WHAT IS THE NAME," +
    " ADDRESS, AND PHONE NUMBER OF YOUR ATTORNEY? ";
            // 
            // comboBoxBankruptcyOffiallyFiled
            // 
            this.comboBoxBankruptcyOffiallyFiled.FormattingEnabled = true;
            this.comboBoxBankruptcyOffiallyFiled.Items.AddRange(new object[] {
            "",
            "Y",
            "N"});
            this.comboBoxBankruptcyOffiallyFiled.Location = new System.Drawing.Point(28, 92);
            this.comboBoxBankruptcyOffiallyFiled.Name = "comboBoxBankruptcyOffiallyFiled";
            this.comboBoxBankruptcyOffiallyFiled.Size = new System.Drawing.Size(49, 21);
            this.comboBoxBankruptcyOffiallyFiled.TabIndex = 2;
            this.comboBoxBankruptcyOffiallyFiled.SelectedIndexChanged += new System.EventHandler(this.comboBoxBankruptcyOffiallyFiled_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(231, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "1. IS THE BANKRUPTCY OFFICIALLY FILED?";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(583, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "ATTORNEY MUST SEND COPY OF PETITION AND MEETING OF CREDITORS.  UNTIL THEN, BORROW" +
    "ER IS RESPONSIBLE FOR THE STATUS OF ACCOUNT.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BankruptcyACPQuestionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "BankruptcyACPQuestionsControl";
            this.Size = new System.Drawing.Size(600, 435);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxExtendedInfo.ResumeLayout(false);
            this.groupBoxExtendedInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxBankruptcyOffiallyFiled;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxAttorneyInformation;
        private System.Windows.Forms.GroupBox groupBoxExtendedInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxCourtInformation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxChapter;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxFilingDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxDocketNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxEndorser;
        private System.Windows.Forms.TextBox textBoxEndorser;
    }
}
