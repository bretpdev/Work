namespace ImagingTransferFileBuilder
{
    partial class ImageScraperControl
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
            this.label4 = new System.Windows.Forms.Label();
            this.ScrapeButton = new System.Windows.Forms.Button();
            this.SheetNameText = new System.Windows.Forms.TextBox();
            this.MasterExcelLocationsBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.MasterExcelLocationText = new System.Windows.Forms.TextBox();
            this.SetupGroup = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.DateAfterPicker = new System.Windows.Forms.DateTimePicker();
            this.LoanProgramTypeText = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ResultsLocationBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ResultsLocationText = new System.Windows.Forms.TextBox();
            this.OpenExcelDialog = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.UsernameText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PasswordText = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DocDateCheck = new System.Windows.Forms.CheckBox();
            this.SetupGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Sheet Name";
            // 
            // ScrapeButton
            // 
            this.ScrapeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScrapeButton.Location = new System.Drawing.Point(263, 362);
            this.ScrapeButton.Name = "ScrapeButton";
            this.ScrapeButton.Size = new System.Drawing.Size(124, 42);
            this.ScrapeButton.TabIndex = 15;
            this.ScrapeButton.Text = "Scrape";
            this.ScrapeButton.UseVisualStyleBackColor = true;
            this.ScrapeButton.Click += new System.EventHandler(this.ScrapeButton_Click);
            // 
            // SheetNameText
            // 
            this.SheetNameText.Location = new System.Drawing.Point(6, 151);
            this.SheetNameText.Name = "SheetNameText";
            this.SheetNameText.Size = new System.Drawing.Size(363, 20);
            this.SheetNameText.TabIndex = 11;
            this.SheetNameText.Text = "Master";
            // 
            // MasterExcelLocationsBrowse
            // 
            this.MasterExcelLocationsBrowse.Location = new System.Drawing.Point(294, 122);
            this.MasterExcelLocationsBrowse.Name = "MasterExcelLocationsBrowse";
            this.MasterExcelLocationsBrowse.Size = new System.Drawing.Size(75, 23);
            this.MasterExcelLocationsBrowse.TabIndex = 10;
            this.MasterExcelLocationsBrowse.Text = "Browse...";
            this.MasterExcelLocationsBrowse.UseVisualStyleBackColor = true;
            this.MasterExcelLocationsBrowse.Click += new System.EventHandler(this.MasterExcelLocationsBrowse_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Master Excel Location";
            // 
            // MasterExcelLocationText
            // 
            this.MasterExcelLocationText.Location = new System.Drawing.Point(6, 96);
            this.MasterExcelLocationText.Name = "MasterExcelLocationText";
            this.MasterExcelLocationText.Size = new System.Drawing.Size(363, 20);
            this.MasterExcelLocationText.TabIndex = 8;
            // 
            // SetupGroup
            // 
            this.SetupGroup.Controls.Add(this.DocDateCheck);
            this.SetupGroup.Controls.Add(this.label6);
            this.SetupGroup.Controls.Add(this.DateAfterPicker);
            this.SetupGroup.Controls.Add(this.LoanProgramTypeText);
            this.SetupGroup.Controls.Add(this.label8);
            this.SetupGroup.Controls.Add(this.label4);
            this.SetupGroup.Controls.Add(this.SheetNameText);
            this.SetupGroup.Controls.Add(this.MasterExcelLocationsBrowse);
            this.SetupGroup.Controls.Add(this.label3);
            this.SetupGroup.Controls.Add(this.MasterExcelLocationText);
            this.SetupGroup.Controls.Add(this.ResultsLocationBrowse);
            this.SetupGroup.Controls.Add(this.label1);
            this.SetupGroup.Controls.Add(this.ResultsLocationText);
            this.SetupGroup.Location = new System.Drawing.Point(3, 3);
            this.SetupGroup.Name = "SetupGroup";
            this.SetupGroup.Size = new System.Drawing.Size(384, 237);
            this.SetupGroup.TabIndex = 14;
            this.SetupGroup.TabStop = false;
            this.SetupGroup.Text = "Setup";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(166, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 13);
            this.label6.TabIndex = 36;
            this.label6.Text = "Only documents after";
            // 
            // DateAfterPicker
            // 
            this.DateAfterPicker.Enabled = false;
            this.DateAfterPicker.Location = new System.Drawing.Point(169, 193);
            this.DateAfterPicker.Name = "DateAfterPicker";
            this.DateAfterPicker.Size = new System.Drawing.Size(200, 20);
            this.DateAfterPicker.TabIndex = 35;
            // 
            // LoanProgramTypeText
            // 
            this.LoanProgramTypeText.Location = new System.Drawing.Point(6, 193);
            this.LoanProgramTypeText.MaxLength = 4;
            this.LoanProgramTypeText.Name = "LoanProgramTypeText";
            this.LoanProgramTypeText.Size = new System.Drawing.Size(66, 20);
            this.LoanProgramTypeText.TabIndex = 33;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 177);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Program Type";
            // 
            // ResultsLocationBrowse
            // 
            this.ResultsLocationBrowse.Location = new System.Drawing.Point(294, 67);
            this.ResultsLocationBrowse.Name = "ResultsLocationBrowse";
            this.ResultsLocationBrowse.Size = new System.Drawing.Size(75, 23);
            this.ResultsLocationBrowse.TabIndex = 5;
            this.ResultsLocationBrowse.Text = "Browse...";
            this.ResultsLocationBrowse.UseVisualStyleBackColor = true;
            this.ResultsLocationBrowse.Click += new System.EventHandler(this.ResultsLocationBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Results Folder";
            // 
            // ResultsLocationText
            // 
            this.ResultsLocationText.Location = new System.Drawing.Point(6, 41);
            this.ResultsLocationText.Name = "ResultsLocationText";
            this.ResultsLocationText.Size = new System.Drawing.Size(363, 20);
            this.ResultsLocationText.TabIndex = 2;
            // 
            // OpenExcelDialog
            // 
            this.OpenExcelDialog.Filter = "Excel (*.xlsx;*.xls) |*.xlsx;*.xls";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Username";
            // 
            // UsernameText
            // 
            this.UsernameText.Location = new System.Drawing.Point(9, 35);
            this.UsernameText.Name = "UsernameText";
            this.UsernameText.Size = new System.Drawing.Size(363, 20);
            this.UsernameText.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Password";
            // 
            // PasswordText
            // 
            this.PasswordText.Location = new System.Drawing.Point(9, 77);
            this.PasswordText.Name = "PasswordText";
            this.PasswordText.PasswordChar = '*';
            this.PasswordText.Size = new System.Drawing.Size(363, 20);
            this.PasswordText.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.PasswordText);
            this.groupBox1.Controls.Add(this.UsernameText);
            this.groupBox1.Location = new System.Drawing.Point(3, 246);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 110);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login";
            // 
            // DocDateCheck
            // 
            this.DocDateCheck.AutoSize = true;
            this.DocDateCheck.Location = new System.Drawing.Point(148, 196);
            this.DocDateCheck.Name = "DocDateCheck";
            this.DocDateCheck.Size = new System.Drawing.Size(15, 14);
            this.DocDateCheck.TabIndex = 37;
            this.DocDateCheck.UseVisualStyleBackColor = true;
            this.DocDateCheck.CheckedChanged += new System.EventHandler(this.DocDateCheck_CheckedChanged);
            // 
            // ImageScraperControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ScrapeButton);
            this.Controls.Add(this.SetupGroup);
            this.Name = "ImageScraperControl";
            this.Size = new System.Drawing.Size(395, 416);
            this.SetupGroup.ResumeLayout(false);
            this.SetupGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button ScrapeButton;
        private System.Windows.Forms.TextBox SheetNameText;
        private System.Windows.Forms.Button MasterExcelLocationsBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox MasterExcelLocationText;
        private System.Windows.Forms.GroupBox SetupGroup;
        private System.Windows.Forms.Button ResultsLocationBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ResultsLocationText;
        private System.Windows.Forms.OpenFileDialog OpenExcelDialog;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox PasswordText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox UsernameText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox LoanProgramTypeText;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker DateAfterPicker;
        private System.Windows.Forms.CheckBox DocDateCheck;
    }
}
