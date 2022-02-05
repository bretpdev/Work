namespace SatisJudg
{
    partial class SatJdg
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtComplaintDate = new System.Windows.Forms.TextBox();
            this.txtCompliantAmt = new System.Windows.Forms.TextBox();
            this.txtAbstractNum = new System.Windows.Forms.TextBox();
            this.cmbCounty = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Complaint Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Complaint Amount:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Abstract Number:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "County:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(117, 114);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtComplaintDate
            // 
            this.txtComplaintDate.Location = new System.Drawing.Point(131, 17);
            this.txtComplaintDate.Name = "txtComplaintDate";
            this.txtComplaintDate.Size = new System.Drawing.Size(164, 20);
            this.txtComplaintDate.TabIndex = 5;
            // 
            // txtCompliantAmt
            // 
            this.txtCompliantAmt.Location = new System.Drawing.Point(131, 40);
            this.txtCompliantAmt.Name = "txtCompliantAmt";
            this.txtCompliantAmt.Size = new System.Drawing.Size(164, 20);
            this.txtCompliantAmt.TabIndex = 6;
            // 
            // txtAbstractNum
            // 
            this.txtAbstractNum.Location = new System.Drawing.Point(131, 63);
            this.txtAbstractNum.Name = "txtAbstractNum";
            this.txtAbstractNum.Size = new System.Drawing.Size(164, 20);
            this.txtAbstractNum.TabIndex = 7;
            // 
            // cmbCounty
            // 
            this.cmbCounty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCounty.FormattingEnabled = true;
            this.cmbCounty.Location = new System.Drawing.Point(131, 87);
            this.cmbCounty.Name = "cmbCounty";
            this.cmbCounty.Size = new System.Drawing.Size(164, 21);
            this.cmbCounty.TabIndex = 8;
            // 
            // SatJdg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 142);
            this.Controls.Add(this.cmbCounty);
            this.Controls.Add(this.txtAbstractNum);
            this.Controls.Add(this.txtCompliantAmt);
            this.Controls.Add(this.txtComplaintDate);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SatJdg";
            this.Text = "Satisfaction of Judgement/Abstract";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtComplaintDate;
        private System.Windows.Forms.TextBox txtCompliantAmt;
        private System.Windows.Forms.TextBox txtAbstractNum;
        private System.Windows.Forms.ComboBox cmbCounty;
    }
}