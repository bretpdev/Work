namespace ALIGNPNOTE
{
    partial class ReconciliationForm
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
            this.Ok = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Information = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BorrowerView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.SSN = new Uheaa.Common.WinForms.WatermarkSsnTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.VersionNumber = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.BorrowerView)).BeginInit();
            this.SuspendLayout();
            // 
            // Ok
            // 
            this.Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ok.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Ok.Location = new System.Drawing.Point(593, 414);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(80, 31);
            this.Ok.TabIndex = 5;
            this.Ok.Text = "OK";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.Location = new System.Drawing.Point(593, 451);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(80, 31);
            this.Cancel.TabIndex = 6;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Information
            // 
            this.Information.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Information.AutoSize = true;
            this.Information.Location = new System.Drawing.Point(27, 419);
            this.Information.MaximumSize = new System.Drawing.Size(550, 80);
            this.Information.Name = "Information";
            this.Information.Size = new System.Drawing.Size(463, 20);
            this.Information.TabIndex = 10;
            this.Information.Text = "Access the Imagining system to look for the borrowers P-note(s).";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(28, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(644, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "This application is used by Document Services to reconcile ALIGN promissory notes" +
    ".";
            // 
            // BorrowerView
            // 
            this.BorrowerView.AllowUserToAddRows = false;
            this.BorrowerView.AllowUserToDeleteRows = false;
            this.BorrowerView.AllowUserToResizeColumns = false;
            this.BorrowerView.AllowUserToResizeRows = false;
            this.BorrowerView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.BorrowerView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.BorrowerView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.BorrowerView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BorrowerView.Location = new System.Drawing.Point(12, 90);
            this.BorrowerView.Name = "BorrowerView";
            this.BorrowerView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.BorrowerView.Size = new System.Drawing.Size(660, 312);
            this.BorrowerView.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(243, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "SSN:";
            // 
            // SSN
            // 
            this.SSN.AllowedSpecialCharacters = "";
            this.SSN.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SSN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.SSN.ForeColor = System.Drawing.SystemColors.WindowText;
            this.SSN.Location = new System.Drawing.Point(295, 55);
            this.SSN.MaxLength = 9;
            this.SSN.Name = "SSN";
            this.SSN.ReadOnly = true;
            this.SSN.Size = new System.Drawing.Size(154, 26);
            this.SSN.Ssn = null;
            this.SSN.TabIndex = 0;
            this.SSN.Text = " ";
            this.SSN.Watermark = "";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 456);
            this.label2.MaximumSize = new System.Drawing.Size(500, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(484, 40);
            this.label2.TabIndex = 14;
            this.label2.Text = " If the associated P-note(s) is/are found for the borrower, select the P-note fou" +
    "nd box.";
            // 
            // VersionNumber
            // 
            this.VersionNumber.AutoSize = true;
            this.VersionNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionNumber.Location = new System.Drawing.Point(590, 485);
            this.VersionNumber.Name = "VersionNumber";
            this.VersionNumber.Size = new System.Drawing.Size(35, 13);
            this.VersionNumber.TabIndex = 15;
            this.VersionNumber.Text = "label3";
            // 
            // ReconciliationForm
            // 
            this.AcceptButton = this.Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 506);
            this.Controls.Add(this.VersionNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BorrowerView);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Information);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.SSN);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(708, 544);
            this.Name = "ReconciliationForm";
            this.ShowIcon = false;
            this.Text = "Align P-Note Reconciliation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReconciliationForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.BorrowerView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.WatermarkSsnTextBox SSN;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label Information;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView BorrowerView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label VersionNumber;
    }
}