namespace DPALETTERS
{ 
    partial class DrawDateInput
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
            this.maskedDateTextBox = new Uheaa.Common.WinForms.MaskedDateTextBox();
            this.label = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // maskedDateTextBox
            // 
            this.maskedDateTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.maskedDateTextBox.Location = new System.Drawing.Point(105, 57);
            this.maskedDateTextBox.Mask = "00/00/0000";
            this.maskedDateTextBox.Name = "maskedDateTextBox";
            this.maskedDateTextBox.Size = new System.Drawing.Size(90, 26);
            this.maskedDateTextBox.TabIndex = 0;
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point(12, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(278, 45);
            this.label.TabIndex = 1;
            this.label.Text = "Enter the first draw date as either the 7th, 15th, or the 22nd in MM/DD/YYYY form" +
    "at.";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.Location = new System.Drawing.Point(15, 93);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(215, 93);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // DrawDateInput
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 128);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label);
            this.Controls.Add(this.maskedDateTextBox);
            this.MaximumSize = new System.Drawing.Size(318, 167);
            this.MinimumSize = new System.Drawing.Size(318, 167);
            this.Name = "DrawDateInput";
            this.Text = "Draw Date";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.MaskedDateTextBox maskedDateTextBox;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}