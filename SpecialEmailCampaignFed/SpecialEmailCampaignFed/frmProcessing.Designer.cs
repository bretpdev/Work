namespace SpecialEmailCampaignFed
{
	partial class frmProcessing
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
			this.lblProcessing = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblProcessing
			// 
			this.lblProcessing.AutoSize = true;
			this.lblProcessing.Font = new System.Drawing.Font("Engravers MT", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProcessing.Location = new System.Drawing.Point(65, 26);
			this.lblProcessing.MaximumSize = new System.Drawing.Size(350, 100);
			this.lblProcessing.Name = "lblProcessing";
			this.lblProcessing.Size = new System.Drawing.Size(329, 37);
			this.lblProcessing.TabIndex = 0;
			this.lblProcessing.Text = "Processing...";
			this.lblProcessing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// frmProcessing
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(461, 95);
			this.Controls.Add(this.lblProcessing);
			this.Name = "frmProcessing";
			this.Text = "Processing";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblProcessing;
	}
}