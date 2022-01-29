namespace SCHDEMOUP
{
    partial class frmDemoData
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboState = new System.Windows.Forms.ComboBox();
            this.txtZip = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.txtForeignCountry = new System.Windows.Forms.TextBox();
            this.txtForeignState = new System.Windows.Forms.TextBox();
            this.txtAddress3 = new System.Windows.Forms.TextBox();
            this.txtAddress2 = new System.Windows.Forms.TextBox();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gbxPhone = new System.Windows.Forms.GroupBox();
            this.txtExtension = new System.Windows.Forms.TextBox();
            this.txtFax = new System.Windows.Forms.MaskedTextBox();
            this.txtPhone = new System.Windows.Forms.MaskedTextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSchoolName = new System.Windows.Forms.TextBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnComplete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbxForeign = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtFaxLocal = new System.Windows.Forms.TextBox();
            this.txtFaxCity = new System.Windows.Forms.TextBox();
            this.txtFaxCNY = new System.Windows.Forms.TextBox();
            this.txtFaxIC = new System.Windows.Forms.TextBox();
            this.txtPhoneLocal = new System.Windows.Forms.TextBox();
            this.txtPhoneCity = new System.Windows.Forms.TextBox();
            this.txtPhoneIC = new System.Windows.Forms.TextBox();
            this.txtPhoneCNY = new System.Windows.Forms.TextBox();
            this.txtForeignExt = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lPSCSchoolDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            this.gbxPhone.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbxForeign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lPSCSchoolDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboState);
            this.groupBox1.Controls.Add(this.txtZip);
            this.groupBox1.Controls.Add(this.txtCity);
            this.groupBox1.Controls.Add(this.txtForeignCountry);
            this.groupBox1.Controls.Add(this.txtForeignState);
            this.groupBox1.Controls.Add(this.txtAddress3);
            this.groupBox1.Controls.Add(this.txtAddress2);
            this.groupBox1.Controls.Add(this.txtAddress1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(413, 166);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Address";
            // 
            // cboState
            // 
            this.cboState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "State", true));
            this.cboState.FormattingEnabled = true;
            this.cboState.ItemHeight = 13;
            this.cboState.Location = new System.Drawing.Point(295, 88);
            this.cboState.Name = "cboState";
            this.cboState.Size = new System.Drawing.Size(43, 21);
            this.cboState.TabIndex = 18;
            // 
            // txtZip
            // 
            this.txtZip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "Zip", true));
            this.txtZip.Location = new System.Drawing.Point(341, 88);
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(60, 20);
            this.txtZip.TabIndex = 17;
            // 
            // txtCity
            // 
            this.txtCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "City", true));
            this.txtCity.Location = new System.Drawing.Point(104, 88);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(188, 20);
            this.txtCity.TabIndex = 15;
            // 
            // txtForeignCountry
            // 
            this.txtForeignCountry.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "Country", true));
            this.txtForeignCountry.Location = new System.Drawing.Point(104, 132);
            this.txtForeignCountry.Name = "txtForeignCountry";
            this.txtForeignCountry.Size = new System.Drawing.Size(297, 20);
            this.txtForeignCountry.TabIndex = 14;
            // 
            // txtForeignState
            // 
            this.txtForeignState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "ForeignState", true));
            this.txtForeignState.Location = new System.Drawing.Point(104, 110);
            this.txtForeignState.Name = "txtForeignState";
            this.txtForeignState.Size = new System.Drawing.Size(297, 20);
            this.txtForeignState.TabIndex = 13;
            // 
            // txtAddress3
            // 
            this.txtAddress3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "Address3", true));
            this.txtAddress3.Location = new System.Drawing.Point(104, 66);
            this.txtAddress3.Name = "txtAddress3";
            this.txtAddress3.Size = new System.Drawing.Size(297, 20);
            this.txtAddress3.TabIndex = 12;
            // 
            // txtAddress2
            // 
            this.txtAddress2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "Address2", true));
            this.txtAddress2.Location = new System.Drawing.Point(104, 44);
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(297, 20);
            this.txtAddress2.TabIndex = 11;
            // 
            // txtAddress1
            // 
            this.txtAddress1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "Address1", true));
            this.txtAddress1.Location = new System.Drawing.Point(104, 22);
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(297, 20);
            this.txtAddress1.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Foreign Country";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Foreign State";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "City, State, Zip";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Address 3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Address 2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Address 1";
            // 
            // gbxPhone
            // 
            this.gbxPhone.Controls.Add(this.txtExtension);
            this.gbxPhone.Controls.Add(this.txtFax);
            this.gbxPhone.Controls.Add(this.txtPhone);
            this.gbxPhone.Controls.Add(this.label15);
            this.gbxPhone.Controls.Add(this.label10);
            this.gbxPhone.Controls.Add(this.label8);
            this.gbxPhone.Location = new System.Drawing.Point(12, 214);
            this.gbxPhone.Name = "gbxPhone";
            this.gbxPhone.Size = new System.Drawing.Size(413, 85);
            this.gbxPhone.TabIndex = 1;
            this.gbxPhone.TabStop = false;
            this.gbxPhone.Text = "Phone/Fax";
            // 
            // txtExtension
            // 
            this.txtExtension.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "Extension", true));
            this.txtExtension.Location = new System.Drawing.Point(341, 27);
            this.txtExtension.Name = "txtExtension";
            this.txtExtension.Size = new System.Drawing.Size(60, 20);
            this.txtExtension.TabIndex = 1;
            // 
            // txtFax
            // 
            this.txtFax.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "Fax", true));
            this.txtFax.Location = new System.Drawing.Point(104, 56);
            this.txtFax.Mask = "(999) 000-0000";
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(112, 20);
            this.txtFax.TabIndex = 2;
            // 
            // txtPhone
            // 
            this.txtPhone.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "Phone", true));
            this.txtPhone.Location = new System.Drawing.Point(104, 27);
            this.txtPhone.Mask = "(999) 000-0000";
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(113, 20);
            this.txtPhone.TabIndex = 0;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(310, 30);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(25, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "Ext.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Fax";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Phone";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtLastName);
            this.groupBox3.Controls.Add(this.txtFirstName);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Location = new System.Drawing.Point(12, 404);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(413, 71);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Contact";
            // 
            // txtLastName
            // 
            this.txtLastName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "Last", true));
            this.txtLastName.Location = new System.Drawing.Point(104, 41);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(297, 20);
            this.txtLastName.TabIndex = 11;
            // 
            // txtFirstName
            // 
            this.txtFirstName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "First", true));
            this.txtFirstName.Location = new System.Drawing.Point(104, 19);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(297, 20);
            this.txtFirstName.TabIndex = 10;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(18, 44);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "Last Name";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "First Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "School Name";
            // 
            // txtSchoolName
            // 
            this.txtSchoolName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "SchoolName", true));
            this.txtSchoolName.Location = new System.Drawing.Point(116, 16);
            this.txtSchoolName.Name = "txtSchoolName";
            this.txtSchoolName.Size = new System.Drawing.Size(297, 20);
            this.txtSchoolName.TabIndex = 9;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(55, 494);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 12;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.Location = new System.Drawing.Point(187, 494);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(75, 23);
            this.btnComplete.TabIndex = 13;
            this.btnComplete.Text = "Complete";
            this.btnComplete.UseVisualStyleBackColor = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(310, 494);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbxForeign
            // 
            this.gbxForeign.Controls.Add(this.label19);
            this.gbxForeign.Controls.Add(this.label18);
            this.gbxForeign.Controls.Add(this.label17);
            this.gbxForeign.Controls.Add(this.label16);
            this.gbxForeign.Controls.Add(this.txtFaxLocal);
            this.gbxForeign.Controls.Add(this.txtFaxCity);
            this.gbxForeign.Controls.Add(this.txtFaxCNY);
            this.gbxForeign.Controls.Add(this.txtFaxIC);
            this.gbxForeign.Controls.Add(this.txtPhoneLocal);
            this.gbxForeign.Controls.Add(this.txtPhoneCity);
            this.gbxForeign.Controls.Add(this.txtPhoneIC);
            this.gbxForeign.Controls.Add(this.txtPhoneCNY);
            this.gbxForeign.Controls.Add(this.txtForeignExt);
            this.gbxForeign.Controls.Add(this.label14);
            this.gbxForeign.Controls.Add(this.label11);
            this.gbxForeign.Controls.Add(this.label9);
            this.gbxForeign.Location = new System.Drawing.Point(12, 305);
            this.gbxForeign.Name = "gbxForeign";
            this.gbxForeign.Size = new System.Drawing.Size(413, 93);
            this.gbxForeign.TabIndex = 15;
            this.gbxForeign.TabStop = false;
            this.gbxForeign.Text = "Foreign Phone/Fax";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(235, 16);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 13);
            this.label19.TabIndex = 43;
            this.label19.Text = "LOCAL";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(176, 16);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(31, 13);
            this.label18.TabIndex = 42;
            this.label18.Text = "CITY";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(139, 16);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(29, 13);
            this.label17.TabIndex = 41;
            this.label17.Text = "CNY";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(109, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 13);
            this.label16.TabIndex = 40;
            this.label16.Text = "IC";
            // 
            // txtFaxLocal
            // 
            this.txtFaxLocal.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "FaxLocal", true));
            this.txtFaxLocal.Location = new System.Drawing.Point(215, 61);
            this.txtFaxLocal.Name = "txtFaxLocal";
            this.txtFaxLocal.Size = new System.Drawing.Size(85, 20);
            this.txtFaxLocal.TabIndex = 39;
            // 
            // txtFaxCity
            // 
            this.txtFaxCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "FaxCity", true));
            this.txtFaxCity.Location = new System.Drawing.Point(169, 61);
            this.txtFaxCity.Name = "txtFaxCity";
            this.txtFaxCity.Size = new System.Drawing.Size(45, 20);
            this.txtFaxCity.TabIndex = 38;
            // 
            // txtFaxCNY
            // 
            this.txtFaxCNY.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "FaxCNY", true));
            this.txtFaxCNY.Location = new System.Drawing.Point(138, 61);
            this.txtFaxCNY.Name = "txtFaxCNY";
            this.txtFaxCNY.Size = new System.Drawing.Size(30, 20);
            this.txtFaxCNY.TabIndex = 37;
            // 
            // txtFaxIC
            // 
            this.txtFaxIC.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "FaxIC", true));
            this.txtFaxIC.Location = new System.Drawing.Point(101, 61);
            this.txtFaxIC.Name = "txtFaxIC";
            this.txtFaxIC.Size = new System.Drawing.Size(36, 20);
            this.txtFaxIC.TabIndex = 36;
            // 
            // txtPhoneLocal
            // 
            this.txtPhoneLocal.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "PhoneLocal", true));
            this.txtPhoneLocal.Location = new System.Drawing.Point(215, 32);
            this.txtPhoneLocal.Name = "txtPhoneLocal";
            this.txtPhoneLocal.Size = new System.Drawing.Size(85, 20);
            this.txtPhoneLocal.TabIndex = 34;
            // 
            // txtPhoneCity
            // 
            this.txtPhoneCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "PhoneCity", true));
            this.txtPhoneCity.Location = new System.Drawing.Point(169, 32);
            this.txtPhoneCity.Name = "txtPhoneCity";
            this.txtPhoneCity.Size = new System.Drawing.Size(45, 20);
            this.txtPhoneCity.TabIndex = 32;
            // 
            // txtPhoneIC
            // 
            this.txtPhoneIC.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "PhoneIC", true));
            this.txtPhoneIC.Location = new System.Drawing.Point(102, 32);
            this.txtPhoneIC.Name = "txtPhoneIC";
            this.txtPhoneIC.Size = new System.Drawing.Size(35, 20);
            this.txtPhoneIC.TabIndex = 30;
            // 
            // txtPhoneCNY
            // 
            this.txtPhoneCNY.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "PhoneCNY", true));
            this.txtPhoneCNY.Location = new System.Drawing.Point(138, 32);
            this.txtPhoneCNY.Name = "txtPhoneCNY";
            this.txtPhoneCNY.Size = new System.Drawing.Size(30, 20);
            this.txtPhoneCNY.TabIndex = 31;
            // 
            // txtForeignExt
            // 
            this.txtForeignExt.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lPSCSchoolDataBindingSource, "ForeignExtension", true));
            this.txtForeignExt.Location = new System.Drawing.Point(338, 32);
            this.txtForeignExt.Name = "txtForeignExt";
            this.txtForeignExt.Size = new System.Drawing.Size(60, 20);
            this.txtForeignExt.TabIndex = 28;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(307, 35);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(25, 13);
            this.label14.TabIndex = 35;
            this.label14.Text = "Ext.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 13);
            this.label11.TabIndex = 33;
            this.label11.Text = "Foreign Phone";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "Foreign Fax";
            // 
            // lPSCSchoolDataBindingSource
            // 
            this.lPSCSchoolDataBindingSource.DataSource = typeof(SCHDEMOUP.LPSCSchoolData);
            // 
            // frmDemoData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 532);
            this.Controls.Add(this.gbxForeign);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.txtSchoolName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbxPhone);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmDemoData";
            this.Text = "School Demographics Update Data Input";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbxPhone.ResumeLayout(false);
            this.gbxPhone.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gbxForeign.ResumeLayout(false);
            this.gbxForeign.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lPSCSchoolDataBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbxPhone;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtZip;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.TextBox txtForeignCountry;
        private System.Windows.Forms.TextBox txtForeignState;
        private System.Windows.Forms.TextBox txtAddress3;
        private System.Windows.Forms.TextBox txtAddress2;
        private System.Windows.Forms.TextBox txtAddress1;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtSchoolName;
        private System.Windows.Forms.TextBox txtExtension;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.MaskedTextBox txtFax;
        private System.Windows.Forms.MaskedTextBox txtPhone;
        private System.Windows.Forms.ComboBox cboState;
        private System.Windows.Forms.GroupBox gbxForeign;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtFaxLocal;
        private System.Windows.Forms.TextBox txtFaxCity;
        private System.Windows.Forms.TextBox txtFaxCNY;
        private System.Windows.Forms.TextBox txtFaxIC;
        private System.Windows.Forms.TextBox txtPhoneLocal;
        private System.Windows.Forms.TextBox txtPhoneCity;
        private System.Windows.Forms.TextBox txtPhoneIC;
        private System.Windows.Forms.TextBox txtPhoneCNY;
        private System.Windows.Forms.TextBox txtForeignExt;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.BindingSource lPSCSchoolDataBindingSource;
    }
}