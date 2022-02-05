namespace MauiDUDE
{
    partial class ServicingLoanDisplay
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
            this.labelSequenceNumber = new System.Windows.Forms.Label();
            this.servicingLoanDetailBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.labelType = new System.Windows.Forms.Label();
            this.labelDisbursement = new System.Windows.Forms.Label();
            this.labelOriginalDisbursement = new System.Windows.Forms.Label();
            this.labelCurrentPrincipal = new System.Windows.Forms.Label();
            this.labelInterestRate = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelSchool = new System.Windows.Forms.Label();
            this.labelSeparationDate = new System.Windows.Forms.Label();
            this.labelGraceEnd = new System.Windows.Forms.Label();
            this.labelRepaymentBeginDate = new System.Windows.Forms.Label();
            this.labelSchedule = new System.Windows.Forms.Label();
            this.labelRPSDate = new System.Windows.Forms.Label();
            this.labelTerm = new System.Windows.Forms.Label();
            this.labelDueDate = new System.Windows.Forms.Label();
            this.labelPaidAhead = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.servicingLoanDetailBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSequenceNumber
            // 
            this.labelSequenceNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "LoanSeqNum", true));
            this.labelSequenceNumber.Location = new System.Drawing.Point(0, 0);
            this.labelSequenceNumber.Name = "labelSequenceNumber";
            this.labelSequenceNumber.Size = new System.Drawing.Size(144, 20);
            this.labelSequenceNumber.TabIndex = 0;
            this.labelSequenceNumber.Text = "Sequence #";
            this.labelSequenceNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // servicingLoanDetailBindingSource
            // 
            this.servicingLoanDetailBindingSource.DataSource = typeof(MauiDUDE.ServicingLoanDetail);
            // 
            // labelType
            // 
            this.labelType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "LoanType", true));
            this.labelType.Location = new System.Drawing.Point(0, 20);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(144, 20);
            this.labelType.TabIndex = 1;
            this.labelType.Text = "Type";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDisbursement
            // 
            this.labelDisbursement.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "FirstDisbursementDate", true));
            this.labelDisbursement.Location = new System.Drawing.Point(0, 40);
            this.labelDisbursement.Name = "labelDisbursement";
            this.labelDisbursement.Size = new System.Drawing.Size(144, 20);
            this.labelDisbursement.TabIndex = 2;
            this.labelDisbursement.Text = "Disbursement";
            this.labelDisbursement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelOriginalDisbursement
            // 
            this.labelOriginalDisbursement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelOriginalDisbursement.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "OriginalBalance", true));
            this.labelOriginalDisbursement.Location = new System.Drawing.Point(0, 60);
            this.labelOriginalDisbursement.Name = "labelOriginalDisbursement";
            this.labelOriginalDisbursement.Size = new System.Drawing.Size(144, 20);
            this.labelOriginalDisbursement.TabIndex = 3;
            this.labelOriginalDisbursement.Text = "Original Disbursement";
            this.labelOriginalDisbursement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCurrentPrincipal
            // 
            this.labelCurrentPrincipal.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "CurrentPrincipal", true));
            this.labelCurrentPrincipal.Location = new System.Drawing.Point(0, 80);
            this.labelCurrentPrincipal.Name = "labelCurrentPrincipal";
            this.labelCurrentPrincipal.Size = new System.Drawing.Size(144, 20);
            this.labelCurrentPrincipal.TabIndex = 4;
            this.labelCurrentPrincipal.Text = "Current Principal";
            this.labelCurrentPrincipal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelInterestRate
            // 
            this.labelInterestRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelInterestRate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "InterestRate", true));
            this.labelInterestRate.Location = new System.Drawing.Point(0, 100);
            this.labelInterestRate.Name = "labelInterestRate";
            this.labelInterestRate.Size = new System.Drawing.Size(144, 20);
            this.labelInterestRate.TabIndex = 5;
            this.labelInterestRate.Text = "Interest Rate";
            this.labelInterestRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelStatus
            // 
            this.labelStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "Status", true));
            this.labelStatus.Location = new System.Drawing.Point(0, 120);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(144, 20);
            this.labelStatus.TabIndex = 6;
            this.labelStatus.Text = "Status";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(0, 137);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 10);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // labelSchool
            // 
            this.labelSchool.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelSchool.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "SchoolName", true));
            this.labelSchool.Location = new System.Drawing.Point(0, 150);
            this.labelSchool.Name = "labelSchool";
            this.labelSchool.Size = new System.Drawing.Size(144, 20);
            this.labelSchool.TabIndex = 8;
            this.labelSchool.Text = "School";
            this.labelSchool.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSeparationDate
            // 
            this.labelSeparationDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "SeparationDate", true));
            this.labelSeparationDate.Location = new System.Drawing.Point(0, 170);
            this.labelSeparationDate.Name = "labelSeparationDate";
            this.labelSeparationDate.Size = new System.Drawing.Size(144, 20);
            this.labelSeparationDate.TabIndex = 9;
            this.labelSeparationDate.Text = "Separation Date";
            this.labelSeparationDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelGraceEnd
            // 
            this.labelGraceEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelGraceEnd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "GraceEndDate", true));
            this.labelGraceEnd.Location = new System.Drawing.Point(0, 190);
            this.labelGraceEnd.Name = "labelGraceEnd";
            this.labelGraceEnd.Size = new System.Drawing.Size(144, 20);
            this.labelGraceEnd.TabIndex = 10;
            this.labelGraceEnd.Text = "Grace End";
            this.labelGraceEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRepaymentBeginDate
            // 
            this.labelRepaymentBeginDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "RepaymentStartDate", true));
            this.labelRepaymentBeginDate.Location = new System.Drawing.Point(0, 210);
            this.labelRepaymentBeginDate.Name = "labelRepaymentBeginDate";
            this.labelRepaymentBeginDate.Size = new System.Drawing.Size(144, 20);
            this.labelRepaymentBeginDate.TabIndex = 11;
            this.labelRepaymentBeginDate.Text = "Repayment Begin Date";
            this.labelRepaymentBeginDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSchedule
            // 
            this.labelSchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelSchedule.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "RepaymentType", true));
            this.labelSchedule.Location = new System.Drawing.Point(0, 230);
            this.labelSchedule.Name = "labelSchedule";
            this.labelSchedule.Size = new System.Drawing.Size(144, 20);
            this.labelSchedule.TabIndex = 12;
            this.labelSchedule.Text = "Sched";
            this.labelSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRPSDate
            // 
            this.labelRPSDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "RPSDate", true));
            this.labelRPSDate.Location = new System.Drawing.Point(0, 250);
            this.labelRPSDate.Name = "labelRPSDate";
            this.labelRPSDate.Size = new System.Drawing.Size(144, 20);
            this.labelRPSDate.TabIndex = 13;
            this.labelRPSDate.Text = "RPS Date";
            this.labelRPSDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTerm
            // 
            this.labelTerm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelTerm.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "RepaymentTerm", true));
            this.labelTerm.Location = new System.Drawing.Point(0, 270);
            this.labelTerm.Name = "labelTerm";
            this.labelTerm.Size = new System.Drawing.Size(144, 20);
            this.labelTerm.TabIndex = 14;
            this.labelTerm.Text = "Term";
            this.labelTerm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDueDate
            // 
            this.labelDueDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "DueDate", true));
            this.labelDueDate.Location = new System.Drawing.Point(0, 290);
            this.labelDueDate.Name = "labelDueDate";
            this.labelDueDate.Size = new System.Drawing.Size(144, 20);
            this.labelDueDate.TabIndex = 15;
            this.labelDueDate.Text = "Due Date";
            this.labelDueDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPaidAhead
            // 
            this.labelPaidAhead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelPaidAhead.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "PaidAhead", true));
            this.labelPaidAhead.Location = new System.Drawing.Point(0, 310);
            this.labelPaidAhead.Name = "labelPaidAhead";
            this.labelPaidAhead.Size = new System.Drawing.Size(144, 20);
            this.labelPaidAhead.TabIndex = 16;
            this.labelPaidAhead.Text = "Paid Ahead";
            this.labelPaidAhead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ServicingLoanDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.labelPaidAhead);
            this.Controls.Add(this.labelDueDate);
            this.Controls.Add(this.labelTerm);
            this.Controls.Add(this.labelRPSDate);
            this.Controls.Add(this.labelSchedule);
            this.Controls.Add(this.labelRepaymentBeginDate);
            this.Controls.Add(this.labelGraceEnd);
            this.Controls.Add(this.labelSeparationDate);
            this.Controls.Add(this.labelSchool);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelInterestRate);
            this.Controls.Add(this.labelCurrentPrincipal);
            this.Controls.Add(this.labelOriginalDisbursement);
            this.Controls.Add(this.labelDisbursement);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.labelSequenceNumber);
            this.Name = "ServicingLoanDisplay";
            this.Size = new System.Drawing.Size(140, 344);
            ((System.ComponentModel.ISupportInitialize)(this.servicingLoanDetailBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelSequenceNumber;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelDisbursement;
        private System.Windows.Forms.Label labelOriginalDisbursement;
        private System.Windows.Forms.Label labelCurrentPrincipal;
        private System.Windows.Forms.Label labelInterestRate;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelSchool;
        private System.Windows.Forms.Label labelSeparationDate;
        private System.Windows.Forms.Label labelGraceEnd;
        private System.Windows.Forms.Label labelRepaymentBeginDate;
        private System.Windows.Forms.Label labelSchedule;
        private System.Windows.Forms.Label labelRPSDate;
        private System.Windows.Forms.Label labelTerm;
        private System.Windows.Forms.Label labelDueDate;
        private System.Windows.Forms.Label labelPaidAhead;
        private System.Windows.Forms.BindingSource servicingLoanDetailBindingSource;
    }
}
