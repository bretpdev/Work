namespace ACDCFlows
{
	partial class StaffAssignment
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
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lblStepDescription = new System.Windows.Forms.Label();
			this.flowStepInfoForUserSearchBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.lblFlowDescription = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.lblDisplayText = new System.Windows.Forms.Label();
			this.lblNotificationType = new System.Windows.Forms.Label();
			this.lblBusinessUnit = new System.Windows.Forms.Label();
			this.lblFlowID = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.lblStaffAssignment = new System.Windows.Forms.Label();
			this.lblAccessKey = new System.Windows.Forms.Label();
			this.lblSystem = new System.Windows.Forms.Label();
			this.lblSequence = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.flowStepInfoForUserSearchBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "FlowID:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Business Unit Access Only:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(15, 45);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Notification Type:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(15, 66);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Control Display Text:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(15, 87);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Status:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(15, 108);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "Flow Description:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(15, 129);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(88, 13);
			this.label7.TabIndex = 6;
			this.label7.Text = "Step Description:";
			// 
			// lblStepDescription
			// 
			this.lblStepDescription.AutoSize = true;
			this.lblStepDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepInfoForUserSearchBindingSource, "StepDescription", true));
			this.lblStepDescription.Location = new System.Drawing.Point(166, 129);
			this.lblStepDescription.Name = "lblStepDescription";
			this.lblStepDescription.Size = new System.Drawing.Size(0, 13);
			this.lblStepDescription.TabIndex = 13;
			// 
			// flowStepInfoForUserSearchBindingSource
			// 
			this.flowStepInfoForUserSearchBindingSource.DataSource = typeof(ACDCFlows.FlowStepInfoForUserSearch);
			// 
			// lblFlowDescription
			// 
			this.lblFlowDescription.AutoSize = true;
			this.lblFlowDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepInfoForUserSearchBindingSource, "FlowDescription", true));
			this.lblFlowDescription.Location = new System.Drawing.Point(166, 108);
			this.lblFlowDescription.Name = "lblFlowDescription";
			this.lblFlowDescription.Size = new System.Drawing.Size(0, 13);
			this.lblFlowDescription.TabIndex = 12;
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepInfoForUserSearchBindingSource, "Status", true));
			this.lblStatus.Location = new System.Drawing.Point(166, 87);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(0, 13);
			this.lblStatus.TabIndex = 11;
			// 
			// lblDisplayText
			// 
			this.lblDisplayText.AutoSize = true;
			this.lblDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepInfoForUserSearchBindingSource, "ControlDisplayText", true));
			this.lblDisplayText.Location = new System.Drawing.Point(166, 66);
			this.lblDisplayText.Name = "lblDisplayText";
			this.lblDisplayText.Size = new System.Drawing.Size(0, 13);
			this.lblDisplayText.TabIndex = 10;
			// 
			// lblNotificationType
			// 
			this.lblNotificationType.AutoSize = true;
			this.lblNotificationType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepInfoForUserSearchBindingSource, "NotificationType", true));
			this.lblNotificationType.Location = new System.Drawing.Point(166, 45);
			this.lblNotificationType.Name = "lblNotificationType";
			this.lblNotificationType.Size = new System.Drawing.Size(0, 13);
			this.lblNotificationType.TabIndex = 9;
			// 
			// lblBusinessUnit
			// 
			this.lblBusinessUnit.AutoSize = true;
			this.lblBusinessUnit.Location = new System.Drawing.Point(166, 24);
			this.lblBusinessUnit.Name = "lblBusinessUnit";
			this.lblBusinessUnit.Size = new System.Drawing.Size(0, 13);
			this.lblBusinessUnit.TabIndex = 8;
			// 
			// lblFlowID
			// 
			this.lblFlowID.AutoSize = true;
			this.lblFlowID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepInfoForUserSearchBindingSource, "FlowID", true));
			this.lblFlowID.Location = new System.Drawing.Point(166, 3);
			this.lblFlowID.Name = "lblFlowID";
			this.lblFlowID.Size = new System.Drawing.Size(0, 13);
			this.lblFlowID.TabIndex = 7;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(514, 3);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(69, 13);
			this.label8.TabIndex = 14;
			this.label8.Text = "Sequence #:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(514, 24);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(44, 13);
			this.label9.TabIndex = 15;
			this.label9.Text = "System:";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(514, 45);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(66, 13);
			this.label10.TabIndex = 16;
			this.label10.Text = "Access Key:";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(514, 66);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(89, 13);
			this.label11.TabIndex = 17;
			this.label11.Text = "Staff Assignment:";
			// 
			// lblStaffAssignment
			// 
			this.lblStaffAssignment.AutoSize = true;
			this.lblStaffAssignment.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepInfoForUserSearchBindingSource, "StaffAssignmentLegalName", true));
			this.lblStaffAssignment.Location = new System.Drawing.Point(615, 66);
			this.lblStaffAssignment.Name = "lblStaffAssignment";
			this.lblStaffAssignment.Size = new System.Drawing.Size(0, 13);
			this.lblStaffAssignment.TabIndex = 21;
			// 
			// lblAccessKey
			// 
			this.lblAccessKey.AutoSize = true;
			this.lblAccessKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepInfoForUserSearchBindingSource, "AccessKey", true));
			this.lblAccessKey.Location = new System.Drawing.Point(615, 45);
			this.lblAccessKey.Name = "lblAccessKey";
			this.lblAccessKey.Size = new System.Drawing.Size(0, 13);
			this.lblAccessKey.TabIndex = 20;
			// 
			// lblSystem
			// 
			this.lblSystem.AutoSize = true;
			this.lblSystem.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepInfoForUserSearchBindingSource, "TheSystem", true));
			this.lblSystem.Location = new System.Drawing.Point(615, 24);
			this.lblSystem.Name = "lblSystem";
			this.lblSystem.Size = new System.Drawing.Size(0, 13);
			this.lblSystem.TabIndex = 19;
			// 
			// lblSequence
			// 
			this.lblSequence.AutoSize = true;
			this.lblSequence.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.flowStepInfoForUserSearchBindingSource, "FlowStepSequenceNumber", true));
			this.lblSequence.Location = new System.Drawing.Point(615, 3);
			this.lblSequence.Name = "lblSequence";
			this.lblSequence.Size = new System.Drawing.Size(0, 13);
			this.lblSequence.TabIndex = 18;
			// 
			// StaffAssignment
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.lblStaffAssignment);
			this.Controls.Add(this.lblAccessKey);
			this.Controls.Add(this.lblSystem);
			this.Controls.Add(this.lblSequence);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.lblStepDescription);
			this.Controls.Add(this.lblFlowDescription);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.lblDisplayText);
			this.Controls.Add(this.lblNotificationType);
			this.Controls.Add(this.lblBusinessUnit);
			this.Controls.Add(this.lblFlowID);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(243)))), ((int)(((byte)(229)))));
			this.Name = "StaffAssignment";
			this.Size = new System.Drawing.Size(1000, 150);
			((System.ComponentModel.ISupportInitialize)(this.flowStepInfoForUserSearchBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblStepDescription;
		private System.Windows.Forms.Label lblFlowDescription;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label lblDisplayText;
		private System.Windows.Forms.Label lblNotificationType;
		private System.Windows.Forms.Label lblBusinessUnit;
		private System.Windows.Forms.Label lblFlowID;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label lblStaffAssignment;
		private System.Windows.Forms.Label lblAccessKey;
		private System.Windows.Forms.Label lblSystem;
		private System.Windows.Forms.Label lblSequence;
		private System.Windows.Forms.BindingSource flowStepInfoForUserSearchBindingSource;
	}
}
