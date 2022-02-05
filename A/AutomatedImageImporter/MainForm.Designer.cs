namespace AutomatedImageImporter
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.ActivityView = new System.Windows.Forms.TreeView();
            this.label6 = new System.Windows.Forms.Label();
            this.ProgressGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Progress";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ActivityView
            // 
            this.ActivityView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActivityView.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivityView.Location = new System.Drawing.Point(225, 35);
            this.ActivityView.Name = "ActivityView";
            this.ActivityView.Size = new System.Drawing.Size(942, 542);
            this.ActivityView.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(220, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(198, 26);
            this.label6.TabIndex = 7;
            this.label6.Text = "Activity Log";
            // 
            // ProgressGrid
            // 
            this.ProgressGrid.AllowUserToAddRows = false;
            this.ProgressGrid.AllowUserToDeleteRows = false;
            this.ProgressGrid.AllowUserToResizeColumns = false;
            this.ProgressGrid.AllowUserToResizeRows = false;
            this.ProgressGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ProgressGrid.BackgroundColor = System.Drawing.Color.White;
            this.ProgressGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProgressGrid.ColumnHeadersVisible = false;
            this.ProgressGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ProgressGrid.Enabled = false;
            this.ProgressGrid.Location = new System.Drawing.Point(12, 35);
            this.ProgressGrid.Name = "ProgressGrid";
            this.ProgressGrid.ReadOnly = true;
            this.ProgressGrid.RowHeadersVisible = false;
            this.ProgressGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ProgressGrid.Size = new System.Drawing.Size(207, 542);
            this.ProgressGrid.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 597);
            this.Controls.Add(this.ProgressGrid);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ActivityView);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "Automated Image Importer";
            ((System.ComponentModel.ISupportInitialize)(this.ProgressGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView ActivityView;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView ProgressGrid;
    }
}

