namespace PMTCANCL
{
    partial class UserQueryForm
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
            this.RegionLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.MadeAfterLabel = new System.Windows.Forms.Label();
            this.BorrowerLabel = new System.Windows.Forms.Label();
            this.SearchButton = new System.Windows.Forms.Button();
            this.UheaaRadioButton = new System.Windows.Forms.RadioButton();
            this.CornerstoneRadioButton = new System.Windows.Forms.RadioButton();
            this.MadeAfterDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.BorrowerTextBox = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ProcessedCheckBox = new System.Windows.Forms.CheckBox();
            this.UnprocessedCheckBox = new System.Windows.Forms.CheckBox();
            this.DateEnabled = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RegionLabel
            // 
            this.RegionLabel.AutoSize = true;
            this.RegionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.RegionLabel.Location = new System.Drawing.Point(5, 20);
            this.RegionLabel.Name = "RegionLabel";
            this.RegionLabel.Size = new System.Drawing.Size(60, 20);
            this.RegionLabel.TabIndex = 0;
            this.RegionLabel.Text = "Region";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.StatusLabel.Location = new System.Drawing.Point(5, 45);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(56, 20);
            this.StatusLabel.TabIndex = 3;
            this.StatusLabel.Text = "Status";
            // 
            // MadeAfterLabel
            // 
            this.MadeAfterLabel.AutoSize = true;
            this.MadeAfterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MadeAfterLabel.Location = new System.Drawing.Point(5, 70);
            this.MadeAfterLabel.Name = "MadeAfterLabel";
            this.MadeAfterLabel.Size = new System.Drawing.Size(113, 20);
            this.MadeAfterLabel.TabIndex = 6;
            this.MadeAfterLabel.Text = "Made On/After";
            // 
            // BorrowerLabel
            // 
            this.BorrowerLabel.AutoSize = true;
            this.BorrowerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.BorrowerLabel.Location = new System.Drawing.Point(5, 95);
            this.BorrowerLabel.Name = "BorrowerLabel";
            this.BorrowerLabel.Size = new System.Drawing.Size(73, 20);
            this.BorrowerLabel.TabIndex = 8;
            this.BorrowerLabel.Text = "Borrower";
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.SearchButton.Location = new System.Drawing.Point(278, 117);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 30);
            this.SearchButton.TabIndex = 10;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // UheaaRadioButton
            // 
            this.UheaaRadioButton.AutoSize = true;
            this.UheaaRadioButton.Checked = true;
            this.UheaaRadioButton.Location = new System.Drawing.Point(0, 8);
            this.UheaaRadioButton.Name = "UheaaRadioButton";
            this.UheaaRadioButton.Size = new System.Drawing.Size(57, 17);
            this.UheaaRadioButton.TabIndex = 1;
            this.UheaaRadioButton.TabStop = true;
            this.UheaaRadioButton.Text = "Uheaa";
            this.UheaaRadioButton.UseVisualStyleBackColor = true;
            this.UheaaRadioButton.CheckedChanged += new System.EventHandler(this.RegionCheckChanged);
            // 
            // CornerstoneRadioButton
            // 
            this.CornerstoneRadioButton.AutoSize = true;
            this.CornerstoneRadioButton.Location = new System.Drawing.Point(75, 8);
            this.CornerstoneRadioButton.Name = "CornerstoneRadioButton";
            this.CornerstoneRadioButton.Size = new System.Drawing.Size(82, 17);
            this.CornerstoneRadioButton.TabIndex = 2;
            this.CornerstoneRadioButton.TabStop = true;
            this.CornerstoneRadioButton.Text = "Cornerstone";
            this.CornerstoneRadioButton.UseVisualStyleBackColor = true;
            this.CornerstoneRadioButton.Visible = false;
            this.CornerstoneRadioButton.CheckedChanged += new System.EventHandler(this.RegionCheckChanged);
            // 
            // MadeAfterDateTimePicker
            // 
            this.MadeAfterDateTimePicker.Enabled = false;
            this.MadeAfterDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.MadeAfterDateTimePicker.Location = new System.Drawing.Point(208, 69);
            this.MadeAfterDateTimePicker.Name = "MadeAfterDateTimePicker";
            this.MadeAfterDateTimePicker.Size = new System.Drawing.Size(157, 20);
            this.MadeAfterDateTimePicker.TabIndex = 7;
            // 
            // BorrowerTextBox
            // 
            this.BorrowerTextBox.AccountNumber = null;
            this.BorrowerTextBox.AllowedSpecialCharacters = "";
            this.BorrowerTextBox.Location = new System.Drawing.Point(135, 95);
            this.BorrowerTextBox.MaxLength = 10;
            this.BorrowerTextBox.Name = "BorrowerTextBox";
            this.BorrowerTextBox.Size = new System.Drawing.Size(157, 20);
            this.BorrowerTextBox.Ssn = null;
            this.BorrowerTextBox.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.UheaaRadioButton);
            this.panel1.Controls.Add(this.CornerstoneRadioButton);
            this.panel1.Location = new System.Drawing.Point(135, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(163, 31);
            this.panel1.TabIndex = 1;
            // 
            // ProcessedCheckBox
            // 
            this.ProcessedCheckBox.AutoSize = true;
            this.ProcessedCheckBox.Checked = true;
            this.ProcessedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ProcessedCheckBox.Location = new System.Drawing.Point(135, 47);
            this.ProcessedCheckBox.Name = "ProcessedCheckBox";
            this.ProcessedCheckBox.Size = new System.Drawing.Size(76, 17);
            this.ProcessedCheckBox.TabIndex = 12;
            this.ProcessedCheckBox.Text = "Processed";
            this.ProcessedCheckBox.UseVisualStyleBackColor = true;
            this.ProcessedCheckBox.CheckedChanged += new System.EventHandler(this.ProcessedCheckChanged);
            // 
            // UnprocessedCheckBox
            // 
            this.UnprocessedCheckBox.AutoSize = true;
            this.UnprocessedCheckBox.Checked = true;
            this.UnprocessedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UnprocessedCheckBox.Location = new System.Drawing.Point(210, 47);
            this.UnprocessedCheckBox.Name = "UnprocessedCheckBox";
            this.UnprocessedCheckBox.Size = new System.Drawing.Size(89, 17);
            this.UnprocessedCheckBox.TabIndex = 13;
            this.UnprocessedCheckBox.Text = "Unprocessed";
            this.UnprocessedCheckBox.UseVisualStyleBackColor = true;
            this.UnprocessedCheckBox.CheckedChanged += new System.EventHandler(this.ProcessedCheckChanged);
            // 
            // DateEnabled
            // 
            this.DateEnabled.AutoSize = true;
            this.DateEnabled.Location = new System.Drawing.Point(135, 70);
            this.DateEnabled.Name = "DateEnabled";
            this.DateEnabled.Size = new System.Drawing.Size(59, 17);
            this.DateEnabled.TabIndex = 14;
            this.DateEnabled.Text = "Enable";
            this.DateEnabled.UseVisualStyleBackColor = true;
            this.DateEnabled.CheckedChanged += new System.EventHandler(this.DateEnabled_CheckedChanged);
            // 
            // UserQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 161);
            this.Controls.Add(this.DateEnabled);
            this.Controls.Add(this.UnprocessedCheckBox);
            this.Controls.Add(this.ProcessedCheckBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BorrowerTextBox);
            this.Controls.Add(this.MadeAfterDateTimePicker);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.BorrowerLabel);
            this.Controls.Add(this.MadeAfterLabel);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.RegionLabel);
            this.MinimumSize = new System.Drawing.Size(385, 200);
            this.Name = "UserQueryForm";
            this.Text = "UserQueryForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RegionLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Label MadeAfterLabel;
        private System.Windows.Forms.Label BorrowerLabel;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.RadioButton UheaaRadioButton;
        private System.Windows.Forms.RadioButton CornerstoneRadioButton;
        private System.Windows.Forms.DateTimePicker MadeAfterDateTimePicker;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox BorrowerTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox ProcessedCheckBox;
        private System.Windows.Forms.CheckBox UnprocessedCheckBox;
        private System.Windows.Forms.CheckBox DateEnabled;
    }
}