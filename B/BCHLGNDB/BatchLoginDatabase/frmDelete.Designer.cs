namespace BatchLoginDatabase
{
	partial class frmDelete
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
			this.components = new System.ComponentModel.Container();
			this.lsbSelectedIds = new System.Windows.Forms.ListBox();
			this.userIdsAndPasswordsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.txtCurrentPassword = new System.Windows.Forms.TextBox();
			this.lblGeneralInstructions = new System.Windows.Forms.Label();
			this.lblCurrentPassword = new System.Windows.Forms.Label();
			this.btnAdd = new System.Windows.Forms.Button();
			this.lboIdsToDelete = new System.Windows.Forms.ListBox();
			this.userIdsAndPasswordsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
			this.lblIdsToDelete = new System.Windows.Forms.Label();
			this.btnSaveAll = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.userIdsAndPasswordsBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.userIdsAndPasswordsBindingSource1)).BeginInit();
			this.SuspendLayout();
			// 
			// lsbSelectedIds
			// 
			this.lsbSelectedIds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lsbSelectedIds.DataSource = this.userIdsAndPasswordsBindingSource;
			this.lsbSelectedIds.DisplayMember = "UserNameId";
			this.lsbSelectedIds.FormattingEnabled = true;
			this.lsbSelectedIds.Location = new System.Drawing.Point(15, 77);
			this.lsbSelectedIds.Name = "lsbSelectedIds";
			this.lsbSelectedIds.Size = new System.Drawing.Size(238, 405);
			this.lsbSelectedIds.TabIndex = 0;
			// 
			// userIdsAndPasswordsBindingSource
			// 
			this.userIdsAndPasswordsBindingSource.DataSource = typeof(BatchLoginDatabase.UserIdsAndPasswords);
			// 
			// txtCurrentPassword
			// 
			this.txtCurrentPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtCurrentPassword.Location = new System.Drawing.Point(273, 93);
			this.txtCurrentPassword.Name = "txtCurrentPassword";
			this.txtCurrentPassword.Size = new System.Drawing.Size(100, 20);
			this.txtCurrentPassword.TabIndex = 1;
			this.txtCurrentPassword.UseSystemPasswordChar = true;
			// 
			// lblGeneralInstructions
			// 
			this.lblGeneralInstructions.Location = new System.Drawing.Point(12, 9);
			this.lblGeneralInstructions.Name = "lblGeneralInstructions";
			this.lblGeneralInstructions.Size = new System.Drawing.Size(244, 52);
			this.lblGeneralInstructions.TabIndex = 2;
			this.lblGeneralInstructions.Text = "Select the user id you wish to delete, enter the current password and select add." +
				"  Once all ids have been enterd select save all to commit the changes.\r\n";
			// 
			// lblCurrentPassword
			// 
			this.lblCurrentPassword.AutoSize = true;
			this.lblCurrentPassword.Location = new System.Drawing.Point(270, 73);
			this.lblCurrentPassword.Name = "lblCurrentPassword";
			this.lblCurrentPassword.Size = new System.Drawing.Size(90, 13);
			this.lblCurrentPassword.TabIndex = 3;
			this.lblCurrentPassword.Text = "Current Password";
			// 
			// btnAdd
			// 
			this.btnAdd.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnAdd.Location = new System.Drawing.Point(395, 90);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 4;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = false;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// lboIdsToDelete
			// 
			this.lboIdsToDelete.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lboIdsToDelete.DataSource = this.userIdsAndPasswordsBindingSource1;
			this.lboIdsToDelete.DisplayMember = "UserNameId";
			this.lboIdsToDelete.FormattingEnabled = true;
			this.lboIdsToDelete.Location = new System.Drawing.Point(273, 160);
			this.lboIdsToDelete.Name = "lboIdsToDelete";
			this.lboIdsToDelete.Size = new System.Drawing.Size(222, 262);
			this.lboIdsToDelete.TabIndex = 5;
			// 
			// userIdsAndPasswordsBindingSource1
			// 
			this.userIdsAndPasswordsBindingSource1.DataSource = typeof(BatchLoginDatabase.UserIdsAndPasswords);
			// 
			// lblIdsToDelete
			// 
			this.lblIdsToDelete.AutoSize = true;
			this.lblIdsToDelete.Location = new System.Drawing.Point(270, 140);
			this.lblIdsToDelete.Name = "lblIdsToDelete";
			this.lblIdsToDelete.Size = new System.Drawing.Size(166, 13);
			this.lblIdsToDelete.TabIndex = 6;
			this.lblIdsToDelete.Text = "The Following Ids will be deleted: ";
			// 
			// btnSaveAll
			// 
			this.btnSaveAll.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnSaveAll.Location = new System.Drawing.Point(273, 444);
			this.btnSaveAll.Name = "btnSaveAll";
			this.btnSaveAll.Size = new System.Drawing.Size(100, 40);
			this.btnSaveAll.TabIndex = 7;
			this.btnSaveAll.Text = "Save All";
			this.btnSaveAll.UseVisualStyleBackColor = false;
			this.btnSaveAll.Visible = false;
			this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnCancel.Location = new System.Drawing.Point(395, 444);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 40);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// frmDelete
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(510, 497);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSaveAll);
			this.Controls.Add(this.lblIdsToDelete);
			this.Controls.Add(this.lboIdsToDelete);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.lblCurrentPassword);
			this.Controls.Add(this.lblGeneralInstructions);
			this.Controls.Add(this.txtCurrentPassword);
			this.Controls.Add(this.lsbSelectedIds);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmDelete";
			this.ShowIcon = false;
			this.Text = "Delete";
			((System.ComponentModel.ISupportInitialize)(this.userIdsAndPasswordsBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.userIdsAndPasswordsBindingSource1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lsbSelectedIds;
		private System.Windows.Forms.TextBox txtCurrentPassword;
		private System.Windows.Forms.Label lblGeneralInstructions;
		private System.Windows.Forms.Label lblCurrentPassword;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ListBox lboIdsToDelete;
		private System.Windows.Forms.Label lblIdsToDelete;
		private System.Windows.Forms.Button btnSaveAll;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.BindingSource userIdsAndPasswordsBindingSource;
		private System.Windows.Forms.BindingSource userIdsAndPasswordsBindingSource1;
	}
}