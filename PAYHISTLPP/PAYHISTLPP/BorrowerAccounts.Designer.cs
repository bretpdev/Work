
namespace PAYHISTLPP
{
    partial class BorrowerAccounts
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
            this.AddLabel = new System.Windows.Forms.Label();
            this.Add = new System.Windows.Forms.Button();
            this.SsnList = new System.Windows.Forms.ListBox();
            this.Process = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.AccountIdentifier = new System.Windows.Forms.TextBox();
            this.Manual = new System.Windows.Forms.Button();
            this.CountLabel = new System.Windows.Forms.Label();
            this.Count = new System.Windows.Forms.Label();
            this.Users = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Lender = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Tilp = new System.Windows.Forms.CheckBox();
            this.NumberToProcess = new Uheaa.Common.WinForms.NumericTextBox();
            this.VersionLbl = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.Upload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddLabel
            // 
            this.AddLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AddLabel.Enabled = false;
            this.AddLabel.Location = new System.Drawing.Point(14, 174);
            this.AddLabel.Name = "AddLabel";
            this.AddLabel.Size = new System.Drawing.Size(215, 65);
            this.AddLabel.TabIndex = 0;
            this.AddLabel.Text = "Add a single SSN/Acct #,  a comma delimited list or upload a file with accounts";
            this.AddLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Add
            // 
            this.Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Add.Enabled = false;
            this.Add.Location = new System.Drawing.Point(154, 271);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(72, 28);
            this.Add.TabIndex = 7;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // SsnList
            // 
            this.SsnList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SsnList.Enabled = false;
            this.SsnList.FormattingEnabled = true;
            this.SsnList.ItemHeight = 20;
            this.SsnList.Location = new System.Drawing.Point(17, 305);
            this.SsnList.Name = "SsnList";
            this.SsnList.Size = new System.Drawing.Size(209, 384);
            this.SsnList.TabIndex = 3;
            this.SsnList.SelectedValueChanged += new System.EventHandler(this.SsnList_SelectedValueChanged);
            // 
            // Process
            // 
            this.Process.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Process.Enabled = false;
            this.Process.Location = new System.Drawing.Point(142, 134);
            this.Process.Name = "Process";
            this.Process.Size = new System.Drawing.Size(87, 31);
            this.Process.TabIndex = 4;
            this.Process.Text = "Process";
            this.Process.UseVisualStyleBackColor = true;
            this.Process.Click += new System.EventHandler(this.Process_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(21, 717);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(87, 31);
            this.Cancel.TabIndex = 8;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // AccountIdentifier
            // 
            this.AccountIdentifier.Enabled = false;
            this.AccountIdentifier.Location = new System.Drawing.Point(17, 272);
            this.AccountIdentifier.MaxLength = 49999;
            this.AccountIdentifier.Name = "AccountIdentifier";
            this.AccountIdentifier.Size = new System.Drawing.Size(133, 26);
            this.AccountIdentifier.TabIndex = 6;
            this.AccountIdentifier.TextChanged += new System.EventHandler(this.AccountIdentifier_TextChanged);
            // 
            // Manual
            // 
            this.Manual.Location = new System.Drawing.Point(18, 134);
            this.Manual.Name = "Manual";
            this.Manual.Size = new System.Drawing.Size(87, 31);
            this.Manual.TabIndex = 5;
            this.Manual.Text = "Manual";
            this.Manual.UseVisualStyleBackColor = true;
            this.Manual.Click += new System.EventHandler(this.Manual_Click);
            // 
            // CountLabel
            // 
            this.CountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CountLabel.AutoSize = true;
            this.CountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CountLabel.Location = new System.Drawing.Point(152, 694);
            this.CountLabel.Name = "CountLabel";
            this.CountLabel.Size = new System.Drawing.Size(38, 13);
            this.CountLabel.TabIndex = 5;
            this.CountLabel.Text = "Count:";
            // 
            // Count
            // 
            this.Count.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Count.AutoSize = true;
            this.Count.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Count.Location = new System.Drawing.Point(197, 694);
            this.Count.Name = "Count";
            this.Count.Size = new System.Drawing.Size(35, 13);
            this.Count.TabIndex = 6;
            this.Count.Text = "label3";
            // 
            // Users
            // 
            this.Users.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Users.FormattingEnabled = true;
            this.Users.Location = new System.Drawing.Point(88, 6);
            this.Users.Name = "Users";
            this.Users.Size = new System.Drawing.Size(144, 28);
            this.Users.TabIndex = 0;
            this.Users.SelectedIndexChanged += new System.EventHandler(this.Users_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(17, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Run By:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "Lender:";
            // 
            // Lender
            // 
            this.Lender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Lender.FormattingEnabled = true;
            this.Lender.Location = new System.Drawing.Point(88, 40);
            this.Lender.Name = "Lender";
            this.Lender.Size = new System.Drawing.Size(144, 28);
            this.Lender.TabIndex = 1;
            this.Lender.SelectedIndexChanged += new System.EventHandler(this.Lender_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Amount to Process:";
            // 
            // Tilp
            // 
            this.Tilp.AutoSize = true;
            this.Tilp.Enabled = false;
            this.Tilp.Location = new System.Drawing.Point(84, 73);
            this.Tilp.Name = "Tilp";
            this.Tilp.Size = new System.Drawing.Size(87, 24);
            this.Tilp.TabIndex = 2;
            this.Tilp.Text = "Tilp Only";
            this.Tilp.UseVisualStyleBackColor = true;
            // 
            // NumberToProcess
            // 
            this.NumberToProcess.AllowedSpecialCharacters = "";
            this.NumberToProcess.Location = new System.Drawing.Point(171, 97);
            this.NumberToProcess.Name = "NumberToProcess";
            this.NumberToProcess.Size = new System.Drawing.Size(61, 26);
            this.NumberToProcess.TabIndex = 3;
            this.NumberToProcess.Text = "2500";
            // 
            // VersionLbl
            // 
            this.VersionLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VersionLbl.AutoSize = true;
            this.VersionLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionLbl.Location = new System.Drawing.Point(127, 735);
            this.VersionLbl.Name = "VersionLbl";
            this.VersionLbl.Size = new System.Drawing.Size(45, 13);
            this.VersionLbl.TabIndex = 12;
            this.VersionLbl.Text = "Version:";
            // 
            // Version
            // 
            this.Version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Version.AutoSize = true;
            this.Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Version.Location = new System.Drawing.Point(168, 735);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(0, 13);
            this.Version.TabIndex = 13;
            // 
            // Upload
            // 
            this.Upload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Upload.Enabled = false;
            this.Upload.Location = new System.Drawing.Point(88, 236);
            this.Upload.Name = "Upload";
            this.Upload.Size = new System.Drawing.Size(72, 28);
            this.Upload.TabIndex = 14;
            this.Upload.Text = "Upload";
            this.Upload.UseVisualStyleBackColor = true;
            this.Upload.Click += new System.EventHandler(this.Upload_Click);
            // 
            // BorrowerAccounts
            // 
            this.AcceptButton = this.Process;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(246, 760);
            this.Controls.Add(this.Upload);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.VersionLbl);
            this.Controls.Add(this.Tilp);
            this.Controls.Add(this.NumberToProcess);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Lender);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Users);
            this.Controls.Add(this.Count);
            this.Controls.Add(this.CountLabel);
            this.Controls.Add(this.Manual);
            this.Controls.Add(this.AccountIdentifier);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Process);
            this.Controls.Add(this.SsnList);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.AddLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(262, 799);
            this.Name = "BorrowerAccounts";
            this.Text = "Accounts";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AddLabel;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.ListBox SsnList;
        private System.Windows.Forms.Button Process;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TextBox AccountIdentifier;
        private System.Windows.Forms.Button Manual;
        private System.Windows.Forms.Label CountLabel;
        private System.Windows.Forms.Label Count;
        private System.Windows.Forms.ComboBox Users;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Lender;
        private System.Windows.Forms.Label label2;
        private Uheaa.Common.WinForms.NumericTextBox NumberToProcess;
        private System.Windows.Forms.CheckBox Tilp;
        private System.Windows.Forms.Label VersionLbl;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.Button Upload;
    }
}