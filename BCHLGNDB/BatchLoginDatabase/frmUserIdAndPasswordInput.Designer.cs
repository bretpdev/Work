namespace BatchLoginDatabase
{
	partial class frmUserIdAndPasswordInput
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
			this.btnChangeId = new System.Windows.Forms.Button();
			this.btnCreateNewId = new System.Windows.Forms.Button();
			this.txtNotes = new System.Windows.Forms.TextBox();
			this.lblNotes = new System.Windows.Forms.Label();
			this.btnDelete = new System.Windows.Forms.Button();
			this.clbUserIds = new System.Windows.Forms.CheckedListBox();
			this.btnExit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnChangeId
			// 
			this.btnChangeId.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnChangeId.Location = new System.Drawing.Point(12, 374);
			this.btnChangeId.Name = "btnChangeId";
			this.btnChangeId.Size = new System.Drawing.Size(102, 39);
			this.btnChangeId.TabIndex = 0;
			this.btnChangeId.Text = "Change Passwords";
			this.btnChangeId.UseVisualStyleBackColor = false;
			this.btnChangeId.Click += new System.EventHandler(this.btnChangeId_Click);
			// 
			// btnCreateNewId
			// 
			this.btnCreateNewId.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnCreateNewId.Location = new System.Drawing.Point(154, 374);
			this.btnCreateNewId.Name = "btnCreateNewId";
			this.btnCreateNewId.Size = new System.Drawing.Size(102, 39);
			this.btnCreateNewId.TabIndex = 1;
			this.btnCreateNewId.Text = "Create New Stored ID";
			this.btnCreateNewId.UseVisualStyleBackColor = false;
			this.btnCreateNewId.Click += new System.EventHandler(this.btnCreateNewId_Click);
			// 
			// txtNotes
			// 
			this.txtNotes.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtNotes.Location = new System.Drawing.Point(311, 48);
			this.txtNotes.MaxLength = 100;
			this.txtNotes.Multiline = true;
			this.txtNotes.Name = "txtNotes";
			this.txtNotes.ReadOnly = true;
			this.txtNotes.Size = new System.Drawing.Size(192, 135);
			this.txtNotes.TabIndex = 3;
			// 
			// lblNotes
			// 
			this.lblNotes.AutoSize = true;
			this.lblNotes.Location = new System.Drawing.Point(308, 23);
			this.lblNotes.Name = "lblNotes";
			this.lblNotes.Size = new System.Drawing.Size(107, 13);
			this.lblNotes.TabIndex = 3;
			this.lblNotes.Text = "Notes for selected ID";
			// 
			// btnDelete
			// 
			this.btnDelete.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnDelete.Location = new System.Drawing.Point(291, 374);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(102, 39);
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Text = "Delete Stored ID";
			this.btnDelete.UseVisualStyleBackColor = false;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// clbUserIds
			// 
			this.clbUserIds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.clbUserIds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.clbUserIds.FormattingEnabled = true;
			this.clbUserIds.Location = new System.Drawing.Point(35, 32);
			this.clbUserIds.Name = "clbUserIds";
			this.clbUserIds.Size = new System.Drawing.Size(238, 302);
			this.clbUserIds.TabIndex = 5;
			this.clbUserIds.ThreeDCheckBoxes = true;
			this.clbUserIds.SelectedIndexChanged += new System.EventHandler(this.clbUserIds_SelectedIndexChanged);
			// 
			// btnExit
			// 
			this.btnExit.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnExit.Location = new System.Drawing.Point(421, 374);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(102, 39);
			this.btnExit.TabIndex = 6;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// frmUserIdAndPasswordInput
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(535, 425);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.clbUserIds);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.lblNotes);
			this.Controls.Add(this.txtNotes);
			this.Controls.Add(this.btnCreateNewId);
			this.Controls.Add(this.btnChangeId);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmUserIdAndPasswordInput";
			this.ShowIcon = false;
			this.Text = "Select User Ids";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnChangeId;
		private System.Windows.Forms.Button btnCreateNewId;
		private System.Windows.Forms.TextBox txtNotes;
		private System.Windows.Forms.Label lblNotes;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.CheckedListBox clbUserIds;
		private System.Windows.Forms.Button btnExit;
	}
}