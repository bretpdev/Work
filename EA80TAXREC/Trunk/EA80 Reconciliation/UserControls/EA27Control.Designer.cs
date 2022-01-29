namespace EA80Reconciliation.UserControls
{
    partial class EA27Control
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
			this.label5 = new System.Windows.Forms.Label();
			this.LoanSaleDatePicker = new System.Windows.Forms.DateTimePicker();
			this.EA80FolderLocationText = new System.Windows.Forms.TextBox();
			this.EA80LocationBrowse = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.EA27LocationText = new System.Windows.Forms.TextBox();
			this.EA27LocationBrowse = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.StartReconciliationButton = new System.Windows.Forms.Button();
			this.OpenExcelDialog = new System.Windows.Forms.OpenFileDialog();
			this.SettingsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// SettingsGroup
			// 
			this.SettingsGroup.Controls.Add(this.label5);
			this.SettingsGroup.Controls.Add(this.LoanSaleDatePicker);
			this.SettingsGroup.Controls.Add(this.EA80FolderLocationText);
			this.SettingsGroup.Controls.Add(this.EA80LocationBrowse);
			this.SettingsGroup.Controls.Add(this.label2);
			this.SettingsGroup.Controls.Add(this.EA27LocationText);
			this.SettingsGroup.Controls.Add(this.EA27LocationBrowse);
			this.SettingsGroup.Controls.Add(this.label1);
			this.SettingsGroup.Location = new System.Drawing.Point(3, 3);
			this.SettingsGroup.MinimumSize = new System.Drawing.Size(385, 289);
			this.SettingsGroup.Name = "SettingsGroup";
			this.SettingsGroup.Size = new System.Drawing.Size(385, 289);
			this.SettingsGroup.TabIndex = 31;
			this.SettingsGroup.TabStop = false;
			this.SettingsGroup.Text = "Settings";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(2, 232);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(81, 13);
			this.label5.TabIndex = 28;
			this.label5.Text = "Loan Sale Date";
			// 
			// LoanSaleDatePicker
			// 
			this.LoanSaleDatePicker.CustomFormat = "MM/dd/yyyy";
			this.LoanSaleDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.LoanSaleDatePicker.Location = new System.Drawing.Point(5, 252);
			this.LoanSaleDatePicker.Name = "LoanSaleDatePicker";
			this.LoanSaleDatePicker.Size = new System.Drawing.Size(105, 20);
			this.LoanSaleDatePicker.TabIndex = 27;
			// 
			// EA80FolderLocationText
			// 
			this.EA80FolderLocationText.Location = new System.Drawing.Point(5, 141);
			this.EA80FolderLocationText.Name = "EA80FolderLocationText";
			this.EA80FolderLocationText.Size = new System.Drawing.Size(363, 20);
			this.EA80FolderLocationText.TabIndex = 17;
			// 
			// EA80LocationBrowse
			// 
			this.EA80LocationBrowse.Location = new System.Drawing.Point(293, 167);
			this.EA80LocationBrowse.Name = "EA80LocationBrowse";
			this.EA80LocationBrowse.Size = new System.Drawing.Size(75, 23);
			this.EA80LocationBrowse.TabIndex = 19;
			this.EA80LocationBrowse.Text = "Browse...";
			this.EA80LocationBrowse.UseVisualStyleBackColor = true;
			this.EA80LocationBrowse.Click += new System.EventHandler(this.EA80FolderLocationBrowse_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(2, 125);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(109, 13);
			this.label2.TabIndex = 18;
			this.label2.Text = "EA80 Folder Location";
			// 
			// EA27LocationText
			// 
			this.EA27LocationText.Location = new System.Drawing.Point(5, 35);
			this.EA27LocationText.Name = "EA27LocationText";
			this.EA27LocationText.Size = new System.Drawing.Size(363, 20);
			this.EA27LocationText.TabIndex = 14;
			// 
			// EA27LocationBrowse
			// 
			this.EA27LocationBrowse.Location = new System.Drawing.Point(293, 61);
			this.EA27LocationBrowse.Name = "EA27LocationBrowse";
			this.EA27LocationBrowse.Size = new System.Drawing.Size(75, 23);
			this.EA27LocationBrowse.TabIndex = 16;
			this.EA27LocationBrowse.Text = "Browse...";
			this.EA27LocationBrowse.UseVisualStyleBackColor = true;
			this.EA27LocationBrowse.Click += new System.EventHandler(this.EA27LocationBrowse_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(2, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 13);
			this.label1.TabIndex = 15;
			this.label1.Text = "EA27 File Location";
			// 
			// StartReconciliationButton
			// 
			this.StartReconciliationButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StartReconciliationButton.Location = new System.Drawing.Point(263, 298);
			this.StartReconciliationButton.Name = "StartReconciliationButton";
			this.StartReconciliationButton.Size = new System.Drawing.Size(124, 49);
			this.StartReconciliationButton.TabIndex = 32;
			this.StartReconciliationButton.Text = "Start Reconciliation";
			this.StartReconciliationButton.UseVisualStyleBackColor = true;
			this.StartReconciliationButton.Click += new System.EventHandler(this.StartReconciliationButton_Click);
			// 
			// OpenExcelDialog
			// 
			this.OpenExcelDialog.Filter = "Excel (*.xlsx;*.xls) |*.xlsx;*.xls";
			// 
			// EA27Control
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.SettingsGroup);
			this.Controls.Add(this.StartReconciliationButton);
			this.Name = "EA27Control";
			this.Size = new System.Drawing.Size(398, 350);
			this.SettingsGroup.ResumeLayout(false);
			this.SettingsGroup.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.GroupBox SettingsGroup;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.DateTimePicker LoanSaleDatePicker;
		private System.Windows.Forms.TextBox EA80FolderLocationText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox EA27LocationText;
        private System.Windows.Forms.Button EA27LocationBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartReconciliationButton;
        private System.Windows.Forms.OpenFileDialog OpenExcelDialog;
		private System.Windows.Forms.Button EA80LocationBrowse;
    }
}
