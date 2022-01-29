namespace INCIDENTRP
{
	partial class BorrowerDataInvolvedOption
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox14 = new System.Windows.Forms.CheckBox();
            this.borrowerDataInvolvedBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbRegion = new System.Windows.Forms.ComboBox();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.txtRelationship = new System.Windows.Forms.TextBox();
            this.lblRelationship = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radYes = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox16 = new System.Windows.Forms.CheckBox();
            this.checkBox15 = new System.Windows.Forms.CheckBox();
            this.checkBox13 = new System.Windows.Forms.CheckBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.borrowerDataInvolvedBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.checkBox14);
            this.groupBox1.Controls.Add(this.cmbRegion);
            this.groupBox1.Controls.Add(this.cmbState);
            this.groupBox1.Controls.Add(this.txtRelationship);
            this.groupBox1.Controls.Add(this.lblRelationship);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radYes);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(3, -2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 234);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(284, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Region";
            // 
            // checkBox14
            // 
            this.checkBox14.AutoSize = true;
            this.checkBox14.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "BorrowerInformationIsVerified", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox14.Location = new System.Drawing.Point(10, 78);
            this.checkBox14.Name = "checkBox14";
            this.checkBox14.Size = new System.Drawing.Size(161, 17);
            this.checkBox14.TabIndex = 4;
            this.checkBox14.Text = "Verified Borrower Information";
            this.checkBox14.UseVisualStyleBackColor = true;
            // 
            // borrowerDataInvolvedBindingSource
            // 
            this.borrowerDataInvolvedBindingSource.DataSource = typeof(INCIDENTRP.BorrowerDataInvolved);
            // 
            // cmbRegion
            // 
            this.cmbRegion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbRegion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbRegion.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDataInvolvedBindingSource, "DataRegion", true));
            this.cmbRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegion.FormattingEnabled = true;
            this.cmbRegion.Location = new System.Drawing.Point(331, 51);
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.Size = new System.Drawing.Size(97, 21);
            this.cmbRegion.TabIndex = 3;
            // 
            // cmbState
            // 
            this.cmbState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDataInvolvedBindingSource, "State", true));
            this.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbState.FormattingEnabled = true;
            this.cmbState.Location = new System.Drawing.Point(174, 51);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(97, 21);
            this.cmbState.TabIndex = 2;
            // 
            // txtRelationship
            // 
            this.txtRelationship.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDataInvolvedBindingSource, "NotifierRelationshipToPiiOwner", true));
            this.txtRelationship.Location = new System.Drawing.Point(301, 98);
            this.txtRelationship.Name = "txtRelationship";
            this.txtRelationship.Size = new System.Drawing.Size(127, 20);
            this.txtRelationship.TabIndex = 7;
            this.txtRelationship.Visible = false;
            // 
            // lblRelationship
            // 
            this.lblRelationship.AutoSize = true;
            this.lblRelationship.Location = new System.Drawing.Point(301, 82);
            this.lblRelationship.Name = "lblRelationship";
            this.lblRelationship.Size = new System.Drawing.Size(122, 13);
            this.lblRelationship.TabIndex = 10;
            this.lblRelationship.Text = "Relationship to Borrower";
            this.lblRelationship.Visible = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(256, 99);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(39, 17);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "No";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radYes
            // 
            this.radYes.AutoSize = true;
            this.radYes.Location = new System.Drawing.Point(202, 99);
            this.radYes.Name = "radYes";
            this.radYes.Size = new System.Drawing.Size(43, 17);
            this.radYes.TabIndex = 5;
            this.radYes.Text = "Yes";
            this.radYes.UseVisualStyleBackColor = true;
            this.radYes.CheckedChanged += new System.EventHandler(this.radYes_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(179, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Does the Notifier Know the Borrower";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Borrower State of Residence";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Borrower Account Number";
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDataInvolvedBindingSource, "AccountNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox2.Location = new System.Drawing.Point(198, 30);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(230, 20);
            this.textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDataInvolvedBindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox1.Location = new System.Drawing.Point(198, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(230, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Borrower Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox16);
            this.groupBox2.Controls.Add(this.checkBox15);
            this.groupBox2.Controls.Add(this.checkBox13);
            this.groupBox2.Controls.Add(this.checkBox12);
            this.groupBox2.Controls.Add(this.checkBox10);
            this.groupBox2.Controls.Add(this.checkBox11);
            this.groupBox2.Controls.Add(this.checkBox9);
            this.groupBox2.Controls.Add(this.checkBox8);
            this.groupBox2.Controls.Add(this.checkBox7);
            this.groupBox2.Controls.Add(this.checkBox6);
            this.groupBox2.Controls.Add(this.checkBox5);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Location = new System.Drawing.Point(10, 124);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(418, 104);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Elements Released";
            // 
            // checkBox16
            // 
            this.checkBox16.AutoSize = true;
            this.checkBox16.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "CreditReportOrScoreWasReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox16.Location = new System.Drawing.Point(6, 51);
            this.checkBox16.Name = "checkBox16";
            this.checkBox16.Size = new System.Drawing.Size(126, 17);
            this.checkBox16.TabIndex = 2;
            this.checkBox16.Text = "Credit Report/Scores";
            this.checkBox16.UseVisualStyleBackColor = true;
            // 
            // checkBox15
            // 
            this.checkBox15.AutoSize = true;
            this.checkBox15.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "PromissoryNotesWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox15.Location = new System.Drawing.Point(281, 67);
            this.checkBox15.Name = "checkBox15";
            this.checkBox15.Size = new System.Drawing.Size(107, 17);
            this.checkBox15.TabIndex = 13;
            this.checkBox15.Text = "Promissory Notes";
            this.checkBox15.UseVisualStyleBackColor = true;
            // 
            // checkBox13
            // 
            this.checkBox13.AutoSize = true;
            this.checkBox13.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "PhoneNumberWasReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox13.Location = new System.Drawing.Point(281, 51);
            this.checkBox13.Name = "checkBox13";
            this.checkBox13.Size = new System.Drawing.Size(97, 17);
            this.checkBox13.TabIndex = 12;
            this.checkBox13.Text = "Phone Number";
            this.checkBox13.UseVisualStyleBackColor = true;
            // 
            // checkBox12
            // 
            this.checkBox12.AutoSize = true;
            this.checkBox12.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "LoanApplicationsWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox12.Location = new System.Drawing.Point(138, 51);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(110, 17);
            this.checkBox12.TabIndex = 7;
            this.checkBox12.Text = "Loan Applications";
            this.checkBox12.UseVisualStyleBackColor = true;
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "AddressWasReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox10.Location = new System.Drawing.Point(6, 19);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(64, 17);
            this.checkBox10.TabIndex = 0;
            this.checkBox10.Text = "Address";
            this.checkBox10.UseVisualStyleBackColor = true;
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "EmployeeIdNumberWasReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox11.Location = new System.Drawing.Point(6, 84);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(126, 17);
            this.checkBox11.TabIndex = 4;
            this.checkBox11.Text = "Employee ID Number";
            this.checkBox11.UseVisualStyleBackColor = true;
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "MedicalOrConditionalDisabilityWasReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox9.Location = new System.Drawing.Point(281, 19);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(127, 17);
            this.checkBox9.TabIndex = 10;
            this.checkBox9.Text = "Med./Cond. Disability";
            this.checkBox9.UseVisualStyleBackColor = true;
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "EmployerIdNumberWasReleased", true));
            this.checkBox8.Location = new System.Drawing.Point(138, 19);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(123, 17);
            this.checkBox8.TabIndex = 5;
            this.checkBox8.Text = "Employer ID Number";
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "DateOfBirthWasReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox7.Location = new System.Drawing.Point(6, 68);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(85, 17);
            this.checkBox7.TabIndex = 3;
            this.checkBox7.Text = "Date of Birth";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "PayoffAmountsWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox6.Location = new System.Drawing.Point(281, 35);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(100, 17);
            this.checkBox6.TabIndex = 11;
            this.checkBox6.Text = "Payoff Amounts";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "BankAccountNumbersWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox5.Location = new System.Drawing.Point(6, 35);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(121, 17);
            this.checkBox5.TabIndex = 1;
            this.checkBox5.Text = "Bank Acct Numbers";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "LoanPaymentHistoriesWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox4.Location = new System.Drawing.Point(138, 84);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(137, 17);
            this.checkBox4.TabIndex = 9;
            this.checkBox4.Text = "Loan Payment Histories";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "LoanAmountsOrBalancesWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox3.Location = new System.Drawing.Point(138, 35);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(125, 17);
            this.checkBox3.TabIndex = 6;
            this.checkBox3.Text = "Loan Amts/Balances";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "LoanIdsOrNumbersWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox2.Location = new System.Drawing.Point(138, 67);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(126, 17);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "Loan IDs or Numbers";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerDataInvolvedBindingSource, "SocialSecurityNumbersWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1.Location = new System.Drawing.Point(281, 84);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(53, 17);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "SSNs";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // BorrowerDataInvolvedOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "BorrowerDataInvolvedOption";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.borrowerDataInvolvedBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkBox11;
		private System.Windows.Forms.CheckBox checkBox9;
		private System.Windows.Forms.CheckBox checkBox8;
		private System.Windows.Forms.CheckBox checkBox7;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radYes;
		private System.Windows.Forms.Label lblRelationship;
		private System.Windows.Forms.TextBox txtRelationship;
		private System.Windows.Forms.ComboBox cmbState;
		private System.Windows.Forms.BindingSource borrowerDataInvolvedBindingSource;
		private System.Windows.Forms.CheckBox checkBox14;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cmbRegion;
		private System.Windows.Forms.CheckBox checkBox10;
		private System.Windows.Forms.CheckBox checkBox12;
		private System.Windows.Forms.CheckBox checkBox13;
		private System.Windows.Forms.CheckBox checkBox15;
		private System.Windows.Forms.CheckBox checkBox16;
	}
}
