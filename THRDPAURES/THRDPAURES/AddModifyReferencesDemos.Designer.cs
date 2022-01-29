namespace THRDPAURES
{
    partial class AddModifyReferencesDemos
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
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Relationship = new System.Windows.Forms.ComboBox();
            this.NoDemos = new System.Windows.Forms.CheckBox();
            this.ForeignDemos = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.StateLabel = new System.Windows.Forms.Label();
            this.ForeignCodeLabel = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.AddressType = new System.Windows.Forms.ComboBox();
            this.ForeignCode = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.ValidPhone = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.PhoneType = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.EmailAddress = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FirstName = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.Ssn = new Uheaa.Common.WinForms.SsnTextBox();
            this.LastName = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.MiddleName = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.City = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.Address3 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.Address2 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.Address1 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.State = new Uheaa.Common.WinForms.AlphaTextBox();
            this.ZipCode = new Uheaa.Common.WinForms.NumericTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SourceCode = new System.Windows.Forms.ComboBox();
            this.LastVerifiedDate = new Uheaa.Common.WinForms.MaskedDateTextBox();
            this.Mbl = new System.Windows.Forms.ComboBox();
            this.Phone = new Uheaa.Common.WinForms.NumericMaskedTextBox();
            this.Consent = new Uheaa.Common.WinForms.AlphaTextBox();
            this.PhoneExt = new Uheaa.Common.WinForms.NumericTextBox();
            this.ForeignPhoneExt = new Uheaa.Common.WinForms.NumericTextBox();
            this.ForeignPhone = new Uheaa.Common.WinForms.NumericMaskedTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Borrower\'s SSN:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(2, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Relationship:";
            // 
            // Relationship
            // 
            this.Relationship.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Relationship.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Relationship.FormattingEnabled = true;
            this.Relationship.Location = new System.Drawing.Point(109, 92);
            this.Relationship.Name = "Relationship";
            this.Relationship.Size = new System.Drawing.Size(272, 28);
            this.Relationship.TabIndex = 5;
            // 
            // NoDemos
            // 
            this.NoDemos.AutoSize = true;
            this.NoDemos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoDemos.Location = new System.Drawing.Point(157, 9);
            this.NoDemos.Name = "NoDemos";
            this.NoDemos.Size = new System.Drawing.Size(176, 24);
            this.NoDemos.TabIndex = 10;
            this.NoDemos.Text = "No Address Provided";
            this.NoDemos.UseVisualStyleBackColor = true;
            this.NoDemos.CheckedChanged += new System.EventHandler(this.NoDemos_CheckedChanged);
            // 
            // ForeignDemos
            // 
            this.ForeignDemos.AutoSize = true;
            this.ForeignDemos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeignDemos.Location = new System.Drawing.Point(6, 9);
            this.ForeignDemos.Name = "ForeignDemos";
            this.ForeignDemos.Size = new System.Drawing.Size(145, 24);
            this.ForeignDemos.TabIndex = 6;
            this.ForeignDemos.Text = "Foreign Address";
            this.ForeignDemos.UseVisualStyleBackColor = true;
            this.ForeignDemos.CheckedChanged += new System.EventHandler(this.ForeignDemos_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Address 1:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 128);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 20);
            this.label7.TabIndex = 13;
            this.label7.Text = "Address 2:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 20);
            this.label8.TabIndex = 14;
            this.label8.Text = "Address 3:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(465, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 20);
            this.label9.TabIndex = 15;
            this.label9.Text = "City:";
            // 
            // StateLabel
            // 
            this.StateLabel.AutoSize = true;
            this.StateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StateLabel.Location = new System.Drawing.Point(452, 90);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(52, 20);
            this.StateLabel.TabIndex = 16;
            this.StateLabel.Text = "State:";
            // 
            // ForeignCodeLabel
            // 
            this.ForeignCodeLabel.AutoSize = true;
            this.ForeignCodeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeignCodeLabel.Location = new System.Drawing.Point(427, 165);
            this.ForeignCodeLabel.Name = "ForeignCodeLabel";
            this.ForeignCodeLabel.Size = new System.Drawing.Size(109, 20);
            this.ForeignCodeLabel.TabIndex = 17;
            this.ForeignCodeLabel.Text = "Foreign Code:";
            this.ForeignCodeLabel.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(427, 128);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 20);
            this.label12.TabIndex = 18;
            this.label12.Text = "Zip Code:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(6, 52);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(110, 20);
            this.label14.TabIndex = 20;
            this.label14.Text = "Address Type:";
            // 
            // AddressType
            // 
            this.AddressType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AddressType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddressType.Location = new System.Drawing.Point(118, 49);
            this.AddressType.Name = "AddressType";
            this.AddressType.Size = new System.Drawing.Size(284, 28);
            this.AddressType.TabIndex = 7;
            this.AddressType.SelectionChangeCommitted += new System.EventHandler(this.AddressType_SelectionChangeCommitted);
            this.AddressType.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AddressType_MouseClick);
            // 
            // ForeignCode
            // 
            this.ForeignCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ForeignCode.DropDownWidth = 280;
            this.ForeignCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeignCode.FormattingEnabled = true;
            this.ForeignCode.Location = new System.Drawing.Point(542, 160);
            this.ForeignCode.Name = "ForeignCode";
            this.ForeignCode.Size = new System.Drawing.Size(121, 28);
            this.ForeignCode.TabIndex = 15;
            this.ForeignCode.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(7, 53);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 20);
            this.label10.TabIndex = 30;
            this.label10.Text = "MBL:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(110, 53);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(73, 20);
            this.label11.TabIndex = 32;
            this.label11.Text = "Consent:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(245, 53);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(141, 20);
            this.label13.TabIndex = 34;
            this.label13.Text = "Last Verified Date:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(7, 90);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(130, 20);
            this.label15.TabIndex = 36;
            this.label15.Text = "Domestic Phone:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(279, 90);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(83, 20);
            this.label16.TabIndex = 39;
            this.label16.Text = "Extension:";
            // 
            // ValidPhone
            // 
            this.ValidPhone.AutoSize = true;
            this.ValidPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValidPhone.Location = new System.Drawing.Point(456, 89);
            this.ValidPhone.Name = "ValidPhone";
            this.ValidPhone.Size = new System.Drawing.Size(80, 24);
            this.ValidPhone.TabIndex = 24;
            this.ValidPhone.Text = "Is Valid";
            this.ValidPhone.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(7, 124);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(117, 20);
            this.label17.TabIndex = 42;
            this.label17.Text = "Foreign Phone:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(339, 124);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(83, 20);
            this.label18.TabIndex = 44;
            this.label18.Text = "Extension:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(494, 53);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(106, 20);
            this.label19.TabIndex = 47;
            this.label19.Text = "Source Code:";
            // 
            // PhoneType
            // 
            this.PhoneType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PhoneType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PhoneType.FormattingEnabled = true;
            this.PhoneType.Location = new System.Drawing.Point(110, 13);
            this.PhoneType.Name = "PhoneType";
            this.PhoneType.Size = new System.Drawing.Size(139, 28);
            this.PhoneType.TabIndex = 16;
            this.PhoneType.SelectionChangeCommitted += new System.EventHandler(this.PhoneType_SelectionChangeCommitted);
            this.PhoneType.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PhoneType_MouseClick);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(7, 16);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(97, 20);
            this.label20.TabIndex = 50;
            this.label20.Text = "Phone Type:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(7, 158);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(115, 20);
            this.label21.TabIndex = 51;
            this.label21.Text = "Email Address:";
            // 
            // OK
            // 
            this.OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK.Location = new System.Drawing.Point(564, 139);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(72, 42);
            this.OK.TabIndex = 28;
            this.OK.Text = "Save";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.Location = new System.Drawing.Point(647, 139);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(72, 42);
            this.Cancel.TabIndex = 29;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // EmailAddress
            // 
            this.EmailAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmailAddress.Location = new System.Drawing.Point(128, 155);
            this.EmailAddress.MaxLength = 254;
            this.EmailAddress.Name = "EmailAddress";
            this.EmailAddress.Size = new System.Drawing.Size(405, 26);
            this.EmailAddress.TabIndex = 27;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FirstName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Ssn);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.Relationship);
            this.groupBox1.Controls.Add(this.LastName);
            this.groupBox1.Controls.Add(this.MiddleName);
            this.groupBox1.Location = new System.Drawing.Point(0, -5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(734, 141);
            this.groupBox1.TabIndex = 63;
            this.groupBox1.TabStop = false;
            // 
            // FirstName
            // 
            this.FirstName.Enabled = false;
            this.FirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.FirstName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FirstName.Location = new System.Drawing.Point(6, 45);
            this.FirstName.MaxLength = 13;
            this.FirstName.Name = "FirstName";
            this.FirstName.Size = new System.Drawing.Size(168, 26);
            this.FirstName.TabIndex = 2;
            this.FirstName.Text = "First Name";
            this.FirstName.Watermark = "First Name";
            // 
            // Ssn
            // 
            this.Ssn.AllowedSpecialCharacters = null;
            this.Ssn.Enabled = false;
            this.Ssn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ssn.Location = new System.Drawing.Point(132, 13);
            this.Ssn.MaxLength = 9;
            this.Ssn.Name = "Ssn";
            this.Ssn.Size = new System.Drawing.Size(100, 26);
            this.Ssn.Ssn = null;
            this.Ssn.TabIndex = 1;
            // 
            // LastName
            // 
            this.LastName.Enabled = false;
            this.LastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.LastName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LastName.Location = new System.Drawing.Point(358, 45);
            this.LastName.MaxLength = 23;
            this.LastName.Name = "LastName";
            this.LastName.Size = new System.Drawing.Size(299, 26);
            this.LastName.TabIndex = 4;
            this.LastName.Text = "Last Name";
            this.LastName.Watermark = "Last Name";
            // 
            // MiddleName
            // 
            this.MiddleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.MiddleName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.MiddleName.Location = new System.Drawing.Point(184, 45);
            this.MiddleName.MaxLength = 13;
            this.MiddleName.Name = "MiddleName";
            this.MiddleName.Size = new System.Drawing.Size(168, 26);
            this.MiddleName.TabIndex = 3;
            this.MiddleName.Text = "Middle Name";
            this.MiddleName.Watermark = "Middle Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.NoDemos);
            this.groupBox2.Controls.Add(this.City);
            this.groupBox2.Controls.Add(this.ForeignDemos);
            this.groupBox2.Controls.Add(this.Address3);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.Address2);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.Address1);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.StateLabel);
            this.groupBox2.Controls.Add(this.ForeignCodeLabel);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.AddressType);
            this.groupBox2.Controls.Add(this.State);
            this.groupBox2.Controls.Add(this.ZipCode);
            this.groupBox2.Controls.Add(this.ForeignCode);
            this.groupBox2.Location = new System.Drawing.Point(0, 133);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(734, 213);
            this.groupBox2.TabIndex = 64;
            this.groupBox2.TabStop = false;
            // 
            // City
            // 
            this.City.AllowedSpecialCharacters = " ";
            this.City.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.City.Location = new System.Drawing.Point(508, 52);
            this.City.MaxLength = 20;
            this.City.Name = "City";
            this.City.Size = new System.Drawing.Size(170, 26);
            this.City.TabIndex = 12;
            // 
            // Address3
            // 
            this.Address3.AllowedSpecialCharacters = " ,.";
            this.Address3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Address3.Location = new System.Drawing.Point(97, 162);
            this.Address3.MaxLength = 30;
            this.Address3.Name = "Address3";
            this.Address3.Size = new System.Drawing.Size(311, 26);
            this.Address3.TabIndex = 10;
            // 
            // Address2
            // 
            this.Address2.AllowedSpecialCharacters = " ,.";
            this.Address2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Address2.Location = new System.Drawing.Point(97, 125);
            this.Address2.MaxLength = 30;
            this.Address2.Name = "Address2";
            this.Address2.Size = new System.Drawing.Size(311, 26);
            this.Address2.TabIndex = 9;
            // 
            // Address1
            // 
            this.Address1.AllowedSpecialCharacters = " ,.";
            this.Address1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Address1.Location = new System.Drawing.Point(97, 87);
            this.Address1.MaxLength = 30;
            this.Address1.Name = "Address1";
            this.Address1.Size = new System.Drawing.Size(311, 26);
            this.Address1.TabIndex = 8;
            // 
            // State
            // 
            this.State.AllowedSpecialCharacters = null;
            this.State.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.State.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.State.Location = new System.Drawing.Point(508, 90);
            this.State.MaxLength = 2;
            this.State.Name = "State";
            this.State.Size = new System.Drawing.Size(37, 26);
            this.State.TabIndex = 13;
            // 
            // ZipCode
            // 
            this.ZipCode.AllowedSpecialCharacters = null;
            this.ZipCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZipCode.Location = new System.Drawing.Point(508, 125);
            this.ZipCode.Name = "ZipCode";
            this.ZipCode.Size = new System.Drawing.Size(100, 26);
            this.ZipCode.TabIndex = 14;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.SourceCode);
            this.groupBox3.Controls.Add(this.LastVerifiedDate);
            this.groupBox3.Controls.Add(this.Mbl);
            this.groupBox3.Controls.Add(this.Phone);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.Cancel);
            this.groupBox3.Controls.Add(this.Consent);
            this.groupBox3.Controls.Add(this.OK);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.EmailAddress);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.PhoneType);
            this.groupBox3.Controls.Add(this.PhoneExt);
            this.groupBox3.Controls.Add(this.ValidPhone);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.ForeignPhoneExt);
            this.groupBox3.Controls.Add(this.ForeignPhone);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Location = new System.Drawing.Point(0, 339);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(734, 201);
            this.groupBox3.TabIndex = 65;
            this.groupBox3.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(266, 15);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(163, 24);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "No Phone Provided";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // SourceCode
            // 
            this.SourceCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SourceCode.DropDownWidth = 280;
            this.SourceCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SourceCode.FormattingEnabled = true;
            this.SourceCode.Items.AddRange(new object[] {
            "01      ELECTRONIC MEDIA",
            "02      APPLICATION",
            "03      PROMISSORY NOTE",
            "04      APP/PROM NOTE",
            "05      CORRESPONDENCE",
            "06      REPAYMENT OBLIGATI",
            "07      CORRECT FORM",
            "08      DEFERMENT FORM",
            "09      FORBEARANCE FORM",
            "10      GUARANTEE STATEMENT",
            "11      CREDIT REPORT",
            "12      PARENT",
            "13      SPOUSE",
            "14      SIBLING",
            "15      ROOMMATE",
            "16      NEIGHBOR",
            "18      AUNT/UNCLE",
            "19      GRANDPARENT",
            "20      COUSIN",
            "21      NIECE/NEPHEW",
            "22      CHILD",
            "23      EMPLOYER",
            "24      DIRECTORY ASSISTANC",
            "26      DEPART MOTOR VEHIC",
            "27      LANDLORD",
            "28      MILITARY",
            "29      IRS",
            "30      OUT COLLECTOR",
            "31      UNKNOWN",
            "32      INFO FROM BRWR",
            "33      RETURNED EMAIL",
            "41      BORROWER PHONE CALL",
            "42      2ND PRTY PHONE CAL",
            "43      3RD PARTY PHONE CAL",
            "44      PRISON",
            "45      FRIEND",
            "46      STUDENT",
            "47      PAROLE",
            "49      COUPON STATEMNENT",
            "50      FORMER EMPLOYER",
            "51      BAR ASSOCIATION",
            "52      VR MAILBX",
            "55      WEBMASTER",
            "56      GUARANTOR",
            "57      SCHOOL",
            "58      NSLC",
            "59      TELEPHONE COMPANY",
            "60      PERSON LOCATOR",
            "61      ACXIOM PH SCRUB",
            "62      EMAIL",
            "63      LENDER",
            "64      DUPLICATE PHONE",
            "97      CRC",
            "98      CAM",
            "99      ANNUAL STATEMENT"});
            this.SourceCode.Location = new System.Drawing.Point(606, 48);
            this.SourceCode.Name = "SourceCode";
            this.SourceCode.Size = new System.Drawing.Size(113, 28);
            this.SourceCode.TabIndex = 21;
            // 
            // LastVerifiedDate
            // 
            this.LastVerifiedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.LastVerifiedDate.Location = new System.Drawing.Point(392, 50);
            this.LastVerifiedDate.Mask = "00/00/0000";
            this.LastVerifiedDate.Name = "LastVerifiedDate";
            this.LastVerifiedDate.Size = new System.Drawing.Size(96, 26);
            this.LastVerifiedDate.TabIndex = 20;
            // 
            // Mbl
            // 
            this.Mbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mbl.FormattingEnabled = true;
            this.Mbl.Items.AddRange(new object[] {
            "U",
            "M",
            "L"});
            this.Mbl.Location = new System.Drawing.Point(59, 48);
            this.Mbl.Name = "Mbl";
            this.Mbl.Size = new System.Drawing.Size(43, 28);
            this.Mbl.TabIndex = 18;
            this.Mbl.SelectionChangeCommitted += new System.EventHandler(this.Mbl_SelectionChangeCommitted);
            // 
            // Phone
            // 
            this.Phone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Phone.Location = new System.Drawing.Point(143, 87);
            this.Phone.Mask = "(999) 000-0000";
            this.Phone.Name = "Phone";
            this.Phone.Size = new System.Drawing.Size(130, 26);
            this.Phone.TabIndex = 22;
            this.Phone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Consent
            // 
            this.Consent.AllowedSpecialCharacters = null;
            this.Consent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Consent.Location = new System.Drawing.Point(189, 50);
            this.Consent.MaxLength = 1;
            this.Consent.Name = "Consent";
            this.Consent.Size = new System.Drawing.Size(42, 26);
            this.Consent.TabIndex = 19;
            this.Consent.Text = "N";
            // 
            // PhoneExt
            // 
            this.PhoneExt.AllowedSpecialCharacters = null;
            this.PhoneExt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PhoneExt.Location = new System.Drawing.Point(368, 87);
            this.PhoneExt.MaxLength = 5;
            this.PhoneExt.Name = "PhoneExt";
            this.PhoneExt.Size = new System.Drawing.Size(70, 26);
            this.PhoneExt.TabIndex = 23;
            // 
            // ForeignPhoneExt
            // 
            this.ForeignPhoneExt.AllowedSpecialCharacters = null;
            this.ForeignPhoneExt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeignPhoneExt.Location = new System.Drawing.Point(428, 121);
            this.ForeignPhoneExt.MaxLength = 5;
            this.ForeignPhoneExt.Name = "ForeignPhoneExt";
            this.ForeignPhoneExt.Size = new System.Drawing.Size(70, 26);
            this.ForeignPhoneExt.TabIndex = 26;
            // 
            // ForeignPhone
            // 
            this.ForeignPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeignPhone.Location = new System.Drawing.Point(130, 121);
            this.ForeignPhone.Mask = "000-00000-00000000000";
            this.ForeignPhone.Name = "ForeignPhone";
            this.ForeignPhone.Size = new System.Drawing.Size(203, 26);
            this.ForeignPhone.TabIndex = 25;
            this.ForeignPhone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AddModifyReferencesDemos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(731, 536);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "AddModifyReferencesDemos";
            this.ShowIcon = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Uheaa.Common.WinForms.SsnTextBox Ssn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox Relationship;
        private System.Windows.Forms.CheckBox NoDemos;
        private System.Windows.Forms.CheckBox ForeignDemos;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.Label ForeignCodeLabel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox AddressType;
        private Uheaa.Common.WinForms.AlphaTextBox State;
        private Uheaa.Common.WinForms.NumericTextBox ZipCode;
        private System.Windows.Forms.ComboBox ForeignCode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private Uheaa.Common.WinForms.AlphaTextBox Consent;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private Uheaa.Common.WinForms.NumericMaskedTextBox Phone;
        private System.Windows.Forms.Label label16;
        private Uheaa.Common.WinForms.NumericTextBox PhoneExt;
        private System.Windows.Forms.CheckBox ValidPhone;
        private System.Windows.Forms.Label label17;
        private Uheaa.Common.WinForms.NumericMaskedTextBox ForeignPhone;
        private Uheaa.Common.WinForms.NumericTextBox ForeignPhoneExt;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox PhoneType;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private Uheaa.Common.WinForms.WatermarkTextBox FirstName;
        private Uheaa.Common.WinForms.WatermarkTextBox MiddleName;
        private Uheaa.Common.WinForms.WatermarkTextBox LastName;
        private Uheaa.Common.WinForms.AlphaNumericTextBox Address1;
        private Uheaa.Common.WinForms.AlphaNumericTextBox Address2;
        private Uheaa.Common.WinForms.AlphaNumericTextBox Address3;
        private Uheaa.Common.WinForms.AlphaNumericTextBox City;
        private System.Windows.Forms.TextBox EmailAddress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox Mbl;
        private Uheaa.Common.WinForms.MaskedDateTextBox LastVerifiedDate;
        private System.Windows.Forms.ComboBox SourceCode;
        private System.Windows.Forms.CheckBox checkBox1;



    }
}