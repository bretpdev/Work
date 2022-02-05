namespace PUTSUSPCOM
{
    partial class DeleteAndReapplyRow
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
            this.txtLoanSequence = new System.Windows.Forms.TextBox();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.txtDisbursementDate = new System.Windows.Forms.TextBox();
            this.txtTransactionType = new System.Windows.Forms.TextBox();
            this.deleteAndReapplyRowDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.deleteAndReapplyRowDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLoanSequence
            // 
            this.txtLoanSequence.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.deleteAndReapplyRowDataBindingSource, "LoanSequenceNumber", true));
            this.txtLoanSequence.Location = new System.Drawing.Point(0, 4);
            this.txtLoanSequence.Name = "txtLoanSequence";
            this.txtLoanSequence.Size = new System.Drawing.Size(150, 20);
            this.txtLoanSequence.TabIndex = 0;
            // 
            // txtAmount
            // 
            this.txtAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.deleteAndReapplyRowDataBindingSource, "Amount", true));
            this.txtAmount.Location = new System.Drawing.Point(151, 4);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(150, 20);
            this.txtAmount.TabIndex = 1;
            // 
            // txtDisbursementDate
            // 
            this.txtDisbursementDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.deleteAndReapplyRowDataBindingSource, "DisbursementDate", true));
            this.txtDisbursementDate.Location = new System.Drawing.Point(302, 4);
            this.txtDisbursementDate.Name = "txtDisbursementDate";
            this.txtDisbursementDate.Size = new System.Drawing.Size(150, 20);
            this.txtDisbursementDate.TabIndex = 2;
            // 
            // txtTransactionType
            // 
            this.txtTransactionType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.deleteAndReapplyRowDataBindingSource, "TransactionType", true));
            this.txtTransactionType.Location = new System.Drawing.Point(452, 4);
            this.txtTransactionType.Name = "txtTransactionType";
            this.txtTransactionType.Size = new System.Drawing.Size(150, 20);
            this.txtTransactionType.TabIndex = 3;
            // 
            // deleteAndReapplyRowDataBindingSource
            // 
            this.deleteAndReapplyRowDataBindingSource.DataSource = typeof(PUTSUSPCOM.DeleteAndReapplyRowData);
            // 
            // DeleteAndReapplyRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtTransactionType);
            this.Controls.Add(this.txtDisbursementDate);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.txtLoanSequence);
            this.Name = "DeleteAndReapplyRow";
            this.Size = new System.Drawing.Size(606, 28);
            ((System.ComponentModel.ISupportInitialize)(this.deleteAndReapplyRowDataBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLoanSequence;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.TextBox txtDisbursementDate;
        private System.Windows.Forms.TextBox txtTransactionType;
        private System.Windows.Forms.BindingSource deleteAndReapplyRowDataBindingSource;
    }
}
