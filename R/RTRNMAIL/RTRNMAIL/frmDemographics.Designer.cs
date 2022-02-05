namespace RTRNMAIL
{
	partial class frmDemographics
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
			this.btnDaRules = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnContinue = new System.Windows.Forms.Button();
			this.ForwardingAddressBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.lblName = new System.Windows.Forms.Label();
			this.lblAcctNo = new System.Windows.Forms.Label();
			this.lblSsn = new System.Windows.Forms.Label();
			this.cmbState = new System.Windows.Forms.ComboBox();
			this.txtAddress1 = new System.Windows.Forms.TextBox();
			this.txtCity = new System.Windows.Forms.TextBox();
			this.txtAddress2 = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtZip = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.txtCountry = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.ForwardingAddressBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// btnDaRules
			// 
			this.btnDaRules.Location = new System.Drawing.Point(226, 221);
			this.btnDaRules.Name = "btnDaRules";
			this.btnDaRules.Size = new System.Drawing.Size(75, 29);
			this.btnDaRules.TabIndex = 8;
			this.btnDaRules.Text = "Da\' Rules";
			this.btnDaRules.UseVisualStyleBackColor = true;
			this.btnDaRules.Click += new System.EventHandler(this.btnDaRules_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(120, 221);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 29);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnContinue
			// 
			this.btnContinue.Location = new System.Drawing.Point(15, 221);
			this.btnContinue.Name = "btnContinue";
			this.btnContinue.Size = new System.Drawing.Size(75, 29);
			this.btnContinue.TabIndex = 6;
			this.btnContinue.Text = "Continue";
			this.btnContinue.UseVisualStyleBackColor = true;
			this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
			// 
			// ForwardingAddressBindingSource
			// 
			this.ForwardingAddressBindingSource.DataSource = typeof(RTRNMAIL.MailingAddress);
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(93, 53);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(45, 13);
			this.lblName.TabIndex = 20;
			this.lblName.Text = "lblName";
			// 
			// lblAcctNo
			// 
			this.lblAcctNo.AutoSize = true;
			this.lblAcctNo.Location = new System.Drawing.Point(93, 31);
			this.lblAcctNo.Name = "lblAcctNo";
			this.lblAcctNo.Size = new System.Drawing.Size(53, 13);
			this.lblAcctNo.TabIndex = 19;
			this.lblAcctNo.Text = "lblAcctNo";
			// 
			// lblSsn
			// 
			this.lblSsn.AutoSize = true;
			this.lblSsn.Location = new System.Drawing.Point(93, 9);
			this.lblSsn.Name = "lblSsn";
			this.lblSsn.Size = new System.Drawing.Size(35, 13);
			this.lblSsn.TabIndex = 18;
			this.lblSsn.Text = "lblSsn";
			// 
			// cmbState
			// 
			this.cmbState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ForwardingAddressBindingSource, "State", true));
			this.cmbState.FormattingEnabled = true;
			this.cmbState.Location = new System.Drawing.Point(96, 138);
			this.cmbState.Name = "cmbState";
			this.cmbState.Size = new System.Drawing.Size(70, 21);
			this.cmbState.TabIndex = 3;
			// 
			// txtAddress1
			// 
			this.txtAddress1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ForwardingAddressBindingSource, "Address1", true));
			this.txtAddress1.Location = new System.Drawing.Point(96, 72);
			this.txtAddress1.Name = "txtAddress1";
			this.txtAddress1.Size = new System.Drawing.Size(205, 20);
			this.txtAddress1.TabIndex = 0;
			// 
			// txtCity
			// 
			this.txtCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ForwardingAddressBindingSource, "City", true));
			this.txtCity.Location = new System.Drawing.Point(96, 116);
			this.txtCity.Name = "txtCity";
			this.txtCity.Size = new System.Drawing.Size(205, 20);
			this.txtCity.TabIndex = 2;
			// 
			// txtAddress2
			// 
			this.txtAddress2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ForwardingAddressBindingSource, "Address2", true));
			this.txtAddress2.Location = new System.Drawing.Point(96, 94);
			this.txtAddress2.Name = "txtAddress2";
			this.txtAddress2.Size = new System.Drawing.Size(205, 20);
			this.txtAddress2.TabIndex = 1;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(12, 163);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(22, 13);
			this.label9.TabIndex = 12;
			this.label9.Text = "Zip";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(12, 141);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(32, 13);
			this.label8.TabIndex = 11;
			this.label8.Text = "State";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 119);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(24, 13);
			this.label7.TabIndex = 10;
			this.label7.Text = "City";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 97);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(54, 13);
			this.label6.TabIndex = 9;
			this.label6.Text = "Address 2";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 75);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(54, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Address 1";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 53);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Name";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 31);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Account #";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(29, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "SSN";
			// 
			// txtZip
			// 
			this.txtZip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ForwardingAddressBindingSource, "Zip", true));
			this.txtZip.Location = new System.Drawing.Point(96, 160);
			this.txtZip.Name = "txtZip";
			this.txtZip.Size = new System.Drawing.Size(70, 20);
			this.txtZip.TabIndex = 4;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(12, 185);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(43, 13);
			this.label10.TabIndex = 25;
			this.label10.Text = "Country";
			// 
			// txtCountry
			// 
			this.txtCountry.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ForwardingAddressBindingSource, "Country", true));
			this.txtCountry.Location = new System.Drawing.Point(96, 186);
			this.txtCountry.Name = "txtCountry";
			this.txtCountry.Size = new System.Drawing.Size(205, 20);
			this.txtCountry.TabIndex = 5;
			// 
			// frmDemographics
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(315, 263);
			this.Controls.Add(this.txtCountry);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.btnDaRules);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnContinue);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.lblAcctNo);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lblSsn);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.cmbState);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.txtZip);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.txtAddress1);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.txtCity);
			this.Controls.Add(this.txtAddress2);
			this.Name = "frmDemographics";
			this.Text = "Forwarding Address";
			((System.ComponentModel.ISupportInitialize)(this.ForwardingAddressBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtAddress1;
		private System.Windows.Forms.TextBox txtCity;
		private System.Windows.Forms.TextBox txtAddress2;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbState;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblAcctNo;
		private System.Windows.Forms.Label lblSsn;
		private System.Windows.Forms.Button btnContinue;
		private System.Windows.Forms.Button btnDaRules;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.BindingSource ForwardingAddressBindingSource;
		private System.Windows.Forms.TextBox txtZip;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtCountry;
	}
}