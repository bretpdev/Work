namespace TPERM
{
    partial class CoMakerInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoMakerInfo));
            this.coData = new System.Windows.Forms.DataGridView();
            this.Cont = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.coData)).BeginInit();
            this.SuspendLayout();
            // 
            // coData
            // 
            this.coData.AllowUserToAddRows = false;
            this.coData.AllowUserToDeleteRows = false;
            this.coData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.coData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.coData.Location = new System.Drawing.Point(13, 13);
            this.coData.Margin = new System.Windows.Forms.Padding(4);
            this.coData.Name = "coData";
            this.coData.RowHeadersVisible = false;
            this.coData.Size = new System.Drawing.Size(561, 227);
            this.coData.TabIndex = 0;
            // 
            // Cont
            // 
            this.Cont.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cont.Location = new System.Drawing.Point(485, 248);
            this.Cont.Margin = new System.Windows.Forms.Padding(4);
            this.Cont.Name = "Cont";
            this.Cont.Size = new System.Drawing.Size(89, 31);
            this.Cont.TabIndex = 1;
            this.Cont.Text = "Continue";
            this.Cont.UseVisualStyleBackColor = true;
            this.Cont.Click += new System.EventHandler(this.Cont_Click);
            // 
            // CoMakerInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 286);
            this.Controls.Add(this.Cont);
            this.Controls.Add(this.coData);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CoMakerInfo";
            this.Text = "Select the comaker that signed the form";
            ((System.ComponentModel.ISupportInitialize)(this.coData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView coData;
        private System.Windows.Forms.Button Cont;
    }
}