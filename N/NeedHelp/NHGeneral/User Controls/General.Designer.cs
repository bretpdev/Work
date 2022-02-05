using SubSystemShared;
namespace NHGeneral
{
    partial class General
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(General));
            this.txtProcedures = new System.Windows.Forms.TextBox();
            this.ticketDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblProcedures = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblResolution = new System.Windows.Forms.Label();
            this.txtPrevention = new System.Windows.Forms.TextBox();
            this.lblPrevention = new System.Windows.Forms.Label();
            this.txtFix = new System.Windows.Forms.TextBox();
            this.lblFix = new System.Windows.Forms.Label();
            this.lblCause = new System.Windows.Forms.Label();
            this.cboCause = new System.Windows.Forms.ComboBox();
            this.txtQC = new System.Windows.Forms.TextBox();
            this.lblQC = new System.Windows.Forms.Label();
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
            this.txtCCC = new System.Windows.Forms.TextBox();
            this.lblCCC = new System.Windows.Forms.Label();
            this.txtRequestProj = new System.Windows.Forms.TextBox();
            this.cboRequester = new System.Windows.Forms.ComboBox();
            this.activeDirectoryUserBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dtpReqDate = new System.Windows.Forms.DateTimePicker();
            this.dtpRequestDate = new System.Windows.Forms.DateTimePicker();
            this.lblRequiredDate = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.lblRequestProj = new System.Windows.Forms.Label();
            this.dtpStatusDate = new System.Windows.Forms.DateTimePicker();
            this.cboFuncArea = new System.Windows.Forms.ComboBox();
            this.cboCourt = new System.Windows.Forms.ComboBox();
            this.lblFunctionalArea = new System.Windows.Forms.Label();
            this.dtpCourtDate = new System.Windows.Forms.DateTimePicker();
            this.lblRequester = new System.Windows.Forms.Label();
            this.txtUpdate = new System.Windows.Forms.TextBox();
            this.lblRequesetDate = new System.Windows.Forms.Label();
            this.cboBussUnit = new System.Windows.Forms.ComboBox();
            this.lblBusUnit = new System.Windows.Forms.Label();
            this.cboAssignedTo = new System.Windows.Forms.ComboBox();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.lblCurrentFiles = new System.Windows.Forms.Label();
            this.lbxEmailRecip = new System.Windows.Forms.ListBox();
            this.userSelectedEmailRecipientsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lbxSystems = new System.Windows.Forms.ListBox();
            this.lbxCurrentFiles = new System.Windows.Forms.ListBox();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.RemoveRecipient = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ticketDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activeDirectoryUserBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userSelectedEmailRecipientsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoveRecipient)).BeginInit();
            this.SuspendLayout();
            // 
            // txtProcedures
            // 
            this.txtProcedures.CausesValidation = false;
            this.txtProcedures.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "RelatedProcedures", true));
            this.txtProcedures.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProcedures.Location = new System.Drawing.Point(880, 368);
            this.txtProcedures.Name = "txtProcedures";
            this.txtProcedures.Size = new System.Drawing.Size(272, 20);
            this.txtProcedures.TabIndex = 19;
            // 
            // ticketDataBindingSource
            // 
            this.ticketDataBindingSource.DataSource = typeof(NHGeneral.TicketData);
            // 
            // lblProcedures
            // 
            this.lblProcedures.AutoSize = true;
            this.lblProcedures.BackColor = System.Drawing.Color.Transparent;
            this.lblProcedures.ForeColor = System.Drawing.Color.White;
            this.lblProcedures.Location = new System.Drawing.Point(810, 371);
            this.lblProcedures.Name = "lblProcedures";
            this.lblProcedures.Size = new System.Drawing.Size(64, 13);
            this.lblProcedures.TabIndex = 226;
            this.lblProcedures.Text = "Procedures:";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.CausesValidation = false;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 486);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1210, 2);
            this.label2.TabIndex = 225;
            // 
            // lblResolution
            // 
            this.lblResolution.AutoSize = true;
            this.lblResolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResolution.ForeColor = System.Drawing.Color.White;
            this.lblResolution.Location = new System.Drawing.Point(559, 500);
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new System.Drawing.Size(109, 24);
            this.lblResolution.TabIndex = 224;
            this.lblResolution.Text = "Resolution";
            // 
            // txtPrevention
            // 
            this.txtPrevention.CausesValidation = false;
            this.txtPrevention.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "ResolutionPrevention", true));
            this.txtPrevention.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrevention.Location = new System.Drawing.Point(674, 532);
            this.txtPrevention.Multiline = true;
            this.txtPrevention.Name = "txtPrevention";
            this.txtPrevention.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPrevention.Size = new System.Drawing.Size(490, 58);
            this.txtPrevention.TabIndex = 24;
            // 
            // lblPrevention
            // 
            this.lblPrevention.AutoSize = true;
            this.lblPrevention.BackColor = System.Drawing.Color.Transparent;
            this.lblPrevention.ForeColor = System.Drawing.Color.White;
            this.lblPrevention.Location = new System.Drawing.Point(607, 552);
            this.lblPrevention.Name = "lblPrevention";
            this.lblPrevention.Size = new System.Drawing.Size(61, 13);
            this.lblPrevention.TabIndex = 222;
            this.lblPrevention.Text = "Prevention:";
            // 
            // txtFix
            // 
            this.txtFix.CausesValidation = false;
            this.txtFix.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "ResolutionFix", true));
            this.txtFix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFix.Location = new System.Drawing.Point(125, 532);
            this.txtFix.Multiline = true;
            this.txtFix.Name = "txtFix";
            this.txtFix.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFix.Size = new System.Drawing.Size(448, 58);
            this.txtFix.TabIndex = 23;
            // 
            // lblFix
            // 
            this.lblFix.AutoSize = true;
            this.lblFix.BackColor = System.Drawing.Color.Transparent;
            this.lblFix.ForeColor = System.Drawing.Color.White;
            this.lblFix.Location = new System.Drawing.Point(96, 560);
            this.lblFix.Name = "lblFix";
            this.lblFix.Size = new System.Drawing.Size(23, 13);
            this.lblFix.TabIndex = 220;
            this.lblFix.Text = "Fix:";
            // 
            // lblCause
            // 
            this.lblCause.AutoSize = true;
            this.lblCause.BackColor = System.Drawing.Color.Transparent;
            this.lblCause.ForeColor = System.Drawing.Color.White;
            this.lblCause.Location = new System.Drawing.Point(79, 508);
            this.lblCause.Name = "lblCause";
            this.lblCause.Size = new System.Drawing.Size(40, 13);
            this.lblCause.TabIndex = 219;
            this.lblCause.Text = "Cause:";
            // 
            // cboCause
            // 
            this.cboCause.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboCause.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCause.CausesValidation = false;
            this.cboCause.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "ResolutionCause", true));
            this.cboCause.FormattingEnabled = true;
            this.cboCause.Location = new System.Drawing.Point(125, 500);
            this.cboCause.Name = "cboCause";
            this.cboCause.Size = new System.Drawing.Size(254, 21);
            this.cboCause.TabIndex = 22;
            // 
            // txtQC
            // 
            this.txtQC.CausesValidation = false;
            this.txtQC.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "RelatedQCIssues", true));
            this.txtQC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQC.Location = new System.Drawing.Point(880, 398);
            this.txtQC.Name = "txtQC";
            this.txtQC.Size = new System.Drawing.Size(272, 20);
            this.txtQC.TabIndex = 20;
            // 
            // lblQC
            // 
            this.lblQC.AutoSize = true;
            this.lblQC.BackColor = System.Drawing.Color.Transparent;
            this.lblQC.ForeColor = System.Drawing.Color.White;
            this.lblQC.Location = new System.Drawing.Point(849, 402);
            this.lblQC.Name = "lblQC";
            this.lblQC.Size = new System.Drawing.Size(25, 13);
            this.lblQC.TabIndex = 216;
            this.lblQC.Text = "QC:";
            // 
            // lblRecipAdd
            // 
            this.lblRecipAdd.BackColor = System.Drawing.Color.Transparent;
            this.lblRecipAdd.ForeColor = System.Drawing.Color.White;
            this.lblRecipAdd.Location = new System.Drawing.Point(420, 425);
            this.lblRecipAdd.Name = "lblRecipAdd";
            this.lblRecipAdd.Size = new System.Drawing.Size(67, 42);
            this.lblRecipAdd.TabIndex = 174;
            this.lblRecipAdd.Text = "Recipient(s) Attached To Ticket:";
            // 
            // lblAssignedTo
            // 
            this.lblAssignedTo.AutoSize = true;
            this.lblAssignedTo.BackColor = System.Drawing.Color.Transparent;
            this.lblAssignedTo.ForeColor = System.Drawing.Color.White;
            this.lblAssignedTo.Location = new System.Drawing.Point(420, 341);
            this.lblAssignedTo.Name = "lblAssignedTo";
            this.lblAssignedTo.Size = new System.Drawing.Size(69, 13);
            this.lblAssignedTo.TabIndex = 175;
            this.lblAssignedTo.Text = "Assigned To:";
            // 
            // lblSystem
            // 
            this.lblSystem.AutoSize = true;
            this.lblSystem.BackColor = System.Drawing.Color.Transparent;
            this.lblSystem.ForeColor = System.Drawing.Color.White;
            this.lblSystem.Location = new System.Drawing.Point(825, 431);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(49, 13);
            this.lblSystem.TabIndex = 176;
            this.lblSystem.Text = "Systems:";
            // 
            // lblUpdate
            // 
            this.lblUpdate.AutoSize = true;
            this.lblUpdate.BackColor = System.Drawing.Color.Transparent;
            this.lblUpdate.ForeColor = System.Drawing.Color.White;
            this.lblUpdate.Location = new System.Drawing.Point(11, 252);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(45, 13);
            this.lblUpdate.TabIndex = 177;
            this.lblUpdate.Text = "Update:";
            // 
            // lblCourtDate
            // 
            this.lblCourtDate.AutoSize = true;
            this.lblCourtDate.BackColor = System.Drawing.Color.Transparent;
            this.lblCourtDate.ForeColor = System.Drawing.Color.White;
            this.lblCourtDate.Location = new System.Drawing.Point(327, 40);
            this.lblCourtDate.Name = "lblCourtDate";
            this.lblCourtDate.Size = new System.Drawing.Size(61, 13);
            this.lblCourtDate.TabIndex = 178;
            this.lblCourtDate.Text = "Court Date:";
            // 
            // lblCourt
            // 
            this.lblCourt.AutoSize = true;
            this.lblCourt.BackColor = System.Drawing.Color.Transparent;
            this.lblCourt.ForeColor = System.Drawing.Color.White;
            this.lblCourt.Location = new System.Drawing.Point(21, 40);
            this.lblCourt.Name = "lblCourt";
            this.lblCourt.Size = new System.Drawing.Size(35, 13);
            this.lblCourt.TabIndex = 179;
            this.lblCourt.Text = "Court:";
            // 
            // lblStatusDate
            // 
            this.lblStatusDate.AutoSize = true;
            this.lblStatusDate.BackColor = System.Drawing.Color.Transparent;
            this.lblStatusDate.ForeColor = System.Drawing.Color.White;
            this.lblStatusDate.Location = new System.Drawing.Point(322, 10);
            this.lblStatusDate.Name = "lblStatusDate";
            this.lblStatusDate.Size = new System.Drawing.Size(66, 13);
            this.lblStatusDate.TabIndex = 180;
            this.lblStatusDate.Text = "Status Date:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(16, 10);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 181;
            this.lblStatus.Text = "Status:";
            // 
            // lblSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.BackColor = System.Drawing.Color.Transparent;
            this.lblSubject.ForeColor = System.Drawing.Color.White;
            this.lblSubject.Location = new System.Drawing.Point(83, 341);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(46, 13);
            this.lblSubject.TabIndex = 182;
            this.lblSubject.Text = "Subject:";
            // 
            // lblUrgency
            // 
            this.lblUrgency.AutoSize = true;
            this.lblUrgency.BackColor = System.Drawing.Color.Transparent;
            this.lblUrgency.ForeColor = System.Drawing.Color.White;
            this.lblUrgency.Location = new System.Drawing.Point(908, 41);
            this.lblUrgency.Name = "lblUrgency";
            this.lblUrgency.Size = new System.Drawing.Size(50, 13);
            this.lblUrgency.TabIndex = 183;
            this.lblUrgency.Text = "Urgency:";
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.BackColor = System.Drawing.Color.Transparent;
            this.lblCategory.ForeColor = System.Drawing.Color.White;
            this.lblCategory.Location = new System.Drawing.Point(906, 8);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(52, 13);
            this.lblCategory.TabIndex = 184;
            this.lblCategory.Text = "Category:";
            // 
            // cboCategory
            // 
            this.cboCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCategory.CausesValidation = false;
            this.cboCategory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "CatOption", true));
            this.cboCategory.FormattingEnabled = true;
            this.cboCategory.Location = new System.Drawing.Point(964, 7);
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
            this.lnkExport.Location = new System.Drawing.Point(1102, 333);
            this.lnkExport.Name = "lnkExport";
            this.lnkExport.Size = new System.Drawing.Size(72, 13);
            this.lnkExport.TabIndex = 25;
            this.lnkExport.TabStop = true;
            this.lnkExport.Text = "Export History";
            this.lnkExport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkExport_LinkClicked);
            // 
            // cboUrgency
            // 
            this.cboUrgency.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboUrgency.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboUrgency.CausesValidation = false;
            this.cboUrgency.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "UrgencyOption", true));
            this.cboUrgency.FormattingEnabled = true;
            this.cboUrgency.Location = new System.Drawing.Point(964, 38);
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
            this.txtIssue.Location = new System.Drawing.Point(62, 71);
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
            this.lblIssue.Location = new System.Drawing.Point(21, 115);
            this.lblIssue.Name = "lblIssue";
            this.lblIssue.Size = new System.Drawing.Size(35, 13);
            this.lblIssue.TabIndex = 212;
            this.lblIssue.Text = "Issue:";
            // 
            // txtCCC
            // 
            this.txtCCC.CausesValidation = false;
            this.txtCCC.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "CCCIssue", true));
            this.txtCCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCCC.Location = new System.Drawing.Point(880, 339);
            this.txtCCC.Name = "txtCCC";
            this.txtCCC.Size = new System.Drawing.Size(172, 20);
            this.txtCCC.TabIndex = 18;
            // 
            // lblCCC
            // 
            this.lblCCC.AutoSize = true;
            this.lblCCC.BackColor = System.Drawing.Color.Transparent;
            this.lblCCC.ForeColor = System.Drawing.Color.White;
            this.lblCCC.Location = new System.Drawing.Point(815, 341);
            this.lblCCC.Name = "lblCCC";
            this.lblCCC.Size = new System.Drawing.Size(59, 13);
            this.lblCCC.TabIndex = 210;
            this.lblCCC.Text = "CCC Issue:";
            // 
            // txtRequestProj
            // 
            this.txtRequestProj.CausesValidation = false;
            this.txtRequestProj.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "RequestProjectNum", true));
            this.txtRequestProj.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRequestProj.Location = new System.Drawing.Point(494, 398);
            this.txtRequestProj.Name = "txtRequestProj";
            this.txtRequestProj.Size = new System.Drawing.Size(272, 20);
            this.txtRequestProj.TabIndex = 16;
            // 
            // cboRequester
            // 
            this.cboRequester.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboRequester.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboRequester.CausesValidation = false;
            this.cboRequester.DataSource = this.activeDirectoryUserBindingSource;
            this.cboRequester.DisplayMember = "LegalName";
            this.cboRequester.FormattingEnabled = true;
            this.cboRequester.Location = new System.Drawing.Point(494, 368);
            this.cboRequester.Name = "cboRequester";
            this.cboRequester.Size = new System.Drawing.Size(272, 21);
            this.cboRequester.TabIndex = 15;
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
            this.dtpReqDate.Location = new System.Drawing.Point(697, 6);
            this.dtpReqDate.MinDate = new System.DateTime(2012, 12, 12, 0, 0, 0, 0);
            this.dtpReqDate.Name = "dtpReqDate";
            this.dtpReqDate.Size = new System.Drawing.Size(185, 20);
            this.dtpReqDate.TabIndex = 4;
            this.dtpReqDate.Value = new System.DateTime(2012, 12, 12, 0, 0, 0, 0);
            // 
            // dtpRequestDate
            // 
            this.dtpRequestDate.CausesValidation = false;
            this.dtpRequestDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketDataBindingSource, "Requested", true));
            this.dtpRequestDate.Location = new System.Drawing.Point(697, 37);
            this.dtpRequestDate.Name = "dtpRequestDate";
            this.dtpRequestDate.Size = new System.Drawing.Size(185, 20);
            this.dtpRequestDate.TabIndex = 5;
            // 
            // lblRequiredDate
            // 
            this.lblRequiredDate.AutoSize = true;
            this.lblRequiredDate.BackColor = System.Drawing.Color.Transparent;
            this.lblRequiredDate.ForeColor = System.Drawing.Color.White;
            this.lblRequiredDate.Location = new System.Drawing.Point(612, 10);
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
            this.txtStatus.Location = new System.Drawing.Point(62, 6);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(240, 20);
            this.txtStatus.TabIndex = 0;
            // 
            // lblRequestProj
            // 
            this.lblRequestProj.AutoSize = true;
            this.lblRequestProj.BackColor = System.Drawing.Color.Transparent;
            this.lblRequestProj.ForeColor = System.Drawing.Color.White;
            this.lblRequestProj.Location = new System.Drawing.Point(436, 401);
            this.lblRequestProj.Name = "lblRequestProj";
            this.lblRequestProj.Size = new System.Drawing.Size(53, 13);
            this.lblRequestProj.TabIndex = 206;
            this.lblRequestProj.Text = "Req/Proj:";
            // 
            // dtpStatusDate
            // 
            this.dtpStatusDate.CausesValidation = false;
            this.dtpStatusDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketDataBindingSource, "StatusDate", true));
            this.dtpStatusDate.Location = new System.Drawing.Point(394, 6);
            this.dtpStatusDate.Name = "dtpStatusDate";
            this.dtpStatusDate.Size = new System.Drawing.Size(185, 20);
            this.dtpStatusDate.TabIndex = 2;
            // 
            // cboFuncArea
            // 
            this.cboFuncArea.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboFuncArea.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFuncArea.CausesValidation = false;
            this.cboFuncArea.FormattingEnabled = true;
            this.cboFuncArea.Location = new System.Drawing.Point(135, 398);
            this.cboFuncArea.Name = "cboFuncArea";
            this.cboFuncArea.Size = new System.Drawing.Size(278, 21);
            this.cboFuncArea.TabIndex = 12;
            // 
            // cboCourt
            // 
            this.cboCourt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboCourt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCourt.CausesValidation = false;
            this.cboCourt.DataSource = this.activeDirectoryUserBindingSource;
            this.cboCourt.DisplayMember = "LegalName";
            this.cboCourt.FormattingEnabled = true;
            this.cboCourt.Location = new System.Drawing.Point(62, 37);
            this.cboCourt.Name = "cboCourt";
            this.cboCourt.Size = new System.Drawing.Size(240, 21);
            this.cboCourt.TabIndex = 1;
            this.cboCourt.ValueMember = "SecurityId";
            this.cboCourt.Leave += new System.EventHandler(this.CboCourt_Leave);
            // 
            // lblFunctionalArea
            // 
            this.lblFunctionalArea.AutoSize = true;
            this.lblFunctionalArea.BackColor = System.Drawing.Color.Transparent;
            this.lblFunctionalArea.ForeColor = System.Drawing.Color.White;
            this.lblFunctionalArea.Location = new System.Drawing.Point(45, 398);
            this.lblFunctionalArea.Name = "lblFunctionalArea";
            this.lblFunctionalArea.Size = new System.Drawing.Size(84, 13);
            this.lblFunctionalArea.TabIndex = 204;
            this.lblFunctionalArea.Text = "Functional Area:";
            // 
            // dtpCourtDate
            // 
            this.dtpCourtDate.CausesValidation = false;
            this.dtpCourtDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ticketDataBindingSource, "CourtDate", true));
            this.dtpCourtDate.Location = new System.Drawing.Point(394, 37);
            this.dtpCourtDate.Name = "dtpCourtDate";
            this.dtpCourtDate.Size = new System.Drawing.Size(185, 20);
            this.dtpCourtDate.TabIndex = 3;
            // 
            // lblRequester
            // 
            this.lblRequester.AutoSize = true;
            this.lblRequester.BackColor = System.Drawing.Color.Transparent;
            this.lblRequester.ForeColor = System.Drawing.Color.White;
            this.lblRequester.Location = new System.Drawing.Point(430, 370);
            this.lblRequester.Name = "lblRequester";
            this.lblRequester.Size = new System.Drawing.Size(59, 13);
            this.lblRequester.TabIndex = 203;
            this.lblRequester.Text = "Requester:";
            // 
            // txtUpdate
            // 
            this.txtUpdate.CausesValidation = false;
            this.txtUpdate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "IssueUpdate", true));
            this.txtUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdate.Location = new System.Drawing.Point(62, 205);
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
            this.lblRequesetDate.Location = new System.Drawing.Point(612, 40);
            this.lblRequesetDate.Name = "lblRequesetDate";
            this.lblRequesetDate.Size = new System.Drawing.Size(76, 13);
            this.lblRequesetDate.TabIndex = 202;
            this.lblRequesetDate.Text = "Request Date:";
            // 
            // cboBussUnit
            // 
            this.cboBussUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboBussUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboBussUnit.CausesValidation = false;
            this.cboBussUnit.FormattingEnabled = true;
            this.cboBussUnit.Location = new System.Drawing.Point(135, 368);
            this.cboBussUnit.Name = "cboBussUnit";
            this.cboBussUnit.Size = new System.Drawing.Size(278, 21);
            this.cboBussUnit.TabIndex = 11;
            // 
            // lblBusUnit
            // 
            this.lblBusUnit.AutoSize = true;
            this.lblBusUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblBusUnit.ForeColor = System.Drawing.Color.White;
            this.lblBusUnit.Location = new System.Drawing.Point(55, 370);
            this.lblBusUnit.Name = "lblBusUnit";
            this.lblBusUnit.Size = new System.Drawing.Size(74, 13);
            this.lblBusUnit.TabIndex = 201;
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
            this.cboAssignedTo.Location = new System.Drawing.Point(494, 339);
            this.cboAssignedTo.Name = "cboAssignedTo";
            this.cboAssignedTo.Size = new System.Drawing.Size(272, 21);
            this.cboAssignedTo.TabIndex = 14;
            this.cboAssignedTo.ValueMember = "SecurityId";
            // 
            // txtHistory
            // 
            this.txtHistory.CausesValidation = false;
            this.txtHistory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "History", true));
            this.txtHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHistory.Location = new System.Drawing.Point(512, 71);
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ReadOnly = true;
            this.txtHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHistory.Size = new System.Drawing.Size(662, 259);
            this.txtHistory.TabIndex = 198;
            // 
            // lblCurrentFiles
            // 
            this.lblCurrentFiles.AutoSize = true;
            this.lblCurrentFiles.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentFiles.ForeColor = System.Drawing.Color.White;
            this.lblCurrentFiles.Location = new System.Drawing.Point(61, 439);
            this.lblCurrentFiles.Name = "lblCurrentFiles";
            this.lblCurrentFiles.Size = new System.Drawing.Size(68, 13);
            this.lblCurrentFiles.TabIndex = 200;
            this.lblCurrentFiles.Text = "Current Files:";
            // 
            // lbxEmailRecip
            // 
            this.lbxEmailRecip.CausesValidation = false;
            this.lbxEmailRecip.DisplayMember = "SecurityId";
            this.lbxEmailRecip.FormattingEnabled = true;
            this.lbxEmailRecip.Location = new System.Drawing.Point(493, 425);
            this.lbxEmailRecip.MultiColumn = true;
            this.lbxEmailRecip.Name = "lbxEmailRecip";
            this.lbxEmailRecip.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbxEmailRecip.Size = new System.Drawing.Size(272, 56);
            this.lbxEmailRecip.TabIndex = 17;
            this.lbxEmailRecip.ValueMember = "SecurityId";
            // 
            // userSelectedEmailRecipientsBindingSource
            // 
            this.userSelectedEmailRecipientsBindingSource.DataMember = "UserSelectedEmailRecipients";
            this.userSelectedEmailRecipientsBindingSource.DataSource = this.ticketDataBindingSource;
            // 
            // lbxSystems
            // 
            this.lbxSystems.CausesValidation = false;
            this.lbxSystems.FormattingEnabled = true;
            this.lbxSystems.Location = new System.Drawing.Point(880, 425);
            this.lbxSystems.Name = "lbxSystems";
            this.lbxSystems.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbxSystems.Size = new System.Drawing.Size(272, 56);
            this.lbxSystems.TabIndex = 227;
            // 
            // lbxCurrentFiles
            // 
            this.lbxCurrentFiles.CausesValidation = false;
            this.lbxCurrentFiles.FormattingEnabled = true;
            this.lbxCurrentFiles.HorizontalScrollbar = true;
            this.lbxCurrentFiles.Location = new System.Drawing.Point(135, 425);
            this.lbxCurrentFiles.Name = "lbxCurrentFiles";
            this.lbxCurrentFiles.Size = new System.Drawing.Size(279, 56);
            this.lbxCurrentFiles.TabIndex = 228;
            this.lbxCurrentFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LbxCurrentFiles_MouseDoubleClick);
            // 
            // txtSubject
            // 
            this.txtSubject.CausesValidation = false;
            this.txtSubject.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ticketDataBindingSource, "Subject", true));
            this.txtSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubject.Location = new System.Drawing.Point(135, 339);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(279, 20);
            this.txtSubject.TabIndex = 229;
            // 
            // RemoveRecipient
            // 
            this.RemoveRecipient.Image = ((System.Drawing.Image)(resources.GetObject("RemoveRecipient.Image")));
            this.RemoveRecipient.InitialImage = ((System.Drawing.Image)(resources.GetObject("RemoveRecipient.InitialImage")));
            this.RemoveRecipient.Location = new System.Drawing.Point(771, 452);
            this.RemoveRecipient.Name = "RemoveRecipient";
            this.RemoveRecipient.Size = new System.Drawing.Size(15, 15);
            this.RemoveRecipient.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.RemoveRecipient.TabIndex = 230;
            this.RemoveRecipient.TabStop = false;
            this.RemoveRecipient.Click += new System.EventHandler(this.RemoveRecipient_Click);
            // 
            // General
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.CausesValidation = false;
            this.Controls.Add(this.RemoveRecipient);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.lbxCurrentFiles);
            this.Controls.Add(this.lbxSystems);
            this.Controls.Add(this.lbxEmailRecip);
            this.Controls.Add(this.txtProcedures);
            this.Controls.Add(this.lblProcedures);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblResolution);
            this.Controls.Add(this.txtPrevention);
            this.Controls.Add(this.lblPrevention);
            this.Controls.Add(this.txtFix);
            this.Controls.Add(this.lblFix);
            this.Controls.Add(this.lblCause);
            this.Controls.Add(this.cboCause);
            this.Controls.Add(this.txtQC);
            this.Controls.Add(this.lblQC);
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
            this.Controls.Add(this.txtCCC);
            this.Controls.Add(this.lblCCC);
            this.Controls.Add(this.txtRequestProj);
            this.Controls.Add(this.cboRequester);
            this.Controls.Add(this.dtpReqDate);
            this.Controls.Add(this.dtpRequestDate);
            this.Controls.Add(this.lblRequiredDate);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.lblRequestProj);
            this.Controls.Add(this.dtpStatusDate);
            this.Controls.Add(this.cboFuncArea);
            this.Controls.Add(this.cboCourt);
            this.Controls.Add(this.lblFunctionalArea);
            this.Controls.Add(this.dtpCourtDate);
            this.Controls.Add(this.lblRequester);
            this.Controls.Add(this.txtUpdate);
            this.Controls.Add(this.lblRequesetDate);
            this.Controls.Add(this.cboBussUnit);
            this.Controls.Add(this.lblBusUnit);
            this.Controls.Add(this.cboAssignedTo);
            this.Controls.Add(this.txtHistory);
            this.Controls.Add(this.lblCurrentFiles);
            this.Name = "General";
            this.Size = new System.Drawing.Size(1188, 600);
            ((System.ComponentModel.ISupportInitialize)(this.ticketDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activeDirectoryUserBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userSelectedEmailRecipientsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoveRecipient)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtProcedures;
        private System.Windows.Forms.Label lblProcedures;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblResolution;
        private System.Windows.Forms.TextBox txtPrevention;
        private System.Windows.Forms.Label lblPrevention;
        private System.Windows.Forms.TextBox txtFix;
        private System.Windows.Forms.Label lblFix;
        private System.Windows.Forms.Label lblCause;
        private System.Windows.Forms.ComboBox cboCause;
        private System.Windows.Forms.TextBox txtQC;
        private System.Windows.Forms.Label lblQC;
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
        private System.Windows.Forms.TextBox txtCCC;
        private System.Windows.Forms.Label lblCCC;
        private System.Windows.Forms.TextBox txtRequestProj;
        private System.Windows.Forms.ComboBox cboRequester;
        private System.Windows.Forms.DateTimePicker dtpReqDate;
        private System.Windows.Forms.DateTimePicker dtpRequestDate;
        private System.Windows.Forms.Label lblRequiredDate;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label lblRequestProj;
        private System.Windows.Forms.DateTimePicker dtpStatusDate;
        private System.Windows.Forms.ComboBox cboFuncArea;
        private System.Windows.Forms.ComboBox cboCourt;
        private System.Windows.Forms.Label lblFunctionalArea;
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
        public System.Windows.Forms.ListBox lbxSystems;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.BindingSource userSelectedEmailRecipientsBindingSource;
        private System.Windows.Forms.BindingSource activeDirectoryUserBindingSource;
        private System.Windows.Forms.ListBox lbxEmailRecip;
        private System.Windows.Forms.ListBox lbxCurrentFiles;
        private System.Windows.Forms.PictureBox RemoveRecipient;

    }
}
