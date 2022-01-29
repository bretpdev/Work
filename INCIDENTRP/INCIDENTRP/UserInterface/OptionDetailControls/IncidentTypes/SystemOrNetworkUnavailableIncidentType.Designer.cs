namespace INCIDENTRP
{
	partial class SystemOrNetworkUnavailableIncidentType
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
			this.chkDisruption = new System.Windows.Forms.CheckBox();
			this.chkLogin = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkLogin);
			this.groupBox1.Controls.Add(this.chkDisruption);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 54);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// chkDisruption
			// 
			this.chkDisruption.AutoSize = true;
			this.chkDisruption.Location = new System.Drawing.Point(7, 10);
			this.chkDisruption.Name = "chkDisruption";
			this.chkDisruption.Size = new System.Drawing.Size(159, 17);
			this.chkDisruption.TabIndex = 0;
			this.chkDisruption.Text = "Denial/Disruption of Service";
			this.chkDisruption.UseVisualStyleBackColor = true;
			this.chkDisruption.CheckedChanged += new System.EventHandler(this.chkDisruption_CheckedChanged);
			// 
			// chkLogin
			// 
			this.chkLogin.AutoSize = true;
			this.chkLogin.Location = new System.Drawing.Point(7, 33);
			this.chkLogin.Name = "chkLogin";
			this.chkLogin.Size = new System.Drawing.Size(172, 17);
			this.chkLogin.TabIndex = 1;
			this.chkLogin.Text = "Inability to Log into an Account";
			this.chkLogin.UseVisualStyleBackColor = true;
			this.chkLogin.CheckedChanged += new System.EventHandler(this.chkLogin_CheckedChanged);
			// 
			// SystemOrNetworkUnavailableIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "SystemOrNetworkUnavailableIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkDisruption;
		private System.Windows.Forms.CheckBox chkLogin;
	}
}
