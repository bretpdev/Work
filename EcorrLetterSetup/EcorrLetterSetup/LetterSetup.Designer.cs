namespace EcorrLetterSetup
{
    partial class LetterSetup
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
            this.LetterIdLabel = new System.Windows.Forms.Label();
            this.Add = new System.Windows.Forms.Button();
            this.LetterDataGrid = new System.Windows.Forms.DataGridView();
            this.SelectForm = new System.Windows.Forms.Button();
            this.PreviewForm = new System.Windows.Forms.Button();
            this.FormPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveForm = new System.Windows.Forms.Button();
            this.DeleteForm = new System.Windows.Forms.Button();
            this.Promote = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LetterDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // LetterIdLabel
            // 
            this.LetterIdLabel.AutoSize = true;
            this.LetterIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LetterIdLabel.Location = new System.Drawing.Point(12, 9);
            this.LetterIdLabel.Name = "LetterIdLabel";
            this.LetterIdLabel.Size = new System.Drawing.Size(73, 20);
            this.LetterIdLabel.TabIndex = 0;
            this.LetterIdLabel.Text = "Letter Id:";
            // 
            // Add
            // 
            this.Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Add.Location = new System.Drawing.Point(483, 5);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 33);
            this.Add.TabIndex = 2;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // LetterDataGrid
            // 
            this.LetterDataGrid.AllowUserToAddRows = false;
            this.LetterDataGrid.AllowUserToDeleteRows = false;
            this.LetterDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LetterDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LetterDataGrid.Location = new System.Drawing.Point(12, 44);
            this.LetterDataGrid.Name = "LetterDataGrid";
            this.LetterDataGrid.ReadOnly = true;
            this.LetterDataGrid.RowHeadersVisible = false;
            this.LetterDataGrid.Size = new System.Drawing.Size(548, 206);
            this.LetterDataGrid.TabIndex = 3;
            this.LetterDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.LetterDataGrid_CellDoubleClick);
            // 
            // SelectForm
            // 
            this.SelectForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectForm.Location = new System.Drawing.Point(485, 261);
            this.SelectForm.Name = "SelectForm";
            this.SelectForm.Size = new System.Drawing.Size(75, 23);
            this.SelectForm.TabIndex = 12;
            this.SelectForm.Text = "Select Form";
            this.SelectForm.UseVisualStyleBackColor = true;
            this.SelectForm.Click += new System.EventHandler(this.SelectForm_Click);
            // 
            // PreviewForm
            // 
            this.PreviewForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PreviewForm.Location = new System.Drawing.Point(16, 292);
            this.PreviewForm.Name = "PreviewForm";
            this.PreviewForm.Size = new System.Drawing.Size(104, 23);
            this.PreviewForm.TabIndex = 11;
            this.PreviewForm.Text = "Preview Form";
            this.PreviewForm.UseVisualStyleBackColor = true;
            this.PreviewForm.Click += new System.EventHandler(this.PreviewForm_Click);
            // 
            // FormPath
            // 
            this.FormPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FormPath.Enabled = false;
            this.FormPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormPath.Location = new System.Drawing.Point(63, 260);
            this.FormPath.Name = "FormPath";
            this.FormPath.Size = new System.Drawing.Size(416, 24);
            this.FormPath.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 263);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 9;
            this.label1.Text = "Form";
            // 
            // SaveForm
            // 
            this.SaveForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveForm.Location = new System.Drawing.Point(126, 292);
            this.SaveForm.Name = "SaveForm";
            this.SaveForm.Size = new System.Drawing.Size(104, 23);
            this.SaveForm.TabIndex = 13;
            this.SaveForm.Text = "Save Form";
            this.SaveForm.UseVisualStyleBackColor = true;
            this.SaveForm.Click += new System.EventHandler(this.SaveForm_Click);
            // 
            // DeleteForm
            // 
            this.DeleteForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteForm.Location = new System.Drawing.Point(237, 292);
            this.DeleteForm.Name = "DeleteForm";
            this.DeleteForm.Size = new System.Drawing.Size(104, 23);
            this.DeleteForm.TabIndex = 14;
            this.DeleteForm.Text = "Delete From";
            this.DeleteForm.UseVisualStyleBackColor = true;
            this.DeleteForm.Click += new System.EventHandler(this.DeleteForm_Click);
            // 
            // Promote
            // 
            this.Promote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Promote.Location = new System.Drawing.Point(495, 292);
            this.Promote.Name = "Promote";
            this.Promote.Size = new System.Drawing.Size(63, 23);
            this.Promote.TabIndex = 15;
            this.Promote.Text = "Promote";
            this.Promote.UseVisualStyleBackColor = true;
            this.Promote.Click += new System.EventHandler(this.Promote_Click);
            // 
            // LetterSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 327);
            this.Controls.Add(this.Promote);
            this.Controls.Add(this.DeleteForm);
            this.Controls.Add(this.SaveForm);
            this.Controls.Add(this.SelectForm);
            this.Controls.Add(this.PreviewForm);
            this.Controls.Add(this.FormPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LetterDataGrid);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.LetterIdLabel);
            this.Name = "LetterSetup";
            this.ShowIcon = false;
            this.Text = "ScriptLetter";
            ((System.ComponentModel.ISupportInitialize)(this.LetterDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LetterIdLabel;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.DataGridView LetterDataGrid;
        private System.Windows.Forms.Button SelectForm;
        private System.Windows.Forms.Button PreviewForm;
        private System.Windows.Forms.TextBox FormPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SaveForm;
        private System.Windows.Forms.Button DeleteForm;
        private System.Windows.Forms.Button Promote;
    }
}