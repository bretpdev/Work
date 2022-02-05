namespace IDRUSERPRO
{
    partial class PaystubControl
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
            this.FtwLabel = new System.Windows.Forms.Label();
            this.GrossLabel = new System.Windows.Forms.Label();
            this.DeductionsLabel = new System.Windows.Forms.Label();
            this.BonusLabel = new System.Windows.Forms.Label();
            this.OvertimeLabel = new System.Windows.Forms.Label();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.OvertimeBox = new Uheaa.Common.WinForms.NumericDecimalTextBox();
            this.BonusBox = new Uheaa.Common.WinForms.NumericDecimalTextBox();
            this.DeductionsBox = new Uheaa.Common.WinForms.NumericDecimalTextBox();
            this.GrossBox = new Uheaa.Common.WinForms.NumericDecimalTextBox();
            this.FtwBox = new Uheaa.Common.WinForms.NumericDecimalTextBox();
            this.AgiBox = new Uheaa.Common.WinForms.NumericDecimalTextBox();
            this.SuspendLayout();
            // 
            // FtwLabel
            // 
            this.FtwLabel.AutoSize = true;
            this.FtwLabel.Location = new System.Drawing.Point(118, -3);
            this.FtwLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FtwLabel.Name = "FtwLabel";
            this.FtwLabel.Size = new System.Drawing.Size(43, 20);
            this.FtwLabel.TabIndex = 0;
            this.FtwLabel.Text = "FTW";
            // 
            // GrossLabel
            // 
            this.GrossLabel.AutoSize = true;
            this.GrossLabel.Location = new System.Drawing.Point(224, -3);
            this.GrossLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GrossLabel.Name = "GrossLabel";
            this.GrossLabel.Size = new System.Drawing.Size(52, 20);
            this.GrossLabel.TabIndex = 2;
            this.GrossLabel.Text = "Gross";
            // 
            // DeductionsLabel
            // 
            this.DeductionsLabel.AutoSize = true;
            this.DeductionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeductionsLabel.Location = new System.Drawing.Point(331, 2);
            this.DeductionsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DeductionsLabel.Name = "DeductionsLabel";
            this.DeductionsLabel.Size = new System.Drawing.Size(101, 13);
            this.DeductionsLabel.TabIndex = 4;
            this.DeductionsLabel.Text = "Pre-Tax Deductions";
            // 
            // BonusLabel
            // 
            this.BonusLabel.AutoSize = true;
            this.BonusLabel.Location = new System.Drawing.Point(436, -3);
            this.BonusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BonusLabel.Name = "BonusLabel";
            this.BonusLabel.Size = new System.Drawing.Size(55, 20);
            this.BonusLabel.TabIndex = 6;
            this.BonusLabel.Text = "Bonus";
            // 
            // OvertimeLabel
            // 
            this.OvertimeLabel.AutoSize = true;
            this.OvertimeLabel.Location = new System.Drawing.Point(542, -3);
            this.OvertimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OvertimeLabel.Name = "OvertimeLabel";
            this.OvertimeLabel.Size = new System.Drawing.Size(72, 20);
            this.OvertimeLabel.TabIndex = 8;
            this.OvertimeLabel.Text = "Overtime";
            // 
            // RemoveButton
            // 
            this.RemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveButton.Location = new System.Drawing.Point(651, 17);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(80, 28);
            this.RemoveButton.TabIndex = 6;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, -3);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 20);
            this.label7.TabIndex = 13;
            this.label7.Text = "AGI";
            // 
            // OvertimeBox
            // 
            this.OvertimeBox.AllowedSpecialCharacters = "";
            this.OvertimeBox.Location = new System.Drawing.Point(546, 17);
            this.OvertimeBox.Name = "OvertimeBox";
            this.OvertimeBox.Size = new System.Drawing.Size(100, 26);
            this.OvertimeBox.TabIndex = 5;
            this.OvertimeBox.TextChanged += new System.EventHandler(this.Control_TextChanged);
            this.OvertimeBox.Enter += new System.EventHandler(this.Control_Enter);
            this.OvertimeBox.Leave += new System.EventHandler(this.Control_Leave);
            // 
            // BonusBox
            // 
            this.BonusBox.AllowedSpecialCharacters = "";
            this.BonusBox.Location = new System.Drawing.Point(440, 17);
            this.BonusBox.Name = "BonusBox";
            this.BonusBox.Size = new System.Drawing.Size(100, 26);
            this.BonusBox.TabIndex = 4;
            this.BonusBox.TextChanged += new System.EventHandler(this.Control_TextChanged);
            this.BonusBox.Enter += new System.EventHandler(this.Control_Enter);
            this.BonusBox.Leave += new System.EventHandler(this.Control_Leave);
            // 
            // DeductionsBox
            // 
            this.DeductionsBox.AllowedSpecialCharacters = "";
            this.DeductionsBox.Location = new System.Drawing.Point(334, 17);
            this.DeductionsBox.Name = "DeductionsBox";
            this.DeductionsBox.Size = new System.Drawing.Size(100, 26);
            this.DeductionsBox.TabIndex = 3;
            this.DeductionsBox.TextChanged += new System.EventHandler(this.Control_TextChanged);
            this.DeductionsBox.Enter += new System.EventHandler(this.Control_Enter);
            this.DeductionsBox.Leave += new System.EventHandler(this.Control_Leave);
            // 
            // GrossBox
            // 
            this.GrossBox.AllowedSpecialCharacters = "";
            this.GrossBox.Location = new System.Drawing.Point(228, 18);
            this.GrossBox.Name = "GrossBox";
            this.GrossBox.Size = new System.Drawing.Size(100, 26);
            this.GrossBox.TabIndex = 2;
            this.GrossBox.TextChanged += new System.EventHandler(this.Control_TextChanged);
            this.GrossBox.Enter += new System.EventHandler(this.Control_Enter);
            this.GrossBox.Leave += new System.EventHandler(this.Control_Leave);
            // 
            // FtwBox
            // 
            this.FtwBox.AllowedSpecialCharacters = "";
            this.FtwBox.Location = new System.Drawing.Point(122, 19);
            this.FtwBox.Name = "FtwBox";
            this.FtwBox.Size = new System.Drawing.Size(100, 26);
            this.FtwBox.TabIndex = 1;
            this.FtwBox.TextChanged += new System.EventHandler(this.Control_TextChanged);
            this.FtwBox.Enter += new System.EventHandler(this.Control_Enter);
            this.FtwBox.Leave += new System.EventHandler(this.Control_Leave);
            // 
            // AgiBox
            // 
            this.AgiBox.AllowedSpecialCharacters = "";
            this.AgiBox.Location = new System.Drawing.Point(16, 20);
            this.AgiBox.Name = "AgiBox";
            this.AgiBox.ReadOnly = true;
            this.AgiBox.Size = new System.Drawing.Size(100, 26);
            this.AgiBox.TabIndex = 0;
            this.AgiBox.TabStop = false;
            // 
            // PaystubControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.OvertimeBox);
            this.Controls.Add(this.BonusBox);
            this.Controls.Add(this.DeductionsBox);
            this.Controls.Add(this.GrossBox);
            this.Controls.Add(this.FtwBox);
            this.Controls.Add(this.AgiBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.OvertimeLabel);
            this.Controls.Add(this.BonusLabel);
            this.Controls.Add(this.DeductionsLabel);
            this.Controls.Add(this.GrossLabel);
            this.Controls.Add(this.FtwLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PaystubControl";
            this.Size = new System.Drawing.Size(734, 57);
            this.Leave += new System.EventHandler(this.PaystubControl_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FtwLabel;
        private System.Windows.Forms.Label GrossLabel;
        private System.Windows.Forms.Label DeductionsLabel;
        private System.Windows.Forms.Label BonusLabel;
        private System.Windows.Forms.Label OvertimeLabel;
        private System.Windows.Forms.Button RemoveButton;
        private Uheaa.Common.WinForms.NumericDecimalTextBox AgiBox;
        private System.Windows.Forms.Label label7;
        private Uheaa.Common.WinForms.NumericDecimalTextBox FtwBox;
        private Uheaa.Common.WinForms.NumericDecimalTextBox GrossBox;
        private Uheaa.Common.WinForms.NumericDecimalTextBox DeductionsBox;
        private Uheaa.Common.WinForms.NumericDecimalTextBox BonusBox;
        private Uheaa.Common.WinForms.NumericDecimalTextBox OvertimeBox;
    }
}
