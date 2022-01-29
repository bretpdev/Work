namespace PAYARRREHB
{
    partial class ArrangementForm
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
            this.Generate = new System.Windows.Forms.RadioButton();
            this.Setup = new System.Windows.Forms.RadioButton();
            this.SSN = new Uheaa.Common.WinForms.WatermarkAccountIdentifierTextBox();
            this.Verify = new System.Windows.Forms.Button();
            this.TypeSelection = new System.Windows.Forms.GroupBox();
            this.ArrangementData = new System.Windows.Forms.GroupBox();
            this.Amount = new Uheaa.Common.WinForms.NumericTextBox();
            this.DueDate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Process = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.TypeSelection.SuspendLayout();
            this.ArrangementData.SuspendLayout();
            this.SuspendLayout();
            // 
            // Generate
            // 
            this.Generate.AutoSize = true;
            this.Generate.Location = new System.Drawing.Point(6, 25);
            this.Generate.Name = "Generate";
            this.Generate.Size = new System.Drawing.Size(230, 24);
            this.Generate.TabIndex = 0;
            this.Generate.TabStop = true;
            this.Generate.Text = "Generate Rehab Agreement";
            this.Generate.UseVisualStyleBackColor = true;
            this.Generate.CheckedChanged += new System.EventHandler(this.Generate_CheckedChanged);
            // 
            // Setup
            // 
            this.Setup.AutoSize = true;
            this.Setup.Location = new System.Drawing.Point(6, 55);
            this.Setup.Name = "Setup";
            this.Setup.Size = new System.Drawing.Size(167, 24);
            this.Setup.TabIndex = 1;
            this.Setup.TabStop = true;
            this.Setup.Text = "Setup Arrangement";
            this.Setup.UseVisualStyleBackColor = true;
            this.Setup.CheckedChanged += new System.EventHandler(this.Setup_CheckedChanged);
            // 
            // SSN
            // 
            this.SSN.AccountNumber = null;
            this.SSN.AllowedSpecialCharacters = "";
            this.SSN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.SSN.ForeColor = System.Drawing.SystemColors.WindowText;
            this.SSN.Location = new System.Drawing.Point(28, 39);
            this.SSN.MaxLength = 10;
            this.SSN.Name = "SSN";
            this.SSN.Size = new System.Drawing.Size(165, 26);
            this.SSN.Ssn = null;
            this.SSN.TabIndex = 1;
            this.SSN.Text = "SSN / Acct #";
            this.SSN.Watermark = "SSN / Acct #";
            this.SSN.TextChanged += new System.EventHandler(this.SSN_TextChanged);
            this.SSN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SSN_KeyPress);
            // 
            // Verify
            // 
            this.Verify.Enabled = false;
            this.Verify.Location = new System.Drawing.Point(210, 36);
            this.Verify.Name = "Verify";
            this.Verify.Size = new System.Drawing.Size(82, 30);
            this.Verify.TabIndex = 2;
            this.Verify.Text = "Verify";
            this.Verify.UseVisualStyleBackColor = true;
            this.Verify.Click += new System.EventHandler(this.Verify_Click);
            // 
            // TypeSelection
            // 
            this.TypeSelection.Controls.Add(this.Generate);
            this.TypeSelection.Controls.Add(this.Setup);
            this.TypeSelection.Enabled = false;
            this.TypeSelection.Location = new System.Drawing.Point(28, 81);
            this.TypeSelection.Name = "TypeSelection";
            this.TypeSelection.Size = new System.Drawing.Size(264, 102);
            this.TypeSelection.TabIndex = 3;
            this.TypeSelection.TabStop = false;
            this.TypeSelection.Text = "Arrangement Type";
            // 
            // ArrangementData
            // 
            this.ArrangementData.Controls.Add(this.Amount);
            this.ArrangementData.Controls.Add(this.DueDate);
            this.ArrangementData.Controls.Add(this.label2);
            this.ArrangementData.Controls.Add(this.label1);
            this.ArrangementData.Controls.Add(this.Process);
            this.ArrangementData.Enabled = false;
            this.ArrangementData.Location = new System.Drawing.Point(28, 189);
            this.ArrangementData.Name = "ArrangementData";
            this.ArrangementData.Size = new System.Drawing.Size(264, 191);
            this.ArrangementData.TabIndex = 5;
            this.ArrangementData.TabStop = false;
            this.ArrangementData.Text = "Arrangement Data";
            // 
            // Amount
            // 
            this.Amount.AllowedSpecialCharacters = ".";
            this.Amount.Location = new System.Drawing.Point(16, 55);
            this.Amount.MaxLength = 11;
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(100, 26);
            this.Amount.TabIndex = 0;
            // 
            // DueDate
            // 
            this.DueDate.DropDownHeight = 200;
            this.DueDate.DropDownWidth = 250;
            this.DueDate.FormattingEnabled = true;
            this.DueDate.IntegralHeight = false;
            this.DueDate.Location = new System.Drawing.Point(16, 115);
            this.DueDate.Name = "DueDate";
            this.DueDate.Size = new System.Drawing.Size(220, 28);
            this.DueDate.TabIndex = 1;
            this.DueDate.SelectedIndexChanged += new System.EventHandler(this.DueDate_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "First Due Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Monthly Payment Amount";
            // 
            // Process
            // 
            this.Process.Location = new System.Drawing.Point(88, 152);
            this.Process.Name = "Process";
            this.Process.Size = new System.Drawing.Size(82, 30);
            this.Process.TabIndex = 2;
            this.Process.Text = "Process";
            this.Process.UseVisualStyleBackColor = true;
            this.Process.Click += new System.EventHandler(this.Process_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(210, 386);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(82, 30);
            this.Cancel.TabIndex = 6;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "SSN / Account Number";
            // 
            // ArrangementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(313, 429);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.ArrangementData);
            this.Controls.Add(this.TypeSelection);
            this.Controls.Add(this.Verify);
            this.Controls.Add(this.SSN);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(330, 630);
            this.Name = "ArrangementForm";
            this.Text = "Payment Arrangement for Rehab";
            this.TypeSelection.ResumeLayout(false);
            this.TypeSelection.PerformLayout();
            this.ArrangementData.ResumeLayout(false);
            this.ArrangementData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton Generate;
        private System.Windows.Forms.RadioButton Setup;
        private Uheaa.Common.WinForms.WatermarkAccountIdentifierTextBox SSN;
        private System.Windows.Forms.Button Verify;
        private System.Windows.Forms.GroupBox TypeSelection;
        private System.Windows.Forms.GroupBox ArrangementData;
        private System.Windows.Forms.ComboBox DueDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Process;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label3;
        private Uheaa.Common.WinForms.NumericTextBox Amount;
    }
}