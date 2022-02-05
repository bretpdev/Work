namespace MauiDUDE
{
    partial class PhoneIndicator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhoneIndicator));
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxHome = new System.Windows.Forms.CheckBox();
            this.checkBoxOther = new System.Windows.Forms.CheckBox();
            this.checkBoxOther2 = new System.Windows.Forms.CheckBox();
            this.checkBoxManualInput = new System.Windows.Forms.CheckBox();
            this.textBoxHome = new System.Windows.Forms.TextBox();
            this.textBoxOther = new System.Windows.Forms.TextBox();
            this.textBoxOther2 = new System.Windows.Forms.TextBox();
            this.textBoxManualInput = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(43, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 44);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please choose the phone where you reached the borrower.";
            // 
            // checkBoxHome
            // 
            this.checkBoxHome.AutoSize = true;
            this.checkBoxHome.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxHome.Location = new System.Drawing.Point(46, 59);
            this.checkBoxHome.Name = "checkBoxHome";
            this.checkBoxHome.Size = new System.Drawing.Size(54, 17);
            this.checkBoxHome.TabIndex = 1;
            this.checkBoxHome.Text = "Home";
            this.checkBoxHome.UseVisualStyleBackColor = true;
            this.checkBoxHome.CheckedChanged += new System.EventHandler(this.checkBoxHome_CheckedChanged);
            // 
            // checkBoxOther
            // 
            this.checkBoxOther.AutoSize = true;
            this.checkBoxOther.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxOther.Location = new System.Drawing.Point(48, 90);
            this.checkBoxOther.Name = "checkBoxOther";
            this.checkBoxOther.Size = new System.Drawing.Size(52, 17);
            this.checkBoxOther.TabIndex = 2;
            this.checkBoxOther.Text = "Other";
            this.checkBoxOther.UseVisualStyleBackColor = true;
            this.checkBoxOther.CheckedChanged += new System.EventHandler(this.checkBoxOther_CheckedChanged);
            // 
            // checkBoxOther2
            // 
            this.checkBoxOther2.AutoSize = true;
            this.checkBoxOther2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxOther2.Location = new System.Drawing.Point(39, 121);
            this.checkBoxOther2.Name = "checkBoxOther2";
            this.checkBoxOther2.Size = new System.Drawing.Size(61, 17);
            this.checkBoxOther2.TabIndex = 3;
            this.checkBoxOther2.Text = "Other 2";
            this.checkBoxOther2.UseVisualStyleBackColor = true;
            this.checkBoxOther2.CheckedChanged += new System.EventHandler(this.checkBoxOther2_CheckedChanged);
            // 
            // checkBoxManualInput
            // 
            this.checkBoxManualInput.AutoSize = true;
            this.checkBoxManualInput.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxManualInput.Location = new System.Drawing.Point(12, 152);
            this.checkBoxManualInput.Name = "checkBoxManualInput";
            this.checkBoxManualInput.Size = new System.Drawing.Size(88, 17);
            this.checkBoxManualInput.TabIndex = 4;
            this.checkBoxManualInput.Text = "Manual Input";
            this.checkBoxManualInput.UseVisualStyleBackColor = true;
            this.checkBoxManualInput.CheckedChanged += new System.EventHandler(this.checkBoxManualInput_CheckedChanged);
            // 
            // textBoxHome
            // 
            this.textBoxHome.Location = new System.Drawing.Point(106, 56);
            this.textBoxHome.Name = "textBoxHome";
            this.textBoxHome.Size = new System.Drawing.Size(100, 20);
            this.textBoxHome.TabIndex = 5;
            // 
            // textBoxOther
            // 
            this.textBoxOther.Location = new System.Drawing.Point(106, 87);
            this.textBoxOther.Name = "textBoxOther";
            this.textBoxOther.Size = new System.Drawing.Size(100, 20);
            this.textBoxOther.TabIndex = 6;
            // 
            // textBoxOther2
            // 
            this.textBoxOther2.Location = new System.Drawing.Point(106, 119);
            this.textBoxOther2.Name = "textBoxOther2";
            this.textBoxOther2.Size = new System.Drawing.Size(100, 20);
            this.textBoxOther2.TabIndex = 7;
            // 
            // textBoxManualInput
            // 
            this.textBoxManualInput.Location = new System.Drawing.Point(106, 150);
            this.textBoxManualInput.Name = "textBoxManualInput";
            this.textBoxManualInput.Size = new System.Drawing.Size(100, 20);
            this.textBoxManualInput.TabIndex = 8;
            // 
            // buttonOK
            // 
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new System.Drawing.Point(82, 185);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 9;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // PhoneIndicator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(174)))), ((int)(((byte)(231)))));
            this.ClientSize = new System.Drawing.Size(237, 224);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxManualInput);
            this.Controls.Add(this.textBoxOther2);
            this.Controls.Add(this.textBoxOther);
            this.Controls.Add(this.textBoxHome);
            this.Controls.Add(this.checkBoxManualInput);
            this.Controls.Add(this.checkBoxOther2);
            this.Controls.Add(this.checkBoxOther);
            this.Controls.Add(this.checkBoxHome);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(253, 263);
            this.Name = "PhoneIndicator";
            this.Text = "Choose a phone number";
            this.Load += new System.EventHandler(this.PhoneIndicator_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxHome;
        private System.Windows.Forms.CheckBox checkBoxOther;
        private System.Windows.Forms.CheckBox checkBoxOther2;
        private System.Windows.Forms.CheckBox checkBoxManualInput;
        private System.Windows.Forms.TextBox textBoxHome;
        private System.Windows.Forms.TextBox textBoxOther;
        private System.Windows.Forms.TextBox textBoxOther2;
        private System.Windows.Forms.TextBox textBoxManualInput;
        private System.Windows.Forms.Button buttonOK;
    }
}