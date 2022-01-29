namespace ImagingTransferFileBuilder
{
    partial class GeneratorControl
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
            this.GenerateButton = new System.Windows.Forms.Button();
            this.SetupGroup = new System.Windows.Forms.GroupBox();
            this.ReplaceCheck = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SheetNameText = new System.Windows.Forms.TextBox();
            this.MasterExcelLocationsBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.MasterExcelLocationText = new System.Windows.Forms.TextBox();
            this.LoadLocationsRemoveButton = new System.Windows.Forms.Button();
            this.LoadLocationsAddButton = new System.Windows.Forms.Button();
            this.ResultsLocationBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ResultsLocationText = new System.Windows.Forms.TextBox();
            this.LoadLocationsList = new System.Windows.Forms.ListBox();
            this.OpenExcelDialog = new System.Windows.Forms.OpenFileDialog();
            this.SetupGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // GenerateButton
            // 
            this.GenerateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateButton.Location = new System.Drawing.Point(263, 366);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(124, 42);
            this.GenerateButton.TabIndex = 13;
            this.GenerateButton.Text = "Generate";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // SetupGroup
            // 
            this.SetupGroup.Controls.Add(this.ReplaceCheck);
            this.SetupGroup.Controls.Add(this.label4);
            this.SetupGroup.Controls.Add(this.SheetNameText);
            this.SetupGroup.Controls.Add(this.MasterExcelLocationsBrowse);
            this.SetupGroup.Controls.Add(this.label3);
            this.SetupGroup.Controls.Add(this.MasterExcelLocationText);
            this.SetupGroup.Controls.Add(this.LoadLocationsRemoveButton);
            this.SetupGroup.Controls.Add(this.LoadLocationsAddButton);
            this.SetupGroup.Controls.Add(this.ResultsLocationBrowse);
            this.SetupGroup.Controls.Add(this.label2);
            this.SetupGroup.Controls.Add(this.label1);
            this.SetupGroup.Controls.Add(this.ResultsLocationText);
            this.SetupGroup.Controls.Add(this.LoadLocationsList);
            this.SetupGroup.Location = new System.Drawing.Point(3, 3);
            this.SetupGroup.Name = "SetupGroup";
            this.SetupGroup.Size = new System.Drawing.Size(384, 357);
            this.SetupGroup.TabIndex = 12;
            this.SetupGroup.TabStop = false;
            this.SetupGroup.Text = "Setup";
            // 
            // ReplaceCheck
            // 
            this.ReplaceCheck.AutoSize = true;
            this.ReplaceCheck.Checked = true;
            this.ReplaceCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ReplaceCheck.Location = new System.Drawing.Point(10, 67);
            this.ReplaceCheck.Name = "ReplaceCheck";
            this.ReplaceCheck.Size = new System.Drawing.Size(146, 17);
            this.ReplaceCheck.TabIndex = 13;
            this.ReplaceCheck.Text = "Clear any previous results";
            this.ReplaceCheck.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 298);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Sheet Name";
            // 
            // SheetNameText
            // 
            this.SheetNameText.Location = new System.Drawing.Point(6, 317);
            this.SheetNameText.Name = "SheetNameText";
            this.SheetNameText.Size = new System.Drawing.Size(363, 20);
            this.SheetNameText.TabIndex = 11;
            this.SheetNameText.Text = "Master";
            // 
            // MasterExcelLocationsBrowse
            // 
            this.MasterExcelLocationsBrowse.Location = new System.Drawing.Point(294, 288);
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
            this.label3.Location = new System.Drawing.Point(7, 243);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Master Excel Location";
            // 
            // MasterExcelLocationText
            // 
            this.MasterExcelLocationText.Location = new System.Drawing.Point(6, 262);
            this.MasterExcelLocationText.Name = "MasterExcelLocationText";
            this.MasterExcelLocationText.Size = new System.Drawing.Size(363, 20);
            this.MasterExcelLocationText.TabIndex = 8;
            // 
            // LoadLocationsRemoveButton
            // 
            this.LoadLocationsRemoveButton.Enabled = false;
            this.LoadLocationsRemoveButton.Location = new System.Drawing.Point(6, 206);
            this.LoadLocationsRemoveButton.Name = "LoadLocationsRemoveButton";
            this.LoadLocationsRemoveButton.Size = new System.Drawing.Size(116, 23);
            this.LoadLocationsRemoveButton.TabIndex = 7;
            this.LoadLocationsRemoveButton.Text = "Remove Selected";
            this.LoadLocationsRemoveButton.UseVisualStyleBackColor = true;
            this.LoadLocationsRemoveButton.Click += new System.EventHandler(this.LoadLocationsRemoveButton_Click);
            // 
            // LoadLocationsAddButton
            // 
            this.LoadLocationsAddButton.Location = new System.Drawing.Point(294, 206);
            this.LoadLocationsAddButton.Name = "LoadLocationsAddButton";
            this.LoadLocationsAddButton.Size = new System.Drawing.Size(75, 23);
            this.LoadLocationsAddButton.TabIndex = 6;
            this.LoadLocationsAddButton.Text = "Add...";
            this.LoadLocationsAddButton.UseVisualStyleBackColor = true;
            this.LoadLocationsAddButton.Click += new System.EventHandler(this.LoadLocationsAddButton_Click);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Load Locations";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
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
            // LoadLocationsList
            // 
            this.LoadLocationsList.FormattingEnabled = true;
            this.LoadLocationsList.Location = new System.Drawing.Point(6, 105);
            this.LoadLocationsList.Name = "LoadLocationsList";
            this.LoadLocationsList.Size = new System.Drawing.Size(363, 95);
            this.LoadLocationsList.TabIndex = 0;
            this.LoadLocationsList.SelectedIndexChanged += new System.EventHandler(this.LoadLocationsList_SelectedIndexChanged);
            // 
            // OpenExcelDialog
            // 
            this.OpenExcelDialog.Filter = "Excel (*.xlsx;*.xls) |*.xlsx;*.xls";
            // 
            // GeneratorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.SetupGroup);
            this.Name = "GeneratorControl";
            this.Size = new System.Drawing.Size(399, 422);
            this.SetupGroup.ResumeLayout(false);
            this.SetupGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.GroupBox SetupGroup;
        private System.Windows.Forms.CheckBox ReplaceCheck;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SheetNameText;
        private System.Windows.Forms.Button MasterExcelLocationsBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox MasterExcelLocationText;
        private System.Windows.Forms.Button LoadLocationsRemoveButton;
        private System.Windows.Forms.Button LoadLocationsAddButton;
        private System.Windows.Forms.Button ResultsLocationBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ResultsLocationText;
        private System.Windows.Forms.ListBox LoadLocationsList;
        private System.Windows.Forms.OpenFileDialog OpenExcelDialog;
    }
}
