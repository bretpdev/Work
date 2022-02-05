namespace DayFileRec
{
	partial class frmStatus
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
			this.txtStatus = new System.Windows.Forms.TextBox();
			this.btnAction = new System.Windows.Forms.Button();
			this.lblLocation = new System.Windows.Forms.Label();
			this.radFtp = new System.Windows.Forms.RadioButton();
			this.radArchive = new System.Windows.Forms.RadioButton();
			this.lblDownloadDate = new System.Windows.Forms.Label();
			this.dtpDownloadDate = new System.Windows.Forms.DateTimePicker();
			this.SuspendLayout();
			// 
			// txtStatus
			// 
			this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtStatus.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtStatus.Location = new System.Drawing.Point(0, 52);
			this.txtStatus.Multiline = true;
			this.txtStatus.Name = "txtStatus";
			this.txtStatus.ReadOnly = true;
			this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtStatus.Size = new System.Drawing.Size(556, 420);
			this.txtStatus.TabIndex = 0;
			// 
			// btnAction
			// 
			this.btnAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAction.Location = new System.Drawing.Point(469, 12);
			this.btnAction.Name = "btnAction";
			this.btnAction.Size = new System.Drawing.Size(75, 23);
			this.btnAction.TabIndex = 1;
			this.btnAction.Text = "Start";
			this.btnAction.UseVisualStyleBackColor = true;
			this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
			// 
			// lblLocation
			// 
			this.lblLocation.AutoSize = true;
			this.lblLocation.Location = new System.Drawing.Point(12, 17);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.Size = new System.Drawing.Size(84, 13);
			this.lblLocation.TabIndex = 2;
			this.lblLocation.Text = "Search location:";
			// 
			// radFtp
			// 
			this.radFtp.AutoSize = true;
			this.radFtp.Checked = true;
			this.radFtp.Location = new System.Drawing.Point(102, 15);
			this.radFtp.Name = "radFtp";
			this.radFtp.Size = new System.Drawing.Size(45, 17);
			this.radFtp.TabIndex = 3;
			this.radFtp.TabStop = true;
			this.radFtp.Text = "FTP";
			this.radFtp.UseVisualStyleBackColor = true;
			this.radFtp.CheckedChanged += new System.EventHandler(this.radFtp_CheckedChanged);
			// 
			// radArchive
			// 
			this.radArchive.AutoSize = true;
			this.radArchive.Location = new System.Drawing.Point(153, 15);
			this.radArchive.Name = "radArchive";
			this.radArchive.Size = new System.Drawing.Size(61, 17);
			this.radArchive.TabIndex = 4;
			this.radArchive.Text = "Archive";
			this.radArchive.UseVisualStyleBackColor = true;
			this.radArchive.CheckedChanged += new System.EventHandler(this.radArchive_CheckedChanged);
			// 
			// lblDownloadDate
			// 
			this.lblDownloadDate.AutoSize = true;
			this.lblDownloadDate.Enabled = false;
			this.lblDownloadDate.Location = new System.Drawing.Point(240, 17);
			this.lblDownloadDate.Name = "lblDownloadDate";
			this.lblDownloadDate.Size = new System.Drawing.Size(82, 13);
			this.lblDownloadDate.TabIndex = 5;
			this.lblDownloadDate.Text = "Download date:";
			// 
			// dtpDownloadDate
			// 
			this.dtpDownloadDate.Enabled = false;
			this.dtpDownloadDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpDownloadDate.Location = new System.Drawing.Point(328, 13);
			this.dtpDownloadDate.Name = "dtpDownloadDate";
			this.dtpDownloadDate.Size = new System.Drawing.Size(100, 20);
			this.dtpDownloadDate.TabIndex = 6;
			this.dtpDownloadDate.ValueChanged += new System.EventHandler(this.dtpDownloadDate_ValueChanged);
			// 
			// frmStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(556, 472);
			this.Controls.Add(this.dtpDownloadDate);
			this.Controls.Add(this.lblDownloadDate);
			this.Controls.Add(this.radArchive);
			this.Controls.Add(this.radFtp);
			this.Controls.Add(this.lblLocation);
			this.Controls.Add(this.btnAction);
			this.Controls.Add(this.txtStatus);
			this.MinimumSize = new System.Drawing.Size(564, 116);
			this.Name = "frmStatus";
			this.ShowIcon = false;
			this.Text = "File Reconciliation";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtStatus;
		private System.Windows.Forms.Button btnAction;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.RadioButton radFtp;
		private System.Windows.Forms.RadioButton radArchive;
		private System.Windows.Forms.Label lblDownloadDate;
		private System.Windows.Forms.DateTimePicker dtpDownloadDate;
	}
}

