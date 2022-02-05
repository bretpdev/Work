using SubSystemShared;
namespace NHGeneral
{
    partial class ChangeRequest
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeRequest));
            this.lbxEmailRecip = new System.Windows.Forms.ListBox();
            this.lbxCurrentFiles = new System.Windows.Forms.ListBox();
            this.lblForms = new System.Windows.Forms.Label();
            this.lbxForms = new System.Windows.Forms.ListBox();
            this.txtOtherComments = new System.Windows.Forms.TextBox();
            this.ticketDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblOtherComments = new System.Windows.Forms.Label();
            this.cboSystem = new System.Windows.Forms.ComboBox();
            this.systemsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblRecipAdd = new System.Windows.Forms.Label();
            this.lblAssignedTo = new System.Windows.Forms.Label();
            this.lblSystem = new System.Windows.Forms.Label();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.lblCourtDate = new System.Windows.Forms.Label();
            this.lblCourt = new System.Windows.Forms.Label();
            this.lblStatusDate = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblSubject = new System.Windows.Forms.Label();
            this.lblUrgency = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.lnkExport = new System.Windows.Forms.LinkLabel();
            this.cboUrgency = new System.Windows.Forms.ComboBox();
            this.txtIssue = new System.Windows.Forms.TextBox();
            this.lblIssue = new System.Windows.Forms.Label();
            this.cboRequester = new System.Windows.Forms.ComboBox();
            this.activeDirectoryUserBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dtpReqDate = new System.Windows.Forms.DateTimePicker();
            this.dtpRequestDate = new System.Windows.Forms.DateTimePicker();
            this.lblRequiredDate = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.dtpStatusDate = new System.Windows.Forms.DateTimePicker();
            this.cboCourt = new System.Windows.Forms.ComboBox();
            this.dtpCourtDate = new System.Windows.Forms.DateTimePicker();
            this.lblRequester = new System.Windows.Forms.Label();
            this.txtUpdate = new System.Windows.Forms.TextBox();
            this.lblRequesetDate = new System.Windows.Forms.Label();
            this.cboBussUnit = new System.Windows.Forms.ComboBox();
            this.lblBusUnit = new System.Windows.Forms.Label();
            this.cboAssignedTo = new System.Windows.Forms.ComboBox();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.lblCurrentFiles = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.txtQC = new System.Windows.Forms.TextBox();
            this.lblQC = new System.Windows.Forms.Label();
            this.RemoveRecipient = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ticketDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activeDirectoryUserBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoveRecipient)).BeginInit();
            this.SuspendLayout();
            // 
            // lbxEmailRecip
            // 
            this.lbxEmailRecip.CausesValidation = false;
            this.lbxEmailRecip.FormattingEnabled = true;
            this.lbxEmailRecip.Location = new System.Drawing.Point(489, 400);
            this.lbxEmailRecip.Name = "lbxEmailRecip";
            this.lbxEmailRecip.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbxEmailRecip.Size = new System.Drawing.Size(273, 56);
            this.lbxEmailRecip.TabIndex = 15;
            // 
            // lbxCurrentFiles
            // 
            this.lbxCurrentFiles.CausesValidation = false;
            this.lbxCurrentFiles.FormattingEnabled = true;
            this.lbxCurrentFiles.Location = new System.Drawing.Point(130, 400);
            this.lbxCurrentFiles.Name = "lbxCurrentFiles";
            this.lbxCurrentFiles.Size = new System.Drawing.Size(254, 56);
            this.lbxCurrentFiles.TabIndex = 12;
            this.lbxCurrentFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LbxCurrentFiles_MouseDoubleClick);
            // 
            // lblForms
            // 
            this.lblForms.AutoSize = true;
            this.lblForms.BackColor = System.Drawing.Color.Transparent;
            this.lblForms.ForeColor = System.Drawing.Color.White;
            this.lblForms.Location = new System.Drawing.Point(306, 495);
            this.lblForms.Name = "lblForms";
            this.lblForms.Size = new System.Drawing.Size(38, 13);
            this.lblForms.TabIndex = 216;
            this.lblForms.Text = "Forms:";
            // 
            // lbxForms
            // 
            this.lbxForms.CausesValidation = false;
            this.lbxForms.FormattingEnabled = true;
            this.lbxForms.Location = new System.Drawing.Point(350, 466);
            this.lbxForms.Name = "lbxForms";
            this.lbxForms.Size = new System.Drawing.Size(516, 69);
            this.lbxForms.TabIndex = 18;
            this.lbxForms.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LbxForms_MouseDoubleClick);
            // 
            // txtOtherComments
            // 
            this.txtOtherComments.CausesValidation = false;
            this.txtOtherComments.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "Comments", true));
            this.txtOtherComments.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOtherComments.Location = new System.Drawing.Point(872, 400);
            this.txtOtherComments.Multiline = true;
            this.txtOtherComments.Name = "txtOtherComments";
            this.txtOtherComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOtherComments.Size = new System.Drawing.Size(297, 56);
            this.txtOtherComments.TabIndex = 17;
            // 
            // ticketDataBindingSource
            // 
            this.ticketDataBindingSource.DataSource = typeof(NHGeneral.TicketData);
            // 
            // lblOtherComments
            // 
            this.lblOtherComments.AutoSize = true;
            this.lblOtherComments.BackColor = System.Drawing.Color.Transparent;
            this.lblOtherComments.ForeColor = System.Drawing.Color.White;
            this.lblOtherComments.Location = new System.Drawing.Point(778, 400);
            this.lblOtherComments.Name = "lblOtherComments";
            this.lblOtherComments.Size = new System.Drawing.Size(88, 13);
            this.lblOtherComments.TabIndex = 213;
            this.lblOtherComments.Text = "Other Comments:";
            // 
            // cboSystem
            // 
            this.cboSystem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSystem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSystem.CausesValidation = false;
            this.cboSystem.DataSource = this.systemsBindingSource;
            this.cboSystem.FormattingEnabled = true;
            this.cboSystem.Location = new System.Drawing.Point(872, 344);
            this.cboSystem.Name = "cboSystem";
            this.cboSystem.Size = new System.Drawing.Size(181, 21);
            this.cboSystem.TabIndex = 16;
            // 
            // systemsBindingSource
            // 
            this.systemsBindingSource.DataMember = "Systems";
            this.systemsBindingSource.DataSource = this.ticketDataBindingSource;
            // 
            // lblRecipAdd
            // 
            this.lblRecipAdd.BackColor = System.Drawing.Color.Transparent;
            this.lblRecipAdd.ForeColor = System.Drawing.Color.White;
            this.lblRecipAdd.Location = new System.Drawing.Point(415, 400);
            this.lblRecipAdd.Name = "lblRecipAdd";
            this.lblRecipAdd.Size = new System.Drawing.Size(67, 42);
            this.lblRecipAdd.TabIndex = 179;
            this.lblRecipAdd.Text = "Recipient(s) Attached To Ticket:";
            // 
            // lblAssignedTo
            // 
            this.lblAssignedTo.AutoSize = true;
            this.lblAssignedTo.BackColor = System.Drawing.Color.Transparent;
            this.lblAssignedTo.ForeColor = System.Drawing.Color.White;
            this.lblAssignedTo.Location = new System.Drawing.Point(415, 346);
            this.lblAssignedTo.Name = "lblAssignedTo";
            this.lblAssignedTo.Size = new System.Drawing.Size(69, 13);
            this.lblAssignedTo.TabIndex = 180;
            this.lblAssignedTo.Text = "Assigned To:";
            // 
            // lblSystem
            // 
            this.lblSystem.AutoSize = true;
            this.lblSystem.BackColor = System.Drawing.Color.Transparent;
            this.lblSystem.ForeColor = System.Drawing.Color.White;
            this.lblSystem.Location = new System.Drawing.Point(817, 347);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(49, 13);
            this.lblSystem.TabIndex = 181;
            this.lblSystem.Text = "Systems:";
            // 
            // lblUpdate
            // 
            this.lblUpdate.AutoSize = true;
            this.lblUpdate.BackColor = System.Drawing.Color.Transparent;
            this.lblUpdate.ForeColor = System.Drawing.Color.White;
            this.lblUpdate.Location = new System.Drawing.Point(6, 257);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(45, 13);
            this.lblUpdate.TabIndex = 182;
            this.lblUpdate.Text = "Update:";
            // 
            // lblCourtDate
            // 
            this.lblCourtDate.AutoSize = true;
            this.lblCourtDate.BackColor = System.Drawing.Color.Transparent;
            this.lblCourtDate.ForeColor = System.Drawing.Color.White;
            this.lblCourtDate.Location = new System.Drawing.Point(322, 45);
            this.lblCourtDate.Name = "lblCourtDate";
            this.lblCourtDate.Size = new System.Drawing.Size(61, 13);
            this.lblCourtDate.TabIndex = 183;
            this.lblCourtDate.Text = "Court Date:";
            // 
            // lblCourt
            // 
            this.lblCourt.AutoSize = true;
            this.lblCourt.BackColor = System.Drawing.Color.Transparent;
            this.lblCourt.ForeColor = System.Drawing.Color.White;
            this.lblCourt.Location = new System.Drawing.Point(16, 45);
            this.lblCourt.Name = "lblCourt";
            this.lblCourt.Size = new System.Drawing.Size(35, 13);
            this.lblCourt.TabIndex = 184;
            this.lblCourt.Text = "Court:";
            // 
            // lblStatusDate
            // 
            this.lblStatusDate.AutoSize = true;
            this.lblStatusDate.BackColor = System.Drawing.Color.Transparent;
            this.lblStatusDate.ForeColor = System.Drawing.Color.White;
            this.lblStatusDate.Location = new System.Drawing.Point(317, 15);
            this.lblStatusDate.Name = "lblStatusDate";
            this.lblStatusDate.Size = new System.Drawing.Size(66, 13);
            this.lblStatusDate.TabIndex = 185;
            this.lblStatusDate.Text = "Status Date:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(11, 15);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 186;
            this.lblStatus.Text = "Status:";
            // 
            // lblSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.BackColor = System.Drawing.Color.Transparent;
            this.lblSubject.ForeColor = System.Drawing.Color.White;
            this.lblSubject.Location = new System.Drawing.Point(78, 346);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(46, 13);
            this.lblSubject.TabIndex = 187;
            this.lblSubject.Text = "Subject:";
            // 
            // lblUrgency
            // 
            this.lblUrgency.AutoSize = true;
            this.lblUrgency.BackColor = System.Drawing.Color.Transparent;
            this.lblUrgency.ForeColor = System.Drawing.Color.White;
            this.lblUrgency.Location = new System.Drawing.Point(903, 46);
            this.lblUrgency.Name = "lblUrgency";
            this.lblUrgency.Size = new System.Drawing.Size(50, 13);
            this.lblUrgency.TabIndex = 188;
            this.lblUrgency.Text = "Urgency:";
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.BackColor = System.Drawing.Color.Transparent;
            this.lblCategory.ForeColor = System.Drawing.Color.White;
            this.lblCategory.Location = new System.Drawing.Point(901, 13);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(52, 13);
            this.lblCategory.TabIndex = 189;
            this.lblCategory.Text = "Category:";
            // 
            // cboCategory
            // 
            this.cboCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCategory.CausesValidation = false;
            this.cboCategory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "CatOption", true));
            this.cboCategory.FormattingEnabled = true;
            this.cboCategory.Location = new System.Drawing.Point(959, 12);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(210, 21);
            this.cboCategory.TabIndex = 6;
            this.cboCategory.SelectedIndexChanged += new System.EventHandler(this.CboCategory_SelectedIndexChanged);
            this.cboCategory.Leave += new System.EventHandler(this.CboCategory_Leave);
            // 
            // lnkExport
            // 
            this.lnkExport.AutoSize = true;
            this.lnkExport.BackColor = System.Drawing.Color.Transparent;
            this.lnkExport.CausesValidation = false;
            this.lnkExport.LinkColor = System.Drawing.Color.Yellow;
            this.lnkExport.Location = new System.Drawing.Point(1097, 338);
            this.lnkExport.Name = "lnkExport";
            this.lnkExport.Size = new System.Drawing.Size(72, 13);
            this.lnkExport.TabIndex = 19;
            this.lnkExport.TabStop = true;
            this.lnkExport.Text = "Export History";
            this.lnkExport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExport_LinkClicked);
            // 
            // cboUrgency
            // 
            this.cboUrgency.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboUrgency.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboUrgency.CausesValidation = false;
            this.cboUrgency.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "UrgencyOption", true));
            this.cboUrgency.FormattingEnabled = true;
            this.cboUrgency.Location = new System.Drawing.Point(959, 43);
            this.cboUrgency.Name = "cboUrgency";
            this.cboUrgency.Size = new System.Drawing.Size(210, 21);
            this.cboUrgency.TabIndex = 7;
            this.cboUrgency.SelectedIndexChanged += new System.EventHandler(this.CboUrgency_SelectedIndexChanged);
            this.cboUrgency.Leave += new System.EventHandler(this.CboUrgency_Leave);
            // 
            // txtIssue
            // 
            this.txtIssue.CausesValidation = false;
            this.txtIssue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "Issue", true));
            this.txtIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIssue.Location = new System.Drawing.Point(57, 76);
            this.txtIssue.Multiline = true;
            this.txtIssue.Name = "txtIssue";
            this.txtIssue.ReadOnly = true;
            this.txtIssue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtIssue.Size = new System.Drawing.Size(440, 125);
            this.txtIssue.TabIndex = 8;
            // 
            // lblIssue
            // 
            this.lblIssue.AutoSize = true;
            this.lblIssue.BackColor = System.Drawing.Color.Transparent;
            this.lblIssue.ForeColor = System.Drawing.Color.White;
            this.lblIssue.Location = new System.Drawing.Point(16, 120);
            this.lblIssue.Name = "lblIssue";
            this.lblIssue.Size = new System.Drawing.Size(35, 13);
            this.lblIssue.TabIndex = 209;
            this.lblIssue.Text = "Issue:";
            // 
            // cboRequester
            // 
            this.cboRequester.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboRequester.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboRequester.CausesValidation = false;
            this.cboRequester.DataSource = this.activeDirectoryUserBindingSource;
            this.cboRequester.DisplayMember = "LegalName";
            this.cboRequester.FormattingEnabled = true;
            this.cboRequester.Location = new System.Drawing.Point(489, 373);
            this.cboRequester.Name = "cboRequester";
            this.cboRequester.Size = new System.Drawing.Size(272, 21);
            this.cboRequester.TabIndex = 14;
            this.cboRequester.ValueMember = "SecurityId";
            // 
            // activeDirectoryUserBindingSource
            // 
            this.activeDirectoryUserBindingSource.DataSource = typeof(SubSystemShared.SqlUser);
            // 
            // dtpReqDate
            // 
            this.dtpReqDate.CausesValidation = false;
            this.dtpReqDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketDataBindingSource, "Required", true));
            this.dtpReqDate.Location = new System.Drawing.Point(692, 11);
            this.dtpReqDate.Name = "dtpReqDate";
            this.dtpReqDate.Size = new System.Drawing.Size(185, 20);
            this.dtpReqDate.TabIndex = 4;
            this.dtpReqDate.Value = new System.DateTime(2012, 1, 26, 0, 0, 0, 0);
            // 
            // dtpRequestDate
            // 
            this.dtpRequestDate.CausesValidation = false;
            this.dtpRequestDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketDataBindingSource, "Requested", true));
            this.dtpRequestDate.Location = new System.Drawing.Point(692, 42);
            this.dtpRequestDate.Name = "dtpRequestDate";
            this.dtpRequestDate.Size = new System.Drawing.Size(185, 20);
            this.dtpRequestDate.TabIndex = 5;
            // 
            // lblRequiredDate
            // 
            this.lblRequiredDate.AutoSize = true;
            this.lblRequiredDate.BackColor = System.Drawing.Color.Transparent;
            this.lblRequiredDate.ForeColor = System.Drawing.Color.White;
            this.lblRequiredDate.Location = new System.Drawing.Point(607, 15);
            this.lblRequiredDate.Name = "lblRequiredDate";
            this.lblRequiredDate.Size = new System.Drawing.Size(79, 13);
            this.lblRequiredDate.TabIndex = 207;
            this.lblRequiredDate.Text = "Required Date:";
            // 
            // txtStatus
            // 
            this.txtStatus.CausesValidation = false;
            this.txtStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "Status", true));
            this.txtStatus.Enabled = false;
            this.txtStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatus.Location = new System.Drawing.Point(57, 11);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(240, 20);
            this.txtStatus.TabIndex = 0;
            // 
            // dtpStatusDate
            // 
            this.dtpStatusDate.CausesValidation = false;
            this.dtpStatusDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketDataBindingSource, "StatusDate", true));
            this.dtpStatusDate.Location = new System.Drawing.Point(389, 11);
            this.dtpStatusDate.Name = "dtpStatusDate";
            this.dtpStatusDate.Size = new System.Drawing.Size(185, 20);
            this.dtpStatusDate.TabIndex = 2;
            // 
            // cboCourt
            // 
            this.cboCourt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboCourt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCourt.CausesValidation = false;
            this.cboCourt.DataSource = this.activeDirectoryUserBindingSource;
            this.cboCourt.DisplayMember = "LegalName";
            this.cboCourt.FormattingEnabled = true;
            this.cboCourt.Location = new System.Drawing.Point(57, 42);
            this.cboCourt.Name = "cboCourt";
            this.cboCourt.Size = new System.Drawing.Size(240, 21);
            this.cboCourt.TabIndex = 1;
            this.cboCourt.ValueMember = "SecurityId";
            this.cboCourt.Leave += new System.EventHandler(this.CboCourt_Leave);
            // 
            // dtpCourtDate
            // 
            this.dtpCourtDate.CausesValidation = false;
            this.dtpCourtDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketDataBindingSource, "CourtDate", true));
            this.dtpCourtDate.Location = new System.Drawing.Point(389, 42);
            this.dtpCourtDate.Name = "dtpCourtDate";
            this.dtpCourtDate.Size = new System.Drawing.Size(185, 20);
            this.dtpCourtDate.TabIndex = 3;
            // 
            // lblRequester
            // 
            this.lblRequester.AutoSize = true;
            this.lblRequester.BackColor = System.Drawing.Color.Transparent;
            this.lblRequester.ForeColor = System.Drawing.Color.White;
            this.lblRequester.Location = new System.Drawing.Point(425, 375);
            this.lblRequester.Name = "lblRequester";
            this.lblRequester.Size = new System.Drawing.Size(59, 13);
            this.lblRequester.TabIndex = 206;
            this.lblRequester.Text = "Requester:";
            // 
            // txtUpdate
            // 
            this.txtUpdate.CausesValidation = false;
            this.txtUpdate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "IssueUpdate", true));
            this.txtUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdate.Location = new System.Drawing.Point(57, 210);
            this.txtUpdate.Multiline = true;
            this.txtUpdate.Name = "txtUpdate";
            this.txtUpdate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUpdate.Size = new System.Drawing.Size(440, 125);
            this.txtUpdate.TabIndex = 9;
            // 
            // lblRequesetDate
            // 
            this.lblRequesetDate.AutoSize = true;
            this.lblRequesetDate.BackColor = System.Drawing.Color.Transparent;
            this.lblRequesetDate.ForeColor = System.Drawing.Color.White;
            this.lblRequesetDate.Location = new System.Drawing.Point(607, 45);
            this.lblRequesetDate.Name = "lblRequesetDate";
            this.lblRequesetDate.Size = new System.Drawing.Size(76, 13);
            this.lblRequesetDate.TabIndex = 205;
            this.lblRequesetDate.Text = "Request Date:";
            // 
            // cboBussUnit
            // 
            this.cboBussUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboBussUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboBussUnit.CausesValidation = false;
            this.cboBussUnit.FormattingEnabled = true;
            this.cboBussUnit.Location = new System.Drawing.Point(130, 373);
            this.cboBussUnit.Name = "cboBussUnit";
            this.cboBussUnit.Size = new System.Drawing.Size(253, 21);
            this.cboBussUnit.TabIndex = 11;
            // 
            // lblBusUnit
            // 
            this.lblBusUnit.AutoSize = true;
            this.lblBusUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblBusUnit.ForeColor = System.Drawing.Color.White;
            this.lblBusUnit.Location = new System.Drawing.Point(50, 375);
            this.lblBusUnit.Name = "lblBusUnit";
            this.lblBusUnit.Size = new System.Drawing.Size(74, 13);
            this.lblBusUnit.TabIndex = 204;
            this.lblBusUnit.Text = "Business Unit:";
            // 
            // cboAssignedTo
            // 
            this.cboAssignedTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboAssignedTo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAssignedTo.CausesValidation = false;
            this.cboAssignedTo.DataSource = this.activeDirectoryUserBindingSource;
            this.cboAssignedTo.DisplayMember = "LegalName";
            this.cboAssignedTo.FormattingEnabled = true;
            this.cboAssignedTo.Location = new System.Drawing.Point(489, 344);
            this.cboAssignedTo.Name = "cboAssignedTo";
            this.cboAssignedTo.Size = new System.Drawing.Size(272, 21);
            this.cboAssignedTo.TabIndex = 13;
            this.cboAssignedTo.ValueMember = "SecurityId";
            // 
            // txtHistory
            // 
            this.txtHistory.CausesValidation = false;
            this.txtHistory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "History", true));
            this.txtHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHistory.Location = new System.Drawing.Point(507, 76);
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ReadOnly = true;
            this.txtHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHistory.Size = new System.Drawing.Size(662, 259);
            this.txtHistory.TabIndex = 202;
            // 
            // lblCurrentFiles
            // 
            this.lblCurrentFiles.AutoSize = true;
            this.lblCurrentFiles.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentFiles.ForeColor = System.Drawing.Color.White;
            this.lblCurrentFiles.Location = new System.Drawing.Point(56, 414);
            this.lblCurrentFiles.Name = "lblCurrentFiles";
            this.lblCurrentFiles.Size = new System.Drawing.Size(68, 13);
            this.lblCurrentFiles.TabIndex = 203;
            this.lblCurrentFiles.Text = "Current Files:";
            // 
            // txtSubject
            // 
            this.txtSubject.CausesValidation = false;
            this.txtSubject.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "Subject", true));
            this.txtSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubject.Location = new System.Drawing.Point(130, 343);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(253, 20);
            this.txtSubject.TabIndex = 10;
            // 
            // txtQC
            // 
            this.txtQC.CausesValidation = false;
            this.txtQC.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "RelatedQCIssues", true));
            this.txtQC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQC.Location = new System.Drawing.Point(872, 373);
            this.txtQC.Name = "txtQC";
            this.txtQC.Size = new System.Drawing.Size(181, 20);
            this.txtQC.TabIndex = 217;
            // 
            // lblQC
            // 
            this.lblQC.AutoSize = true;
            this.lblQC.BackColor = System.Drawing.Color.Transparent;
            this.lblQC.ForeColor = System.Drawing.Color.White;
            this.lblQC.Location = new System.Drawing.Point(841, 376);
            this.lblQC.Name = "lblQC";
            this.lblQC.Size = new System.Drawing.Size(25, 13);
            this.lblQC.TabIndex = 218;
            this.lblQC.Text = "QC:";
            // 
            // RemoveRecipient
            // 
            this.RemoveRecipient.Image = ((System.Drawing.Image)(resources.GetObject("RemoveRecipient.Image")));
            this.RemoveRecipient.InitialImage = ((System.Drawing.Image)(resources.GetObject("RemoveRecipient.InitialImage")));
            this.RemoveRecipient.Location = new System.Drawing.Point(768, 427);
            this.RemoveRecipient.Name = "RemoveRecipient";
            this.RemoveRecipient.Size = new System.Drawing.Size(15, 15);
            this.RemoveRecipient.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.RemoveRecipient.TabIndex = 232;
            this.RemoveRecipient.TabStop = false;
            this.RemoveRecipient.Click += new System.EventHandler(this.RemoveRecipient_Click);
            // 
            // ChangeRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.CausesValidation = false;
            this.Controls.Add(this.RemoveRecipient);
            this.Controls.Add(this.txtQC);
            this.Controls.Add(this.lblQC);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.lbxEmailRecip);
            this.Controls.Add(this.lbxCurrentFiles);
            this.Controls.Add(this.lblForms);
            this.Controls.Add(this.lbxForms);
            this.Controls.Add(this.txtOtherComments);
            this.Controls.Add(this.lblOtherComments);
            this.Controls.Add(this.cboSystem);
            this.Controls.Add(this.lblRecipAdd);
            this.Controls.Add(this.lblAssignedTo);
            this.Controls.Add(this.lblSystem);
            this.Controls.Add(this.lblUpdate);
            this.Controls.Add(this.lblCourtDate);
            this.Controls.Add(this.lblCourt);
            this.Controls.Add(this.lblStatusDate);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblSubject);
            this.Controls.Add(this.lblUrgency);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.cboCategory);
            this.Controls.Add(this.lnkExport);
            this.Controls.Add(this.cboUrgency);
            this.Controls.Add(this.txtIssue);
            this.Controls.Add(this.lblIssue);
            this.Controls.Add(this.cboRequester);
            this.Controls.Add(this.dtpReqDate);
            this.Controls.Add(this.dtpRequestDate);
            this.Controls.Add(this.lblRequiredDate);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.dtpStatusDate);
            this.Controls.Add(this.cboCourt);
            this.Controls.Add(this.dtpCourtDate);
            this.Controls.Add(this.lblRequester);
            this.Controls.Add(this.txtUpdate);
            this.Controls.Add(this.lblRequesetDate);
            this.Controls.Add(this.cboBussUnit);
            this.Controls.Add(this.lblBusUnit);
            this.Controls.Add(this.cboAssignedTo);
            this.Controls.Add(this.txtHistory);
            this.Controls.Add(this.lblCurrentFiles);
            this.Name = "ChangeRequest";
            this.Size = new System.Drawing.Size(1188, 556);
            ((System.ComponentModel.ISupportInitialize)(this.ticketDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activeDirectoryUserBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoveRecipient)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblForms;
        private System.Windows.Forms.ListBox lbxForms;
        private System.Windows.Forms.TextBox txtOtherComments;
        private System.Windows.Forms.Label lblOtherComments;
        private System.Windows.Forms.ComboBox cboSystem;
        private System.Windows.Forms.Label lblRecipAdd;
        private System.Windows.Forms.Label lblAssignedTo;
        private System.Windows.Forms.Label lblSystem;
        private System.Windows.Forms.Label lblUpdate;
        private System.Windows.Forms.Label lblCourtDate;
        private System.Windows.Forms.Label lblCourt;
        private System.Windows.Forms.Label lblStatusDate;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.Label lblUrgency;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.LinkLabel lnkExport;
        private System.Windows.Forms.Label lblIssue;
        private System.Windows.Forms.ComboBox cboRequester;
        private System.Windows.Forms.DateTimePicker dtpReqDate;
        private System.Windows.Forms.DateTimePicker dtpRequestDate;
        private System.Windows.Forms.Label lblRequiredDate;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.DateTimePicker dtpStatusDate;
        private System.Windows.Forms.ComboBox cboCourt;
        private System.Windows.Forms.DateTimePicker dtpCourtDate;
        private System.Windows.Forms.Label lblRequester;
        private System.Windows.Forms.TextBox txtUpdate;
        private System.Windows.Forms.Label lblRequesetDate;
        private System.Windows.Forms.ComboBox cboBussUnit;
        private System.Windows.Forms.Label lblBusUnit;
        private System.Windows.Forms.ComboBox cboAssignedTo;
        private System.Windows.Forms.TextBox txtHistory;
        private System.Windows.Forms.Label lblCurrentFiles;
        private System.Windows.Forms.BindingSource ticketDataBindingSource;
        public System.Windows.Forms.ComboBox cboCategory;
        public System.Windows.Forms.ComboBox cboUrgency;
        public System.Windows.Forms.TextBox txtIssue;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.ListBox lbxEmailRecip;
        private System.Windows.Forms.ListBox lbxCurrentFiles;
        private System.Windows.Forms.BindingSource activeDirectoryUserBindingSource;
		private System.Windows.Forms.BindingSource systemsBindingSource;
        private System.Windows.Forms.TextBox txtQC;
        private System.Windows.Forms.Label lblQC;
        private System.Windows.Forms.PictureBox RemoveRecipient;
    }
}
