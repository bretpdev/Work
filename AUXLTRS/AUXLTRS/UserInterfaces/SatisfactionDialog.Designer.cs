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
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the Ssn or Account Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 74);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ssn/Acct No";
            // 
            // txtSsn
            // 
            this.txtSsn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSsn.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.satisfiedBorrowerBindingSource, "BorrowerId", true));
            this.txtSsn.Location = new System.Drawing.Point(136, 69);
            this.txtSsn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSsn.Name = "txtSsn";
            this.txtSsn.Size = new System.Drawing.Size(134, 26);
            this.txtSsn.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(87, 262);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 35);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "and select a reason";
            // 
            // chkSmallBalance
            // 
            this.chkSmallBalance.AutoSize = true;
            this.chkSmallBalance.Location = new System.Drawing.Point(22, 109);
            this.chkSmallBalance.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSmallBalance.Name = "chkSmallBalance";
            this.chkSmallBalance.Size = new System.Drawing.Size(129, 24);
            this.chkSmallBalance.TabIndex = 9;
            this.chkSmallBalance.Text = "Small Balance";
            this.chkSmallBalance.UseVisualStyleBackColor = true;
            this.chkSmallBalance.Click += new System.EventHandler(this.chkSmallBalance_Click);
            // 
            // chkEmployerRequest
            // 
            this.chkEmployerRequest.AutoSize = true;
            this.chkEmployerRequest.Location = new System.Drawing.Point(22, 145);
            this.chkEmployerRequest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkEmployerRequest.Name = "chkEmployerRequest";
            this.chkEmployerRequest.Size = new System.Drawing.Size(229, 24);
            this.chkEmployerRequest.TabIndex = 10;
            this.chkEmployerRequest.Text = "Employer Request (Resend)";
            this.chkEmployerRequest.UseVisualStyleBackColor = true;
            this.chkEmployerRequest.Click += new System.EventHandler(this.chkEmployerRequest_Click);
            // 
            // chkBankruptcy
            // 
            this.chkBankruptcy.AutoSize = true;
            this.chkBankruptcy.Location = new System.Drawing.Point(22, 180);
            this.chkBankruptcy.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkBankruptcy.Name = "chkBankruptcy";
            this.chkBankruptcy.Size = new System.Drawing.Size(108, 24);
            this.chkBankruptcy.TabIndex = 11;
            this.chkBankruptcy.Text = "Bankruptcy";
            this.chkBankruptcy.UseVisualStyleBackColor = true;
            this.chkBankruptcy.Click += new System.EventHandler(this.chkBankruptcy_Click);
            // 
            // chkOther
            // 
            this.chkOther.AutoSize = true;
            this.chkOther.Location = new System.Drawing.Point(22, 215);
            this.chkOther.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkOther.Name = "chkOther";
            this.chkOther.Size = new System.Drawing.Size(166, 24);
            this.chkOther.TabIndex = 12;
            this.chkOther.Text = "Management/Other";
            this.chkOther.UseVisualStyleBackColor = true;
            this.chkOther.Click += new System.EventHandler(this.chkOther_Click);
            // 
            // SatisfactionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 314);
            this.Controls.Add(this.chkOther);
            this.Controls.Add(this.chkBankruptcy);
            this.Controls.Add(this.chkEmployerRequest);
            this.Controls.Add(this.chkSmallBalance);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtSsn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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