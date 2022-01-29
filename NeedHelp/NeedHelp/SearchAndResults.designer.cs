using SubSystemShared;
using Uheaa.Common.ProcessLogger;
namespace NeedHelp
{
    partial class SearchAndResults
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchAndResults));
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.gbxSearch = new System.Windows.Forms.GroupBox();
            this.txtTicketNo = new Uheaa.Common.WinForms.NumericTextBox();
            this.cboSorting = new System.Windows.Forms.ComboBox();
            this.cboAssignedTo = new System.Windows.Forms.ComboBox();
            this.cboFunctional = new System.Windows.Forms.ComboBox();
            this.cboBusinessUnit = new System.Windows.Forms.ComboBox();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.cboRequester = new System.Windows.Forms.ComboBox();
            this.cboCourt = new System.Windows.Forms.ComboBox();
            this.cboSubject = new System.Windows.Forms.ComboBox();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.cboKeyword = new System.Windows.Forms.ComboBox();
            this.keywordScopeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.lblTye = new System.Windows.Forms.Label();
            this.txtKeywordScope = new System.Windows.Forms.TextBox();
            this.searchCriteriaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblDateTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblSorting = new System.Windows.Forms.Label();
            this.lblFunctional = new System.Windows.Forms.Label();
            this.lblAssignedTo = new System.Windows.Forms.Label();
            this.btnAllTickets = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnOpenTickets = new System.Windows.Forms.Button();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.lblBusinessUnit = new System.Windows.Forms.Label();
            this.lblRequester = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCourt = new System.Windows.Forms.Label();
            this.lblSubject = new System.Windows.Forms.Label();
            this.lblKeywordScope = new System.Windows.Forms.Label();
            this.lblTicketNo = new System.Windows.Forms.Label();
            this.sortFieldBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.assignedToBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ticketTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.businessUnitBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.requesterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.courtBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblCreateNew = new System.Windows.Forms.Label();
            this.lblResults = new System.Windows.Forms.Label();
            this.gbxResults = new System.Windows.Forms.GroupBox();
            this.dgvSearchResults = new System.Windows.Forms.DataGridView();
            this.ticketCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketNumberDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subjectDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priorityDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.searchResultItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TicketCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subjectDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priorityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCreateNew = new System.Windows.Forms.Button();
            this.cboNewTicketType = new System.Windows.Forms.ComboBox();
            this.lblUheaaSubSystems = new System.Windows.Forms.Label();
            this.pnlUheaaSubSystems = new System.Windows.Forms.FlowLayoutPanel();
            this.lblAllStaff = new System.Windows.Forms.Label();
            this.pnlAllStaff = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.gbxSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.keywordScopeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriaBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sortFieldBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assignedToBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessUnitBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.requesterBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.courtBindingSource)).BeginInit();
            this.gbxResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchResultItemBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(634, -8);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(544, 176);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // gbxSearch
            // 
            this.gbxSearch.BackColor = System.Drawing.Color.Transparent;
            this.gbxSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gbxSearch.BackgroundImage")));
            this.gbxSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.gbxSearch.Controls.Add(this.txtTicketNo);
            this.gbxSearch.Controls.Add(this.cboSorting);
            this.gbxSearch.Controls.Add(this.cboAssignedTo);
            this.gbxSearch.Controls.Add(this.cboFunctional);
            this.gbxSearch.Controls.Add(this.cboBusinessUnit);
            this.gbxSearch.Controls.Add(this.cboStatus);
            this.gbxSearch.Controls.Add(this.cboRequester);
            this.gbxSearch.Controls.Add(this.cboCourt);
            this.gbxSearch.Controls.Add(this.cboSubject);
            this.gbxSearch.Controls.Add(this.cboType);
            this.gbxSearch.Controls.Add(this.btnClear);
            this.gbxSearch.Controls.Add(this.cboKeyword);
            this.gbxSearch.Controls.Add(this.lblDateFrom);
            this.gbxSearch.Controls.Add(this.lblTye);
            this.gbxSearch.Controls.Add(this.txtKeywordScope);
            this.gbxSearch.Controls.Add(this.lblDateTo);
            this.gbxSearch.Controls.Add(this.dtpTo);
            this.gbxSearch.Controls.Add(this.dtpFrom);
            this.gbxSearch.Controls.Add(this.lblSorting);
            this.gbxSearch.Controls.Add(this.lblFunctional);
            this.gbxSearch.Controls.Add(this.lblAssignedTo);
            this.gbxSearch.Controls.Add(this.btnAllTickets);
            this.gbxSearch.Controls.Add(this.btnSearch);
            this.gbxSearch.Controls.Add(this.btnOpenTickets);
            this.gbxSearch.Controls.Add(this.txtKeyword);
            this.gbxSearch.Controls.Add(this.lblBusinessUnit);
            this.gbxSearch.Controls.Add(this.lblRequester);
            this.gbxSearch.Controls.Add(this.lblStatus);
            this.gbxSearch.Controls.Add(this.lblCourt);
            this.gbxSearch.Controls.Add(this.lblSubject);
            this.gbxSearch.Controls.Add(this.lblKeywordScope);
            this.gbxSearch.Controls.Add(this.lblTicketNo);
            this.gbxSearch.Enabled = false;
            this.gbxSearch.Location = new System.Drawing.Point(14, 206);
            this.gbxSearch.Name = "gbxSearch";
            this.gbxSearch.Size = new System.Drawing.Size(418, 498);
            this.gbxSearch.TabIndex = 2;
            this.gbxSearch.TabStop = false;
            // 
            // txtTicketNo
            // 
            this.txtTicketNo.AllowedSpecialCharacters = "";
            this.txtTicketNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTicketNo.Location = new System.Drawing.Point(146, 19);
            this.txtTicketNo.MaximumSize = new System.Drawing.Size(247, 28);
            this.txtTicketNo.MaxLength = 10;
            this.txtTicketNo.Name = "txtTicketNo";
            this.txtTicketNo.Size = new System.Drawing.Size(247, 26);
            this.txtTicketNo.TabIndex = 34;
            // 
            // cboSorting
            // 
            this.cboSorting.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSorting.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSorting.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSorting.ForeColor = System.Drawing.Color.Black;
            this.cboSorting.FormattingEnabled = true;
            this.cboSorting.Location = new System.Drawing.Point(146, 358);
            this.cboSorting.Name = "cboSorting";
            this.cboSorting.Size = new System.Drawing.Size(247, 28);
            this.cboSorting.TabIndex = 11;
            // 
            // cboAssignedTo
            // 
            this.cboAssignedTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboAssignedTo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAssignedTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAssignedTo.ForeColor = System.Drawing.Color.Black;
            this.cboAssignedTo.FormattingEnabled = true;
            this.cboAssignedTo.Location = new System.Drawing.Point(146, 324);
            this.cboAssignedTo.Name = "cboAssignedTo";
            this.cboAssignedTo.Size = new System.Drawing.Size(247, 28);
            this.cboAssignedTo.TabIndex = 10;
            // 
            // cboFunctional
            // 
            this.cboFunctional.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboFunctional.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFunctional.DropDownWidth = 500;
            this.cboFunctional.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFunctional.ForeColor = System.Drawing.Color.Black;
            this.cboFunctional.FormattingEnabled = true;
            this.cboFunctional.Location = new System.Drawing.Point(146, 290);
            this.cboFunctional.Name = "cboFunctional";
            this.cboFunctional.Size = new System.Drawing.Size(247, 28);
            this.cboFunctional.TabIndex = 9;
            // 
            // cboBusinessUnit
            // 
            this.cboBusinessUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboBusinessUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboBusinessUnit.DropDownWidth = 350;
            this.cboBusinessUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBusinessUnit.ForeColor = System.Drawing.Color.Black;
            this.cboBusinessUnit.FormattingEnabled = true;
            this.cboBusinessUnit.Location = new System.Drawing.Point(146, 256);
            this.cboBusinessUnit.Name = "cboBusinessUnit";
            this.cboBusinessUnit.Size = new System.Drawing.Size(247, 28);
            this.cboBusinessUnit.TabIndex = 8;
            // 
            // cboStatus
            // 
            this.cboStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStatus.DropDownWidth = 250;
            this.cboStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStatus.ForeColor = System.Drawing.Color.Black;
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(146, 222);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(247, 28);
            this.cboStatus.TabIndex = 7;
            // 
            // cboRequester
            // 
            this.cboRequester.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboRequester.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboRequester.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRequester.ForeColor = System.Drawing.Color.Black;
            this.cboRequester.FormattingEnabled = true;
            this.cboRequester.Location = new System.Drawing.Point(146, 188);
            this.cboRequester.Name = "cboRequester";
            this.cboRequester.Size = new System.Drawing.Size(247, 28);
            this.cboRequester.TabIndex = 6;
            // 
            // cboCourt
            // 
            this.cboCourt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboCourt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCourt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCourt.ForeColor = System.Drawing.Color.Black;
            this.cboCourt.FormattingEnabled = true;
            this.cboCourt.Location = new System.Drawing.Point(146, 154);
            this.cboCourt.Name = "cboCourt";
            this.cboCourt.Size = new System.Drawing.Size(247, 28);
            this.cboCourt.TabIndex = 5;
            // 
            // cboSubject
            // 
            this.cboSubject.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSubject.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSubject.DropDownWidth = 450;
            this.cboSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSubject.ForeColor = System.Drawing.Color.Black;
            this.cboSubject.FormattingEnabled = true;
            this.cboSubject.Location = new System.Drawing.Point(146, 120);
            this.cboSubject.Name = "cboSubject";
            this.cboSubject.Size = new System.Drawing.Size(247, 28);
            this.cboSubject.TabIndex = 4;
            // 
            // cboType
            // 
            this.cboType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboType.DropDownWidth = 450;
            this.cboType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboType.ForeColor = System.Drawing.Color.Black;
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(146, 51);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(247, 28);
            this.cboType.TabIndex = 1;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.SystemColors.Control;
            this.btnClear.Location = new System.Drawing.Point(308, 459);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(85, 23);
            this.btnClear.TabIndex = 17;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // cboKeyword
            // 
            this.cboKeyword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboKeyword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboKeyword.DataSource = this.keywordScopeBindingSource;
            this.cboKeyword.Location = new System.Drawing.Point(308, 89);
            this.cboKeyword.Name = "cboKeyword";
            this.cboKeyword.Size = new System.Drawing.Size(78, 21);
            this.cboKeyword.TabIndex = 3;
            // 
            // keywordScopeBindingSource
            // 
            this.keywordScopeBindingSource.DataSource = typeof(SubSystemShared.KeyWordScope);
            // 
            // lblDateFrom
            // 
            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblDateFrom.Location = new System.Drawing.Point(80, 396);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.Size = new System.Drawing.Size(56, 20);
            this.lblDateFrom.TabIndex = 31;
            this.lblDateFrom.Text = "Dates:";
            // 
            // lblTye
            // 
            this.lblTye.AutoSize = true;
            this.lblTye.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTye.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblTye.Location = new System.Drawing.Point(89, 54);
            this.lblTye.Name = "lblTye";
            this.lblTye.Size = new System.Drawing.Size(47, 20);
            this.lblTye.TabIndex = 24;
            this.lblTye.Text = "Type:";
            // 
            // txtKeywordScope
            // 
            this.txtKeywordScope.BackColor = System.Drawing.Color.SkyBlue;
            this.txtKeywordScope.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtKeywordScope.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.searchCriteriaBindingSource, "KeyWord", true));
            this.txtKeywordScope.ForeColor = System.Drawing.Color.SkyBlue;
            this.txtKeywordScope.Location = new System.Drawing.Point(294, 85);
            this.txtKeywordScope.MaxLength = 1;
            this.txtKeywordScope.Multiline = true;
            this.txtKeywordScope.Name = "txtKeywordScope";
            this.txtKeywordScope.Size = new System.Drawing.Size(98, 29);
            this.txtKeywordScope.TabIndex = 33;
            // 
            // searchCriteriaBindingSource
            // 
            this.searchCriteriaBindingSource.DataSource = typeof(SubSystemShared.SearchCriteria);
            // 
            // lblDateTo
            // 
            this.lblDateTo.AutoSize = true;
            this.lblDateTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblDateTo.Location = new System.Drawing.Point(263, 392);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.Size = new System.Drawing.Size(16, 24);
            this.lblDateTo.TabIndex = 30;
            this.lblDateTo.Text = "-";
            // 
            // dtpTo
            // 
            this.dtpTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(281, 392);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(112, 26);
            this.dtpTo.TabIndex = 13;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(146, 392);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(109, 26);
            this.dtpFrom.TabIndex = 12;
            this.dtpFrom.Value = new System.DateTime(2012, 1, 29, 0, 0, 0, 0);
            // 
            // lblSorting
            // 
            this.lblSorting.AutoSize = true;
            this.lblSorting.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSorting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblSorting.Location = new System.Drawing.Point(72, 361);
            this.lblSorting.Name = "lblSorting";
            this.lblSorting.Size = new System.Drawing.Size(64, 20);
            this.lblSorting.TabIndex = 27;
            this.lblSorting.Text = "Sorting:";
            // 
            // lblFunctional
            // 
            this.lblFunctional.AutoSize = true;
            this.lblFunctional.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFunctional.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblFunctional.Location = new System.Drawing.Point(13, 293);
            this.lblFunctional.Name = "lblFunctional";
            this.lblFunctional.Size = new System.Drawing.Size(125, 20);
            this.lblFunctional.TabIndex = 26;
            this.lblFunctional.Text = "Functional Area:";
            // 
            // lblAssignedTo
            // 
            this.lblAssignedTo.AutoSize = true;
            this.lblAssignedTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAssignedTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblAssignedTo.Location = new System.Drawing.Point(35, 327);
            this.lblAssignedTo.Name = "lblAssignedTo";
            this.lblAssignedTo.Size = new System.Drawing.Size(101, 20);
            this.lblAssignedTo.TabIndex = 25;
            this.lblAssignedTo.Text = "Assigned To:";
            // 
            // btnAllTickets
            // 
            this.btnAllTickets.BackColor = System.Drawing.SystemColors.Control;
            this.btnAllTickets.Location = new System.Drawing.Point(25, 459);
            this.btnAllTickets.Name = "btnAllTickets";
            this.btnAllTickets.Size = new System.Drawing.Size(85, 23);
            this.btnAllTickets.TabIndex = 16;
            this.btnAllTickets.Text = "All Tickets";
            this.btnAllTickets.UseVisualStyleBackColor = true;
            this.btnAllTickets.Click += new System.EventHandler(this.BtnAllTickets_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearch.Location = new System.Drawing.Point(214, 459);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 23);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // btnOpenTickets
            // 
            this.btnOpenTickets.BackColor = System.Drawing.SystemColors.Control;
            this.btnOpenTickets.Location = new System.Drawing.Point(120, 459);
            this.btnOpenTickets.Name = "btnOpenTickets";
            this.btnOpenTickets.Size = new System.Drawing.Size(85, 23);
            this.btnOpenTickets.TabIndex = 15;
            this.btnOpenTickets.Text = "Open Tickets";
            this.btnOpenTickets.UseVisualStyleBackColor = true;
            this.btnOpenTickets.Click += new System.EventHandler(this.BtnOpenTickets_Click);
            // 
            // txtKeyword
            // 
            this.txtKeyword.BackColor = System.Drawing.Color.SkyBlue;
            this.txtKeyword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtKeyword.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKeyword.Location = new System.Drawing.Point(146, 85);
            this.txtKeyword.Multiline = true;
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(156, 29);
            this.txtKeyword.TabIndex = 2;
            // 
            // lblBusinessUnit
            // 
            this.lblBusinessUnit.AutoSize = true;
            this.lblBusinessUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBusinessUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblBusinessUnit.Location = new System.Drawing.Point(25, 259);
            this.lblBusinessUnit.Name = "lblBusinessUnit";
            this.lblBusinessUnit.Size = new System.Drawing.Size(111, 20);
            this.lblBusinessUnit.TabIndex = 15;
            this.lblBusinessUnit.Text = "Business Unit:";
            // 
            // lblRequester
            // 
            this.lblRequester.AutoSize = true;
            this.lblRequester.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequester.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblRequester.Location = new System.Drawing.Point(48, 191);
            this.lblRequester.Name = "lblRequester";
            this.lblRequester.Size = new System.Drawing.Size(88, 20);
            this.lblRequester.TabIndex = 14;
            this.lblRequester.Text = "Requester:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblStatus.Location = new System.Drawing.Point(76, 225);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 20);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "Status:";
            // 
            // lblCourt
            // 
            this.lblCourt.AutoSize = true;
            this.lblCourt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCourt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblCourt.Location = new System.Drawing.Point(84, 157);
            this.lblCourt.Name = "lblCourt";
            this.lblCourt.Size = new System.Drawing.Size(52, 20);
            this.lblCourt.TabIndex = 12;
            this.lblCourt.Text = "Court:";
            // 
            // lblSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubject.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblSubject.Location = new System.Drawing.Point(69, 123);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(67, 20);
            this.lblSubject.TabIndex = 11;
            this.lblSubject.Text = "Subject:";
            // 
            // lblKeywordScope
            // 
            this.lblKeywordScope.AutoSize = true;
            this.lblKeywordScope.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeywordScope.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblKeywordScope.Location = new System.Drawing.Point(13, 91);
            this.lblKeywordScope.Name = "lblKeywordScope";
            this.lblKeywordScope.Size = new System.Drawing.Size(123, 20);
            this.lblKeywordScope.TabIndex = 9;
            this.lblKeywordScope.Text = "Keyword Scope:";
            // 
            // lblTicketNo
            // 
            this.lblTicketNo.AutoSize = true;
            this.lblTicketNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblTicketNo.Location = new System.Drawing.Point(21, 22);
            this.lblTicketNo.Name = "lblTicketNo";
            this.lblTicketNo.Size = new System.Drawing.Size(115, 20);
            this.lblTicketNo.TabIndex = 8;
            this.lblTicketNo.Text = "Ticket Number:";
            // 
            // sortFieldBindingSource
            // 
            this.sortFieldBindingSource.DataSource = typeof(SubSystemShared.SortField);
            // 
            // assignedToBindingSource
            // 
            this.assignedToBindingSource.DataSource = typeof(SubSystemShared.SqlUser);
            // 
            // ticketTypeBindingSource
            // 
            this.ticketTypeBindingSource.DataSource = typeof(SubSystemShared.TicketType);
            // 
            // businessUnitBindingSource
            // 
            this.businessUnitBindingSource.DataSource = typeof(Uheaa.Common.ProcessLogger.BusinessUnit);
            // 
            // requesterBindingSource
            // 
            this.requesterBindingSource.DataSource = typeof(SubSystemShared.SqlUser);
            // 
            // courtBindingSource
            // 
            this.courtBindingSource.DataSource = typeof(SubSystemShared.SqlUser);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.BackColor = System.Drawing.Color.Transparent;
            this.lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblSearch.Location = new System.Drawing.Point(9, 173);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(87, 26);
            this.lblSearch.TabIndex = 3;
            this.lblSearch.Text = "Search";
            // 
            // lblCreateNew
            // 
            this.lblCreateNew.AutoSize = true;
            this.lblCreateNew.BackColor = System.Drawing.Color.Transparent;
            this.lblCreateNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreateNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblCreateNew.Location = new System.Drawing.Point(671, 150);
            this.lblCreateNew.Name = "lblCreateNew";
            this.lblCreateNew.Size = new System.Drawing.Size(137, 26);
            this.lblCreateNew.TabIndex = 5;
            this.lblCreateNew.Text = "Create New";
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.BackColor = System.Drawing.Color.Transparent;
            this.lblResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResults.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblResults.Location = new System.Drawing.Point(451, 173);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(174, 26);
            this.lblResults.TabIndex = 6;
            this.lblResults.Text = "Search Results";
            // 
            // gbxResults
            // 
            this.gbxResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxResults.BackColor = System.Drawing.Color.Transparent;
            this.gbxResults.Controls.Add(this.dgvSearchResults);
            this.gbxResults.Location = new System.Drawing.Point(456, 207);
            this.gbxResults.Name = "gbxResults";
            this.gbxResults.Size = new System.Drawing.Size(722, 498);
            this.gbxResults.TabIndex = 7;
            this.gbxResults.TabStop = false;
            // 
            // dgvSearchResults
            // 
            this.dgvSearchResults.AllowUserToAddRows = false;
            this.dgvSearchResults.AllowUserToDeleteRows = false;
            this.dgvSearchResults.AutoGenerateColumns = false;
            this.dgvSearchResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearchResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ticketCodeDataGridViewTextBoxColumn,
            this.ticketNumberDataGridViewTextBoxColumn1,
            this.subjectDataGridViewTextBoxColumn1,
            this.statusDataGridViewTextBoxColumn1,
            this.priorityDataGridViewTextBoxColumn1});
            this.dgvSearchResults.DataSource = this.searchResultItemBindingSource;
            this.dgvSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSearchResults.Location = new System.Drawing.Point(3, 16);
            this.dgvSearchResults.MultiSelect = false;
            this.dgvSearchResults.Name = "dgvSearchResults";
            this.dgvSearchResults.ReadOnly = true;
            this.dgvSearchResults.RowHeadersVisible = false;
            this.dgvSearchResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSearchResults.Size = new System.Drawing.Size(716, 479);
            this.dgvSearchResults.TabIndex = 0;
            this.dgvSearchResults.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSearchResults_CellDoubleClick);
            this.dgvSearchResults.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvSearchResults_ColumnHeaderMouseClick);
            // 
            // ticketCodeDataGridViewTextBoxColumn
            // 
            this.ticketCodeDataGridViewTextBoxColumn.DataPropertyName = "TicketCode";
            this.ticketCodeDataGridViewTextBoxColumn.HeaderText = "Type";
            this.ticketCodeDataGridViewTextBoxColumn.Name = "ticketCodeDataGridViewTextBoxColumn";
            this.ticketCodeDataGridViewTextBoxColumn.ReadOnly = true;
            this.ticketCodeDataGridViewTextBoxColumn.Width = 75;
            // 
            // ticketNumberDataGridViewTextBoxColumn1
            // 
            this.ticketNumberDataGridViewTextBoxColumn1.DataPropertyName = "TicketNumber";
            this.ticketNumberDataGridViewTextBoxColumn1.HeaderText = "Ticket";
            this.ticketNumberDataGridViewTextBoxColumn1.Name = "ticketNumberDataGridViewTextBoxColumn1";
            this.ticketNumberDataGridViewTextBoxColumn1.ReadOnly = true;
            this.ticketNumberDataGridViewTextBoxColumn1.Width = 75;
            // 
            // subjectDataGridViewTextBoxColumn1
            // 
            this.subjectDataGridViewTextBoxColumn1.DataPropertyName = "Subject";
            this.subjectDataGridViewTextBoxColumn1.HeaderText = "Subject";
            this.subjectDataGridViewTextBoxColumn1.MinimumWidth = 350;
            this.subjectDataGridViewTextBoxColumn1.Name = "subjectDataGridViewTextBoxColumn1";
            this.subjectDataGridViewTextBoxColumn1.ReadOnly = true;
            this.subjectDataGridViewTextBoxColumn1.Width = 350;
            // 
            // statusDataGridViewTextBoxColumn1
            // 
            this.statusDataGridViewTextBoxColumn1.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn1.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn1.Name = "statusDataGridViewTextBoxColumn1";
            this.statusDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // priorityDataGridViewTextBoxColumn1
            // 
            this.priorityDataGridViewTextBoxColumn1.DataPropertyName = "Priority";
            this.priorityDataGridViewTextBoxColumn1.HeaderText = "PR";
            this.priorityDataGridViewTextBoxColumn1.Name = "priorityDataGridViewTextBoxColumn1";
            this.priorityDataGridViewTextBoxColumn1.ReadOnly = true;
            this.priorityDataGridViewTextBoxColumn1.Width = 35;
            // 
            // searchResultItemBindingSource
            // 
            this.searchResultItemBindingSource.DataSource = typeof(SubSystemShared.SearchResultItem);
            // 
            // TicketCode
            // 
            this.TicketCode.Name = "TicketCode";
            // 
            // ticketNumberDataGridViewTextBoxColumn
            // 
            this.ticketNumberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ticketNumberDataGridViewTextBoxColumn.DataPropertyName = "TicketNumber";
            this.ticketNumberDataGridViewTextBoxColumn.Frozen = true;
            this.ticketNumberDataGridViewTextBoxColumn.HeaderText = "#";
            this.ticketNumberDataGridViewTextBoxColumn.Name = "ticketNumberDataGridViewTextBoxColumn";
            this.ticketNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.ticketNumberDataGridViewTextBoxColumn.Width = 50;
            // 
            // subjectDataGridViewTextBoxColumn
            // 
            this.subjectDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.subjectDataGridViewTextBoxColumn.DataPropertyName = "Subject";
            this.subjectDataGridViewTextBoxColumn.Frozen = true;
            this.subjectDataGridViewTextBoxColumn.HeaderText = "Subject";
            this.subjectDataGridViewTextBoxColumn.MinimumWidth = 450;
            this.subjectDataGridViewTextBoxColumn.Name = "subjectDataGridViewTextBoxColumn";
            this.subjectDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.Frozen = true;
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.MinimumWidth = 110;
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // priorityDataGridViewTextBoxColumn
            // 
            this.priorityDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.priorityDataGridViewTextBoxColumn.DataPropertyName = "Priority";
            this.priorityDataGridViewTextBoxColumn.Frozen = true;
            this.priorityDataGridViewTextBoxColumn.HeaderText = "Pri";
            this.priorityDataGridViewTextBoxColumn.Name = "priorityDataGridViewTextBoxColumn";
            this.priorityDataGridViewTextBoxColumn.ReadOnly = true;
            this.priorityDataGridViewTextBoxColumn.Width = 30;
            // 
            // btnCreateNew
            // 
            this.btnCreateNew.BackColor = System.Drawing.SystemColors.Control;
            this.btnCreateNew.Enabled = false;
            this.btnCreateNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateNew.Location = new System.Drawing.Point(1039, 177);
            this.btnCreateNew.Name = "btnCreateNew";
            this.btnCreateNew.Size = new System.Drawing.Size(123, 30);
            this.btnCreateNew.TabIndex = 8;
            this.btnCreateNew.Text = "Create New";
            this.btnCreateNew.UseVisualStyleBackColor = true;
            this.btnCreateNew.Click += new System.EventHandler(this.BtnCreateNew_Click);
            // 
            // cboNewTicketType
            // 
            this.cboNewTicketType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboNewTicketType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNewTicketType.DataSource = this.ticketTypeBindingSource;
            this.cboNewTicketType.DisplayMember = "Description";
            this.cboNewTicketType.Enabled = false;
            this.cboNewTicketType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboNewTicketType.Location = new System.Drawing.Point(676, 180);
            this.cboNewTicketType.Name = "cboNewTicketType";
            this.cboNewTicketType.Size = new System.Drawing.Size(332, 24);
            this.cboNewTicketType.TabIndex = 0;
            // 
            // lblUheaaSubSystems
            // 
            this.lblUheaaSubSystems.AutoSize = true;
            this.lblUheaaSubSystems.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUheaaSubSystems.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblUheaaSubSystems.Location = new System.Drawing.Point(67, 10);
            this.lblUheaaSubSystems.Name = "lblUheaaSubSystems";
            this.lblUheaaSubSystems.Size = new System.Drawing.Size(62, 20);
            this.lblUheaaSubSystems.TabIndex = 10;
            this.lblUheaaSubSystems.Text = "Uheaa";
            this.lblUheaaSubSystems.Visible = false;
            // 
            // pnlUheaaSubSystems
            // 
            this.pnlUheaaSubSystems.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlUheaaSubSystems.Location = new System.Drawing.Point(14, 33);
            this.pnlUheaaSubSystems.Name = "pnlUheaaSubSystems";
            this.pnlUheaaSubSystems.Size = new System.Drawing.Size(175, 125);
            this.pnlUheaaSubSystems.TabIndex = 12;
            // 
            // lblAllStaff
            // 
            this.lblAllStaff.AutoSize = true;
            this.lblAllStaff.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllStaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblAllStaff.Location = new System.Drawing.Point(313, 10);
            this.lblAllStaff.Name = "lblAllStaff";
            this.lblAllStaff.Size = new System.Drawing.Size(59, 20);
            this.lblAllStaff.TabIndex = 13;
            this.lblAllStaff.Text = "USHE";
            this.lblAllStaff.Visible = false;
            // 
            // pnlAllStaff
            // 
            this.pnlAllStaff.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlAllStaff.Location = new System.Drawing.Point(257, 33);
            this.pnlAllStaff.Name = "pnlAllStaff";
            this.pnlAllStaff.Size = new System.Drawing.Size(175, 125);
            this.pnlAllStaff.TabIndex = 13;
            // 
            // SearchAndResults
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(1192, 716);
            this.Controls.Add(this.pnlAllStaff);
            this.Controls.Add(this.lblAllStaff);
            this.Controls.Add(this.pnlUheaaSubSystems);
            this.Controls.Add(this.lblUheaaSubSystems);
            this.Controls.Add(this.cboNewTicketType);
            this.Controls.Add(this.btnCreateNew);
            this.Controls.Add(this.gbxResults);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.lblCreateNew);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.gbxSearch);
            this.Controls.Add(this.pictureBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1208, 754);
            this.Name = "SearchAndResults";
            this.Text = "SearchAndResults";
            this.Shown += new System.EventHandler(this.SearchAndResults_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.gbxSearch.ResumeLayout(false);
            this.gbxSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.keywordScopeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriaBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sortFieldBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assignedToBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.businessUnitBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.requesterBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.courtBindingSource)).EndInit();
            this.gbxResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchResultItemBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.GroupBox gbxSearch;
		private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Label lblCreateNew;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.GroupBox gbxResults;
        private System.Windows.Forms.Button btnCreateNew;
        private System.Windows.Forms.Label lblBusinessUnit;
        private System.Windows.Forms.Label lblRequester;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCourt;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.Label lblKeywordScope;
		private System.Windows.Forms.Label lblTicketNo;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Button btnAllTickets;
        private System.Windows.Forms.Button btnOpenTickets;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblDateFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblSorting;
        private System.Windows.Forms.Label lblFunctional;
        private System.Windows.Forms.Label lblAssignedTo;
        private System.Windows.Forms.Label lblTye;
        private System.Windows.Forms.ComboBox cboKeyword;
        private System.Windows.Forms.ComboBox cboNewTicketType;
        private System.Windows.Forms.BindingSource ticketTypeBindingSource;
		private System.Windows.Forms.BindingSource courtBindingSource;
		private System.Windows.Forms.BindingSource assignedToBindingSource;
		private System.Windows.Forms.BindingSource requesterBindingSource;
		private System.Windows.Forms.BindingSource sortFieldBindingSource;
		private System.Windows.Forms.BindingSource keywordScopeBindingSource;
		private System.Windows.Forms.BindingSource searchCriteriaBindingSource;
		private System.Windows.Forms.DataGridView dgvSearchResults;
		private System.Windows.Forms.BindingSource searchResultItemBindingSource;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtKeywordScope;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TicketCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticketNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subjectDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn priorityDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn ticketCodeDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn ticketNumberDataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn subjectDataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn priorityDataGridViewTextBoxColumn1;
        private System.Windows.Forms.Label lblUheaaSubSystems;
        private System.Windows.Forms.FlowLayoutPanel pnlUheaaSubSystems;
        private System.Windows.Forms.Label lblAllStaff;
        private System.Windows.Forms.BindingSource businessUnitBindingSource;
        private System.Windows.Forms.FlowLayoutPanel pnlAllStaff;
        private System.Windows.Forms.ComboBox cboCourt;
        private System.Windows.Forms.ComboBox cboSubject;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.ComboBox cboBusinessUnit;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.ComboBox cboRequester;
        private System.Windows.Forms.ComboBox cboFunctional;
        private System.Windows.Forms.ComboBox cboAssignedTo;
        private System.Windows.Forms.ComboBox cboSorting;
        private Uheaa.Common.WinForms.NumericTextBox txtTicketNo;
    }
}