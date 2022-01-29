namespace INCIDENTRP
{
	partial class BorrowersRelativeCauseOption
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.chkYes = new System.Windows.Forms.CheckBox();
			this.incidentBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.chkNo = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.incidentBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.chkYes);
			this.groupBox1.Controls.Add(this.chkNo);
			this.groupBox1.Location = new System.Drawing.Point(4, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 40);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(185, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Did You Verify Borrower\'s SSN/DOB?";
			// 
			// chkYes
			// 
			this.chkYes.AutoSize = true;
			this.chkYes.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.incidentBindingSource, "BorrowerSsnAndDobAreVerified", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.chkYes.Location = new System.Drawing.Point(206, 15);
			this.chkYes.Name = "chkYes";
			this.chkYes.Size = new System.Drawing.Size(44, 17);
			this.chkYes.TabIndex = 0;
			this.chkYes.Text = "Yes";
			this.chkYes.UseVisualStyleBackColor = true;
			this.chkYes.CheckedChanged += new System.EventHandler(this.chkYes_CheckedChanged);
			// 
			// incidentBindingSource
			// 
			this.incidentBindingSource.DataSource = typeof(INCIDENTRP.Incident);
			// 
			// chkNo
			// 
			this.chkNo.AutoSize = true;
			this.chkNo.Checked = true;
			this.chkNo.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkNo.Location = new System.Drawing.Point(265, 15);
			this.chkNo.Name = "chkNo";
			this.chkNo.Size = new System.Drawing.Size(40, 17);
			this.chkNo.TabIndex = 1;
			this.chkNo.Text = "No";
			this.chkNo.UseVisualStyleBackColor = true;
			this.chkNo.CheckedChanged += new System.EventHandler(this.chkNo_CheckedChanged);
			// 
			// BorrowersRelativeCauseOption
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "BorrowersRelativeCauseOption";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.incidentBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkYes;
		private System.Windows.Forms.CheckBox chkNo;
		private System.Windows.Forms.BindingSource incidentBindingSource;
	}
}
