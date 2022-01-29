namespace CommonTesting
{
    partial class AutoFontTest
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
            this.autoFontLabel1 = new Uheaa.Common.WinForms.AutoFontLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // autoFontLabel1
            // 
            this.autoFontLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoFontLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.autoFontLabel1.Font = new System.Drawing.Font("Arial", 14F);
            this.autoFontLabel1.Location = new System.Drawing.Point(12, 160);
            this.autoFontLabel1.MaxFontSize = 14F;
            this.autoFontLabel1.Name = "autoFontLabel1";
            this.autoFontLabel1.Size = new System.Drawing.Size(685, 55);
            this.autoFontLabel1.TabIndex = 0;
            this.autoFontLabel1.Text = "very long text which eventually has to wrap around the edges of the box.  I mean," +
    " it has to eventually, right?";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(685, 118);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "very long text which eventually has to wrap around the edges of the box.  I mean," +
    " it has to eventually, right?";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // AutoFontTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 225);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.autoFontLabel1);
            this.Name = "AutoFontTest";
            this.Text = "AutoFontTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.AutoFontLabel autoFontLabel1;
        private System.Windows.Forms.TextBox textBox1;
    }
}