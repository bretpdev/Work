namespace INCIDENTRP
{
	partial class OddComputerBehaviorIncidentType
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
			this.chkMalware = new System.Windows.Forms.CheckBox();
			this.chkNewFiles = new System.Windows.Forms.CheckBox();
			this.chkFileLength = new System.Windows.Forms.CheckBox();
			this.chkDate = new System.Windows.Forms.CheckBox();
			this.chkSystemFiles = new System.Windows.Forms.CheckBox();
			this.chkDenialOfService = new System.Windows.Forms.CheckBox();
			this.chkPhishing = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkMalware);
			this.groupBox1.Controls.Add(this.chkNewFiles);
			this.groupBox1.Controls.Add(this.chkFileLength);
			this.groupBox1.Controls.Add(this.chkDate);
			this.groupBox1.Controls.Add(this.chkSystemFiles);
			this.groupBox1.Controls.Add(this.chkDenialOfService);
			this.groupBox1.Controls.Add(this.chkPhishing);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 169);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// chkMalware
			// 
			this.chkMalware.AutoSize = true;
			this.chkMalware.Location = new System.Drawing.Point(7, 148);
			this.chkMalware.Name = "chkMalware";
			this.chkMalware.Size = new System.Drawing.Size(127, 17);
			this.chkMalware.TabIndex = 6;
			this.chkMalware.Text = "Virus/Worm/Malware";
			this.chkMalware.UseVisualStyleBackColor = true;
			this.chkMalware.CheckedChanged += new System.EventHandler(this.chkMalware_CheckedChanged);
			// 
			// chkNewFiles
			// 
			this.chkNewFiles.AutoSize = true;
			this.chkNewFiles.Location = new System.Drawing.Point(7, 125);
			this.chkNewFiles.Name = "chkNewFiles";
			this.chkNewFiles.Size = new System.Drawing.Size(250, 17);
			this.chkNewFiles.TabIndex = 5;
			this.chkNewFiles.Text = "Unexplained New Files or Unfamiliar File Names";
			this.chkNewFiles.UseVisualStyleBackColor = true;
			this.chkNewFiles.CheckedChanged += new System.EventHandler(this.chkNewFiles_CheckedChanged);
			// 
			// chkFileLength
			// 
			this.chkFileLength.AutoSize = true;
			this.chkFileLength.Location = new System.Drawing.Point(7, 102);
			this.chkFileLength.Name = "chkFileLength";
			this.chkFileLength.Size = new System.Drawing.Size(288, 17);
			this.chkFileLength.TabIndex = 4;
			this.chkFileLength.Text = "Unexplained Modifications to File Lengths and/or Dates";
			this.chkFileLength.UseVisualStyleBackColor = true;
			this.chkFileLength.CheckedChanged += new System.EventHandler(this.chkFileLength_CheckedChanged);
			// 
			// chkDate
			// 
			this.chkDate.AutoSize = true;
			this.chkDate.Location = new System.Drawing.Point(7, 79);
			this.chkDate.Name = "chkDate";
			this.chkDate.Size = new System.Drawing.Size(237, 17);
			this.chkDate.TabIndex = 3;
			this.chkDate.Text = "Unexplained Modification or Deletion of Date";
			this.chkDate.UseVisualStyleBackColor = true;
			this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
			// 
			// chkSystemFiles
			// 
			this.chkSystemFiles.AutoSize = true;
			this.chkSystemFiles.Location = new System.Drawing.Point(7, 56);
			this.chkSystemFiles.Name = "chkSystemFiles";
			this.chkSystemFiles.Size = new System.Drawing.Size(366, 17);
			this.chkSystemFiles.TabIndex = 2;
			this.chkSystemFiles.Text = "Unexplained Attempt to Write to System Files or Changes in System Files";
			this.chkSystemFiles.UseVisualStyleBackColor = true;
			this.chkSystemFiles.CheckedChanged += new System.EventHandler(this.chkSystemFiles_CheckedChanged);
			// 
			// chkDenialOfService
			// 
			this.chkDenialOfService.AutoSize = true;
			this.chkDenialOfService.Location = new System.Drawing.Point(6, 33);
			this.chkDenialOfService.Name = "chkDenialOfService";
			this.chkDenialOfService.Size = new System.Drawing.Size(213, 17);
			this.chkDenialOfService.TabIndex = 1;
			this.chkDenialOfService.Text = "Participation in Denial of Service Attack";
			this.chkDenialOfService.UseVisualStyleBackColor = true;
			this.chkDenialOfService.CheckedChanged += new System.EventHandler(this.chkDenialOfService_CheckedChanged);
			// 
			// chkPhishing
			// 
			this.chkPhishing.AutoSize = true;
			this.chkPhishing.Location = new System.Drawing.Point(7, 10);
			this.chkPhishing.Name = "chkPhishing";
			this.chkPhishing.Size = new System.Drawing.Size(127, 17);
			this.chkPhishing.TabIndex = 0;
			this.chkPhishing.Text = "E-mail Phishing/Hoax";
			this.chkPhishing.UseVisualStyleBackColor = true;
			this.chkPhishing.CheckedChanged += new System.EventHandler(this.chkPhishing_CheckedChanged);
			// 
			// OddComputerBehaviorIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "OddComputerBehaviorIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkPhishing;
		private System.Windows.Forms.CheckBox chkMalware;
		private System.Windows.Forms.CheckBox chkNewFiles;
		private System.Windows.Forms.CheckBox chkFileLength;
		private System.Windows.Forms.CheckBox chkDate;
		private System.Windows.Forms.CheckBox chkSystemFiles;
		private System.Windows.Forms.CheckBox chkDenialOfService;
	}
}
