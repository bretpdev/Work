namespace AUXLTRS
{
    partial class SatisfactionDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSsn = new System.Windows.Forms.TextBox();
            this.satisfiedBorrowerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.chkSmallBalance = new System.Windows.Forms.CheckBox();
            this.chkEmployerRequest = new System.Windows.Forms.CheckBox();
            this.chkBankruptcy = new System.Windows.Forms.CheckBox();
            this.chkOther = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.satisfiedBorrowerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the SSN or Account Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "SSN/Acct No";
            // 
            // txtSsn
            // 
            this.txtSsn.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.satisfiedBorrowerBindingSource, "BorrowerId", true));
            this.txtSsn.Location = new System.Drawing.Point(91, 45);
            this.txtSsn.Name = "txtSsn";
            this.txtSsn.Size = new System.Drawing.Size(91, 20);
            this.txtSsn.TabIndex = 1;
            // 
            // satisfiedBorrowerBindingSource
            // 
            this.satisfiedBorrowerBindingSource.DataSource = typeof(AUXLTRS.Satisfaction);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(58, 170);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "and select a reason";
            // 
            // chkSmallBalance
            // 
            this.chkSmallBalance.AutoSize = true;
            this.chkSmallBalance.Location = new System.Drawing.Point(15, 71);
            this.chkSmallBalance.Name = "chkSmallBalance";
            this.chkSmallBalance.Size = new System.Drawing.Size(93, 17);
            this.chkSmallBalance.TabIndex = 9;
            this.chkSmallBalance.Text = "Small Balance";
            this.chkSmallBalance.UseVisualStyleBackColor = true;
            this.chkSmallBalance.Click += new System.EventHandler(this.chkSmallBalance_Click);
            // 
            // chkEmployerRequest
            // 
            this.chkEmployerRequest.AutoSize = true;
            this.chkEmployerRequest.Location = new System.Drawing.Point(15, 94);
            this.chkEmployerRequest.Name = "chkEmployerRequest";
            this.chkEmployerRequest.Size = new System.Drawing.Size(158, 17);
            this.chkEmployerRequest.TabIndex = 10;
            this.chkEmployerRequest.Text = "Employer Request (Resend)";
            this.chkEmployerRequest.UseVisualStyleBackColor = true;
            this.chkEmployerRequest.Click += new System.EventHandler(this.chkEmployerRequest_Click);
            // 
            // chkBankruptcy
            // 
            this.chkBankruptcy.AutoSize = true;
            this.chkBankruptcy.Location = new System.Drawing.Point(15, 117);
            this.chkBankruptcy.Name = "chkBankruptcy";
            this.chkBankruptcy.Size = new System.Drawing.Size(80, 17);
            this.chkBankruptcy.TabIndex = 11;
            this.chkBankruptcy.Text = "Bankruptcy";
            this.chkBankruptcy.UseVisualStyleBackColor = true;
            this.chkBankruptcy.Click += new System.EventHandler(this.chkBankruptcy_Click);
            // 
            // chkOther
            // 
            this.chkOther.AutoSize = true;
            this.chkOther.Location = new System.Drawing.Point(15, 140);
            this.chkOther.Name = "chkOther";
            this.chkOther.Size = new System.Drawing.Size(119, 17);
            this.chkOther.TabIndex = 12;
            this.chkOther.Text = "Management/Other";
            this.chkOther.UseVisualStyleBackColor = true;
            this.chkOther.Click += new System.EventHandler(this.chkOther_Click);
            // 
            // SatisfactionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(194, 204);
            this.Controls.Add(this.chkOther);
            this.Controls.Add(this.chkBankruptcy);
            this.Controls.Add(this.chkEmployerRequest);
            this.Controls.Add(this.chkSmallBalance);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtSsn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SatisfactionDialog";
            this.Text = "Notice of Satisfaction";
            ((System.ComponentModel.ISupportInitialize)(this.satisfiedBorrowerBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSsn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource satisfiedBorrowerBindingSource;
        private System.Windows.Forms.CheckBox chkSmallBalance;
        private System.Windows.Forms.CheckBox chkEmployerRequest;
        private System.Windows.Forms.CheckBox chkBankruptcy;
        private System.Windows.Forms.CheckBox chkOther;
    }
}