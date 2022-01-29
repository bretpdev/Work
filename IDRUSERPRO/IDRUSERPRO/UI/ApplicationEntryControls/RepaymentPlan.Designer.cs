namespace IDRUSERPRO
{
    partial class RepaymentPlan
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
            this.MonthlyInstallmentLabel = new System.Windows.Forms.Label();
            this.PlanTitleLink = new System.Windows.Forms.LinkLabel();
            this.LoansBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MonthlyInstallmentLabel
            // 
            this.MonthlyInstallmentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MonthlyInstallmentLabel.Location = new System.Drawing.Point(8, 39);
            this.MonthlyInstallmentLabel.Name = "MonthlyInstallmentLabel";
            this.MonthlyInstallmentLabel.Size = new System.Drawing.Size(173, 20);
            this.MonthlyInstallmentLabel.TabIndex = 17;
            this.MonthlyInstallmentLabel.Text = "$0.00 Monthly";
            this.MonthlyInstallmentLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // PlanTitleLink
            // 
            this.PlanTitleLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlanTitleLink.Location = new System.Drawing.Point(3, 14);
            this.PlanTitleLink.Name = "PlanTitleLink";
            this.PlanTitleLink.Size = new System.Drawing.Size(178, 20);
            this.PlanTitleLink.TabIndex = 14;
            this.PlanTitleLink.TabStop = true;
            this.PlanTitleLink.Text = "PLAN";
            this.PlanTitleLink.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.PlanTitleLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.PlanTitleLink_LinkClicked);
            // 
            // LoansBox
            // 
            this.LoansBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoansBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoansBox.FormattingEnabled = true;
            this.LoansBox.IntegralHeight = false;
            this.LoansBox.ItemHeight = 16;
            this.LoansBox.Location = new System.Drawing.Point(12, 90);
            this.LoansBox.Name = "LoansBox";
            this.LoansBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.LoansBox.Size = new System.Drawing.Size(158, 180);
            this.LoansBox.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "Loans Selected";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // RepaymentPlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.MonthlyInstallmentLabel);
            this.Controls.Add(this.PlanTitleLink);
            this.Controls.Add(this.LoansBox);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "RepaymentPlan";
            this.Size = new System.Drawing.Size(184, 282);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label MonthlyInstallmentLabel;
        private System.Windows.Forms.LinkLabel PlanTitleLink;
        private System.Windows.Forms.ListBox LoansBox;
        private System.Windows.Forms.Label label3;
    }
}
