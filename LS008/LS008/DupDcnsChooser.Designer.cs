namespace LS008
{
    partial class DupDcnsChooser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DupDcnsChooser));
            this.DCNS = new System.Windows.Forms.DataGridView();
            this.Continue = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CurDcn = new System.Windows.Forms.Label();
            this.CDN = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DCNS)).BeginInit();
            this.SuspendLayout();
            // 
            // DCNS
            // 
            this.DCNS.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DCNS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DCNS.Location = new System.Drawing.Point(12, 101);
            this.DCNS.Name = "DCNS";
            this.DCNS.Size = new System.Drawing.Size(298, 251);
            this.DCNS.TabIndex = 0;
            // 
            // Continue
            // 
            this.Continue.Location = new System.Drawing.Point(220, 358);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(90, 35);
            this.Continue.TabIndex = 1;
            this.Continue.Text = "Continue";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.Continue_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current DCN:";
            // 
            // CurDcn
            // 
            this.CurDcn.AutoSize = true;
            this.CurDcn.Location = new System.Drawing.Point(122, 75);
            this.CurDcn.Name = "CurDcn";
            this.CurDcn.Size = new System.Drawing.Size(162, 20);
            this.CurDcn.TabIndex = 3;
            this.CurDcn.Text = "99916259028925393";
            // 
            // CDN
            // 
            this.CDN.Image = ((System.Drawing.Image)(resources.GetObject("CDN.Image")));
            this.CDN.Location = new System.Drawing.Point(290, 75);
            this.CDN.Name = "CDN";
            this.CDN.Size = new System.Drawing.Size(20, 20);
            this.CDN.TabIndex = 23;
            this.CDN.UseVisualStyleBackColor = true;
            this.CDN.Click += new System.EventHandler(this.CDN_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(298, 45);
            this.label2.TabIndex = 24;
            this.label2.Text = "The borrower has multiple LS008 tasks.  Please review and select all duplicate ta" +
    "sks.\r\n";
            // 
            // DupDcnsChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 404);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CDN);
            this.Controls.Add(this.CurDcn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.DCNS);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DupDcnsChooser";
            this.Text = "Select Duplicate DCN\'s";
            ((System.ComponentModel.ISupportInitialize)(this.DCNS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DCNS;
        private System.Windows.Forms.Button Continue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label CurDcn;
        private System.Windows.Forms.Button CDN;
        private System.Windows.Forms.Label label2;
    }
}