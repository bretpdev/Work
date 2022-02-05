namespace BCSRETMAIL
{
	partial class ScannerDialog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScannerDialog));
            this.ScannerInputTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ReceivedDate = new System.Windows.Forms.DateTimePicker();
            this.ProcessedCount = new System.Windows.Forms.Label();
            this.DateSelection = new System.Windows.Forms.DateTimePicker();
            this.Refresh = new System.Windows.Forms.Button();
            this.DailyProcessed = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.BorrowerName = new System.Windows.Forms.Label();
            this.LetterCode = new System.Windows.Forms.ComboBox();
            this.CreateDate = new System.Windows.Forms.MaskedTextBox();
            this.Cancel = new Uheaa.Common.WinForms.ValidationButton();
            this.Add = new Uheaa.Common.WinForms.ValidationButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Clear = new System.Windows.Forms.Button();
            this.Lookup = new System.Windows.Forms.Button();
            this.Forward = new System.Windows.Forms.Button();
            this.Check = new System.Windows.Forms.PictureBox();
            this.ReturnReason = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.FocusText = new System.Windows.Forms.Timer(this.components);
            this.AccountIdentifier = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.DailyProcessed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Check)).BeginInit();
            this.SuspendLayout();
            // 
            // ScannerInputTxt
            // 
            this.ScannerInputTxt.Location = new System.Drawing.Point(12, 29);
            this.ScannerInputTxt.Name = "ScannerInputTxt";
            this.ScannerInputTxt.Size = new System.Drawing.Size(0, 26);
            this.ScannerInputTxt.TabIndex = 25;
            this.ScannerInputTxt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ScannerInputTxt_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(388, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Please select the date the returned mail was received.";
            // 
            // ReceivedDate
            // 
            this.ReceivedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReceivedDate.Location = new System.Drawing.Point(93, 29);
            this.ReceivedDate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ReceivedDate.Name = "ReceivedDate";
            this.ReceivedDate.Size = new System.Drawing.Size(288, 26);
            this.ReceivedDate.TabIndex = 0;
            this.ReceivedDate.Value = new System.DateTime(2019, 6, 27, 11, 29, 47, 0);
            this.ReceivedDate.ValueChanged += new System.EventHandler(this.ReceivedDate_ValueChanged);
            this.ReceivedDate.Enter += new System.EventHandler(this.ReceivedDate_Enter);
            this.ReceivedDate.Leave += new System.EventHandler(this.ReceivedDate_Leave);
            // 
            // ProcessedCount
            // 
            this.ProcessedCount.AutoSize = true;
            this.ProcessedCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcessedCount.Location = new System.Drawing.Point(873, 9);
            this.ProcessedCount.Name = "ProcessedCount";
            this.ProcessedCount.Size = new System.Drawing.Size(0, 15);
            this.ProcessedCount.TabIndex = 33;
            // 
            // DateSelection
            // 
            this.DateSelection.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateSelection.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateSelection.Location = new System.Drawing.Point(755, 4);
            this.DateSelection.Name = "DateSelection";
            this.DateSelection.Size = new System.Drawing.Size(100, 22);
            this.DateSelection.TabIndex = 10;
            this.DateSelection.ValueChanged += new System.EventHandler(this.DateSelection_ValueChanged);
            this.DateSelection.Enter += new System.EventHandler(this.DateSelection_Enter);
            this.DateSelection.Leave += new System.EventHandler(this.DateSelection_Leave);
            // 
            // Refresh
            // 
            this.Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Refresh.Location = new System.Drawing.Point(1250, 3);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(84, 24);
            this.Refresh.TabIndex = 11;
            this.Refresh.Text = "Refresh";
            this.Refresh.UseVisualStyleBackColor = true;
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
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
            this.DailyProcessed.Location = new System.Drawing.Point(516, 33);
            this.DailyProcessed.Name = "DailyProcessed";
            this.DailyProcessed.ReadOnly = true;
            this.DailyProcessed.RowHeadersVisible = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DailyProcessed.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DailyProcessed.Size = new System.Drawing.Size(818, 439);
            this.DailyProcessed.TabIndex = 14;
            this.DailyProcessed.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DailyProcessed_ColumnHeaderMouseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(512, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(237, 24);
            this.label2.TabIndex = 32;
            this.label2.Text = "Documents Processed On:";
            // 
            // BorrowerName
            // 
            this.BorrowerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BorrowerName.ForeColor = System.Drawing.Color.Red;
            this.BorrowerName.Location = new System.Drawing.Point(19, 60);
            this.BorrowerName.Name = "BorrowerName";
            this.BorrowerName.Size = new System.Drawing.Size(460, 82);
            this.BorrowerName.TabIndex = 34;
            this.BorrowerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LetterCode
            // 
            this.LetterCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.LetterCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.LetterCode.FormattingEnabled = true;
            this.LetterCode.Location = new System.Drawing.Point(252, 224);
            this.LetterCode.MaxLength = 10;
            this.LetterCode.Name = "LetterCode";
            this.LetterCode.Size = new System.Drawing.Size(226, 28);
            this.LetterCode.TabIndex = 3;
            this.LetterCode.Enter += new System.EventHandler(this.LetterCode_Enter);
            this.LetterCode.Leave += new System.EventHandler(this.LetterCode_Leave);
            // 
            // CreateDate
            // 
            this.CreateDate.Location = new System.Drawing.Point(253, 316);
            this.CreateDate.Mask = "00/00/0000";
            this.CreateDate.Name = "CreateDate";
            this.CreateDate.Size = new System.Drawing.Size(96, 26);
            this.CreateDate.TabIndex = 5;
            this.CreateDate.ValidatingType = typeof(System.DateTime);
            this.CreateDate.TextChanged += new System.EventHandler(this.CreateDate_TextChanged);
            this.CreateDate.Enter += new System.EventHandler(this.CreateDate_Enter);
            this.CreateDate.Leave += new System.EventHandler(this.CreateDate_Leave);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(19, 444);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(84, 28);
            this.Cancel.TabIndex = 9;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Add
            // 
            this.Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Add.Enabled = false;
            this.Add.Location = new System.Drawing.Point(394, 444);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(84, 28);
            this.Add.TabIndex = 7;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(147, 319);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 42;
            this.label3.Text = "Create Date:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(147, 227);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 20);
            this.label6.TabIndex = 40;
            this.label6.Text = "Letter Code:";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(11, 174);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(236, 47);
            this.label7.TabIndex = 36;
            this.label7.Text = "ACS Encryption Code / SSN / AccountNumber / Reference ID";
            // 
            // Clear
            // 
            this.Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Clear.Location = new System.Drawing.Point(198, 444);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(84, 28);
            this.Clear.TabIndex = 8;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // Lookup
            // 
            this.Lookup.Location = new System.Drawing.Point(390, 142);
            this.Lookup.Name = "Lookup";
            this.Lookup.Size = new System.Drawing.Size(89, 32);
            this.Lookup.TabIndex = 1;
            this.Lookup.Text = "Lookup";
            this.Lookup.UseVisualStyleBackColor = true;
            this.Lookup.Click += new System.EventHandler(this.Lookup_Click);
            // 
            // Forward
            // 
            this.Forward.Location = new System.Drawing.Point(252, 366);
            this.Forward.Name = "Forward";
            this.Forward.Size = new System.Drawing.Size(173, 31);
            this.Forward.TabIndex = 6;
            this.Forward.Text = "Forwarding Address";
            this.Forward.UseVisualStyleBackColor = true;
            this.Forward.Click += new System.EventHandler(this.Forward_Click);
            // 
            // Check
            // 
            this.Check.Location = new System.Drawing.Point(431, 366);
            this.Check.Name = "Check";
            this.Check.Size = new System.Drawing.Size(37, 31);
            this.Check.TabIndex = 56;
            this.Check.TabStop = false;
            // 
            // ReturnReason
            // 
            this.ReturnReason.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ReturnReason.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ReturnReason.FormattingEnabled = true;
            this.ReturnReason.Location = new System.Drawing.Point(252, 269);
            this.ReturnReason.Name = "ReturnReason";
            this.ReturnReason.Size = new System.Drawing.Size(226, 28);
            this.ReturnReason.TabIndex = 4;
            this.ReturnReason.SelectedIndexChanged += new System.EventHandler(this.ReturnReason_SelectedIndexChanged);
            this.ReturnReason.Enter += new System.EventHandler(this.ReturnReason_Enter);
            this.ReturnReason.Leave += new System.EventHandler(this.ReturnReason_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(122, 272);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 20);
            this.label4.TabIndex = 58;
            this.label4.Text = "Return Reason:";
            // 
            // FocusText
            // 
            this.FocusText.Interval = 1000;
            this.FocusText.Tick += new System.EventHandler(this.FocusText_Tick);
            // 
            // AccountIdentifier
            // 
            this.AccountIdentifier.Location = new System.Drawing.Point(253, 184);
            this.AccountIdentifier.MaxLength = 10;
            this.AccountIdentifier.Name = "AccountIdentifier";
            this.AccountIdentifier.Size = new System.Drawing.Size(225, 26);
            this.AccountIdentifier.TabIndex = 59;
            this.AccountIdentifier.TextChanged += new System.EventHandler(this.AccountIdentifier_TextChanged);
            this.AccountIdentifier.Enter += new System.EventHandler(this.AccountIdentifier_Enter);
            this.AccountIdentifier.Leave += new System.EventHandler(this.AccountIdentifier_Leave);
            // 
            // ScannerDialog
            // 
            this.AcceptButton = this.Add;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(1347, 484);
            this.Controls.Add(this.AccountIdentifier);
            this.Controls.Add(this.ReturnReason);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Check);
            this.Controls.Add(this.Forward);
            this.Controls.Add(this.Lookup);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.LetterCode);
            this.Controls.Add(this.CreateDate);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.BorrowerName);
            this.Controls.Add(this.ProcessedCount);
            this.Controls.Add(this.DateSelection);
            this.Controls.Add(this.Refresh);
            this.Controls.Add(this.DailyProcessed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReceivedDate);
            this.Controls.Add(this.ScannerInputTxt);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1363, 523);
            this.Name = "ScannerDialog";
            this.Text = "Return Mail Scanner";
            this.Shown += new System.EventHandler(this.ScannerDialog_Shown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ScannerDialog_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.DailyProcessed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Check)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox ScannerInputTxt;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DateTimePicker ReceivedDate;
        private System.Windows.Forms.Label ProcessedCount;
        private System.Windows.Forms.DateTimePicker DateSelection;
        private System.Windows.Forms.Button Refresh;
        private System.Windows.Forms.DataGridView DailyProcessed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label BorrowerName;
        private System.Windows.Forms.ComboBox LetterCode;
        private System.Windows.Forms.MaskedTextBox CreateDate;
        private Uheaa.Common.WinForms.ValidationButton Cancel;
        private Uheaa.Common.WinForms.ValidationButton Add;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.Button Lookup;
        private System.Windows.Forms.Button Forward;
        private System.Windows.Forms.PictureBox Check;
        private System.Windows.Forms.ComboBox ReturnReason;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer FocusText;
        private System.Windows.Forms.TextBox AccountIdentifier;
    }
}
