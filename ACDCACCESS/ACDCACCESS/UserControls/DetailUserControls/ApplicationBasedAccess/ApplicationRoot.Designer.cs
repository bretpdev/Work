namespace ACDCAccess
{
    partial class ApplicationRoot
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
			this.btnExpand = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lblApplication = new System.Windows.Forms.Label();
			this.pnlAccessKey = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			// 
			// btnExpand
			// 
			this.btnExpand.Location = new System.Drawing.Point(3, 0);
			this.btnExpand.Name = "btnExpand";
			this.btnExpand.Size = new System.Drawing.Size(19, 20);
			this.btnExpand.TabIndex = 0;
			this.btnExpand.Text = "+";
			this.btnExpand.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.btnExpand.UseVisualStyleBackColor = true;
			this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(28, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "Application";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblApplication
			// 
			this.lblApplication.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblApplication.Location = new System.Drawing.Point(116, 3);
			this.lblApplication.Name = "lblApplication";
			this.lblApplication.Size = new System.Drawing.Size(881, 17);
			this.lblApplication.TabIndex = 2;
			this.lblApplication.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pnlAccessKey
			// 
			this.pnlAccessKey.AutoSize = true;
			this.pnlAccessKey.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlAccessKey.Location = new System.Drawing.Point(116, 21);
			this.pnlAccessKey.MaximumSize = new System.Drawing.Size(881, 4);
			this.pnlAccessKey.MinimumSize = new System.Drawing.Size(881, 4);
			this.pnlAccessKey.Name = "pnlAccessKey";
			this.pnlAccessKey.Size = new System.Drawing.Size(881, 4);
			this.pnlAccessKey.TabIndex = 3;
			this.pnlAccessKey.TabStop = true;
			// 
			// ApplicationRoot
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.pnlAccessKey);
			this.Controls.Add(this.lblApplication);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnExpand);
			this.MaximumSize = new System.Drawing.Size(1000, 0);
			this.MinimumSize = new System.Drawing.Size(1000, 0);
			this.Name = "ApplicationRoot";
			this.Size = new System.Drawing.Size(1000, 28);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblApplication;
        private System.Windows.Forms.FlowLayoutPanel pnlAccessKey;
    }
}
