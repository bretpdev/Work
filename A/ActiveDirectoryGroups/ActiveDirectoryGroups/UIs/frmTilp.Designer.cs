namespace ActiveDirectoryGroups
{
    partial class frmTilp
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
            this.labelUser = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Description = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.UserIdtxt = new Uheaa.Common.WinForms.RequiredTextBox();
            this.AuthListCbo = new System.Windows.Forms.ComboBox();
            this.ValidChk = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelUser
            // 
            this.labelUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(22, 29);
            this.labelUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(68, 20);
            this.labelUser.TabIndex = 0;
            this.labelUser.Text = "User ID:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 90);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Authorization Level:";
            // 
            // Description
            // 
            this.Description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Description.Location = new System.Drawing.Point(22, 144);
            this.Description.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(256, 87);
            this.Description.TabIndex = 3;
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.Location = new System.Drawing.Point(174, 249);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(105, 41);
            this.OK.TabIndex = 3;
            this.OK.Text = "Add";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(12, 249);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(105, 41);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // UserIdtxt
            // 
            this.UserIdtxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserIdtxt.Location = new System.Drawing.Point(26, 52);
            this.UserIdtxt.Name = "UserIdtxt";
            this.UserIdtxt.Size = new System.Drawing.Size(183, 26);
            this.UserIdtxt.TabIndex = 0;
            // 
            // AuthListCbo
            // 
            this.AuthListCbo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AuthListCbo.FormattingEnabled = true;
            this.AuthListCbo.Location = new System.Drawing.Point(26, 113);
            this.AuthListCbo.Name = "AuthListCbo";
            this.AuthListCbo.Size = new System.Drawing.Size(64, 28);
            this.AuthListCbo.TabIndex = 1;
            this.AuthListCbo.SelectedIndexChanged += new System.EventHandler(this.AuthListCbo_SelectedIndexChanged);
            // 
            // ValidChk
            // 
            this.ValidChk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValidChk.AutoSize = true;
            this.ValidChk.Location = new System.Drawing.Point(215, 117);
            this.ValidChk.Name = "ValidChk";
            this.ValidChk.Size = new System.Drawing.Size(63, 24);
            this.ValidChk.TabIndex = 2;
            this.ValidChk.Text = "Valid";
            this.ValidChk.UseVisualStyleBackColor = true;
            // 
            // frmTilp
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(291, 302);
            this.Controls.Add(this.ValidChk);
            this.Controls.Add(this.AuthListCbo);
            this.Controls.Add(this.UserIdtxt);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Description);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelUser);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmTilp";
            this.Text = "frmTilp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Description;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private Uheaa.Common.WinForms.RequiredTextBox UserIdtxt;
        private System.Windows.Forms.ComboBox AuthListCbo;
        private System.Windows.Forms.CheckBox ValidChk;
    }
}