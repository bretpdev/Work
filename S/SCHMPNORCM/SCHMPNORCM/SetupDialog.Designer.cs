namespace SCHMPNORCM
{
	partial class SetupDialog
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblRecordsInFile = new System.Windows.Forms.Label();
			this.txtSchoolCode = new System.Windows.Forms.TextBox();
			this.dtpEffectiveDate = new System.Windows.Forms.DateTimePicker();
			this.grpSchoolSetTo = new System.Windows.Forms.GroupBox();
			this.radNslds = new System.Windows.Forms.RadioButton();
			this.radClearinghouse = new System.Windows.Forms.RadioButton();
			this.radCommonline = new System.Windows.Forms.RadioButton();
			this.radSerialMpn = new System.Windows.Forms.RadioButton();
			this.grpRequiredOptions = new System.Windows.Forms.GroupBox();
			this.chkServiceBureau = new System.Windows.Forms.CheckBox();
			this.chkModificationResponse = new System.Windows.Forms.CheckBox();
			this.chkClChange = new System.Windows.Forms.CheckBox();
			this.chkHoldAllDisb = new System.Windows.Forms.CheckBox();
			this.chkClDisbursementRoster = new System.Windows.Forms.CheckBox();
			this.chkClApp = new System.Windows.Forms.CheckBox();
			this.radStffrd = new System.Windows.Forms.RadioButton();
			this.radPlus = new System.Windows.Forms.RadioButton();
			this.grpOptionalOptions = new System.Windows.Forms.GroupBox();
			this.chkElmres = new System.Windows.Forms.CheckBox();
			this.btnProductionUpdate = new System.Windows.Forms.Button();
			this.btnTestUpdate = new System.Windows.Forms.Button();
			this.btnSaveInfo = new System.Windows.Forms.Button();
			this.btnDeleteFile = new System.Windows.Forms.Button();
			this.btnGenerateEmail = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpSchoolSetTo.SuspendLayout();
			this.grpRequiredOptions.SuspendLayout();
			this.grpOptionalOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(376, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "This script sets a school up as Commonline or Serial MPN. Click Cancel to exit.";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 67);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "School Code";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(14, 94);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(75, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Effective Date";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(245, 323);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(77, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Records in file:";
			// 
			// lblRecordsInFile
			// 
			this.lblRecordsInFile.AutoSize = true;
			this.lblRecordsInFile.Location = new System.Drawing.Point(328, 323);
			this.lblRecordsInFile.Name = "lblRecordsInFile";
			this.lblRecordsInFile.Size = new System.Drawing.Size(13, 13);
			this.lblRecordsInFile.TabIndex = 4;
			this.lblRecordsInFile.Text = "0";
			// 
			// txtSchoolCode
			// 
			this.txtSchoolCode.Location = new System.Drawing.Point(103, 64);
			this.txtSchoolCode.Name = "txtSchoolCode";
			this.txtSchoolCode.Size = new System.Drawing.Size(100, 20);
			this.txtSchoolCode.TabIndex = 0;
			// 
			// dtpEffectiveDate
			// 
			this.dtpEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpEffectiveDate.Location = new System.Drawing.Point(103, 90);
			this.dtpEffectiveDate.Name = "dtpEffectiveDate";
			this.dtpEffectiveDate.Size = new System.Drawing.Size(100, 20);
			this.dtpEffectiveDate.TabIndex = 1;
			// 
			// grpSchoolSetTo
			// 
			this.grpSchoolSetTo.Controls.Add(this.radNslds);
			this.grpSchoolSetTo.Controls.Add(this.radClearinghouse);
			this.grpSchoolSetTo.Controls.Add(this.radCommonline);
			this.grpSchoolSetTo.Controls.Add(this.radSerialMpn);
			this.grpSchoolSetTo.Location = new System.Drawing.Point(16, 125);
			this.grpSchoolSetTo.Name = "grpSchoolSetTo";
			this.grpSchoolSetTo.Size = new System.Drawing.Size(554, 53);
			this.grpSchoolSetTo.TabIndex = 2;
			this.grpSchoolSetTo.TabStop = false;
			this.grpSchoolSetTo.Text = "School Set To";
			// 
			// radNslds
			// 
			this.radNslds.AutoSize = true;
			this.radNslds.Location = new System.Drawing.Point(348, 22);
			this.radNslds.Name = "radNslds";
			this.radNslds.Size = new System.Drawing.Size(61, 17);
			this.radNslds.TabIndex = 3;
			this.radNslds.TabStop = true;
			this.radNslds.Text = "NSLDS";
			this.radNslds.UseVisualStyleBackColor = true;
			// 
			// radClearinghouse
			// 
			this.radClearinghouse.AutoSize = true;
			this.radClearinghouse.Location = new System.Drawing.Point(226, 22);
			this.radClearinghouse.Name = "radClearinghouse";
			this.radClearinghouse.Size = new System.Drawing.Size(92, 17);
			this.radClearinghouse.TabIndex = 2;
			this.radClearinghouse.TabStop = true;
			this.radClearinghouse.Text = "Clearinghouse";
			this.radClearinghouse.UseVisualStyleBackColor = true;
			// 
			// radCommonline
			// 
			this.radCommonline.AutoSize = true;
			this.radCommonline.Location = new System.Drawing.Point(114, 22);
			this.radCommonline.Name = "radCommonline";
			this.radCommonline.Size = new System.Drawing.Size(82, 17);
			this.radCommonline.TabIndex = 1;
			this.radCommonline.TabStop = true;
			this.radCommonline.Text = "Commonline";
			this.radCommonline.UseVisualStyleBackColor = true;
			this.radCommonline.CheckedChanged += new System.EventHandler(this.radCommonline_CheckedChanged);
			// 
			// radSerialMpn
			// 
			this.radSerialMpn.AutoSize = true;
			this.radSerialMpn.Location = new System.Drawing.Point(6, 22);
			this.radSerialMpn.Name = "radSerialMpn";
			this.radSerialMpn.Size = new System.Drawing.Size(78, 17);
			this.radSerialMpn.TabIndex = 0;
			this.radSerialMpn.TabStop = true;
			this.radSerialMpn.Text = "Serial MPN";
			this.radSerialMpn.UseVisualStyleBackColor = true;
			this.radSerialMpn.CheckedChanged += new System.EventHandler(this.radSerialMpn_CheckedChanged);
			// 
			// grpRequiredOptions
			// 
			this.grpRequiredOptions.Controls.Add(this.chkServiceBureau);
			this.grpRequiredOptions.Controls.Add(this.chkModificationResponse);
			this.grpRequiredOptions.Controls.Add(this.chkClChange);
			this.grpRequiredOptions.Controls.Add(this.chkHoldAllDisb);
			this.grpRequiredOptions.Controls.Add(this.chkClDisbursementRoster);
			this.grpRequiredOptions.Controls.Add(this.chkClApp);
			this.grpRequiredOptions.Controls.Add(this.radStffrd);
			this.grpRequiredOptions.Controls.Add(this.radPlus);
			this.grpRequiredOptions.Location = new System.Drawing.Point(16, 184);
			this.grpRequiredOptions.Name = "grpRequiredOptions";
			this.grpRequiredOptions.Size = new System.Drawing.Size(554, 75);
			this.grpRequiredOptions.TabIndex = 3;
			this.grpRequiredOptions.TabStop = false;
			this.grpRequiredOptions.Text = "Required Options";
			// 
			// chkServiceBureau
			// 
			this.chkServiceBureau.AutoSize = true;
			this.chkServiceBureau.Location = new System.Drawing.Point(382, 46);
			this.chkServiceBureau.Name = "chkServiceBureau";
			this.chkServiceBureau.Size = new System.Drawing.Size(152, 17);
			this.chkServiceBureau.TabIndex = 7;
			this.chkServiceBureau.Text = "Service Bureau Participant";
			this.chkServiceBureau.UseVisualStyleBackColor = true;
			this.chkServiceBureau.CheckedChanged += new System.EventHandler(this.chkServiceBureau_CheckedChanged);
			// 
			// chkModificationResponse
			// 
			this.chkModificationResponse.AutoSize = true;
			this.chkModificationResponse.Location = new System.Drawing.Point(212, 45);
			this.chkModificationResponse.Name = "chkModificationResponse";
			this.chkModificationResponse.Size = new System.Drawing.Size(134, 17);
			this.chkModificationResponse.TabIndex = 6;
			this.chkModificationResponse.Text = "Modification Response";
			this.chkModificationResponse.UseVisualStyleBackColor = true;
			// 
			// chkClChange
			// 
			this.chkClChange.AutoSize = true;
			this.chkClChange.Location = new System.Drawing.Point(103, 45);
			this.chkClChange.Name = "chkClChange";
			this.chkClChange.Size = new System.Drawing.Size(79, 17);
			this.chkClChange.TabIndex = 5;
			this.chkClChange.Text = "CL Change";
			this.chkClChange.UseVisualStyleBackColor = true;
			// 
			// chkHoldAllDisb
			// 
			this.chkHoldAllDisb.AutoSize = true;
			this.chkHoldAllDisb.Location = new System.Drawing.Point(382, 23);
			this.chkHoldAllDisb.Name = "chkHoldAllDisb";
			this.chkHoldAllDisb.Size = new System.Drawing.Size(86, 17);
			this.chkHoldAllDisb.TabIndex = 3;
			this.chkHoldAllDisb.Text = "Hold All Disb";
			this.chkHoldAllDisb.UseVisualStyleBackColor = true;
			// 
			// chkClDisbursementRoster
			// 
			this.chkClDisbursementRoster.AutoSize = true;
			this.chkClDisbursementRoster.Location = new System.Drawing.Point(212, 23);
			this.chkClDisbursementRoster.Name = "chkClDisbursementRoster";
			this.chkClDisbursementRoster.Size = new System.Drawing.Size(140, 17);
			this.chkClDisbursementRoster.TabIndex = 2;
			this.chkClDisbursementRoster.Text = "CL Disbursement Roster";
			this.chkClDisbursementRoster.UseVisualStyleBackColor = true;
			// 
			// chkClApp
			// 
			this.chkClApp.AutoSize = true;
			this.chkClApp.Location = new System.Drawing.Point(103, 23);
			this.chkClApp.Name = "chkClApp";
			this.chkClApp.Size = new System.Drawing.Size(61, 17);
			this.chkClApp.TabIndex = 1;
			this.chkClApp.Text = "CL App";
			this.chkClApp.UseVisualStyleBackColor = true;
			// 
			// radStffrd
			// 
			this.radStffrd.AutoSize = true;
			this.radStffrd.Location = new System.Drawing.Point(6, 22);
			this.radStffrd.Name = "radStffrd";
			this.radStffrd.Size = new System.Drawing.Size(67, 17);
			this.radStffrd.TabIndex = 0;
			this.radStffrd.TabStop = true;
			this.radStffrd.Text = "STFFRD";
			this.radStffrd.UseVisualStyleBackColor = true;
			// 
			// radPlus
			// 
			this.radPlus.AutoSize = true;
			this.radPlus.Location = new System.Drawing.Point(6, 45);
			this.radPlus.Name = "radPlus";
			this.radPlus.Size = new System.Drawing.Size(53, 17);
			this.radPlus.TabIndex = 4;
			this.radPlus.TabStop = true;
			this.radPlus.Text = "PLUS";
			this.radPlus.UseVisualStyleBackColor = true;
			// 
			// grpOptionalOptions
			// 
			this.grpOptionalOptions.Controls.Add(this.chkElmres);
			this.grpOptionalOptions.Location = new System.Drawing.Point(16, 265);
			this.grpOptionalOptions.Name = "grpOptionalOptions";
			this.grpOptionalOptions.Size = new System.Drawing.Size(554, 46);
			this.grpOptionalOptions.TabIndex = 4;
			this.grpOptionalOptions.TabStop = false;
			this.grpOptionalOptions.Text = "Optional Options";
			// 
			// chkElmres
			// 
			this.chkElmres.AutoSize = true;
			this.chkElmres.Location = new System.Drawing.Point(242, 19);
			this.chkElmres.Name = "chkElmres";
			this.chkElmres.Size = new System.Drawing.Size(70, 17);
			this.chkElmres.TabIndex = 0;
			this.chkElmres.Text = "ELMRES";
			this.chkElmres.UseVisualStyleBackColor = true;
			// 
			// btnProductionUpdate
			// 
			this.btnProductionUpdate.Location = new System.Drawing.Point(41, 353);
			this.btnProductionUpdate.Name = "btnProductionUpdate";
			this.btnProductionUpdate.Size = new System.Drawing.Size(158, 30);
			this.btnProductionUpdate.TabIndex = 5;
			this.btnProductionUpdate.Text = "Production Region Update";
			this.btnProductionUpdate.UseVisualStyleBackColor = true;
			this.btnProductionUpdate.Click += new System.EventHandler(this.btnProductionUpdate_Click);
			// 
			// btnTestUpdate
			// 
			this.btnTestUpdate.Location = new System.Drawing.Point(41, 398);
			this.btnTestUpdate.Name = "btnTestUpdate";
			this.btnTestUpdate.Size = new System.Drawing.Size(158, 30);
			this.btnTestUpdate.TabIndex = 6;
			this.btnTestUpdate.Text = "Test Region Update";
			this.btnTestUpdate.UseVisualStyleBackColor = true;
			this.btnTestUpdate.Click += new System.EventHandler(this.btnTestUpdate_Click);
			// 
			// btnSaveInfo
			// 
			this.btnSaveInfo.Location = new System.Drawing.Point(214, 353);
			this.btnSaveInfo.Name = "btnSaveInfo";
			this.btnSaveInfo.Size = new System.Drawing.Size(158, 30);
			this.btnSaveInfo.TabIndex = 7;
			this.btnSaveInfo.Text = "Save Information";
			this.btnSaveInfo.UseVisualStyleBackColor = true;
			this.btnSaveInfo.Click += new System.EventHandler(this.btnSaveInfo_Click);
			// 
			// btnDeleteFile
			// 
			this.btnDeleteFile.Location = new System.Drawing.Point(214, 398);
			this.btnDeleteFile.Name = "btnDeleteFile";
			this.btnDeleteFile.Size = new System.Drawing.Size(158, 30);
			this.btnDeleteFile.TabIndex = 8;
			this.btnDeleteFile.Text = "Delete Text File";
			this.btnDeleteFile.UseVisualStyleBackColor = true;
			this.btnDeleteFile.Click += new System.EventHandler(this.btnDeleteFile_Click);
			// 
			// btnGenerateEmail
			// 
			this.btnGenerateEmail.Location = new System.Drawing.Point(387, 353);
			this.btnGenerateEmail.Name = "btnGenerateEmail";
			this.btnGenerateEmail.Size = new System.Drawing.Size(158, 30);
			this.btnGenerateEmail.TabIndex = 9;
			this.btnGenerateEmail.Text = "Generate Email Loan Delivery";
			this.btnGenerateEmail.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(387, 398);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(158, 30);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// SetupDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(586, 443);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnGenerateEmail);
			this.Controls.Add(this.btnDeleteFile);
			this.Controls.Add(this.btnSaveInfo);
			this.Controls.Add(this.btnTestUpdate);
			this.Controls.Add(this.btnProductionUpdate);
			this.Controls.Add(this.grpRequiredOptions);
			this.Controls.Add(this.grpOptionalOptions);
			this.Controls.Add(this.grpSchoolSetTo);
			this.Controls.Add(this.dtpEffectiveDate);
			this.Controls.Add(this.txtSchoolCode);
			this.Controls.Add(this.lblRecordsInFile);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "SetupDialog";
			this.Text = "School MPN as Serial/Commonline";
			this.grpSchoolSetTo.ResumeLayout(false);
			this.grpSchoolSetTo.PerformLayout();
			this.grpRequiredOptions.ResumeLayout(false);
			this.grpRequiredOptions.PerformLayout();
			this.grpOptionalOptions.ResumeLayout(false);
			this.grpOptionalOptions.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblRecordsInFile;
		private System.Windows.Forms.TextBox txtSchoolCode;
		private System.Windows.Forms.DateTimePicker dtpEffectiveDate;
		private System.Windows.Forms.GroupBox grpSchoolSetTo;
		private System.Windows.Forms.GroupBox grpRequiredOptions;
		private System.Windows.Forms.GroupBox grpOptionalOptions;
		private System.Windows.Forms.RadioButton radNslds;
		private System.Windows.Forms.RadioButton radClearinghouse;
		private System.Windows.Forms.RadioButton radCommonline;
		private System.Windows.Forms.RadioButton radSerialMpn;
		private System.Windows.Forms.CheckBox chkServiceBureau;
		private System.Windows.Forms.CheckBox chkModificationResponse;
		private System.Windows.Forms.CheckBox chkClChange;
		private System.Windows.Forms.CheckBox chkHoldAllDisb;
		private System.Windows.Forms.CheckBox chkClDisbursementRoster;
		private System.Windows.Forms.CheckBox chkClApp;
		private System.Windows.Forms.RadioButton radStffrd;
		private System.Windows.Forms.RadioButton radPlus;
		private System.Windows.Forms.CheckBox chkElmres;
		private System.Windows.Forms.Button btnProductionUpdate;
		private System.Windows.Forms.Button btnTestUpdate;
		private System.Windows.Forms.Button btnSaveInfo;
		private System.Windows.Forms.Button btnDeleteFile;
		private System.Windows.Forms.Button btnGenerateEmail;
		private System.Windows.Forms.Button btnCancel;
	}
}