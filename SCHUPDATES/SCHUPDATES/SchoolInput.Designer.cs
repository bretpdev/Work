namespace SCHUPDATES
{
    partial class SchoolInput
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
            this.LoanPgms = new System.Windows.Forms.CheckedListBox();
            this.Guarantor = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TX10Approval = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TX13Approval = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ApprovalDate = new System.Windows.Forms.MaskedTextBox();
            this.Submit = new Uheaa.Common.WinForms.ValidationButton();
            this.SchoolId = new Uheaa.Common.WinForms.RequiredTextBox();
            this.SelectAllPgms = new System.Windows.Forms.CheckBox();
            this.SelectAllGuar = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.MergedSchool = new System.Windows.Forms.TextBox();
            this.MergedDate = new System.Windows.Forms.MaskedTextBox();
            this.TX13Reason = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "School ID:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Loan Programs:";
            // 
            // LoanPgms
            // 
            this.LoanPgms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LoanPgms.CheckOnClick = true;
            this.LoanPgms.FormattingEnabled = true;
            this.LoanPgms.Location = new System.Drawing.Point(12, 105);
            this.LoanPgms.MultiColumn = true;
            this.LoanPgms.Name = "LoanPgms";
            this.LoanPgms.Size = new System.Drawing.Size(333, 235);
            this.LoanPgms.TabIndex = 4;
            this.LoanPgms.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.LoanPgms_ItemCheck);
            // 
            // Guarantor
            // 
            this.Guarantor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Guarantor.CheckOnClick = true;
            this.Guarantor.FormattingEnabled = true;
            this.Guarantor.Location = new System.Drawing.Point(12, 371);
            this.Guarantor.MultiColumn = true;
            this.Guarantor.Name = "Guarantor";
            this.Guarantor.Size = new System.Drawing.Size(332, 235);
            this.Guarantor.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 348);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Guarantors:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 611);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "TX10 Approval Type";
            // 
            // TX10Approval
            // 
            this.TX10Approval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TX10Approval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TX10Approval.FormattingEnabled = true;
            this.TX10Approval.Location = new System.Drawing.Point(12, 634);
            this.TX10Approval.Name = "TX10Approval";
            this.TX10Approval.Size = new System.Drawing.Size(332, 28);
            this.TX10Approval.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 665);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(164, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "TX13 Approval Status";
            // 
            // TX13Approval
            // 
            this.TX13Approval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TX13Approval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TX13Approval.FormattingEnabled = true;
            this.TX13Approval.Location = new System.Drawing.Point(12, 688);
            this.TX13Approval.Name = "TX13Approval";
            this.TX13Approval.Size = new System.Drawing.Size(332, 28);
            this.TX13Approval.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 719);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Approval Date";
            // 
            // ApprovalDate
            // 
            this.ApprovalDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ApprovalDate.Location = new System.Drawing.Point(16, 742);
            this.ApprovalDate.Mask = "00/00/0000";
            this.ApprovalDate.Name = "ApprovalDate";
            this.ApprovalDate.Size = new System.Drawing.Size(86, 26);
            this.ApprovalDate.TabIndex = 9;
            this.ApprovalDate.ValidatingType = typeof(System.DateTime);
            this.ApprovalDate.Leave += new System.EventHandler(this.ApprovalDate_Leave);
            // 
            // Submit
            // 
            this.Submit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Submit.Location = new System.Drawing.Point(16, 796);
            this.Submit.Name = "Submit";
            this.Submit.Size = new System.Drawing.Size(328, 39);
            this.Submit.TabIndex = 11;
            this.Submit.Text = "Submit";
            this.Submit.UseVisualStyleBackColor = true;
            this.Submit.OnValidate += new Uheaa.Common.WinForms.ValidationButton.ValidationHandler(this.Submit_OnValidate);
            // 
            // SchoolId
            // 
            this.SchoolId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SchoolId.Location = new System.Drawing.Point(139, 6);
            this.SchoolId.MaxLength = 8;
            this.SchoolId.Name = "SchoolId";
            this.SchoolId.Size = new System.Drawing.Size(206, 26);
            this.SchoolId.TabIndex = 0;
            // 
            // SelectAllPgms
            // 
            this.SelectAllPgms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectAllPgms.AutoSize = true;
            this.SelectAllPgms.Location = new System.Drawing.Point(250, 78);
            this.SelectAllPgms.Name = "SelectAllPgms";
            this.SelectAllPgms.Size = new System.Drawing.Size(94, 24);
            this.SelectAllPgms.TabIndex = 3;
            this.SelectAllPgms.Text = "Select All";
            this.SelectAllPgms.UseVisualStyleBackColor = true;
            this.SelectAllPgms.CheckedChanged += new System.EventHandler(this.SelectAllPgms_CheckedChanged);
            // 
            // SelectAllGuar
            // 
            this.SelectAllGuar.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SelectAllGuar.AutoSize = true;
            this.SelectAllGuar.Location = new System.Drawing.Point(250, 344);
            this.SelectAllGuar.Name = "SelectAllGuar";
            this.SelectAllGuar.Size = new System.Drawing.Size(94, 24);
            this.SelectAllGuar.TabIndex = 5;
            this.SelectAllGuar.Text = "Select All";
            this.SelectAllGuar.UseVisualStyleBackColor = true;
            this.SelectAllGuar.CheckedChanged += new System.EventHandler(this.SelectAllGuar_CheckedChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 20);
            this.label7.TabIndex = 12;
            this.label7.Text = "Merged School:";
            // 
            // MergedSchool
            // 
            this.MergedSchool.Location = new System.Drawing.Point(139, 46);
            this.MergedSchool.MaxLength = 8;
            this.MergedSchool.Name = "MergedSchool";
            this.MergedSchool.Size = new System.Drawing.Size(99, 26);
            this.MergedSchool.TabIndex = 1;
            this.MergedSchool.TextChanged += new System.EventHandler(this.MergedSchool_TextChanged);
            // 
            // MergedDate
            // 
            this.MergedDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MergedDate.Location = new System.Drawing.Point(259, 46);
            this.MergedDate.Mask = "00/00/0000";
            this.MergedDate.Name = "MergedDate";
            this.MergedDate.Size = new System.Drawing.Size(86, 26);
            this.MergedDate.TabIndex = 2;
            this.MergedDate.ValidatingType = typeof(System.DateTime);
            this.MergedDate.TextChanged += new System.EventHandler(this.MergedDate_TextChanged);
            // 
            // TX13Reason
            // 
            this.TX13Reason.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TX13Reason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TX13Reason.FormattingEnabled = true;
            this.TX13Reason.Location = new System.Drawing.Point(129, 740);
            this.TX13Reason.Name = "TX13Reason";
            this.TX13Reason.Size = new System.Drawing.Size(216, 28);
            this.TX13Reason.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(129, 720);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(158, 20);
            this.label8.TabIndex = 14;
            this.label8.Text = "TX13 Status Reason";
            // 
            // SchoolInput
            // 
            this.AcceptButton = this.Submit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 847);
            this.Controls.Add(this.TX13Reason);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.MergedDate);
            this.Controls.Add(this.MergedSchool);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.SelectAllGuar);
            this.Controls.Add(this.SelectAllPgms);
            this.Controls.Add(this.ApprovalDate);
            this.Controls.Add(this.Submit);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TX13Approval);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TX10Approval);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Guarantor);
            this.Controls.Add(this.LoanPgms);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SchoolId);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(377, 886);
            this.Name = "SchoolInput";
            this.ShowIcon = false;
            this.Text = "Enter School Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Uheaa.Common.WinForms.RequiredTextBox SchoolId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox LoanPgms;
        private System.Windows.Forms.CheckedListBox Guarantor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox TX10Approval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox TX13Approval;
        private System.Windows.Forms.Label label6;
        private Uheaa.Common.WinForms.ValidationButton Submit;
        private System.Windows.Forms.MaskedTextBox ApprovalDate;
        private System.Windows.Forms.CheckBox SelectAllPgms;
        private System.Windows.Forms.CheckBox SelectAllGuar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox MergedSchool;
        private System.Windows.Forms.MaskedTextBox MergedDate;
        private System.Windows.Forms.ComboBox TX13Reason;
        private System.Windows.Forms.Label label8;
    }
}