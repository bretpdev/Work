namespace OPSCHKPHN
{
    partial class Confirmation
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.oPSEntryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label15 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpReminders = new System.Windows.Forms.GroupBox();
            this.chkReminder3 = new System.Windows.Forms.CheckBox();
            this.chkReminder2 = new System.Windows.Forms.CheckBox();
            this.chkReminder1 = new System.Windows.Forms.CheckBox();
            this.btnApprove = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.oPSEntryBindingSource)).BeginInit();
            this.grpReminders.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(449, 55);
            this.label1.TabIndex = 0;
            this.label1.Text = "Confirmation";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(14, 69);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(449, 157);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Summary";
            // 
            // label14
            // 
            this.label14.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "EffectiveDate", true));
            this.label14.Location = new System.Drawing.Point(145, 126);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(288, 18);
            this.label14.TabIndex = 13;
            // 
            // oPSEntryBindingSource
            // 
            this.oPSEntryBindingSource.DataSource = typeof(OPSCHKPHN.OPSEntry);
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(21, 126);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(117, 18);
            this.label15.TabIndex = 12;
            this.label15.Text = "Effective Date";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(117, 27);
            this.label12.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "AccountHolderName", true));
            this.label10.Location = new System.Drawing.Point(143, 107);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(288, 18);
            this.label10.TabIndex = 9;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(20, 107);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(117, 18);
            this.label11.TabIndex = 8;
            this.label11.Text = "Acct Holder Name";
            // 
            // label8
            // 
            this.label8.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "CalculatedAccountType", true));
            this.label8.Location = new System.Drawing.Point(143, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(288, 18);
            this.label8.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(20, 88);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(117, 18);
            this.label9.TabIndex = 6;
            this.label9.Text = "Account Type";
            // 
            // label6
            // 
            this.label6.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "PaymentAmount", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "C2"));
            this.label6.Location = new System.Drawing.Point(143, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(288, 18);
            this.label6.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(20, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 18);
            this.label7.TabIndex = 4;
            this.label7.Text = "Payment Amount";
            // 
            // label4
            // 
            this.label4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "BankAccountNumber", true));
            this.label4.Location = new System.Drawing.Point(143, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(288, 18);
            this.label4.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(20, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 18);
            this.label5.TabIndex = 2;
            this.label5.Text = "Bank Account #";
            // 
            // label3
            // 
            this.label3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.oPSEntryBindingSource, "RoutingNumber", true));
            this.label3.Location = new System.Drawing.Point(143, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(288, 18);
            this.label3.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(20, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Routing #";
            // 
            // grpReminders
            // 
            this.grpReminders.Controls.Add(this.chkReminder3);
            this.grpReminders.Controls.Add(this.chkReminder2);
            this.grpReminders.Controls.Add(this.chkReminder1);
            this.grpReminders.Location = new System.Drawing.Point(14, 233);
            this.grpReminders.Name = "grpReminders";
            this.grpReminders.Size = new System.Drawing.Size(449, 115);
            this.grpReminders.TabIndex = 2;
            this.grpReminders.TabStop = false;
            this.grpReminders.Text = "Reminders";
            // 
            // chkReminder3
            // 
            this.chkReminder3.AutoSize = true;
            this.chkReminder3.Location = new System.Drawing.Point(23, 72);
            this.chkReminder3.Name = "chkReminder3";
            this.chkReminder3.Size = new System.Drawing.Size(108, 19);
            this.chkReminder3.TabIndex = 2;
            this.chkReminder3.Text = "Offer Auto Pay.";
            this.chkReminder3.UseVisualStyleBackColor = true;
            // 
            // chkReminder2
            // 
            this.chkReminder2.AutoSize = true;
            this.chkReminder2.Location = new System.Drawing.Point(23, 48);
            this.chkReminder2.Name = "chkReminder2";
            this.chkReminder2.Size = new System.Drawing.Size(346, 19);
            this.chkReminder2.TabIndex = 1;
            this.chkReminder2.Text = "Remember to list this electronic check in your check register.";
            this.chkReminder2.UseVisualStyleBackColor = true;
            // 
            // chkReminder1
            // 
            this.chkReminder1.AutoSize = true;
            this.chkReminder1.Location = new System.Drawing.Point(23, 27);
            this.chkReminder1.Name = "chkReminder1";
            this.chkReminder1.Size = new System.Drawing.Size(322, 19);
            this.chkReminder1.TabIndex = 0;
            this.chkReminder1.Text = "Amount will be deducted within the next 24 to 48 hours.";
            this.chkReminder1.UseVisualStyleBackColor = true;
            // 
            // btnApprove
            // 
            this.btnApprove.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApprove.Location = new System.Drawing.Point(191, 367);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(87, 27);
            this.btnApprove.TabIndex = 3;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // Confirmation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 400);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.grpReminders);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximumSize = new System.Drawing.Size(488, 438);
            this.MinimumSize = new System.Drawing.Size(488, 438);
            this.Name = "Confirmation";
            this.Text = "Check By Phone Confirmation (OPS)";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.oPSEntryBindingSource)).EndInit();
            this.grpReminders.ResumeLayout(false);
            this.grpReminders.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpReminders;
        private System.Windows.Forms.CheckBox chkReminder3;
        private System.Windows.Forms.CheckBox chkReminder2;
        private System.Windows.Forms.CheckBox chkReminder1;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.BindingSource oPSEntryBindingSource;
    }
}