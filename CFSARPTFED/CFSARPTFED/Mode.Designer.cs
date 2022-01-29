namespace CFSARPTFED
{
    partial class Mode
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
            this.grpRadBtns = new System.Windows.Forms.GroupBox();
            this.rdoSpecificRpt = new System.Windows.Forms.RadioButton();
            this.rdoAllReports = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboReport = new System.Windows.Forms.ComboBox();
            this.lblSelectRpt = new System.Windows.Forms.Label();
            this.grpRadBtns.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpRadBtns
            // 
            this.grpRadBtns.Controls.Add(this.rdoSpecificRpt);
            this.grpRadBtns.Controls.Add(this.rdoAllReports);
            this.grpRadBtns.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.grpRadBtns.Location = new System.Drawing.Point(12, 12);
            this.grpRadBtns.Name = "grpRadBtns";
            this.grpRadBtns.Size = new System.Drawing.Size(206, 100);
            this.grpRadBtns.TabIndex = 1;
            this.grpRadBtns.TabStop = false;
            this.grpRadBtns.Text = "Select an option below:";
            // 
            // rdoSpecificRpt
            // 
            this.rdoSpecificRpt.AutoSize = true;
            this.rdoSpecificRpt.Location = new System.Drawing.Point(25, 66);
            this.rdoSpecificRpt.Name = "rdoSpecificRpt";
            this.rdoSpecificRpt.Size = new System.Drawing.Size(141, 17);
            this.rdoSpecificRpt.TabIndex = 1;
            this.rdoSpecificRpt.TabStop = true;
            this.rdoSpecificRpt.Text = "Create a Specific Report";
            this.rdoSpecificRpt.UseVisualStyleBackColor = true;
            this.rdoSpecificRpt.CheckedChanged += new System.EventHandler(this.rdoSpecificRpt_CheckedChanged);
            // 
            // rdoAllReports
            // 
            this.rdoAllReports.AutoSize = true;
            this.rdoAllReports.Location = new System.Drawing.Point(25, 32);
            this.rdoAllReports.Name = "rdoAllReports";
            this.rdoAllReports.Size = new System.Drawing.Size(110, 17);
            this.rdoAllReports.TabIndex = 0;
            this.rdoAllReports.TabStop = true;
            this.rdoAllReports.Text = "Create All Reports";
            this.rdoAllReports.UseVisualStyleBackColor = true;
            this.rdoAllReports.CheckedChanged += new System.EventHandler(this.rdoAllReports_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(63, 198);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(144, 198);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cboReport
            // 
            this.cboReport.DropDownWidth = 350;
            this.cboReport.Enabled = false;
            this.cboReport.FormattingEnabled = true;
            this.cboReport.Location = new System.Drawing.Point(12, 142);
            this.cboReport.Name = "cboReport";
            this.cboReport.Size = new System.Drawing.Size(206, 21);
            this.cboReport.TabIndex = 4;
            // 
            // lblSelectRpt
            // 
            this.lblSelectRpt.AutoSize = true;
            this.lblSelectRpt.Location = new System.Drawing.Point(12, 126);
            this.lblSelectRpt.Name = "lblSelectRpt";
            this.lblSelectRpt.Size = new System.Drawing.Size(196, 13);
            this.lblSelectRpt.TabIndex = 5;
            this.lblSelectRpt.Text = "Select the Report you want to generate.";
            // 
            // Mode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 233);
            this.Controls.Add(this.lblSelectRpt);
            this.Controls.Add(this.cboReport);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grpRadBtns);
            this.Name = "Mode";
            this.Text = "Mode";
            this.grpRadBtns.ResumeLayout(false);
            this.grpRadBtns.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpRadBtns;
        private System.Windows.Forms.RadioButton rdoSpecificRpt;
        private System.Windows.Forms.RadioButton rdoAllReports;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cboReport;
        private System.Windows.Forms.Label lblSelectRpt;

    }
}