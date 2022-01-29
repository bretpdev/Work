namespace SftpCoordinator
{
    partial class RunHistoryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunHistoryForm));
            this.ResultsGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.RunIdText = new System.Windows.Forms.TextBox();
            this.StartedOnText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.EndedOnText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.RunByText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // ResultsGrid
            // 
            this.ResultsGrid.AllowUserToAddRows = false;
            this.ResultsGrid.AllowUserToDeleteRows = false;
            this.ResultsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultsGrid.Location = new System.Drawing.Point(12, 71);
            this.ResultsGrid.Name = "ResultsGrid";
            this.ResultsGrid.RowHeadersVisible = false;
            this.ResultsGrid.Size = new System.Drawing.Size(681, 408);
            this.ResultsGrid.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Run #";
            // 
            // RunIdText
            // 
            this.RunIdText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunIdText.Location = new System.Drawing.Point(12, 36);
            this.RunIdText.Name = "RunIdText";
            this.RunIdText.ReadOnly = true;
            this.RunIdText.Size = new System.Drawing.Size(100, 29);
            this.RunIdText.TabIndex = 2;
            this.RunIdText.Text = "44";
            this.RunIdText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // StartedOnText
            // 
            this.StartedOnText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartedOnText.Location = new System.Drawing.Point(277, 36);
            this.StartedOnText.Name = "StartedOnText";
            this.StartedOnText.ReadOnly = true;
            this.StartedOnText.Size = new System.Drawing.Size(205, 29);
            this.StartedOnText.TabIndex = 4;
            this.StartedOnText.Text = "01/01/2013 05:40 AM";
            this.StartedOnText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(273, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Started On";
            // 
            // EndedOnText
            // 
            this.EndedOnText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndedOnText.Location = new System.Drawing.Point(488, 36);
            this.EndedOnText.Name = "EndedOnText";
            this.EndedOnText.ReadOnly = true;
            this.EndedOnText.Size = new System.Drawing.Size(205, 29);
            this.EndedOnText.TabIndex = 6;
            this.EndedOnText.Text = "01/01/2013 05:40 AM";
            this.EndedOnText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(484, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Ended On";
            // 
            // RunByText
            // 
            this.RunByText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunByText.Location = new System.Drawing.Point(118, 36);
            this.RunByText.Name = "RunByText";
            this.RunByText.ReadOnly = true;
            this.RunByText.Size = new System.Drawing.Size(153, 29);
            this.RunByText.TabIndex = 8;
            this.RunByText.Text = "longusername";
            this.RunByText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(114, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Run By";
            // 
            // RunHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 491);
            this.Controls.Add(this.RunByText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.EndedOnText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.StartedOnText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RunIdText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ResultsGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RunHistoryForm";
            this.Text = "Activity Log";
            ((System.ComponentModel.ISupportInitialize)(this.ResultsGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ResultsGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox RunIdText;
        private System.Windows.Forms.TextBox StartedOnText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox EndedOnText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox RunByText;
        private System.Windows.Forms.Label label4;
    }
}