namespace INCIDENTRP
{
	partial class IncidentDetail
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.incidentBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblOtherRelationship = new System.Windows.Forms.Label();
            this.txtOtherRelationship = new System.Windows.Forms.TextBox();
            this.notifierBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbRelationship = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblOtherTypeDescription = new System.Windows.Forms.Label();
            this.txtOtherTypeDescription = new System.Windows.Forms.TextBox();
            this.lblOtherMethodDescription = new System.Windows.Forms.Label();
            this.txtOtherMethodDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFunctionalArea = new System.Windows.Forms.ComboBox();
            this.ticketBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.IncidentDate = new System.Windows.Forms.DateTimePicker();
            this.cmbNotifierType = new System.Windows.Forms.ComboBox();
            this.cmbNotificationMethod = new System.Windows.Forms.ComboBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.incidentBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.notifierBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.lblOtherRelationship);
            this.groupBox1.Controls.Add(this.txtOtherRelationship);
            this.groupBox1.Controls.Add(this.cmbRelationship);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lblOtherTypeDescription);
            this.groupBox1.Controls.Add(this.txtOtherTypeDescription);
            this.groupBox1.Controls.Add(this.lblOtherMethodDescription);
            this.groupBox1.Controls.Add(this.txtOtherMethodDescription);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbFunctionalArea);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.IncidentDate);
            this.groupBox1.Controls.Add(this.cmbNotifierType);
            this.groupBox1.Controls.Add(this.cmbNotificationMethod);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(454, 293);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Incident Detail";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Physical Location of the Incident";
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.incidentBindingSource, "Location", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox1.Location = new System.Drawing.Point(174, 76);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(274, 20);
            this.textBox1.TabIndex = 3;
            // 
            // incidentBindingSource
            // 
            this.incidentBindingSource.DataSource = typeof(INCIDENTRP.Incident);
            // 
            // lblOtherRelationship
            // 
            this.lblOtherRelationship.AutoSize = true;
            this.lblOtherRelationship.Location = new System.Drawing.Point(171, 271);
            this.lblOtherRelationship.Name = "lblOtherRelationship";
            this.lblOtherRelationship.Size = new System.Drawing.Size(60, 13);
            this.lblOtherRelationship.TabIndex = 27;
            this.lblOtherRelationship.Text = "Description";
            // 
            // txtOtherRelationship
            // 
            this.txtOtherRelationship.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "OtherRelationship", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOtherRelationship.Location = new System.Drawing.Point(237, 268);
            this.txtOtherRelationship.Name = "txtOtherRelationship";
            this.txtOtherRelationship.Size = new System.Drawing.Size(211, 20);
            this.txtOtherRelationship.TabIndex = 12;
            // 
            // notifierBindingSource
            // 
            this.notifierBindingSource.DataSource = typeof(INCIDENTRP.Notifier);
            // 
            // cmbRelationship
            // 
            this.cmbRelationship.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbRelationship.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbRelationship.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "Relationship", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbRelationship.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRelationship.FormattingEnabled = true;
            this.cmbRelationship.Location = new System.Drawing.Point(174, 246);
            this.cmbRelationship.Name = "cmbRelationship";
            this.cmbRelationship.Size = new System.Drawing.Size(274, 21);
            this.cmbRelationship.TabIndex = 11;
            this.cmbRelationship.SelectedIndexChanged += new System.EventHandler(this.cmbRelationship_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 249);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Notifier Relationship";
            // 
            // lblOtherTypeDescription
            // 
            this.lblOtherTypeDescription.AutoSize = true;
            this.lblOtherTypeDescription.Location = new System.Drawing.Point(171, 165);
            this.lblOtherTypeDescription.Name = "lblOtherTypeDescription";
            this.lblOtherTypeDescription.Size = new System.Drawing.Size(60, 13);
            this.lblOtherTypeDescription.TabIndex = 23;
            this.lblOtherTypeDescription.Text = "Description";
            // 
            // txtOtherTypeDescription
            // 
            this.txtOtherTypeDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "OtherType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOtherTypeDescription.Location = new System.Drawing.Point(237, 162);
            this.txtOtherTypeDescription.Name = "txtOtherTypeDescription";
            this.txtOtherTypeDescription.Size = new System.Drawing.Size(211, 20);
            this.txtOtherTypeDescription.TabIndex = 7;
            // 
            // lblOtherMethodDescription
            // 
            this.lblOtherMethodDescription.AutoSize = true;
            this.lblOtherMethodDescription.Location = new System.Drawing.Point(171, 122);
            this.lblOtherMethodDescription.Name = "lblOtherMethodDescription";
            this.lblOtherMethodDescription.Size = new System.Drawing.Size(60, 13);
            this.lblOtherMethodDescription.TabIndex = 21;
            this.lblOtherMethodDescription.Text = "Description";
            // 
            // txtOtherMethodDescription
            // 
            this.txtOtherMethodDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "OtherMethod", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOtherMethodDescription.Location = new System.Drawing.Point(237, 119);
            this.txtOtherMethodDescription.Name = "txtOtherMethodDescription";
            this.txtOtherMethodDescription.Size = new System.Drawing.Size(211, 20);
            this.txtOtherMethodDescription.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Functional Area";
            // 
            // cmbFunctionalArea
            // 
            this.cmbFunctionalArea.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbFunctionalArea.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFunctionalArea.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketBindingSource, "FunctionalArea", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbFunctionalArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFunctionalArea.FormattingEnabled = true;
            this.cmbFunctionalArea.Location = new System.Drawing.Point(174, 12);
            this.cmbFunctionalArea.Name = "cmbFunctionalArea";
            this.cmbFunctionalArea.Size = new System.Drawing.Size(274, 21);
            this.cmbFunctionalArea.TabIndex = 0;
            // 
            // ticketBindingSource
            // 
            this.ticketBindingSource.DataSource = typeof(INCIDENTRP.Ticket);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "hh:mm tt";
            this.dateTimePicker2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketBindingSource, "IncidentDateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(174, 55);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.ShowUpDown = true;
            this.dateTimePicker2.Size = new System.Drawing.Size(274, 20);
            this.dateTimePicker2.TabIndex = 2;
            // 
            // IncidentDate
            // 
            this.IncidentDate.CustomFormat = "MM/dd/yyyy";
            this.IncidentDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketBindingSource, "IncidentDateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IncidentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.IncidentDate.Location = new System.Drawing.Point(174, 34);
            this.IncidentDate.Name = "IncidentDate";
            this.IncidentDate.Size = new System.Drawing.Size(274, 20);
            this.IncidentDate.TabIndex = 1;
            // 
            // cmbNotifierType
            // 
            this.cmbNotifierType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbNotifierType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbNotifierType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "Type", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbNotifierType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNotifierType.FormattingEnabled = true;
            this.cmbNotifierType.Location = new System.Drawing.Point(174, 140);
            this.cmbNotifierType.Name = "cmbNotifierType";
            this.cmbNotifierType.Size = new System.Drawing.Size(274, 21);
            this.cmbNotifierType.TabIndex = 6;
            this.cmbNotifierType.SelectedIndexChanged += new System.EventHandler(this.cmbNotifierType_SelectedIndexChanged);
            // 
            // cmbNotificationMethod
            // 
            this.cmbNotificationMethod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbNotificationMethod.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbNotificationMethod.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "Method", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbNotificationMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNotificationMethod.FormattingEnabled = true;
            this.cmbNotificationMethod.Location = new System.Drawing.Point(174, 97);
            this.cmbNotificationMethod.Name = "cmbNotificationMethod";
            this.cmbNotificationMethod.Size = new System.Drawing.Size(274, 21);
            this.cmbNotificationMethod.TabIndex = 4;
            this.cmbNotificationMethod.SelectedIndexChanged += new System.EventHandler(this.cmbNotificationMethod_SelectedIndexChanged);
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "PhoneNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox4.Location = new System.Drawing.Point(174, 225);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(274, 20);
            this.textBox4.TabIndex = 10;
            // 
            // textBox3
            // 
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "EmailAddress", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox3.Location = new System.Drawing.Point(174, 204);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(274, 20);
            this.textBox3.TabIndex = 9;
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox2.Location = new System.Drawing.Point(174, 183);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(274, 20);
            this.textBox2.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 228);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Notifier Phone Number";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 206);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Notifier E-mail Address";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 186);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Notifier Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Notified By";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Notifier Method";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Incident Time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Incident Date";
            // 
            // IncidentDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "IncidentDetail";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.incidentBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.notifierBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketBindingSource)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DateTimePicker dateTimePicker2;
		private System.Windows.Forms.DateTimePicker IncidentDate;
		private System.Windows.Forms.ComboBox cmbNotifierType;
		private System.Windows.Forms.ComboBox cmbNotificationMethod;
		private System.Windows.Forms.BindingSource ticketBindingSource;
		private System.Windows.Forms.BindingSource notifierBindingSource;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbFunctionalArea;
		private System.Windows.Forms.TextBox txtOtherMethodDescription;
		private System.Windows.Forms.Label lblOtherTypeDescription;
		private System.Windows.Forms.TextBox txtOtherTypeDescription;
		private System.Windows.Forms.Label lblOtherMethodDescription;
		private System.Windows.Forms.Label lblOtherRelationship;
		private System.Windows.Forms.TextBox txtOtherRelationship;
		private System.Windows.Forms.ComboBox cmbRelationship;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.BindingSource incidentBindingSource;
	}
}
