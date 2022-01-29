namespace INCIDENTRP
{
	partial class ElectronicMailDeliveryIncidentType
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
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.chkIncorrectEmailAddress = new System.Windows.Forms.CheckBox();
			this.chkIncorrectAttachment = new System.Windows.Forms.CheckBox();
			this.chkFtp = new System.Windows.Forms.CheckBox();
			this.chkEmailAddressesDisclosed = new System.Windows.Forms.CheckBox();
			this.txtDetail = new System.Windows.Forms.TextBox();
			this.electronicMailDeliveryIncidentBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.lblDetail = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.electronicMailDeliveryIncidentBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.chkIncorrectEmailAddress);
			this.groupBox1.Controls.Add(this.chkIncorrectAttachment);
			this.groupBox1.Controls.Add(this.chkFtp);
			this.groupBox1.Controls.Add(this.chkEmailAddressesDisclosed);
			this.groupBox1.Controls.Add(this.txtDetail);
			this.groupBox1.Controls.Add(this.lblDetail);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 129);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Location = new System.Drawing.Point(264, 44);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(164, 43);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 9);
			this.label1.Margin = new System.Windows.Forms.Padding(3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(158, 32);
			this.label1.TabIndex = 12;
			this.label1.Text = "PII includes date of birth, SSN, and loan detail information.";
			// 
			// chkIncorrectEmailAddress
			// 
			this.chkIncorrectEmailAddress.AutoSize = true;
			this.chkIncorrectEmailAddress.Location = new System.Drawing.Point(6, 79);
			this.chkIncorrectEmailAddress.Name = "chkIncorrectEmailAddress";
			this.chkIncorrectEmailAddress.Size = new System.Drawing.Size(222, 17);
			this.chkIncorrectEmailAddress.TabIndex = 3;
			this.chkIncorrectEmailAddress.Text = "Incorrect E-mail Address--E-mail Delivered";
			this.chkIncorrectEmailAddress.UseVisualStyleBackColor = true;
			this.chkIncorrectEmailAddress.CheckedChanged += new System.EventHandler(this.chkIncorrectEmailAddress_CheckedChanged);
			// 
			// chkIncorrectAttachment
			// 
			this.chkIncorrectAttachment.AutoSize = true;
			this.chkIncorrectAttachment.Location = new System.Drawing.Point(6, 56);
			this.chkIncorrectAttachment.Name = "chkIncorrectAttachment";
			this.chkIncorrectAttachment.Size = new System.Drawing.Size(194, 17);
			this.chkIncorrectAttachment.TabIndex = 2;
			this.chkIncorrectAttachment.Text = "Incorrect Attachment Containing PII";
			this.chkIncorrectAttachment.UseVisualStyleBackColor = true;
			this.chkIncorrectAttachment.CheckedChanged += new System.EventHandler(this.chkIncorrectAttachment_CheckedChanged);
			// 
			// chkFtp
			// 
			this.chkFtp.AutoSize = true;
			this.chkFtp.Location = new System.Drawing.Point(6, 33);
			this.chkFtp.Name = "chkFtp";
			this.chkFtp.Size = new System.Drawing.Size(248, 17);
			this.chkFtp.TabIndex = 1;
			this.chkFtp.Text = "FTP Transmission Sent to Incorrect Destination";
			this.chkFtp.UseVisualStyleBackColor = true;
			this.chkFtp.CheckedChanged += new System.EventHandler(this.chkFtp_CheckedChanged);
			// 
			// chkEmailAddressesDisclosed
			// 
			this.chkEmailAddressesDisclosed.AutoSize = true;
			this.chkEmailAddressesDisclosed.Location = new System.Drawing.Point(6, 10);
			this.chkEmailAddressesDisclosed.Name = "chkEmailAddressesDisclosed";
			this.chkEmailAddressesDisclosed.Size = new System.Drawing.Size(201, 17);
			this.chkEmailAddressesDisclosed.TabIndex = 0;
			this.chkEmailAddressesDisclosed.Text = "E-mail Addresses of Others Disclosed";
			this.chkEmailAddressesDisclosed.UseVisualStyleBackColor = true;
			this.chkEmailAddressesDisclosed.CheckedChanged += new System.EventHandler(this.chkEmailAddressesDisclosed_CheckedChanged);
			// 
			// txtDetail
			// 
			this.txtDetail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.electronicMailDeliveryIncidentBindingSource, "Detail", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtDetail.Location = new System.Drawing.Point(229, 102);
			this.txtDetail.Name = "txtDetail";
			this.txtDetail.Size = new System.Drawing.Size(199, 20);
			this.txtDetail.TabIndex = 4;
			// 
			// electronicMailDeliveryIncidentBindingSource
			// 
			this.electronicMailDeliveryIncidentBindingSource.DataSource = typeof(INCIDENTRP.ElectronicMailDeliveryIncident);
			// 
			// lblDetail
			// 
			this.lblDetail.AutoSize = true;
			this.lblDetail.Location = new System.Drawing.Point(6, 105);
			this.lblDetail.Name = "lblDetail";
			this.lblDetail.Size = new System.Drawing.Size(0, 13);
			this.lblDetail.TabIndex = 0;
			// 
			// ElectronicMailDeliveryIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "ElectronicMailDeliveryIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.electronicMailDeliveryIncidentBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtDetail;
		private System.Windows.Forms.Label lblDetail;
		private System.Windows.Forms.CheckBox chkEmailAddressesDisclosed;
		private System.Windows.Forms.CheckBox chkFtp;
		private System.Windows.Forms.CheckBox chkIncorrectAttachment;
		private System.Windows.Forms.CheckBox chkIncorrectEmailAddress;
		private System.Windows.Forms.BindingSource electronicMailDeliveryIncidentBindingSource;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
	}
}
