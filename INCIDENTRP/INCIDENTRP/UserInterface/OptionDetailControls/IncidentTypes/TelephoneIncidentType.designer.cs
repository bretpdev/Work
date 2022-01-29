namespace INCIDENTRP
{
	partial class TelephoneIncidentType
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
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.telephoneIncidentBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.chkUnauthorizedIndividual = new System.Windows.Forms.CheckBox();
			this.chkVoicemail = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.telephoneIncidentBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.chkUnauthorizedIndividual);
			this.groupBox1.Controls.Add(this.chkVoicemail);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 82);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// textBox1
			// 
			this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.telephoneIncidentBindingSource, "UnauthorizedIndividual", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBox1.Location = new System.Drawing.Point(173, 56);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(255, 20);
			this.textBox1.TabIndex = 2;
			// 
			// telephoneIncidentBindingSource
			// 
			this.telephoneIncidentBindingSource.DataSource = typeof(INCIDENTRP.TelephoneIncident);
			// 
			// chkUnauthorizedIndividual
			// 
			this.chkUnauthorizedIndividual.AutoSize = true;
			this.chkUnauthorizedIndividual.Location = new System.Drawing.Point(7, 33);
			this.chkUnauthorizedIndividual.Name = "chkUnauthorizedIndividual";
			this.chkUnauthorizedIndividual.Size = new System.Drawing.Size(253, 17);
			this.chkUnauthorizedIndividual.TabIndex = 1;
			this.chkUnauthorizedIndividual.Text = "Revealed Information to Unauthorized Individual";
			this.chkUnauthorizedIndividual.UseVisualStyleBackColor = true;
			this.chkUnauthorizedIndividual.CheckedChanged += new System.EventHandler(this.chkUnauthorizedIndividual_CheckedChanged);
			// 
			// chkVoicemail
			// 
			this.chkVoicemail.AutoSize = true;
			this.chkVoicemail.Location = new System.Drawing.Point(7, 10);
			this.chkVoicemail.Name = "chkVoicemail";
			this.chkVoicemail.Size = new System.Drawing.Size(190, 17);
			this.chkVoicemail.TabIndex = 0;
			this.chkVoicemail.Text = "Revealed Information on Voicemail";
			this.chkVoicemail.UseVisualStyleBackColor = true;
			this.chkVoicemail.CheckedChanged += new System.EventHandler(this.chkVoicemail_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 59);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(161, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Name of Unauthorized Individual";
			// 
			// TelephoneIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "TelephoneIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.telephoneIncidentBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkUnauthorizedIndividual;
		private System.Windows.Forms.CheckBox chkVoicemail;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.BindingSource telephoneIncidentBindingSource;
		private System.Windows.Forms.Label label1;
	}
}
