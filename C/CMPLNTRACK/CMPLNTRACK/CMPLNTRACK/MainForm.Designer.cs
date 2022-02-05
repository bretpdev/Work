namespace CMPLNTRACK
{
    partial class MainForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FilterFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.AccountNumberBox = new Uheaa.Common.WinForms.AccountNumberTextBox();
            this.OpenClosedFilterBox = new System.Windows.Forms.ComboBox();
            this.FlagsFilterBox = new System.Windows.Forms.ComboBox();
            this.PartiesFilterBox = new System.Windows.Forms.ComboBox();
            this.TypesFilterBox = new System.Windows.Forms.ComboBox();
            this.GroupsFilterBox = new System.Windows.Forms.ComboBox();
            this.NewComplaintButton = new System.Windows.Forms.Button();
            this.ResultsGrid = new System.Windows.Forms.DataGridView();
            this.AccountNumberHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BorrowerNameHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartyHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddedByHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NeedHelpTicketNumberHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupNameHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.FlagsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PartiesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.TypesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.GroupsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.FilterFlowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsGrid)).BeginInit();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.FilterFlowPanel);
            this.groupBox1.Location = new System.Drawing.Point(12, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 394);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter Complaints";
            // 
            // FilterFlowPanel
            // 
            this.FilterFlowPanel.AutoScroll = true;
            this.FilterFlowPanel.Controls.Add(this.label1);
            this.FilterFlowPanel.Controls.Add(this.AccountNumberBox);
            this.FilterFlowPanel.Controls.Add(this.OpenClosedFilterBox);
            this.FilterFlowPanel.Controls.Add(this.FlagsFilterBox);
            this.FilterFlowPanel.Controls.Add(this.PartiesFilterBox);
            this.FilterFlowPanel.Controls.Add(this.TypesFilterBox);
            this.FilterFlowPanel.Controls.Add(this.GroupsFilterBox);
            this.FilterFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilterFlowPanel.Location = new System.Drawing.Point(3, 19);
            this.FilterFlowPanel.Name = "FilterFlowPanel";
            this.FilterFlowPanel.Size = new System.Drawing.Size(239, 372);
            this.FilterFlowPanel.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Account Number";
            // 
            // AccountNumberBox
            // 
            this.AccountNumberBox.AllowedSpecialCharacters = "";
            this.AccountNumberBox.Location = new System.Drawing.Point(3, 20);
            this.AccountNumberBox.MaxLength = 10;
            this.AccountNumberBox.Name = "AccountNumberBox";
            this.AccountNumberBox.Size = new System.Drawing.Size(233, 23);
            this.AccountNumberBox.Ssn = null;
            this.AccountNumberBox.TabIndex = 5;
            this.AccountNumberBox.TextChanged += new System.EventHandler(this.AccountNumberBox_TextChanged);
            // 
            // OpenClosedFilterBox
            // 
            this.OpenClosedFilterBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OpenClosedFilterBox.FormattingEnabled = true;
            this.OpenClosedFilterBox.Items.AddRange(new object[] {
            "Open and Closed Complaints",
            "Open Complaints",
            "Closed Complaints"});
            this.OpenClosedFilterBox.Location = new System.Drawing.Point(3, 49);
            this.OpenClosedFilterBox.Name = "OpenClosedFilterBox";
            this.OpenClosedFilterBox.Size = new System.Drawing.Size(233, 24);
            this.OpenClosedFilterBox.TabIndex = 10;
            this.OpenClosedFilterBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // FlagsFilterBox
            // 
            this.FlagsFilterBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FlagsFilterBox.FormattingEnabled = true;
            this.FlagsFilterBox.Items.AddRange(new object[] {
            "All Complaint Flags"});
            this.FlagsFilterBox.Location = new System.Drawing.Point(3, 79);
            this.FlagsFilterBox.Name = "FlagsFilterBox";
            this.FlagsFilterBox.Size = new System.Drawing.Size(233, 24);
            this.FlagsFilterBox.TabIndex = 11;
            this.FlagsFilterBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // PartiesFilterBox
            // 
            this.PartiesFilterBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PartiesFilterBox.FormattingEnabled = true;
            this.PartiesFilterBox.Items.AddRange(new object[] {
            "All Complaint Parties"});
            this.PartiesFilterBox.Location = new System.Drawing.Point(3, 109);
            this.PartiesFilterBox.Name = "PartiesFilterBox";
            this.PartiesFilterBox.Size = new System.Drawing.Size(233, 24);
            this.PartiesFilterBox.TabIndex = 12;
            this.PartiesFilterBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // TypesFilterBox
            // 
            this.TypesFilterBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypesFilterBox.FormattingEnabled = true;
            this.TypesFilterBox.Items.AddRange(new object[] {
            "All Complaint Types"});
            this.TypesFilterBox.Location = new System.Drawing.Point(3, 139);
            this.TypesFilterBox.Name = "TypesFilterBox";
            this.TypesFilterBox.Size = new System.Drawing.Size(233, 24);
            this.TypesFilterBox.TabIndex = 13;
            this.TypesFilterBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // GroupsFilterBox
            // 
            this.GroupsFilterBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GroupsFilterBox.FormattingEnabled = true;
            this.GroupsFilterBox.Items.AddRange(new object[] {
            "All Complaint Groups"});
            this.GroupsFilterBox.Location = new System.Drawing.Point(3, 169);
            this.GroupsFilterBox.Name = "GroupsFilterBox";
            this.GroupsFilterBox.Size = new System.Drawing.Size(233, 24);
            this.GroupsFilterBox.TabIndex = 14;
            this.GroupsFilterBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            // 
            // NewComplaintButton
            // 
            this.NewComplaintButton.Location = new System.Drawing.Point(15, 27);
            this.NewComplaintButton.Name = "NewComplaintButton";
            this.NewComplaintButton.Size = new System.Drawing.Size(242, 31);
            this.NewComplaintButton.TabIndex = 1;
            this.NewComplaintButton.Text = "File New Complaint";
            this.NewComplaintButton.UseVisualStyleBackColor = true;
            this.NewComplaintButton.Click += new System.EventHandler(this.NewComplaintButton_Click);
            // 
            // ResultsGrid
            // 
            this.ResultsGrid.AllowUserToAddRows = false;
            this.ResultsGrid.AllowUserToDeleteRows = false;
            this.ResultsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AccountNumberHeader,
            this.BorrowerNameHeader,
            this.TypeHeader,
            this.PartyHeader,
            this.AddedByHeader,
            this.NeedHelpTicketNumberHeader,
            this.GroupNameHeader,
            this.DescriptionHeader});
            this.ResultsGrid.Location = new System.Drawing.Point(263, 27);
            this.ResultsGrid.Name = "ResultsGrid";
            this.ResultsGrid.ReadOnly = true;
            this.ResultsGrid.RowHeadersVisible = false;
            this.ResultsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ResultsGrid.Size = new System.Drawing.Size(1308, 428);
            this.ResultsGrid.TabIndex = 2;
            this.ResultsGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ResultsGrid_CellDoubleClick);
            // 
            // AccountNumberHeader
            // 
            this.AccountNumberHeader.DataPropertyName = "AccountNumber";
            this.AccountNumberHeader.HeaderText = "Account #";
            this.AccountNumberHeader.Name = "AccountNumberHeader";
            this.AccountNumberHeader.ReadOnly = true;
            // 
            // BorrowerNameHeader
            // 
            this.BorrowerNameHeader.DataPropertyName = "BorrowerName";
            this.BorrowerNameHeader.HeaderText = "Name";
            this.BorrowerNameHeader.Name = "BorrowerNameHeader";
            this.BorrowerNameHeader.ReadOnly = true;
            // 
            // TypeHeader
            // 
            this.TypeHeader.DataPropertyName = "TypeName";
            this.TypeHeader.HeaderText = "Type";
            this.TypeHeader.Name = "TypeHeader";
            this.TypeHeader.ReadOnly = true;
            // 
            // PartyHeader
            // 
            this.PartyHeader.DataPropertyName = "PartyName";
            this.PartyHeader.HeaderText = "Party";
            this.PartyHeader.Name = "PartyHeader";
            this.PartyHeader.ReadOnly = true;
            // 
            // AddedByHeader
            // 
            this.AddedByHeader.DataPropertyName = "ComplaintDate";
            this.AddedByHeader.HeaderText = "Date Opened";
            this.AddedByHeader.Name = "AddedByHeader";
            this.AddedByHeader.ReadOnly = true;
            this.AddedByHeader.Width = 120;
            // 
            // NeedHelpTicketNumberHeader
            // 
            this.NeedHelpTicketNumberHeader.DataPropertyName = "NeedHelpTicketNumber";
            this.NeedHelpTicketNumberHeader.HeaderText = "Need Help #";
            this.NeedHelpTicketNumberHeader.Name = "NeedHelpTicketNumberHeader";
            this.NeedHelpTicketNumberHeader.ReadOnly = true;
            this.NeedHelpTicketNumberHeader.Width = 120;
            // 
            // GroupNameHeader
            // 
            this.GroupNameHeader.DataPropertyName = "GroupName";
            this.GroupNameHeader.HeaderText = "Group Name";
            this.GroupNameHeader.Name = "GroupNameHeader";
            this.GroupNameHeader.ReadOnly = true;
            this.GroupNameHeader.Width = 200;
            // 
            // DescriptionHeader
            // 
            this.DescriptionHeader.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DescriptionHeader.DataPropertyName = "ComplaintDescription";
            this.DescriptionHeader.HeaderText = "Description";
            this.DescriptionHeader.Name = "DescriptionHeader";
            this.DescriptionHeader.ReadOnly = true;
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FlagsMenu,
            this.PartiesMenu,
            this.TypesMenu,
            this.GroupsMenu});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1583, 24);
            this.MainMenu.TabIndex = 3;
            this.MainMenu.Text = "menuStrip1";
            // 
            // FlagsMenu
            // 
            this.FlagsMenu.Name = "FlagsMenu";
            this.FlagsMenu.Size = new System.Drawing.Size(92, 20);
            this.FlagsMenu.Text = "Manage &Flags";
            this.FlagsMenu.Click += new System.EventHandler(this.FlagsMenu_Click);
            // 
            // PartiesMenu
            // 
            this.PartiesMenu.Name = "PartiesMenu";
            this.PartiesMenu.Size = new System.Drawing.Size(100, 20);
            this.PartiesMenu.Text = "Manage &Parties";
            this.PartiesMenu.Click += new System.EventHandler(this.PartiesMenu_Click);
            // 
            // TypesMenu
            // 
            this.TypesMenu.Name = "TypesMenu";
            this.TypesMenu.Size = new System.Drawing.Size(95, 20);
            this.TypesMenu.Text = "Manage &Types";
            this.TypesMenu.Click += new System.EventHandler(this.TypesMenu_Click);
            // 
            // GroupsMenu
            // 
            this.GroupsMenu.Name = "GroupsMenu";
            this.GroupsMenu.Size = new System.Drawing.Size(103, 20);
            this.GroupsMenu.Text = "Manage &Groups";
            this.GroupsMenu.Click += new System.EventHandler(this.GroupsMenu_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1583, 465);
            this.Controls.Add(this.NewComplaintButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ResultsGrid);
            this.Controls.Add(this.MainMenu);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(796, 357);
            this.Name = "MainForm";
            this.Text = "{0} Complaints";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.FilterFlowPanel.ResumeLayout(false);
            this.FilterFlowPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsGrid)).EndInit();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button NewComplaintButton;
        private System.Windows.Forms.DataGridView ResultsGrid;
        private System.Windows.Forms.FlowLayoutPanel FilterFlowPanel;
        private System.Windows.Forms.Label label1;
        private Uheaa.Common.WinForms.AccountNumberTextBox AccountNumberBox;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem FlagsMenu;
        private System.Windows.Forms.ToolStripMenuItem PartiesMenu;
        private System.Windows.Forms.ToolStripMenuItem TypesMenu;
        private System.Windows.Forms.ToolStripMenuItem GroupsMenu;
        private System.Windows.Forms.ComboBox OpenClosedFilterBox;
        private System.Windows.Forms.ComboBox FlagsFilterBox;
        private System.Windows.Forms.ComboBox PartiesFilterBox;
        private System.Windows.Forms.ComboBox TypesFilterBox;
        private System.Windows.Forms.ComboBox GroupsFilterBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountNumberHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn BorrowerNameHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartyHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddedByHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn NeedHelpTicketNumberHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupNameHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionHeader;
    }
}

