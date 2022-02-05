namespace PWRATRNY
{
    partial class AddAndModifyForm
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
            this.BorrowerSSNLabel = new System.Windows.Forms.Label();
            this.MiddleInitialLabel = new System.Windows.Forms.Label();
            this.ReferenceLastLabel = new System.Windows.Forms.Label();
            this.ReferenceFirstLabel = new System.Windows.Forms.Label();
            this.BorrowerSSNTextBox = new Uheaa.Common.WinForms.SsnTextBox();
            this.MiddleInitialTextBox = new Uheaa.Common.WinForms.AlphaTextBox();
            this.Address2Label = new System.Windows.Forms.Label();
            this.Address1Label = new System.Windows.Forms.Label();
            this.RelationshipLabel = new System.Windows.Forms.Label();
            this.RelationshipCbo = new System.Windows.Forms.ComboBox();
            this.Address1TextBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.Address2TextBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.CityLabel = new System.Windows.Forms.Label();
            this.StateLabel = new System.Windows.Forms.Label();
            this.ZipLabel = new System.Windows.Forms.Label();
            this.ZipTextBox = new Uheaa.Common.WinForms.NumericTextBox();
            this.HomePhoneLabel = new System.Windows.Forms.Label();
            this.OtherPhoneLabel = new System.Windows.Forms.Label();
            this.ForeignPhoneLabel = new System.Windows.Forms.Label();
            this.MandatoryNotificationLabel = new System.Windows.Forms.Label();
            this.EmailLabel = new System.Windows.Forms.Label();
            this.EmailTextBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.OK = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.HomePhoneTextBox = new Uheaa.Common.WinForms.NumericTextBox();
            this.OtherPhoneTextBox = new Uheaa.Common.WinForms.NumericTextBox();
            this.ForeignPhoneTextBox = new Uheaa.Common.WinForms.NumericTextBox();
            this.StateCbo = new System.Windows.Forms.ComboBox();
            this.ReferenceFirstTextBox = new Uheaa.Common.WinForms.AlphaTextBox();
            this.ReferenceLastTextBox = new Uheaa.Common.WinForms.AlphaTextBox();
            this.CityTextBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.SuspendLayout();
            // 
            // BorrowerSSNLabel
            // 
            this.BorrowerSSNLabel.AutoSize = true;
            this.BorrowerSSNLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.BorrowerSSNLabel.Location = new System.Drawing.Point(20, 20);
            this.BorrowerSSNLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BorrowerSSNLabel.Name = "BorrowerSSNLabel";
            this.BorrowerSSNLabel.Size = new System.Drawing.Size(116, 20);
            this.BorrowerSSNLabel.TabIndex = 0;
            this.BorrowerSSNLabel.Text = "Borrower SSN*";
            // 
            // MiddleInitialLabel
            // 
            this.MiddleInitialLabel.AutoSize = true;
            this.MiddleInitialLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MiddleInitialLabel.Location = new System.Drawing.Point(20, 140);
            this.MiddleInitialLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MiddleInitialLabel.Name = "MiddleInitialLabel";
            this.MiddleInitialLabel.Size = new System.Drawing.Size(96, 20);
            this.MiddleInitialLabel.TabIndex = 6;
            this.MiddleInitialLabel.Text = "Middle Initial";
            // 
            // ReferenceLastLabel
            // 
            this.ReferenceLastLabel.AutoSize = true;
            this.ReferenceLastLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ReferenceLastLabel.Location = new System.Drawing.Point(20, 100);
            this.ReferenceLastLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ReferenceLastLabel.Name = "ReferenceLastLabel";
            this.ReferenceLastLabel.Size = new System.Drawing.Size(125, 20);
            this.ReferenceLastLabel.TabIndex = 4;
            this.ReferenceLastLabel.Text = "Reference Last*";
            // 
            // ReferenceFirstLabel
            // 
            this.ReferenceFirstLabel.AutoSize = true;
            this.ReferenceFirstLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ReferenceFirstLabel.Location = new System.Drawing.Point(20, 60);
            this.ReferenceFirstLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ReferenceFirstLabel.Name = "ReferenceFirstLabel";
            this.ReferenceFirstLabel.Size = new System.Drawing.Size(125, 20);
            this.ReferenceFirstLabel.TabIndex = 2;
            this.ReferenceFirstLabel.Text = "Reference First*";
            // 
            // BorrowerSSNTextBox
            // 
            this.BorrowerSSNTextBox.AllowedSpecialCharacters = "";
            this.BorrowerSSNTextBox.Location = new System.Drawing.Point(167, 20);
            this.BorrowerSSNTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BorrowerSSNTextBox.MaxLength = 9;
            this.BorrowerSSNTextBox.Name = "BorrowerSSNTextBox";
            this.BorrowerSSNTextBox.ReadOnly = true;
            this.BorrowerSSNTextBox.Size = new System.Drawing.Size(148, 26);
            this.BorrowerSSNTextBox.Ssn = null;
            this.BorrowerSSNTextBox.TabIndex = 0;
            // 
            // MiddleInitialTextBox
            // 
            this.MiddleInitialTextBox.AllowedSpecialCharacters = "";
            this.MiddleInitialTextBox.Location = new System.Drawing.Point(167, 140);
            this.MiddleInitialTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MiddleInitialTextBox.MaxLength = 1;
            this.MiddleInitialTextBox.Name = "MiddleInitialTextBox";
            this.MiddleInitialTextBox.Size = new System.Drawing.Size(28, 26);
            this.MiddleInitialTextBox.TabIndex = 3;
            this.MiddleInitialTextBox.TextChanged += new System.EventHandler(this.SetDirtyName);
            // 
            // Address2Label
            // 
            this.Address2Label.AutoSize = true;
            this.Address2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Address2Label.Location = new System.Drawing.Point(20, 258);
            this.Address2Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Address2Label.Name = "Address2Label";
            this.Address2Label.Size = new System.Drawing.Size(81, 20);
            this.Address2Label.TabIndex = 14;
            this.Address2Label.Text = "Address 2";
            // 
            // Address1Label
            // 
            this.Address1Label.AutoSize = true;
            this.Address1Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Address1Label.Location = new System.Drawing.Point(20, 218);
            this.Address1Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Address1Label.Name = "Address1Label";
            this.Address1Label.Size = new System.Drawing.Size(87, 20);
            this.Address1Label.TabIndex = 12;
            this.Address1Label.Text = "Address 1*";
            // 
            // RelationshipLabel
            // 
            this.RelationshipLabel.AutoSize = true;
            this.RelationshipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.RelationshipLabel.Location = new System.Drawing.Point(20, 178);
            this.RelationshipLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RelationshipLabel.Name = "RelationshipLabel";
            this.RelationshipLabel.Size = new System.Drawing.Size(103, 20);
            this.RelationshipLabel.TabIndex = 10;
            this.RelationshipLabel.Text = "Relationship*";
            // 
            // RelationshipCbo
            // 
            this.RelationshipCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RelationshipCbo.FormattingEnabled = true;
            this.RelationshipCbo.Location = new System.Drawing.Point(167, 180);
            this.RelationshipCbo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RelationshipCbo.Name = "RelationshipCbo";
            this.RelationshipCbo.Size = new System.Drawing.Size(180, 28);
            this.RelationshipCbo.TabIndex = 4;
            this.RelationshipCbo.TextChanged += new System.EventHandler(this.SetDirtyRelationship);
            // 
            // Address1TextBox
            // 
            this.Address1TextBox.AllowAllCharacters = false;
            this.Address1TextBox.AllowAlphaCharacters = true;
            this.Address1TextBox.AllowedAdditionalCharacters = "- ";
            this.Address1TextBox.AllowNumericCharacters = true;
            this.Address1TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Address1TextBox.InvalidColor = System.Drawing.Color.LightPink;
            this.Address1TextBox.Location = new System.Drawing.Point(167, 218);
            this.Address1TextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Address1TextBox.Mask = "";
            this.Address1TextBox.Name = "Address1TextBox";
            this.Address1TextBox.Size = new System.Drawing.Size(148, 26);
            this.Address1TextBox.TabIndex = 5;
            this.Address1TextBox.ValidationMessage = null;
            this.Address1TextBox.ValidColor = System.Drawing.Color.LightGreen;
            this.Address1TextBox.TextChanged += new System.EventHandler(this.SetDirtyAddress);
            // 
            // Address2TextBox
            // 
            this.Address2TextBox.AllowAllCharacters = false;
            this.Address2TextBox.AllowAlphaCharacters = true;
            this.Address2TextBox.AllowedAdditionalCharacters = "- ";
            this.Address2TextBox.AllowNumericCharacters = true;
            this.Address2TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Address2TextBox.InvalidColor = System.Drawing.Color.LightPink;
            this.Address2TextBox.Location = new System.Drawing.Point(167, 258);
            this.Address2TextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Address2TextBox.Mask = "";
            this.Address2TextBox.Name = "Address2TextBox";
            this.Address2TextBox.Size = new System.Drawing.Size(148, 26);
            this.Address2TextBox.TabIndex = 6;
            this.Address2TextBox.ValidationMessage = null;
            this.Address2TextBox.ValidColor = System.Drawing.Color.LightGreen;
            this.Address2TextBox.TextChanged += new System.EventHandler(this.SetDirtyAddress);
            // 
            // CityLabel
            // 
            this.CityLabel.AutoSize = true;
            this.CityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.CityLabel.Location = new System.Drawing.Point(20, 298);
            this.CityLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CityLabel.Name = "CityLabel";
            this.CityLabel.Size = new System.Drawing.Size(41, 20);
            this.CityLabel.TabIndex = 16;
            this.CityLabel.Text = "City*";
            // 
            // StateLabel
            // 
            this.StateLabel.AutoSize = true;
            this.StateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.StateLabel.Location = new System.Drawing.Point(20, 338);
            this.StateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(54, 20);
            this.StateLabel.TabIndex = 18;
            this.StateLabel.Text = "State*";
            // 
            // ZipLabel
            // 
            this.ZipLabel.AutoSize = true;
            this.ZipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ZipLabel.Location = new System.Drawing.Point(20, 378);
            this.ZipLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ZipLabel.Name = "ZipLabel";
            this.ZipLabel.Size = new System.Drawing.Size(37, 20);
            this.ZipLabel.TabIndex = 20;
            this.ZipLabel.Text = "Zip*";
            // 
            // ZipTextBox
            // 
            this.ZipTextBox.AllowedSpecialCharacters = "";
            this.ZipTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ZipTextBox.Location = new System.Drawing.Point(167, 377);
            this.ZipTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ZipTextBox.Name = "ZipTextBox";
            this.ZipTextBox.Size = new System.Drawing.Size(148, 26);
            this.ZipTextBox.TabIndex = 9;
            this.ZipTextBox.TextChanged += new System.EventHandler(this.SetDirtyAddress);
            // 
            // HomePhoneLabel
            // 
            this.HomePhoneLabel.AutoSize = true;
            this.HomePhoneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.HomePhoneLabel.Location = new System.Drawing.Point(20, 418);
            this.HomePhoneLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HomePhoneLabel.Name = "HomePhoneLabel";
            this.HomePhoneLabel.Size = new System.Drawing.Size(102, 20);
            this.HomePhoneLabel.TabIndex = 22;
            this.HomePhoneLabel.Text = "Home Phone";
            // 
            // OtherPhoneLabel
            // 
            this.OtherPhoneLabel.AutoSize = true;
            this.OtherPhoneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.OtherPhoneLabel.Location = new System.Drawing.Point(20, 458);
            this.OtherPhoneLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OtherPhoneLabel.Name = "OtherPhoneLabel";
            this.OtherPhoneLabel.Size = new System.Drawing.Size(99, 20);
            this.OtherPhoneLabel.TabIndex = 24;
            this.OtherPhoneLabel.Text = "Other Phone";
            // 
            // ForeignPhoneLabel
            // 
            this.ForeignPhoneLabel.AutoSize = true;
            this.ForeignPhoneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ForeignPhoneLabel.Location = new System.Drawing.Point(20, 498);
            this.ForeignPhoneLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ForeignPhoneLabel.Name = "ForeignPhoneLabel";
            this.ForeignPhoneLabel.Size = new System.Drawing.Size(113, 20);
            this.ForeignPhoneLabel.TabIndex = 26;
            this.ForeignPhoneLabel.Text = "Foreign Phone";
            // 
            // MandatoryNotificationLabel
            // 
            this.MandatoryNotificationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MandatoryNotificationLabel.AutoSize = true;
            this.MandatoryNotificationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MandatoryNotificationLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.MandatoryNotificationLabel.Location = new System.Drawing.Point(149, 611);
            this.MandatoryNotificationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MandatoryNotificationLabel.Name = "MandatoryNotificationLabel";
            this.MandatoryNotificationLabel.Size = new System.Drawing.Size(84, 20);
            this.MandatoryNotificationLabel.TabIndex = 30;
            this.MandatoryNotificationLabel.Text = "* Required";
            // 
            // EmailLabel
            // 
            this.EmailLabel.AutoSize = true;
            this.EmailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.EmailLabel.Location = new System.Drawing.Point(20, 538);
            this.EmailLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(48, 20);
            this.EmailLabel.TabIndex = 28;
            this.EmailLabel.Text = "Email";
            // 
            // EmailTextBox
            // 
            this.EmailTextBox.AllowAllCharacters = true;
            this.EmailTextBox.AllowAlphaCharacters = true;
            this.EmailTextBox.AllowedAdditionalCharacters = "";
            this.EmailTextBox.AllowNumericCharacters = true;
            this.EmailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EmailTextBox.InvalidColor = System.Drawing.Color.LightPink;
            this.EmailTextBox.Location = new System.Drawing.Point(167, 538);
            this.EmailTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EmailTextBox.Mask = "";
            this.EmailTextBox.Name = "EmailTextBox";
            this.EmailTextBox.Size = new System.Drawing.Size(148, 26);
            this.EmailTextBox.TabIndex = 13;
            this.EmailTextBox.ValidationMessage = null;
            this.EmailTextBox.ValidColor = System.Drawing.Color.LightGreen;
            this.EmailTextBox.TextChanged += new System.EventHandler(this.SetDirtyEmail);
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.OK.Location = new System.Drawing.Point(258, 591);
            this.OK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(112, 40);
            this.OK.TabIndex = 14;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.button2.Location = new System.Drawing.Point(13, 591);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 40);
            this.button2.TabIndex = 15;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // HomePhoneTextBox
            // 
            this.HomePhoneTextBox.AllowedSpecialCharacters = "";
            this.HomePhoneTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HomePhoneTextBox.Location = new System.Drawing.Point(167, 418);
            this.HomePhoneTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.HomePhoneTextBox.Name = "HomePhoneTextBox";
            this.HomePhoneTextBox.Size = new System.Drawing.Size(148, 26);
            this.HomePhoneTextBox.TabIndex = 10;
            this.HomePhoneTextBox.TextChanged += new System.EventHandler(this.SetDirtyHomePhone);
            // 
            // OtherPhoneTextBox
            // 
            this.OtherPhoneTextBox.AllowedSpecialCharacters = "";
            this.OtherPhoneTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OtherPhoneTextBox.Location = new System.Drawing.Point(167, 460);
            this.OtherPhoneTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OtherPhoneTextBox.Name = "OtherPhoneTextBox";
            this.OtherPhoneTextBox.Size = new System.Drawing.Size(148, 26);
            this.OtherPhoneTextBox.TabIndex = 11;
            this.OtherPhoneTextBox.TextChanged += new System.EventHandler(this.SetDirtyOtherPhone);
            // 
            // ForeignPhoneTextBox
            // 
            this.ForeignPhoneTextBox.AllowedSpecialCharacters = "";
            this.ForeignPhoneTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForeignPhoneTextBox.Location = new System.Drawing.Point(167, 498);
            this.ForeignPhoneTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ForeignPhoneTextBox.Name = "ForeignPhoneTextBox";
            this.ForeignPhoneTextBox.Size = new System.Drawing.Size(148, 26);
            this.ForeignPhoneTextBox.TabIndex = 12;
            this.ForeignPhoneTextBox.TextChanged += new System.EventHandler(this.SetDirtyForeignPhone);
            // 
            // StateCbo
            // 
            this.StateCbo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.StateCbo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.StateCbo.FormattingEnabled = true;
            this.StateCbo.Location = new System.Drawing.Point(167, 338);
            this.StateCbo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.StateCbo.Name = "StateCbo";
            this.StateCbo.Size = new System.Drawing.Size(66, 28);
            this.StateCbo.TabIndex = 8;
            this.StateCbo.TextChanged += new System.EventHandler(this.SetDirtyAddress);
            // 
            // ReferenceFirstTextBox
            // 
            this.ReferenceFirstTextBox.AllowedSpecialCharacters = "";
            this.ReferenceFirstTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReferenceFirstTextBox.Location = new System.Drawing.Point(167, 58);
            this.ReferenceFirstTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ReferenceFirstTextBox.Name = "ReferenceFirstTextBox";
            this.ReferenceFirstTextBox.Size = new System.Drawing.Size(148, 26);
            this.ReferenceFirstTextBox.TabIndex = 1;
            this.ReferenceFirstTextBox.TextChanged += new System.EventHandler(this.SetDirtyName);
            // 
            // ReferenceLastTextBox
            // 
            this.ReferenceLastTextBox.AllowedSpecialCharacters = "";
            this.ReferenceLastTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReferenceLastTextBox.Location = new System.Drawing.Point(167, 98);
            this.ReferenceLastTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ReferenceLastTextBox.Name = "ReferenceLastTextBox";
            this.ReferenceLastTextBox.Size = new System.Drawing.Size(148, 26);
            this.ReferenceLastTextBox.TabIndex = 2;
            this.ReferenceLastTextBox.TextChanged += new System.EventHandler(this.SetDirtyName);
            // 
            // CityTextBox
            // 
            this.CityTextBox.AllowAllCharacters = false;
            this.CityTextBox.AllowAlphaCharacters = true;
            this.CityTextBox.AllowedAdditionalCharacters = " ";
            this.CityTextBox.AllowNumericCharacters = false;
            this.CityTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CityTextBox.InvalidColor = System.Drawing.Color.LightPink;
            this.CityTextBox.Location = new System.Drawing.Point(167, 297);
            this.CityTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CityTextBox.Mask = "";
            this.CityTextBox.Name = "CityTextBox";
            this.CityTextBox.Size = new System.Drawing.Size(148, 26);
            this.CityTextBox.TabIndex = 7;
            this.CityTextBox.ValidationMessage = null;
            this.CityTextBox.ValidColor = System.Drawing.Color.LightGreen;
            this.CityTextBox.TextChanged += new System.EventHandler(this.SetDirtyAddress);
            // 
            // AddAndModifyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 645);
            this.Controls.Add(this.CityTextBox);
            this.Controls.Add(this.ReferenceLastTextBox);
            this.Controls.Add(this.ReferenceFirstTextBox);
            this.Controls.Add(this.StateCbo);
            this.Controls.Add(this.ForeignPhoneTextBox);
            this.Controls.Add(this.OtherPhoneTextBox);
            this.Controls.Add(this.HomePhoneTextBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.EmailTextBox);
            this.Controls.Add(this.EmailLabel);
            this.Controls.Add(this.MandatoryNotificationLabel);
            this.Controls.Add(this.ForeignPhoneLabel);
            this.Controls.Add(this.OtherPhoneLabel);
            this.Controls.Add(this.HomePhoneLabel);
            this.Controls.Add(this.ZipTextBox);
            this.Controls.Add(this.ZipLabel);
            this.Controls.Add(this.StateLabel);
            this.Controls.Add(this.CityLabel);
            this.Controls.Add(this.Address2TextBox);
            this.Controls.Add(this.Address1TextBox);
            this.Controls.Add(this.RelationshipCbo);
            this.Controls.Add(this.RelationshipLabel);
            this.Controls.Add(this.Address1Label);
            this.Controls.Add(this.Address2Label);
            this.Controls.Add(this.MiddleInitialTextBox);
            this.Controls.Add(this.BorrowerSSNTextBox);
            this.Controls.Add(this.ReferenceFirstLabel);
            this.Controls.Add(this.ReferenceLastLabel);
            this.Controls.Add(this.MiddleInitialLabel);
            this.Controls.Add(this.BorrowerSSNLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(399, 684);
            this.Name = "AddAndModifyForm";
            this.Text = "AddAndModifyForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BorrowerSSNLabel;
        private System.Windows.Forms.Label MiddleInitialLabel;
        private System.Windows.Forms.Label ReferenceLastLabel;
        private System.Windows.Forms.Label ReferenceFirstLabel;
        private Uheaa.Common.WinForms.SsnTextBox BorrowerSSNTextBox;
        private Uheaa.Common.WinForms.AlphaTextBox MiddleInitialTextBox;
        private System.Windows.Forms.Label Address2Label;
        private System.Windows.Forms.Label Address1Label;
        private System.Windows.Forms.Label RelationshipLabel;
        private System.Windows.Forms.ComboBox RelationshipCbo;
        private Uheaa.Common.WinForms.OmniTextBox Address1TextBox;
        private Uheaa.Common.WinForms.OmniTextBox Address2TextBox;
        private System.Windows.Forms.Label CityLabel;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.Label ZipLabel;
        private Uheaa.Common.WinForms.NumericTextBox ZipTextBox;
        private System.Windows.Forms.Label HomePhoneLabel;
        private System.Windows.Forms.Label OtherPhoneLabel;
        private System.Windows.Forms.Label ForeignPhoneLabel;
        private System.Windows.Forms.Label MandatoryNotificationLabel;
        private System.Windows.Forms.Label EmailLabel;
        private Uheaa.Common.WinForms.OmniTextBox EmailTextBox;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button button2;
        private Uheaa.Common.WinForms.NumericTextBox HomePhoneTextBox;
        private Uheaa.Common.WinForms.NumericTextBox OtherPhoneTextBox;
        private Uheaa.Common.WinForms.NumericTextBox ForeignPhoneTextBox;
        private System.Windows.Forms.ComboBox StateCbo;
        private Uheaa.Common.WinForms.AlphaTextBox ReferenceFirstTextBox;
        private Uheaa.Common.WinForms.AlphaTextBox ReferenceLastTextBox;
        private Uheaa.Common.WinForms.OmniTextBox CityTextBox;
    }
}