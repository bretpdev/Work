namespace INCIDENTRP
{
	partial class RegularMailDeliveryIncidentType
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
			this.chkIncorrectContents = new System.Windows.Forms.CheckBox();
			this.chkIncorrectAddress = new System.Windows.Forms.CheckBox();
			this.txtTrackingNumber = new System.Windows.Forms.TextBox();
			this.regularMailDeliveryIncidentBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.lblTrackingNumber = new System.Windows.Forms.Label();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.regularMailDeliveryIncidentBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkIncorrectContents);
			this.groupBox1.Controls.Add(this.chkIncorrectAddress);
			this.groupBox1.Controls.Add(this.txtTrackingNumber);
			this.groupBox1.Controls.Add(this.lblTrackingNumber);
			this.groupBox1.Controls.Add(this.textBox5);
			this.groupBox1.Controls.Add(this.textBox4);
			this.groupBox1.Controls.Add(this.textBox3);
			this.groupBox1.Controls.Add(this.textBox2);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 191);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			// 
			// chkIncorrectContents
			// 
			this.chkIncorrectContents.AutoSize = true;
			this.chkIncorrectContents.Location = new System.Drawing.Point(6, 33);
			this.chkIncorrectContents.Name = "chkIncorrectContents";
			this.chkIncorrectContents.Size = new System.Drawing.Size(345, 17);
			this.chkIncorrectContents.TabIndex = 1;
			this.chkIncorrectContents.Text = "Incorrect Documents Contained in Envelope/Package and Opened";
			this.chkIncorrectContents.UseVisualStyleBackColor = true;
			this.chkIncorrectContents.CheckedChanged += new System.EventHandler(this.chkIncorrectContents_CheckedChanged);
			// 
			// chkIncorrectAddress
			// 
			this.chkIncorrectAddress.AutoSize = true;
			this.chkIncorrectAddress.Location = new System.Drawing.Point(6, 10);
			this.chkIncorrectAddress.Name = "chkIncorrectAddress";
			this.chkIncorrectAddress.Size = new System.Drawing.Size(235, 17);
			this.chkIncorrectAddress.TabIndex = 0;
			this.chkIncorrectAddress.Text = "Mail Delivered to Incorrect Address--Opened";
			this.chkIncorrectAddress.UseVisualStyleBackColor = true;
			this.chkIncorrectAddress.CheckedChanged += new System.EventHandler(this.chkIncorrectAddress_CheckedChanged);
			// 
			// txtTrackingNumber
			// 
			this.txtTrackingNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.regularMailDeliveryIncidentBindingSource, "TrackingNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtTrackingNumber.Location = new System.Drawing.Point(100, 165);
			this.txtTrackingNumber.Name = "txtTrackingNumber";
			this.txtTrackingNumber.Size = new System.Drawing.Size(328, 20);
			this.txtTrackingNumber.TabIndex = 7;
			this.txtTrackingNumber.Visible = false;
			// 
			// regularMailDeliveryIncidentBindingSource
			// 
			this.regularMailDeliveryIncidentBindingSource.DataSource = typeof(INCIDENTRP.RegularMailDeliveryIncident);
			// 
			// lblTrackingNumber
			// 
			this.lblTrackingNumber.AutoSize = true;
			this.lblTrackingNumber.Location = new System.Drawing.Point(6, 168);
			this.lblTrackingNumber.Name = "lblTrackingNumber";
			this.lblTrackingNumber.Size = new System.Drawing.Size(59, 13);
			this.lblTrackingNumber.TabIndex = 10;
			this.lblTrackingNumber.Text = "Tracking #";
			this.lblTrackingNumber.Visible = false;
			// 
			// textBox5
			// 
			this.textBox5.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.regularMailDeliveryIncidentBindingSource, "Address1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBox5.Location = new System.Drawing.Point(100, 65);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(328, 20);
			this.textBox5.TabIndex = 2;
			// 
			// textBox4
			// 
			this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.regularMailDeliveryIncidentBindingSource, "Address2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBox4.Location = new System.Drawing.Point(100, 85);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(328, 20);
			this.textBox4.TabIndex = 3;
			// 
			// textBox3
			// 
			this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.regularMailDeliveryIncidentBindingSource, "City", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBox3.Location = new System.Drawing.Point(100, 105);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(328, 20);
			this.textBox3.TabIndex = 4;
			// 
			// textBox2
			// 
			this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.regularMailDeliveryIncidentBindingSource, "State", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBox2.Location = new System.Drawing.Point(100, 125);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(155, 20);
			this.textBox2.TabIndex = 5;
			// 
			// textBox1
			// 
			this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.regularMailDeliveryIncidentBindingSource, "Zip", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBox1.Location = new System.Drawing.Point(100, 145);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(230, 20);
			this.textBox1.TabIndex = 6;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 68);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(84, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Address Line #1";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(84, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Address Line #2";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 108);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(24, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "City";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 128);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "State";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 148);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(24, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "ZIP";
			// 
			// RegularMailDeliveryIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "RegularMailDeliveryIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.regularMailDeliveryIncidentBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblTrackingNumber;
		private System.Windows.Forms.TextBox txtTrackingNumber;
		private System.Windows.Forms.CheckBox chkIncorrectContents;
		private System.Windows.Forms.CheckBox chkIncorrectAddress;
		private System.Windows.Forms.BindingSource regularMailDeliveryIncidentBindingSource;
	}
}
