namespace INCIDENTRP
{
	partial class DisposalOrDestructionIncidentType
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
			this.chkPaperMethod = new System.Windows.Forms.CheckBox();
			this.chkPaperError = new System.Windows.Forms.CheckBox();
			this.chkMicrofilmError = new System.Windows.Forms.CheckBox();
			this.chkMicrofilmMethod = new System.Windows.Forms.CheckBox();
			this.chkElectronicMethod = new System.Windows.Forms.CheckBox();
			this.chkElectronicError = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkPaperMethod);
			this.groupBox1.Controls.Add(this.chkPaperError);
			this.groupBox1.Controls.Add(this.chkMicrofilmError);
			this.groupBox1.Controls.Add(this.chkMicrofilmMethod);
			this.groupBox1.Controls.Add(this.chkElectronicMethod);
			this.groupBox1.Controls.Add(this.chkElectronicError);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 131);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// chkPaperMethod
			// 
			this.chkPaperMethod.AutoSize = true;
			this.chkPaperMethod.Location = new System.Drawing.Point(6, 110);
			this.chkPaperMethod.Name = "chkPaperMethod";
			this.chkPaperMethod.Size = new System.Drawing.Size(318, 17);
			this.chkPaperMethod.TabIndex = 5;
			this.chkPaperMethod.Text = "Paper Records Destroyed Using Incorrect Method/Technique";
			this.chkPaperMethod.UseVisualStyleBackColor = true;
			this.chkPaperMethod.CheckedChanged += new System.EventHandler(this.chkPaperMethod_CheckedChanged);
			// 
			// chkPaperError
			// 
			this.chkPaperError.AutoSize = true;
			this.chkPaperError.Location = new System.Drawing.Point(6, 90);
			this.chkPaperError.Name = "chkPaperError";
			this.chkPaperError.Size = new System.Drawing.Size(184, 17);
			this.chkPaperError.TabIndex = 4;
			this.chkPaperError.Text = "Paper Records Destroyed in Error";
			this.chkPaperError.UseVisualStyleBackColor = true;
			this.chkPaperError.CheckedChanged += new System.EventHandler(this.chkPaperError_CheckedChanged);
			// 
			// chkMicrofilmError
			// 
			this.chkMicrofilmError.AutoSize = true;
			this.chkMicrofilmError.Location = new System.Drawing.Point(6, 50);
			this.chkMicrofilmError.Name = "chkMicrofilmError";
			this.chkMicrofilmError.Size = new System.Drawing.Size(219, 17);
			this.chkMicrofilmError.TabIndex = 2;
			this.chkMicrofilmError.Text = "Microfilm with Records Destroyed in Error";
			this.chkMicrofilmError.UseVisualStyleBackColor = true;
			this.chkMicrofilmError.CheckedChanged += new System.EventHandler(this.chkMicrofilmError_CheckedChanged);
			// 
			// chkMicrofilmMethod
			// 
			this.chkMicrofilmMethod.AutoSize = true;
			this.chkMicrofilmMethod.Location = new System.Drawing.Point(6, 70);
			this.chkMicrofilmMethod.Name = "chkMicrofilmMethod";
			this.chkMicrofilmMethod.Size = new System.Drawing.Size(353, 17);
			this.chkMicrofilmMethod.TabIndex = 3;
			this.chkMicrofilmMethod.Text = "Microfilm with Records Destroyed Using Incorrect Method/Technique";
			this.chkMicrofilmMethod.UseVisualStyleBackColor = true;
			this.chkMicrofilmMethod.CheckedChanged += new System.EventHandler(this.chkMicrofilmMethod_CheckedChanged);
			// 
			// chkElectronicMethod
			// 
			this.chkElectronicMethod.AutoSize = true;
			this.chkElectronicMethod.Location = new System.Drawing.Point(6, 30);
			this.chkElectronicMethod.Name = "chkElectronicMethod";
			this.chkElectronicMethod.Size = new System.Drawing.Size(369, 17);
			this.chkElectronicMethod.TabIndex = 1;
			this.chkElectronicMethod.Text = "Electronic Media Records Destroyed Using Incorrect Method/Technique";
			this.chkElectronicMethod.UseVisualStyleBackColor = true;
			this.chkElectronicMethod.CheckedChanged += new System.EventHandler(this.chkElectronicMethod_CheckedChanged);
			// 
			// chkElectronicError
			// 
			this.chkElectronicError.AutoSize = true;
			this.chkElectronicError.Location = new System.Drawing.Point(6, 10);
			this.chkElectronicError.Name = "chkElectronicError";
			this.chkElectronicError.Size = new System.Drawing.Size(235, 17);
			this.chkElectronicError.TabIndex = 0;
			this.chkElectronicError.Text = "Electronic Media Records Destroyed in Error";
			this.chkElectronicError.UseVisualStyleBackColor = true;
			this.chkElectronicError.CheckedChanged += new System.EventHandler(this.chkElectronicError_CheckedChanged);
			// 
			// DisposalOrDestructionIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "DisposalOrDestructionIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkPaperMethod;
		private System.Windows.Forms.CheckBox chkPaperError;
		private System.Windows.Forms.CheckBox chkMicrofilmError;
		private System.Windows.Forms.CheckBox chkMicrofilmMethod;
		private System.Windows.Forms.CheckBox chkElectronicMethod;
		private System.Windows.Forms.CheckBox chkElectronicError;
	}
}
