namespace EcorrLetterSetup
{
    partial class LetterSearch
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
            this.LetterId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Search = new System.Windows.Forms.Button();
            this.Letters = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.Letters)).BeginInit();
            this.SuspendLayout();
            // 
            // LetterId
            // 
            this.LetterId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LetterId.Location = new System.Drawing.Point(94, 11);
            this.LetterId.MaxLength = 10;
            this.LetterId.Name = "LetterId";
            this.LetterId.Size = new System.Drawing.Size(184, 26);
            this.LetterId.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Letter ID:";
            // 
            // Search
            // 
            this.Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Search.Location = new System.Drawing.Point(284, 11);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(79, 26);
            this.Search.TabIndex = 2;
            this.Search.Text = "Search";
            this.Search.UseVisualStyleBackColor = true;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // Letters
            // 
            this.Letters.AllowUserToAddRows = false;
            this.Letters.AllowUserToDeleteRows = false;
            this.Letters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Letters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Letters.Location = new System.Drawing.Point(14, 51);
            this.Letters.MultiSelect = false;
            this.Letters.Name = "Letters";
            this.Letters.ReadOnly = true;
            this.Letters.RowHeadersVisible = false;
            this.Letters.Size = new System.Drawing.Size(420, 229);
            this.Letters.TabIndex = 3;
            this.Letters.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Letters_CellDoubleClick);
            // 
            // LetterSearch
            // 
            this.AcceptButton = this.Search;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 292);
            this.Controls.Add(this.Letters);
            this.Controls.Add(this.Search);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LetterId);
            this.Name = "LetterSearch";
            this.ShowIcon = false;
            this.Text = "Letter Setup";
            ((System.ComponentModel.ISupportInitialize)(this.Letters)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LetterId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Search;
        private System.Windows.Forms.DataGridView Letters;
    }
}

