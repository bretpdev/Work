namespace OLDEMOS
{
    partial class Demographics
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
            this.Process = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.BorrowerDemos = new System.Windows.Forms.GroupBox();
            this.DobTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.AccountTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SsnTxt = new System.Windows.Forms.TextBox();
            this.NameTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Address1 = new Uheaa.Common.WinForms.OmniTextBox();
            this.Address2 = new Uheaa.Common.WinForms.OmniTextBox();
            this.City = new Uheaa.Common.WinForms.OmniTextBox();
            this.ZipCode = new Uheaa.Common.WinForms.OmniTextBox();
            this.State = new System.Windows.Forms.ComboBox();
            this.Country = new System.Windows.Forms.ComboBox();
            this.Email = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.Source = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.AddressForeign = new System.Windows.Forms.CheckBox();
            this.AddressValid = new System.Windows.Forms.CheckBox();
            this.EmailValid = new System.Windows.Forms.CheckBox();
            this.AlternatePhoneValid = new System.Windows.Forms.CheckBox();
            this.PrimaryPhoneValid = new System.Windows.Forms.CheckBox();
            this.OtherPhoneValid = new System.Windows.Forms.CheckBox();
            this.PrimaryPhoneConsent = new System.Windows.Forms.CheckBox();
            this.AlternatePhoneConsent = new System.Windows.Forms.CheckBox();
            this.OtherPhoneConsent = new System.Windows.Forms.CheckBox();
            this.ForeignOtherPhone = new OLDEMOS.PhoneBox();
            this.ForeignAltPhone = new OLDEMOS.PhoneBox();
            this.ForeignPrimaryPhone = new OLDEMOS.PhoneBox();
            this.AlternatePhone = new OLDEMOS.PhoneBox();
            this.OtherPhone = new OLDEMOS.PhoneBox();
            this.PrimaryPhone = new OLDEMOS.PhoneBox();
            this.toolStripMenuItem411 = new System.Windows.Forms.ToolStripMenuItem();
            this.IRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.securityIncidentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.physicalThreatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.BorrowerDemos.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Process
            // 
            this.Process.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Process.Location = new System.Drawing.Point(673, 644);
            this.Process.Name = "Process";
            this.Process.Size = new System.Drawing.Size(92, 34);
            this.Process.TabIndex = 0;
            this.Process.Text = "Process";
            this.Process.UseVisualStyleBackColor = true;
            this.Process.Click += new System.EventHandler(this.Process_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(12, 644);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(92, 34);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // BorrowerDemos
            // 
            this.BorrowerDemos.Controls.Add(this.DobTxt);
            this.BorrowerDemos.Controls.Add(this.label4);
            this.BorrowerDemos.Controls.Add(this.AccountTxt);
            this.BorrowerDemos.Controls.Add(this.label3);
            this.BorrowerDemos.Controls.Add(this.SsnTxt);
            this.BorrowerDemos.Controls.Add(this.NameTxt);
            this.BorrowerDemos.Controls.Add(this.label2);
            this.BorrowerDemos.Controls.Add(this.label1);
            this.BorrowerDemos.Location = new System.Drawing.Point(12, 27);
            this.BorrowerDemos.Name = "BorrowerDemos";
            this.BorrowerDemos.Size = new System.Drawing.Size(637, 122);
            this.BorrowerDemos.TabIndex = 2;
            this.BorrowerDemos.TabStop = false;
            this.BorrowerDemos.Text = "Borrower Demographics";
            // 
            // DobTxt
            // 
            this.DobTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DobTxt.Enabled = false;
            this.DobTxt.Location = new System.Drawing.Point(519, 71);
            this.DobTxt.MaxLength = 9;
            this.DobTxt.Name = "DobTxt";
            this.DobTxt.Size = new System.Drawing.Size(98, 26);
            this.DobTxt.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(471, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "DOB";
            // 
            // AccountTxt
            // 
            this.AccountTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountTxt.Enabled = false;
            this.AccountTxt.Location = new System.Drawing.Point(283, 71);
            this.AccountTxt.MaxLength = 10;
            this.AccountTxt.Name = "AccountTxt";
            this.AccountTxt.Size = new System.Drawing.Size(162, 26);
            this.AccountTxt.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(196, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Account #";
            // 
            // SsnTxt
            // 
            this.SsnTxt.Enabled = false;
            this.SsnTxt.Location = new System.Drawing.Point(74, 71);
            this.SsnTxt.MaxLength = 9;
            this.SsnTxt.Name = "SsnTxt";
            this.SsnTxt.Size = new System.Drawing.Size(98, 26);
            this.SsnTxt.TabIndex = 3;
            // 
            // NameTxt
            // 
            this.NameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameTxt.Enabled = false;
            this.NameTxt.Location = new System.Drawing.Point(74, 32);
            this.NameTxt.Name = "NameTxt";
            this.NameTxt.Size = new System.Drawing.Size(543, 26);
            this.NameTxt.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "SSN";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(91, 594);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(48, 20);
            this.label16.TabIndex = 35;
            this.label16.Text = "Email";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 419);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(124, 20);
            this.label13.TabIndex = 34;
            this.label13.Text = "Alternate Phone";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(40, 454);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(99, 20);
            this.label12.TabIndex = 33;
            this.label12.Text = "Other Phone";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(28, 389);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 20);
            this.label11.TabIndex = 32;
            this.label11.Text = "Primary Phone";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(75, 354);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 20);
            this.label10.TabIndex = 31;
            this.label10.Text = "Country";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(341, 320);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 20);
            this.label9.TabIndex = 30;
            this.label9.Text = "Zip";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(91, 317);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 20);
            this.label8.TabIndex = 29;
            this.label8.Text = "State";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(104, 283);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 20);
            this.label7.TabIndex = 28;
            this.label7.Text = "City";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(58, 249);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 20);
            this.label6.TabIndex = 27;
            this.label6.Text = "Address 2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(58, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 20);
            this.label5.TabIndex = 26;
            this.label5.Text = "Address 1";
            // 
            // Address1
            // 
            this.Address1.AllowAllCharacters = true;
            this.Address1.AllowAlphaCharacters = true;
            this.Address1.AllowedAdditionalCharacters = "";
            this.Address1.AllowNumericCharacters = true;
            this.Address1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Address1.InvalidColor = System.Drawing.Color.LightPink;
            this.Address1.Location = new System.Drawing.Point(155, 215);
            this.Address1.Mask = "";
            this.Address1.Name = "Address1";
            this.Address1.Size = new System.Drawing.Size(372, 26);
            this.Address1.TabIndex = 52;
            this.Address1.ValidationMessage = null;
            this.Address1.ValidColor = System.Drawing.Color.LightGreen;
            this.Address1.TextChanged += new System.EventHandler(this.Address1_TextChanged);
            // 
            // Address2
            // 
            this.Address2.AllowAllCharacters = true;
            this.Address2.AllowAlphaCharacters = true;
            this.Address2.AllowedAdditionalCharacters = "";
            this.Address2.AllowNumericCharacters = true;
            this.Address2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Address2.InvalidColor = System.Drawing.Color.LightPink;
            this.Address2.Location = new System.Drawing.Point(155, 248);
            this.Address2.Mask = "";
            this.Address2.Name = "Address2";
            this.Address2.Size = new System.Drawing.Size(372, 26);
            this.Address2.TabIndex = 53;
            this.Address2.ValidationMessage = null;
            this.Address2.ValidColor = System.Drawing.Color.LightGreen;
            this.Address2.TextChanged += new System.EventHandler(this.Address2_TextChanged);
            // 
            // City
            // 
            this.City.AllowAllCharacters = true;
            this.City.AllowAlphaCharacters = true;
            this.City.AllowedAdditionalCharacters = "";
            this.City.AllowNumericCharacters = true;
            this.City.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.City.InvalidColor = System.Drawing.Color.LightPink;
            this.City.Location = new System.Drawing.Point(155, 281);
            this.City.Mask = "";
            this.City.Name = "City";
            this.City.Size = new System.Drawing.Size(372, 26);
            this.City.TabIndex = 54;
            this.City.ValidationMessage = null;
            this.City.ValidColor = System.Drawing.Color.LightGreen;
            this.City.TextChanged += new System.EventHandler(this.City_TextChanged);
            // 
            // ZipCode
            // 
            this.ZipCode.AllowAllCharacters = true;
            this.ZipCode.AllowAlphaCharacters = true;
            this.ZipCode.AllowedAdditionalCharacters = "";
            this.ZipCode.AllowNumericCharacters = true;
            this.ZipCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ZipCode.InvalidColor = System.Drawing.Color.LightPink;
            this.ZipCode.Location = new System.Drawing.Point(383, 317);
            this.ZipCode.Mask = "";
            this.ZipCode.Name = "ZipCode";
            this.ZipCode.Size = new System.Drawing.Size(144, 26);
            this.ZipCode.TabIndex = 55;
            this.ZipCode.ValidationMessage = null;
            this.ZipCode.ValidColor = System.Drawing.Color.LightGreen;
            this.ZipCode.TextChanged += new System.EventHandler(this.ZipCode_TextChanged);
            // 
            // State
            // 
            this.State.FormattingEnabled = true;
            this.State.Items.AddRange(new object[] {
            "AA",
            "AE",
            "AK",
            "AL",
            "AP",
            "AR",
            "AS",
            "AZ",
            "CA",
            "CO",
            "CT",
            "DC",
            "DE",
            "FL",
            "FM",
            "GA",
            "GU",
            "HI",
            "IA",
            "ID",
            "IL",
            "IN",
            "KS",
            "KY",
            "LA",
            "MA",
            "MD",
            "ME",
            "MH",
            "MI",
            "MN",
            "MO",
            "MP",
            "MS",
            "MT",
            "NC",
            "ND",
            "NE",
            "NH",
            "NJ",
            "NM",
            "NV",
            "NY",
            "OH",
            "OK",
            "OR",
            "PA",
            "PR",
            "PW",
            "RI",
            "SC",
            "SD",
            "TN",
            "TX",
            "UT",
            "VA",
            "VI",
            "VT",
            "WA",
            "WI",
            "WV",
            "WY"});
            this.State.Location = new System.Drawing.Point(155, 314);
            this.State.Name = "State";
            this.State.Size = new System.Drawing.Size(105, 28);
            this.State.TabIndex = 56;
            this.State.SelectedIndexChanged += new System.EventHandler(this.State_SelectedIndexChanged);
            this.State.Leave += new System.EventHandler(this.State_Leave);
            // 
            // Country
            // 
            this.Country.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Country.FormattingEnabled = true;
            this.Country.Location = new System.Drawing.Point(155, 351);
            this.Country.Name = "Country";
            this.Country.Size = new System.Drawing.Size(372, 28);
            this.Country.TabIndex = 58;
            this.Country.SelectedIndexChanged += new System.EventHandler(this.Country_SelectedIndexChanged);
            // 
            // Email
            // 
            this.Email.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Email.Location = new System.Drawing.Point(155, 589);
            this.Email.Name = "Email";
            this.Email.Size = new System.Drawing.Size(500, 26);
            this.Email.TabIndex = 69;
            this.Email.TextChanged += new System.EventHandler(this.Email_TextChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.LightGreen;
            this.label21.Location = new System.Drawing.Point(6, 22);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(62, 20);
            this.label21.TabIndex = 80;
            this.label21.Text = "Original";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.LightYellow;
            this.label22.Location = new System.Drawing.Point(6, 51);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(71, 20);
            this.label22.TabIndex = 81;
            this.label22.Text = "Updated";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.LightPink;
            this.label23.Location = new System.Drawing.Point(6, 80);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(74, 20);
            this.label23.TabIndex = 82;
            this.label23.Text = "Required";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Location = new System.Drawing.Point(662, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(96, 111);
            this.groupBox1.TabIndex = 83;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Key";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(7, 517);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(132, 20);
            this.label25.TabIndex = 85;
            this.label25.Text = "Foreign Alternate";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(20, 486);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(119, 20);
            this.label26.TabIndex = 84;
            this.label26.Text = "Foreign Primary";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(32, 551);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(107, 20);
            this.label15.TabIndex = 96;
            this.label15.Text = "Foreign Other";
            // 
            // Source
            // 
            this.Source.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Source.FormattingEnabled = true;
            this.Source.Location = new System.Drawing.Point(155, 168);
            this.Source.Name = "Source";
            this.Source.Size = new System.Drawing.Size(500, 28);
            this.Source.TabIndex = 103;
            this.Source.SelectedIndexChanged += new System.EventHandler(this.Source_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(75, 171);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 20);
            this.label14.TabIndex = 102;
            this.label14.Text = "Source";
            // 
            // AddressForeign
            // 
            this.AddressForeign.AutoSize = true;
            this.AddressForeign.Location = new System.Drawing.Point(537, 217);
            this.AddressForeign.Name = "AddressForeign";
            this.AddressForeign.Size = new System.Drawing.Size(82, 24);
            this.AddressForeign.TabIndex = 104;
            this.AddressForeign.Text = "Foreign";
            this.AddressForeign.UseVisualStyleBackColor = true;
            this.AddressForeign.CheckedChanged += new System.EventHandler(this.CheckAddressForeign);
            // 
            // AddressValid
            // 
            this.AddressValid.AutoSize = true;
            this.AddressValid.Location = new System.Drawing.Point(537, 355);
            this.AddressValid.Name = "AddressValid";
            this.AddressValid.Size = new System.Drawing.Size(63, 24);
            this.AddressValid.TabIndex = 105;
            this.AddressValid.Text = "Valid";
            this.AddressValid.UseVisualStyleBackColor = true;
            this.AddressValid.CheckedChanged += new System.EventHandler(this.CheckAddressValid);
            // 
            // EmailValid
            // 
            this.EmailValid.AutoSize = true;
            this.EmailValid.Location = new System.Drawing.Point(662, 590);
            this.EmailValid.Name = "EmailValid";
            this.EmailValid.Size = new System.Drawing.Size(63, 24);
            this.EmailValid.TabIndex = 106;
            this.EmailValid.Text = "Valid";
            this.EmailValid.UseVisualStyleBackColor = true;
            this.EmailValid.CheckedChanged += new System.EventHandler(this.CheckEmailValid);
            // 
            // AlternatePhoneValid
            // 
            this.AlternatePhoneValid.AutoSize = true;
            this.AlternatePhoneValid.Location = new System.Drawing.Point(439, 421);
            this.AlternatePhoneValid.Name = "AlternatePhoneValid";
            this.AlternatePhoneValid.Size = new System.Drawing.Size(63, 24);
            this.AlternatePhoneValid.TabIndex = 107;
            this.AlternatePhoneValid.Text = "Valid";
            this.AlternatePhoneValid.UseVisualStyleBackColor = true;
            this.AlternatePhoneValid.CheckedChanged += new System.EventHandler(this.CheckAlternateValid);
            // 
            // PrimaryPhoneValid
            // 
            this.PrimaryPhoneValid.AutoSize = true;
            this.PrimaryPhoneValid.Location = new System.Drawing.Point(439, 389);
            this.PrimaryPhoneValid.Name = "PrimaryPhoneValid";
            this.PrimaryPhoneValid.Size = new System.Drawing.Size(63, 24);
            this.PrimaryPhoneValid.TabIndex = 108;
            this.PrimaryPhoneValid.Text = "Valid";
            this.PrimaryPhoneValid.UseVisualStyleBackColor = true;
            this.PrimaryPhoneValid.CheckedChanged += new System.EventHandler(this.CheckPrimaryPhoneValid);
            // 
            // OtherPhoneValid
            // 
            this.OtherPhoneValid.AutoSize = true;
            this.OtherPhoneValid.Location = new System.Drawing.Point(439, 453);
            this.OtherPhoneValid.Name = "OtherPhoneValid";
            this.OtherPhoneValid.Size = new System.Drawing.Size(63, 24);
            this.OtherPhoneValid.TabIndex = 109;
            this.OtherPhoneValid.Text = "Valid";
            this.OtherPhoneValid.UseVisualStyleBackColor = true;
            this.OtherPhoneValid.CheckedChanged += new System.EventHandler(this.CheckOtherPhoneValid);
            // 
            // PrimaryPhoneConsent
            // 
            this.PrimaryPhoneConsent.AutoSize = true;
            this.PrimaryPhoneConsent.Location = new System.Drawing.Point(345, 389);
            this.PrimaryPhoneConsent.Name = "PrimaryPhoneConsent";
            this.PrimaryPhoneConsent.Size = new System.Drawing.Size(88, 24);
            this.PrimaryPhoneConsent.TabIndex = 110;
            this.PrimaryPhoneConsent.Text = "Consent";
            this.PrimaryPhoneConsent.UseVisualStyleBackColor = true;
            this.PrimaryPhoneConsent.CheckedChanged += new System.EventHandler(this.CheckPrimaryPhoneConsent);
            // 
            // AlternatePhoneConsent
            // 
            this.AlternatePhoneConsent.AutoSize = true;
            this.AlternatePhoneConsent.Location = new System.Drawing.Point(345, 421);
            this.AlternatePhoneConsent.Name = "AlternatePhoneConsent";
            this.AlternatePhoneConsent.Size = new System.Drawing.Size(88, 24);
            this.AlternatePhoneConsent.TabIndex = 111;
            this.AlternatePhoneConsent.Text = "Consent";
            this.AlternatePhoneConsent.UseVisualStyleBackColor = true;
            this.AlternatePhoneConsent.CheckedChanged += new System.EventHandler(this.CheckAlternateConsent);
            // 
            // OtherPhoneConsent
            // 
            this.OtherPhoneConsent.AutoSize = true;
            this.OtherPhoneConsent.Location = new System.Drawing.Point(345, 453);
            this.OtherPhoneConsent.Name = "OtherPhoneConsent";
            this.OtherPhoneConsent.Size = new System.Drawing.Size(88, 24);
            this.OtherPhoneConsent.TabIndex = 112;
            this.OtherPhoneConsent.Text = "Consent";
            this.OtherPhoneConsent.UseVisualStyleBackColor = true;
            this.OtherPhoneConsent.CheckedChanged += new System.EventHandler(this.CheckOtherPhoneConsent);
            // 
            // ForeignOtherPhone
            // 
            this.ForeignOtherPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForeignOtherPhone.Extension = "";
            this.ForeignOtherPhone.ForeignCity = "";
            this.ForeignOtherPhone.ForeignCountry = "";
            this.ForeignOtherPhone.ForeignLocal = "";
            this.ForeignOtherPhone.IsForeign = false;
            this.ForeignOtherPhone.Location = new System.Drawing.Point(155, 549);
            this.ForeignOtherPhone.Name = "ForeignOtherPhone";
            this.ForeignOtherPhone.PhoneNumber = "";
            this.ForeignOtherPhone.Size = new System.Drawing.Size(178, 26);
            this.ForeignOtherPhone.TabIndex = 99;
            this.ForeignOtherPhone.TextChanged += new System.EventHandler(this.ForeignOtherPhone_TextChanged);
            // 
            // ForeignAltPhone
            // 
            this.ForeignAltPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForeignAltPhone.Extension = "";
            this.ForeignAltPhone.ForeignCity = "";
            this.ForeignAltPhone.ForeignCountry = "";
            this.ForeignAltPhone.ForeignLocal = "";
            this.ForeignAltPhone.IsForeign = false;
            this.ForeignAltPhone.Location = new System.Drawing.Point(155, 517);
            this.ForeignAltPhone.Name = "ForeignAltPhone";
            this.ForeignAltPhone.PhoneNumber = "";
            this.ForeignAltPhone.Size = new System.Drawing.Size(178, 26);
            this.ForeignAltPhone.TabIndex = 91;
            this.ForeignAltPhone.TextChanged += new System.EventHandler(this.ForeignAltPhone_TextChanged);
            // 
            // ForeignPrimaryPhone
            // 
            this.ForeignPrimaryPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForeignPrimaryPhone.Extension = "";
            this.ForeignPrimaryPhone.ForeignCity = "";
            this.ForeignPrimaryPhone.ForeignCountry = "";
            this.ForeignPrimaryPhone.ForeignLocal = "";
            this.ForeignPrimaryPhone.IsForeign = false;
            this.ForeignPrimaryPhone.Location = new System.Drawing.Point(155, 484);
            this.ForeignPrimaryPhone.Name = "ForeignPrimaryPhone";
            this.ForeignPrimaryPhone.PhoneNumber = "";
            this.ForeignPrimaryPhone.Size = new System.Drawing.Size(178, 26);
            this.ForeignPrimaryPhone.TabIndex = 90;
            this.ForeignPrimaryPhone.TextChanged += new System.EventHandler(this.ForeignPrimaryPhone_TextChanged);
            // 
            // AlternatePhone
            // 
            this.AlternatePhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AlternatePhone.Extension = "";
            this.AlternatePhone.ForeignCity = "";
            this.AlternatePhone.ForeignCountry = "";
            this.AlternatePhone.ForeignLocal = "";
            this.AlternatePhone.IsForeign = false;
            this.AlternatePhone.Location = new System.Drawing.Point(155, 418);
            this.AlternatePhone.Name = "AlternatePhone";
            this.AlternatePhone.PhoneNumber = "";
            this.AlternatePhone.Size = new System.Drawing.Size(178, 26);
            this.AlternatePhone.TabIndex = 61;
            this.AlternatePhone.TextChanged += new System.EventHandler(this.AlternatePhone_TextChanged);
            // 
            // OtherPhone
            // 
            this.OtherPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OtherPhone.Extension = "";
            this.OtherPhone.ForeignCity = "";
            this.OtherPhone.ForeignCountry = "";
            this.OtherPhone.ForeignLocal = "";
            this.OtherPhone.IsForeign = false;
            this.OtherPhone.Location = new System.Drawing.Point(155, 451);
            this.OtherPhone.Name = "OtherPhone";
            this.OtherPhone.PhoneNumber = "";
            this.OtherPhone.Size = new System.Drawing.Size(178, 26);
            this.OtherPhone.TabIndex = 60;
            this.OtherPhone.TextChanged += new System.EventHandler(this.OtherPhone_TextChanged);
            // 
            // PrimaryPhone
            // 
            this.PrimaryPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PrimaryPhone.Extension = "";
            this.PrimaryPhone.ForeignCity = "";
            this.PrimaryPhone.ForeignCountry = "";
            this.PrimaryPhone.ForeignLocal = "";
            this.PrimaryPhone.IsForeign = false;
            this.PrimaryPhone.Location = new System.Drawing.Point(155, 386);
            this.PrimaryPhone.Name = "PrimaryPhone";
            this.PrimaryPhone.PhoneNumber = "";
            this.PrimaryPhone.Size = new System.Drawing.Size(178, 26);
            this.PrimaryPhone.TabIndex = 59;
            this.PrimaryPhone.TextChanged += new System.EventHandler(this.PrimaryPhone_TextChanged);
            // 
            // toolStripMenuItem411
            // 
            this.toolStripMenuItem411.Name = "toolStripMenuItem411";
            this.toolStripMenuItem411.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem411.Text = "411";
            this.toolStripMenuItem411.Click += new System.EventHandler(this.ToolStripMenuItem411_Click);
            // 
            // IRToolStripMenuItem
            // 
            this.IRToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.securityIncidentToolStripMenuItem,
            this.physicalThreatToolStripMenuItem});
            this.IRToolStripMenuItem.Name = "IRToolStripMenuItem";
            this.IRToolStripMenuItem.Size = new System.Drawing.Size(117, 20);
            this.IRToolStripMenuItem.Text = "Incident Reporting";
            // 
            // securityIncidentToolStripMenuItem
            // 
            this.securityIncidentToolStripMenuItem.Name = "securityIncidentToolStripMenuItem";
            this.securityIncidentToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.securityIncidentToolStripMenuItem.Text = "Security Incident";
            this.securityIncidentToolStripMenuItem.Click += new System.EventHandler(this.SecurityIncidentToolStripMenuItem_Click);
            // 
            // physicalThreatToolStripMenuItem
            // 
            this.physicalThreatToolStripMenuItem.Name = "physicalThreatToolStripMenuItem";
            this.physicalThreatToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.physicalThreatToolStripMenuItem.Text = "Physical Threat";
            this.physicalThreatToolStripMenuItem.Click += new System.EventHandler(this.PhysicalThreatToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem411,
            this.IRToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(777, 24);
            this.menuStrip1.TabIndex = 79;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Demographics
            // 
            this.AcceptButton = this.Process;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(174)))), ((int)(((byte)(231)))));
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(777, 690);
            this.Controls.Add(this.OtherPhoneConsent);
            this.Controls.Add(this.AlternatePhoneConsent);
            this.Controls.Add(this.PrimaryPhoneConsent);
            this.Controls.Add(this.OtherPhoneValid);
            this.Controls.Add(this.PrimaryPhoneValid);
            this.Controls.Add(this.AlternatePhoneValid);
            this.Controls.Add(this.EmailValid);
            this.Controls.Add(this.AddressValid);
            this.Controls.Add(this.AddressForeign);
            this.Controls.Add(this.Source);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.ForeignOtherPhone);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.ForeignAltPhone);
            this.Controls.Add(this.ForeignPrimaryPhone);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Email);
            this.Controls.Add(this.AlternatePhone);
            this.Controls.Add(this.OtherPhone);
            this.Controls.Add(this.PrimaryPhone);
            this.Controls.Add(this.Country);
            this.Controls.Add(this.State);
            this.Controls.Add(this.ZipCode);
            this.Controls.Add(this.City);
            this.Controls.Add(this.Address2);
            this.Controls.Add(this.Address1);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BorrowerDemos);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Process);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(793, 729);
            this.Name = "Demographics";
            this.Text = "Demographics";
            this.BorrowerDemos.ResumeLayout(false);
            this.BorrowerDemos.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Process;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.GroupBox BorrowerDemos;
        private System.Windows.Forms.TextBox DobTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AccountTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SsnTxt;
        private System.Windows.Forms.TextBox NameTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private Uheaa.Common.WinForms.OmniTextBox Address1;
        private Uheaa.Common.WinForms.OmniTextBox Address2;
        private Uheaa.Common.WinForms.OmniTextBox City;
        private Uheaa.Common.WinForms.OmniTextBox ZipCode;
        private System.Windows.Forms.ComboBox State;
        private System.Windows.Forms.ComboBox Country;
        private PhoneBox PrimaryPhone;
        private PhoneBox OtherPhone;
        private PhoneBox AlternatePhone;
        private System.Windows.Forms.TextBox Email;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.GroupBox groupBox1;
        private PhoneBox ForeignAltPhone;
        private PhoneBox ForeignPrimaryPhone;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private PhoneBox ForeignOtherPhone;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox Source;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox AddressForeign;
        private System.Windows.Forms.CheckBox AddressValid;
        private System.Windows.Forms.CheckBox EmailValid;
        private System.Windows.Forms.CheckBox AlternatePhoneValid;
        private System.Windows.Forms.CheckBox PrimaryPhoneValid;
        private System.Windows.Forms.CheckBox OtherPhoneValid;
        private System.Windows.Forms.CheckBox PrimaryPhoneConsent;
        private System.Windows.Forms.CheckBox AlternatePhoneConsent;
        private System.Windows.Forms.CheckBox OtherPhoneConsent;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem411;
        private System.Windows.Forms.ToolStripMenuItem IRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem securityIncidentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem physicalThreatToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
    }
}