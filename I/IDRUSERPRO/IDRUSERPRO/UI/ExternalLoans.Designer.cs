namespace IDRUSERPRO
{
    partial class ExternalLoans
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoFfelp = new System.Windows.Forms.RadioButton();
            this.rdoDirect = new System.Windows.Forms.RadioButton();
            this.lblLoanCount = new System.Windows.Forms.Label();
            this.lblLoanType = new System.Windows.Forms.Label();
            this.txtLoanType = new System.Windows.Forms.TextBox();
            this.lblOwner = new System.Windows.Forms.Label();
            this.lblbalance = new System.Windows.Forms.Label();
            this.txtOutstandingBalance = new System.Windows.Forms.TextBox();
            this.lblPay = new System.Windows.Forms.Label();
            this.txtMonthlyPay = new System.Windows.Forms.TextBox();
            this.lblInterestrate = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCont = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.SameRegion = new System.Windows.Forms.GroupBox();
            this.SameRegionNo = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.SameRegionYes = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.Loans = new System.Windows.Forms.DataGridView();
            this.txtOwnerLender = new Uheaa.Common.WinForms.NumericTextBox();
            this.txtIntRate = new Uheaa.Common.WinForms.NumericTextBox();
            this.txtOutstandingInterest = new System.Windows.Forms.TextBox();
            this.lblOutstandingInterest = new System.Windows.Forms.Label();
            this.BorrowerTypeLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SameRegion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Loans)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox1.Controls.Add(this.rdoFfelp);
            this.groupBox1.Controls.Add(this.rdoDirect);
            this.groupBox1.Controls.Add(this.lblLoanCount);
            this.groupBox1.Location = new System.Drawing.Point(18, 73);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(353, 48);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // rdoFfelp
            // 
            this.rdoFfelp.AutoSize = true;
            this.rdoFfelp.Location = new System.Drawing.Point(98, 18);
            this.rdoFfelp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdoFfelp.Name = "rdoFfelp";
            this.rdoFfelp.Size = new System.Drawing.Size(77, 24);
            this.rdoFfelp.TabIndex = 1;
            this.rdoFfelp.Text = "FFELP";
            this.rdoFfelp.UseVisualStyleBackColor = true;
            // 
            // rdoDirect
            // 
            this.rdoDirect.AutoSize = true;
            this.rdoDirect.Checked = true;
            this.rdoDirect.Location = new System.Drawing.Point(9, 18);
            this.rdoDirect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdoDirect.Name = "rdoDirect";
            this.rdoDirect.Size = new System.Drawing.Size(69, 24);
            this.rdoDirect.TabIndex = 0;
            this.rdoDirect.TabStop = true;
            this.rdoDirect.Text = "Direct";
            this.rdoDirect.UseVisualStyleBackColor = true;
            // 
            // lblLoanCount
            // 
            this.lblLoanCount.AutoSize = true;
            this.lblLoanCount.Location = new System.Drawing.Point(359, 0);
            this.lblLoanCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoanCount.Name = "lblLoanCount";
            this.lblLoanCount.Size = new System.Drawing.Size(100, 20);
            this.lblLoanCount.TabIndex = 2;
            this.lblLoanCount.Text = "Loan Count: ";
            // 
            // lblLoanType
            // 
            this.lblLoanType.AutoSize = true;
            this.lblLoanType.Location = new System.Drawing.Point(160, 172);
            this.lblLoanType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoanType.Name = "lblLoanType";
            this.lblLoanType.Size = new System.Drawing.Size(87, 20);
            this.lblLoanType.TabIndex = 2;
            this.lblLoanType.Text = "Loan Type:";
            // 
            // txtLoanType
            // 
            this.txtLoanType.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLoanType.Location = new System.Drawing.Point(259, 169);
            this.txtLoanType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtLoanType.MaxLength = 6;
            this.txtLoanType.Name = "txtLoanType";
            this.txtLoanType.Size = new System.Drawing.Size(111, 26);
            this.txtLoanType.TabIndex = 3;
            this.txtLoanType.TextChanged += new System.EventHandler(this.Field_TextChanged);
            this.txtLoanType.Leave += new System.EventHandler(this.txtLoanType_Leave);
            // 
            // lblOwner
            // 
            this.lblOwner.AutoSize = true;
            this.lblOwner.Location = new System.Drawing.Point(135, 205);
            this.lblOwner.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOwner.Name = "lblOwner";
            this.lblOwner.Size = new System.Drawing.Size(113, 20);
            this.lblOwner.TabIndex = 4;
            this.lblOwner.Text = "Owner/Lender:";
            // 
            // lblbalance
            // 
            this.lblbalance.AutoSize = true;
            this.lblbalance.Location = new System.Drawing.Point(88, 235);
            this.lblbalance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblbalance.Name = "lblbalance";
            this.lblbalance.Size = new System.Drawing.Size(162, 20);
            this.lblbalance.TabIndex = 6;
            this.lblbalance.Text = "Outstanding Balance:";
            // 
            // txtOutstandingBalance
            // 
            this.txtOutstandingBalance.Location = new System.Drawing.Point(259, 231);
            this.txtOutstandingBalance.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOutstandingBalance.Name = "txtOutstandingBalance";
            this.txtOutstandingBalance.Size = new System.Drawing.Size(148, 26);
            this.txtOutstandingBalance.TabIndex = 5;
            this.txtOutstandingBalance.TextChanged += new System.EventHandler(this.Field_TextChanged);
            this.txtOutstandingBalance.Leave += new System.EventHandler(this.txtOutstandingBalance_Leave);
            // 
            // lblPay
            // 
            this.lblPay.AutoSize = true;
            this.lblPay.Location = new System.Drawing.Point(78, 340);
            this.lblPay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPay.Name = "lblPay";
            this.lblPay.Size = new System.Drawing.Size(171, 20);
            this.lblPay.TabIndex = 10;
            this.lblPay.Text = "Initial STD Monthly Pay";
            // 
            // txtMonthlyPay
            // 
            this.txtMonthlyPay.Enabled = false;
            this.txtMonthlyPay.Location = new System.Drawing.Point(259, 337);
            this.txtMonthlyPay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMonthlyPay.MaxLength = 10;
            this.txtMonthlyPay.Name = "txtMonthlyPay";
            this.txtMonthlyPay.Size = new System.Drawing.Size(148, 26);
            this.txtMonthlyPay.TabIndex = 8;
            this.txtMonthlyPay.TextChanged += new System.EventHandler(this.Field_TextChanged);
            // 
            // lblInterestrate
            // 
            this.lblInterestrate.AutoSize = true;
            this.lblInterestrate.Location = new System.Drawing.Point(150, 307);
            this.lblInterestrate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInterestrate.Name = "lblInterestrate";
            this.lblInterestrate.Size = new System.Drawing.Size(103, 20);
            this.lblInterestrate.TabIndex = 8;
            this.lblInterestrate.Text = "Interest Rate";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(8, 484);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(112, 35);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(1296, 484);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 35);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCont
            // 
            this.btnCont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCont.Enabled = false;
            this.btnCont.Location = new System.Drawing.Point(1190, 484);
            this.btnCont.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCont.Name = "btnCont";
            this.btnCont.Size = new System.Drawing.Size(97, 35);
            this.btnCont.TabIndex = 13;
            this.btnCont.Text = "Continue";
            this.btnCont.UseVisualStyleBackColor = true;
            this.btnCont.Click += new System.EventHandler(this.btnCont_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(129, 484);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(112, 35);
            this.btnRemove.TabIndex = 12;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(550, 11);
            this.lblCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(18, 20);
            this.lblCount.TabIndex = 19;
            this.lblCount.Text = "0";
            // 
            // SameRegion
            // 
            this.SameRegion.BackColor = System.Drawing.SystemColors.Control;
            this.SameRegion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SameRegion.Controls.Add(this.SameRegionNo);
            this.SameRegion.Controls.Add(this.label2);
            this.SameRegion.Controls.Add(this.SameRegionYes);
            this.SameRegion.Enabled = false;
            this.SameRegion.Location = new System.Drawing.Point(15, 120);
            this.SameRegion.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SameRegion.Name = "SameRegion";
            this.SameRegion.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SameRegion.Size = new System.Drawing.Size(385, 48);
            this.SameRegion.TabIndex = 2;
            this.SameRegion.TabStop = false;
            // 
            // SameRegionNo
            // 
            this.SameRegionNo.AutoSize = true;
            this.SameRegionNo.Location = new System.Drawing.Point(309, 11);
            this.SameRegionNo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SameRegionNo.Name = "SameRegionNo";
            this.SameRegionNo.Size = new System.Drawing.Size(47, 24);
            this.SameRegionNo.TabIndex = 1;
            this.SameRegionNo.Text = "No";
            this.SameRegionNo.UseVisualStyleBackColor = true;
            this.SameRegionNo.CheckedChanged += new System.EventHandler(this.SameRegion_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(229, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Spouse Loans in Same Region";
            // 
            // SameRegionYes
            // 
            this.SameRegionYes.AutoSize = true;
            this.SameRegionYes.Location = new System.Drawing.Point(244, 11);
            this.SameRegionYes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SameRegionYes.Name = "SameRegionYes";
            this.SameRegionYes.Size = new System.Drawing.Size(55, 24);
            this.SameRegionYes.TabIndex = 0;
            this.SameRegionYes.TabStop = true;
            this.SameRegionYes.Text = "Yes";
            this.SameRegionYes.UseVisualStyleBackColor = true;
            this.SameRegionYes.CheckedChanged += new System.EventHandler(this.SameRegion_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(441, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.TabIndex = 18;
            this.label1.Text = "Loan Count: ";
            // 
            // Loans
            // 
            this.Loans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Loans.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.Loans.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Loans.Location = new System.Drawing.Point(444, 38);
            this.Loans.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Loans.Name = "Loans";
            this.Loans.ReadOnly = true;
            this.Loans.RowHeadersVisible = false;
            this.Loans.Size = new System.Drawing.Size(945, 434);
            this.Loans.TabIndex = 20;
            this.Loans.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Loans_CellContentDoubleClick);
            // 
            // txtOwnerLender
            // 
            this.txtOwnerLender.AllowedSpecialCharacters = "";
            this.txtOwnerLender.Location = new System.Drawing.Point(259, 201);
            this.txtOwnerLender.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOwnerLender.MaxLength = 6;
            this.txtOwnerLender.Name = "txtOwnerLender";
            this.txtOwnerLender.Size = new System.Drawing.Size(131, 26);
            this.txtOwnerLender.TabIndex = 4;
            this.txtOwnerLender.TextChanged += new System.EventHandler(this.Field_TextChanged);
            this.txtOwnerLender.Leave += new System.EventHandler(this.txtOwnerLender_Leave);
            // 
            // txtIntRate
            // 
            this.txtIntRate.AllowedSpecialCharacters = ".";
            this.txtIntRate.Location = new System.Drawing.Point(259, 303);
            this.txtIntRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtIntRate.MaxLength = 6;
            this.txtIntRate.Name = "txtIntRate";
            this.txtIntRate.Size = new System.Drawing.Size(56, 26);
            this.txtIntRate.TabIndex = 7;
            this.txtIntRate.TextChanged += new System.EventHandler(this.Field_TextChanged);
            this.txtIntRate.Leave += new System.EventHandler(this.txtIntRate_Leave);
            // 
            // txtOutstandingInterest
            // 
            this.txtOutstandingInterest.Location = new System.Drawing.Point(259, 267);
            this.txtOutstandingInterest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOutstandingInterest.Name = "txtOutstandingInterest";
            this.txtOutstandingInterest.Size = new System.Drawing.Size(148, 26);
            this.txtOutstandingInterest.TabIndex = 6;
            this.txtOutstandingInterest.TextChanged += new System.EventHandler(this.Field_TextChanged);
            // 
            // lblOutstandingInterest
            // 
            this.lblOutstandingInterest.AutoSize = true;
            this.lblOutstandingInterest.Location = new System.Drawing.Point(88, 271);
            this.lblOutstandingInterest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutstandingInterest.Name = "lblOutstandingInterest";
            this.lblOutstandingInterest.Size = new System.Drawing.Size(159, 20);
            this.lblOutstandingInterest.TabIndex = 22;
            this.lblOutstandingInterest.Text = "Outstanding Interest:";
            // 
            // BorrowerTypeLabel
            // 
            this.BorrowerTypeLabel.AutoSize = true;
            this.BorrowerTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BorrowerTypeLabel.Location = new System.Drawing.Point(22, 26);
            this.BorrowerTypeLabel.Name = "BorrowerTypeLabel";
            this.BorrowerTypeLabel.Size = new System.Drawing.Size(251, 26);
            this.BorrowerTypeLabel.TabIndex = 23;
            this.BorrowerTypeLabel.Text = "Borrower External Loans";
            // 
            // ExternalLoans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1406, 535);
            this.Controls.Add(this.BorrowerTypeLabel);
            this.Controls.Add(this.txtOutstandingInterest);
            this.Controls.Add(this.lblOutstandingInterest);
            this.Controls.Add(this.Loans);
            this.Controls.Add(this.SameRegion);
            this.Controls.Add(this.txtOwnerLender);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnCont);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtIntRate);
            this.Controls.Add(this.lblInterestrate);
            this.Controls.Add(this.txtMonthlyPay);
            this.Controls.Add(this.lblPay);
            this.Controls.Add(this.txtOutstandingBalance);
            this.Controls.Add(this.lblbalance);
            this.Controls.Add(this.lblOwner);
            this.Controls.Add(this.txtLoanType);
            this.Controls.Add(this.lblLoanType);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1422, 533);
            this.Name = "ExternalLoans";
            this.ShowIcon = false;
            this.Text = "Enter External Loans";
            this.Load += new System.EventHandler(this.LoansOtherServicers_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.SameRegion.ResumeLayout(false);
            this.SameRegion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Loans)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoFfelp;
        private System.Windows.Forms.RadioButton rdoDirect;
        private System.Windows.Forms.Label lblLoanType;
        private System.Windows.Forms.TextBox txtLoanType;
        private System.Windows.Forms.Label lblOwner;
        private System.Windows.Forms.Label lblbalance;
        private System.Windows.Forms.TextBox txtOutstandingBalance;
        private System.Windows.Forms.Label lblPay;
        private System.Windows.Forms.TextBox txtMonthlyPay;
        private System.Windows.Forms.Label lblInterestrate;
        private Uheaa.Common.WinForms.NumericTextBox txtIntRate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCont;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label lblLoanCount;
        private System.Windows.Forms.Label lblCount;
        private Uheaa.Common.WinForms.NumericTextBox txtOwnerLender;
        private System.Windows.Forms.GroupBox SameRegion;
        private System.Windows.Forms.RadioButton SameRegionNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton SameRegionYes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView Loans;
        private System.Windows.Forms.TextBox txtOutstandingInterest;
        private System.Windows.Forms.Label lblOutstandingInterest;
        private System.Windows.Forms.Label BorrowerTypeLabel;
    }
}