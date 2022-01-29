namespace BLKADDFED.UserControls
{
    partial class PhoneRecord
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
            this.chkSelect = new System.Windows.Forms.CheckBox();
            this.borrowerPhoneBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblVerifiedDate = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblValidity = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblDomesticAreaCode = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblConsent = new System.Windows.Forms.Label();
            this.lblDomesticPrefix = new System.Windows.Forms.Label();
            this.lblDomesticLineNumber = new System.Windows.Forms.Label();
            this.lblExtension = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblForeignLocalNumber = new System.Windows.Forms.Label();
            this.lblForeignCityCode = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblForeignCountryCode = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.borrowerPhoneBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // chkSelect
            // 
            this.chkSelect.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.borrowerPhoneBindingSource, "Selected", true));
            this.chkSelect.Location = new System.Drawing.Point(8, 8);
            this.chkSelect.Name = "chkSelect";
            this.chkSelect.Size = new System.Drawing.Size(20, 20);
            this.chkSelect.TabIndex = 29;
            this.chkSelect.UseVisualStyleBackColor = true;
            // 
            // borrowerPhoneBindingSource
            // 
            this.borrowerPhoneBindingSource.DataSource = typeof(BorrowerPhone);
            // 
            // lblVerifiedDate
            // 
            this.lblVerifiedDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVerifiedDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPhoneBindingSource, "VerifiedDate", true));
            this.lblVerifiedDate.Location = new System.Drawing.Point(128, 32);
            this.lblVerifiedDate.Name = "lblVerifiedDate";
            this.lblVerifiedDate.Size = new System.Drawing.Size(72, 20);
            this.lblVerifiedDate.TabIndex = 28;
            this.lblVerifiedDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(328, 32);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 20);
            this.label13.TabIndex = 27;
            this.label13.Text = "Valid";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblValidity
            // 
            this.lblValidity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblValidity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPhoneBindingSource, "ValidityIndicator", true));
            this.lblValidity.Location = new System.Drawing.Point(424, 32);
            this.lblValidity.Name = "lblValidity";
            this.lblValidity.Size = new System.Drawing.Size(24, 20);
            this.lblValidity.TabIndex = 26;
            this.lblValidity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(32, 32);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 20);
            this.label11.TabIndex = 25;
            this.label11.Text = "Verified";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(32, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 20);
            this.label9.TabIndex = 23;
            this.label9.Text = "Domestic";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(32, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 20);
            this.label7.TabIndex = 21;
            this.label7.Text = "Type";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDomesticAreaCode
            // 
            this.lblDomesticAreaCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDomesticAreaCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPhoneBindingSource, "DomesticAreaCode", true));
            this.lblDomesticAreaCode.Location = new System.Drawing.Point(128, 80);
            this.lblDomesticAreaCode.Name = "lblDomesticAreaCode";
            this.lblDomesticAreaCode.Size = new System.Drawing.Size(28, 20);
            this.lblDomesticAreaCode.TabIndex = 16;
            this.lblDomesticAreaCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblType
            // 
            this.lblType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPhoneBindingSource, "PhoneType", true));
            this.lblType.Location = new System.Drawing.Point(128, 8);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(24, 20);
            this.lblType.TabIndex = 15;
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(328, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 20);
            this.label1.TabIndex = 31;
            this.label1.Text = "MBL";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMbl
            // 
            this.lblMbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMbl.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPhoneBindingSource, "MBLIndicator", true));
            this.lblMbl.Location = new System.Drawing.Point(424, 56);
            this.lblMbl.Name = "lblMbl";
            this.lblMbl.Size = new System.Drawing.Size(24, 20);
            this.lblMbl.TabIndex = 30;
            this.lblMbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(32, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 20);
            this.label2.TabIndex = 33;
            this.label2.Text = "Consent";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConsent
            // 
            this.lblConsent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblConsent.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPhoneBindingSource, "ConsentIndicator", true));
            this.lblConsent.Location = new System.Drawing.Point(128, 56);
            this.lblConsent.Name = "lblConsent";
            this.lblConsent.Size = new System.Drawing.Size(24, 20);
            this.lblConsent.TabIndex = 32;
            this.lblConsent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDomesticPrefix
            // 
            this.lblDomesticPrefix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDomesticPrefix.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPhoneBindingSource, "DomesticPrefix", true));
            this.lblDomesticPrefix.Location = new System.Drawing.Point(160, 80);
            this.lblDomesticPrefix.Name = "lblDomesticPrefix";
            this.lblDomesticPrefix.Size = new System.Drawing.Size(28, 20);
            this.lblDomesticPrefix.TabIndex = 34;
            this.lblDomesticPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDomesticLineNumber
            // 
            this.lblDomesticLineNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDomesticLineNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPhoneBindingSource, "DomesticLineNumber", true));
            this.lblDomesticLineNumber.Location = new System.Drawing.Point(192, 80);
            this.lblDomesticLineNumber.Name = "lblDomesticLineNumber";
            this.lblDomesticLineNumber.Size = new System.Drawing.Size(36, 20);
            this.lblDomesticLineNumber.TabIndex = 35;
            this.lblDomesticLineNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExtension
            // 
            this.lblExtension.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblExtension.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPhoneBindingSource, "Extension", true));
            this.lblExtension.Location = new System.Drawing.Point(408, 80);
            this.lblExtension.Name = "lblExtension";
            this.lblExtension.Size = new System.Drawing.Size(40, 20);
            this.lblExtension.TabIndex = 36;
            this.lblExtension.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(328, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 20);
            this.label4.TabIndex = 37;
            this.label4.Text = "Extension";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblForeignLocalNumber
            // 
            this.lblForeignLocalNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblForeignLocalNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPhoneBindingSource, "ForeignLocalNumber", true));
            this.lblForeignLocalNumber.Location = new System.Drawing.Point(204, 104);
            this.lblForeignLocalNumber.Name = "lblForeignLocalNumber";
            this.lblForeignLocalNumber.Size = new System.Drawing.Size(76, 20);
            this.lblForeignLocalNumber.TabIndex = 41;
            this.lblForeignLocalNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblForeignCityCode
            // 
            this.lblForeignCityCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblForeignCityCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPhoneBindingSource, "ForeignCityCode", true));
            this.lblForeignCityCode.Location = new System.Drawing.Point(160, 104);
            this.lblForeignCityCode.Name = "lblForeignCityCode";
            this.lblForeignCityCode.Size = new System.Drawing.Size(40, 20);
            this.lblForeignCityCode.TabIndex = 40;
            this.lblForeignCityCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(32, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 20);
            this.label6.TabIndex = 39;
            this.label6.Text = "Foreign";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblForeignCountryCode
            // 
            this.lblForeignCountryCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblForeignCountryCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerPhoneBindingSource, "ForeignCountryCode", true));
            this.lblForeignCountryCode.Location = new System.Drawing.Point(128, 104);
            this.lblForeignCountryCode.Name = "lblForeignCountryCode";
            this.lblForeignCountryCode.Size = new System.Drawing.Size(28, 20);
            this.lblForeignCountryCode.TabIndex = 38;
            this.lblForeignCountryCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PhoneRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblForeignLocalNumber);
            this.Controls.Add(this.lblForeignCityCode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblForeignCountryCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblExtension);
            this.Controls.Add(this.lblDomesticLineNumber);
            this.Controls.Add(this.lblDomesticPrefix);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblConsent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMbl);
            this.Controls.Add(this.chkSelect);
            this.Controls.Add(this.lblVerifiedDate);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lblValidity);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblDomesticAreaCode);
            this.Controls.Add(this.lblType);
            this.Name = "PhoneRecord";
            this.Size = new System.Drawing.Size(456, 133);
            ((System.ComponentModel.ISupportInitialize)(this.borrowerPhoneBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSelect;
        private System.Windows.Forms.Label lblVerifiedDate;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblValidity;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblDomesticAreaCode;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblConsent;
        private System.Windows.Forms.Label lblDomesticPrefix;
        private System.Windows.Forms.Label lblDomesticLineNumber;
        private System.Windows.Forms.Label lblExtension;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblForeignLocalNumber;
        private System.Windows.Forms.Label lblForeignCityCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblForeignCountryCode;
        private System.Windows.Forms.BindingSource borrowerPhoneBindingSource;
    }
}
