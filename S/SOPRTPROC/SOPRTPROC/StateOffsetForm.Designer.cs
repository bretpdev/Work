namespace SOPRTPROC
{
    partial class StateOffsetForm
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
            this.StateOffset = new System.Windows.Forms.GroupBox();
            this.GarnishAmount = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.SSN = new Uheaa.Common.WinForms.WatermarkCurrencyTextBox();
            this.ListDateLabel = new System.Windows.Forms.Label();
            this.Joint = new System.Windows.Forms.RadioButton();
            this.Single = new System.Windows.Forms.RadioButton();
            this.WarrantNumber = new Uheaa.Common.WinForms.WatermarkNumericTextBox();
            this.ListDate = new Uheaa.Common.WinForms.ValidatableMaskedTextBox();
            this.ProcessOk = new System.Windows.Forms.Button();
            this.Borrower = new System.Windows.Forms.GroupBox();
            this.Update = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.State = new Uheaa.Common.WinForms.StateSelector();
            this.ZipCode = new Uheaa.Common.WinForms.WatermarkRequiredTextBox();
            this.City = new Uheaa.Common.WinForms.WatermarkRequiredTextBox();
            this.Address2 = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.Address1 = new Uheaa.Common.WinForms.WatermarkRequiredTextBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.StateOffset.SuspendLayout();
            this.Borrower.SuspendLayout();
            this.SuspendLayout();
            // 
            // StateOffset
            // 
            this.StateOffset.Controls.Add(this.GarnishAmount);
            this.StateOffset.Controls.Add(this.SSN);
            this.StateOffset.Controls.Add(this.ListDateLabel);
            this.StateOffset.Controls.Add(this.Joint);
            this.StateOffset.Controls.Add(this.Single);
            this.StateOffset.Controls.Add(this.WarrantNumber);
            this.StateOffset.Controls.Add(this.ListDate);
            this.StateOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StateOffset.Location = new System.Drawing.Point(18, 12);
            this.StateOffset.Name = "StateOffset";
            this.StateOffset.Size = new System.Drawing.Size(253, 198);
            this.StateOffset.TabIndex = 0;
            this.StateOffset.TabStop = false;
            this.StateOffset.Text = "State Offset Report Processing";
            // 
            // GarnishAmount
            // 
            this.GarnishAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.GarnishAmount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.GarnishAmount.Location = new System.Drawing.Point(22, 57);
            this.GarnishAmount.MaxLength = 8;
            this.GarnishAmount.Name = "GarnishAmount";
            this.GarnishAmount.Size = new System.Drawing.Size(182, 26);
            this.GarnishAmount.TabIndex = 2;
            this.GarnishAmount.Text = "Garnishment Amount";
            this.GarnishAmount.Watermark = "Garnishment Amount";
            this.GarnishAmount.Leave += new System.EventHandler(this.GarnishAmount_Leave);
            // 
            // SSN
            // 
            this.SSN.AllowedSpecialCharacters = "";
            this.SSN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.SSN.ForeColor = System.Drawing.SystemColors.WindowText;
            this.SSN.Location = new System.Drawing.Point(22, 25);
            this.SSN.MaxLength = 10;
            this.SSN.Name = "SSN";
            this.SSN.Size = new System.Drawing.Size(182, 26);
            this.SSN.TabIndex = 1;
            this.SSN.Text = "SSN / Account Number";
            this.SSN.Watermark = "SSN / Account Number";
            this.SSN.TextChanged += new System.EventHandler(this.SSN_TextChanged);
            // 
            // ListDateLabel
            // 
            this.ListDateLabel.AutoSize = true;
            this.ListDateLabel.Location = new System.Drawing.Point(119, 92);
            this.ListDateLabel.Name = "ListDateLabel";
            this.ListDateLabel.Size = new System.Drawing.Size(73, 20);
            this.ListDateLabel.TabIndex = 0;
            this.ListDateLabel.Text = "List Date";
            // 
            // Joint
            // 
            this.Joint.AutoSize = true;
            this.Joint.Location = new System.Drawing.Point(120, 155);
            this.Joint.Name = "Joint";
            this.Joint.Size = new System.Drawing.Size(61, 24);
            this.Joint.TabIndex = 6;
            this.Joint.TabStop = true;
            this.Joint.Text = "Joint";
            this.Joint.UseVisualStyleBackColor = true;
            // 
            // Single
            // 
            this.Single.AutoSize = true;
            this.Single.Location = new System.Drawing.Point(43, 155);
            this.Single.Name = "Single";
            this.Single.Size = new System.Drawing.Size(71, 24);
            this.Single.TabIndex = 5;
            this.Single.TabStop = true;
            this.Single.Text = "Single";
            this.Single.UseVisualStyleBackColor = true;
            // 
            // WarrantNumber
            // 
            this.WarrantNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.WarrantNumber.ForeColor = System.Drawing.SystemColors.WindowText;
            this.WarrantNumber.Location = new System.Drawing.Point(22, 120);
            this.WarrantNumber.MaxLength = 10;
            this.WarrantNumber.Name = "WarrantNumber";
            this.WarrantNumber.Size = new System.Drawing.Size(182, 26);
            this.WarrantNumber.TabIndex = 4;
            this.WarrantNumber.Text = "10 digit Warrant Number";
            this.WarrantNumber.Watermark = "10 digit Warrant Number";
            // 
            // ListDate
            // 
            this.ListDate.Location = new System.Drawing.Point(22, 89);
            this.ListDate.Mask = "00/00/0000";
            this.ListDate.Name = "ListDate";
            this.ListDate.Size = new System.Drawing.Size(91, 26);
            this.ListDate.TabIndex = 3;
            this.ListDate.ValidatingType = typeof(System.DateTime);
            // 
            // ProcessOk
            // 
            this.ProcessOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcessOk.Location = new System.Drawing.Point(517, 216);
            this.ProcessOk.Name = "ProcessOk";
            this.ProcessOk.Size = new System.Drawing.Size(89, 36);
            this.ProcessOk.TabIndex = 7;
            this.ProcessOk.Text = "OK";
            this.ProcessOk.UseVisualStyleBackColor = true;
            this.ProcessOk.Click += new System.EventHandler(this.ProcessOk_Click);
            // 
            // Borrower
            // 
            this.Borrower.Controls.Add(this.Update);
            this.Borrower.Controls.Add(this.label1);
            this.Borrower.Controls.Add(this.State);
            this.Borrower.Controls.Add(this.ZipCode);
            this.Borrower.Controls.Add(this.City);
            this.Borrower.Controls.Add(this.Address2);
            this.Borrower.Controls.Add(this.Address1);
            this.Borrower.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Borrower.Location = new System.Drawing.Point(308, 12);
            this.Borrower.Name = "Borrower";
            this.Borrower.Size = new System.Drawing.Size(298, 198);
            this.Borrower.TabIndex = 1;
            this.Borrower.TabStop = false;
            this.Borrower.Text = "Borrower Demographic Info";
            // 
            // Update
            // 
            this.Update.Location = new System.Drawing.Point(190, 148);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(89, 36);
            this.Update.TabIndex = 0;
            this.Update.Text = "Update";
            this.Update.UseVisualStyleBackColor = true;
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "State";
            // 
            // State
            // 
            this.State.Enabled = false;
            this.State.FormattingEnabled = true;
            this.State.Location = new System.Drawing.Point(6, 118);
            this.State.Name = "State";
            this.State.Size = new System.Drawing.Size(68, 28);
            this.State.TabIndex = 4;
            // 
            // ZipCode
            // 
            this.ZipCode.Enabled = false;
            this.ZipCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.ZipCode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ZipCode.Location = new System.Drawing.Point(6, 153);
            this.ZipCode.MaxLength = 9;
            this.ZipCode.Name = "ZipCode";
            this.ZipCode.Size = new System.Drawing.Size(91, 26);
            this.ZipCode.TabIndex = 5;
            this.ZipCode.Text = "Zip Code";
            this.ZipCode.Watermark = "Zip Code";
            // 
            // City
            // 
            this.City.Enabled = false;
            this.City.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.City.ForeColor = System.Drawing.SystemColors.WindowText;
            this.City.Location = new System.Drawing.Point(6, 89);
            this.City.Name = "City";
            this.City.Size = new System.Drawing.Size(273, 26);
            this.City.TabIndex = 3;
            this.City.Text = "City";
            this.City.Watermark = "City";
            // 
            // Address2
            // 
            this.Address2.Enabled = false;
            this.Address2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.Address2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Address2.Location = new System.Drawing.Point(6, 58);
            this.Address2.Name = "Address2";
            this.Address2.Size = new System.Drawing.Size(273, 26);
            this.Address2.TabIndex = 2;
            this.Address2.Text = "Address 2";
            this.Address2.Watermark = "Address 2";
            // 
            // Address1
            // 
            this.Address1.Enabled = false;
            this.Address1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.Address1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Address1.Location = new System.Drawing.Point(6, 27);
            this.Address1.Name = "Address1";
            this.Address1.Size = new System.Drawing.Size(273, 26);
            this.Address1.TabIndex = 1;
            this.Address1.Text = "Address 1";
            this.Address1.Watermark = "Address 1";
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.Location = new System.Drawing.Point(18, 216);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(89, 36);
            this.Cancel.TabIndex = 8;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // StateOffsetForm
            // 
            this.AcceptButton = this.ProcessOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(623, 266);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Borrower);
            this.Controls.Add(this.ProcessOk);
            this.Controls.Add(this.StateOffset);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(639, 305);
            this.MinimumSize = new System.Drawing.Size(639, 305);
            this.Name = "StateOffsetForm";
            this.ShowIcon = false;
            this.Text = "State Offset";
            this.StateOffset.ResumeLayout(false);
            this.StateOffset.PerformLayout();
            this.Borrower.ResumeLayout(false);
            this.Borrower.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox StateOffset;
        private System.Windows.Forms.GroupBox Borrower;
        private Uheaa.Common.WinForms.ValidatableMaskedTextBox ListDate;
        private System.Windows.Forms.RadioButton Joint;
        private System.Windows.Forms.RadioButton Single;
        private Uheaa.Common.WinForms.WatermarkNumericTextBox WarrantNumber;
        private Uheaa.Common.WinForms.StateSelector State;
        private Uheaa.Common.WinForms.WatermarkRequiredTextBox ZipCode;
        private Uheaa.Common.WinForms.WatermarkRequiredTextBox City;
        private Uheaa.Common.WinForms.WatermarkTextBox Address2;
        private Uheaa.Common.WinForms.WatermarkRequiredTextBox Address1;
        private System.Windows.Forms.Button ProcessOk;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label ListDateLabel;
        private System.Windows.Forms.Label label1;
        private Uheaa.Common.WinForms.WatermarkCurrencyTextBox SSN;
        private System.Windows.Forms.Button Update;
        private Uheaa.Common.WinForms.WatermarkTextBox GarnishAmount;
    }
}