namespace BatchLoginDatabase
{
	partial class frmBatchLogin
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
			this.lblUserId = new System.Windows.Forms.Label();
			this.lblPassword = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.loginCredentaisBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.loginCredentaisBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// txtUserId
			// 
			this.txtUserId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.loginCredentaisBindingSource, "UserId", true));
			this.txtUserId.Location = new System.Drawing.Point(74, 6);
			this.txtUserId.Name = "txtUserId";
			this.txtUserId.Size = new System.Drawing.Size(108, 20);
			this.txtUserId.TabIndex = 0;
			// 
			// txtPassword
			// 
			this.txtPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.loginCredentaisBindingSource, "Password", true));
			this.txtPassword.Location = new System.Drawing.Point(74, 42);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(108, 20);
			this.txtPassword.TabIndex = 1;
			this.txtPassword.UseSystemPasswordChar = true;
			// 
			// lblUserId
			// 
			this.lblUserId.AutoSize = true;
			this.lblUserId.Location = new System.Drawing.Point(12, 9);
			this.lblUserId.Name = "lblUserId";
			this.lblUserId.Size = new System.Drawing.Size(41, 13);
			this.lblUserId.TabIndex = 2;
			this.lblUserId.Text = "User Id";
			// 
			// lblPassword
			// 
			this.lblPassword.AutoSize = true;
			this.lblPassword.Location = new System.Drawing.Point(12, 42);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(53, 13);
			this.lblPassword.TabIndex = 3;
			this.lblPassword.Text = "Password";
			// 
			// btnOK
			// 
			this.btnOK.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnOK.Location = new System.Drawing.Point(12, 82);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(107, 82);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// loginCredentaisBindingSource
			// 
			this.loginCredentaisBindingSource.DataSource = typeof(BatchLoginDatabase.LoginCredentais);
			// 
			// frmBatchLogin
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(194, 111);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.lblUserId);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.txtUserId);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmBatchLogin";
			this.ShowIcon = false;
			this.Text = "Login";
			((System.ComponentModel.ISupportInitialize)(this.loginCredentaisBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtUserId;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblUserId;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.BindingSource loginCredentaisBindingSource;
	}
}