namespace BatchLoginDatabase
{
    partial class ChangeLoginTypes
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
            this.AllowedLogins = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.LoginTypesCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Save = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.AllowedLogins)).BeginInit();
            this.SuspendLayout();
            // 
            // AllowedLogins
            // 
            this.AllowedLogins.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AllowedLogins.Location = new System.Drawing.Point(143, 39);
            this.AllowedLogins.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AllowedLogins.Name = "AllowedLogins";
            this.AllowedLogins.ReadOnly = true;
            this.AllowedLogins.Size = new System.Drawing.Size(139, 26);
            this.AllowedLogins.TabIndex = 10;
            this.AllowedLogins.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Allowed Logins:";
            // 
            // LoginTypesCombo
            // 
            this.LoginTypesCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginTypesCombo.FormattingEnabled = true;
            this.LoginTypesCombo.Location = new System.Drawing.Point(143, 7);
            this.LoginTypesCombo.Name = "LoginTypesCombo";
            this.LoginTypesCombo.Size = new System.Drawing.Size(182, 26);
            this.LoginTypesCombo.TabIndex = 9;
            this.LoginTypesCombo.SelectedIndexChanged += new System.EventHandler(this.LoginTypesCombo_SelectedIndexChanged);
            this.LoginTypesCombo.SelectionChangeCommitted += new System.EventHandler(this.LoginTypesCombo_SelectionChangeCommitted);
            this.LoginTypesCombo.Leave += new System.EventHandler(this.LoginTypesCombo_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(47, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Login Type:";
            // 
            // Save
            // 
            this.Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Save.Location = new System.Drawing.Point(239, 81);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(86, 29);
            this.Save.TabIndex = 12;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // ChangeLoginTypes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 119);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.AllowedLogins);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LoginTypesCombo);
            this.Controls.Add(this.label1);
            this.Name = "ChangeLoginTypes";
            this.ShowIcon = false;
            this.Text = "ChangeLoginTypes";
            ((System.ComponentModel.ISupportInitialize)(this.AllowedLogins)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown AllowedLogins;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox LoginTypesCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Save;
    }
}