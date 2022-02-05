namespace INCIDENTRP
{
	partial class UnauthorizedSystemAccessIncidentType
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
			this.chkUnusualTime = new System.Windows.Forms.CheckBox();
			this.chkNewAccounts = new System.Windows.Forms.CheckBox();
			this.chkCredentials = new System.Windows.Forms.CheckBox();
			this.chkAccounting = new System.Windows.Forms.CheckBox();
			this.chkSystemLogs = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkUnusualTime);
			this.groupBox1.Controls.Add(this.chkNewAccounts);
			this.groupBox1.Controls.Add(this.chkCredentials);
			this.groupBox1.Controls.Add(this.chkAccounting);
			this.groupBox1.Controls.Add(this.chkSystemLogs);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 122);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// chkUnusualTime
			// 
			this.chkUnusualTime.AutoSize = true;
			this.chkUnusualTime.Location = new System.Drawing.Point(6, 102);
			this.chkUnusualTime.Name = "chkUnusualTime";
			this.chkUnusualTime.Size = new System.Drawing.Size(137, 17);
			this.chkUnusualTime.TabIndex = 12;
			this.chkUnusualTime.Text = "Unusual Time of Usage";
			this.chkUnusualTime.UseVisualStyleBackColor = true;
			this.chkUnusualTime.CheckedChanged += new System.EventHandler(this.chkUnusualTime_CheckedChanged);
			// 
			// chkNewAccounts
			// 
			this.chkNewAccounts.AutoSize = true;
			this.chkNewAccounts.Location = new System.Drawing.Point(6, 79);
			this.chkNewAccounts.Name = "chkNewAccounts";
			this.chkNewAccounts.Size = new System.Drawing.Size(183, 17);
			this.chkNewAccounts.TabIndex = 11;
			this.chkNewAccounts.Text = "Unexplained New User Accounts";
			this.chkNewAccounts.UseVisualStyleBackColor = true;
			this.chkNewAccounts.CheckedChanged += new System.EventHandler(this.chkNewAccounts_CheckedChanged);
			// 
			// chkCredentials
			// 
			this.chkCredentials.AutoSize = true;
			this.chkCredentials.Location = new System.Drawing.Point(6, 56);
			this.chkCredentials.Name = "chkCredentials";
			this.chkCredentials.Size = new System.Drawing.Size(203, 17);
			this.chkCredentials.TabIndex = 9;
			this.chkCredentials.Text = "Unauthorized Use of User Credentials";
			this.chkCredentials.UseVisualStyleBackColor = true;
			this.chkCredentials.CheckedChanged += new System.EventHandler(this.chkCredentials_CheckedChanged);
			// 
			// chkAccounting
			// 
			this.chkAccounting.AutoSize = true;
			this.chkAccounting.Location = new System.Drawing.Point(6, 33);
			this.chkAccounting.Name = "chkAccounting";
			this.chkAccounting.Size = new System.Drawing.Size(187, 17);
			this.chkAccounting.TabIndex = 8;
			this.chkAccounting.Text = "System Accounting Discrepancies";
			this.chkAccounting.UseVisualStyleBackColor = true;
			this.chkAccounting.CheckedChanged += new System.EventHandler(this.chkAccounting_CheckedChanged);
			// 
			// chkSystemLogs
			// 
			this.chkSystemLogs.AutoSize = true;
			this.chkSystemLogs.Location = new System.Drawing.Point(6, 10);
			this.chkSystemLogs.Name = "chkSystemLogs";
			this.chkSystemLogs.Size = new System.Drawing.Size(241, 17);
			this.chkSystemLogs.TabIndex = 7;
			this.chkSystemLogs.Text = "Suspicious Entries in System or Network Logs";
			this.chkSystemLogs.UseVisualStyleBackColor = true;
			this.chkSystemLogs.CheckedChanged += new System.EventHandler(this.chkSystemLogs_CheckedChanged);
			// 
			// UnauthorizedSystemAccessIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "UnauthorizedSystemAccessIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkUnusualTime;
		private System.Windows.Forms.CheckBox chkNewAccounts;
		private System.Windows.Forms.CheckBox chkCredentials;
		private System.Windows.Forms.CheckBox chkAccounting;
		private System.Windows.Forms.CheckBox chkSystemLogs;
	}
}
