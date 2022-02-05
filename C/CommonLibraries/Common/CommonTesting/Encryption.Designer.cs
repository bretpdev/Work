namespace CommonTesting
{
    partial class Encryption
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
            this.SourceText = new System.Windows.Forms.TextBox();
            this.DecryptedText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SourceText
            // 
            this.SourceText.Location = new System.Drawing.Point(12, 23);
            this.SourceText.Name = "SourceText";
            this.SourceText.Size = new System.Drawing.Size(100, 20);
            this.SourceText.TabIndex = 0;
            this.SourceText.TextChanged += new System.EventHandler(this.SourceText_TextChanged);
            // 
            // DecryptedText
            // 
            this.DecryptedText.Location = new System.Drawing.Point(177, 23);
            this.DecryptedText.Name = "DecryptedText";
            this.DecryptedText.Size = new System.Drawing.Size(100, 20);
            this.DecryptedText.TabIndex = 1;
            // 
            // Encryption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 167);
            this.Controls.Add(this.DecryptedText);
            this.Controls.Add(this.SourceText);
            this.DoubleBuffered = true;
            this.Name = "Encryption";
            this.Text = "Encryption";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SourceText;
        private System.Windows.Forms.TextBox DecryptedText;
    }
}