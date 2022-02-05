namespace INCIDENTRP
{
	partial class AccessControlIncidentType
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
			this.chkGranted = new System.Windows.Forms.CheckBox();
			this.chkSystem = new System.Windows.Forms.CheckBox();
			this.chkPhysical = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkPhysical);
			this.groupBox1.Controls.Add(this.chkSystem);
			this.groupBox1.Controls.Add(this.chkGranted);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 76);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// chkGranted
			// 
			this.chkGranted.AutoSize = true;
			this.chkGranted.Location = new System.Drawing.Point(6, 10);
			this.chkGranted.Name = "chkGranted";
			this.chkGranted.Size = new System.Drawing.Size(146, 17);
			this.chkGranted.TabIndex = 0;
			this.chkGranted.Text = "Improper Access Granted";
			this.chkGranted.UseVisualStyleBackColor = true;
			this.chkGranted.CheckedChanged += new System.EventHandler(this.chkGranted_CheckedChanged);
			// 
			// chkSystem
			// 
			this.chkSystem.AutoSize = true;
			this.chkSystem.Location = new System.Drawing.Point(6, 33);
			this.chkSystem.Name = "chkSystem";
			this.chkSystem.Size = new System.Drawing.Size(235, 17);
			this.chkSystem.TabIndex = 1;
			this.chkSystem.Text = "System Access NOT Terminated or Modified";
			this.chkSystem.UseVisualStyleBackColor = true;
			this.chkSystem.CheckedChanged += new System.EventHandler(this.chkSystem_CheckedChanged);
			// 
			// chkPhysical
			// 
			this.chkPhysical.AutoSize = true;
			this.chkPhysical.Location = new System.Drawing.Point(6, 56);
			this.chkPhysical.Name = "chkPhysical";
			this.chkPhysical.Size = new System.Drawing.Size(240, 17);
			this.chkPhysical.TabIndex = 2;
			this.chkPhysical.Text = "Physical Access NOT Terminated or Modified";
			this.chkPhysical.UseVisualStyleBackColor = true;
			this.chkPhysical.CheckedChanged += new System.EventHandler(this.chkPhysical_CheckedChanged);
			// 
			// AccessControlIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "AccessControlIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkPhysical;
		private System.Windows.Forms.CheckBox chkSystem;
		private System.Windows.Forms.CheckBox chkGranted;
	}
}
