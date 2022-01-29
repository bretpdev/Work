namespace TEXTCOORD
{
    partial class Search
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Search));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Segment = new System.Windows.Forms.CheckedListBox();
            this.ExcelPreview = new System.Windows.Forms.DataGridView();
            this.Export = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.PerformanceCat = new System.Windows.Forms.CheckedListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Campaign = new System.Windows.Forms.ComboBox();
            this.picLoader = new System.Windows.Forms.PictureBox();
            this.SearchSub = new Uheaa.Common.WinForms.ValidationButton();
            this.UpperDelinquency = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            this.LowerDelinquency = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            this.NumberToSend = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.LowerAge = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            this.UpperAge = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoader)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "# of Message to Send:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Delinquency Range:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(461, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Segment:";
            // 
            // Segment
            // 
            this.Segment.CheckOnClick = true;
            this.Segment.ColumnWidth = 70;
            this.Segment.FormattingEnabled = true;
            this.Segment.Items.AddRange(new object[] {
            "1 Consol or PLUS",
            "2 Graduated < 3 years ago",
            "3 Graduated 3+ years ago",
            "4 Dropped Out < 3 years ago",
            "5 Dropped Out 3+ years ago",
            "6 Rehabbed",
            "7 Other, not listed above"});
            this.Segment.Location = new System.Drawing.Point(465, 37);
            this.Segment.Name = "Segment";
            this.Segment.Size = new System.Drawing.Size(241, 172);
            this.Segment.TabIndex = 7;
            // 
            // ExcelPreview
            // 
            this.ExcelPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExcelPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.ExcelPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ExcelPreview.Location = new System.Drawing.Point(12, 225);
            this.ExcelPreview.Name = "ExcelPreview";
            this.ExcelPreview.RowHeadersVisible = false;
            this.ExcelPreview.Size = new System.Drawing.Size(1253, 341);
            this.ExcelPreview.TabIndex = 8;
            // 
            // Export
            // 
            this.Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Export.Enabled = false;
            this.Export.Location = new System.Drawing.Point(1139, 183);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(126, 36);
            this.Export.TabIndex = 10;
            this.Export.Text = "Export to Excel";
            this.Export.UseVisualStyleBackColor = true;
            this.Export.Click += new System.EventHandler(this.Export_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(742, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 20);
            this.label5.TabIndex = 12;
            this.label5.Text = "Performance Category:";
            // 
            // PerformanceCat
            // 
            this.PerformanceCat.CheckOnClick = true;
            this.PerformanceCat.ColumnWidth = 140;
            this.PerformanceCat.FormattingEnabled = true;
            this.PerformanceCat.Items.AddRange(new object[] {
            "1 School",
            "2 Grace",
            "3 Repay",
            "4 Military",
            "5 Defer",
            "6 Forb",
            "7 Delq 6-30",
            "8 Delq 31-90",
            "9 Delq 91-150",
            "10 Delq 151-270",
            "11 Delq 271-360",
            "12 Delq 361+"});
            this.PerformanceCat.Location = new System.Drawing.Point(746, 37);
            this.PerformanceCat.MultiColumn = true;
            this.PerformanceCat.Name = "PerformanceCat";
            this.PerformanceCat.Size = new System.Drawing.Size(361, 172);
            this.PerformanceCat.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Campaign:";
            // 
            // Campaign
            // 
            this.Campaign.FormattingEnabled = true;
            this.Campaign.Location = new System.Drawing.Point(99, 6);
            this.Campaign.Name = "Campaign";
            this.Campaign.Size = new System.Drawing.Size(315, 28);
            this.Campaign.TabIndex = 15;
            this.Campaign.SelectedIndexChanged += new System.EventHandler(this.Campaign_SelectedIndexChanged);
            // 
            // picLoader
            // 
            this.picLoader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picLoader.Image = ((System.Drawing.Image)(resources.GetObject("picLoader.Image")));
            this.picLoader.InitialImage = ((System.Drawing.Image)(resources.GetObject("picLoader.InitialImage")));
            this.picLoader.Location = new System.Drawing.Point(599, 292);
            this.picLoader.Name = "picLoader";
            this.picLoader.Size = new System.Drawing.Size(100, 100);
            this.picLoader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLoader.TabIndex = 17;
            this.picLoader.TabStop = false;
            this.picLoader.Visible = false;
            // 
            // SearchSub
            // 
            this.SearchSub.Location = new System.Drawing.Point(12, 183);
            this.SearchSub.Name = "SearchSub";
            this.SearchSub.Size = new System.Drawing.Size(75, 36);
            this.SearchSub.TabIndex = 11;
            this.SearchSub.Text = "Search";
            this.SearchSub.UseVisualStyleBackColor = true;
            this.SearchSub.OnValidate += new Uheaa.Common.WinForms.ValidationButton.ValidationHandler(this.SearchSub_OnValidate);
            this.SearchSub.Click += new System.EventHandler(this.SearchSub_Click);
            // 
            // UpperDelinquency
            // 
            this.UpperDelinquency.AllowedSpecialCharacters = "";
            this.UpperDelinquency.Location = new System.Drawing.Point(289, 84);
            this.UpperDelinquency.Name = "UpperDelinquency";
            this.UpperDelinquency.Size = new System.Drawing.Size(46, 26);
            this.UpperDelinquency.TabIndex = 5;
            // 
            // LowerDelinquency
            // 
            this.LowerDelinquency.AllowedSpecialCharacters = "";
            this.LowerDelinquency.Location = new System.Drawing.Point(217, 84);
            this.LowerDelinquency.Name = "LowerDelinquency";
            this.LowerDelinquency.Size = new System.Drawing.Size(46, 26);
            this.LowerDelinquency.TabIndex = 3;
            // 
            // NumberToSend
            // 
            this.NumberToSend.AllowedSpecialCharacters = "";
            this.NumberToSend.Location = new System.Drawing.Point(217, 52);
            this.NumberToSend.Name = "NumberToSend";
            this.NumberToSend.Size = new System.Drawing.Size(76, 26);
            this.NumberToSend.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Age Range:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(269, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 20);
            this.label9.TabIndex = 21;
            this.label9.Text = "-";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(269, 120);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 20);
            this.label10.TabIndex = 22;
            this.label10.Text = "-";
            // 
            // LowerAge
            // 
            this.LowerAge.AllowedSpecialCharacters = "";
            this.LowerAge.Location = new System.Drawing.Point(217, 117);
            this.LowerAge.Name = "LowerAge";
            this.LowerAge.Size = new System.Drawing.Size(46, 26);
            this.LowerAge.TabIndex = 23;
            this.LowerAge.Leave += new System.EventHandler(this.LowerAge_Leave);
            // 
            // UpperAge
            // 
            this.UpperAge.AllowedSpecialCharacters = "";
            this.UpperAge.Location = new System.Drawing.Point(289, 117);
            this.UpperAge.Name = "UpperAge";
            this.UpperAge.Size = new System.Drawing.Size(46, 26);
            this.UpperAge.TabIndex = 24;
            this.UpperAge.Leave += new System.EventHandler(this.UpperAge_Leave);
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1277, 578);
            this.Controls.Add(this.UpperAge);
            this.Controls.Add(this.LowerAge);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.picLoader);
            this.Controls.Add(this.Campaign);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.PerformanceCat);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SearchSub);
            this.Controls.Add(this.Export);
            this.Controls.Add(this.ExcelPreview);
            this.Controls.Add(this.Segment);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.UpperDelinquency);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LowerDelinquency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NumberToSend);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Search";
            this.Text = "Create Texting Spreadsheet";
            ((System.ComponentModel.ISupportInitialize)(this.ExcelPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Uheaa.Common.WinForms.RequiredNumericTextBox NumberToSend;
        private System.Windows.Forms.Label label2;
        private Uheaa.Common.WinForms.RequiredNumericTextBox LowerDelinquency;
        private Uheaa.Common.WinForms.RequiredNumericTextBox UpperDelinquency;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckedListBox Segment;
        private System.Windows.Forms.DataGridView ExcelPreview;
        private System.Windows.Forms.Button Export;
        private Uheaa.Common.WinForms.ValidationButton SearchSub;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox PerformanceCat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox Campaign;
        private System.Windows.Forms.PictureBox picLoader;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private Uheaa.Common.WinForms.RequiredNumericTextBox LowerAge;
        private Uheaa.Common.WinForms.RequiredNumericTextBox UpperAge;
    }
}
