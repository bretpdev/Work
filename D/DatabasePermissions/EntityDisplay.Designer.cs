namespace DatabasePermissions
{
	partial class EntityDisplay
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
			this.chkExecute = new System.Windows.Forms.CheckBox();
			this.chkSelect = new System.Windows.Forms.CheckBox();
			this.chkDelete = new System.Windows.Forms.CheckBox();
			this.chkUpdate = new System.Windows.Forms.CheckBox();
			this.chkInsert = new System.Windows.Forms.CheckBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.entityBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.entityBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkExecute);
			this.groupBox1.Controls.Add(this.chkSelect);
			this.groupBox1.Controls.Add(this.chkDelete);
			this.groupBox1.Controls.Add(this.chkUpdate);
			this.groupBox1.Controls.Add(this.chkInsert);
			this.groupBox1.Controls.Add(this.txtName);
			this.groupBox1.Location = new System.Drawing.Point(0, -4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(542, 35);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// chkExecute
			// 
			this.chkExecute.AutoSize = true;
			this.chkExecute.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.entityBindingSource, "Execute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.chkExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkExecute.Location = new System.Drawing.Point(509, 13);
			this.chkExecute.Name = "chkExecute";
			this.chkExecute.Size = new System.Drawing.Size(15, 14);
			this.chkExecute.TabIndex = 5;
			this.chkExecute.UseVisualStyleBackColor = true;
			// 
			// chkSelect
			// 
			this.chkSelect.AutoSize = true;
			this.chkSelect.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.entityBindingSource, "Select", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.chkSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkSelect.Location = new System.Drawing.Point(469, 13);
			this.chkSelect.Name = "chkSelect";
			this.chkSelect.Size = new System.Drawing.Size(15, 14);
			this.chkSelect.TabIndex = 4;
			this.chkSelect.UseVisualStyleBackColor = true;
			// 
			// chkDelete
			// 
			this.chkDelete.AutoSize = true;
			this.chkDelete.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.entityBindingSource, "Delete", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.chkDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkDelete.Location = new System.Drawing.Point(429, 13);
			this.chkDelete.Name = "chkDelete";
			this.chkDelete.Size = new System.Drawing.Size(15, 14);
			this.chkDelete.TabIndex = 3;
			this.chkDelete.UseVisualStyleBackColor = true;
			// 
			// chkUpdate
			// 
			this.chkUpdate.AutoSize = true;
			this.chkUpdate.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.entityBindingSource, "Update", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.chkUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkUpdate.Location = new System.Drawing.Point(389, 13);
			this.chkUpdate.Name = "chkUpdate";
			this.chkUpdate.Size = new System.Drawing.Size(15, 14);
			this.chkUpdate.TabIndex = 2;
			this.chkUpdate.UseVisualStyleBackColor = true;
			// 
			// chkInsert
			// 
			this.chkInsert.AutoSize = true;
			this.chkInsert.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.entityBindingSource, "Insert", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.chkInsert.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkInsert.Location = new System.Drawing.Point(349, 13);
			this.chkInsert.Name = "chkInsert";
			this.chkInsert.Size = new System.Drawing.Size(15, 14);
			this.chkInsert.TabIndex = 1;
			this.chkInsert.UseVisualStyleBackColor = true;
			// 
			// txtName
			// 
			this.txtName.BackColor = System.Drawing.Color.White;
			this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.entityBindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtName.Location = new System.Drawing.Point(4, 10);
			this.txtName.Name = "txtName";
			this.txtName.ReadOnly = true;
			this.txtName.Size = new System.Drawing.Size(320, 20);
			this.txtName.TabIndex = 0;
			// 
			// entityBindingSource
			// 
			this.entityBindingSource.DataSource = typeof(DatabasePermissions.Entity);
			// 
			// EntityDisplay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "EntityDisplay";
			this.Size = new System.Drawing.Size(542, 30);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.entityBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkInsert;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.CheckBox chkSelect;
		private System.Windows.Forms.CheckBox chkDelete;
		private System.Windows.Forms.CheckBox chkUpdate;
		private System.Windows.Forms.CheckBox chkExecute;
		private System.Windows.Forms.BindingSource entityBindingSource;
	}
}
