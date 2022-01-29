namespace OALETTERS
{
    partial class LetterSelection
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
            this.LetterId = new System.Windows.Forms.ComboBox();
            this.AccountIdentifier = new Uheaa.Common.WinForms.WatermarkAccountIdentifierTextBox();
            this.BorrowerName = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.Address1 = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.Address2 = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.City = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.State = new Uheaa.Common.WinForms.StateSelector();
            this.Zip = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.RefundAmount = new Uheaa.Common.WinForms.WatermarkCurrencyTextBox();
            this.EffectiveDate = new Uheaa.Common.WinForms.MaskedDateTextBox();
            this.EffDate = new System.Windows.Forms.Label();
            this.Country = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.Ok = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Company = new System.Windows.Forms.Button();
            this.PaymentSource = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.UtId = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.Pwd = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.SuspendLayout();
            // 
            // LetterId
            // 
            this.LetterId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LetterId.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.LetterId.FormattingEnabled = true;
            this.LetterId.Location = new System.Drawing.Point(13, 14);
            this.LetterId.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LetterId.Name = "LetterId";
            this.LetterId.Size = new System.Drawing.Size(403, 28);
            this.LetterId.TabIndex = 0;
            this.LetterId.SelectedIndexChanged += new System.EventHandler(this.LetterId_SelectedIndexChanged);
            // 
            // AccountIdentifier
            // 
            this.AccountIdentifier.AccountNumber = null;
            this.AccountIdentifier.AllowedSpecialCharacters = "";
            this.AccountIdentifier.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountIdentifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.AccountIdentifier.ForeColor = System.Drawing.SystemColors.WindowText;
            this.AccountIdentifier.Location = new System.Drawing.Point(13, 50);
            this.AccountIdentifier.MaxLength = 10;
            this.AccountIdentifier.Name = "AccountIdentifier";
            this.AccountIdentifier.Size = new System.Drawing.Size(156, 26);
            this.AccountIdentifier.Ssn = null;
            this.AccountIdentifier.TabIndex = 1;
            this.AccountIdentifier.Text = "SSN / Acct Num";
            this.AccountIdentifier.Watermark = "SSN / Acct Num";
            this.AccountIdentifier.TextChanged += new System.EventHandler(this.AccountIdentifier_TextChanged);
            // 
            // BorrowerName
            // 
            this.BorrowerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BorrowerName.Enabled = false;
            this.BorrowerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.BorrowerName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.BorrowerName.Location = new System.Drawing.Point(13, 84);
            this.BorrowerName.MaxLength = 150;
            this.BorrowerName.Name = "BorrowerName";
            this.BorrowerName.Size = new System.Drawing.Size(403, 26);
            this.BorrowerName.TabIndex = 10;
            this.BorrowerName.Text = "Borrower Name";
            this.BorrowerName.Watermark = "Borrower Name";
            // 
            // Address1
            // 
            this.Address1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Address1.Enabled = false;
            this.Address1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.Address1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Address1.Location = new System.Drawing.Point(13, 116);
            this.Address1.MaxLength = 200;
            this.Address1.Name = "Address1";
            this.Address1.Size = new System.Drawing.Size(403, 26);
            this.Address1.TabIndex = 11;
            this.Address1.Text = "Address1";
            this.Address1.Watermark = "Address1";
            // 
            // Address2
            // 
            this.Address2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Address2.Enabled = false;
            this.Address2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.Address2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Address2.Location = new System.Drawing.Point(13, 148);
            this.Address2.MaxLength = 200;
            this.Address2.Name = "Address2";
            this.Address2.Size = new System.Drawing.Size(403, 26);
            this.Address2.TabIndex = 12;
            this.Address2.Text = "Address2";
            this.Address2.Watermark = "Address2";
            // 
            // City
            // 
            this.City.Enabled = false;
            this.City.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.City.ForeColor = System.Drawing.SystemColors.WindowText;
            this.City.Location = new System.Drawing.Point(13, 182);
            this.City.MaxLength = 150;
            this.City.Name = "City";
            this.City.Size = new System.Drawing.Size(186, 26);
            this.City.TabIndex = 13;
            this.City.Text = "City";
            this.City.Watermark = "City";
            // 
            // State
            // 
            this.State.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.State.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.State.Enabled = false;
            this.State.FormattingEnabled = true;
            this.State.Location = new System.Drawing.Point(205, 182);
            this.State.Name = "State";
            this.State.Size = new System.Drawing.Size(75, 28);
            this.State.TabIndex = 14;
            // 
            // Zip
            // 
            this.Zip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Zip.Enabled = false;
            this.Zip.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.Zip.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Zip.Location = new System.Drawing.Point(286, 182);
            this.Zip.MaxLength = 5;
            this.Zip.Name = "Zip";
            this.Zip.Size = new System.Drawing.Size(130, 26);
            this.Zip.TabIndex = 15;
            this.Zip.Text = "Zip Code";
            this.Zip.Watermark = "Zip Code";
            // 
            // RefundAmount
            // 
            this.RefundAmount.AllowedSpecialCharacters = ".";
            this.RefundAmount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefundAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.RefundAmount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.RefundAmount.Location = new System.Drawing.Point(13, 252);
            this.RefundAmount.Name = "RefundAmount";
            this.RefundAmount.Size = new System.Drawing.Size(136, 26);
            this.RefundAmount.TabIndex = 2;
            this.RefundAmount.Text = "Refund Amount";
            this.RefundAmount.Watermark = "Refund Amount";
            this.RefundAmount.Leave += new System.EventHandler(this.RefundAmount_Leave);
            // 
            // EffectiveDate
            // 
            this.EffectiveDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EffectiveDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.EffectiveDate.Location = new System.Drawing.Point(326, 255);
            this.EffectiveDate.Mask = "00/00/0000";
            this.EffectiveDate.Name = "EffectiveDate";
            this.EffectiveDate.Size = new System.Drawing.Size(90, 26);
            this.EffectiveDate.TabIndex = 3;
            // 
            // EffDate
            // 
            this.EffDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EffDate.AutoSize = true;
            this.EffDate.Location = new System.Drawing.Point(212, 258);
            this.EffDate.Name = "EffDate";
            this.EffDate.Size = new System.Drawing.Size(110, 20);
            this.EffDate.TabIndex = 11;
            this.EffDate.Text = "Effective Date";
            // 
            // Country
            // 
            this.Country.Enabled = false;
            this.Country.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.Country.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Country.Location = new System.Drawing.Point(13, 212);
            this.Country.MaxLength = 150;
            this.Country.Name = "Country";
            this.Country.Size = new System.Drawing.Size(186, 26);
            this.Country.TabIndex = 16;
            this.Country.Text = "Country";
            this.Country.Watermark = "Country";
            // 
            // Ok
            // 
            this.Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ok.Location = new System.Drawing.Point(316, 356);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(100, 37);
            this.Ok.TabIndex = 8;
            this.Ok.Text = "Finished";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(12, 356);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(100, 37);
            this.Cancel.TabIndex = 9;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Company
            // 
            this.Company.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Company.Location = new System.Drawing.Point(286, 214);
            this.Company.Name = "Company";
            this.Company.Size = new System.Drawing.Size(130, 35);
            this.Company.TabIndex = 7;
            this.Company.Text = "Company";
            this.Company.UseVisualStyleBackColor = true;
            this.Company.Click += new System.EventHandler(this.Company_Click);
            // 
            // PaymentSource
            // 
            this.PaymentSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PaymentSource.Enabled = false;
            this.PaymentSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.PaymentSource.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PaymentSource.Location = new System.Drawing.Point(13, 288);
            this.PaymentSource.MaxLength = 150;
            this.PaymentSource.Name = "PaymentSource";
            this.PaymentSource.Size = new System.Drawing.Size(136, 26);
            this.PaymentSource.TabIndex = 4;
            this.PaymentSource.Text = "Payment Source";
            this.PaymentSource.Watermark = "Payment Source";
            // 
            // UtId
            // 
            this.UtId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.UtId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.UtId.ForeColor = System.Drawing.SystemColors.WindowText;
            this.UtId.Location = new System.Drawing.Point(79, 320);
            this.UtId.MaxLength = 7;
            this.UtId.Name = "UtId";
            this.UtId.Size = new System.Drawing.Size(136, 26);
            this.UtId.TabIndex = 5;
            this.UtId.Text = "UT ID";
            this.UtId.Watermark = "UT ID";
            // 
            // Pwd
            // 
            this.Pwd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Pwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.Pwd.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Pwd.Location = new System.Drawing.Point(231, 320);
            this.Pwd.MaxLength = 8;
            this.Pwd.MinimumSize = new System.Drawing.Size(142, 26);
            this.Pwd.Name = "Pwd";
            this.Pwd.PasswordChar = '*';
            this.Pwd.Size = new System.Drawing.Size(142, 26);
            this.Pwd.TabIndex = 6;
            this.Pwd.Text = "Password";
            this.Pwd.Watermark = "Password";
            // 
            // LetterSelection
            // 
            this.AcceptButton = this.Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(434, 405);
            this.Controls.Add(this.Pwd);
            this.Controls.Add(this.UtId);
            this.Controls.Add(this.PaymentSource);
            this.Controls.Add(this.Company);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.Country);
            this.Controls.Add(this.EffDate);
            this.Controls.Add(this.EffectiveDate);
            this.Controls.Add(this.RefundAmount);
            this.Controls.Add(this.Zip);
            this.Controls.Add(this.State);
            this.Controls.Add(this.City);
            this.Controls.Add(this.Address2);
            this.Controls.Add(this.Address1);
            this.Controls.Add(this.BorrowerName);
            this.Controls.Add(this.AccountIdentifier);
            this.Controls.Add(this.LetterId);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 443);
            this.Name = "LetterSelection";
            this.Text = "Operation Accounting Letters";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox LetterId;
        private Uheaa.Common.WinForms.WatermarkAccountIdentifierTextBox AccountIdentifier;
        private Uheaa.Common.WinForms.WatermarkTextBox BorrowerName;
        private Uheaa.Common.WinForms.WatermarkTextBox Address1;
        private Uheaa.Common.WinForms.WatermarkTextBox Address2;
        private Uheaa.Common.WinForms.WatermarkTextBox City;
        private Uheaa.Common.WinForms.StateSelector State;
        private Uheaa.Common.WinForms.WatermarkTextBox Zip;
        private Uheaa.Common.WinForms.WatermarkCurrencyTextBox RefundAmount;
        private Uheaa.Common.WinForms.MaskedDateTextBox EffectiveDate;
        private System.Windows.Forms.Label EffDate;
        private Uheaa.Common.WinForms.WatermarkTextBox Country;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Company;
        private Uheaa.Common.WinForms.WatermarkTextBox PaymentSource;
        private Uheaa.Common.WinForms.WatermarkTextBox UtId;
        private Uheaa.Common.WinForms.WatermarkTextBox Pwd;
    }
}

