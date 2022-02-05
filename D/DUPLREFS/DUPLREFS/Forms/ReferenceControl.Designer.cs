
using System;

namespace DUPLREFS.Forms
{
    partial class ReferenceControl
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
            this.lbl_RefCountry = new Uheaa.Common.WinForms.AutoFontLabel();
            this.tb_RefCountry = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.lbl_RefPhone = new Uheaa.Common.WinForms.AutoFontLabel();
            this.tb_RefPhone = new Uheaa.Common.WinForms.NumericTextBox();
            this.tb_RefZip = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.lbl_RefZip = new Uheaa.Common.WinForms.AutoFontLabel();
            this.cbo_RefState = new System.Windows.Forms.ComboBox();
            this.lbl_RefState = new Uheaa.Common.WinForms.AutoFontLabel();
            this.lbl_RefCity = new Uheaa.Common.WinForms.AutoFontLabel();
            this.tb_RefCity = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.lbl_RefAddress2 = new Uheaa.Common.WinForms.AutoFontLabel();
            this.tb_RefAddress2 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.lbl_RefAddress1 = new Uheaa.Common.WinForms.AutoFontLabel();
            this.tb_RefAddress1 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.lbl_RefName = new Uheaa.Common.WinForms.AutoFontLabel();
            this.tb_RefName = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.rb_RefLeaveActive = new System.Windows.Forms.RadioButton();
            this.lbl_RefLeaveActive = new Uheaa.Common.WinForms.AutoFontLabel();
            this.lbl_RefID = new Uheaa.Common.WinForms.AutoFontLabel();
            this.tb_RefID = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.SuspendLayout();
            // 
            // lbl_RefCountry
            // 
            this.lbl_RefCountry.AutoSize = true;
            this.lbl_RefCountry.Location = new System.Drawing.Point(212, 147);
            this.lbl_RefCountry.MaxFontSize = 0F;
            this.lbl_RefCountry.Name = "lbl_RefCountry";
            this.lbl_RefCountry.Size = new System.Drawing.Size(43, 13);
            this.lbl_RefCountry.TabIndex = 111;
            this.lbl_RefCountry.Text = "Country";
            // 
            // tb_RefCountry
            // 
            this.tb_RefCountry.AllowedSpecialCharacters = " ";
            this.tb_RefCountry.Location = new System.Drawing.Point(267, 143);
            this.tb_RefCountry.Name = "tb_RefCountry";
            this.tb_RefCountry.Size = new System.Drawing.Size(100, 20);
            this.tb_RefCountry.TabIndex = 8;
            // 
            // lbl_RefPhone
            // 
            this.lbl_RefPhone.AutoSize = true;
            this.lbl_RefPhone.Location = new System.Drawing.Point(13, 146);
            this.lbl_RefPhone.MaxFontSize = 0F;
            this.lbl_RefPhone.Name = "lbl_RefPhone";
            this.lbl_RefPhone.Size = new System.Drawing.Size(38, 13);
            this.lbl_RefPhone.TabIndex = 107;
            this.lbl_RefPhone.Text = "Phone";
            // 
            // tb_RefPhone
            // 
            this.tb_RefPhone.AllowedSpecialCharacters = " ";
            this.tb_RefPhone.Location = new System.Drawing.Point(70, 143);
            this.tb_RefPhone.Name = "tb_RefPhone";
            this.tb_RefPhone.Size = new System.Drawing.Size(126, 20);
            this.tb_RefPhone.TabIndex = 7;
            // 
            // tb_RefZip
            // 
            this.tb_RefZip.AllowedSpecialCharacters = " ";
            this.tb_RefZip.Location = new System.Drawing.Point(288, 116);
            this.tb_RefZip.Name = "tb_RefZip";
            this.tb_RefZip.Size = new System.Drawing.Size(78, 20);
            this.tb_RefZip.TabIndex = 6;
            // 
            // lbl_RefZip
            // 
            this.lbl_RefZip.AutoSize = true;
            this.lbl_RefZip.Location = new System.Drawing.Point(260, 119);
            this.lbl_RefZip.MaxFontSize = 0F;
            this.lbl_RefZip.Name = "lbl_RefZip";
            this.lbl_RefZip.Size = new System.Drawing.Size(22, 13);
            this.lbl_RefZip.TabIndex = 104;
            this.lbl_RefZip.Text = "Zip";
            // 
            // cbo_RefState
            // 
            this.cbo_RefState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_RefState.FormattingEnabled = true;
            this.cbo_RefState.Location = new System.Drawing.Point(197, 115);
            this.cbo_RefState.Name = "cbo_RefState";
            this.cbo_RefState.Size = new System.Drawing.Size(57, 21);
            this.cbo_RefState.TabIndex = 5;
            // 
            // lbl_RefState
            // 
            this.lbl_RefState.AutoSize = true;
            this.lbl_RefState.Location = new System.Drawing.Point(170, 122);
            this.lbl_RefState.MaxFontSize = 0F;
            this.lbl_RefState.Name = "lbl_RefState";
            this.lbl_RefState.Size = new System.Drawing.Size(21, 13);
            this.lbl_RefState.TabIndex = 102;
            this.lbl_RefState.Text = "ST";
            // 
            // lbl_RefCity
            // 
            this.lbl_RefCity.AutoSize = true;
            this.lbl_RefCity.Location = new System.Drawing.Point(13, 119);
            this.lbl_RefCity.MaxFontSize = 0F;
            this.lbl_RefCity.Name = "lbl_RefCity";
            this.lbl_RefCity.Size = new System.Drawing.Size(24, 13);
            this.lbl_RefCity.TabIndex = 101;
            this.lbl_RefCity.Text = "City";
            // 
            // tb_RefCity
            // 
            this.tb_RefCity.AllowedSpecialCharacters = " ";
            this.tb_RefCity.Location = new System.Drawing.Point(70, 116);
            this.tb_RefCity.Name = "tb_RefCity";
            this.tb_RefCity.Size = new System.Drawing.Size(94, 20);
            this.tb_RefCity.TabIndex = 4;
            // 
            // lbl_RefAddress2
            // 
            this.lbl_RefAddress2.AutoSize = true;
            this.lbl_RefAddress2.Location = new System.Drawing.Point(13, 93);
            this.lbl_RefAddress2.MaxFontSize = 0F;
            this.lbl_RefAddress2.Name = "lbl_RefAddress2";
            this.lbl_RefAddress2.Size = new System.Drawing.Size(51, 13);
            this.lbl_RefAddress2.TabIndex = 99;
            this.lbl_RefAddress2.Text = "Address2";
            // 
            // tb_RefAddress2
            // 
            this.tb_RefAddress2.AllowedSpecialCharacters = " ";
            this.tb_RefAddress2.Location = new System.Drawing.Point(70, 90);
            this.tb_RefAddress2.Name = "tb_RefAddress2";
            this.tb_RefAddress2.Size = new System.Drawing.Size(296, 20);
            this.tb_RefAddress2.TabIndex = 3;
            // 
            // lbl_RefAddress1
            // 
            this.lbl_RefAddress1.AutoSize = true;
            this.lbl_RefAddress1.Location = new System.Drawing.Point(13, 67);
            this.lbl_RefAddress1.MaxFontSize = 0F;
            this.lbl_RefAddress1.Name = "lbl_RefAddress1";
            this.lbl_RefAddress1.Size = new System.Drawing.Size(51, 13);
            this.lbl_RefAddress1.TabIndex = 97;
            this.lbl_RefAddress1.Text = "Address1";
            // 
            // tb_RefAddress1
            // 
            this.tb_RefAddress1.AllowedSpecialCharacters = " ";
            this.tb_RefAddress1.Location = new System.Drawing.Point(70, 64);
            this.tb_RefAddress1.Name = "tb_RefAddress1";
            this.tb_RefAddress1.Size = new System.Drawing.Size(296, 20);
            this.tb_RefAddress1.TabIndex = 2;
            // 
            // lbl_RefName
            // 
            this.lbl_RefName.AutoSize = true;
            this.lbl_RefName.Location = new System.Drawing.Point(13, 41);
            this.lbl_RefName.MaxFontSize = 0F;
            this.lbl_RefName.Name = "lbl_RefName";
            this.lbl_RefName.Size = new System.Drawing.Size(35, 13);
            this.lbl_RefName.TabIndex = 95;
            this.lbl_RefName.Text = "Name";
            // 
            // tb_RefName
            // 
            this.tb_RefName.AllowedSpecialCharacters = " ";
            this.tb_RefName.Location = new System.Drawing.Point(70, 38);
            this.tb_RefName.Name = "tb_RefName";
            this.tb_RefName.ReadOnly = true;
            this.tb_RefName.Size = new System.Drawing.Size(296, 20);
            this.tb_RefName.TabIndex = 94;
            // 
            // rb_RefLeaveActive
            // 
            this.rb_RefLeaveActive.AutoSize = true;
            this.rb_RefLeaveActive.Location = new System.Drawing.Point(352, 15);
            this.rb_RefLeaveActive.Name = "rb_RefLeaveActive";
            this.rb_RefLeaveActive.Size = new System.Drawing.Size(14, 13);
            this.rb_RefLeaveActive.TabIndex = 1;
            this.rb_RefLeaveActive.TabStop = true;
            this.rb_RefLeaveActive.UseVisualStyleBackColor = true;
            this.rb_RefLeaveActive.CheckedChanged += new System.EventHandler(this.rb_RefLeaveActive_CheckedChanged);
            this.rb_RefLeaveActive.Click += new System.EventHandler(this.rb_RefLeaveActive_OnClick);
            // 
            // lbl_RefLeaveActive
            // 
            this.lbl_RefLeaveActive.AutoSize = true;
            this.lbl_RefLeaveActive.Location = new System.Drawing.Point(220, 15);
            this.lbl_RefLeaveActive.MaxFontSize = 0F;
            this.lbl_RefLeaveActive.Name = "lbl_RefLeaveActive";
            this.lbl_RefLeaveActive.Size = new System.Drawing.Size(108, 13);
            this.lbl_RefLeaveActive.TabIndex = 92;
            this.lbl_RefLeaveActive.Text = "Click to Leave Active";
            // 
            // lbl_RefID
            // 
            this.lbl_RefID.AutoSize = true;
            this.lbl_RefID.Location = new System.Drawing.Point(13, 19);
            this.lbl_RefID.MaxFontSize = 0F;
            this.lbl_RefID.Name = "lbl_RefID";
            this.lbl_RefID.Size = new System.Drawing.Size(38, 13);
            this.lbl_RefID.TabIndex = 91;
            this.lbl_RefID.Text = "Ref ID";
            // 
            // tb_RefID
            // 
            this.tb_RefID.AllowedSpecialCharacters = " ";
            this.tb_RefID.Location = new System.Drawing.Point(70, 12);
            this.tb_RefID.Name = "tb_RefID";
            this.tb_RefID.ReadOnly = true;
            this.tb_RefID.Size = new System.Drawing.Size(126, 20);
            this.tb_RefID.TabIndex = 90;
            // 
            // ReferenceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_RefCountry);
            this.Controls.Add(this.tb_RefCountry);
            this.Controls.Add(this.lbl_RefPhone);
            this.Controls.Add(this.tb_RefPhone);
            this.Controls.Add(this.tb_RefZip);
            this.Controls.Add(this.lbl_RefZip);
            this.Controls.Add(this.cbo_RefState);
            this.Controls.Add(this.lbl_RefState);
            this.Controls.Add(this.lbl_RefCity);
            this.Controls.Add(this.tb_RefCity);
            this.Controls.Add(this.lbl_RefAddress2);
            this.Controls.Add(this.tb_RefAddress2);
            this.Controls.Add(this.lbl_RefAddress1);
            this.Controls.Add(this.tb_RefAddress1);
            this.Controls.Add(this.lbl_RefName);
            this.Controls.Add(this.tb_RefName);
            this.Controls.Add(this.rb_RefLeaveActive);
            this.Controls.Add(this.lbl_RefLeaveActive);
            this.Controls.Add(this.lbl_RefID);
            this.Controls.Add(this.tb_RefID);
            this.Name = "ReferenceControl";
            this.Size = new System.Drawing.Size(385, 182);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private Uheaa.Common.WinForms.AutoFontLabel lbl_RefCountry;
        private Uheaa.Common.WinForms.AlphaNumericTextBox tb_RefCountry;
        private Uheaa.Common.WinForms.AutoFontLabel lbl_RefPhone;
        private Uheaa.Common.WinForms.NumericTextBox tb_RefPhone;
        private Uheaa.Common.WinForms.AlphaNumericTextBox tb_RefZip;
        private Uheaa.Common.WinForms.AutoFontLabel lbl_RefZip;
        private System.Windows.Forms.ComboBox cbo_RefState;
        private Uheaa.Common.WinForms.AutoFontLabel lbl_RefState;
        private Uheaa.Common.WinForms.AutoFontLabel lbl_RefCity;
        private Uheaa.Common.WinForms.AlphaNumericTextBox tb_RefCity;
        private Uheaa.Common.WinForms.AutoFontLabel lbl_RefAddress2;
        private Uheaa.Common.WinForms.AlphaNumericTextBox tb_RefAddress2;
        private Uheaa.Common.WinForms.AutoFontLabel lbl_RefAddress1;
        private Uheaa.Common.WinForms.AlphaNumericTextBox tb_RefAddress1;
        private Uheaa.Common.WinForms.AutoFontLabel lbl_RefName;
        private Uheaa.Common.WinForms.AlphaNumericTextBox tb_RefName;
        private System.Windows.Forms.RadioButton rb_RefLeaveActive;
        private Uheaa.Common.WinForms.AutoFontLabel lbl_RefLeaveActive;
        private Uheaa.Common.WinForms.AutoFontLabel lbl_RefID;
        private Uheaa.Common.WinForms.AlphaNumericTextBox tb_RefID;
    }
}
