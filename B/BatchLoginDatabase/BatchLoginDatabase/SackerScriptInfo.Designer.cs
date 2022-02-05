namespace BatchLoginDatabase
{
    partial class SackerScriptInfo
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
            this.SackerScriptIds = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AllowedLogins = new System.Windows.Forms.NumericUpDown();
            this.UserIds = new System.Windows.Forms.CheckedListBox();
            this.Save = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.AllowedLogins)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(44, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sacker Script Id";
            // 
            // SackerScriptIds
            // 
            this.SackerScriptIds.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.SackerScriptIds.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.SackerScriptIds.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SackerScriptIds.FormattingEnabled = true;
            this.SackerScriptIds.Location = new System.Drawing.Point(172, 9);
            this.SackerScriptIds.Name = "SackerScriptIds";
            this.SackerScriptIds.Size = new System.Drawing.Size(149, 26);
            this.SackerScriptIds.TabIndex = 1;
            this.SackerScriptIds.Leave += new System.EventHandler(this.SackerScriptIds_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(186, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Maximum Allowed Logins";
            // 
            // AllowedLogins
            // 
            this.AllowedLogins.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AllowedLogins.Location = new System.Drawing.Point(201, 41);
            this.AllowedLogins.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AllowedLogins.Name = "AllowedLogins";
            this.AllowedLogins.Size = new System.Drawing.Size(120, 26);
            this.AllowedLogins.TabIndex = 3;
            this.AllowedLogins.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // UserIds
            // 
            this.UserIds.CheckOnClick = true;
            this.UserIds.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserIds.FormattingEnabled = true;
            this.UserIds.Location = new System.Drawing.Point(17, 85);
            this.UserIds.Name = "UserIds";
            this.UserIds.Size = new System.Drawing.Size(308, 172);
            this.UserIds.TabIndex = 4;
            // 
            // Save
            // 
            this.Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Save.Location = new System.Drawing.Point(236, 275);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(88, 36);
            this.Save.TabIndex = 5;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // SackerScriptInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 321);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.UserIds);
            this.Controls.Add(this.AllowedLogins);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SackerScriptIds);
            this.Controls.Add(this.label1);
            this.Name = "SackerScriptInfo";
            this.ShowIcon = false;
            this.Text = "SackerScriptInfo";
            ((System.ComponentModel.ISupportInitialize)(this.AllowedLogins)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SackerScriptIds;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown AllowedLogins;
        private System.Windows.Forms.CheckedListBox UserIds;
        private System.Windows.Forms.Button Save;
    }
}