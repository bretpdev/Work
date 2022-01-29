namespace ACDCAccess
{
    partial class BusinessUnitRoot
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
            this.lblBusinessUnit = new System.Windows.Forms.Label();
            this.pnlApplications = new System.Windows.Forms.FlowLayoutPanel();
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
            this.label1.Text = "Business Unit";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBusinessUnit
            // 
            this.lblBusinessUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBusinessUnit.Location = new System.Drawing.Point(116, 3);
            this.lblBusinessUnit.Name = "lblBusinessUnit";
            this.lblBusinessUnit.Size = new System.Drawing.Size(881, 17);
            this.lblBusinessUnit.TabIndex = 2;
            this.lblBusinessUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSystems
            // 
            this.pnlApplications.AutoSize = true;
            this.pnlApplications.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlApplications.Location = new System.Drawing.Point(116, 21);
            this.pnlApplications.MaximumSize = new System.Drawing.Size(881, 4);
            this.pnlApplications.MinimumSize = new System.Drawing.Size(881, 4);
            this.pnlApplications.Name = "pnlSystems";
            this.pnlApplications.Size = new System.Drawing.Size(881, 4);
            this.pnlApplications.TabIndex = 3;
            this.pnlApplications.TabStop = true;
            // 
            // BusinessUnitRoot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.pnlApplications);
            this.Controls.Add(this.lblBusinessUnit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExpand);
            this.MaximumSize = new System.Drawing.Size(1000, 0);
            this.MinimumSize = new System.Drawing.Size(1000, 0);
            this.Name = "BusinessUnitRoot";
            this.Size = new System.Drawing.Size(1000, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblBusinessUnit;
        private System.Windows.Forms.FlowLayoutPanel pnlApplications;
    }
}
