namespace ScriptSyncTester
{
    partial class FullTest
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
            this.TestGrid = new System.Windows.Forms.DataGridView();
            this.ResultDisplay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScriptId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeDisplay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RegionDisplay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModeDisplay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.TestGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // TestGrid
            // 
            this.TestGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TestGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ResultDisplay,
            this.ScriptId,
            this.TypeDisplay,
            this.RegionDisplay,
            this.ModeDisplay,
            this.Location,
            this.Notes});
            this.TestGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TestGrid.Location = new System.Drawing.Point(0, 0);
            this.TestGrid.Name = "TestGrid";
            this.TestGrid.Size = new System.Drawing.Size(1304, 641);
            this.TestGrid.TabIndex = 0;
            // 
            // ResultDisplay
            // 
            this.ResultDisplay.DataPropertyName = "ResultDisplay";
            this.ResultDisplay.HeaderText = "Status";
            this.ResultDisplay.Name = "ResultDisplay";
            this.ResultDisplay.ReadOnly = true;
            // 
            // ScriptId
            // 
            this.ScriptId.DataPropertyName = "ScriptId";
            this.ScriptId.HeaderText = "ScriptId";
            this.ScriptId.Name = "ScriptId";
            this.ScriptId.ReadOnly = true;
            // 
            // TypeDisplay
            // 
            this.TypeDisplay.DataPropertyName = "TypeDisplay";
            this.TypeDisplay.HeaderText = "Type";
            this.TypeDisplay.Name = "TypeDisplay";
            this.TypeDisplay.ReadOnly = true;
            // 
            // RegionDisplay
            // 
            this.RegionDisplay.DataPropertyName = "RegionDisplay";
            this.RegionDisplay.HeaderText = "Region";
            this.RegionDisplay.Name = "RegionDisplay";
            this.RegionDisplay.ReadOnly = true;
            // 
            // ModeDisplay
            // 
            this.ModeDisplay.DataPropertyName = "ModeDisplay";
            this.ModeDisplay.HeaderText = "Mode";
            this.ModeDisplay.Name = "ModeDisplay";
            // 
            // Location
            // 
            this.Location.DataPropertyName = "Location";
            this.Location.HeaderText = "Location";
            this.Location.Name = "Location";
            this.Location.Width = 400;
            // 
            // Notes
            // 
            this.Notes.DataPropertyName = "Notes";
            this.Notes.HeaderText = "Notes";
            this.Notes.Name = "Notes";
            this.Notes.Width = 300;
            // 
            // FullTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1304, 641);
            this.Controls.Add(this.TestGrid);
            this.Name = "FullTest";
            this.Text = "FullTest";
            this.Load += new System.EventHandler(this.FullTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TestGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView TestGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResultDisplay;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScriptId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeDisplay;
        private System.Windows.Forms.DataGridViewTextBoxColumn RegionDisplay;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModeDisplay;
        private System.Windows.Forms.DataGridViewTextBoxColumn Location;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
    }
}