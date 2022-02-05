namespace OLDEMOS
{
    partial class LandingForm
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
            this.BorrowerResults = new Uheaa.Common.WinForms.BorrowerResultsControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PreviousBorrowersGrid = new System.Windows.Forms.DataGridView();
            this.PreviousNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PreviousSSNColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BorrowerSearch = new Uheaa.Common.WinForms.BorrowerSearchControl();
            this.AccIdBox = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.SsnAcctLabel = new System.Windows.Forms.Label();
            this.GoButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.commonMenu1 = new OLDEMOS.CommonMenu();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PreviousBorrowersGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // BorrowerResults
            // 
            this.BorrowerResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BorrowerResults.ChooseBorrowerOnSelection = false;
            this.BorrowerResults.EmailsColumnVisible = true;
            this.BorrowerResults.Location = new System.Drawing.Point(16, 371);
            this.BorrowerResults.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BorrowerResults.Name = "BorrowerResults";
            this.BorrowerResults.PhoneColumnsVisible = true;
            this.BorrowerResults.Size = new System.Drawing.Size(746, 273);
            this.BorrowerResults.TabIndex = 5;
            this.BorrowerResults.OnSelectionChanged += new Uheaa.Common.WinForms.BorrowerResultsControl.SelectedBorrowerDelegate(this.BorrowerResults_OnSelectionChanged);
            this.BorrowerResults.OnBorrowerChosen += new Uheaa.Common.WinForms.BorrowerResultsControl.SelectedBorrowerDelegate(this.BorrowerResults_OnBorrowerChosen);
            this.BorrowerResults.Enter += new System.EventHandler(this.BorrowerResults_Enter);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.PreviousBorrowersGrid);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F);
            this.groupBox1.Location = new System.Drawing.Point(451, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(312, 278);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Previous Borrowers";
            // 
            // PreviousBorrowersGrid
            // 
            this.PreviousBorrowersGrid.AllowUserToAddRows = false;
            this.PreviousBorrowersGrid.AllowUserToDeleteRows = false;
            this.PreviousBorrowersGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PreviousBorrowersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PreviousBorrowersGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PreviousNameColumn,
            this.PreviousSSNColumn});
            this.PreviousBorrowersGrid.Location = new System.Drawing.Point(6, 34);
            this.PreviousBorrowersGrid.MultiSelect = false;
            this.PreviousBorrowersGrid.Name = "PreviousBorrowersGrid";
            this.PreviousBorrowersGrid.ReadOnly = true;
            this.PreviousBorrowersGrid.RowHeadersVisible = false;
            this.PreviousBorrowersGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PreviousBorrowersGrid.Size = new System.Drawing.Size(296, 238);
            this.PreviousBorrowersGrid.TabIndex = 0;
            this.PreviousBorrowersGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PreviousBorrowersGrid_CellClick);
            this.PreviousBorrowersGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PreviousBorrowersGrid_CellContentDoubleClick);
            this.PreviousBorrowersGrid.SelectionChanged += new System.EventHandler(this.PreviousBorrowersGrid_SelectionChanged);
            this.PreviousBorrowersGrid.Enter += new System.EventHandler(this.PreviousBorrowersGrid_Enter);
            this.PreviousBorrowersGrid.Leave += new System.EventHandler(this.PreviousBorrowersGrid_Leave);
            // 
            // PreviousNameColumn
            // 
            this.PreviousNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PreviousNameColumn.DataPropertyName = "FullName";
            this.PreviousNameColumn.FillWeight = 85F;
            this.PreviousNameColumn.HeaderText = "Name";
            this.PreviousNameColumn.Name = "PreviousNameColumn";
            this.PreviousNameColumn.ReadOnly = true;
            // 
            // PreviousSSNColumn
            // 
            this.PreviousSSNColumn.DataPropertyName = "SSN";
            this.PreviousSSNColumn.HeaderText = "SSN";
            this.PreviousSSNColumn.Name = "PreviousSSNColumn";
            this.PreviousSSNColumn.ReadOnly = true;
            this.PreviousSSNColumn.Width = 120;
            // 
            // BorrowerSearch
            // 
            this.BorrowerSearch.LDA = null;
            this.BorrowerSearch.Location = new System.Drawing.Point(16, 85);
            this.BorrowerSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BorrowerSearch.Name = "BorrowerSearch";
            this.BorrowerSearch.OnelinkEnabled = true;
            this.BorrowerSearch.Size = new System.Drawing.Size(429, 278);
            this.BorrowerSearch.TabIndex = 3;
            this.BorrowerSearch.UheaaEnabled = true;
            this.BorrowerSearch.OnSearchResultsRetrieved += new Uheaa.Common.WinForms.BorrowerSearchControl.SearchResultsRetrieved(this.BorrowerSearch_OnSearchResultsRetrieved);
            this.BorrowerSearch.SearchCleared += new System.EventHandler(this.BorrowerSearch_SearchCleared);
            // 
            // AccIdBox
            // 
            this.AccIdBox.AccountNumber = null;
            this.AccIdBox.AllowedSpecialCharacters = "";
            this.AccIdBox.Font = new System.Drawing.Font("Arial", 14F);
            this.AccIdBox.Location = new System.Drawing.Point(148, 31);
            this.AccIdBox.MaxLength = 10;
            this.AccIdBox.Name = "AccIdBox";
            this.AccIdBox.Size = new System.Drawing.Size(202, 29);
            this.AccIdBox.Ssn = null;
            this.AccIdBox.TabIndex = 1;
            this.AccIdBox.TextChanged += new System.EventHandler(this.AccIdBox_TextChanged);
            // 
            // SsnAcctLabel
            // 
            this.SsnAcctLabel.Font = new System.Drawing.Font("Arial", 14F);
            this.SsnAcctLabel.Location = new System.Drawing.Point(13, 34);
            this.SsnAcctLabel.Name = "SsnAcctLabel";
            this.SsnAcctLabel.Size = new System.Drawing.Size(129, 22);
            this.SsnAcctLabel.TabIndex = 0;
            this.SsnAcctLabel.Text = "SSN or Acct #";
            this.SsnAcctLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GoButton
            // 
            this.GoButton.Enabled = false;
            this.GoButton.Font = new System.Drawing.Font("Arial", 14F);
            this.GoButton.Location = new System.Drawing.Point(356, 31);
            this.GoButton.Name = "GoButton";
            this.GoButton.Size = new System.Drawing.Size(75, 29);
            this.GoButton.TabIndex = 6;
            this.GoButton.Text = "Go";
            this.GoButton.UseVisualStyleBackColor = true;
            this.GoButton.Click += new System.EventHandler(this.GoButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(0, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(771, 3);
            this.label2.TabIndex = 7;
            // 
            // commonMenu1
            // 
            this.commonMenu1.Location = new System.Drawing.Point(0, 0);
            this.commonMenu1.Name = "commonMenu1";
            this.commonMenu1.Size = new System.Drawing.Size(771, 24);
            this.commonMenu1.TabIndex = 8;
            this.commonMenu1.Text = "commonMenu1";
            // 
            // LandingForm
            // 
            this.AcceptButton = this.GoButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 657);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.GoButton);
            this.Controls.Add(this.BorrowerResults);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BorrowerSearch);
            this.Controls.Add(this.AccIdBox);
            this.Controls.Add(this.SsnAcctLabel);
            this.Controls.Add(this.commonMenu1);
            this.MainMenuStrip = this.commonMenu1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(787, 696);
            this.Name = "LandingForm";
            this.Text = "Account Lookup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LandingForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PreviousBorrowersGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SsnAcctLabel;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox AccIdBox;
        private Uheaa.Common.WinForms.BorrowerSearchControl BorrowerSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView PreviousBorrowersGrid;
        private Uheaa.Common.WinForms.BorrowerResultsControl BorrowerResults;
        private System.Windows.Forms.Button GoButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn PreviousNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PreviousSSNColumn;
        private CommonMenu commonMenu1;
    }
}