namespace INCIDENTRP
{
	partial class ScansProbesIncidentType
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
			this.chkVulnerabilityScan = new System.Windows.Forms.CheckBox();
			this.chkPortScan = new System.Windows.Forms.CheckBox();
			this.chkAlarm = new System.Windows.Forms.CheckBox();
			this.chkSniffer = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkVulnerabilityScan);
			this.groupBox1.Controls.Add(this.chkPortScan);
			this.groupBox1.Controls.Add(this.chkAlarm);
			this.groupBox1.Controls.Add(this.chkSniffer);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 99);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// chkVulnerabilityScan
			// 
			this.chkVulnerabilityScan.AutoSize = true;
			this.chkVulnerabilityScan.Location = new System.Drawing.Point(6, 79);
			this.chkVulnerabilityScan.Name = "chkVulnerabilityScan";
			this.chkVulnerabilityScan.Size = new System.Drawing.Size(196, 17);
			this.chkVulnerabilityScan.TabIndex = 3;
			this.chkVulnerabilityScan.Text = "Unauthorized Vulnerability Scanning";
			this.chkVulnerabilityScan.UseVisualStyleBackColor = true;
			this.chkVulnerabilityScan.CheckedChanged += new System.EventHandler(this.chkVulnerabilityScan_CheckedChanged);
			// 
			// chkPortScan
			// 
			this.chkPortScan.AutoSize = true;
			this.chkPortScan.Location = new System.Drawing.Point(6, 56);
			this.chkPortScan.Name = "chkPortScan";
			this.chkPortScan.Size = new System.Drawing.Size(159, 17);
			this.chkPortScan.TabIndex = 2;
			this.chkPortScan.Text = "Unauthorized Port Scanning";
			this.chkPortScan.UseVisualStyleBackColor = true;
			this.chkPortScan.CheckedChanged += new System.EventHandler(this.chkPortScan_CheckedChanged);
			// 
			// chkAlarm
			// 
			this.chkAlarm.AutoSize = true;
			this.chkAlarm.Location = new System.Drawing.Point(6, 33);
			this.chkAlarm.Name = "chkAlarm";
			this.chkAlarm.Size = new System.Drawing.Size(331, 17);
			this.chkAlarm.TabIndex = 1;
			this.chkAlarm.Text = "Priority System Alarm or Indication from IDS (Not a False Positive)";
			this.chkAlarm.UseVisualStyleBackColor = true;
			this.chkAlarm.CheckedChanged += new System.EventHandler(this.chkAlarm_CheckedChanged);
			// 
			// chkSniffer
			// 
			this.chkSniffer.AutoSize = true;
			this.chkSniffer.Location = new System.Drawing.Point(6, 10);
			this.chkSniffer.Name = "chkSniffer";
			this.chkSniffer.Size = new System.Drawing.Size(289, 17);
			this.chkSniffer.TabIndex = 0;
			this.chkSniffer.Text = "Operation of an Unauthorized Program or Sniffer Device";
			this.chkSniffer.UseVisualStyleBackColor = true;
			this.chkSniffer.CheckedChanged += new System.EventHandler(this.chkSniffer_CheckedChanged);
			// 
			// ScansProbesIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "ScansProbesIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkVulnerabilityScan;
		private System.Windows.Forms.CheckBox chkPortScan;
		private System.Windows.Forms.CheckBox chkAlarm;
		private System.Windows.Forms.CheckBox chkSniffer;
	}
}
