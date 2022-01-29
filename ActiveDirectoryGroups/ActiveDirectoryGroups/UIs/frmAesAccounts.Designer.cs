namespace ActiveDirectoryGroups
{
    partial class frmAesAccounts
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
            this.AccountNumberTxt = new Uheaa.Common.WinForms.AccountNumberTextBox();
            this.RemoveBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.CurrentAccounts = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AddBtn = new Uheaa.Common.WinForms.ValidationButton();
            this.UndoBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AccountNumberTxt
            // 
            this.AccountNumberTxt.AllowedSpecialCharacters = "";
            this.AccountNumberTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountNumberTxt.Location = new System.Drawing.Point(13, 38);
            this.AccountNumberTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AccountNumberTxt.MaxLength = 10;
            this.AccountNumberTxt.Name = "AccountNumberTxt";
            this.AccountNumberTxt.Size = new System.Drawing.Size(148, 26);
            this.AccountNumberTxt.Ssn = null;
            this.AccountNumberTxt.TabIndex = 0;
            // 
            // RemoveBtn
            // 
            this.RemoveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveBtn.Location = new System.Drawing.Point(167, 166);
            this.RemoveBtn.Name = "RemoveBtn";
            this.RemoveBtn.Size = new System.Drawing.Size(87, 26);
            this.RemoveBtn.TabIndex = 2;
            this.RemoveBtn.Text = "Remove";
            this.RemoveBtn.UseVisualStyleBackColor = true;
            this.RemoveBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(13, 251);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(78, 26);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // CurrentAccounts
            // 
            this.CurrentAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentAccounts.FormattingEnabled = true;
            this.CurrentAccounts.ItemHeight = 20;
            this.CurrentAccounts.Location = new System.Drawing.Point(13, 121);
            this.CurrentAccounts.Name = "CurrentAccounts";
            this.CurrentAccounts.Size = new System.Drawing.Size(148, 124);
            this.CurrentAccounts.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "New Account";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Current Accounts";
            // 
            // AddBtn
            // 
            this.AddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddBtn.Location = new System.Drawing.Point(167, 38);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(56, 26);
            this.AddBtn.TabIndex = 7;
            this.AddBtn.Text = "Add";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // UndoBtn
            // 
            this.UndoBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UndoBtn.Enabled = false;
            this.UndoBtn.Location = new System.Drawing.Point(168, 219);
            this.UndoBtn.Name = "UndoBtn";
            this.UndoBtn.Size = new System.Drawing.Size(65, 26);
            this.UndoBtn.TabIndex = 8;
            this.UndoBtn.Text = "Undo";
            this.UndoBtn.UseVisualStyleBackColor = true;
            this.UndoBtn.Click += new System.EventHandler(this.UndoBtn_Click);
            // 
            // frmAesAccounts
            // 
            this.AcceptButton = this.AddBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(267, 289);
            this.Controls.Add(this.UndoBtn);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CurrentAccounts);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.RemoveBtn);
            this.Controls.Add(this.AccountNumberTxt);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(283, 328);
            this.Name = "frmAesAccounts";
            this.Text = "AesAccounts";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.AccountNumberTextBox AccountNumberTxt;
        private System.Windows.Forms.Button RemoveBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.ListBox CurrentAccounts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Uheaa.Common.WinForms.ValidationButton AddBtn;
        private System.Windows.Forms.Button UndoBtn;
    }
}