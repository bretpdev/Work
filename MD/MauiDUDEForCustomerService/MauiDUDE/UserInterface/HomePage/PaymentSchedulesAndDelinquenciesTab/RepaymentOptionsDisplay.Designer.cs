namespace MauiDUDE
{
    partial class RepaymentOptionsDisplay
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelMonths = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelRepayAmount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelBeginDate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Months:";
            // 
            // labelMonths
            // 
            this.labelMonths.AutoSize = true;
            this.labelMonths.Location = new System.Drawing.Point(96, 6);
            this.labelMonths.Name = "labelMonths";
            this.labelMonths.Size = new System.Drawing.Size(0, 13);
            this.labelMonths.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(266, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Repay/Amount:";
            // 
            // labelRepayAmount
            // 
            this.labelRepayAmount.AutoSize = true;
            this.labelRepayAmount.Location = new System.Drawing.Point(352, 6);
            this.labelRepayAmount.Name = "labelRepayAmount";
            this.labelRepayAmount.Size = new System.Drawing.Size(0, 13);
            this.labelRepayAmount.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(468, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Begin Date:";
            // 
            // labelBeginDate
            // 
            this.labelBeginDate.AutoSize = true;
            this.labelBeginDate.Location = new System.Drawing.Point(537, 6);
            this.labelBeginDate.Name = "labelBeginDate";
            this.labelBeginDate.Size = new System.Drawing.Size(0, 13);
            this.labelBeginDate.TabIndex = 5;
            // 
            // RepaymentOptionsDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelBeginDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelRepayAmount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelMonths);
            this.Controls.Add(this.label1);
            this.Name = "RepaymentOptionsDisplay";
            this.Size = new System.Drawing.Size(620, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelMonths;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelRepayAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelBeginDate;
    }
}
