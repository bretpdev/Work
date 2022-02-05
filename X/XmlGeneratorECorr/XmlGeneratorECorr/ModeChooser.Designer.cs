namespace XmlGeneratorECorr
{
    partial class ModeChooser
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
            this.BatchMode = new System.Windows.Forms.Button();
            this.UserMode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BatchMode
            // 
            this.BatchMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BatchMode.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BatchMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BatchMode.Location = new System.Drawing.Point(12, 12);
            this.BatchMode.Name = "BatchMode";
            this.BatchMode.Size = new System.Drawing.Size(107, 52);
            this.BatchMode.TabIndex = 0;
            this.BatchMode.Text = "Batch Mode";
            this.BatchMode.UseVisualStyleBackColor = true;
            this.BatchMode.Click += new System.EventHandler(this.BatchMode_Click);
            // 
            // UserMode
            // 
            this.UserMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserMode.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.UserMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserMode.Location = new System.Drawing.Point(12, 90);
            this.UserMode.Name = "UserMode";
            this.UserMode.Size = new System.Drawing.Size(107, 52);
            this.UserMode.TabIndex = 1;
            this.UserMode.Text = "User Mode";
            this.UserMode.UseVisualStyleBackColor = true;
            this.UserMode.Click += new System.EventHandler(this.UserMode_Click);
            // 
            // ModeChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(132, 159);
            this.Controls.Add(this.UserMode);
            this.Controls.Add(this.BatchMode);
            this.MinimumSize = new System.Drawing.Size(148, 197);
            this.Name = "ModeChooser";
            this.ShowIcon = false;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BatchMode;
        private System.Windows.Forms.Button UserMode;
    }
}