namespace ImagingTransferFileBuilder
{
    partial class ValidatorControl
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
            this.OpenZipDialog = new System.Windows.Forms.OpenFileDialog();
            this.SettingsGroup = new System.Windows.Forms.GroupBox();
            this.ZipLocationText = new System.Windows.Forms.TextBox();
            this.ZipLocationBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ValidateButton = new System.Windows.Forms.Button();
            this.SheetNameText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.MasterExcelLocationText = new System.Windows.Forms.TextBox();
            this.MasterExcelLocationBrowse = new System.Windows.Forms.Button();
            this.OpenExcelDialog = new System.Windows.Forms.OpenFileDialog();
            this.SettingsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenZipDialog
            // 
            this.OpenZipDialog.Filter = "Zip Files (*.zip)|*.zip";
            // 
            // SettingsGroup
            // 
            this.SettingsGroup.Controls.Add(this.MasterExcelLocationBrowse);
            this.SettingsGroup.Controls.Add(this.label3);
            this.SettingsGroup.Controls.Add(this.MasterExcelLocationText);
            this.SettingsGroup.Controls.Add(this.SheetNameText);
            this.SettingsGroup.Controls.Add(this.label2);
            this.SettingsGroup.Controls.Add(this.ZipLocationText);
            this.SettingsGroup.Controls.Add(this.ZipLocationBrowse);
            this.SettingsGroup.Controls.Add(this.label1);
            this.SettingsGroup.Location = new System.Drawing.Point(3, 3);
            this.SettingsGroup.Name = "SettingsGroup";
            this.SettingsGroup.Size = new System.Drawing.Size(385, 205);
            this.SettingsGroup.TabIndex = 27;
            this.SettingsGroup.TabStop = false;
            this.SettingsGroup.Text = "Settings";
            // 
            // ZipLocationText
            // 
            this.ZipLocationText.Location = new System.Drawing.Point(9, 35);
            this.ZipLocationText.Name = "ZipLocationText";
            this.ZipLocationText.Size = new System.Drawing.Size(363, 20);
            this.ZipLocationText.TabIndex = 14;
            // 
            // ZipLocationBrowse
            // 
            this.ZipLocationBrowse.Location = new System.Drawing.Point(293, 61);
            this.ZipLocationBrowse.Name = "ZipLocationBrowse";
            this.ZipLocationBrowse.Size = new System.Drawing.Size(75, 23);
            this.ZipLocationBrowse.TabIndex = 16;
            this.ZipLocationBrowse.Text = "Browse...";
            this.ZipLocationBrowse.UseVisualStyleBackColor = true;
            this.ZipLocationBrowse.Click += new System.EventHandler(this.ZipLocationBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Zip File Location";
            // 
            // ValidateButton
            // 
            this.ValidateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValidateButton.Location = new System.Drawing.Point(264, 210);
            this.ValidateButton.Name = "ValidateButton";
            this.ValidateButton.Size = new System.Drawing.Size(124, 42);
            this.ValidateButton.TabIndex = 26;
            this.ValidateButton.Text = "Validate";
            this.ValidateButton.UseVisualStyleBackColor = true;
            this.ValidateButton.Click += new System.EventHandler(this.ValidateButton_Click);
            // 
            // SheetNameText
            // 
            this.SheetNameText.Location = new System.Drawing.Point(9, 160);
            this.SheetNameText.Name = "SheetNameText";
            this.SheetNameText.Size = new System.Drawing.Size(363, 20);
            this.SheetNameText.TabIndex = 17;
            this.SheetNameText.Text = "Master";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Master Sheet Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Master Excel Location";
            // 
            // MasterExcelLocationText
            // 
            this.MasterExcelLocationText.Location = new System.Drawing.Point(9, 90);
            this.MasterExcelLocationText.Name = "MasterExcelLocationText";
            this.MasterExcelLocationText.Size = new System.Drawing.Size(363, 20);
            this.MasterExcelLocationText.TabIndex = 19;
            // 
            // MasterExcelLocationBrowse
            // 
            this.MasterExcelLocationBrowse.Location = new System.Drawing.Point(293, 116);
            this.MasterExcelLocationBrowse.Name = "MasterExcelLocationBrowse";
            this.MasterExcelLocationBrowse.Size = new System.Drawing.Size(75, 23);
            this.MasterExcelLocationBrowse.TabIndex = 21;
            this.MasterExcelLocationBrowse.Text = "Browse...";
            this.MasterExcelLocationBrowse.UseVisualStyleBackColor = true;
            this.MasterExcelLocationBrowse.Click += new System.EventHandler(this.MasterExcelLocationBrowse_Click);
            // 
            // OpenExcelDialog
            // 
            this.OpenExcelDialog.Filter = "Excel (*.xlsx;*.xls) |*.xlsx;*.xls";
            // 
            // ValidatorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SettingsGroup);
            this.Controls.Add(this.ValidateButton);
            this.Name = "ValidatorControl";
            this.Size = new System.Drawing.Size(398, 264);
            this.SettingsGroup.ResumeLayout(false);
            this.SettingsGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OpenZipDialog;
        private System.Windows.Forms.GroupBox SettingsGroup;
        private System.Windows.Forms.TextBox SheetNameText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ZipLocationText;
        private System.Windows.Forms.Button ZipLocationBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ValidateButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox MasterExcelLocationText;
        private System.Windows.Forms.Button MasterExcelLocationBrowse;
        private System.Windows.Forms.OpenFileDialog OpenExcelDialog;
    }
}
