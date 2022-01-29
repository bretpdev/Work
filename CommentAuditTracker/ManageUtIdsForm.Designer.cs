namespace CommentAuditTracker
{
    partial class ManageUtIdsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageUtIdsForm));
            this.AgentNameBox = new System.Windows.Forms.TextBox();
            this.IdsGrid = new System.Windows.Forms.DataGridView();
            this.IdsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.IdsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // AgentNameBox
            // 
            this.AgentNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AgentNameBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.AgentNameBox.Enabled = false;
            this.AgentNameBox.Location = new System.Drawing.Point(13, 13);
            this.AgentNameBox.Margin = new System.Windows.Forms.Padding(4);
            this.AgentNameBox.Name = "AgentNameBox";
            this.AgentNameBox.ReadOnly = true;
            this.AgentNameBox.Size = new System.Drawing.Size(400, 26);
            this.AgentNameBox.TabIndex = 0;
            // 
            // IdsGrid
            // 
            this.IdsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IdsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.IdsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdsColumn});
            this.IdsGrid.Location = new System.Drawing.Point(13, 46);
            this.IdsGrid.MultiSelect = false;
            this.IdsGrid.Name = "IdsGrid";
            this.IdsGrid.RowHeadersVisible = false;
            this.IdsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.IdsGrid.Size = new System.Drawing.Size(400, 253);
            this.IdsGrid.TabIndex = 1;
            this.IdsGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.IdsGrid_CellEndEdit);
            // 
            // IdsColumn
            // 
            this.IdsColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.IdsColumn.HeaderText = "UT IDs";
            this.IdsColumn.MaxInputLength = 7;
            this.IdsColumn.Name = "IdsColumn";
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.Location = new System.Drawing.Point(338, 305);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 35);
            this.OkButton.TabIndex = 2;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelButton.Location = new System.Drawing.Point(12, 305);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 35);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ManageUtIdsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 348);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.IdsGrid);
            this.Controls.Add(this.AgentNameBox);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ManageUtIdsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UT IDs";
            ((System.ComponentModel.ISupportInitialize)(this.IdsGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AgentNameBox;
        private System.Windows.Forms.DataGridView IdsGrid;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdsColumn;
    }
}