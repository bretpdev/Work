namespace ACDCFlows
{
	partial class ExistingFlowSteps
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
            this.cboStaffAssignID = new System.Windows.Forms.ComboBox();
            this.flowStepBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboStaffAssignment = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.cboAccessKey = new System.Windows.Forms.ComboBox();
            this.cboDataValidation = new System.Windows.Forms.ComboBox();
            this.cboNotificationType = new System.Windows.Forms.ComboBox();
            this.cboBusinessUnit = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblCounter = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.txtDisplayText = new System.Windows.Forms.TextBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.flowStepBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // cboStaffAssignID
            // 
            this.cboStaffAssignID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "StaffAssignmentCalculationID", true));
            this.cboStaffAssignID.DataSource = this.flowStepBindingSource;
            this.cboStaffAssignID.DisplayMember = "StaffAssignmentCalculationID";
            this.cboStaffAssignID.FormattingEnabled = true;
            this.cboStaffAssignID.Location = new System.Drawing.Point(719, 32);
            this.cboStaffAssignID.Name = "cboStaffAssignID";
            this.cboStaffAssignID.Size = new System.Drawing.Size(241, 21);
            this.cboStaffAssignID.TabIndex = 33;
            this.cboStaffAssignID.SelectedIndexChanged += new System.EventHandler(this.cboStaffAssignID_SelectedIndexChanged);
            // 
            // flowStepBindingSource
            // 
            this.flowStepBindingSource.DataSource = typeof(ACDCFlows.FlowStep);
            // 
            // cboStaffAssignment
            // 
            this.cboStaffAssignment.FormattingEnabled = true;
            this.cboStaffAssignment.Location = new System.Drawing.Point(719, 5);
            this.cboStaffAssignment.Name = "cboStaffAssignment";
            this.cboStaffAssignment.Size = new System.Drawing.Size(241, 21);
            this.cboStaffAssignment.TabIndex = 32;
            this.cboStaffAssignment.SelectedIndexChanged += new System.EventHandler(this.cboStaffAssignment_SelectedIndexChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "Description", true));
            this.txtDescription.Location = new System.Drawing.Point(154, 86);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(806, 35);
            this.txtDescription.TabIndex = 30;
            // 
            // cboAccessKey
            // 
            this.cboAccessKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "AccessKey", true));
            this.cboAccessKey.DataSource = this.flowStepBindingSource;
            this.cboAccessKey.DisplayMember = "AccessKey";
            this.cboAccessKey.FormattingEnabled = true;
            this.cboAccessKey.Location = new System.Drawing.Point(424, 32);
            this.cboAccessKey.Name = "cboAccessKey";
            this.cboAccessKey.Size = new System.Drawing.Size(165, 21);
            this.cboAccessKey.TabIndex = 29;
            // 
            // cboDataValidation
            // 
            this.cboDataValidation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "DataValidationID", true));
            this.cboDataValidation.DataSource = this.flowStepBindingSource;
            this.cboDataValidation.DisplayMember = "DataValidationID";
            this.cboDataValidation.FormattingEnabled = true;
            this.cboDataValidation.Location = new System.Drawing.Point(424, 5);
            this.cboDataValidation.Name = "cboDataValidation";
            this.cboDataValidation.Size = new System.Drawing.Size(95, 21);
            this.cboDataValidation.TabIndex = 28;
            // 
            // cboNotificationType
            // 
            this.cboNotificationType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "NotificationType", true));
            this.cboNotificationType.DataSource = this.flowStepBindingSource;
            this.cboNotificationType.DisplayMember = "NotificationType";
            this.cboNotificationType.FormattingEnabled = true;
            this.cboNotificationType.Location = new System.Drawing.Point(154, 32);
            this.cboNotificationType.Name = "cboNotificationType";
            this.cboNotificationType.Size = new System.Drawing.Size(144, 21);
            this.cboNotificationType.TabIndex = 27;
            // 
            // cboBusinessUnit
            // 
            this.cboBusinessUnit.FormattingEnabled = true;
            this.cboBusinessUnit.Items.AddRange(new object[] {
            "",
            "True",
            "False"});
            this.cboBusinessUnit.Location = new System.Drawing.Point(154, 5);
            this.cboBusinessUnit.Name = "cboBusinessUnit";
            this.cboBusinessUnit.Size = new System.Drawing.Size(102, 21);
            this.cboBusinessUnit.TabIndex = 26;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(607, 35);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(103, 13);
            this.label22.TabIndex = 25;
            this.label22.Text = "Staff Assignment ID:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(607, 8);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(89, 13);
            this.label21.TabIndex = 24;
            this.label21.Text = "Staff Assignment:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(322, 35);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(66, 13);
            this.label20.TabIndex = 23;
            this.label20.Text = "Access Key:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(322, 63);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(40, 13);
            this.label19.TabIndex = 22;
            this.label19.Text = "Status:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 63);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(104, 13);
            this.label18.TabIndex = 21;
            this.label18.Text = "Control Display Text:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(322, 8);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(96, 13);
            this.label17.TabIndex = 20;
            this.label17.Text = "Data Validation ID:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 89);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(63, 13);
            this.label16.TabIndex = 19;
            this.label16.Text = "Description:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 35);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(90, 13);
            this.label15.TabIndex = 18;
            this.label15.Text = "Notification Type:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(136, 13);
            this.label13.TabIndex = 17;
            this.label13.Text = "Business Unit Access Only:";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(29, 131);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 34;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnUpdate.ForeColor = System.Drawing.Color.Black;
            this.btnUpdate.Location = new System.Drawing.Point(122, 131);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 35;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblCounter
            // 
            this.lblCounter.AutoSize = true;
            this.lblCounter.BackColor = System.Drawing.Color.White;
            this.lblCounter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCounter.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "FlowStepSequenceNumber", true));
            this.lblCounter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCounter.ForeColor = System.Drawing.Color.Black;
            this.lblCounter.Location = new System.Drawing.Point(833, 136);
            this.lblCounter.MaximumSize = new System.Drawing.Size(40, 25);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(2, 22);
            this.lblCounter.TabIndex = 36;
            // 
            // cboStatus
            // 
            this.cboStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "Status", true));
            this.cboStatus.DataSource = this.flowStepBindingSource;
            this.cboStatus.DisplayMember = "Status";
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(424, 60);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(165, 21);
            this.cboStatus.TabIndex = 39;
            // 
            // txtDisplayText
            // 
            this.txtDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "ControlDisplayText", true));
            this.txtDisplayText.Location = new System.Drawing.Point(154, 59);
            this.txtDisplayText.Name = "txtDisplayText";
            this.txtDisplayText.Size = new System.Drawing.Size(142, 20);
            this.txtDisplayText.TabIndex = 40;
            // 
            // btnDown
            // 
            this.btnDown.BackgroundImage = global::ACDCFlows.Properties.Resources.DownArrow;
            this.btnDown.FlatAppearance.BorderSize = 0;
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.btnDown.Location = new System.Drawing.Point(925, 126);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(35, 35);
            this.btnDown.TabIndex = 38;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(10)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.btnUp.BackgroundImage = global::ACDCFlows.Properties.Resources.UpArrow;
            this.btnUp.FlatAppearance.BorderSize = 0;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.btnUp.Location = new System.Drawing.Point(884, 126);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(35, 35);
            this.btnUp.TabIndex = 37;
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // ExistingFlowSteps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.txtDisplayText);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.lblCounter);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.cboStaffAssignID);
            this.Controls.Add(this.cboStaffAssignment);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.cboAccessKey);
            this.Controls.Add(this.cboDataValidation);
            this.Controls.Add(this.cboNotificationType);
            this.Controls.Add(this.cboBusinessUnit);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label13);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(243)))), ((int)(((byte)(229)))));
            this.MaximumSize = new System.Drawing.Size(972, 170);
            this.Name = "ExistingFlowSteps";
            this.Size = new System.Drawing.Size(968, 166);
            ((System.ComponentModel.ISupportInitialize)(this.flowStepBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cboStaffAssignID;
		private System.Windows.Forms.ComboBox cboStaffAssignment;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.ComboBox cboAccessKey;
		private System.Windows.Forms.ComboBox cboDataValidation;
		private System.Windows.Forms.ComboBox cboNotificationType;
		private System.Windows.Forms.ComboBox cboBusinessUnit;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Label lblCounter;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.ComboBox cboStatus;
		private System.Windows.Forms.TextBox txtDisplayText;
		private System.Windows.Forms.BindingSource flowStepBindingSource;
	}
}
