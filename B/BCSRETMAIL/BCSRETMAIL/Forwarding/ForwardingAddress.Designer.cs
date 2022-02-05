namespace BCSRETMAIL
{
    partial class ForwardingAddress
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
            this.ForwardClear = new System.Windows.Forms.Button();
            this.Zip = new Uheaa.Common.WinForms.NumericTextBox();
            this.City = new Uheaa.Common.WinForms.AlphaTextBox();
            this.Address2 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.Address1 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.BorrowerName = new System.Windows.Forms.Label();
            this.AccountNumber = new System.Windows.Forms.Label();
            this.Rules = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Continue = new System.Windows.Forms.Button();
            this.State = new System.Windows.Forms.ComboBox();
            this.lblZip = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.lblAddress1 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblAccountNum = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.OnelinkAddress1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.OnelinkAddress2 = new System.Windows.Forms.Label();
            this.OnelinkCityStZip = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.OnelinkAddressValid = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OnelinkAddressDate = new System.Windows.Forms.Label();
            this.OnelinkForward = new System.Windows.Forms.Button();
            this.OnelinkDemos = new System.Windows.Forms.GroupBox();
            this.OnelinkInvalidate = new System.Windows.Forms.Button();
            this.lblCornAddrEffDate = new System.Windows.Forms.Label();
            this.CompassAddress1 = new System.Windows.Forms.Label();
            this.lblAddrValid = new System.Windows.Forms.Label();
            this.CompassAddress2 = new System.Windows.Forms.Label();
            this.CompassCityStZip = new System.Windows.Forms.Label();
            this.lblCornCity = new System.Windows.Forms.Label();
            this.lblCornAddr2 = new System.Windows.Forms.Label();
            this.CompassAddressValid = new System.Windows.Forms.Label();
            this.lblCornAddr1 = new System.Windows.Forms.Label();
            this.CompassAddressDate = new System.Windows.Forms.Label();
            this.CompassForward = new System.Windows.Forms.Button();
            this.CompassDemos = new System.Windows.Forms.GroupBox();
            this.CompassInvalidate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.InvalidAddress1 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.InvalidateClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.InvalidState = new System.Windows.Forms.ComboBox();
            this.InvalidZip = new Uheaa.Common.WinForms.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.InvalidCity = new Uheaa.Common.WinForms.AlphaTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.InvalidAddress2 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.OnelinkDemos.SuspendLayout();
            this.CompassDemos.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ForwardClear
            // 
            this.ForwardClear.Location = new System.Drawing.Point(425, 244);
            this.ForwardClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ForwardClear.Name = "ForwardClear";
            this.ForwardClear.Size = new System.Drawing.Size(112, 35);
            this.ForwardClear.TabIndex = 5;
            this.ForwardClear.Text = "Clear";
            this.ForwardClear.UseVisualStyleBackColor = true;
            this.ForwardClear.Click += new System.EventHandler(this.ForwardClear_Click);
            // 
            // Zip
            // 
            this.Zip.AllowedSpecialCharacters = "";
            this.Zip.Location = new System.Drawing.Point(137, 241);
            this.Zip.MaxLength = 9;
            this.Zip.Name = "Zip";
            this.Zip.Size = new System.Drawing.Size(137, 26);
            this.Zip.TabIndex = 4;
            this.Zip.TextChanged += new System.EventHandler(this.Zip_TextChanged);
            // 
            // City
            // 
            this.City.AllowedSpecialCharacters = " ";
            this.City.Location = new System.Drawing.Point(137, 166);
            this.City.MaxLength = 20;
            this.City.Name = "City";
            this.City.Size = new System.Drawing.Size(235, 26);
            this.City.TabIndex = 2;
            this.City.TextChanged += new System.EventHandler(this.City_TextChanged);
            // 
            // Address2
            // 
            this.Address2.AllowedSpecialCharacters = " ";
            this.Address2.Location = new System.Drawing.Point(137, 134);
            this.Address2.MaxLength = 30;
            this.Address2.Name = "Address2";
            this.Address2.Size = new System.Drawing.Size(373, 26);
            this.Address2.TabIndex = 1;
            this.Address2.TextChanged += new System.EventHandler(this.Address2_TextChanged);
            // 
            // Address1
            // 
            this.Address1.AllowedSpecialCharacters = " ";
            this.Address1.Location = new System.Drawing.Point(137, 100);
            this.Address1.MaxLength = 30;
            this.Address1.Name = "Address1";
            this.Address1.Size = new System.Drawing.Size(373, 26);
            this.Address1.TabIndex = 0;
            this.Address1.TextChanged += new System.EventHandler(this.Address1_TextChanged);
            // 
            // BorrowerName
            // 
            this.BorrowerName.AutoSize = true;
            this.BorrowerName.Location = new System.Drawing.Point(133, 59);
            this.BorrowerName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BorrowerName.Name = "BorrowerName";
            this.BorrowerName.Size = new System.Drawing.Size(51, 20);
            this.BorrowerName.TabIndex = 20;
            this.BorrowerName.Text = "label3";
            // 
            // AccountNumber
            // 
            this.AccountNumber.AutoSize = true;
            this.AccountNumber.Location = new System.Drawing.Point(133, 28);
            this.AccountNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AccountNumber.Name = "AccountNumber";
            this.AccountNumber.Size = new System.Drawing.Size(51, 20);
            this.AccountNumber.TabIndex = 19;
            this.AccountNumber.Text = "label2";
            // 
            // Rules
            // 
            this.Rules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Rules.Location = new System.Drawing.Point(721, 566);
            this.Rules.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Rules.Name = "Rules";
            this.Rules.Size = new System.Drawing.Size(112, 35);
            this.Rules.TabIndex = 2;
            this.Rules.Text = "Rules";
            this.Rules.UseVisualStyleBackColor = true;
            this.Rules.Click += new System.EventHandler(this.Rules_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(893, 566);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(112, 35);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Continue
            // 
            this.Continue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Continue.Location = new System.Drawing.Point(1061, 566);
            this.Continue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(112, 35);
            this.Continue.TabIndex = 0;
            this.Continue.Text = "Continue";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.BtnContinue_Click);
            // 
            // State
            // 
            this.State.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.State.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.State.FormattingEnabled = true;
            this.State.Location = new System.Drawing.Point(137, 204);
            this.State.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.State.Name = "State";
            this.State.Size = new System.Drawing.Size(180, 28);
            this.State.TabIndex = 3;
            this.State.SelectedIndexChanged += new System.EventHandler(this.State_SelectedIndexChanged);
            // 
            // lblZip
            // 
            this.lblZip.AutoSize = true;
            this.lblZip.Location = new System.Drawing.Point(66, 244);
            this.lblZip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(31, 20);
            this.lblZip.TabIndex = 8;
            this.lblZip.Text = "Zip";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(49, 207);
            this.lblState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(48, 20);
            this.lblState.TabIndex = 7;
            this.lblState.Text = "State";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(62, 172);
            this.lblCity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(35, 20);
            this.lblCity.TabIndex = 6;
            this.lblCity.Text = "City";
            // 
            // lblAddress2
            // 
            this.lblAddress2.AutoSize = true;
            this.lblAddress2.Location = new System.Drawing.Point(16, 137);
            this.lblAddress2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(81, 20);
            this.lblAddress2.TabIndex = 5;
            this.lblAddress2.Text = "Address 2";
            // 
            // lblAddress1
            // 
            this.lblAddress1.AutoSize = true;
            this.lblAddress1.Location = new System.Drawing.Point(12, 102);
            this.lblAddress1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(85, 20);
            this.lblAddress1.TabIndex = 4;
            this.lblAddress1.Text = "Address  1";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(46, 59);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(51, 20);
            this.lblName.TabIndex = 22;
            this.lblName.Text = "Name";
            // 
            // lblAccountNum
            // 
            this.lblAccountNum.AutoSize = true;
            this.lblAccountNum.Location = new System.Drawing.Point(16, 28);
            this.lblAccountNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccountNum.Name = "lblAccountNum";
            this.lblAccountNum.Size = new System.Drawing.Size(81, 20);
            this.lblAccountNum.TabIndex = 21;
            this.lblAccountNum.Text = "Account #";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 148);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(173, 20);
            this.label10.TabIndex = 6;
            this.label10.Text = "Address Effective Date";
            // 
            // OnelinkAddress1
            // 
            this.OnelinkAddress1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OnelinkAddress1.AutoSize = true;
            this.OnelinkAddress1.Location = new System.Drawing.Point(194, 20);
            this.OnelinkAddress1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OnelinkAddress1.Name = "OnelinkAddress1";
            this.OnelinkAddress1.Size = new System.Drawing.Size(51, 20);
            this.OnelinkAddress1.TabIndex = 7;
            this.OnelinkAddress1.Text = "label2";
            this.OnelinkAddress1.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(59, 116);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 20);
            this.label8.TabIndex = 5;
            this.label8.Text = "Address Validity";
            // 
            // OnelinkAddress2
            // 
            this.OnelinkAddress2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OnelinkAddress2.AutoSize = true;
            this.OnelinkAddress2.Location = new System.Drawing.Point(194, 52);
            this.OnelinkAddress2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OnelinkAddress2.Name = "OnelinkAddress2";
            this.OnelinkAddress2.Size = new System.Drawing.Size(51, 20);
            this.OnelinkAddress2.TabIndex = 8;
            this.OnelinkAddress2.Text = "label4";
            this.OnelinkAddress2.Visible = false;
            // 
            // OnelinkCityStZip
            // 
            this.OnelinkCityStZip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OnelinkCityStZip.AutoSize = true;
            this.OnelinkCityStZip.Location = new System.Drawing.Point(194, 84);
            this.OnelinkCityStZip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OnelinkCityStZip.Name = "OnelinkCityStZip";
            this.OnelinkCityStZip.Size = new System.Drawing.Size(51, 20);
            this.OnelinkCityStZip.TabIndex = 9;
            this.OnelinkCityStZip.Text = "label5";
            this.OnelinkCityStZip.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(73, 84);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "City, State Zip";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(104, 52);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Address2";
            // 
            // OnelinkAddressValid
            // 
            this.OnelinkAddressValid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OnelinkAddressValid.AutoSize = true;
            this.OnelinkAddressValid.Location = new System.Drawing.Point(194, 116);
            this.OnelinkAddressValid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OnelinkAddressValid.Name = "OnelinkAddressValid";
            this.OnelinkAddressValid.Size = new System.Drawing.Size(51, 20);
            this.OnelinkAddressValid.TabIndex = 12;
            this.OnelinkAddressValid.Text = "label8";
            this.OnelinkAddressValid.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Address1";
            // 
            // OnelinkAddressDate
            // 
            this.OnelinkAddressDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OnelinkAddressDate.AutoSize = true;
            this.OnelinkAddressDate.Location = new System.Drawing.Point(194, 148);
            this.OnelinkAddressDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OnelinkAddressDate.Name = "OnelinkAddressDate";
            this.OnelinkAddressDate.Size = new System.Drawing.Size(51, 20);
            this.OnelinkAddressDate.TabIndex = 13;
            this.OnelinkAddressDate.Text = "label9";
            this.OnelinkAddressDate.Visible = false;
            // 
            // OnelinkForward
            // 
            this.OnelinkForward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OnelinkForward.Location = new System.Drawing.Point(345, 201);
            this.OnelinkForward.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OnelinkForward.Name = "OnelinkForward";
            this.OnelinkForward.Size = new System.Drawing.Size(197, 30);
            this.OnelinkForward.TabIndex = 1;
            this.OnelinkForward.Text = "Onelink Forwarding";
            this.OnelinkForward.UseVisualStyleBackColor = true;
            this.OnelinkForward.Click += new System.EventHandler(this.OnelinkForward_Click);
            // 
            // OnelinkDemos
            // 
            this.OnelinkDemos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OnelinkDemos.Controls.Add(this.OnelinkInvalidate);
            this.OnelinkDemos.Controls.Add(this.OnelinkForward);
            this.OnelinkDemos.Controls.Add(this.OnelinkAddressDate);
            this.OnelinkDemos.Controls.Add(this.label2);
            this.OnelinkDemos.Controls.Add(this.OnelinkAddressValid);
            this.OnelinkDemos.Controls.Add(this.label4);
            this.OnelinkDemos.Controls.Add(this.label5);
            this.OnelinkDemos.Controls.Add(this.OnelinkCityStZip);
            this.OnelinkDemos.Controls.Add(this.OnelinkAddress2);
            this.OnelinkDemos.Controls.Add(this.label8);
            this.OnelinkDemos.Controls.Add(this.OnelinkAddress1);
            this.OnelinkDemos.Controls.Add(this.label10);
            this.OnelinkDemos.Enabled = false;
            this.OnelinkDemos.Location = new System.Drawing.Point(624, 302);
            this.OnelinkDemos.Name = "OnelinkDemos";
            this.OnelinkDemos.Size = new System.Drawing.Size(549, 239);
            this.OnelinkDemos.TabIndex = 14;
            this.OnelinkDemos.TabStop = false;
            this.OnelinkDemos.Text = "Onelink";
            // 
            // OnelinkInvalidate
            // 
            this.OnelinkInvalidate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OnelinkInvalidate.Location = new System.Drawing.Point(12, 201);
            this.OnelinkInvalidate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OnelinkInvalidate.Name = "OnelinkInvalidate";
            this.OnelinkInvalidate.Size = new System.Drawing.Size(197, 30);
            this.OnelinkInvalidate.TabIndex = 0;
            this.OnelinkInvalidate.Text = "Onelink Invalidate";
            this.OnelinkInvalidate.UseVisualStyleBackColor = true;
            this.OnelinkInvalidate.Click += new System.EventHandler(this.OnelinkInvalidate_Click);
            // 
            // lblCornAddrEffDate
            // 
            this.lblCornAddrEffDate.AutoSize = true;
            this.lblCornAddrEffDate.Location = new System.Drawing.Point(8, 148);
            this.lblCornAddrEffDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornAddrEffDate.Name = "lblCornAddrEffDate";
            this.lblCornAddrEffDate.Size = new System.Drawing.Size(173, 20);
            this.lblCornAddrEffDate.TabIndex = 6;
            this.lblCornAddrEffDate.Text = "Address Effective Date";
            // 
            // CompassAddress1
            // 
            this.CompassAddress1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CompassAddress1.AutoSize = true;
            this.CompassAddress1.Location = new System.Drawing.Point(194, 20);
            this.CompassAddress1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CompassAddress1.Name = "CompassAddress1";
            this.CompassAddress1.Size = new System.Drawing.Size(51, 20);
            this.CompassAddress1.TabIndex = 7;
            this.CompassAddress1.Text = "label2";
            this.CompassAddress1.Visible = false;
            // 
            // lblAddrValid
            // 
            this.lblAddrValid.AutoSize = true;
            this.lblAddrValid.Location = new System.Drawing.Point(59, 116);
            this.lblAddrValid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddrValid.Name = "lblAddrValid";
            this.lblAddrValid.Size = new System.Drawing.Size(122, 20);
            this.lblAddrValid.TabIndex = 5;
            this.lblAddrValid.Text = "Address Validity";
            // 
            // CompassAddress2
            // 
            this.CompassAddress2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CompassAddress2.AutoSize = true;
            this.CompassAddress2.Location = new System.Drawing.Point(194, 52);
            this.CompassAddress2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CompassAddress2.Name = "CompassAddress2";
            this.CompassAddress2.Size = new System.Drawing.Size(51, 20);
            this.CompassAddress2.TabIndex = 8;
            this.CompassAddress2.Text = "label4";
            this.CompassAddress2.Visible = false;
            // 
            // CompassCityStZip
            // 
            this.CompassCityStZip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CompassCityStZip.AutoSize = true;
            this.CompassCityStZip.Location = new System.Drawing.Point(194, 84);
            this.CompassCityStZip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CompassCityStZip.Name = "CompassCityStZip";
            this.CompassCityStZip.Size = new System.Drawing.Size(51, 20);
            this.CompassCityStZip.TabIndex = 9;
            this.CompassCityStZip.Text = "label5";
            this.CompassCityStZip.Visible = false;
            // 
            // lblCornCity
            // 
            this.lblCornCity.AutoSize = true;
            this.lblCornCity.Location = new System.Drawing.Point(73, 84);
            this.lblCornCity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornCity.Name = "lblCornCity";
            this.lblCornCity.Size = new System.Drawing.Size(108, 20);
            this.lblCornCity.TabIndex = 2;
            this.lblCornCity.Text = "City, State Zip";
            // 
            // lblCornAddr2
            // 
            this.lblCornAddr2.AutoSize = true;
            this.lblCornAddr2.Location = new System.Drawing.Point(104, 52);
            this.lblCornAddr2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornAddr2.Name = "lblCornAddr2";
            this.lblCornAddr2.Size = new System.Drawing.Size(77, 20);
            this.lblCornAddr2.TabIndex = 1;
            this.lblCornAddr2.Text = "Address2";
            // 
            // CompassAddressValid
            // 
            this.CompassAddressValid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CompassAddressValid.AutoSize = true;
            this.CompassAddressValid.Location = new System.Drawing.Point(194, 116);
            this.CompassAddressValid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CompassAddressValid.Name = "CompassAddressValid";
            this.CompassAddressValid.Size = new System.Drawing.Size(51, 20);
            this.CompassAddressValid.TabIndex = 12;
            this.CompassAddressValid.Text = "label8";
            this.CompassAddressValid.Visible = false;
            // 
            // lblCornAddr1
            // 
            this.lblCornAddr1.AutoSize = true;
            this.lblCornAddr1.Location = new System.Drawing.Point(104, 20);
            this.lblCornAddr1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornAddr1.Name = "lblCornAddr1";
            this.lblCornAddr1.Size = new System.Drawing.Size(77, 20);
            this.lblCornAddr1.TabIndex = 0;
            this.lblCornAddr1.Text = "Address1";
            // 
            // CompassAddressDate
            // 
            this.CompassAddressDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CompassAddressDate.AutoSize = true;
            this.CompassAddressDate.Location = new System.Drawing.Point(194, 148);
            this.CompassAddressDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CompassAddressDate.Name = "CompassAddressDate";
            this.CompassAddressDate.Size = new System.Drawing.Size(51, 20);
            this.CompassAddressDate.TabIndex = 13;
            this.CompassAddressDate.Text = "label9";
            this.CompassAddressDate.Visible = false;
            // 
            // CompassForward
            // 
            this.CompassForward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CompassForward.Location = new System.Drawing.Point(345, 195);
            this.CompassForward.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CompassForward.Name = "CompassForward";
            this.CompassForward.Size = new System.Drawing.Size(197, 30);
            this.CompassForward.TabIndex = 1;
            this.CompassForward.Text = "Compass Forwarding";
            this.CompassForward.UseVisualStyleBackColor = true;
            this.CompassForward.Click += new System.EventHandler(this.CompassForward_Click);
            // 
            // CompassDemos
            // 
            this.CompassDemos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CompassDemos.Controls.Add(this.CompassInvalidate);
            this.CompassDemos.Controls.Add(this.CompassForward);
            this.CompassDemos.Controls.Add(this.CompassAddressDate);
            this.CompassDemos.Controls.Add(this.lblCornAddr1);
            this.CompassDemos.Controls.Add(this.CompassAddressValid);
            this.CompassDemos.Controls.Add(this.lblCornAddr2);
            this.CompassDemos.Controls.Add(this.lblCornCity);
            this.CompassDemos.Controls.Add(this.CompassCityStZip);
            this.CompassDemos.Controls.Add(this.CompassAddress2);
            this.CompassDemos.Controls.Add(this.lblAddrValid);
            this.CompassDemos.Controls.Add(this.CompassAddress1);
            this.CompassDemos.Controls.Add(this.lblCornAddrEffDate);
            this.CompassDemos.Enabled = false;
            this.CompassDemos.Location = new System.Drawing.Point(624, 38);
            this.CompassDemos.Name = "CompassDemos";
            this.CompassDemos.Size = new System.Drawing.Size(549, 233);
            this.CompassDemos.TabIndex = 3;
            this.CompassDemos.TabStop = false;
            this.CompassDemos.Text = "Compass";
            // 
            // CompassInvalidate
            // 
            this.CompassInvalidate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CompassInvalidate.Location = new System.Drawing.Point(12, 195);
            this.CompassInvalidate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CompassInvalidate.Name = "CompassInvalidate";
            this.CompassInvalidate.Size = new System.Drawing.Size(197, 30);
            this.CompassInvalidate.TabIndex = 0;
            this.CompassInvalidate.Text = "Compass Invalidate";
            this.CompassInvalidate.UseVisualStyleBackColor = true;
            this.CompassInvalidate.Click += new System.EventHandler(this.CompassInvalidate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Address1);
            this.groupBox1.Controls.Add(this.ForwardClear);
            this.groupBox1.Controls.Add(this.lblZip);
            this.groupBox1.Controls.Add(this.State);
            this.groupBox1.Controls.Add(this.Zip);
            this.groupBox1.Controls.Add(this.lblState);
            this.groupBox1.Controls.Add(this.City);
            this.groupBox1.Controls.Add(this.lblCity);
            this.groupBox1.Controls.Add(this.Address2);
            this.groupBox1.Controls.Add(this.lblAddress2);
            this.groupBox1.Controls.Add(this.lblAddress1);
            this.groupBox1.Controls.Add(this.lblAccountNum);
            this.groupBox1.Controls.Add(this.AccountNumber);
            this.groupBox1.Controls.Add(this.BorrowerName);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(563, 291);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Forwarding Address";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.InvalidAddress1);
            this.groupBox2.Controls.Add(this.InvalidateClear);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.InvalidState);
            this.groupBox2.Controls.Add(this.InvalidZip);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.InvalidCity);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.InvalidAddress2);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(12, 354);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(563, 245);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Invalidate Address";
            // 
            // InvalidAddress1
            // 
            this.InvalidAddress1.AllowedSpecialCharacters = " ";
            this.InvalidAddress1.Location = new System.Drawing.Point(137, 47);
            this.InvalidAddress1.MaxLength = 30;
            this.InvalidAddress1.Name = "InvalidAddress1";
            this.InvalidAddress1.Size = new System.Drawing.Size(373, 26);
            this.InvalidAddress1.TabIndex = 0;
            this.InvalidAddress1.TextChanged += new System.EventHandler(this.InvalidAddress1_TextChanged);
            // 
            // InvalidateClear
            // 
            this.InvalidateClear.Location = new System.Drawing.Point(425, 191);
            this.InvalidateClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InvalidateClear.Name = "InvalidateClear";
            this.InvalidateClear.Size = new System.Drawing.Size(112, 35);
            this.InvalidateClear.TabIndex = 23;
            this.InvalidateClear.Text = "Clear";
            this.InvalidateClear.UseVisualStyleBackColor = true;
            this.InvalidateClear.Click += new System.EventHandler(this.InvalidateClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 191);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Zip";
            // 
            // InvalidState
            // 
            this.InvalidState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.InvalidState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.InvalidState.FormattingEnabled = true;
            this.InvalidState.Location = new System.Drawing.Point(137, 151);
            this.InvalidState.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InvalidState.Name = "InvalidState";
            this.InvalidState.Size = new System.Drawing.Size(180, 28);
            this.InvalidState.TabIndex = 3;
            this.InvalidState.SelectedIndexChanged += new System.EventHandler(this.InvalidState_SelectedIndexChanged);
            // 
            // InvalidZip
            // 
            this.InvalidZip.AllowedSpecialCharacters = "";
            this.InvalidZip.Location = new System.Drawing.Point(137, 188);
            this.InvalidZip.MaxLength = 9;
            this.InvalidZip.Name = "InvalidZip";
            this.InvalidZip.Size = new System.Drawing.Size(137, 26);
            this.InvalidZip.TabIndex = 4;
            this.InvalidZip.TextChanged += new System.EventHandler(this.InvalidZip_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 154);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "State";
            // 
            // InvalidCity
            // 
            this.InvalidCity.AllowedSpecialCharacters = " ";
            this.InvalidCity.Location = new System.Drawing.Point(137, 113);
            this.InvalidCity.MaxLength = 20;
            this.InvalidCity.Name = "InvalidCity";
            this.InvalidCity.Size = new System.Drawing.Size(235, 26);
            this.InvalidCity.TabIndex = 2;
            this.InvalidCity.TextChanged += new System.EventHandler(this.InvalidCity_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(62, 119);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 20);
            this.label6.TabIndex = 6;
            this.label6.Text = "City";
            // 
            // InvalidAddress2
            // 
            this.InvalidAddress2.AllowedSpecialCharacters = " ";
            this.InvalidAddress2.Location = new System.Drawing.Point(137, 81);
            this.InvalidAddress2.MaxLength = 30;
            this.InvalidAddress2.Name = "InvalidAddress2";
            this.InvalidAddress2.Size = new System.Drawing.Size(373, 26);
            this.InvalidAddress2.TabIndex = 1;
            this.InvalidAddress2.TextChanged += new System.EventHandler(this.InvalidAddress2_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 84);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 20);
            this.label7.TabIndex = 5;
            this.label7.Text = "Address 2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 49);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 20);
            this.label9.TabIndex = 4;
            this.label9.Text = "Address  1";
            // 
            // ForwardingAddress
            // 
            this.AcceptButton = this.Continue;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(1191, 615);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OnelinkDemos);
            this.Controls.Add(this.CompassDemos);
            this.Controls.Add(this.Rules);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.Cancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(1207, 654);
            this.Name = "ForwardingAddress";
            this.Text = "Forwarding Address";
            this.OnelinkDemos.ResumeLayout(false);
            this.OnelinkDemos.PerformLayout();
            this.CompassDemos.ResumeLayout(false);
            this.CompassDemos.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblAccountNum;
        private System.Windows.Forms.Label lblZip;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblAddress2;
        private System.Windows.Forms.Label lblAddress1;
        private System.Windows.Forms.Label BorrowerName;
        private System.Windows.Forms.Label AccountNumber;
        private System.Windows.Forms.Button Rules;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Continue;
        private System.Windows.Forms.ComboBox State;
        private Uheaa.Common.WinForms.AlphaNumericTextBox Address2;
        private Uheaa.Common.WinForms.AlphaNumericTextBox Address1;
        private Uheaa.Common.WinForms.NumericTextBox Zip;
        private Uheaa.Common.WinForms.AlphaTextBox City;
        private System.Windows.Forms.Button ForwardClear;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label OnelinkAddress1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label OnelinkAddress2;
        private System.Windows.Forms.Label OnelinkCityStZip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label OnelinkAddressValid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label OnelinkAddressDate;
        private System.Windows.Forms.Button OnelinkForward;
        private System.Windows.Forms.GroupBox OnelinkDemos;
        private System.Windows.Forms.Label lblCornAddrEffDate;
        private System.Windows.Forms.Label CompassAddress1;
        private System.Windows.Forms.Label lblAddrValid;
        private System.Windows.Forms.Label CompassAddress2;
        private System.Windows.Forms.Label CompassCityStZip;
        private System.Windows.Forms.Label lblCornCity;
        private System.Windows.Forms.Label lblCornAddr2;
        private System.Windows.Forms.Label CompassAddressValid;
        private System.Windows.Forms.Label lblCornAddr1;
        private System.Windows.Forms.Label CompassAddressDate;
        private System.Windows.Forms.Button CompassForward;
        private System.Windows.Forms.GroupBox CompassDemos;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button OnelinkInvalidate;
        private System.Windows.Forms.Button CompassInvalidate;
        private System.Windows.Forms.GroupBox groupBox2;
        private Uheaa.Common.WinForms.AlphaNumericTextBox InvalidAddress1;
        private System.Windows.Forms.Button InvalidateClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox InvalidState;
        private Uheaa.Common.WinForms.NumericTextBox InvalidZip;
        private System.Windows.Forms.Label label3;
        private Uheaa.Common.WinForms.AlphaTextBox InvalidCity;
        private System.Windows.Forms.Label label6;
        private Uheaa.Common.WinForms.AlphaNumericTextBox InvalidAddress2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
    }
}