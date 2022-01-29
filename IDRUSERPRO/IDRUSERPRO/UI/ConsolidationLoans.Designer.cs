namespace IDRUSERPRO
{
    partial class ConsolidationLoans
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblLoans = new System.Windows.Forms.Label();
            this.lsvConsolLoans = new System.Windows.Forms.ListView();
            this.LoanId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LoanType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LoanSeq = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DisbDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(314, 349);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 35);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(413, 349);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 35);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblLoans
            // 
            this.lblLoans.AutoSize = true;
            this.lblLoans.Location = new System.Drawing.Point(18, 14);
            this.lblLoans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoans.Name = "lblLoans";
            this.lblLoans.Size = new System.Drawing.Size(213, 20);
            this.lblLoans.TabIndex = 3;
            this.lblLoans.Text = "Select all Non-Eligible Loans:";
            // 
            // lsvConsolLoans
            // 
            this.lsvConsolLoans.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvConsolLoans.CheckBoxes = true;
            this.lsvConsolLoans.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.LoanId,
            this.LoanType,
            this.LoanSeq,
            this.DisbDate});
            this.lsvConsolLoans.Location = new System.Drawing.Point(22, 39);
            this.lsvConsolLoans.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lsvConsolLoans.Name = "lsvConsolLoans";
            this.lsvConsolLoans.Size = new System.Drawing.Size(476, 299);
            this.lsvConsolLoans.TabIndex = 4;
            this.lsvConsolLoans.UseCompatibleStateImageBehavior = false;
            this.lsvConsolLoans.View = System.Windows.Forms.View.Details;
            // 
            // LoanId
            // 
            this.LoanId.Text = "Loan Id";
            this.LoanId.Width = 78;
            // 
            // LoanType
            // 
            this.LoanType.Text = "Loan Type";
            this.LoanType.Width = 111;
            // 
            // LoanSeq
            // 
            this.LoanSeq.Text = "Loan Sequence";
            this.LoanSeq.Width = 144;
            // 
            // DisbDate
            // 
            this.DisbDate.Text = "Disb Date";
            this.DisbDate.Width = 79;
            // 
            // ConsolidationLoans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 392);
            this.Controls.Add(this.lsvConsolLoans);
            this.Controls.Add(this.lblLoans);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(535, 430);
            this.Name = "ConsolidationLoans";
            this.ShowIcon = false;
            this.Text = "Loan Verification";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblLoans;
        private System.Windows.Forms.ListView lsvConsolLoans;
        private System.Windows.Forms.ColumnHeader LoanId;
        private System.Windows.Forms.ColumnHeader LoanType;
        private System.Windows.Forms.ColumnHeader LoanSeq;
        private System.Windows.Forms.ColumnHeader DisbDate;
    }
}