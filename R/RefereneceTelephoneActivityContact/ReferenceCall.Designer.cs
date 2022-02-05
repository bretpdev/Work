namespace RefereneceTelephoneActivityContact
{
	partial class ReferenceCall
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReferenceCall));
			this.txtBorrowerId = new System.Windows.Forms.TextBox();
			this.contactDetailBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.txtReferenceId = new System.Windows.Forms.TextBox();
			this.lblBorrowerId = new System.Windows.Forms.Label();
			this.lblReferenceId = new System.Windows.Forms.Label();
			this.cmbContactResult = new System.Windows.Forms.ComboBox();
			this.lblContactResult = new System.Windows.Forms.Label();
			this.lblReferenceResult = new System.Windows.Forms.Label();
			this.cmbReferenceResult = new System.Windows.Forms.ComboBox();
			this.txtComment = new System.Windows.Forms.TextBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.contactDetailBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// txtBorrowerId
			// 
			this.txtBorrowerId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.contactDetailBindingSource, "BorrowerId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtBorrowerId.Location = new System.Drawing.Point(89, 13);
			this.txtBorrowerId.Name = "txtBorrowerId";
			this.txtBorrowerId.ReadOnly = true;
			this.txtBorrowerId.Size = new System.Drawing.Size(81, 20);
			this.txtBorrowerId.TabIndex = 0;
			// 
			// contactDetailBindingSource
			// 
			this.contactDetailBindingSource.DataSource = typeof(RefereneceTelephoneActivityContact.ContactDetail);
			// 
			// txtReferenceId
			// 
			this.txtReferenceId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.contactDetailBindingSource, "ReferenceId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtReferenceId.Location = new System.Drawing.Point(89, 39);
			this.txtReferenceId.Name = "txtReferenceId";
			this.txtReferenceId.ReadOnly = true;
			this.txtReferenceId.Size = new System.Drawing.Size(81, 20);
			this.txtReferenceId.TabIndex = 1;
			// 
			// lblBorrowerId
			// 
			this.lblBorrowerId.AutoSize = true;
			this.lblBorrowerId.Location = new System.Drawing.Point(12, 16);
			this.lblBorrowerId.Name = "lblBorrowerId";
			this.lblBorrowerId.Size = new System.Drawing.Size(63, 13);
			this.lblBorrowerId.TabIndex = 2;
			this.lblBorrowerId.Text = "Borrower ID";
			// 
			// lblReferenceId
			// 
			this.lblReferenceId.AutoSize = true;
			this.lblReferenceId.Location = new System.Drawing.Point(12, 42);
			this.lblReferenceId.Name = "lblReferenceId";
			this.lblReferenceId.Size = new System.Drawing.Size(71, 13);
			this.lblReferenceId.TabIndex = 3;
			this.lblReferenceId.Text = "Reference ID";
			// 
			// cmbContactResult
			// 
			this.cmbContactResult.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.contactDetailBindingSource, "Contact", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.cmbContactResult.FormattingEnabled = true;
			this.cmbContactResult.Location = new System.Drawing.Point(304, 13);
			this.cmbContactResult.Name = "cmbContactResult";
			this.cmbContactResult.Size = new System.Drawing.Size(206, 21);
			this.cmbContactResult.TabIndex = 4;
			this.cmbContactResult.TextChanged += new System.EventHandler(this.cmbContactResult_TextChanged);
			// 
			// lblContactResult
			// 
			this.lblContactResult.AutoSize = true;
			this.lblContactResult.Location = new System.Drawing.Point(208, 16);
			this.lblContactResult.Name = "lblContactResult";
			this.lblContactResult.Size = new System.Drawing.Size(77, 13);
			this.lblContactResult.TabIndex = 5;
			this.lblContactResult.Text = "Contact Result";
			// 
			// lblReferenceResult
			// 
			this.lblReferenceResult.AutoSize = true;
			this.lblReferenceResult.Enabled = false;
			this.lblReferenceResult.Location = new System.Drawing.Point(208, 45);
			this.lblReferenceResult.Name = "lblReferenceResult";
			this.lblReferenceResult.Size = new System.Drawing.Size(90, 13);
			this.lblReferenceResult.TabIndex = 6;
			this.lblReferenceResult.Text = "Reference Result";
			// 
			// cmbReferenceResult
			// 
			this.cmbReferenceResult.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.contactDetailBindingSource, "Result", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.cmbReferenceResult.Enabled = false;
			this.cmbReferenceResult.FormattingEnabled = true;
			this.cmbReferenceResult.Location = new System.Drawing.Point(304, 42);
			this.cmbReferenceResult.Name = "cmbReferenceResult";
			this.cmbReferenceResult.Size = new System.Drawing.Size(206, 21);
			this.cmbReferenceResult.TabIndex = 7;
			// 
			// txtComment
			// 
			this.txtComment.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.contactDetailBindingSource, "Comment", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtComment.Location = new System.Drawing.Point(15, 69);
			this.txtComment.Multiline = true;
			this.txtComment.Name = "txtComment";
			this.txtComment.Size = new System.Drawing.Size(495, 113);
			this.txtComment.TabIndex = 8;
			// 
			// btnOk
			// 
			this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOk.Location = new System.Drawing.Point(112, 199);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(92, 35);
			this.btnOk.TabIndex = 9;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(314, 199);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(92, 35);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// ReferenceCall
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(524, 248);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.txtComment);
			this.Controls.Add(this.cmbReferenceResult);
			this.Controls.Add(this.lblReferenceResult);
			this.Controls.Add(this.lblContactResult);
			this.Controls.Add(this.cmbContactResult);
			this.Controls.Add(this.lblReferenceId);
			this.Controls.Add(this.lblBorrowerId);
			this.Controls.Add(this.txtReferenceId);
			this.Controls.Add(this.txtBorrowerId);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ReferenceCall";
			this.Text = "Reference Telephone Activity Contact: COMPASS";
			((System.ComponentModel.ISupportInitialize)(this.contactDetailBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtBorrowerId;
		private System.Windows.Forms.TextBox txtReferenceId;
		private System.Windows.Forms.Label lblBorrowerId;
		private System.Windows.Forms.Label lblReferenceId;
		private System.Windows.Forms.ComboBox cmbContactResult;
		private System.Windows.Forms.Label lblContactResult;
		private System.Windows.Forms.Label lblReferenceResult;
		private System.Windows.Forms.ComboBox cmbReferenceResult;
		private System.Windows.Forms.TextBox txtComment;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.BindingSource contactDetailBindingSource;
	}
}

