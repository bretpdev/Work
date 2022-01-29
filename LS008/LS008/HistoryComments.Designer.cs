namespace LS008
{
    partial class HistoryComments
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoryComments));
            this.Comments = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.Search = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Comments)).BeginInit();
            this.SuspendLayout();
            // 
            // Comments
            // 
            this.Comments.AllowUserToAddRows = false;
            this.Comments.AllowUserToDeleteRows = false;
            this.Comments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Comments.DefaultCellStyle = dataGridViewCellStyle1;
            this.Comments.Location = new System.Drawing.Point(12, 36);
            this.Comments.Name = "Comments";
            this.Comments.ReadOnly = true;
            this.Comments.Size = new System.Drawing.Size(1191, 526);
            this.Comments.TabIndex = 0;
            this.Comments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Comments_CellDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Description Filter:";
            // 
            // Search
            // 
            this.Search.Location = new System.Drawing.Point(136, 6);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(265, 23);
            this.Search.TabIndex = 2;
            this.Search.TextChanged += new System.EventHandler(this.Search_TextChanged);
            // 
            // HistoryComments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 574);
            this.Controls.Add(this.Search);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Comments);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "HistoryComments";
            this.Text = "HistoryComments";
            ((System.ComponentModel.ISupportInitialize)(this.Comments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView Comments;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Search;
    }
}