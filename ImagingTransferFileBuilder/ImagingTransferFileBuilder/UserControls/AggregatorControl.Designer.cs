namespace ImagingTransferFileBuilder
{
    partial class AggregatorControl
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
            this.ResultsLocationText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ResultsLocationBrowse = new System.Windows.Forms.Button();
            this.DealIDText = new System.Windows.Forms.TextBox();
            this.SaleDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SettingsGroup = new System.Windows.Forms.GroupBox();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.LoanProgramTypeText = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SettingsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ResultsLocationText
            // 
            this.ResultsLocationText.Location = new System.Drawing.Point(5, 35);
            this.ResultsLocationText.Name = "ResultsLocationText";
            this.ResultsLocationText.Size = new System.Drawing.Size(363, 20);
            this.ResultsLocationText.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Sale Date";
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
            // DealIDText
            // 
            this.DealIDText.Location = new System.Drawing.Point(5, 90);
            this.DealIDText.MaxLength = 5;
            this.DealIDText.Name = "DealIDText";
            this.DealIDText.Size = new System.Drawing.Size(66, 20);
            this.DealIDText.TabIndex = 18;
            this.DealIDText.Text = "U0000";
            // 
            // SaleDatePicker
            // 
            this.SaleDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.SaleDatePicker.Location = new System.Drawing.Point(77, 90);
            this.SaleDatePicker.Name = "SaleDatePicker";
            this.SaleDatePicker.Size = new System.Drawing.Size(105, 20);
            this.SaleDatePicker.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Deal ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Results Folder";
            // 
            // SettingsGroup
            // 
            this.SettingsGroup.Controls.Add(this.LoanProgramTypeText);
            this.SettingsGroup.Controls.Add(this.label8);
            this.SettingsGroup.Controls.Add(this.ResultsLocationText);
            this.SettingsGroup.Controls.Add(this.label3);
            this.SettingsGroup.Controls.Add(this.ResultsLocationBrowse);
            this.SettingsGroup.Controls.Add(this.DealIDText);
            this.SettingsGroup.Controls.Add(this.SaleDatePicker);
            this.SettingsGroup.Controls.Add(this.label2);
            this.SettingsGroup.Controls.Add(this.label1);
            this.SettingsGroup.Location = new System.Drawing.Point(3, 3);
            this.SettingsGroup.Name = "SettingsGroup";
            this.SettingsGroup.Size = new System.Drawing.Size(385, 124);
            this.SettingsGroup.TabIndex = 25;
            this.SettingsGroup.TabStop = false;
            this.SettingsGroup.Text = "Settings";
            // 
            // GenerateButton
            // 
            this.GenerateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateButton.Location = new System.Drawing.Point(247, 133);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(124, 42);
            this.GenerateButton.TabIndex = 24;
            this.GenerateButton.Text = "Aggregate";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // LoanProgramTypeText
            // 
            this.LoanProgramTypeText.Location = new System.Drawing.Point(188, 90);
            this.LoanProgramTypeText.MaxLength = 4;
            this.LoanProgramTypeText.Name = "LoanProgramTypeText";
            this.LoanProgramTypeText.Size = new System.Drawing.Size(66, 20);
            this.LoanProgramTypeText.TabIndex = 35;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(185, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Program Type";
            // 
            // AggregatorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SettingsGroup);
            this.Controls.Add(this.GenerateButton);
            this.Name = "AggregatorControl";
            this.Size = new System.Drawing.Size(395, 188);
            this.SettingsGroup.ResumeLayout(false);
            this.SettingsGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox ResultsLocationText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ResultsLocationBrowse;
        private System.Windows.Forms.TextBox DealIDText;
        private System.Windows.Forms.DateTimePicker SaleDatePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox SettingsGroup;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.TextBox LoanProgramTypeText;
        private System.Windows.Forms.Label label8;
    }
}
