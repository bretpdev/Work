namespace NHGeneral
{
    partial class NeedHelpTickets
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NeedHelpTickets));
            this.pbxCompanyLogo = new System.Windows.Forms.PictureBox();
            this.pbxNeedHelpLogo = new System.Windows.Forms.PictureBox();
            this.lblHLine = new System.Windows.Forms.Label();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.lnkUpdate = new System.Windows.Forms.LinkLabel();
            this.lnkReturn = new System.Windows.Forms.LinkLabel();
            this.lnkWithdraw = new System.Windows.Forms.LinkLabel();
            this.lnkPrevious = new System.Windows.Forms.LinkLabel();
            this.lblPriority = new System.Windows.Forms.Label();
            this.lnkHold = new System.Windows.Forms.LinkLabel();
            this.lnkTicketStatus = new System.Windows.Forms.LinkLabel();
            this.pbxPriority = new System.Windows.Forms.PictureBox();
            this.pnlTicket = new System.Windows.Forms.Panel();
            this.Version = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoIndividual = new System.Windows.Forms.RadioButton();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.rdoCourt = new System.Windows.Forms.RadioButton();
            this.cboPreviousStatus = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTicketNumber = new System.Windows.Forms.Label();
            this.emailRecipientsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GeneralMenuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cboEmailRecipients = new System.Windows.Forms.ToolStripComboBox();
            this.btnAddRecipient = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnUploadFile = new System.Windows.Forms.ToolStripMenuItem();
            this.lblUploadComplete = new System.Windows.Forms.ToolStripMenuItem();
            this.TimeElapsed = new System.Windows.Forms.ToolStripMenuItem();
            this.TimerSeparator = new System.Windows.Forms.ToolStripMenuItem();
            this.StartStop = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlTickets = new System.Windows.Forms.Panel();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.StartTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbxCompanyLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNeedHelpLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPriority)).BeginInit();
            this.pnlTicket.SuspendLayout();
            this.GeneralMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbxCompanyLogo
            // 
            this.pbxCompanyLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbxCompanyLogo.Location = new System.Drawing.Point(13, 7);
            this.pbxCompanyLogo.Name = "pbxCompanyLogo";
            this.pbxCompanyLogo.Size = new System.Drawing.Size(150, 70);
            this.pbxCompanyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxCompanyLogo.TabIndex = 145;
            this.pbxCompanyLogo.TabStop = false;
            // 
            // pbxNeedHelpLogo
            // 
            this.pbxNeedHelpLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbxNeedHelpLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxNeedHelpLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbxNeedHelpLogo.Image")));
            this.pbxNeedHelpLogo.Location = new System.Drawing.Point(165, -4);
            this.pbxNeedHelpLogo.Name = "pbxNeedHelpLogo";
            this.pbxNeedHelpLogo.Size = new System.Drawing.Size(285, 84);
            this.pbxNeedHelpLogo.TabIndex = 1;
            this.pbxNeedHelpLogo.TabStop = false;
            // 
            // lblHLine
            // 
            this.lblHLine.BackColor = System.Drawing.Color.Transparent;
            this.lblHLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblHLine.CausesValidation = false;
            this.lblHLine.ForeColor = System.Drawing.Color.White;
            this.lblHLine.Location = new System.Drawing.Point(-1, 81);
            this.lblHLine.Name = "lblHLine";
            this.lblHLine.Size = new System.Drawing.Size(1210, 2);
            this.lblHLine.TabIndex = 2;
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.CausesValidation = false;
            this.lnkSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSave.LinkColor = System.Drawing.Color.Yellow;
            this.lnkSave.Location = new System.Drawing.Point(688, 35);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(40, 16);
            this.lnkSave.TabIndex = 136;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "Save";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkSave_LinkClicked);
            // 
            // lnkUpdate
            // 
            this.lnkUpdate.AutoSize = true;
            this.lnkUpdate.CausesValidation = false;
            this.lnkUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkUpdate.LinkColor = System.Drawing.Color.Yellow;
            this.lnkUpdate.Location = new System.Drawing.Point(739, 35);
            this.lnkUpdate.Name = "lnkUpdate";
            this.lnkUpdate.Size = new System.Drawing.Size(53, 16);
            this.lnkUpdate.TabIndex = 135;
            this.lnkUpdate.TabStop = true;
            this.lnkUpdate.Text = "Update";
            this.lnkUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkUpdate_LinkClicked);
            // 
            // lnkReturn
            // 
            this.lnkReturn.AutoSize = true;
            this.lnkReturn.CausesValidation = false;
            this.lnkReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkReturn.LinkColor = System.Drawing.Color.Yellow;
            this.lnkReturn.Location = new System.Drawing.Point(798, 35);
            this.lnkReturn.Name = "lnkReturn";
            this.lnkReturn.Size = new System.Drawing.Size(47, 16);
            this.lnkReturn.TabIndex = 134;
            this.lnkReturn.TabStop = true;
            this.lnkReturn.Text = "Return";
            this.lnkReturn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkReturn_LinkClicked);
            // 
            // lnkWithdraw
            // 
            this.lnkWithdraw.AutoSize = true;
            this.lnkWithdraw.CausesValidation = false;
            this.lnkWithdraw.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkWithdraw.LinkColor = System.Drawing.Color.Yellow;
            this.lnkWithdraw.Location = new System.Drawing.Point(900, 35);
            this.lnkWithdraw.Name = "lnkWithdraw";
            this.lnkWithdraw.Size = new System.Drawing.Size(63, 16);
            this.lnkWithdraw.TabIndex = 133;
            this.lnkWithdraw.TabStop = true;
            this.lnkWithdraw.Text = "Withdraw";
            this.lnkWithdraw.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkWithdraw_LinkClicked);
            // 
            // lnkPrevious
            // 
            this.lnkPrevious.AutoSize = true;
            this.lnkPrevious.CausesValidation = false;
            this.lnkPrevious.Enabled = false;
            this.lnkPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPrevious.LinkColor = System.Drawing.Color.Yellow;
            this.lnkPrevious.Location = new System.Drawing.Point(1040, 33);
            this.lnkPrevious.Name = "lnkPrevious";
            this.lnkPrevious.Size = new System.Drawing.Size(124, 16);
            this.lnkPrevious.TabIndex = 132;
            this.lnkPrevious.TabStop = true;
            this.lnkPrevious.Text = "Set Previous Status";
            this.lnkPrevious.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkPrevious_LinkClicked);
            // 
            // lblPriority
            // 
            this.lblPriority.AutoSize = true;
            this.lblPriority.BackColor = System.Drawing.Color.Transparent;
            this.lblPriority.ForeColor = System.Drawing.Color.White;
            this.lblPriority.Location = new System.Drawing.Point(474, 7);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.Size = new System.Drawing.Size(38, 13);
            this.lblPriority.TabIndex = 84;
            this.lblPriority.Text = "Priority";
            // 
            // lnkHold
            // 
            this.lnkHold.AutoSize = true;
            this.lnkHold.CausesValidation = false;
            this.lnkHold.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkHold.LinkColor = System.Drawing.Color.Yellow;
            this.lnkHold.Location = new System.Drawing.Point(851, 35);
            this.lnkHold.Name = "lnkHold";
            this.lnkHold.Size = new System.Drawing.Size(37, 16);
            this.lnkHold.TabIndex = 131;
            this.lnkHold.TabStop = true;
            this.lnkHold.Text = "Hold";
            this.lnkHold.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkHold_LinkClicked);
            // 
            // lnkTicketStatus
            // 
            this.lnkTicketStatus.AutoSize = true;
            this.lnkTicketStatus.CausesValidation = false;
            this.lnkTicketStatus.DisabledLinkColor = System.Drawing.Color.Red;
            this.lnkTicketStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkTicketStatus.LinkColor = System.Drawing.Color.Yellow;
            this.lnkTicketStatus.Location = new System.Drawing.Point(562, 35);
            this.lnkTicketStatus.Name = "lnkTicketStatus";
            this.lnkTicketStatus.Size = new System.Drawing.Size(49, 16);
            this.lnkTicketStatus.TabIndex = 130;
            this.lnkTicketStatus.TabStop = true;
            this.lnkTicketStatus.Text = "Submit";
            this.lnkTicketStatus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTicketStatus_LinkClicked);
            // 
            // pbxPriority
            // 
            this.pbxPriority.BackColor = System.Drawing.Color.Transparent;
            this.pbxPriority.Image = ((System.Drawing.Image)(resources.GetObject("pbxPriority.Image")));
            this.pbxPriority.Location = new System.Drawing.Point(469, 23);
            this.pbxPriority.Name = "pbxPriority";
            this.pbxPriority.Size = new System.Drawing.Size(50, 50);
            this.pbxPriority.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbxPriority.TabIndex = 91;
            this.pbxPriority.TabStop = false;
            // 
            // pnlTicket
            // 
            this.pnlTicket.BackColor = System.Drawing.Color.Transparent;
            this.pnlTicket.Controls.Add(this.Version);
            this.pnlTicket.Controls.Add(this.label1);
            this.pnlTicket.Controls.Add(this.rdoIndividual);
            this.pnlTicket.Controls.Add(this.rdoAll);
            this.pnlTicket.Controls.Add(this.rdoCourt);
            this.pnlTicket.Controls.Add(this.cboPreviousStatus);
            this.pnlTicket.Controls.Add(this.pbxCompanyLogo);
            this.pnlTicket.Controls.Add(this.label4);
            this.pnlTicket.Controls.Add(this.pbxNeedHelpLogo);
            this.pnlTicket.Controls.Add(this.lblHLine);
            this.pnlTicket.Controls.Add(this.lnkSave);
            this.pnlTicket.Controls.Add(this.lnkUpdate);
            this.pnlTicket.Controls.Add(this.lnkReturn);
            this.pnlTicket.Controls.Add(this.lnkWithdraw);
            this.pnlTicket.Controls.Add(this.label3);
            this.pnlTicket.Controls.Add(this.lnkPrevious);
            this.pnlTicket.Controls.Add(this.lblPriority);
            this.pnlTicket.Controls.Add(this.lnkHold);
            this.pnlTicket.Controls.Add(this.lblTicketNumber);
            this.pnlTicket.Controls.Add(this.lnkTicketStatus);
            this.pnlTicket.Controls.Add(this.pbxPriority);
            this.pnlTicket.Location = new System.Drawing.Point(0, 27);
            this.pnlTicket.Name = "pnlTicket";
            this.pnlTicket.Size = new System.Drawing.Size(1192, 86);
            this.pnlTicket.TabIndex = 149;
            // 
            // Version
            // 
            this.Version.AutoSize = true;
            this.Version.ForeColor = System.Drawing.Color.White;
            this.Version.Location = new System.Drawing.Point(1048, 7);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(35, 13);
            this.Version.TabIndex = 151;
            this.Version.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(243)))), ((int)(((byte)(229)))));
            this.label1.Location = new System.Drawing.Point(564, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 150;
            this.label1.Text = "Notify:";
            // 
            // rdoIndividual
            // 
            this.rdoIndividual.AutoSize = true;
            this.rdoIndividual.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(243)))), ((int)(((byte)(229)))));
            this.rdoIndividual.Location = new System.Drawing.Point(705, 59);
            this.rdoIndividual.Name = "rdoIndividual";
            this.rdoIndividual.Size = new System.Drawing.Size(70, 17);
            this.rdoIndividual.TabIndex = 149;
            this.rdoIndividual.Text = "Individual";
            this.rdoIndividual.UseVisualStyleBackColor = true;
            this.rdoIndividual.CheckedChanged += new System.EventHandler(this.RdoIndividual_CheckedChanged);
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = true;
            this.rdoAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(243)))), ((int)(((byte)(229)))));
            this.rdoAll.Location = new System.Drawing.Point(663, 59);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(36, 17);
            this.rdoAll.TabIndex = 148;
            this.rdoAll.Text = "All";
            this.rdoAll.UseVisualStyleBackColor = true;
            this.rdoAll.CheckedChanged += new System.EventHandler(this.RdoAll_CheckedChanged);
            // 
            // rdoCourt
            // 
            this.rdoCourt.AutoSize = true;
            this.rdoCourt.Checked = true;
            this.rdoCourt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(243)))), ((int)(((byte)(229)))));
            this.rdoCourt.Location = new System.Drawing.Point(607, 59);
            this.rdoCourt.Name = "rdoCourt";
            this.rdoCourt.Size = new System.Drawing.Size(50, 17);
            this.rdoCourt.TabIndex = 147;
            this.rdoCourt.TabStop = true;
            this.rdoCourt.Text = "Court";
            this.rdoCourt.UseVisualStyleBackColor = true;
            this.rdoCourt.CheckedChanged += new System.EventHandler(this.RdoCourt_CheckedChanged);
            // 
            // cboPreviousStatus
            // 
            this.cboPreviousStatus.FormattingEnabled = true;
            this.cboPreviousStatus.Location = new System.Drawing.Point(1024, 52);
            this.cboPreviousStatus.Name = "cboPreviousStatus";
            this.cboPreviousStatus.Size = new System.Drawing.Size(148, 21);
            this.cboPreviousStatus.TabIndex = 146;
            this.cboPreviousStatus.SelectedIndexChanged += new System.EventHandler(this.cboPreviousStatus_SelectedIndexChanged);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.label3.Location = new System.Drawing.Point(474, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 84;
            this.label3.Text = "Priority";
            // 
            // lblTicketNumber
            // 
            this.lblTicketNumber.AutoSize = true;
            this.lblTicketNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(229)))));
            this.lblTicketNumber.Location = new System.Drawing.Point(561, 3);
            this.lblTicketNumber.Name = "lblTicketNumber";
            this.lblTicketNumber.Size = new System.Drawing.Size(0, 24);
            this.lblTicketNumber.TabIndex = 129;
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
            this.addToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // uploadFileToolStripMenuItem
            // 
            this.uploadFileToolStripMenuItem.Name = "uploadFileToolStripMenuItem";
            this.uploadFileToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.uploadFileToolStripMenuItem.Text = "Upload File";
            // 
            // GeneralMenuStrip
            // 
            this.GeneralMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1,
            this.cboEmailRecipients,
            this.btnAddRecipient,
            this.toolStripMenuItem1,
            this.btnUploadFile,
            this.lblUploadComplete,
            this.TimeElapsed,
            this.TimerSeparator,
            this.StartStop});
            this.GeneralMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.GeneralMenuStrip.Name = "GeneralMenuStrip";
            this.GeneralMenuStrip.Size = new System.Drawing.Size(1192, 27);
            this.GeneralMenuStrip.TabIndex = 148;
            this.GeneralMenuStrip.Text = "menuStrip1";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(12, 23);
            // 
            // cboEmailRecipients
            // 
            this.cboEmailRecipients.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboEmailRecipients.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboEmailRecipients.Name = "cboEmailRecipients";
            this.cboEmailRecipients.Size = new System.Drawing.Size(300, 23);
            this.cboEmailRecipients.SelectedIndexChanged += new System.EventHandler(this.CboEmailRecipients_SelectedIndexChanged);
            // 
            // btnAddRecipient
            // 
            this.btnAddRecipient.Name = "btnAddRecipient";
            this.btnAddRecipient.Size = new System.Drawing.Size(125, 23);
            this.btnAddRecipient.Text = "Add Email Recipient";
            this.btnAddRecipient.Click += new System.EventHandler(this.BtnAddRecipient_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(22, 23);
            this.toolStripMenuItem1.Text = "|";
            // 
            // btnUploadFile
            // 
            this.btnUploadFile.Name = "btnUploadFile";
            this.btnUploadFile.Size = new System.Drawing.Size(78, 23);
            this.btnUploadFile.Text = "Upload File";
            this.btnUploadFile.Click += new System.EventHandler(this.BtnUploadFile_Click);
            // 
            // lblUploadComplete
            // 
            this.lblUploadComplete.ForeColor = System.Drawing.Color.Red;
            this.lblUploadComplete.Name = "lblUploadComplete";
            this.lblUploadComplete.Size = new System.Drawing.Size(112, 23);
            this.lblUploadComplete.Text = "Upload Complete";
            this.lblUploadComplete.Visible = false;
            // 
            // TimeElapsed
            // 
            this.TimeElapsed.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TimeElapsed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TimeElapsed.Enabled = false;
            this.TimeElapsed.Name = "TimeElapsed";
            this.TimeElapsed.Size = new System.Drawing.Size(61, 23);
            this.TimeElapsed.Text = "00:00:00";
            // 
            // TimerSeparator
            // 
            this.TimerSeparator.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TimerSeparator.Enabled = false;
            this.TimerSeparator.Name = "TimerSeparator";
            this.TimerSeparator.Size = new System.Drawing.Size(22, 23);
            this.TimerSeparator.Text = "|";
            // 
            // StartStop
            // 
            this.StartStop.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.StartStop.Enabled = false;
            this.StartStop.Name = "StartStop";
            this.StartStop.Size = new System.Drawing.Size(43, 23);
            this.StartStop.Text = "Start";
            this.StartStop.Click += new System.EventHandler(this.StartStop_Click);
            // 
            // pnlTickets
            // 
            this.pnlTickets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTickets.AutoScroll = true;
            this.pnlTickets.AutoSize = true;
            this.pnlTickets.BackColor = System.Drawing.Color.Transparent;
            this.pnlTickets.Location = new System.Drawing.Point(0, 110);
            this.pnlTickets.Name = "pnlTickets";
            this.pnlTickets.Size = new System.Drawing.Size(1192, 462);
            this.pnlTickets.TabIndex = 150;
            // 
            // fileDialog
            // 
            this.fileDialog.FileName = "openFileDialog1";
            this.fileDialog.Filter = "All Files (*.*)|*.*";
            this.fileDialog.InitialDirectory = "T:\\";
            this.fileDialog.Title = "Select File to Attach to Ticket";
            // 
            // saveFile
            // 
            this.saveFile.InitialDirectory = "T:\\";
            // 
            // StartTimer
            // 
            this.StartTimer.Interval = 1000;
            this.StartTimer.Tick += new System.EventHandler(this.StartTimer_Tick);
            // 
            // NeedHelpTickets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(47)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(1192, 574);
            this.Controls.Add(this.pnlTickets);
            this.Controls.Add(this.pnlTicket);
            this.Controls.Add(this.GeneralMenuStrip);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(2)))), ((int)(((byte)(10)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NeedHelpTickets";
            this.Text = "Need Help";
            ((System.ComponentModel.ISupportInitialize)(this.pbxCompanyLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNeedHelpLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPriority)).EndInit();
            this.pnlTicket.ResumeLayout(false);
            this.pnlTicket.PerformLayout();
            this.GeneralMenuStrip.ResumeLayout(false);
            this.GeneralMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxCompanyLogo;
        private System.Windows.Forms.PictureBox pbxNeedHelpLogo;
        private System.Windows.Forms.Label lblHLine;
        private System.Windows.Forms.LinkLabel lnkSave;
        private System.Windows.Forms.LinkLabel lnkUpdate;
        private System.Windows.Forms.LinkLabel lnkReturn;
        private System.Windows.Forms.LinkLabel lnkWithdraw;
        private System.Windows.Forms.LinkLabel lnkPrevious;
        private System.Windows.Forms.Label lblPriority;
        private System.Windows.Forms.LinkLabel lnkHold;
		public System.Windows.Forms.LinkLabel lnkTicketStatus;
        private System.Windows.Forms.PictureBox pbxPriority;
        private System.Windows.Forms.Panel pnlTicket;
        private System.Windows.Forms.ToolStripMenuItem emailRecipientsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadFileToolStripMenuItem;
		private System.Windows.Forms.MenuStrip GeneralMenuStrip;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTicketNumber;
        private System.Windows.Forms.ToolStripMenuItem toolStripComboBox1;
        private System.Windows.Forms.ToolStripComboBox cboEmailRecipients;
        private System.Windows.Forms.ToolStripMenuItem btnAddRecipient;
        private System.Windows.Forms.Panel pnlTickets;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem btnUploadFile;
        private System.Windows.Forms.OpenFileDialog fileDialog;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.ToolStripMenuItem lblUploadComplete;
        private System.Windows.Forms.ComboBox cboPreviousStatus;
		private System.Windows.Forms.RadioButton rdoIndividual;
		private System.Windows.Forms.RadioButton rdoAll;
		private System.Windows.Forms.RadioButton rdoCourt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem StartStop;
        private System.Windows.Forms.ToolStripMenuItem TimerSeparator;
        private System.Windows.Forms.ToolStripMenuItem TimeElapsed;
        private System.Windows.Forms.Timer StartTimer;
        private System.Windows.Forms.Label Version;
    }
}