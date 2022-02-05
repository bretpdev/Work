namespace MauiDUDE
{
    partial class Phone
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
            this.textBoxPhone1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPhone2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPhone3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxExtension = new System.Windows.Forms.TextBox();
            this.comboBoxMobile = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // textBoxPhone1
            // 
            this.textBoxPhone1.Location = new System.Drawing.Point(4, 4);
            this.textBoxPhone1.MaxLength = 3;
            this.textBoxPhone1.Name = "textBoxPhone1";
            this.textBoxPhone1.Size = new System.Drawing.Size(32, 20);
            this.textBoxPhone1.TabIndex = 0;
            this.textBoxPhone1.TextChanged += new System.EventHandler(this.textBoxPhone1_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(37, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "-";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxPhone2
            // 
            this.textBoxPhone2.Location = new System.Drawing.Point(47, 4);
            this.textBoxPhone2.MaxLength = 3;
            this.textBoxPhone2.Name = "textBoxPhone2";
            this.textBoxPhone2.Size = new System.Drawing.Size(32, 20);
            this.textBoxPhone2.TabIndex = 2;
            this.textBoxPhone2.TextChanged += new System.EventHandler(this.textBoxPhone2_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(81, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(8, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "-";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxPhone3
            // 
            this.textBoxPhone3.Location = new System.Drawing.Point(93, 4);
            this.textBoxPhone3.MaxLength = 4;
            this.textBoxPhone3.Name = "textBoxPhone3";
            this.textBoxPhone3.Size = new System.Drawing.Size(48, 20);
            this.textBoxPhone3.TabIndex = 4;
            this.textBoxPhone3.TextChanged += new System.EventHandler(this.textBoxPhone3_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(143, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Ext:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxExtension
            // 
            this.textBoxExtension.Location = new System.Drawing.Point(171, 4);
            this.textBoxExtension.MaxLength = 4;
            this.textBoxExtension.Name = "textBoxExtension";
            this.textBoxExtension.Size = new System.Drawing.Size(48, 20);
            this.textBoxExtension.TabIndex = 6;
            // 
            // comboBoxMobile
            // 
            this.comboBoxMobile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMobile.FormattingEnabled = true;
            this.comboBoxMobile.Items.AddRange(new object[] {
            "M",
            "L",
            "U"});
            this.comboBoxMobile.Location = new System.Drawing.Point(226, 4);
            this.comboBoxMobile.Name = "comboBoxMobile";
            this.comboBoxMobile.Size = new System.Drawing.Size(38, 21);
            this.comboBoxMobile.TabIndex = 7;
            // 
            // Phone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxMobile);
            this.Controls.Add(this.textBoxExtension);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxPhone3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxPhone2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPhone1);
            this.Name = "Phone";
            this.Size = new System.Drawing.Size(271, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPhone1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPhone2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPhone3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxExtension;
        private System.Windows.Forms.ComboBox comboBoxMobile;
    }
}
