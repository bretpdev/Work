namespace ACDCAccess
{
    partial class ApplicationSubRoot
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
			this.pnlAccessKeys = new System.Windows.Forms.FlowLayoutPanel();
			this.lblApplication = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnExpand = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pnlAccessKeys
			// 
			this.pnlAccessKeys.AutoSize = true;
			this.pnlAccessKeys.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlAccessKeys.Location = new System.Drawing.Point(117, 23);
			this.pnlAccessKeys.MaximumSize = new System.Drawing.Size(740, 4);
			this.pnlAccessKeys.MinimumSize = new System.Drawing.Size(740, 4);
			this.pnlAccessKeys.Name = "pnlAccessKeys";
			this.pnlAccessKeys.Size = new System.Drawing.Size(740, 4);
			this.pnlAccessKeys.TabIndex = 7;
			this.pnlAccessKeys.TabStop = true;
			// 
			// lblApplication
			// 
			this.lblApplication.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblApplication.Location = new System.Drawing.Point(117, 1);
			this.lblApplication.Name = "lblApplication";
			this.lblApplication.Size = new System.Drawing.Size(740, 20);
			this.lblApplication.TabIndex = 6;
			this.lblApplication.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(29, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 17);
			this.label1.TabIndex = 5;
			this.label1.Text = "Application";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnExpand
			// 
			this.btnExpand.Location = new System.Drawing.Point(4, 1);
			this.btnExpand.Name = "btnExpand";
			this.btnExpand.Size = new System.Drawing.Size(19, 20);
			this.btnExpand.TabIndex = 1;
			this.btnExpand.Text = "+";
			this.btnExpand.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.btnExpand.UseVisualStyleBackColor = true;
			this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
			// 
			// ApplicationSubRoot
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.pnlAccessKeys);
			this.Controls.Add(this.lblApplication);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnExpand);
			this.Name = "ApplicationSubRoot";
			this.Size = new System.Drawing.Size(860, 30);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlAccessKeys;
        private System.Windows.Forms.Label lblApplication;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExpand;
    }
}
