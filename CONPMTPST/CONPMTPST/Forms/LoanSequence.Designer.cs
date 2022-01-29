namespace CONPMTPST
{
    partial class LoanSequence
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
            this.BorrowerName = new System.Windows.Forms.Label();
            this.PayoffAmount = new System.Windows.Forms.Label();
            this.DisbursementDate = new System.Windows.Forms.Label();
            this.LoanType = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LoanSeqManual = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            this.Ok = new Uheaa.Common.WinForms.ValidationButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.MaximumSize = new System.Drawing.Size(340, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(323, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "The Loan Sequence number was not found. Please supply the sequence number.";
            // 
            // BorrowerName
            // 
            this.BorrowerName.AutoSize = true;
            this.BorrowerName.Location = new System.Drawing.Point(143, 70);
            this.BorrowerName.Name = "BorrowerName";
            this.BorrowerName.Size = new System.Drawing.Size(0, 20);
            this.BorrowerName.TabIndex = 1;
            // 
            // PayoffAmount
            // 
            this.PayoffAmount.AutoSize = true;
            this.PayoffAmount.Location = new System.Drawing.Point(143, 101);
            this.PayoffAmount.Name = "PayoffAmount";
            this.PayoffAmount.Size = new System.Drawing.Size(0, 20);
            this.PayoffAmount.TabIndex = 2;
            // 
            // DisbursementDate
            // 
            this.DisbursementDate.AutoSize = true;
            this.DisbursementDate.Location = new System.Drawing.Point(143, 132);
            this.DisbursementDate.Name = "DisbursementDate";
            this.DisbursementDate.Size = new System.Drawing.Size(0, 20);
            this.DisbursementDate.TabIndex = 3;
            // 
            // LoanType
            // 
            this.LoanType.AutoSize = true;
            this.LoanType.Location = new System.Drawing.Point(143, 163);
            this.LoanType.Name = "LoanType";
            this.LoanType.Size = new System.Drawing.Size(0, 20);
            this.LoanType.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Loan Type:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Disb. Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Payoff Amount:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(60, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "Borrower:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 193);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 20);
            this.label6.TabIndex = 9;
            this.label6.Text = "Loan Sequence:";
            // 
            // LoanSeqManual
            // 
            this.LoanSeqManual.AllowedSpecialCharacters = "";
            this.LoanSeqManual.Location = new System.Drawing.Point(147, 190);
            this.LoanSeqManual.MaxLength = 2;
            this.LoanSeqManual.Name = "LoanSeqManual";
            this.LoanSeqManual.Size = new System.Drawing.Size(45, 26);
            this.LoanSeqManual.TabIndex = 10;
            this.LoanSeqManual.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoanSeqManual_KeyDown);
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(278, 228);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(88, 34);
            this.Ok.TabIndex = 12;
            this.Ok.Text = "OK";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // LoanSequence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 268);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.LoanSeqManual);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.LoanType);
            this.Controls.Add(this.DisbursementDate);
            this.Controls.Add(this.PayoffAmount);
            this.Controls.Add(this.BorrowerName);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(391, 306);
            this.MinimumSize = new System.Drawing.Size(391, 306);
            this.Name = "LoanSequence";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loan Sequence";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoanSequence_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label BorrowerName;
        private System.Windows.Forms.Label PayoffAmount;
        private System.Windows.Forms.Label DisbursementDate;
        private System.Windows.Forms.Label LoanType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Uheaa.Common.WinForms.RequiredNumericTextBox LoanSeqManual;
        private Uheaa.Common.WinForms.ValidationButton Ok;
    }
}