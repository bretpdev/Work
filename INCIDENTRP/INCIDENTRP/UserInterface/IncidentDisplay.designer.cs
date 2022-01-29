using SubSystemShared;
using Uheaa.Common.ProcessLogger;

namespace INCIDENTRP
{
    partial class IncidentDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IncidentDisplay));
            this.lblHLine = new System.Windows.Forms.Label();
            this.pnlTicket = new System.Windows.Forms.Panel();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.incidentBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblTicket = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ticketBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pbxCompanyLogo = new System.Windows.Forms.PictureBox();
            this.lnkUpdate = new System.Windows.Forms.LinkLabel();
            this.lnkNextFlowStep = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTicketNumber = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPriority = new System.Windows.Forms.Label();
            this.emailRecipientsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblCourtDate = new System.Windows.Forms.Label();
            this.lblCourt = new System.Windows.Forms.Label();
            this.lblStatusDate = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblIncidentDate = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.cmbCourt = new System.Windows.Forms.ComboBox();
            this.activeDirectoryUserBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblIncidentTime = new System.Windows.Forms.Label();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.txtUpdate = new System.Windows.Forms.TextBox();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.lblHistory = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabIncidentInformation = new System.Windows.Forms.TabPage();
            this.lblOtherRelationship = new System.Windows.Forms.Label();
            this.txtOtherRelationship = new System.Windows.Forms.TextBox();
            this.notifierBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbNotifierRelationship = new System.Windows.Forms.ComboBox();
            this.label41 = new System.Windows.Forms.Label();
            this.lblOtherType = new System.Windows.Forms.Label();
            this.txtOtherType = new System.Windows.Forms.TextBox();
            this.lblOtherMethod = new System.Windows.Forms.Label();
            this.txtOtherMethod = new System.Windows.Forms.TextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.cmbCause = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.cmbNotifierType = new System.Windows.Forms.ComboBox();
            this.cmbNotificationMethod = new System.Windows.Forms.ComboBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbBusinessUnit = new System.Windows.Forms.ComboBox();
            this.reporterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.businessUnitBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbReporterName = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.txtReporterEmailAddress = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabIncidentTypeDetails = new System.Windows.Forms.TabPage();
            this.cmbIncidentType = new System.Windows.Forms.ComboBox();
            this.pnlIncidentTypeDetails = new System.Windows.Forms.Panel();
            this.tabDataInvolved = new System.Windows.Forms.TabPage();
            this.cmbDataInvolved = new System.Windows.Forms.ComboBox();
            this.pnlDataInvolved = new System.Windows.Forms.Panel();
            this.tabActionsTaken = new System.Windows.Forms.TabPage();
            this.lblActionsTaken = new System.Windows.Forms.Label();
            this.pnlActionsTaken = new System.Windows.Forms.FlowLayoutPanel();
            this.chkContactedInfoTech = new System.Windows.Forms.CheckBox();
            this.contactedInfoTechBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpContactedInfoTech = new System.Windows.Forms.GroupBox();
            this.ContactedITDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.chkNotifiedAffectedIndividual = new System.Windows.Forms.CheckBox();
            this.notifiedAffectedIndividualBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpNotifiedAffectedIndividual = new System.Windows.Forms.GroupBox();
            this.NotifiedDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker6 = new System.Windows.Forms.DateTimePicker();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.chkAskedCallerToReturnCorrespondence = new System.Windows.Forms.CheckBox();
            this.askedCallerToReturnCorrespondenceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpAskedCallerToReturnCorrespondence = new System.Windows.Forms.GroupBox();
            this.AskedDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker8 = new System.Windows.Forms.DateTimePicker();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.chkDeletedFiles = new System.Windows.Forms.CheckBox();
            this.deletedFilesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpDeletedFiles = new System.Windows.Forms.GroupBox();
            this.DeletedDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker10 = new System.Windows.Forms.DateTimePicker();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.chkCorrectedData = new System.Windows.Forms.CheckBox();
            this.correctedDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpCorrectedData = new System.Windows.Forms.GroupBox();
            this.CorrectedDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker12 = new System.Windows.Forms.DateTimePicker();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.chkRemovedSystemFromNetwork = new System.Windows.Forms.CheckBox();
            this.removedSystemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpRemovedSystemFromNetwork = new System.Windows.Forms.GroupBox();
            this.RemovedDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker14 = new System.Windows.Forms.DateTimePicker();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.chkRebootedSystem = new System.Windows.Forms.CheckBox();
            this.rebootedSystemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpRebootedSystem = new System.Windows.Forms.GroupBox();
            this.RebootedDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker16 = new System.Windows.Forms.DateTimePicker();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.chkLoggedOffSystem = new System.Windows.Forms.CheckBox();
            this.loggedOffSystemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpLoggedOffSystem = new System.Windows.Forms.GroupBox();
            this.LoggedOffDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker18 = new System.Windows.Forms.DateTimePicker();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.chkShutDownSystem = new System.Windows.Forms.CheckBox();
            this.shutDownSystemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpShutDownSystem = new System.Windows.Forms.GroupBox();
            this.ShutDownDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker20 = new System.Windows.Forms.DateTimePicker();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.chkContactedLawEnforcement = new System.Windows.Forms.CheckBox();
            this.contactedLawEnforcementBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpContactedLawEnforcement = new System.Windows.Forms.GroupBox();
            this.ContactedDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker22 = new System.Windows.Forms.DateTimePicker();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.IncidentDate = new System.Windows.Forms.DateTimePicker();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.pnlTicket.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.incidentBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCompanyLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activeDirectoryUserBindingSource)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabIncidentInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.notifierBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporterBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessUnitBindingSource)).BeginInit();
            this.tabIncidentTypeDetails.SuspendLayout();
            this.tabDataInvolved.SuspendLayout();
            this.tabActionsTaken.SuspendLayout();
            this.pnlActionsTaken.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contactedInfoTechBindingSource)).BeginInit();
            this.grpContactedInfoTech.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.notifiedAffectedIndividualBindingSource)).BeginInit();
            this.grpNotifiedAffectedIndividual.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.askedCallerToReturnCorrespondenceBindingSource)).BeginInit();
            this.grpAskedCallerToReturnCorrespondence.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deletedFilesBindingSource)).BeginInit();
            this.grpDeletedFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.correctedDataBindingSource)).BeginInit();
            this.grpCorrectedData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.removedSystemBindingSource)).BeginInit();
            this.grpRemovedSystemFromNetwork.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rebootedSystemBindingSource)).BeginInit();
            this.grpRebootedSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loggedOffSystemBindingSource)).BeginInit();
            this.grpLoggedOffSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shutDownSystemBindingSource)).BeginInit();
            this.grpShutDownSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contactedLawEnforcementBindingSource)).BeginInit();
            this.grpContactedLawEnforcement.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHLine
            // 
            this.lblHLine.BackColor = System.Drawing.Color.Transparent;
            this.lblHLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblHLine.CausesValidation = false;
            this.lblHLine.ForeColor = System.Drawing.Color.White;
            this.lblHLine.Location = new System.Drawing.Point(-1, 81);
            this.lblHLine.Name = "lblHLine";
            this.lblHLine.Size = new System.Drawing.Size(1190, 2);
            this.lblHLine.TabIndex = 2;
            // 
            // pnlTicket
            // 
            this.pnlTicket.BackColor = System.Drawing.Color.Transparent;
            this.pnlTicket.Controls.Add(this.lnkSave);
            this.pnlTicket.Controls.Add(this.label1);
            this.pnlTicket.Controls.Add(this.lblTicket);
            this.pnlTicket.Controls.Add(this.textBox1);
            this.pnlTicket.Controls.Add(this.pbxCompanyLogo);
            this.pnlTicket.Controls.Add(this.lnkUpdate);
            this.pnlTicket.Controls.Add(this.lnkNextFlowStep);
            this.pnlTicket.Controls.Add(this.label4);
            this.pnlTicket.Controls.Add(this.lblHLine);
            this.pnlTicket.Controls.Add(this.lblTicketNumber);
            this.pnlTicket.Controls.Add(this.lblTitle);
            this.pnlTicket.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTicket.Location = new System.Drawing.Point(0, 0);
            this.pnlTicket.Name = "pnlTicket";
            this.pnlTicket.Size = new System.Drawing.Size(1192, 86);
            this.pnlTicket.TabIndex = 149;
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.LinkColor = System.Drawing.Color.Yellow;
            this.lnkSave.Location = new System.Drawing.Point(813, 52);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(32, 13);
            this.lnkSave.TabIndex = 283;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "Save";
            this.lnkSave.Click += new System.EventHandler(this.LnkSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.incidentBindingSource, "Type", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.label1.Location = new System.Drawing.Point(172, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 38);
            this.label1.TabIndex = 227;
            this.label1.Text = "Incident Type";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // incidentBindingSource
            // 
            this.incidentBindingSource.DataSource = typeof(INCIDENTRP.Incident);
            // 
            // lblTicket
            // 
            this.lblTicket.AutoSize = true;
            this.lblTicket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblTicket.Location = new System.Drawing.Point(998, 24);
            this.lblTicket.Name = "lblTicket";
            this.lblTicket.Size = new System.Drawing.Size(40, 13);
            this.lblTicket.TabIndex = 165;
            this.lblTicket.Text = "Ticket:";
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketBindingSource, "Number", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox1.Location = new System.Drawing.Point(1041, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 164;
            // 
            // ticketBindingSource
            // 
            this.ticketBindingSource.DataSource = typeof(INCIDENTRP.Ticket);
            // 
            // pbxCompanyLogo
            // 
            this.pbxCompanyLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxCompanyLogo.Image = global::INCIDENTRP.Properties.Resources.UheaaLogo;
            this.pbxCompanyLogo.Location = new System.Drawing.Point(21, 10);
            this.pbxCompanyLogo.Name = "pbxCompanyLogo";
            this.pbxCompanyLogo.Size = new System.Drawing.Size(145, 68);
            this.pbxCompanyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxCompanyLogo.TabIndex = 161;
            this.pbxCompanyLogo.TabStop = false;
            // 
            // lnkUpdate
            // 
            this.lnkUpdate.AutoSize = true;
            this.lnkUpdate.LinkColor = System.Drawing.Color.Yellow;
            this.lnkUpdate.Location = new System.Drawing.Point(851, 52);
            this.lnkUpdate.Name = "lnkUpdate";
            this.lnkUpdate.Size = new System.Drawing.Size(42, 13);
            this.lnkUpdate.TabIndex = 154;
            this.lnkUpdate.TabStop = true;
            this.lnkUpdate.Text = "Update";
            this.lnkUpdate.Click += new System.EventHandler(this.LnkUpdate_Click);
            // 
            // lnkNextFlowStep
            // 
            this.lnkNextFlowStep.AutoSize = true;
            this.lnkNextFlowStep.LinkColor = System.Drawing.Color.Yellow;
            this.lnkNextFlowStep.Location = new System.Drawing.Point(899, 52);
            this.lnkNextFlowStep.Name = "lnkNextFlowStep";
            this.lnkNextFlowStep.Size = new System.Drawing.Size(64, 13);
            this.lnkNextFlowStep.TabIndex = 153;
            this.lnkNextFlowStep.TabStop = true;
            this.lnkNextFlowStep.Text = "Move Along";
            this.lnkNextFlowStep.Visible = false;
            this.lnkNextFlowStep.Click += new System.EventHandler(this.LnkNextFlowStep_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.CausesValidation = false;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(-1, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1190, 2);
            this.label4.TabIndex = 2;
            // 
            // lblTicketNumber
            // 
            this.lblTicketNumber.AutoSize = true;
            this.lblTicketNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblTicketNumber.Location = new System.Drawing.Point(561, 11);
            this.lblTicketNumber.Name = "lblTicketNumber";
            this.lblTicketNumber.Size = new System.Drawing.Size(0, 24);
            this.lblTicketNumber.TabIndex = 129;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Century Schoolbook", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblTitle.Location = new System.Drawing.Point(172, 37);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(248, 38);
            this.lblTitle.TabIndex = 162;
            this.lblTitle.Text = "Incident Report";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.label3.Location = new System.Drawing.Point(1000, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 147;
            this.label3.Text = "Priority";
            // 
            // lblPriority
            // 
            this.lblPriority.AutoSize = true;
            this.lblPriority.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketBindingSource, "Priority", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblPriority.Font = new System.Drawing.Font("Century Schoolbook", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriority.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblPriority.Location = new System.Drawing.Point(988, 91);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.Size = new System.Drawing.Size(69, 77);
            this.lblPriority.TabIndex = 163;
            this.lblPriority.Text = "9";
            // 
            // emailRecipientsToolStripMenuItem
            // 
            this.emailRecipientsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.emailRecipientsToolStripMenuItem.Name = "emailRecipientsToolStripMenuItem";
            this.emailRecipientsToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.emailRecipientsToolStripMenuItem.Text = "Email Recipients";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            // 
            // uploadFileToolStripMenuItem
            // 
            this.uploadFileToolStripMenuItem.Name = "uploadFileToolStripMenuItem";
            this.uploadFileToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.uploadFileToolStripMenuItem.Text = "Upload File";
            // 
            // lblCourtDate
            // 
            this.lblCourtDate.AutoSize = true;
            this.lblCourtDate.BackColor = System.Drawing.Color.Transparent;
            this.lblCourtDate.ForeColor = System.Drawing.Color.White;
            this.lblCourtDate.Location = new System.Drawing.Point(317, 150);
            this.lblCourtDate.Name = "lblCourtDate";
            this.lblCourtDate.Size = new System.Drawing.Size(61, 13);
            this.lblCourtDate.TabIndex = 209;
            this.lblCourtDate.Text = "Court Date:";
            // 
            // lblCourt
            // 
            this.lblCourt.AutoSize = true;
            this.lblCourt.BackColor = System.Drawing.Color.Transparent;
            this.lblCourt.ForeColor = System.Drawing.Color.White;
            this.lblCourt.Location = new System.Drawing.Point(11, 150);
            this.lblCourt.Name = "lblCourt";
            this.lblCourt.Size = new System.Drawing.Size(35, 13);
            this.lblCourt.TabIndex = 210;
            this.lblCourt.Text = "Court:";
            // 
            // lblStatusDate
            // 
            this.lblStatusDate.AutoSize = true;
            this.lblStatusDate.BackColor = System.Drawing.Color.Transparent;
            this.lblStatusDate.ForeColor = System.Drawing.Color.White;
            this.lblStatusDate.Location = new System.Drawing.Point(312, 120);
            this.lblStatusDate.Name = "lblStatusDate";
            this.lblStatusDate.Size = new System.Drawing.Size(66, 13);
            this.lblStatusDate.TabIndex = 211;
            this.lblStatusDate.Text = "Status Date:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(6, 120);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 212;
            this.lblStatus.Text = "Status:";
            // 
            // lblIncidentDate
            // 
            this.lblIncidentDate.AutoSize = true;
            this.lblIncidentDate.BackColor = System.Drawing.Color.Transparent;
            this.lblIncidentDate.ForeColor = System.Drawing.Color.White;
            this.lblIncidentDate.Location = new System.Drawing.Point(602, 120);
            this.lblIncidentDate.Name = "lblIncidentDate";
            this.lblIncidentDate.Size = new System.Drawing.Size(74, 13);
            this.lblIncidentDate.TabIndex = 223;
            this.lblIncidentDate.Text = "Incident Date:";
            // 
            // txtStatus
            // 
            this.txtStatus.CausesValidation = false;
            this.txtStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketBindingSource, "Status", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatus.Location = new System.Drawing.Point(55, 116);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(240, 20);
            this.txtStatus.TabIndex = 218;
            // 
            // cmbCourt
            // 
            this.cmbCourt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCourt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCourt.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.ticketBindingSource, "Court", true));
            this.cmbCourt.DataSource = this.activeDirectoryUserBindingSource;
            this.cmbCourt.DisplayMember = "LegalName";
            this.cmbCourt.FormattingEnabled = true;
            this.cmbCourt.Location = new System.Drawing.Point(55, 147);
            this.cmbCourt.Name = "cmbCourt";
            this.cmbCourt.Size = new System.Drawing.Size(240, 21);
            this.cmbCourt.TabIndex = 220;
            this.cmbCourt.ValueMember = "SecurityId";
            this.cmbCourt.SelectedIndexChanged += new System.EventHandler(this.CmbCourt_SelectedIndexChanged);
            // 
            // activeDirectoryUserBindingSource
            // 
            this.activeDirectoryUserBindingSource.DataSource = typeof(SubSystemShared.SqlUser);
            // 
            // lblIncidentTime
            // 
            this.lblIncidentTime.AutoSize = true;
            this.lblIncidentTime.BackColor = System.Drawing.Color.Transparent;
            this.lblIncidentTime.ForeColor = System.Drawing.Color.White;
            this.lblIncidentTime.Location = new System.Drawing.Point(602, 150);
            this.lblIncidentTime.Name = "lblIncidentTime";
            this.lblIncidentTime.Size = new System.Drawing.Size(74, 13);
            this.lblIncidentTime.TabIndex = 222;
            this.lblIncidentTime.Text = "Incident Time:";
            // 
            // lblUpdate
            // 
            this.lblUpdate.AutoSize = true;
            this.lblUpdate.BackColor = System.Drawing.Color.Transparent;
            this.lblUpdate.ForeColor = System.Drawing.Color.White;
            this.lblUpdate.Location = new System.Drawing.Point(813, 183);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(45, 13);
            this.lblUpdate.TabIndex = 228;
            this.lblUpdate.Text = "Update:";
            // 
            // txtUpdate
            // 
            this.txtUpdate.CausesValidation = false;
            this.txtUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdate.Location = new System.Drawing.Point(816, 199);
            this.txtUpdate.Multiline = true;
            this.txtUpdate.Name = "txtUpdate";
            this.txtUpdate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUpdate.Size = new System.Drawing.Size(355, 125);
            this.txtUpdate.TabIndex = 229;
            // 
            // txtHistory
            // 
            this.txtHistory.CausesValidation = false;
            this.txtHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHistory.Location = new System.Drawing.Point(816, 343);
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ReadOnly = true;
            this.txtHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHistory.Size = new System.Drawing.Size(355, 259);
            this.txtHistory.TabIndex = 230;
            // 
            // lblHistory
            // 
            this.lblHistory.AutoSize = true;
            this.lblHistory.BackColor = System.Drawing.Color.Transparent;
            this.lblHistory.ForeColor = System.Drawing.Color.White;
            this.lblHistory.Location = new System.Drawing.Point(813, 327);
            this.lblHistory.Name = "lblHistory";
            this.lblHistory.Size = new System.Drawing.Size(42, 13);
            this.lblHistory.TabIndex = 231;
            this.lblHistory.Text = "History:";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.CausesValidation = false;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1185, 2);
            this.label2.TabIndex = 233;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabIncidentInformation);
            this.tabControl1.Controls.Add(this.tabIncidentTypeDetails);
            this.tabControl1.Controls.Add(this.tabDataInvolved);
            this.tabControl1.Controls.Add(this.tabActionsTaken);
            this.tabControl1.Location = new System.Drawing.Point(12, 183);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(795, 513);
            this.tabControl1.TabIndex = 234;
            // 
            // tabIncidentInformation
            // 
            this.tabIncidentInformation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.tabIncidentInformation.Controls.Add(this.lblOtherRelationship);
            this.tabIncidentInformation.Controls.Add(this.txtOtherRelationship);
            this.tabIncidentInformation.Controls.Add(this.cmbNotifierRelationship);
            this.tabIncidentInformation.Controls.Add(this.label41);
            this.tabIncidentInformation.Controls.Add(this.lblOtherType);
            this.tabIncidentInformation.Controls.Add(this.txtOtherType);
            this.tabIncidentInformation.Controls.Add(this.lblOtherMethod);
            this.tabIncidentInformation.Controls.Add(this.txtOtherMethod);
            this.tabIncidentInformation.Controls.Add(this.label39);
            this.tabIncidentInformation.Controls.Add(this.cmbCause);
            this.tabIncidentInformation.Controls.Add(this.label15);
            this.tabIncidentInformation.Controls.Add(this.textBox8);
            this.tabIncidentInformation.Controls.Add(this.cmbNotifierType);
            this.tabIncidentInformation.Controls.Add(this.cmbNotificationMethod);
            this.tabIncidentInformation.Controls.Add(this.textBox4);
            this.tabIncidentInformation.Controls.Add(this.textBox5);
            this.tabIncidentInformation.Controls.Add(this.textBox6);
            this.tabIncidentInformation.Controls.Add(this.textBox7);
            this.tabIncidentInformation.Controls.Add(this.label9);
            this.tabIncidentInformation.Controls.Add(this.label10);
            this.tabIncidentInformation.Controls.Add(this.label11);
            this.tabIncidentInformation.Controls.Add(this.label12);
            this.tabIncidentInformation.Controls.Add(this.label13);
            this.tabIncidentInformation.Controls.Add(this.label14);
            this.tabIncidentInformation.Controls.Add(this.cmbBusinessUnit);
            this.tabIncidentInformation.Controls.Add(this.cmbReporterName);
            this.tabIncidentInformation.Controls.Add(this.textBox2);
            this.tabIncidentInformation.Controls.Add(this.txtReporterEmailAddress);
            this.tabIncidentInformation.Controls.Add(this.label5);
            this.tabIncidentInformation.Controls.Add(this.label6);
            this.tabIncidentInformation.Controls.Add(this.label7);
            this.tabIncidentInformation.Controls.Add(this.label8);
            this.tabIncidentInformation.Location = new System.Drawing.Point(4, 22);
            this.tabIncidentInformation.Name = "tabIncidentInformation";
            this.tabIncidentInformation.Padding = new System.Windows.Forms.Padding(3);
            this.tabIncidentInformation.Size = new System.Drawing.Size(787, 487);
            this.tabIncidentInformation.TabIndex = 0;
            this.tabIncidentInformation.Text = "Incident Information";
            this.tabIncidentInformation.UseVisualStyleBackColor = true;
            // 
            // lblOtherRelationship
            // 
            this.lblOtherRelationship.AutoSize = true;
            this.lblOtherRelationship.ForeColor = System.Drawing.Color.White;
            this.lblOtherRelationship.Location = new System.Drawing.Point(125, 315);
            this.lblOtherRelationship.Name = "lblOtherRelationship";
            this.lblOtherRelationship.Size = new System.Drawing.Size(60, 13);
            this.lblOtherRelationship.TabIndex = 247;
            this.lblOtherRelationship.Text = "Description";
            // 
            // txtOtherRelationship
            // 
            this.txtOtherRelationship.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "OtherRelationship", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOtherRelationship.Location = new System.Drawing.Point(191, 312);
            this.txtOtherRelationship.Name = "txtOtherRelationship";
            this.txtOtherRelationship.Size = new System.Drawing.Size(213, 20);
            this.txtOtherRelationship.TabIndex = 246;
            // 
            // notifierBindingSource
            // 
            this.notifierBindingSource.DataSource = typeof(INCIDENTRP.Notifier);
            // 
            // cmbNotifierRelationship
            // 
            this.cmbNotifierRelationship.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbNotifierRelationship.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbNotifierRelationship.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "Relationship", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbNotifierRelationship.FormattingEnabled = true;
            this.cmbNotifierRelationship.Location = new System.Drawing.Point(128, 288);
            this.cmbNotifierRelationship.Name = "cmbNotifierRelationship";
            this.cmbNotifierRelationship.Size = new System.Drawing.Size(276, 21);
            this.cmbNotifierRelationship.TabIndex = 245;
            this.cmbNotifierRelationship.SelectedIndexChanged += new System.EventHandler(this.CmbNotifierRelationship_SelectedIndexChanged);
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.ForeColor = System.Drawing.Color.White;
            this.label41.Location = new System.Drawing.Point(13, 292);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(101, 13);
            this.label41.TabIndex = 244;
            this.label41.Text = "Notifier Relationship";
            // 
            // lblOtherType
            // 
            this.lblOtherType.AutoSize = true;
            this.lblOtherType.ForeColor = System.Drawing.Color.White;
            this.lblOtherType.Location = new System.Drawing.Point(125, 197);
            this.lblOtherType.Name = "lblOtherType";
            this.lblOtherType.Size = new System.Drawing.Size(60, 13);
            this.lblOtherType.TabIndex = 243;
            this.lblOtherType.Text = "Description";
            // 
            // txtOtherType
            // 
            this.txtOtherType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "OtherType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOtherType.Location = new System.Drawing.Point(191, 194);
            this.txtOtherType.Name = "txtOtherType";
            this.txtOtherType.Size = new System.Drawing.Size(213, 20);
            this.txtOtherType.TabIndex = 242;
            // 
            // lblOtherMethod
            // 
            this.lblOtherMethod.AutoSize = true;
            this.lblOtherMethod.ForeColor = System.Drawing.Color.White;
            this.lblOtherMethod.Location = new System.Drawing.Point(125, 127);
            this.lblOtherMethod.Name = "lblOtherMethod";
            this.lblOtherMethod.Size = new System.Drawing.Size(60, 13);
            this.lblOtherMethod.TabIndex = 241;
            this.lblOtherMethod.Text = "Description";
            // 
            // txtOtherMethod
            // 
            this.txtOtherMethod.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "OtherMethod", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOtherMethod.Location = new System.Drawing.Point(191, 124);
            this.txtOtherMethod.Name = "txtOtherMethod";
            this.txtOtherMethod.Size = new System.Drawing.Size(213, 20);
            this.txtOtherMethod.TabIndex = 240;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.ForeColor = System.Drawing.Color.White;
            this.label39.Location = new System.Drawing.Point(13, 338);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(37, 13);
            this.label39.TabIndex = 239;
            this.label39.Text = "Cause";
            // 
            // cmbCause
            // 
            this.cmbCause.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbCause.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCause.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.incidentBindingSource, "Cause", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbCause.FormattingEnabled = true;
            this.cmbCause.Location = new System.Drawing.Point(128, 335);
            this.cmbCause.Name = "cmbCause";
            this.cmbCause.Size = new System.Drawing.Size(276, 21);
            this.cmbCause.TabIndex = 235;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(413, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(91, 13);
            this.label15.TabIndex = 232;
            this.label15.Text = "Incident Narrative";
            // 
            // textBox8
            // 
            this.textBox8.CausesValidation = false;
            this.textBox8.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.incidentBindingSource, "Narrative", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox8.Location = new System.Drawing.Point(505, 6);
            this.textBox8.Multiline = true;
            this.textBox8.Name = "textBox8";
            this.textBox8.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox8.Size = new System.Drawing.Size(276, 232);
            this.textBox8.TabIndex = 231;
            // 
            // cmbNotifierType
            // 
            this.cmbNotifierType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbNotifierType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbNotifierType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "Type", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbNotifierType.FormattingEnabled = true;
            this.cmbNotifierType.Location = new System.Drawing.Point(128, 170);
            this.cmbNotifierType.Name = "cmbNotifierType";
            this.cmbNotifierType.Size = new System.Drawing.Size(276, 21);
            this.cmbNotifierType.TabIndex = 27;
            this.cmbNotifierType.SelectedIndexChanged += new System.EventHandler(this.CmbNotifierType_SelectedIndexChanged);
            // 
            // cmbNotificationMethod
            // 
            this.cmbNotificationMethod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbNotificationMethod.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbNotificationMethod.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "Method", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbNotificationMethod.FormattingEnabled = true;
            this.cmbNotificationMethod.Location = new System.Drawing.Point(128, 100);
            this.cmbNotificationMethod.Name = "cmbNotificationMethod";
            this.cmbNotificationMethod.Size = new System.Drawing.Size(276, 21);
            this.cmbNotificationMethod.TabIndex = 26;
            this.cmbNotificationMethod.SelectedIndexChanged += new System.EventHandler(this.CmbNotificationMethod_SelectedIndexChanged);
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "PhoneNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox4.Location = new System.Drawing.Point(128, 265);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(276, 20);
            this.textBox4.TabIndex = 25;
            // 
            // textBox5
            // 
            this.textBox5.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "EmailAddress", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox5.Location = new System.Drawing.Point(128, 241);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(276, 20);
            this.textBox5.TabIndex = 24;
            // 
            // textBox6
            // 
            this.textBox6.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifierBindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox6.Location = new System.Drawing.Point(128, 217);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(276, 20);
            this.textBox6.TabIndex = 23;
            // 
            // textBox7
            // 
            this.textBox7.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.incidentBindingSource, "Location", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox7.Location = new System.Drawing.Point(128, 147);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(276, 20);
            this.textBox7.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(13, 266);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Notifier Phone Number";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(13, 243);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(112, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Notifier E-mail Address";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(13, 220);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Notifier Name";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(13, 174);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Notified By";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(13, 103);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 17;
            this.label13.Text = "Notifier Method";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(13, 150);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(90, 13);
            this.label14.TabIndex = 16;
            this.label14.Text = "Physical Location";
            // 
            // cmbBusinessUnit
            // 
            this.cmbBusinessUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbBusinessUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBusinessUnit.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.reporterBindingSource, "BusinessUnit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbBusinessUnit.DataSource = this.businessUnitBindingSource;
            this.cmbBusinessUnit.DisplayMember = "Name";
            this.cmbBusinessUnit.FormattingEnabled = true;
            this.cmbBusinessUnit.Location = new System.Drawing.Point(128, 30);
            this.cmbBusinessUnit.Name = "cmbBusinessUnit";
            this.cmbBusinessUnit.Size = new System.Drawing.Size(276, 21);
            this.cmbBusinessUnit.TabIndex = 15;
            this.cmbBusinessUnit.ValueMember = "ID";
            // 
            // reporterBindingSource
            // 
            this.reporterBindingSource.DataSource = typeof(INCIDENTRP.Reporter);
            // 
            // businessUnitBindingSource
            // 
            this.businessUnitBindingSource.DataSource = typeof(Uheaa.Common.ProcessLogger.BusinessUnit);
            // 
            // cmbReporterName
            // 
            this.cmbReporterName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbReporterName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbReporterName.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.reporterBindingSource, "User", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbReporterName.DataSource = this.activeDirectoryUserBindingSource;
            this.cmbReporterName.DisplayMember = "LegalName";
            this.cmbReporterName.FormattingEnabled = true;
            this.cmbReporterName.Location = new System.Drawing.Point(128, 6);
            this.cmbReporterName.Name = "cmbReporterName";
            this.cmbReporterName.Size = new System.Drawing.Size(276, 21);
            this.cmbReporterName.TabIndex = 14;
            this.cmbReporterName.ValueMember = "SecurityId";
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.reporterBindingSource, "PhoneNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox2.Location = new System.Drawing.Point(128, 77);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(276, 20);
            this.textBox2.TabIndex = 13;
            // 
            // txtReporterEmailAddress
            // 
            this.txtReporterEmailAddress.Location = new System.Drawing.Point(128, 54);
            this.txtReporterEmailAddress.Name = "txtReporterEmailAddress";
            this.txtReporterEmailAddress.Size = new System.Drawing.Size(276, 20);
            this.txtReporterEmailAddress.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(13, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Phone Number";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(13, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "E-mail Address";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(13, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Business Unit";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(13, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Reporter Name";
            // 
            // tabIncidentTypeDetails
            // 
            this.tabIncidentTypeDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.tabIncidentTypeDetails.Controls.Add(this.cmbIncidentType);
            this.tabIncidentTypeDetails.Controls.Add(this.pnlIncidentTypeDetails);
            this.tabIncidentTypeDetails.Location = new System.Drawing.Point(4, 22);
            this.tabIncidentTypeDetails.Name = "tabIncidentTypeDetails";
            this.tabIncidentTypeDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tabIncidentTypeDetails.Size = new System.Drawing.Size(787, 487);
            this.tabIncidentTypeDetails.TabIndex = 3;
            this.tabIncidentTypeDetails.Text = "Details";
            this.tabIncidentTypeDetails.UseVisualStyleBackColor = true;
            // 
            // cmbIncidentType
            // 
            this.cmbIncidentType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbIncidentType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbIncidentType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.incidentBindingSource, "Type", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbIncidentType.FormattingEnabled = true;
            this.cmbIncidentType.Location = new System.Drawing.Point(6, 6);
            this.cmbIncidentType.Name = "cmbIncidentType";
            this.cmbIncidentType.Size = new System.Drawing.Size(277, 21);
            this.cmbIncidentType.TabIndex = 240;
            this.cmbIncidentType.SelectionChangeCommitted += new System.EventHandler(this.CmbIncidentType_SelectionChangeCommitted);
            // 
            // pnlIncidentTypeDetails
            // 
            this.pnlIncidentTypeDetails.Location = new System.Drawing.Point(289, 6);
            this.pnlIncidentTypeDetails.Name = "pnlIncidentTypeDetails";
            this.pnlIncidentTypeDetails.Size = new System.Drawing.Size(441, 245);
            this.pnlIncidentTypeDetails.TabIndex = 238;
            // 
            // tabDataInvolved
            // 
            this.tabDataInvolved.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.tabDataInvolved.Controls.Add(this.cmbDataInvolved);
            this.tabDataInvolved.Controls.Add(this.pnlDataInvolved);
            this.tabDataInvolved.Location = new System.Drawing.Point(4, 22);
            this.tabDataInvolved.Name = "tabDataInvolved";
            this.tabDataInvolved.Padding = new System.Windows.Forms.Padding(3);
            this.tabDataInvolved.Size = new System.Drawing.Size(787, 487);
            this.tabDataInvolved.TabIndex = 2;
            this.tabDataInvolved.Text = "Data Involved";
            this.tabDataInvolved.UseVisualStyleBackColor = true;
            // 
            // cmbDataInvolved
            // 
            this.cmbDataInvolved.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbDataInvolved.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDataInvolved.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.incidentBindingSource, "DataInvolved", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbDataInvolved.FormattingEnabled = true;
            this.cmbDataInvolved.Location = new System.Drawing.Point(6, 6);
            this.cmbDataInvolved.Name = "cmbDataInvolved";
            this.cmbDataInvolved.Size = new System.Drawing.Size(277, 21);
            this.cmbDataInvolved.TabIndex = 239;
            this.cmbDataInvolved.SelectedIndexChanged += new System.EventHandler(this.CmbDataInvolved_SelectedIndexChanged);
            // 
            // pnlDataInvolved
            // 
            this.pnlDataInvolved.Location = new System.Drawing.Point(289, 6);
            this.pnlDataInvolved.Name = "pnlDataInvolved";
            this.pnlDataInvolved.Size = new System.Drawing.Size(441, 245);
            this.pnlDataInvolved.TabIndex = 237;
            // 
            // tabActionsTaken
            // 
            this.tabActionsTaken.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.tabActionsTaken.Controls.Add(this.lblActionsTaken);
            this.tabActionsTaken.Controls.Add(this.pnlActionsTaken);
            this.tabActionsTaken.Location = new System.Drawing.Point(4, 22);
            this.tabActionsTaken.Name = "tabActionsTaken";
            this.tabActionsTaken.Padding = new System.Windows.Forms.Padding(3);
            this.tabActionsTaken.Size = new System.Drawing.Size(787, 487);
            this.tabActionsTaken.TabIndex = 1;
            this.tabActionsTaken.Text = "Actions Taken";
            this.tabActionsTaken.UseVisualStyleBackColor = true;
            // 
            // lblActionsTaken
            // 
            this.lblActionsTaken.AutoSize = true;
            this.lblActionsTaken.ForeColor = System.Drawing.Color.White;
            this.lblActionsTaken.Location = new System.Drawing.Point(6, 6);
            this.lblActionsTaken.Name = "lblActionsTaken";
            this.lblActionsTaken.Size = new System.Drawing.Size(76, 13);
            this.lblActionsTaken.TabIndex = 236;
            this.lblActionsTaken.Text = "Actions Taken";
            // 
            // pnlActionsTaken
            // 
            this.pnlActionsTaken.AutoScroll = true;
            this.pnlActionsTaken.Controls.Add(this.chkContactedInfoTech);
            this.pnlActionsTaken.Controls.Add(this.grpContactedInfoTech);
            this.pnlActionsTaken.Controls.Add(this.chkNotifiedAffectedIndividual);
            this.pnlActionsTaken.Controls.Add(this.grpNotifiedAffectedIndividual);
            this.pnlActionsTaken.Controls.Add(this.chkAskedCallerToReturnCorrespondence);
            this.pnlActionsTaken.Controls.Add(this.grpAskedCallerToReturnCorrespondence);
            this.pnlActionsTaken.Controls.Add(this.chkDeletedFiles);
            this.pnlActionsTaken.Controls.Add(this.grpDeletedFiles);
            this.pnlActionsTaken.Controls.Add(this.chkCorrectedData);
            this.pnlActionsTaken.Controls.Add(this.grpCorrectedData);
            this.pnlActionsTaken.Controls.Add(this.chkRemovedSystemFromNetwork);
            this.pnlActionsTaken.Controls.Add(this.grpRemovedSystemFromNetwork);
            this.pnlActionsTaken.Controls.Add(this.chkRebootedSystem);
            this.pnlActionsTaken.Controls.Add(this.grpRebootedSystem);
            this.pnlActionsTaken.Controls.Add(this.chkLoggedOffSystem);
            this.pnlActionsTaken.Controls.Add(this.grpLoggedOffSystem);
            this.pnlActionsTaken.Controls.Add(this.chkShutDownSystem);
            this.pnlActionsTaken.Controls.Add(this.grpShutDownSystem);
            this.pnlActionsTaken.Controls.Add(this.chkContactedLawEnforcement);
            this.pnlActionsTaken.Controls.Add(this.grpContactedLawEnforcement);
            this.pnlActionsTaken.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlActionsTaken.ForeColor = System.Drawing.Color.White;
            this.pnlActionsTaken.Location = new System.Drawing.Point(6, 19);
            this.pnlActionsTaken.Name = "pnlActionsTaken";
            this.pnlActionsTaken.Size = new System.Drawing.Size(388, 462);
            this.pnlActionsTaken.TabIndex = 235;
            this.pnlActionsTaken.WrapContents = false;
            // 
            // chkContactedInfoTech
            // 
            this.chkContactedInfoTech.AutoSize = true;
            this.chkContactedInfoTech.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.contactedInfoTechBindingSource, "ActionWasTaken", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkContactedInfoTech.Location = new System.Drawing.Point(3, 3);
            this.chkContactedInfoTech.Name = "chkContactedInfoTech";
            this.chkContactedInfoTech.Size = new System.Drawing.Size(318, 17);
            this.chkContactedInfoTech.TabIndex = 7;
            this.chkContactedInfoTech.Text = "Contacted Information Technology/Information Security Office";
            this.chkContactedInfoTech.UseVisualStyleBackColor = true;
            this.chkContactedInfoTech.CheckedChanged += new System.EventHandler(this.ChkContactedInfoTech_CheckedChanged);
            // 
            // contactedInfoTechBindingSource
            // 
            this.contactedInfoTechBindingSource.DataSource = typeof(INCIDENTRP.ActionTaken);
            // 
            // grpContactedInfoTech
            // 
            this.grpContactedInfoTech.Controls.Add(this.ContactedITDate);
            this.grpContactedInfoTech.Controls.Add(this.dateTimePicker4);
            this.grpContactedInfoTech.Controls.Add(this.textBox9);
            this.grpContactedInfoTech.Controls.Add(this.label16);
            this.grpContactedInfoTech.Controls.Add(this.label17);
            this.grpContactedInfoTech.Controls.Add(this.label18);
            this.grpContactedInfoTech.Location = new System.Drawing.Point(3, 26);
            this.grpContactedInfoTech.Name = "grpContactedInfoTech";
            this.grpContactedInfoTech.Size = new System.Drawing.Size(364, 84);
            this.grpContactedInfoTech.TabIndex = 10;
            this.grpContactedInfoTech.TabStop = false;
            this.grpContactedInfoTech.Text = "Details";
            this.grpContactedInfoTech.Visible = false;
            // 
            // ContactedITDate
            // 
            this.ContactedITDate.CustomFormat = "MM/dd/yyyy";
            this.ContactedITDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.contactedInfoTechBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ContactedITDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ContactedITDate.Location = new System.Drawing.Point(170, 34);
            this.ContactedITDate.Name = "ContactedITDate";
            this.ContactedITDate.Size = new System.Drawing.Size(187, 20);
            this.ContactedITDate.TabIndex = 5;
            // 
            // dateTimePicker4
            // 
            this.dateTimePicker4.CustomFormat = "hh:mm tt";
            this.dateTimePicker4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.contactedInfoTechBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker4.Location = new System.Drawing.Point(170, 57);
            this.dateTimePicker4.Name = "dateTimePicker4";
            this.dateTimePicker4.ShowUpDown = true;
            this.dateTimePicker4.Size = new System.Drawing.Size(187, 20);
            this.dateTimePicker4.TabIndex = 4;
            // 
            // textBox9
            // 
            this.textBox9.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.contactedInfoTechBindingSource, "PersonContacted", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox9.Location = new System.Drawing.Point(170, 13);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(187, 20);
            this.textBox9.TabIndex = 3;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(15, 38);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(82, 13);
            this.label16.TabIndex = 2;
            this.label16.Text = "Date Contacted";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(15, 61);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(82, 13);
            this.label17.TabIndex = 1;
            this.label17.Text = "Time Contacted";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(15, 16);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(147, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Name of Individual Contacted";
            // 
            // chkNotifiedAffectedIndividual
            // 
            this.chkNotifiedAffectedIndividual.AutoSize = true;
            this.chkNotifiedAffectedIndividual.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.notifiedAffectedIndividualBindingSource, "ActionWasTaken", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkNotifiedAffectedIndividual.Location = new System.Drawing.Point(3, 116);
            this.chkNotifiedAffectedIndividual.Name = "chkNotifiedAffectedIndividual";
            this.chkNotifiedAffectedIndividual.Size = new System.Drawing.Size(153, 17);
            this.chkNotifiedAffectedIndividual.TabIndex = 11;
            this.chkNotifiedAffectedIndividual.Text = "Notified Affected Individual";
            this.chkNotifiedAffectedIndividual.UseVisualStyleBackColor = true;
            this.chkNotifiedAffectedIndividual.CheckedChanged += new System.EventHandler(this.ChkNotifiedAffectedIndividual_CheckedChanged);
            // 
            // notifiedAffectedIndividualBindingSource
            // 
            this.notifiedAffectedIndividualBindingSource.DataSource = typeof(INCIDENTRP.ActionTaken);
            // 
            // grpNotifiedAffectedIndividual
            // 
            this.grpNotifiedAffectedIndividual.Controls.Add(this.NotifiedDate);
            this.grpNotifiedAffectedIndividual.Controls.Add(this.dateTimePicker6);
            this.grpNotifiedAffectedIndividual.Controls.Add(this.textBox10);
            this.grpNotifiedAffectedIndividual.Controls.Add(this.label19);
            this.grpNotifiedAffectedIndividual.Controls.Add(this.label20);
            this.grpNotifiedAffectedIndividual.Controls.Add(this.label21);
            this.grpNotifiedAffectedIndividual.Location = new System.Drawing.Point(3, 139);
            this.grpNotifiedAffectedIndividual.Name = "grpNotifiedAffectedIndividual";
            this.grpNotifiedAffectedIndividual.Size = new System.Drawing.Size(364, 84);
            this.grpNotifiedAffectedIndividual.TabIndex = 9;
            this.grpNotifiedAffectedIndividual.TabStop = false;
            this.grpNotifiedAffectedIndividual.Text = "Details";
            this.grpNotifiedAffectedIndividual.Visible = false;
            // 
            // NotifiedDate
            // 
            this.NotifiedDate.CustomFormat = "MM/dd/yyyy";
            this.NotifiedDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.notifiedAffectedIndividualBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.NotifiedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.NotifiedDate.Location = new System.Drawing.Point(170, 34);
            this.NotifiedDate.Name = "NotifiedDate";
            this.NotifiedDate.Size = new System.Drawing.Size(187, 20);
            this.NotifiedDate.TabIndex = 5;
            // 
            // dateTimePicker6
            // 
            this.dateTimePicker6.CustomFormat = "hh:mm tt";
            this.dateTimePicker6.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.notifiedAffectedIndividualBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker6.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker6.Location = new System.Drawing.Point(170, 57);
            this.dateTimePicker6.Name = "dateTimePicker6";
            this.dateTimePicker6.ShowUpDown = true;
            this.dateTimePicker6.Size = new System.Drawing.Size(187, 20);
            this.dateTimePicker6.TabIndex = 4;
            // 
            // textBox10
            // 
            this.textBox10.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.notifiedAffectedIndividualBindingSource, "PersonContacted", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox10.Location = new System.Drawing.Point(170, 13);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(187, 20);
            this.textBox10.TabIndex = 3;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(15, 38);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(69, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "Date Notified";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(15, 61);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(69, 13);
            this.label20.TabIndex = 1;
            this.label20.Text = "Time Notified";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(15, 16);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(134, 13);
            this.label21.TabIndex = 0;
            this.label21.Text = "Name of Individual Notified";
            // 
            // chkAskedCallerToReturnCorrespondence
            // 
            this.chkAskedCallerToReturnCorrespondence.AutoSize = true;
            this.chkAskedCallerToReturnCorrespondence.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.askedCallerToReturnCorrespondenceBindingSource, "ActionWasTaken", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAskedCallerToReturnCorrespondence.Location = new System.Drawing.Point(3, 229);
            this.chkAskedCallerToReturnCorrespondence.Name = "chkAskedCallerToReturnCorrespondence";
            this.chkAskedCallerToReturnCorrespondence.Size = new System.Drawing.Size(213, 17);
            this.chkAskedCallerToReturnCorrespondence.TabIndex = 13;
            this.chkAskedCallerToReturnCorrespondence.Text = "Asked Caller to Return Correspondence";
            this.chkAskedCallerToReturnCorrespondence.UseVisualStyleBackColor = true;
            this.chkAskedCallerToReturnCorrespondence.CheckedChanged += new System.EventHandler(this.ChkAskedCallerToReturnCorrespondence_CheckedChanged);
            // 
            // askedCallerToReturnCorrespondenceBindingSource
            // 
            this.askedCallerToReturnCorrespondenceBindingSource.DataSource = typeof(INCIDENTRP.ActionTaken);
            // 
            // grpAskedCallerToReturnCorrespondence
            // 
            this.grpAskedCallerToReturnCorrespondence.Controls.Add(this.AskedDate);
            this.grpAskedCallerToReturnCorrespondence.Controls.Add(this.dateTimePicker8);
            this.grpAskedCallerToReturnCorrespondence.Controls.Add(this.label22);
            this.grpAskedCallerToReturnCorrespondence.Controls.Add(this.label23);
            this.grpAskedCallerToReturnCorrespondence.Location = new System.Drawing.Point(3, 252);
            this.grpAskedCallerToReturnCorrespondence.Name = "grpAskedCallerToReturnCorrespondence";
            this.grpAskedCallerToReturnCorrespondence.Size = new System.Drawing.Size(364, 64);
            this.grpAskedCallerToReturnCorrespondence.TabIndex = 12;
            this.grpAskedCallerToReturnCorrespondence.TabStop = false;
            this.grpAskedCallerToReturnCorrespondence.Text = "Details";
            this.grpAskedCallerToReturnCorrespondence.Visible = false;
            // 
            // AskedDate
            // 
            this.AskedDate.CustomFormat = "MM/dd/yyyy";
            this.AskedDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.askedCallerToReturnCorrespondenceBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AskedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.AskedDate.Location = new System.Drawing.Point(171, 12);
            this.AskedDate.Name = "AskedDate";
            this.AskedDate.Size = new System.Drawing.Size(187, 20);
            this.AskedDate.TabIndex = 5;
            // 
            // dateTimePicker8
            // 
            this.dateTimePicker8.CustomFormat = "hh:mm tt";
            this.dateTimePicker8.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.askedCallerToReturnCorrespondenceBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker8.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker8.Location = new System.Drawing.Point(171, 38);
            this.dateTimePicker8.Name = "dateTimePicker8";
            this.dateTimePicker8.ShowUpDown = true;
            this.dateTimePicker8.Size = new System.Drawing.Size(187, 20);
            this.dateTimePicker8.TabIndex = 4;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(15, 16);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(63, 13);
            this.label22.TabIndex = 2;
            this.label22.Text = "Date Asked";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(15, 42);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(63, 13);
            this.label23.TabIndex = 1;
            this.label23.Text = "Time Asked";
            // 
            // chkDeletedFiles
            // 
            this.chkDeletedFiles.AutoSize = true;
            this.chkDeletedFiles.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.deletedFilesBindingSource, "ActionWasTaken", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkDeletedFiles.Location = new System.Drawing.Point(3, 322);
            this.chkDeletedFiles.Name = "chkDeletedFiles";
            this.chkDeletedFiles.Size = new System.Drawing.Size(87, 17);
            this.chkDeletedFiles.TabIndex = 15;
            this.chkDeletedFiles.Text = "Deleted Files";
            this.chkDeletedFiles.UseVisualStyleBackColor = true;
            this.chkDeletedFiles.CheckedChanged += new System.EventHandler(this.ChkDeletedFiles_CheckedChanged);
            // 
            // deletedFilesBindingSource
            // 
            this.deletedFilesBindingSource.DataSource = typeof(INCIDENTRP.ActionTaken);
            // 
            // grpDeletedFiles
            // 
            this.grpDeletedFiles.Controls.Add(this.DeletedDate);
            this.grpDeletedFiles.Controls.Add(this.dateTimePicker10);
            this.grpDeletedFiles.Controls.Add(this.label24);
            this.grpDeletedFiles.Controls.Add(this.label25);
            this.grpDeletedFiles.Location = new System.Drawing.Point(3, 345);
            this.grpDeletedFiles.Name = "grpDeletedFiles";
            this.grpDeletedFiles.Size = new System.Drawing.Size(364, 64);
            this.grpDeletedFiles.TabIndex = 14;
            this.grpDeletedFiles.TabStop = false;
            this.grpDeletedFiles.Text = "Details";
            this.grpDeletedFiles.Visible = false;
            // 
            // DeletedDate
            // 
            this.DeletedDate.CustomFormat = "MM/dd/yyyy";
            this.DeletedDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.deletedFilesBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DeletedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DeletedDate.Location = new System.Drawing.Point(171, 12);
            this.DeletedDate.Name = "DeletedDate";
            this.DeletedDate.Size = new System.Drawing.Size(187, 20);
            this.DeletedDate.TabIndex = 5;
            // 
            // dateTimePicker10
            // 
            this.dateTimePicker10.CustomFormat = "hh:mm tt";
            this.dateTimePicker10.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.deletedFilesBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker10.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker10.Location = new System.Drawing.Point(171, 38);
            this.dateTimePicker10.Name = "dateTimePicker10";
            this.dateTimePicker10.ShowUpDown = true;
            this.dateTimePicker10.Size = new System.Drawing.Size(187, 20);
            this.dateTimePicker10.TabIndex = 4;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(15, 16);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(70, 13);
            this.label24.TabIndex = 2;
            this.label24.Text = "Date Deleted";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(15, 42);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(70, 13);
            this.label25.TabIndex = 1;
            this.label25.Text = "Time Deleted";
            // 
            // chkCorrectedData
            // 
            this.chkCorrectedData.AutoSize = true;
            this.chkCorrectedData.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.correctedDataBindingSource, "ActionWasTaken", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkCorrectedData.Location = new System.Drawing.Point(3, 415);
            this.chkCorrectedData.Name = "chkCorrectedData";
            this.chkCorrectedData.Size = new System.Drawing.Size(98, 17);
            this.chkCorrectedData.TabIndex = 17;
            this.chkCorrectedData.Text = "Corrected Data";
            this.chkCorrectedData.UseVisualStyleBackColor = true;
            this.chkCorrectedData.CheckedChanged += new System.EventHandler(this.ChkCorrectedData_CheckedChanged);
            // 
            // correctedDataBindingSource
            // 
            this.correctedDataBindingSource.DataSource = typeof(INCIDENTRP.ActionTaken);
            // 
            // grpCorrectedData
            // 
            this.grpCorrectedData.Controls.Add(this.CorrectedDate);
            this.grpCorrectedData.Controls.Add(this.dateTimePicker12);
            this.grpCorrectedData.Controls.Add(this.label26);
            this.grpCorrectedData.Controls.Add(this.label27);
            this.grpCorrectedData.Location = new System.Drawing.Point(3, 438);
            this.grpCorrectedData.Name = "grpCorrectedData";
            this.grpCorrectedData.Size = new System.Drawing.Size(364, 64);
            this.grpCorrectedData.TabIndex = 16;
            this.grpCorrectedData.TabStop = false;
            this.grpCorrectedData.Text = "Details";
            this.grpCorrectedData.Visible = false;
            // 
            // CorrectedDate
            // 
            this.CorrectedDate.CustomFormat = "MM/dd/yyyy";
            this.CorrectedDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.correctedDataBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CorrectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CorrectedDate.Location = new System.Drawing.Point(171, 12);
            this.CorrectedDate.Name = "CorrectedDate";
            this.CorrectedDate.Size = new System.Drawing.Size(187, 20);
            this.CorrectedDate.TabIndex = 5;
            // 
            // dateTimePicker12
            // 
            this.dateTimePicker12.CustomFormat = "hh:mm tt";
            this.dateTimePicker12.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.correctedDataBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker12.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker12.Location = new System.Drawing.Point(171, 38);
            this.dateTimePicker12.Name = "dateTimePicker12";
            this.dateTimePicker12.ShowUpDown = true;
            this.dateTimePicker12.Size = new System.Drawing.Size(187, 20);
            this.dateTimePicker12.TabIndex = 4;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(15, 16);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(79, 13);
            this.label26.TabIndex = 2;
            this.label26.Text = "Date Corrected";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(15, 42);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(79, 13);
            this.label27.TabIndex = 1;
            this.label27.Text = "Time Corrected";
            // 
            // chkRemovedSystemFromNetwork
            // 
            this.chkRemovedSystemFromNetwork.AutoSize = true;
            this.chkRemovedSystemFromNetwork.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.removedSystemBindingSource, "ActionWasTaken", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkRemovedSystemFromNetwork.Location = new System.Drawing.Point(3, 508);
            this.chkRemovedSystemFromNetwork.Name = "chkRemovedSystemFromNetwork";
            this.chkRemovedSystemFromNetwork.Size = new System.Drawing.Size(175, 17);
            this.chkRemovedSystemFromNetwork.TabIndex = 19;
            this.chkRemovedSystemFromNetwork.Text = "Removed System from Network";
            this.chkRemovedSystemFromNetwork.UseVisualStyleBackColor = true;
            this.chkRemovedSystemFromNetwork.CheckedChanged += new System.EventHandler(this.ChkRemovedSystemFromNetwork_CheckedChanged);
            // 
            // removedSystemBindingSource
            // 
            this.removedSystemBindingSource.DataSource = typeof(INCIDENTRP.ActionTaken);
            // 
            // grpRemovedSystemFromNetwork
            // 
            this.grpRemovedSystemFromNetwork.Controls.Add(this.RemovedDate);
            this.grpRemovedSystemFromNetwork.Controls.Add(this.dateTimePicker14);
            this.grpRemovedSystemFromNetwork.Controls.Add(this.label28);
            this.grpRemovedSystemFromNetwork.Controls.Add(this.label29);
            this.grpRemovedSystemFromNetwork.Location = new System.Drawing.Point(3, 531);
            this.grpRemovedSystemFromNetwork.Name = "grpRemovedSystemFromNetwork";
            this.grpRemovedSystemFromNetwork.Size = new System.Drawing.Size(364, 64);
            this.grpRemovedSystemFromNetwork.TabIndex = 18;
            this.grpRemovedSystemFromNetwork.TabStop = false;
            this.grpRemovedSystemFromNetwork.Text = "Details";
            this.grpRemovedSystemFromNetwork.Visible = false;
            // 
            // RemovedDate
            // 
            this.RemovedDate.CustomFormat = "MM/dd/yyyy";
            this.RemovedDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.removedSystemBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RemovedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.RemovedDate.Location = new System.Drawing.Point(171, 12);
            this.RemovedDate.Name = "RemovedDate";
            this.RemovedDate.Size = new System.Drawing.Size(187, 20);
            this.RemovedDate.TabIndex = 5;
            // 
            // dateTimePicker14
            // 
            this.dateTimePicker14.CustomFormat = "hh:mm tt";
            this.dateTimePicker14.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.removedSystemBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker14.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker14.Location = new System.Drawing.Point(171, 38);
            this.dateTimePicker14.Name = "dateTimePicker14";
            this.dateTimePicker14.ShowUpDown = true;
            this.dateTimePicker14.Size = new System.Drawing.Size(187, 20);
            this.dateTimePicker14.TabIndex = 4;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(15, 16);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(79, 13);
            this.label28.TabIndex = 2;
            this.label28.Text = "Date Removed";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(15, 42);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(79, 13);
            this.label29.TabIndex = 1;
            this.label29.Text = "Time Removed";
            // 
            // chkRebootedSystem
            // 
            this.chkRebootedSystem.AutoSize = true;
            this.chkRebootedSystem.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.rebootedSystemBindingSource, "ActionWasTaken", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkRebootedSystem.Location = new System.Drawing.Point(3, 601);
            this.chkRebootedSystem.Name = "chkRebootedSystem";
            this.chkRebootedSystem.Size = new System.Drawing.Size(110, 17);
            this.chkRebootedSystem.TabIndex = 21;
            this.chkRebootedSystem.Text = "Rebooted System";
            this.chkRebootedSystem.UseVisualStyleBackColor = true;
            this.chkRebootedSystem.CheckedChanged += new System.EventHandler(this.ChkRebootedSystem_CheckedChanged);
            // 
            // rebootedSystemBindingSource
            // 
            this.rebootedSystemBindingSource.DataSource = typeof(INCIDENTRP.ActionTaken);
            // 
            // grpRebootedSystem
            // 
            this.grpRebootedSystem.Controls.Add(this.RebootedDate);
            this.grpRebootedSystem.Controls.Add(this.dateTimePicker16);
            this.grpRebootedSystem.Controls.Add(this.label30);
            this.grpRebootedSystem.Controls.Add(this.label31);
            this.grpRebootedSystem.Location = new System.Drawing.Point(3, 624);
            this.grpRebootedSystem.Name = "grpRebootedSystem";
            this.grpRebootedSystem.Size = new System.Drawing.Size(364, 64);
            this.grpRebootedSystem.TabIndex = 20;
            this.grpRebootedSystem.TabStop = false;
            this.grpRebootedSystem.Text = "Details";
            this.grpRebootedSystem.Visible = false;
            // 
            // RebootedDate
            // 
            this.RebootedDate.CustomFormat = "MM/dd/yyyy";
            this.RebootedDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.rebootedSystemBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RebootedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.RebootedDate.Location = new System.Drawing.Point(171, 12);
            this.RebootedDate.Name = "RebootedDate";
            this.RebootedDate.Size = new System.Drawing.Size(187, 20);
            this.RebootedDate.TabIndex = 5;
            // 
            // dateTimePicker16
            // 
            this.dateTimePicker16.CustomFormat = "hh:mm tt";
            this.dateTimePicker16.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.rebootedSystemBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker16.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker16.Location = new System.Drawing.Point(171, 38);
            this.dateTimePicker16.Name = "dateTimePicker16";
            this.dateTimePicker16.ShowUpDown = true;
            this.dateTimePicker16.Size = new System.Drawing.Size(187, 20);
            this.dateTimePicker16.TabIndex = 4;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(15, 16);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(80, 13);
            this.label30.TabIndex = 2;
            this.label30.Text = "Date Rebooted";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(15, 42);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(80, 13);
            this.label31.TabIndex = 1;
            this.label31.Text = "Time Rebooted";
            // 
            // chkLoggedOffSystem
            // 
            this.chkLoggedOffSystem.AutoSize = true;
            this.chkLoggedOffSystem.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.loggedOffSystemBindingSource, "ActionWasTaken", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkLoggedOffSystem.Location = new System.Drawing.Point(3, 694);
            this.chkLoggedOffSystem.Name = "chkLoggedOffSystem";
            this.chkLoggedOffSystem.Size = new System.Drawing.Size(116, 17);
            this.chkLoggedOffSystem.TabIndex = 23;
            this.chkLoggedOffSystem.Text = "Logged Off System";
            this.chkLoggedOffSystem.UseVisualStyleBackColor = true;
            this.chkLoggedOffSystem.CheckedChanged += new System.EventHandler(this.ChkLoggedOffSystem_CheckedChanged);
            // 
            // loggedOffSystemBindingSource
            // 
            this.loggedOffSystemBindingSource.DataSource = typeof(INCIDENTRP.ActionTaken);
            // 
            // grpLoggedOffSystem
            // 
            this.grpLoggedOffSystem.Controls.Add(this.LoggedOffDate);
            this.grpLoggedOffSystem.Controls.Add(this.dateTimePicker18);
            this.grpLoggedOffSystem.Controls.Add(this.label32);
            this.grpLoggedOffSystem.Controls.Add(this.label33);
            this.grpLoggedOffSystem.Location = new System.Drawing.Point(3, 717);
            this.grpLoggedOffSystem.Name = "grpLoggedOffSystem";
            this.grpLoggedOffSystem.Size = new System.Drawing.Size(364, 64);
            this.grpLoggedOffSystem.TabIndex = 22;
            this.grpLoggedOffSystem.TabStop = false;
            this.grpLoggedOffSystem.Text = "Details";
            this.grpLoggedOffSystem.Visible = false;
            // 
            // LoggedOffDate
            // 
            this.LoggedOffDate.CustomFormat = "MM/dd/yyyy";
            this.LoggedOffDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.loggedOffSystemBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.LoggedOffDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.LoggedOffDate.Location = new System.Drawing.Point(171, 12);
            this.LoggedOffDate.Name = "LoggedOffDate";
            this.LoggedOffDate.Size = new System.Drawing.Size(187, 20);
            this.LoggedOffDate.TabIndex = 5;
            // 
            // dateTimePicker18
            // 
            this.dateTimePicker18.CustomFormat = "hh:mm tt";
            this.dateTimePicker18.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.loggedOffSystemBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker18.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker18.Location = new System.Drawing.Point(171, 38);
            this.dateTimePicker18.Name = "dateTimePicker18";
            this.dateTimePicker18.ShowUpDown = true;
            this.dateTimePicker18.Size = new System.Drawing.Size(187, 20);
            this.dateTimePicker18.TabIndex = 4;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(15, 16);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(86, 13);
            this.label32.TabIndex = 2;
            this.label32.Text = "Date Logged Off";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(15, 42);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(86, 13);
            this.label33.TabIndex = 1;
            this.label33.Text = "Time Logged Off";
            // 
            // chkShutDownSystem
            // 
            this.chkShutDownSystem.AutoSize = true;
            this.chkShutDownSystem.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.shutDownSystemBindingSource, "ActionWasTaken", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkShutDownSystem.Location = new System.Drawing.Point(3, 787);
            this.chkShutDownSystem.Name = "chkShutDownSystem";
            this.chkShutDownSystem.Size = new System.Drawing.Size(116, 17);
            this.chkShutDownSystem.TabIndex = 25;
            this.chkShutDownSystem.Text = "Shut Down System";
            this.chkShutDownSystem.UseVisualStyleBackColor = true;
            this.chkShutDownSystem.CheckedChanged += new System.EventHandler(this.ChkShutDownSystem_CheckedChanged);
            // 
            // shutDownSystemBindingSource
            // 
            this.shutDownSystemBindingSource.DataSource = typeof(INCIDENTRP.ActionTaken);
            // 
            // grpShutDownSystem
            // 
            this.grpShutDownSystem.Controls.Add(this.ShutDownDate);
            this.grpShutDownSystem.Controls.Add(this.dateTimePicker20);
            this.grpShutDownSystem.Controls.Add(this.label34);
            this.grpShutDownSystem.Controls.Add(this.label35);
            this.grpShutDownSystem.Location = new System.Drawing.Point(3, 810);
            this.grpShutDownSystem.Name = "grpShutDownSystem";
            this.grpShutDownSystem.Size = new System.Drawing.Size(364, 64);
            this.grpShutDownSystem.TabIndex = 24;
            this.grpShutDownSystem.TabStop = false;
            this.grpShutDownSystem.Text = "Details";
            this.grpShutDownSystem.Visible = false;
            // 
            // ShutDownDate
            // 
            this.ShutDownDate.CustomFormat = "MM/dd/yyyy";
            this.ShutDownDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.shutDownSystemBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ShutDownDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ShutDownDate.Location = new System.Drawing.Point(171, 12);
            this.ShutDownDate.Name = "ShutDownDate";
            this.ShutDownDate.Size = new System.Drawing.Size(187, 20);
            this.ShutDownDate.TabIndex = 5;
            // 
            // dateTimePicker20
            // 
            this.dateTimePicker20.CustomFormat = "hh:mm tt";
            this.dateTimePicker20.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.shutDownSystemBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker20.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker20.Location = new System.Drawing.Point(171, 38);
            this.dateTimePicker20.Name = "dateTimePicker20";
            this.dateTimePicker20.ShowUpDown = true;
            this.dateTimePicker20.Size = new System.Drawing.Size(187, 20);
            this.dateTimePicker20.TabIndex = 4;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(15, 16);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(86, 13);
            this.label34.TabIndex = 2;
            this.label34.Text = "Date Shut Down";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(15, 42);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(86, 13);
            this.label35.TabIndex = 1;
            this.label35.Text = "Time Shut Down";
            // 
            // chkContactedLawEnforcement
            // 
            this.chkContactedLawEnforcement.AutoSize = true;
            this.chkContactedLawEnforcement.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.contactedLawEnforcementBindingSource, "ActionWasTaken", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkContactedLawEnforcement.Location = new System.Drawing.Point(3, 880);
            this.chkContactedLawEnforcement.Name = "chkContactedLawEnforcement";
            this.chkContactedLawEnforcement.Size = new System.Drawing.Size(161, 17);
            this.chkContactedLawEnforcement.TabIndex = 9;
            this.chkContactedLawEnforcement.Text = "Contacted Law Enforcement";
            this.chkContactedLawEnforcement.UseVisualStyleBackColor = true;
            this.chkContactedLawEnforcement.CheckedChanged += new System.EventHandler(this.ChkContactedLawEnforcement_CheckedChanged);
            // 
            // contactedLawEnforcementBindingSource
            // 
            this.contactedLawEnforcementBindingSource.DataSource = typeof(INCIDENTRP.ActionTaken);
            // 
            // grpContactedLawEnforcement
            // 
            this.grpContactedLawEnforcement.Controls.Add(this.ContactedDate);
            this.grpContactedLawEnforcement.Controls.Add(this.dateTimePicker22);
            this.grpContactedLawEnforcement.Controls.Add(this.textBox11);
            this.grpContactedLawEnforcement.Controls.Add(this.label36);
            this.grpContactedLawEnforcement.Controls.Add(this.label37);
            this.grpContactedLawEnforcement.Controls.Add(this.label38);
            this.grpContactedLawEnforcement.Location = new System.Drawing.Point(3, 903);
            this.grpContactedLawEnforcement.Name = "grpContactedLawEnforcement";
            this.grpContactedLawEnforcement.Size = new System.Drawing.Size(364, 84);
            this.grpContactedLawEnforcement.TabIndex = 8;
            this.grpContactedLawEnforcement.TabStop = false;
            this.grpContactedLawEnforcement.Text = "Details";
            this.grpContactedLawEnforcement.Visible = false;
            // 
            // ContactedDate
            // 
            this.ContactedDate.CustomFormat = "MM/dd/yyyy";
            this.ContactedDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.contactedLawEnforcementBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ContactedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ContactedDate.Location = new System.Drawing.Point(170, 34);
            this.ContactedDate.Name = "ContactedDate";
            this.ContactedDate.Size = new System.Drawing.Size(187, 20);
            this.ContactedDate.TabIndex = 5;
            // 
            // dateTimePicker22
            // 
            this.dateTimePicker22.CustomFormat = "hh:mm tt";
            this.dateTimePicker22.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.contactedLawEnforcementBindingSource, "DateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker22.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker22.Location = new System.Drawing.Point(170, 57);
            this.dateTimePicker22.Name = "dateTimePicker22";
            this.dateTimePicker22.ShowUpDown = true;
            this.dateTimePicker22.Size = new System.Drawing.Size(187, 20);
            this.dateTimePicker22.TabIndex = 4;
            // 
            // textBox11
            // 
            this.textBox11.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.contactedLawEnforcementBindingSource, "PersonContacted", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox11.Location = new System.Drawing.Point(170, 13);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(187, 20);
            this.textBox11.TabIndex = 3;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(15, 38);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(82, 13);
            this.label36.TabIndex = 2;
            this.label36.Text = "Date Contacted";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(15, 61);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(82, 13);
            this.label37.TabIndex = 1;
            this.label37.Text = "Time Contacted";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(15, 16);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(147, 13);
            this.label38.TabIndex = 0;
            this.label38.Text = "Name of Individual Contacted";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "hh:mm tt";
            this.dateTimePicker2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketBindingSource, "IncidentDateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(682, 148);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.ShowUpDown = true;
            this.dateTimePicker2.Size = new System.Drawing.Size(185, 20);
            this.dateTimePicker2.TabIndex = 236;
            // 
            // IncidentDate
            // 
            this.IncidentDate.CustomFormat = "MM/dd/yyyy";
            this.IncidentDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketBindingSource, "IncidentDateTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IncidentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.IncidentDate.Location = new System.Drawing.Point(682, 117);
            this.IncidentDate.Name = "IncidentDate";
            this.IncidentDate.Size = new System.Drawing.Size(185, 20);
            this.IncidentDate.TabIndex = 235;
            // 
            // textBox12
            // 
            this.textBox12.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketBindingSource, "StatusDate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, null, "d"));
            this.textBox12.Location = new System.Drawing.Point(391, 117);
            this.textBox12.Name = "textBox12";
            this.textBox12.ReadOnly = true;
            this.textBox12.Size = new System.Drawing.Size(185, 20);
            this.textBox12.TabIndex = 237;
            // 
            // textBox13
            // 
            this.textBox13.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketBindingSource, "CourtDate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, null, "d"));
            this.textBox13.Location = new System.Drawing.Point(391, 147);
            this.textBox13.Name = "textBox13";
            this.textBox13.ReadOnly = true;
            this.textBox13.Size = new System.Drawing.Size(185, 20);
            this.textBox13.TabIndex = 238;
            // 
            // IncidentDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(1192, 720);
            this.Controls.Add(this.textBox13);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.IncidentDate);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblHistory);
            this.Controls.Add(this.lblUpdate);
            this.Controls.Add(this.txtUpdate);
            this.Controls.Add(this.txtHistory);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCourtDate);
            this.Controls.Add(this.lblCourt);
            this.Controls.Add(this.lblStatusDate);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblIncidentDate);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.lblPriority);
            this.Controls.Add(this.cmbCourt);
            this.Controls.Add(this.lblIncidentTime);
            this.Controls.Add(this.pnlTicket);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(2)))), ((int)(((byte)(10)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1208, 759);
            this.MinimumSize = new System.Drawing.Size(1208, 759);
            this.Name = "IncidentDisplay";
            this.Text = "Incident Report";
            this.pnlTicket.ResumeLayout(false);
            this.pnlTicket.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.incidentBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCompanyLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activeDirectoryUserBindingSource)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabIncidentInformation.ResumeLayout(false);
            this.tabIncidentInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.notifierBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporterBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessUnitBindingSource)).EndInit();
            this.tabIncidentTypeDetails.ResumeLayout(false);
            this.tabDataInvolved.ResumeLayout(false);
            this.tabActionsTaken.ResumeLayout(false);
            this.tabActionsTaken.PerformLayout();
            this.pnlActionsTaken.ResumeLayout(false);
            this.pnlActionsTaken.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contactedInfoTechBindingSource)).EndInit();
            this.grpContactedInfoTech.ResumeLayout(false);
            this.grpContactedInfoTech.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.notifiedAffectedIndividualBindingSource)).EndInit();
            this.grpNotifiedAffectedIndividual.ResumeLayout(false);
            this.grpNotifiedAffectedIndividual.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.askedCallerToReturnCorrespondenceBindingSource)).EndInit();
            this.grpAskedCallerToReturnCorrespondence.ResumeLayout(false);
            this.grpAskedCallerToReturnCorrespondence.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deletedFilesBindingSource)).EndInit();
            this.grpDeletedFiles.ResumeLayout(false);
            this.grpDeletedFiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.correctedDataBindingSource)).EndInit();
            this.grpCorrectedData.ResumeLayout(false);
            this.grpCorrectedData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.removedSystemBindingSource)).EndInit();
            this.grpRemovedSystemFromNetwork.ResumeLayout(false);
            this.grpRemovedSystemFromNetwork.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rebootedSystemBindingSource)).EndInit();
            this.grpRebootedSystem.ResumeLayout(false);
            this.grpRebootedSystem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loggedOffSystemBindingSource)).EndInit();
            this.grpLoggedOffSystem.ResumeLayout(false);
            this.grpLoggedOffSystem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shutDownSystemBindingSource)).EndInit();
            this.grpShutDownSystem.ResumeLayout(false);
            this.grpShutDownSystem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contactedLawEnforcementBindingSource)).EndInit();
            this.grpContactedLawEnforcement.ResumeLayout(false);
            this.grpContactedLawEnforcement.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHLine;
        private System.Windows.Forms.Panel pnlTicket;
        private System.Windows.Forms.ToolStripMenuItem emailRecipientsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem uploadFileToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTicketNumber;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbxCompanyLogo;
        private System.Windows.Forms.LinkLabel lnkUpdate;
		private System.Windows.Forms.LinkLabel lnkNextFlowStep;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPriority;
        private System.Windows.Forms.Label lblCourtDate;
        private System.Windows.Forms.Label lblCourt;
        private System.Windows.Forms.Label lblStatusDate;
		private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblIncidentDate;
		private System.Windows.Forms.TextBox txtStatus;
		private System.Windows.Forms.ComboBox cmbCourt;
        private System.Windows.Forms.Label lblIncidentTime;
        private System.Windows.Forms.Label lblTicket;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUpdate;
        private System.Windows.Forms.TextBox txtUpdate;
        private System.Windows.Forms.TextBox txtHistory;
        private System.Windows.Forms.Label lblHistory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabIncidentInformation;
        private System.Windows.Forms.TabPage tabActionsTaken;
        private System.Windows.Forms.ComboBox cmbBusinessUnit;
        private System.Windows.Forms.ComboBox cmbReporterName;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox txtReporterEmailAddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbNotifierType;
        private System.Windows.Forms.ComboBox cmbNotificationMethod;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker IncidentDate;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label lblActionsTaken;
        private System.Windows.Forms.FlowLayoutPanel pnlActionsTaken;
        private System.Windows.Forms.CheckBox chkContactedInfoTech;
        private System.Windows.Forms.GroupBox grpContactedInfoTech;
        private System.Windows.Forms.DateTimePicker ContactedITDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker4;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox chkNotifiedAffectedIndividual;
        private System.Windows.Forms.GroupBox grpNotifiedAffectedIndividual;
        private System.Windows.Forms.DateTimePicker NotifiedDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker6;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckBox chkAskedCallerToReturnCorrespondence;
        private System.Windows.Forms.GroupBox grpAskedCallerToReturnCorrespondence;
        private System.Windows.Forms.DateTimePicker AskedDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker8;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.CheckBox chkDeletedFiles;
        private System.Windows.Forms.GroupBox grpDeletedFiles;
        private System.Windows.Forms.DateTimePicker DeletedDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker10;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox chkCorrectedData;
        private System.Windows.Forms.GroupBox grpCorrectedData;
        private System.Windows.Forms.DateTimePicker CorrectedDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker12;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.CheckBox chkRemovedSystemFromNetwork;
        private System.Windows.Forms.GroupBox grpRemovedSystemFromNetwork;
        private System.Windows.Forms.DateTimePicker RemovedDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker14;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.CheckBox chkRebootedSystem;
        private System.Windows.Forms.GroupBox grpRebootedSystem;
        private System.Windows.Forms.DateTimePicker RebootedDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker16;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.CheckBox chkLoggedOffSystem;
        private System.Windows.Forms.GroupBox grpLoggedOffSystem;
        private System.Windows.Forms.DateTimePicker LoggedOffDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker18;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.CheckBox chkShutDownSystem;
        private System.Windows.Forms.GroupBox grpShutDownSystem;
        private System.Windows.Forms.DateTimePicker ShutDownDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker20;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.CheckBox chkContactedLawEnforcement;
        private System.Windows.Forms.GroupBox grpContactedLawEnforcement;
        private System.Windows.Forms.DateTimePicker ContactedDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker22;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
		private System.Windows.Forms.ComboBox cmbCause;
		private System.Windows.Forms.BindingSource ticketBindingSource;
		private System.Windows.Forms.TextBox textBox12;
		private System.Windows.Forms.TextBox textBox13;
		private System.Windows.Forms.BindingSource activeDirectoryUserBindingSource;
		private System.Windows.Forms.BindingSource businessUnitBindingSource;
		private System.Windows.Forms.BindingSource contactedInfoTechBindingSource;
		private System.Windows.Forms.BindingSource notifiedAffectedIndividualBindingSource;
		private System.Windows.Forms.BindingSource askedCallerToReturnCorrespondenceBindingSource;
		private System.Windows.Forms.BindingSource deletedFilesBindingSource;
		private System.Windows.Forms.BindingSource correctedDataBindingSource;
		private System.Windows.Forms.BindingSource removedSystemBindingSource;
		private System.Windows.Forms.BindingSource rebootedSystemBindingSource;
		private System.Windows.Forms.BindingSource loggedOffSystemBindingSource;
		private System.Windows.Forms.BindingSource shutDownSystemBindingSource;
		private System.Windows.Forms.BindingSource contactedLawEnforcementBindingSource;
		private System.Windows.Forms.BindingSource reporterBindingSource;
		private System.Windows.Forms.BindingSource notifierBindingSource;
		private System.Windows.Forms.BindingSource incidentBindingSource;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.TabPage tabDataInvolved;
		private System.Windows.Forms.TabPage tabIncidentTypeDetails;
		private System.Windows.Forms.ComboBox cmbDataInvolved;
		private System.Windows.Forms.Panel pnlDataInvolved;
		private System.Windows.Forms.Panel pnlIncidentTypeDetails;
		private System.Windows.Forms.ComboBox cmbIncidentType;
		private System.Windows.Forms.LinkLabel lnkSave;
		private System.Windows.Forms.TextBox txtOtherMethod;
		private System.Windows.Forms.Label lblOtherMethod;
		private System.Windows.Forms.Label lblOtherType;
		private System.Windows.Forms.TextBox txtOtherType;
		private System.Windows.Forms.Label lblOtherRelationship;
		private System.Windows.Forms.TextBox txtOtherRelationship;
		private System.Windows.Forms.ComboBox cmbNotifierRelationship;
		private System.Windows.Forms.Label label41;
    }
}