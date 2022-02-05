namespace BLKADDFED.UserControls
{
    partial class AddressRecord
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
            this.components = new System.ComponentModel.Container();
            this.lblAddress1 = new System.Windows.Forms.Label();
            this.borrowerAddressBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.lbZip = new System.Windows.Forms.Label();
            this.lblCountry = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblValidity = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblVerifiedDate = new System.Windows.Forms.Label();
            this.chkSelect = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.borrowerAddressBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAddress1
            // 
            this.lblAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddress1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerAddressBindingSource, "Address1", true));
            this.lblAddress1.Location = new System.Drawing.Point(128, 32);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(320, 20);
            this.lblAddress1.TabIndex = 0;
            this.lblAddress1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // borrowerAddressBindingSource
            // 
            this.borrowerAddressBindingSource.DataSource = typeof(BorrowerAddress);
            // 
            // lblAddress2
            // 
            this.lblAddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddress2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerAddressBindingSource, "Address2", true));
            this.lblAddress2.Location = new System.Drawing.Point(128, 56);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(320, 20);
            this.lblAddress2.TabIndex = 1;
            this.lblAddress2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCity
            // 
            this.lblCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerAddressBindingSource, "City", true));
            this.lblCity.Location = new System.Drawing.Point(128, 80);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(208, 20);
            this.lblCity.TabIndex = 2;
            this.lblCity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblState
            // 
            this.lblState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerAddressBindingSource, "State", true));
            this.lblState.Location = new System.Drawing.Point(344, 80);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(24, 20);
            this.lblState.TabIndex = 3;
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbZip
            // 
            this.lbZip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbZip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerAddressBindingSource, "Zip", true));
            this.lbZip.Location = new System.Drawing.Point(376, 80);
            this.lbZip.Name = "lbZip";
            this.lbZip.Size = new System.Drawing.Size(72, 20);
            this.lbZip.TabIndex = 4;
            this.lbZip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCountry
            // 
            this.lblCountry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCountry.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerAddressBindingSource, "Country", true));
            this.lblCountry.Location = new System.Drawing.Point(128, 104);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(320, 20);
            this.lblCountry.TabIndex = 5;
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(32, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Address 1";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(32, 80);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 20);
            this.label8.TabIndex = 7;
            this.label8.Text = "City State Zip";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(32, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 20);
            this.label9.TabIndex = 8;
            this.label9.Text = "Address 2";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(32, 104);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 20);
            this.label10.TabIndex = 9;
            this.label10.Text = "Country";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(32, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 20);
            this.label11.TabIndex = 10;
            this.label11.Text = "Verified";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblValidity
            // 
            this.lblValidity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblValidity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerAddressBindingSource, "ValidityIndicator", true));
            this.lblValidity.Location = new System.Drawing.Point(424, 8);
            this.lblValidity.Name = "lblValidity";
            this.lblValidity.Size = new System.Drawing.Size(24, 20);
            this.lblValidity.TabIndex = 11;
            this.lblValidity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(328, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 20);
            this.label13.TabIndex = 12;
            this.label13.Text = "Valid";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVerifiedDate
            // 
            this.lblVerifiedDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVerifiedDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerAddressBindingSource, "VerifiedDate", true));
            this.lblVerifiedDate.Location = new System.Drawing.Point(128, 8);
            this.lblVerifiedDate.Name = "lblVerifiedDate";
            this.lblVerifiedDate.Size = new System.Drawing.Size(72, 20);
            this.lblVerifiedDate.TabIndex = 13;
            this.lblVerifiedDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkSelect
            // 
            this.chkSelect.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerAddressBindingSource, "Selected", true));
            this.chkSelect.Location = new System.Drawing.Point(8, 8);
            this.chkSelect.Name = "chkSelect";
            this.chkSelect.Size = new System.Drawing.Size(20, 20);
            this.chkSelect.TabIndex = 14;
            this.chkSelect.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.chkSelect);
            this.Controls.Add(this.lblVerifiedDate);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lblValidity);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblCountry);
            this.Controls.Add(this.lbZip);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.lblCity);
            this.Controls.Add(this.lblAddress2);
            this.Controls.Add(this.lblAddress1);
            this.Name = "Address";
            this.Size = new System.Drawing.Size(458, 135);
            ((System.ComponentModel.ISupportInitialize)(this.borrowerAddressBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAddress1;
        private System.Windows.Forms.Label lblAddress2;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lbZip;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblValidity;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblVerifiedDate;
        private System.Windows.Forms.CheckBox chkSelect;
        private System.Windows.Forms.BindingSource borrowerAddressBindingSource;

    }
}
