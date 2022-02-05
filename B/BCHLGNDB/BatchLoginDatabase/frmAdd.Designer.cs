namespace BatchLoginDatabase
{
	partial class frmAdd
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
			this.txtUserId = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtConfirmPassword = new System.Windows.Forms.TextBox();
			this.txtNotes = new System.Windows.Forms.TextBox();
			this.lblUserId = new System.Windows.Forms.Label();
			this.lblPassword = new System.Windows.Forms.Label();
			this.lblConfirmPassword = new System.Windows.Forms.Label();
			this.lblNotes = new System.Windows.Forms.Label();
			this.btnAdd = new System.Windows.Forms.Button();
			this.lsbidsToCHange = new System.Windows.Forms.ListBox();
			this.userIdsAndPasswordsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.lblIdsToAdd = new System.Windows.Forms.Label();
			this.btnSaveAll = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.userIdsAndPasswordsBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// txtUserId
			// 
			this.txtUserId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtUserId.Location = new System.Drawing.Point(15, 45);
			this.txtUserId.MaxLength = 50;
			this.txtUserId.Name = "txtUserId";
			this.txtUserId.Size = new System.Drawing.Size(100, 20);
			this.txtUserId.TabIndex = 0;
			// 
			// txtPassword
			// 
			this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPassword.Location = new System.Drawing.Point(131, 45);
			this.txtPassword.MaxLength = 50;
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(100, 20);
			this.txtPassword.TabIndex = 1;
			this.txtPassword.UseSystemPasswordChar = true;
			// 
			// txtConfirmPassword
			// 
			this.txtConfirmPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtConfirmPassword.Location = new System.Drawing.Point(249, 45);
			this.txtConfirmPassword.MaxLength = 50;
			this.txtConfirmPassword.Name = "txtConfirmPassword";
			this.txtConfirmPassword.Size = new System.Drawing.Size(100, 20);
			this.txtConfirmPassword.TabIndex = 2;
			this.txtConfirmPassword.UseSystemPasswordChar = true;
			// 
			// txtNotes
			// 
			this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtNotes.Location = new System.Drawing.Point(15, 114);
			this.txtNotes.MaxLength = 100;
			this.txtNotes.Multiline = true;
			this.txtNotes.Name = "txtNotes";
			this.txtNotes.Size = new System.Drawing.Size(334, 79);
			this.txtNotes.TabIndex = 3;
			// 
			// lblUserId
			// 
			this.lblUserId.AutoSize = true;
			this.lblUserId.Location = new System.Drawing.Point(12, 26);
			this.lblUserId.Name = "lblUserId";
			this.lblUserId.Size = new System.Drawing.Size(41, 13);
			this.lblUserId.TabIndex = 4;
			this.lblUserId.Text = "User Id";
			// 
			// lblPassword
			// 
			this.lblPassword.AutoSize = true;
			this.lblPassword.Location = new System.Drawing.Point(128, 26);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(53, 13);
			this.lblPassword.TabIndex = 5;
			this.lblPassword.Text = "Password";
			// 
			// lblConfirmPassword
			// 
			this.lblConfirmPassword.AutoSize = true;
			this.lblConfirmPassword.Location = new System.Drawing.Point(246, 26);
			this.lblConfirmPassword.Name = "lblConfirmPassword";
			this.lblConfirmPassword.Size = new System.Drawing.Size(91, 13);
			this.lblConfirmPassword.TabIndex = 6;
			this.lblConfirmPassword.Text = "Confirm Password";
			// 
			// lblNotes
			// 
			this.lblNotes.AutoSize = true;
			this.lblNotes.Location = new System.Drawing.Point(12, 95);
			this.lblNotes.Name = "lblNotes";
			this.lblNotes.Size = new System.Drawing.Size(35, 13);
			this.lblNotes.TabIndex = 7;
			this.lblNotes.Text = "Notes";
			// 
			// btnAdd
			// 
			this.btnAdd.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnAdd.Location = new System.Drawing.Point(268, 213);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(81, 40);
			this.btnAdd.TabIndex = 4;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = false;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// lsbidsToCHange
			// 
			this.lsbidsToCHange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lsbidsToCHange.DataSource = this.userIdsAndPasswordsBindingSource;
			this.lsbidsToCHange.DisplayMember = "UserNameId";
			this.lsbidsToCHange.FormattingEnabled = true;
			this.lsbidsToCHange.Location = new System.Drawing.Point(15, 305);
			this.lsbidsToCHange.Name = "lsbidsToCHange";
			this.lsbidsToCHange.Size = new System.Drawing.Size(334, 119);
			this.lsbidsToCHange.TabIndex = 9;
			// 
			// userIdsAndPasswordsBindingSource
			// 
			this.userIdsAndPasswordsBindingSource.DataSource = typeof(BatchLoginDatabase.UserIdsAndPasswords);
			// 
			// lblIdsToAdd
			// 
			this.lblIdsToAdd.AutoSize = true;
			this.lblIdsToAdd.Location = new System.Drawing.Point(12, 286);
			this.lblIdsToAdd.Name = "lblIdsToAdd";
			this.lblIdsToAdd.Size = new System.Drawing.Size(183, 13);
			this.lblIdsToAdd.TabIndex = 10;
			this.lblIdsToAdd.Text = "The following User Ids will be added: ";
			// 
			// btnSaveAll
			// 
			this.btnSaveAll.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnSaveAll.Location = new System.Drawing.Point(84, 447);
			this.btnSaveAll.Name = "btnSaveAll";
			this.btnSaveAll.Size = new System.Drawing.Size(117, 39);
			this.btnSaveAll.TabIndex = 11;
			this.btnSaveAll.Text = "Save All";
			this.btnSaveAll.UseVisualStyleBackColor = false;
			this.btnSaveAll.Visible = false;
			this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnCancel.Location = new System.Drawing.Point(232, 447);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(117, 39);
			this.btnCancel.TabIndex = 12;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// frmAdd
			// 
			this.AcceptButton = this.btnAdd;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(372, 503);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSaveAll);
			this.Controls.Add(this.lblIdsToAdd);
			this.Controls.Add(this.lsbidsToCHange);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.lblNotes);
			this.Controls.Add(this.lblConfirmPassword);
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.lblUserId);
			this.Controls.Add(this.txtNotes);
			this.Controls.Add(this.txtConfirmPassword);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.txtUserId);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmAdd";
			this.ShowIcon = false;
			this.Text = "Add";
			((System.ComponentModel.ISupportInitialize)(this.userIdsAndPasswordsBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtUserId;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtConfirmPassword;
		private System.Windows.Forms.TextBox txtNotes;
		private System.Windows.Forms.Label lblUserId;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.Label lblConfirmPassword;
		private System.Windows.Forms.Label lblNotes;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ListBox lsbidsToCHange;
		private System.Windows.Forms.Label lblIdsToAdd;
		private System.Windows.Forms.Button btnSaveAll;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.BindingSource userIdsAndPasswordsBindingSource;
	}
}