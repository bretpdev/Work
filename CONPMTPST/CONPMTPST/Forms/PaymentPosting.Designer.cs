namespace CONPMTPST
{
    partial class PaymentPosting
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
            this.label3 = new System.Windows.Forms.Label();
            this.Source = new System.Windows.Forms.ComboBox();
            this.Cash = new System.Windows.Forms.RadioButton();
            this.Wire = new System.Windows.Forms.RadioButton();
            this.Cancel = new System.Windows.Forms.Button();
            this.Ok = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Setup = new System.Windows.Forms.ToolStripMenuItem();
            this.PaymentTypeSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.PaymentSourceSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.Amount = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total Amount Received";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(165, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Payment Type";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(244, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Source of Consolidation Payment";
            // 
            // Source
            // 
            this.Source.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Source.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Source.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.Source.FormattingEnabled = true;
            this.Source.Location = new System.Drawing.Point(280, 115);
            this.Source.Name = "Source";
            this.Source.Size = new System.Drawing.Size(162, 28);
            this.Source.TabIndex = 3;
            // 
            // Cash
            // 
            this.Cash.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Cash.AutoSize = true;
            this.Cash.Location = new System.Drawing.Point(284, 81);
            this.Cash.Name = "Cash";
            this.Cash.Size = new System.Drawing.Size(64, 24);
            this.Cash.TabIndex = 1;
            this.Cash.Text = "Cash";
            this.Cash.UseVisualStyleBackColor = true;
            // 
            // Wire
            // 
            this.Wire.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Wire.AutoSize = true;
            this.Wire.Checked = true;
            this.Wire.Location = new System.Drawing.Point(354, 81);
            this.Wire.Name = "Wire";
            this.Wire.Size = new System.Drawing.Size(59, 24);
            this.Wire.TabIndex = 2;
            this.Wire.TabStop = true;
            this.Wire.Text = "Wire";
            this.Wire.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(34, 155);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(100, 37);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Ok
            // 
            this.Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ok.Location = new System.Drawing.Point(342, 155);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(100, 37);
            this.Ok.TabIndex = 4;
            this.Ok.Text = "OK";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Setup});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(473, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Setup
            // 
            this.Setup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PaymentTypeSetup,
            this.PaymentSourceSetup});
            this.Setup.Name = "Setup";
            this.Setup.Size = new System.Drawing.Size(49, 20);
            this.Setup.Text = "Setup";
            // 
            // PaymentTypeSetup
            // 
            this.PaymentTypeSetup.Name = "PaymentTypeSetup";
            this.PaymentTypeSetup.Size = new System.Drawing.Size(160, 22);
            this.PaymentTypeSetup.Text = "Payment Type";
            this.PaymentTypeSetup.Click += new System.EventHandler(this.PaymentTypeSetup_Click);
            // 
            // PaymentSourceSetup
            // 
            this.PaymentSourceSetup.Name = "PaymentSourceSetup";
            this.PaymentSourceSetup.Size = new System.Drawing.Size(160, 22);
            this.PaymentSourceSetup.Text = "Payment Source";
            this.PaymentSourceSetup.Click += new System.EventHandler(this.PaymentSourceSetup_Click);
            // 
            // Amount
            // 
            this.Amount.AllowedSpecialCharacters = ".";
            this.Amount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Amount.Location = new System.Drawing.Point(280, 36);
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(133, 26);
            this.Amount.TabIndex = 7;
            // 
            // PaymentPosting
            // 
            this.AcceptButton = this.Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(473, 207);
            this.Controls.Add(this.Amount);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Wire);
            this.Controls.Add(this.Cash);
            this.Controls.Add(this.Source);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(700, 246);
            this.MinimumSize = new System.Drawing.Size(489, 246);
            this.Name = "PaymentPosting";
            this.Text = "LPP Consolidation Payment Posting";
            this.TopMost = true;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Source;
        private System.Windows.Forms.RadioButton Cash;
        private System.Windows.Forms.RadioButton Wire;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Setup;
        private System.Windows.Forms.ToolStripMenuItem PaymentTypeSetup;
        private System.Windows.Forms.ToolStripMenuItem PaymentSourceSetup;
        private Uheaa.Common.WinForms.RequiredNumericTextBox Amount;

    }
}