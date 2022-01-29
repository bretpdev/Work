namespace SCHDEMOUP
{
    partial class frmSchoolType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSchoolType));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.rdoBursar = new System.Windows.Forms.RadioButton();
            this.rdoRegistrar = new System.Windows.Forms.RadioButton();
            this.rdoFinancial = new System.Windows.Forms.RadioButton();
            this.rdoGeneral = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSchoolCode = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnUpdates = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.MaximumSize = new System.Drawing.Size(300, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(296, 52);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoAll);
            this.groupBox1.Controls.Add(this.rdoBursar);
            this.groupBox1.Controls.Add(this.rdoRegistrar);
            this.groupBox1.Controls.Add(this.rdoFinancial);
            this.groupBox1.Controls.Add(this.rdoGeneral);
            this.groupBox1.Location = new System.Drawing.Point(15, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 148);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Update Type";
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = true;
            this.rdoAll.Location = new System.Drawing.Point(15, 119);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(149, 17);
            this.rdoAll.TabIndex = 4;
            this.rdoAll.Text = "5. All Departments Update";
            this.rdoAll.UseVisualStyleBackColor = true;
            // 
            // rdoBursar
            // 
            this.rdoBursar.AutoSize = true;
            this.rdoBursar.Location = new System.Drawing.Point(15, 94);
            this.rdoBursar.Name = "rdoBursar";
            this.rdoBursar.Size = new System.Drawing.Size(163, 17);
            this.rdoBursar.TabIndex = 3;
            this.rdoBursar.Text = "4. Bursar Department Update";
            this.rdoBursar.UseVisualStyleBackColor = true;
            // 
            // rdoRegistrar
            // 
            this.rdoRegistrar.AutoSize = true;
            this.rdoRegistrar.Location = new System.Drawing.Point(15, 69);
            this.rdoRegistrar.Name = "rdoRegistrar";
            this.rdoRegistrar.Size = new System.Drawing.Size(175, 17);
            this.rdoRegistrar.TabIndex = 2;
            this.rdoRegistrar.Text = "3. Registrar Department Update";
            this.rdoRegistrar.UseVisualStyleBackColor = true;
            // 
            // rdoFinancial
            // 
            this.rdoFinancial.AutoSize = true;
            this.rdoFinancial.Location = new System.Drawing.Point(15, 44);
            this.rdoFinancial.Name = "rdoFinancial";
            this.rdoFinancial.Size = new System.Drawing.Size(193, 17);
            this.rdoFinancial.TabIndex = 1;
            this.rdoFinancial.Text = "2. Financial Aid Department Update";
            this.rdoFinancial.UseVisualStyleBackColor = true;
            // 
            // rdoGeneral
            // 
            this.rdoGeneral.AutoSize = true;
            this.rdoGeneral.Location = new System.Drawing.Point(15, 19);
            this.rdoGeneral.Name = "rdoGeneral";
            this.rdoGeneral.Size = new System.Drawing.Size(170, 17);
            this.rdoGeneral.TabIndex = 0;
            this.rdoGeneral.Text = "1. General Department Update";
            this.rdoGeneral.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 248);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "School Code";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Date Submitted";
            // 
            // txtSchoolCode
            // 
            this.txtSchoolCode.Location = new System.Drawing.Point(118, 245);
            this.txtSchoolCode.MaxLength = 8;
            this.txtSchoolCode.Name = "txtSchoolCode";
            this.txtSchoolCode.Size = new System.Drawing.Size(100, 20);
            this.txtSchoolCode.TabIndex = 5;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(30, 325);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnUpdates
            // 
            this.btnUpdates.Location = new System.Drawing.Point(118, 325);
            this.btnUpdates.Name = "btnUpdates";
            this.btnUpdates.Size = new System.Drawing.Size(100, 23);
            this.btnUpdates.TabIndex = 7;
            this.btnUpdates.Text = "Run Updates";
            this.btnUpdates.UseVisualStyleBackColor = true;
            this.btnUpdates.Click += new System.EventHandler(this.btnUpdates_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(233, 325);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(118, 270);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(100, 20);
            this.dtpDate.TabIndex = 9;
            // 
            // frmSchoolType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 384);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdates);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtSchoolCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "frmSchoolType";
            this.Text = "School Update Type";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoAll;
        private System.Windows.Forms.RadioButton rdoBursar;
        private System.Windows.Forms.RadioButton rdoRegistrar;
        private System.Windows.Forms.RadioButton rdoFinancial;
        private System.Windows.Forms.RadioButton rdoGeneral;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSchoolCode;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnUpdates;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DateTimePicker dtpDate;
    }
}