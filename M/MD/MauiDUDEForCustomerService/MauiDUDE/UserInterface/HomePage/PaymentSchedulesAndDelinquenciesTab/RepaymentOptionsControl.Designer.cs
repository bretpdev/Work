namespace MauiDUDE
{
    partial class RepaymentOptionsControl
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonPrivateLoans = new System.Windows.Forms.Button();
            this.buttonConsolidation = new System.Windows.Forms.Button();
            this.buttonStafford_PLUS_TILP = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.flowLayoutPanelCurrentOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelDisplayOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanelCurrentOptions);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanelDisplayOptions);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(695, 183);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Repayment Plan";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonPrivateLoans);
            this.panel1.Controls.Add(this.buttonConsolidation);
            this.panel1.Controls.Add(this.buttonStafford_PLUS_TILP);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(3, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(655, 75);
            this.panel1.TabIndex = 1;
            // 
            // buttonPrivateLoans
            // 
            this.buttonPrivateLoans.Location = new System.Drawing.Point(397, 41);
            this.buttonPrivateLoans.Name = "buttonPrivateLoans";
            this.buttonPrivateLoans.Size = new System.Drawing.Size(128, 23);
            this.buttonPrivateLoans.TabIndex = 3;
            this.buttonPrivateLoans.Text = "Private Loans";
            this.buttonPrivateLoans.UseVisualStyleBackColor = true;
            this.buttonPrivateLoans.Click += new System.EventHandler(this.buttonPrivateLoans_Click);
            // 
            // buttonConsolidation
            // 
            this.buttonConsolidation.Location = new System.Drawing.Point(248, 41);
            this.buttonConsolidation.Name = "buttonConsolidation";
            this.buttonConsolidation.Size = new System.Drawing.Size(128, 23);
            this.buttonConsolidation.TabIndex = 2;
            this.buttonConsolidation.Text = "Consolidation";
            this.buttonConsolidation.UseVisualStyleBackColor = true;
            this.buttonConsolidation.Click += new System.EventHandler(this.buttonConsolidation_Click);
            // 
            // buttonStafford_PLUS_TILP
            // 
            this.buttonStafford_PLUS_TILP.Location = new System.Drawing.Point(95, 41);
            this.buttonStafford_PLUS_TILP.Name = "buttonStafford_PLUS_TILP";
            this.buttonStafford_PLUS_TILP.Size = new System.Drawing.Size(128, 23);
            this.buttonStafford_PLUS_TILP.TabIndex = 1;
            this.buttonStafford_PLUS_TILP.Text = "Stafford / PLUS / TILP";
            this.buttonStafford_PLUS_TILP.UseVisualStyleBackColor = true;
            this.buttonStafford_PLUS_TILP.Click += new System.EventHandler(this.buttonStafford_PLUS_TILP_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(228, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Alternate Repayment Plans";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanelCurrentOptions
            // 
            this.flowLayoutPanelCurrentOptions.Location = new System.Drawing.Point(664, 23);
            this.flowLayoutPanelCurrentOptions.Name = "flowLayoutPanelCurrentOptions";
            this.flowLayoutPanelCurrentOptions.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanelCurrentOptions.TabIndex = 2;
            // 
            // flowLayoutPanelDisplayOptions
            // 
            this.flowLayoutPanelDisplayOptions.Location = new System.Drawing.Point(670, 23);
            this.flowLayoutPanelDisplayOptions.Name = "flowLayoutPanelDisplayOptions";
            this.flowLayoutPanelDisplayOptions.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanelDisplayOptions.TabIndex = 3;
            // 
            // RepaymentOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "RepaymentOptionsControl";
            this.Size = new System.Drawing.Size(695, 183);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonStafford_PLUS_TILP;
        private System.Windows.Forms.Button buttonPrivateLoans;
        private System.Windows.Forms.Button buttonConsolidation;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCurrentOptions;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDisplayOptions;
    }
}
