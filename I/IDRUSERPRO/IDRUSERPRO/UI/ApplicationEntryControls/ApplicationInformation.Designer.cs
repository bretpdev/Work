namespace IDRUSERPRO
{
    partial class ApplicationInformation
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ApplicationDateBox = new System.Windows.Forms.MaskedTextBox();
            this.InactiveChk = new System.Windows.Forms.CheckBox();
            this.AwardIdTxt = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.AwardIdLbl = new System.Windows.Forms.Label();
            this.ApplicationIdTxt = new System.Windows.Forms.Label();
            this.ApplicationIdLabel = new System.Windows.Forms.Label();
            this.AccountNumberBox = new System.Windows.Forms.Label();
            this.CodIdTxt = new Uheaa.Common.WinForms.NumericTextBox();
            this.ApplicationDateLabel = new System.Windows.Forms.Label();
            this.CodLbl = new System.Windows.Forms.Label();
            this.AccountNumberLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ApplicationDateBox
            // 
            this.ApplicationDateBox.Location = new System.Drawing.Point(228, 134);
            this.ApplicationDateBox.Mask = "00/00/0000";
            this.ApplicationDateBox.Name = "ApplicationDateBox";
            this.ApplicationDateBox.Size = new System.Drawing.Size(90, 26);
            this.ApplicationDateBox.TabIndex = 1;
            this.ApplicationDateBox.ValidatingType = typeof(System.DateTime);
            this.ApplicationDateBox.TextChanged += new System.EventHandler(this.Controls_TextChanged);
            // 
            // InactiveChk
            // 
            this.InactiveChk.AutoSize = true;
            this.InactiveChk.Location = new System.Drawing.Point(16, 280);
            this.InactiveChk.Name = "InactiveChk";
            this.InactiveChk.Size = new System.Drawing.Size(83, 24);
            this.InactiveChk.TabIndex = 3;
            this.InactiveChk.Text = "Inactive";
            this.InactiveChk.UseVisualStyleBackColor = true;
            // 
            // AwardIdTxt
            // 
            this.AwardIdTxt.AllowedSpecialCharacters = "";
            this.AwardIdTxt.Location = new System.Drawing.Point(228, 186);
            this.AwardIdTxt.MaxLength = 21;
            this.AwardIdTxt.Name = "AwardIdTxt";
            this.AwardIdTxt.Size = new System.Drawing.Size(100, 26);
            this.AwardIdTxt.TabIndex = 2;
            this.AwardIdTxt.Visible = false;
            this.AwardIdTxt.TextChanged += new System.EventHandler(this.Controls_TextChanged);
            // 
            // AwardIdLbl
            // 
            this.AwardIdLbl.AutoSize = true;
            this.AwardIdLbl.Location = new System.Drawing.Point(133, 189);
            this.AwardIdLbl.Name = "AwardIdLbl";
            this.AwardIdLbl.Size = new System.Drawing.Size(79, 20);
            this.AwardIdLbl.TabIndex = 21;
            this.AwardIdLbl.Text = "Award ID:";
            this.AwardIdLbl.Visible = false;
            // 
            // ApplicationIdTxt
            // 
            this.ApplicationIdTxt.AutoSize = true;
            this.ApplicationIdTxt.Location = new System.Drawing.Point(224, 97);
            this.ApplicationIdTxt.Name = "ApplicationIdTxt";
            this.ApplicationIdTxt.Size = new System.Drawing.Size(0, 20);
            this.ApplicationIdTxt.TabIndex = 20;
            // 
            // ApplicationIdLabel
            // 
            this.ApplicationIdLabel.AutoSize = true;
            this.ApplicationIdLabel.Location = new System.Drawing.Point(100, 97);
            this.ApplicationIdLabel.Name = "ApplicationIdLabel";
            this.ApplicationIdLabel.Size = new System.Drawing.Size(112, 20);
            this.ApplicationIdLabel.TabIndex = 19;
            this.ApplicationIdLabel.Text = "Application ID:";
            // 
            // AccountNumberBox
            // 
            this.AccountNumberBox.AutoSize = true;
            this.AccountNumberBox.Location = new System.Drawing.Point(224, 9);
            this.AccountNumberBox.Name = "AccountNumberBox";
            this.AccountNumberBox.Size = new System.Drawing.Size(0, 20);
            this.AccountNumberBox.TabIndex = 18;
            // 
            // CodIdTxt
            // 
            this.CodIdTxt.AllowedSpecialCharacters = "";
            this.CodIdTxt.BackColor = System.Drawing.SystemColors.Window;
            this.CodIdTxt.Location = new System.Drawing.Point(228, 56);
            this.CodIdTxt.MaxLength = 8;
            this.CodIdTxt.Name = "CodIdTxt";
            this.CodIdTxt.Size = new System.Drawing.Size(100, 26);
            this.CodIdTxt.TabIndex = 0;
            this.CodIdTxt.TextChanged += new System.EventHandler(this.Controls_TextChanged);
            // 
            // ApplicationDateLabel
            // 
            this.ApplicationDateLabel.AutoSize = true;
            this.ApplicationDateLabel.Location = new System.Drawing.Point(12, 137);
            this.ApplicationDateLabel.Name = "ApplicationDateLabel";
            this.ApplicationDateLabel.Size = new System.Drawing.Size(200, 20);
            this.ApplicationDateLabel.TabIndex = 17;
            this.ApplicationDateLabel.Text = "Application Received Date:";
            // 
            // CodLbl
            // 
            this.CodLbl.AutoSize = true;
            this.CodLbl.Location = new System.Drawing.Point(149, 59);
            this.CodLbl.Name = "CodLbl";
            this.CodLbl.Size = new System.Drawing.Size(63, 20);
            this.CodLbl.TabIndex = 16;
            this.CodLbl.Text = "Cod ID:";
            // 
            // AccountNumberLabel
            // 
            this.AccountNumberLabel.AutoSize = true;
            this.AccountNumberLabel.Location = new System.Drawing.Point(80, 9);
            this.AccountNumberLabel.Name = "AccountNumberLabel";
            this.AccountNumberLabel.Size = new System.Drawing.Size(132, 20);
            this.AccountNumberLabel.TabIndex = 14;
            this.AccountNumberLabel.Text = "Account Number:";
            // 
            // ApplicationInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ApplicationDateBox);
            this.Controls.Add(this.InactiveChk);
            this.Controls.Add(this.AwardIdTxt);
            this.Controls.Add(this.AwardIdLbl);
            this.Controls.Add(this.ApplicationIdTxt);
            this.Controls.Add(this.ApplicationIdLabel);
            this.Controls.Add(this.AccountNumberBox);
            this.Controls.Add(this.CodIdTxt);
            this.Controls.Add(this.ApplicationDateLabel);
            this.Controls.Add(this.CodLbl);
            this.Controls.Add(this.AccountNumberLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ApplicationInformation";
            this.Size = new System.Drawing.Size(340, 312);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox ApplicationDateBox;
        private System.Windows.Forms.CheckBox InactiveChk;
        private Uheaa.Common.WinForms.AlphaNumericTextBox AwardIdTxt;
        private System.Windows.Forms.Label AwardIdLbl;
        private System.Windows.Forms.Label ApplicationIdTxt;
        private System.Windows.Forms.Label ApplicationIdLabel;
        private System.Windows.Forms.Label AccountNumberBox;
        private Uheaa.Common.WinForms.NumericTextBox CodIdTxt;
        private System.Windows.Forms.Label ApplicationDateLabel;
        private System.Windows.Forms.Label CodLbl;
        private System.Windows.Forms.Label AccountNumberLabel;
    }
}
