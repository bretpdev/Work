namespace IDRUSERPRO
{
    partial class ApplicationEntry
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
            this.tabs = new System.Windows.Forms.TabControl();
            this.ApplicationTab = new System.Windows.Forms.TabPage();
            this.ApplicationInformationControl = new IDRUSERPRO.ApplicationInformation();
            this.PlanTab = new System.Windows.Forms.TabPage();
            this.PlanDueDate = new System.Windows.Forms.ComboBox();
            this.RequestReasonCbo = new System.Windows.Forms.ComboBox();
            this.RequestReasonLbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.HouseholdTab = new System.Windows.Forms.TabPage();
            this.FamilySizeIncreasedBox = new System.Windows.Forms.ComboBox();
            this.SpouseExternalLoansBox = new Uheaa.Common.WinForms.YesNoComboBox();
            this.SpouseExternalLoansLabel = new System.Windows.Forms.Label();
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
            this.IncomeTab = new System.Windows.Forms.TabPage();
            this.SpouseIncomeInformation = new IDRUSERPRO.IncomeInformation();
            this.BorrowerIncomeInformation = new IDRUSERPRO.IncomeInformation();
            this.LoansTab = new System.Windows.Forms.TabPage();
            this.SpouseInfoGroup = new System.Windows.Forms.Panel();
            this.SpouseDisplayBtn = new System.Windows.Forms.Button();
            this.label29 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ExternalLoansBox = new Uheaa.Common.WinForms.YesNoComboBox();
            this.ExternalLoansLabel = new System.Windows.Forms.Label();
            this.BorDisplayBtn = new System.Windows.Forms.Button();
            this.BorEligibilityCbo = new System.Windows.Forms.ComboBox();
            this.BorGradeLevelCbo = new System.Windows.Forms.ComboBox();
            this.BorEligibilityLbl = new System.Windows.Forms.Label();
            this.BorGradeLevelLbl = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.DefTab = new System.Windows.Forms.TabPage();
            this.RequestedRpfBox = new Uheaa.Common.WinForms.NumericDecimalTextBox();
            this.DefForbCbo = new System.Windows.Forms.ComboBox();
            this.RequestedRpfLabel = new System.Windows.Forms.Label();
            this.DefForbLbl = new System.Windows.Forms.Label();
            this.StatusTab = new System.Windows.Forms.TabPage();
            this.AppSubstatusCbo = new System.Windows.Forms.ComboBox();
            this.AppSubstatusLbl = new System.Windows.Forms.Label();
            this.AppStatusCbo = new System.Windows.Forms.ComboBox();
            this.AppStatusLbl = new System.Windows.Forms.Label();
            this.StatusRequestedPlanLbl = new System.Windows.Forms.Label();
            this.ProcessedBox = new System.Windows.Forms.ComboBox();
            this.label32 = new System.Windows.Forms.Label();
            this.StatusProcessedLbl = new System.Windows.Forms.Label();
            this.SpouseSigned = new System.Windows.Forms.CheckBox();
            this.BorrowerSigned = new System.Windows.Forms.CheckBox();
            this.PlansContainer = new System.Windows.Forms.Panel();
            this.Ibr2014Plan = new IDRUSERPRO.RepaymentPlan();
            this.IbrPlan = new IDRUSERPRO.RepaymentPlan();
            this.Cancel = new System.Windows.Forms.Button();
            this.Validation = new System.Windows.Forms.Button();
            this.Complete = new System.Windows.Forms.Button();
            this.NextStep = new System.Windows.Forms.Button();
            this.tabs.SuspendLayout();
            this.ApplicationTab.SuspendLayout();
            this.PlanTab.SuspendLayout();
            this.HouseholdTab.SuspendLayout();
            this.SpouseGroup.SuspendLayout();
            this.IncomeTab.SuspendLayout();
            this.LoansTab.SuspendLayout();
            this.SpouseInfoGroup.SuspendLayout();
            this.panel4.SuspendLayout();
            this.DefTab.SuspendLayout();
            this.StatusTab.SuspendLayout();
            this.PlansContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.ApplicationTab);
            this.tabs.Controls.Add(this.PlanTab);
            this.tabs.Controls.Add(this.HouseholdTab);
            this.tabs.Controls.Add(this.IncomeTab);
            this.tabs.Controls.Add(this.LoansTab);
            this.tabs.Controls.Add(this.DefTab);
            this.tabs.Controls.Add(this.StatusTab);
            this.tabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabs.HotTrack = true;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.ShowToolTips = true;
            this.tabs.Size = new System.Drawing.Size(966, 365);
            this.tabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabs.TabIndex = 0;
            this.tabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_DrawItem);
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_SelectedIndexChanged);
            this.tabs.TabIndexChanged += new System.EventHandler(this.tabs_SelectedIndexChanged);
            // 
            // ApplicationTab
            // 
            this.ApplicationTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ApplicationTab.Controls.Add(this.ApplicationInformationControl);
            this.ApplicationTab.Location = new System.Drawing.Point(4, 29);
            this.ApplicationTab.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.ApplicationTab.Name = "ApplicationTab";
            this.ApplicationTab.Padding = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.ApplicationTab.Size = new System.Drawing.Size(958, 332);
            this.ApplicationTab.TabIndex = 0;
            this.ApplicationTab.Text = "Application";
            // 
            // ApplicationInformationControl
            // 
            this.ApplicationInformationControl.AccountNumber = "Account Number:";
            this.ApplicationInformationControl.ApplicationId = null;
            this.ApplicationInformationControl.ApplicationReceivedDate = null;
            this.ApplicationInformationControl.ApplicationSourceId = null;
            this.ApplicationInformationControl.AwardId = "";
            this.ApplicationInformationControl.CodId = "";
            this.ApplicationInformationControl.CodIdMaxLength = 9;
            this.ApplicationInformationControl.DA = null;
            this.ApplicationInformationControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplicationInformationControl.Inactive = false;
            this.ApplicationInformationControl.Location = new System.Drawing.Point(2, 18);
            this.ApplicationInformationControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ApplicationInformationControl.Name = "ApplicationInformationControl";
            this.ApplicationInformationControl.NewApp = false;
            this.ApplicationInformationControl.Size = new System.Drawing.Size(340, 312);
            this.ApplicationInformationControl.TabIndex = 0;
            // 
            // PlanTab
            // 
            this.PlanTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PlanTab.Controls.Add(this.PlanDueDate);
            this.PlanTab.Controls.Add(this.RequestReasonCbo);
            this.PlanTab.Controls.Add(this.RequestReasonLbl);
            this.PlanTab.Controls.Add(this.label2);
            this.PlanTab.Location = new System.Drawing.Point(4, 29);
            this.PlanTab.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.PlanTab.Name = "PlanTab";
            this.PlanTab.Padding = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.PlanTab.Size = new System.Drawing.Size(958, 332);
            this.PlanTab.TabIndex = 1;
            this.PlanTab.Text = "Plan";
            // 
            // PlanDueDate
            // 
            this.PlanDueDate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.PlanDueDate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.PlanDueDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlanDueDate.FormattingEnabled = true;
            this.PlanDueDate.Location = new System.Drawing.Point(298, 64);
            this.PlanDueDate.Name = "PlanDueDate";
            this.PlanDueDate.Size = new System.Drawing.Size(42, 28);
            this.PlanDueDate.TabIndex = 1;
            // 
            // RequestReasonCbo
            // 
            this.RequestReasonCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RequestReasonCbo.FormattingEnabled = true;
            this.RequestReasonCbo.Location = new System.Drawing.Point(298, 21);
            this.RequestReasonCbo.Name = "RequestReasonCbo";
            this.RequestReasonCbo.Size = new System.Drawing.Size(200, 28);
            this.RequestReasonCbo.TabIndex = 0;
            this.RequestReasonCbo.SelectedIndexChanged += new System.EventHandler(this.RequestReasonCbo_SelectedIndexChanged);
            // 
            // RequestReasonLbl
            // 
            this.RequestReasonLbl.AutoSize = true;
            this.RequestReasonLbl.Location = new System.Drawing.Point(144, 24);
            this.RequestReasonLbl.Name = "RequestReasonLbl";
            this.RequestReasonLbl.Size = new System.Drawing.Size(134, 20);
            this.RequestReasonLbl.TabIndex = 1;
            this.RequestReasonLbl.Text = "Request Reason:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Due Date Requested:";
            // 
            // HouseholdTab
            // 
            this.HouseholdTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.HouseholdTab.Controls.Add(this.FamilySizeIncreasedBox);
            this.HouseholdTab.Controls.Add(this.SpouseExternalLoansBox);
            this.HouseholdTab.Controls.Add(this.SpouseExternalLoansLabel);
            this.HouseholdTab.Controls.Add(this.FilingStatusCbo);
            this.HouseholdTab.Controls.Add(this.MaritalStatusCbo);
            this.HouseholdTab.Controls.Add(this.SpouseGroup);
            this.HouseholdTab.Controls.Add(this.FilingStatusLbl);
            this.HouseholdTab.Controls.Add(this.MaritalStatusLbl);
            this.HouseholdTab.Controls.Add(this.DependentsLabel);
            this.HouseholdTab.Controls.Add(this.ChildrenLabel);
            this.HouseholdTab.Controls.Add(this.Dependents);
            this.HouseholdTab.Controls.Add(this.Children);
            this.HouseholdTab.Location = new System.Drawing.Point(4, 29);
            this.HouseholdTab.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.HouseholdTab.Name = "HouseholdTab";
            this.HouseholdTab.Size = new System.Drawing.Size(958, 332);
            this.HouseholdTab.TabIndex = 2;
            this.HouseholdTab.Text = "Household";
            // 
            // FamilySizeIncreasedBox
            // 
            this.FamilySizeIncreasedBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FamilySizeIncreasedBox.FormattingEnabled = true;
            this.FamilySizeIncreasedBox.Items.AddRange(new object[] {
            "Family size increase has NOT been confirmed",
            "Family size increase has been confirmed"});
            this.FamilySizeIncreasedBox.Location = new System.Drawing.Point(171, 22);
            this.FamilySizeIncreasedBox.Name = "FamilySizeIncreasedBox";
            this.FamilySizeIncreasedBox.Size = new System.Drawing.Size(365, 28);
            this.FamilySizeIncreasedBox.TabIndex = 1;
            this.FamilySizeIncreasedBox.Visible = false;
            // 
            // SpouseExternalLoansBox
            // 
            this.SpouseExternalLoansBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SpouseExternalLoansBox.FormattingEnabled = true;
            this.SpouseExternalLoansBox.Location = new System.Drawing.Point(138, 197);
            this.SpouseExternalLoansBox.Name = "SpouseExternalLoansBox";
            this.SpouseExternalLoansBox.SelectedValue = null;
            this.SpouseExternalLoansBox.Size = new System.Drawing.Size(121, 28);
            this.SpouseExternalLoansBox.TabIndex = 6;
            this.SpouseExternalLoansBox.SelectedValueChanged += new System.EventHandler(this.SpouseExternalLoansBox_SelectedValueChanged);
            // 
            // SpouseExternalLoansLabel
            // 
            this.SpouseExternalLoansLabel.Location = new System.Drawing.Point(6, 187);
            this.SpouseExternalLoansLabel.Name = "SpouseExternalLoansLabel";
            this.SpouseExternalLoansLabel.Size = new System.Drawing.Size(126, 46);
            this.SpouseExternalLoansLabel.TabIndex = 10;
            this.SpouseExternalLoansLabel.Text = "Spouse has External Loans:";
            this.SpouseExternalLoansLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FilingStatusCbo
            // 
            this.FilingStatusCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilingStatusCbo.FormattingEnabled = true;
            this.FilingStatusCbo.Location = new System.Drawing.Point(138, 153);
            this.FilingStatusCbo.Name = "FilingStatusCbo";
            this.FilingStatusCbo.Size = new System.Drawing.Size(373, 28);
            this.FilingStatusCbo.TabIndex = 5;
            this.FilingStatusCbo.SelectedIndexChanged += new System.EventHandler(this.MaritalFilingSpouse_DataChanged);
            // 
            // MaritalStatusCbo
            // 
            this.MaritalStatusCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MaritalStatusCbo.FormattingEnabled = true;
            this.MaritalStatusCbo.Location = new System.Drawing.Point(138, 102);
            this.MaritalStatusCbo.Name = "MaritalStatusCbo";
            this.MaritalStatusCbo.Size = new System.Drawing.Size(280, 28);
            this.MaritalStatusCbo.TabIndex = 4;
            this.MaritalStatusCbo.SelectedIndexChanged += new System.EventHandler(this.MaritalStatusCbo_SelectedIndexChanged);
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
            this.SpouseGroup.Location = new System.Drawing.Point(542, 22);
            this.SpouseGroup.Name = "SpouseGroup";
            this.SpouseGroup.Size = new System.Drawing.Size(362, 269);
            this.SpouseGroup.TabIndex = 7;
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
            this.SpouseSsn.TextChanged += new System.EventHandler(this.SpouseSsn_TextChanged);
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
            this.FilingStatusLbl.Location = new System.Drawing.Point(6, 153);
            this.FilingStatusLbl.Name = "FilingStatusLbl";
            this.FilingStatusLbl.Size = new System.Drawing.Size(126, 20);
            this.FilingStatusLbl.TabIndex = 3;
            this.FilingStatusLbl.Text = "Filing Status:";
            this.FilingStatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MaritalStatusLbl
            // 
            this.MaritalStatusLbl.Location = new System.Drawing.Point(6, 105);
            this.MaritalStatusLbl.Name = "MaritalStatusLbl";
            this.MaritalStatusLbl.Size = new System.Drawing.Size(126, 20);
            this.MaritalStatusLbl.TabIndex = 2;
            this.MaritalStatusLbl.Text = "Marital Status:";
            this.MaritalStatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DependentsLabel
            // 
            this.DependentsLabel.Location = new System.Drawing.Point(6, 63);
            this.DependentsLabel.Name = "DependentsLabel";
            this.DependentsLabel.Size = new System.Drawing.Size(126, 20);
            this.DependentsLabel.TabIndex = 1;
            this.DependentsLabel.Text = "Dependents:";
            this.DependentsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ChildrenLabel
            // 
            this.ChildrenLabel.Location = new System.Drawing.Point(6, 22);
            this.ChildrenLabel.Name = "ChildrenLabel";
            this.ChildrenLabel.Size = new System.Drawing.Size(126, 20);
            this.ChildrenLabel.TabIndex = 0;
            this.ChildrenLabel.Text = "Children:";
            this.ChildrenLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Dependents
            // 
            this.Dependents.AllowedSpecialCharacters = "";
            this.Dependents.Location = new System.Drawing.Point(138, 60);
            this.Dependents.MaxLength = 2;
            this.Dependents.Name = "Dependents";
            this.Dependents.Size = new System.Drawing.Size(27, 26);
            this.Dependents.TabIndex = 2;
            this.Dependents.TextChanged += new System.EventHandler(this.MaritalFilingSpouse_DataChanged);
            // 
            // Children
            // 
            this.Children.AllowedSpecialCharacters = "";
            this.Children.Location = new System.Drawing.Point(138, 22);
            this.Children.MaxLength = 2;
            this.Children.Name = "Children";
            this.Children.Size = new System.Drawing.Size(27, 26);
            this.Children.TabIndex = 0;
            this.Children.TextChanged += new System.EventHandler(this.MaritalFilingSpouse_DataChanged);
            // 
            // IncomeTab
            // 
            this.IncomeTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.IncomeTab.Controls.Add(this.SpouseIncomeInformation);
            this.IncomeTab.Controls.Add(this.BorrowerIncomeInformation);
            this.IncomeTab.Location = new System.Drawing.Point(4, 29);
            this.IncomeTab.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.IncomeTab.Name = "IncomeTab";
            this.IncomeTab.Size = new System.Drawing.Size(958, 332);
            this.IncomeTab.TabIndex = 3;
            this.IncomeTab.Text = "Income";
            // 
            // SpouseIncomeInformation
            // 
            this.SpouseIncomeInformation.AgiFromTaxes = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.SpouseIncomeInformation.AgiReflectsCurrentIncome = null;
            this.SpouseIncomeInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SpouseIncomeInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpouseIncomeInformation.Location = new System.Drawing.Point(513, 12);
            this.SpouseIncomeInformation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SpouseIncomeInformation.Name = "SpouseIncomeInformation";
            this.SpouseIncomeInformation.ReceivedDate = null;
            this.SpouseIncomeInformation.Size = new System.Drawing.Size(403, 311);
            this.SpouseIncomeInformation.SpouseFilingStatusId = null;
            this.SpouseIncomeInformation.StateCode = "";
            this.SpouseIncomeInformation.SupportingDocsRequired = null;
            this.SpouseIncomeInformation.TabIndex = 3;
            this.SpouseIncomeInformation.TaxableIncome = null;
            this.SpouseIncomeInformation.TaxesFiled = null;
            this.SpouseIncomeInformation.TaxYear = null;
            // 
            // BorrowerIncomeInformation
            // 
            this.BorrowerIncomeInformation.AgiFromTaxes = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.BorrowerIncomeInformation.AgiReflectsCurrentIncome = null;
            this.BorrowerIncomeInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BorrowerIncomeInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BorrowerIncomeInformation.Location = new System.Drawing.Point(48, 12);
            this.BorrowerIncomeInformation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BorrowerIncomeInformation.Name = "BorrowerIncomeInformation";
            this.BorrowerIncomeInformation.ReceivedDate = null;
            this.BorrowerIncomeInformation.Size = new System.Drawing.Size(403, 311);
            this.BorrowerIncomeInformation.SpouseFilingStatusId = null;
            this.BorrowerIncomeInformation.StateCode = "";
            this.BorrowerIncomeInformation.SupportingDocsRequired = null;
            this.BorrowerIncomeInformation.TabIndex = 2;
            this.BorrowerIncomeInformation.TaxableIncome = null;
            this.BorrowerIncomeInformation.TaxesFiled = null;
            this.BorrowerIncomeInformation.TaxYear = null;
            // 
            // LoansTab
            // 
            this.LoansTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LoansTab.Controls.Add(this.SpouseInfoGroup);
            this.LoansTab.Controls.Add(this.panel4);
            this.LoansTab.Location = new System.Drawing.Point(4, 29);
            this.LoansTab.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.LoansTab.Name = "LoansTab";
            this.LoansTab.Size = new System.Drawing.Size(958, 332);
            this.LoansTab.TabIndex = 4;
            this.LoansTab.Text = "Loans";
            // 
            // SpouseInfoGroup
            // 
            this.SpouseInfoGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpouseInfoGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SpouseInfoGroup.Controls.Add(this.SpouseDisplayBtn);
            this.SpouseInfoGroup.Controls.Add(this.label29);
            this.SpouseInfoGroup.Location = new System.Drawing.Point(499, 12);
            this.SpouseInfoGroup.Name = "SpouseInfoGroup";
            this.SpouseInfoGroup.Size = new System.Drawing.Size(405, 304);
            this.SpouseInfoGroup.TabIndex = 3;
            // 
            // SpouseDisplayBtn
            // 
            this.SpouseDisplayBtn.Location = new System.Drawing.Point(139, 79);
            this.SpouseDisplayBtn.Name = "SpouseDisplayBtn";
            this.SpouseDisplayBtn.Size = new System.Drawing.Size(143, 37);
            this.SpouseDisplayBtn.TabIndex = 0;
            this.SpouseDisplayBtn.Text = "Display Loans";
            this.SpouseDisplayBtn.UseVisualStyleBackColor = true;
            this.SpouseDisplayBtn.Click += new System.EventHandler(this.SpouseDisplayBtn_Click);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(145, 4);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(107, 20);
            this.label29.TabIndex = 0;
            this.label29.Text = "Spouse\'s Info";
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.ExternalLoansBox);
            this.panel4.Controls.Add(this.ExternalLoansLabel);
            this.panel4.Controls.Add(this.BorDisplayBtn);
            this.panel4.Controls.Add(this.BorEligibilityCbo);
            this.panel4.Controls.Add(this.BorGradeLevelCbo);
            this.panel4.Controls.Add(this.BorEligibilityLbl);
            this.panel4.Controls.Add(this.BorGradeLevelLbl);
            this.panel4.Controls.Add(this.label36);
            this.panel4.Location = new System.Drawing.Point(50, 12);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(405, 304);
            this.panel4.TabIndex = 2;
            // 
            // ExternalLoansBox
            // 
            this.ExternalLoansBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ExternalLoansBox.FormattingEnabled = true;
            this.ExternalLoansBox.Location = new System.Drawing.Point(180, 130);
            this.ExternalLoansBox.Name = "ExternalLoansBox";
            this.ExternalLoansBox.SelectedValue = null;
            this.ExternalLoansBox.Size = new System.Drawing.Size(143, 28);
            this.ExternalLoansBox.TabIndex = 7;
            this.ExternalLoansBox.SelectedValueChanged += new System.EventHandler(this.ExternalLoansBox_SelectedValueChanged);
            // 
            // ExternalLoansLabel
            // 
            this.ExternalLoansLabel.AutoSize = true;
            this.ExternalLoansLabel.Location = new System.Drawing.Point(55, 133);
            this.ExternalLoansLabel.Name = "ExternalLoansLabel";
            this.ExternalLoansLabel.Size = new System.Drawing.Size(119, 20);
            this.ExternalLoansLabel.TabIndex = 6;
            this.ExternalLoansLabel.Text = "External Loans:";
            // 
            // BorDisplayBtn
            // 
            this.BorDisplayBtn.Location = new System.Drawing.Point(180, 168);
            this.BorDisplayBtn.Name = "BorDisplayBtn";
            this.BorDisplayBtn.Size = new System.Drawing.Size(143, 37);
            this.BorDisplayBtn.TabIndex = 3;
            this.BorDisplayBtn.Text = "Display Loans";
            this.BorDisplayBtn.UseVisualStyleBackColor = true;
            this.BorDisplayBtn.Visible = false;
            this.BorDisplayBtn.Click += new System.EventHandler(this.BorDisplayBtn_Click);
            // 
            // BorEligibilityCbo
            // 
            this.BorEligibilityCbo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.BorEligibilityCbo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.BorEligibilityCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BorEligibilityCbo.FormattingEnabled = true;
            this.BorEligibilityCbo.Location = new System.Drawing.Point(180, 93);
            this.BorEligibilityCbo.Name = "BorEligibilityCbo";
            this.BorEligibilityCbo.Size = new System.Drawing.Size(210, 28);
            this.BorEligibilityCbo.TabIndex = 1;
            this.BorEligibilityCbo.SelectionChangeCommitted += new System.EventHandler(this.BorEligibilityCbo_SelectionChangedCommitted);
            this.BorEligibilityCbo.Leave += new System.EventHandler(this.BorEligibilityCbo_Leave);
            this.BorEligibilityCbo.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.BorEligibilityCbo_PreviewKeyDown);
            // 
            // BorGradeLevelCbo
            // 
            this.BorGradeLevelCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BorGradeLevelCbo.FormattingEnabled = true;
            this.BorGradeLevelCbo.Items.AddRange(new object[] {
            "Undergraduate",
            "Graduate"});
            this.BorGradeLevelCbo.Location = new System.Drawing.Point(180, 56);
            this.BorGradeLevelCbo.Name = "BorGradeLevelCbo";
            this.BorGradeLevelCbo.Size = new System.Drawing.Size(160, 28);
            this.BorGradeLevelCbo.TabIndex = 0;
            // 
            // BorEligibilityLbl
            // 
            this.BorEligibilityLbl.AutoSize = true;
            this.BorEligibilityLbl.Location = new System.Drawing.Point(34, 96);
            this.BorEligibilityLbl.Name = "BorEligibilityLbl";
            this.BorEligibilityLbl.Size = new System.Drawing.Size(140, 20);
            this.BorEligibilityLbl.TabIndex = 5;
            this.BorEligibilityLbl.Text = "Borrower Eligibility:";
            // 
            // BorGradeLevelLbl
            // 
            this.BorGradeLevelLbl.AutoSize = true;
            this.BorGradeLevelLbl.Location = new System.Drawing.Point(75, 59);
            this.BorGradeLevelLbl.Name = "BorGradeLevelLbl";
            this.BorGradeLevelLbl.Size = new System.Drawing.Size(99, 20);
            this.BorGradeLevelLbl.TabIndex = 3;
            this.BorGradeLevelLbl.Text = "Grade Level:";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(143, 4);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(116, 20);
            this.label36.TabIndex = 0;
            this.label36.Text = "Borrower\'s Info";
            // 
            // DefTab
            // 
            this.DefTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DefTab.Controls.Add(this.RequestedRpfBox);
            this.DefTab.Controls.Add(this.DefForbCbo);
            this.DefTab.Controls.Add(this.RequestedRpfLabel);
            this.DefTab.Controls.Add(this.DefForbLbl);
            this.DefTab.Location = new System.Drawing.Point(4, 29);
            this.DefTab.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.DefTab.Name = "DefTab";
            this.DefTab.Size = new System.Drawing.Size(958, 332);
            this.DefTab.TabIndex = 5;
            this.DefTab.Text = "Def/Forb";
            // 
            // RequestedRpfBox
            // 
            this.RequestedRpfBox.AllowedSpecialCharacters = "";
            this.RequestedRpfBox.Location = new System.Drawing.Point(257, 73);
            this.RequestedRpfBox.Name = "RequestedRpfBox";
            this.RequestedRpfBox.Size = new System.Drawing.Size(240, 26);
            this.RequestedRpfBox.TabIndex = 2;
            this.RequestedRpfBox.Text = "0.00";
            this.RequestedRpfBox.TextChanged += new System.EventHandler(this.RequestRpfTxt_TextChanged);
            this.RequestedRpfBox.Enter += new System.EventHandler(this.RequestedRpfBox_Enter);
            // 
            // DefForbCbo
            // 
            this.DefForbCbo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DefForbCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DefForbCbo.FormattingEnabled = true;
            this.DefForbCbo.Location = new System.Drawing.Point(257, 25);
            this.DefForbCbo.Name = "DefForbCbo";
            this.DefForbCbo.Size = new System.Drawing.Size(677, 28);
            this.DefForbCbo.TabIndex = 0;
            // 
            // RequestedRpfLabel
            // 
            this.RequestedRpfLabel.AutoSize = true;
            this.RequestedRpfLabel.Location = new System.Drawing.Point(123, 76);
            this.RequestedRpfLabel.Name = "RequestedRpfLabel";
            this.RequestedRpfLabel.Size = new System.Drawing.Size(128, 20);
            this.RequestedRpfLabel.TabIndex = 1;
            this.RequestedRpfLabel.Text = "Requested RPF:";
            // 
            // DefForbLbl
            // 
            this.DefForbLbl.AutoSize = true;
            this.DefForbLbl.Location = new System.Drawing.Point(42, 28);
            this.DefForbLbl.Name = "DefForbLbl";
            this.DefForbLbl.Size = new System.Drawing.Size(209, 20);
            this.DefForbLbl.TabIndex = 0;
            this.DefForbLbl.Text = "On Deferment/Forbearance:";
            // 
            // StatusTab
            // 
            this.StatusTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StatusTab.Controls.Add(this.AppSubstatusCbo);
            this.StatusTab.Controls.Add(this.AppSubstatusLbl);
            this.StatusTab.Controls.Add(this.AppStatusCbo);
            this.StatusTab.Controls.Add(this.AppStatusLbl);
            this.StatusTab.Controls.Add(this.StatusRequestedPlanLbl);
            this.StatusTab.Controls.Add(this.ProcessedBox);
            this.StatusTab.Controls.Add(this.label32);
            this.StatusTab.Controls.Add(this.StatusProcessedLbl);
            this.StatusTab.Controls.Add(this.SpouseSigned);
            this.StatusTab.Controls.Add(this.BorrowerSigned);
            this.StatusTab.Controls.Add(this.PlansContainer);
            this.StatusTab.Location = new System.Drawing.Point(4, 29);
            this.StatusTab.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.StatusTab.Name = "StatusTab";
            this.StatusTab.Size = new System.Drawing.Size(958, 332);
            this.StatusTab.TabIndex = 6;
            this.StatusTab.Text = "App Status";
            // 
            // AppSubstatusCbo
            // 
            this.AppSubstatusCbo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AppSubstatusCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AppSubstatusCbo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AppSubstatusCbo.FormattingEnabled = true;
            this.AppSubstatusCbo.Location = new System.Drawing.Point(807, 3);
            this.AppSubstatusCbo.Name = "AppSubstatusCbo";
            this.AppSubstatusCbo.Size = new System.Drawing.Size(146, 28);
            this.AppSubstatusCbo.TabIndex = 2;
            this.AppSubstatusCbo.Enter += new System.EventHandler(this.AppSubstatusCbo_Enter);
            this.AppSubstatusCbo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AppSubstatusCbo_MouseDown);
            this.AppSubstatusCbo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AppSubstatusCbo_MouseUp);
            // 
            // AppSubstatusLbl
            // 
            this.AppSubstatusLbl.AutoSize = true;
            this.AppSubstatusLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AppSubstatusLbl.Location = new System.Drawing.Point(691, 6);
            this.AppSubstatusLbl.Name = "AppSubstatusLbl";
            this.AppSubstatusLbl.Size = new System.Drawing.Size(119, 20);
            this.AppSubstatusLbl.TabIndex = 8;
            this.AppSubstatusLbl.Text = "App Substatus:";
            // 
            // AppStatusCbo
            // 
            this.AppStatusCbo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AppStatusCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AppStatusCbo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AppStatusCbo.FormattingEnabled = true;
            this.AppStatusCbo.Location = new System.Drawing.Point(590, 4);
            this.AppStatusCbo.Name = "AppStatusCbo";
            this.AppStatusCbo.Size = new System.Drawing.Size(100, 28);
            this.AppStatusCbo.TabIndex = 1;
            this.AppStatusCbo.SelectedIndexChanged += new System.EventHandler(this.AppStatus_SelectedIndexChanged);
            // 
            // AppStatusLbl
            // 
            this.AppStatusLbl.AutoSize = true;
            this.AppStatusLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AppStatusLbl.Location = new System.Drawing.Point(497, 8);
            this.AppStatusLbl.Name = "AppStatusLbl";
            this.AppStatusLbl.Size = new System.Drawing.Size(93, 20);
            this.AppStatusLbl.TabIndex = 7;
            this.AppStatusLbl.Text = "App Status:";
            // 
            // StatusRequestedPlanLbl
            // 
            this.StatusRequestedPlanLbl.AutoSize = true;
            this.StatusRequestedPlanLbl.Location = new System.Drawing.Point(272, 7);
            this.StatusRequestedPlanLbl.Name = "StatusRequestedPlanLbl";
            this.StatusRequestedPlanLbl.Size = new System.Drawing.Size(0, 20);
            this.StatusRequestedPlanLbl.TabIndex = 14;
            // 
            // ProcessedBox
            // 
            this.ProcessedBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProcessedBox.FormattingEnabled = true;
            this.ProcessedBox.Location = new System.Drawing.Point(90, 3);
            this.ProcessedBox.Name = "ProcessedBox";
            this.ProcessedBox.Size = new System.Drawing.Size(80, 28);
            this.ProcessedBox.TabIndex = 0;
            this.ProcessedBox.SelectedIndexChanged += new System.EventHandler(this.ProcessedBox_SelectedIndexChanged);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(181, 7);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(92, 20);
            this.label32.TabIndex = 13;
            this.label32.Text = "Requested:";
            // 
            // StatusProcessedLbl
            // 
            this.StatusProcessedLbl.AutoSize = true;
            this.StatusProcessedLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusProcessedLbl.Location = new System.Drawing.Point(4, 7);
            this.StatusProcessedLbl.Name = "StatusProcessedLbl";
            this.StatusProcessedLbl.Size = new System.Drawing.Size(88, 20);
            this.StatusProcessedLbl.TabIndex = 11;
            this.StatusProcessedLbl.Text = "Processed:";
            // 
            // SpouseSigned
            // 
            this.SpouseSigned.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SpouseSigned.AutoSize = true;
            this.SpouseSigned.Location = new System.Drawing.Point(160, 294);
            this.SpouseSigned.Name = "SpouseSigned";
            this.SpouseSigned.Size = new System.Drawing.Size(137, 24);
            this.SpouseSigned.TabIndex = 5;
            this.SpouseSigned.Text = "Spouse Signed";
            this.SpouseSigned.UseVisualStyleBackColor = true;
            this.SpouseSigned.Visible = false;
            // 
            // BorrowerSigned
            // 
            this.BorrowerSigned.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BorrowerSigned.AutoSize = true;
            this.BorrowerSigned.Location = new System.Drawing.Point(8, 294);
            this.BorrowerSigned.Name = "BorrowerSigned";
            this.BorrowerSigned.Size = new System.Drawing.Size(146, 24);
            this.BorrowerSigned.TabIndex = 4;
            this.BorrowerSigned.Text = "Borrower Signed";
            this.BorrowerSigned.UseVisualStyleBackColor = true;
            // 
            // PlansContainer
            // 
            this.PlansContainer.Controls.Add(this.Ibr2014Plan);
            this.PlansContainer.Controls.Add(this.IbrPlan);
            this.PlansContainer.Location = new System.Drawing.Point(-1, 43);
            this.PlansContainer.Name = "PlansContainer";
            this.PlansContainer.Size = new System.Drawing.Size(957, 251);
            this.PlansContainer.TabIndex = 3;
            // 
            // Ibr2014Plan
            // 
            this.Ibr2014Plan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Ibr2014Plan.Enabled = false;
            this.Ibr2014Plan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ibr2014Plan.Location = new System.Drawing.Point(192, 0);
            this.Ibr2014Plan.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Ibr2014Plan.Name = "Ibr2014Plan";
            this.Ibr2014Plan.PlanTitle = "IBR 2014";
            this.Ibr2014Plan.Size = new System.Drawing.Size(191, 242);
            this.Ibr2014Plan.TabIndex = 1;
            this.Ibr2014Plan.TabStop = false;
            this.Ibr2014Plan.PlanSelected += new IDRUSERPRO.RepaymentPlan.OnPlanSelected(this.RepaymentPlan_PlanSelected);
            // 
            // IbrPlan
            // 
            this.IbrPlan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.IbrPlan.Enabled = false;
            this.IbrPlan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IbrPlan.Location = new System.Drawing.Point(4, 0);
            this.IbrPlan.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.IbrPlan.Name = "IbrPlan";
            this.IbrPlan.PlanTitle = "IBR";
            this.IbrPlan.Size = new System.Drawing.Size(186, 242);
            this.IbrPlan.TabIndex = 0;
            this.IbrPlan.TabStop = false;
            this.IbrPlan.PlanSelected += new IDRUSERPRO.RepaymentPlan.OnPlanSelected(this.RepaymentPlan_PlanSelected);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(12, 374);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(96, 34);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Validation
            // 
            this.Validation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Validation.Location = new System.Drawing.Point(675, 374);
            this.Validation.Name = "Validation";
            this.Validation.Size = new System.Drawing.Size(135, 34);
            this.Validation.TabIndex = 2;
            this.Validation.Text = "Run Validation";
            this.Validation.UseVisualStyleBackColor = true;
            this.Validation.Click += new System.EventHandler(this.Validation_Click);
            // 
            // Complete
            // 
            this.Complete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Complete.Enabled = false;
            this.Complete.Location = new System.Drawing.Point(858, 374);
            this.Complete.Name = "Complete";
            this.Complete.Size = new System.Drawing.Size(96, 34);
            this.Complete.TabIndex = 3;
            this.Complete.Text = "Complete";
            this.Complete.UseVisualStyleBackColor = true;
            this.Complete.Click += new System.EventHandler(this.Complete_Click);
            // 
            // NextStep
            // 
            this.NextStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NextStep.Location = new System.Drawing.Point(519, 374);
            this.NextStep.Name = "NextStep";
            this.NextStep.Size = new System.Drawing.Size(105, 34);
            this.NextStep.TabIndex = 1;
            this.NextStep.Text = "Next Step";
            this.NextStep.UseVisualStyleBackColor = true;
            this.NextStep.Click += new System.EventHandler(this.NextStep_Click);
            // 
            // ApplicationEntry
            // 
            this.AcceptButton = this.Complete;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(966, 420);
            this.Controls.Add(this.NextStep);
            this.Controls.Add(this.Complete);
            this.Controls.Add(this.Validation);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.tabs);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(982, 458);
            this.Name = "ApplicationEntry";
            this.Text = "Application Entry";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ApplicationEntry_KeyDown);
            this.tabs.ResumeLayout(false);
            this.ApplicationTab.ResumeLayout(false);
            this.PlanTab.ResumeLayout(false);
            this.PlanTab.PerformLayout();
            this.HouseholdTab.ResumeLayout(false);
            this.HouseholdTab.PerformLayout();
            this.SpouseGroup.ResumeLayout(false);
            this.SpouseGroup.PerformLayout();
            this.IncomeTab.ResumeLayout(false);
            this.LoansTab.ResumeLayout(false);
            this.SpouseInfoGroup.ResumeLayout(false);
            this.SpouseInfoGroup.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.DefTab.ResumeLayout(false);
            this.DefTab.PerformLayout();
            this.StatusTab.ResumeLayout(false);
            this.StatusTab.PerformLayout();
            this.PlansContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage ApplicationTab;
        private System.Windows.Forms.TabPage PlanTab;
        private System.Windows.Forms.TabPage HouseholdTab;
        private System.Windows.Forms.TabPage IncomeTab;
        private System.Windows.Forms.TabPage LoansTab;
        private System.Windows.Forms.TabPage DefTab;
        private System.Windows.Forms.TabPage StatusTab;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Validation;
        private System.Windows.Forms.Button Complete;
        private System.Windows.Forms.Button NextStep;
        private System.Windows.Forms.ComboBox RequestReasonCbo;
        private System.Windows.Forms.Label RequestReasonLbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox PlanDueDate;
        private System.Windows.Forms.Label SpouseLastNameLabel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label SpouseFirstNameLabel;
        private System.Windows.Forms.Label SpouseDobLabel;
        private System.Windows.Forms.Label SpouseSsnLabel;
        private System.Windows.Forms.Label FilingStatusLbl;
        private System.Windows.Forms.Label MaritalStatusLbl;
        private System.Windows.Forms.Label DependentsLabel;
        private System.Windows.Forms.Label ChildrenLabel;
        private System.Windows.Forms.ComboBox FilingStatusCbo;
        private System.Windows.Forms.ComboBox MaritalStatusCbo;
        private Uheaa.Common.WinForms.NumericTextBox Dependents;
        private Uheaa.Common.WinForms.NumericTextBox Children;
        private System.Windows.Forms.GroupBox SpouseGroup;
        private Uheaa.Common.WinForms.SsnTextBox SpouseSsn;
        private System.Windows.Forms.Panel SpouseInfoGroup;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox BorEligibilityCbo;
        private System.Windows.Forms.ComboBox BorGradeLevelCbo;
        private System.Windows.Forms.Label BorEligibilityLbl;
        private System.Windows.Forms.Label BorGradeLevelLbl;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.ComboBox DefForbCbo;
        private System.Windows.Forms.Label RequestedRpfLabel;
        private System.Windows.Forms.Label DefForbLbl;
        private System.Windows.Forms.MaskedTextBox SpouseDOB;
        private System.Windows.Forms.Button SpouseDisplayBtn;
        private System.Windows.Forms.Button BorDisplayBtn;
        private System.Windows.Forms.Label StatusProcessedLbl;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.ComboBox AppStatusCbo;
        private System.Windows.Forms.Label AppStatusLbl;
        private System.Windows.Forms.ComboBox AppSubstatusCbo;
        private System.Windows.Forms.Label AppSubstatusLbl;
        private System.Windows.Forms.Label StatusRequestedPlanLbl;
        private Uheaa.Common.WinForms.OmniTextBox SpouseLastName;
        private Uheaa.Common.WinForms.OmniTextBox SpouseMiddleName;
        private Uheaa.Common.WinForms.OmniTextBox SpouseFirstName;
        private System.Windows.Forms.Label SpouseExternalLoansLabel;
        private Uheaa.Common.WinForms.YesNoComboBox SpouseExternalLoansBox;
        private IncomeInformation BorrowerIncomeInformation;
        private IncomeInformation SpouseIncomeInformation;
        private System.Windows.Forms.ComboBox ProcessedBox;
        private ApplicationInformation ApplicationInformationControl;
        private System.Windows.Forms.CheckBox SpouseSigned;
        private System.Windows.Forms.CheckBox BorrowerSigned;
        private System.Windows.Forms.Panel PlansContainer;
        private RepaymentPlan Ibr2014Plan;
        private RepaymentPlan IbrPlan;
        private Uheaa.Common.WinForms.NumericDecimalTextBox RequestedRpfBox;
        private Uheaa.Common.WinForms.YesNoComboBox ExternalLoansBox;
        private System.Windows.Forms.Label ExternalLoansLabel;
        private System.Windows.Forms.ComboBox FamilySizeIncreasedBox;
    }
}