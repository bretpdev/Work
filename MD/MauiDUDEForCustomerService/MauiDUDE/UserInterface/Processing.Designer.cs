namespace MauiDUDE
{
    partial class Processing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Processing));
            this.labelSurfer = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelSurfer
            // 
            this.labelSurfer.BackColor = System.Drawing.Color.Transparent;
            this.labelSurfer.Location = new System.Drawing.Point(184, 12);
            this.labelSurfer.Name = "labelSurfer";
            this.labelSurfer.Size = new System.Drawing.Size(136, 135);
            this.labelSurfer.TabIndex = 0;
            // 
            // Processing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 203);
            this.Controls.Add(this.labelSurfer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Processing";
            this.Text = "Processing Request";
            this.TransparencyKey = System.Drawing.Color.Red;
            this.VisibleChanged += new System.EventHandler(this.Processing_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelSurfer;
    }
}