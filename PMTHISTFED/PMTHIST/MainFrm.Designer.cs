namespace PMTHISTFED
{
    partial class MainFrm
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
            this.txtSSN = new System.Windows.Forms.TextBox();
            this.borrowerPaymentInformationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel1 = new System.Windows.Forms.Button();
            this.lvwLoans = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.btnCancel2 = new System.Windows.Forms.Button();
            this.btnGetPaymentData = new System.Windows.Forms.Button();
            this.grpBorrowerInformation = new System.Windows.Forms.GroupBox();
            this.grpLoanInformation = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.borrowerPaymentInformationBindingSource)).BeginInit();
            this.grpBorrowerInformation.SuspendLayout();
            this.grpLoanInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSSN
            // 
            this.txtSSN.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPaymentInformationBindingSource, "AccountNumber", true));
            this.txtSSN.Location = new System.Drawing.Point(164, 18);
            this.txtSSN.MaxLength = 10;
            this.txtSSN.Name = "txtSSN";
            this.txtSSN.Size = new System.Drawing.Size(144, 20);
            this.txtSSN.TabIndex = 0;
            // 
            // borrowerPaymentInformationBindingSource
            // 
            this.borrowerPaymentInformationBindingSource.DataSource = typeof(PMTHISTFED.BorrowerPaymentInformation);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Account Number";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(16, 46);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(143, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel1
            // 
            this.btnCancel1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel1.Location = new System.Drawing.Point(165, 46);
            this.btnCancel1.Name = "btnCancel1";
            this.btnCancel1.Size = new System.Drawing.Size(143, 23);
            this.btnCancel1.TabIndex = 3;
            this.btnCancel1.Text = "Cancel";
            this.btnCancel1.UseVisualStyleBackColor = true;
            // 
            // lvwLoans
            // 
            this.lvwLoans.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvwLoans.FullRowSelect = true;
            this.lvwLoans.Location = new System.Drawing.Point(16, 19);
            this.lvwLoans.Name = "lvwLoans";
            this.lvwLoans.Size = new System.Drawing.Size(292, 285);
            this.lvwLoans.TabIndex = 4;
            this.lvwLoans.UseCompatibleStateImageBehavior = false;
            this.lvwLoans.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Sequence #";
            this.columnHeader1.Width = 77;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "1st Disb Date";
            this.columnHeader2.Width = 78;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Program";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Balance";
            this.columnHeader4.Width = 73;
            // 
            // btnCancel2
            // 
            this.btnCancel2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel2.Location = new System.Drawing.Point(163, 310);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size(143, 23);
            this.btnCancel2.TabIndex = 6;
            this.btnCancel2.Text = "Cancel";
            this.btnCancel2.UseVisualStyleBackColor = true;
            // 
            // btnGetPaymentData
            // 
            this.btnGetPaymentData.Location = new System.Drawing.Point(14, 310);
            this.btnGetPaymentData.Name = "btnGetPaymentData";
            this.btnGetPaymentData.Size = new System.Drawing.Size(143, 23);
            this.btnGetPaymentData.TabIndex = 5;
            this.btnGetPaymentData.Text = "Get Payment Data";
            this.btnGetPaymentData.UseVisualStyleBackColor = true;
            this.btnGetPaymentData.Click += new System.EventHandler(this.btnGetPaymentData_Click);
            // 
            // grpBorrowerInformation
            // 
            this.grpBorrowerInformation.Controls.Add(this.btnOK);
            this.grpBorrowerInformation.Controls.Add(this.txtSSN);
            this.grpBorrowerInformation.Controls.Add(this.label1);
            this.grpBorrowerInformation.Controls.Add(this.btnCancel1);
            this.grpBorrowerInformation.Location = new System.Drawing.Point(3, 3);
            this.grpBorrowerInformation.Name = "grpBorrowerInformation";
            this.grpBorrowerInformation.Size = new System.Drawing.Size(322, 80);
            this.grpBorrowerInformation.TabIndex = 7;
            this.grpBorrowerInformation.TabStop = false;
            this.grpBorrowerInformation.Text = "Borrower Information";
            // 
            // grpLoanInformation
            // 
            this.grpLoanInformation.Controls.Add(this.lvwLoans);
            this.grpLoanInformation.Controls.Add(this.btnGetPaymentData);
            this.grpLoanInformation.Controls.Add(this.btnCancel2);
            this.grpLoanInformation.Enabled = false;
            this.grpLoanInformation.Location = new System.Drawing.Point(3, 89);
            this.grpLoanInformation.Name = "grpLoanInformation";
            this.grpLoanInformation.Size = new System.Drawing.Size(322, 344);
            this.grpLoanInformation.TabIndex = 8;
            this.grpLoanInformation.TabStop = false;
            this.grpLoanInformation.Text = "Loan Information";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 435);
            this.Controls.Add(this.grpLoanInformation);
            this.Controls.Add(this.grpBorrowerInformation);
            this.Name = "MainFrm";
            this.Text = "Compass Borrower Payment History";
            ((System.ComponentModel.ISupportInitialize)(this.borrowerPaymentInformationBindingSource)).EndInit();
            this.grpBorrowerInformation.ResumeLayout(false);
            this.grpBorrowerInformation.PerformLayout();
            this.grpLoanInformation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSSN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel1;
        private System.Windows.Forms.ListView lvwLoans;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btnCancel2;
        private System.Windows.Forms.Button btnGetPaymentData;
        private System.Windows.Forms.BindingSource borrowerPaymentInformationBindingSource;
        private System.Windows.Forms.GroupBox grpBorrowerInformation;
        private System.Windows.Forms.GroupBox grpLoanInformation;
    }
}