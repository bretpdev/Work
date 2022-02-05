namespace INCIDENTRP
{
	partial class FaxIncidentType
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
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.faxIncidentBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.label2 = new System.Windows.Forms.Label();
			this.chkIncorrectFaxNumber = new System.Windows.Forms.CheckBox();
			this.chkIncorrectDocuments = new System.Windows.Forms.CheckBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.faxIncidentBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textBox2);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.chkIncorrectFaxNumber);
			this.groupBox1.Controls.Add(this.chkIncorrectDocuments);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(3, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(434, 110);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// textBox2
			// 
			this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.faxIncidentBindingSource, "Recipient", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBox2.Location = new System.Drawing.Point(76, 39);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(144, 20);
			this.textBox2.TabIndex = 1;
			// 
			// faxIncidentBindingSource
			// 
			this.faxIncidentBindingSource.DataSource = typeof(INCIDENTRP.FaxIncident);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(52, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Recipient";
			// 
			// chkIncorrectFaxNumber
			// 
			this.chkIncorrectFaxNumber.AutoSize = true;
			this.chkIncorrectFaxNumber.Location = new System.Drawing.Point(9, 88);
			this.chkIncorrectFaxNumber.Name = "chkIncorrectFaxNumber";
			this.chkIncorrectFaxNumber.Size = new System.Drawing.Size(128, 17);
			this.chkIncorrectFaxNumber.TabIndex = 3;
			this.chkIncorrectFaxNumber.Text = "Incorrect Fax Number";
			this.chkIncorrectFaxNumber.UseVisualStyleBackColor = true;
			this.chkIncorrectFaxNumber.CheckedChanged += new System.EventHandler(this.chkIncorrectFaxNumber_CheckedChanged);
			// 
			// chkIncorrectDocuments
			// 
			this.chkIncorrectDocuments.AutoSize = true;
			this.chkIncorrectDocuments.Location = new System.Drawing.Point(9, 65);
			this.chkIncorrectDocuments.Name = "chkIncorrectDocuments";
			this.chkIncorrectDocuments.Size = new System.Drawing.Size(157, 17);
			this.chkIncorrectDocuments.TabIndex = 2;
			this.chkIncorrectDocuments.Text = "Incorrect Documents Faxed";
			this.chkIncorrectDocuments.UseVisualStyleBackColor = true;
			this.chkIncorrectDocuments.CheckedChanged += new System.EventHandler(this.chkIncorrectDocuments_CheckedChanged);
			// 
			// textBox1
			// 
			this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.faxIncidentBindingSource, "FaxNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.textBox1.Location = new System.Drawing.Point(76, 13);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(144, 20);
			this.textBox1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Fax Number";
			// 
			// FaxIncidentType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "FaxIncidentType";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.faxIncidentBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkIncorrectFaxNumber;
		private System.Windows.Forms.BindingSource faxIncidentBindingSource;
		private System.Windows.Forms.CheckBox chkIncorrectDocuments;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label2;
	}
}
