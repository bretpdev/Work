namespace INCIDENTRP
{
	partial class SecurityIncidentSection
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
			this.pbxValid = new System.Windows.Forms.PictureBox();
			this.lblNavigationText = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pbxValid)).BeginInit();
			this.SuspendLayout();
			// 
			// pbxValid
			// 
			this.pbxValid.Location = new System.Drawing.Point(270, 3);
			this.pbxValid.Name = "pbxValid";
			this.pbxValid.Size = new System.Drawing.Size(25, 25);
			this.pbxValid.TabIndex = 0;
			this.pbxValid.TabStop = false;
			// 
			// lblNavigationText
			// 
			this.lblNavigationText.AutoSize = true;
			this.lblNavigationText.Cursor = System.Windows.Forms.Cursors.Hand;
			this.lblNavigationText.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblNavigationText.ForeColor = System.Drawing.Color.Blue;
			this.lblNavigationText.Location = new System.Drawing.Point(3, 3);
			this.lblNavigationText.Name = "lblNavigationText";
			this.lblNavigationText.Size = new System.Drawing.Size(190, 25);
			this.lblNavigationText.TabIndex = 1;
			this.lblNavigationText.Text = "Nav Text Goes Here";
			this.lblNavigationText.Click += new System.EventHandler(this.LblNavigationText_Click);
			// 
			// SecurityIncidentSection
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.lblNavigationText);
			this.Controls.Add(this.pbxValid);
			this.Name = "SecurityIncidentSection";
			this.Size = new System.Drawing.Size(299, 30);
			((System.ComponentModel.ISupportInitialize)(this.pbxValid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pbxValid;
		private System.Windows.Forms.Label lblNavigationText;
	}
}
