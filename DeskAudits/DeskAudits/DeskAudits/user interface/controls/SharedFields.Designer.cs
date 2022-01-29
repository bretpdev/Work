
namespace DeskAudits
{
    partial class SharedFields
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
            this.lblFailReason = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblAuditee = new System.Windows.Forms.Label();
            this.lblAuditor = new System.Windows.Forms.Label();
            this.FailReasonField = new System.Windows.Forms.ComboBox();
            this.ResultField = new System.Windows.Forms.ComboBox();
            this.AuditeeField = new System.Windows.Forms.ComboBox();
            this.AuditorField = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblFailReason
            // 
            this.lblFailReason.AutoSize = true;
            this.lblFailReason.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblFailReason.Location = new System.Drawing.Point(298, 85);
            this.lblFailReason.Name = "lblFailReason";
            this.lblFailReason.Size = new System.Drawing.Size(110, 24);
            this.lblFailReason.TabIndex = 15;
            this.lblFailReason.Text = "Fail Reason";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblResult.Location = new System.Drawing.Point(68, 85);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(62, 24);
            this.lblResult.TabIndex = 14;
            this.lblResult.Text = "Result";
            // 
            // lblAuditee
            // 
            this.lblAuditee.AutoSize = true;
            this.lblAuditee.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblAuditee.Location = new System.Drawing.Point(321, 13);
            this.lblAuditee.Name = "lblAuditee";
            this.lblAuditee.Size = new System.Drawing.Size(75, 24);
            this.lblAuditee.TabIndex = 13;
            this.lblAuditee.Text = "Auditee";
            // 
            // lblAuditor
            // 
            this.lblAuditor.AutoSize = true;
            this.lblAuditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblAuditor.Location = new System.Drawing.Point(68, 13);
            this.lblAuditor.Name = "lblAuditor";
            this.lblAuditor.Size = new System.Drawing.Size(70, 24);
            this.lblAuditor.TabIndex = 12;
            this.lblAuditor.Text = "Auditor";
            // 
            // FailReasonField
            // 
            this.FailReasonField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FailReasonField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.FailReasonField.FormattingEnabled = true;
            this.FailReasonField.Location = new System.Drawing.Point(242, 112);
            this.FailReasonField.Name = "FailReasonField";
            this.FailReasonField.Size = new System.Drawing.Size(217, 28);
            this.FailReasonField.TabIndex = 11;
            this.FailReasonField.SelectedIndexChanged += new System.EventHandler(this.FailReasonField_SelectedIndexChanged);
            // 
            // ResultField
            // 
            this.ResultField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ResultField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ResultField.FormattingEnabled = true;
            this.ResultField.Location = new System.Drawing.Point(11, 112);
            this.ResultField.Name = "ResultField";
            this.ResultField.Size = new System.Drawing.Size(195, 28);
            this.ResultField.TabIndex = 10;
            this.ResultField.SelectedIndexChanged += new System.EventHandler(this.ResultField_SelectedIndexChanged);
            // 
            // AuditeeField
            // 
            this.AuditeeField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AuditeeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.AuditeeField.FormattingEnabled = true;
            this.AuditeeField.Location = new System.Drawing.Point(242, 40);
            this.AuditeeField.Name = "AuditeeField";
            this.AuditeeField.Size = new System.Drawing.Size(217, 28);
            this.AuditeeField.TabIndex = 9;
            // 
            // AuditorField
            // 
            this.AuditorField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AuditorField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.AuditorField.FormattingEnabled = true;
            this.AuditorField.Location = new System.Drawing.Point(11, 40);
            this.AuditorField.Name = "AuditorField";
            this.AuditorField.Size = new System.Drawing.Size(195, 28);
            this.AuditorField.TabIndex = 16;
            // 
            // SharedFields
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.AuditorField);
            this.Controls.Add(this.lblFailReason);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblAuditee);
            this.Controls.Add(this.lblAuditor);
            this.Controls.Add(this.FailReasonField);
            this.Controls.Add(this.ResultField);
            this.Controls.Add(this.AuditeeField);
            this.Name = "SharedFields";
            this.Size = new System.Drawing.Size(462, 143);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFailReason;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblAuditee;
        private System.Windows.Forms.Label lblAuditor;
        private System.Windows.Forms.ComboBox FailReasonField;
        private System.Windows.Forms.ComboBox ResultField;
        private System.Windows.Forms.ComboBox AuditeeField;
        private System.Windows.Forms.ComboBox AuditorField;
    }
}
