namespace Uheaa.Common.WinForms
{
    partial class BorrowerResultsControl
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
            this.SearchResultsBox = new System.Windows.Forms.GroupBox();
            this.SearchResultsGrid = new System.Windows.Forms.DataGridView();
            this.SSNColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RegionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DOBColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddressColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZipColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HomePhoneColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkPhoneColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AltPhoneColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmailsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SearchResultsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SearchResultsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // SearchResultsBox
            // 
            this.SearchResultsBox.Controls.Add(this.SearchResultsGrid);
            this.SearchResultsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchResultsBox.Font = new System.Drawing.Font("Arial", 12F);
            this.SearchResultsBox.Location = new System.Drawing.Point(0, 0);
            this.SearchResultsBox.Name = "SearchResultsBox";
            this.SearchResultsBox.Size = new System.Drawing.Size(743, 176);
            this.SearchResultsBox.TabIndex = 6;
            this.SearchResultsBox.TabStop = false;
            this.SearchResultsBox.Text = "Search Results";
            // 
            // SearchResultsGrid
            // 
            this.SearchResultsGrid.AllowUserToAddRows = false;
            this.SearchResultsGrid.AllowUserToDeleteRows = false;
            this.SearchResultsGrid.AllowUserToOrderColumns = true;
            this.SearchResultsGrid.AllowUserToResizeRows = false;
            this.SearchResultsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchResultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SearchResultsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SSNColumn,
            this.RegionColumn,
            this.NameColumn,
            this.DOBColumn,
            this.AddressColumn,
            this.CityColumn,
            this.StateColumn,
            this.ZipColumn,
            this.HomePhoneColumn,
            this.WorkPhoneColumn,
            this.AltPhoneColumn,
            this.EmailsColumn});
            this.SearchResultsGrid.Location = new System.Drawing.Point(10, 25);
            this.SearchResultsGrid.MultiSelect = false;
            this.SearchResultsGrid.Name = "SearchResultsGrid";
            this.SearchResultsGrid.RowHeadersVisible = false;
            this.SearchResultsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.SearchResultsGrid.Size = new System.Drawing.Size(724, 137);
            this.SearchResultsGrid.TabIndex = 0;
            this.SearchResultsGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SearchResultsGrid_CellContentDoubleClick);
            this.SearchResultsGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.SearchResultsGrid_CellFormatting);
            this.SearchResultsGrid.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.SearchResultsGrid_ColumnDisplayIndexChanged);
            this.SearchResultsGrid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SearchResultsGrid_ColumnHeaderMouseClick);
            this.SearchResultsGrid.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.SearchResultsGrid_ColumnWidthChanged);
            this.SearchResultsGrid.SelectionChanged += new System.EventHandler(this.SearchResultsGrid_SelectionChanged);
            this.SearchResultsGrid.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.SearchResultsGrid_PreviewKeyDown);
            // 
            // SSNColumn
            // 
            this.SSNColumn.DataPropertyName = "SSN";
            this.SSNColumn.HeaderText = "SSN";
            this.SSNColumn.Name = "SSNColumn";
            this.SSNColumn.ReadOnly = true;
            this.SSNColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // RegionColumn
            // 
            this.RegionColumn.DataPropertyName = "Region";
            this.RegionColumn.HeaderText = "Region";
            this.RegionColumn.Name = "RegionColumn";
            this.RegionColumn.ReadOnly = true;
            this.RegionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // NameColumn
            // 
            this.NameColumn.DataPropertyName = "FullName";
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            this.NameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // DOBColumn
            // 
            this.DOBColumn.DataPropertyName = "DOB";
            this.DOBColumn.HeaderText = "DOB";
            this.DOBColumn.Name = "DOBColumn";
            this.DOBColumn.ReadOnly = true;
            this.DOBColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // AddressColumn
            // 
            this.AddressColumn.DataPropertyName = "FullAddress";
            this.AddressColumn.HeaderText = "Address";
            this.AddressColumn.Name = "AddressColumn";
            this.AddressColumn.ReadOnly = true;
            this.AddressColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // CityColumn
            // 
            this.CityColumn.DataPropertyName = "City";
            this.CityColumn.HeaderText = "City";
            this.CityColumn.Name = "CityColumn";
            this.CityColumn.ReadOnly = true;
            this.CityColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // StateColumn
            // 
            this.StateColumn.DataPropertyName = "StateCode";
            this.StateColumn.HeaderText = "State";
            this.StateColumn.Name = "StateColumn";
            this.StateColumn.ReadOnly = true;
            this.StateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // ZipColumn
            // 
            this.ZipColumn.DataPropertyName = "Zip";
            this.ZipColumn.HeaderText = "Zip";
            this.ZipColumn.Name = "ZipColumn";
            this.ZipColumn.ReadOnly = true;
            this.ZipColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // HomePhoneColumn
            // 
            this.HomePhoneColumn.DataPropertyName = "HomePhone";
            this.HomePhoneColumn.HeaderText = "Home";
            this.HomePhoneColumn.Name = "HomePhoneColumn";
            this.HomePhoneColumn.ReadOnly = true;
            this.HomePhoneColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // WorkPhoneColumn
            // 
            this.WorkPhoneColumn.DataPropertyName = "WorkPhone";
            this.WorkPhoneColumn.HeaderText = "Work";
            this.WorkPhoneColumn.Name = "WorkPhoneColumn";
            this.WorkPhoneColumn.ReadOnly = true;
            this.WorkPhoneColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // AltPhoneColumn
            // 
            this.AltPhoneColumn.DataPropertyName = "AlternatePhone";
            this.AltPhoneColumn.HeaderText = "Alt";
            this.AltPhoneColumn.Name = "AltPhoneColumn";
            this.AltPhoneColumn.ReadOnly = true;
            this.AltPhoneColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // EmailsColumn
            // 
            this.EmailsColumn.DataPropertyName = "Emails";
            this.EmailsColumn.HeaderText = "Emails";
            this.EmailsColumn.Name = "EmailsColumn";
            this.EmailsColumn.ReadOnly = true;
            this.EmailsColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // BorrowerResultsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SearchResultsBox);
            this.Name = "BorrowerResultsControl";
            this.Size = new System.Drawing.Size(743, 176);
            this.SearchResultsBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SearchResultsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SearchResultsBox;
        private System.Windows.Forms.DataGridView SearchResultsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn SSNColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RegionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOBColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddressColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CityColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZipColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn HomePhoneColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkPhoneColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AltPhoneColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmailsColumn;

    }
}
