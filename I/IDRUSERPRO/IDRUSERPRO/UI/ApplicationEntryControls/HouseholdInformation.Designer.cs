namespace IDRUSERPRO
{
    partial class HouseholdInformation
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
            this.SpouseExternalLoansBox = new Uheaa.Common.WinForms.YesNoComboBox();
            this.SpouseExternalLoansLabel = new System.Windows.Forms.Label();
            this.FamilySizeIncreasedCbx = new System.Windows.Forms.CheckBox();
            this.FilingStatusCbo = new System.Windows.Forms.ComboBox();
            this.MaritalStatusCbo = new System.Windows.Forms.ComboBox();
            this.SpouseGroup = new System.Windows.Forms.GroupBox();
            this.SpouseLastName = new Uheaa.Common.WinForms.OmniTextBox();
            this.SpouseMiddleName = new Uheaa.Common.WinForms.OmniTextBox();
            this.SpouseFirstName = new Uheaa.Common.WinForms.OmniTextBox();
            this.SpouseDOB = new System.Windows.Forms.MaskedTextBox();
            this.SpouseSsn = new Uheaa.Common.WinForms.SsnTextBox();
            this.SpouseSsnLabel = new System.Windows.Forms.Label();
            this.SpouseLastNameLabel = new System.Windows.Forms.Label();
            this.SpouseDobLabel = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.SpouseFirstNameLabel = new System.Windows.Forms.Label();
            this.FilingStatusLbl = new System.Windows.Forms.Label();
            this.MaritalStatusLbl = new System.Windows.Forms.Label();
            this.DependentsLabel = new System.Windows.Forms.Label();
            this.ChildrenLabel = new System.Windows.Forms.Label();
            this.Dependents = new Uheaa.Common.WinForms.NumericTextBox();
            this.Children = new Uheaa.Common.WinForms.NumericTextBox();
            this.SpouseGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // SpouseExternalLoansBox
            // 
            this.SpouseExternalLoansBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SpouseExternalLoansBox.FormattingEnabled = true;
            this.SpouseExternalLoansBox.Location = new System.Drawing.Point(130, 182);
            this.SpouseExternalLoansBox.Name = "SpouseExternalLoansBox";
            this.SpouseExternalLoansBox.SelectedValue = null;
            this.SpouseExternalLoansBox.Size = new System.Drawing.Size(121, 28);
            this.SpouseExternalLoansBox.TabIndex = 20;
            this.SpouseExternalLoansBox.SelectedValueChanged += new System.EventHandler(this.MaritalFilingSpouse_DataChanged);
            // 
            // SpouseExternalLoansLabel
            // 
            this.SpouseExternalLoansLabel.Location = new System.Drawing.Point(-2, 172);
            this.SpouseExternalLoansLabel.Name = "SpouseExternalLoansLabel";
            this.SpouseExternalLoansLabel.Size = new System.Drawing.Size(126, 46);
            this.SpouseExternalLoansLabel.TabIndex = 22;
            this.SpouseExternalLoansLabel.Text = "Spouse has External Loans:";
            this.SpouseExternalLoansLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FamilySizeIncreasedCbx
            // 
            this.FamilySizeIncreasedCbx.AutoSize = true;
            this.FamilySizeIncreasedCbx.Location = new System.Drawing.Point(172, 46);
            this.FamilySizeIncreasedCbx.Name = "FamilySizeIncreasedCbx";
            this.FamilySizeIncreasedCbx.Size = new System.Drawing.Size(278, 24);
            this.FamilySizeIncreasedCbx.TabIndex = 15;
            this.FamilySizeIncreasedCbx.Text = "Proof of Family Size Docs Required";
            this.FamilySizeIncreasedCbx.UseVisualStyleBackColor = true;
            this.FamilySizeIncreasedCbx.Visible = false;
            // 
            // FilingStatusCbo
            // 
            this.FilingStatusCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilingStatusCbo.FormattingEnabled = true;
            this.FilingStatusCbo.Location = new System.Drawing.Point(130, 138);
            this.FilingStatusCbo.Name = "FilingStatusCbo";
            this.FilingStatusCbo.Size = new System.Drawing.Size(373, 28);
            this.FilingStatusCbo.TabIndex = 19;
            this.FilingStatusCbo.SelectedIndexChanged += new System.EventHandler(this.MaritalFilingSpouse_DataChanged);
            // 
            // MaritalStatusCbo
            // 
            this.MaritalStatusCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MaritalStatusCbo.FormattingEnabled = true;
            this.MaritalStatusCbo.Location = new System.Drawing.Point(130, 87);
            this.MaritalStatusCbo.Name = "MaritalStatusCbo";
            this.MaritalStatusCbo.Size = new System.Drawing.Size(280, 28);
            this.MaritalStatusCbo.TabIndex = 17;
            this.MaritalStatusCbo.SelectedIndexChanged += new System.EventHandler(this.MaritalFilingSpouse_DataChanged);
            // 
            // SpouseGroup
            // 
            this.SpouseGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SpouseGroup.Controls.Add(this.SpouseLastName);
            this.SpouseGroup.Controls.Add(this.SpouseMiddleName);
            this.SpouseGroup.Controls.Add(this.SpouseFirstName);
            this.SpouseGroup.Controls.Add(this.SpouseDOB);
            this.SpouseGroup.Controls.Add(this.SpouseSsn);
            this.SpouseGroup.Controls.Add(this.SpouseSsnLabel);
            this.SpouseGroup.Controls.Add(this.SpouseLastNameLabel);
            this.SpouseGroup.Controls.Add(this.SpouseDobLabel);
            this.SpouseGroup.Controls.Add(this.label13);
            this.SpouseGroup.Controls.Add(this.SpouseFirstNameLabel);
            this.SpouseGroup.Location = new System.Drawing.Point(534, 7);
            this.SpouseGroup.Name = "SpouseGroup";
            this.SpouseGroup.Size = new System.Drawing.Size(362, 269);
            this.SpouseGroup.TabIndex = 21;
            this.SpouseGroup.TabStop = false;
            this.SpouseGroup.Text = "Spouse Information";
            this.SpouseGroup.Visible = false;
            // 
            // SpouseLastName
            // 
            this.SpouseLastName.AllowAllCharacters = true;
            this.SpouseLastName.AllowAlphaCharacters = true;
            this.SpouseLastName.AllowedAdditionalCharacters = "";
            this.SpouseLastName.AllowNumericCharacters = true;
            this.SpouseLastName.InvalidColor = System.Drawing.Color.LightPink;
            this.SpouseLastName.Location = new System.Drawing.Point(137, 112);
            this.SpouseLastName.Mask = "";
            this.SpouseLastName.MaxLength = 35;
            this.SpouseLastName.Name = "SpouseLastName";
            this.SpouseLastName.Size = new System.Drawing.Size(191, 26);
            this.SpouseLastName.TabIndex = 2;
            this.SpouseLastName.ValidationMessage = null;
            this.SpouseLastName.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // SpouseMiddleName
            // 
            this.SpouseMiddleName.AllowAllCharacters = true;
            this.SpouseMiddleName.AllowAlphaCharacters = true;
            this.SpouseMiddleName.AllowedAdditionalCharacters = "";
            this.SpouseMiddleName.AllowNumericCharacters = true;
            this.SpouseMiddleName.InvalidColor = System.Drawing.Color.LightPink;
            this.SpouseMiddleName.Location = new System.Drawing.Point(137, 74);
            this.SpouseMiddleName.Mask = "";
            this.SpouseMiddleName.MaxLength = 35;
            this.SpouseMiddleName.Name = "SpouseMiddleName";
            this.SpouseMiddleName.Size = new System.Drawing.Size(191, 26);
            this.SpouseMiddleName.TabIndex = 1;
            this.SpouseMiddleName.ValidationMessage = null;
            this.SpouseMiddleName.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // SpouseFirstName
            // 
            this.SpouseFirstName.AllowAllCharacters = true;
            this.SpouseFirstName.AllowAlphaCharacters = true;
            this.SpouseFirstName.AllowedAdditionalCharacters = "";
            this.SpouseFirstName.AllowNumericCharacters = true;
            this.SpouseFirstName.InvalidColor = System.Drawing.Color.LightPink;
            this.SpouseFirstName.Location = new System.Drawing.Point(137, 36);
            this.SpouseFirstName.Mask = "";
            this.SpouseFirstName.MaxLength = 35;
            this.SpouseFirstName.Name = "SpouseFirstName";
            this.SpouseFirstName.Size = new System.Drawing.Size(191, 26);
            this.SpouseFirstName.TabIndex = 0;
            this.SpouseFirstName.ValidationMessage = null;
            this.SpouseFirstName.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // SpouseDOB
            // 
            this.SpouseDOB.Location = new System.Drawing.Point(137, 191);
            this.SpouseDOB.Mask = "00/00/0000";
            this.SpouseDOB.Name = "SpouseDOB";
            this.SpouseDOB.Size = new System.Drawing.Size(87, 26);
            this.SpouseDOB.TabIndex = 4;
            this.SpouseDOB.ValidatingType = typeof(System.DateTime);
            // 
            // SpouseSsn
            // 
            this.SpouseSsn.AllowedSpecialCharacters = "";
            this.SpouseSsn.Location = new System.Drawing.Point(137, 150);
            this.SpouseSsn.MaxLength = 9;
            this.SpouseSsn.Name = "SpouseSsn";
            this.SpouseSsn.Size = new System.Drawing.Size(100, 26);
            this.SpouseSsn.Ssn = null;
            this.SpouseSsn.TabIndex = 3;
            // 
            // SpouseSsnLabel
            // 
            this.SpouseSsnLabel.AutoSize = true;
            this.SpouseSsnLabel.Location = new System.Drawing.Point(74, 153);
            this.SpouseSsnLabel.Name = "SpouseSsnLabel";
            this.SpouseSsnLabel.Size = new System.Drawing.Size(46, 20);
            this.SpouseSsnLabel.TabIndex = 4;
            this.SpouseSsnLabel.Text = "SSN:";
            // 
            // SpouseLastNameLabel
            // 
            this.SpouseLastNameLabel.AutoSize = true;
            this.SpouseLastNameLabel.Location = new System.Drawing.Point(30, 115);
            this.SpouseLastNameLabel.Name = "SpouseLastNameLabel";
            this.SpouseLastNameLabel.Size = new System.Drawing.Size(90, 20);
            this.SpouseLastNameLabel.TabIndex = 8;
            this.SpouseLastNameLabel.Text = "Last Name:";
            // 
            // SpouseDobLabel
            // 
            this.SpouseDobLabel.AutoSize = true;
            this.SpouseDobLabel.Location = new System.Drawing.Point(72, 191);
            this.SpouseDobLabel.Name = "SpouseDobLabel";
            this.SpouseDobLabel.Size = new System.Drawing.Size(48, 20);
            this.SpouseDobLabel.TabIndex = 5;
            this.SpouseDobLabel.Text = "DOB:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 77);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(105, 20);
            this.label13.TabIndex = 7;
            this.label13.Text = "Middle Name:";
            // 
            // SpouseFirstNameLabel
            // 
            this.SpouseFirstNameLabel.AutoSize = true;
            this.SpouseFirstNameLabel.Location = new System.Drawing.Point(30, 39);
            this.SpouseFirstNameLabel.Name = "SpouseFirstNameLabel";
            this.SpouseFirstNameLabel.Size = new System.Drawing.Size(90, 20);
            this.SpouseFirstNameLabel.TabIndex = 6;
            this.SpouseFirstNameLabel.Text = "First Name:";
            // 
            // FilingStatusLbl
            // 
            this.FilingStatusLbl.Location = new System.Drawing.Point(-2, 138);
            this.FilingStatusLbl.Name = "FilingStatusLbl";
            this.FilingStatusLbl.Size = new System.Drawing.Size(126, 20);
            this.FilingStatusLbl.TabIndex = 18;
            this.FilingStatusLbl.Text = "Filing Status:";
            this.FilingStatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MaritalStatusLbl
            // 
            this.MaritalStatusLbl.Location = new System.Drawing.Point(-2, 90);
            this.MaritalStatusLbl.Name = "MaritalStatusLbl";
            this.MaritalStatusLbl.Size = new System.Drawing.Size(126, 20);
            this.MaritalStatusLbl.TabIndex = 16;
            this.MaritalStatusLbl.Text = "Marital Status:";
            this.MaritalStatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DependentsLabel
            // 
            this.DependentsLabel.Location = new System.Drawing.Point(-2, 48);
            this.DependentsLabel.Name = "DependentsLabel";
            this.DependentsLabel.Size = new System.Drawing.Size(126, 20);
            this.DependentsLabel.TabIndex = 13;
            this.DependentsLabel.Text = "Dependents:";
            this.DependentsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ChildrenLabel
            // 
            this.ChildrenLabel.Location = new System.Drawing.Point(-2, 7);
            this.ChildrenLabel.Name = "ChildrenLabel";
            this.ChildrenLabel.Size = new System.Drawing.Size(126, 20);
            this.ChildrenLabel.TabIndex = 11;
            this.ChildrenLabel.Text = "Children:";
            this.ChildrenLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Dependents
            // 
            this.Dependents.AllowedSpecialCharacters = "";
            this.Dependents.Location = new System.Drawing.Point(130, 45);
            this.Dependents.MaxLength = 2;
            this.Dependents.Name = "Dependents";
            this.Dependents.Size = new System.Drawing.Size(27, 26);
            this.Dependents.TabIndex = 14;
            this.Dependents.TextChanged += new System.EventHandler(this.MaritalFilingSpouse_DataChanged);
            // 
            // Children
            // 
            this.Children.AllowedSpecialCharacters = "";
            this.Children.Location = new System.Drawing.Point(130, 7);
            this.Children.MaxLength = 2;
            this.Children.Name = "Children";
            this.Children.Size = new System.Drawing.Size(27, 26);
            this.Children.TabIndex = 12;
            this.Children.TextChanged += new System.EventHandler(this.MaritalFilingSpouse_DataChanged);
            // 
            // HouseholdInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SpouseExternalLoansBox);
            this.Controls.Add(this.SpouseExternalLoansLabel);
            this.Controls.Add(this.FamilySizeIncreasedCbx);
            this.Controls.Add(this.FilingStatusCbo);
            this.Controls.Add(this.MaritalStatusCbo);
            this.Controls.Add(this.SpouseGroup);
            this.Controls.Add(this.FilingStatusLbl);
            this.Controls.Add(this.MaritalStatusLbl);
            this.Controls.Add(this.DependentsLabel);
            this.Controls.Add(this.ChildrenLabel);
            this.Controls.Add(this.Dependents);
            this.Controls.Add(this.Children);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "HouseholdInformation";
            this.Size = new System.Drawing.Size(895, 283);
            this.SpouseGroup.ResumeLayout(false);
            this.SpouseGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.YesNoComboBox SpouseExternalLoansBox;
        private System.Windows.Forms.Label SpouseExternalLoansLabel;
        private System.Windows.Forms.CheckBox FamilySizeIncreasedCbx;
        private System.Windows.Forms.ComboBox FilingStatusCbo;
        private System.Windows.Forms.ComboBox MaritalStatusCbo;
        private System.Windows.Forms.GroupBox SpouseGroup;
        private Uheaa.Common.WinForms.OmniTextBox SpouseLastName;
        private Uheaa.Common.WinForms.OmniTextBox SpouseMiddleName;
        private Uheaa.Common.WinForms.OmniTextBox SpouseFirstName;
        private System.Windows.Forms.MaskedTextBox SpouseDOB;
        private Uheaa.Common.WinForms.SsnTextBox SpouseSsn;
        private System.Windows.Forms.Label SpouseSsnLabel;
        private System.Windows.Forms.Label SpouseLastNameLabel;
        private System.Windows.Forms.Label SpouseDobLabel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label SpouseFirstNameLabel;
        private System.Windows.Forms.Label FilingStatusLbl;
        private System.Windows.Forms.Label MaritalStatusLbl;
        private System.Windows.Forms.Label DependentsLabel;
        private System.Windows.Forms.Label ChildrenLabel;
        private Uheaa.Common.WinForms.NumericTextBox Dependents;
        private Uheaa.Common.WinForms.NumericTextBox Children;
    }
}
