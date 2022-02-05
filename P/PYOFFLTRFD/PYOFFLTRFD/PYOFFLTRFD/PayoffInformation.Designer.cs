namespace PYOFFLTRFD
{
    partial class PayoffInformation
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
            this.label2 = new System.Windows.Forms.Label();
            this.AllLoans = new System.Windows.Forms.CheckBox();
            this.OK = new System.Windows.Forms.Button();
            this.AccountIdentifier = new Uheaa.Common.WinForms.RequiredAccountNumberTextBox();
            this.VersionNumber = new System.Windows.Forms.Label();
            this.PayoffDate = new System.Windows.Forms.DateTimePicker();
            this.PayoffDataGrid = new PYOFFLTRFD.MyDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.PayoffDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(43, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Account Number / SSN";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(123, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Payoff Date";
            // 
            // AllLoans
            // 
            this.AllLoans.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AllLoans.AutoSize = true;
            this.AllLoans.Enabled = false;
            this.AllLoans.Location = new System.Drawing.Point(220, 96);
            this.AllLoans.Name = "AllLoans";
            this.AllLoans.Size = new System.Drawing.Size(142, 24);
            this.AllLoans.TabIndex = 2;
            this.AllLoans.Text = "Payoff All Loans";
            this.AllLoans.UseVisualStyleBackColor = true;
            this.AllLoans.CheckedChanged += new System.EventHandler(this.AllLoans_CheckedChanged);
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.Enabled = false;
            this.OK.Location = new System.Drawing.Point(421, 372);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 31);
            this.OK.TabIndex = 4;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // AccountIdentifier
            // 
            this.AccountIdentifier.AllowedSpecialCharacters = "";
            this.AccountIdentifier.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AccountIdentifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountIdentifier.Location = new System.Drawing.Point(220, 14);
            this.AccountIdentifier.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AccountIdentifier.MaxLength = 10;
            this.AccountIdentifier.Name = "AccountIdentifier";
            this.AccountIdentifier.Size = new System.Drawing.Size(106, 26);
            this.AccountIdentifier.TabIndex = 0;
            this.AccountIdentifier.TextChanged += new System.EventHandler(this.AccountIdentifier_TextChanged);
            // 
            // VersionNumber
            // 
            this.VersionNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VersionNumber.AutoSize = true;
            this.VersionNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionNumber.Location = new System.Drawing.Point(12, 387);
            this.VersionNumber.Name = "VersionNumber";
            this.VersionNumber.Size = new System.Drawing.Size(54, 16);
            this.VersionNumber.TabIndex = 5;
            this.VersionNumber.Text = "Version";
            // 
            // PayoffDate
            // 
            this.PayoffDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PayoffDate.Enabled = false;
            this.PayoffDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.PayoffDate.Location = new System.Drawing.Point(220, 59);
            this.PayoffDate.Name = "PayoffDate";
            this.PayoffDate.Size = new System.Drawing.Size(113, 26);
            this.PayoffDate.TabIndex = 1;
            // 
            // PayoffDataGrid
            // 
            this.PayoffDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PayoffDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.PayoffDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PayoffDataGrid.Enabled = false;
            this.PayoffDataGrid.Location = new System.Drawing.Point(12, 136);
            this.PayoffDataGrid.Name = "PayoffDataGrid";
            this.PayoffDataGrid.ReadOnly = true;
            this.PayoffDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PayoffDataGrid.Size = new System.Drawing.Size(484, 223);
            this.PayoffDataGrid.TabIndex = 3;
            // 
            // PayoffInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 415);
            this.Controls.Add(this.PayoffDate);
            this.Controls.Add(this.VersionNumber);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.PayoffDataGrid);
            this.Controls.Add(this.AllLoans);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AccountIdentifier);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(521, 453);
            this.Name = "PayoffInformation";
            this.Text = "Payoff Information";
            this.Shown += new System.EventHandler(this.PayoffInformation_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.PayoffDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.RequiredAccountNumberTextBox AccountIdentifier;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox AllLoans;
        private MyDataGridView PayoffDataGrid;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Label VersionNumber;
        private System.Windows.Forms.DateTimePicker PayoffDate;
    }
}