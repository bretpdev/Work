namespace BARCODEFED
{
	partial class DemographicUpdateScreen
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtZip = new Uheaa.Common.WinForms.NumericTextBox();
            this.forwardingInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtCity = new Uheaa.Common.WinForms.AlphaTextBox();
            this.txtAddress2 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.txtAddress1 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.noBarcodeDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblActAcctNum = new System.Windows.Forms.Label();
            this.lblSSN1 = new System.Windows.Forms.Label();
            this.RulesBtn = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.cbState = new System.Windows.Forms.ComboBox();
            this.lblZip = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.lblAddress1 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblAccountNum = new System.Windows.Forms.Label();
            this.lblSsn = new System.Windows.Forms.Label();
            this.lblText = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCornerStoneAddr = new System.Windows.Forms.Button();
            this.lblCornActAdrEffDate = new System.Windows.Forms.Label();
            this.borrowerDemosBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblCornAdrValid = new System.Windows.Forms.Label();
            this.lblCornActZip = new System.Windows.Forms.Label();
            this.lblCornActState = new System.Windows.Forms.Label();
            this.lblCornActCity = new System.Windows.Forms.Label();
            this.lblCornActAddr2 = new System.Windows.Forms.Label();
            this.lblCornActAdr1 = new System.Windows.Forms.Label();
            this.lblCornAddrEffDate = new System.Windows.Forms.Label();
            this.lblAddrValid = new System.Windows.Forms.Label();
            this.lblCornZip = new System.Windows.Forms.Label();
            this.lblCornSt = new System.Windows.Forms.Label();
            this.lblCornCity = new System.Windows.Forms.Label();
            this.lblCornAddr2 = new System.Windows.Forms.Label();
            this.lblCornAddr1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forwardingInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noBarcodeDataBindingSource)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.borrowerDemosBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtZip);
            this.panel1.Controls.Add(this.txtCity);
            this.panel1.Controls.Add(this.txtAddress2);
            this.panel1.Controls.Add(this.txtAddress1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblActAcctNum);
            this.panel1.Controls.Add(this.lblSSN1);
            this.panel1.Controls.Add(this.RulesBtn);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnContinue);
            this.panel1.Controls.Add(this.cbState);
            this.panel1.Controls.Add(this.lblZip);
            this.panel1.Controls.Add(this.lblState);
            this.panel1.Controls.Add(this.lblCity);
            this.panel1.Controls.Add(this.lblAddress2);
            this.panel1.Controls.Add(this.lblAddress1);
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.lblAccountNum);
            this.panel1.Controls.Add(this.lblSsn);
            this.panel1.Location = new System.Drawing.Point(18, 38);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(587, 363);
            this.panel1.TabIndex = 1;
            // 
            // txtZip
            // 
            this.txtZip.AllowedSpecialCharacters = "";
            this.txtZip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.forwardingInfoBindingSource, "Zip", true));
            this.txtZip.Location = new System.Drawing.Point(148, 259);
            this.txtZip.MaxLength = 9;
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(137, 26);
            this.txtZip.TabIndex = 4;
            // 
            // forwardingInfoBindingSource
            // 
            this.forwardingInfoBindingSource.DataSource = typeof(BARCODEFED.ForwardingInfo);
            // 
            // txtCity
            // 
            this.txtCity.AllowedSpecialCharacters = " ";
            this.txtCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.forwardingInfoBindingSource, "City", true));
            this.txtCity.Location = new System.Drawing.Point(148, 184);
            this.txtCity.MaxLength = 20;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(235, 26);
            this.txtCity.TabIndex = 2;
            // 
            // txtAddress2
            // 
            this.txtAddress2.AllowedSpecialCharacters = " ";
            this.txtAddress2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.forwardingInfoBindingSource, "Address2", true));
            this.txtAddress2.Location = new System.Drawing.Point(148, 152);
            this.txtAddress2.MaxLength = 30;
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(373, 26);
            this.txtAddress2.TabIndex = 1;
            // 
            // txtAddress1
            // 
            this.txtAddress1.AllowedSpecialCharacters = " ";
            this.txtAddress1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.forwardingInfoBindingSource, "Address1", true));
            this.txtAddress1.Location = new System.Drawing.Point(148, 118);
            this.txtAddress1.MaxLength = 30;
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(373, 26);
            this.txtAddress1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.noBarcodeDataBindingSource, "Name", true));
            this.label3.Location = new System.Drawing.Point(144, 77);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 20;
            this.label3.Text = "label3";
            // 
            // noBarcodeDataBindingSource
            // 
            this.noBarcodeDataBindingSource.DataSource = typeof(BARCODEFED.NoBarcodeData);
            // 
            // lblActAcctNum
            // 
            this.lblActAcctNum.AutoSize = true;
            this.lblActAcctNum.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.noBarcodeDataBindingSource, "AccountNum", true));
            this.lblActAcctNum.Location = new System.Drawing.Point(144, 46);
            this.lblActAcctNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActAcctNum.Name = "lblActAcctNum";
            this.lblActAcctNum.Size = new System.Drawing.Size(51, 20);
            this.lblActAcctNum.TabIndex = 19;
            this.lblActAcctNum.Text = "label2";
            // 
            // lblSSN1
            // 
            this.lblSSN1.AutoSize = true;
            this.lblSSN1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.noBarcodeDataBindingSource, "SSN", true));
            this.lblSSN1.Location = new System.Drawing.Point(144, 15);
            this.lblSSN1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSSN1.Name = "lblSSN1";
            this.lblSSN1.Size = new System.Drawing.Size(51, 20);
            this.lblSSN1.TabIndex = 18;
            this.lblSSN1.Text = "label1";
            // 
            // RulesBtn
            // 
            this.RulesBtn.Location = new System.Drawing.Point(458, 321);
            this.RulesBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RulesBtn.Name = "RulesBtn";
            this.RulesBtn.Size = new System.Drawing.Size(112, 35);
            this.RulesBtn.TabIndex = 7;
            this.RulesBtn.Text = "Rules";
            this.RulesBtn.UseVisualStyleBackColor = true;
            this.RulesBtn.Click += new System.EventHandler(this.btnDaRules_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(244, 321);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 35);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(18, 321);
            this.btnContinue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(112, 35);
            this.btnContinue.TabIndex = 5;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // cbState
            // 
            this.cbState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.forwardingInfoBindingSource, "State", true));
            this.cbState.FormattingEnabled = true;
            this.cbState.Location = new System.Drawing.Point(148, 222);
            this.cbState.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbState.Name = "cbState";
            this.cbState.Size = new System.Drawing.Size(180, 28);
            this.cbState.TabIndex = 3;
            // 
            // lblZip
            // 
            this.lblZip.AutoSize = true;
            this.lblZip.Location = new System.Drawing.Point(77, 262);
            this.lblZip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(31, 20);
            this.lblZip.TabIndex = 8;
            this.lblZip.Text = "Zip";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(60, 225);
            this.lblState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(48, 20);
            this.lblState.TabIndex = 7;
            this.lblState.Text = "State";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(73, 190);
            this.lblCity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(35, 20);
            this.lblCity.TabIndex = 6;
            this.lblCity.Text = "City";
            // 
            // lblAddress2
            // 
            this.lblAddress2.AutoSize = true;
            this.lblAddress2.Location = new System.Drawing.Point(27, 155);
            this.lblAddress2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(81, 20);
            this.lblAddress2.TabIndex = 5;
            this.lblAddress2.Text = "Address 2";
            // 
            // lblAddress1
            // 
            this.lblAddress1.AutoSize = true;
            this.lblAddress1.Location = new System.Drawing.Point(23, 120);
            this.lblAddress1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(85, 20);
            this.lblAddress1.TabIndex = 4;
            this.lblAddress1.Text = "Address  1";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(57, 77);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(51, 20);
            this.lblName.TabIndex = 22;
            this.lblName.Text = "Name";
            // 
            // lblAccountNum
            // 
            this.lblAccountNum.AutoSize = true;
            this.lblAccountNum.Location = new System.Drawing.Point(27, 46);
            this.lblAccountNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccountNum.Name = "lblAccountNum";
            this.lblAccountNum.Size = new System.Drawing.Size(81, 20);
            this.lblAccountNum.TabIndex = 21;
            this.lblAccountNum.Text = "Account #";
            // 
            // lblSsn
            // 
            this.lblSsn.AutoSize = true;
            this.lblSsn.Location = new System.Drawing.Point(66, 15);
            this.lblSsn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSsn.Name = "lblSsn";
            this.lblSsn.Size = new System.Drawing.Size(42, 20);
            this.lblSsn.TabIndex = 20;
            this.lblSsn.Text = "SSN";
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(18, 14);
            this.lblText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(214, 20);
            this.lblText.TabIndex = 1;
            this.lblText.Text = "Enter the forwarding address";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnCornerStoneAddr);
            this.panel2.Controls.Add(this.lblCornActAdrEffDate);
            this.panel2.Controls.Add(this.lblCornAdrValid);
            this.panel2.Controls.Add(this.lblCornActZip);
            this.panel2.Controls.Add(this.lblCornActState);
            this.panel2.Controls.Add(this.lblCornActCity);
            this.panel2.Controls.Add(this.lblCornActAddr2);
            this.panel2.Controls.Add(this.lblCornActAdr1);
            this.panel2.Controls.Add(this.lblCornAddrEffDate);
            this.panel2.Controls.Add(this.lblAddrValid);
            this.panel2.Controls.Add(this.lblCornZip);
            this.panel2.Controls.Add(this.lblCornSt);
            this.panel2.Controls.Add(this.lblCornCity);
            this.panel2.Controls.Add(this.lblCornAddr2);
            this.panel2.Controls.Add(this.lblCornAddr1);
            this.panel2.Location = new System.Drawing.Point(646, 38);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(532, 299);
            this.panel2.TabIndex = 0;
            // 
            // btnCornerStoneAddr
            // 
            this.btnCornerStoneAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCornerStoneAddr.Location = new System.Drawing.Point(374, 240);
            this.btnCornerStoneAddr.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCornerStoneAddr.Name = "btnCornerStoneAddr";
            this.btnCornerStoneAddr.Size = new System.Drawing.Size(152, 52);
            this.btnCornerStoneAddr.TabIndex = 0;
            this.btnCornerStoneAddr.Text = "CornerStone Address";
            this.btnCornerStoneAddr.UseVisualStyleBackColor = true;
            this.btnCornerStoneAddr.Click += new System.EventHandler(this.btnCornerStoneAddr_Click);
            // 
            // lblCornActAdrEffDate
            // 
            this.lblCornActAdrEffDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCornActAdrEffDate.AutoSize = true;
            this.lblCornActAdrEffDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDemosBindingSource, "AddrEffectiveDate", true));
            this.lblCornActAdrEffDate.Location = new System.Drawing.Point(216, 206);
            this.lblCornActAdrEffDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornActAdrEffDate.Name = "lblCornActAdrEffDate";
            this.lblCornActAdrEffDate.Size = new System.Drawing.Size(51, 20);
            this.lblCornActAdrEffDate.TabIndex = 13;
            this.lblCornActAdrEffDate.Text = "label9";
            // 
            // borrowerDemosBindingSource
            // 
            this.borrowerDemosBindingSource.DataSource = typeof(BARCODEFED.BorrowerDemos);
            // 
            // lblCornAdrValid
            // 
            this.lblCornAdrValid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCornAdrValid.AutoSize = true;
            this.lblCornAdrValid.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDemosBindingSource, "AddrValid", true));
            this.lblCornAdrValid.Location = new System.Drawing.Point(216, 176);
            this.lblCornAdrValid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornAdrValid.Name = "lblCornAdrValid";
            this.lblCornAdrValid.Size = new System.Drawing.Size(51, 20);
            this.lblCornAdrValid.TabIndex = 12;
            this.lblCornAdrValid.Text = "label8";
            // 
            // lblCornActZip
            // 
            this.lblCornActZip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCornActZip.AutoSize = true;
            this.lblCornActZip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDemosBindingSource, "Zip", true));
            this.lblCornActZip.Location = new System.Drawing.Point(216, 146);
            this.lblCornActZip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornActZip.Name = "lblCornActZip";
            this.lblCornActZip.Size = new System.Drawing.Size(51, 20);
            this.lblCornActZip.TabIndex = 11;
            this.lblCornActZip.Text = "label7";
            // 
            // lblCornActState
            // 
            this.lblCornActState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCornActState.AutoSize = true;
            this.lblCornActState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDemosBindingSource, "State", true));
            this.lblCornActState.Location = new System.Drawing.Point(216, 116);
            this.lblCornActState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornActState.Name = "lblCornActState";
            this.lblCornActState.Size = new System.Drawing.Size(51, 20);
            this.lblCornActState.TabIndex = 10;
            this.lblCornActState.Text = "label6";
            // 
            // lblCornActCity
            // 
            this.lblCornActCity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCornActCity.AutoSize = true;
            this.lblCornActCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDemosBindingSource, "City", true));
            this.lblCornActCity.Location = new System.Drawing.Point(216, 86);
            this.lblCornActCity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornActCity.Name = "lblCornActCity";
            this.lblCornActCity.Size = new System.Drawing.Size(51, 20);
            this.lblCornActCity.TabIndex = 9;
            this.lblCornActCity.Text = "label5";
            // 
            // lblCornActAddr2
            // 
            this.lblCornActAddr2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCornActAddr2.AutoSize = true;
            this.lblCornActAddr2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDemosBindingSource, "Address2", true));
            this.lblCornActAddr2.Location = new System.Drawing.Point(216, 56);
            this.lblCornActAddr2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornActAddr2.Name = "lblCornActAddr2";
            this.lblCornActAddr2.Size = new System.Drawing.Size(51, 20);
            this.lblCornActAddr2.TabIndex = 8;
            this.lblCornActAddr2.Text = "label4";
            // 
            // lblCornActAdr1
            // 
            this.lblCornActAdr1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCornActAdr1.AutoSize = true;
            this.lblCornActAdr1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDemosBindingSource, "Address1", true));
            this.lblCornActAdr1.Location = new System.Drawing.Point(216, 26);
            this.lblCornActAdr1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornActAdr1.Name = "lblCornActAdr1";
            this.lblCornActAdr1.Size = new System.Drawing.Size(51, 20);
            this.lblCornActAdr1.TabIndex = 7;
            this.lblCornActAdr1.Text = "label2";
            // 
            // lblCornAddrEffDate
            // 
            this.lblCornAddrEffDate.AutoSize = true;
            this.lblCornAddrEffDate.Location = new System.Drawing.Point(22, 206);
            this.lblCornAddrEffDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornAddrEffDate.Name = "lblCornAddrEffDate";
            this.lblCornAddrEffDate.Size = new System.Drawing.Size(169, 20);
            this.lblCornAddrEffDate.TabIndex = 6;
            this.lblCornAddrEffDate.Text = "Address EffectiveDate";
            // 
            // lblAddrValid
            // 
            this.lblAddrValid.AutoSize = true;
            this.lblAddrValid.Location = new System.Drawing.Point(69, 176);
            this.lblAddrValid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddrValid.Name = "lblAddrValid";
            this.lblAddrValid.Size = new System.Drawing.Size(122, 20);
            this.lblAddrValid.TabIndex = 5;
            this.lblAddrValid.Text = "Address Validity";
            // 
            // lblCornZip
            // 
            this.lblCornZip.AutoSize = true;
            this.lblCornZip.Location = new System.Drawing.Point(160, 146);
            this.lblCornZip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornZip.Name = "lblCornZip";
            this.lblCornZip.Size = new System.Drawing.Size(31, 20);
            this.lblCornZip.TabIndex = 4;
            this.lblCornZip.Text = "Zip";
            // 
            // lblCornSt
            // 
            this.lblCornSt.AutoSize = true;
            this.lblCornSt.Location = new System.Drawing.Point(143, 116);
            this.lblCornSt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornSt.Name = "lblCornSt";
            this.lblCornSt.Size = new System.Drawing.Size(48, 20);
            this.lblCornSt.TabIndex = 3;
            this.lblCornSt.Text = "State";
            // 
            // lblCornCity
            // 
            this.lblCornCity.AutoSize = true;
            this.lblCornCity.Location = new System.Drawing.Point(156, 86);
            this.lblCornCity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornCity.Name = "lblCornCity";
            this.lblCornCity.Size = new System.Drawing.Size(35, 20);
            this.lblCornCity.TabIndex = 2;
            this.lblCornCity.Text = "City";
            // 
            // lblCornAddr2
            // 
            this.lblCornAddr2.AutoSize = true;
            this.lblCornAddr2.Location = new System.Drawing.Point(114, 56);
            this.lblCornAddr2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornAddr2.Name = "lblCornAddr2";
            this.lblCornAddr2.Size = new System.Drawing.Size(77, 20);
            this.lblCornAddr2.TabIndex = 1;
            this.lblCornAddr2.Text = "Address2";
            // 
            // lblCornAddr1
            // 
            this.lblCornAddr1.AutoSize = true;
            this.lblCornAddr1.Location = new System.Drawing.Point(114, 26);
            this.lblCornAddr1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCornAddr1.Name = "lblCornAddr1";
            this.lblCornAddr1.Size = new System.Drawing.Size(77, 20);
            this.lblCornAddr1.TabIndex = 0;
            this.lblCornAddr1.Text = "Address1";
            // 
            // DemographicUpdateScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 425);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(1207, 463);
            this.Name = "DemographicUpdateScreen";
            this.Text = "DemographicUpdateScreen";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forwardingInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noBarcodeDataBindingSource)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.borrowerDemosBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblAccountNum;
		private System.Windows.Forms.Label lblSsn;
		private System.Windows.Forms.Label lblZip;
		private System.Windows.Forms.Label lblState;
		private System.Windows.Forms.Label lblCity;
		private System.Windows.Forms.Label lblAddress2;
		private System.Windows.Forms.Label lblAddress1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblActAcctNum;
		private System.Windows.Forms.Label lblSSN1;
		private System.Windows.Forms.Button RulesBtn;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnContinue;
		private System.Windows.Forms.ComboBox cbState;
		private System.Windows.Forms.BindingSource noBarcodeDataBindingSource;
		private System.Windows.Forms.Label lblText;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label lblCornAddrEffDate;
		private System.Windows.Forms.Label lblAddrValid;
		private System.Windows.Forms.Label lblCornZip;
		private System.Windows.Forms.Label lblCornSt;
		private System.Windows.Forms.Label lblCornCity;
		private System.Windows.Forms.Label lblCornAddr2;
		private System.Windows.Forms.Label lblCornAddr1;
		private System.Windows.Forms.Label lblCornActAdrEffDate;
		private System.Windows.Forms.Label lblCornAdrValid;
		private System.Windows.Forms.Label lblCornActZip;
		private System.Windows.Forms.Label lblCornActState;
		private System.Windows.Forms.Label lblCornActCity;
		private System.Windows.Forms.Label lblCornActAddr2;
		private System.Windows.Forms.Label lblCornActAdr1;
		private System.Windows.Forms.BindingSource borrowerDemosBindingSource;
		private System.Windows.Forms.Button btnCornerStoneAddr;
		private System.Windows.Forms.BindingSource forwardingInfoBindingSource;
        private Uheaa.Common.WinForms.AlphaNumericTextBox txtAddress2;
        private Uheaa.Common.WinForms.AlphaNumericTextBox txtAddress1;
        private Uheaa.Common.WinForms.NumericTextBox txtZip;
        private Uheaa.Common.WinForms.AlphaTextBox txtCity;
    }
}