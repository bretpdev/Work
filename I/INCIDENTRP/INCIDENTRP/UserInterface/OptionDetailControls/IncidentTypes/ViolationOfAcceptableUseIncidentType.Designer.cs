namespace INCIDENTRP
{
	partial class ViolationOfAcceptableUseIncidentType
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
			this.chkSystemCredentials = new System.Windows.Forms.CheckBox();
			this.chkSystemResources = new System.Windows.Forms.CheckBox();
			this.chkKeycard = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkSystemCredentials);
			this.groupBox1.Controls.Add(this.chkSystemResources);
			this.groupBox1.Controls.Add(this.chkKeycard);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 76);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// chkSystemCredentials
			// 
			this.chkSystemCredentials.AutoSize = true;
			this.chkSystemCredentials.Location = new System.Drawing.Point(6, 56);
			this.chkSystemCredentials.Name = "chkSystemCredentials";
			this.chkSystemCredentials.Size = new System.Drawing.Size(177, 17);
			this.chkSystemCredentials.TabIndex = 2;
			this.chkSystemCredentials.Text = "User System Credentials Shared";
			this.chkSystemCredentials.UseVisualStyleBackColor = true;
			this.chkSystemCredentials.CheckedChanged += new System.EventHandler(this.chkSystemCredentials_CheckedChanged);
			// 
			// chkSystemResources
			// 
			this.chkSystemResources.AutoSize = true;
			this.chkSystemResources.Location = new System.Drawing.Point(6, 33);
			this.chkSystemResources.Name = "chkSystemResources";
			this.chkSystemResources.Size = new System.Drawing.Size(227, 17);
			this.chkSystemResources.TabIndex = 1;
			this.chkSystemResources.Text = "Misuse of System Resources by Valid User";
			this.chkSystemResources.UseVisualStyleBackColor = true;
			this.chkSystemResources.CheckedChanged += new System.EventHandler(this.chkSystemResources_CheckedChanged);
			// 
			// chkKeycard
			// 
			this.chkKeycard.AutoSize = true;
			this.chkKeycard.Location = new System.Drawing.Point(6, 10);
			this.chkKeycard.Name = "chkKeycard";
			this.chkKeycard.Size = new System.Drawing.Size(140, 17);
			this.chkKeycard.TabIndex = 0;
			this.chkKeycard.Text = "Access Keycard Shared";
			this.chkKeycard.UseVisualStyleBackColor = true;
			this.chkKeycard.CheckedChanged += new System.EventHandler(this.chkKeycard_CheckedChanged);
			// 
			// ViolationOfAcceptableUseIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "ViolationOfAcceptableUseIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkSystemCredentials;
		private System.Windows.Forms.CheckBox chkSystemResources;
		private System.Windows.Forms.CheckBox chkKeycard;
	}
}
