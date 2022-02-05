namespace SIRPTFED
{
    partial class ServicerMetrics
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
            this.label1 = new System.Windows.Forms.Label();
            this.Category = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Metric = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Month = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspenseAmountLabel = new System.Windows.Forms.Label();
            this.SuspenseTotalLabel = new System.Windows.Forms.Label();
            this.SuspenseTotalDecimalTextBox = new Uheaa.Common.WinForms.NumericDecimalTextBox();
            this.SuspenseAmountDecimalTextBox = new Uheaa.Common.WinForms.NumericDecimalTextBox();
            this.AvgBacklog = new Uheaa.Common.WinForms.NumericTextBox();
            this.validationButton2 = new Uheaa.Common.WinForms.ValidationButton();
            this.validationButton1 = new Uheaa.Common.WinForms.ValidationButton();
            this.TotalRecords = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            this.CompliantRecords = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            this.Year = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(94, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Category:";
            // 
            // Category
            // 
            this.Category.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Category.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Category.FormattingEnabled = true;
            this.Category.Location = new System.Drawing.Point(177, 6);
            this.Category.Name = "Category";
            this.Category.Size = new System.Drawing.Size(273, 28);
            this.Category.TabIndex = 1;
            this.Category.SelectionChangeCommitted += new System.EventHandler(this.Category_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(115, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Metric:";
            // 
            // Metric
            // 
            this.Metric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Metric.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Metric.FormattingEnabled = true;
            this.Metric.Location = new System.Drawing.Point(177, 40);
            this.Metric.Name = "Metric";
            this.Metric.Size = new System.Drawing.Size(331, 28);
            this.Metric.TabIndex = 3;
            this.Metric.SelectionChangeCommitted += new System.EventHandler(this.Metric_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Compliant Records:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Total Records:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(113, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 20);
            this.label6.TabIndex = 7;
            this.label6.Text = "Month:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(124, 111);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 20);
            this.label7.TabIndex = 8;
            this.label7.Text = "Year:";
            // 
            // Month
            // 
            this.Month.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Month.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Month.FormattingEnabled = true;
            this.Month.Location = new System.Drawing.Point(177, 74);
            this.Month.Name = "Month";
            this.Month.Size = new System.Drawing.Size(121, 28);
            this.Month.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 211);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "Average Backog Age:";
            // 
            // SuspenseAmountLabel
            // 
            this.SuspenseAmountLabel.AutoSize = true;
            this.SuspenseAmountLabel.Location = new System.Drawing.Point(23, 246);
            this.SuspenseAmountLabel.Name = "SuspenseAmountLabel";
            this.SuspenseAmountLabel.Size = new System.Drawing.Size(145, 20);
            this.SuspenseAmountLabel.TabIndex = 19;
            this.SuspenseAmountLabel.Text = "Suspense Amount:";
            // 
            // SuspenseTotalLabel
            // 
            this.SuspenseTotalLabel.AutoSize = true;
            this.SuspenseTotalLabel.Location = new System.Drawing.Point(44, 278);
            this.SuspenseTotalLabel.Name = "SuspenseTotalLabel";
            this.SuspenseTotalLabel.Size = new System.Drawing.Size(124, 20);
            this.SuspenseTotalLabel.TabIndex = 20;
            this.SuspenseTotalLabel.Text = "Suspense Total:";
            // 
            // SuspenseTotalDecimalTextBox
            // 
            this.SuspenseTotalDecimalTextBox.AllowedSpecialCharacters = "";
            this.SuspenseTotalDecimalTextBox.Location = new System.Drawing.Point(177, 272);
            this.SuspenseTotalDecimalTextBox.Name = "SuspenseTotalDecimalTextBox";
            this.SuspenseTotalDecimalTextBox.Size = new System.Drawing.Size(174, 26);
            this.SuspenseTotalDecimalTextBox.TabIndex = 18;
            // 
            // SuspenseAmountDecimalTextBox
            // 
            this.SuspenseAmountDecimalTextBox.AllowedSpecialCharacters = "";
            this.SuspenseAmountDecimalTextBox.Location = new System.Drawing.Point(177, 240);
            this.SuspenseAmountDecimalTextBox.Name = "SuspenseAmountDecimalTextBox";
            this.SuspenseAmountDecimalTextBox.Size = new System.Drawing.Size(174, 26);
            this.SuspenseAmountDecimalTextBox.TabIndex = 17;
            // 
            // AvgBacklog
            // 
            this.AvgBacklog.AllowedSpecialCharacters = "";
            this.AvgBacklog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AvgBacklog.Location = new System.Drawing.Point(177, 208);
            this.AvgBacklog.Name = "AvgBacklog";
            this.AvgBacklog.Size = new System.Drawing.Size(100, 26);
            this.AvgBacklog.TabIndex = 16;
            // 
            // validationButton2
            // 
            this.validationButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.validationButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.validationButton2.Location = new System.Drawing.Point(12, 335);
            this.validationButton2.Name = "validationButton2";
            this.validationButton2.Size = new System.Drawing.Size(97, 41);
            this.validationButton2.TabIndex = 14;
            this.validationButton2.Text = "Cancel";
            this.validationButton2.UseVisualStyleBackColor = true;
            // 
            // validationButton1
            // 
            this.validationButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.validationButton1.Location = new System.Drawing.Point(467, 335);
            this.validationButton1.Name = "validationButton1";
            this.validationButton1.Size = new System.Drawing.Size(97, 41);
            this.validationButton1.TabIndex = 13;
            this.validationButton1.Text = "OK";
            this.validationButton1.UseVisualStyleBackColor = true;
            this.validationButton1.Click += new System.EventHandler(this.validationButton1_Click);
            // 
            // TotalRecords
            // 
            this.TotalRecords.AllowedSpecialCharacters = ".";
            this.TotalRecords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalRecords.Location = new System.Drawing.Point(177, 176);
            this.TotalRecords.MaxLength = 10;
            this.TotalRecords.Name = "TotalRecords";
            this.TotalRecords.Size = new System.Drawing.Size(152, 26);
            this.TotalRecords.TabIndex = 12;
            // 
            // CompliantRecords
            // 
            this.CompliantRecords.AllowedSpecialCharacters = ".";
            this.CompliantRecords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CompliantRecords.Location = new System.Drawing.Point(177, 142);
            this.CompliantRecords.MaxLength = 10;
            this.CompliantRecords.Name = "CompliantRecords";
            this.CompliantRecords.Size = new System.Drawing.Size(152, 26);
            this.CompliantRecords.TabIndex = 11;
            // 
            // Year
            // 
            this.Year.AllowedSpecialCharacters = "";
            this.Year.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Year.Location = new System.Drawing.Point(177, 108);
            this.Year.MaxLength = 4;
            this.Year.Name = "Year";
            this.Year.Size = new System.Drawing.Size(65, 26);
            this.Year.TabIndex = 10;
            // 
            // ServicerMetrics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 388);
            this.Controls.Add(this.SuspenseTotalLabel);
            this.Controls.Add(this.SuspenseAmountLabel);
            this.Controls.Add(this.SuspenseTotalDecimalTextBox);
            this.Controls.Add(this.SuspenseAmountDecimalTextBox);
            this.Controls.Add(this.AvgBacklog);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.validationButton2);
            this.Controls.Add(this.validationButton1);
            this.Controls.Add(this.TotalRecords);
            this.Controls.Add(this.CompliantRecords);
            this.Controls.Add(this.Year);
            this.Controls.Add(this.Month);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Metric);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Category);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(592, 354);
            this.Name = "ServicerMetrics";
            this.ShowIcon = false;
            this.Text = "Servicer Metrics";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Category;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Metric;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox Month;
        private Uheaa.Common.WinForms.RequiredNumericTextBox Year;
        private Uheaa.Common.WinForms.RequiredNumericTextBox CompliantRecords;
        private Uheaa.Common.WinForms.RequiredNumericTextBox TotalRecords;
        private Uheaa.Common.WinForms.ValidationButton validationButton1;
        private Uheaa.Common.WinForms.ValidationButton validationButton2;
        private System.Windows.Forms.Label label3;
        private Uheaa.Common.WinForms.NumericTextBox AvgBacklog;
        private Uheaa.Common.WinForms.NumericDecimalTextBox SuspenseAmountDecimalTextBox;
        private Uheaa.Common.WinForms.NumericDecimalTextBox SuspenseTotalDecimalTextBox;
        private System.Windows.Forms.Label SuspenseAmountLabel;
        private System.Windows.Forms.Label SuspenseTotalLabel;
    }
}

