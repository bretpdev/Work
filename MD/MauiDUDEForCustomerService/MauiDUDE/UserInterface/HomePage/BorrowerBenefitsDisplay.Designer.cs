namespace MauiDUDE
{
    partial class BorrowerBenefitsDisplay
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
            this.label1 = new System.Windows.Forms.Label();
            this.servicingLoanDetailBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.servicingLoanDetailBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "LoanSeqNum", true));
            this.label1.Location = new System.Drawing.Point(-2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sequence #";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // servicingLoanDetailBindingSource
            // 
            this.servicingLoanDetailBindingSource.DataSource = typeof(MauiDUDE.ServicingLoanDetail);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "LoanType", true));
            this.label2.Location = new System.Drawing.Point(-3, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "FirstDisbursementDate", true));
            this.label3.Location = new System.Drawing.Point(-2, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Disbursement";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "OriginalBalance", true));
            this.label4.Location = new System.Drawing.Point(-2, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "Original Disbursement";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "CurrentPrincipal", true));
            this.label5.Location = new System.Drawing.Point(-2, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 18);
            this.label5.TabIndex = 4;
            this.label5.Text = "Current Principal";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label6.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "Status", true));
            this.label6.Location = new System.Drawing.Point(-2, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(144, 18);
            this.label6.TabIndex = 5;
            this.label6.Text = "Status";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-10, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(161, 10);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label7.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "InterestRate", true));
            this.label7.Location = new System.Drawing.Point(-2, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 18);
            this.label7.TabIndex = 7;
            this.label7.Text = "Current Interest";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "RegInt", true));
            this.label8.Location = new System.Drawing.Point(-3, 140);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(144, 18);
            this.label8.TabIndex = 8;
            this.label8.Text = "Reg Int";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label9.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "OnACH", true));
            this.label9.Location = new System.Drawing.Point(-3, 158);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(144, 18);
            this.label9.TabIndex = 9;
            this.label9.Text = "On ACH";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "ACHEligible", true));
            this.label10.Location = new System.Drawing.Point(-3, 176);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(144, 18);
            this.label10.TabIndex = 10;
            this.label10.Text = "ACH Eligibility";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label11.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "ACHRate", true));
            this.label11.Location = new System.Drawing.Point(-3, 194);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 18);
            this.label11.TabIndex = 11;
            this.label11.Text = "RDC ACH%";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "RIRCount", true));
            this.label12.Location = new System.Drawing.Point(-3, 212);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(144, 18);
            this.label12.TabIndex = 12;
            this.label12.Text = "RIR Count";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label13.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "RIRInt", true));
            this.label13.Location = new System.Drawing.Point(-3, 230);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 18);
            this.label13.TabIndex = 13;
            this.label13.Text = "RIR Int";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "RIRType", true));
            this.label14.Location = new System.Drawing.Point(-3, 248);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(144, 18);
            this.label14.TabIndex = 14;
            this.label14.Text = "RIR Type";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label15.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "HEP", true));
            this.label15.Location = new System.Drawing.Point(-3, 266);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(144, 18);
            this.label15.TabIndex = 15;
            this.label15.Text = "Grandfathered";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "RIREligibility", true));
            this.label16.Location = new System.Drawing.Point(-3, 284);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(144, 18);
            this.label16.TabIndex = 16;
            this.label16.Text = "RIR Eligibility";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label17.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.servicingLoanDetailBindingSource, "RIREligibilityDate", true));
            this.label17.Location = new System.Drawing.Point(-3, 303);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(144, 18);
            this.label17.TabIndex = 17;
            this.label17.Text = "RIR Eligibility Date";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BorrowerBenefitsDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "BorrowerBenefitsDisplay";
            this.Size = new System.Drawing.Size(113, 319);
            ((System.ComponentModel.ISupportInitialize)(this.servicingLoanDetailBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.BindingSource servicingLoanDetailBindingSource;
    }
}
