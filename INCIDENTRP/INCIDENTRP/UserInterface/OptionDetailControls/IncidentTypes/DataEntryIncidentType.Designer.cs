namespace INCIDENTRP
{
	partial class DataEntryIncidentType
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
			this.chkChanged = new System.Windows.Forms.CheckBox();
			this.chkDeleted = new System.Windows.Forms.CheckBox();
			this.chkAdded = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkChanged);
			this.groupBox1.Controls.Add(this.chkDeleted);
			this.groupBox1.Controls.Add(this.chkAdded);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 71);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// chkChanged
			// 
			this.chkChanged.AutoSize = true;
			this.chkChanged.Location = new System.Drawing.Point(6, 50);
			this.chkChanged.Name = "chkChanged";
			this.chkChanged.Size = new System.Drawing.Size(176, 17);
			this.chkChanged.TabIndex = 5;
			this.chkChanged.Text = "Information Incorrectly Changed";
			this.chkChanged.UseVisualStyleBackColor = true;
			this.chkChanged.CheckedChanged += new System.EventHandler(this.chkChanged_CheckedChanged);
			// 
			// chkDeleted
			// 
			this.chkDeleted.AutoSize = true;
			this.chkDeleted.Location = new System.Drawing.Point(6, 30);
			this.chkDeleted.Name = "chkDeleted";
			this.chkDeleted.Size = new System.Drawing.Size(170, 17);
			this.chkDeleted.TabIndex = 4;
			this.chkDeleted.Text = "Information Incorrectly Deleted";
			this.chkDeleted.UseVisualStyleBackColor = true;
			this.chkDeleted.CheckedChanged += new System.EventHandler(this.chkDeleted_CheckedChanged);
			// 
			// chkAdded
			// 
			this.chkAdded.AutoSize = true;
			this.chkAdded.Location = new System.Drawing.Point(6, 10);
			this.chkAdded.Name = "chkAdded";
			this.chkAdded.Size = new System.Drawing.Size(157, 17);
			this.chkAdded.TabIndex = 3;
			this.chkAdded.Text = "Incorrect Information Added";
			this.chkAdded.UseVisualStyleBackColor = true;
			this.chkAdded.CheckedChanged += new System.EventHandler(this.chkAdded_CheckedChanged);
			// 
			// DataEntryIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "DataEntryIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkAdded;
		private System.Windows.Forms.CheckBox chkChanged;
		private System.Windows.Forms.CheckBox chkDeleted;
	}
}
