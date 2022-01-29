namespace ImagingTransferFileBuilder
{
    partial class PdfScraperControl
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
            this.SettingsGroup = new System.Windows.Forms.GroupBox();
            this.LoanProgramTypeText = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.DocDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.DealIDText = new System.Windows.Forms.TextBox();
            this.SaleDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SheetNameText = new System.Windows.Forms.TextBox();
            this.MasterExcelLocationsBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.MasterExcelLocationText = new System.Windows.Forms.TextBox();
            this.PdfLocationText = new System.Windows.Forms.TextBox();
            this.PdfLocationBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ResultsLocationText = new System.Windows.Forms.TextBox();
            this.ResultsLocationBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ScrapeButton = new System.Windows.Forms.Button();
            this.OpenExcelDialog = new System.Windows.Forms.OpenFileDialog();
            this.SettingsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsGroup
            // 
            this.SettingsGroup.Controls.Add(this.LoanProgramTypeText);
            this.SettingsGroup.Controls.Add(this.label8);
            this.SettingsGroup.Controls.Add(this.label7);
            this.SettingsGroup.Controls.Add(this.DocDatePicker);
            this.SettingsGroup.Controls.Add(this.label5);
            this.SettingsGroup.Controls.Add(this.DealIDText);
            this.SettingsGroup.Controls.Add(this.SaleDatePicker);
            this.SettingsGroup.Controls.Add(this.label6);
            this.SettingsGroup.Controls.Add(this.label4);
            this.SettingsGroup.Controls.Add(this.SheetNameText);
            this.SettingsGroup.Controls.Add(this.MasterExcelLocationsBrowse);
            this.SettingsGroup.Controls.Add(this.label3);
            this.SettingsGroup.Controls.Add(this.MasterExcelLocationText);
            this.SettingsGroup.Controls.Add(this.PdfLocationText);
            this.SettingsGroup.Controls.Add(this.PdfLocationBrowse);
            this.SettingsGroup.Controls.Add(this.label2);
            this.SettingsGroup.Controls.Add(this.ResultsLocationText);
            this.SettingsGroup.Controls.Add(this.ResultsLocationBrowse);
            this.SettingsGroup.Controls.Add(this.label1);
            this.SettingsGroup.Location = new System.Drawing.Point(3, 3);
            this.SettingsGroup.Name = "SettingsGroup";
            this.SettingsGroup.Size = new System.Drawing.Size(385, 289);
            this.SettingsGroup.TabIndex = 27;
            this.SettingsGroup.TabStop = false;
            this.SettingsGroup.Text = "Settings";
            // 
            // LoanProgramTypeText
            // 
            this.LoanProgramTypeText.Location = new System.Drawing.Point(77, 253);
            this.LoanProgramTypeText.MaxLength = 4;
            this.LoanProgramTypeText.Name = "LoanProgramTypeText";
            this.LoanProgramTypeText.Size = new System.Drawing.Size(66, 20);
            this.LoanProgramTypeText.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(70, 234);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "Program Type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(260, 232);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "DocDate";
            // 
            // DocDatePicker
            // 
            this.DocDatePicker.CustomFormat = "MM/dd/yyyy";
            this.DocDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DocDatePicker.Location = new System.Drawing.Point(260, 252);
            this.DocDatePicker.Name = "DocDatePicker";
            this.DocDatePicker.Size = new System.Drawing.Size(105, 20);
            this.DocDatePicker.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(149, 233);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Sale Date";
            // 
            // DealIDText
            // 
            this.DealIDText.Location = new System.Drawing.Point(5, 253);
            this.DealIDText.MaxLength = 5;
            this.DealIDText.Name = "DealIDText";
            this.DealIDText.Size = new System.Drawing.Size(66, 20);
            this.DealIDText.TabIndex = 25;
            this.DealIDText.Text = "U0000";
            // 
            // SaleDatePicker
            // 
            this.SaleDatePicker.CustomFormat = "MM/dd/yyyy";
            this.SaleDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.SaleDatePicker.Location = new System.Drawing.Point(149, 253);
            this.SaleDatePicker.Name = "SaleDatePicker";
            this.SaleDatePicker.Size = new System.Drawing.Size(105, 20);
            this.SaleDatePicker.TabIndex = 27;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 234);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Deal ID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Sheet Name";
            // 
            // SheetNameText
            // 
            this.SheetNameText.Location = new System.Drawing.Point(5, 200);
            this.SheetNameText.Name = "SheetNameText";
            this.SheetNameText.Size = new System.Drawing.Size(363, 20);
            this.SheetNameText.TabIndex = 23;
            this.SheetNameText.Text = "Master";
            // 
            // MasterExcelLocationsBrowse
            // 
            this.MasterExcelLocationsBrowse.Location = new System.Drawing.Point(293, 171);
            this.MasterExcelLocationsBrowse.Name = "MasterExcelLocationsBrowse";
            this.MasterExcelLocationsBrowse.Size = new System.Drawing.Size(75, 23);
            this.MasterExcelLocationsBrowse.TabIndex = 22;
            this.MasterExcelLocationsBrowse.Text = "Browse...";
            this.MasterExcelLocationsBrowse.UseVisualStyleBackColor = true;
            this.MasterExcelLocationsBrowse.Click += new System.EventHandler(this.MasterExcelLocationsBrowse_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Master Excel Location";
            // 
            // MasterExcelLocationText
            // 
            this.MasterExcelLocationText.Location = new System.Drawing.Point(5, 145);
            this.MasterExcelLocationText.Name = "MasterExcelLocationText";
            this.MasterExcelLocationText.Size = new System.Drawing.Size(363, 20);
            this.MasterExcelLocationText.TabIndex = 20;
            // 
            // PdfLocationText
            // 
            this.PdfLocationText.Location = new System.Drawing.Point(5, 90);
            this.PdfLocationText.Name = "PdfLocationText";
            this.PdfLocationText.Size = new System.Drawing.Size(363, 20);
            this.PdfLocationText.TabIndex = 17;
            // 
            // PdfLocationBrowse
            // 
            this.PdfLocationBrowse.Location = new System.Drawing.Point(293, 116);
            this.PdfLocationBrowse.Name = "PdfLocationBrowse";
            this.PdfLocationBrowse.Size = new System.Drawing.Size(75, 23);
            this.PdfLocationBrowse.TabIndex = 19;
            this.PdfLocationBrowse.Text = "Browse...";
            this.PdfLocationBrowse.UseVisualStyleBackColor = true;
            this.PdfLocationBrowse.Click += new System.EventHandler(this.PdfLocationBrowse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "PDF Folder";
            // 
            // ResultsLocationText
            // 
            this.ResultsLocationText.Location = new System.Drawing.Point(5, 35);
            this.ResultsLocationText.Name = "ResultsLocationText";
            this.ResultsLocationText.Size = new System.Drawing.Size(363, 20);
            this.ResultsLocationText.TabIndex = 14;
            // 
            // ResultsLocationBrowse
            // 
            this.ResultsLocationBrowse.Location = new System.Drawing.Point(293, 61);
            this.ResultsLocationBrowse.Name = "ResultsLocationBrowse";
            this.ResultsLocationBrowse.Size = new System.Drawing.Size(75, 23);
            this.ResultsLocationBrowse.TabIndex = 16;
            this.ResultsLocationBrowse.Text = "Browse...";
            this.ResultsLocationBrowse.UseVisualStyleBackColor = true;
            this.ResultsLocationBrowse.Click += new System.EventHandler(this.ResultsLocationBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Save Results Location";
            // 
            // ScrapeButton
            // 
            this.ScrapeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScrapeButton.Location = new System.Drawing.Point(263, 298);
            this.ScrapeButton.Name = "ScrapeButton";
            this.ScrapeButton.Size = new System.Drawing.Size(124, 42);
            this.ScrapeButton.TabIndex = 30;
            this.ScrapeButton.Text = "Scrape";
            this.ScrapeButton.UseVisualStyleBackColor = true;
            this.ScrapeButton.Click += new System.EventHandler(this.ScrapeButton_Click);
            // 
            // OpenExcelDialog
            // 
            this.OpenExcelDialog.Filter = "Excel (*.xlsx;*.xls) |*.xlsx;*.xls";
            // 
            // PdfScraperControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SettingsGroup);
            this.Controls.Add(this.ScrapeButton);
            this.Name = "PdfScraperControl";
            this.Size = new System.Drawing.Size(393, 352);
            this.SettingsGroup.ResumeLayout(false);
            this.SettingsGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SettingsGroup;
        private System.Windows.Forms.TextBox ResultsLocationText;
        private System.Windows.Forms.Button ResultsLocationBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ScrapeButton;
        private System.Windows.Forms.TextBox PdfLocationText;
        private System.Windows.Forms.Button PdfLocationBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button MasterExcelLocationsBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox MasterExcelLocationText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SheetNameText;
        private System.Windows.Forms.OpenFileDialog OpenExcelDialog;
        private System.Windows.Forms.TextBox LoanProgramTypeText;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker DocDatePicker;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox DealIDText;
        private System.Windows.Forms.DateTimePicker SaleDatePicker;
        private System.Windows.Forms.Label label6;
    }
}
