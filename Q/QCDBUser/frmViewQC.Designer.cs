namespace QCDBUser
{
    partial class frmViewQC
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
            this.btnLog = new System.Windows.Forms.Button();
            this.btnDiscard = new System.Windows.Forms.Button();
            this.btnSaveLater = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtReportName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LoginName = new System.Windows.Forms.Label();
            this.txtRespName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtActDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSvdDate = new System.Windows.Forms.TextBox();
            this.cmbUserID = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.txtSubjectName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnLog
            // 
            this.btnLog.Location = new System.Drawing.Point(25, 237);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(83, 23);
            this.btnLog.TabIndex = 0;
            this.btnLog.Text = "Log Issue";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // btnDiscard
            // 
            this.btnDiscard.Location = new System.Drawing.Point(126, 237);
            this.btnDiscard.Name = "btnDiscard";
            this.btnDiscard.Size = new System.Drawing.Size(83, 23);
            this.btnDiscard.TabIndex = 1;
            this.btnDiscard.Text = "Discard";
            this.btnDiscard.UseVisualStyleBackColor = true;
            this.btnDiscard.Click += new System.EventHandler(this.btnDiscard_Click);
            // 
            // btnSaveLater
            // 
            this.btnSaveLater.Location = new System.Drawing.Point(303, 237);
            this.btnSaveLater.Name = "btnSaveLater";
            this.btnSaveLater.Size = new System.Drawing.Size(83, 23);
            this.btnSaveLater.TabIndex = 2;
            this.btnSaveLater.Text = "Save for Later";
            this.btnSaveLater.UseVisualStyleBackColor = true;
            this.btnSaveLater.Click += new System.EventHandler(this.btnSaveLater_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Location = new System.Drawing.Point(405, 237);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtReportName
            // 
            this.txtReportName.Location = new System.Drawing.Point(82, 2);
            this.txtReportName.Name = "txtReportName";
            this.txtReportName.ReadOnly = true;
            this.txtReportName.Size = new System.Drawing.Size(135, 20);
            this.txtReportName.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Report Name:";
            this.label1.Visible = false;
            // 
            // LoginName
            // 
            this.LoginName.AutoSize = true;
            this.LoginName.Location = new System.Drawing.Point(248, 5);
            this.LoginName.Name = "LoginName";
            this.LoginName.Size = new System.Drawing.Size(64, 13);
            this.LoginName.TabIndex = 7;
            this.LoginName.Text = "LoginName:";
            this.LoginName.Visible = false;
            // 
            // txtRespName
            // 
            this.txtRespName.Location = new System.Drawing.Point(353, 2);
            this.txtRespName.Name = "txtRespName";
            this.txtRespName.ReadOnly = true;
            this.txtRespName.Size = new System.Drawing.Size(135, 20);
            this.txtRespName.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(274, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "User ID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Activity Date:";
            // 
            // txtActDate
            // 
            this.txtActDate.Location = new System.Drawing.Point(126, 94);
            this.txtActDate.Name = "txtActDate";
            this.txtActDate.ReadOnly = true;
            this.txtActDate.Size = new System.Drawing.Size(135, 20);
            this.txtActDate.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(274, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Saved Date:";
            // 
            // txtSvdDate
            // 
            this.txtSvdDate.Location = new System.Drawing.Point(353, 94);
            this.txtSvdDate.Name = "txtSvdDate";
            this.txtSvdDate.ReadOnly = true;
            this.txtSvdDate.Size = new System.Drawing.Size(135, 20);
            this.txtSvdDate.TabIndex = 12;
            // 
            // cmbUserID
            // 
            this.cmbUserID.FormattingEnabled = true;
            this.cmbUserID.Location = new System.Drawing.Point(353, 63);
            this.cmbUserID.Name = "cmbUserID";
            this.cmbUserID.Size = new System.Drawing.Size(135, 21);
            this.cmbUserID.TabIndex = 16;
            this.cmbUserID.SelectedIndexChanged += new System.EventHandler(this.cmbUserID_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Description:";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(126, 130);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(362, 91);
            this.txtDesc.TabIndex = 17;
            // 
            // txtSubjectName
            // 
            this.txtSubjectName.Location = new System.Drawing.Point(126, 28);
            this.txtSubjectName.Name = "txtSubjectName";
            this.txtSubjectName.ReadOnly = true;
            this.txtSubjectName.Size = new System.Drawing.Size(362, 20);
            this.txtSubjectName.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Subject Name:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Responsible Name:";
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(127, 63);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.ReadOnly = true;
            this.txtFullName.Size = new System.Drawing.Size(135, 20);
            this.txtFullName.TabIndex = 21;
            // 
            // frmViewQC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 272);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSubjectName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.cmbUserID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSvdDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtActDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LoginName);
            this.Controls.Add(this.txtRespName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtReportName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveLater);
            this.Controls.Add(this.btnDiscard);
            this.Controls.Add(this.btnLog);
            this.Name = "frmViewQC";
            this.Text = "QC Incident View";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Button btnDiscard;
        private System.Windows.Forms.Button btnSaveLater;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtReportName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LoginName;
        private System.Windows.Forms.TextBox txtRespName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtActDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSvdDate;
        private System.Windows.Forms.ComboBox cmbUserID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.TextBox txtSubjectName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtFullName;
    }
}