namespace ErrorFinder
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
            this.ResultsGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.RecordsFoundLabel = new System.Windows.Forms.Label();
            this.ErrorSelector = new System.Windows.Forms.ComboBox();
            this.ExportMenu = new System.Windows.Forms.MenuStrip();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportCurrentMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportDataMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportAllMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeExportMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenExcelButton = new System.Windows.Forms.Button();
            this.BatchSelector = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsGrid)).BeginInit();
            this.ExportMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ResultsGrid
            // 
            this.ResultsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultsGrid.Location = new System.Drawing.Point(12, 70);
            this.ResultsGrid.Name = "ResultsGrid";
            this.ResultsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ResultsGrid.Size = new System.Drawing.Size(466, 258);
            this.ResultsGrid.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Error Code";
            // 
            // RecordsFoundLabel
            // 
            this.RecordsFoundLabel.AutoSize = true;
            this.RecordsFoundLabel.Location = new System.Drawing.Point(160, 46);
            this.RecordsFoundLabel.Name = "RecordsFoundLabel";
            this.RecordsFoundLabel.Size = new System.Drawing.Size(95, 13);
            this.RecordsFoundLabel.TabIndex = 4;
            this.RecordsFoundLabel.Text = "100 Total Records";
            // 
            // ErrorSelector
            // 
            this.ErrorSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ErrorSelector.FormattingEnabled = true;
            this.ErrorSelector.Location = new System.Drawing.Point(12, 43);
            this.ErrorSelector.Name = "ErrorSelector";
            this.ErrorSelector.Size = new System.Drawing.Size(68, 21);
            this.ErrorSelector.TabIndex = 5;
            this.ErrorSelector.SelectedIndexChanged += new System.EventHandler(this.ErrorSelector_SelectedIndexChanged);
            // 
            // ExportMenu
            // 
            this.ExportMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem});
            this.ExportMenu.Location = new System.Drawing.Point(0, 0);
            this.ExportMenu.Name = "ExportMenu";
            this.ExportMenu.Size = new System.Drawing.Size(490, 24);
            this.ExportMenu.TabIndex = 9;
            this.ExportMenu.Text = "menuStrip1";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportCurrentMenu,
            this.ExportDataMenu,
            this.ExportAllMenu,
            this.ChangeExportMenu,
            this.RefreshMenu});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.exportToolStripMenuItem.Text = "&Export";
            // 
            // ExportCurrentMenu
            // 
            this.ExportCurrentMenu.Enabled = false;
            this.ExportCurrentMenu.Name = "ExportCurrentMenu";
            this.ExportCurrentMenu.Size = new System.Drawing.Size(254, 22);
            this.ExportCurrentMenu.Text = "Export &Current View";
            this.ExportCurrentMenu.Click += new System.EventHandler(this.ExportCurrentMenu_Click);
            // 
            // ExportDataMenu
            // 
            this.ExportDataMenu.Name = "ExportDataMenu";
            this.ExportDataMenu.Size = new System.Drawing.Size(254, 22);
            this.ExportDataMenu.Text = "Export All &Data As Single CSV";
            this.ExportDataMenu.Click += new System.EventHandler(this.ExportDataMenu_Click);
            // 
            // ExportAllMenu
            // 
            this.ExportAllMenu.Name = "ExportAllMenu";
            this.ExportAllMenu.Size = new System.Drawing.Size(254, 22);
            this.ExportAllMenu.Text = "Export &All Views";
            this.ExportAllMenu.Click += new System.EventHandler(this.ExportAllMenu_Click);
            // 
            // ChangeExportMenu
            // 
            this.ChangeExportMenu.Name = "ChangeExportMenu";
            this.ChangeExportMenu.Size = new System.Drawing.Size(254, 22);
            this.ChangeExportMenu.Text = "Change Export &Location";
            this.ChangeExportMenu.Click += new System.EventHandler(this.ChangeExportMenu_Click);
            // 
            // RefreshMenu
            // 
            this.RefreshMenu.Name = "RefreshMenu";
            this.RefreshMenu.Size = new System.Drawing.Size(254, 22);
            this.RefreshMenu.Text = "&Refresh OPSDEV with Loaded Data";
            this.RefreshMenu.Click += new System.EventHandler(this.RefreshMenu_Click);
            // 
            // OpenExcelButton
            // 
            this.OpenExcelButton.Location = new System.Drawing.Point(341, 41);
            this.OpenExcelButton.Name = "OpenExcelButton";
            this.OpenExcelButton.Size = new System.Drawing.Size(137, 23);
            this.OpenExcelButton.TabIndex = 10;
            this.OpenExcelButton.Text = "Open this view in Excel";
            this.OpenExcelButton.UseVisualStyleBackColor = true;
            this.OpenExcelButton.Click += new System.EventHandler(this.OpenExcelButton_Click);
            // 
            // BatchSelector
            // 
            this.BatchSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BatchSelector.FormattingEnabled = true;
            this.BatchSelector.Location = new System.Drawing.Point(86, 43);
            this.BatchSelector.Name = "BatchSelector";
            this.BatchSelector.Size = new System.Drawing.Size(68, 21);
            this.BatchSelector.TabIndex = 12;
            this.BatchSelector.SelectedIndexChanged += new System.EventHandler(this.BatchSelector_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(83, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Minor Batch";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 340);
            this.Controls.Add(this.BatchSelector);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OpenExcelButton);
            this.Controls.Add(this.ErrorSelector);
            this.Controls.Add(this.RecordsFoundLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ResultsGrid);
            this.Controls.Add(this.ExportMenu);
            this.MainMenuStrip = this.ExportMenu;
            this.MinimumSize = new System.Drawing.Size(506, 336);
            this.Name = "MainForm";
            this.Text = "Error Finder";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.ResultsGrid)).EndInit();
            this.ExportMenu.ResumeLayout(false);
            this.ExportMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ResultsGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label RecordsFoundLabel;
        private System.Windows.Forms.ComboBox ErrorSelector;
        private System.Windows.Forms.MenuStrip ExportMenu;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportCurrentMenu;
        private System.Windows.Forms.ToolStripMenuItem ExportAllMenu;
        private System.Windows.Forms.ToolStripMenuItem ChangeExportMenu;
        private System.Windows.Forms.ToolStripMenuItem RefreshMenu;
        private System.Windows.Forms.ToolStripMenuItem ExportDataMenu;
        private System.Windows.Forms.Button OpenExcelButton;
        private System.Windows.Forms.ComboBox BatchSelector;
        private System.Windows.Forms.Label label2;

    }
}

