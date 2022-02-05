
namespace DOCIDUHEAA
{
    partial class Selection
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Selection));
            this.label1 = new System.Windows.Forms.Label();
            this.ProcessButton = new System.Windows.Forms.Button();
            this.BorrowerName = new System.Windows.Forms.Label();
            this.LookupButton = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.CorrFax = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SelectedArc = new System.Windows.Forms.Label();
            this.SelectedId = new System.Windows.Forms.Label();
            this.SelectedType = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.DocumentType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.DocumentIdCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DailyProcessed = new System.Windows.Forms.DataGridView();
            this.Refresh = new System.Windows.Forms.Button();
            this.DateSelection = new System.Windows.Forms.DateTimePicker();
            this.ProcessedCount = new System.Windows.Forms.Label();
            this.Error = new System.Windows.Forms.Label();
            this.Skip = new System.Windows.Forms.Button();
            this.Clear = new System.Windows.Forms.Button();
            this.AcctId = new Uheaa.Common.WinForms.OmniTextBox();
            this.UheaaRegion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DailyProcessed)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 44);
            this.label1.TabIndex = 2;
            this.label1.Text = "SSN / Acct / Case / Ref";
            // 
            // ProcessButton
            // 
            this.ProcessButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ProcessButton.Enabled = false;
            this.ProcessButton.Font = new System.Drawing.Font("Arial", 14F);
            this.ProcessButton.Location = new System.Drawing.Point(564, 444);
            this.ProcessButton.Name = "ProcessButton";
            this.ProcessButton.Size = new System.Drawing.Size(97, 29);
            this.ProcessButton.TabIndex = 5;
            this.ProcessButton.Text = "Process";
            this.ProcessButton.UseVisualStyleBackColor = true;
            this.ProcessButton.Click += new System.EventHandler(this.ProcessButton_Click);
            // 
            // BorrowerName
            // 
            this.BorrowerName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BorrowerName.Location = new System.Drawing.Point(301, 19);
            this.BorrowerName.Name = "BorrowerName";
            this.BorrowerName.Size = new System.Drawing.Size(369, 131);
            this.BorrowerName.TabIndex = 9;
            // 
            // LookupButton
            // 
            this.LookupButton.Font = new System.Drawing.Font("Arial", 14F);
            this.LookupButton.Location = new System.Drawing.Point(187, 89);
            this.LookupButton.Name = "LookupButton";
            this.LookupButton.Size = new System.Drawing.Size(97, 29);
            this.LookupButton.TabIndex = 9;
            this.LookupButton.Text = "Lookup";
            this.LookupButton.UseVisualStyleBackColor = true;
            this.LookupButton.Click += new System.EventHandler(this.LookupButton_Click);
            // 
            // Close
            // 
            this.Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close.Font = new System.Drawing.Font("Arial", 14F);
            this.Close.Location = new System.Drawing.Point(12, 444);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(97, 29);
            this.Close.TabIndex = 10;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Leave += new System.EventHandler(this.Close_Leave);
            // 
            // CorrFax
            // 
            this.CorrFax.AutoSize = true;
            this.CorrFax.Enabled = false;
            this.CorrFax.Location = new System.Drawing.Point(59, 244);
            this.CorrFax.Name = "CorrFax";
            this.CorrFax.Size = new System.Drawing.Size(102, 26);
            this.CorrFax.TabIndex = 4;
            this.CorrFax.Text = "Corr Fax";
            this.CorrFax.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 340);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 22);
            this.label3.TabIndex = 10;
            this.label3.Text = "Doc Type:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 302);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 22);
            this.label6.TabIndex = 11;
            this.label6.Text = "Doc ID:";
            // 
            // SelectedArc
            // 
            this.SelectedArc.AutoSize = true;
            this.SelectedArc.Location = new System.Drawing.Point(134, 378);
            this.SelectedArc.Name = "SelectedArc";
            this.SelectedArc.Size = new System.Drawing.Size(0, 22);
            this.SelectedArc.TabIndex = 12;
            // 
            // SelectedId
            // 
            this.SelectedId.AutoSize = true;
            this.SelectedId.Location = new System.Drawing.Point(134, 302);
            this.SelectedId.Name = "SelectedId";
            this.SelectedId.Size = new System.Drawing.Size(0, 22);
            this.SelectedId.TabIndex = 13;
            // 
            // SelectedType
            // 
            this.SelectedType.AutoSize = true;
            this.SelectedType.Location = new System.Drawing.Point(134, 340);
            this.SelectedType.Name = "SelectedType";
            this.SelectedType.Size = new System.Drawing.Size(0, 22);
            this.SelectedType.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(51, 378);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 22);
            this.label10.TabIndex = 15;
            this.label10.Text = "ARC:";
            // 
            // DocumentType
            // 
            this.DocumentType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.DocumentType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.DocumentType.DropDownWidth = 750;
            this.DocumentType.Enabled = false;
            this.DocumentType.Location = new System.Drawing.Point(171, 165);
            this.DocumentType.Name = "DocumentType";
            this.DocumentType.Size = new System.Drawing.Size(490, 30);
            this.DocumentType.TabIndex = 2;
            this.DocumentType.SelectedIndexChanged += new System.EventHandler(this.DocumentType_SelectedIndexChanged);
            this.DocumentType.TextChanged += new System.EventHandler(this.DocumentType_TextChanged);
            this.DocumentType.Leave += new System.EventHandler(this.DocumentType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 22);
            this.label4.TabIndex = 19;
            this.label4.Text = "Document Type:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 22);
            this.label5.TabIndex = 20;
            this.label5.Text = "Document ID:";
            // 
            // DocumentIdCombo
            // 
            this.DocumentIdCombo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.DocumentIdCombo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.DocumentIdCombo.DropDownWidth = 750;
            this.DocumentIdCombo.Enabled = false;
            this.DocumentIdCombo.Location = new System.Drawing.Point(171, 204);
            this.DocumentIdCombo.Name = "DocumentIdCombo";
            this.DocumentIdCombo.Size = new System.Drawing.Size(490, 30);
            this.DocumentIdCombo.TabIndex = 3;
            this.DocumentIdCombo.SelectedIndexChanged += new System.EventHandler(this.DocumentIdCombo_SelectedIndexChanged);
            this.DocumentIdCombo.TextChanged += new System.EventHandler(this.DocumentIdCombo_TextChanged);
            this.DocumentIdCombo.Leave += new System.EventHandler(this.DocumentIdCombo_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(672, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 22);
            this.label2.TabIndex = 24;
            this.label2.Text = "Documents Processed On:";
            // 
            // DailyProcessed
            // 
            this.DailyProcessed.AllowUserToAddRows = false;
            this.DailyProcessed.AllowUserToDeleteRows = false;
            this.DailyProcessed.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DailyProcessed.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.DailyProcessed.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DailyProcessed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DailyProcessed.Location = new System.Drawing.Point(676, 34);
            this.DailyProcessed.Name = "DailyProcessed";
            this.DailyProcessed.ReadOnly = true;
            this.DailyProcessed.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DailyProcessed.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.DailyProcessed.Size = new System.Drawing.Size(659, 439);
            this.DailyProcessed.TabIndex = 13;
            this.DailyProcessed.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DailyProcessed_ColumnHeaderMouseClick);
            // 
            // Refresh
            // 
            this.Refresh.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Refresh.Location = new System.Drawing.Point(1251, 8);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(84, 23);
            this.Refresh.TabIndex = 12;
            this.Refresh.Text = "Refresh";
            this.Refresh.UseVisualStyleBackColor = true;
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // DateSelection
            // 
            this.DateSelection.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateSelection.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateSelection.Location = new System.Drawing.Point(918, 9);
            this.DateSelection.Name = "DateSelection";
            this.DateSelection.Size = new System.Drawing.Size(100, 22);
            this.DateSelection.TabIndex = 11;
            this.DateSelection.ValueChanged += new System.EventHandler(this.DateSelection_ValueChanged);
            // 
            // ProcessedCount
            // 
            this.ProcessedCount.AutoSize = true;
            this.ProcessedCount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcessedCount.Location = new System.Drawing.Point(1035, 12);
            this.ProcessedCount.Name = "ProcessedCount";
            this.ProcessedCount.Size = new System.Drawing.Size(0, 15);
            this.ProcessedCount.TabIndex = 28;
            // 
            // Error
            // 
            this.Error.AutoSize = true;
            this.Error.ForeColor = System.Drawing.Color.Red;
            this.Error.Location = new System.Drawing.Point(119, 415);
            this.Error.Name = "Error";
            this.Error.Size = new System.Drawing.Size(0, 22);
            this.Error.TabIndex = 30;
            // 
            // Skip
            // 
            this.Skip.Font = new System.Drawing.Font("Arial", 14F);
            this.Skip.Location = new System.Drawing.Point(187, 121);
            this.Skip.Name = "Skip";
            this.Skip.Size = new System.Drawing.Size(97, 29);
            this.Skip.TabIndex = 1;
            this.Skip.Text = "Skip";
            this.Skip.UseVisualStyleBackColor = true;
            this.Skip.Visible = false;
            this.Skip.Click += new System.EventHandler(this.Skip_Click);
            // 
            // Clear
            // 
            this.Clear.Font = new System.Drawing.Font("Arial", 14F);
            this.Clear.Location = new System.Drawing.Point(187, 56);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(97, 29);
            this.Clear.TabIndex = 8;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // AcctId
            // 
            this.AcctId.AllowAllCharacters = true;
            this.AcctId.AllowAlphaCharacters = true;
            this.AcctId.AllowedAdditionalCharacters = "RFP@P";
            this.AcctId.AllowNumericCharacters = true;
            this.AcctId.InvalidColor = System.Drawing.Color.LightPink;
            this.AcctId.Location = new System.Drawing.Point(156, 19);
            this.AcctId.Mask = "";
            this.AcctId.MaxLength = 12;
            this.AcctId.Name = "AcctId";
            this.AcctId.Size = new System.Drawing.Size(128, 29);
            this.AcctId.TabIndex = 0;
            this.AcctId.ValidationMessage = null;
            this.AcctId.ValidColor = System.Drawing.Color.LightGreen;
            this.AcctId.TextChanged += new System.EventHandler(this.AcctId_TextChanged);
            // 
            // UheaaRegion
            // 
            this.UheaaRegion.AutoSize = true;
            this.UheaaRegion.Location = new System.Drawing.Point(21, 79);
            this.UheaaRegion.Name = "UheaaRegion";
            this.UheaaRegion.Size = new System.Drawing.Size(0, 22);
            this.UheaaRegion.TabIndex = 31;
            // 
            // Selection
            // 
            this.AcceptButton = this.ProcessButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Close;
            this.ClientSize = new System.Drawing.Size(1347, 485);
            this.Controls.Add(this.UheaaRegion);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.Skip);
            this.Controls.Add(this.Error);
            this.Controls.Add(this.ProcessedCount);
            this.Controls.Add(this.DateSelection);
            this.Controls.Add(this.Refresh);
            this.Controls.Add(this.DailyProcessed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DocumentType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DocumentIdCombo);
            this.Controls.Add(this.AcctId);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.SelectedType);
            this.Controls.Add(this.SelectedId);
            this.Controls.Add(this.SelectedArc);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CorrFax);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.LookupButton);
            this.Controls.Add(this.BorrowerName);
            this.Controls.Add(this.ProcessButton);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MinimumSize = new System.Drawing.Size(1363, 523);
            this.Name = "Selection";
            this.Text = "Doc ID";
            this.Activated += new System.EventHandler(this.Selection_Validated);
            ((System.ComponentModel.ISupportInitialize)(this.DailyProcessed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ProcessButton;
        private System.Windows.Forms.Label BorrowerName;
        private System.Windows.Forms.Button LookupButton;
        private new System.Windows.Forms.Button Close;
        private System.Windows.Forms.CheckBox CorrFax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label SelectedArc;
        private System.Windows.Forms.Label SelectedId;
        private System.Windows.Forms.Label SelectedType;
        private System.Windows.Forms.Label label10;
        private Uheaa.Common.WinForms.OmniTextBox AcctId;
        private System.Windows.Forms.ComboBox DocumentType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox DocumentIdCombo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView DailyProcessed;
        private new System.Windows.Forms.Button Refresh;
        private System.Windows.Forms.DateTimePicker DateSelection;
        private System.Windows.Forms.Label ProcessedCount;
        private System.Windows.Forms.Label Error;
        private System.Windows.Forms.Button Skip;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.Label UheaaRegion;
    }
}

