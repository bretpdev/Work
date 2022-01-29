namespace IDRUSERPRO
{
    partial class AccountEntry
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblAcctNum = new System.Windows.Forms.Label();
            this.cbMisroutedApp = new System.Windows.Forms.CheckBox();
            this.invalidDataToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.NewApplication = new System.Windows.Forms.Button();
            this.ExistingApplications = new System.Windows.Forms.DataGridView();
            this.Search = new System.Windows.Forms.Button();
            this.accountIdentifierText = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ExistingApplications)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(441, 317);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 37);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblAcctNum
            // 
            this.lblAcctNum.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblAcctNum.Location = new System.Drawing.Point(13, 9);
            this.lblAcctNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAcctNum.Name = "lblAcctNum";
            this.lblAcctNum.Size = new System.Drawing.Size(140, 20);
            this.lblAcctNum.TabIndex = 2;
            this.lblAcctNum.Text = "Account Identifier:";
            this.lblAcctNum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbMisroutedApp
            // 
            this.cbMisroutedApp.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbMisroutedApp.AutoSize = true;
            this.cbMisroutedApp.Location = new System.Drawing.Point(13, 324);
            this.cbMisroutedApp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbMisroutedApp.Name = "cbMisroutedApp";
            this.cbMisroutedApp.Size = new System.Drawing.Size(180, 24);
            this.cbMisroutedApp.TabIndex = 2;
            this.cbMisroutedApp.Text = "Misrouted Application";
            this.invalidDataToolTip.SetToolTip(this.cbMisroutedApp, "Misrouted Application is disabled when searching by account number.");
            this.cbMisroutedApp.UseVisualStyleBackColor = true;
            this.cbMisroutedApp.CheckedChanged += new System.EventHandler(this.cbMisroutedApp_CheckedChanged);
            // 
            // NewApplication
            // 
            this.NewApplication.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NewApplication.Enabled = false;
            this.NewApplication.Location = new System.Drawing.Point(526, 317);
            this.NewApplication.Name = "NewApplication";
            this.NewApplication.Size = new System.Drawing.Size(131, 37);
            this.NewApplication.TabIndex = 4;
            this.NewApplication.Text = "New Application";
            this.NewApplication.UseVisualStyleBackColor = true;
            this.NewApplication.Click += new System.EventHandler(this.NewApplication_Click);
            // 
            // ExistingApplications
            // 
            this.ExistingApplications.AllowUserToAddRows = false;
            this.ExistingApplications.AllowUserToDeleteRows = false;
            this.ExistingApplications.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.ExistingApplications.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ExistingApplications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ExistingApplications.Location = new System.Drawing.Point(13, 43);
            this.ExistingApplications.MultiSelect = false;
            this.ExistingApplications.Name = "ExistingApplications";
            this.ExistingApplications.ReadOnly = true;
            this.ExistingApplications.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.ExistingApplications.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ExistingApplications.Size = new System.Drawing.Size(644, 256);
            this.ExistingApplications.TabIndex = 4;
            this.ExistingApplications.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ExistingApplications_CellDoubleClick);
            // 
            // Search
            // 
            this.Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Search.Location = new System.Drawing.Point(331, 7);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(75, 26);
            this.Search.TabIndex = 1;
            this.Search.Text = "Search";
            this.Search.UseVisualStyleBackColor = true;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // accountIdentifierText
            // 
            this.accountIdentifierText.AccountNumber = null;
            this.accountIdentifierText.AllowedSpecialCharacters = "";
            this.accountIdentifierText.Location = new System.Drawing.Point(160, 6);
            this.accountIdentifierText.MaxLength = 10;
            this.accountIdentifierText.Name = "accountIdentifierText";
            this.accountIdentifierText.Size = new System.Drawing.Size(165, 26);
            this.accountIdentifierText.Ssn = null;
            this.accountIdentifierText.TabIndex = 0;
            this.accountIdentifierText.TextChanged += new System.EventHandler(this.accountIdentifierText_TextChanged);
            // 
            // AccountEntry
            // 
            this.AcceptButton = this.Search;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(669, 365);
            this.Controls.Add(this.accountIdentifierText);
            this.Controls.Add(this.Search);
            this.Controls.Add(this.ExistingApplications);
            this.Controls.Add(this.NewApplication);
            this.Controls.Add(this.cbMisroutedApp);
            this.Controls.Add(this.lblAcctNum);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(689, 408);
            this.Name = "AccountEntry";
            this.ShowIcon = false;
            this.Text = "Account Search";
            ((System.ComponentModel.ISupportInitialize)(this.ExistingApplications)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblAcctNum;
        private System.Windows.Forms.CheckBox cbMisroutedApp;
        private System.Windows.Forms.ToolTip invalidDataToolTip;
        private System.Windows.Forms.Button NewApplication;
        private System.Windows.Forms.DataGridView ExistingApplications;
        private System.Windows.Forms.Button Search;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox accountIdentifierText;
    }
}