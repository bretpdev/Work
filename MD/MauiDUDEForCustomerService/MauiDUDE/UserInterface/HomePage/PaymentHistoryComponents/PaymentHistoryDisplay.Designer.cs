namespace MauiDUDE
{ 
    partial class PaymentHistoryDisplay
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
            this.components = new System.ComponentModel.Container();
            this.buttonExpand = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.financialTransactionWithLoanLevelDetailBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.financialTransactionWithLoanLevelDetailBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonExpand
            // 
            this.buttonExpand.Location = new System.Drawing.Point(1, 1);
            this.buttonExpand.Name = "buttonExpand";
            this.buttonExpand.Size = new System.Drawing.Size(22, 22);
            this.buttonExpand.TabIndex = 0;
            this.buttonExpand.Text = "+";
            this.buttonExpand.UseVisualStyleBackColor = true;
            this.buttonExpand.Click += new System.EventHandler(this.buttonExpand_Click);
            // 
            // label1
            // 
            this.label1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.financialTransactionWithLoanLevelDetailBindingSource, "ReversalReason", true));
            this.label1.Location = new System.Drawing.Point(24, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Reversal";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.financialTransactionWithLoanLevelDetailBindingSource, "EffectiveDate", true));
            this.label2.Location = new System.Drawing.Point(126, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Effective Date";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.financialTransactionWithLoanLevelDetailBindingSource, "PostedDate", true));
            this.label3.Location = new System.Drawing.Point(216, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 18);
            this.label3.TabIndex = 3;
            this.label3.Text = "Date Posted";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.financialTransactionWithLoanLevelDetailBindingSource, "TransactionType", true));
            this.label4.Location = new System.Drawing.Point(323, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 18);
            this.label4.TabIndex = 4;
            this.label4.Text = "Transaction Type";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel.Location = new System.Drawing.Point(329, 22);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(515, 2);
            this.flowLayoutPanel.TabIndex = 5;
            this.flowLayoutPanel.Visible = false;
            // 
            // label5
            // 
            this.label5.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.financialTransactionWithLoanLevelDetailBindingSource, "TransactionAmount", true));
            this.label5.Location = new System.Drawing.Point(416, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 18);
            this.label5.TabIndex = 6;
            this.label5.Text = "Transaction Amount";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label6.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.financialTransactionWithLoanLevelDetailBindingSource, "AppliedPrincipal", true));
            this.label6.Location = new System.Drawing.Point(533, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 18);
            this.label6.TabIndex = 7;
            this.label6.Text = "Principal";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.financialTransactionWithLoanLevelDetailBindingSource, "AppliedInterest", true));
            this.label7.Location = new System.Drawing.Point(629, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 18);
            this.label7.TabIndex = 8;
            this.label7.Text = "Interest";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label8.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.financialTransactionWithLoanLevelDetailBindingSource, "AppliedLateFee", true));
            this.label8.Location = new System.Drawing.Point(747, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 18);
            this.label8.TabIndex = 9;
            this.label8.Text = "Late Fees";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // financialTransactionWithLoanLevelDetailBindingSource
            // 
            this.financialTransactionWithLoanLevelDetailBindingSource.DataSource = typeof(MauiDUDE.FinancialTransactionWithLoanLevelDetail);
            // 
            // PaymentHistoryDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonExpand);
            this.Name = "PaymentHistoryDisplay";
            this.Size = new System.Drawing.Size(851, 39);
            ((System.ComponentModel.ISupportInitialize)(this.financialTransactionWithLoanLevelDetailBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonExpand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    private System.Windows.Forms.BindingSource financialTransactionWithLoanLevelDetailBindingSource;
}
}
