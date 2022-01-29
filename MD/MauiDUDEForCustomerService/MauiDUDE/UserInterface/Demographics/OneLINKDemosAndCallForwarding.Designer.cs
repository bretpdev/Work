namespace MauiDUDE
{
    partial class OneLINKDemosAndCallForwarding
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OneLINKDemosAndCallForwarding));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelSSN = new System.Windows.Forms.Label();
            this.labelAccountNumber = new System.Windows.Forms.Label();
            this.labelDOB = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.groupBoxCallForwarding = new System.Windows.Forms.GroupBox();
            this.listBoxCallForwardingResults = new System.Windows.Forms.ListBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.borrowerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBoxCallForwarding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.borrowerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "SSN:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(140, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Account #:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(346, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "DOB:";
            // 
            // labelSSN
            // 
            this.labelSSN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSSN.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerBindingSource, "SSN", true));
            this.labelSSN.Location = new System.Drawing.Point(44, 6);
            this.labelSSN.Name = "labelSSN";
            this.labelSSN.Size = new System.Drawing.Size(90, 23);
            this.labelSSN.TabIndex = 4;
            this.labelSSN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelAccountNumber
            // 
            this.labelAccountNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelAccountNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerBindingSource, "AccountNumber", true));
            this.labelAccountNumber.Location = new System.Drawing.Point(204, 6);
            this.labelAccountNumber.Name = "labelAccountNumber";
            this.labelAccountNumber.Size = new System.Drawing.Size(134, 23);
            this.labelAccountNumber.TabIndex = 5;
            this.labelAccountNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDOB
            // 
            this.labelDOB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelDOB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerBindingSource, "DOB", true));
            this.labelDOB.Location = new System.Drawing.Point(385, 6);
            this.labelDOB.Name = "labelDOB";
            this.labelDOB.Size = new System.Drawing.Size(77, 23);
            this.labelDOB.TabIndex = 6;
            this.labelDOB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelName
            // 
            this.labelName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerBindingSource, "FullName", true));
            this.labelName.Location = new System.Drawing.Point(44, 32);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(418, 23);
            this.labelName.TabIndex = 7;
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxCallForwarding
            // 
            this.groupBoxCallForwarding.Controls.Add(this.listBoxCallForwardingResults);
            this.groupBoxCallForwarding.Location = new System.Drawing.Point(8, 58);
            this.groupBoxCallForwarding.Name = "groupBoxCallForwarding";
            this.groupBoxCallForwarding.Size = new System.Drawing.Size(456, 72);
            this.groupBoxCallForwarding.TabIndex = 8;
            this.groupBoxCallForwarding.TabStop = false;
            this.groupBoxCallForwarding.Text = "Call Forwarding";
            // 
            // listBoxCallForwardingResults
            // 
            this.listBoxCallForwardingResults.FormattingEnabled = true;
            this.listBoxCallForwardingResults.Location = new System.Drawing.Point(6, 19);
            this.listBoxCallForwardingResults.Name = "listBoxCallForwardingResults";
            this.listBoxCallForwardingResults.Size = new System.Drawing.Size(444, 43);
            this.listBoxCallForwardingResults.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(204, 136);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 9;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // borrowerBindingSource
            // 
            this.borrowerBindingSource.DataSource = typeof(MauiDUDE.Borrower);
            // 
            // OneLINKDemosAndCallForwarding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(174)))), ((int)(((byte)(231)))));
            this.ClientSize = new System.Drawing.Size(475, 168);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxCallForwarding);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.labelDOB);
            this.Controls.Add(this.labelAccountNumber);
            this.Controls.Add(this.labelSSN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(491, 207);
            this.Name = "OneLINKDemosAndCallForwarding";
            this.Text = "Call forwarding";
            this.groupBoxCallForwarding.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.borrowerBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelSSN;
        private System.Windows.Forms.Label labelAccountNumber;
        private System.Windows.Forms.Label labelDOB;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.GroupBox groupBoxCallForwarding;
        private System.Windows.Forms.ListBox listBoxCallForwardingResults;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.BindingSource borrowerBindingSource;
    }
}