namespace INCIDENTRP
{
	partial class ThirdPartyDataInvolvedOption
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
            this.cmbRegion = new System.Windows.Forms.ComboBox();
            this.thirdPartyDataInvolvedBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtRelationship = new System.Windows.Forms.TextBox();
            this.lblRelationship = new System.Windows.Forms.Label();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radYes = new System.Windows.Forms.RadioButton();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thirdPartyDataInvolvedBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbRegion);
            this.groupBox1.Controls.Add(this.txtRelationship);
            this.groupBox1.Controls.Add(this.lblRelationship);
            this.groupBox1.Controls.Add(this.cmbState);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radYes);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
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
            this.label5.Location = new System.Drawing.Point(7, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Region";
            // 
            // cmbRegion
            // 
            this.cmbRegion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbRegion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbRegion.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.thirdPartyDataInvolvedBindingSource, "DataRegion", true));
            this.cmbRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegion.FormattingEnabled = true;
            this.cmbRegion.Location = new System.Drawing.Point(198, 75);
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.Size = new System.Drawing.Size(97, 21);
            this.cmbRegion.TabIndex = 3;
            // 
            // thirdPartyDataInvolvedBindingSource
            // 
            this.thirdPartyDataInvolvedBindingSource.DataSource = typeof(INCIDENTRP.ThirdPartyDataInvolved);
            // 
            // txtRelationship
            // 
            this.txtRelationship.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.thirdPartyDataInvolvedBindingSource, "NotifierRelationshipToPiiOwner", true));
            this.txtRelationship.Location = new System.Drawing.Point(304, 113);
            this.txtRelationship.Name = "txtRelationship";
            this.txtRelationship.Size = new System.Drawing.Size(124, 20);
            this.txtRelationship.TabIndex = 6;
            this.txtRelationship.Visible = false;
            // 
            // lblRelationship
            // 
            this.lblRelationship.AutoSize = true;
            this.lblRelationship.Location = new System.Drawing.Point(301, 97);
            this.lblRelationship.Name = "lblRelationship";
            this.lblRelationship.Size = new System.Drawing.Size(122, 13);
            this.lblRelationship.TabIndex = 10;
            this.lblRelationship.Text = "Relationship to 3rd Party";
            this.lblRelationship.Visible = false;
            // 
            // cmbState
            // 
            this.cmbState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.thirdPartyDataInvolvedBindingSource, "State", true));
            this.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbState.FormattingEnabled = true;
            this.cmbState.Location = new System.Drawing.Point(198, 51);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(97, 21);
            this.cmbState.TabIndex = 2;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(256, 103);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(39, 17);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "No";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radYes
            // 
            this.radYes.AutoSize = true;
            this.radYes.Location = new System.Drawing.Point(207, 103);
            this.radYes.Name = "radYes";
            this.radYes.Size = new System.Drawing.Size(43, 17);
            this.radYes.TabIndex = 4;
            this.radYes.Text = "Yes";
            this.radYes.UseVisualStyleBackColor = true;
            this.radYes.CheckedChanged += new System.EventHandler(this.radYes_CheckedChanged);
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.thirdPartyDataInvolvedBindingSource, "AccountNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox2.Location = new System.Drawing.Point(198, 30);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(230, 20);
            this.textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.thirdPartyDataInvolvedBindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox1.Location = new System.Drawing.Point(198, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(230, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(188, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Does the Notifier Know the Third Party";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Third Party State of Residence";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Third Party Account Number";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Third Party Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox8);
            this.groupBox2.Controls.Add(this.checkBox7);
            this.groupBox2.Controls.Add(this.checkBox6);
            this.groupBox2.Controls.Add(this.checkBox5);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Location = new System.Drawing.Point(10, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(418, 89);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Elements Released";
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.thirdPartyDataInvolvedBindingSource, "MedicalOrConditionalDisabilityWasReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox8.Location = new System.Drawing.Point(164, 69);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(164, 17);
            this.checkBox8.TabIndex = 7;
            this.checkBox8.Text = "Medical/Conditional Disability";
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.thirdPartyDataInvolvedBindingSource, "DateOfBirthWasReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox7.Location = new System.Drawing.Point(164, 53);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(85, 17);
            this.checkBox7.TabIndex = 6;
            this.checkBox7.Text = "Date of Birth";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.thirdPartyDataInvolvedBindingSource, "PayoffAmountsWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox6.Location = new System.Drawing.Point(164, 19);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(100, 17);
            this.checkBox6.TabIndex = 4;
            this.checkBox6.Text = "Payoff Amounts";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.thirdPartyDataInvolvedBindingSource, "BankAccountNumbersWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox5.Location = new System.Drawing.Point(164, 36);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(139, 17);
            this.checkBox5.TabIndex = 5;
            this.checkBox5.Text = "Bank Account Numbers";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.thirdPartyDataInvolvedBindingSource, "LoanPaymentHistoriesWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox4.Location = new System.Drawing.Point(6, 68);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(137, 17);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "Loan Payment Histories";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.thirdPartyDataInvolvedBindingSource, "LoanAmountsOrBalancesWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox3.Location = new System.Drawing.Point(6, 52);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(143, 17);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "Loan Amounts/Balances";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.thirdPartyDataInvolvedBindingSource, "LoanIdsOrNumbersWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox2.Location = new System.Drawing.Point(6, 36);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(126, 17);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Loan IDs or Numbers";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.thirdPartyDataInvolvedBindingSource, "SocialSecurityNumbersWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1.Location = new System.Drawing.Point(6, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(141, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Social Security Numbers";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // ThirdPartyDataInvolvedOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ThirdPartyDataInvolvedOption";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thirdPartyDataInvolvedBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.CheckBox checkBox8;
		private System.Windows.Forms.CheckBox checkBox7;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radYes;
		private System.Windows.Forms.Label lblRelationship;
		private System.Windows.Forms.ComboBox cmbState;
		private System.Windows.Forms.TextBox txtRelationship;
		private System.Windows.Forms.ComboBox cmbRegion;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.BindingSource thirdPartyDataInvolvedBindingSource;
	}
}
