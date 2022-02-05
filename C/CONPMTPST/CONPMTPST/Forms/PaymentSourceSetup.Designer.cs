namespace CONPMTPST
{
    partial class PaymentSourceSetup
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
            this.PaymentSource = new System.Windows.Forms.ComboBox();
            this.PaymentSourceLbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FileTypeLbl = new System.Windows.Forms.Label();
            this.FileType = new System.Windows.Forms.ComboBox();
            this.Active = new System.Windows.Forms.CheckBox();
            this.Ok = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.InstitutionId = new Uheaa.Common.WinForms.ValidatableTextBox();
            this.FileName = new Uheaa.Common.WinForms.ValidatableTextBox();
            this.SuspendLayout();
            // 
            // PaymentSource
            // 
            this.PaymentSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PaymentSource.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.PaymentSource.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.PaymentSource.FormattingEnabled = true;
            this.PaymentSource.Location = new System.Drawing.Point(18, 39);
            this.PaymentSource.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PaymentSource.Name = "PaymentSource";
            this.PaymentSource.Size = new System.Drawing.Size(288, 28);
            this.PaymentSource.TabIndex = 0;
            this.PaymentSource.SelectedIndexChanged += new System.EventHandler(this.PaymentSource_SelectedIndexChanged);
            // 
            // PaymentSourceLbl
            // 
            this.PaymentSourceLbl.AutoSize = true;
            this.PaymentSourceLbl.Location = new System.Drawing.Point(18, 14);
            this.PaymentSourceLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PaymentSourceLbl.Name = "PaymentSourceLbl";
            this.PaymentSourceLbl.Size = new System.Drawing.Size(126, 20);
            this.PaymentSourceLbl.TabIndex = 1;
            this.PaymentSourceLbl.Text = "Payment Source";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 79);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Institution ID";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 140);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "File Name";
            // 
            // FileTypeLbl
            // 
            this.FileTypeLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.FileTypeLbl.AutoSize = true;
            this.FileTypeLbl.Location = new System.Drawing.Point(18, 206);
            this.FileTypeLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FileTypeLbl.Name = "FileTypeLbl";
            this.FileTypeLbl.Size = new System.Drawing.Size(72, 20);
            this.FileTypeLbl.TabIndex = 7;
            this.FileTypeLbl.Text = "File Type";
            // 
            // FileType
            // 
            this.FileType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.FileType.FormattingEnabled = true;
            this.FileType.Items.AddRange(new object[] {
            "Flat",
            "Direct",
            "Comma Delimited"});
            this.FileType.Location = new System.Drawing.Point(18, 231);
            this.FileType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FileType.Name = "FileType";
            this.FileType.Size = new System.Drawing.Size(148, 28);
            this.FileType.TabIndex = 3;
            // 
            // Active
            // 
            this.Active.AutoSize = true;
            this.Active.Checked = true;
            this.Active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Active.Location = new System.Drawing.Point(22, 271);
            this.Active.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Active.Name = "Active";
            this.Active.Size = new System.Drawing.Size(71, 24);
            this.Active.TabIndex = 4;
            this.Active.Text = "Active";
            this.Active.UseVisualStyleBackColor = true;
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(194, 316);
            this.Ok.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(112, 35);
            this.Ok.TabIndex = 5;
            this.Ok.Text = "OK";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(18, 316);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(112, 35);
            this.Cancel.TabIndex = 6;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // InstitutionId
            // 
            this.InstitutionId.Location = new System.Drawing.Point(22, 102);
            this.InstitutionId.MaxLength = 6;
            this.InstitutionId.Name = "InstitutionId";
            this.InstitutionId.Size = new System.Drawing.Size(100, 26);
            this.InstitutionId.TabIndex = 1;
            // 
            // FileName
            // 
            this.FileName.Location = new System.Drawing.Point(22, 163);
            this.FileName.MaxLength = 25;
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(284, 26);
            this.FileName.TabIndex = 2;
            // 
            // PaymentSourceSetup
            // 
            this.AcceptButton = this.Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(336, 375);
            this.Controls.Add(this.FileName);
            this.Controls.Add(this.InstitutionId);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.Active);
            this.Controls.Add(this.FileTypeLbl);
            this.Controls.Add(this.FileType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PaymentSourceLbl);
            this.Controls.Add(this.PaymentSource);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(352, 413);
            this.MinimumSize = new System.Drawing.Size(352, 413);
            this.Name = "PaymentSourceSetup";
            this.Text = "Payment Source Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox PaymentSource;
        private System.Windows.Forms.Label PaymentSourceLbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label FileTypeLbl;
        private System.Windows.Forms.ComboBox FileType;
        private System.Windows.Forms.CheckBox Active;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Cancel;
        private Uheaa.Common.WinForms.ValidatableTextBox InstitutionId;
        private Uheaa.Common.WinForms.ValidatableTextBox FileName;
    }
}