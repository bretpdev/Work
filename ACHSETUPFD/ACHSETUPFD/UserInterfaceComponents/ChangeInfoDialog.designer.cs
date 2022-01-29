namespace ACHSETUPFD
{
	partial class ChangeInfoDialog
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
            this.changeDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.txtAccountType = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.lblEft = new System.Windows.Forms.Label();
            this.chkPaper = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox1 = new Uheaa.Common.WinForms.SsnTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.changeDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // changeDataBindingSource
            // 
            this.changeDataBindingSource.DataSource = typeof(ACHSETUPFD.ChangeData);
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.changeDataBindingSource, "RecipientSsn", true));
            this.textBox2.Location = new System.Drawing.Point(222, 100);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(148, 26);
            this.textBox2.TabIndex = 1;
            // 
            // textBox3
            // 
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.changeDataBindingSource, "AbaNumber", true));
            this.textBox3.Location = new System.Drawing.Point(222, 140);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(148, 26);
            this.textBox3.TabIndex = 2;
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.changeDataBindingSource, "BankAccountNumber", true));
            this.textBox4.Location = new System.Drawing.Point(222, 180);
            this.textBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(148, 26);
            this.textBox4.TabIndex = 3;
            // 
            // txtAccountType
            // 
            this.txtAccountType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.changeDataBindingSource, "AccountType", true));
            this.txtAccountType.Location = new System.Drawing.Point(222, 220);
            this.txtAccountType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAccountType.Name = "txtAccountType";
            this.txtAccountType.Size = new System.Drawing.Size(28, 26);
            this.txtAccountType.TabIndex = 4;
            this.txtAccountType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAccountType_KeyDown);
            // 
            // textBox6
            // 
            this.textBox6.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.changeDataBindingSource, "AdditionalWithdrawalAmount", true));
            this.textBox6.Location = new System.Drawing.Point(222, 306);
            this.textBox6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(148, 26);
            this.textBox6.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Borrower SSN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 105);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Recipient SSN";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 145);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "ABA Number";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 185);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Bank Acct Number";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 225);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Acct Type";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 311);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(194, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "Additional Withdrawal Amt";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(381, 105);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 20);
            this.label7.TabIndex = 12;
            this.label7.Text = "Optional Field";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(261, 225);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(219, 20);
            this.label8.TabIndex = 13;
            this.label8.Text = "\"C\" = Checking, \"S\" = Savings";
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(76, 360);
            this.OK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(150, 46);
            this.OK.TabIndex = 14;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(276, 360);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(150, 46);
            this.Cancel.TabIndex = 15;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // lblEft
            // 
            this.lblEft.AutoSize = true;
            this.lblEft.Location = new System.Drawing.Point(18, 265);
            this.lblEft.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEft.Name = "lblEft";
            this.lblEft.Size = new System.Drawing.Size(98, 20);
            this.lblEft.TabIndex = 31;
            this.lblEft.Text = "EFT Source:";
            // 
            // chkPaper
            // 
            this.chkPaper.AutoSize = true;
            this.chkPaper.Checked = true;
            this.chkPaper.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPaper.Enabled = false;
            this.chkPaper.Location = new System.Drawing.Point(222, 265);
            this.chkPaper.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkPaper.Name = "chkPaper";
            this.chkPaper.Size = new System.Drawing.Size(70, 24);
            this.chkPaper.TabIndex = 30;
            this.chkPaper.Text = "Paper";
            this.chkPaper.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 20);
            this.label9.TabIndex = 32;
            this.label9.Text = "Person Type";
            // 
            // textBox5
            // 
            this.textBox5.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.changeDataBindingSource, "PersonType", true));
            this.textBox5.Location = new System.Drawing.Point(222, 60);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(28, 26);
            this.textBox5.TabIndex = 33;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(261, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(226, 20);
            this.label10.TabIndex = 34;
            this.label10.Text = "\"B\" = Borrower, \"E\" = Endorser";
            // 
            // textBox1
            // 
            this.textBox1.AllowedSpecialCharacters = "";
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.changeDataBindingSource, "BorrowerSsn", true));
            this.textBox1.Location = new System.Drawing.Point(221, 20);
            this.textBox1.MaxLength = 9;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(149, 26);
            this.textBox1.Ssn = null;
            this.textBox1.TabIndex = 35;
            // 
            // ChangeInfoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 445);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblEft);
            this.Controls.Add(this.chkPaper);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.txtAccountType);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(520, 485);
            this.MinimumSize = new System.Drawing.Size(517, 484);
            this.Name = "ChangeInfoDialog";
            this.Text = "ACH Change Information";
            ((System.ComponentModel.ISupportInitialize)(this.changeDataBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.TextBox txtAccountType;
		private System.Windows.Forms.TextBox textBox6;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.BindingSource changeDataBindingSource;
		private System.Windows.Forms.Label lblEft;
        private System.Windows.Forms.CheckBox chkPaper;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label10;
        private Uheaa.Common.WinForms.SsnTextBox textBox1;
    }
}