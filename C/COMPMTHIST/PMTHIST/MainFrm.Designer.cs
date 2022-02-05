namespace COMPMTHIST
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
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCancel2 = new System.Windows.Forms.Button();
            this.btnGetPaymentData = new System.Windows.Forms.Button();
            this.grpBorrowerInformation = new System.Windows.Forms.GroupBox();
            this.grpLoanInformation = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.borrowerPaymentInformationBindingSource)).BeginInit();
            this.grpBorrowerInformation.SuspendLayout();
            this.grpLoanInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSSN
            // 
            this.txtSSN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSSN.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPaymentInformationBindingSource, "SSN", true));
            this.txtSSN.Location = new System.Drawing.Point(57, 30);
            this.txtSSN.MaxLength = 9;
            this.txtSSN.Name = "txtSSN";
            this.txtSSN.Size = new System.Drawing.Size(171, 26);
            this.txtSSN.TabIndex = 0;
            // 
            // borrowerPaymentInformationBindingSource
            // 
            this.borrowerPaymentInformationBindingSource.DataSource = typeof(COMPMTHIST.BorrowerPaymentInformation);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "SSN";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(263, 25);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(99, 36);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel1
            // 
            this.btnCancel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel1.Location = new System.Drawing.Point(368, 25);
            this.btnCancel1.Name = "btnCancel1";
            this.btnCancel1.Size = new System.Drawing.Size(99, 36);
            this.btnCancel1.TabIndex = 3;
            this.btnCancel1.Text = "Cancel";
            this.btnCancel1.UseVisualStyleBackColor = true;
            // 
            // lvwLoans
            // 
            this.lvwLoans.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwLoans.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvwLoans.FullRowSelect = true;
            this.lvwLoans.HideSelection = false;
            this.lvwLoans.Location = new System.Drawing.Point(3, 22);
            this.lvwLoans.Name = "lvwLoans";
            this.lvwLoans.Size = new System.Drawing.Size(482, 334);
            this.lvwLoans.TabIndex = 4;
            this.lvwLoans.UseCompatibleStateImageBehavior = false;
            this.lvwLoans.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Sequence #";
            this.columnHeader1.Width = 103;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "1st Disb Date";
            this.columnHeader2.Width = 116;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Program";
            this.columnHeader3.Width = 110;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Balance";
            this.columnHeader4.Width = 137;
            // 
            // btnCancel2
            // 
            this.btnCancel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel2.Location = new System.Drawing.Point(322, 365);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size(152, 39);
            this.btnCancel2.TabIndex = 6;
            this.btnCancel2.Text = "Cancel";
            this.btnCancel2.UseVisualStyleBackColor = true;
            // 
            // btnGetPaymentData
            // 
            this.btnGetPaymentData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGetPaymentData.Location = new System.Drawing.Point(6, 365);
            this.btnGetPaymentData.Name = "btnGetPaymentData";
            this.btnGetPaymentData.Size = new System.Drawing.Size(152, 39);
            this.btnGetPaymentData.TabIndex = 5;
            this.btnGetPaymentData.Text = "Get Payment Data";
            this.btnGetPaymentData.UseVisualStyleBackColor = true;
            this.btnGetPaymentData.Click += new System.EventHandler(this.btnGetPaymentData_Click);
            // 
            // grpBorrowerInformation
            // 
            this.grpBorrowerInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBorrowerInformation.Controls.Add(this.btnOK);
            this.grpBorrowerInformation.Controls.Add(this.txtSSN);
            this.grpBorrowerInformation.Controls.Add(this.label1);
            this.grpBorrowerInformation.Controls.Add(this.btnCancel1);
            this.grpBorrowerInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBorrowerInformation.Location = new System.Drawing.Point(18, 3);
            this.grpBorrowerInformation.Name = "grpBorrowerInformation";
            this.grpBorrowerInformation.Size = new System.Drawing.Size(488, 76);
            this.grpBorrowerInformation.TabIndex = 7;
            this.grpBorrowerInformation.TabStop = false;
            this.grpBorrowerInformation.Text = "Borrower Information";
            // 
            // grpLoanInformation
            // 
            this.grpLoanInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLoanInformation.Controls.Add(this.button1);
            this.grpLoanInformation.Controls.Add(this.lvwLoans);
            this.grpLoanInformation.Controls.Add(this.btnGetPaymentData);
            this.grpLoanInformation.Controls.Add(this.btnCancel2);
            this.grpLoanInformation.Enabled = false;
            this.grpLoanInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpLoanInformation.Location = new System.Drawing.Point(18, 85);
            this.grpLoanInformation.Name = "grpLoanInformation";
            this.grpLoanInformation.Size = new System.Drawing.Size(488, 414);
            this.grpLoanInformation.TabIndex = 8;
            this.grpLoanInformation.TabStop = false;
            this.grpLoanInformation.Text = "Loan Information";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(164, 365);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(152, 39);
            this.button1.TabIndex = 7;
            this.button1.Text = "Select All Loans";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 503);
            this.Controls.Add(this.grpLoanInformation);
            this.Controls.Add(this.grpBorrowerInformation);
            this.MinimumSize = new System.Drawing.Size(536, 542);
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
        private System.Windows.Forms.Button button1;
    }
}