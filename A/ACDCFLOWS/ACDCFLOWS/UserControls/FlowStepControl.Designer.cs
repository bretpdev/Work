namespace ACDCFlows
{
	partial class FlowStepControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblDataValidation = new System.Windows.Forms.Label();
            this.lblDisplayText = new System.Windows.Forms.Label();
            this.lblNotificationType = new System.Windows.Forms.Label();
            this.lblBusinessUnit = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblStaffCalculation = new System.Windows.Forms.Label();
            this.lblAccessKey = new System.Windows.Forms.Label();
            this.lblStaffAssignment = new System.Windows.Forms.Label();
            this.lblSeqNum = new System.Windows.Forms.Label();
            this.flowStepBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.flowStepBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Business Unit Access Only:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Notification Type:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Control Display Text:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Data Validation ID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Description:";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "Description", true));
            this.lblDescription.Location = new System.Drawing.Point(158, 74);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(0, 13);
            this.lblDescription.TabIndex = 9;
            // 
            // lblDataValidation
            // 
            this.lblDataValidation.AutoSize = true;
            this.lblDataValidation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "DataValidationID", true));
            this.lblDataValidation.Location = new System.Drawing.Point(158, 57);
            this.lblDataValidation.Name = "lblDataValidation";
            this.lblDataValidation.Size = new System.Drawing.Size(0, 13);
            this.lblDataValidation.TabIndex = 8;
            // 
            // lblDisplayText
            // 
            this.lblDisplayText.AutoSize = true;
            this.lblDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "ControlDisplayText", true));
            this.lblDisplayText.Location = new System.Drawing.Point(158, 39);
            this.lblDisplayText.Name = "lblDisplayText";
            this.lblDisplayText.Size = new System.Drawing.Size(0, 13);
            this.lblDisplayText.TabIndex = 7;
            // 
            // lblNotificationType
            // 
            this.lblNotificationType.AutoSize = true;
            this.lblNotificationType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "NotificationType", true));
            this.lblNotificationType.Location = new System.Drawing.Point(158, 21);
            this.lblNotificationType.Name = "lblNotificationType";
            this.lblNotificationType.Size = new System.Drawing.Size(0, 13);
            this.lblNotificationType.TabIndex = 6;
            // 
            // lblBusinessUnit
            // 
            this.lblBusinessUnit.AutoSize = true;
            this.lblBusinessUnit.Location = new System.Drawing.Point(158, 4);
            this.lblBusinessUnit.Name = "lblBusinessUnit";
            this.lblBusinessUnit.Size = new System.Drawing.Size(0, 13);
            this.lblBusinessUnit.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(428, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Staff Assignment:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(428, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Access Key:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(428, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(144, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Staff Assignment Calculation:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(428, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Status:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "Status", true));
            this.lblStatus.Location = new System.Drawing.Point(590, 57);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 17;
            // 
            // lblStaffCalculation
            // 
            this.lblStaffCalculation.AutoSize = true;
            this.lblStaffCalculation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "StaffAssignmentCalculationID", true));
            this.lblStaffCalculation.Location = new System.Drawing.Point(590, 39);
            this.lblStaffCalculation.Name = "lblStaffCalculation";
            this.lblStaffCalculation.Size = new System.Drawing.Size(0, 13);
            this.lblStaffCalculation.TabIndex = 16;
            // 
            // lblAccessKey
            // 
            this.lblAccessKey.AutoSize = true;
            this.lblAccessKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "AccessKey", true));
            this.lblAccessKey.Location = new System.Drawing.Point(590, 4);
            this.lblAccessKey.Name = "lblAccessKey";
            this.lblAccessKey.Size = new System.Drawing.Size(0, 13);
            this.lblAccessKey.TabIndex = 15;
            // 
            // lblStaffAssignment
            // 
            this.lblStaffAssignment.AutoSize = true;
            this.lblStaffAssignment.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "StaffAssignmentLegalName", true));
            this.lblStaffAssignment.Location = new System.Drawing.Point(590, 21);
            this.lblStaffAssignment.Name = "lblStaffAssignment";
            this.lblStaffAssignment.Size = new System.Drawing.Size(0, 13);
            this.lblStaffAssignment.TabIndex = 14;
            // 
            // lblSeqNum
            // 
            this.lblSeqNum.AutoSize = true;
            this.lblSeqNum.BackColor = System.Drawing.Color.White;
            this.lblSeqNum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeqNum.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepBindingSource, "FlowStepSequenceNumber", true));
            this.lblSeqNum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSeqNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeqNum.ForeColor = System.Drawing.Color.Black;
            this.lblSeqNum.Location = new System.Drawing.Point(933, 4);
            this.lblSeqNum.Name = "lblSeqNum";
            this.lblSeqNum.Size = new System.Drawing.Size(2, 26);
            this.lblSeqNum.TabIndex = 18;
            // 
            // flowStepBindingSource
            // 
            this.flowStepBindingSource.DataSource = typeof(ACDCFlows.FlowStep);
            // 
            // FlowStepControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.lblSeqNum);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblStaffCalculation);
            this.Controls.Add(this.lblAccessKey);
            this.Controls.Add(this.lblStaffAssignment);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblDataValidation);
            this.Controls.Add(this.lblDisplayText);
            this.Controls.Add(this.lblNotificationType);
            this.Controls.Add(this.lblBusinessUnit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(243)))), ((int)(((byte)(229)))));
            this.MaximumSize = new System.Drawing.Size(990, 110);
            this.Name = "FlowStepControl";
            this.Size = new System.Drawing.Size(986, 106);
            ((System.ComponentModel.ISupportInitialize)(this.flowStepBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblDataValidation;
		private System.Windows.Forms.Label lblDisplayText;
		private System.Windows.Forms.Label lblNotificationType;
		private System.Windows.Forms.Label lblBusinessUnit;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label lblStaffCalculation;
		private System.Windows.Forms.Label lblAccessKey;
		private System.Windows.Forms.Label lblStaffAssignment;
		private System.Windows.Forms.BindingSource flowStepBindingSource;
		private System.Windows.Forms.Label lblSeqNum;
	}
}
