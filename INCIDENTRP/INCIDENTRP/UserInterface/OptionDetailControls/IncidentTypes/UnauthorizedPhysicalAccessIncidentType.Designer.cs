namespace INCIDENTRP
{
	partial class UnauthorizedPhysicalAccessIncidentType
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkTimeOfUsage = new System.Windows.Forms.CheckBox();
			this.chkNewKeycard = new System.Windows.Forms.CheckBox();
			this.chkUseOfKeycard = new System.Windows.Forms.CheckBox();
			this.chkVideoLogs = new System.Windows.Forms.CheckBox();
			this.chkAccessLogs = new System.Windows.Forms.CheckBox();
			this.chkPiggybacking = new System.Windows.Forms.CheckBox();
			this.chkTrespassing = new System.Windows.Forms.CheckBox();
			this.chkAccessAccounting = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkTimeOfUsage);
			this.groupBox1.Controls.Add(this.chkNewKeycard);
			this.groupBox1.Controls.Add(this.chkUseOfKeycard);
			this.groupBox1.Controls.Add(this.chkVideoLogs);
			this.groupBox1.Controls.Add(this.chkAccessLogs);
			this.groupBox1.Controls.Add(this.chkPiggybacking);
			this.groupBox1.Controls.Add(this.chkTrespassing);
			this.groupBox1.Controls.Add(this.chkAccessAccounting);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(424, 192);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// chkTimeOfUsage
			// 
			this.chkTimeOfUsage.AutoSize = true;
			this.chkTimeOfUsage.Location = new System.Drawing.Point(7, 171);
			this.chkTimeOfUsage.Name = "chkTimeOfUsage";
			this.chkTimeOfUsage.Size = new System.Drawing.Size(137, 17);
			this.chkTimeOfUsage.TabIndex = 7;
			this.chkTimeOfUsage.Text = "Unusual Time of Usage";
			this.chkTimeOfUsage.UseVisualStyleBackColor = true;
			this.chkTimeOfUsage.CheckedChanged += new System.EventHandler(this.chkTimeOfUsage_CheckedChanged);
			// 
			// chkNewKeycard
			// 
			this.chkNewKeycard.AutoSize = true;
			this.chkNewKeycard.Location = new System.Drawing.Point(7, 148);
			this.chkNewKeycard.Name = "chkNewKeycard";
			this.chkNewKeycard.Size = new System.Drawing.Size(157, 17);
			this.chkNewKeycard.TabIndex = 6;
			this.chkNewKeycard.Text = "Unexplained New Keycards";
			this.chkNewKeycard.UseVisualStyleBackColor = true;
			this.chkNewKeycard.CheckedChanged += new System.EventHandler(this.chkNewKeycard_CheckedChanged);
			// 
			// chkUseOfKeycard
			// 
			this.chkUseOfKeycard.AutoSize = true;
			this.chkUseOfKeycard.Location = new System.Drawing.Point(7, 125);
			this.chkUseOfKeycard.Name = "chkUseOfKeycard";
			this.chkUseOfKeycard.Size = new System.Drawing.Size(165, 17);
			this.chkUseOfKeycard.TabIndex = 5;
			this.chkUseOfKeycard.Text = "Unauthorized Use of Keycard";
			this.chkUseOfKeycard.UseVisualStyleBackColor = true;
			this.chkUseOfKeycard.CheckedChanged += new System.EventHandler(this.chkUseOfKeycard_CheckedChanged);
			// 
			// chkVideoLogs
			// 
			this.chkVideoLogs.AutoSize = true;
			this.chkVideoLogs.Location = new System.Drawing.Point(7, 102);
			this.chkVideoLogs.Name = "chkVideoLogs";
			this.chkVideoLogs.Size = new System.Drawing.Size(179, 17);
			this.chkVideoLogs.TabIndex = 4;
			this.chkVideoLogs.Text = "Suspicious Entries in Video Logs";
			this.chkVideoLogs.UseVisualStyleBackColor = true;
			this.chkVideoLogs.CheckedChanged += new System.EventHandler(this.chkVideoLogs_CheckedChanged);
			// 
			// chkAccessLogs
			// 
			this.chkAccessLogs.AutoSize = true;
			this.chkAccessLogs.Location = new System.Drawing.Point(7, 79);
			this.chkAccessLogs.Name = "chkAccessLogs";
			this.chkAccessLogs.Size = new System.Drawing.Size(187, 17);
			this.chkAccessLogs.TabIndex = 3;
			this.chkAccessLogs.Text = "Suspicious Entries in Access Logs";
			this.chkAccessLogs.UseVisualStyleBackColor = true;
			this.chkAccessLogs.CheckedChanged += new System.EventHandler(this.chkAccessLogs_CheckedChanged);
			// 
			// chkPiggybacking
			// 
			this.chkPiggybacking.AutoSize = true;
			this.chkPiggybacking.Location = new System.Drawing.Point(7, 56);
			this.chkPiggybacking.Name = "chkPiggybacking";
			this.chkPiggybacking.Size = new System.Drawing.Size(141, 17);
			this.chkPiggybacking.TabIndex = 2;
			this.chkPiggybacking.Text = "Piggybacking/Tailgating";
			this.chkPiggybacking.UseVisualStyleBackColor = true;
			this.chkPiggybacking.CheckedChanged += new System.EventHandler(this.chkPiggybacking_CheckedChanged);
			// 
			// chkTrespassing
			// 
			this.chkTrespassing.AutoSize = true;
			this.chkTrespassing.Location = new System.Drawing.Point(7, 33);
			this.chkTrespassing.Name = "chkTrespassing";
			this.chkTrespassing.Size = new System.Drawing.Size(168, 17);
			this.chkTrespassing.TabIndex = 1;
			this.chkTrespassing.Text = "Building Break-In/Trespassing";
			this.chkTrespassing.UseVisualStyleBackColor = true;
			this.chkTrespassing.CheckedChanged += new System.EventHandler(this.chkTrespassing_CheckedChanged);
			// 
			// chkAccessAccounting
			// 
			this.chkAccessAccounting.AutoSize = true;
			this.chkAccessAccounting.Location = new System.Drawing.Point(7, 10);
			this.chkAccessAccounting.Name = "chkAccessAccounting";
			this.chkAccessAccounting.Size = new System.Drawing.Size(188, 17);
			this.chkAccessAccounting.TabIndex = 0;
			this.chkAccessAccounting.Text = "Access Accounting Discrepancies";
			this.chkAccessAccounting.UseVisualStyleBackColor = true;
			this.chkAccessAccounting.CheckedChanged += new System.EventHandler(this.chkAccessAccounting_CheckedChanged);
			// 
			// UnauthorizedPhysicalAccessIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "UnauthorizedPhysicalAccessIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkTimeOfUsage;
		private System.Windows.Forms.CheckBox chkNewKeycard;
		private System.Windows.Forms.CheckBox chkUseOfKeycard;
		private System.Windows.Forms.CheckBox chkVideoLogs;
		private System.Windows.Forms.CheckBox chkAccessLogs;
		private System.Windows.Forms.CheckBox chkPiggybacking;
		private System.Windows.Forms.CheckBox chkTrespassing;
		private System.Windows.Forms.CheckBox chkAccessAccounting;
	}
}
