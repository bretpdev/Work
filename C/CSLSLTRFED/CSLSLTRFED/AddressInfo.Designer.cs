namespace CSLSLTRFED
{
    partial class AddressInfo
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
            this.rdoBwrsCurrentAddr = new System.Windows.Forms.RadioButton();
            this.rdbNotBwrCurrAddr = new System.Windows.Forms.RadioButton();
            this.rdbCoBwrsCurAddr = new System.Windows.Forms.RadioButton();
            this.rdbNotCoBwrsCurrAddr = new System.Windows.Forms.RadioButton();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.grpBorrower = new System.Windows.Forms.GroupBox();
            this.grpCoBorrower = new System.Windows.Forms.GroupBox();
            this.ECorrGroup = new System.Windows.Forms.GroupBox();
            this.ecorrMessage = new System.Windows.Forms.Label();
            this.grpBorrower.SuspendLayout();
            this.grpCoBorrower.SuspendLayout();
            this.ECorrGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdoBwrsCurrentAddr
            // 
            this.rdoBwrsCurrentAddr.AutoSize = true;
            this.rdoBwrsCurrentAddr.Location = new System.Drawing.Point(52, 29);
            this.rdoBwrsCurrentAddr.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdoBwrsCurrentAddr.Name = "rdoBwrsCurrentAddr";
            this.rdoBwrsCurrentAddr.Size = new System.Drawing.Size(222, 24);
            this.rdoBwrsCurrentAddr.TabIndex = 1;
            this.rdoBwrsCurrentAddr.Text = "Borrower\'s Current Address";
            this.rdoBwrsCurrentAddr.UseVisualStyleBackColor = true;
            this.rdoBwrsCurrentAddr.CheckedChanged += new System.EventHandler(this.rdoBwrsCurrentAddr_CheckedChanged);
            // 
            // rdbNotBwrCurrAddr
            // 
            this.rdbNotBwrCurrAddr.AutoSize = true;
            this.rdbNotBwrCurrAddr.Location = new System.Drawing.Point(52, 65);
            this.rdbNotBwrCurrAddr.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdbNotBwrCurrAddr.Name = "rdbNotBwrCurrAddr";
            this.rdbNotBwrCurrAddr.Size = new System.Drawing.Size(251, 24);
            this.rdbNotBwrCurrAddr.TabIndex = 2;
            this.rdbNotBwrCurrAddr.Text = "Not Borrower\'s Current Address";
            this.rdbNotBwrCurrAddr.UseVisualStyleBackColor = true;
            this.rdbNotBwrCurrAddr.CheckedChanged += new System.EventHandler(this.rdbNotBwrCurrAddr_CheckedChanged);
            // 
            // rdbCoBwrsCurAddr
            // 
            this.rdbCoBwrsCurAddr.AutoSize = true;
            this.rdbCoBwrsCurAddr.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rdbCoBwrsCurAddr.Location = new System.Drawing.Point(54, 40);
            this.rdbCoBwrsCurAddr.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdbCoBwrsCurAddr.Name = "rdbCoBwrsCurAddr";
            this.rdbCoBwrsCurAddr.Size = new System.Drawing.Size(247, 24);
            this.rdbCoBwrsCurAddr.TabIndex = 4;
            this.rdbCoBwrsCurAddr.Text = "Co-Borrower\'s Current Address";
            this.rdbCoBwrsCurAddr.UseVisualStyleBackColor = true;
            this.rdbCoBwrsCurAddr.CheckedChanged += new System.EventHandler(this.rdbCoBwrsCurAddr_CheckedChanged);
            // 
            // rdbNotCoBwrsCurrAddr
            // 
            this.rdbNotCoBwrsCurrAddr.AutoSize = true;
            this.rdbNotCoBwrsCurrAddr.Location = new System.Drawing.Point(52, 75);
            this.rdbNotCoBwrsCurrAddr.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdbNotCoBwrsCurrAddr.Name = "rdbNotCoBwrsCurrAddr";
            this.rdbNotCoBwrsCurrAddr.Size = new System.Drawing.Size(276, 24);
            this.rdbNotCoBwrsCurrAddr.TabIndex = 5;
            this.rdbNotCoBwrsCurrAddr.Text = "Not Co-Borrower\'s Current Address";
            this.rdbNotCoBwrsCurrAddr.UseVisualStyleBackColor = true;
            this.rdbNotCoBwrsCurrAddr.CheckedChanged += new System.EventHandler(this.rdbNotCoBwrsCurrAddr_CheckedChanged);
            // 
            // btnClear
            // 
            this.btnClear.Enabled = false;
            this.btnClear.Location = new System.Drawing.Point(13, 415);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(112, 35);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.Enabled = false;
            this.btnContinue.Location = new System.Drawing.Point(265, 415);
            this.btnContinue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(112, 35);
            this.btnContinue.TabIndex = 4;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // grpBorrower
            // 
            this.grpBorrower.Controls.Add(this.rdoBwrsCurrentAddr);
            this.grpBorrower.Controls.Add(this.rdbNotBwrCurrAddr);
            this.grpBorrower.Enabled = false;
            this.grpBorrower.Location = new System.Drawing.Point(13, 160);
            this.grpBorrower.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpBorrower.Name = "grpBorrower";
            this.grpBorrower.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpBorrower.Size = new System.Drawing.Size(364, 115);
            this.grpBorrower.TabIndex = 1;
            this.grpBorrower.TabStop = false;
            this.grpBorrower.Text = "Borrower\'s Address:";
            // 
            // grpCoBorrower
            // 
            this.grpCoBorrower.Controls.Add(this.rdbNotCoBwrsCurrAddr);
            this.grpCoBorrower.Controls.Add(this.rdbCoBwrsCurAddr);
            this.grpCoBorrower.Enabled = false;
            this.grpCoBorrower.Location = new System.Drawing.Point(13, 285);
            this.grpCoBorrower.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpCoBorrower.Name = "grpCoBorrower";
            this.grpCoBorrower.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpCoBorrower.Size = new System.Drawing.Size(364, 115);
            this.grpCoBorrower.TabIndex = 2;
            this.grpCoBorrower.TabStop = false;
            this.grpCoBorrower.Text = "Co-Borrowers Address:";
            // 
            // ECorrGroup
            // 
            this.ECorrGroup.Controls.Add(this.ecorrMessage);
            this.ECorrGroup.Enabled = false;
            this.ECorrGroup.Location = new System.Drawing.Point(13, 14);
            this.ECorrGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ECorrGroup.Name = "ECorrGroup";
            this.ECorrGroup.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ECorrGroup.Size = new System.Drawing.Size(364, 135);
            this.ECorrGroup.TabIndex = 0;
            this.ECorrGroup.TabStop = false;
            this.ECorrGroup.Text = "Ecorr Process";
            // 
            // ecorrMessage
            // 
            this.ecorrMessage.AutoSize = true;
            this.ecorrMessage.Location = new System.Drawing.Point(23, 30);
            this.ecorrMessage.MaximumSize = new System.Drawing.Size(330, 100);
            this.ecorrMessage.MinimumSize = new System.Drawing.Size(330, 100);
            this.ecorrMessage.Name = "ecorrMessage";
            this.ecorrMessage.Size = new System.Drawing.Size(330, 100);
            this.ecorrMessage.TabIndex = 0;
            this.ecorrMessage.Text = "All applicable recipients will be sent correspondence through the Ecorr process.";
            // 
            // AddressInfo
            // 
            this.AcceptButton = this.btnContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(395, 472);
            this.Controls.Add(this.ECorrGroup);
            this.Controls.Add(this.grpCoBorrower);
            this.Controls.Add(this.grpBorrower);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.btnClear);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(411, 511);
            this.MinimumSize = new System.Drawing.Size(411, 511);
            this.Name = "AddressInfo";
            this.ShowIcon = false;
            this.Text = "Address Information";
            this.grpBorrower.ResumeLayout(false);
            this.grpBorrower.PerformLayout();
            this.grpCoBorrower.ResumeLayout(false);
            this.grpCoBorrower.PerformLayout();
            this.ECorrGroup.ResumeLayout(false);
            this.ECorrGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoBwrsCurrentAddr;
        private System.Windows.Forms.RadioButton rdbNotBwrCurrAddr;
        private System.Windows.Forms.RadioButton rdbCoBwrsCurAddr;
        private System.Windows.Forms.RadioButton rdbNotCoBwrsCurrAddr;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.GroupBox grpBorrower;
        private System.Windows.Forms.GroupBox grpCoBorrower;
        private System.Windows.Forms.GroupBox ECorrGroup;
        private System.Windows.Forms.Label ecorrMessage;
    }
}