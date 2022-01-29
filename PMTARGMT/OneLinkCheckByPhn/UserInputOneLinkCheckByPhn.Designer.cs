namespace OneLinkCheckByPhn
{
    partial class UserInputOneLinkCheckByPhn
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
            this.txtSSNorAcctNum = new System.Windows.Forms.TextBox();
            this.checkByPhoneDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRouting = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVerRouting = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtChecking = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtVerChecking = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPayment = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.txtPaymentEffectDate = new System.Windows.Forms.MaskedTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.checkByPhoneDataBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSSNorAcctNum
            // 
            this.txtSSNorAcctNum.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.checkByPhoneDataBindingSource, "SSNOrAcctNum", true));
            this.txtSSNorAcctNum.Location = new System.Drawing.Point(187, 57);
            this.txtSSNorAcctNum.Name = "txtSSNorAcctNum";
            this.txtSSNorAcctNum.Size = new System.Drawing.Size(173, 20);
            this.txtSSNorAcctNum.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Account Number or SSN";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Routing Number";
            // 
            // txtRouting
            // 
            this.txtRouting.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.checkByPhoneDataBindingSource, "Routing", true));
            this.txtRouting.Location = new System.Drawing.Point(187, 79);
            this.txtRouting.MaxLength = 9;
            this.txtRouting.Name = "txtRouting";
            this.txtRouting.PasswordChar = '*';
            this.txtRouting.Size = new System.Drawing.Size(173, 20);
            this.txtRouting.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "Verify Routing Number";
            // 
            // txtVerRouting
            // 
            this.txtVerRouting.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.checkByPhoneDataBindingSource, "VerRouting", true));
            this.txtVerRouting.Location = new System.Drawing.Point(187, 101);
            this.txtVerRouting.Name = "txtVerRouting";
            this.txtVerRouting.Size = new System.Drawing.Size(173, 20);
            this.txtVerRouting.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 21);
            this.label4.TabIndex = 7;
            this.label4.Text = "Checking Account Number";
            // 
            // txtChecking
            // 
            this.txtChecking.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.checkByPhoneDataBindingSource, "CheckingAcctNum", true));
            this.txtChecking.Location = new System.Drawing.Point(187, 123);
            this.txtChecking.Name = "txtChecking";
            this.txtChecking.PasswordChar = '*';
            this.txtChecking.Size = new System.Drawing.Size(173, 20);
            this.txtChecking.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(175, 21);
            this.label5.TabIndex = 9;
            this.label5.Text = "Verify Checking Account Number";
            // 
            // txtVerChecking
            // 
            this.txtVerChecking.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.checkByPhoneDataBindingSource, "VerCheckingAcctNum", true));
            this.txtVerChecking.Location = new System.Drawing.Point(187, 145);
            this.txtVerChecking.Name = "txtVerChecking";
            this.txtVerChecking.Size = new System.Drawing.Size(173, 20);
            this.txtVerChecking.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(167, 21);
            this.label6.TabIndex = 11;
            this.label6.Text = "Payment Amount";
            // 
            // txtPayment
            // 
            this.txtPayment.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.checkByPhoneDataBindingSource, "PaymentAmount", true));
            this.txtPayment.Location = new System.Drawing.Point(187, 167);
            this.txtPayment.Name = "txtPayment";
            this.txtPayment.Size = new System.Drawing.Size(173, 20);
            this.txtPayment.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 192);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(167, 39);
            this.label7.TabIndex = 13;
            this.label7.Text = "Payment Posting Date (up to 14 days in the future)";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(12, 231);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(167, 21);
            this.label8.TabIndex = 14;
            this.label8.Text = "Account Type";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(187, 210);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(175, 57);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.checkByPhoneDataBindingSource, "Savings", true));
            this.radioButton2.Location = new System.Drawing.Point(99, 21);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(63, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Savings";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.checkByPhoneDataBindingSource, "Checking", true));
            this.radioButton1.Location = new System.Drawing.Point(15, 21);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(70, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Checking";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(95, 277);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(200, 277);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 17;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // txtPaymentEffectDate
            // 
            this.txtPaymentEffectDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.checkByPhoneDataBindingSource, "PaymentEffectiveDate", true));
            this.txtPaymentEffectDate.Location = new System.Drawing.Point(187, 189);
            this.txtPaymentEffectDate.Mask = "00/00/0000";
            this.txtPaymentEffectDate.Name = "txtPaymentEffectDate";
            this.txtPaymentEffectDate.Size = new System.Drawing.Size(173, 20);
            this.txtPaymentEffectDate.TabIndex = 12;
            this.txtPaymentEffectDate.ValidatingType = typeof(System.DateTime);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton4);
            this.groupBox2.Location = new System.Drawing.Point(187, -2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(175, 57);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.checkByPhoneDataBindingSource, "Outbound", true));
            this.radioButton3.Location = new System.Drawing.Point(99, 21);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(72, 17);
            this.radioButton3.TabIndex = 1;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Outbound";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.checkByPhoneDataBindingSource, "Inbound", true));
            this.radioButton4.Location = new System.Drawing.Point(15, 21);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(64, 17);
            this.radioButton4.TabIndex = 0;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Inbound";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(12, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(167, 21);
            this.label9.TabIndex = 18;
            this.label9.Text = "Call Type";
            // 
            // UserInputOneLinkCheckByPhn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 309);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtPaymentEffectDate);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPayment);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtVerChecking);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtChecking);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtVerRouting);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRouting);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSSNorAcctNum);
            this.Name = "UserInputOneLinkCheckByPhn";
            this.Text = "OneLINK Check By Phone";
            this.Shown += new System.EventHandler(this.UserInputOneLinkCheckByPhn_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.checkByPhoneDataBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.BindingSource checkByPhoneDataBindingSource;
        private System.Windows.Forms.TextBox txtSSNorAcctNum;
        private System.Windows.Forms.TextBox txtRouting;
        private System.Windows.Forms.TextBox txtVerRouting;
        private System.Windows.Forms.TextBox txtChecking;
        private System.Windows.Forms.TextBox txtVerChecking;
        private System.Windows.Forms.TextBox txtPayment;
        private System.Windows.Forms.MaskedTextBox txtPaymentEffectDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Label label9;
    }
}