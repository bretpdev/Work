namespace IDRUSERPRO
{
    partial class IncomeInformation
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
            this.AgiFromTaxesBox = new Uheaa.Common.WinForms.NumericDecimalTextBox();
            this.ReceivedDateBox = new System.Windows.Forms.MaskedTextBox();
            this.SupportingDocsRequiredLabel = new System.Windows.Forms.Label();
            this.TaxableIncomeLabel = new System.Windows.Forms.Label();
            this.TaxYearBox = new Uheaa.Common.WinForms.NumericTextBox();
            this.TaxYearLabel = new System.Windows.Forms.Label();
            this.AgiFromTaxesLabel = new System.Windows.Forms.Label();
            this.IncomeSourceBox = new System.Windows.Forms.Label();
            this.AltIncomeBox = new System.Windows.Forms.Label();
            this.TotalAltIncomeBox = new System.Windows.Forms.Label();
            this.CalcButton = new System.Windows.Forms.Button();
            this.AltIncomeLabel = new System.Windows.Forms.Label();
            this.TotalAltIncomeLabel = new System.Windows.Forms.Label();
            this.ReceivedDateLabel = new System.Windows.Forms.Label();
            this.AgiReflectsCurrentIncomeLabel = new System.Windows.Forms.Label();
            this.IncomeSourceLabel = new System.Windows.Forms.Label();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.TaxableIncomeBox = new Uheaa.Common.WinForms.YesNoComboBox();
            this.SupportingDocsRequiredBox = new Uheaa.Common.WinForms.YesNoComboBox();
            this.AgiPanel = new System.Windows.Forms.Panel();
            this.AltIncomePanel = new System.Windows.Forms.Panel();
            this.AgiReflectsCurrentIncomeBox = new System.Windows.Forms.ComboBox();
            this.SpouseFilingStatusBox = new System.Windows.Forms.ComboBox();
            this.SpouseFilingStatusLabel = new System.Windows.Forms.Label();
            this.StateBox = new Uheaa.Common.WinForms.AlphaTextBox();
            this.StateLabel = new System.Windows.Forms.Label();
            this.SpouseFilingStatusPanel = new System.Windows.Forms.Panel();
            this.StatePanel = new System.Windows.Forms.Panel();
            this.AgiPanel.SuspendLayout();
            this.AltIncomePanel.SuspendLayout();
            this.SpouseFilingStatusPanel.SuspendLayout();
            this.StatePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // AgiFromTaxesBox
            // 
            this.AgiFromTaxesBox.AllowedSpecialCharacters = "";
            this.AgiFromTaxesBox.Location = new System.Drawing.Point(148, 7);
            this.AgiFromTaxesBox.Name = "AgiFromTaxesBox";
            this.AgiFromTaxesBox.Size = new System.Drawing.Size(100, 26);
            this.AgiFromTaxesBox.TabIndex = 0;
            this.AgiFromTaxesBox.Leave += new System.EventHandler(this.AgiFromTaxesBox_Leave);
            // 
            // ReceivedDateBox
            // 
            this.ReceivedDateBox.Location = new System.Drawing.Point(221, 176);
            this.ReceivedDateBox.Mask = "00/00/0000";
            this.ReceivedDateBox.Name = "ReceivedDateBox";
            this.ReceivedDateBox.Size = new System.Drawing.Size(92, 26);
            this.ReceivedDateBox.TabIndex = 3;
            this.ReceivedDateBox.ValidatingType = typeof(System.DateTime);
            this.ReceivedDateBox.TextChanged += new System.EventHandler(this.Control_Changed);
            // 
            // SupportingDocsRequiredLabel
            // 
            this.SupportingDocsRequiredLabel.AutoSize = true;
            this.SupportingDocsRequiredLabel.Location = new System.Drawing.Point(4, 144);
            this.SupportingDocsRequiredLabel.Name = "SupportingDocsRequiredLabel";
            this.SupportingDocsRequiredLabel.Size = new System.Drawing.Size(201, 20);
            this.SupportingDocsRequiredLabel.TabIndex = 48;
            this.SupportingDocsRequiredLabel.Text = "Supporting Docs Required:";
            // 
            // TaxableIncomeLabel
            // 
            this.TaxableIncomeLabel.AutoSize = true;
            this.TaxableIncomeLabel.Location = new System.Drawing.Point(80, 113);
            this.TaxableIncomeLabel.Name = "TaxableIncomeLabel";
            this.TaxableIncomeLabel.Size = new System.Drawing.Size(125, 20);
            this.TaxableIncomeLabel.TabIndex = 47;
            this.TaxableIncomeLabel.Text = "Taxable Income:";
            // 
            // TaxYearBox
            // 
            this.TaxYearBox.AllowedSpecialCharacters = "";
            this.TaxYearBox.Location = new System.Drawing.Point(148, 38);
            this.TaxYearBox.MaxLength = 4;
            this.TaxYearBox.Name = "TaxYearBox";
            this.TaxYearBox.Size = new System.Drawing.Size(57, 26);
            this.TaxYearBox.TabIndex = 1;
            this.TaxYearBox.TextChanged += new System.EventHandler(this.Control_Changed);
            // 
            // TaxYearLabel
            // 
            this.TaxYearLabel.AutoSize = true;
            this.TaxYearLabel.Location = new System.Drawing.Point(56, 42);
            this.TaxYearLabel.Name = "TaxYearLabel";
            this.TaxYearLabel.Size = new System.Drawing.Size(76, 20);
            this.TaxYearLabel.TabIndex = 46;
            this.TaxYearLabel.Text = "Tax Year:";
            // 
            // AgiFromTaxesLabel
            // 
            this.AgiFromTaxesLabel.AutoSize = true;
            this.AgiFromTaxesLabel.Location = new System.Drawing.Point(3, 10);
            this.AgiFromTaxesLabel.Name = "AgiFromTaxesLabel";
            this.AgiFromTaxesLabel.Size = new System.Drawing.Size(129, 20);
            this.AgiFromTaxesLabel.TabIndex = 45;
            this.AgiFromTaxesLabel.Text = "AGI From Taxes:";
            // 
            // IncomeSourceBox
            // 
            this.IncomeSourceBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IncomeSourceBox.Location = new System.Drawing.Point(171, 38);
            this.IncomeSourceBox.Name = "IncomeSourceBox";
            this.IncomeSourceBox.Size = new System.Drawing.Size(142, 20);
            this.IncomeSourceBox.TabIndex = 44;
            // 
            // AltIncomeBox
            // 
            this.AltIncomeBox.Location = new System.Drawing.Point(163, 9);
            this.AltIncomeBox.Name = "AltIncomeBox";
            this.AltIncomeBox.Size = new System.Drawing.Size(102, 20);
            this.AltIncomeBox.TabIndex = 43;
            this.AltIncomeBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AltIncomeBox.TextChanged += new System.EventHandler(this.AltIncomeBox_TextChanged);
            // 
            // TotalAltIncomeBox
            // 
            this.TotalAltIncomeBox.Location = new System.Drawing.Point(163, 42);
            this.TotalAltIncomeBox.Name = "TotalAltIncomeBox";
            this.TotalAltIncomeBox.Size = new System.Drawing.Size(102, 20);
            this.TotalAltIncomeBox.TabIndex = 42;
            this.TotalAltIncomeBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CalcButton
            // 
            this.CalcButton.Location = new System.Drawing.Point(271, 7);
            this.CalcButton.Name = "CalcButton";
            this.CalcButton.Size = new System.Drawing.Size(87, 26);
            this.CalcButton.TabIndex = 0;
            this.CalcButton.Text = "Paystubs";
            this.CalcButton.UseVisualStyleBackColor = true;
            this.CalcButton.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // AltIncomeLabel
            // 
            this.AltIncomeLabel.AutoSize = true;
            this.AltIncomeLabel.Location = new System.Drawing.Point(8, 10);
            this.AltIncomeLabel.Name = "AltIncomeLabel";
            this.AltIncomeLabel.Size = new System.Drawing.Size(157, 20);
            this.AltIncomeLabel.TabIndex = 40;
            this.AltIncomeLabel.Text = "Borrower Alt Income:";
            // 
            // TotalAltIncomeLabel
            // 
            this.TotalAltIncomeLabel.AutoSize = true;
            this.TotalAltIncomeLabel.Location = new System.Drawing.Point(37, 42);
            this.TotalAltIncomeLabel.Name = "TotalAltIncomeLabel";
            this.TotalAltIncomeLabel.Size = new System.Drawing.Size(128, 20);
            this.TotalAltIncomeLabel.TabIndex = 39;
            this.TotalAltIncomeLabel.Text = "Total Alt Income:";
            // 
            // ReceivedDateLabel
            // 
            this.ReceivedDateLabel.AutoSize = true;
            this.ReceivedDateLabel.Location = new System.Drawing.Point(87, 180);
            this.ReceivedDateLabel.Name = "ReceivedDateLabel";
            this.ReceivedDateLabel.Size = new System.Drawing.Size(118, 20);
            this.ReceivedDateLabel.TabIndex = 31;
            this.ReceivedDateLabel.Text = "Received Date:";
            // 
            // AgiReflectsCurrentIncomeLabel
            // 
            this.AgiReflectsCurrentIncomeLabel.Location = new System.Drawing.Point(80, 63);
            this.AgiReflectsCurrentIncomeLabel.Name = "AgiReflectsCurrentIncomeLabel";
            this.AgiReflectsCurrentIncomeLabel.Size = new System.Drawing.Size(125, 44);
            this.AgiReflectsCurrentIncomeLabel.TabIndex = 29;
            this.AgiReflectsCurrentIncomeLabel.Text = "AGI Reflects Current Income:";
            // 
            // IncomeSourceLabel
            // 
            this.IncomeSourceLabel.AutoSize = true;
            this.IncomeSourceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IncomeSourceLabel.Location = new System.Drawing.Point(32, 39);
            this.IncomeSourceLabel.Name = "IncomeSourceLabel";
            this.IncomeSourceLabel.Size = new System.Drawing.Size(135, 20);
            this.IncomeSourceLabel.TabIndex = 28;
            this.IncomeSourceLabel.Text = "Income Source:";
            // 
            // TitleLabel
            // 
            this.TitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(51, 14);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(297, 20);
            this.TitleLabel.TabIndex = 25;
            this.TitleLabel.Text = "Borrower\'s  Income";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TaxableIncomeBox
            // 
            this.TaxableIncomeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TaxableIncomeBox.FormattingEnabled = true;
            this.TaxableIncomeBox.Location = new System.Drawing.Point(221, 110);
            this.TaxableIncomeBox.Name = "TaxableIncomeBox";
            this.TaxableIncomeBox.SelectedValue = null;
            this.TaxableIncomeBox.Size = new System.Drawing.Size(121, 28);
            this.TaxableIncomeBox.TabIndex = 1;
            this.TaxableIncomeBox.SelectedValueChanged += new System.EventHandler(this.Control_Changed);
            // 
            // SupportingDocsRequiredBox
            // 
            this.SupportingDocsRequiredBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SupportingDocsRequiredBox.FormattingEnabled = true;
            this.SupportingDocsRequiredBox.Location = new System.Drawing.Point(221, 142);
            this.SupportingDocsRequiredBox.Name = "SupportingDocsRequiredBox";
            this.SupportingDocsRequiredBox.SelectedValue = null;
            this.SupportingDocsRequiredBox.Size = new System.Drawing.Size(121, 28);
            this.SupportingDocsRequiredBox.TabIndex = 2;
            this.SupportingDocsRequiredBox.SelectedValueChanged += new System.EventHandler(this.Control_Changed);
            // 
            // AgiPanel
            // 
            this.AgiPanel.Controls.Add(this.AgiFromTaxesLabel);
            this.AgiPanel.Controls.Add(this.TaxYearLabel);
            this.AgiPanel.Controls.Add(this.TaxYearBox);
            this.AgiPanel.Controls.Add(this.AgiFromTaxesBox);
            this.AgiPanel.Location = new System.Drawing.Point(73, 229);
            this.AgiPanel.Name = "AgiPanel";
            this.AgiPanel.Size = new System.Drawing.Size(260, 71);
            this.AgiPanel.TabIndex = 6;
            // 
            // AltIncomePanel
            // 
            this.AltIncomePanel.Controls.Add(this.AltIncomeLabel);
            this.AltIncomePanel.Controls.Add(this.TotalAltIncomeLabel);
            this.AltIncomePanel.Controls.Add(this.TotalAltIncomeBox);
            this.AltIncomePanel.Controls.Add(this.AltIncomeBox);
            this.AltIncomePanel.Controls.Add(this.CalcButton);
            this.AltIncomePanel.Location = new System.Drawing.Point(39, 229);
            this.AltIncomePanel.Name = "AltIncomePanel";
            this.AltIncomePanel.Size = new System.Drawing.Size(361, 71);
            this.AltIncomePanel.TabIndex = 7;
            // 
            // AgiReflectsCurrentIncomeBox
            // 
            this.AgiReflectsCurrentIncomeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AgiReflectsCurrentIncomeBox.FormattingEnabled = true;
            this.AgiReflectsCurrentIncomeBox.Items.AddRange(new object[] {
            "",
            "Yes",
            "No",
            "No Taxes Filed in the Last 2 Years"});
            this.AgiReflectsCurrentIncomeBox.Location = new System.Drawing.Point(221, 76);
            this.AgiReflectsCurrentIncomeBox.Name = "AgiReflectsCurrentIncomeBox";
            this.AgiReflectsCurrentIncomeBox.Size = new System.Drawing.Size(121, 28);
            this.AgiReflectsCurrentIncomeBox.TabIndex = 0;
            this.AgiReflectsCurrentIncomeBox.SelectedIndexChanged += new System.EventHandler(this.AgiReflectsCurrentIncomeBox_SelectedIndexChanged);
            // 
            // SpouseFilingStatusBox
            // 
            this.SpouseFilingStatusBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SpouseFilingStatusBox.FormattingEnabled = true;
            this.SpouseFilingStatusBox.Location = new System.Drawing.Point(218, 1);
            this.SpouseFilingStatusBox.Name = "SpouseFilingStatusBox";
            this.SpouseFilingStatusBox.Size = new System.Drawing.Size(176, 28);
            this.SpouseFilingStatusBox.TabIndex = 53;
            // 
            // SpouseFilingStatusLabel
            // 
            this.SpouseFilingStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SpouseFilingStatusLabel.Location = new System.Drawing.Point(20, 0);
            this.SpouseFilingStatusLabel.Name = "SpouseFilingStatusLabel";
            this.SpouseFilingStatusLabel.Size = new System.Drawing.Size(182, 29);
            this.SpouseFilingStatusLabel.TabIndex = 54;
            this.SpouseFilingStatusLabel.Text = "Spouse Filing Status:";
            this.SpouseFilingStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StateBox
            // 
            this.StateBox.AllowedSpecialCharacters = "";
            this.StateBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.StateBox.Location = new System.Drawing.Point(81, 3);
            this.StateBox.MaxLength = 2;
            this.StateBox.Name = "StateBox";
            this.StateBox.Size = new System.Drawing.Size(36, 26);
            this.StateBox.TabIndex = 55;
            // 
            // StateLabel
            // 
            this.StateLabel.AutoSize = true;
            this.StateLabel.Location = new System.Drawing.Point(7, 8);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(52, 20);
            this.StateLabel.TabIndex = 56;
            this.StateLabel.Text = "State:";
            // 
            // SpouseFilingStatusPanel
            // 
            this.SpouseFilingStatusPanel.Controls.Add(this.SpouseFilingStatusBox);
            this.SpouseFilingStatusPanel.Controls.Add(this.SpouseFilingStatusLabel);
            this.SpouseFilingStatusPanel.Location = new System.Drawing.Point(3, 203);
            this.SpouseFilingStatusPanel.Name = "SpouseFilingStatusPanel";
            this.SpouseFilingStatusPanel.Size = new System.Drawing.Size(401, 32);
            this.SpouseFilingStatusPanel.TabIndex = 5;
            this.SpouseFilingStatusPanel.Visible = false;
            // 
            // StatePanel
            // 
            this.StatePanel.Controls.Add(this.StateBox);
            this.StatePanel.Controls.Add(this.StateLabel);
            this.StatePanel.Location = new System.Drawing.Point(140, 202);
            this.StatePanel.Name = "StatePanel";
            this.StatePanel.Size = new System.Drawing.Size(193, 33);
            this.StatePanel.TabIndex = 4;
            // 
            // IncomeInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.SpouseFilingStatusPanel);
            this.Controls.Add(this.StatePanel);
            this.Controls.Add(this.AgiReflectsCurrentIncomeBox);
            this.Controls.Add(this.SupportingDocsRequiredBox);
            this.Controls.Add(this.TaxableIncomeBox);
            this.Controls.Add(this.ReceivedDateBox);
            this.Controls.Add(this.SupportingDocsRequiredLabel);
            this.Controls.Add(this.TaxableIncomeLabel);
            this.Controls.Add(this.IncomeSourceBox);
            this.Controls.Add(this.ReceivedDateLabel);
            this.Controls.Add(this.AgiReflectsCurrentIncomeLabel);
            this.Controls.Add(this.IncomeSourceLabel);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.AltIncomePanel);
            this.Controls.Add(this.AgiPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "IncomeInformation";
            this.Size = new System.Drawing.Size(403, 306);
            this.AgiPanel.ResumeLayout(false);
            this.AgiPanel.PerformLayout();
            this.AltIncomePanel.ResumeLayout(false);
            this.AltIncomePanel.PerformLayout();
            this.SpouseFilingStatusPanel.ResumeLayout(false);
            this.StatePanel.ResumeLayout(false);
            this.StatePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.NumericDecimalTextBox AgiFromTaxesBox;
        private System.Windows.Forms.MaskedTextBox ReceivedDateBox;
        private System.Windows.Forms.Label SupportingDocsRequiredLabel;
        private System.Windows.Forms.Label TaxableIncomeLabel;
        private Uheaa.Common.WinForms.NumericTextBox TaxYearBox;
        private System.Windows.Forms.Label TaxYearLabel;
        private System.Windows.Forms.Label AgiFromTaxesLabel;
        private System.Windows.Forms.Label IncomeSourceBox;
        private System.Windows.Forms.Label AltIncomeBox;
        private System.Windows.Forms.Label TotalAltIncomeBox;
        private System.Windows.Forms.Button CalcButton;
        private System.Windows.Forms.Label AltIncomeLabel;
        private System.Windows.Forms.Label TotalAltIncomeLabel;
        private System.Windows.Forms.Label ReceivedDateLabel;
        private System.Windows.Forms.Label AgiReflectsCurrentIncomeLabel;
        private System.Windows.Forms.Label IncomeSourceLabel;
        private System.Windows.Forms.Label TitleLabel;
        private Uheaa.Common.WinForms.YesNoComboBox TaxableIncomeBox;
        private Uheaa.Common.WinForms.YesNoComboBox SupportingDocsRequiredBox;
        private System.Windows.Forms.Panel AgiPanel;
        private System.Windows.Forms.Panel AltIncomePanel;
        private System.Windows.Forms.ComboBox AgiReflectsCurrentIncomeBox;
        private System.Windows.Forms.ComboBox SpouseFilingStatusBox;
        private System.Windows.Forms.Label SpouseFilingStatusLabel;
        private Uheaa.Common.WinForms.AlphaTextBox StateBox;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.Panel SpouseFilingStatusPanel;
        private System.Windows.Forms.Panel StatePanel;
    }
}
