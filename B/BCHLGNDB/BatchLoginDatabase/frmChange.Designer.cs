namespace BatchLoginDatabase
{
	partial class frmChange
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
			this.lboSelectedIds = new System.Windows.Forms.ListBox();
			this.userIdsAndPasswordsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.txtCurrentPassword = new System.Windows.Forms.TextBox();
			this.txtNewPassword = new System.Windows.Forms.TextBox();
			this.txtConfirmPassword = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnSaveAll = new System.Windows.Forms.Button();
			this.lblCurrentPassword = new System.Windows.Forms.Label();
			this.lblNewPassword = new System.Windows.Forms.Label();
			this.lblConfirmPassword = new System.Windows.Forms.Label();
			this.lboChangedIds = new System.Windows.Forms.ListBox();
			this.userIdsAndPasswordsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
			this.lblGeneralDirections = new System.Windows.Forms.Label();
			this.txtNotes = new System.Windows.Forms.TextBox();
			this.lblNotes = new System.Windows.Forms.Label();
			this.lblChangedIds = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.userIdsAndPasswordsBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.userIdsAndPasswordsBindingSource1)).BeginInit();
			this.SuspendLayout();
			// 
			// lboSelectedIds
			// 
			this.lboSelectedIds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lboSelectedIds.DataSource = this.userIdsAndPasswordsBindingSource;
			this.lboSelectedIds.DisplayMember = "UserNameId";
			this.lboSelectedIds.FormattingEnabled = true;
			this.lboSelectedIds.Location = new System.Drawing.Point(12, 77);
			this.lboSelectedIds.Name = "lboSelectedIds";
			this.lboSelectedIds.Size = new System.Drawing.Size(253, 457);
			this.lboSelectedIds.TabIndex = 0;
			// 
			// userIdsAndPasswordsBindingSource
			// 
			this.userIdsAndPasswordsBindingSource.DataSource = typeof(BatchLoginDatabase.UserIdsAndPasswords);
			// 
			// txtCurrentPassword
			// 
			this.txtCurrentPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtCurrentPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.userIdsAndPasswordsBindingSource, "CurrentPassword", true));
			this.txtCurrentPassword.Location = new System.Drawing.Point(303, 74);
			this.txtCurrentPassword.Name = "txtCurrentPassword";
			this.txtCurrentPassword.Size = new System.Drawing.Size(100, 20);
			this.txtCurrentPassword.TabIndex = 1;
			this.txtCurrentPassword.UseSystemPasswordChar = true;
			// 
			// txtNewPassword
			// 
			this.txtNewPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtNewPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.userIdsAndPasswordsBindingSource, "NewPassword", true));
			this.txtNewPassword.Location = new System.Drawing.Point(420, 74);
			this.txtNewPassword.Name = "txtNewPassword";
			this.txtNewPassword.Size = new System.Drawing.Size(100, 20);
			this.txtNewPassword.TabIndex = 2;
			this.txtNewPassword.UseSystemPasswordChar = true;
			// 
			// txtConfirmPassword
			// 
			this.txtConfirmPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtConfirmPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.userIdsAndPasswordsBindingSource, "ConfirmPassword", true));
			this.txtConfirmPassword.Location = new System.Drawing.Point(536, 74);
			this.txtConfirmPassword.Name = "txtConfirmPassword";
			this.txtConfirmPassword.Size = new System.Drawing.Size(100, 20);
			this.txtConfirmPassword.TabIndex = 3;
			this.txtConfirmPassword.UseSystemPasswordChar = true;
			// 
			// btnAdd
			// 
			this.btnAdd.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnAdd.Location = new System.Drawing.Point(536, 170);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(96, 35);
			this.btnAdd.TabIndex = 5;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = false;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnSaveAll
			// 
			this.btnSaveAll.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnSaveAll.Location = new System.Drawing.Point(536, 458);
			this.btnSaveAll.Name = "btnSaveAll";
			this.btnSaveAll.Size = new System.Drawing.Size(96, 35);
			this.btnSaveAll.TabIndex = 6;
			this.btnSaveAll.Text = "Save All";
			this.btnSaveAll.UseVisualStyleBackColor = false;
			this.btnSaveAll.Visible = false;
			this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
			// 
			// lblCurrentPassword
			// 
			this.lblCurrentPassword.AutoSize = true;
			this.lblCurrentPassword.Location = new System.Drawing.Point(300, 49);
			this.lblCurrentPassword.Name = "lblCurrentPassword";
			this.lblCurrentPassword.Size = new System.Drawing.Size(90, 13);
			this.lblCurrentPassword.TabIndex = 7;
			this.lblCurrentPassword.Text = "Current Password";
			// 
			// lblNewPassword
			// 
			this.lblNewPassword.AutoSize = true;
			this.lblNewPassword.Location = new System.Drawing.Point(417, 49);
			this.lblNewPassword.Name = "lblNewPassword";
			this.lblNewPassword.Size = new System.Drawing.Size(78, 13);
			this.lblNewPassword.TabIndex = 8;
			this.lblNewPassword.Text = "New Password";
			// 
			// lblConfirmPassword
			// 
			this.lblConfirmPassword.AutoSize = true;
			this.lblConfirmPassword.Location = new System.Drawing.Point(533, 49);
			this.lblConfirmPassword.Name = "lblConfirmPassword";
			this.lblConfirmPassword.Size = new System.Drawing.Size(91, 13);
			this.lblConfirmPassword.TabIndex = 9;
			this.lblConfirmPassword.Text = "Confirm Password";
			// 
			// lboChangedIds
			// 
			this.lboChangedIds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lboChangedIds.DataSource = this.userIdsAndPasswordsBindingSource1;
			this.lboChangedIds.DisplayMember = "UserNameId";
			this.lboChangedIds.FormattingEnabled = true;
			this.lboChangedIds.Location = new System.Drawing.Point(303, 285);
			this.lboChangedIds.Name = "lboChangedIds";
			this.lboChangedIds.Size = new System.Drawing.Size(217, 249);
			this.lboChangedIds.TabIndex = 10;
			// 
			// userIdsAndPasswordsBindingSource1
			// 
			this.userIdsAndPasswordsBindingSource1.DataSource = typeof(BatchLoginDatabase.UserIdsAndPasswords);
			// 
			// lblGeneralDirections
			// 
			this.lblGeneralDirections.Location = new System.Drawing.Point(12, 13);
			this.lblGeneralDirections.Name = "lblGeneralDirections";
			this.lblGeneralDirections.Size = new System.Drawing.Size(270, 61);
			this.lblGeneralDirections.TabIndex = 11;
			this.lblGeneralDirections.Text = "Select the user id you wish to change, enter the current password, new password, " +
				"and confirm the new password and select add.  Once all ids have been updated sel" +
				"ect save all";
			// 
			// txtNotes
			// 
			this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.userIdsAndPasswordsBindingSource, "Notes", true));
			this.txtNotes.Location = new System.Drawing.Point(303, 134);
			this.txtNotes.MaxLength = 100;
			this.txtNotes.Multiline = true;
			this.txtNotes.Name = "txtNotes";
			this.txtNotes.Size = new System.Drawing.Size(217, 105);
			this.txtNotes.TabIndex = 4;
			// 
			// lblNotes
			// 
			this.lblNotes.AutoSize = true;
			this.lblNotes.Location = new System.Drawing.Point(300, 109);
			this.lblNotes.Name = "lblNotes";
			this.lblNotes.Size = new System.Drawing.Size(139, 13);
			this.lblNotes.TabIndex = 13;
			this.lblNotes.Text = "Notes (Limit 100 characters)";
			// 
			// lblChangedIds
			// 
			this.lblChangedIds.AutoSize = true;
			this.lblChangedIds.Location = new System.Drawing.Point(300, 259);
			this.lblChangedIds.Name = "lblChangedIds";
			this.lblChangedIds.Size = new System.Drawing.Size(67, 13);
			this.lblChangedIds.TabIndex = 14;
			this.lblChangedIds.Text = "Changed Ids";
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnCancel.Location = new System.Drawing.Point(536, 499);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(96, 35);
			this.btnCancel.TabIndex = 15;
			this.btnCancel.Text = "Cencel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// frmChange
			// 
			this.AcceptButton = this.btnAdd;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(644, 553);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblChangedIds);
			this.Controls.Add(this.lblNotes);
			this.Controls.Add(this.txtNotes);
			this.Controls.Add(this.lblGeneralDirections);
			this.Controls.Add(this.lboChangedIds);
			this.Controls.Add(this.lblConfirmPassword);
			this.Controls.Add(this.lblNewPassword);
			this.Controls.Add(this.lblCurrentPassword);
			this.Controls.Add(this.btnSaveAll);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.txtConfirmPassword);
			this.Controls.Add(this.txtNewPassword);
			this.Controls.Add(this.txtCurrentPassword);
			this.Controls.Add(this.lboSelectedIds);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmChange";
			this.ShowIcon = false;
			this.Text = "Change Passwords";
			((System.ComponentModel.ISupportInitialize)(this.userIdsAndPasswordsBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.userIdsAndPasswordsBindingSource1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lboSelectedIds;
		private System.Windows.Forms.TextBox txtCurrentPassword;
		private System.Windows.Forms.TextBox txtNewPassword;
		private System.Windows.Forms.TextBox txtConfirmPassword;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnSaveAll;
		private System.Windows.Forms.Label lblCurrentPassword;
		private System.Windows.Forms.Label lblNewPassword;
		private System.Windows.Forms.Label lblConfirmPassword;
		private System.Windows.Forms.ListBox lboChangedIds;
		private System.Windows.Forms.Label lblGeneralDirections;
		private System.Windows.Forms.TextBox txtNotes;
		private System.Windows.Forms.Label lblNotes;
		private System.Windows.Forms.Label lblChangedIds;
		private System.Windows.Forms.BindingSource userIdsAndPasswordsBindingSource;
		private System.Windows.Forms.BindingSource userIdsAndPasswordsBindingSource1;
		private System.Windows.Forms.Button btnCancel;
	}
}