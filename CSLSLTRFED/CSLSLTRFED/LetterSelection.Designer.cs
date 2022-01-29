namespace CSLSLTRFED
{
    partial class LetterSelection
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
            this.lblLetterType = new System.Windows.Forms.Label();
            this.cboLetterType = new System.Windows.Forms.ComboBox();
            this.lblLetterOptions = new System.Windows.Forms.Label();
            this.cboLetterOptions = new System.Windows.Forms.ComboBox();
            this.lblChoices = new System.Windows.Forms.Label();
            this.cboChoices = new System.Windows.Forms.ComboBox();
            this.panelOptions = new System.Windows.Forms.Panel();
            this.txtManualDenial = new System.Windows.Forms.TextBox();
            this.lblManualDenial = new System.Windows.Forms.Label();
            this.txtDischargeAmt = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.lblDash = new System.Windows.Forms.Label();
            this.lblLowIncome = new System.Windows.Forms.Label();
            this.dtpLoanTermEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblLoanTermEndDate = new System.Windows.Forms.Label();
            this.dtpDefForbDate = new System.Windows.Forms.DateTimePicker();
            this.lblDefForbDate = new System.Windows.Forms.Label();
            this.cboDefForb = new System.Windows.Forms.ComboBox();
            this.lblDefForb = new System.Windows.Forms.Label();
            this.dtpSchoolClosure = new System.Windows.Forms.DateTimePicker();
            this.lblDateOfClosure = new System.Windows.Forms.Label();
            this.lblLastdate = new System.Windows.Forms.Label();
            this.dtpLastDate = new System.Windows.Forms.DateTimePicker();
            this.txtSchoolName = new System.Windows.Forms.TextBox();
            this.lblSchoolName = new System.Windows.Forms.Label();
            this.lblAmtAprv = new System.Windows.Forms.Label();
            this.lblAdditionDenialResons = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cboAdditionalDenial = new System.Windows.Forms.ComboBox();
            this.lsvAdditionalReasons = new System.Windows.Forms.ListBox();
            this.ckbAdditionDenial = new System.Windows.Forms.CheckBox();
            this.CurrentVersion = new System.Windows.Forms.Label();
            this.lblManualLetters = new System.Windows.Forms.Label();
            this.txtEndYear = new Uheaa.Common.WinForms.NumericTextBox();
            this.txtBeginYear = new Uheaa.Common.WinForms.NumericTextBox();
            this.txtAccountNumber = new Uheaa.Common.WinForms.WatermarkAccountIdentifierTextBox();
            this.cboManualLetters = new System.Windows.Forms.ComboBox();
            this.inputDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panelOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inputDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLetterType
            // 
            this.lblLetterType.AutoSize = true;
            this.lblLetterType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLetterType.Location = new System.Drawing.Point(33, 18);
            this.lblLetterType.Name = "lblLetterType";
            this.lblLetterType.Size = new System.Drawing.Size(93, 20);
            this.lblLetterType.TabIndex = 0;
            this.lblLetterType.Text = "Letter Type:";
            // 
            // cboLetterType
            // 
            this.cboLetterType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLetterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLetterType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLetterType.FormattingEnabled = true;
            this.cboLetterType.Location = new System.Drawing.Point(132, 15);
            this.cboLetterType.Name = "cboLetterType";
            this.cboLetterType.Size = new System.Drawing.Size(248, 28);
            this.cboLetterType.TabIndex = 0;
            this.cboLetterType.SelectedIndexChanged += new System.EventHandler(this.cboLetterType_SelectedIndexChanged);
            this.cboLetterType.SelectionChangeCommitted += new System.EventHandler(this.cboLetterType_SelectionChangeCommitted);
            // 
            // lblLetterOptions
            // 
            this.lblLetterOptions.AutoSize = true;
            this.lblLetterOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLetterOptions.Location = new System.Drawing.Point(12, 54);
            this.lblLetterOptions.Name = "lblLetterOptions";
            this.lblLetterOptions.Size = new System.Drawing.Size(114, 20);
            this.lblLetterOptions.TabIndex = 2;
            this.lblLetterOptions.Text = "Letter Options:";
            // 
            // cboLetterOptions
            // 
            this.cboLetterOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLetterOptions.DropDownWidth = 375;
            this.cboLetterOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLetterOptions.Location = new System.Drawing.Point(132, 49);
            this.cboLetterOptions.Name = "cboLetterOptions";
            this.cboLetterOptions.Size = new System.Drawing.Size(248, 28);
            this.cboLetterOptions.TabIndex = 1;
            this.cboLetterOptions.SelectionChangeCommitted += new System.EventHandler(this.cboLetterOptions_SelectionChangeCommitted);
            this.cboLetterOptions.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cboLetterOptions_MouseClick);
            // 
            // lblChoices
            // 
            this.lblChoices.AutoEllipsis = true;
            this.lblChoices.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChoices.Location = new System.Drawing.Point(12, 87);
            this.lblChoices.Name = "lblChoices";
            this.lblChoices.Size = new System.Drawing.Size(114, 26);
            this.lblChoices.TabIndex = 4;
            this.lblChoices.Text = "Choices:";
            this.lblChoices.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboChoices
            // 
            this.cboChoices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboChoices.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboChoices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChoices.DropDownWidth = 300;
            this.cboChoices.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboChoices.FormattingEnabled = true;
            this.cboChoices.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cboChoices.Location = new System.Drawing.Point(132, 87);
            this.cboChoices.Name = "cboChoices";
            this.cboChoices.Size = new System.Drawing.Size(248, 27);
            this.cboChoices.TabIndex = 2;
            this.cboChoices.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboChoices_DrawItem);
            this.cboChoices.SelectionChangeCommitted += new System.EventHandler(this.cboChoices_SelectionChangeCommitted);
            this.cboChoices.DropDownClosed += new System.EventHandler(this.cboChoices_DropDownClosed);
            // 
            // panelOptions
            // 
            this.panelOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOptions.Controls.Add(this.txtManualDenial);
            this.panelOptions.Controls.Add(this.lblManualDenial);
            this.panelOptions.Controls.Add(this.txtDischargeAmt);
            this.panelOptions.Controls.Add(this.txtEndYear);
            this.panelOptions.Controls.Add(this.txtBeginYear);
            this.panelOptions.Controls.Add(this.txtAccountNumber);
            this.panelOptions.Controls.Add(this.btnCancel);
            this.panelOptions.Controls.Add(this.btnContinue);
            this.panelOptions.Controls.Add(this.lblDash);
            this.panelOptions.Controls.Add(this.lblLowIncome);
            this.panelOptions.Controls.Add(this.dtpLoanTermEndDate);
            this.panelOptions.Controls.Add(this.lblLoanTermEndDate);
            this.panelOptions.Controls.Add(this.dtpDefForbDate);
            this.panelOptions.Controls.Add(this.lblDefForbDate);
            this.panelOptions.Controls.Add(this.cboDefForb);
            this.panelOptions.Controls.Add(this.lblDefForb);
            this.panelOptions.Controls.Add(this.dtpSchoolClosure);
            this.panelOptions.Controls.Add(this.lblDateOfClosure);
            this.panelOptions.Controls.Add(this.lblLastdate);
            this.panelOptions.Controls.Add(this.dtpLastDate);
            this.panelOptions.Controls.Add(this.txtSchoolName);
            this.panelOptions.Controls.Add(this.lblSchoolName);
            this.panelOptions.Controls.Add(this.lblAmtAprv);
            this.panelOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelOptions.Location = new System.Drawing.Point(393, 18);
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Size = new System.Drawing.Size(622, 416);
            this.panelOptions.TabIndex = 6;
            // 
            // txtManualDenial
            // 
            this.txtManualDenial.Enabled = false;
            this.txtManualDenial.Location = new System.Drawing.Point(278, 213);
            this.txtManualDenial.MaxLength = 500;
            this.txtManualDenial.Multiline = true;
            this.txtManualDenial.Name = "txtManualDenial";
            this.txtManualDenial.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtManualDenial.Size = new System.Drawing.Size(329, 63);
            this.txtManualDenial.TabIndex = 7;
            this.txtManualDenial.TextChanged += new System.EventHandler(this.txtManualDenial_TextChanged);
            // 
            // lblManualDenial
            // 
            this.lblManualDenial.AutoSize = true;
            this.lblManualDenial.Location = new System.Drawing.Point(95, 229);
            this.lblManualDenial.Name = "lblManualDenial";
            this.lblManualDenial.Size = new System.Drawing.Size(174, 20);
            this.lblManualDenial.TabIndex = 18;
            this.lblManualDenial.Text = "Manual Denial Reason:";
            // 
            // txtDischargeAmt
            // 
            this.txtDischargeAmt.Location = new System.Drawing.Point(278, 8);
            this.txtDischargeAmt.MaxLength = 10;
            this.txtDischargeAmt.Name = "txtDischargeAmt";
            this.txtDischargeAmt.Size = new System.Drawing.Size(145, 26);
            this.txtDischargeAmt.TabIndex = 0;
            this.txtDischargeAmt.TextChanged += new System.EventHandler(this.txtDischargeAmt_TextChanged);
            this.txtDischargeAmt.Validating += new System.ComponentModel.CancelEventHandler(this.txtDischargeAmt_Validating);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(453, 366);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(103, 32);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnContinue
            // 
            this.btnContinue.Enabled = false;
            this.btnContinue.Location = new System.Drawing.Point(279, 366);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(103, 32);
            this.btnContinue.TabIndex = 11;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // lblDash
            // 
            this.lblDash.AutoSize = true;
            this.lblDash.Location = new System.Drawing.Point(418, 292);
            this.lblDash.Name = "lblDash";
            this.lblDash.Size = new System.Drawing.Size(19, 20);
            this.lblDash.TabIndex = 16;
            this.lblDash.Text = "--";
            // 
            // lblLowIncome
            // 
            this.lblLowIncome.AutoSize = true;
            this.lblLowIncome.Location = new System.Drawing.Point(3, 280);
            this.lblLowIncome.MaximumSize = new System.Drawing.Size(300, 40);
            this.lblLowIncome.Name = "lblLowIncome";
            this.lblLowIncome.Size = new System.Drawing.Size(281, 40);
            this.lblLowIncome.TabIndex = 14;
            this.lblLowIncome.Text = "Enter the year the school will apear on the Low Income Directory:";
            // 
            // dtpLoanTermEndDate
            // 
            this.dtpLoanTermEndDate.Enabled = false;
            this.dtpLoanTermEndDate.Location = new System.Drawing.Point(278, 184);
            this.dtpLoanTermEndDate.Name = "dtpLoanTermEndDate";
            this.dtpLoanTermEndDate.Size = new System.Drawing.Size(277, 26);
            this.dtpLoanTermEndDate.TabIndex = 6;
            // 
            // lblLoanTermEndDate
            // 
            this.lblLoanTermEndDate.AutoSize = true;
            this.lblLoanTermEndDate.Location = new System.Drawing.Point(45, 191);
            this.lblLoanTermEndDate.Name = "lblLoanTermEndDate";
            this.lblLoanTermEndDate.Size = new System.Drawing.Size(227, 20);
            this.lblLoanTermEndDate.TabIndex = 12;
            this.lblLoanTermEndDate.Text = "Select a Loan Term End Date: ";
            // 
            // dtpDefForbDate
            // 
            this.dtpDefForbDate.Enabled = false;
            this.dtpDefForbDate.Location = new System.Drawing.Point(278, 155);
            this.dtpDefForbDate.Name = "dtpDefForbDate";
            this.dtpDefForbDate.Size = new System.Drawing.Size(277, 26);
            this.dtpDefForbDate.TabIndex = 5;
            // 
            // lblDefForbDate
            // 
            this.lblDefForbDate.AutoSize = true;
            this.lblDefForbDate.Location = new System.Drawing.Point(12, 158);
            this.lblDefForbDate.Name = "lblDefForbDate";
            this.lblDefForbDate.Size = new System.Drawing.Size(260, 20);
            this.lblDefForbDate.TabIndex = 10;
            this.lblDefForbDate.Text = "Deferment/Forbearance End Date: ";
            // 
            // cboDefForb
            // 
            this.cboDefForb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDefForb.DropDownWidth = 400;
            this.cboDefForb.Enabled = false;
            this.cboDefForb.FormattingEnabled = true;
            this.cboDefForb.Location = new System.Drawing.Point(278, 124);
            this.cboDefForb.Name = "cboDefForb";
            this.cboDefForb.Size = new System.Drawing.Size(328, 28);
            this.cboDefForb.TabIndex = 4;
            // 
            // lblDefForb
            // 
            this.lblDefForb.AutoSize = true;
            this.lblDefForb.Location = new System.Drawing.Point(50, 127);
            this.lblDefForb.Name = "lblDefForb";
            this.lblDefForb.Size = new System.Drawing.Size(222, 20);
            this.lblDefForb.TabIndex = 8;
            this.lblDefForb.Text = "Deferment/Forbearance Type:";
            // 
            // dtpSchoolClosure
            // 
            this.dtpSchoolClosure.Enabled = false;
            this.dtpSchoolClosure.Location = new System.Drawing.Point(278, 95);
            this.dtpSchoolClosure.Name = "dtpSchoolClosure";
            this.dtpSchoolClosure.Size = new System.Drawing.Size(277, 26);
            this.dtpSchoolClosure.TabIndex = 3;
            // 
            // lblDateOfClosure
            // 
            this.lblDateOfClosure.AutoSize = true;
            this.lblDateOfClosure.Location = new System.Drawing.Point(113, 98);
            this.lblDateOfClosure.Name = "lblDateOfClosure";
            this.lblDateOfClosure.Size = new System.Drawing.Size(159, 20);
            this.lblDateOfClosure.TabIndex = 6;
            this.lblDateOfClosure.Text = "School Closure Date:";
            // 
            // lblLastdate
            // 
            this.lblLastdate.AutoSize = true;
            this.lblLastdate.Location = new System.Drawing.Point(84, 69);
            this.lblLastdate.Name = "lblLastdate";
            this.lblLastdate.Size = new System.Drawing.Size(188, 20);
            this.lblLastdate.TabIndex = 5;
            this.lblLastdate.Text = "Last Date of Attendance:";
            // 
            // dtpLastDate
            // 
            this.dtpLastDate.Enabled = false;
            this.dtpLastDate.Location = new System.Drawing.Point(278, 66);
            this.dtpLastDate.Name = "dtpLastDate";
            this.dtpLastDate.Size = new System.Drawing.Size(277, 26);
            this.dtpLastDate.TabIndex = 2;
            // 
            // txtSchoolName
            // 
            this.txtSchoolName.Enabled = false;
            this.txtSchoolName.Location = new System.Drawing.Point(278, 37);
            this.txtSchoolName.MaxLength = 60;
            this.txtSchoolName.Name = "txtSchoolName";
            this.txtSchoolName.Size = new System.Drawing.Size(329, 26);
            this.txtSchoolName.TabIndex = 1;
            // 
            // lblSchoolName
            // 
            this.lblSchoolName.AutoSize = true;
            this.lblSchoolName.Location = new System.Drawing.Point(117, 40);
            this.lblSchoolName.Name = "lblSchoolName";
            this.lblSchoolName.Size = new System.Drawing.Size(155, 20);
            this.lblSchoolName.TabIndex = 2;
            this.lblSchoolName.Text = "Enter School Name: ";
            // 
            // lblAmtAprv
            // 
            this.lblAmtAprv.AutoSize = true;
            this.lblAmtAprv.Location = new System.Drawing.Point(32, 11);
            this.lblAmtAprv.Name = "lblAmtAprv";
            this.lblAmtAprv.Size = new System.Drawing.Size(240, 20);
            this.lblAmtAprv.TabIndex = 0;
            this.lblAmtAprv.Text = "Amount Approved for Discharge:";
            // 
            // lblAdditionDenialResons
            // 
            this.lblAdditionDenialResons.AutoEllipsis = true;
            this.lblAdditionDenialResons.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdditionDenialResons.Location = new System.Drawing.Point(12, 184);
            this.lblAdditionDenialResons.Name = "lblAdditionDenialResons";
            this.lblAdditionDenialResons.Size = new System.Drawing.Size(277, 20);
            this.lblAdditionDenialResons.TabIndex = 8;
            this.lblAdditionDenialResons.Text = "Additional Denial Reasons:";
            this.lblAdditionDenialResons.Visible = false;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.StripAmpersands = true;
            // 
            // cboAdditionalDenial
            // 
            this.cboAdditionalDenial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAdditionalDenial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAdditionalDenial.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAdditionalDenial.FormattingEnabled = true;
            this.cboAdditionalDenial.Location = new System.Drawing.Point(16, 207);
            this.cboAdditionalDenial.Name = "cboAdditionalDenial";
            this.cboAdditionalDenial.Size = new System.Drawing.Size(364, 28);
            this.cboAdditionalDenial.TabIndex = 5;
            this.cboAdditionalDenial.Visible = false;
            this.cboAdditionalDenial.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboAdditionalDenial_DrawItem);
            this.cboAdditionalDenial.SelectionChangeCommitted += new System.EventHandler(this.cboAdditionalDenial_SelectionChangeCommitted);
            this.cboAdditionalDenial.DropDownClosed += new System.EventHandler(this.cboAdditionalDenial_DropDownClosed);
            // 
            // lsvAdditionalReasons
            // 
            this.lsvAdditionalReasons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvAdditionalReasons.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsvAdditionalReasons.FormattingEnabled = true;
            this.lsvAdditionalReasons.ItemHeight = 20;
            this.lsvAdditionalReasons.Location = new System.Drawing.Point(16, 247);
            this.lsvAdditionalReasons.Name = "lsvAdditionalReasons";
            this.lsvAdditionalReasons.Size = new System.Drawing.Size(364, 184);
            this.lsvAdditionalReasons.TabIndex = 6;
            this.lsvAdditionalReasons.Visible = false;
            this.lsvAdditionalReasons.DoubleClick += new System.EventHandler(this.lsvAdditionalReasons_DoubleClick);
            // 
            // ckbAdditionDenial
            // 
            this.ckbAdditionDenial.AutoSize = true;
            this.ckbAdditionDenial.Enabled = false;
            this.ckbAdditionDenial.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckbAdditionDenial.Location = new System.Drawing.Point(132, 153);
            this.ckbAdditionDenial.Name = "ckbAdditionDenial";
            this.ckbAdditionDenial.Size = new System.Drawing.Size(248, 24);
            this.ckbAdditionDenial.TabIndex = 4;
            this.ckbAdditionDenial.Text = "Add Additional Denial Reasons";
            this.ckbAdditionDenial.UseVisualStyleBackColor = true;
            this.ckbAdditionDenial.CheckedChanged += new System.EventHandler(this.ckbAdditionDenial_CheckedChanged);
            // 
            // CurrentVersion
            // 
            this.CurrentVersion.Location = new System.Drawing.Point(771, 437);
            this.CurrentVersion.Name = "CurrentVersion";
            this.CurrentVersion.Size = new System.Drawing.Size(246, 20);
            this.CurrentVersion.TabIndex = 12;
            this.CurrentVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblManualLetters
            // 
            this.lblManualLetters.AutoEllipsis = true;
            this.lblManualLetters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblManualLetters.Location = new System.Drawing.Point(5, 120);
            this.lblManualLetters.Name = "lblManualLetters";
            this.lblManualLetters.Size = new System.Drawing.Size(121, 26);
            this.lblManualLetters.TabIndex = 14;
            this.lblManualLetters.Text = "Manual Letters:";
            this.lblManualLetters.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEndYear
            // 
            this.txtEndYear.AllowedSpecialCharacters = "";
            this.txtEndYear.Location = new System.Drawing.Point(443, 290);
            this.txtEndYear.Name = "txtEndYear";
            this.txtEndYear.Size = new System.Drawing.Size(71, 26);
            this.txtEndYear.TabIndex = 9;
            // 
            // txtBeginYear
            // 
            this.txtBeginYear.AllowedSpecialCharacters = "";
            this.txtBeginYear.Location = new System.Drawing.Point(341, 290);
            this.txtBeginYear.Name = "txtBeginYear";
            this.txtBeginYear.Size = new System.Drawing.Size(71, 26);
            this.txtBeginYear.TabIndex = 8;
            this.txtBeginYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.AccountNumber = null;
            this.txtAccountNumber.AllowedSpecialCharacters = "";
            this.txtAccountNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.txtAccountNumber.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtAccountNumber.Location = new System.Drawing.Point(332, 324);
            this.txtAccountNumber.MaxLength = 10;
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Size = new System.Drawing.Size(202, 26);
            this.txtAccountNumber.Ssn = null;
            this.txtAccountNumber.TabIndex = 10;
            this.txtAccountNumber.Text = "SSN / Account Number";
            this.txtAccountNumber.Watermark = "SSN / Account Number";
            this.txtAccountNumber.TextChanged += new System.EventHandler(this.txtAccountNumber_TextChanged);
            this.txtAccountNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAccountNumber_KeyPress);
            // 
            // cboManualLetters
            // 
            this.cboManualLetters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboManualLetters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboManualLetters.FormattingEnabled = true;
            this.cboManualLetters.Location = new System.Drawing.Point(132, 121);
            this.cboManualLetters.Name = "cboManualLetters";
            this.cboManualLetters.Size = new System.Drawing.Size(248, 28);
            this.cboManualLetters.TabIndex = 3;
            this.cboManualLetters.SelectionChangeCommitted += new System.EventHandler(this.cboManualLetters_SelectionChangeCommitted);
            // 
            // inputDataBindingSource
            // 
            this.inputDataBindingSource.DataSource = typeof(CSLSLTRFED.InputData);
            // 
            // LetterSelection
            // 
            this.AcceptButton = this.btnContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1029, 466);
            this.Controls.Add(this.cboManualLetters);
            this.Controls.Add(this.lblManualLetters);
            this.Controls.Add(this.CurrentVersion);
            this.Controls.Add(this.ckbAdditionDenial);
            this.Controls.Add(this.lsvAdditionalReasons);
            this.Controls.Add(this.cboAdditionalDenial);
            this.Controls.Add(this.lblAdditionDenialResons);
            this.Controls.Add(this.panelOptions);
            this.Controls.Add(this.cboChoices);
            this.Controls.Add(this.lblChoices);
            this.Controls.Add(this.cboLetterOptions);
            this.Controls.Add(this.lblLetterOptions);
            this.Controls.Add(this.cboLetterType);
            this.Controls.Add(this.lblLetterType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1045, 505);
            this.Name = "LetterSelection";
            this.ShowIcon = false;
            this.Text = "Cornerstone Loan Servicing Letters";
            this.panelOptions.ResumeLayout(false);
            this.panelOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inputDataBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLetterType;
        private System.Windows.Forms.ComboBox cboLetterType;
        private System.Windows.Forms.Label lblLetterOptions;
        private System.Windows.Forms.ComboBox cboLetterOptions;
        private System.Windows.Forms.Label lblChoices;
        private System.Windows.Forms.ComboBox cboChoices;
		private System.Windows.Forms.Panel panelOptions;
        private System.Windows.Forms.Label lblAmtAprv;
        private System.Windows.Forms.TextBox txtSchoolName;
        private System.Windows.Forms.Label lblSchoolName;
        private System.Windows.Forms.DateTimePicker dtpSchoolClosure;
        private System.Windows.Forms.Label lblDateOfClosure;
        private System.Windows.Forms.Label lblLastdate;
        private System.Windows.Forms.DateTimePicker dtpLastDate;
        private System.Windows.Forms.DateTimePicker dtpDefForbDate;
        private System.Windows.Forms.Label lblDefForbDate;
        private System.Windows.Forms.ComboBox cboDefForb;
        private System.Windows.Forms.Label lblDefForb;
		private System.Windows.Forms.Label lblLowIncome;
		private System.Windows.Forms.Label lblDash;
        private System.Windows.Forms.Label lblAdditionDenialResons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox cboAdditionalDenial;
        private System.Windows.Forms.ListBox lsvAdditionalReasons;
        private System.Windows.Forms.CheckBox ckbAdditionDenial;
        private System.Windows.Forms.BindingSource inputDataBindingSource;
        private System.Windows.Forms.Label CurrentVersion;
        private Uheaa.Common.WinForms.WatermarkAccountIdentifierTextBox txtAccountNumber;
		private Uheaa.Common.WinForms.NumericTextBox txtEndYear;
		private Uheaa.Common.WinForms.NumericTextBox txtBeginYear;
        private System.Windows.Forms.TextBox txtDischargeAmt;
        private System.Windows.Forms.TextBox txtManualDenial;
        private System.Windows.Forms.Label lblManualDenial;
        private System.Windows.Forms.DateTimePicker dtpLoanTermEndDate;
        private System.Windows.Forms.Label lblLoanTermEndDate;
        private System.Windows.Forms.Label lblManualLetters;
        private System.Windows.Forms.ComboBox cboManualLetters;
    }
}