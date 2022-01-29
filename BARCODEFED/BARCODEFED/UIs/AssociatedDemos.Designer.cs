namespace BARCODEFED
{
    partial class AssociatedDemos
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AccountName = new System.Windows.Forms.Label();
            this.Address1 = new System.Windows.Forms.Label();
            this.Address2 = new System.Windows.Forms.Label();
            this.CityStateZip = new System.Windows.Forms.Label();
            this.SelectCbo = new System.Windows.Forms.CheckBox();
            this.AcctIdentifier = new System.Windows.Forms.Label();
            this.RegionLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AccountName
            // 
            this.AccountName.AutoSize = true;
            this.AccountName.Location = new System.Drawing.Point(29, 33);
            this.AccountName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AccountName.Name = "AccountName";
            this.AccountName.Size = new System.Drawing.Size(0, 20);
            this.AccountName.TabIndex = 0;
            // 
            // Address1
            // 
            this.Address1.AutoSize = true;
            this.Address1.Location = new System.Drawing.Point(29, 60);
            this.Address1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Address1.Name = "Address1";
            this.Address1.Size = new System.Drawing.Size(0, 20);
            this.Address1.TabIndex = 1;
            // 
            // Address2
            // 
            this.Address2.AutoSize = true;
            this.Address2.Location = new System.Drawing.Point(29, 87);
            this.Address2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Address2.Name = "Address2";
            this.Address2.Size = new System.Drawing.Size(0, 20);
            this.Address2.TabIndex = 2;
            // 
            // CityStateZip
            // 
            this.CityStateZip.AutoSize = true;
            this.CityStateZip.Location = new System.Drawing.Point(29, 114);
            this.CityStateZip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CityStateZip.Name = "CityStateZip";
            this.CityStateZip.Size = new System.Drawing.Size(0, 20);
            this.CityStateZip.TabIndex = 3;
            // 
            // SelectCbo
            // 
            this.SelectCbo.AutoSize = true;
            this.SelectCbo.Location = new System.Drawing.Point(3, 60);
            this.SelectCbo.Name = "SelectCbo";
            this.SelectCbo.Size = new System.Drawing.Size(15, 14);
            this.SelectCbo.TabIndex = 4;
            this.SelectCbo.UseVisualStyleBackColor = true;
            // 
            // AcctIdentifier
            // 
            this.AcctIdentifier.AutoSize = true;
            this.AcctIdentifier.Location = new System.Drawing.Point(29, 6);
            this.AcctIdentifier.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AcctIdentifier.Name = "AcctIdentifier";
            this.AcctIdentifier.Size = new System.Drawing.Size(0, 20);
            this.AcctIdentifier.TabIndex = 5;
            // 
            // RegionLbl
            // 
            this.RegionLbl.AutoSize = true;
            this.RegionLbl.Location = new System.Drawing.Point(239, 6);
            this.RegionLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RegionLbl.Name = "RegionLbl";
            this.RegionLbl.Size = new System.Drawing.Size(0, 20);
            this.RegionLbl.TabIndex = 6;
            // 
            // AssociatedDemos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.RegionLbl);
            this.Controls.Add(this.AcctIdentifier);
            this.Controls.Add(this.SelectCbo);
            this.Controls.Add(this.CityStateZip);
            this.Controls.Add(this.Address2);
            this.Controls.Add(this.Address1);
            this.Controls.Add(this.AccountName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AssociatedDemos";
            this.Size = new System.Drawing.Size(343, 140);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AccountName;
        private System.Windows.Forms.Label Address1;
        private System.Windows.Forms.Label Address2;
        private System.Windows.Forms.Label CityStateZip;
        public System.Windows.Forms.CheckBox SelectCbo;
        private System.Windows.Forms.Label AcctIdentifier;
        private System.Windows.Forms.Label RegionLbl;
    }
}
