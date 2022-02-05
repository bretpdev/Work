namespace IDRUSERPRO
{
    partial class AnnivesaryDate
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
            this.ADate = new Uheaa.Common.WinForms.MaskedDateTextBox();
            this.OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(294, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the borrowers Anniversary Date:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ADate
            // 
            this.ADate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ADate.Location = new System.Drawing.Point(61, 35);
            this.ADate.Mask = "00/00/0000";
            this.ADate.Name = "ADate";
            this.ADate.Size = new System.Drawing.Size(87, 26);
            this.ADate.TabIndex = 1;
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(186, 33);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(67, 32);
            this.OK.TabIndex = 2;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.button1_Click);
            // 
            // AnniverDate
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 84);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.ADate);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(334, 123);
            this.Name = "AnniverDate";
            this.ShowIcon = false;
            this.Text = "Anniversary Date";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Uheaa.Common.WinForms.MaskedDateTextBox ADate;
        private System.Windows.Forms.Button OK;
    }
}