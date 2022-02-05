namespace BatchLettersUI
{
    partial class BatchLettersUI
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
            this.Letters = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddRecord = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.Letters)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Letters
            // 
            this.Letters.AllowUserToAddRows = false;
            this.Letters.AllowUserToDeleteRows = false;
            this.Letters.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Letters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Letters.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Letters.Location = new System.Drawing.Point(0, 28);
            this.Letters.Name = "Letters";
            this.Letters.ReadOnly = true;
            this.Letters.RowHeadersVisible = false;
            this.Letters.Size = new System.Drawing.Size(1449, 490);
            this.Letters.TabIndex = 0;
            this.Letters.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Letters_CellDoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddRecord});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(101, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // AddRecord
            // 
            this.AddRecord.Image = global::BatchLettersUI.Properties.Resources.Icon;
            this.AddRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddRecord.Name = "AddRecord";
            this.AddRecord.Size = new System.Drawing.Size(89, 22);
            this.AddRecord.Text = "Add Record";
            this.AddRecord.Click += new System.EventHandler(this.AddRecord_Click);
            // 
            // BatchLettersUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1449, 518);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.Letters);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "BatchLettersUI";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.Letters)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView Letters;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton AddRecord;
    }
}

