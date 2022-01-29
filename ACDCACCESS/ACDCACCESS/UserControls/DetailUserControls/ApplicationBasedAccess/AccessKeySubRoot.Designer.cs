namespace ACDCAccess
{
    partial class AccessKeySubRoot
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
            this.pnlBusinessUnits = new System.Windows.Forms.FlowLayoutPanel();
            this.lblAccessKey = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExpand = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblKeyDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pnlBusinessUnits
            // 
            this.pnlBusinessUnits.AutoSize = true;
            this.pnlBusinessUnits.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBusinessUnits.Location = new System.Drawing.Point(117, 47);
            this.pnlBusinessUnits.MaximumSize = new System.Drawing.Size(740, 4);
            this.pnlBusinessUnits.MinimumSize = new System.Drawing.Size(740, 4);
            this.pnlBusinessUnits.Name = "pnlBusinessUnits";
            this.pnlBusinessUnits.Size = new System.Drawing.Size(740, 4);
            this.pnlBusinessUnits.TabIndex = 3;
            this.pnlBusinessUnits.TabStop = true;
            // 
            // lblAccessKey
            // 
            this.lblAccessKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAccessKey.Location = new System.Drawing.Point(117, 1);
            this.lblAccessKey.Name = "lblAccessKey";
            this.lblAccessKey.Size = new System.Drawing.Size(740, 20);
            this.lblAccessKey.TabIndex = 6;
            this.lblAccessKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(29, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Access Key";
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
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(29, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 18;
            this.label3.Text = "Description";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblKeyDescription
            // 
            this.lblKeyDescription.AutoSize = true;
            this.lblKeyDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblKeyDescription.Location = new System.Drawing.Point(117, 24);
            this.lblKeyDescription.MaximumSize = new System.Drawing.Size(740, 2);
            this.lblKeyDescription.MinimumSize = new System.Drawing.Size(740, 20);
            this.lblKeyDescription.Name = "lblKeyDescription";
            this.lblKeyDescription.Size = new System.Drawing.Size(740, 20);
            this.lblKeyDescription.TabIndex = 17;
            this.lblKeyDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AccessKeySubRoot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblKeyDescription);
            this.Controls.Add(this.pnlBusinessUnits);
            this.Controls.Add(this.lblAccessKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExpand);
            this.Name = "AccessKeySubRoot";
            this.Size = new System.Drawing.Size(860, 59);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlBusinessUnits;
        private System.Windows.Forms.Label lblAccessKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblKeyDescription;
    }
}
