namespace PAYOFF
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
            this.accountIdentifierTextBox = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.dateTimePickerPayoffDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxPayoffAll = new System.Windows.Forms.CheckBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.dataGridViewIgnoreHeader = new PAYOFF.DataGridViewIgnoreHeader();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIgnoreHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // accountIdentifierTextBox
            // 
            this.accountIdentifierTextBox.AccountNumber = null;
            this.accountIdentifierTextBox.AllowedSpecialCharacters = "";
            this.accountIdentifierTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.accountIdentifierTextBox.Location = new System.Drawing.Point(223, 12);
            this.accountIdentifierTextBox.MaxLength = 10;
            this.accountIdentifierTextBox.Name = "accountIdentifierTextBox";
            this.accountIdentifierTextBox.Size = new System.Drawing.Size(115, 26);
            this.accountIdentifierTextBox.Ssn = null;
            this.accountIdentifierTextBox.TabIndex = 0;
            this.accountIdentifierTextBox.TextChanged += new System.EventHandler(this.accountIdentifierTextBox_TextChanged);
            // 
            // dateTimePickerPayoffDate
            // 
            this.dateTimePickerPayoffDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dateTimePickerPayoffDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerPayoffDate.Location = new System.Drawing.Point(223, 44);
            this.dateTimePickerPayoffDate.Name = "dateTimePickerPayoffDate";
            this.dateTimePickerPayoffDate.Size = new System.Drawing.Size(115, 26);
            this.dateTimePickerPayoffDate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(44, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Account Number / SSN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(124, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Payoff Date";
            // 
            // checkBoxPayoffAll
            // 
            this.checkBoxPayoffAll.AutoSize = true;
            this.checkBoxPayoffAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBoxPayoffAll.Location = new System.Drawing.Point(223, 77);
            this.checkBoxPayoffAll.Name = "checkBoxPayoffAll";
            this.checkBoxPayoffAll.Size = new System.Drawing.Size(142, 24);
            this.checkBoxPayoffAll.TabIndex = 4;
            this.checkBoxPayoffAll.Text = "Payoff All Loans";
            this.checkBoxPayoffAll.UseVisualStyleBackColor = true;
            this.checkBoxPayoffAll.CheckedChanged += new System.EventHandler(this.checkBoxPayoffAll_CheckedChanged);
            // 
            // labelVersion
            // 
            this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(12, 408);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(42, 13);
            this.labelVersion.TabIndex = 6;
            this.labelVersion.Text = "Version";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonOK.Location = new System.Drawing.Point(459, 392);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 29);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // dataGridViewIgnoreHeader
            // 
            this.dataGridViewIgnoreHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewIgnoreHeader.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridViewIgnoreHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewIgnoreHeader.Enabled = false;
            this.dataGridViewIgnoreHeader.Location = new System.Drawing.Point(12, 107);
            this.dataGridViewIgnoreHeader.Name = "dataGridViewIgnoreHeader";
            this.dataGridViewIgnoreHeader.ReadOnly = true;
            this.dataGridViewIgnoreHeader.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewIgnoreHeader.Size = new System.Drawing.Size(531, 279);
            this.dataGridViewIgnoreHeader.TabIndex = 5;
            // 
            // PayoffInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 430);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.dataGridViewIgnoreHeader);
            this.Controls.Add(this.checkBoxPayoffAll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePickerPayoffDate);
            this.Controls.Add(this.accountIdentifierTextBox);
            this.Name = "PayoffInformation";
            this.Text = "Payoff Information";
            this.Shown += new System.EventHandler(this.PayoffInformation_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIgnoreHeader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.AccountIdentifierTextBox accountIdentifierTextBox;
        private System.Windows.Forms.DateTimePicker dateTimePickerPayoffDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxPayoffAll;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Button buttonOK;
        private PAYOFF.DataGridViewIgnoreHeader dataGridViewIgnoreHeader;
    }
}