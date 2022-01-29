namespace ACDCAccess
{
    partial class frmUpdateUserRole
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
            this.lblRoleChange = new System.Windows.Forms.Label();
            this.cboRoles = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblRoleChange
            // 
            this.lblRoleChange.AutoSize = true;
            this.lblRoleChange.Location = new System.Drawing.Point(12, 9);
            this.lblRoleChange.MaximumSize = new System.Drawing.Size(230, 39);
            this.lblRoleChange.Name = "lblRoleChange";
            this.lblRoleChange.Size = new System.Drawing.Size(228, 39);
            this.lblRoleChange.TabIndex = 0;
            this.lblRoleChange.Text = "There are @@@@ users that used the @@@@@ role. Please choose a new role for these" +
                " users.";
            // 
            // cboRoles
            // 
            this.cboRoles.FormattingEnabled = true;
            this.cboRoles.Location = new System.Drawing.Point(15, 69);
            this.cboRoles.Name = "cboRoles";
            this.cboRoles.Size = new System.Drawing.Size(230, 21);
            this.cboRoles.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(83, 96);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmUpdateUserRole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 128);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cboRoles);
            this.Controls.Add(this.lblRoleChange);
            this.Name = "frmUpdateUserRole";
            this.Text = "frmUpdateUserRole";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRoleChange;
        private System.Windows.Forms.ComboBox cboRoles;
        private System.Windows.Forms.Button btnOk;
    }
}