namespace INCIDENTRP
{
	partial class IncidentDetailSummary
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
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pnlSummary = new System.Windows.Forms.FlowLayoutPanel();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(460, 33);
			this.label1.TabIndex = 2;
			this.label1.Text = "Please review all information before submitting. If changes are necessary, use th" +
				"e link on the side bar or the back buttons. If all information is correct, click" +
				" Submit.";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pnlSummary);
			this.groupBox1.Location = new System.Drawing.Point(4, 25);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(453, 272);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			// 
			// pnlSummary
			// 
			this.pnlSummary.AutoScroll = true;
			this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSummary.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.pnlSummary.Location = new System.Drawing.Point(3, 16);
			this.pnlSummary.Name = "pnlSummary";
			this.pnlSummary.Size = new System.Drawing.Size(447, 253);
			this.pnlSummary.TabIndex = 0;
			this.pnlSummary.WrapContents = false;
			// 
			// IncidentDetailSummary
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Name = "IncidentDetailSummary";
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.groupBox1, 0);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.FlowLayoutPanel pnlSummary;
	}
}
