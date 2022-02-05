namespace XmlGeneratorECorr
{
    partial class EcorrUpdater
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
            this.Attributes = new System.Windows.Forms.TabPage();
            this.LetterTypesTab = new System.Windows.Forms.TabPage();
            this.LettersTab = new System.Windows.Forms.TabPage();
            this.DocDetails = new System.Windows.Forms.TabPage();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.AddB = new System.Windows.Forms.Button();
            this.DocDetailsDataGrid = new Uheaa.Common.WinForms.CopyableDataGridView();
            this.LettersDataGrid = new Uheaa.Common.WinForms.CopyableDataGridView();
            this.LetterTypesDataGrid = new Uheaa.Common.WinForms.CopyableDataGridView();
            this.AttributesDataGrid = new Uheaa.Common.WinForms.CopyableDataGridView();
            this.Attributes.SuspendLayout();
            this.LetterTypesTab.SuspendLayout();
            this.LettersTab.SuspendLayout();
            this.DocDetails.SuspendLayout();
            this.Tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DocDetailsDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LettersDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LetterTypesDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttributesDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // Attributes
            // 
            this.Attributes.Controls.Add(this.AttributesDataGrid);
            this.Attributes.Location = new System.Drawing.Point(4, 27);
            this.Attributes.Name = "Attributes";
            this.Attributes.Padding = new System.Windows.Forms.Padding(3);
            this.Attributes.Size = new System.Drawing.Size(884, 340);
            this.Attributes.TabIndex = 4;
            this.Attributes.Text = "Attributes";
            this.Attributes.UseVisualStyleBackColor = true;
            // 
            // LetterTypesTab
            // 
            this.LetterTypesTab.Controls.Add(this.LetterTypesDataGrid);
            this.LetterTypesTab.Location = new System.Drawing.Point(4, 27);
            this.LetterTypesTab.Name = "LetterTypesTab";
            this.LetterTypesTab.Padding = new System.Windows.Forms.Padding(3);
            this.LetterTypesTab.Size = new System.Drawing.Size(884, 340);
            this.LetterTypesTab.TabIndex = 2;
            this.LetterTypesTab.Text = "Letter Types";
            this.LetterTypesTab.UseVisualStyleBackColor = true;
            // 
            // LettersTab
            // 
            this.LettersTab.Controls.Add(this.LettersDataGrid);
            this.LettersTab.Location = new System.Drawing.Point(4, 27);
            this.LettersTab.Name = "LettersTab";
            this.LettersTab.Padding = new System.Windows.Forms.Padding(3);
            this.LettersTab.Size = new System.Drawing.Size(884, 340);
            this.LettersTab.TabIndex = 1;
            this.LettersTab.Text = "Letters";
            this.LettersTab.UseVisualStyleBackColor = true;
            // 
            // DocDetails
            // 
            this.DocDetails.Controls.Add(this.DocDetailsDataGrid);
            this.DocDetails.Location = new System.Drawing.Point(4, 27);
            this.DocDetails.Name = "DocDetails";
            this.DocDetails.Padding = new System.Windows.Forms.Padding(3);
            this.DocDetails.Size = new System.Drawing.Size(884, 340);
            this.DocDetails.TabIndex = 0;
            this.DocDetails.Text = "Document Details";
            this.DocDetails.UseVisualStyleBackColor = true;
            // 
            // Tabs
            // 
            this.Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs.Controls.Add(this.DocDetails);
            this.Tabs.Controls.Add(this.LettersTab);
            this.Tabs.Controls.Add(this.LetterTypesTab);
            this.Tabs.Controls.Add(this.Attributes);
            this.Tabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Tabs.Location = new System.Drawing.Point(13, 37);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(892, 371);
            this.Tabs.TabIndex = 0;
            this.Tabs.Tag = "";
            // 
            // AddB
            // 
            this.AddB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddB.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddB.Location = new System.Drawing.Point(805, 8);
            this.AddB.Name = "AddB";
            this.AddB.Size = new System.Drawing.Size(100, 35);
            this.AddB.TabIndex = 1;
            this.AddB.Text = "Add";
            this.AddB.UseVisualStyleBackColor = true;
            this.AddB.Click += new System.EventHandler(this.AddB_Click);
            // 
            // DocDetailsDataGrid
            // 
            this.DocDetailsDataGrid.AllowUserToAddRows = false;
            this.DocDetailsDataGrid.AllowUserToDeleteRows = false;
            this.DocDetailsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DocDetailsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DocDetailsDataGrid.Location = new System.Drawing.Point(3, 3);
            this.DocDetailsDataGrid.Name = "DocDetailsDataGrid";
            this.DocDetailsDataGrid.ReadOnly = true;
            this.DocDetailsDataGrid.RowHeadersVisible = false;
            this.DocDetailsDataGrid.Size = new System.Drawing.Size(878, 334);
            this.DocDetailsDataGrid.TabIndex = 0;
            this.DocDetailsDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DocDetailsDataGrid_CellDoubleClick);
            // 
            // LettersDataGrid
            // 
            this.LettersDataGrid.AllowUserToAddRows = false;
            this.LettersDataGrid.AllowUserToDeleteRows = false;
            this.LettersDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LettersDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LettersDataGrid.Location = new System.Drawing.Point(6, 7);
            this.LettersDataGrid.Name = "LettersDataGrid";
            this.LettersDataGrid.ReadOnly = true;
            this.LettersDataGrid.RowHeadersVisible = false;
            this.LettersDataGrid.Size = new System.Drawing.Size(878, 327);
            this.LettersDataGrid.TabIndex = 1;
            this.LettersDataGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.LettersDataGrid_CellContentDoubleClick);
            // 
            // LetterTypesDataGrid
            // 
            this.LetterTypesDataGrid.AllowUserToAddRows = false;
            this.LetterTypesDataGrid.AllowUserToDeleteRows = false;
            this.LetterTypesDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LetterTypesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LetterTypesDataGrid.Location = new System.Drawing.Point(6, 7);
            this.LetterTypesDataGrid.Name = "LetterTypesDataGrid";
            this.LetterTypesDataGrid.ReadOnly = true;
            this.LetterTypesDataGrid.RowHeadersVisible = false;
            this.LetterTypesDataGrid.Size = new System.Drawing.Size(875, 327);
            this.LetterTypesDataGrid.TabIndex = 1;
            this.LetterTypesDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.LetterTypesDataGrid_CellDoubleClick);
            // 
            // AttributesDataGrid
            // 
            this.AttributesDataGrid.AllowUserToAddRows = false;
            this.AttributesDataGrid.AllowUserToDeleteRows = false;
            this.AttributesDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AttributesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AttributesDataGrid.Location = new System.Drawing.Point(6, 7);
            this.AttributesDataGrid.Name = "AttributesDataGrid";
            this.AttributesDataGrid.ReadOnly = true;
            this.AttributesDataGrid.RowHeadersVisible = false;
            this.AttributesDataGrid.Size = new System.Drawing.Size(875, 327);
            this.AttributesDataGrid.TabIndex = 1;
            this.AttributesDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AttributesDataGrid_CellDoubleClick);
            // 
            // EcorrUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 415);
            this.Controls.Add(this.AddB);
            this.Controls.Add(this.Tabs);
            this.MinimumSize = new System.Drawing.Size(933, 453);
            this.Name = "EcorrUpdater";
            this.ShowIcon = false;
            this.Text = "ECorr Update";
            this.Attributes.ResumeLayout(false);
            this.LetterTypesTab.ResumeLayout(false);
            this.LettersTab.ResumeLayout(false);
            this.DocDetails.ResumeLayout(false);
            this.Tabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DocDetailsDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LettersDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LetterTypesDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttributesDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage Attributes;
        private Uheaa.Common.WinForms.CopyableDataGridView AttributesDataGrid;
        private System.Windows.Forms.TabPage LetterTypesTab;
        private Uheaa.Common.WinForms.CopyableDataGridView LetterTypesDataGrid;
        private System.Windows.Forms.TabPage LettersTab;
        private Uheaa.Common.WinForms.CopyableDataGridView LettersDataGrid;
        private System.Windows.Forms.TabPage DocDetails;
        private Uheaa.Common.WinForms.CopyableDataGridView DocDetailsDataGrid;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.Button AddB;

    }
}

