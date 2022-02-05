using Uheaa.Common.ProcessLogger;
using SubSystemShared;

namespace INCIDENTRP
{
    partial class ThreatDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThreatDisplay));
            this.dtpIncidentTime = new System.Windows.Forms.DateTimePicker();
            this.ticketBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dtpIncidentDate = new System.Windows.Forms.DateTimePicker();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblHistory = new System.Windows.Forms.Label();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.txtUpdate = new System.Windows.Forms.TextBox();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.lblCourtDate = new System.Windows.Forms.Label();
            this.lblCourt = new System.Windows.Forms.Label();
            this.lblStatusDate = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblIncidentDate = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.lblTicket = new System.Windows.Forms.Label();
            this.pbxCompanyLogo = new System.Windows.Forms.PictureBox();
            this.cmbCourt = new System.Windows.Forms.ComboBox();
            this.lnkUpdate = new System.Windows.Forms.LinkLabel();
            this.lnkNextFlowStep = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPriority = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblIncidentTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.GroupBox7 = new System.Windows.Forms.GroupBox();
            this.Label23 = new System.Windows.Forms.Label();
            this.TextBox13 = new System.Windows.Forms.TextBox();
            this.bombInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Label24 = new System.Windows.Forms.Label();
            this.TextBox14 = new System.Windows.Forms.TextBox();
            this.Label20 = new System.Windows.Forms.Label();
            this.TextBox10 = new System.Windows.Forms.TextBox();
            this.Label21 = new System.Windows.Forms.Label();
            this.TextBox11 = new System.Windows.Forms.TextBox();
            this.Label22 = new System.Windows.Forms.Label();
            this.TextBox12 = new System.Windows.Forms.TextBox();
            this.GroupBox6 = new System.Windows.Forms.GroupBox();
            this.Label19 = new System.Windows.Forms.Label();
            this.TextBox9 = new System.Windows.Forms.TextBox();
            this.threatInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Label18 = new System.Windows.Forms.Label();
            this.TextBox8 = new System.Windows.Forms.TextBox();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.grpLawEnforcementContactInfo = new System.Windows.Forms.GroupBox();
            this.Label15 = new System.Windows.Forms.Label();
            this.Label16 = new System.Windows.Forms.Label();
            this.TextBox7 = new System.Windows.Forms.TextBox();
            this.lawActionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dateTimePicker7 = new System.Windows.Forms.DateTimePicker();
            this.ContactLawDate = new System.Windows.Forms.DateTimePicker();
            this.Label17 = new System.Windows.Forms.Label();
            this.chkContactedLawEnforcement = new System.Windows.Forms.CheckBox();
            this.grpInformationSecurityContactInfo = new System.Windows.Forms.GroupBox();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label13 = new System.Windows.Forms.Label();
            this.TextBox6 = new System.Windows.Forms.TextBox();
            this.infoTechActionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ContactITTime = new System.Windows.Forms.DateTimePicker();
            this.ContactITDate = new System.Windows.Forms.DateTimePicker();
            this.Label14 = new System.Windows.Forms.Label();
            this.chkContactedInformationSecurity = new System.Windows.Forms.CheckBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.lblOtherType = new System.Windows.Forms.Label();
            this.txtOtherType = new System.Windows.Forms.TextBox();
            this.notifierBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblOtherMethod = new System.Windows.Forms.Label();
            this.txtOtherMethod = new System.Windows.Forms.TextBox();
            this.Label11 = new System.Windows.Forms.Label();
            this.TextBox5 = new System.Windows.Forms.TextBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.TextBox4 = new System.Windows.Forms.TextBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.TextBox3 = new System.Windows.Forms.TextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.cmbNotifierType = new System.Windows.Forms.ComboBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.cmbNotificationMethod = new System.Windows.Forms.ComboBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.reporterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label27 = new System.Windows.Forms.Label();
            this.txtReporterEmailAddress = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.cmbBusinessUnit = new System.Windows.Forms.ComboBox();
            this.businessUnitBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label29 = new System.Windows.Forms.Label();
            this.cmbReporter = new System.Windows.Forms.ComboBox();
            this.reporterNameBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.GroupBox8 = new System.Windows.Forms.GroupBox();
            this.label34 = new System.Windows.Forms.Label();
            this.textBox21 = new System.Windows.Forms.TextBox();
            this.callerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Label32 = new System.Windows.Forms.Label();
            this.TextBox18 = new System.Windows.Forms.TextBox();
            this.Label33 = new System.Windows.Forms.Label();
            this.TextBox20 = new System.Windows.Forms.TextBox();
            this.chkGenderUnsure = new System.Windows.Forms.CheckBox();
            this.chkGenderFemale = new System.Windows.Forms.CheckBox();
            this.chkGenderMale = new System.Windows.Forms.CheckBox();
            this.chkFamiliarNo = new System.Windows.Forms.CheckBox();
            this.chkFamiliarYes = new System.Windows.Forms.CheckBox();
            this.chkRecognizedVoiceNo = new System.Windows.Forms.CheckBox();
            this.chkRecognizedVoiceYes = new System.Windows.Forms.CheckBox();
            this.Label31 = new System.Windows.Forms.Label();
            this.Label30 = new System.Windows.Forms.Label();
            this.Label25 = new System.Windows.Forms.Label();
            this.TextBox15 = new System.Windows.Forms.TextBox();
            this.Label26 = new System.Windows.Forms.Label();
            this.TextBox16 = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.textBox19 = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.textBox22 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtVoiceOther = new System.Windows.Forms.TextBox();
            this.voiceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.chkVoiceOther = new System.Windows.Forms.CheckBox();
            this.CheckBox9 = new System.Windows.Forms.CheckBox();
            this.CheckBox10 = new System.Windows.Forms.CheckBox();
            this.CheckBox11 = new System.Windows.Forms.CheckBox();
            this.CheckBox12 = new System.Windows.Forms.CheckBox();
            this.CheckBox4 = new System.Windows.Forms.CheckBox();
            this.CheckBox5 = new System.Windows.Forms.CheckBox();
            this.CheckBox6 = new System.Windows.Forms.CheckBox();
            this.CheckBox3 = new System.Windows.Forms.CheckBox();
            this.CheckBox2 = new System.Windows.Forms.CheckBox();
            this.CheckBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.CheckBox32 = new System.Windows.Forms.CheckBox();
            this.mannerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CheckBox33 = new System.Windows.Forms.CheckBox();
            this.CheckBox31 = new System.Windows.Forms.CheckBox();
            this.CheckBox30 = new System.Windows.Forms.CheckBox();
            this.txtMannerOther = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkMannerOther = new System.Windows.Forms.CheckBox();
            this.CheckBox19 = new System.Windows.Forms.CheckBox();
            this.CheckBox20 = new System.Windows.Forms.CheckBox();
            this.CheckBox21 = new System.Windows.Forms.CheckBox();
            this.CheckBox22 = new System.Windows.Forms.CheckBox();
            this.CheckBox23 = new System.Windows.Forms.CheckBox();
            this.CheckBox24 = new System.Windows.Forms.CheckBox();
            this.CheckBox25 = new System.Windows.Forms.CheckBox();
            this.CheckBox27 = new System.Windows.Forms.CheckBox();
            this.CheckBox28 = new System.Windows.Forms.CheckBox();
            this.CheckBox29 = new System.Windows.Forms.CheckBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.txtDialectRegionalAmericanOther = new System.Windows.Forms.TextBox();
            this.dialectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label38 = new System.Windows.Forms.Label();
            this.chkDialectRegionalAmericanOther = new System.Windows.Forms.CheckBox();
            this.txtDialectForeignAccentOther = new System.Windows.Forms.TextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.chkDialectForeignAccentOther = new System.Windows.Forms.CheckBox();
            this.CheckBox26 = new System.Windows.Forms.CheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.txtLanguageOther = new System.Windows.Forms.TextBox();
            this.languageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label40 = new System.Windows.Forms.Label();
            this.chkLanguageOther = new System.Windows.Forms.CheckBox();
            this.CheckBox13 = new System.Windows.Forms.CheckBox();
            this.CheckBox14 = new System.Windows.Forms.CheckBox();
            this.CheckBox15 = new System.Windows.Forms.CheckBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.CheckBox35 = new System.Windows.Forms.CheckBox();
            this.backgroundNoiseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CheckBox36 = new System.Windows.Forms.CheckBox();
            this.CheckBox37 = new System.Windows.Forms.CheckBox();
            this.txtBackgroundNoiseOther = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.chkBackgroundNoiseOther = new System.Windows.Forms.CheckBox();
            this.CheckBox39 = new System.Windows.Forms.CheckBox();
            this.CheckBox40 = new System.Windows.Forms.CheckBox();
            this.CheckBox41 = new System.Windows.Forms.CheckBox();
            this.CheckBox43 = new System.Windows.Forms.CheckBox();
            this.CheckBox44 = new System.Windows.Forms.CheckBox();
            this.CheckBox45 = new System.Windows.Forms.CheckBox();
            this.CheckBox46 = new System.Windows.Forms.CheckBox();
            this.CheckBox47 = new System.Windows.Forms.CheckBox();
            this.CheckBox48 = new System.Windows.Forms.CheckBox();
            this.label42 = new System.Windows.Forms.Label();
            this.threatBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.textBox23 = new System.Windows.Forms.TextBox();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ticketBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCompanyLogo)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.GroupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bombInfoBindingSource)).BeginInit();
            this.GroupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threatInfoBindingSource)).BeginInit();
            this.GroupBox3.SuspendLayout();
            this.grpLawEnforcementContactInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lawActionBindingSource)).BeginInit();
            this.grpInformationSecurityContactInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoTechActionBindingSource)).BeginInit();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.notifierBindingSource)).BeginInit();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reporterBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessUnitBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporterNameBindingSource)).BeginInit();
            this.GroupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.callerBindingSource)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.voiceBindingSource)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mannerBindingSource)).BeginInit();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dialectBindingSource)).BeginInit();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.languageBindingSource)).BeginInit();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundNoiseBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threatBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpIncidentTime
            // 
            this.dtpIncidentTime.CustomFormat = "hh:mm tt";
            this.dtpIncidentTime.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketBindingSource, "IncidentDateTime", true));
            this.dtpIncidentTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpIncidentTime.Location = new System.Drawing.Point(683, 150);
            this.dtpIncidentTime.Name = "dtpIncidentTime";
            this.dtpIncidentTime.ShowUpDown = true;
            this.dtpIncidentTime.Size = new System.Drawing.Size(185, 20);
            this.dtpIncidentTime.TabIndex = 277;
            // 
            // ticketBindingSource
            // 
            this.ticketBindingSource.DataSource = typeof(INCIDENTRP.Ticket);
            // 
            // dtpIncidentDate
            // 
            this.dtpIncidentDate.CustomFormat = "MM/dd/yyyy";
            this.dtpIncidentDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketBindingSource, "IncidentDateTime", true));
            this.dtpIncidentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpIncidentDate.Location = new System.Drawing.Point(683, 119);
            this.dtpIncidentDate.Name = "dtpIncidentDate";
            this.dtpIncidentDate.Size = new System.Drawing.Size(185, 20);
            this.dtpIncidentDate.TabIndex = 276;
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketBindingSource, "Number", true));
            this.textBox1.Location = new System.Drawing.Point(1042, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 253;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.CausesValidation = false;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1193, 10);
            this.label2.TabIndex = 275;
            // 
            // lblHistory
            // 
            this.lblHistory.AutoSize = true;
            this.lblHistory.BackColor = System.Drawing.Color.Transparent;
            this.lblHistory.ForeColor = System.Drawing.Color.White;
            this.lblHistory.Location = new System.Drawing.Point(832, 329);
            this.lblHistory.Name = "lblHistory";
            this.lblHistory.Size = new System.Drawing.Size(42, 13);
            this.lblHistory.TabIndex = 273;
            this.lblHistory.Text = "History:";
            // 
            // lblUpdate
            // 
            this.lblUpdate.AutoSize = true;
            this.lblUpdate.BackColor = System.Drawing.Color.Transparent;
            this.lblUpdate.ForeColor = System.Drawing.Color.White;
            this.lblUpdate.Location = new System.Drawing.Point(832, 185);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(45, 13);
            this.lblUpdate.TabIndex = 270;
            this.lblUpdate.Text = "Update:";
            // 
            // txtUpdate
            // 
            this.txtUpdate.CausesValidation = false;
            this.txtUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdate.Location = new System.Drawing.Point(832, 201);
            this.txtUpdate.Multiline = true;
            this.txtUpdate.Name = "txtUpdate";
            this.txtUpdate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUpdate.Size = new System.Drawing.Size(340, 125);
            this.txtUpdate.TabIndex = 271;
            // 
            // txtHistory
            // 
            this.txtHistory.CausesValidation = false;
            this.txtHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHistory.Location = new System.Drawing.Point(832, 345);
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ReadOnly = true;
            this.txtHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHistory.Size = new System.Drawing.Size(340, 259);
            this.txtHistory.TabIndex = 272;
            // 
            // lblCourtDate
            // 
            this.lblCourtDate.AutoSize = true;
            this.lblCourtDate.BackColor = System.Drawing.Color.Transparent;
            this.lblCourtDate.ForeColor = System.Drawing.Color.White;
            this.lblCourtDate.Location = new System.Drawing.Point(318, 152);
            this.lblCourtDate.Name = "lblCourtDate";
            this.lblCourtDate.Size = new System.Drawing.Size(61, 13);
            this.lblCourtDate.TabIndex = 255;
            this.lblCourtDate.Text = "Court Date:";
            // 
            // lblCourt
            // 
            this.lblCourt.AutoSize = true;
            this.lblCourt.BackColor = System.Drawing.Color.Transparent;
            this.lblCourt.ForeColor = System.Drawing.Color.White;
            this.lblCourt.Location = new System.Drawing.Point(12, 152);
            this.lblCourt.Name = "lblCourt";
            this.lblCourt.Size = new System.Drawing.Size(35, 13);
            this.lblCourt.TabIndex = 256;
            this.lblCourt.Text = "Court:";
            // 
            // lblStatusDate
            // 
            this.lblStatusDate.AutoSize = true;
            this.lblStatusDate.BackColor = System.Drawing.Color.Transparent;
            this.lblStatusDate.ForeColor = System.Drawing.Color.White;
            this.lblStatusDate.Location = new System.Drawing.Point(313, 122);
            this.lblStatusDate.Name = "lblStatusDate";
            this.lblStatusDate.Size = new System.Drawing.Size(66, 13);
            this.lblStatusDate.TabIndex = 257;
            this.lblStatusDate.Text = "Status Date:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(7, 122);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 258;
            this.lblStatus.Text = "Status:";
            // 
            // lblIncidentDate
            // 
            this.lblIncidentDate.AutoSize = true;
            this.lblIncidentDate.BackColor = System.Drawing.Color.Transparent;
            this.lblIncidentDate.ForeColor = System.Drawing.Color.White;
            this.lblIncidentDate.Location = new System.Drawing.Point(603, 122);
            this.lblIncidentDate.Name = "lblIncidentDate";
            this.lblIncidentDate.Size = new System.Drawing.Size(74, 13);
            this.lblIncidentDate.TabIndex = 268;
            this.lblIncidentDate.Text = "Incident Date:";
            // 
            // txtStatus
            // 
            this.txtStatus.CausesValidation = false;
            this.txtStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketBindingSource, "Status", true));
            this.txtStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatus.Location = new System.Drawing.Point(56, 118);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(240, 20);
            this.txtStatus.TabIndex = 263;
            // 
            // lblTicket
            // 
            this.lblTicket.AutoSize = true;
            this.lblTicket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblTicket.Location = new System.Drawing.Point(999, 26);
            this.lblTicket.Name = "lblTicket";
            this.lblTicket.Size = new System.Drawing.Size(40, 13);
            this.lblTicket.TabIndex = 254;
            this.lblTicket.Text = "Ticket:";
            // 
            // pbxCompanyLogo
            // 
            this.pbxCompanyLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbxCompanyLogo.Image = global::INCIDENTRP.Properties.Resources.UheaaLogo;
            this.pbxCompanyLogo.Location = new System.Drawing.Point(22, 12);
            this.pbxCompanyLogo.Name = "pbxCompanyLogo";
            this.pbxCompanyLogo.Size = new System.Drawing.Size(145, 68);
            this.pbxCompanyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxCompanyLogo.TabIndex = 250;
            this.pbxCompanyLogo.TabStop = false;
            // 
            // cmbCourt
            // 
            this.cmbCourt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCourt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCourt.FormattingEnabled = true;
            this.cmbCourt.Location = new System.Drawing.Point(56, 149);
            this.cmbCourt.Name = "cmbCourt";
            this.cmbCourt.Size = new System.Drawing.Size(240, 21);
            this.cmbCourt.TabIndex = 265;
            this.cmbCourt.SelectedIndexChanged += new System.EventHandler(this.CmbCourt_SelectedIndexChanged);
            // 
            // lnkUpdate
            // 
            this.lnkUpdate.AutoSize = true;
            this.lnkUpdate.LinkColor = System.Drawing.Color.Yellow;
            this.lnkUpdate.Location = new System.Drawing.Point(867, 54);
            this.lnkUpdate.Name = "lnkUpdate";
            this.lnkUpdate.Size = new System.Drawing.Size(42, 13);
            this.lnkUpdate.TabIndex = 243;
            this.lnkUpdate.TabStop = true;
            this.lnkUpdate.Text = "Update";
            this.lnkUpdate.Click += new System.EventHandler(this.LnkUpdate_Click);
            // 
            // lnkNextFlowStep
            // 
            this.lnkNextFlowStep.AutoSize = true;
            this.lnkNextFlowStep.LinkColor = System.Drawing.Color.Yellow;
            this.lnkNextFlowStep.Location = new System.Drawing.Point(915, 54);
            this.lnkNextFlowStep.Name = "lnkNextFlowStep";
            this.lnkNextFlowStep.Size = new System.Drawing.Size(64, 13);
            this.lnkNextFlowStep.TabIndex = 242;
            this.lnkNextFlowStep.TabStop = true;
            this.lnkNextFlowStep.Text = "Move Along";
            this.lnkNextFlowStep.Visible = false;
            this.lnkNextFlowStep.Click += new System.EventHandler(this.LnkNextFlowStep_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.label3.Location = new System.Drawing.Point(1001, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 237;
            this.label3.Text = "Priority";
            // 
            // lblPriority
            // 
            this.lblPriority.AutoSize = true;
            this.lblPriority.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketBindingSource, "Priority", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblPriority.Font = new System.Drawing.Font("Century Schoolbook", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriority.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblPriority.Location = new System.Drawing.Point(989, 97);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.Size = new System.Drawing.Size(69, 77);
            this.lblPriority.TabIndex = 252;
            this.lblPriority.Text = "9";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Century Schoolbook", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblTitle.Location = new System.Drawing.Point(222, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(177, 57);
            this.lblTitle.TabIndex = 251;
            this.lblTitle.Text = "Threat";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblIncidentTime
            // 
            this.lblIncidentTime.AutoSize = true;
            this.lblIncidentTime.BackColor = System.Drawing.Color.Transparent;
            this.lblIncidentTime.ForeColor = System.Drawing.Color.White;
            this.lblIncidentTime.Location = new System.Drawing.Point(603, 152);
            this.lblIncidentTime.Name = "lblIncidentTime";
            this.lblIncidentTime.Size = new System.Drawing.Size(74, 13);
            this.lblIncidentTime.TabIndex = 267;
            this.lblIncidentTime.Text = "Incident Time:";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.CausesValidation = false;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1193, 10);
            this.label1.TabIndex = 278;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(10, 187);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(816, 570);
            this.tabControl1.TabIndex = 279;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.tabPage1.Controls.Add(this.GroupBox7);
            this.tabPage1.Controls.Add(this.GroupBox6);
            this.tabPage1.Controls.Add(this.GroupBox3);
            this.tabPage1.Controls.Add(this.GroupBox2);
            this.tabPage1.Controls.Add(this.GroupBox1);
            this.tabPage1.Controls.Add(this.GroupBox8);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(808, 544);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Call Information";
            // 
            // GroupBox7
            // 
            this.GroupBox7.Controls.Add(this.Label23);
            this.GroupBox7.Controls.Add(this.TextBox13);
            this.GroupBox7.Controls.Add(this.Label24);
            this.GroupBox7.Controls.Add(this.TextBox14);
            this.GroupBox7.Controls.Add(this.Label20);
            this.GroupBox7.Controls.Add(this.TextBox10);
            this.GroupBox7.Controls.Add(this.Label21);
            this.GroupBox7.Controls.Add(this.TextBox11);
            this.GroupBox7.Controls.Add(this.Label22);
            this.GroupBox7.Controls.Add(this.TextBox12);
            this.GroupBox7.ForeColor = System.Drawing.Color.White;
            this.GroupBox7.Location = new System.Drawing.Point(409, 134);
            this.GroupBox7.Name = "GroupBox7";
            this.GroupBox7.Size = new System.Drawing.Size(390, 135);
            this.GroupBox7.TabIndex = 20;
            this.GroupBox7.TabStop = false;
            this.GroupBox7.Text = "Bomb Threat Information";
            // 
            // Label23
            // 
            this.Label23.AutoSize = true;
            this.Label23.Location = new System.Drawing.Point(6, 114);
            this.Label23.Name = "Label23";
            this.Label23.Size = new System.Drawing.Size(183, 13);
            this.Label23.TabIndex = 24;
            this.Label23.Text = "Name of person/organization calling?";
            // 
            // TextBox13
            // 
            this.TextBox13.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bombInfoBindingSource, "CallerName", true));
            this.TextBox13.Location = new System.Drawing.Point(200, 111);
            this.TextBox13.Name = "TextBox13";
            this.TextBox13.Size = new System.Drawing.Size(181, 20);
            this.TextBox13.TabIndex = 23;
            // 
            // bombInfoBindingSource
            // 
            this.bombInfoBindingSource.DataSource = typeof(INCIDENTRP.BombInfo);
            // 
            // Label24
            // 
            this.Label24.AutoSize = true;
            this.Label24.Location = new System.Drawing.Point(6, 91);
            this.Label24.Name = "Label24";
            this.Label24.Size = new System.Drawing.Size(161, 13);
            this.Label24.TabIndex = 22;
            this.Label24.Text = "Who placed the bomb and why?";
            // 
            // TextBox14
            // 
            this.TextBox14.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bombInfoBindingSource, "WhoPlacedAndWhy", true));
            this.TextBox14.Location = new System.Drawing.Point(200, 88);
            this.TextBox14.Name = "TextBox14";
            this.TextBox14.Size = new System.Drawing.Size(181, 20);
            this.TextBox14.TabIndex = 21;
            // 
            // Label20
            // 
            this.Label20.AutoSize = true;
            this.Label20.Location = new System.Drawing.Point(6, 68);
            this.Label20.Name = "Label20";
            this.Label20.Size = new System.Drawing.Size(115, 13);
            this.Label20.TabIndex = 20;
            this.Label20.Text = "What does it look like?";
            // 
            // TextBox10
            // 
            this.TextBox10.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bombInfoBindingSource, "Appearance", true));
            this.TextBox10.Location = new System.Drawing.Point(200, 65);
            this.TextBox10.Name = "TextBox10";
            this.TextBox10.Size = new System.Drawing.Size(181, 20);
            this.TextBox10.TabIndex = 19;
            // 
            // Label21
            // 
            this.Label21.AutoSize = true;
            this.Label21.Location = new System.Drawing.Point(6, 45);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(97, 13);
            this.Label21.TabIndex = 18;
            this.Label21.Text = "When will it go off?";
            // 
            // TextBox11
            // 
            this.TextBox11.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bombInfoBindingSource, "DetonationTime", true));
            this.TextBox11.Location = new System.Drawing.Point(200, 42);
            this.TextBox11.Name = "TextBox11";
            this.TextBox11.Size = new System.Drawing.Size(181, 20);
            this.TextBox11.TabIndex = 17;
            // 
            // Label22
            // 
            this.Label22.AutoSize = true;
            this.Label22.Location = new System.Drawing.Point(6, 22);
            this.Label22.Name = "Label22";
            this.Label22.Size = new System.Drawing.Size(101, 13);
            this.Label22.TabIndex = 16;
            this.Label22.Text = "Where is it located?";
            // 
            // TextBox12
            // 
            this.TextBox12.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bombInfoBindingSource, "Location", true));
            this.TextBox12.Location = new System.Drawing.Point(200, 19);
            this.TextBox12.Name = "TextBox12";
            this.TextBox12.Size = new System.Drawing.Size(181, 20);
            this.TextBox12.TabIndex = 15;
            // 
            // GroupBox6
            // 
            this.GroupBox6.Controls.Add(this.Label19);
            this.GroupBox6.Controls.Add(this.TextBox9);
            this.GroupBox6.Controls.Add(this.Label18);
            this.GroupBox6.Controls.Add(this.TextBox8);
            this.GroupBox6.ForeColor = System.Drawing.Color.White;
            this.GroupBox6.Location = new System.Drawing.Point(409, 5);
            this.GroupBox6.Name = "GroupBox6";
            this.GroupBox6.Size = new System.Drawing.Size(390, 123);
            this.GroupBox6.TabIndex = 19;
            this.GroupBox6.TabStop = false;
            this.GroupBox6.Text = "Threat Information";
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(6, 72);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(71, 13);
            this.Label19.TabIndex = 20;
            this.Label19.Text = "Nature of Call";
            // 
            // TextBox9
            // 
            this.TextBox9.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.threatInfoBindingSource, "NatureOfCall", true));
            this.TextBox9.Location = new System.Drawing.Point(135, 69);
            this.TextBox9.Multiline = true;
            this.TextBox9.Name = "TextBox9";
            this.TextBox9.Size = new System.Drawing.Size(246, 48);
            this.TextBox9.TabIndex = 19;
            // 
            // threatInfoBindingSource
            // 
            this.threatInfoBindingSource.DataSource = typeof(INCIDENTRP.ThreatInfo);
            // 
            // Label18
            // 
            this.Label18.AutoSize = true;
            this.Label18.Location = new System.Drawing.Point(6, 15);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(123, 13);
            this.Label18.TabIndex = 18;
            this.Label18.Text = "Exact Wording of Threat";
            // 
            // TextBox8
            // 
            this.TextBox8.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.threatInfoBindingSource, "WordingOfThreat", true));
            this.TextBox8.Location = new System.Drawing.Point(135, 12);
            this.TextBox8.Multiline = true;
            this.TextBox8.Name = "TextBox8";
            this.TextBox8.Size = new System.Drawing.Size(246, 51);
            this.TextBox8.TabIndex = 17;
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.grpLawEnforcementContactInfo);
            this.GroupBox3.Controls.Add(this.chkContactedLawEnforcement);
            this.GroupBox3.Controls.Add(this.grpInformationSecurityContactInfo);
            this.GroupBox3.Controls.Add(this.chkContactedInformationSecurity);
            this.GroupBox3.ForeColor = System.Drawing.Color.White;
            this.GroupBox3.Location = new System.Drawing.Point(8, 296);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(390, 242);
            this.GroupBox3.TabIndex = 18;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Actions Taken";
            // 
            // grpLawEnforcementContactInfo
            // 
            this.grpLawEnforcementContactInfo.Controls.Add(this.Label15);
            this.grpLawEnforcementContactInfo.Controls.Add(this.Label16);
            this.grpLawEnforcementContactInfo.Controls.Add(this.TextBox7);
            this.grpLawEnforcementContactInfo.Controls.Add(this.dateTimePicker7);
            this.grpLawEnforcementContactInfo.Controls.Add(this.ContactLawDate);
            this.grpLawEnforcementContactInfo.Controls.Add(this.Label17);
            this.grpLawEnforcementContactInfo.Enabled = false;
            this.grpLawEnforcementContactInfo.Location = new System.Drawing.Point(37, 148);
            this.grpLawEnforcementContactInfo.Name = "grpLawEnforcementContactInfo";
            this.grpLawEnforcementContactInfo.Size = new System.Drawing.Size(345, 88);
            this.grpLawEnforcementContactInfo.TabIndex = 25;
            this.grpLawEnforcementContactInfo.TabStop = false;
            this.grpLawEnforcementContactInfo.Text = "Contact Information";
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.Location = new System.Drawing.Point(15, 16);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(147, 13);
            this.Label15.TabIndex = 18;
            this.Label15.Text = "Name of Individual Contacted";
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.Location = new System.Drawing.Point(15, 61);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(82, 13);
            this.Label16.TabIndex = 22;
            this.Label16.Text = "Time Contacted";
            // 
            // TextBox7
            // 
            this.TextBox7.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lawActionBindingSource, "PersonContacted", true));
            this.TextBox7.Location = new System.Drawing.Point(170, 13);
            this.TextBox7.Name = "TextBox7";
            this.TextBox7.Size = new System.Drawing.Size(169, 20);
            this.TextBox7.TabIndex = 17;
            // 
            // lawActionBindingSource
            // 
            this.lawActionBindingSource.DataSource = typeof(INCIDENTRP.ActionTaken);
            // 
            // dateTimePicker7
            // 
            this.dateTimePicker7.CustomFormat = "hh:mm tt";
            this.dateTimePicker7.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.lawActionBindingSource, "DateTime", true));
            this.dateTimePicker7.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker7.Location = new System.Drawing.Point(170, 57);
            this.dateTimePicker7.Name = "dateTimePicker7";
            this.dateTimePicker7.ShowUpDown = true;
            this.dateTimePicker7.Size = new System.Drawing.Size(169, 20);
            this.dateTimePicker7.TabIndex = 21;
            // 
            // ContactLawDate
            // 
            this.ContactLawDate.CustomFormat = "MM/dd/yyyy";
            this.ContactLawDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.lawActionBindingSource, "DateTime", true));
            this.ContactLawDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ContactLawDate.Location = new System.Drawing.Point(170, 34);
            this.ContactLawDate.Name = "ContactLawDate";
            this.ContactLawDate.Size = new System.Drawing.Size(169, 20);
            this.ContactLawDate.TabIndex = 19;
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Location = new System.Drawing.Point(15, 38);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(82, 13);
            this.Label17.TabIndex = 20;
            this.Label17.Text = "Date Contacted";
            // 
            // chkContactedLawEnforcement
            // 
            this.chkContactedLawEnforcement.AutoSize = true;
            this.chkContactedLawEnforcement.Location = new System.Drawing.Point(19, 130);
            this.chkContactedLawEnforcement.Name = "chkContactedLawEnforcement";
            this.chkContactedLawEnforcement.Size = new System.Drawing.Size(161, 17);
            this.chkContactedLawEnforcement.TabIndex = 24;
            this.chkContactedLawEnforcement.Text = "Contacted Law Enforcement";
            this.chkContactedLawEnforcement.UseVisualStyleBackColor = true;
            this.chkContactedLawEnforcement.CheckedChanged += new System.EventHandler(this.ChkContactedLawEnforcement_CheckedChanged);
            // 
            // grpInformationSecurityContactInfo
            // 
            this.grpInformationSecurityContactInfo.Controls.Add(this.Label12);
            this.grpInformationSecurityContactInfo.Controls.Add(this.Label13);
            this.grpInformationSecurityContactInfo.Controls.Add(this.TextBox6);
            this.grpInformationSecurityContactInfo.Controls.Add(this.ContactITTime);
            this.grpInformationSecurityContactInfo.Controls.Add(this.ContactITDate);
            this.grpInformationSecurityContactInfo.Controls.Add(this.Label14);
            this.grpInformationSecurityContactInfo.Enabled = false;
            this.grpInformationSecurityContactInfo.Location = new System.Drawing.Point(37, 36);
            this.grpInformationSecurityContactInfo.Name = "grpInformationSecurityContactInfo";
            this.grpInformationSecurityContactInfo.Size = new System.Drawing.Size(345, 88);
            this.grpInformationSecurityContactInfo.TabIndex = 23;
            this.grpInformationSecurityContactInfo.TabStop = false;
            this.grpInformationSecurityContactInfo.Text = "Contact Information";
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(15, 16);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(147, 13);
            this.Label12.TabIndex = 18;
            this.Label12.Text = "Name of Individual Contacted";
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(15, 61);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(82, 13);
            this.Label13.TabIndex = 22;
            this.Label13.Text = "Time Contacted";
            // 
            // TextBox6
            // 
            this.TextBox6.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.infoTechActionBindingSource, "PersonContacted", true));
            this.TextBox6.Location = new System.Drawing.Point(170, 13);
            this.TextBox6.Name = "TextBox6";
            this.TextBox6.Size = new System.Drawing.Size(169, 20);
            this.TextBox6.TabIndex = 17;
            // 
            // infoTechActionBindingSource
            // 
            this.infoTechActionBindingSource.DataSource = typeof(INCIDENTRP.ActionTaken);
            // 
            // ContactITTime
            // 
            this.ContactITTime.CustomFormat = "hh:mm tt";
            this.ContactITTime.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.infoTechActionBindingSource, "DateTime", true));
            this.ContactITTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ContactITTime.Location = new System.Drawing.Point(170, 57);
            this.ContactITTime.Name = "ContactITTime";
            this.ContactITTime.ShowUpDown = true;
            this.ContactITTime.Size = new System.Drawing.Size(169, 20);
            this.ContactITTime.TabIndex = 21;
            // 
            // ContactITDate
            // 
            this.ContactITDate.CustomFormat = "MM/dd/yyyy";
            this.ContactITDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.infoTechActionBindingSource, "DateTime", true));
            this.ContactITDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ContactITDate.Location = new System.Drawing.Point(170, 34);
            this.ContactITDate.Name = "ContactITDate";
            this.ContactITDate.Size = new System.Drawing.Size(169, 20);
            this.ContactITDate.TabIndex = 19;
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.Location = new System.Drawing.Point(15, 38);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(82, 13);
            this.Label14.TabIndex = 20;
            this.Label14.Text = "Date Contacted";
            // 
            // chkContactedInformationSecurity
            // 
            this.chkContactedInformationSecurity.AutoSize = true;
            this.chkContactedInformationSecurity.Location = new System.Drawing.Point(19, 19);
            this.chkContactedInformationSecurity.Name = "chkContactedInformationSecurity";
            this.chkContactedInformationSecurity.Size = new System.Drawing.Size(318, 17);
            this.chkContactedInformationSecurity.TabIndex = 0;
            this.chkContactedInformationSecurity.Text = "Contacted Information Technology/Information Security Office";
            this.chkContactedInformationSecurity.UseVisualStyleBackColor = true;
            this.chkContactedInformationSecurity.CheckedChanged += new System.EventHandler(this.ChkContactedInformationSecurity_CheckedChanged);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.lblOtherType);
            this.GroupBox2.Controls.Add(this.txtOtherType);
            this.GroupBox2.Controls.Add(this.lblOtherMethod);
            this.GroupBox2.Controls.Add(this.txtOtherMethod);
            this.GroupBox2.Controls.Add(this.Label11);
            this.GroupBox2.Controls.Add(this.TextBox5);
            this.GroupBox2.Controls.Add(this.Label10);
            this.GroupBox2.Controls.Add(this.TextBox4);
            this.GroupBox2.Controls.Add(this.Label9);
            this.GroupBox2.Controls.Add(this.TextBox3);
            this.GroupBox2.Controls.Add(this.Label8);
            this.GroupBox2.Controls.Add(this.cmbNotifierType);
            this.GroupBox2.Controls.Add(this.Label7);
            this.GroupBox2.Controls.Add(this.cmbNotificationMethod);
            this.GroupBox2.ForeColor = System.Drawing.Color.White;
            this.GroupBox2.Location = new System.Drawing.Point(8, 119);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(390, 171);
            this.GroupBox2.TabIndex = 17;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Incident Detail";
            // 
            // lblOtherType
            // 
            this.lblOtherType.AutoSize = true;
            this.lblOtherType.Location = new System.Drawing.Point(183, 84);
            this.lblOtherType.Name = "lblOtherType";
            this.lblOtherType.Size = new System.Drawing.Size(60, 13);
            this.lblOtherType.TabIndex = 24;
            this.lblOtherType.Text = "Description";
            // 
            // txtOtherType
            // 
            this.txtOtherType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "OtherType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOtherType.Location = new System.Drawing.Point(249, 81);
            this.txtOtherType.Name = "txtOtherType";
            this.txtOtherType.Size = new System.Drawing.Size(135, 20);
            this.txtOtherType.TabIndex = 23;
            // 
            // notifierBindingSource
            // 
            this.notifierBindingSource.DataSource = typeof(INCIDENTRP.Notifier);
            // 
            // lblOtherMethod
            // 
            this.lblOtherMethod.AutoSize = true;
            this.lblOtherMethod.Location = new System.Drawing.Point(181, 39);
            this.lblOtherMethod.Name = "lblOtherMethod";
            this.lblOtherMethod.Size = new System.Drawing.Size(60, 13);
            this.lblOtherMethod.TabIndex = 22;
            this.lblOtherMethod.Text = "Description";
            // 
            // txtOtherMethod
            // 
            this.txtOtherMethod.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "OtherMethod", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOtherMethod.Location = new System.Drawing.Point(249, 36);
            this.txtOtherMethod.Name = "txtOtherMethod";
            this.txtOtherMethod.Size = new System.Drawing.Size(135, 20);
            this.txtOtherMethod.TabIndex = 21;
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(19, 150);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(114, 13);
            this.Label11.TabIndex = 20;
            this.Label11.Text = "Notifier Phone Number";
            // 
            // TextBox5
            // 
            this.TextBox5.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "PhoneNumber", true));
            this.TextBox5.Location = new System.Drawing.Point(186, 147);
            this.TextBox5.Name = "TextBox5";
            this.TextBox5.Size = new System.Drawing.Size(198, 20);
            this.TextBox5.TabIndex = 19;
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(19, 128);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(112, 13);
            this.Label10.TabIndex = 18;
            this.Label10.Text = "Notifier E-mail Address";
            // 
            // TextBox4
            // 
            this.TextBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "EmailAddress", true));
            this.TextBox4.Location = new System.Drawing.Point(186, 125);
            this.TextBox4.Name = "TextBox4";
            this.TextBox4.Size = new System.Drawing.Size(198, 20);
            this.TextBox4.TabIndex = 17;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(19, 106);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(71, 13);
            this.Label9.TabIndex = 16;
            this.Label9.Text = "Notifier Name";
            // 
            // TextBox3
            // 
            this.TextBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "Name", true));
            this.TextBox3.Location = new System.Drawing.Point(186, 103);
            this.TextBox3.Name = "TextBox3";
            this.TextBox3.Size = new System.Drawing.Size(198, 20);
            this.TextBox3.TabIndex = 15;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(19, 61);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(58, 13);
            this.Label8.TabIndex = 14;
            this.Label8.Text = "Notified By";
            // 
            // cmbNotifierType
            // 
            this.cmbNotifierType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbNotifierType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbNotifierType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "Type", true));
            this.cmbNotifierType.FormattingEnabled = true;
            this.cmbNotifierType.Location = new System.Drawing.Point(186, 58);
            this.cmbNotifierType.Name = "cmbNotifierType";
            this.cmbNotifierType.Size = new System.Drawing.Size(198, 21);
            this.cmbNotifierType.TabIndex = 13;
            this.cmbNotifierType.SelectedIndexChanged += new System.EventHandler(this.CmbNotifierType_SelectedIndexChanged);
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(17, 16);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(79, 13);
            this.Label7.TabIndex = 12;
            this.Label7.Text = "Notifier Method";
            // 
            // cmbNotificationMethod
            // 
            this.cmbNotificationMethod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbNotificationMethod.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbNotificationMethod.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "Method", true));
            this.cmbNotificationMethod.FormattingEnabled = true;
            this.cmbNotificationMethod.Location = new System.Drawing.Point(186, 13);
            this.cmbNotificationMethod.Name = "cmbNotificationMethod";
            this.cmbNotificationMethod.Size = new System.Drawing.Size(198, 21);
            this.cmbNotificationMethod.TabIndex = 11;
            this.cmbNotificationMethod.SelectedIndexChanged += new System.EventHandler(this.CmbNotificationMethod_SelectedIndexChanged);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.TextBox2);
            this.GroupBox1.Controls.Add(this.label27);
            this.GroupBox1.Controls.Add(this.txtReporterEmailAddress);
            this.GroupBox1.Controls.Add(this.label28);
            this.GroupBox1.Controls.Add(this.cmbBusinessUnit);
            this.GroupBox1.Controls.Add(this.label29);
            this.GroupBox1.Controls.Add(this.cmbReporter);
            this.GroupBox1.ForeColor = System.Drawing.Color.White;
            this.GroupBox1.Location = new System.Drawing.Point(8, 5);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(390, 108);
            this.GroupBox1.TabIndex = 16;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Incident Reporter";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(17, 84);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(78, 13);
            this.Label4.TabIndex = 7;
            this.Label4.Text = "Phone Number";
            // 
            // TextBox2
            // 
            this.TextBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.reporterBindingSource, "PhoneNumber", true));
            this.TextBox2.Location = new System.Drawing.Point(184, 81);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(198, 20);
            this.TextBox2.TabIndex = 6;
            // 
            // reporterBindingSource
            // 
            this.reporterBindingSource.DataSource = typeof(INCIDENTRP.Reporter);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(17, 62);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(76, 13);
            this.label27.TabIndex = 5;
            this.label27.Text = "E-mail Address";
            // 
            // txtReporterEmailAddress
            // 
            this.txtReporterEmailAddress.Location = new System.Drawing.Point(184, 59);
            this.txtReporterEmailAddress.Name = "txtReporterEmailAddress";
            this.txtReporterEmailAddress.Size = new System.Drawing.Size(198, 20);
            this.txtReporterEmailAddress.TabIndex = 4;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(17, 39);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(71, 13);
            this.label28.TabIndex = 3;
            this.label28.Text = "Business Unit";
            // 
            // cmbBusinessUnit
            // 
            this.cmbBusinessUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBusinessUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBusinessUnit.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.reporterBindingSource, "BusinessUnit", true));
            this.cmbBusinessUnit.DataSource = this.businessUnitBindingSource;
            this.cmbBusinessUnit.DisplayMember = "Name";
            this.cmbBusinessUnit.FormattingEnabled = true;
            this.cmbBusinessUnit.Location = new System.Drawing.Point(184, 36);
            this.cmbBusinessUnit.Name = "cmbBusinessUnit";
            this.cmbBusinessUnit.Size = new System.Drawing.Size(198, 21);
            this.cmbBusinessUnit.TabIndex = 2;
            this.cmbBusinessUnit.ValueMember = "ID";
            // 
            // businessUnitBindingSource
            // 
            this.businessUnitBindingSource.DataSource = typeof(Uheaa.Common.ProcessLogger.BusinessUnit);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(17, 16);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(60, 13);
            this.label29.TabIndex = 1;
            this.label29.Text = "User Name";
            // 
            // cmbReporter
            // 
            this.cmbReporter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbReporter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbReporter.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.reporterBindingSource, "User", true));
            this.cmbReporter.DataSource = this.reporterNameBindingSource;
            this.cmbReporter.FormattingEnabled = true;
            this.cmbReporter.Location = new System.Drawing.Point(184, 13);
            this.cmbReporter.Name = "cmbReporter";
            this.cmbReporter.Size = new System.Drawing.Size(198, 21);
            this.cmbReporter.TabIndex = 0;
            // 
            // reporterNameBindingSource
            // 
            this.reporterNameBindingSource.DataSource = typeof(SubSystemShared.SqlUser);
            // 
            // GroupBox8
            // 
            this.GroupBox8.Controls.Add(this.label34);
            this.GroupBox8.Controls.Add(this.textBox21);
            this.GroupBox8.Controls.Add(this.Label32);
            this.GroupBox8.Controls.Add(this.TextBox18);
            this.GroupBox8.Controls.Add(this.Label33);
            this.GroupBox8.Controls.Add(this.TextBox20);
            this.GroupBox8.Controls.Add(this.chkGenderUnsure);
            this.GroupBox8.Controls.Add(this.chkGenderFemale);
            this.GroupBox8.Controls.Add(this.chkGenderMale);
            this.GroupBox8.Controls.Add(this.chkFamiliarNo);
            this.GroupBox8.Controls.Add(this.chkFamiliarYes);
            this.GroupBox8.Controls.Add(this.chkRecognizedVoiceNo);
            this.GroupBox8.Controls.Add(this.chkRecognizedVoiceYes);
            this.GroupBox8.Controls.Add(this.Label31);
            this.GroupBox8.Controls.Add(this.Label30);
            this.GroupBox8.Controls.Add(this.Label25);
            this.GroupBox8.Controls.Add(this.TextBox15);
            this.GroupBox8.Controls.Add(this.Label26);
            this.GroupBox8.Controls.Add(this.TextBox16);
            this.GroupBox8.Controls.Add(this.label35);
            this.GroupBox8.Controls.Add(this.textBox19);
            this.GroupBox8.Controls.Add(this.label36);
            this.GroupBox8.Controls.Add(this.label37);
            this.GroupBox8.Controls.Add(this.textBox22);
            this.GroupBox8.ForeColor = System.Drawing.Color.White;
            this.GroupBox8.Location = new System.Drawing.Point(409, 271);
            this.GroupBox8.Name = "GroupBox8";
            this.GroupBox8.Size = new System.Drawing.Size(390, 267);
            this.GroupBox8.TabIndex = 21;
            this.GroupBox8.TabStop = false;
            this.GroupBox8.Text = "Call Information";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(326, 72);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(50, 13);
            this.label34.TabIndex = 42;
            this.label34.Text = "Specifics";
            // 
            // textBox21
            // 
            this.textBox21.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.callerBindingSource, "FamiliaritySpecifics", true));
            this.textBox21.Location = new System.Drawing.Point(200, 88);
            this.textBox21.Multiline = true;
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new System.Drawing.Size(181, 36);
            this.textBox21.TabIndex = 41;
            // 
            // callerBindingSource
            // 
            this.callerBindingSource.DataSource = typeof(INCIDENTRP.Caller);
            // 
            // Label32
            // 
            this.Label32.AutoSize = true;
            this.Label32.Location = new System.Drawing.Point(6, 245);
            this.Label32.Name = "Label32";
            this.Label32.Size = new System.Drawing.Size(123, 13);
            this.Label32.TabIndex = 39;
            this.Label32.Text = "Caller\'s Account Number";
            // 
            // TextBox18
            // 
            this.TextBox18.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.callerBindingSource, "AccountNumber", true));
            this.TextBox18.Location = new System.Drawing.Point(200, 242);
            this.TextBox18.Name = "TextBox18";
            this.TextBox18.Size = new System.Drawing.Size(181, 20);
            this.TextBox18.TabIndex = 38;
            // 
            // Label33
            // 
            this.Label33.AutoSize = true;
            this.Label33.Location = new System.Drawing.Point(6, 222);
            this.Label33.Name = "Label33";
            this.Label33.Size = new System.Drawing.Size(81, 13);
            this.Label33.TabIndex = 37;
            this.Label33.Text = "Caller\'s Address";
            // 
            // TextBox20
            // 
            this.TextBox20.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.callerBindingSource, "Address", true));
            this.TextBox20.Location = new System.Drawing.Point(200, 219);
            this.TextBox20.Name = "TextBox20";
            this.TextBox20.Size = new System.Drawing.Size(181, 20);
            this.TextBox20.TabIndex = 36;
            // 
            // chkGenderUnsure
            // 
            this.chkGenderUnsure.AutoSize = true;
            this.chkGenderUnsure.Checked = true;
            this.chkGenderUnsure.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenderUnsure.Location = new System.Drawing.Point(316, 130);
            this.chkGenderUnsure.Name = "chkGenderUnsure";
            this.chkGenderUnsure.Size = new System.Drawing.Size(60, 17);
            this.chkGenderUnsure.TabIndex = 35;
            this.chkGenderUnsure.Text = "Unsure";
            this.chkGenderUnsure.UseVisualStyleBackColor = true;
            this.chkGenderUnsure.CheckedChanged += new System.EventHandler(this.ChkGenderUnsure_CheckedChanged);
            // 
            // chkGenderFemale
            // 
            this.chkGenderFemale.AutoSize = true;
            this.chkGenderFemale.Location = new System.Drawing.Point(250, 130);
            this.chkGenderFemale.Name = "chkGenderFemale";
            this.chkGenderFemale.Size = new System.Drawing.Size(60, 17);
            this.chkGenderFemale.TabIndex = 34;
            this.chkGenderFemale.Text = "Female";
            this.chkGenderFemale.UseVisualStyleBackColor = true;
            this.chkGenderFemale.CheckedChanged += new System.EventHandler(this.ChkGenderFemale_CheckedChanged);
            // 
            // chkGenderMale
            // 
            this.chkGenderMale.AutoSize = true;
            this.chkGenderMale.Location = new System.Drawing.Point(200, 130);
            this.chkGenderMale.Name = "chkGenderMale";
            this.chkGenderMale.Size = new System.Drawing.Size(49, 17);
            this.chkGenderMale.TabIndex = 33;
            this.chkGenderMale.Text = "Male";
            this.chkGenderMale.UseVisualStyleBackColor = true;
            this.chkGenderMale.CheckedChanged += new System.EventHandler(this.ChkGenderMale_CheckedChanged);
            // 
            // chkFamiliarNo
            // 
            this.chkFamiliarNo.AutoSize = true;
            this.chkFamiliarNo.Checked = true;
            this.chkFamiliarNo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFamiliarNo.Location = new System.Drawing.Point(250, 65);
            this.chkFamiliarNo.Name = "chkFamiliarNo";
            this.chkFamiliarNo.Size = new System.Drawing.Size(40, 17);
            this.chkFamiliarNo.TabIndex = 32;
            this.chkFamiliarNo.Text = "No";
            this.chkFamiliarNo.UseVisualStyleBackColor = true;
            this.chkFamiliarNo.CheckedChanged += new System.EventHandler(this.ChkFamiliarNo_CheckedChanged);
            // 
            // chkFamiliarYes
            // 
            this.chkFamiliarYes.AutoSize = true;
            this.chkFamiliarYes.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.callerBindingSource, "CallerIsFamiliarWithUheaa", true));
            this.chkFamiliarYes.Location = new System.Drawing.Point(200, 65);
            this.chkFamiliarYes.Name = "chkFamiliarYes";
            this.chkFamiliarYes.Size = new System.Drawing.Size(44, 17);
            this.chkFamiliarYes.TabIndex = 31;
            this.chkFamiliarYes.Text = "Yes";
            this.chkFamiliarYes.UseVisualStyleBackColor = true;
            this.chkFamiliarYes.CheckedChanged += new System.EventHandler(this.ChkFamiliarYes_CheckedChanged);
            // 
            // chkRecognizedVoiceNo
            // 
            this.chkRecognizedVoiceNo.AutoSize = true;
            this.chkRecognizedVoiceNo.Checked = true;
            this.chkRecognizedVoiceNo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRecognizedVoiceNo.Location = new System.Drawing.Point(250, 45);
            this.chkRecognizedVoiceNo.Name = "chkRecognizedVoiceNo";
            this.chkRecognizedVoiceNo.Size = new System.Drawing.Size(40, 17);
            this.chkRecognizedVoiceNo.TabIndex = 30;
            this.chkRecognizedVoiceNo.Text = "No";
            this.chkRecognizedVoiceNo.UseVisualStyleBackColor = true;
            this.chkRecognizedVoiceNo.CheckedChanged += new System.EventHandler(this.ChkRecognizedVoiceNo_CheckedChanged);
            // 
            // chkRecognizedVoiceYes
            // 
            this.chkRecognizedVoiceYes.AutoSize = true;
            this.chkRecognizedVoiceYes.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.callerBindingSource, "RecognizedTheCallersVoice", true));
            this.chkRecognizedVoiceYes.Location = new System.Drawing.Point(200, 45);
            this.chkRecognizedVoiceYes.Name = "chkRecognizedVoiceYes";
            this.chkRecognizedVoiceYes.Size = new System.Drawing.Size(44, 17);
            this.chkRecognizedVoiceYes.TabIndex = 29;
            this.chkRecognizedVoiceYes.Text = "Yes";
            this.chkRecognizedVoiceYes.UseVisualStyleBackColor = true;
            this.chkRecognizedVoiceYes.CheckedChanged += new System.EventHandler(this.ChkRecognizedVoiceYes_CheckedChanged);
            // 
            // Label31
            // 
            this.Label31.Location = new System.Drawing.Point(6, 65);
            this.Label31.Name = "Label31";
            this.Label31.Size = new System.Drawing.Size(161, 53);
            this.Label31.TabIndex = 28;
            this.Label31.Text = "Did the description of the bomb location or anything else the caller said indicat" +
    "e familiarity with UHEAA premises?";
            // 
            // Label30
            // 
            this.Label30.AutoSize = true;
            this.Label30.Location = new System.Drawing.Point(6, 46);
            this.Label30.Name = "Label30";
            this.Label30.Size = new System.Drawing.Size(180, 13);
            this.Label30.TabIndex = 26;
            this.Label30.Text = "Did you recognize the caller\'s voice?";
            // 
            // Label25
            // 
            this.Label25.AutoSize = true;
            this.Label25.Location = new System.Drawing.Point(6, 200);
            this.Label25.Name = "Label25";
            this.Label25.Size = new System.Drawing.Size(114, 13);
            this.Label25.TabIndex = 24;
            this.Label25.Text = "Caller\'s Phone Number";
            // 
            // TextBox15
            // 
            this.TextBox15.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.callerBindingSource, "PhoneNumber", true));
            this.TextBox15.Location = new System.Drawing.Point(200, 197);
            this.TextBox15.Name = "TextBox15";
            this.TextBox15.Size = new System.Drawing.Size(181, 20);
            this.TextBox15.TabIndex = 23;
            // 
            // Label26
            // 
            this.Label26.AutoSize = true;
            this.Label26.Location = new System.Drawing.Point(6, 177);
            this.Label26.Name = "Label26";
            this.Label26.Size = new System.Drawing.Size(71, 13);
            this.Label26.TabIndex = 22;
            this.Label26.Text = "Caller\'s Name";
            // 
            // TextBox16
            // 
            this.TextBox16.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.callerBindingSource, "Name", true));
            this.TextBox16.Location = new System.Drawing.Point(200, 174);
            this.TextBox16.Name = "TextBox16";
            this.TextBox16.Size = new System.Drawing.Size(181, 20);
            this.TextBox16.TabIndex = 21;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(6, 154);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(123, 13);
            this.label35.TabIndex = 20;
            this.label35.Text = "Caller\'s Approximate Age";
            // 
            // textBox19
            // 
            this.textBox19.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.callerBindingSource, "Age", true));
            this.textBox19.Location = new System.Drawing.Point(200, 151);
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new System.Drawing.Size(181, 20);
            this.textBox19.TabIndex = 19;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(6, 131);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(61, 13);
            this.label36.TabIndex = 18;
            this.label36.Text = "Caller\'s Sex";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(6, 22);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(110, 13);
            this.label37.TabIndex = 16;
            this.label37.Text = "Call duration (minutes)";
            // 
            // textBox22
            // 
            this.textBox22.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.callerBindingSource, "CallDuration", true));
            this.textBox22.Location = new System.Drawing.Point(200, 19);
            this.textBox22.Name = "textBox22";
            this.textBox22.Size = new System.Drawing.Size(181, 20);
            this.textBox22.TabIndex = 15;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.txtRemarks);
            this.tabPage2.Controls.Add(this.groupBox9);
            this.tabPage2.Controls.Add(this.groupBox10);
            this.tabPage2.Controls.Add(this.groupBox11);
            this.tabPage2.Controls.Add(this.label42);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(808, 544);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Caller Information";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtVoiceOther);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.chkVoiceOther);
            this.groupBox4.Controls.Add(this.CheckBox9);
            this.groupBox4.Controls.Add(this.CheckBox10);
            this.groupBox4.Controls.Add(this.CheckBox11);
            this.groupBox4.Controls.Add(this.CheckBox12);
            this.groupBox4.Controls.Add(this.CheckBox4);
            this.groupBox4.Controls.Add(this.CheckBox5);
            this.groupBox4.Controls.Add(this.CheckBox6);
            this.groupBox4.Controls.Add(this.CheckBox3);
            this.groupBox4.Controls.Add(this.CheckBox2);
            this.groupBox4.Controls.Add(this.CheckBox1);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(8, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(289, 163);
            this.groupBox4.TabIndex = 23;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Caller\'s Voice and Speech Manner (select all that apply)";
            // 
            // txtVoiceOther
            // 
            this.txtVoiceOther.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.voiceBindingSource, "OtherDescription", true));
            this.txtVoiceOther.Location = new System.Drawing.Point(174, 132);
            this.txtVoiceOther.Name = "txtVoiceOther";
            this.txtVoiceOther.Size = new System.Drawing.Size(94, 20);
            this.txtVoiceOther.TabIndex = 12;
            // 
            // voiceBindingSource
            // 
            this.voiceBindingSource.DataSource = typeof(INCIDENTRP.Voice);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(119, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Describe";
            // 
            // chkVoiceOther
            // 
            this.chkVoiceOther.AutoSize = true;
            this.chkVoiceOther.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.voiceBindingSource, "Other", true));
            this.chkVoiceOther.Location = new System.Drawing.Point(122, 111);
            this.chkVoiceOther.Name = "chkVoiceOther";
            this.chkVoiceOther.Size = new System.Drawing.Size(52, 17);
            this.chkVoiceOther.TabIndex = 10;
            this.chkVoiceOther.Text = "Other";
            this.chkVoiceOther.UseVisualStyleBackColor = true;
            // 
            // CheckBox9
            // 
            this.CheckBox9.AutoSize = true;
            this.CheckBox9.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.voiceBindingSource, "Nasal", true));
            this.CheckBox9.Location = new System.Drawing.Point(122, 19);
            this.CheckBox9.Name = "CheckBox9";
            this.CheckBox9.Size = new System.Drawing.Size(53, 17);
            this.CheckBox9.TabIndex = 9;
            this.CheckBox9.Text = "Nasal";
            this.CheckBox9.UseVisualStyleBackColor = true;
            // 
            // CheckBox10
            // 
            this.CheckBox10.AutoSize = true;
            this.CheckBox10.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.voiceBindingSource, "Slurred", true));
            this.CheckBox10.Location = new System.Drawing.Point(122, 65);
            this.CheckBox10.Name = "CheckBox10";
            this.CheckBox10.Size = new System.Drawing.Size(59, 17);
            this.CheckBox10.TabIndex = 8;
            this.CheckBox10.Text = "Slurred";
            this.CheckBox10.UseVisualStyleBackColor = true;
            // 
            // CheckBox11
            // 
            this.CheckBox11.AutoSize = true;
            this.CheckBox11.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.voiceBindingSource, "Distorted", true));
            this.CheckBox11.Location = new System.Drawing.Point(6, 42);
            this.CheckBox11.Name = "CheckBox11";
            this.CheckBox11.Size = new System.Drawing.Size(68, 17);
            this.CheckBox11.TabIndex = 7;
            this.CheckBox11.Text = "Distorted";
            this.CheckBox11.UseVisualStyleBackColor = true;
            // 
            // CheckBox12
            // 
            this.CheckBox12.AutoSize = true;
            this.CheckBox12.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.voiceBindingSource, "Stuttering", true));
            this.CheckBox12.Location = new System.Drawing.Point(122, 88);
            this.CheckBox12.Name = "CheckBox12";
            this.CheckBox12.Size = new System.Drawing.Size(71, 17);
            this.CheckBox12.TabIndex = 6;
            this.CheckBox12.Text = "Stuttering";
            this.CheckBox12.UseVisualStyleBackColor = true;
            // 
            // CheckBox4
            // 
            this.CheckBox4.AutoSize = true;
            this.CheckBox4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.voiceBindingSource, "High", true));
            this.CheckBox4.Location = new System.Drawing.Point(6, 88);
            this.CheckBox4.Name = "CheckBox4";
            this.CheckBox4.Size = new System.Drawing.Size(48, 17);
            this.CheckBox4.TabIndex = 5;
            this.CheckBox4.Text = "High";
            this.CheckBox4.UseVisualStyleBackColor = true;
            // 
            // CheckBox5
            // 
            this.CheckBox5.AutoSize = true;
            this.CheckBox5.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.voiceBindingSource, "Distinct", true));
            this.CheckBox5.Location = new System.Drawing.Point(6, 19);
            this.CheckBox5.Name = "CheckBox5";
            this.CheckBox5.Size = new System.Drawing.Size(61, 17);
            this.CheckBox5.TabIndex = 4;
            this.CheckBox5.Text = "Distinct";
            this.CheckBox5.UseVisualStyleBackColor = true;
            // 
            // CheckBox6
            // 
            this.CheckBox6.AutoSize = true;
            this.CheckBox6.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.voiceBindingSource, "Hoarse", true));
            this.CheckBox6.Location = new System.Drawing.Point(6, 111);
            this.CheckBox6.Name = "CheckBox6";
            this.CheckBox6.Size = new System.Drawing.Size(60, 17);
            this.CheckBox6.TabIndex = 3;
            this.CheckBox6.Text = "Hoarse";
            this.CheckBox6.UseVisualStyleBackColor = true;
            // 
            // CheckBox3
            // 
            this.CheckBox3.AutoSize = true;
            this.CheckBox3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.voiceBindingSource, "Lisp", true));
            this.CheckBox3.Location = new System.Drawing.Point(6, 134);
            this.CheckBox3.Name = "CheckBox3";
            this.CheckBox3.Size = new System.Drawing.Size(45, 17);
            this.CheckBox3.TabIndex = 2;
            this.CheckBox3.Text = "Lisp";
            this.CheckBox3.UseVisualStyleBackColor = true;
            // 
            // CheckBox2
            // 
            this.CheckBox2.AutoSize = true;
            this.CheckBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.voiceBindingSource, "Slow", true));
            this.CheckBox2.Location = new System.Drawing.Point(122, 42);
            this.CheckBox2.Name = "CheckBox2";
            this.CheckBox2.Size = new System.Drawing.Size(49, 17);
            this.CheckBox2.TabIndex = 1;
            this.CheckBox2.Text = "Slow";
            this.CheckBox2.UseVisualStyleBackColor = true;
            // 
            // CheckBox1
            // 
            this.CheckBox1.AutoSize = true;
            this.CheckBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.voiceBindingSource, "Fast", true));
            this.CheckBox1.Location = new System.Drawing.Point(6, 65);
            this.CheckBox1.Name = "CheckBox1";
            this.CheckBox1.Size = new System.Drawing.Size(46, 17);
            this.CheckBox1.TabIndex = 0;
            this.CheckBox1.Text = "Fast";
            this.CheckBox1.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.CheckBox32);
            this.groupBox5.Controls.Add(this.CheckBox33);
            this.groupBox5.Controls.Add(this.CheckBox31);
            this.groupBox5.Controls.Add(this.CheckBox30);
            this.groupBox5.Controls.Add(this.txtMannerOther);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.chkMannerOther);
            this.groupBox5.Controls.Add(this.CheckBox19);
            this.groupBox5.Controls.Add(this.CheckBox20);
            this.groupBox5.Controls.Add(this.CheckBox21);
            this.groupBox5.Controls.Add(this.CheckBox22);
            this.groupBox5.Controls.Add(this.CheckBox23);
            this.groupBox5.Controls.Add(this.CheckBox24);
            this.groupBox5.Controls.Add(this.CheckBox25);
            this.groupBox5.Controls.Add(this.CheckBox27);
            this.groupBox5.Controls.Add(this.CheckBox28);
            this.groupBox5.Controls.Add(this.CheckBox29);
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(303, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(287, 208);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Caller\'s Manner (select all that apply)";
            // 
            // CheckBox32
            // 
            this.CheckBox32.AutoSize = true;
            this.CheckBox32.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Slow", true));
            this.CheckBox32.Location = new System.Drawing.Point(131, 134);
            this.CheckBox32.Name = "CheckBox32";
            this.CheckBox32.Size = new System.Drawing.Size(49, 17);
            this.CheckBox32.TabIndex = 16;
            this.CheckBox32.Text = "Slow";
            this.CheckBox32.UseVisualStyleBackColor = true;
            // 
            // mannerBindingSource
            // 
            this.mannerBindingSource.DataSource = typeof(INCIDENTRP.Manner);
            // 
            // CheckBox33
            // 
            this.CheckBox33.AutoSize = true;
            this.CheckBox33.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Shouting", true));
            this.CheckBox33.Location = new System.Drawing.Point(131, 111);
            this.CheckBox33.Name = "CheckBox33";
            this.CheckBox33.Size = new System.Drawing.Size(68, 17);
            this.CheckBox33.TabIndex = 15;
            this.CheckBox33.Text = "Shouting";
            this.CheckBox33.UseVisualStyleBackColor = true;
            // 
            // CheckBox31
            // 
            this.CheckBox31.AutoSize = true;
            this.CheckBox31.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Righteous", true));
            this.CheckBox31.Location = new System.Drawing.Point(131, 88);
            this.CheckBox31.Name = "CheckBox31";
            this.CheckBox31.Size = new System.Drawing.Size(74, 17);
            this.CheckBox31.TabIndex = 14;
            this.CheckBox31.Text = "Righteous";
            this.CheckBox31.UseVisualStyleBackColor = true;
            // 
            // CheckBox30
            // 
            this.CheckBox30.AutoSize = true;
            this.CheckBox30.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "IllAtEase", true));
            this.CheckBox30.Location = new System.Drawing.Point(5, 157);
            this.CheckBox30.Name = "CheckBox30";
            this.CheckBox30.Size = new System.Drawing.Size(73, 17);
            this.CheckBox30.TabIndex = 13;
            this.CheckBox30.Text = "Ill-At-Ease";
            this.CheckBox30.UseVisualStyleBackColor = true;
            // 
            // txtMannerOther
            // 
            this.txtMannerOther.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mannerBindingSource, "OtherDescription", true));
            this.txtMannerOther.Location = new System.Drawing.Point(183, 178);
            this.txtMannerOther.Name = "txtMannerOther";
            this.txtMannerOther.Size = new System.Drawing.Size(94, 20);
            this.txtMannerOther.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(128, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Describe";
            // 
            // chkMannerOther
            // 
            this.chkMannerOther.AutoSize = true;
            this.chkMannerOther.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Other", true));
            this.chkMannerOther.Location = new System.Drawing.Point(131, 157);
            this.chkMannerOther.Name = "chkMannerOther";
            this.chkMannerOther.Size = new System.Drawing.Size(52, 17);
            this.chkMannerOther.TabIndex = 10;
            this.chkMannerOther.Text = "Other";
            this.chkMannerOther.UseVisualStyleBackColor = true;
            // 
            // CheckBox19
            // 
            this.CheckBox19.AutoSize = true;
            this.CheckBox19.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Rational", true));
            this.CheckBox19.Location = new System.Drawing.Point(131, 65);
            this.CheckBox19.Name = "CheckBox19";
            this.CheckBox19.Size = new System.Drawing.Size(65, 17);
            this.CheckBox19.TabIndex = 9;
            this.CheckBox19.Text = "Rational";
            this.CheckBox19.UseVisualStyleBackColor = true;
            // 
            // CheckBox20
            // 
            this.CheckBox20.AutoSize = true;
            this.CheckBox20.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Laughing", true));
            this.CheckBox20.Location = new System.Drawing.Point(131, 42);
            this.CheckBox20.Name = "CheckBox20";
            this.CheckBox20.Size = new System.Drawing.Size(70, 17);
            this.CheckBox20.TabIndex = 8;
            this.CheckBox20.Text = "Laughing";
            this.CheckBox20.UseVisualStyleBackColor = true;
            // 
            // CheckBox21
            // 
            this.CheckBox21.AutoSize = true;
            this.CheckBox21.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Irrational", true));
            this.CheckBox21.Location = new System.Drawing.Point(131, 19);
            this.CheckBox21.Name = "CheckBox21";
            this.CheckBox21.Size = new System.Drawing.Size(66, 17);
            this.CheckBox21.TabIndex = 7;
            this.CheckBox21.Text = "Irrational";
            this.CheckBox21.UseVisualStyleBackColor = true;
            // 
            // CheckBox22
            // 
            this.CheckBox22.AutoSize = true;
            this.CheckBox22.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Incoherent", true));
            this.CheckBox22.Location = new System.Drawing.Point(5, 180);
            this.CheckBox22.Name = "CheckBox22";
            this.CheckBox22.Size = new System.Drawing.Size(77, 17);
            this.CheckBox22.TabIndex = 6;
            this.CheckBox22.Text = "Incoherent";
            this.CheckBox22.UseVisualStyleBackColor = true;
            // 
            // CheckBox23
            // 
            this.CheckBox23.AutoSize = true;
            this.CheckBox23.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Emotional", true));
            this.CheckBox23.Location = new System.Drawing.Point(5, 134);
            this.CheckBox23.Name = "CheckBox23";
            this.CheckBox23.Size = new System.Drawing.Size(72, 17);
            this.CheckBox23.TabIndex = 5;
            this.CheckBox23.Text = "Emotional";
            this.CheckBox23.UseVisualStyleBackColor = true;
            // 
            // CheckBox24
            // 
            this.CheckBox24.AutoSize = true;
            this.CheckBox24.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Deliberate", true));
            this.CheckBox24.Location = new System.Drawing.Point(5, 111);
            this.CheckBox24.Name = "CheckBox24";
            this.CheckBox24.Size = new System.Drawing.Size(74, 17);
            this.CheckBox24.TabIndex = 4;
            this.CheckBox24.Text = "Deliberate";
            this.CheckBox24.UseVisualStyleBackColor = true;
            // 
            // CheckBox25
            // 
            this.CheckBox25.AutoSize = true;
            this.CheckBox25.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Coherent", true));
            this.CheckBox25.Location = new System.Drawing.Point(5, 88);
            this.CheckBox25.Name = "CheckBox25";
            this.CheckBox25.Size = new System.Drawing.Size(69, 17);
            this.CheckBox25.TabIndex = 3;
            this.CheckBox25.Text = "Coherent";
            this.CheckBox25.UseVisualStyleBackColor = true;
            // 
            // CheckBox27
            // 
            this.CheckBox27.AutoSize = true;
            this.CheckBox27.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Calm", true));
            this.CheckBox27.Location = new System.Drawing.Point(6, 65);
            this.CheckBox27.Name = "CheckBox27";
            this.CheckBox27.Size = new System.Drawing.Size(49, 17);
            this.CheckBox27.TabIndex = 2;
            this.CheckBox27.Text = "Calm";
            this.CheckBox27.UseVisualStyleBackColor = true;
            // 
            // CheckBox28
            // 
            this.CheckBox28.AutoSize = true;
            this.CheckBox28.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "BusinessLike", true));
            this.CheckBox28.Location = new System.Drawing.Point(6, 42);
            this.CheckBox28.Name = "CheckBox28";
            this.CheckBox28.Size = new System.Drawing.Size(91, 17);
            this.CheckBox28.TabIndex = 1;
            this.CheckBox28.Text = "Business-Like";
            this.CheckBox28.UseVisualStyleBackColor = true;
            // 
            // CheckBox29
            // 
            this.CheckBox29.AutoSize = true;
            this.CheckBox29.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mannerBindingSource, "Angry", true));
            this.CheckBox29.Location = new System.Drawing.Point(6, 19);
            this.CheckBox29.Name = "CheckBox29";
            this.CheckBox29.Size = new System.Drawing.Size(53, 17);
            this.CheckBox29.TabIndex = 0;
            this.CheckBox29.Text = "Angry";
            this.CheckBox29.UseVisualStyleBackColor = true;
            // 
            // txtRemarks
            // 
            this.txtRemarks.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.threatInfoBindingSource, "AdditionalRemarks", true));
            this.txtRemarks.Location = new System.Drawing.Point(8, 421);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(582, 58);
            this.txtRemarks.TabIndex = 28;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.txtDialectRegionalAmericanOther);
            this.groupBox9.Controls.Add(this.label38);
            this.groupBox9.Controls.Add(this.chkDialectRegionalAmericanOther);
            this.groupBox9.Controls.Add(this.txtDialectForeignAccentOther);
            this.groupBox9.Controls.Add(this.label39);
            this.groupBox9.Controls.Add(this.chkDialectForeignAccentOther);
            this.groupBox9.Controls.Add(this.CheckBox26);
            this.groupBox9.ForeColor = System.Drawing.Color.White;
            this.groupBox9.Location = new System.Drawing.Point(8, 267);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(289, 127);
            this.groupBox9.TabIndex = 25;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Caller\'s Dialect (select all that apply)";
            // 
            // txtDialectRegionalAmericanOther
            // 
            this.txtDialectRegionalAmericanOther.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dialectBindingSource, "RegionalAmericanDescription", true));
            this.txtDialectRegionalAmericanOther.Location = new System.Drawing.Point(174, 40);
            this.txtDialectRegionalAmericanOther.Name = "txtDialectRegionalAmericanOther";
            this.txtDialectRegionalAmericanOther.Size = new System.Drawing.Size(94, 20);
            this.txtDialectRegionalAmericanOther.TabIndex = 15;
            // 
            // dialectBindingSource
            // 
            this.dialectBindingSource.DataSource = typeof(INCIDENTRP.Dialect);
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(119, 43);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(49, 13);
            this.label38.TabIndex = 14;
            this.label38.Text = "Describe";
            // 
            // chkDialectRegionalAmericanOther
            // 
            this.chkDialectRegionalAmericanOther.AutoSize = true;
            this.chkDialectRegionalAmericanOther.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.dialectBindingSource, "RegionalAmerican", true));
            this.chkDialectRegionalAmericanOther.Location = new System.Drawing.Point(122, 19);
            this.chkDialectRegionalAmericanOther.Name = "chkDialectRegionalAmericanOther";
            this.chkDialectRegionalAmericanOther.Size = new System.Drawing.Size(115, 17);
            this.chkDialectRegionalAmericanOther.TabIndex = 13;
            this.chkDialectRegionalAmericanOther.Text = "Regional American";
            this.chkDialectRegionalAmericanOther.UseVisualStyleBackColor = true;
            // 
            // txtDialectForeignAccentOther
            // 
            this.txtDialectForeignAccentOther.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dialectBindingSource, "ForeignAccentDescription", true));
            this.txtDialectForeignAccentOther.Location = new System.Drawing.Point(174, 89);
            this.txtDialectForeignAccentOther.Name = "txtDialectForeignAccentOther";
            this.txtDialectForeignAccentOther.Size = new System.Drawing.Size(94, 20);
            this.txtDialectForeignAccentOther.TabIndex = 12;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(119, 92);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(49, 13);
            this.label39.TabIndex = 11;
            this.label39.Text = "Describe";
            // 
            // chkDialectForeignAccentOther
            // 
            this.chkDialectForeignAccentOther.AutoSize = true;
            this.chkDialectForeignAccentOther.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.dialectBindingSource, "ForeignAccent", true));
            this.chkDialectForeignAccentOther.Location = new System.Drawing.Point(122, 66);
            this.chkDialectForeignAccentOther.Name = "chkDialectForeignAccentOther";
            this.chkDialectForeignAccentOther.Size = new System.Drawing.Size(98, 17);
            this.chkDialectForeignAccentOther.TabIndex = 10;
            this.chkDialectForeignAccentOther.Text = "Foreign Accent";
            this.chkDialectForeignAccentOther.UseVisualStyleBackColor = true;
            // 
            // CheckBox26
            // 
            this.CheckBox26.AutoSize = true;
            this.CheckBox26.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.dialectBindingSource, "English", true));
            this.CheckBox26.Location = new System.Drawing.Point(16, 19);
            this.CheckBox26.Name = "CheckBox26";
            this.CheckBox26.Size = new System.Drawing.Size(60, 17);
            this.CheckBox26.TabIndex = 0;
            this.CheckBox26.Text = "English";
            this.CheckBox26.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.txtLanguageOther);
            this.groupBox10.Controls.Add(this.label40);
            this.groupBox10.Controls.Add(this.chkLanguageOther);
            this.groupBox10.Controls.Add(this.CheckBox13);
            this.groupBox10.Controls.Add(this.CheckBox14);
            this.groupBox10.Controls.Add(this.CheckBox15);
            this.groupBox10.ForeColor = System.Drawing.Color.White;
            this.groupBox10.Location = new System.Drawing.Point(8, 170);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(289, 95);
            this.groupBox10.TabIndex = 24;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Caller\'s Choice of Language (select all that apply)";
            // 
            // txtLanguageOther
            // 
            this.txtLanguageOther.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.languageBindingSource, "OtherDescription", true));
            this.txtLanguageOther.Location = new System.Drawing.Point(174, 40);
            this.txtLanguageOther.Name = "txtLanguageOther";
            this.txtLanguageOther.Size = new System.Drawing.Size(94, 20);
            this.txtLanguageOther.TabIndex = 15;
            // 
            // languageBindingSource
            // 
            this.languageBindingSource.DataSource = typeof(INCIDENTRP.Language);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(119, 43);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(49, 13);
            this.label40.TabIndex = 14;
            this.label40.Text = "Describe";
            // 
            // chkLanguageOther
            // 
            this.chkLanguageOther.AutoSize = true;
            this.chkLanguageOther.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.languageBindingSource, "Other", true));
            this.chkLanguageOther.Location = new System.Drawing.Point(122, 19);
            this.chkLanguageOther.Name = "chkLanguageOther";
            this.chkLanguageOther.Size = new System.Drawing.Size(52, 17);
            this.chkLanguageOther.TabIndex = 13;
            this.chkLanguageOther.Text = "Other";
            this.chkLanguageOther.UseVisualStyleBackColor = true;
            // 
            // CheckBox13
            // 
            this.CheckBox13.AutoSize = true;
            this.CheckBox13.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.languageBindingSource, "FoulOrProfane", true));
            this.CheckBox13.Location = new System.Drawing.Point(6, 65);
            this.CheckBox13.Name = "CheckBox13";
            this.CheckBox13.Size = new System.Drawing.Size(98, 17);
            this.CheckBox13.TabIndex = 10;
            this.CheckBox13.Text = "Foul or Profane";
            this.CheckBox13.UseVisualStyleBackColor = true;
            // 
            // CheckBox14
            // 
            this.CheckBox14.AutoSize = true;
            this.CheckBox14.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.languageBindingSource, "Uneducated", true));
            this.CheckBox14.Location = new System.Drawing.Point(6, 42);
            this.CheckBox14.Name = "CheckBox14";
            this.CheckBox14.Size = new System.Drawing.Size(85, 17);
            this.CheckBox14.TabIndex = 9;
            this.CheckBox14.Text = "Uneducated";
            this.CheckBox14.UseVisualStyleBackColor = true;
            // 
            // CheckBox15
            // 
            this.CheckBox15.AutoSize = true;
            this.CheckBox15.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.languageBindingSource, "Educated", true));
            this.CheckBox15.Location = new System.Drawing.Point(6, 19);
            this.CheckBox15.Name = "CheckBox15";
            this.CheckBox15.Size = new System.Drawing.Size(72, 17);
            this.CheckBox15.TabIndex = 8;
            this.CheckBox15.Text = "Educated";
            this.CheckBox15.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.CheckBox35);
            this.groupBox11.Controls.Add(this.CheckBox36);
            this.groupBox11.Controls.Add(this.CheckBox37);
            this.groupBox11.Controls.Add(this.txtBackgroundNoiseOther);
            this.groupBox11.Controls.Add(this.label41);
            this.groupBox11.Controls.Add(this.chkBackgroundNoiseOther);
            this.groupBox11.Controls.Add(this.CheckBox39);
            this.groupBox11.Controls.Add(this.CheckBox40);
            this.groupBox11.Controls.Add(this.CheckBox41);
            this.groupBox11.Controls.Add(this.CheckBox43);
            this.groupBox11.Controls.Add(this.CheckBox44);
            this.groupBox11.Controls.Add(this.CheckBox45);
            this.groupBox11.Controls.Add(this.CheckBox46);
            this.groupBox11.Controls.Add(this.CheckBox47);
            this.groupBox11.Controls.Add(this.CheckBox48);
            this.groupBox11.ForeColor = System.Drawing.Color.White;
            this.groupBox11.Location = new System.Drawing.Point(303, 220);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(287, 185);
            this.groupBox11.TabIndex = 27;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Background Noise (select all that apply)";
            // 
            // CheckBox35
            // 
            this.CheckBox35.AutoSize = true;
            this.CheckBox35.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "Trains", true));
            this.CheckBox35.Location = new System.Drawing.Point(131, 111);
            this.CheckBox35.Name = "CheckBox35";
            this.CheckBox35.Size = new System.Drawing.Size(55, 17);
            this.CheckBox35.TabIndex = 15;
            this.CheckBox35.Text = "Trains";
            this.CheckBox35.UseVisualStyleBackColor = true;
            // 
            // backgroundNoiseBindingSource
            // 
            this.backgroundNoiseBindingSource.DataSource = typeof(INCIDENTRP.BackgroundNoise);
            // 
            // CheckBox36
            // 
            this.CheckBox36.AutoSize = true;
            this.CheckBox36.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "StreetTraffic", true));
            this.CheckBox36.Location = new System.Drawing.Point(131, 88);
            this.CheckBox36.Name = "CheckBox36";
            this.CheckBox36.Size = new System.Drawing.Size(87, 17);
            this.CheckBox36.TabIndex = 14;
            this.CheckBox36.Text = "Street Traffic";
            this.CheckBox36.UseVisualStyleBackColor = true;
            // 
            // CheckBox37
            // 
            this.CheckBox37.AutoSize = true;
            this.CheckBox37.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "OfficeMachines", true));
            this.CheckBox37.Location = new System.Drawing.Point(6, 157);
            this.CheckBox37.Name = "CheckBox37";
            this.CheckBox37.Size = new System.Drawing.Size(103, 17);
            this.CheckBox37.TabIndex = 13;
            this.CheckBox37.Text = "Office Machines";
            this.CheckBox37.UseVisualStyleBackColor = true;
            // 
            // txtBackgroundNoiseOther
            // 
            this.txtBackgroundNoiseOther.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.backgroundNoiseBindingSource, "OtherDescription", true));
            this.txtBackgroundNoiseOther.Location = new System.Drawing.Point(183, 155);
            this.txtBackgroundNoiseOther.Name = "txtBackgroundNoiseOther";
            this.txtBackgroundNoiseOther.Size = new System.Drawing.Size(94, 20);
            this.txtBackgroundNoiseOther.TabIndex = 12;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(128, 158);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(49, 13);
            this.label41.TabIndex = 11;
            this.label41.Text = "Describe";
            // 
            // chkBackgroundNoiseOther
            // 
            this.chkBackgroundNoiseOther.AutoSize = true;
            this.chkBackgroundNoiseOther.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "Other", true));
            this.chkBackgroundNoiseOther.Location = new System.Drawing.Point(131, 134);
            this.chkBackgroundNoiseOther.Name = "chkBackgroundNoiseOther";
            this.chkBackgroundNoiseOther.Size = new System.Drawing.Size(52, 17);
            this.chkBackgroundNoiseOther.TabIndex = 10;
            this.chkBackgroundNoiseOther.Text = "Other";
            this.chkBackgroundNoiseOther.UseVisualStyleBackColor = true;
            // 
            // CheckBox39
            // 
            this.CheckBox39.AutoSize = true;
            this.CheckBox39.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "SchoolBell", true));
            this.CheckBox39.Location = new System.Drawing.Point(131, 65);
            this.CheckBox39.Name = "CheckBox39";
            this.CheckBox39.Size = new System.Drawing.Size(79, 17);
            this.CheckBox39.TabIndex = 9;
            this.CheckBox39.Text = "School Bell";
            this.CheckBox39.UseVisualStyleBackColor = true;
            // 
            // CheckBox40
            // 
            this.CheckBox40.AutoSize = true;
            this.CheckBox40.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "PublicAddressSystem", true));
            this.CheckBox40.Location = new System.Drawing.Point(131, 42);
            this.CheckBox40.Name = "CheckBox40";
            this.CheckBox40.Size = new System.Drawing.Size(133, 17);
            this.CheckBox40.TabIndex = 8;
            this.CheckBox40.Text = "Public Address System";
            this.CheckBox40.UseVisualStyleBackColor = true;
            // 
            // CheckBox41
            // 
            this.CheckBox41.AutoSize = true;
            this.CheckBox41.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "Party", true));
            this.CheckBox41.Location = new System.Drawing.Point(131, 19);
            this.CheckBox41.Name = "CheckBox41";
            this.CheckBox41.Size = new System.Drawing.Size(50, 17);
            this.CheckBox41.TabIndex = 7;
            this.CheckBox41.Text = "Party";
            this.CheckBox41.UseVisualStyleBackColor = true;
            // 
            // CheckBox43
            // 
            this.CheckBox43.AutoSize = true;
            this.CheckBox43.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "Music", true));
            this.CheckBox43.Location = new System.Drawing.Point(6, 134);
            this.CheckBox43.Name = "CheckBox43";
            this.CheckBox43.Size = new System.Drawing.Size(54, 17);
            this.CheckBox43.TabIndex = 5;
            this.CheckBox43.Text = "Music";
            this.CheckBox43.UseVisualStyleBackColor = true;
            // 
            // CheckBox44
            // 
            this.CheckBox44.AutoSize = true;
            this.CheckBox44.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "FactoryMachines", true));
            this.CheckBox44.Location = new System.Drawing.Point(6, 111);
            this.CheckBox44.Name = "CheckBox44";
            this.CheckBox44.Size = new System.Drawing.Size(110, 17);
            this.CheckBox44.TabIndex = 4;
            this.CheckBox44.Text = "Factory Machines";
            this.CheckBox44.UseVisualStyleBackColor = true;
            // 
            // CheckBox45
            // 
            this.CheckBox45.AutoSize = true;
            this.CheckBox45.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "Crowd", true));
            this.CheckBox45.Location = new System.Drawing.Point(6, 88);
            this.CheckBox45.Name = "CheckBox45";
            this.CheckBox45.Size = new System.Drawing.Size(56, 17);
            this.CheckBox45.TabIndex = 3;
            this.CheckBox45.Text = "Crowd";
            this.CheckBox45.UseVisualStyleBackColor = true;
            // 
            // CheckBox46
            // 
            this.CheckBox46.AutoSize = true;
            this.CheckBox46.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "Conversation", true));
            this.CheckBox46.Location = new System.Drawing.Point(6, 65);
            this.CheckBox46.Name = "CheckBox46";
            this.CheckBox46.Size = new System.Drawing.Size(88, 17);
            this.CheckBox46.TabIndex = 2;
            this.CheckBox46.Text = "Conversation";
            this.CheckBox46.UseVisualStyleBackColor = true;
            // 
            // CheckBox47
            // 
            this.CheckBox47.AutoSize = true;
            this.CheckBox47.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "Animals", true));
            this.CheckBox47.Location = new System.Drawing.Point(6, 42);
            this.CheckBox47.Name = "CheckBox47";
            this.CheckBox47.Size = new System.Drawing.Size(62, 17);
            this.CheckBox47.TabIndex = 1;
            this.CheckBox47.Text = "Animals";
            this.CheckBox47.UseVisualStyleBackColor = true;
            // 
            // CheckBox48
            // 
            this.CheckBox48.AutoSize = true;
            this.CheckBox48.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.backgroundNoiseBindingSource, "Airplanes", true));
            this.CheckBox48.Location = new System.Drawing.Point(6, 19);
            this.CheckBox48.Name = "CheckBox48";
            this.CheckBox48.Size = new System.Drawing.Size(69, 17);
            this.CheckBox48.TabIndex = 0;
            this.CheckBox48.Text = "Airplanes";
            this.CheckBox48.UseVisualStyleBackColor = true;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.ForeColor = System.Drawing.Color.White;
            this.label42.Location = new System.Drawing.Point(11, 405);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(261, 13);
            this.label42.TabIndex = 29;
            this.label42.Text = "Additional Remarks/Comments about the Call or Caller";
            // 
            // threatBindingSource
            // 
            this.threatBindingSource.DataSource = typeof(INCIDENTRP.Threat);
            // 
            // textBox17
            // 
            this.textBox17.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketBindingSource, "StatusDate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, null, "d"));
            this.textBox17.Location = new System.Drawing.Point(386, 118);
            this.textBox17.Name = "textBox17";
            this.textBox17.ReadOnly = true;
            this.textBox17.Size = new System.Drawing.Size(204, 20);
            this.textBox17.TabIndex = 280;
            // 
            // textBox23
            // 
            this.textBox23.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketBindingSource, "CourtDate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, null, "d"));
            this.textBox23.Location = new System.Drawing.Point(386, 149);
            this.textBox23.Name = "textBox23";
            this.textBox23.ReadOnly = true;
            this.textBox23.Size = new System.Drawing.Size(204, 20);
            this.textBox23.TabIndex = 281;
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.LinkColor = System.Drawing.Color.Yellow;
            this.lnkSave.Location = new System.Drawing.Point(829, 54);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(32, 13);
            this.lnkSave.TabIndex = 282;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "Save";
            this.lnkSave.Click += new System.EventHandler(this.LnkSave_Click);
            // 
            // ThreatDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(1192, 769);
            this.Controls.Add(this.lnkSave);
            this.Controls.Add(this.textBox23);
            this.Controls.Add(this.textBox17);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpIncidentTime);
            this.Controls.Add(this.dtpIncidentDate);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblHistory);
            this.Controls.Add(this.lblUpdate);
            this.Controls.Add(this.txtUpdate);
            this.Controls.Add(this.txtHistory);
            this.Controls.Add(this.lblCourtDate);
            this.Controls.Add(this.lblCourt);
            this.Controls.Add(this.lblStatusDate);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblIncidentDate);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.lblTicket);
            this.Controls.Add(this.pbxCompanyLogo);
            this.Controls.Add(this.cmbCourt);
            this.Controls.Add(this.lnkUpdate);
            this.Controls.Add(this.lnkNextFlowStep);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblPriority);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblIncidentTime);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1208, 808);
            this.MinimumSize = new System.Drawing.Size(1208, 808);
            this.Name = "ThreatDisplay";
            this.Text = "ThreatDisplay";
            ((System.ComponentModel.ISupportInitialize)(this.ticketBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCompanyLogo)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.GroupBox7.ResumeLayout(false);
            this.GroupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bombInfoBindingSource)).EndInit();
            this.GroupBox6.ResumeLayout(false);
            this.GroupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threatInfoBindingSource)).EndInit();
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            this.grpLawEnforcementContactInfo.ResumeLayout(false);
            this.grpLawEnforcementContactInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lawActionBindingSource)).EndInit();
            this.grpInformationSecurityContactInfo.ResumeLayout(false);
            this.grpInformationSecurityContactInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoTechActionBindingSource)).EndInit();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.notifierBindingSource)).EndInit();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reporterBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessUnitBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporterNameBindingSource)).EndInit();
            this.GroupBox8.ResumeLayout(false);
            this.GroupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.callerBindingSource)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.voiceBindingSource)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mannerBindingSource)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dialectBindingSource)).EndInit();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.languageBindingSource)).EndInit();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundNoiseBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threatBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dtpIncidentTime;
        private System.Windows.Forms.DateTimePicker dtpIncidentDate;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblHistory;
        private System.Windows.Forms.Label lblUpdate;
        private System.Windows.Forms.TextBox txtUpdate;
        private System.Windows.Forms.TextBox txtHistory;
        private System.Windows.Forms.Label lblCourtDate;
        private System.Windows.Forms.Label lblCourt;
        private System.Windows.Forms.Label lblStatusDate;
		private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblIncidentDate;
        private System.Windows.Forms.TextBox txtStatus;
		private System.Windows.Forms.Label lblTicket;
        private System.Windows.Forms.PictureBox pbxCompanyLogo;
		private System.Windows.Forms.ComboBox cmbCourt;
        private System.Windows.Forms.LinkLabel lnkUpdate;
		private System.Windows.Forms.LinkLabel lnkNextFlowStep;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPriority;
		private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblIncidentTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        internal System.Windows.Forms.GroupBox GroupBox7;
        internal System.Windows.Forms.Label Label23;
        internal System.Windows.Forms.TextBox TextBox13;
        private System.Windows.Forms.BindingSource bombInfoBindingSource;
        internal System.Windows.Forms.Label Label24;
        internal System.Windows.Forms.TextBox TextBox14;
        internal System.Windows.Forms.Label Label20;
        internal System.Windows.Forms.TextBox TextBox10;
        internal System.Windows.Forms.Label Label21;
        internal System.Windows.Forms.TextBox TextBox11;
        internal System.Windows.Forms.Label Label22;
        internal System.Windows.Forms.TextBox TextBox12;
        internal System.Windows.Forms.GroupBox GroupBox6;
        internal System.Windows.Forms.Label Label19;
        internal System.Windows.Forms.TextBox TextBox9;
        private System.Windows.Forms.BindingSource threatInfoBindingSource;
        internal System.Windows.Forms.Label Label18;
        internal System.Windows.Forms.TextBox TextBox8;
        internal System.Windows.Forms.GroupBox GroupBox3;
        internal System.Windows.Forms.GroupBox grpLawEnforcementContactInfo;
        internal System.Windows.Forms.Label Label15;
        internal System.Windows.Forms.Label Label16;
        internal System.Windows.Forms.TextBox TextBox7;
        private System.Windows.Forms.BindingSource lawActionBindingSource;
        internal System.Windows.Forms.DateTimePicker dateTimePicker7;
        internal System.Windows.Forms.DateTimePicker ContactLawDate;
        internal System.Windows.Forms.Label Label17;
        internal System.Windows.Forms.CheckBox chkContactedLawEnforcement;
        internal System.Windows.Forms.GroupBox grpInformationSecurityContactInfo;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.Label Label13;
        internal System.Windows.Forms.TextBox TextBox6;
        private System.Windows.Forms.BindingSource infoTechActionBindingSource;
        internal System.Windows.Forms.DateTimePicker ContactITTime;
        internal System.Windows.Forms.DateTimePicker ContactITDate;
        internal System.Windows.Forms.Label Label14;
        internal System.Windows.Forms.CheckBox chkContactedInformationSecurity;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.TextBox TextBox5;
        private System.Windows.Forms.BindingSource notifierBindingSource;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.TextBox TextBox4;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.TextBox TextBox3;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.ComboBox cmbNotifierType;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.ComboBox cmbNotificationMethod;
        private System.Windows.Forms.BindingSource threatBindingSource;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox TextBox2;
        private System.Windows.Forms.BindingSource reporterBindingSource;
        internal System.Windows.Forms.Label label27;
        internal System.Windows.Forms.TextBox txtReporterEmailAddress;
        internal System.Windows.Forms.Label label28;
        internal System.Windows.Forms.ComboBox cmbBusinessUnit;
        internal System.Windows.Forms.Label label29;
        internal System.Windows.Forms.ComboBox cmbReporter;
        internal System.Windows.Forms.GroupBox GroupBox8;
        internal System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox textBox21;
        private System.Windows.Forms.BindingSource callerBindingSource;
        internal System.Windows.Forms.Label Label32;
        internal System.Windows.Forms.TextBox TextBox18;
        internal System.Windows.Forms.Label Label33;
        internal System.Windows.Forms.TextBox TextBox20;
        internal System.Windows.Forms.CheckBox chkGenderUnsure;
        internal System.Windows.Forms.CheckBox chkGenderFemale;
        internal System.Windows.Forms.CheckBox chkGenderMale;
        internal System.Windows.Forms.CheckBox chkFamiliarNo;
        internal System.Windows.Forms.CheckBox chkFamiliarYes;
        internal System.Windows.Forms.CheckBox chkRecognizedVoiceNo;
        internal System.Windows.Forms.CheckBox chkRecognizedVoiceYes;
        internal System.Windows.Forms.Label Label31;
        internal System.Windows.Forms.Label Label30;
        internal System.Windows.Forms.Label Label25;
        internal System.Windows.Forms.TextBox TextBox15;
        internal System.Windows.Forms.Label Label26;
        internal System.Windows.Forms.TextBox TextBox16;
        internal System.Windows.Forms.Label label35;
        internal System.Windows.Forms.TextBox textBox19;
        internal System.Windows.Forms.Label label36;
        internal System.Windows.Forms.Label label37;
        internal System.Windows.Forms.TextBox textBox22;
        internal System.Windows.Forms.GroupBox groupBox4;
        internal System.Windows.Forms.TextBox txtVoiceOther;
        private System.Windows.Forms.BindingSource voiceBindingSource;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.CheckBox chkVoiceOther;
        internal System.Windows.Forms.CheckBox CheckBox9;
        internal System.Windows.Forms.CheckBox CheckBox10;
        internal System.Windows.Forms.CheckBox CheckBox11;
        internal System.Windows.Forms.CheckBox CheckBox12;
        internal System.Windows.Forms.CheckBox CheckBox4;
        internal System.Windows.Forms.CheckBox CheckBox5;
        internal System.Windows.Forms.CheckBox CheckBox6;
        internal System.Windows.Forms.CheckBox CheckBox3;
        internal System.Windows.Forms.CheckBox CheckBox2;
        internal System.Windows.Forms.CheckBox CheckBox1;
        internal System.Windows.Forms.GroupBox groupBox5;
        internal System.Windows.Forms.CheckBox CheckBox32;
        private System.Windows.Forms.BindingSource mannerBindingSource;
        internal System.Windows.Forms.CheckBox CheckBox33;
        internal System.Windows.Forms.CheckBox CheckBox31;
        internal System.Windows.Forms.CheckBox CheckBox30;
        internal System.Windows.Forms.TextBox txtMannerOther;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.CheckBox chkMannerOther;
        internal System.Windows.Forms.CheckBox CheckBox19;
        internal System.Windows.Forms.CheckBox CheckBox20;
        internal System.Windows.Forms.CheckBox CheckBox21;
        internal System.Windows.Forms.CheckBox CheckBox22;
        internal System.Windows.Forms.CheckBox CheckBox23;
        internal System.Windows.Forms.CheckBox CheckBox24;
        internal System.Windows.Forms.CheckBox CheckBox25;
        internal System.Windows.Forms.CheckBox CheckBox27;
        internal System.Windows.Forms.CheckBox CheckBox28;
        internal System.Windows.Forms.CheckBox CheckBox29;
        internal System.Windows.Forms.TextBox txtRemarks;
        internal System.Windows.Forms.GroupBox groupBox9;
        internal System.Windows.Forms.TextBox txtDialectRegionalAmericanOther;
        private System.Windows.Forms.BindingSource dialectBindingSource;
        internal System.Windows.Forms.Label label38;
        internal System.Windows.Forms.CheckBox chkDialectRegionalAmericanOther;
        internal System.Windows.Forms.TextBox txtDialectForeignAccentOther;
        internal System.Windows.Forms.Label label39;
        internal System.Windows.Forms.CheckBox chkDialectForeignAccentOther;
        internal System.Windows.Forms.CheckBox CheckBox26;
        internal System.Windows.Forms.GroupBox groupBox10;
        internal System.Windows.Forms.TextBox txtLanguageOther;
        private System.Windows.Forms.BindingSource languageBindingSource;
        internal System.Windows.Forms.Label label40;
        internal System.Windows.Forms.CheckBox chkLanguageOther;
        internal System.Windows.Forms.CheckBox CheckBox13;
        internal System.Windows.Forms.CheckBox CheckBox14;
        internal System.Windows.Forms.CheckBox CheckBox15;
        internal System.Windows.Forms.GroupBox groupBox11;
        internal System.Windows.Forms.CheckBox CheckBox35;
        private System.Windows.Forms.BindingSource backgroundNoiseBindingSource;
        internal System.Windows.Forms.CheckBox CheckBox36;
        internal System.Windows.Forms.CheckBox CheckBox37;
        internal System.Windows.Forms.TextBox txtBackgroundNoiseOther;
        internal System.Windows.Forms.Label label41;
        internal System.Windows.Forms.CheckBox chkBackgroundNoiseOther;
        internal System.Windows.Forms.CheckBox CheckBox39;
        internal System.Windows.Forms.CheckBox CheckBox40;
        internal System.Windows.Forms.CheckBox CheckBox41;
        internal System.Windows.Forms.CheckBox CheckBox43;
        internal System.Windows.Forms.CheckBox CheckBox44;
        internal System.Windows.Forms.CheckBox CheckBox45;
        internal System.Windows.Forms.CheckBox CheckBox46;
        internal System.Windows.Forms.CheckBox CheckBox47;
        internal System.Windows.Forms.CheckBox CheckBox48;
		internal System.Windows.Forms.Label label42;
		private System.Windows.Forms.BindingSource ticketBindingSource;
		private System.Windows.Forms.BindingSource businessUnitBindingSource;
		private System.Windows.Forms.BindingSource reporterNameBindingSource;
		private System.Windows.Forms.TextBox textBox17;
		private System.Windows.Forms.TextBox textBox23;
		private System.Windows.Forms.LinkLabel lnkSave;
		internal System.Windows.Forms.Label lblOtherMethod;
		private System.Windows.Forms.TextBox txtOtherMethod;
		internal System.Windows.Forms.Label lblOtherType;
		private System.Windows.Forms.TextBox txtOtherType;
    }
}