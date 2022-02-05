using Uheaa.Common.WinForms;
namespace CommonTesting
{
    partial class FormatLabelTest
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
            this.formatLabel1 = new Uheaa.Common.WinForms.FormatLabel();
            this.SuspendLayout();
            // 
            // formatLabel1
            // 
            this.formatLabel1.Location = new System.Drawing.Point(56, 29);
            this.formatLabel1.Name = "formatLabel1";
            this.formatLabel1.Size = new System.Drawing.Size(200, 100);
            this.formatLabel1.SourceMarkup = null;
            this.formatLabel1.TabIndex = 0;
            // 
            // FormatLabelTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.formatLabel1);
            this.Name = "FormatLabelTest";
            this.Text = "FormatLabelTest";
            this.ResumeLayout(false);

        }

        #endregion

        private FormatLabel formatLabel1;



    }
}