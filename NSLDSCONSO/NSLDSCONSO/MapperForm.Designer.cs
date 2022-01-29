namespace NSLDSCONSO
{
    partial class MapperForm
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
            this.BorrowerList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SsnBox = new System.Windows.Forms.TextBox();
            this.BorrowerIdBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ConsolidationLoansGrid = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.UnderlyingLoansGrid = new System.Windows.Forms.DataGridView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.AbandonButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConsolidationLoansGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnderlyingLoansGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // BorrowerList
            // 
            this.BorrowerList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.BorrowerList.FormattingEnabled = true;
            this.BorrowerList.IntegralHeight = false;
            this.BorrowerList.ItemHeight = 16;
            this.BorrowerList.Location = new System.Drawing.Point(12, 28);
            this.BorrowerList.Name = "BorrowerList";
            this.BorrowerList.Size = new System.Drawing.Size(183, 408);
            this.BorrowerList.TabIndex = 0;
            this.BorrowerList.SelectedIndexChanged += new System.EventHandler(this.BorrowerList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Borrowers";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.NameBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.BorrowerIdBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.SsnBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(201, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(508, 76);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Borrower Info";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "SSN";
            // 
            // SsnBox
            // 
            this.SsnBox.Location = new System.Drawing.Point(9, 38);
            this.SsnBox.Name = "SsnBox";
            this.SsnBox.ReadOnly = true;
            this.SsnBox.Size = new System.Drawing.Size(114, 23);
            this.SsnBox.TabIndex = 1;
            // 
            // BorrowerIdBox
            // 
            this.BorrowerIdBox.Location = new System.Drawing.Point(129, 38);
            this.BorrowerIdBox.Name = "BorrowerIdBox";
            this.BorrowerIdBox.ReadOnly = true;
            this.BorrowerIdBox.Size = new System.Drawing.Size(75, 23);
            this.BorrowerIdBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(126, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "BorrowerID";
            // 
            // NameBox
            // 
            this.NameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameBox.Location = new System.Drawing.Point(210, 38);
            this.NameBox.Name = "NameBox";
            this.NameBox.ReadOnly = true;
            this.NameBox.Size = new System.Drawing.Size(292, 23);
            this.NameBox.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(207, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "Name";
            // 
            // ConsolidationLoansGrid
            // 
            this.ConsolidationLoansGrid.AllowUserToAddRows = false;
            this.ConsolidationLoansGrid.AllowUserToDeleteRows = false;
            this.ConsolidationLoansGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConsolidationLoansGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConsolidationLoansGrid.Location = new System.Drawing.Point(204, 110);
            this.ConsolidationLoansGrid.Name = "ConsolidationLoansGrid";
            this.ConsolidationLoansGrid.ReadOnly = true;
            this.ConsolidationLoansGrid.Size = new System.Drawing.Size(499, 125);
            this.ConsolidationLoansGrid.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(201, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Consolidation Loans";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(201, 238);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Underlying Loans";
            // 
            // UnderlyingLoansGrid
            // 
            this.UnderlyingLoansGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UnderlyingLoansGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UnderlyingLoansGrid.Location = new System.Drawing.Point(204, 257);
            this.UnderlyingLoansGrid.Name = "UnderlyingLoansGrid";
            this.UnderlyingLoansGrid.Size = new System.Drawing.Size(499, 146);
            this.UnderlyingLoansGrid.TabIndex = 5;
            this.UnderlyingLoansGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.UnderlyingLoansGrid_CellValueChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Enabled = false;
            this.SaveButton.Location = new System.Drawing.Point(411, 409);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(292, 27);
            this.SaveButton.TabIndex = 7;
            this.SaveButton.Text = "Save Changes to Underlying Loans";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // AbandonButton
            // 
            this.AbandonButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AbandonButton.Enabled = false;
            this.AbandonButton.Location = new System.Drawing.Point(204, 409);
            this.AbandonButton.Name = "AbandonButton";
            this.AbandonButton.Size = new System.Drawing.Size(150, 27);
            this.AbandonButton.TabIndex = 8;
            this.AbandonButton.Text = "Abandon Changes";
            this.AbandonButton.UseVisualStyleBackColor = true;
            this.AbandonButton.Click += new System.EventHandler(this.AbandonButton_Click);
            // 
            // MapperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 448);
            this.Controls.Add(this.AbandonButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.UnderlyingLoansGrid);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ConsolidationLoansGrid);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BorrowerList);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MapperForm";
            this.Text = "Fix Unmapped Loans";
            this.Load += new System.EventHandler(this.MapperForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConsolidationLoansGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnderlyingLoansGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox BorrowerList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox BorrowerIdBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SsnBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView ConsolidationLoansGrid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView UnderlyingLoansGrid;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button AbandonButton;
    }
}