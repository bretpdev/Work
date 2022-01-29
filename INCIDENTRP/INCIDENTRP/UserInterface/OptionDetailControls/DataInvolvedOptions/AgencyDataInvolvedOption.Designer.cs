namespace INCIDENTRP
{
	partial class AgencyDataInvolvedOption
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
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.agencyDataInvolvedBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.txtOther = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chkOther = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox7 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.agencyDataInvolvedBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Location = new System.Drawing.Point(3, -2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(437, 239);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkBox5);
			this.groupBox2.Controls.Add(this.txtOther);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.chkOther);
			this.groupBox2.Controls.Add(this.checkBox1);
			this.groupBox2.Controls.Add(this.checkBox7);
			this.groupBox2.Controls.Add(this.checkBox2);
			this.groupBox2.Controls.Add(this.checkBox6);
			this.groupBox2.Controls.Add(this.checkBox3);
			this.groupBox2.Controls.Add(this.checkBox4);
			this.groupBox2.Location = new System.Drawing.Point(6, 19);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(425, 114);
			this.groupBox2.TabIndex = 12;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Data Elements Released";
			// 
			// checkBox5
			// 
			this.checkBox5.AutoSize = true;
			this.checkBox5.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.agencyDataInvolvedBindingSource, "OperationsReportsWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkBox5.Location = new System.Drawing.Point(208, 19);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(117, 17);
			this.checkBox5.TabIndex = 4;
			this.checkBox5.Text = "Operations Reports";
			this.checkBox5.UseVisualStyleBackColor = true;
			// 
			// agencyDataInvolvedBindingSource
			// 
			this.agencyDataInvolvedBindingSource.DataSource = typeof(INCIDENTRP.AgencyDataInvolved);
			// 
			// txtOther
			// 
			this.txtOther.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.agencyDataInvolvedBindingSource, "OtherInformation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtOther.Enabled = false;
			this.txtOther.Location = new System.Drawing.Point(281, 85);
			this.txtOther.Name = "txtOther";
			this.txtOther.Size = new System.Drawing.Size(135, 20);
			this.txtOther.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(226, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(49, 13);
			this.label2.TabIndex = 11;
			this.label2.Text = "Describe";
			// 
			// chkOther
			// 
			this.chkOther.AutoSize = true;
			this.chkOther.Location = new System.Drawing.Point(208, 68);
			this.chkOther.Name = "chkOther";
			this.chkOther.Size = new System.Drawing.Size(52, 17);
			this.chkOther.TabIndex = 7;
			this.chkOther.Text = "Other";
			this.chkOther.UseVisualStyleBackColor = true;
			this.chkOther.CheckedChanged += new System.EventHandler(this.chkOther_CheckedChanged);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.agencyDataInvolvedBindingSource, "AccountingOrAdministrativeRecordsWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkBox1.Location = new System.Drawing.Point(6, 19);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(193, 17);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Accounting/Administrative Records";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// checkBox7
			// 
			this.checkBox7.AutoSize = true;
			this.checkBox7.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.agencyDataInvolvedBindingSource, "UespParticipantRecordsWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkBox7.Location = new System.Drawing.Point(208, 52);
			this.checkBox7.Name = "checkBox7";
			this.checkBox7.Size = new System.Drawing.Size(151, 17);
			this.checkBox7.TabIndex = 6;
			this.checkBox7.Text = "UESP Participant Records";
			this.checkBox7.UseVisualStyleBackColor = true;
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.agencyDataInvolvedBindingSource, "ClosedSchoolRecordsWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkBox2.Location = new System.Drawing.Point(6, 36);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(137, 17);
			this.checkBox2.TabIndex = 1;
			this.checkBox2.Text = "Closed School Records";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// checkBox6
			// 
			this.checkBox6.AutoSize = true;
			this.checkBox6.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.agencyDataInvolvedBindingSource, "ProposalAndLoanPurchaseRequestsWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkBox6.Location = new System.Drawing.Point(208, 36);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(211, 17);
			this.checkBox6.TabIndex = 5;
			this.checkBox6.Text = "Proposal and Loan Purchase Requests";
			this.checkBox6.UseVisualStyleBackColor = true;
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.agencyDataInvolvedBindingSource, "ConfidentialCaseFilesWereReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkBox3.Location = new System.Drawing.Point(6, 52);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(132, 17);
			this.checkBox3.TabIndex = 2;
			this.checkBox3.Text = "Confidential Case Files";
			this.checkBox3.UseVisualStyleBackColor = true;
			// 
			// checkBox4
			// 
			this.checkBox4.AutoSize = true;
			this.checkBox4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.agencyDataInvolvedBindingSource, "ContractInformationWasReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkBox4.Location = new System.Drawing.Point(6, 68);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(121, 17);
			this.checkBox4.TabIndex = 3;
			this.checkBox4.Text = "Contract Information";
			this.checkBox4.UseVisualStyleBackColor = true;
			// 
			// AgencyDataInvolvedOption
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "AgencyDataInvolvedOption";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.agencyDataInvolvedBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtOther;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkOther;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox7;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.BindingSource agencyDataInvolvedBindingSource;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.CheckBox checkBox5;
	}
}
