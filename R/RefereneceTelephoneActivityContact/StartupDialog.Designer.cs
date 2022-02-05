namespace RefereneceTelephoneActivityContact
{
	partial class StartupDialog
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartupDialog));
			this.btnRefCall = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnRefCall
			// 
			this.btnRefCall.BackColor = System.Drawing.Color.Tan;
			this.btnRefCall.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnRefCall.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnRefCall.Location = new System.Drawing.Point(42, 15);
			this.btnRefCall.Name = "btnRefCall";
			this.btnRefCall.Size = new System.Drawing.Size(120, 47);
			this.btnRefCall.TabIndex = 0;
			this.btnRefCall.Text = "REFCALL";
			this.btnRefCall.UseVisualStyleBackColor = false;
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.Tan;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(212, 15);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(120, 47);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "CANCEL";
			this.btnCancel.UseVisualStyleBackColor = false;
			// 
			// StartupDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.DarkGoldenrod;
			this.ClientSize = new System.Drawing.Size(374, 77);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnRefCall);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "StartupDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Reference Telephone Activity Contact";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnRefCall;
		private System.Windows.Forms.Button btnCancel;
	}
}