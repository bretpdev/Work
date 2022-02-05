namespace PUTSUSPCOM
{
    partial class Entry
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.borrowerDemographicsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radDeleteReappliedPayment = new System.Windows.Forms.RadioButton();
            this.radTarget = new System.Windows.Forms.RadioButton();
            this.radDeletePayment = new System.Windows.Forms.RadioButton();
            this.pnlTheGuts = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.suspenseDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.borrowerDemographicsBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.suspenseDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDemographicsBindingSource, "FName", true));
            this.textBox1.Location = new System.Drawing.Point(170, 66);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(165, 20);
            this.textBox1.TabIndex = 0;
            // 
            // borrowerDemographicsBindingSource
            // 
            this.borrowerDemographicsBindingSource.DataSource = typeof(Q.BorrowerDemographics);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(1, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "First Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(1, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Middle Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDemographicsBindingSource, "MI", true));
            this.textBox2.Location = new System.Drawing.Point(170, 89);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(165, 20);
            this.textBox2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(1, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Last Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox3
            // 
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDemographicsBindingSource, "LName", true));
            this.textBox3.Location = new System.Drawing.Point(170, 113);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(165, 20);
            this.textBox3.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(1, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "Effective Date";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suspenseDataBindingSource, "EffectiveDate", true));
            this.textBox4.Location = new System.Drawing.Point(170, 136);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(165, 20);
            this.textBox4.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(1, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "SSN";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox5
            // 
            this.textBox5.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDemographicsBindingSource, "SSN", true));
            this.textBox5.Location = new System.Drawing.Point(170, 21);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(165, 20);
            this.textBox5.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(1, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(163, 23);
            this.label6.TabIndex = 11;
            this.label6.Text = "Account Number";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox6
            // 
            this.textBox6.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDemographicsBindingSource, "AccountNumber", true));
            this.textBox6.Location = new System.Drawing.Point(170, 43);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(165, 20);
            this.textBox6.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(435, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(163, 23);
            this.label7.TabIndex = 23;
            this.label7.Text = "Transaction Amount";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox7
            // 
            this.textBox7.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suspenseDataBindingSource, "Amount", true));
            this.textBox7.Location = new System.Drawing.Point(604, 43);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(165, 20);
            this.textBox7.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(435, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(163, 23);
            this.label8.TabIndex = 21;
            this.label8.Text = "Transaction Type/Sub Type";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox8
            // 
            this.textBox8.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suspenseDataBindingSource, "TransactionType", true));
            this.textBox8.Location = new System.Drawing.Point(604, 21);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(81, 20);
            this.textBox8.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(435, 134);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(163, 23);
            this.label9.TabIndex = 19;
            this.label9.Text = "Batch Code";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox9
            // 
            this.textBox9.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suspenseDataBindingSource, "BatchCode", true));
            this.textBox9.Location = new System.Drawing.Point(604, 136);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(165, 20);
            this.textBox9.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(435, 111);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(163, 23);
            this.label10.TabIndex = 17;
            this.label10.Text = "Sequence Number";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox10
            // 
            this.textBox10.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suspenseDataBindingSource, "SequenceNumber", true));
            this.textBox10.Location = new System.Drawing.Point(604, 113);
            this.textBox10.Name = "textBox10";
            this.textBox10.ReadOnly = true;
            this.textBox10.Size = new System.Drawing.Size(165, 20);
            this.textBox10.TabIndex = 16;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(435, 87);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(163, 23);
            this.label11.TabIndex = 15;
            this.label11.Text = "Batch Number";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox11
            // 
            this.textBox11.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suspenseDataBindingSource, "BatchNumber", true));
            this.textBox11.Location = new System.Drawing.Point(604, 89);
            this.textBox11.Name = "textBox11";
            this.textBox11.ReadOnly = true;
            this.textBox11.Size = new System.Drawing.Size(165, 20);
            this.textBox11.TabIndex = 14;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(435, 64);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(163, 23);
            this.label12.TabIndex = 13;
            this.label12.Text = "Assigned To";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox12
            // 
            this.textBox12.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suspenseDataBindingSource, "AssignedTo", true));
            this.textBox12.Location = new System.Drawing.Point(604, 66);
            this.textBox12.Name = "textBox12";
            this.textBox12.ReadOnly = true;
            this.textBox12.Size = new System.Drawing.Size(165, 20);
            this.textBox12.TabIndex = 12;
            // 
            // textBox13
            // 
            this.textBox13.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suspenseDataBindingSource, "TransactionSubType", true));
            this.textBox13.Location = new System.Drawing.Point(688, 21);
            this.textBox13.Name = "textBox13";
            this.textBox13.ReadOnly = true;
            this.textBox13.Size = new System.Drawing.Size(81, 20);
            this.textBox13.TabIndex = 24;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox13);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox7);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox8);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox9);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.textBox10);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.textBox6);
            this.groupBox1.Controls.Add(this.textBox11);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBox12);
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(835, 170);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Suspense Information";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radDeleteReappliedPayment);
            this.groupBox2.Controls.Add(this.radTarget);
            this.groupBox2.Controls.Add(this.radDeletePayment);
            this.groupBox2.Location = new System.Drawing.Point(208, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(435, 49);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // radDeleteReappliedPayment
            // 
            this.radDeleteReappliedPayment.AutoSize = true;
            this.radDeleteReappliedPayment.Location = new System.Drawing.Point(273, 19);
            this.radDeleteReappliedPayment.Name = "radDeleteReappliedPayment";
            this.radDeleteReappliedPayment.Size = new System.Drawing.Size(153, 17);
            this.radDeleteReappliedPayment.TabIndex = 2;
            this.radDeleteReappliedPayment.TabStop = true;
            this.radDeleteReappliedPayment.Text = "Delete/Reapplied Payment";
            this.radDeleteReappliedPayment.UseVisualStyleBackColor = true;
            this.radDeleteReappliedPayment.CheckedChanged += new System.EventHandler(this.radDeleteReappliedPayment_CheckedChanged);
            // 
            // radTarget
            // 
            this.radTarget.AutoSize = true;
            this.radTarget.Location = new System.Drawing.Point(167, 19);
            this.radTarget.Name = "radTarget";
            this.radTarget.Size = new System.Drawing.Size(100, 17);
            this.radTarget.TabIndex = 1;
            this.radTarget.TabStop = true;
            this.radTarget.Text = "Target Payment";
            this.radTarget.UseVisualStyleBackColor = true;
            this.radTarget.CheckedChanged += new System.EventHandler(this.radTarget_CheckedChanged);
            // 
            // radDeletePayment
            // 
            this.radDeletePayment.AutoSize = true;
            this.radDeletePayment.Location = new System.Drawing.Point(11, 19);
            this.radDeletePayment.Name = "radDeletePayment";
            this.radDeletePayment.Size = new System.Drawing.Size(150, 17);
            this.radDeletePayment.TabIndex = 0;
            this.radDeletePayment.TabStop = true;
            this.radDeletePayment.Text = "Delete Suspense Payment";
            this.radDeletePayment.UseVisualStyleBackColor = true;
            this.radDeletePayment.CheckedChanged += new System.EventHandler(this.radDeletePayment_CheckedChanged);
            // 
            // pnlTheGuts
            // 
            this.pnlTheGuts.Location = new System.Drawing.Point(3, 222);
            this.pnlTheGuts.Name = "pnlTheGuts";
            this.pnlTheGuts.Size = new System.Drawing.Size(835, 371);
            this.pnlTheGuts.TabIndex = 27;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(332, 599);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 30;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(429, 599);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // suspenseDataBindingSource
            // 
            this.suspenseDataBindingSource.DataSource = typeof(PUTSUSPCOM.Suspense);
            // 
            // Entry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 630);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlTheGuts);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Entry";
            this.Text = "Option Entry";
            ((System.ComponentModel.ISupportInitialize)(this.borrowerDemographicsBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.suspenseDataBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radDeleteReappliedPayment;
        private System.Windows.Forms.RadioButton radTarget;
        private System.Windows.Forms.RadioButton radDeletePayment;
        private System.Windows.Forms.FlowLayoutPanel pnlTheGuts;
        private System.Windows.Forms.BindingSource borrowerDemographicsBindingSource;
        private System.Windows.Forms.BindingSource suspenseDataBindingSource;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}