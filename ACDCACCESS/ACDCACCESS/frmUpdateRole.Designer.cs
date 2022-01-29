namespace ACDCAccess
{
	partial class frmUpdateRole
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtRoleName = new System.Windows.Forms.TextBox();
            this.btnSubmitRole = new System.Windows.Forms.Button();
            this.lblRoleChange = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(243)))), ((int)(((byte)(229)))));
            this.label1.Location = new System.Drawing.Point(20, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Role Name:";
            // 
            // txtRoleName
            // 
            this.txtRoleName.Location = new System.Drawing.Point(89, 34);
            this.txtRoleName.MaxLength = 72;
            this.txtRoleName.Name = "txtRoleName";
            this.txtRoleName.Size = new System.Drawing.Size(464, 20);
            this.txtRoleName.TabIndex = 1;
            this.txtRoleName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRoleName_KeyDown);
            // 
            // btnSubmitRole
            // 
            this.btnSubmitRole.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSubmitRole.Location = new System.Drawing.Point(276, 60);
            this.btnSubmitRole.Name = "btnSubmitRole";
            this.btnSubmitRole.Size = new System.Drawing.Size(75, 23);
            this.btnSubmitRole.TabIndex = 2;
            this.btnSubmitRole.Text = "Submit";
            this.btnSubmitRole.UseVisualStyleBackColor = false;
            this.btnSubmitRole.Click += new System.EventHandler(this.btnSubmitRole_Click);
            // 
            // lblRoleChange
            // 
            this.lblRoleChange.AutoSize = true;
            this.lblRoleChange.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(243)))), ((int)(((byte)(229)))));
            this.lblRoleChange.Location = new System.Drawing.Point(20, 9);
            this.lblRoleChange.Name = "lblRoleChange";
            this.lblRoleChange.Size = new System.Drawing.Size(106, 13);
            this.lblRoleChange.TabIndex = 3;
            this.lblRoleChange.Text = "Change @@@@ to:";
            // 
            // frmUpdateRole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(564, 93);
            this.Controls.Add(this.lblRoleChange);
            this.Controls.Add(this.btnSubmitRole);
            this.Controls.Add(this.txtRoleName);
            this.Controls.Add(this.label1);
            this.Name = "frmUpdateRole";
            this.Text = "New Role Name";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtRoleName;
		private System.Windows.Forms.Button btnSubmitRole;
		private System.Windows.Forms.Label lblRoleChange;
	}
}