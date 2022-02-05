using Uheaa.Common.ProcessLogger;

namespace INCIDENTRP
{
	partial class IncidentReporterDetail
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
            this.label6 = new System.Windows.Forms.Label();
            this.cmbPriority = new System.Windows.Forms.ComboBox();
            this.incidentBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.reporterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.cmbBusinessUnit = new System.Windows.Forms.ComboBox();
            this.businessUnitBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbUser = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.txtEmailAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.incidentBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporterBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessUnitBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmbPriority);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbBusinessUnit);
            this.groupBox1.Controls.Add(this.cmbUser);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.txtEmailAddress);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(454, 155);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Incident Reporter";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Priority";
            // 
            // cmbPriority
            // 
            this.cmbPriority.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbPriority.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbPriority.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.incidentBindingSource, "Priority", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPriority.FormattingEnabled = true;
            this.cmbPriority.Location = new System.Drawing.Point(172, 128);
            this.cmbPriority.Name = "cmbPriority";
            this.cmbPriority.Size = new System.Drawing.Size(276, 21);
            this.cmbPriority.TabIndex = 5;
            // 
            // incidentBindingSource
            // 
            this.incidentBindingSource.DataSource = typeof(INCIDENTRP.Incident);
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.reporterBindingSource, "Location", true));
            this.textBox1.Location = new System.Drawing.Point(172, 106);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(276, 20);
            this.textBox1.TabIndex = 4;
            // 
            // reporterBindingSource
            // 
            this.reporterBindingSource.DataSource = typeof(INCIDENTRP.Reporter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Location";
            // 
            // cmbBusinessUnit
            // 
            this.cmbBusinessUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbBusinessUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBusinessUnit.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.reporterBindingSource, "BusinessUnit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbBusinessUnit.DataSource = this.businessUnitBindingSource;
            this.cmbBusinessUnit.DisplayMember = "Name";
            this.cmbBusinessUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBusinessUnit.FormattingEnabled = true;
            this.cmbBusinessUnit.Location = new System.Drawing.Point(172, 39);
            this.cmbBusinessUnit.Name = "cmbBusinessUnit";
            this.cmbBusinessUnit.Size = new System.Drawing.Size(276, 21);
            this.cmbBusinessUnit.TabIndex = 1;
            this.cmbBusinessUnit.ValueMember = "ID";
            // 
            // businessUnitBindingSource
            // 
            this.businessUnitBindingSource.DataSource = typeof(Uheaa.Common.ProcessLogger.BusinessUnit);
            // 
            // cmbUser
            // 
            this.cmbUser.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbUser.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbUser.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.reporterBindingSource, "User", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.Location = new System.Drawing.Point(172, 16);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(276, 21);
            this.cmbUser.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.reporterBindingSource, "PhoneNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox2.Location = new System.Drawing.Point(172, 84);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(276, 20);
            this.textBox2.TabIndex = 3;
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.Location = new System.Drawing.Point(172, 62);
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(276, 20);
            this.txtEmailAddress.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Phone Number";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "E-mail Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Business Unit";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Name";
            // 
            // IncidentReporterDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "IncidentReporterDetail";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.incidentBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporterBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessUnitBindingSource)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cmbUser;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox txtEmailAddress;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBusinessUnit;
		private System.Windows.Forms.BindingSource reporterBindingSource;
		private System.Windows.Forms.BindingSource businessUnitBindingSource;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbPriority;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.BindingSource incidentBindingSource;
	}
}