namespace CommonTesting
{
    partial class CheckButtonTest
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
            this.checkButton1 = new Uheaa.Common.WinForms.CheckButton();
            this.SuspendLayout();
            // 
            // checkButton1
            // 
            this.checkButton1.IsChecked = false;
            this.checkButton1.Location = new System.Drawing.Point(24, 12);
            this.checkButton1.Name = "checkButton1";
            this.checkButton1.Size = new System.Drawing.Size(151, 67);
            this.checkButton1.TabIndex = 0;
            this.checkButton1.Text = "checkButton1";
            this.checkButton1.UseVisualStyleBackColor = true;
            // 
            // CheckButtonTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.checkButton1);
            this.Name = "CheckButtonTest";
            this.Text = "CheckButtonTest";
            this.ResumeLayout(false);

        }

        #endregion

        private Uheaa.Common.WinForms.CheckButton checkButton1;
    }
}