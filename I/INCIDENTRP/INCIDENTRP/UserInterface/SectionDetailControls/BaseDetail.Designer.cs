namespace INCIDENTRP
{
	partial class BaseDetail
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
			this.btnPrevious = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSubmit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnPrevious
			// 
			this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrevious.Image = global::INCIDENTRP.Properties.Resources.PreviousArrow;
			this.btnPrevious.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnPrevious.Location = new System.Drawing.Point(278, 303);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(84, 34);
			this.btnPrevious.TabIndex = 0;
			this.btnPrevious.Text = "Previous";
			this.btnPrevious.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnPrevious.UseVisualStyleBackColor = true;
			// 
			// btnNext
			// 
			this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNext.Image = global::INCIDENTRP.Properties.Resources.NextArrow;
			this.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnNext.Location = new System.Drawing.Point(368, 303);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(84, 34);
			this.btnNext.TabIndex = 1;
			this.btnNext.Text = "Next";
			this.btnNext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnNext.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Image = global::INCIDENTRP.Properties.Resources.wrongx_icon;
			this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnCancel.Location = new System.Drawing.Point(8, 303);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(84, 34);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnSubmit
			// 
			this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSubmit.Enabled = false;
			this.btnSubmit.Image = global::INCIDENTRP.Properties.Resources.icon_checkmark_128;
			this.btnSubmit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnSubmit.Location = new System.Drawing.Point(143, 303);
			this.btnSubmit.Name = "btnSubmit";
			this.btnSubmit.Size = new System.Drawing.Size(84, 34);
			this.btnSubmit.TabIndex = 3;
			this.btnSubmit.Text = "Submit";
			this.btnSubmit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnSubmit.UseVisualStyleBackColor = true;
			this.btnSubmit.Visible = false;
			// 
			// BaseDetail
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnSubmit);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.btnPrevious);
			this.Name = "BaseDetail";
			this.Size = new System.Drawing.Size(460, 340);
			this.ResumeLayout(false);

		}

		#endregion

		internal System.Windows.Forms.Button btnPrevious;
		internal System.Windows.Forms.Button btnNext;
		internal System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.Button btnSubmit;

	}
}
