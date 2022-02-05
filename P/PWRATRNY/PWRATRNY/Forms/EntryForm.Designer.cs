namespace PWRATRNY
{
    partial class EntryForm
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
            this.AccountIdentifierLabel = new System.Windows.Forms.Label();
            this.ReferenceFirstLabel = new System.Windows.Forms.Label();
            this.ReferenceLastLabel = new System.Windows.Forms.Label();
            this.POAExpirationLabel = new System.Windows.Forms.Label();
            this.AccountIdentfierTextBox = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.ReferenceFirstTextBox = new Uheaa.Common.WinForms.AlphaTextBox();
            this.ReferenceLastTextBox = new Uheaa.Common.WinForms.AlphaTextBox();
            this.POAExpirationTextBox = new PWRATRNY.NonRequiredMaskedDateTextBox();
            this.SuspendLayout();
            // 
            // AccountIdentifierLabel
            // 
            this.AccountIdentifierLabel.AutoSize = true;
            this.AccountIdentifierLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.AccountIdentifierLabel.Location = new System.Drawing.Point(20, 20);
            this.AccountIdentifierLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AccountIdentifierLabel.Name = "AccountIdentifierLabel";
            this.AccountIdentifierLabel.Size = new System.Drawing.Size(134, 20);
            this.AccountIdentifierLabel.TabIndex = 6;
            this.AccountIdentifierLabel.Text = "Account Identifier";
            // 
            // ReferenceFirstLabel
            // 
            this.ReferenceFirstLabel.AutoSize = true;
            this.ReferenceFirstLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ReferenceFirstLabel.Location = new System.Drawing.Point(20, 60);
            this.ReferenceFirstLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ReferenceFirstLabel.Name = "ReferenceFirstLabel";
            this.ReferenceFirstLabel.Size = new System.Drawing.Size(119, 20);
            this.ReferenceFirstLabel.TabIndex = 7;
            this.ReferenceFirstLabel.Text = "Reference First";
            // 
            // ReferenceLastLabel
            // 
            this.ReferenceLastLabel.AutoSize = true;
            this.ReferenceLastLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ReferenceLastLabel.Location = new System.Drawing.Point(20, 100);
            this.ReferenceLastLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ReferenceLastLabel.Name = "ReferenceLastLabel";
            this.ReferenceLastLabel.Size = new System.Drawing.Size(119, 20);
            this.ReferenceLastLabel.TabIndex = 8;
            this.ReferenceLastLabel.Text = "Reference Last";
            // 
            // POAExpirationLabel
            // 
            this.POAExpirationLabel.AutoSize = true;
            this.POAExpirationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.POAExpirationLabel.Location = new System.Drawing.Point(20, 140);
            this.POAExpirationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.POAExpirationLabel.Name = "POAExpirationLabel";
            this.POAExpirationLabel.Size = new System.Drawing.Size(116, 20);
            this.POAExpirationLabel.TabIndex = 9;
            this.POAExpirationLabel.Text = "POA Expiration";
            // 
            // AccountIdentfierTextBox
            // 
            this.AccountIdentfierTextBox.AccountNumber = null;
            this.AccountIdentfierTextBox.AllowedSpecialCharacters = "";
            this.AccountIdentfierTextBox.Location = new System.Drawing.Point(168, 20);
            this.AccountIdentfierTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AccountIdentfierTextBox.MaxLength = 10;
            this.AccountIdentfierTextBox.Name = "AccountIdentfierTextBox";
            this.AccountIdentfierTextBox.Size = new System.Drawing.Size(148, 26);
            this.AccountIdentfierTextBox.Ssn = null;
            this.AccountIdentfierTextBox.TabIndex = 0;
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.OK.Location = new System.Drawing.Point(237, 182);
            this.OK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(112, 40);
            this.OK.TabIndex = 5;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Cancel.Location = new System.Drawing.Point(28, 182);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(112, 40);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // ReferenceFirstTextBox
            // 
            this.ReferenceFirstTextBox.AllowedSpecialCharacters = "";
            this.ReferenceFirstTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReferenceFirstTextBox.Location = new System.Drawing.Point(168, 58);
            this.ReferenceFirstTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ReferenceFirstTextBox.Name = "ReferenceFirstTextBox";
            this.ReferenceFirstTextBox.Size = new System.Drawing.Size(163, 26);
            this.ReferenceFirstTextBox.TabIndex = 1;
            // 
            // ReferenceLastTextBox
            // 
            this.ReferenceLastTextBox.AllowedSpecialCharacters = "";
            this.ReferenceLastTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReferenceLastTextBox.Location = new System.Drawing.Point(168, 98);
            this.ReferenceLastTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ReferenceLastTextBox.Name = "ReferenceLastTextBox";
            this.ReferenceLastTextBox.Size = new System.Drawing.Size(163, 26);
            this.ReferenceLastTextBox.TabIndex = 2;
            // 
            // POAExpirationTextBox
            // 
            this.POAExpirationTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.POAExpirationTextBox.Location = new System.Drawing.Point(168, 138);
            this.POAExpirationTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.POAExpirationTextBox.Mask = "00/00/0000";
            this.POAExpirationTextBox.Name = "POAExpirationTextBox";
            this.POAExpirationTextBox.Size = new System.Drawing.Size(90, 26);
            this.POAExpirationTextBox.TabIndex = 3;
            // 
            // EntryForm
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(362, 230);
            this.Controls.Add(this.POAExpirationTextBox);
            this.Controls.Add(this.ReferenceLastTextBox);
            this.Controls.Add(this.ReferenceFirstTextBox);
            this.Controls.Add(this.AccountIdentfierTextBox);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.POAExpirationLabel);
            this.Controls.Add(this.ReferenceLastLabel);
            this.Controls.Add(this.ReferenceFirstLabel);
            this.Controls.Add(this.AccountIdentifierLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(378, 269);
            this.Name = "EntryForm";
            this.Text = "Power Of Attorney";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AccountIdentifierLabel;
        private System.Windows.Forms.Label ReferenceFirstLabel;
        private System.Windows.Forms.Label ReferenceLastLabel;
        private System.Windows.Forms.Label POAExpirationLabel;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox AccountIdentfierTextBox;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private Uheaa.Common.WinForms.AlphaTextBox ReferenceFirstTextBox;
        private Uheaa.Common.WinForms.AlphaTextBox ReferenceLastTextBox;
        private NonRequiredMaskedDateTextBox POAExpirationTextBox;
    }
}