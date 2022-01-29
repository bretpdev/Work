namespace INCARBWRS
{
	partial class PrisonDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrisonDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.prisonInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.contactSourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.Ssn = new Uheaa.Common.WinForms.WatermarkAccountIdentifierTextBox();
            this.PrisonName = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.PrisonAddress = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.PrisonCity = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.PrisonZip = new Uheaa.Common.WinForms.WatermarkNumericTextBox();
            this.PrisonPhone = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.PrisonInmateNumber = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.PrisonReleaseDate = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.PrisonFollowUpDate = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.watermarkTextBox1 = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.prisonInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contactSourceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(451, 141);
            this.label1.TabIndex = 17;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // prisonInfoBindingSource
            // 
            this.prisonInfoBindingSource.DataSource = typeof(INCARBWRS.PrisonInfo);
            // 
            // contactSourceBindingSource
            // 
            this.contactSourceBindingSource.DataSource = typeof(INCARBWRS.ContactSource);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(112, 625);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(96, 46);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(249, 625);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 46);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label4.Location = new System.Drawing.Point(338, 285);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 20);
            this.label4.TabIndex = 16;
            this.label4.Text = "State *";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.prisonInfoBindingSource, "Contact", true));
            this.comboBox1.DataSource = this.contactSourceBindingSource;
            this.comboBox1.DisplayMember = "Source";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(30, 521);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(369, 28);
            this.comboBox1.TabIndex = 10;
            // 
            // cmbState
            // 
            this.cmbState.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cmbState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.prisonInfoBindingSource, "State", true));
            this.cmbState.FormattingEnabled = true;
            this.cmbState.Location = new System.Drawing.Point(258, 280);
            this.cmbState.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(73, 28);
            this.cmbState.TabIndex = 4;
            // 
            // Ssn
            // 
            this.Ssn.AccountNumber = null;
            this.Ssn.AllowedSpecialCharacters = "";
            this.Ssn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Ssn.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.prisonInfoBindingSource, "SSN", true));
            this.Ssn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.Ssn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Ssn.Location = new System.Drawing.Point(30, 163);
            this.Ssn.MaxLength = 10;
            this.Ssn.Name = "Ssn";
            this.Ssn.Size = new System.Drawing.Size(137, 26);
            this.Ssn.Ssn = null;
            this.Ssn.TabIndex = 0;
            this.Ssn.Text = "SSN / Account # *";
            this.Ssn.Watermark = "SSN / Account # *";
            // 
            // PrisonName
            // 
            this.PrisonName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PrisonName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.prisonInfoBindingSource, "Name", true));
            this.PrisonName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.PrisonName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PrisonName.Location = new System.Drawing.Point(30, 203);
            this.PrisonName.Name = "PrisonName";
            this.PrisonName.Size = new System.Drawing.Size(314, 26);
            this.PrisonName.TabIndex = 1;
            this.PrisonName.Text = "Prison Name *";
            this.PrisonName.Watermark = "Prison Name *";
            // 
            // PrisonAddress
            // 
            this.PrisonAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PrisonAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.prisonInfoBindingSource, "Address", true));
            this.PrisonAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.PrisonAddress.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PrisonAddress.Location = new System.Drawing.Point(30, 240);
            this.PrisonAddress.Name = "PrisonAddress";
            this.PrisonAddress.Size = new System.Drawing.Size(314, 26);
            this.PrisonAddress.TabIndex = 2;
            this.PrisonAddress.Text = "Prison Address *";
            this.PrisonAddress.Watermark = "Prison Address *";
            // 
            // PrisonCity
            // 
            this.PrisonCity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PrisonCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.prisonInfoBindingSource, "City", true));
            this.PrisonCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.PrisonCity.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PrisonCity.Location = new System.Drawing.Point(30, 280);
            this.PrisonCity.Name = "PrisonCity";
            this.PrisonCity.Size = new System.Drawing.Size(222, 26);
            this.PrisonCity.TabIndex = 3;
            this.PrisonCity.Text = "Prison City *";
            this.PrisonCity.Watermark = "Prison City *";
            // 
            // PrisonZip
            // 
            this.PrisonZip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PrisonZip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.prisonInfoBindingSource, "ZIP", true));
            this.PrisonZip.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.PrisonZip.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PrisonZip.Location = new System.Drawing.Point(30, 320);
            this.PrisonZip.MaxLength = 9;
            this.PrisonZip.Name = "PrisonZip";
            this.PrisonZip.Size = new System.Drawing.Size(101, 26);
            this.PrisonZip.TabIndex = 5;
            this.PrisonZip.Text = "Zip Code *";
            this.PrisonZip.Watermark = "Zip Code *";
            // 
            // PrisonPhone
            // 
            this.PrisonPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PrisonPhone.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.prisonInfoBindingSource, "Phone", true));
            this.PrisonPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.PrisonPhone.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PrisonPhone.Location = new System.Drawing.Point(30, 363);
            this.PrisonPhone.MaxLength = 10;
            this.PrisonPhone.Name = "PrisonPhone";
            this.PrisonPhone.Size = new System.Drawing.Size(101, 26);
            this.PrisonPhone.TabIndex = 6;
            this.PrisonPhone.Text = "Phone *";
            this.PrisonPhone.Watermark = "Phone *";
            // 
            // PrisonInmateNumber
            // 
            this.PrisonInmateNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PrisonInmateNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.prisonInfoBindingSource, "InmateNumber", true));
            this.PrisonInmateNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.PrisonInmateNumber.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PrisonInmateNumber.Location = new System.Drawing.Point(30, 403);
            this.PrisonInmateNumber.Name = "PrisonInmateNumber";
            this.PrisonInmateNumber.Size = new System.Drawing.Size(101, 26);
            this.PrisonInmateNumber.TabIndex = 7;
            this.PrisonInmateNumber.Text = "Inmate #";
            this.PrisonInmateNumber.Watermark = "Inmate #";
            // 
            // PrisonReleaseDate
            // 
            this.PrisonReleaseDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PrisonReleaseDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.prisonInfoBindingSource, "AnticipatedReleaseDate", true));
            this.PrisonReleaseDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.PrisonReleaseDate.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PrisonReleaseDate.Location = new System.Drawing.Point(30, 443);
            this.PrisonReleaseDate.Name = "PrisonReleaseDate";
            this.PrisonReleaseDate.Size = new System.Drawing.Size(137, 26);
            this.PrisonReleaseDate.TabIndex = 8;
            this.PrisonReleaseDate.Text = "Release Date";
            this.PrisonReleaseDate.Watermark = "Release Date";
            // 
            // PrisonFollowUpDate
            // 
            this.PrisonFollowUpDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PrisonFollowUpDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.prisonInfoBindingSource, "FollowUpDate", true));
            this.PrisonFollowUpDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.PrisonFollowUpDate.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PrisonFollowUpDate.Location = new System.Drawing.Point(30, 480);
            this.PrisonFollowUpDate.Name = "PrisonFollowUpDate";
            this.PrisonFollowUpDate.Size = new System.Drawing.Size(137, 26);
            this.PrisonFollowUpDate.TabIndex = 9;
            this.PrisonFollowUpDate.Text = "Follow Up Date *";
            this.PrisonFollowUpDate.Watermark = "Follow Up Date *";
            // 
            // watermarkTextBox1
            // 
            this.watermarkTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.watermarkTextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.prisonInfoBindingSource, "OtherInfo", true));
            this.watermarkTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.watermarkTextBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.watermarkTextBox1.Location = new System.Drawing.Point(30, 564);
            this.watermarkTextBox1.Name = "watermarkTextBox1";
            this.watermarkTextBox1.Size = new System.Drawing.Size(369, 26);
            this.watermarkTextBox1.TabIndex = 11;
            this.watermarkTextBox1.Text = "Other Information";
            this.watermarkTextBox1.Watermark = "Other Information";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label2.Location = new System.Drawing.Point(407, 524);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.TabIndex = 15;
            this.label2.Text = "Source *";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(27, 603);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 17);
            this.label3.TabIndex = 18;
            this.label3.Text = "* Required";
            // 
            // PrisonDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(494, 676);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.watermarkTextBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.PrisonFollowUpDate);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.PrisonReleaseDate);
            this.Controls.Add(this.PrisonInmateNumber);
            this.Controls.Add(this.PrisonPhone);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PrisonZip);
            this.Controls.Add(this.Ssn);
            this.Controls.Add(this.PrisonCity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PrisonAddress);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.PrisonName);
            this.Controls.Add(this.cmbState);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(510, 715);
            this.Name = "PrisonDialog";
            this.Text = "Incarcerated Borrower Script";
            ((System.ComponentModel.ISupportInitialize)(this.prisonInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contactSourceBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.BindingSource prisonInfoBindingSource;
        private System.Windows.Forms.BindingSource contactSourceBindingSource;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox cmbState;
        private Uheaa.Common.WinForms.WatermarkAccountIdentifierTextBox Ssn;
        private Uheaa.Common.WinForms.WatermarkTextBox PrisonName;
        private Uheaa.Common.WinForms.WatermarkTextBox PrisonAddress;
        private Uheaa.Common.WinForms.WatermarkTextBox PrisonCity;
        private Uheaa.Common.WinForms.WatermarkNumericTextBox PrisonZip;
        private Uheaa.Common.WinForms.WatermarkTextBox PrisonPhone;
        private Uheaa.Common.WinForms.WatermarkTextBox PrisonInmateNumber;
        private Uheaa.Common.WinForms.WatermarkTextBox PrisonReleaseDate;
        private Uheaa.Common.WinForms.WatermarkTextBox PrisonFollowUpDate;
        private Uheaa.Common.WinForms.WatermarkTextBox watermarkTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}