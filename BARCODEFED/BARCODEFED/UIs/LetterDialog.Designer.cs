namespace BARCODEFED
{
	partial class LetterDialog
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
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.barcodeInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRecipientId = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.LetterIds = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.barcodeInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker2.CustomFormat = "";
            this.dateTimePicker2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.barcodeInfoBindingSource, "CreateDate", true));
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(144, 98);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(152, 26);
            this.dateTimePicker2.TabIndex = 2;
            // 
            // barcodeInfoBindingSource
            // 
            this.barcodeInfoBindingSource.DataSource = typeof(BARCODEFED.BarcodeInfo);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Letter ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 105);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Date Sent";
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.Location = new System.Drawing.Point(184, 156);
            this.OK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(112, 35);
            this.OK.TabIndex = 3;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Recipient ID";
            // 
            // txtRecipientId
            // 
            this.txtRecipientId.AccountNumber = null;
            this.txtRecipientId.AllowedSpecialCharacters = "";
            this.txtRecipientId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.barcodeInfoBindingSource, "RecipientId", true));
            this.txtRecipientId.Location = new System.Drawing.Point(144, 20);
            this.txtRecipientId.MaxLength = 10;
            this.txtRecipientId.Name = "txtRecipientId";
            this.txtRecipientId.Size = new System.Drawing.Size(152, 26);
            this.txtRecipientId.Ssn = null;
            this.txtRecipientId.TabIndex = 0;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(20, 156);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(112, 35);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // LetterIds
            // 
            this.LetterIds.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.LetterIds.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.LetterIds.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.barcodeInfoBindingSource, "LetterId", true));
            this.LetterIds.FormattingEnabled = true;
            this.LetterIds.Location = new System.Drawing.Point(144, 60);
            this.LetterIds.Name = "LetterIds";
            this.LetterIds.Size = new System.Drawing.Size(152, 28);
            this.LetterIds.TabIndex = 1;
            // 
            // LetterDialog
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 205);
            this.Controls.Add(this.LetterIds);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.txtRecipientId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(332, 243);
            this.Name = "LetterDialog";
            this.Text = "Letter Info";
            ((System.ComponentModel.ISupportInitialize)(this.barcodeInfoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.DateTimePicker dateTimePicker2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.BindingSource barcodeInfoBindingSource;
		private System.Windows.Forms.Label label1;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox txtRecipientId;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.ComboBox LetterIds;
    }
}