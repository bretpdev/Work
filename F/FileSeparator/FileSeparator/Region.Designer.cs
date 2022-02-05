namespace FileSeparator
{
    partial class Region
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
            this.Cornerstone = new System.Windows.Forms.Button();
            this.Uheaa = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Cornerstone
            // 
            this.Cornerstone.Location = new System.Drawing.Point(120, 13);
            this.Cornerstone.Name = "Cornerstone";
            this.Cornerstone.Size = new System.Drawing.Size(90, 23);
            this.Cornerstone.TabIndex = 3;
            this.Cornerstone.Text = "CornerStone";
            this.Cornerstone.UseVisualStyleBackColor = true;
            this.Cornerstone.Click += new System.EventHandler(this.Cornerstone_Click);
            // 
            // Uheaa
            // 
            this.Uheaa.Location = new System.Drawing.Point(12, 13);
            this.Uheaa.Name = "Uheaa";
            this.Uheaa.Size = new System.Drawing.Size(90, 23);
            this.Uheaa.TabIndex = 2;
            this.Uheaa.Text = "Uheaa";
            this.Uheaa.UseVisualStyleBackColor = true;
            this.Uheaa.Click += new System.EventHandler(this.Uheaa_Click);
            // 
            // Region
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 48);
            this.ControlBox = false;
            this.Controls.Add(this.Cornerstone);
            this.Controls.Add(this.Uheaa);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Region";
            this.Text = "Region";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Cornerstone;
        private System.Windows.Forms.Button Uheaa;
    }
}